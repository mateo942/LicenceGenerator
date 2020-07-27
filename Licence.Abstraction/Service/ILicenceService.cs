using System;
using System.Threading.Tasks;
using Licence.Abstraction.Model;

namespace Licence.Abstraction.Service
{
    public interface ILicenceService
    {
        Task<ILicenceEntity> Create(Guid keyId, ILicenceData licenceData);
    }
}
