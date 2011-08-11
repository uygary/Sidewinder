using System;
using System.IO;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder
{
    internal class Startup
    {
        private static void Main(string[] args)
        {
            var retCode = 0;
            SidewinderCommands commands;

            if (!GetCommands(out commands))
            {
                return;
            }

            if (commands.DistributeFiles != null)
            {
                Console.WriteLine("\tDetected DistributeFiles command...executing...");
                retCode = DistributorFactory.Setup(config => config.CommandIs(commands.DistributeFiles))
                                                       .Execute() ? 0 : -1;

                Console.WriteLine("Press a key to continue...");
                Console.ReadKey();
            }

            Environment.ExitCode = retCode; 
        }

        protected static bool GetCommands(out SidewinderCommands commands)
        {
            commands = null;

            var commandFile = Fluent.IO.Path.Get(SmartLocation.GetBinFolder(), Constants.SidewinderCommandFile).FullPath;
            if (File.Exists(commandFile))
            {
                commands = SerialisationHelper<SidewinderCommands>.DataContractDeserializeFromFile(commandFile);
                return true;
            }
            
            // try the parent (if we are in the lib folder)
            commandFile = Fluent.IO.Path.Get(commandFile).Parent().Parent().Combine(Constants.SidewinderCommandFile).FullPath;
            if (File.Exists(commandFile))
            {
                commands = SerialisationHelper<SidewinderCommands>.DataContractDeserializeFromFile(commandFile);
                return true;
            }
            
            // try the parent (if we are in the lib\framework folder)
            commandFile = Fluent.IO.Path.Get(commandFile).Parent().Parent().Combine(Constants.SidewinderCommandFile).FullPath;
            if (File.Exists(commandFile))
            {
                commands = SerialisationHelper<SidewinderCommands>.DataContractDeserializeFromFile(commandFile);
                return true;
            }

            return false;
        }
    }
}