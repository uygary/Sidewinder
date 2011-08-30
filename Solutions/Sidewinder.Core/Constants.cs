namespace Sidewinder.Core
{
    public class Constants
    {
        public class NuGet
        {
            public const string OfficialFeedUrl = "https://go.microsoft.com/fwlink/?LinkID=206669";
            public const string ContentFolder = "content";
            public const string LibFolder = "lib";
            public const string ToolsFolder = "tools";
        }
        

        public class Sidewinder
        {
#if TESTING
            public const string OfficialFeedUrl = "http://www.myget.org/F/sidewinder/";
#else
            public const string OfficialFeedUrl = NuGet.OfficialFeedUrl;
#endif
            public const string CommandFile = "_sidewinder.xml";
            public const string VersionFile = "_sidewinder_versions.xml";
            public const string ExeFilename = "sidewinder.exe";
            public const string NuGetPackageName = "sidewinder";
            public const string DefaultDownloadFolder = "_update";
            public const string DefaultBackupFolder = "_backup";


        }
    }
}