using System;
using System.Diagnostics;

namespace Sidewinder.Core.Interfaces.Entities
{
    [DebuggerDisplay("{Target.Name}, {NewVersion}")]
    public class UpdatedPackage
    {
        public TargetPackage Target { get; set; }
        public Version NewVersion { get; set; }
    }
}