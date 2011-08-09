using Sidewinder.Updater;

namespace Sidewinder.Tests.StandIns
{
    public class TestBackupStep : BackupApplication
    {
        public const string BackupFile = "test.zip";

        protected override string BuildBackupFilename()
        {
            return BackupFile;
        }
    }
}