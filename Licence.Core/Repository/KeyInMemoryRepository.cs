using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licence.Abstraction.Model;
using Licence.Abstraction.Repository;

namespace Licence.Core.Repository
{
    public class KeyInMemoryRepository : IKeyRepository
    {
        private static readonly ISet<IKeyEntity> _keyEntities = new HashSet<IKeyEntity>();

        public KeyInMemoryRepository()
        {
        }

        public Task<IKeyEntity> Add(IKeyEntity entity)
        {
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            _keyEntities.Add(entity);

            return Task.FromResult(entity);
        }

        public Task Delete(Guid id)
        {
            var key = _keyEntities.SingleOrDefault(x => x.Id == id);
            _keyEntities.Remove(key);

            return Task.CompletedTask;
        }

        public Task<IKeyEntity> Get(Guid id)
        {
            var key = _keyEntities.SingleOrDefault(x => x.Id == id);

            return Task.FromResult(key);
        }
    }
}
