using System;
using System.Collections.Generic;

namespace Sidewinder.Core.Interfaces.Entities
{
    public class UpdateConfig
    {
        public bool SkipOfficialFeed { get; set; }
        public bool Backup { get; set; }
        public bool JustThis { get; set; }
        public Version TargetFrameworkVersion { get; set; }
        public TargetPackages TargetPackages { get; set; }
        public ConflictResolutionTypes ConflictResolution { get; set; }
        public List<string> BackupFoldersToIgnore { get; set; }
        public ILogger Logger { get; set; }
        public Level LoggingLevel { get; set; }
        public string LaunchProcess { get; set; }
        public string CustomSidewinderFeedUrl { get; set; }

        private string _backupFolder;
        public string BackupFolder
        {
            get { return _backupFolder; }
            set { _backupFolder = SmartLocation.GetLocation(value); }
        }
        
        private string _downloadFolder;
        public string DownloadFolder
        {
            get { return _downloadFolder; }
            set { _downloadFolder = SmartLocation.GetLocation(value); }
        }

        private string _installFolder;
        public string InstallFolder
        {
            get { return _installFolder; }
            set { _installFolder = SmartLocation.GetLocation(value); }
        }
    }
}