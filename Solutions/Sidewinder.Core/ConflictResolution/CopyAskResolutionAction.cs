using System;
using Sidewinder.Core.Interfaces;

namespace Sidewinder.Core.ConflictResolution
{
    public class CopyAskResolutionAction : IConflictResolutionAction
    {
        public bool Resolve(string source, string dest)
        {
            Console.Write("Overwrite {0}? (Y/N)", source);

            var key = Convert.ToChar(Console.In.Read());

            switch (key)
            {
                case 'Y':
                case 'y':
                    return true;
                default:
                    return false;
            }
        }
    }
}