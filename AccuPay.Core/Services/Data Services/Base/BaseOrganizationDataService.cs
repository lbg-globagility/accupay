using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public abstract class BaseOrganizationDataService<T> : AuditableDataService<T> where T : OrganizationalEntity
    {
        public BaseOrganizationDataService(
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

            if (entity.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");
        }

        protected override async Task RecordDelete(T entity, int currentlyLoggedInUserId)
        {
            await _userActivityRepository.RecordDeleteAsync(
                currentlyLoggedInUserId,
                entityId: entity.RowID.Value,
                entityName: GetUserActivityName(entity),
                suffixIdentifier: CreateUserActivitySuffixIdentifier(entity),
                organizationId: entity.OrganizationID.Value);
        }

        protected override async Task RecordAdd(T entity)
        {
            await _userActivityRepository.RecordAddAsync(
                entity.CreatedBy.Value,
                entityId: entity.RowID.Value,
                entityName: GetUserActivityName(entity),
                suffixIdentifier: CreateUserActivitySuffixIdentifier(entity),
                organizationId: entity.OrganizationID.Value);
        }
    }
}
