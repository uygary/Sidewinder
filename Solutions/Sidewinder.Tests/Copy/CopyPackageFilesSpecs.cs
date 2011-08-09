using NUnit.Framework;
using Sidewinder.Distributor;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;
using StoryQ;

namespace Sidewinder.Tests.Copy
{
    [TestFixture]
    public partial class CopyPackageFilesSpecs
    {
        private readonly Feature myStory;
        private readonly IPipelineStep<DistributorContext> myStep;

        public CopyPackageFilesSpecs()
        {
            myStep = new CopyPackageFiles();

            myStory = new Story("Copy the content of a NuGet package to another folder")
                .InOrderTo("update the content of an application")
                .AsA("application user")
                .IWant("the content of the nuget package distributed to my application installation folder");
        }

        [Test]
        public void CopyPackageFilesHappyPath()
        {
            myStory.WithScenario("The installation folder is empty")
                .Given(TheDirectory_ContainsThePackageFiles, @"testdata\update")
                    .And(TheDirectory_ContainsThePackageBinaryFiles, @"testdata\update\lib\net40")
                    .And(TheInstallLocation_IsUsed, @"testdata\install_write")
                    .And(TheInstallationLocationIsCleaned)
                .When(TheFilesAreCopied)
                .Then(TheInstallationFolderContentIsCorrect)
                .ExecuteWithReport();
        }
    }
}