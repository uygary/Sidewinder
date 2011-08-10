
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
            Console.WriteLine("\t\tPackage.DownloadFolder: {0}", myConfig.Package.DownloadFolder);
            Console.WriteLine("\t\tPackage.FrameworkHint: {0}", myConfig.Package.FrameworkHint);
            Console.WriteLine("\t\tPackage.SecondsToWait: {0}", myConfig.Package.SecondsToWait);
            Console.WriteLine("\t\tPackage.TargetProcessFilename: {0}", myConfig.Package.TargetProcessFilename);
            return myPipeline.Execute(new DistributorContext
            {
                Config = myConfig
            });
        }

    }
}