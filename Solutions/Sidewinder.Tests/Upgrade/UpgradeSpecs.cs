
using NUnit.Framework;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;
using StoryQ;

namespace Sidewinder.Tests.Upgrade
{
    [TestFixture]
    public partial class UpgradeSpecs
    {
        private readonly Feature _story;

        public UpgradeSpecs()
        {
            _story = new Story("Ensure that upgrades from previous versions works")
                .InOrderTo("Upgrade the application")
                .AsA("new version of Sidewinder")
                .IWant("to ensure that an upgrade will not crash or be incompatible");
        }

        [Test]
        public void UpgradeFrom1Dot2()
        {
            _story.WithScenario("Simulate an upgrade from v1.2")
                .Given(The_CommandFile, "sidewinder-v1.2.xml")
                .When(ItIsDeserialisedIntoTheCommand)
                .Then(TheCommandContainsTheDistributeCommand)
                    .And(TheConflictResolutionInstructionShouldBe_, ConflictResolutionTypes.Ask)
                .ExecuteWithReport();
        }

        [Test]
        public void UpgradeFrom1Dot4()
        {
            _story.WithScenario("Simulate an upgrade from v1.4")
                .Given(The_CommandFile, "sidewinder-v1.4.xml")
                .When(ItIsDeserialisedIntoTheCommand)
                .Then(TheCommandContainsTheDistributeCommand)
                    .And(TheLogLevelShouldBe_, Level.Debug)
                .ExecuteWithReport();
        }

        [Test]
        public void UpgradeFromPreTargetProcessId()
        {
            _story.WithScenario("Simulate an upgrade from a version that only supported TargetProcessName")
                .Given(The_CommandFile, "sidewinder-v1.4.xml")
                .When(ItIsDeserialisedIntoTheCommand)
                .Then(TheCommandContainsTheDistributeCommand)
                    .And(TheTargetProcessIdShouldBeNull)
                    .And(TheTargetProcessFilenameShouldBe_, @"C:\temp\Debug\Wolfpack.Agent.exe")
                .ExecuteWithReport(); 
        }

        [Test]
        public void UpgradeFrom1Dot5Dot2()
        {
            _story.WithScenario("Simulate an upgrade from a version that supports TargetProcessId")
                .Given(The_CommandFile, "sidewinder-v1.5.2.xml")
                .When(ItIsDeserialisedIntoTheCommand)
                .Then(TheCommandContainsTheDistributeCommand)
                    .And(TheTargetProcessIdShouldBe_, 1234)
                    .And(TheTargetProcessFilenameShouldBe_, @"C:\temp\Debug\Wolfpack.Agent.exe")
                .ExecuteWithReport(); 
        }
    }
}