using System.Diagnostics;
using Fluent.IO;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Updater
{
    public class WriteUpdateCommandFile : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
        }

        public bool Execute(UpdaterContext context)
        {
            var commandFile = Path.Get(context.Config.DownloadFolder).Combine(Constants.Sidewinder.CommandFile).FullPath;

            Logger.Debug("\tWriting command file to {0}", commandFile);
            var command = new SidewinderCommands
            {
                LogLevel = Logger.Level,
                DistributeFiles = new DistributeFiles
                {
                    ConflictResolution = context.Config.ConflictResolution,
                    InstallFolder = context.Config.InstallFolder,
                    TargetFrameworkVersion = context.Config.TargetFrameworkVersion,
                    TargetProcessFilename = Process.GetCurrentProcess().MainModule.FileName,
                    DownloadFolder = context.Config.DownloadFolder,
                    Updates = context.Updates,
                    LaunchProcess = context.Config.LaunchProcess,
                    LaunchProcessArguments = context.Config.LaunchProcessArguments
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