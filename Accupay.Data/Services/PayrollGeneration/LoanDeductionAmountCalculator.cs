using AccuPay.Data.Entities;
using AccuPay.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class LoanDeductionAmountCalculator
    {
        private readonly IPolicyHelper _policy;

        public LoanDeductionAmountCalculator(IPolicyHelper policy)
        {
            _policy = policy;
        }

        /// <summary>
        /// Used for calculating the deduction amount and interest amount that will be saved in LoanTransaction. Deduction amount is inclusive of interest amount.
        /// </summary>
        /// <param name="loan"></param>
        /// <param name="payPeriod"></param>
        /// <param name="yearlyLoanInterest"></param>
        /// <param name="previousLoanTransactions"></param>
        /// <returns></returns>
        public (decimal deductionAmount, decimal interestAmount, YearlyLoanInterest newYearlyLoanInterest) Calculate(
            LoanSchedule loan,
            PayPeriod payPeriod,
            YearlyLoanInterest yearlyLoanInterest,
            IReadOnlyCollection<LoanTransaction> previousLoanTransactions)
        {
            decimal deductionAmount = 0;
            decimal interestAmount = 0;
            YearlyLoanInterest newYearlyLoanInterest = null;

            if (loan.DeductionAmount > loan.TotalBalanceLeft)
            {
                deductionAmount = loan.TotalBalanceLeft;
            }
            else
            {
                deductionAmount = loan.DeductionAmount;

                if (_policy.UseLoanDeductFromBonus)
                {
                    if (loan.LoanPaymentFromBonuses != null &&
                        loan.LoanPaymentFromBonuses.Any())
                    {
                        deductionAmount = loan.LoanPaymentFromBonuses.Sum(l => l.AmountPayment);
                    }
                }

                if (_policy.UseGoldwingsLoanInterest && loan.IsEligibleForGoldwingsInterest())
                {
                    if (yearlyLoanInterest == null || !yearlyLoanInterest.IsWithInPayPeriod(payPeriod))
                    {
                        yearlyLoanInterest = new YearlyLoanInterest(loan, payPeriod, previousLoanTransactions);
                        newYearlyLoanInterest = yearlyLoanInterest;
                    }

                    (deductionAmount, interestAmount) = yearlyLoanInterest
                        .CalculateCutOffDeduction(deductionAmount, loan, previousLoanTransactions);
                }
            }

            return (
                deductionAmount: deductionAmount,
                interestAmount: interestAmount,
                newYearlyLoanInterest: newYearlyLoanInterest);
        }
    }
}
