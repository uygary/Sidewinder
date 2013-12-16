using System;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;
using System.Linq;

namespace Sidewinder.Core.Updater
{
    /// <summary>
    /// This will append the list of installed packages to the list of target packages to update.
    /// </summary>
    public class AddInstalledPackagesToUpdates : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            
        }

        public bool Execute(UpdaterContext context)
        {
            if (context.InstalledPackages == null || context.Config.JustThis)
            {
                Logger.Info("\tNo additional packages installed or to check");
                return true;
            }

            context.InstalledPackages.ToList().ForEach(ip =>
                                                           {
                                                               if (context.Config.TargetPackages.ContainsKey(ip.Key))
                                                                   return;

                                                               context.Config.TargetPackages.Add(ip.Value);
                                                               Logger.Debug("\tAdded '{0}' to Targets", ip.Value.Name);
                                                           });
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}