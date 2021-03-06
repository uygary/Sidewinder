﻿using System.Diagnostics;
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
            var sidewinderPath = NuGetHelper.GetLibFrameworkPath(context.Config.DownloadFolder,
                Constants.Sidewinder.NuGetPackageName,
                context.Config.TargetFrameworkVersion)
                .Combine(Constants.Sidewinder.ExeFilename).FullPath;
            Logger.Info("\tLaunching Sidewinder @{0}...", sidewinderPath);

            if (!File.Exists(sidewinderPath))
            {
                Logger.Error("\t\tSidewinder.exe not found...terminating update process :(");
                return false;
            }

            var processStartInfo = new ProcessStartInfo
                {
                    FileName = sidewinderPath
                };

            Process.Start(processStartInfo);
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}