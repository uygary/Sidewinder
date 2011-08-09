using System;
using NuGet;

namespace Sidewinder.Interfaces.Entities
{
    public class UpdaterContext
    {
        public Version CurrentVersion { get; set; }
        public IPackage Package { get; set; }
        public UpdateConfig Config { get; set; }
    }
}