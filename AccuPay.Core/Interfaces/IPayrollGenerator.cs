using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPayrollGenerator
    {
        Task SavePayroll(
            int currentlyLoggedInUserId,
            string currentSystemOwner,
            ListOfValueCollection settings,
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

        Task<PaystubEmployeeResult> Start(
            int employeeId,
            IPayrollResources resources,
            int organizationId,
            int currentlyLoggedInUserId);
    }
}
