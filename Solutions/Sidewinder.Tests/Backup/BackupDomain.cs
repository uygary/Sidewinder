using System.IO;
using NUnit.Framework;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;
using Sidewinder.Tests.StandIns;

namespace Sidewinder.Tests.Backup
{
    public partial class BackupSpecs
    {
        private string myDirectoryToBackup;
        private string myBackupToDirectory;
        private IPipelineStep<UpdaterContext> myBackupStep;

        private void TheDirectory_ShouldBeBackedUp(string directory)
        {
            myDirectoryToBackup = SmartLocation.GetLocation(directory);
        }

        private void ItIsBackedup()
        {
            myBackupStep = new TestBackupStep();
            myBackupStep.Execute(new UpdaterContext
                                     {
                                         Config = new UpdateConfig
                                                      {
                                                          Backup = true,
                                                          BackupFolder = myBackupToDirectory,
                                                          InstallFolder = myDirectoryToBackup,
                                                          TargetPackage = "TestPackage"
                                                      }
                                     });
        }

        private void TheBackupArtifactsAreCreated()
        {
            string backupFile = Path.Combine(myBackupToDirectory, TestBackupStep.BackupFile);
            Assert.That(File.Exists(backupFile), Is.True);
        }

        private void TheBackupLocation_IsUsed(string archive)
        {
            myBackupToDirectory = archive;
        }
    }
}