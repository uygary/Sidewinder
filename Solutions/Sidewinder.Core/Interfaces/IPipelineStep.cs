namespace Sidewinder.Core.Interfaces
{
    public interface IPipelineStep<in T>
    {
        void EntryConditions(T context);        
        bool Execute(T context);
        void ExitConditions(T context);
    }
}