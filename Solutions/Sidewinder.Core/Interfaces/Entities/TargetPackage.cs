namespace Sidewinder.Core.Interfaces.Entities
{
    public class TargetPackage : NuGetPackage
    {
        public TargetPackage()
        {
        }

        public TargetPackage(InstalledPackage package)
        {
            Name = package.Name;
            NuGetFeedUrl = package.NuGetFeedUrl;
            Version = package.Version;
        }

        public bool UpdateDependencies { get; set; }
        public bool Force { get; set; }
    }
}