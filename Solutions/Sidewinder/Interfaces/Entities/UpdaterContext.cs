using System.Collections.Generic;
using NuGet;

namespace Sidewinder.Interfaces.Entities
{
    public class UpdaterContext
    {
        public UpdaterContext()
        {
            Updates = new List<UpdatedPackage>();
        }

        public List<UpdatedPackage> Updates { get; set; }
        public UpdateConfig Config { get; set; }
    }
}