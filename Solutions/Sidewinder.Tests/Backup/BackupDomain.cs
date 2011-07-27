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
        private IBackupAgent myBackupAgent;

        private void TheDirectory_ShouldBeBackedUp(string directory)
        {
            myDirectoryToBackup = SmartLocation.GetLocation(directory);
        }

        private void ItIsBackedup()
        {
            myBackupAgent = new TestBackupAgent();
            myBackupAgent.Backup(new BackupConfig
                                     {
                                         DirectoryToBackup = myDirectoryToBackup,
                                         BackupTo = myBackupToDirectory
                                     });
        }

        private void TheBackupArtifactsAreCreated()
        {
            string backupFile = Path.Combine(myBackupToDirectory, TestBackupAgent.BackupFile);
            Assert.That(File.Exists(backupFile), Is.True);
        }

        private void TheBackupLocation_IsUsed(string archive)
        {
            myBackupToDirectory = archive;
        }
    }
}