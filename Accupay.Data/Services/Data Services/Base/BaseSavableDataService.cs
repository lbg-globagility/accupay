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
            PolicyHelper policy,
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

        #endregion Protected Methods

        #region Virtual Methods

        public async virtual Task DeleteAsync(int id, int changedByUserId)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                throw new BusinessLogicException($"{EntityName} does not exists.");

            await AdditionalDeleteValidation(entity);

            await _repository.DeleteAsync(entity);

            await PostDeleteAction(entity, changedByUserId);
        }

        /// <summary>
        /// Create or update the entity. If the entity has a RowID, it will be updated, otherwise it will be created.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task SaveAsync(T entity)
        {
            bool isNew = entity.IsNewEntity;

            T oldEntity = null;
            if (!isNew)
            {
                oldEntity = await _repository.GetByIdAsync(entity.RowID.Value);

                if (oldEntity == null)
                    throw new BusinessLogicException($"{EntityName} no longer exists.");
            }

            await ValidateData(entity, oldEntity);
            await AdditionalSaveValidation(entity, oldEntity);

            DetachOldEntity(oldEntity);
            await _repository.SaveAsync(entity);

            SaveType saveType = isNew ? SaveType.Insert : SaveType.Update;
            await PostSaveAction(entity, oldEntity, saveType);
        }

        public virtual async Task SaveManyAsync(List<T> entities)
        {
            if (entities == null)
                throw new BusinessLogicException($"No {EntityNamePlural} to be saved.");

            var insertEntities = entities.Where(x => x.IsNewEntity).ToList();
            var updateEntities = entities.Where(x => !x.IsNewEntity).ToList();

            await SaveManyAsync(added: insertEntities, updated: updateEntities);
        }

        public virtual async Task SaveManyAsync(
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

            await CallPostSaveManyAction(added, oldEntities, SaveType.Insert);
            await CallPostSaveManyAction(updated, oldEntities, SaveType.Update);
            await CallPostSaveManyAction(deleted, oldEntities, SaveType.Delete);
        }

        // TODO: change this to a synchronus method. All validations that needs database
        // operations (they usually need async methods) should be moved to SaveValidation methods

        protected virtual Task SanitizeEntity(T entity, T oldEntity)
        {
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

        protected virtual Task PostDeleteAction(T entity, int changedByUserId)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostSaveAction(T entity, T oldEntity, SaveType saveType)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostSaveManyAction(IReadOnlyCollection<T> entities, IReadOnlyCollection<T> oldEntities, SaveType saveType)
        {
            return Task.CompletedTask;
        }

        #endregion Virtual Methods

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

        private async Task ValidateData(T entity, T oldEntity)
        {
            if (entity == null)
                throw new BusinessLogicException($"Invalid {EntityName}.");

            if (entity.IsNewEntity && oldEntity != null)
                throw new BusinessLogicException("Your data is no longer up to date. Please refresh the form/page.");

            await SanitizeEntity(entity, oldEntity);
        }

        private async Task<ICollection<T>> ValidateMultipleEntities(
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
                    var oldEntity = oldEntities.FirstOrDefault(x => x.RowID == entity?.RowID);
                    await ValidateData(entity, oldEntity);
                }
            }

            if (updated != null && updated.Any())
            {
                foreach (var entity in updated)
                {
                    var oldEntity = oldEntities.FirstOrDefault(x => x.RowID == entity?.RowID);

                    if (oldEntity == null)
                        throw new BusinessLogicException($"One of the {EntityNamePlural} no longer exists.");

                    await ValidateData(entity, oldEntity);
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

        private async Task CallPostSaveManyAction(List<T> updated, ICollection<T> oldEntities, SaveType saveType)
        {
            if (updated != null && updated.Any())
                await PostSaveManyAction(updated, GetOldEntitiesOfPassedEntities(updated, oldEntities), saveType);
        }

        #endregion Private Methods
    }
}
