using NUnit.Framework;
using StoryQ;

namespace Sidewinder.Tests.Copy
{
    [TestFixture]
    public partial class ConflictResolutionActionSpecs
    {
        private readonly Feature myStory;

        public ConflictResolutionActionSpecs()
        {
            myStory = new Story("Testing the conflict resolution actions")
                .InOrderTo("ensure the right action is taken")
                .AsA("component copying a file")
                .IWant("the component to indicate the correct action based upon it behaviour");
        }

        [Test]
        public void CopyAlwaysAction()
        {
            myStory.WithScenario("The file should always be copied")
                .Given(TheCopyAlwaysResolutionComponentIsUsed)
                .When(TheComponentIsExecuted)
                .Then(TheResultShouldBeTrue)
                .ExecuteWithReport();
        }

        [Test]
        public void CopyNeverAction()
        {
            myStory.WithScenario("The file should never be copied")
                .Given(TheCopyNeverResolutionComponentIsUsed)
                .When(TheComponentIsExecuted)
                .Then(TheResultShouldBeFalse)
                .ExecuteWithReport();
        }

        [Test]
        public void CopyConsoleAskWithYesResponse()
        {
            myStory.WithScenario("The user should be prompted for action and answers 'Y' to overwrite")
                .Given(TheCopyConsoleAskResolutionComponentIsUsedWithResponse_, 'Y')
                .When(TheComponentIsExecuted)
                .Then(TheResultShouldBeTrue)         
                .ExecuteWithReport();
        }

        [Test]
        public void CopyConsoleAskWithNoResponse()
        {
            myStory.WithScenario("The user should be prompted for action and answers 'N' to overwrite")
                .Given(TheCopyConsoleAskResolutionComponentIsUsedWithResponse_, 'N')
                .When(TheComponentIsExecuted)
                .Then(TheResultShouldBeFalse)
                .ExecuteWithReport();
        }
    }
}