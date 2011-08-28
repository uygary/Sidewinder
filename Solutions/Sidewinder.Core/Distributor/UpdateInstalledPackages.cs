using System;
using Fluent.IO;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Distributor
{
    /// <summary>
    /// This will update the list of installed packages
    /// </summary>
    public class UpdateInstalledPackages : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            
        }

        public bool Execute(DistributorContext context)
        {
            var versionPath = Path.Get(context.Config.InstallFolder, Constants.Sidewinder.VersionFile);

            if (!versionPath.Exists)
                return true;

            var path = versionPath.FullPath;
            Console.WriteLine("\tFound installed versions file @{0}...", path);
            var versions = SerialisationHelper<InstalledPackages>.DataContractDeserializeFromFile(path);

            //context.Config.Command.Updates

            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            // TODO: check all the download\package folders have been deleted (except for sidewinder)
        }
    }
}