using System;
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
            context.Config.TargetPackages.ForEach(target =>
                                                      {
                                                          Console.WriteLine("Checking {0} for update to {1} v{2}...",
                                                                            target.NuGetFeedUrl, target.Name,
                                                                            target.Version);

                                                          var repo =
                                                              PackageRepositoryFactory.Default.CreateRepository(
                                                                  target.NuGetFeedUrl);
                                                          var update = repo.FindPackage(target.Name);

                                                          if (update.Version <= target.Version)
                                                          {
                                                              Console.WriteLine(
                                                                  "\tNo update available...running the latest version!");
                                                          }
                                                          else
                                                          {
                                                              Console.WriteLine("\tUpdated version v{0} is available",
                                                                                update.Version);
                                                              context.Updates.Add(new UpdatedPackage
                                                                                      {
                                                                                          Target = target,
                                                                                          Package = update
                                                                                      });
                                                          }
                                                      });
            return (context.Updates.Count > 0);
        }

        public void ExitConditions(UpdaterContext context)
        {
        }
    }
}