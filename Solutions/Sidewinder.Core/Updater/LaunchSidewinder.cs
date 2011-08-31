using System;
using System.Diagnostics;
using System.IO;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Updater
{
    public class LaunchSidewinder : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            
        }

        public bool Execute(UpdaterContext context)
        {
            var sidewinderPath = Fluent.IO.Path.Get(context.Config.DownloadFolder,
                Constants.Sidewinder.NuGetPackageName,
                "lib",                
                Framework.GetBestLibFrameworkFolder(context.Config.TargetFrameworkVersion),
                Constants.Sidewinder.ExeFilename).FullPath;
            Console.WriteLine("\tLaunching Sidewinder @{0}...", sidewinderPath);

            if (!File.Exists(sidewinderPath))
            {
                Console.WriteLine("\t\tSidewinder does not exist...terminating update process :(");
                return false;
            }

            var app = Process.Start(new ProcessStartInfo
            {
                FileName = sidewinderPath
            });

            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}