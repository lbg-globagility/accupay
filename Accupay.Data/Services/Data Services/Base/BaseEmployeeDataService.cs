using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public abstract class BaseEmployeeDataService<T> : BaseOrganizationDataService<T> where T : BaseEmployeeDataEntity
    {
        public BaseEmployeeDataService(
            SavableRepository<T> repository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            PolicyHelper policy,
            string entityName,
            string entityNamePlural = null) :

            base(repository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName,
                entityNamePlural)
        {
        }

        protected override async Task RecordDelete(T entity, int changedByUserId)
        {
            await _userActivityRepository.RecordDeleteAsync(
                changedByUserId,
                entityId: entity.RowID.Value,
                entityName: GetUserActivityName(entity),
                suffixIdentifier: CreateUserActivitySuffixIdentifier(entity),
                organizationId: entity.OrganizationID.Value,
                changedEmployeeId: entity.EmployeeID.Value);
        }

        protected override async Task RecordAdd(T entity)
        {
            await _userActivityRepository.RecordAddAsync(
                entity.CreatedBy.Value,
                entityId: entity.RowID.Value,
                entityName: GetUserActivityName(entity),
                suffixIdentifier: CreateUserActivitySuffixIdentifier(entity),
                organizationId: entity.OrganizationID.Value,
                changedEmployeeId: entity.EmployeeID.Value);
        }

        // TODO: delete this later. Every data service should implement this.
        protected override Task RecordUpdate(T entity, T oldEntity)
        {
            return Task.CompletedTask;
        }
    }
}
