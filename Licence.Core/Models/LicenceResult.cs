using System;
using Licence.Abstraction.Model;

namespace Licence.Core.Models
{
    public class LicenceResult : ILicenceResult
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }

        public string Licence { get; set; }

        internal LicenceResult() { }
    }
}
