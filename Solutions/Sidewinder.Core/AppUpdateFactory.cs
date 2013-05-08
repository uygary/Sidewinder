
using System;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Updater;

namespace Sidewinder.Core
{
    /// <summary>
    /// This will build and configure a pipeline to download a set of nuget packages
    /// </summary>
    public class AppUpdateFactory
    {
        public static IUpdateAgent Setup(Action<UpdateConfigBuilder> setup)
        {
            var builder = new UpdateConfigBuilder();
            setup(builder);
            var config = builder.Build();

            var logger = config.Logger ?? new ConsoleLogger(config.LoggingLevel);
            Logger.Initialise(logger).Debug("Logger is {0}", logger.GetType().Name);

            var agent = new DefaultUpdateAgent(config);
            return agent;
        }
    }
}