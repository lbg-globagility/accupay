using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class BaseDailyPayrollDataService<T> : BaseSavableDataService<T> where T : BaseEntity, IPayrollEntity
    {
        public BaseDailyPayrollDataService(
            SavableRepository<T> savableRepository,
            PayPeriodRepository payPeriodRepository,
            PolicyHelper policy,
            string entityDoesNotExistOnDeleteErrorMessage) :

            base(savableRepository,
                payPeriodRepository,
                policy,
                entityDoesNotExistOnDeleteErrorMessage)
        {
        }

        public async override Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                throw new BusinessLogicException(EntityDoesNotExistOnDeleteErrorMessage);

            await ValidateDate(entity, entity, checkOldEntity: false);

            await _repository.DeleteAsync(entity);
        }

        protected override async Task AdditionalSaveValidation(T entity, T oldEntity)
        {
            if (entity.PayrollDate == null)
                throw new BusinessLogicException("Payroll Date is required");

            if (entity.OrganizationID == null)
                throw new BusinessLogicException("Organization is required");

            await ValidateDate(entity, oldEntity);
        }

        protected override async Task AdditionalSaveManyValidation(List<T> entities, List<T> oldEntities)
        {
            int? organizationId = null;
            foreach (var entity in entities)
            {
                if (entity.PayrollDate == null)
                    throw new BusinessLogicException("Payroll Date is required");

                if (entity.OrganizationID == null)
                    throw new BusinessLogicException("Organization is required");

                organizationId = ValidateOrganization(organizationId, entity.OrganizationID);
            }

            await ValidateDates(entities, oldEntities, organizationId);
        }

        protected async Task ValidateDate(T entity, T oldEntity, bool checkOldEntity = true)
        {
            if (checkOldEntity && IsNewEntity(entity.RowID) == false)
            {
                if (oldEntity.PayrollDate != entity.PayrollDate)
                {
                    await CheckIfDataIsWithinClosedPayroll(oldEntity.PayrollDate.Value, oldEntity.OrganizationID.Value);
                }
            }

            await CheckIfDataIsWithinClosedPayroll(entity.PayrollDate.Value, entity.OrganizationID.Value);
        }

        protected async Task ValidateDates(List<T> entities, List<T> oldEntities, int? organizationId)
        {
            var payrollDates = entities
                .Select(x => x.PayrollDate.Value)
                .Distinct()
                .ToList();

            payrollDates.AddRange(oldEntities.Select(x => x.PayrollDate.Value).Distinct());

            await CheckIfDataIsWithinClosedPayroll(payrollDates.Distinct(), organizationId.Value);
        }
    }
}