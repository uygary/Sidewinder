namespace Sidewinder.Interfaces.Entities
{
    public class DistributorConfig
    {
        public DistributeFiles Process { get; set; }
        public string PackageFrameworkHint { get; set; }

        private string myDownloadFolder;
        public string DownloadFolder
        {
            get { return myDownloadFolder; }
            set { myDownloadFolder = SmartLocation.GetLocation(value); }
        }

        private string myInstallFolder;
        public string InstallFolder
        {
            get { return myInstallFolder; }
            set { myInstallFolder = SmartLocation.GetLocation(value); }
        }
    }
}