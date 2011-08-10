using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;
using Sidewinder.Pipeline;

namespace Sidewinder.Updater
{
    /// <summary>
    /// This contains all the code to self update this application via NuGet
    /// </summary>
    public class EmbeddedUpdateAgent : IUpdateAgent
    {
        protected UpdateConfig myConfig;
        protected Pipeline<UpdaterContext> myPipeline;

        public EmbeddedUpdateAgent(UpdateConfig config)
        {
            myConfig = config;
            myPipeline = Pipeline<UpdaterContext>.Run(new GetNuGetPackage())
                .Then(new DownloadPackageFiles())
                .Then(new BackupApplication())
                .Then(new WriteUpdateCommandFile());
        }

        public bool Execute()
        {
            return myPipeline.Execute(new UpdaterContext
                                   {
                                       Config = myConfig
                                   });
        }

        protected virtual bool DistributeFilesAfterDownload(SidewinderCommands commands)
        {
            // my go?
            if (commands.DistributeFiles == null)
                return false;

            // yup - time for action...
            // 1. wait until the launching app has terminated
            if (!WaitForParentProcessToTerminate(commands.DistributeFiles))
            {
                return false;
            }

            // 2. copy files 
            string targetFolder = Path.GetDirectoryName(commands.DistributeFiles.TargetProcessFilename);
            return true;
        }

        private bool WaitForParentProcessToTerminate(DistributeFiles command)
        {
            Process process =
                Process.GetProcesses().Where(p => string.Compare(p.MainModule.FileName, command.TargetProcessFilename) == 0)
                    .FirstOrDefault();

            var timer = new Stopwatch();
            while (process != null)
            {
                Console.WriteLine("\tWaiting for parent process to terminate...");

                if (timer.Elapsed.Seconds > command.SecondsToWait)
                {
                    // timeout waiting for launching process to terminate
                    Console.WriteLine("\tParent process still running after {0}s...aborting", command.SecondsToWait);
                    return false;
                }

                process =
                    Process.GetProcesses().Where(p => string.Compare(p.MainModule.FileName, command.TargetProcessFilename) == 0)
                        .FirstOrDefault();
                if (process != null)
                    continue;

                Console.WriteLine("\tParent process still running...trying again...");
                Thread.Sleep(1000);
            }

            // process has terminated...yay!
            return true;
        }
    }
}