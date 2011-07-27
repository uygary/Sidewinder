using System;
using Sidewinder.Interfaces;

namespace Sidewinder
{
    /// <summary>
    /// This contains all the code to self update this application via NuGet
    /// </summary>
    public class UpdaterFactory
    {
        public static IUpdateAgent Try(Action<UpdateConfigBuilder> setup)
        {
            var updater = new DefaultUpdateAgent();

            var builder = new UpdateConfigBuilder();
            setup(builder);
            var config = builder.Build();

            updater.Initialise(config);
            return updater;
        }
    }
}