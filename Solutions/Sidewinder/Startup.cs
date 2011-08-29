using System;
using System.IO;
using Sidewinder.Core;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder
{
    internal class Startup
    {
        private static void Main(string[] args)
        {
            try
            {
                var retCode = 0;
                SidewinderCommands commands;

                if (args.Length > 0)
                {
                    // https://github.com/littlebits/args
                    var command = Args.Configuration.Configure<CmdlineArgs>().CreateAndBind(args);

                    Console.WriteLine("Updating package {0}...", command.Package);
                    retCode = CmdlineUpdateFactory.Setup(config =>
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
                                                               .InstallInto(command.InstallFolder);

                                                           // if a hint is supplied then use it otherwise
                                                           // the default will be net40
                                                           if (command.Net11)
                                                               config.TargetFrameworkVersion11();
                                                           if (command.Net20)
                                                               config.TargetFrameworkVersion20();
                                                           if (command.Net40)
                                                               config.TargetFrameworkVersion40();
                                                       })
                                  .Execute()
                                  ? 0
                                  : -1;
                }
                else if (!GetCommandFile(out commands))
                {
                    Console.WriteLine("**WARNING** No command file detected - aborting!");
                }
                else
                {

                    if (commands.DistributeFiles != null)
                    {
                        Console.WriteLine("\tDetected DistributeFiles command...executing...");
                        retCode = DistributorFactory.Setup(config => config.CommandIs(commands.DistributeFiles))
                                      .Execute()
                                      ? 0
                                      : -1;

                        Console.WriteLine("Press a key to continue...");
                        Console.ReadKey();
                    }
                }

                Environment.ExitCode = retCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        protected static bool GetCommandFile(out SidewinderCommands commands)
        {
            commands = null;

            Console.WriteLine("Sidewinder is looking for command file...");
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

            Console.WriteLine("\tLooking here...{0}...", commandFile);

            if (!File.Exists(commandFile))
                return false;

            Console.WriteLine("\t\tFound at {0}", commandFile);
            commands = SerialisationHelper<SidewinderCommands>.DataContractDeserializeFromFile(commandFile);
            return true;
        }
    }
}