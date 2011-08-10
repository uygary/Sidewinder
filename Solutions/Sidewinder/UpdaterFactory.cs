using System;
using Sidewinder.Interfaces;
using Sidewinder.Updater;

namespace Sidewinder
{
    /// <summary>
    /// This contains all the code to self update this application via NuGet
    /// </summary>
    public class UpdaterFactory
    {
        public static IUpdateAgent Setup(Action<UpdateConfigBuilder> setup)
        {
            var builder = new UpdateConfigBuilder();
            setup(builder);
            var config = builder.Build();

            var agent = new EmbeddedUpdateAgent(config);
            return agent;
        }
    }
}