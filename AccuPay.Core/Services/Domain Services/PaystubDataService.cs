using AccuPay.Core;
using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Core.ValueObjects;
using AccuPay.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Core.Entities.Paystub;

namespace AccuPay.Infrastructure.Data
{
    public class PaystubDataService : BasePaystubDataService, IPaystubDataService
    {
        private readonly IPaystubRepository _paystubRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly PaystubDataHelper _paystubDataHelper;

        public PaystubDataService(
            IPaystubRepository paystubRepository,
            ISalaryRepository salaryRepository,
            IPayPeriodRepository payPeriodRepository,
            PaystubDataHelper paystubDataHelper) : base(payPeriodRepository)
        {
            _paystubRepository = paystubRepository;
            _salaryRepository = salaryRepository;
            _paystubDataHelper = paystubDataHelper;
        }

        #region Save

        public async Task DeleteAsync(EmployeeCompositeKey key, int currentlyLoggedInUserId, int organizationId)
        {
            var paystub = await _paystubRepository.GetByCompositeKeyAsync(key);
            await DeleteAsync(
                paystub,
                currentlyLoggedInUserId: currentlyLoggedInUserId,
                organizationId: organizationId);
        }

        public async Task DeleteAsync(Paystub paystub, int currentlyLoggedInUserId, int organizationId)
        {
            if (paystub == null)
                throw new BusinessLogicException("Paystub does not exists.");

            var payPeriod = (await _paystubRepository.GetWithPayPeriod(paystub.RowID.Value))?.PayPeriod;
            var payPeriodId = payPeriod?.RowID;

            await ValidateIfPayPeriodIsOpenAsync(
                organizationId: organizationId,
                payPeriodId: payPeriodId);

            await _paystubRepository.DeleteAsync(
                id: paystub.RowID.Value,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            // TODO: maybe record delete for adjustments
            await RecordDelete(paystub, currentlyLoggedInUserId, payPeriod);
        }

        public async Task DeleteByPeriodAsync(int payPeriodId, int currentlyLoggedInUserId, int organizationId)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            ValidateIfPayPeriodIsOpenAsync(payPeriod);

            var paystubs = await _paystubRepository.DeleteByPeriodAsync(
                payPeriodId: payPeriodId,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            // TODO: maybe record delete for adjustments
            await _paystubDataHelper.RecordDelete(currentlyLoggedInUserId, paystubs, payPeriod);
        }

        public async Task SaveAsync(
            int currentlyLoggedInUserId,
            string currentSystemOwner,
            IPolicyHelper policy,
            PayPeriod payPeriod,
            Paystub paystub,
            Employee employee,
            Product bpiInsuranceProduct,
            Product sickLeaveProduct,
            Product vacationLeaveProduct,
            IReadOnlyCollection<Loan> loans,
            ICollection<AllowanceItem> allowanceItems,
            ICollection<LoanTransaction> loanTransactions,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<Leave> leaves,
            IReadOnlyCollection<Bonus> bonuses)
        {
            bool isNew = paystub.IsNewEntity;

            await _paystubRepository.SaveAsync(
                currentlyLoggedInUserId: currentlyLoggedInUserId,
                currentSystemOwner,
                policy,
                payPeriod,
                paystub,
                employee,
                bpiInsuranceProduct: bpiInsuranceProduct,
                sickLeaveProduct: sickLeaveProduct,
                vacationLeaveProduct: vacationLeaveProduct,
                loans: loans,
                allowanceItems: allowanceItems,
                loanTransactions: loanTransactions,
                timeEntries: timeEntries,
                leaves: leaves,
                bonuses: bonuses);

            if (isNew)
            {
                await RecordCreate(currentlyLoggedInUserId, paystub, payPeriod);
            }
            else
            {
                await RecordEdit(currentlyLoggedInUserId, paystub, payPeriod);
            }
        }

        public async Task RecordDelete(Paystub paystub, int currentlyLoggedInUserId, PayPeriod payPeriod)
        {
            await _paystubDataHelper.RecordDelete(currentlyLoggedInUserId, paystub, payPeriod);
        }

        public async Task RecordCreate(int currentlyLoggedInUserId, Paystub paystub, PayPeriod payPeriod)
        {
            await _paystubDataHelper.RecordCreate(currentlyLoggedInUserId, paystub, payPeriod);
        }

        public async Task RecordEdit(int currentlyLoggedInUserId, Paystub paystub, PayPeriod payPeriod)
        {
            await _paystubDataHelper.RecordEdit(currentlyLoggedInUserId, paystub, payPeriod);
        }

        public async Task UpdateManyThirteenthMonthPaysAsync(ICollection<ThirteenthMonthPay> thirteenthMonthPays)
        {
            var paystubIds = thirteenthMonthPays
                .Where(x => x.RowID.HasValue)
                .Select(x => x.PaystubID.Value)
                .Distinct()
                .ToArray();

            var payPeriods = (await _paystubRepository.GetWithPayPeriod(paystubIds))?.Select(x => x.PayPeriod);

            // TODO:
            // get payperiods through paystubIds
            // validate payperiod

            await _paystubRepository.UpdateManyThirteenthMonthPaysAsync(thirteenthMonthPays);
        }

        public async Task UpdateAdjustmentsAsync<T>(
            int paystubId,
            ICollection<T> allAdjustments,
            int currentlyLoggedInUserId) where T : IAdjustment
        {
            foreach (var adjustment in allAdjustments)
            {
                SanitizeAdjustment(currentlyLoggedInUserId, adjustment);
            }

            (IReadOnlyCollection<T> added,
                IReadOnlyCollection<T> updated,
                IReadOnlyCollection<T> deleted,
                IReadOnlyCollection<T> originalAdjustments) =
                await _paystubRepository.UpdateAdjustmentsAsync(paystubId, allAdjustments);

            await _paystubDataHelper.RecordCreateAdjustments(currentlyLoggedInUserId, added);
            await _paystubDataHelper.RecordUpdateAdjustments(currentlyLoggedInUserId, updated, originalAdjustments);
            await _paystubDataHelper.RecordDeleteAdjustments(currentlyLoggedInUserId, deleted);
        }

        private static void SanitizeAdjustment<T>(int currentlyLoggedInUserId, T adjustment) where T : IAdjustment
        {
            if (adjustment.PaystubID == null)
                throw new BusinessLogicException("Paystub is required.");

            if (adjustment.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (adjustment.ProductID == null)
                throw new BusinessLogicException("Adjustment Type is required.");

            if (adjustment.Amount == 0)
                throw new BusinessLogicException("Amount cannot be equal to 0.");

            adjustment.Amount = AccuMath.CommercialRound(adjustment.Amount);

            adjustment.AuditUser(currentlyLoggedInUserId);
        }

        #endregion Save

        public async Task<ICollection<PaystubData>> GetAllAsync(int payPeriodId)
        {
            var paystubs = await _paystubRepository.GetByPayPeriodFullPaystubAsync(payPeriodId);

            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            var employeeIds = paystubs.Select(x => x.EmployeeID.Value).ToArray();

            var salaries = await _salaryRepository.GetByMultipleEmployeeAsync(employeeIds, payPeriod.PayToDate);

            var paystubsWithSalary = paystubs
                .Select(paystub => new PaystubData(
                    paystub,
                    salaries.FirstOrDefault(s => s.EmployeeID == paystub.EmployeeID)
                )).ToList();

            return paystubsWithSalary;
        }

        public async Task<ICollection<IAdjustment>> GetAdjustmentsByEmployeeAndDatePeriodAsync(
            int organizationId,
            int[] employeeIds,
            TimePeriod timePeriod,
            bool includeActual = true)
        {
            IEnumerable<Paystub> paystubs = await _paystubRepository.GetPaystubWithAdjustmentsByEmployeeAndDatePeriodAsync(
                organizationId,
                employeeIds,
                timePeriod,
                includeActual);

            var adjustments = new List<IAdjustment>(paystubs.SelectMany(x => x.Adjustments.Select(a => a)).ToList());

            if (includeActual)
            {
                adjustments.AddRange(paystubs.SelectMany(x => x.ActualAdjustments.Select(a => a)).ToList());
            }

            return adjustments;
        }

        public async Task<ICollection<Paystub>> GetByPaystubsForLoanPaymentFrom13thMonthAsync(int payPeriodId)
        {
            var result = await _paystubRepository
                .GetByPaystubsForLoanPaymentFrom13thMonthAsync(payPeriodId);

            if (!result.Any())
            {
                throw new BusinessLogicException("Thirteenth Month pay is not released on this period.");
            }

            return result;
        }
    }
}
