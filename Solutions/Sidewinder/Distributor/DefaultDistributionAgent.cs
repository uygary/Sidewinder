
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;
using Sidewinder.Pipeline;

namespace Sidewinder.Distributor
{
    /// <summary>
    /// This contains all the code to self update this application via NuGet
    /// </summary>
    public class DefaultDistributionAgent : IDistributionAgent
    {
        protected DistributorConfig myConfig;
        protected Pipeline<DistributorContext> myPipeline;

        public DefaultDistributionAgent()
        {
            myPipeline = Pipeline<DistributorContext>.Run(new GetPackageBinariesLocation())
                .Then(new WaitForProcessShutdown())
                .Then(new CopyPackageFiles());
        }

        public IDistributionAgent Initialise(DistributorConfig config)
        {
            myConfig = config;
            return this;
        }

        public bool Execute()
        {
            return myPipeline.Execute(new DistributorContext
            {
                Config = myConfig
            });
        }

    }
}