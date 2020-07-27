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
        private static readonly ISet<ILicenceEntity> _licences = new HashSet<ILicenceEntity>();

        public LicenceInMemoryRepository()
        {
        }

        public Task<ILicenceEntity> Add(ILicenceEntity licenceData)
        {
            if (licenceData.Id == Guid.Empty)
                licenceData.Id = Guid.NewGuid();

            _licences.Add(licenceData);

            return Task.FromResult(licenceData);
        }

        public Task<ILicenceEntity> Get(Guid id)
        {
            var licence = _licences
                .SingleOrDefault(x => x.Id == id);

            if (licence == null)
                throw new KeyNotFoundException();

            return Task.FromResult(licence);
        }
    }
}
