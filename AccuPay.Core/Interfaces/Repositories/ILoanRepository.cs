using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILoanRepository : ISavableRepository<Loan>
    {
        Task DeleteAllLoansExceptGovernmentLoansAsync(int employeeId, int pagibigLoanId, int ssLoanId);

        Task<ICollection<Loan>> GetActiveLoansByLoanNameAsync(string loanName, int employeeId);

        Task<ICollection<Loan>> GetByEmployeeAsync(int employeeId);

        Task<Loan> GetByIdWithEmployeeAndProductAsync(int id);

        Task<ICollection<Loan>> GetCurrentPayrollLoansAsync(int organizationId, PayPeriod payPeriod, IReadOnlyCollection<Paystub> paystubs);

        Task<PaginatedList<LoanTransaction>> GetLoanTransactionsAsync(PageOptions options, int id);

        Task<PaginatedList<LoanTransaction>> GetLoanTransactionsAsync(PageOptions options, int[] id);

        Task<ICollection<LoanTransaction>> GetLoanTransactionsWithPayPeriodAsync(int id);

        Task<PaginatedList<Loan>> GetPaginatedListAsync(LoanPageOptions options, int organizationId);

        List<string> GetStatusList();
    }
}
