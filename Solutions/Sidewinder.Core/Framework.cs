using System;
using System.Collections.Generic;
using Fluent.IO;

namespace Sidewinder.Core
{
    /// <summary>
    /// Provides helper methods for matching assembly versions to NuGet framework (lib) folders
    /// </summary>
    public class Framework
    {
        public static LinkedList<string> SupportedLibFrameworks = new LinkedList<string>(
            new[]
                {
                    "net40-full",
                    "net4-full",
                    "net40-client",
                    "net4-client",
                    "net40",
                    "net4",
                    "40",
                    "4",
                    // framework 2.0
                    "net20-full",
                    "net2-full",
                    "net20-client",
                    "net2-client",
                    "net20",
                    "net2",
                    "20",
                    "2",
                    // framework 1.1
                    "net11-full",
                    "net1-full",
                    "net11-client",
                    "net1-client",
                    "net11",
                    "net1",
                    "11",
                    "1",
                });


        public static string GetBestLibFrameworkFolder(Version version)
        {
            if (version.Major >= 4)
                return "net40-full";
            if (version.Major >= 2)
                return "net20-full";
            if (version.Major >= 1)
                return "net11-full";
            return null;
        }

        public static Version GetPreviousFrameworkVersion(Version version)
        {
            if (version.Major >= 4)
                return new Version(2,0);
            if (version.Major >= 2)
                return new Version(1,0);
            return null;
        }

        public static Path GetLibFrameworkPath(string downloadFolder, string packageName, Version targetFramework)
        {            
            Path frameworkPath;
            // gets the best match for this version number
            var frameworkFolder = GetBestLibFrameworkFolder(targetFramework);

            do
            {                
                frameworkPath = Path.Get(downloadFolder,
                                       packageName,
                                       Constants.NuGet.LibFolder,
                                       frameworkFolder);

                if (frameworkPath.Exists)
                    break;

                // get the next value to try
                var node = SupportedLibFrameworks.Find(frameworkFolder);
                if (node == null)
                    throw new InvalidOperationException(string.Format("The lib\framework '{0}' is not supported!", frameworkFolder));

                var next = node.Next;
                if (next == null)
                {
                    // run out of options, just return the root lib folder
                    frameworkPath = Path.Get(downloadFolder,
                                             packageName,
                                             Constants.NuGet.LibFolder);
                    break;
                }

                frameworkFolder = next.Value;
            } while (true);

            return frameworkPath;
        }
    }
}