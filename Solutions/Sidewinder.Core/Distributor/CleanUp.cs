using System;
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
            if (context.Config.Command.ConflictResolution != ConflictResolutionTypes.Overwrite)
            {
                Logger.Warn("\t** Packages remain in {0} for manual conflict resolution **", context.Config.Command.DownloadFolder);
                return true;
            }

            // if we overwrote the content then we are good to remove the update packages
            // as there is no conflicts to resolve.
            var foldersToDelete = Path.Get(context.Config.Command.DownloadFolder)
                .Directories()
                .Where(path => (string.Compare(path.FileName, Constants.Sidewinder.NuGetPackageName) != 0));

            Logger.Debug("\tRemoving these update packages from {0}...", context.Config.Command.DownloadFolder);
            foldersToDelete.MakeRelativeTo(context.Config.Command.DownloadFolder).ForEach(fp =>
                Logger.Debug("\t\t{0}", fp.FileName));

            foldersToDelete.Delete(true);
            Logger.Debug("\tDone"); 
            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            // TODO: check all the download\package folders have been deleted (except for sidewinder)
            // If not ConflictResolutionTypes.Manual)
        }
    }
}