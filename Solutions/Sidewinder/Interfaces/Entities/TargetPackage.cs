using System;

namespace Sidewinder.Interfaces.Entities
{
    public class TargetPackage
    {
        public TargetPackage()
        {
            NuGetFeedUrl = Constants.OfficialNuGetFeedUrl;
        }

        public string Name { get; set; }
        public Version Version { get; set; }
        public string FrameworkHint { get; set; }
        public string NuGetFeedUrl { get; set; }
    }
}