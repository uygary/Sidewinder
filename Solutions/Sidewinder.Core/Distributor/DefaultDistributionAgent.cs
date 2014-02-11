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
        protected DistributorConfig _config;
        protected Pipeline<DistributorContext> _pipeline;

        public DefaultDistributionAgent(DistributorConfig config)
        {
            _config = config;
            _pipeline = Pipeline<DistributorContext>.Run(new WaitForProcessShutdown())
                .Then(new CopyContentFiles())
                .Then(new CopyOtherPackageFiles())
                .Then(new UpdateInstalledPackages())
                .Then(new LaunchReadme())
                .Then(new LaunchProcess())
                .Then(new CleanUp());
        }

        public bool Execute()
        {
            Logger.Debug("Running Distribution Pipeline...");
            Logger.Debug("\tCommand.ConflictResolution: {0}", _config.Command.ConflictResolution);
            Logger.Debug("\tCommand.InstallFolder: {0}", _config.Command.InstallFolder);
            Logger.Debug("\tCommand.DownloadFolder: {0}", _config.Command.DownloadFolder);
            Logger.Debug("\tCommand.SecondsToWait: {0}", _config.Command.SecondsToWait);
            Logger.Debug("\tCommand.TargetProcessFilename: {0}", _config.Command.TargetProcessFilename);
            Logger.Debug("\tCommand.TargetProcessId: {0}", _config.Command.TargetProcessId.GetValueOrDefault());
            Logger.Debug("\tCommand.LaunchProcess: {0}", _config.Command.LaunchProcess);
            Logger.Debug("\tCommand.Updates...");
            _config.Command.Updates.ToList().ForEach(update => Logger.Debug("\t\t{0} -> v{1}", update.Target.Name,
                update.NewVersion));

            return _pipeline.Execute(new DistributorContext
            {
                Config = _config
            });
        }
    }
}