using System;
using System.IO;
using System.Linq;
using NuGet;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Updater
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

        public bool Execute(UpdaterContext context)
        {
            context.Config.TargetPackages.ToList().ForEach(
                target => GetNuGetPackage(context, target.Value));

            // if there are no updates then abort the pipeline
            if (context.Updates.Count == 0)
            {
                Console.WriteLine("\tNo updates found!");
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
                Console.WriteLine("\tGetting the latest version of {0} from {1}...",
                                  target.Name,
                                  target.NuGetFeedUrl);
            }
            else
            {
                Console.WriteLine("\tChecking {0} for update to {1} v{2}...",
                                  target.NuGetFeedUrl, target.Name,
                                  target.Version);
            }

            IPackage update;
            if (!FindPackage(context, target, out update))
            {
                return;
            }

            // should we download it? yes if...
            // o forcing a download
            // o no version has been specified
            // o upgrade to existing version available
            if (!target.Force  && ((target.Version != null) && (target.Version >= update.Version)))
            {
                Console.WriteLine("\t\tNo update available...running the latest version!");
                return;
            }

            // ok, lets download it!...
            Console.WriteLine("\t\tUpdated version v{0} is available", update.Version);

            var downloadFolder = Fluent.IO.Path.Get(context.Config.DownloadFolder, target.Name).FullPath;
            Console.WriteLine("\tDownloading package '{0}' content to: {1}...", target.Name, downloadFolder);

            Fluent.IO.Path.CreateDirectory(downloadFolder);
            var files = update.GetFiles();
            files.ToList().ForEach(file =>
            {
                Console.WriteLine("\t\t{0}", file.Path);
                DownloadFile(downloadFolder, file);
            });

            // add to list of updates (as long as it's not sidewinder itself)
            if (string.Compare(update.Id, Constants.Sidewinder.NuGetPackageName,
                StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                context.Updates.Add(new UpdatedPackage
                {
                    NewVersion = update.Version,
                    Target = target
                });
            }

            // should we update any related packages?
            if (!target.UpdateDependencies)
                return;

            Console.WriteLine("\t\tChecking for updates to dependent packages...");
            if (update.Dependencies != null)
            {
                update.Dependencies.ToList().ForEach(
                    dep => GetNuGetPackage(context, new TargetPackage
                    {
                        Name = dep.Id,
                        NuGetFeedUrl = target.NuGetFeedUrl,
                        UpdateDependencies = true
                    }));
            }
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
                Console.WriteLine("\t\t**WARNING** Package {0} does not exist on feed {1}",
                    target.Name,
                    target.NuGetFeedUrl);

                if (string.Compare(target.NuGetFeedUrl, Constants.NuGet.OfficialFeedUrl,
                    StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return false;
                }

                Console.WriteLine("\t\tAttempting to find it on the official feed...");
                repo = PackageRepositoryFactory.Default.CreateRepository(Constants.NuGet.OfficialFeedUrl);
                package = repo.FindPackage(target.Name);

                if (package == null)
                {
                    Console.WriteLine("\t\t**WARNING** Package {0} does not exist on feed {1}",
                                        target.Name,
                                        target.NuGetFeedUrl);
                    return false;
                }
            }

            // update the target feed just in case we get it from the fallback (official) feed
            target.NuGetFeedUrl = repo.Source;
            return true;
        }

        protected virtual void DownloadFile(string downloadFolder, IPackageFile file)
        {
            using (var stream = file.GetStream())
            {
                var filename = Path.GetFileName(file.Path);
                var folder = Fluent.IO.Path.Get(downloadFolder).Combine(file.Path).Parent().FullPath;
                Directory.CreateDirectory(folder);

                using (var destination = File.Create(Path.Combine(folder, filename)))
                    stream.CopyTo(destination);
            }
        }

    }
}