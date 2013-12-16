using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Sidewinder.Core.Interfaces.Entities
{
    [DataContract]
    public abstract class PackagesBase<T> where T : NuGetPackage
    {
        [DataMember]
        private readonly Dictionary<string, T> myPackages;

        protected PackagesBase()
        {
            myPackages = new Dictionary<string, T>();
        }

        protected PackagesBase(IEnumerable<T> packages)
        {
            packages.ToList().ForEach(Add);
        }

        public T this[string name]
        {
            get { return myPackages[NormaliseKey(name)]; }
        }

        public void Add(T package)
        {
            myPackages.Add(NormaliseKey(package.Name), package);
        }

        public bool ContainsKey(string key)
        {
            return myPackages.ContainsKey(NormaliseKey(key));
        }

        public void Remove(string key)
        {
            myPackages.Remove(NormaliseKey(key));
        }

        public List<KeyValuePair<string, T>> ToList()
        {
            return myPackages.ToList();
        }

        private static string NormaliseKey(string key)
        {
            return key.ToLowerInvariant();
        }        
    }
}