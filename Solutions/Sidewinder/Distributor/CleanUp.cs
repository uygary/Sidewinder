using System;
using Fluent.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Distributor
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
            Path.Get(context.Config.Command.DownloadFolder)
                .Directories(folder => string.Compare(folder.DirectoryName, Constants.Sidewinder.UpdateFolder,
                                                      StringComparison.InvariantCultureIgnoreCase) != 0)
                .Delete(true);

            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            
        }
    }
}