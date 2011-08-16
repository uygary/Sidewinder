using System;
using System.IO;
using System.Linq;
using NuGet;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Updater
{
    public class DownloadPackageFiles : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.Config == null)
                throw new ArgumentException("Config property is null", "context");
        }

        public bool Execute(UpdaterContext context)
        {
            context.Updates.ForEach(update =>
                                        {
                                            var downloadFolder = Fluent.IO.Path.Get(context.Config.DownloadFolder,
                                                                                    update.Target.Name).FullPath;
                                            Console.WriteLine("Downloading package '{0}' content to: {1}...", 
                                                update.Target.Name,
                                                downloadFolder);
                                            Fluent.IO.Path.CreateDirectory(downloadFolder);
                                            var files = update.Package.GetFiles();

                                            files.ToList().ForEach(file =>
                                            {
                                                Console.WriteLine("\t{0}", file.Path);
                                                DownloadFile(context, file);
                                            });                                            
                                        });
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }

    }
}