using System;

namespace Sidewinder.Interfaces.Entities
{
    public class NuGetPackage
    {
        public NuGetPackage()
        {
            NuGetFeedUrl = Constants.NuGet.OfficialFeedUrl;
        }

        public string Name { get; set; }
        public Version Version { get; set; }
        public string NuGetFeedUrl { get; set; }
    }
}