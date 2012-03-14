using System;
using NUnit.Framework;
using Sidewinder.Core.Interfaces.Entities;
using StoryQ;

namespace Sidewinder.Tests.Upgrade
{
    [TestFixture]
    public partial class UpgradeSpecs
    {
        private readonly Feature myStory;        

        public UpgradeSpecs()
        {
            myStory = new Story("Ensure that upgrades from previous versions works")
                .InOrderTo("Upgrade the application")
                .AsA("new version of Sidewinder")
                .IWant("to ensure that an upgrade will not crash or be incompatible");
        }

        [Test]
        public void UpgradeFrom1Dot2To1Dot3()
        {
            myStory.WithScenario("Simulate an upgrade from v1.2 to v1.3")
                .Given(The_CommandFile, "_sidewinder_v1.2.xml")
                .When(ItIsDeserialisedIntoTheCommand)
                .Then(TheCommandContainsTheDistributeCommand)
                    .And(TheConflictResolutionInstructionShouldBe_, ConflictResolutionTypes.Ask)
                .ExecuteWithReport();
        }
    }
}