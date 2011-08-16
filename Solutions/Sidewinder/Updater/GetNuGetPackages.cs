using System;
using System.IO;
using System.Linq;
using NuGet;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Updater
{
    public class GetNuGetPackages : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.Config == null)
                throw new ArgumentException("Config property is null", "context");
            if (context.Config.TargetPackages == null)
                throw new ArgumentException("Config.TargetPackages property is null", "context");
        }

        public bool Execute(UpdaterContext context)
        {
            var targets = context.Config.TargetPackages.ToList();

            targets.ForEach(target =>
                                {
                                    if (target.Value.Version == null)
                                    {
                                        Console.WriteLine("\tGetting the latest version of {0} from {1}...",
                                                          target.Value.Name,
                                                          target.Value.NuGetFeedUrl);
                                    }
                                    else
                                    {
                                        Console.WriteLine("\tChecking {0} for update to {1} v{2}...",
                                                          target.Value.NuGetFeedUrl, target.Value.Name,
                                                          target.Value.Version);
                                    }

                                    var repo = PackageRepositoryFactory.Default.CreateRepository(target.Value.NuGetFeedUrl);
                                    var update = repo.FindPackage(target.Value.Name);

                                    if (update == null)
                                    {
                                        Console.WriteLine("\t\t**WARNING** Package {0} does not exist on feed {1}",
                                            target.Value.Name,
                                            target.Value.NuGetFeedUrl);
                                        return;
                                    }

                                    if ((target.Value.Version != null) && 
                                        (update.Version <= target.Value.Version))
                                    {
                                        Console.WriteLine("\t\tNo update available...running the latest version!");
                                        return;
                                    }

                                    Console.WriteLine("\t\tUpdated version v{0} is available", update.Version);

                                    var downloadFolder = Fluent.IO.Path.Get(context.Config.DownloadFolder, target.Value.Name).FullPath;
                                    Console.WriteLine("\tDownloading package '{0}' content to: {1}...", target.Value.Name, downloadFolder);
                                    
                                    Fluent.IO.Path.CreateDirectory(downloadFolder);
                                    var files = update.GetFiles();
                                    files.ToList().ForEach(file =>
                                                               {
                                                                   Console.WriteLine("\t\t{0}", file.Path);
                                                                   DownloadFile(downloadFolder, file);
                                                               });

                                    // add to list of updates (as long as it's not sidewinder itself)
                                    if (string.Compare(update.Id, Constants.Sidewinder.NuGetPackageName,
                                        StringComparison.InvariantCultureIgnoreCase) != 0)
                                    {
                                        context.Updates.Add(new UpdatedPackage
                                                                {
                                                                    NewVersion = update.Version,
                                                                    Target = target.Value
                                                                });
                                    }
                                });

            return (context.Updates.Count > 0);
        }

        public void ExitConditions(UpdaterContext context)
        {
        }


        protected virtual void DownloadFile(string downloadFolder, IPackageFile file)
        {
            using (var stream = file.GetStream())
            {
                var filename = Path.GetFileName(file.Path);
                var folder = Fluent.IO.Path.Get(downloadFolder).Combine(file.Path).Parent().FullPath;
                Directory.CreateDirectory(folder);

                using (var destination = File.Create(Path.Combine(folder, filename)))
                    stream.CopyTo(destination);
            }
        }

    }
}