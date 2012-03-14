using System;
using NUnit.Framework;
using Sidewinder.Core;
using Sidewinder.Core.Interfaces.Entities;
using FluentAssertions;
using System.Linq;

namespace Sidewinder.Tests.Builders
{
    [TestFixture]
    public partial class PackageBuilderConfigBuilderSpecs
    {
        private PackageBuilderConfigBuilder mySut;
        private PackageBuilderConfig myActualResult;


        private void ThePackageBuilderBuildsTheConfig()
        {
            myActualResult = mySut.Build();
        }

        private void TheMetadataPropertyIsSet()
        {
            myActualResult.Metadata.Should().NotBeNull();
        }

        private void TheMetadataIsSetFrom(PackageMetadataConfig metadata)
        {
            mySut.Metadata(setup => setup.Authors(metadata.Authors.ToArray())
                .Copyright(metadata.Copyright)
                .Description(metadata.Description)
                );
        }

        private void TheBuilderIsCreated()
        {
            mySut = new PackageBuilderConfigBuilder();
        }
    }
}