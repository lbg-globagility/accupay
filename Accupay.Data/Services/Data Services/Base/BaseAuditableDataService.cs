using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public abstract class BaseAuditableDataService<T> : BaseSavableDataService<T> where T : BaseEntity
    {
        protected readonly UserActivityRepository _userActivityRepository;

        public BaseAuditableDataService(
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

        protected abstract string GetUserActivityName(T entity);

        protected abstract string CreateUserActivitySuffixIdentifier(T entity);

        protected abstract Task RecordDelete(T entity, int changedByUserId);

        protected abstract Task RecordAdd(T entity);

        protected abstract Task RecordUpdate(T entity, T oldEntity);

        protected override async Task PostDeleteAction(T entity, int changedByUserId)
        {
            await RecordDelete(entity, changedByUserId);
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
    }
}
