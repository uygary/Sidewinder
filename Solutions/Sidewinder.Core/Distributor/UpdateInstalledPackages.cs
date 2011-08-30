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
            InstalledPackages installed;
            var versionPath = Path.Get(context.Config.InstallFolder, Constants.Sidewinder.VersionFile);

            if (versionPath.Exists)
            {
                Console.WriteLine("\tFound installed versions file @{0}...", versionPath.FullPath);
                installed = SerialisationHelper<InstalledPackages>.DataContractDeserializeFromFile(versionPath.FullPath);
            }
            else
            {
                Console.WriteLine("\tCreating installed versions file @{0}...", versionPath.FullPath);
                installed = new InstalledPackages();
            }

            context.Config.Command.Updates.ForEach(update =>
                                                       {
                                                           if (installed.ContainsKey(update.Target.Name))
                                                               installed.Remove(update.Target.Name);
                                                           installed.Add(update.Target.Name, new InstalledPackage
                                                                                                 {
                                                                                                     Name = update.Target.Name,
                                                                                                     NuGetFeedUrl = update.Target.NuGetFeedUrl,
                                                                                                     Version = update.NewVersion
                                                                                                 });
                                                       });

            SerialisationHelper<InstalledPackages>.DataContractSerialize(versionPath.FullPath, installed);
            return true;
        }

        public void ExitConditions(DistributorContext context)
        {            
        }
    }
}