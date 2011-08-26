
using System;
using Fluent.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Updater
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
            Console.WriteLine("\tFound installed versions file @{0}...", path);
            context.InstalledPackages = SerialisationHelper<InstalledPackages>.DataContractDeserializeFromFile(path);
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}