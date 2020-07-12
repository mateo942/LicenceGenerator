using System;
using System.Threading.Tasks;

namespace Licence.Abstraction.Repository
{
    public interface ILicenceRepository
    {
        Task<Model.ILicenceData> Get(string deviceId);
        Task<Model.ILicenceData> Add(Model.ILicenceData licenceData);
    }
}
