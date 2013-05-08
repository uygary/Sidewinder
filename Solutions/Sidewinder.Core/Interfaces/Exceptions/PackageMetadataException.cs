using System;
using System.Collections.Generic;
using Sidewinder.Core.Interfaces.Entities;

namespace Sidewinder.Core.Interfaces.Exceptions
{
    public class PackageMetadataException : ApplicationException
    {
        private PackageMetadataConfig myConfig;
        private IEnumerable<string> myErrors;

        public PackageMetadataException(PackageMetadataConfig config, IEnumerable<string> errors)
        {
            myConfig = config;
            myErrors = errors;
        }
    }
}