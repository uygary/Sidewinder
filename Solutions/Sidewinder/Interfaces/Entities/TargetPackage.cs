using System;

namespace Sidewinder.Interfaces.Entities
{
    public class TargetPackage : NuGetPackage
    {
        public bool UpdateDependencies { get; set; }
        public bool Force { get; set; }
    }
}