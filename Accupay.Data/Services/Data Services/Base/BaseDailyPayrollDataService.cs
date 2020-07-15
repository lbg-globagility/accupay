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
            var entity = await _savableRepository.GetByIdAsync(id);

            if (entity == null)
                throw new BusinessLogicException(EntityDoesNotExistOnDeleteErrorMessage);

            await ValidateDate(entity, checkOldEntity: false);

            await _savableRepository.DeleteAsync(entity);
        }

        public override async Task SaveAsync(T entity)
        {
            await ValidateData(entity);

            if (entity.PayrollDate == null)
                throw new BusinessLogicException("Payroll Date is required");

            if (entity.OrganizationID == null)
                throw new BusinessLogicException("Organization is required");

            await ValidateDate(entity);
            await AdditionalSaveValidation(entity);

            await _savableRepository.SaveAsync(entity);
        }

        public override async Task SaveManyAsync(List<T> entities)
        {
            int? organizationId = null;
            foreach (var entity in entities)
            {
                await ValidateData(entity);

                if (entity.PayrollDate == null)
                    throw new BusinessLogicException("Payroll Date is required");

                if (entity.OrganizationID == null)
                    throw new BusinessLogicException("Organization is required");

                organizationId = ValidateOrganization(organizationId, entity);
            }

            await ValidateDates(entities, organizationId);

            await AdditionalSaveManyValidation(entities);

            await _savableRepository.SaveManyAsync(entities);
        }

        protected async Task ValidateDate(T entity, bool checkOldEntity = true)
        {
            if (checkOldEntity && IsNewEntity(entity.RowID) == false)
            {
                var oldEntity = await _savableRepository.GetByIdAsync(entity.RowID.Value);
                if (oldEntity.PayrollDate != entity.PayrollDate)
                {
                    await CheckIfDataIsWithinClosedPayroll(oldEntity.PayrollDate.Value, oldEntity.OrganizationID.Value);
                }
            }

            await CheckIfDataIsWithinClosedPayroll(entity.PayrollDate.Value, entity.OrganizationID.Value);
        }

        protected async Task ValidateDates(List<T> entities, int? organizationId)
        {
            var entityIds = entities
                .Where(x => IsNewEntity(x.RowID) == false)
                .Select(x => x.RowID.Value)
                .Distinct()
                .ToArray();

            var oldEntities = await _savableRepository.GetManyByIdsAsync(entityIds);

            var payrollDates = entities
                .Select(x => x.PayrollDate.Value)
                .Distinct()
                .ToList();

            payrollDates.AddRange(oldEntities.Select(x => x.PayrollDate.Value).Distinct());

            await CheckIfDataIsWithinClosedPayroll(payrollDates.Distinct(), organizationId.Value);
        }

        private int? ValidateOrganization(int? organizationId, T entity)
        {
            if (organizationId == null)
            {
                organizationId = entity.OrganizationID;
            }
            else
            {
                if (organizationId != entity.OrganizationID)
                    throw new BusinessLogicException("Cannot save multiple data from different organizations.");
            }

            return organizationId;
        }
    }
}