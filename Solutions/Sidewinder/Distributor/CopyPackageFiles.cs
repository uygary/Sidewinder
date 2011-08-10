
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
            if (string.IsNullOrWhiteSpace(context.Config.Package.DownloadFolder))
                throw new ArgumentException("Config.Package.DownloadFolder property not set", "context");
            if (string.IsNullOrWhiteSpace(context.Config.InstallFolder))
                throw new ArgumentException("Config.InstallFolder property not set", "context");
        }

        public bool Execute(DistributorContext context)
        {
            Path.Get(context.Config.InstallFolder).CreateDirectory();

            // copy the content files
            Path.Get(context.Config.Package.DownloadFolder, "Content")
                .Copy(context.Config.InstallFolder, Overwrite.Never, true);

            Path.Get(context.BinariesFolder)
                .Copy(context.Config.InstallFolder, Overwrite.Always, true);

            Path.Get(context.Config.Package.DownloadFolder, "Tools")
                .Copy(context.Config.InstallFolder, Overwrite.Always, true);
            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            
        }
    }
}