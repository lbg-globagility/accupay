using AccuPay.Data.Entities;

namespace AccuPay.Web.Loans
{
    public class LoanTransactionDto
    {
        public int Id { get; set; }
        public string LoanNumber { get; set; }
        public string LoanType { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DeductionAmount { get; set; }

        public decimal Balance { get; set; }

        public static LoanTransactionDto Convert(LoanTransaction loanTransaction)
        {
            if (loanTransaction == null) return null;

            return new LoanTransactionDto
            {
                Id = loanTransaction.RowID.Value,
                LoanNumber = loanTransaction.Loan.LoanNumber,
                LoanType = loanTransaction.Loan.LoanType.DisplayName,
                TotalAmount = loanTransaction.Loan.TotalLoanAmount,
                DeductionAmount = loanTransaction.DeductionAmount,
                Balance = loanTransaction.TotalBalance
            };
        }
    }
}
