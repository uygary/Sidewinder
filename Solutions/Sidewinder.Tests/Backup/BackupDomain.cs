using System.IO;
using NUnit.Framework;
using Sidewinder.Core;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;
using Sidewinder.Tests.StandIns;

namespace Sidewinder.Tests.Backup
{
    public partial class BackupSpecs
    {
        private string _directoryToBackup;
        private string _backupToDirectory;
        private IPipelineStep<UpdaterContext> _backupStep;

        private void TheDirectory_ShouldBeBackedUp(string directory)
        {
            _directoryToBackup = SmartLocation.GetLocation(directory);
        }

        private void ItIsBackedup()
        {
            _backupStep = new TestBackupStep();

            var config = new UpdateConfig
                             {
                                 Backup = true,
                                 BackupFolder = _backupToDirectory,
                                 InstallFolder = _directoryToBackup,
                             };

            _backupStep.Execute(new UpdaterContext
                                     {
                                         Config = config
                                     });
        }

        private void TheBackupArtifactsAreCreated()
        {
            string backupFile = Path.Combine(_backupToDirectory, TestBackupStep.BackupFile);
            Assert.That(File.Exists(backupFile), Is.True);
        }

        private void TheBackupLocation_IsUsed(string archive)
        {
            _backupToDirectory = archive;
        }
    }
}