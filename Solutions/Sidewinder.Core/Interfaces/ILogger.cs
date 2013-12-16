namespace Sidewinder.Core.Interfaces
{
    public interface ILogger
    {
        Level Level { get; }
        ILogger Debug(string format, params object[] args);
        ILogger Info(string format, params object[] args);
        ILogger Warn(string format, params object[] args);
        ILogger Error(string format, params object[] args);
    }

    public enum Level
    {
        Debug = 0,
        Info = 5,
        Warn = 10,
        Error = 15
    }
}