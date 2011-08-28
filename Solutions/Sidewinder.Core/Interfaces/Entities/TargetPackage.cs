namespace Sidewinder.Core.Interfaces.Entities
{
    public class TargetPackage : NuGetPackage
    {
        public bool UpdateDependencies { get; set; }
        public bool Force { get; set; }
    }
}