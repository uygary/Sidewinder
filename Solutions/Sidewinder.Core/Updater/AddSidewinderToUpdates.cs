using System;
using System.Diagnostics;
using Fluent.IO;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Updater
{
    /// <summary>
    /// Sidewinder is downloaded as part of any update run
    /// but only if it does not already exist or a newer version is available.
    /// </summary>
    public class AddSidewinderToUpdates : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            
        }

        public bool Execute(UpdaterContext context)
        {
            if (!Path.Get(context.Config.DownloadFolder, Constants.Sidewinder.NuGetPackageName).Exists)
            {
                // sidewinder package not downloaded
                if (!context.Config.TargetPackages.ContainsKey(Constants.Sidewinder.NuGetPackageName))
                {
                    // get the latest sidewinder from the official nuget feed
                    context.Config.TargetPackages.Add(new TargetPackage
                                                          {
                                                              Name = Constants.Sidewinder.NuGetPackageName,
                                                              UpdateDependencies = true,
                                                              NuGetFeedUrl = Constants.Sidewinder.OfficialFeedUrl
                                                          });
                    Console.WriteLine("\tAdded Sidewinder to targets");
                }

                return true;
            }

            // sidewinder package exists - grab the file version number
            Version ver = null;
            var sidewinderPath = Path.Get(context.Config.DownloadFolder, 
                Constants.Sidewinder.NuGetPackageName,
                Constants.NuGet.LibFolder,
                "net40",
                Constants.Sidewinder.ExeFilename).FullPath;

            // (only if this is not the running app)
            if (string.Compare(Path.Get(Process.GetCurrentProcess().MainModule.FileName).FullPath,
                sidewinderPath, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return true;
            }

            if (Path.Get(sidewinderPath).Exists)
            {
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(sidewinderPath);
                ver = new Version(fileVersionInfo.FileVersion);
            }

            if (context.Config.TargetPackages.ContainsKey(Constants.Sidewinder.NuGetPackageName))
                context.Config.TargetPackages.Remove(Constants.Sidewinder.NuGetPackageName);
            context.Config.TargetPackages.Add(new TargetPackage
                                                  {
                                                      Name = Constants.Sidewinder.NuGetPackageName,
                                                      Version = ver,
                                                      UpdateDependencies = true,
                                                      NuGetFeedUrl = Constants.Sidewinder.OfficialFeedUrl
                                                  });
            Console.WriteLine("\tAdded Sidewinder {0}to targets (update)", ver == null ? string.Empty : ver + " ");
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}