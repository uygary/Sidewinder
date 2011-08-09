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
            Console.WriteLine("\tWriting command file...");
            var commandFile = Fluent.IO.Path.Get(context.Config.DownloadFolder).Combine(Constants.SidewinderCommandFile).FullPath;
            var command = new SidewinderCommands
            {
                DistributeFiles = new DistributeFiles
                {
                    TargetProcessFilename = Process.GetCurrentProcess().MainModule.FileName
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