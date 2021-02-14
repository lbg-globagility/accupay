using AccuPay.Core.Entities;
using System.Collections.Generic;

namespace AccuPay.Core.UnitTests
{
    public static class LoanTransactionMother
    {
        public static LoanTransaction Simple(
            int loanId = 0,
            int employeeId = 0,
            PayPeriod payPeriod = null)
        {
            LoanTransaction transaction = new LoanTransaction()
            {
                LoanID = loanId,
                EmployeeID = employeeId
            };

            if (payPeriod != null)
            {
                transaction.PayPeriod = payPeriod;
                transaction.PayPeriodID = payPeriod.RowID;
            }

            return transaction;
        }

        public static LoanTransaction WithInterest(
            decimal interestAmount,
            decimal deductionAmount,
            PayPeriod payPeriod,
            int loanId = 0,
            int employeeId = 0)
        {
            var loanTransaction = Simple(loanId, employeeId, payPeriod);

            loanTransaction.InterestAmount = interestAmount;
            loanTransaction.DeductionAmount = deductionAmount;

            return loanTransaction;
        }

        public static List<LoanTransaction> ListWithInterest(
            int listCount,
            decimal interestAmount,
            decimal deductionAmount,
            PayPeriod payPeriod,
            int loanId = 0,
            int employeeId = 0)
        {
            List<LoanTransaction> list = new List<LoanTransaction>();

            for (var index = 1; index <= listCount; index++)
            {
                var loanTransaction = Simple(loanId, employeeId, payPeriod);

                loanTransaction.InterestAmount = interestAmount;
                loanTransaction.DeductionAmount = deductionAmount;

                list.Add(loanTransaction);
            }

            return list;
        }
    }
}
