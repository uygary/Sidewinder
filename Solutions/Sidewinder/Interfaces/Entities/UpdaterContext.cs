using System.Collections.Generic;

namespace Sidewinder.Interfaces.Entities
{
    public class UpdaterContext
    {
        public UpdaterContext()
        {
            Updates = new List<UpdatedPackage>();
            InstalledPackages = new InstalledPackages();
        }

        /// <summary>
        /// This is the config the pipeline was launched with
        /// </summary>
        public UpdateConfig Config { get; set; }
        /// <summary>
        /// This is a list of packages already installed
        /// </summary>
        public InstalledPackages InstalledPackages { get; set; }
        /// <summary>
        /// This is a list of updates available
        /// </summary>
        public List<UpdatedPackage> Updates { get; set; }
    }
}