using System;
using Licence.Abstraction.Model;

namespace Licence.Core.Models
{
    public class LicenceEntity : ILicenceEntity
    {
        public Guid Id { get; set; }
        public Guid KeyId { get; set; }
        public ILicenceData LicenceData { get; set; }
        public string LicenceString { get; set; }
    }
}
