using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class BaseSavableDataService<T> : BaseDataService where T : BaseEntity
    {
        protected readonly SavableRepository<T> _savableRepository;

        protected readonly string EntityDoesNotExistOnDeleteErrorMessage = "Entity does not exists.";

        public BaseSavableDataService(
            SavableRepository<T> savableRepository,
            PayPeriodRepository payPeriodRepository,
            string entityDoesNotExistOnDeleteErrorMessage) : base(payPeriodRepository)
        {
            _savableRepository = savableRepository;

            EntityDoesNotExistOnDeleteErrorMessage = string.IsNullOrWhiteSpace(entityDoesNotExistOnDeleteErrorMessage) ?
                "Entity does not exists." :
                entityDoesNotExistOnDeleteErrorMessage;
        }

        protected bool IsNewEntity(int? id)
        {
            // sometimes it's not int.MinValue
            return id == null || id <= 0;
        }

        public async virtual Task DeleteAsync(int id)
        {
            var entity = await _savableRepository.GetByIdAsync(id);

            if (entity == null)
                throw new BusinessLogicException(EntityDoesNotExistOnDeleteErrorMessage);

            await _savableRepository.DeleteAsync(entity);
        }

        public virtual async Task SaveAsync(T entity)
        {
            await ValidateData(entity);

            await _savableRepository.SaveAsync(entity);
        }

        public virtual async Task SaveManyAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await ValidateData(entity);
            }

            await _savableRepository.SaveManyAsync(entities);
        }

        protected async Task ValidateData(T entity)
        {
            if (entity == null)
                throw new BusinessLogicException("Invalid data.");

            await SanitizeEntity(entity);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        protected virtual async Task SanitizeEntity(T entity)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }
    }
}