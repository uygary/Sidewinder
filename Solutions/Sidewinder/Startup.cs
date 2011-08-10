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
                retCode = DistributorFactory.Setup(config => config.InstallTo(@"c:\temp\sidewinder_wp")
                                                       .PackageIs(commands.DistributeFiles))
                                                       .Execute() ? 0 : -1;
            }

            Environment.ExitCode = retCode; 
        }

        protected static bool GetCommands(out SidewinderCommands commands)
        {
            commands = null;
            var commandFile = Fluent.IO.Path.Current.Combine(Constants.SidewinderCommandFile).FullPath;

            if (!File.Exists(commandFile))
                return false;
            commands = SerialisationHelper<SidewinderCommands>.DataContractDeserializeFromFile(commandFile);
            return true;
        }

    }
}