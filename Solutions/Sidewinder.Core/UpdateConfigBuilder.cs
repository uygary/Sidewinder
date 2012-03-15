using System;
using System.Collections.Generic;
using System.Reflection;
using Fluent.IO;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core
{
    public class UpdateConfigBuilder
    {
        private readonly UpdateConfig myConfig;

        public UpdateConfigBuilder()
        {
            myConfig = new UpdateConfig
                           {        
                               Backup = true,
                               ConflictResolution = ConflictResolutionTypes.Ask,
                               TargetPackages = new TargetPackages(),
                               InstallFolder = SmartLocation.GetBinFolder(),
                               // try to guess the runtime from the app we are running inside
                               TargetFrameworkVersion = GetRuntimeVersion(),
                               BackupFoldersToIgnore = new List<string>
                                                           {
                                                               Constants.Sidewinder.DefaultDownloadFolder,
                                                               Constants.Sidewinder.DefaultBackupFolder
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
            return Update(name, null, Constants.NuGet.OfficialFeedUrl);
        }

        /// <summary>
        /// This will add the named package to the list to update using the version number
        /// specfied as the current installed version from the default (official) nuget feed
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public UpdateConfigBuilder Update(string name, Version version)
        {
            return Update(name, version, Constants.NuGet.OfficialFeedUrl);
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
            return Update(name, null, feedUrl);
        }

        /// <summary>
        /// This will add the named, versioned package to the list to update. It will 
        /// use the specified feed to locate the package
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
                myConfig.TargetPackages = new TargetPackages();

            if (string.IsNullOrWhiteSpace(package.NuGetFeedUrl))
                package.NuGetFeedUrl = Constants.NuGet.OfficialFeedUrl;

            myConfig.TargetPackages.Add(package);
            return this;
        }

        public UpdateConfigBuilder InstallInto(string folder)
        {
            myConfig.InstallFolder = folder;
            return this;
        }

        /// <summary>
        /// Gets the version number of the currently running app
        /// </summary>
        /// <returns></returns>
        public Version CurrentAppVersion()
        {
            return Assembly.GetEntryAssembly().GetName().Version;
        }

        /// <summary>
        /// Ensures that only the packages you have specified are updated - otherwise 
        /// all installed packages will be checked for updates.
        /// </summary>
        /// <returns></returns>
        public UpdateConfigBuilder JustThesePackages()
        {
            myConfig.JustThis = true;
            return this;
        }

        public UpdateConfigBuilder DoNotBackup()
        {
            myConfig.Backup = false;
            return this;
        }

        /// <summary>
        /// By default, resolution of a conflict where a content file already exists is
        /// manual - the new file will be left in the update folder and the user must decide
        /// the course of action to take (merge/overwrite etc). If this method is called 
        /// then any content files that exist in the installation folder will be overwritten
        /// by the version in the update package.
        /// </summary>
        /// <returns></returns>
        public UpdateConfigBuilder OverwriteFiles()
        {
            myConfig.ConflictResolution = ConflictResolutionTypes.Overwrite;
            return this;
        }

        /// <summary>
        /// By default, resolution of a conflict where a content file already exists is
        /// manual - the new file will be left in the update folder and the user must decide
        /// the course of action to take (merge/overwrite etc). If this method is called 
        /// then the user is prompted to skip or overwrite the conflicted file. If skipped
        /// the file will be left in the update folder for the user to deal with post update.
        /// </summary>
        /// <returns></returns>
        public UpdateConfigBuilder AskUserToOverwrite()
        {
            myConfig.ConflictResolution = ConflictResolutionTypes.Ask;
            return this;
        }

        public UpdateConfig Build()
        {
            var config = new UpdateConfig
                             {
                                 Backup = myConfig.Backup,                                 
                                 BackupFolder = GetFolderOrDefault(myConfig.BackupFolder, GetDefaultBackupFolder()),
                                 ConflictResolution = myConfig.ConflictResolution,
                                 DownloadFolder = GetFolderOrDefault(myConfig.DownloadFolder, GetDefaultDownloadFolder()),
                                 InstallFolder = myConfig.InstallFolder, 
                                 TargetPackages = myConfig.TargetPackages,
                                 BackupFoldersToIgnore = myConfig.BackupFoldersToIgnore,
                                 TargetFrameworkVersion = myConfig.TargetFrameworkVersion,
                                 JustThis = myConfig.JustThis
                             };
            return config;
        }

        private string GetDefaultBackupFolder()
        {
            return Path.Get(myConfig.InstallFolder, Constants.Sidewinder.DefaultBackupFolder).FullPath;
        }

        private string GetDefaultDownloadFolder()
        {
            return Path.Get(myConfig.InstallFolder, Constants.Sidewinder.DefaultDownloadFolder).FullPath;
        }

        private static string GetFolderOrDefault(string folder, string defaultFolder)
        {
            return SmartLocation.GetLocation(string.IsNullOrWhiteSpace(folder) ? defaultFolder : folder);
        }
    }
}