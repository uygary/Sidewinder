
using Fluent.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;
using FluentAssertions;

namespace Sidewinder.Distributor
{
    /// <summary>
    /// This step will copy the files from the nuget package to the application installation folder
    /// </summary>
    public class CopyPackageFiles : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            context.Config.DownloadFolder.Should().NotBeNullOrEmpty();
            context.Config.InstallFolder.Should().NotBeNullOrEmpty();
        }

        public bool Execute(DistributorContext context)
        {
            Path.Get(context.Config.InstallFolder).CreateDirectory();

            // copy the content files
            Path.Get(context.Config.DownloadFolder, "Content")
                .Copy(context.Config.InstallFolder, Overwrite.Never, true);

            Path.Get(context.BinariesFolder)
                .Copy(context.Config.InstallFolder, Overwrite.Always, true);

            Path.Get(context.Config.DownloadFolder, "Tools")
                .Copy(context.Config.InstallFolder, Overwrite.Always, true);
            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            
        }
    }
}