using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Fluent.IO;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Distributor
{
    /// <summary>
    /// This step waits for the process that launched this process to terminate
    /// </summary>
    public class WaitForProcessShutdown : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            
        }

        public bool Execute(DistributorContext context)
        {
            if (string.Compare(Path.Get(context.Config.Command.TargetProcessFilename).Parent().FullPath.TrimEnd('\\'),
                context.Config.Command.InstallFolder.TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase) != 0)
                return true;

            var attempt = 1;
            var timer = new Stopwatch();
            timer.Start();

            Logger.Warn("\tWaiting for parent process to terminate...");

            do
            {
                try
                {
                    var running = Process.GetProcesses().ToList();

                    if (running.FirstOrDefault(p => (String.CompareOrdinal(p.ProcessName, "System") != 0) &&
                                                    (String.CompareOrdinal(p.ProcessName, "Idle") != 0) &&
                                                    (String.CompareOrdinal(p.MainModule.FileName, context.Config.Command.TargetProcessFilename) == 0)) == null)
                        return true;
                }
                catch (Exception ex)
                {
                    Logger.Warn("\t** Failed to get process list ** ({0}), attempt:={1}", 
                        ex.Message, attempt);
                }
                                
                if (timer.Elapsed.Seconds > context.Config.Command.SecondsToWait)
                {
                    // timeout waiting for launching process to terminate
                    Logger.Warn("\tParent process still running after {0}s...aborting", context.Config.Command.SecondsToWait);
                    return false;
                }

                Logger.Warn(string.Format("\tParent process still running...trying again (attempt:={0}...", attempt));
                Thread.Sleep(1000);
                attempt++;
            }
            while (true);
        }

        public void ExitConditions(DistributorContext context)
        {
            
        }
    }
}