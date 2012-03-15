using System;
using System.Linq;
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
        protected UpdateConfig myConfig;
        protected Pipeline<UpdaterContext> myPipeline;

        protected UpdateAgentBase(UpdateConfig config, Pipeline<UpdaterContext> pipeline)
        {
            myConfig = config;
            myPipeline = pipeline;
        }

        public virtual bool Execute()
        {
            Console.WriteLine("Running Update Pipeline...");
            Console.WriteLine("\tConfig.Backup: {0}", myConfig.Backup);
            Console.WriteLine("\tConfig.BackupFolder: {0}", myConfig.BackupFolder);
            Console.WriteLine("\tConfig.ConflictResolution: {0}", myConfig.ConflictResolution);
            Console.WriteLine("\tConfig.DownloadFolder: {0}", myConfig.DownloadFolder);
            Console.WriteLine("\tConfig.InstallFolder: {0}", myConfig.InstallFolder);
            Console.WriteLine("\tConfig.TargetFrameworkVersion: {0}", myConfig.TargetFrameworkVersion);

            return myPipeline.Execute(new UpdaterContext
                                   {
                                       Config = myConfig
                                   });
        }
    }
}