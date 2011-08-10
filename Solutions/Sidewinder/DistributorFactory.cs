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
        public static IDistributionAgent Setup(Action<DistributorConfigBuilder> setup)
        {
            var builder = new DistributorConfigBuilder();
            setup(builder);
            var config = builder.Build();

            var agent = new DefaultDistributionAgent(config);
            return agent;
        }
    }
}