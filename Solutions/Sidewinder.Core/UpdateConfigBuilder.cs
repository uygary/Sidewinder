using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Fluent.IO;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core
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
                               // try to guess the runtime from the app we are running inside
                               TargetFrameworkVersion = GetRuntimeVersion(),
                               BackupFoldersToIgnore = new List<string>
                                                           {
                                                               DefaultDownloadFolder,
                                                               DefaultBackupFolder
                                                           }
                           };
        }

        private static Version GetRuntimeVersion()
        {
            return new Version(Assembly.GetEntryAssembly().ImageRuntimeVersion.TrimStart('v'));
        }

        public UpdateConfigBuilder TargetFrameworkVersion11()
        {
            return TargetFrameworkVersion(new Version(1, 1));
        }

        public UpdateConfigBuilder TargetFrameworkVersion20()
        {
            return TargetFrameworkVersion(new Version(2, 0));
        }

        public UpdateConfigBuilder TargetFrameworkVersion40()
        {
            return TargetFrameworkVersion(new Version(4, 0));
        }

        public UpdateConfigBuilder TargetFrameworkVersion(Version version)
        {
            myConfig.TargetFrameworkVersion = version;
            return this;
        }

        /// <summary>
        /// This will add the latest version of the named package to the list 
        /// to update. This will be downloaded and updated irrespective of the 
        /// currently installed version from the default (official) nuget feed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Get(string name)
        {
            return Get(name, Constants.NuGet.OfficialFeedUrl, false);
        }

        /// <summary>
        /// This will add the latest version of the named package to the list 
        /// to update. This will be downloaded and updated irrespective of the 
        /// currently installed version from the default (official) nuget feed.
        /// All dependent NuGet packages will be updated as well.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Get(string name, bool force)
        {
            return Get(name, Constants.NuGet.OfficialFeedUrl, force);
        }

        /// <summary>
        /// This will add the latest version of the named package to the list 
        /// to update. This will be downloaded and updated irrespective of the 
        /// currently installed version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="feedUrl"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Get(string name, string feedUrl)
        {
            return Get(name, feedUrl, false);
        }

        /// <summary>
        /// This will add the latest version of the named package to the list 
        /// to update. This will be downloaded and updated irrespective of the 
        /// currently installed version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="feedUrl"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Get(string name, string feedUrl, bool force)
        {
            return Update(new TargetPackage
                              {
                                  Force = force,
                                  Name = name,
                                  NuGetFeedUrl = feedUrl,
                              });
        }

        /// <summary>
        /// This will add the named package to the list to update using the current
        /// version number of the running application from the default (official) nuget feed
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Update(string name)
        {
            return Update(name, Assembly.GetEntryAssembly().GetName().Version, Constants.NuGet.OfficialFeedUrl);
        }

        /// <summary>
        /// This will add the named package to the list to update using the current
        /// version number of the running application
        /// </summary>
        /// <param name="name"></param>
        /// <param name="feedUrl"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Update(string name, string feedUrl)
        {
            return Update(name, Assembly.GetEntryAssembly().GetName().Version, feedUrl);
        }

        /// <summary>
        /// This will add the named, versioned package to the list to update. It will 
        /// use the offical nuget feed as the source
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version">The version number of the current package</param>
        /// <param name="feedUrl"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Update(string name, Version version, string feedUrl)
        {
            return Update(new TargetPackage
                               {
                                   Name = name,
                                   NuGetFeedUrl = feedUrl,
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

        public UpdateConfigBuilder InstallInto(string folder)
        {
            myConfig.InstallFolder = folder;
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
                                 TargetFrameworkVersion = myConfig.TargetFrameworkVersion
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