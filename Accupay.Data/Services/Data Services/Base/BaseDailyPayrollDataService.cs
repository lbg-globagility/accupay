using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    // this is not for daily employees
    // might want to rename this class name
    // check the IPayrollEntity interface
    // to get a better idea of this abstraction
    public abstract class BaseDailyPayrollDataService<T> : BaseEmployeeDataService<T> where T : EmployeeDataEntity, IPayrollEntity
    {
        public BaseDailyPayrollDataService(
            SavableRepository<T> savableRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            PolicyHelper policy,
            string entityName,
            string entityNamePlural = null) :

            base(savableRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName,
                entityNamePlural: entityNamePlural)
        {
        }

        protected override async Task AdditionalDeleteValidation(T entity)
        {
            await ValidateDate(entity, entity, checkOldEntity: false);
        }

        protected override async Task AdditionalSaveValidation(T entity, T oldEntity)
        {
            if (entity.PayrollDate == null)
                throw new BusinessLogicException("Payroll Date is required");

            if (entity.OrganizationID == null)
                throw new BusinessLogicException("Organization is required");

            await ValidateDate(entity, oldEntity);
        }

        protected override async Task AdditionalSaveManyValidation(List<T> entities, List<T> oldEntities, SaveType saveType)
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
            if (checkOldEntity && entity.IsNewEntity == false)
            {
                if (oldEntity.PayrollDate != entity.PayrollDate)
                {
                    await CheckIfDataIsWithinClosedPayPeriod(oldEntity.PayrollDate.Value, oldEntity.OrganizationID.Value);
                }
            }

            await CheckIfDataIsWithinClosedPayPeriod(entity.PayrollDate.Value, entity.OrganizationID.Value);
        }

        protected async Task ValidateDates(List<T> entities, List<T> oldEntities, int? organizationId)
        {
            var payrollDates = entities
                .Select(x => x.PayrollDate.Value)
                .Distinct()
                .ToList();

            payrollDates.AddRange(oldEntities.Select(x => x.PayrollDate.Value).Distinct());

            await CheckIfDataIsWithinClosedPayPeriod(payrollDates.Distinct(), organizationId.Value);
        }
    }
}
