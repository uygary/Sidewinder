namespace Sidewinder.Interfaces.Entities
{
    public class SidewinderCommands
    {
        public DistributeFiles DistributeFiles { get; set; }
    }

    public class DistributeFiles
    {
        public string TargetProcess { get; set; }
    }
}