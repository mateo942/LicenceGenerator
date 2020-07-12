using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licence.Abstraction.Model;
using Licence.Abstraction.Repository;
namespace Licence.Core.Repository
{
    public class LicenceInMemoryRepository : ILicenceRepository
    {
        private static readonly ISet<ILicenceData> _licences = new HashSet<ILicenceData>();

        public LicenceInMemoryRepository()
        {
        }

        public Task<ILicenceData> Add(ILicenceData licenceData)
        {
            if (licenceData.Key.Equals(Guid.Empty))
                licenceData.Key = Guid.NewGuid();

            _licences.Add(licenceData);

            return Task.FromResult(licenceData);
        }

        public Task<ILicenceData> Get(string deviceId)
        {
            var licence = _licences
                .FirstOrDefault(x => x.DeviceId == deviceId);

            if (licence == null)
                throw new KeyNotFoundException();

            return Task.FromResult(licence);
        }
    }
}
