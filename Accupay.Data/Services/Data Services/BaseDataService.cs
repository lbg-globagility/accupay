using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class BaseDataService<T> where T : BaseEntity
    {
        private readonly SavableRepository<T> _repository;

        public BaseDataService(SavableRepository<T> repository)
        {
            _repository = repository;
        }

        protected bool IsNewEntity(int? id)
        {
            // sometimes it's not int.MinValue
            return id == null || id <= 0;
        }

        public virtual async Task SaveAsync(T entity)
        {
            await SanitizeEntity(entity);

            await _repository.SaveAsync(entity);
        }

        public virtual async Task SaveManyAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await SanitizeEntity(entity);
            }

            await _repository.SaveManyAsync(entities);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        protected virtual async Task SanitizeEntity(T entity)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }
    }
}