using System;
using System.Diagnostics;
using Fluent.IO;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Distributor
{
    /// <summary>
    /// This will search for any readme.txt files in the root of the downloaded packages
    /// and if any exist they are ShellExecuted so that they open their registered 
    /// application.
    /// </summary>
    public class LaunchReadme : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            
        }

        public bool Execute(DistributorContext context)
        {
            context.Config.Command.Updates.ForEach(
                update =>
                    {
                        var target = Path.Get(context.Config.Command.DownloadFolder, 
                            update.Target.Name, "readme.txt");

                        if (!target.Exists)
                        {
                            return;
                        }

                        var launch = Process.Start(new ProcessStartInfo
                                                       {
                                                           FileName = target.FullPath,
                                                           UseShellExecute = true
                                                       });
                        launch.Dispose();
                    });
            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
        }
    }
}