using System;
using System.Collections.Generic;
using System.Linq;

namespace Sidewinder.Core.Interfaces.Entities
{
    /// <summary>
    /// Provides a controlled wrapper around a dictionary of <see cref="TargetPackage"/> items
    /// </summary>
    public class TargetPackages : PackagesBase<TargetPackage>
    {
        //private Dictionary<string, TargetPackage> myTargets;

        //public TargetPackages()
        //{
        //    myTargets = new Dictionary<string, TargetPackage>();
        //}

        //public TargetPackages(IEnumerable<TargetPackage> packages)
        //{
        //    packages.ToList().ForEach(Add);
        //}

        //public void Add(TargetPackage package)
        //{
        //    myTargets.Add(NormaliseKey(package.Name), package);
        //}

        /// <summary>
        /// Adds a package using the installed details specified. It will only download if
        /// there is an update and it will include dependent packages
        /// </summary>
        /// <param name="package"></param>
        public void Add(InstalledPackage package)
        {
            Add(package, false, true);
        }

        public void Add(InstalledPackage package, bool force, bool updateDependencies)
        {
            Add(new TargetPackage(package)
                    {
                        Force = force,
                        UpdateDependencies = updateDependencies
                    });
        }

        //public bool ContainsKey(string key)
        //{
        //    return myTargets.ContainsKey(NormaliseKey(key));
        //}

        //public void Remove(string key)
        //{
        //    myTargets.Remove(NormaliseKey(key));
        //}

        //public List<KeyValuePair<string, TargetPackage>> ToList()
        //{
        //    return myTargets.ToList();
        //}

        //private static string NormaliseKey(string key)
        //{
        //    return key.ToLowerInvariant();
        //}
    }
}