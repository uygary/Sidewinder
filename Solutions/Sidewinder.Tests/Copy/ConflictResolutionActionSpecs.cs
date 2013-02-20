using NUnit.Framework;
using StoryQ;

namespace Sidewinder.Tests.Copy
{
    [TestFixture]
    public class ConflictResolutionActionSpecs
    {
        private readonly Feature _story;

        public ConflictResolutionActionSpecs()
        {
            _story = new Story("Testing the conflict resolution actions")
                .InOrderTo("ensure the right action is taken")
                .AsA("component copying a file")
                .IWant("the component to indicate the correct action based upon it behaviour");
        }

        [Test]
        public void CopyAlwaysAction()
        {
            using (var domain = new ConflictResolutionActionDomain())
            {
                _story.WithScenario("The file should always be copied")
                    .Given(domain.TheCopyAlwaysResolutionComponentIsUsed)
                    .When(domain.TheComponentIsExecuted)
                    .Then(domain.TheResultShouldBeTrue)
                    .ExecuteWithReport();
            }
        }

        [Test]
        public void CopyNeverAction()
        {
            using (var domain = new ConflictResolutionActionDomain())
            {
                _story.WithScenario("The file should never be copied")
                    .Given(domain.TheCopyNeverResolutionComponentIsUsed)
                    .When(domain.TheComponentIsExecuted)
                    .Then(domain.TheResultShouldBeFalse)
                    .ExecuteWithReport();
            }
        }

        [Test]
        public void CopyConsoleAskWithYesResponse()
        {
            using (var domain = new ConflictResolutionActionDomain())
            {

                _story.WithScenario("The user should be prompted for action and answers 'Y' to overwrite")
                    .Given(domain.TheCopyConsoleAskResolutionComponentIsUsedWithResponse_, 'Y')
                    .When(domain.TheComponentIsExecuted)
                    .Then(domain.TheResultShouldBeTrue)
                    .ExecuteWithReport();
            }
        }

        [Test]
        public void CopyConsoleAskWithNoResponse()
        {
            using (var domain = new ConflictResolutionActionDomain())
            {
                _story.WithScenario("The user should be prompted for action and answers 'N' to overwrite")
                    .Given(domain.TheCopyConsoleAskResolutionComponentIsUsedWithResponse_, 'N')
                    .When(domain.TheComponentIsExecuted)
                    .Then(domain.TheResultShouldBeFalse)
                    .ExecuteWithReport();
            }
        }
    }
}