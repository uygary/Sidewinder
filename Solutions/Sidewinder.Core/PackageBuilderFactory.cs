
using System;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.PackageBuilder;

namespace Sidewinder.Core
{
    /// <summary>
    /// This will build and configure a pipeline to create a NuGet package
    /// </summary>
    public class PackageBuilderFactory
    {
        public static IPackageBuilderAgent Setup(Action<PackageBuilderConfigBuilder> setup)
        {
            var builder = new PackageBuilderConfigBuilder();
            setup(builder);
            var config = builder.Build();

            var agent = new DefaultPackageBuilderAgent(config);
            return agent;
        }
    }
}