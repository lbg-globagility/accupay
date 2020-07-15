using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
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

        protected override async Task SanitizeEntity(Allowance allowance)
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

        protected async override Task AdditionalSaveValidation(Allowance allowance)
        {
            var payPeriods = await _allowanceRepository.GetPayPeriodsAsync(allowance.RowID.Value);
            CheckIfDataIsWithinClosedPayroll(payPeriods);
        }

        protected async override Task AdditionalSaveManyValidation(List<Allowance> entities)
        {
            var ids = entities
                .Where(x => x.RowID.HasValue)
                .Select(x => x.RowID.Value)
                .ToArray();

            var payPeriods = await _allowanceRepository.GetPayPeriodsAsync(ids);
            CheckIfDataIsWithinClosedPayroll(payPeriods);
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