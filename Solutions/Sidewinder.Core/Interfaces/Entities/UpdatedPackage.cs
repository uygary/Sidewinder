using System;

namespace Sidewinder.Core.Interfaces.Entities
{
    public class UpdatedPackage
    {
        public TargetPackage Target { get; set; }
        public Version NewVersion { get; set; }
    }
}