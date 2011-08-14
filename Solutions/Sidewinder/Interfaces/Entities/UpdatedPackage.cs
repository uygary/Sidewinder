using NuGet;

namespace Sidewinder.Interfaces.Entities
{
    public class UpdatedPackage
    {
        public TargetPackage Target { get; set; }
        public IPackage Package { get; set; }
    }
}