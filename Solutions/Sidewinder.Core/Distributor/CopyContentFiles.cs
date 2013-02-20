using System;
using Fluent.IO;
using Sidewinder.Core.ConflictResolution;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Distributor
{
    /// <summary>
    /// This step will copy the files from content folder within the nuget update package 
    /// to the root of the target folder
    /// </summary>
    public class CopyContentFiles : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.Config == null)
                throw new ArgumentException("Config property is null", "context");
            if (string.IsNullOrWhiteSpace(context.Config.Command.DownloadFolder))
                throw new ArgumentException("Config.Command.DownloadFolder property not set", "context");
            if (string.IsNullOrWhiteSpace(context.Config.Command.InstallFolder))
                throw new ArgumentException("Config.Command.InstallFolder property not set", "context");
        }

        public bool Execute(DistributorContext context)
        {
            var updates = context.Config.Command.Updates;
            var resolution = GetConflictResolutionStrategy(context.Config.Command.ConflictResolution);

            updates.ForEach(update
                =>
                    {
                        Logger.Info("\tProcessing update package (content) {0}->{1}...",
                                            update.Target.Name,
                                            context.Config.Command.InstallFolder);

                        // create it just in case
                        Path.Get(context.Config.Command.InstallFolder).CreateDirectory();

                        // copy the content files
                        var contentPath = Path.Get(context.Config.Command.DownloadFolder,
                                                    update.Target.Name,
                                                    Constants.NuGet.ContentFolder);
                        if (!contentPath.Exists)
                            return;

                        Logger.Debug("\t\tCopying Content files...");
                        contentPath.AllFiles().ForEach(source
                            =>
                                {
                                    var filePath = source.MakeRelativeTo(contentPath);
                                    var dest = Path.Get(context.Config.Command.InstallFolder,
                                        filePath.DirectoryName, filePath.FileName);

                                    if (dest.Exists && !resolution.Resolve(source.FullPath, dest.FullPath)) 
                                        return;

                                    source.Copy(dest, Overwrite.Always);
                                });
                        Logger.Debug("done!");
                    });
            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            
        }

        protected virtual IConflictResolutionAction GetConflictResolutionStrategy(ConflictResolutionTypes strategy)
        {
            switch (strategy)
            {
                case ConflictResolutionTypes.Ask:
                    return new CopyAskResolutionAction();
                case ConflictResolutionTypes.Overwrite:
                    return new CopyAlwaysResolutionAction();
                case ConflictResolutionTypes.Manual:
                    return new CopyNeverResolutionAction();
                default:
                    throw new NotSupportedException(string.Format("Conflict Resolution Type '{0}' is not supported",
                                                                  strategy));
            }
        }
    }
}