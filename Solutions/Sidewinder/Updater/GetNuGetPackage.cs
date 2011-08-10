using System;
using System.Reflection;
using NuGet;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;

namespace Sidewinder.Updater
{
    public class GetNuGetPackage : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.Config == null)
                throw new ArgumentException("Config property is null", "context");
            if (string.IsNullOrWhiteSpace(context.Config.NuGetFeedUrl))
                throw new ArgumentException("Config.NuGetFeedUrl property not set", "context");
            if (string.IsNullOrWhiteSpace(context.Config.TargetPackage))
                throw new ArgumentException("Config.TargetPackage property not set", "context");
        }

        public bool Execute(UpdaterContext context)
        {
            // no command file - just check for update
            context.CurrentVersion = Assembly.GetEntryAssembly().GetName().Version;

            Console.WriteLine("Checking for updates to v{0}...", context.CurrentVersion);
            var repo = PackageRepositoryFactory.Default.CreateRepository(context.Config.NuGetFeedUrl);
            var package = repo.FindPackage(context.Config.TargetPackage);

            if (package.Version <= context.CurrentVersion)
            {
                Console.WriteLine("\tNo update available...running the latest version!");
                return false;
            }

            context.Package = package;
            return true;
        }

        public void ExitConditions(UpdaterContext context)
        {
            if (context.CurrentVersion == null)
                throw new ArgumentException("CurrentVersion property not set", "context");
            if (context.Package == null)
                throw new ArgumentException("Package property not set", "context");
        }
    }
}