using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPaystubDataService
    {
        Task DeleteAsync(Paystub.EmployeeCompositeKey key, int currentlyLoggedInUserId, int organizationId);

        Task DeleteAsync(Paystub paystub, int currentlyLoggedInUserId, int organizationId);

        Task DeleteByPeriodAsync(int payPeriodId, int currentlyLoggedInUserId, int organizationId);

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
            Product singleParentLeaveProduct,
            IReadOnlyCollection<Loan> loans,
            ICollection<AllowanceItem> allowanceItems,
            ICollection<LoanTransaction> loanTransactions,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<Leave> leaves,
            IReadOnlyCollection<Bonus> bonuses);

        Task<ICollection<IAdjustment>> GetAdjustmentsByEmployeeAndDatePeriodAsync(int organizationId, int[] employeeIds, TimePeriod timePeriod, bool includeActual = true);

        Task<ICollection<PaystubData>> GetAllAsync(int payPeriodId);

        Task<ICollection<Paystub>> GetByPaystubsForLoanPaymentFrom13thMonthAsync(int payPeriodId);

        Task RecordCreate(int currentlyLoggedInUserId, Paystub paystub, PayPeriod payPeriod);

        Task RecordDelete(Paystub paystub, int currentlyLoggedInUserId, PayPeriod payPeriod);

        Task RecordEdit(int currentlyLoggedInUserId, Paystub paystub, PayPeriod payPeriod);

        Task UpdateAdjustmentsAsync<T>(int paystubId, ICollection<T> allAdjustments, int currentlyLoggedInUserId) where T : IAdjustment;

        Task UpdateManyThirteenthMonthPaysAsync(ICollection<ThirteenthMonthPay> thirteenthMonthPays);
    }
}
