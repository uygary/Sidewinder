using System.Collections.Generic;

namespace Sidewinder.Interfaces.Entities
{
    public class BackupConfig
    {
        public string DirectoryToBackup { get; set; }
        public string BackupTo { get; set; }
        public List<string> FoldersToIgnore { get; set; }
    }
}