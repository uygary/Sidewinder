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
            if (context.Package == null)
                throw new ArgumentException("Package property not set", "context");
            if (string.IsNullOrWhiteSpace(context.Config.DownloadFolder))
                throw new ArgumentException("Config.DownloadFolder property not set", "context");
        }

        public bool Execute(UpdaterContext context)
        {
            Console.WriteLine("\t\tDownloading package content to: {0}...", context.Config.DownloadFolder);
            Fluent.IO.Path.CreateDirectory(context.Config.DownloadFolder);
            var files = context.Package.GetFiles();

            files.ToList().ForEach(file =>
            {
                Console.WriteLine("\t\tDownloading file: {0}...", file.Path);
                DownloadFile(context, file);
            });

            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            
        }

        protected virtual void DownloadFile(UpdaterContext context, IPackageFile file)
        {
            using (var stream = file.GetStream())
            {
                var filename = Path.GetFileName(file.Path);
                var folder = Fluent.IO.Path.Get(context.Config.DownloadFolder).Combine(file.Path).Parent().FullPath;
                Directory.CreateDirectory(folder);

                using (var destination = File.Create(Path.Combine(folder, filename)))
                    stream.CopyTo(destination);
            }
        }

    }
}