using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class BaseSavableDataService<T> : BaseDataService where T : BaseEntity
    {
        private readonly SavableRepository<T> _savableRepository;

        public BaseSavableDataService(SavableRepository<T> savableRepository, PayPeriodRepository payPeriodRepository) : base(payPeriodRepository)
        {
            _savableRepository = savableRepository;
        }

        protected bool IsNewEntity(int? id)
        {
            // sometimes it's not int.MinValue
            return id == null || id <= 0;
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

        private async Task ValidateData(T entity)
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