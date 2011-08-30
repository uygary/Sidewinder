using Sidewinder.Core.Interfaces.Entities;
using Sidewinder.Core.Pipeline;

namespace Sidewinder.Core.Updater
{
    /// <summary>
    /// This contains all the code to self update this application via NuGet
    /// </summary>
    public class DefaultUpdateAgent : UpdateAgentBase
    {
        public DefaultUpdateAgent(UpdateConfig config)
            : base(config, Pipeline<UpdaterContext>.Run(new AddSidewinderToUpdates())
                .Then(new DiscoverInstalledPackages())
                .Then(new GetNuGetPackages())                
                .Then(new BackupApplication())
                .Then(new WriteUpdateCommandFile())
                .Then(new LaunchSidewinder()))
        {
        }
    }
}