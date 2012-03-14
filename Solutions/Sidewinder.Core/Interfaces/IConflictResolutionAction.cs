namespace Sidewinder.Core.Interfaces
{
    public interface IConflictResolutionAction
    {
        bool Resolve(string source, string dest);
    }
}