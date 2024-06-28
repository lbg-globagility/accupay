using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AccuPay.Core.Entities.Paystub;

namespace AccuPay.Core.Interfaces
{
    public interface IPaystubRepository
    {
        Task DeleteAsync(int id, int currentlyLoggedInUserId);

        Task<IReadOnlyCollection<Paystub>> DeleteByPeriodAsync(int payPeriodId, int currentlyLoggedInUserId);

        Task SaveAsync(
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
            IReadOnlyCollection<Bonus> bonuses);

        Task<ICollection<ActualAdjustment>> GetActualAdjustmentsAsync(int paystubId);

        Task<ICollection<Adjustment>> GetAdjustmentsAsync(int paystubId);

        Task<ICollection<AllowanceItem>> GetAllowanceItemsAsync(int paystubId);

        Task<ICollection<Paystub>> GetAllWithEmployeeAsync(DateCompositeKey key);

        Task<Paystub> GetByCompositeKeyAsync(EmployeeCompositeKey key);

        Task<Paystub> GetByCompositeKeyFullPaystubAsync(EmployeeCompositeKey key);

        Task<Paystub> GetByCompositeKeyWithActualAndThirteenthMonthAsync(EmployeeCompositeKey key);

        Task<Paystub> GetByIdAsync(int id);

        Task<ICollection<Paystub>> GetByPayPeriodFullPaystubAsync(int payPeriodId);

        Task<Paystub> GetByPayPeriodAndEmployeeFullPaystubAsync(int payPeriodId, int employeeId);

        Task<ICollection<Paystub>> GetByPayPeriodWithEmployeeAsync(int payPeriodId);

        Task<ICollection<Paystub>> GetByPayPeriodWithEmployeeDivisionAndThirteenthMonthPayDetailsAsync(int payPeriodId);

        Task<ICollection<Paystub>> GetByPayPeriodWithEmployeeDivisionAsync(int payPeriodId);

        Task<ICollection<Paystub>> GetByPaystubsForLoanPaymentFrom13thMonthAsync(int payPeriodId);

        Task<ICollection<Paystub>> GetByTimePeriodWithThirteenthMonthPayAndEmployeeAsync(TimePeriod timePeriod, int organizationId);

        Task<ICollection<Paystub>> GetByTimePeriodAndEmployeeIdWithThirteenthMonthPayAndEmployeeAsync(TimePeriod timePeriod, int organizationId, int employeeId);

        Task<ICollection<LoanTransaction>> GetLoanTransactionsAsync(int paystubId);

        Task<Paystub> GetPaystubWithAdjustments(int paystubId);

        Task<ICollection<Paystub>> GetPaystubWithAdjustmentsByEmployeeAndDatePeriodAsync(int organizationId, int[] employeeIds, TimePeriod timePeriod, bool includeActual = true);

        Task<ICollection<Paystub>> GetPreviousCutOffPaystubsAsync(DateTime currentCuttOffStart, int organizationId);

        Task<Paystub> GetWithPayPeriod(int id);

        Task<ICollection<Paystub>> GetWithPayPeriod(int[] ids);

        Task<bool> HasPaystubsAfterDateAsync(DateTime date, int employeeId);

        Task<(IReadOnlyCollection<T> added, IReadOnlyCollection<T> updated, IReadOnlyCollection<T> deleted, IReadOnlyCollection<T> originalAdjustments)> UpdateAdjustmentsAsync<T>(int paystubId, ICollection<T> allAdjustments) where T : IAdjustment;

        Task UpdateManyThirteenthMonthPaysAsync(ICollection<ThirteenthMonthPay> thirteenthMonthPays);
    }
}
