using System;
using System.Collections.Generic;

namespace Licence.Abstraction.Model
{
    public interface ILicenceData
    {
        string DeviceId { get; set; }
        Guid Key { get; set; }

        DateTime CreatedAtUtc { get; set; }
        DateTime? ExpiredAt { get; set; }

        string ProjectName { get; set; }

        IEnumerable<string> Modules { get; set; }
        IDictionary<string, string> AditionalInfos { get; set; }
    }
}
