using System;
using System.Diagnostics;
using System.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Updater
{
    public class LaunchUpdater : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            
        }

        public bool Execute(UpdaterContext context)
        {
            var updaterFilename = Fluent.IO.Path.Get(context.Config.DownloadFolder).Combine(Constants.SidewinderUpdater).FullPath;
            Console.WriteLine("\tLaunching updated application @{0}...", updaterFilename);

            if (!File.Exists(updaterFilename))
            {
                Console.WriteLine("\t\tApp update does not exist...terminating update process :(");
                return false;
            }

            var app = Process.Start(new ProcessStartInfo
            {
                FileName = updaterFilename
            });

            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}