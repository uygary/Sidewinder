using Fluent.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;
using FluentAssertions;

namespace Sidewinder.Distributor
{
    /// <summary>
    /// This will find the location of the binaries within the download
    /// </summary>
    public class GetPackageBinariesLocation : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            context.Config.DownloadFolder.Should().NotBeNullOrEmpty();
        }

        public bool Execute(DistributorContext context)
        {
            var folder = Path.Get(context.Config.DownloadFolder, "lib");

            if (!string.IsNullOrWhiteSpace(context.Config.PackageFrameworkHint))
            {
                folder = folder.Combine(context.Config.PackageFrameworkHint);
            }

            context.BinariesFolder = folder.FullPath;
            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            context.BinariesFolder.Should().NotBeNullOrEmpty();
        }
    }
}