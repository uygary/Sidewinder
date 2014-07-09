using System;
using System.IO;
using System.Linq;
using System.Text;
using NuGet;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Updater
{
    public class GetNuGetPackages : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.Config == null)
                throw new ArgumentException("Config property is null", "context");
            if (context.Config.TargetPackages == null)
                throw new ArgumentException("Config.TargetPackages property is null", "context");
        }

        /// <summary>
        /// Get nuget packages (and optionally dependents)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <remarks>Needs to be refactored and split into two steps...very clunky/procedural ATM and ever expanding...
        /// 1) to build a canonical list of download instructions 
        /// 2) do the actual downloads
        /// ...this would make it more testable too
        /// </remarks>
        public bool Execute(UpdaterContext context)
        {
            Logger.Debug("\tConfig.TargetPackages...");

            context.Config.TargetPackages.ToList()
                .ForEach(tp =>
                             {
                                 var sb = new StringBuilder();
                                 sb.AppendFormat("\t\t{0}", tp.Value.Name);

                                 if (tp.Value.Version != null)
                                     sb.AppendFormat(" v{0}", tp.Value.Version);

                                 if (!Constants.NuGet.OfficialFeedUrl.Equals(tp.Value.NuGetFeedUrl,
                                     StringComparison.InvariantCultureIgnoreCase))
                                     sb.AppendFormat(" ({0})", tp.Value.NuGetFeedUrl);

                                 Logger.Debug(sb.ToString());
                             });

            context.Config.TargetPackages.ToList().ForEach(
                target => GetNuGetPackage(context, target.Value));

            // if there are no updates then abort the pipeline
            if (context.Updates.Count == 0)
            {
                Logger.Info("\tNo updates found!");
                return false;
            }

            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
        }

        protected virtual void GetNuGetPackage(UpdaterContext context, TargetPackage target)
        {
            if (context.InstalledPackages.ContainsKey(target.Name))
            {
                // update target version number from the installed package
                // if it exists - this will ensure an update only if a new
                // version is available
                target.Version = context.InstalledPackages[target.Name].Version;
            }

            if (target.Version == null)
            {
                Logger.Debug("\tGetting the latest version of '{0}' from {1}...", target.Name, target.NuGetFeedUrl);
            }
            else
            {
                Logger.Debug("\tChecking {0} for update to {1} v{2}...", target.NuGetFeedUrl, target.Name, target.Version);
            }

            IPackage update;
            if (!FindPackage(context, target, out update))
            {
                return;
            }

            // should we download it? no if...
            // o we already have the current version
            // o not forcing the update
            if (!target.Force && 
                AlreadyInstalled(target) &&
                !IsNewVersion(new SemanticVersion(target.Version), update.Version))
            {
                Logger.Info("\t\tNo update available...running the latest version!");
                return;
            }

            // ok, lets download it!...
            Logger.Debug("\t\tUpdated version v{0} is available {1}", update.Version.Version, 
                target.Force ? " ** FORCED **" : string.Empty);

            DownloadPackage(context, update);

            // add to list of updates (as long as it's not sidewinder itself)
            if (string.Compare(update.Id, Constants.Sidewinder.NuGetPackageName,
                StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                context.Updates.Add(new UpdatedPackage
                {
                    NewVersion = update.Version.Version,
                    Target = target
                });
            }

            // should we update any related packages?
            if (!target.UpdateDependencies)
                return;

            Logger.Debug("\t\tChecking for updates to dependent packages...");
            if (update.DependencySets != null)
            {
                update.DependencySets
                    .SelectMany(_ => _.Dependencies ?? Enumerable.Empty<PackageDependency>()).ToList()
                    .ForEach(dependency => GetPackageDependency(context, target.NuGetFeedUrl, target.Force, dependency));
            }
        }

        protected virtual void GetPackageDependency(UpdaterContext context, string feedUrl, bool force, PackageDependency dependency)
        {
            Logger.Debug("Looking for updates to Package (Dependent) {0}...", dependency.Id);

            IPackage update;
            if (!FindDependency(context, feedUrl, dependency, out update))
            {
                Logger.Warn("Unable to locate Dependency {0}!", dependency.Id);
                return;
            }

            if (!force)
            {
                if (context.InstalledPackages.ContainsKey(dependency.Id))
                {
                    var incumbentPackage = context.InstalledPackages[dependency.Id];
                    if (!IsNewVersion(new SemanticVersion(incumbentPackage.Version), update.Version))
                    {
                        Logger.Debug("\t\tPackage already installed!");
                        return;
                    }
                }
            }

            // ok, lets download it!...
            Logger.Debug("\t\tv{0} is available {1}", update.Version.Version, 
                force ? " ** FORCED DOWNLOAD **" : string.Empty);

            DownloadPackage(context, update);

            // add to list of updates (as long as it's not sidewinder itself)
            if (string.Compare(update.Id, Constants.Sidewinder.NuGetPackageName,
                StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                context.Updates.Add(new UpdatedPackage
                {
                    NewVersion = update.Version.Version,
                    Target = new TargetPackage
                             {
                                 Name = update.Id,
                                 Version = update.Version.Version,
                                 NuGetFeedUrl = feedUrl
                             }
                });
            }

            Logger.Debug("\t\tChecking for updates to dependent packages...");
            if (update.DependencySets != null)
            {
                update.DependencySets
                    .SelectMany(_ => _.Dependencies ?? Enumerable.Empty<PackageDependency>()).ToList()
                    .ForEach(childDependency => GetPackageDependency(context, feedUrl, force, childDependency));
            }
        }


        protected virtual bool AlreadyInstalled(TargetPackage target)
        {
            return target.Version != null;
        }

        protected virtual bool IsNewVersion(SemanticVersion current, SemanticVersion update)
        {
            if (current == null)
                return false;
            if (update == null)
                return false;

            // TODO: support pre-release version declarations

#if TESTING
            Logger.Debug("\tChecking cVal={0} against uVal={1}", cVal, uVal);
#endif

            return update > current;
        }

        /// <summary>
        /// This will attempt to find the package on the feed nominated. If the package does not
        /// exist on the feed and that feed is a custom one it will attempt to find the package
        /// from the official nuget feed as a fallback.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="target"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        protected virtual bool FindPackage(UpdaterContext context, TargetPackage target, out IPackage package)
        {
            var repo = PackageRepositoryFactory.Default.CreateRepository(target.NuGetFeedUrl);
            package = repo.FindPackage(target.Name);

            if (package == null)
            {
                Logger.Warn("\t\t**WARNING** Package {0} does not exist on feed {1}",
                    target.Name,
                    target.NuGetFeedUrl);

                if (target.NuGetFeedUrl.Equals(Constants.NuGet.OfficialFeedUrl,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                if (context.Config.SkipOfficialFeed)
                {
                    Logger.Info("SkipOfficialFeed specified so not looking there for the package!");
                    return false;
                }

                Logger.Debug("\t\tAttempting to find it on the official feed...");
                repo = PackageRepositoryFactory.Default.CreateRepository(Constants.NuGet.OfficialFeedUrl);
                package = repo.FindPackage(target.Name);

                if (package == null)
                {
                    Logger.Warn("\t\t**WARNING** Package {0} does not exist on the offical feed {1}",
                                        target.Name,
                                        Constants.NuGet.OfficialFeedUrl);
                    return false;
                }
            }

            // update the target feed just in case we get it from the fallback (official) feed
            target.NuGetFeedUrl = repo.Source;
            return true;
        }

        protected virtual bool FindDependency(UpdaterContext context, string feedUrl, PackageDependency dependency,
            out IPackage package)
        {
            const bool allowPreRelease = false;
            const bool preferListed = true;

            var repo = PackageRepositoryFactory.Default.CreateRepository(feedUrl);
            package = repo.ResolveDependency(dependency, allowPreRelease, preferListed);

            if (package != null)
            {
                Logger.Debug("\t\tDependency Package {0} v{1} located on {2}", dependency.Id, package.Version, feedUrl);
                return true;
            }

            if (feedUrl.Equals(Constants.NuGet.OfficialFeedUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                // we are alrady using the Official Feed so lets bail...
                return false;
            }
            if (context.Config.SkipOfficialFeed)
            {
                // ditto if we are forced not to use Official...
                Logger.Info("SkipOfficialFeed specified so not looking there for the package!");
                return false;
            }

            Logger.Debug("\t\tAttempting to find dependency on the official feed...");
            repo = PackageRepositoryFactory.Default.CreateRepository(Constants.NuGet.OfficialFeedUrl);
            package = repo.ResolveDependency(dependency, allowPreRelease, preferListed);

            if (package == null) 
                return false;

            Logger.Debug("\t\tDependency Package {0} v{1} located on Official NuGet feed", dependency.Id, package.Version);
            return true;
        }

        protected virtual void DownloadPackage(UpdaterContext context, IPackage package)
        {
            var downloadFolder = Fluent.IO.Path.Get(context.Config.DownloadFolder, package.Id).FullPath;
            Logger.Debug("\tDownloading package '{0}' content to: {1}...", package.Id, downloadFolder);

            Fluent.IO.Path.CreateDirectory(downloadFolder);
            var files = package.GetFiles();
            files.ToList().ForEach(file =>
            {
                Logger.Debug("\t\t{0}", file.Path);
                DownloadFile(downloadFolder, file);
            });
        }

        protected virtual void DownloadFile(string downloadFolder, IPackageFile file)
        {
            using (var stream = file.GetStream())
            {
                var filename = Path.GetFileName(file.Path) ?? file.Path;
                var folder = Fluent.IO.Path.Get(downloadFolder).Combine(file.Path).Parent().FullPath;
                Directory.CreateDirectory(folder);

                using (var destination = File.Create(Path.Combine(folder, filename)))
                    stream.CopyTo(destination);
            }
        }
    }
}