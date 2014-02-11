using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Fluent.IO;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core
{
    public class UpdateConfigBuilder
    {
        private readonly UpdateConfig _config;

        public UpdateConfigBuilder()
        {
            _config = new UpdateConfig
                           { 
                               SkipOfficialFeed = false,
                               Backup = true,
                               LoggingLevel = Level.Debug,
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
            return new Version(GetApplicationAssembly().ImageRuntimeVersion.TrimStart('v'));
        }

        private static Assembly GetApplicationAssembly()
        {
            return Assembly.GetEntryAssembly() // can be null if invoked from a unit-test. See http://msdn.microsoft.com/en-us/library/system.reflection.assembly.getentryassembly(v=vs.100).aspx
                ?? Assembly.GetCallingAssembly();
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

        public UpdateConfigBuilder TargetFrameworkVersion45()
        {
            return TargetFrameworkVersion(new Version(4, 5));
        }

        public UpdateConfigBuilder TargetFrameworkVersion(Version version)
        {
            _config.TargetFrameworkVersion = version;
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
            if (_config.TargetPackages == null)
                _config.TargetPackages = new TargetPackages();

            if (string.IsNullOrWhiteSpace(package.NuGetFeedUrl))
                package.NuGetFeedUrl = Constants.NuGet.OfficialFeedUrl;

            _config.TargetPackages.Add(package);
            return this;
        }

        public UpdateConfigBuilder InstallInto(string folder)
        {
            _config.InstallFolder = folder;
            return this;
        }

        /// <summary>
        /// Gets the version number of the currently running app
        /// </summary>
        /// <returns></returns>
        public Version CurrentAppVersion()
        {
            return GetApplicationAssembly().GetName().Version;
        }

        /// <summary>
        /// Ensures that only the packages you have specified are updated - otherwise 
        /// all installed packages will be checked for updates.
        /// </summary>
        /// <returns></returns>
        public UpdateConfigBuilder JustThesePackages()
        {
            _config.JustThis = true;
            return this;
        }

        public UpdateConfigBuilder DoNotBackup()
        {
            _config.Backup = false;
            return this;
        }

        /// <summary>
        /// Any content files that exist in the installation folder will be overwritten
        /// by the version in the update package.
        /// </summary>
        /// <returns></returns>
        public UpdateConfigBuilder OverwriteContentFiles()
        {
            _config.ConflictResolution = ConflictResolutionTypes.Overwrite;
            return this;
        }

        /// <summary>
        /// By default, resolution of a conflict where a content file already exists is
        /// to ask the user to confirm/skip overwrite the file. The user will be prompted in the
        /// distribution stage when the content files are copied from the downloaded package.
        /// </summary>
        /// <returns></returns>
        public UpdateConfigBuilder AskUserToResolveContentConflicts()
        {
            _config.ConflictResolution = ConflictResolutionTypes.Ask;
            return this;
        }

        /// <summary>
        /// Use this option if you want to manually resolve any file conflicts - the package
        /// will be left in the _updated folder (usually this is deleted post install) and the
        /// user can select which files to update.
        /// </summary>
        public UpdateConfigBuilder  UserWillManuallyResolveContentConflicts()
        {
            _config.ConflictResolution = ConflictResolutionTypes.Manual;
            return this;
        }

        public UpdateConfigBuilder SetLoggingLevel(Level level)
        {
            _config.LoggingLevel = level;
            return this;
        }

        public UpdateConfigBuilder SetLoggingLevel(string level)
        {
            Level enumLevel;

            if (!Enum.TryParse(level, true, out enumLevel))
                enumLevel = Level.Debug;
            _config.LoggingLevel = enumLevel;
            return this;
        }

        public UpdateConfigBuilder UseLogger(ILogger logger)
        {
            _config.Logger = logger;
            return this;
        }

        public UpdateConfigBuilder UseLogPath(string logPath)
        {
            _config.LogPath = logPath;
            return this;
        }

        public UpdateConfigBuilder SkipOfficialFeed()
        {
            _config.SkipOfficialFeed = true;
            return this;
        }

        public UpdateConfigBuilder LaunchAfterUpdate(string cmdLine)
        {
            _config.LaunchProcess = cmdLine;
            return this;
        }

        public UpdateConfigBuilder RelaunchSelfAfterUpdate()
        {
            _config.LaunchProcess = Environment.CommandLine;
            return this;
        }

        public UpdateConfigBuilder UseCustomFeedForSidewinder(string feedUrl)
        {
            _config.CustomSidewinderFeedUrl = feedUrl;
            return this;
        }

        public UpdateConfig Build()
        {
            var config = new UpdateConfig
                             {
                                 SkipOfficialFeed = _config.SkipOfficialFeed,
                                 Backup = _config.Backup,
                                 Logger = _config.Logger,
                                 LoggingLevel = _config.LoggingLevel,
                                 BackupFolder = GetFolderOrDefault(_config.BackupFolder, GetDefaultBackupFolder()),
                                 ConflictResolution = _config.ConflictResolution,
                                 DownloadFolder = GetFolderOrDefault(_config.DownloadFolder, GetDefaultDownloadFolder()),
                                 InstallFolder = _config.InstallFolder, 
                                 LogPath = _config.LogPath,
                                 TargetPackages = _config.TargetPackages,
                                 BackupFoldersToIgnore = _config.BackupFoldersToIgnore,
                                 TargetFrameworkVersion = _config.TargetFrameworkVersion,
                                 JustThis = _config.JustThis,
                                 LaunchProcess = _config.LaunchProcess,
                                 CustomSidewinderFeedUrl = _config.CustomSidewinderFeedUrl
                             };
            return config;
        }

        private string GetDefaultBackupFolder()
        {
            return Path.Get(_config.InstallFolder, Constants.Sidewinder.DefaultBackupFolder).FullPath;
        }

        private string GetDefaultDownloadFolder()
        {
            return Path.Get(_config.InstallFolder, Constants.Sidewinder.DefaultDownloadFolder).FullPath;
        }

        private static string GetFolderOrDefault(string folder, string defaultFolder)
        {
            return SmartLocation.GetLocation(string.IsNullOrWhiteSpace(folder) ? defaultFolder : folder);
        }
    }
}