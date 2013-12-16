
using System;
using System.Collections.Generic;
using Sidewinder.Core.Interfaces.Entities;
using System.Linq;
using Sidewinder.Core.Interfaces.Exceptions;


namespace Sidewinder.Core
{
    /// <summary>
    /// http://docs.nuget.org/docs/reference/nuspec-reference
    /// </summary>
    public class PackageBuilderConfigBuilder
    {
        private readonly PackageBuilderConfig myConfig;

        public PackageBuilderConfigBuilder()
        {
            myConfig = new PackageBuilderConfig
                           {
                           };
        }

        public PackageBuilderConfigBuilder AssembleItHere(string assemblyPath)
        {
            myConfig.AssemblyPath = assemblyPath;
            return this;
        }

        public PackageBuilderConfigBuilder CreateItHere(string targetPath)
        {
            myConfig.TargetPath = targetPath;
            return this;
        }

        public PackageBuilderConfigBuilder Metadata(Action<PackageMetadataBuilder> setup)
        {
            var pmb = new PackageMetadataBuilder();
            setup(pmb);
            myConfig.Metadata = pmb.Build();

            return this;
        }

        public bool Validate(out IEnumerable<string> errors)
        {
            errors = new List<string>();
            // TODO

            return errors.Count() == 0;
        }

        public PackageBuilderConfig Build()
        {
            IEnumerable<string> errors;

            if (!Validate(out errors))
                throw new PackageBuildException(myConfig, errors);
            return myConfig;
        }
    }
} 