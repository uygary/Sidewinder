using System;
using Sidewinder.Core.Interfaces;

namespace Sidewinder.Core
{
    public class ConsoleLogger : ILogger
    {
        private readonly Level _level;

        public ConsoleLogger(Level level)
        {
            _level = level;
        }

        public ConsoleLogger(string level)
        {
            Level enumLevel;

            if (!Enum.TryParse(level, true,  out enumLevel))
                enumLevel = Level.Debug;
            _level = enumLevel;
        }

        private ILogger Write(Level level, string format, params object[] args)
        {
            if (_level <= level)
                Console.WriteLine(format, args);
            return this;
        }

        public Level Level
        {
            get { return _level; }
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