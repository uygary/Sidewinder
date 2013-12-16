using System;
using System.Collections.Generic;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Interfaces.Exceptions
{
    public class PackageBuildException : ApplicationException
    {
        private PackageBuilderConfig myConfig;
        private IEnumerable<string> myErrors;

        public PackageBuildException(PackageBuilderConfig config, IEnumerable<string> errors)
        {
            myConfig = config;
            myErrors = errors;
        }
    }
}