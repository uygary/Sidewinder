using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using NuGet;

namespace Sidewinder.Core.Interfaces.Entities
{
    /// <summary>
    /// http://docs.nuget.org/docs/reference/nuspec-reference
    /// </summary>
    public class PackageMetadataConfig
    {
        public string Id { get; set; }

        public Version Version { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public IEnumerable<string> Owners { get; set; }

        public Uri IconUrl { get; set; }

        public Uri LicenseUrl { get; set; }

        public Uri ProjectUrl { get; set; }

        public bool RequireLicenseAcceptance { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string ReleaseNotes { get; set; }

        public string Language { get; set; }

        public string Tags { get; set; }

        public string Copyright { get; set; }

        public IEnumerable<FrameworkName> FrameworkAssemblies { get; set; }

        public IEnumerable<PackageDependency> Dependencies { get; set; }

        public IEnumerable<string> References { get; set; }
    }
}