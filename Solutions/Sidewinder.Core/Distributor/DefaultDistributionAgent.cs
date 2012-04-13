using System;
using System.Linq;
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
                .Then(new CopyContentFiles())
                .Then(new CopyOtherPackageFiles())
                .Then(new UpdateInstalledPackages())
                .Then(new LaunchReadme())
                .Then(new CleanUp());
        }

        public bool Execute()
        {
            Console.WriteLine("Running Distribution Pipeline...");
            Console.WriteLine("\tCommand.ConflictResolution: {0}", myConfig.Command.ConflictResolution);
            Console.WriteLine("\tCommand.InstallFolder: {0}", myConfig.Command.InstallFolder);
            Console.WriteLine("\tCommand.DownloadFolder: {0}", myConfig.Command.DownloadFolder);
            Console.WriteLine("\tCommand.SecondsToWait: {0}", myConfig.Command.SecondsToWait);
            Console.WriteLine("\tCommand.TargetProcessFilename: {0}", myConfig.Command.TargetProcessFilename);
            Console.WriteLine("\tCommand.Updates...");
            myConfig.Command.Updates.ToList().ForEach(update => Console.WriteLine("\t\t{0} -> v{1}", update.Target.Name,
                                                                                  update.NewVersion));

            return myPipeline.Execute(new DistributorContext
            {
                Config = myConfig
            });
        }
    }
}