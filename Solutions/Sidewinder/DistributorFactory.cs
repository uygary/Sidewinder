using System;
using Sidewinder.Distributor;
using Sidewinder.Interfaces;

namespace Sidewinder
{
    /// <summary>
    /// This provides the mechanism to create a distribution agent
    /// </summary>
    public class DistributorFactory
    {
        public static IDistributionAgent Try(Action<DistributorConfigBuilder> setup)
        {
            var agent = new DefaultDistributionAgent();

            var builder = new DistributorConfigBuilder();
            setup(builder);
            var config = builder.Build();

            agent.Initialise(config);
            return agent;
        }
    }
}