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
            var folder = Fluent.IO.Path.Get(context.Config.DownloadFolder, "lib");
            if (!string.IsNullOrWhiteSpace(context.Config.FrameworkHint))
                folder = Fluent.IO.Path.Get(folder.FullPath, context.Config.FrameworkHint);

            var updaterFilename = folder.Combine(Constants.SidewinderUpdater).FullPath;
            Console.WriteLine("\tLaunching Sidewinder @{0}...", updaterFilename);

            if (!File.Exists(updaterFilename))
            {
                Console.WriteLine("\t\tSidewinder does not exist...terminating update process :(");
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