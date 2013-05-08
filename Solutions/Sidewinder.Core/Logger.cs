using Sidewinder.Core.Interfaces;

namespace Sidewinder.Core
{
    public static class Logger
    {
        private static ILogger _instance;

        public static ILogger Initialise(ILogger logger)
        {
            _instance = logger;
            return _instance;
        }

        public static Level Level
        {
            get { return _instance.Level; }
        }

        public static ILogger Debug(string format, params object[] args)
        {
            return _instance.Debug(format, args);
        }

        public static ILogger Info(string format, params object[] args)
        {
            return _instance.Info(format, args);
        }

        public static ILogger Warn(string format, params object[] args)
        {
            return _instance.Warn(format, args);
        }

        public static ILogger Error(string format, params object[] args)
        {
            return _instance.Error(format, args);
        }
    }
}