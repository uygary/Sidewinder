
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
    public class PackageMetadataBuilder
    {
        private readonly PackageMetadataConfig myConfig;

        public PackageMetadataBuilder()
        {
            myConfig = new PackageMetadataConfig
                           {
                           };
        }

        public PackageMetadataBuilder Id(string id)
        {
            myConfig.Id = id;
            return this;
        }

        public PackageMetadataBuilder Version(Version version)
        {
            myConfig.Version = version;
            return this;
        }

        public PackageMetadataBuilder Title(string title)
        {
            myConfig.Title = title;
            return this;
        }

        public PackageMetadataBuilder Authors(params string[] authors)
        {
            myConfig.Authors = authors;
            return this;
        }

        public PackageMetadataBuilder Owners(params string[] owners)
        {
            myConfig.Owners = owners;
            return this;
        }

        public PackageMetadataBuilder Description(string description)
        {
            myConfig.Description = description;
            return this;
        }

        public PackageMetadataBuilder ReleaseNotes(string releaseNotes)
        {
            myConfig.ReleaseNotes = releaseNotes;
            return this;
        }

        public PackageMetadataBuilder Summary(string summary)
        {
            myConfig.Summary = summary;
            return this;
        }

        public PackageMetadataBuilder Language(string language)
        {
            myConfig.Language = language;
            return this;
        }

        public PackageMetadataBuilder ProjectUrl(string url)
        {
            myConfig.ProjectUrl = new UriBuilder(url).Uri;
            return this;
        }

        public PackageMetadataBuilder IconUrl(string url)
        {
            myConfig.IconUrl = new UriBuilder(url).Uri;
            return this;
        }

        public PackageMetadataBuilder LicenseUrl(string url)
        {
            myConfig.LicenseUrl = new UriBuilder(url).Uri;
            return this;
        }

        public PackageMetadataBuilder Copyright(string copyright)
        {
            myConfig.Copyright = copyright;
            return this;
        }

        public PackageMetadataBuilder MustAcceptLicense()
        {
            myConfig.RequireLicenseAcceptance = true;
            return this;
        }


        public bool Validate(out IEnumerable<string> errors)
        {
            errors = new List<string>();
            // TODO

            return errors.Count() == 0;
        }

        public PackageMetadataConfig Build()
        {
            IEnumerable<string> errors;

            if (!Validate(out errors))
                throw new PackageMetadataException(myConfig, errors);
            return myConfig;
        }
    }
} 