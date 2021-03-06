﻿namespace Sidewinder.Core.Interfaces.Entities
{
    public class CmdlineArgs
    {
        public string LogLevel { get; set; }
        public string Package { get; set; }
        public string Feed { get; set; }
        public string InstallFolder { get; set; }
        public bool Force { get; set; }
        public bool Dependencies { get; set; }
        public bool Overwrite { get; set; }
        public bool Manual { get; set; }
        public bool SkipOfficialFeed { get; set; }
        public bool NoWaitPrompt { get; set; }

        // target framework switches
        public bool Net45 { get; set; }
        public bool Net40 { get; set; }
        public bool Net20 { get; set; }
        public bool Net11 { get; set; }        

        public CmdlineArgs()
        {
            LogLevel = Level.Debug.ToString();
        }
    }
}