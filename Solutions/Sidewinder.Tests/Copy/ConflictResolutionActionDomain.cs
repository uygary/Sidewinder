
using System;
using System.IO;
using System.Text;
using Sidewinder.Core.ConflictResolution;

using FluentAssertions;
using Sidewinder.Core.Interfaces;

namespace Sidewinder.Tests.Copy
{
    
    public partial class ConflictResolutionActionSpecs : IDisposable
    {
        private string _source;
        private string _dest;
        private IConflictResolutionAction _sut;
        private bool _result;

        private StringReader _consoleIn;
        private StringWriter _consoleOut;

        private void TheCopyAlwaysResolutionComponentIsUsed()
        {
            _sut = new CopyAlwaysResolutionAction();
        }

        private void TheCopyNeverResolutionComponentIsUsed()
        {
            _sut = new CopyNeverResolutionAction();
        }

        private void TheComponentIsExecuted()
        {
            _result = _sut.Resolve(_source, _dest);
        }

        private void TheResultShouldBeTrue()
        {
            _result.Should().BeTrue();
        }

        private void TheResultShouldBeFalse()
        {
            _result.Should().BeFalse();
        }

        private void TheCopyConsoleAskResolutionComponentIsUsedWithResponse_(char key)
        {
            _consoleOut = new StringWriter(new StringBuilder());
            Console.SetOut(_consoleOut);

            _consoleIn = new StringReader(new string(key, 1));
            Console.SetIn(_consoleIn);

            _sut = new CopyAskResolutionAction();
        }

        public void Dispose()
        {
            if (_consoleIn != null)
                _consoleIn.Dispose();
            if (_consoleOut != null)
                _consoleOut.Dispose();
        }
    }
}