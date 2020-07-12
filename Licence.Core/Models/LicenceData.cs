using System;
using System.Collections.Generic;
using Licence.Abstraction.Model;

namespace Licence.Core.Models
{
    public class LicenceData : ILicenceData
    {
        public string DeviceId { get; set; }
        public Guid Key { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ExpiredAt { get; set; }

        public string ProjectName { get; set; }

        public IEnumerable<string> Modules { get; set; }
        public IDictionary<string, string> AditionalInfos { get; set; }
    }
}
