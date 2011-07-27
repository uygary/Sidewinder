using System.Diagnostics;
using Fluent.IO;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder
{
    public class UpdateConfigBuilder
    {
        public const string DefaultDownloadFolder = "_update";
        public const string DefaultBackupFolder = "_backup";

        private readonly UpdateConfig myConfig;

        public UpdateConfigBuilder()
        {
            myConfig = new UpdateConfig
                           {
                               NuGetFeedUrl = "https://go.microsoft.com/fwlink/?LinkID=206669",
                               Backup = true
                           };
        }

        public UpdateConfigBuilder Package(string name)
        {
            myConfig.TargetPackage = name;
            return this;
        }

        public UpdateConfigBuilder DoNotBackup()
        {
            myConfig.Backup = false;
            return this;
        }

        public UpdateConfig Build()
        {
            var config = new UpdateConfig
                             {
                                 Backup = myConfig.Backup,                                 
                                 NuGetFeedUrl = myConfig.NuGetFeedUrl,
                                 BackupFolder = GetFolderOrDefault(myConfig.BackupFolder, DefaultBackupFolder),
                                 InstallFolder = GetFolderOrDefault(myConfig.InstallFolder, Path.Get(Process.GetCurrentProcess().MainModule.FileName).Parent().FullPath), 
                                 TargetPackage = myConfig.TargetPackage
                             };

            if (string.IsNullOrWhiteSpace(myConfig.DownloadFolder))
            {
                // use default download folder and take either the specified install folder
                // OR assume the install folder is parent of default location
                config.DownloadFolder = SmartLocation.GetLocation(DefaultDownloadFolder);
                config.InstallFolder = GetFolderOrDefault(myConfig.InstallFolder,
                                                          Path.Get(config.DownloadFolder).Parent().FullPath);

            }
            else
            {
                // download folder specified - this means the installation folder must also be provided
                config.DownloadFolder = GetFolderOrDefault(myConfig.DownloadFolder, DefaultDownloadFolder);
                config.InstallFolder = SmartLocation.GetLocation(myConfig.InstallFolder);
            }

            return config;
        }

        private static string GetFolderOrDefault(string folder, string defaultFolder)
        {
            return SmartLocation.GetLocation(string.IsNullOrWhiteSpace(folder) ? defaultFolder : folder);
        }
    }
}