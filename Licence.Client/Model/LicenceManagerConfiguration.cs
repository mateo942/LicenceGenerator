using System;
using System.Collections.Generic;
using Licence.Abstraction.Manager;

namespace Licence.Client.Model
{
    public class LicenceManagerConfiguration : ILicenceManagerConfiguration
    {
        public string ApplicationName { get; set; }

        public string Version { get; set; }

        public IEnumerable<string> Modules { get; set; }

        public IEnumerable<string> AdditionalInfos { get; set; }
    }
}
