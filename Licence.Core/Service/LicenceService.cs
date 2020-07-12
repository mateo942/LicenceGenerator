using System;
using Licence.Abstraction.Handler;
using Licence.Abstraction.Model;
using Licence.Abstraction.Repository;
namespace Licence.Core.Service
{
    public class LicenceService
    {
        private readonly ILicenceRepository _licenceRepository;
        private readonly ILicenceHandler _licenceHandler;

        public LicenceService(ILicenceRepository licenceRepository, ILicenceHandler licenceHandler)
        {
            _licenceRepository = licenceRepository;
            _licenceHandler = licenceHandler;
        }

        public ILicenceResult Create(ILicenceData licenceData)
        {
            var result = _licenceHandler.Export(licenceData);

            _licenceRepository.Add(licenceData);

            return result;
        }
    }
}
