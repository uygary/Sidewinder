using NUnit.Framework;
using StoryQ;

namespace Sidewinder.Tests.Backup
{
    [TestFixture]
    public partial class BackupSpecs
    {
        private readonly Feature _story;

        public BackupSpecs()
        {
            _story = new Story("Backup a set of folders and files")
                .InOrderTo("be confident that a failed update can be rolled back")
                .AsA("user of the application")
                .IWant("the application backed up prior to update");
        }

        [Test]
        public void BackupAll()
        {
            _story.WithScenario("happy path simple backup")
                .Given(TheDirectory_ShouldBeBackedUp, @"testdata\install_backup")
                    .And(TheBackupLocation_IsUsed, @"testdata\install_backup\_nestedbackups")
                .When(ItIsBackedup)
                .Then(TheBackupArtifactsAreCreated)
                .ExecuteWithReport();
        }
    }
}