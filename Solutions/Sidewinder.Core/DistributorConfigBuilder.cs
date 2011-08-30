using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core
{
    public class DistributorConfigBuilder
    {
        private readonly DistributorConfig myConfig;

        public DistributorConfigBuilder()
        {
            myConfig = new DistributorConfig();
        }

        public DistributorConfig Build()
        {
            var config = new DistributorConfig
                             {
                                 Command = myConfig.Command
                             };
            return config;
        }

        public DistributorConfigBuilder CommandIs(DistributeFiles command)
        {
            myConfig.Command = command;
            return this;
        }
    }
}