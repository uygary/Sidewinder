using System;

namespace Sidewinder.Interfaces.Entities
{
    public class InstalledPackage : NuGetPackage
    {
        public DateTime LastUpdated { get; set; }
    }
}