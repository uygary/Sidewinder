using System;

namespace Sidewinder.Interfaces.Entities
{
    public class TargetPackage
    {
        public TargetPackage()
        {
            NuGetFeedUrl = Constants.NuGet.OfficialFeedUrl;
        }

        public string Name { get; set; }
        public Version Version { get; set; }
        public string NuGetFeedUrl { get; set; }
        public bool UpdateDependencies { get; set; }
        public bool Force { get; set; }
    }
}