using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Distributor
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
            var attempt = 1;
            var timer = new Stopwatch();
            timer.Start();

            Console.WriteLine("\tWaiting for parent process to terminate...");

            do
            {
                try
                {
                    var running = Process.GetProcesses().ToList();

                    if (running.Where(p =>
                        (string.Compare(p.ProcessName, "System") != 0) &&
                        (string.Compare(p.ProcessName, "Idle") != 0) &&
                        (string.Compare(p.MainModule.FileName, context.Config.Command.TargetProcessFilename) == 0))
                            .FirstOrDefault() == null)
                        return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\t** Failed to get process list ** ({0}), attempt:={1}", 
                        ex.Message, attempt);
                }
                                
                if (timer.Elapsed.Seconds > context.Config.Command.SecondsToWait)
                {
                    // timeout waiting for launching process to terminate
                    Console.WriteLine("\tParent process still running after {0}s...aborting", context.Config.Command.SecondsToWait);
                    return false;
                }
                
                Console.WriteLine(string.Format("\tParent process still running...trying again (attempt:={0}...", attempt));
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