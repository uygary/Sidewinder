using NUnit.Framework;
using Sidewinder.Core.Distributor;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;
using StoryQ;

namespace Sidewinder.Tests.Copy
{
    [TestFixture]
    public partial class CopyPackageFilesSpecs : BaseSpec
    {
        private readonly IPipelineStep<DistributorContext> myStep;

        public CopyPackageFilesSpecs()
        {
            myStep = new CopyContentFiles();
        }

        [Test]
        public void CopyContentFilesToEmptyDestinationWithOverwriteResolution()
        {
            TellStory().WithScenario("The installation folder is empty, overwrite resolution is used")
                .Given(The_DirectoryContainsThePackageFiles, @"testdata\update")
                    .And(TheInstallLocation_IsUsed, @"testdata\install_write")
                    .And(TheInstallationLocationIsCleaned)
                    .And(TheOverwriteResolutionActionIsUsed)
                .When(TheFilesAreCopied)
                .Then(TheInstallationFolderContainsTheContentFiles)
                .ExecuteWithReport();
        }

        [Test]
        public void CopyContentFilesToConflictingDestinationWithOverwriteResolution()
        {
            TellStory().WithScenario("The installation folder has conflicts, overwrite resolution is used")
                .Given(The_DirectoryContainsThePackageFiles, @"testdata\update")
                    .And(TheInstallLocation_IsUsed, @"testdata\install_write")
                    .And(TheInstallationLocationIsCleaned)
                    .And(TheConflictingContentFilesAreCopiedToTheInstallationLocation)
                    .And(TheOverwriteResolutionActionIsUsed)
                .When(TheFilesAreCopied)
                .Then(TheInstallationFolderContainsTheContentFiles)
                    .And(AllTheContentFilesHaveBeenUpdated)
                .ExecuteWithReport();
        }

        [Test]
        public void CopyContentFilesToConflictingDestinationWithManualResolution()
        {
            TellStory().WithScenario("The installation folder has conflicts, manual resolution is used")
                .Given(The_DirectoryContainsThePackageFiles, @"testdata\update")
                    .And(TheInstallLocation_IsUsed, @"testdata\install_write")
                    .And(TheInstallationLocationIsCleaned)
                    .And(TheConflictingContentFilesAreCopiedToTheInstallationLocation)
                    .And(TheManualResolutionActionIsUsed)
                .When(TheFilesAreCopied)
                .Then(TheInstallationFolderContainsTheContentFiles)
                    .And(AllTheContentFilesHaveNotBeenUpdated)
                .ExecuteWithReport();
        }

        protected override Feature TellStory()
        {
            return new Story("Copy the content of a NuGet package to another folder")
                .InOrderTo("update the content of an application")
                .AsA("application user")
                .IWant("the content of the nuget package distributed to my application installation folder");

        }
    }
}
