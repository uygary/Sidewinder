using Sidewinder.Interfaces.Entities;

namespace Sidewinder
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
                                 // finalise config from myConfig
                             };
            return config;
        }
    }
}