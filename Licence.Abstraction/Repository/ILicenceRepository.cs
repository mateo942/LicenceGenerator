using System;
using System.Threading.Tasks;

namespace Licence.Abstraction.Repository
{
    public interface ILicenceRepository
    {
        Task<Model.ILicenceEntity> Get(Guid id);
        Task<Model.ILicenceEntity> Add(Model.ILicenceEntity licence);
    }
}
