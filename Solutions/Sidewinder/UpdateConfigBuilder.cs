using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
                               Backup = true,
                               TargetPackages = new Dictionary<string, TargetPackage>(),
                               BackupFoldersToIgnore = new List<string>
                                                           {
                                                               DefaultDownloadFolder,
                                                               DefaultBackupFolder
                                                           }
                           };
        }

        /// <summary>
        /// This will add the latest version of the named package to the list 
        /// to update. This will be downloaded and updated irrespective of the 
        /// currently installed version from the default (official) nuget feed.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="frameworkHint"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Get(string name, string frameworkHint)
        {
            return Update(name, null, frameworkHint, Constants.NuGet.OfficialFeedUrl);
        }

        /// <summary>
        /// This will add the latest version of the named package to the list 
        /// to update. This will be downloaded and updated irrespective of the 
        /// currently installed version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="frameworkHint"></param>
        /// <param name="feedUrl"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Get(string name, string frameworkHint, string feedUrl)
        {
            return Update(name, null, frameworkHint);
        }

        /// <summary>
        /// This will add the named package to the list to update using the current
        /// version number of the running application from the default (official) nuget feed
        /// </summary>
        /// <param name="name"></param>
        /// <param name="frameworkHint"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Update(string name, string frameworkHint)
        {
            return Update(name, Assembly.GetEntryAssembly().GetName().Version, frameworkHint, Constants.NuGet.OfficialFeedUrl);
        }

        /// <summary>
        /// This will add the named package to the list to update using the current
        /// version number of the running application
        /// </summary>
        /// <param name="name"></param>
        /// <param name="frameworkHint"></param>
        /// <param name="feedUrl"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Update(string name, string frameworkHint, string feedUrl)
        {
            return Update(name, Assembly.GetEntryAssembly().GetName().Version, frameworkHint, feedUrl);
        }

        /// <summary>
        /// This will add the named, versioned package to the list to update. It will 
        /// use the offical nuget feed as the source
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version">The version number of the current package</param>
        /// <param name="frameworkHint"></param>
        /// <param name="feedUrl"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Update(string name, Version version, string frameworkHint, string feedUrl)
        {
            return Update(new TargetPackage
                               {
                                   Name = name,
                                   NuGetFeedUrl = feedUrl,
                                   FrameworkHint = frameworkHint,
                                   Version = version
                               });
        }

        /// <summary>
        /// This will add the target package to the list to check for updates. Use this if
        /// you need to check a custom feed or set the frameworkhint to help sidewinder locate
        /// your binaries
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Update(TargetPackage package)
        {
            if (myConfig.TargetPackages == null)
                myConfig.TargetPackages = new Dictionary<string, TargetPackage>();

            if (string.IsNullOrWhiteSpace(package.NuGetFeedUrl))
                package.NuGetFeedUrl = Constants.NuGet.OfficialFeedUrl;

            myConfig.TargetPackages.Add(package.Name, package);
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
                                 BackupFolder = GetFolderOrDefault(myConfig.BackupFolder, DefaultBackupFolder),
                                 InstallFolder = GetFolderOrDefault(myConfig.InstallFolder, Path.Get(Process.GetCurrentProcess().MainModule.FileName).Parent().FullPath), 
                                 TargetPackages = myConfig.TargetPackages,
                                 BackupFoldersToIgnore = myConfig.BackupFoldersToIgnore,
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

            // finally append the target package name to the download folder to allow
            // multiple packages to be downloaded (future feature)
            //config.DownloadFolder = Path.Get(config.DownloadFolder, config.TargetPackages).FullPath;

            return config;
        }

        private static string GetFolderOrDefault(string folder, string defaultFolder)
        {
            return SmartLocation.GetLocation(string.IsNullOrWhiteSpace(folder) ? defaultFolder : folder);
        }
    }
}