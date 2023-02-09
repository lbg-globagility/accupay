using AccuPay.Core.Entities;
using AccuPay.Core.Services.Imports.Loans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILoanDataService : IBaseSavableDataService<Loan>
    {
        Task BatchApply(IReadOnlyCollection<LoanImportModel> validRecords, int organizationId, int currentlyLoggedInUserId);

        Task DeleteAllLoansExceptGovernmentLoansAsync(int employeeId, int pagibigLoanId, int ssLoanId);

        Task<ICollection<Loan>> GetCurrentPayrollLoansAsync(int organizationId, PayPeriod payPeriod, IReadOnlyCollection<Paystub> paystubs);

        Task<ICollection<Loan>> GetLoansByMonthPeriodAsync(int organizationId, DateTime dateTime);
    }
}
