
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
                                                              Name = Constants.Sidewinder.NuGetPackageName
                                                          });
                }

                return true;
            }

            // sidewinder package exists - grab the file version number
            var sidewinderPath = Path.Get(context.Config.DownloadFolder, Constants.Sidewinder.UpdateFolder,
                         Constants.Sidewinder.ExeFilename).FullPath;
            var ver = FileVersionInfo.GetVersionInfo(sidewinderPath);

            throw new NotImplementedException("Add existing sidewinder install to target packages");

            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}