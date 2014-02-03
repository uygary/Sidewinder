using System;
using System.Collections.Generic;

namespace Sidewinder.Core.Interfaces.Entities
{
    public class SidewinderCommands
    {
        public Level LogLevel { get; set; }
        public DistributeFiles DistributeFiles { get; set; }

        public SidewinderCommands()
        {
            LogLevel = Level.Debug;
        }
    }

    public class DistributeFiles
    {
        public Version TargetFrameworkVersion { get; set; }
        public string TargetProcessFilename { get; set; }
        public List<UpdatedPackage> Updates { get; set; }
        public int SecondsToWait { get; set; }
        public ConflictResolutionTypes ConflictResolution { get; set; }
        public string LaunchProcess { get; set; }

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

        public DistributeFiles()
        {
            // default timeout to wait for the running process to terminate
            SecondsToWait = 10;
            ConflictResolution = ConflictResolutionTypes.Ask;            
        }
    }
}