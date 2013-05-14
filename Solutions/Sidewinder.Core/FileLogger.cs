using System.IO;
using Sidewinder.Core.Interfaces;

namespace Sidewinder.Core
{
    public class FileLogger : ILogger
    {
        private readonly string _path;

        public FileLogger(Level level, string path)
        {
            _path = path;
            Level = level;
        }

        public Level Level { get; private set; }

        private ILogger Write(Level level, string format, params object[] args)
        {
            if (Level <= level)
                File.AppendAllLines(_path, new[] {string.Format(format, args)});
            return this;
        }

        public ILogger Debug(string format, params object[] args)
        {
            return Write(Level.Debug, format, args);
        }

        public ILogger Info(string format, params object[] args)
        {
            return Write(Level.Info, format, args);
        }

        public ILogger Warn(string format, params object[] args)
        {
            return Write(Level.Warn, format, args);
        }

        public ILogger Error(string format, params object[] args)
        {
            return Write(Level.Error, format, args);
        }
    }
}