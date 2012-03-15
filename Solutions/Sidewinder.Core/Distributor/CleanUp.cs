using Fluent.IO;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Distributor
{
    /// <summary>
    /// This will remove all the packages downloaded from the update folder apart from the
    /// sidewinder package
    /// </summary>
    public class CleanUp : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            
        }

        public bool Execute(DistributorContext context)
        {
            // if we are manually resolving conflicts then leave all the package updates intact
            if (context.Config.Command.ConflictResolution == ConflictResolutionTypes.Manual)
            {
                return true;
            }

            // if we asked or overwrote the content then we should have resolved any
            // conflicts so delete all update package folders apart from sidewinder
            Path.Get(context.Config.Command.DownloadFolder)
                .Directories()
                .Where(path => (string.Compare(path.FileName, Constants.Sidewinder.NuGetPackageName) != 0))
                .Delete(true);

            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            // TODO: check all the download\package folders have been deleted (except for sidewinder)
            // If not ConflictResolutionTypes.Manual)
        }
    }
}