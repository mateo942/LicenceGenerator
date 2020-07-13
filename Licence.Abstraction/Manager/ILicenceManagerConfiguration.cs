using System;
using System.Collections.Generic;

namespace Licence.Abstraction.Manager
{
    public interface ILicenceManagerConfiguration
    {
        string ApplicationName { get; set; }
        string Version { get; set; }

        IEnumerable<string> Modules { get; set; }
        IEnumerable<string> AdditionalInfos { get; set; }
    }
}
