using System;
using System.Diagnostics;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Updater
{
    public class WriteUpdateCommandFile : IPipelineStep<UpdaterContext>
    {

        public void EntryConditions(UpdaterContext context)
        {
            
        }

        public bool Execute(UpdaterContext context)
        {            
            var commandFile = Fluent.IO.Path.Get(context.Config.DownloadFolder).Combine(Constants.SidewinderCommandFile).FullPath;

            Console.WriteLine("\tWriting command file to {0}", commandFile);
            var command = new SidewinderCommands
            {
                DistributeFiles = new DistributeFiles
                {
                    TargetProcessFilename = Process.GetCurrentProcess().MainModule.FileName,
                    DownloadFolder = context.Config.DownloadFolder,
                    FrameworkHint = context.Config.FrameworkHint
                }
            };
            SerialisationHelper<SidewinderCommands>.DataContractSerialize(commandFile, command);
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
        }
    }
}