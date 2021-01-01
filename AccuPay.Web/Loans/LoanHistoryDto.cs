using AccuPay.Data.Entities;
using System;

namespace AccuPay.Web.Loans
{
    public class LoanHistoryDto
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeNo { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public DateTime DeductionDate { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public static LoanHistoryDto Convert(LoanTransaction loanTransaction)
        {
            if (loanTransaction == null) return null;

            return new LoanHistoryDto
            {
                Id = loanTransaction.RowID.Value,
                EmployeeId = loanTransaction.EmployeeID,
                EmployeeName = loanTransaction.Loan?.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeNo = loanTransaction.Loan?.Employee?.EmployeeNo,
                EmployeeType = loanTransaction.Loan?.Employee?.EmployeeType,
                DeductionDate = loanTransaction.PayPeriodPayToDate.Value,
                Amount = loanTransaction.DeductionAmount,
                Balance = loanTransaction.TotalBalance
            };
        }
    }
}
