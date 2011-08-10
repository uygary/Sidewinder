
using System;
using Fluent.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Distributor
{
    /// <summary>
    /// This step will copy the files from the nuget package to the application installation folder
    /// </summary>
    public class CopyPackageFiles : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.Config == null)
                throw new ArgumentException("Config property is null", "context");
            if (string.IsNullOrWhiteSpace(context.Config.Command.DownloadFolder))
                throw new ArgumentException("Config.Package.DownloadFolder property not set", "context");
            if (string.IsNullOrWhiteSpace(context.Config.InstallFolder))
                throw new ArgumentException("Config.InstallFolder property not set", "context");
        }

        public bool Execute(DistributorContext context)
        {
            Console.WriteLine("\tCopying update files to {0}...", context.Config.InstallFolder);
            Path.Get(context.Config.InstallFolder).CreateDirectory();

            // copy the content files
            if (Path.Get(context.Config.Command.DownloadFolder, "Content").Exists)
            {
                Console.WriteLine("\t\tCopying Content files...");
                Path.Get(context.Config.Command.DownloadFolder, "Content")
                    .Copy(context.Config.InstallFolder, Overwrite.Never, true);
            }

            if (Path.Get(context.BinariesFolder).Exists)
            {
                Console.WriteLine("\t\tCopying Binaries from {0}...", context.BinariesFolder);
                Path.Get(context.BinariesFolder)
                    .Copy(context.Config.InstallFolder, Overwrite.Always, true);
            }

            if (Path.Get(context.Config.Command.DownloadFolder, "Tools").Exists)
            {
                Console.WriteLine("\t\tCopying Tool files...");
                Path.Get(context.Config.Command.DownloadFolder, "Tools")
                    .Copy(context.Config.InstallFolder, Overwrite.Always, true);
            }

            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            
        }
    }
}