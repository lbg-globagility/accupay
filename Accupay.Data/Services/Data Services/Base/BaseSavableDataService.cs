using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public abstract class BaseSavableDataService<T> : BaseDataService where T : BaseEntity
    {
        public enum SaveType
        {
            Insert,
            Update,
            Delete
        }

        protected readonly SavableRepository<T> _repository;
        protected readonly PayrollContext _context;
        protected readonly string EntityName;
        protected readonly string EntityNamePlural;

        public BaseSavableDataService(
            SavableRepository<T> repository,
            PayPeriodRepository payPeriodRepository,
            PayrollContext context,
            IPolicyHelper policy,
            string entityName,
            string entityNamePlural = null) :

            base(payPeriodRepository,
                policy)
        {
            _repository = repository;
            _context = context;

            EntityName = entityName;
            EntityNamePlural = entityNamePlural ?? entityName + "s";
        }

        #region Protected Methods

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

        protected async Task<ICollection<T>> GetOldEntitiesAsync(List<T> entities)
        {
            var updatedEntityIds = entities
                .Where(x => !x.IsNewEntity)
                .Select(x => x.RowID.Value)
                .Distinct()
                .ToArray();

            var oldEntities = await _repository.GetManyByIdsAsync(updatedEntityIds);
            return oldEntities;
        }

        protected T GetOldEntity(ICollection<T> oldEntities, T entity)
        {
            return oldEntities.FirstOrDefault(x => x.RowID == entity?.RowID);
        }

        #endregion Protected Methods

        #region Public Virtual Methods

        public async virtual Task DeleteAsync(int id, int currentlyLoggedInUserId)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                throw new BusinessLogicException($"{EntityName} does not exists.");

            await AdditionalDeleteValidation(entity);

            await _repository.DeleteAsync(entity);

            await PostDeleteAction(entity, currentlyLoggedInUserId);
        }

        /// <summary>
        /// Create or update the entity. If the entity has a RowID, it will be updated, otherwise it will be created.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<T> SaveAsync(T entity, int currentlyLoggedInUserId)
        {
            bool isNew = entity.IsNewEntity;

            T oldEntity = null;
            if (!isNew)
            {
                oldEntity = await _repository.GetByIdAsync(entity.RowID.Value);

                if (oldEntity == null)
                    throw new BusinessLogicException($"{EntityName} no longer exists.");
            }

            await SanitizeEntity(entity, oldEntity, currentlyLoggedInUserId);
            await AdditionalSaveValidation(entity, oldEntity);

            DetachOldEntity(oldEntity);
            await _repository.SaveAsync(entity);

            SaveType saveType = isNew ? SaveType.Insert : SaveType.Update;
            await PostSaveAction(entity, oldEntity, saveType);

            return entity;
        }

        public virtual async Task SaveManyAsync(List<T> entities, int currentlyLoggedInUserId)
        {
            if (entities == null)
                throw new BusinessLogicException($"No {EntityNamePlural} to be saved.");

            var insertEntities = entities.Where(x => x.IsNewEntity).ToList();
            var updateEntities = entities.Where(x => !x.IsNewEntity).ToList();

            await SaveManyAsync(
                currentlyLoggedInUserId,
                added: insertEntities,
                updated: updateEntities);
        }

        public virtual async Task SaveManyAsync(
            int currentlyLoggedInUserId,
            List<T> added = null,
            List<T> updated = null,
            List<T> deleted = null)
        {
            if (added == null && updated == null && deleted == null)
                throw new BusinessLogicException($"No {EntityNamePlural} to be saved.");

            var allEntities = new List<T>();
            if (added != null) allEntities.AddRange(added);
            if (updated != null) allEntities.AddRange(updated);
            if (deleted != null) allEntities.AddRange(deleted);

            ICollection<T> oldEntities = await ValidateMultipleEntities(
                currentlyLoggedInUserId,
                added: added,
                updated: updated,
                deleted: deleted);

            await CallAdditionalSaveManyValidation(added, oldEntities, SaveType.Insert);
            await CallAdditionalSaveManyValidation(updated, oldEntities, SaveType.Update);
            await CallAdditionalSaveManyValidation(deleted, oldEntities, SaveType.Delete);

            DetachOldEntities(oldEntities);

            await _repository.SaveManyAsync(
                added: added,
                updated: updated,
                deleted: deleted);

            await CallPostSaveManyAction(added, oldEntities, SaveType.Insert, currentlyLoggedInUserId);
            await CallPostSaveManyAction(updated, oldEntities, SaveType.Update, currentlyLoggedInUserId);
            await CallPostSaveManyAction(deleted, oldEntities, SaveType.Delete, currentlyLoggedInUserId);
        }

        #endregion Public Virtual Methods

        #region Protected Virtual Methods

        // TODO: change this to a synchronus method. All validations that needs database
        // operations (they usually need async methods) should be moved to SaveValidation methods
        protected virtual Task SanitizeEntity(T entity, T oldEntity, int currentlyLoggedInUserId)
        {
            if (entity == null)
                throw new BusinessLogicException($"Invalid {EntityName}.");

            if (entity.IsNewEntity && oldEntity != null)
                throw new BusinessLogicException("Your data is no longer up to date. Please refresh the form/page.");

            return Task.CompletedTask;
        }

        protected virtual Task AdditionalDeleteValidation(T entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task AdditionalSaveValidation(T entity, T oldEntity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task AdditionalSaveManyValidation(List<T> entities, List<T> oldEntities, SaveType saveType)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostDeleteAction(T entity, int currentlyLoggedInUserId)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostSaveAction(T entity, T oldEntity, SaveType saveType)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostSaveManyAction(IReadOnlyCollection<T> entities, IReadOnlyCollection<T> oldEntities, SaveType saveType, int currentlyLoggedInUserId)
        {
            return Task.CompletedTask;
        }

        #endregion Protected Virtual Methods

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
            if (oldEntities != null && oldEntities.Any())
            {
                foreach (var oldEntity in oldEntities)
                {
                    DetachOldEntity(oldEntity);
                }
            }
        }

        private async Task<ICollection<T>> ValidateMultipleEntities(
            int currentlyLoggedInUserId,
            List<T> added,
            List<T> updated,
            List<T> deleted)
        {
            var allEntities = new List<T>();
            if (added != null) allEntities.AddRange(added);
            if (updated != null) allEntities.AddRange(updated);
            if (deleted != null) allEntities.AddRange(deleted);

            ICollection<T> oldEntities = await GetOldEntitiesAsync(allEntities);

            if (added != null && added.Any())
            {
                foreach (var entity in added)
                {
                    T oldEntity = GetOldEntity(oldEntities, entity);
                    await SanitizeEntity(entity, oldEntity, currentlyLoggedInUserId);
                }
            }

            if (updated != null && updated.Any())
            {
                foreach (var entity in updated)
                {
                    var oldEntity = GetOldEntity(oldEntities, entity);

                    if (oldEntity == null)
                        throw new BusinessLogicException($"One of the {EntityNamePlural} no longer exists.");

                    await SanitizeEntity(entity, oldEntity, currentlyLoggedInUserId);
                }
            }

            if (deleted != null && deleted.Any())
            {
                foreach (var entity in deleted)
                {
                    if (entity == null)
                        throw new BusinessLogicException("Invalid data.");

                    var oldEntity = oldEntities.FirstOrDefault(x => x.RowID == entity.RowID);

                    if (oldEntity == null)
                        throw new BusinessLogicException($"One of the {EntityNamePlural} no longer exists.");
                }
            }

            return oldEntities;
        }

        private static List<T> GetOldEntitiesOfPassedEntities(List<T> entities, ICollection<T> oldEntities)
        {
            var entityIds = entities.Select(a => a.RowID);

            return oldEntities.Where(x => entityIds.Contains(x.RowID)).ToList();
        }

        private async Task CallAdditionalSaveManyValidation(List<T> entities, ICollection<T> oldEntities, SaveType saveType)
        {
            if (entities != null && entities.Any())
                await AdditionalSaveManyValidation(entities, GetOldEntitiesOfPassedEntities(entities, oldEntities), saveType);
        }

        private async Task CallPostSaveManyAction(List<T> savedEntities, ICollection<T> oldEntities, SaveType saveType, int currentlyLoggedInUserId)
        {
            if (savedEntities != null && savedEntities.Any())
                await PostSaveManyAction(savedEntities, GetOldEntitiesOfPassedEntities(savedEntities, oldEntities), saveType, currentlyLoggedInUserId);
        }

        #endregion Private Methods
    }
}
