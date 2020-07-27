using System;
namespace Licence.Abstraction.Model
{
    public interface ILicenceEntity
    {
        public Guid Id { get; set; }
        public Guid KeyId { get; set; }
        public ILicenceData LicenceData { get; set; }
        public string LicenceString { get; set; }
    }
}
