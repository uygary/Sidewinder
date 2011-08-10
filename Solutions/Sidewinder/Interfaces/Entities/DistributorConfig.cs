using Fluent.IO;

namespace Sidewinder.Interfaces.Entities
{
    public class DistributorConfig
    {
        public DistributeFiles Package { get; set; }

        private string myInstallFolder;
        public string InstallFolder
        {
            get { return myInstallFolder; }
            set { myInstallFolder = SmartLocation.GetLocation(value); }
        }
    }
}