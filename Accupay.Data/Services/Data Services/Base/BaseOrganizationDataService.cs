using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public abstract class BaseOrganizationDataService<T> : BaseSavableDataService<T> where T : BaseOrganizationalEntity
    {
        public BaseOrganizationDataService(
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

        protected override async Task PostDeleteAction(T entity, int changedByUserId)
        {
            await _userActivityRepository.RecordDeleteAsync(
                changedByUserId,
                entityId: entity.RowID.Value,
                entityName: GetUserActivityName(entity),
                suffixIdentifier: CreateUserActivitySuffixIdentifier(entity),
                organizationId: entity.OrganizationID.Value);
        }
    }
}
