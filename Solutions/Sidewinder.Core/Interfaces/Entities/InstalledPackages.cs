using System.Collections.Generic;

namespace Sidewinder.Core.Interfaces.Entities
{
    public class InstalledPackages : PackagesBase<InstalledPackage>
    {
        public InstalledPackages()
        {
        }

        public InstalledPackages(IEnumerable<InstalledPackage> packages) 
            : base(packages)
        {
        }
    }
}