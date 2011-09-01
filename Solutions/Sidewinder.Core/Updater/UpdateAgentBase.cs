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
            Console.WriteLine("\tConfig.DownloadFolder: {0}", myConfig.DownloadFolder);
            Console.WriteLine("\tConfig.InstallFolder: {0}", myConfig.InstallFolder);
            Console.WriteLine("\tConfig.TargetFrameworkVersion: {0}", myConfig.TargetFrameworkVersion);
            Console.WriteLine("\tConfig.TargetPackages...");
            myConfig.TargetPackages.ToList().ForEach(tp =>
                                                         {
                                                             Console.Write("\t\t{0}", tp.Value.Name);

                                                             if (tp.Value.Version != null)
                                                                 Console.Write(" v{0}", tp.Value.Version);

                                                             if (string.Compare(Constants.NuGet.OfficialFeedUrl,
                                                                                tp.Value.NuGetFeedUrl,
                                                                                StringComparison.InvariantCultureIgnoreCase) != 0)
                                                                 Console.Write(" ({0})", tp.Value.NuGetFeedUrl);

                                                             Console.WriteLine();
                                                         });
            return myPipeline.Execute(new UpdaterContext
                                   {
                                       Config = myConfig
                                   });
        }
    }
}