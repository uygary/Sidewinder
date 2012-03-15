using NUnit.Framework;
using Sidewinder.Core;
using Sidewinder.Core.Interfaces.Entities;
using StoryQ;

namespace Sidewinder.Tests.Builders
{
    [TestFixture]
    public partial class PackageBuilderConfigBuilderSpecs
    {
        private readonly Feature myStory;
        

        public PackageBuilderConfigBuilderSpecs()
        {
            myStory = new Story("Building the configuration for building a nuget package")
                .InOrderTo("update the content of an application")
                .AsA("application user")
                .IWant("the content of the nuget package distributed to my application installation folder");
        }

        //[Test]
        //public void SettingMetadataProperties()
        //{
        //    var metadata = new PackageMetadataConfig
        //                             {

        //                             };

        //    myStory.WithScenario("")
        //        .Given(TheBuilderIsCreated)
        //            .And(TheMetadataIsSetFrom, metadata)
        //        .When(ThePackageBuilderBuildsTheConfig)
        //        .Then(TheMetadataPropertyIsSet)
        //        .ExecuteWithReport();
        //}
    }
}