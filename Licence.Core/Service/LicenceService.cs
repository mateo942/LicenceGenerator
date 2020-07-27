using System;
using System.Threading.Tasks;
using Licence.Abstraction.Handler;
using Licence.Abstraction.Model;
using Licence.Abstraction.Repository;
using Licence.Abstraction.Service;
using Licence.Core.Models;

namespace Licence.Core.Service
{
    public class LicenceService : ILicenceService
    {
        private readonly ILicenceRepository _licenceRepository;
        private readonly ILicenceHandler _licenceHandler;

        public LicenceService(ILicenceRepository licenceRepository, ILicenceHandler licenceHandler)
        {
            _licenceRepository = licenceRepository;
            _licenceHandler = licenceHandler;
        }

        public async Task<ILicenceEntity> Create(Guid keyId, ILicenceData licenceData)
        {
            var result = await _licenceHandler.Export(licenceData, keyId);

            var licence = await _licenceRepository.Add(new LicenceEntity
            {
                KeyId = keyId,
                LicenceData = licenceData,
                LicenceString = result
            });

            return licence;
        }
    }
}
