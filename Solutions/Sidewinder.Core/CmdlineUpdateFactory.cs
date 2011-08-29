
using System;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Updater;

namespace Sidewinder.Core
{
    /// <summary>
    /// This contains all the code to self update this application via NuGet
    /// </summary>
    public class CmdlineUpdateFactory
    {
        public static IUpdateAgent Setup(Action<UpdateConfigBuilder> setup)
        {
            var builder = new UpdateConfigBuilder();
            setup(builder);
            var config = builder.Build();

            var agent = new CmdlineUpdateAgent(config);
            return agent;
        }
    }
}