
using System;
using Fluent.IO;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Distributor
{
    /// <summary>
    /// This step will copy the files from each nuget package update
    /// </summary>
    public class CopyPackageFiles : IPipelineStep<DistributorContext>
    {
        public void EntryConditions(DistributorContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.Config == null)
                throw new ArgumentException("Config property is null", "context");
            if (string.IsNullOrWhiteSpace(context.Config.Command.DownloadFolder))
                throw new ArgumentException("Config.Package.DownloadFolder property not set", "context");
            if (string.IsNullOrWhiteSpace(context.Config.InstallFolder))
                throw new ArgumentException("Config.InstallFolder property not set", "context");
        }

        public bool Execute(DistributorContext context)
        {
            var updates = context.Config.Command.Updates;

            updates.ForEach(update =>
                                {
                                    Console.WriteLine("\tProcessing package {0} -> {1}...", 
                                        update.Target.Name,
                                        context.Config.InstallFolder);

                                    // create it just in case
                                    Path.Get(context.Config.InstallFolder).CreateDirectory();

                                    // copy the content files
                                    var contentPath = Path.Get(context.Config.Command.DownloadFolder,
                                                           update.Target.Name,
                                                           Constants.NuGet.ContentFolder);
                                    if (contentPath.Exists)
                                    {
                                        Console.WriteLine("\t\tCopying Content files...");
                                        contentPath.Copy(context.Config.InstallFolder, Overwrite.Never, true);
                                    }

                                    // copy binaries
                                    var binPath = Path.Get(context.Config.Command.DownloadFolder,
                                                           update.Target.Name,
                                                           Constants.NuGet.LibFolder,
                                                           update.Target.FrameworkHint);

                                    if (binPath.Exists)
                                    {
                                        Console.WriteLine("\t\tCopying Binaries from {0}...", binPath.FullPath);
                                        binPath.Copy(context.Config.InstallFolder, Overwrite.Always, true);
                                    }

                                    // copy tools
                                    var toolsPath = Path.Get(context.Config.Command.DownloadFolder,
                                                           update.Target.Name,
                                                           Constants.NuGet.ToolsFolder);
                                    if (toolsPath.Exists)
                                    {
                                        Console.WriteLine("\t\tCopying Tool files...");
                                        toolsPath.Copy(context.Config.InstallFolder, Overwrite.Always, true);
                                    }
                                });
            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            
        }
    }
}