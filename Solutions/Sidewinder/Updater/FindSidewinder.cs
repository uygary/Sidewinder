
using System;
using System.Diagnostics;
using Fluent.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Updater
{
    /// <summary>
    /// This will attempt to find the sidewinder instance that will complete
    /// the installation. Sidewinder is downloaded as part of any update run
    /// but only if it does not already exist of a newer version is available.
    /// </summary>
    public class FindSidewinder : IPipelineStep<UpdaterContext>
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
                    context.Config.TargetPackages.Add(Constants.Sidewinder.NuGetPackageName,
                                                      new TargetPackage
                                                          {
                                                              FrameworkHint = "net40",
                                                              Name = Constants.Sidewinder.NuGetPackageName,

                                                              // Testing only
                                                              //NuGetFeedUrl = "http://www.myget.org/F/sidewinder"
                                                          });
                    Console.WriteLine("\tAdded Sidewinder to targets");
                }

                return true;
            }

            // sidewinder package exists - grab the file version number
            Version ver = null;
            var sidewinderPath = Path.Get(context.Config.DownloadFolder, Constants.Sidewinder.NuGetPackageName,
                         Constants.Sidewinder.ExeFilename).FullPath;
            if (Path.Get(sidewinderPath).Exists)
            {
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(sidewinderPath);
                ver = new Version(fileVersionInfo.FileVersion);
            }

            if (context.Config.TargetPackages.ContainsKey(Constants.Sidewinder.NuGetPackageName))
                context.Config.TargetPackages.Remove(Constants.Sidewinder.NuGetPackageName);
            context.Config.TargetPackages.Add(Constants.Sidewinder.NuGetPackageName,
                                                                  new TargetPackage
                                                                  {
                                                                      FrameworkHint = "net40",
                                                                      Name = Constants.Sidewinder.NuGetPackageName,
                                                                      Version = ver,

                                                                      // Testing only
                                                                      //NuGetFeedUrl = "http://www.myget.org/F/sidewinder"
                                                                  });
            Console.WriteLine("\tAdded Sidewinder to targets (update)");
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}