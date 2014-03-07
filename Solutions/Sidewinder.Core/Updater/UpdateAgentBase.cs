using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;
using Sidewinder.Core.Pipeline;

namespace Sidewinder.Core.Updater
{
    /// <summary>
    /// This contains all the code to self update this application via NuGet
    /// </summary>
    public abstract class UpdateAgentBase : IUpdateAgent
    {
        protected UpdateConfig _config;
        protected Pipeline<UpdaterContext> _pipeline;

        protected UpdateAgentBase(UpdateConfig config, Pipeline<UpdaterContext> pipeline)
        {
            _config = config;
            _pipeline = pipeline;
        }

        public virtual bool Execute()
        {
            Logger.Debug("Running Update Pipeline...");
            Logger.Debug("\tConfig.Backup: {0}", _config.Backup);
            Logger.Debug("\tConfig.BackupFolder: {0}", _config.BackupFolder);
            Logger.Debug("\tConfig.ConflictResolution: {0}", _config.ConflictResolution);
            Logger.Debug("\tConfig.DownloadFolder: {0}", _config.DownloadFolder);
            Logger.Debug("\tConfig.InstallFolder: {0}", _config.InstallFolder);
            Logger.Debug("\tConfig.TargetFrameworkVersion: {0}", _config.TargetFrameworkVersion);
            Logger.Debug("\tConfig.NoWaitPrompt: {0}", _config.NoWaitPrompt);

            return _pipeline.Execute(new UpdaterContext
                                   {
                                       Config = _config
                                   });
        }
    }
}