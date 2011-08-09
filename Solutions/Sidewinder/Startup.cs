using System.IO;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder
{
    internal class Startup
    {
        private static void Main(string[] args)
        {
            SidewinderCommands commands;

            if (GetCommands(out commands))
            {
                // add other commands here
                //return DistributeFilesAfterDownload(commands);
            }

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