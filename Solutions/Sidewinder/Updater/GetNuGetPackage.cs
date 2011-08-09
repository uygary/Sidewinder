using System;
using System.Reflection;
using NuGet;
using Sidewinder.Interfaces;
using Sidewinder.Interfaces.Entities;
using FluentAssertions;

namespace Sidewinder.Updater
{
    public class GetNuGetPackage : IPipelineStep<UpdaterContext>
    {
        public void EntryConditions(UpdaterContext context)
        {
            context.Config.Should().NotBeNull();
            context.Config.NuGetFeedUrl.Should().NotBeNullOrEmpty();
            context.Config.TargetPackage.Should().NotBeNullOrEmpty();
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
            context.CurrentVersion.Should().NotBeNull();
            context.Package.Should().NotBeNull();
        }
    }
}