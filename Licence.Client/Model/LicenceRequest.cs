using System;
using System.Collections.Generic;

namespace Licence.Client.Model
{
    public class LicenceRequest
    {
        public string DeviceId { get; set; }
        public IEnumerable<string> Modules { get; set; }
        public IEnumerable<string> AditionalInfos { get; set; }

        public LicenceRequest()
        {
        }
    }
}
