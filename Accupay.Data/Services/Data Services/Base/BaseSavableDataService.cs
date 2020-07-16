using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class BaseSavableDataService<T> : BaseDataService where T : BaseEntity
    {
        protected readonly SavableRepository<T> _repository;
        protected readonly PayrollContext _context;
        protected readonly string EntityDoesNotExistOnDeleteErrorMessage = "Entity does not exists.";

        public BaseSavableDataService(
            SavableRepository<T> repository,
            PayPeriodRepository payPeriodRepository,
            PayrollContext context,
            PolicyHelper policy,
            string entityDoesNotExistOnDeleteErrorMessage) :

            base(payPeriodRepository,
                policy)
        {
            _repository = repository;
            _context = context;

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
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                throw new BusinessLogicException(EntityDoesNotExistOnDeleteErrorMessage);

            await AdditionalDeleteValidation(entity);

            await _repository.DeleteAsync(entity);
        }

        public virtual async Task SaveAsync(T entity)
        {
            T oldEntity = null;
            if (!IsNewEntity(entity.RowID))
            {
                oldEntity = await _repository.GetByIdAsync(entity.RowID.Value);
            }

            await ValidateData(entity, oldEntity);
            await AdditionalSaveValidation(entity, oldEntity);

            DetachOldEntity(oldEntity);
            await _repository.SaveAsync(entity);
        }

        public virtual async Task SaveManyAsync(List<T> entities)
        {
            ICollection<T> oldEntities = await GetOldEntitiesAsync(entities);

            foreach (var entity in entities)
            {
                var oldEntity = oldEntities.FirstOrDefault(x => x.RowID == entity.RowID);
                await ValidateData(entity, oldEntity);
            }

            await AdditionalSaveManyValidation(entities, oldEntities.ToList());

            DetachOldEntities(oldEntities);
            await _repository.SaveManyAsync(entities);
        }

        protected async Task<ICollection<T>> GetOldEntitiesAsync(List<T> entities)
        {
            var updatedEntityIds = entities
                .Where(x => !IsNewEntity(x.RowID))
                .Select(x => x.RowID.Value)
                .Distinct()
                .ToArray();

            var oldEntities = await _repository.GetManyByIdsAsync(updatedEntityIds);
            return oldEntities;
        }

        protected async Task ValidateData(T entity, T oldEntity)
        {
            if (entity == null)
                throw new BusinessLogicException("Invalid data.");

            await SanitizeEntity(entity, oldEntity);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        protected virtual async Task SanitizeEntity(T entity, T oldEntity)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        protected virtual async Task AdditionalDeleteValidation(T entity)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        protected virtual async Task AdditionalSaveValidation(T entity, T oldEntity)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        protected virtual async Task AdditionalSaveManyValidation(List<T> entities, List<T> oldEntities)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

        protected int? ValidateOrganization(int? currentOrganizationId, int? entityOrganizationId)
        {
            if (currentOrganizationId == null)
            {
                currentOrganizationId = entityOrganizationId;
            }
            else
            {
                if (currentOrganizationId != entityOrganizationId)
                    throw new BusinessLogicException("Cannot save multiple data from different organizations.");
            }

            return currentOrganizationId;
        }

        #region Private Methods

        private void DetachOldEntity(T oldEntity)
        {
            if (oldEntity != null)
            {
                _context.Entry(oldEntity).State = EntityState.Detached;
            }
        }

        private void DetachOldEntities(ICollection<T> oldEntities)
        {
            if (oldEntities != null && oldEntities.Count > 0)
            {
                foreach (var oldEntity in oldEntities)
                {
                    DetachOldEntity(oldEntity);
                }
            }
        }

        #endregion Private Methods
    }
}