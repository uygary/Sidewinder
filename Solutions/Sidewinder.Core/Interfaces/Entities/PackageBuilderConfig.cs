using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using NuGet;

namespace Sidewinder.Core.Interfaces.Entities
{
    /// <summary>
    /// http://docs.nuget.org/docs/reference/nuspec-reference
    /// </summary>
    public class PackageBuilderConfig
    {
        /// <summary>
        /// The folder where the package will be constructed. A sub-folder with the package "Id" 
        /// will be created in this location
        /// </summary>
        public string AssemblyPath { get; set; }
        /// <summary>
        /// The fully qualified path, including filename to the .nupkg file
        /// </summary>
        public string TargetPath { get; set; }

        public PackageMetadataConfig Metadata { get; set; }
    }
}