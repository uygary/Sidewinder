using Sidewinder.Core.Interfaces.Entities;
using Sidewinder.Core.Pipeline;

namespace Sidewinder.Core.Updater
{
    /// <summary>
    /// This contains the pipeline of instructions to update an application
    /// </summary>
    public class DefaultUpdateAgent : UpdateAgentBase
    {
        public DefaultUpdateAgent(UpdateConfig config)
            : base(config, Pipeline<UpdaterContext>.Run(new AddSidewinderToUpdates())
                .Then(new DiscoverInstalledPackages())
                .Then(new AddInstalledPackagesToUpdates())
                .Then(new GetNuGetPackages())                
                .Then(new BackupApplication())
                .Then(new WriteUpdateCommandFile())
                .Then(new LaunchSidewinder()))
        {
        }
    }
}