using System.Diagnostics;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Distributor
{
    /// <summary>
    /// This will lanuch the process that is configured to start up after update completes.
    /// </summary>
    public class LaunchProcess : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            
        }

        public bool Execute(DistributorContext context)
        {
            if (string.IsNullOrWhiteSpace(context.Config.Command.LaunchProcess))
                return true;

            using (Process.Start(new ProcessStartInfo
            {
                FileName = context.Config.Command.LaunchProcess,
                UseShellExecute = true
            })) { }

            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
        }
    }
}