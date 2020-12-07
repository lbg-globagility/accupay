using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public abstract class AuditableDataService<T> : BaseSavableDataService<T> where T : AuditableEntity
    {
        protected readonly UserActivityRepository _userActivityRepository;

        public AuditableDataService(
               SavableRepository<T> repository,
               PayPeriodRepository payPeriodRepository,
               UserActivityRepository userActivityRepository,
               PayrollContext context,
               PolicyHelper policy,
               string entityName,
               string entityNamePlural = null) :

               base(repository,
                   payPeriodRepository,
                   context,
                   policy,
                   entityName,
                   entityNamePlural)
        {
            _userActivityRepository = userActivityRepository;
        }

        protected override async Task SanitizeEntity(T entity, T oldEntity, int currentlyLoggedInUserId)
        {
            await base.SanitizeEntity(entity, oldEntity, currentlyLoggedInUserId);

            entity.AuditUser(currentlyLoggedInUserId);
        }

        #region Abstract

        protected abstract string GetUserActivityName(T entity);

        protected abstract string CreateUserActivitySuffixIdentifier(T entity);

        #endregion Abstract

        #region Virtual

        protected virtual Task RecordDelete(T entity, int currentlyLoggedInUserId)
        {
            return Task.CompletedTask;
        }

        protected virtual Task RecordAdd(T entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task RecordUpdate(T entity, T oldEntity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task RecordUpdate(IReadOnlyCollection<T> entities, IReadOnlyCollection<T> oldEntities)
        {
            return Task.CompletedTask;
        }

        protected virtual async Task PostDeleteManyAction(IReadOnlyCollection<T> entities, int currentlyLoggedInUserId)
        {
            foreach (var item in entities)
            {
                await RecordDelete(item, currentlyLoggedInUserId);
            }
        }

        protected virtual async Task PostInsertManyAction(IReadOnlyCollection<T> entities)
        {
            foreach (var item in entities)
            {
                await RecordAdd(item);
            }
        }

        protected virtual async Task PostUpdateManyAction(IReadOnlyCollection<T> entities, IReadOnlyCollection<T> oldEntities)
        {
            // TODO: create an equality comparer Interface for the entities to implement
            // or override the Equals method of the entities to use here to determine
            // the oldEntity from the oldEntities list.
            // If the above TODO will be implemented, PostUpdateManyAction will loop through
            // the entities list just like in PostDeleteManyAction and PostInsertManyAction
            // and there will be no need of a virtual
            // RecordUpdate(IReadOnlyCollection<T> entities, IReadOnlyCollection<T> oldEntities)
            await RecordUpdate(entities, oldEntities);
        }

        #endregion Virtual

        #region Overrides

        protected override async Task PostDeleteAction(T entity, int currentlyLoggedInUserId)
        {
            await RecordDelete(entity, currentlyLoggedInUserId);
        }

        protected override async Task PostSaveAction(T entity, T oldEntity, SaveType saveType)
        {
            if (saveType == SaveType.Insert)
            {
                await RecordAdd(entity);
            }
            else if (saveType == SaveType.Update)
            {
                await RecordUpdate(entity, oldEntity);
            }
        }

        protected override async Task PostSaveManyAction(
            IReadOnlyCollection<T> entities,
            IReadOnlyCollection<T> oldEntities,
            SaveType saveType,
            int currentlyLoggedInUserId)
        {
            switch (saveType)
            {
                case SaveType.Insert:

                    await PostInsertManyAction(entities);

                    break;

                case SaveType.Update:

                    await PostUpdateManyAction(entities, oldEntities);
                    break;

                case SaveType.Delete:

                    await PostDeleteManyAction(entities, currentlyLoggedInUserId);
                    break;

                default:
                    break;
            }
        }

        #endregion Overrides
    }
}
