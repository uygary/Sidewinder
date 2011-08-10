
using System;
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

        public DefaultDistributionAgent(DistributorConfig config)
        {
            myConfig = config;
            myPipeline = Pipeline<DistributorContext>.Run(new GetPackageBinariesLocation())
                .Then(new WaitForProcessShutdown())
                .Then(new CopyPackageFiles());
        }

        public bool Execute()
        {
            Console.WriteLine("\tRunning Distribution Pipeline...");
            Console.WriteLine("\t\tInstallFolder: {0}", myConfig.InstallFolder);
            Console.WriteLine("\t\tCommand.DownloadFolder: {0}", myConfig.Command.DownloadFolder);
            Console.WriteLine("\t\tCommand.FrameworkHint: {0}", myConfig.Command.FrameworkHint);
            Console.WriteLine("\t\tCommand.SecondsToWait: {0}", myConfig.Command.SecondsToWait);
            Console.WriteLine("\t\tCommand.TargetProcessFilename: {0}", myConfig.Command.TargetProcessFilename);
            return myPipeline.Execute(new DistributorContext
            {
                Config = myConfig
            });
        }

    }
}