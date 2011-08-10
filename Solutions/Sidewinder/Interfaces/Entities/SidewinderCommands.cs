namespace Sidewinder.Interfaces.Entities
{
    public class SidewinderCommands
    {
        public DistributeFiles DistributeFiles { get; set; }
    }

    public class DistributeFiles
    {
        public string TargetProcessFilename { get; set; }
        public string FrameworkHint { get; set; }
        public int SecondsToWait { get; set; }

        public DistributeFiles()
        {
            // default timeout to wait for the running process to terminate
            SecondsToWait = 10;
        }
    }
}