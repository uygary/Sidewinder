using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ionic.Zip;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;
using Path = Fluent.IO.Path;

namespace Sidewinder.Core.Updater
{
    public class BackupApplication : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.Config == null)
                throw new ArgumentException("Config property is null", "context");
            if (string.IsNullOrWhiteSpace(context.Config.InstallFolder))
                throw new ArgumentException("Config.InstallFolder property not set", "context");
            if (string.IsNullOrWhiteSpace(context.Config.BackupFolder))
                throw new ArgumentException("Config.BackupFolder property not set", "context");
        }

        public bool Execute(UpdaterContext context)
        {
            if (!context.Config.Backup)
                return true;

            Logger.Info("\tBacking up existing application @{0} to {1}", context.Config.InstallFolder,
                context.Config.BackupFolder);
            if (!Directory.Exists(context.Config.InstallFolder))
                throw new DirectoryNotFoundException(string.Format("Unable to backup folder '{0}' as it does not exist",
                    context.Config.InstallFolder));

            Path.CreateDirectory(context.Config.BackupFolder);
            var zipFile = Path.Get(context.Config.BackupFolder).Combine(BuildBackupFilename()).FullPath;

            using (var archive = new ZipFile())
            {
                archive.AddDirectory(context.Config.InstallFolder);
                RemoveFolderFromArchive(archive, context.Config.BackupFolder);
                    
                if (context.Config.BackupFoldersToIgnore!= null)
                    context.Config.BackupFoldersToIgnore.ForEach(fti => RemoveFolderFromArchive(archive, fti));

                archive.Save(zipFile);
            }

            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }


        protected virtual bool RemoveFolderFromArchive(ZipFile zip, string folder)
        {
            var folderToRemove = Path.Get(folder).FileName + "/";
            var entries = zip.Entries.Where(entry => entry.FileName.StartsWith(folderToRemove));
            entries.ToList().ForEach(entry =>
            {
                zip.RemoveEntry(entry);
                Logger.Debug("\t\tRemoved entry '{0}'", entry.FileName);
            });
            return true;
        }

        /// <summary>
        /// This should build the filename used for the backup archive
        /// </summary>
        /// <returns></returns>
        protected virtual string BuildBackupFilename()
        {
            return string.Format("backup_{0}.zip", Assembly.GetEntryAssembly().GetName().Version);
        }
    }
}