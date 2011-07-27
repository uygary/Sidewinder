using System;
using System.IO;
using System.Reflection;
using Ionic.Zip;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;
using Path = Fluent.IO.Path;
using System.Linq;

namespace Sidewinder
{
    public class DefaultBackupAgent : IBackupAgent
    {
        public void Backup(BackupConfig config)
        {
            if (!Directory.Exists(config.DirectoryToBackup))
                throw new DirectoryNotFoundException(string.Format("Unable to backup folder '{0}' as it does not exist",
                    config.DirectoryToBackup));
            Path.CreateDirectory(config.BackupTo);
            var zipFile = Path.Get(config.BackupTo).Combine(BuildBackupFilename()).FullPath;

            using (var archive = new ZipFile())
            {
                archive.AddDirectory(config.DirectoryToBackup);
                RemoveFolderFromArchive(archive, config.BackupTo);
                    
                if (config.FoldersToIgnore != null)
                    config.FoldersToIgnore.ForEach(fti => RemoveFolderFromArchive(archive, fti));

                archive.Save(zipFile);
            }
        }

        protected virtual bool RemoveFolderFromArchive(ZipFile zip, string folder)
        {
            var folderToRemove = Path.Get(folder).FileName + "/";
            var entries = zip.Entries.Where(entry => entry.FileName.StartsWith(folderToRemove));
            entries.ToList().ForEach(entry =>
                                         {
                                             zip.RemoveEntry(entry);
                                             Console.WriteLine("\t\tRemoved entry '{0}'", entry.FileName);
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

        public void Dispose()
        {
            
        }
    }
}