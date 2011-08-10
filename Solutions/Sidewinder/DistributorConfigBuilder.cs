
using Fluent.IO;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder
{
    public class DistributorConfigBuilder
    {
        private readonly DistributorConfig myConfig;

        public DistributorConfigBuilder()
        {
            myConfig = new DistributorConfig();
        }

        public DistributorConfig Build()
        {
            var config = new DistributorConfig
                             {
                                 InstallFolder = myConfig.InstallFolder,
                                 Package = myConfig.Package
                             };
            return config;
        }

        public DistributorConfigBuilder InstallTo(string folder)
        {
            myConfig.InstallFolder = folder;
            return this;
        }

        public DistributorConfigBuilder PackageIs(DistributeFiles package)
        {
            myConfig.Package = package;

            if (!string.IsNullOrWhiteSpace(myConfig.InstallFolder))
                return this;

            // the install folder must be the one the contains the exe that launched us
            myConfig.InstallFolder = Path.Get(package.TargetProcessFilename).Parent().FullPath;
            return this;
        }
    }
}