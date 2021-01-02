using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public abstract class BaseEmployeeDataService<T> : BaseOrganizationDataService<T> where T : EmployeeDataEntity
    {
        public BaseEmployeeDataService(
            ISavableRepository<T> repository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy,
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

        protected override async Task SanitizeEntity(T entity, T oldEntity, int currentlyLoggedInUserId)
        {
            await base.SanitizeEntity(entity, oldEntity, currentlyLoggedInUserId);

            if (entity.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");
        }

        protected override async Task RecordDelete(T entity, int currentlyLoggedInUserId)
        {
            await _userActivityRepository.RecordDeleteAsync(
                currentlyLoggedInUserId,
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
    }
}
