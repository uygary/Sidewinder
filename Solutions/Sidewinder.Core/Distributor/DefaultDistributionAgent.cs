using System;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;
using Sidewinder.Core.Pipeline;

namespace Sidewinder.Core.Distributor
{
    /// <summary>
    /// This contains the set of instructions to distribute any updates downloaded
    /// </summary>
    public class DefaultDistributionAgent : IDistributionAgent
    {
        protected DistributorConfig myConfig;
        protected Pipeline<DistributorContext> myPipeline;

        public DefaultDistributionAgent(DistributorConfig config)
        {
            myConfig = config;
            myPipeline = Pipeline<DistributorContext>.Run(new WaitForProcessShutdown())
                .Then(new CopyPackageFiles())
                .Then(new CleanUp());
        }

        public bool Execute()
        {
            Console.WriteLine("\tRunning Distribution Pipeline...");
            Console.WriteLine("\t\tInstallFolder: {0}", myConfig.InstallFolder);
            Console.WriteLine("\t\tCommand.DownloadFolder: {0}", myConfig.Command.DownloadFolder);
            Console.WriteLine("\t\tCommand.SecondsToWait: {0}", myConfig.Command.SecondsToWait);
            Console.WriteLine("\t\tCommand.TargetProcessFilename: {0}", myConfig.Command.TargetProcessFilename);
            return myPipeline.Execute(new DistributorContext
            {
                Config = myConfig
            });
        }
    }
}