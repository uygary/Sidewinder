using System;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Interfaces
{
    public interface IBackupAgent : IDisposable
    {
        void Backup(BackupConfig config);
    }
}