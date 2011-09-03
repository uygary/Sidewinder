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
                return true;

            context.InstalledPackages.ToList().ForEach(ip =>
                                                           {
                                                               if (context.Config.TargetPackages.ContainsKey(ip.Key))
                                                                   return;

                                                               context.Config.TargetPackages.Add(ip.Value);
                                                           });
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }
    }
}