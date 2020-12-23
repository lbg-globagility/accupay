using AccuPay.Data.Entities;
using System.Collections.Generic;

namespace AccuPay.Core.UnitTests.Mothers
{
    public class LoanTransactionMother
    {
        public static LoanTransaction Simple(
            int loanScheduleId = 0,
            int employeeId = 0,
            PayPeriod payPeriod = null)
        {
            LoanTransaction transaction = new LoanTransaction()
            {
                LoanScheduleID = loanScheduleId,
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
            int loanScheduleId = 0,
            int employeeId = 0)
        {
            var loanTransaction = Simple(loanScheduleId, employeeId, payPeriod);

            loanTransaction.InterestAmount = interestAmount;
            loanTransaction.DeductionAmount = deductionAmount;

            return loanTransaction;
        }

        public static List<LoanTransaction> ListWithInterest(
            int listCount,
            decimal interestAmount,
            decimal deductionAmount,
            PayPeriod payPeriod,
            int loanScheduleId = 0,
            int employeeId = 0)
        {
            List<LoanTransaction> list = new List<LoanTransaction>();

            for (var index = 1; index <= listCount; index++)
            {
                var loanTransaction = Simple(loanScheduleId, employeeId, payPeriod);

                loanTransaction.InterestAmount = interestAmount;
                loanTransaction.DeductionAmount = deductionAmount;

                list.Add(loanTransaction);
            }

            return list;
        }
    }
}
