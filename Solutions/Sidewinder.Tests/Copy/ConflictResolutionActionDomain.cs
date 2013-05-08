
using System;
using System.IO;
using Sidewinder.Core.ConflictResolution;

using FluentAssertions;
using Sidewinder.Core.Interfaces;

namespace Sidewinder.Tests.Copy
{    
    public class ConflictResolutionActionDomain : IDisposable
    {
        private string _source;
        private string _dest;
        private IConflictResolutionAction _sut;
        private bool _result;

        private StringReader _consoleIn;

        public void TheCopyAlwaysResolutionComponentIsUsed()
        {
            _sut = new CopyAlwaysResolutionAction();
        }

        public void TheCopyNeverResolutionComponentIsUsed()
        {
            _sut = new CopyNeverResolutionAction();
        }

        public void TheComponentIsExecuted()
        {
            _result = _sut.Resolve(_source, _dest);
        }

        public void TheResultShouldBeTrue()
        {
            _result.Should().BeTrue();
        }

        public void TheResultShouldBeFalse()
        {
            _result.Should().BeFalse();
        }

        public void TheCopyConsoleAskResolutionComponentIsUsedWithResponse_(char key)
        {
            _consoleIn = new StringReader(new string(key, 1));
            Console.SetIn(_consoleIn);

            _sut = new CopyAskResolutionAction();
        }

        public void Dispose()
        {
            if (_consoleIn != null)
                _consoleIn.Dispose();
        }
    }
}