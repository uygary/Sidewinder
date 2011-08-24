using System;
using Fluent.IO;

namespace Sidewinder
{
    /// <summary>
    /// Provides helper methods for matching assembly versions to NuGet framework (lib) folders
    /// </summary>
    public class Framework
    {
        public static string GetLibFrameworkFolder(Version version)
        {
            if (version.Major >= 4)
                return "net40";
            if (version.Major >= 2)
                return "net20";
            if (version.Major >= 1)
                return "net11";
            return string.Empty;
        }

        public static Version GetPreviousFrameworkVersion(Version version)
        {
            if (version.Major >= 4)
                return new Version(2,0);
            if (version.Major >= 2)
                return new Version(1,0);
            return null;
        }

        public static string RemoveFrameworkAbbreviation(string version)
        {
            return version.ToLower().Replace("net", string.Empty);
        }

        public static Path GetLibFrameworkPath(string downloadFolder, string packageName, Version targetFramework)
        {            
            Path frameworkPath;

            do
            {
                var frameworkFolder = GetLibFrameworkFolder(targetFramework);
                frameworkPath = Path.Get(downloadFolder,
                                       packageName,
                                       Constants.NuGet.LibFolder,
                                       frameworkFolder);

                if (!frameworkPath.Exists)
                {
                    // try without the framework abbrev.
                    frameworkPath = Path.Get(downloadFolder,
                                       packageName,
                                       Constants.NuGet.LibFolder,
                                       RemoveFrameworkAbbreviation(frameworkFolder));

                    if (!frameworkPath.Exists)
                    {
                        // try previous version
                        targetFramework = GetPreviousFrameworkVersion(targetFramework);

                        if (targetFramework == null)
                        {
                            // run out of versions, just return the root lib folder
                            return Path.Get(downloadFolder,
                                            packageName,
                                            Constants.NuGet.LibFolder);
                        }
                    }
                }
            } while (!frameworkPath.Exists);

            return frameworkPath;
        }
    }
}