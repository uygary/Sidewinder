using System;
using System.IO;
using Sidewinder.Core;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder
{
    internal class Startup
    {
        private static class ExitCode
        {
            public const int CommandExecuteSuccess = 0;            
            public const int UpdateAvailable = 0;
            public const int NoUpdateAvailable = -1;
            public const int NoCommandFile = -2;
            public const int CommandExecuteFailure = -3;
            public const int Fatal = -99;
        }

        private static void Main(string[] args)
        {
            // default logger
            Logger.Initialise(new ConsoleLogger(Level.Debug));

            var noWaitPrompt = false;

            try
            {
                var retCode = ExitCode.NoUpdateAvailable;                

                if (args.Length > 0)
                {
                    // https://github.com/littlebits/args
                    var command = Args.Configuration.Configure<CmdlineArgs>().CreateAndBind(args);
                    noWaitPrompt = command.NoWaitPrompt;

                    retCode = AppUpdateFactory.Setup(config 
                        =>
                            {
                                var feed = command.Feed;
                                if (string.IsNullOrWhiteSpace(feed))
                                    feed = Constants.NuGet.OfficialFeedUrl;

                                config.Update(new TargetPackage
                                                    {
                                                        Force = command.Force,
                                                        Name = command.Package,
                                                        NuGetFeedUrl = feed,
                                                        UpdateDependencies = command.Dependencies
                                                    })
                                    .InstallInto(command.InstallFolder)
                                    .JustThesePackages()
                                    .SetLoggingLevel(command.LogLevel);

                                if (command.SkipOfficialFeed)
                                    config.SkipOfficialFeed();
                                if (command.Overwrite)
                                    config.OverwriteContentFiles();
                                else if (command.Manual)
                                    config.UserWillManuallyResolveContentConflicts();
                                else
                                    config.AskUserToResolveContentConflicts();

                                // if a hint is supplied then use it otherwise
                                // the default will be net40
                                if (command.Net11)
                                    config.TargetFrameworkVersion11();
                                if (command.Net20)
                                    config.TargetFrameworkVersion20();
                                if (command.Net40)
                                    config.TargetFrameworkVersion40();
                                if (command.Net45)
                                    config.TargetFrameworkVersion45();

                                if (command.NoWaitPrompt)
                                    config.NoWaitPrompt();
                            })
                            .Execute()
                            ? ExitCode.UpdateAvailable
                            : ExitCode.NoUpdateAvailable;
                }
                else
                {
                    SidewinderCommands commands;

                    if (!GetCommandFile(out commands))
                    {
                        Logger.Warn("**WARNING** No command file detected - aborting!");
                        retCode = ExitCode.NoCommandFile;
                    }
                    else
                    {
                        noWaitPrompt = commands.NoWaitPrompt;

                        if (!string.IsNullOrWhiteSpace(commands.LogPath))
                        {
                            Logger.Initialise(new FileLogger(commands.LogLevel, commands.LogPath));
                        }
                        else if (commands.LogLevel != Logger.Level)
                        {
                            // reinitialise if logging level different
                            Logger.Initialise(new ConsoleLogger(commands.LogLevel));
                        }

                        if (commands.DistributeFiles != null)
                        {
                            Logger.Info("\tDetected DistributeFiles command...executing...");
                            retCode = DistributorFactory.Setup(config => config.CommandIs(commands.DistributeFiles))
                                          .Execute()
                                          ? ExitCode.CommandExecuteSuccess
                                          : ExitCode.CommandExecuteFailure;
                        }
                        // FUTURE - add other supported commands here
                    }
                }

                Environment.ExitCode = retCode;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                Environment.ExitCode = ExitCode.Fatal;
            }

            if (noWaitPrompt == false && (Environment.ExitCode != ExitCode.CommandExecuteSuccess))
            {
                Console.WriteLine("Press a key to continue...");
                Console.ReadKey();
            }
        }

        protected static bool GetCommandFile(out SidewinderCommands commands)
        {
            commands = null;

            Logger.Debug("Sidewinder is looking for command file...");
            var commandFile = Fluent.IO.Path.Get(SmartLocation.GetBinFolder(), Constants.Sidewinder.CommandFile).FullPath;

            for (int i = 0; i < 4; i++)
            {
                if (LoadCommandFile(commandFile, out commands))
                    return true;

                commandFile = Fluent.IO.Path.Get(commandFile).Up(2).Combine(Constants.Sidewinder.CommandFile).FullPath;
            }

            return false;
        }

        protected static bool LoadCommandFile(string commandFile, out SidewinderCommands commands)
        {
            commands = null;

            Logger.Debug("\tLooking here...{0}...", commandFile);

            if (!File.Exists(commandFile))
                return false;

            Logger.Info("\t\tFound command file at {0}", commandFile);
            commands = SerialisationHelper<SidewinderCommands>.DataContractDeserializeFromFile(commandFile);
            return true;
        }
    }
}