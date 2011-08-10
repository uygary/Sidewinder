using Fluent.IO;
using Sidewinder.Interfaces.Entities;
using FluentAssertions;

namespace Sidewinder.Tests.Copy
{
    public partial class CopyPackageFilesSpecs
    {
        private string myInstallationFolder;
        private string myUpdateFolder;
        private string myUpdateBinariesFolder;

        private void TheDirectory_ContainsThePackageFiles(string folder)
        {
            myUpdateFolder = folder;
        }
        
        private void TheDirectory_ContainsThePackageBinaryFiles(string folder)
        {
            myUpdateBinariesFolder = folder;
        }

        private void TheInstallLocation_IsUsed(string folder)
        {
            myInstallationFolder = folder;
        }

        private void TheInstallationLocationIsCleaned()
        {
            Path.Get(myInstallationFolder).Delete(true);
        }

        private void TheFilesAreCopied()
        {
            var context = new DistributorContext
                              {
                                  BinariesFolder = myUpdateBinariesFolder,
                                  Config = new DistributorConfig
                                               {
                                                   InstallFolder = myInstallationFolder,
                                                   Command = new DistributeFiles
                                                                 {
                                                                     DownloadFolder = myUpdateFolder
                                                                 }
                                               }
                              };

            myStep.EntryConditions(context);
            myStep.Execute(context);
            myStep.ExitConditions(context);
        }

        private void TheInstallationFolderContentIsCorrect()
        {
            // content
            Path.Get(myInstallationFolder).Exists.Should().BeTrue();            
            Path.Get(myInstallationFolder, "Content_Sub1").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Content_Sub1", "content_sub1.txt").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Content_Sub2").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Content_Sub2", "content_sub2.txt").Exists.Should().BeTrue();

            // binaries
            Path.Get(myInstallationFolder, "net40.txt").Exists.Should().BeTrue();

            // tools
            Path.Get(myInstallationFolder, "Tools_Sub1").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Tools_Sub1", "tools_sub1.txt").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Tools_Sub2").Exists.Should().BeTrue();
            Path.Get(myInstallationFolder, "Tools_Sub2", "tools_sub2.txt").Exists.Should().BeTrue();

            // these should not be copied
            Path.Get(myInstallationFolder, "Content").Exists.Should().BeFalse();
            Path.Get(myInstallationFolder, "lib").Exists.Should().BeFalse();
            Path.Get(myInstallationFolder, "net35").Exists.Should().BeFalse();
            Path.Get(myInstallationFolder, "net40").Exists.Should().BeFalse();

        }
    }
}