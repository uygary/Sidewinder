using System;
using Sidewinder.Core;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;
using FluentAssertions;

namespace Sidewinder.Tests.Upgrade
{
    public partial class UpgradeSpecs
    {
        private string _commandFile;
        private SidewinderCommands _commands;

        private void The_CommandFile(string file)
        {
            _commandFile = SmartLocation.GetLocation(@"testdata\" + file);
        }

        private void ItIsDeserialisedIntoTheCommand()
        {
            try
            {
                _commands = SerialisationHelper<SidewinderCommands>.DataContractDeserializeFromFile(_commandFile);
            }
            catch (Exception e)
            {
                var bob = e;
            }
        }

        private void TheConflictResolutionInstructionShouldBe_(ConflictResolutionTypes expected)
        {
            _commands.DistributeFiles.ConflictResolution.Should().Be(expected);
        }

        private void TheCommandContainsTheDistributeCommand()
        {
            _commands.DistributeFiles.Should().NotBeNull();
        }

        private void TheLogLevelShouldBe_(Level expected)
        {
            _commands.LogLevel.Should().Be(expected);
        }

        private void TheTargetProcessIdShouldBe_(int expected)
        {
            _commands.DistributeFiles.TargetProcessId.Should().Be(expected);
        }

        private void TheTargetProcessIdShouldBeNull()
        {
            _commands.DistributeFiles.TargetProcessId.Should().Be(null);
        }

        private void TheTargetProcessFilenameShouldBe_(string expected)
        {
            _commands.DistributeFiles.TargetProcessFilename.Should().Be(expected);
        }
    }
}