using System;
using System.Linq;
using System.Text;
using Fluent.IO;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Updater
{
    /// <summary>
    /// This will attempt to find the version number of any already installed
    /// packages, this allows us to only download a new version if an update is
    /// available. The current versions are recorded in a file found in the app
    /// installation folder.
    /// </summary>
    public class DiscoverInstalledPackages : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            
        }

        public bool Execute(UpdaterContext context)
        {
            var versionPath = Path.Get(context.Config.InstallFolder, Constants.Sidewinder.VersionFile);

            if (!versionPath.Exists)
                return true;

            var path = versionPath.FullPath;
            Logger.Info("\tFound installed versions file @{0}...", path);
            context.InstalledPackages = SerialisationHelper<InstalledPackages>.DataContractDeserializeFromFile(path);

            Logger.Debug("\tConfig.InstalledPackages...");
            context.InstalledPackages.ToList()
                .ForEach(pkg =>
                             {
                                 var sb = new StringBuilder();
                                 sb.AppendFormat("\t\t{0}", pkg.Value.Name);

                                 if (pkg.Value.Version != null)
                                     sb.AppendFormat(" v{0}", pkg.Value.Version);

                                 sb.AppendFormat(" Last Updated:{0}", pkg.Value.LastUpdated);

                                 if (string.Compare(Constants.NuGet.OfficialFeedUrl,
                                                    pkg.Value.NuGetFeedUrl,
                                                    StringComparison.InvariantCultureIgnoreCase) != 0)
                                     sb.AppendFormat(" ({0})", pkg.Value.NuGetFeedUrl);

                                 Logger.Debug(sb.ToString());
                             });
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}