using System;
using System.Collections.Generic;

namespace Sidewinder.Core.Interfaces.Entities
{
    public class UpdateConfig
    {
        public bool Backup { get; set; }
        public bool JustThis { get; set; }
        public Version TargetFrameworkVersion { get; set; }
        public TargetPackages TargetPackages { get; set; }
        public List<string> BackupFoldersToIgnore { get; set; }

        private string myBackupFolder;
        public string BackupFolder
        {
            get { return myBackupFolder; }
            set { myBackupFolder = SmartLocation.GetLocation(value); }
        }
        
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

        public ConflictResolutionTypes ConflictResolution { get; set; }
    }
}