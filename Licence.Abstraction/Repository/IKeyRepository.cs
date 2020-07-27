using System;
using System.Threading.Tasks;
using Licence.Abstraction.Model;

namespace Licence.Abstraction.Repository
{
    public interface IKeyRepository
    {
        Task<IKeyEntity> Get(Guid id);
        Task<IKeyEntity> Add(IKeyEntity entity);
        Task Delete(Guid id);
    }
}
