using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class OrganizationDataService : BaseSavableDataService<Organization>, IOrganizationDataService
    {
        //private const string UserActivityName = "Organization";

        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationDataService(
            IOrganizationRepository organizationRepository,
            IPayPeriodRepository payPeriodRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(organizationRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Organization")
        {
            _organizationRepository = organizationRepository;
        }

        //protected override string GetUserActivityName(Organization organization) => UserActivityName;

        //protected override string CreateUserActivitySuffixIdentifier(Organization organization) =>
        //    $" with name '{organization.Name}'";

        protected override Task SanitizeEntity(Organization entity, Organization oldEntity, int changedByUserId)
        {
            base.SanitizeEntity(
                entity: entity,
                oldEntity: oldEntity,
                currentlyLoggedInUserId: changedByUserId);

            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new BusinessLogicException("Name is required.");

            if (BaseEntity.CheckIfNewEntity(entity.ClientId))
                throw new BusinessLogicException("Client is required.");

            entity.AuditUser(changedByUserId);

            return Task.CompletedTask;
        }

        protected override async Task AdditionalDeleteValidation(Organization entity)
        {
            var clientOrganizations = await _organizationRepository.List(OrganizationPageOptions.AllData, entity.ClientId);

            if (!_policy.UseUserLevel && clientOrganizations.organizations.Count <= 1)
            {
                throw new BusinessLogicException("There should be at least one active organization!");
            }
        }

        protected override async Task AdditionalSaveValidation(Organization entity, Organization oldEntity)
        {
            var doesExists = await _organizationRepository.CheckIfNameExistsAsync(entity.Name, entity.RowID);

            if (doesExists)
                throw new BusinessLogicException("Name already exists!");
        }
    }
}
