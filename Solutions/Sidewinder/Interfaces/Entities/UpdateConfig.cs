namespace Sidewinder.Interfaces.Entities
{
    public class UpdateConfig
    {
        public bool Backup { get; set; }
        public string NuGetFeedUrl { get; set; }
        public string TargetPackage { get; set; }
        public string BackupFolder { get; set; }
        public string DownloadFolder { get; set; }
        public string InstallFolder { get; set; }
    }
}