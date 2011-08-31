﻿using System;
using Fluent.IO;
using Sidewinder.Core.Interfaces;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Distributor
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
                throw new ArgumentException("Config.Command.DownloadFolder property not set", "context");
            if (string.IsNullOrWhiteSpace(context.Config.Command.InstallFolder))
                throw new ArgumentException("Config.Command.InstallFolder property not set", "context");
        }

        public bool Execute(DistributorContext context)
        {
            var updates = context.Config.Command.Updates;

            updates.ForEach(update =>
                                {
                                    Console.WriteLine("\tProcessing update package {0}->{1}...", 
                                        update.Target.Name,
                                        context.Config.Command.InstallFolder);

                                    // create it just in case
                                    Path.Get(context.Config.Command.InstallFolder).CreateDirectory();

                                    // copy the content files
                                    var contentPath = Path.Get(context.Config.Command.DownloadFolder,
                                                           update.Target.Name,
                                                           Constants.NuGet.ContentFolder);
                                    if (contentPath.Exists)
                                    {
                                        Console.Write("\t\tCopying Content files...");
                                        contentPath.Copy(context.Config.Command.InstallFolder, Overwrite.Never, true);
                                        Console.WriteLine("done!");
                                    }

                                    // copy binaries - this will find the best match lib\framework folder within the
                                    // package matched against the target framework requested
                                    var binPath = NuGetHelper.GetLibFrameworkPath(context.Config.Command.DownloadFolder,
                                                                                update.Target.Name,
                                                                                context.Config.Command.TargetFrameworkVersion);

                                    Console.Write("\t\tCopying Binaries from {0}...", binPath.FullPath);
                                    binPath.Copy(context.Config.Command.InstallFolder, Overwrite.Always, true);
                                    Console.WriteLine("done!");

                                    // copy tools
                                    var toolsPath = Path.Get(context.Config.Command.DownloadFolder,
                                                           update.Target.Name,
                                                           Constants.NuGet.ToolsFolder);
                                    if (toolsPath.Exists)
                                    {
                                        Console.Write("\t\tCopying Tool files...");
                                        toolsPath.Copy(context.Config.Command.InstallFolder, Overwrite.Always, true);
                                        Console.WriteLine("done!");
                                    }
                                });
            return true;
        }

        public void ExitConditions(DistributorContext context)
        {
            
        }
    }
}