using System;

namespace Sidewinder.Core.Interfaces.Entities
{
    public class InstalledPackage : NuGetPackage
    {
        public DateTime LastUpdated { get; set; }
    }
}