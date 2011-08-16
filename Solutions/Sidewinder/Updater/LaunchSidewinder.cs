using System;
using System.Diagnostics;
using System.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Updater
{
    public class LaunchSidewinder : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            
        }

        public bool Execute(UpdaterContext context)
        {
            var sidewinder = context.Config.TargetPackages[Constants.Sidewinder.NuGetPackageName];

            var sidewinderPath = Fluent.IO.Path.Get(context.Config.DownloadFolder, 
                "lib", 
                sidewinder.FrameworkHint, 
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