using NUnit.Framework;
using StoryQ;

namespace Sidewinder.Tests.Backup
{
    [TestFixture]
    public partial class BackupSpecs
    {
        private readonly Feature myStory;

        public BackupSpecs()
        {
            myStory = new Story("Backup a set of folders and files")
                .InOrderTo("")
                .AsA("")
                .IWant("");
        }

        [Test]
        public void BackupAll()
        {
            myStory.WithScenario("")
                .Given(TheDirectory_ShouldBeBackedUp, @"testdata\install_backup")
                    .And(TheBackupLocation_IsUsed, @"testdata\install_backup\_nestedbackups")
                .When(ItIsBackedup)
                .Then(TheBackupArtifactsAreCreated)
                .ExecuteWithReport();
        }
    }
}