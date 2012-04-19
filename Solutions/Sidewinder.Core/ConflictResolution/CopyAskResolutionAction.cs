using System;
using Sidewinder.Core.Interfaces;

namespace Sidewinder.Core.ConflictResolution
{
    public class CopyAskResolutionAction : IConflictResolutionAction
    {
        public bool Resolve(string source, string dest)
        {
            Console.WriteLine();
            
            while (true)
            {
                Console.Write("Overwrite {0}? (Y/N)", dest);
                var response = Console.In.ReadLine();

                if (response == "y") return true;
                if (response == "Y") return true;
                if (response == "n") return false;
                if (response == "N") return false;
            }
        }
    }
}