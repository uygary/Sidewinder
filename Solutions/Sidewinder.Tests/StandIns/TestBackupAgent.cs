namespace Sidewinder.Tests.StandIns
{
    public class TestBackupAgent : DefaultBackupAgent
    {
        public const string BackupFile = "test.zip";

        protected override string BuildBackupFilename()
        {
            return BackupFile;
        }
    }
}