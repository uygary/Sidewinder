using Sidewinder.Core.Interfaces;

namespace Sidewinder.Core.ConflictResolution
{
    public abstract class CopyResolutionActionBase : IConflictResolutionAction
    {
        private readonly bool _result;

        protected CopyResolutionActionBase(bool result)
        {
            _result = result;
        }

        public bool Resolve(string source, string dest)
        {
            return _result;
        }
    }
}