using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class AllowanceDataService : BaseSavableDataService<Allowance>
    {
        private readonly AllowanceRepository _allowanceRepository;
        private readonly ProductRepository _productRepository;

        public AllowanceDataService(
            AllowanceRepository allowanceRepository,
            PayPeriodRepository payPeriodRepository,
            ProductRepository productRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(allowanceRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Allowance")
        {
            _allowanceRepository = allowanceRepository;
            _productRepository = productRepository;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        protected override async Task SanitizeEntity(Allowance allowance, Allowance oldAllowance)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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

            if (allowance.EffectiveStartDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

            if (allowance.EffectiveEndDate != null && allowance.EffectiveStartDate > allowance.EffectiveEndDate)
                throw new BusinessLogicException("Start date cannot be greater than end date.");

            if (allowance.Amount < 0)
                throw new BusinessLogicException("Amount cannot be less than 0.");
        }

        protected override async Task AdditionalDeleteValidation(Allowance allowance)
        {
            if (await CheckIfAlreadyUsedInClosedPayPeriodAsync(allowance.RowID.Value))
                throw new BusinessLogicException("This allowance has already been used and therefore cannot be deleted. Try changing its End Date instead.");
        }

        protected async override Task AdditionalSaveValidation(Allowance allowance, Allowance oldAllowance)
        {
            var product = await _productRepository.GetByIdAsync(allowance.ProductID.Value);

            if (product == null)
                throw new BusinessLogicException("The selected allowance type no longer exists.");

            if (allowance.IsMonthly && !product.Fixed)
                throw new BusinessLogicException("Only fixed allowance type are allowed for Monthly allowances.");

            // cannot create a new allowance or change the Start Date of an
            // existing allowance into a date on a "Closed" pay period.
            if (CheckIfStartDateNeedsToBeValidated(new List<Allowance>() { oldAllowance }, allowance))
            {
                await CheckIfDataIsWithinClosedPayPeriod(allowance.EffectiveStartDate, allowance.OrganizationID.Value);
            }

            if (!IsNewEntity(allowance.RowID))
            {
                // validate entities that are for update
                var payPeriods = await _allowanceRepository.GetPayPeriodsAsync(allowance.RowID.Value);
                var alreadyUsedInClosedPayroll = CheckIfDataIsWithinClosedPayPeriod(payPeriods, throwException: false);

                if (alreadyUsedInClosedPayroll)
                {
                    // only end date can be updated if allowance is already used in a closed pay period
                    ValidateAllowableEdit(
                        allowance: allowance,
                        oldAllowance: oldAllowance);

                    // end date cannot be in a closed pay period
                    if (allowance.EffectiveEndDate.HasValue)
                    {
                        if (await CheckIfDataIsWithinClosedPayPeriod(allowance.EffectiveEndDate.Value, allowance.OrganizationID.Value, throwException: false))
                            throw new BusinessLogicException("Cannot update End Date into a date in a Closed Payroll.");
                    }
                }
            }
        }

        protected async override Task AdditionalSaveManyValidation(List<Allowance> allowances, List<Allowance> oldAllowances)
        {
            var allowanceTypeIds = allowances
                .Where(x => x.ProductID.HasValue)
                .Select(x => x.ProductID.Value).Distinct()
                .ToArray();
            var allowanceTypes = await _productRepository.GetManyByIdsAsync(allowanceTypeIds);

            foreach (var allowance in allowances)
            {
                var product = allowanceTypes.FirstOrDefault(x => x.RowID == allowance.ProductID);

                if (product == null)
                    throw new BusinessLogicException("The selected allowance type no longer exists.");

                if (allowance.IsMonthly && !product.Fixed)
                    throw new BusinessLogicException("Only fixed allowance type are allowed for Monthly allowances.");
            }

            await CheckForClosedPayPeriod(allowances, oldAllowances);
        }

        private async Task CheckForClosedPayPeriod(List<Allowance> allowances, List<Allowance> oldAllowances)
        {
            // validate allowances that are for update
            int? organizationId = await ValidateStartDates(allowances, oldAllowances);

            var forUpdateAllowances = allowances
                .Where(x => !IsNewEntity(x.RowID))
                .ToArray();

            var ids = forUpdateAllowances.Select(x => x.RowID.Value).Distinct().ToArray();

            if (ids.Length == 0) return;

            // only end date can be updated if allowance is already used in a closed pay period
            var allowanceItems = await _allowanceRepository.GetAllowanceItemsWithPayPeriodAsync(ids);

            foreach (var allowance in forUpdateAllowances)
            {
                var oldAllowance = oldAllowances.Where(x => x.RowID == allowance.RowID).FirstOrDefault();

                var payPeriods = allowanceItems
                    .Where(x => x.AllowanceID == allowance.RowID)
                    .Select(x => x.PayPeriod)
                    .ToList();

                var query = payPeriods.AsQueryable();
                query = _payPeriodRepository.AddCheckIfClosedPayPeriodQuery(query);

                var alreadyUsedInClosedPayroll = query.Any();

                if (alreadyUsedInClosedPayroll)
                {
                    ValidateAllowableEdit(
                        allowance: allowance,
                        oldAllowance: oldAllowance);
                }
            }

            // end date cannot be in a closed pay period
            var endDates = allowances
                 .Where(x => !IsNewEntity(x.RowID))
                 .Where(x => x.EffectiveEndDate.HasValue)
                 .Select(x => x.EffectiveEndDate.Value)
                 .ToArray();

            if (await CheckIfDataIsWithinClosedPayPeriod(endDates, organizationId.Value, throwException: false))
                throw new BusinessLogicException("Cannot update End Date into a date in a Closed Payroll.");
        }

        private async Task<int?> ValidateStartDates(List<Allowance> allowances, List<Allowance> oldAllowances)
        {
            int? organizationId = null;
            allowances.ForEach(x =>
            {
                organizationId = ValidateOrganization(organizationId, x.OrganizationID);
            });

            // cannot create a new allowance or change the Start Date of an
            // existing allowance into a date on a "Closed" pay period.
            var validatableStartDates = allowances
                 .Where(allowance =>
                 {
                     return CheckIfStartDateNeedsToBeValidated(oldAllowances, allowance);
                 })
                 .Select(x => x.EffectiveStartDate)
                 .ToArray();

            await CheckIfDataIsWithinClosedPayPeriod(validatableStartDates, organizationId.Value);
            return organizationId;
        }

        private bool CheckIfStartDateNeedsToBeValidated(List<Allowance> oldAllowances, Allowance allowance)
        {
            // either a new allowance
            if (IsNewEntity(allowance.RowID)) return true;

            // or an existing allowance where its Start Date is to be updated
            var oldAllowance = oldAllowances.Where(o => o.RowID == allowance.RowID).FirstOrDefault();

            if (allowance.EffectiveStartDate.ToMinimumHourValue() != oldAllowance.EffectiveStartDate.ToMinimumHourValue())
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

        public async Task<bool> CheckIfAlreadyUsedInClosedPayPeriodAsync(int allowanceId)
        {
            var payPeriods = await _allowanceRepository.GetPayPeriodsAsync(allowanceId);
            return CheckIfDataIsWithinClosedPayPeriod(payPeriods, throwException: false);
        }

        #endregion Queries
    }
}