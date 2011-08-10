using System;
using Fluent.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Distributor
{
    /// <summary>
    /// This will find the location of the binaries within the download
    /// </summary>
    public class GetPackageBinariesLocation : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.Config == null)
                throw new ArgumentException("Config property is null", "context");
            if (string.IsNullOrWhiteSpace(context.Config.Command.DownloadFolder))
                throw new ArgumentException("Config.DownloadFolder property not set", "context");
        }

        public bool Execute(DistributorContext context)
        {
            var folder = Path.Get(context.Config.Command.DownloadFolder, "lib");

            if (!string.IsNullOrWhiteSpace(context.Config.Command.FrameworkHint))
            {
                folder = folder.Combine(context.Config.Command.FrameworkHint);
            }

            context.BinariesFolder = folder.FullPath;
            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            if (string.IsNullOrWhiteSpace(context.BinariesFolder))
                throw new ArgumentException("BinariesFolder property not set", "context");
        }
    }
}