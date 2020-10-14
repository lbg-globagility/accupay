using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class OrganizationDataService : BaseSavableDataService<Organization>
    {
        private readonly OrganizationRepository _organizationRepository;

        public OrganizationDataService(
            OrganizationRepository organizationRepository,
            PayPeriodRepository payPeriodRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(organizationRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Organization")
        {
            _organizationRepository = organizationRepository;
        }

        protected override Task SanitizeEntity(Organization entity, Organization oldEntity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new BusinessLogicException("Name is required.");

            if (IsNewEntity(entity.ClientId))
                throw new BusinessLogicException("Client is required.");

            return Task.CompletedTask;
        }

        protected override async Task AdditionalSaveValidation(Organization entity, Organization oldEntity)
        {
            var doesExists = await _organizationRepository.CheckIfNameExistsAsync(entity.Name, entity.RowID);

            if (doesExists)
                throw new BusinessLogicException("Name already exists!");
        }
    }
}