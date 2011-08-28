using System.Collections.Generic;

namespace Sidewinder.Core.Interfaces.Entities
{
    public class BackupConfig
    {
        public string DirectoryToBackup { get; set; }
        public string BackupTo { get; set; }
        public List<string> FoldersToIgnore { get; set; }
    }
}