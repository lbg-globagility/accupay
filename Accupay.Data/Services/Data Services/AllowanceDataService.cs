using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class AllowanceDataService : BaseSavableDataService<Allowance>
    {
        private readonly AllowanceRepository _allowanceRepository;
        private readonly PayrollContext _context;

        public AllowanceDataService(
            AllowanceRepository allowanceRepository,
            PayPeriodRepository payPeriodRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(allowanceRepository,
                payPeriodRepository,
                policy,
                entityDoesNotExistOnDeleteErrorMessage: "Allowance does not exists.")
        {
            _allowanceRepository = allowanceRepository;
            _context = context;
        }

        protected override async Task SanitizeEntity(Allowance allowance, Allowance oldAllowance)
        {
            if (allowance.IsOneTime)
                allowance.EffectiveEndDate = allowance.EffectiveStartDate;

            if (allowance.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (allowance.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (_allowanceRepository.GetFrequencyList().Contains(allowance.AllowanceFrequency) == false)
                throw new BusinessLogicException("Invalid frequency.");

            if (allowance.ProductID == null)
                throw new BusinessLogicException("Allowance type is required.");

            if (allowance.EffectiveEndDate != null && allowance.EffectiveStartDate > allowance.EffectiveEndDate)
                throw new BusinessLogicException("Start date cannot be greater than end date.");

            if (allowance.Amount < 0)
                throw new BusinessLogicException("Amount cannot be less than 0.");

            var product = await _context.Products
                                        .Where(p => p.RowID == allowance.ProductID)
                                        .FirstOrDefaultAsync();

            if (product == null)
                throw new BusinessLogicException("The selected allowance type no longer exists.");

            if (allowance.IsMonthly && !product.Fixed)
                throw new BusinessLogicException("Only fixed allowance type are allowed for Monthly allowances.");
        }

        protected override async Task AdditionalDeleteValidation(Allowance allowance)
        {
            var payPeriods = await _allowanceRepository.GetPayPeriodsAsync(allowance.RowID.Value);
            CheckIfDataIsWithinClosedPayroll(payPeriods);
        }

        protected async override Task AdditionalSaveValidation(Allowance allowance, Allowance oldAllowance)
        {
            // cannot create a new allowance or change the Start Date of an
            // existing allowance into a date on a "Closed" pay period.
            if (CheckIfStartDateNeedsToBeValidated(new List<Allowance>() { oldAllowance }, allowance))
            {
                await CheckIfDataIsWithinClosedPayroll(allowance.EffectiveStartDate, allowance.OrganizationID.Value);
            }

            if (!IsNewEntity(allowance.RowID))
            {
                // validate entities that are for update
                var payPeriods = await _allowanceRepository.GetPayPeriodsAsync(allowance.RowID.Value);
                var alreadyUsedInClosedPayroll = CheckIfDataIsWithinClosedPayroll(payPeriods, throwException: false);

                if (alreadyUsedInClosedPayroll)
                {
                    // only end date can be updated if allowance is already used in a closed pay period
                    ValidateAllowableEdit(
                        allowance: allowance,
                        oldAllowance: oldAllowance);

                    // end date cannot be in a closed pay period
                    if (allowance.EffectiveEndDate.HasValue)
                    {
                        if (await CheckIfDataIsWithinClosedPayroll(allowance.EffectiveEndDate.Value, allowance.OrganizationID.Value, throwException: false))
                            throw new BusinessLogicException("Cannot update End Date into a date in a Closed Payroll.");
                    }
                }
            }
        }

        protected async override Task AdditionalSaveManyValidation(List<Allowance> entities, List<Allowance> oldEntities)
        {
            int? organizationId = null;
            entities.ForEach(x =>
            {
                organizationId = ValidateOrganization(organizationId, x.OrganizationID);
            });

            // cannot create a new allowance or change the Start Date of an
            // existing allowance into a date on a "Closed" pay period.
            var newEntityStartDates = entities
                 .Where(allowance =>
                 {
                     return CheckIfStartDateNeedsToBeValidated(oldEntities, allowance);
                 })
                 .Select(x => x.EffectiveStartDate)
                 .ToArray();

            await CheckIfDataIsWithinClosedPayroll(newEntityStartDates, organizationId.Value);

            // validate entities that are for update
            var forUpdateEntities = entities
                .Where(x => !IsNewEntity(x.RowID))
                .ToArray();

            var ids = forUpdateEntities.Select(x => x.RowID.Value).Distinct().ToArray();

            if (ids.Length == 0) return;

            // only end date can be updated if allowance is already used in a closed pay period
            var allowanceItems = await _allowanceRepository.GetAllowanceItemsWithPayPeriodAsync(ids);

            foreach (var entity in forUpdateEntities)
            {
                var oldEntity = oldEntities.Where(x => x.RowID == entity.RowID).FirstOrDefault();

                var payPeriods = allowanceItems
                    .Where(x => x.AllowanceID == entity.RowID)
                    .Select(x => x.PayPeriod)
                    .ToList();

                var query = payPeriods.AsQueryable();
                query = _payPeriodRepository.AddCheckIfClosedPayPeriodQuery(query);

                var alreadyUsedInClosedPayroll = query.Any();

                if (alreadyUsedInClosedPayroll)
                {
                    ValidateAllowableEdit(
                        allowance: entity,
                        oldAllowance: oldEntity);
                }
            }

            // end date cannot be in a closed pay period
            var endDates = entities
                 .Where(x => !IsNewEntity(x.RowID))
                 .Where(x => x.EffectiveEndDate.HasValue)
                 .Select(x => x.EffectiveEndDate.Value)
                 .ToArray();

            if (await CheckIfDataIsWithinClosedPayroll(endDates, organizationId.Value, throwException: false))
                throw new BusinessLogicException("Cannot update End Date into a date in a Closed Payroll.");
        }

        private bool CheckIfStartDateNeedsToBeValidated(List<Allowance> oldEntities, Allowance allowance)
        {
            // either a new entity
            if (IsNewEntity(allowance.RowID)) return true;

            // or an existing entity where its Start Date is to be updated
            var oldEntity = oldEntities.Where(o => o.RowID == allowance.RowID).FirstOrDefault();

            if (allowance.EffectiveStartDate.ToMinimumHourValue() != oldEntity.EffectiveStartDate.ToMinimumHourValue())
                return true;

            return false;
        }

        private void ValidateAllowableEdit(Allowance allowance, Allowance oldAllowance)
        {
            if (oldAllowance == null) return;

            if (allowance.ProductID != oldAllowance.ProductID ||
                allowance.AllowanceFrequency != oldAllowance.AllowanceFrequency ||
                allowance.EffectiveStartDate.ToMinimumHourValue() != oldAllowance.EffectiveStartDate.ToMinimumHourValue() ||
                allowance.Amount != oldAllowance.Amount)
            {
                throw new BusinessLogicException("The allowance is already used in a Closed Payroll. Only End Date can be edited.");
            }
        }

        #region Queries

        public async Task<bool> CheckIfAlreadyUsedInClosedPeriodAsync(int allowanceId)
        {
            var payPeriods = await _allowanceRepository.GetPayPeriodsAsync(allowanceId);
            return CheckIfDataIsWithinClosedPayroll(payPeriods, throwException: false);
        }

        #endregion Queries
    }
}