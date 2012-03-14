using System;
using System.Collections.Generic;

namespace Sidewinder.Core.Interfaces.Entities
{
    public class SidewinderCommands
    {
        public DistributeFiles DistributeFiles { get; set; }
    }

    public class DistributeFiles
    {
        public Version TargetFrameworkVersion { get; set; }
        public string TargetProcessFilename { get; set; }
        public List<UpdatedPackage> Updates { get; set; }
        public int SecondsToWait { get; set; }
        public ConflictResolutionTypes ConflictResolution { get; set; }

        //public string InstallFolder { get; set; }
        //public string DownloadFolder { get; set; }

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

        public DistributeFiles()
        {
            // default timeout to wait for the running process to terminate
            SecondsToWait = 10;
            ConflictResolution = ConflictResolutionTypes.Ask;
        }
    }
}