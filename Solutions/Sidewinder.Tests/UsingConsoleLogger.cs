using NUnit.Framework;
using Sidewinder.Core;
using Sidewinder.Core.Interfaces;

namespace Sidewinder.Tests
{
    [SetUpFixture]
    public class UsingConsoleLogger
    {
        [SetUp]
        public void Setup()
        {
            Logger.Initialise(new ConsoleLogger(Level.Debug));
        }
    }
}