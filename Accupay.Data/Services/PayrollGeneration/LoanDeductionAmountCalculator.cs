using AccuPay.Data.Entities;
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
        /// <param name="bonusLoanPayments"></param>
        /// <param name="thirteenthMonthPayLoanPayments"></param>
        /// <returns></returns>
        public (decimal deductionAmount, decimal interestAmount, YearlyLoanInterest newYearlyLoanInterest) Calculate(
            LoanSchedule loan,
            PayPeriod payPeriod,
            YearlyLoanInterest yearlyLoanInterest,
            IReadOnlyCollection<LoanTransaction> previousLoanTransactions = null,
            IReadOnlyCollection<LoanPaymentFromBonus> bonusLoanPayments = null,
            IReadOnlyCollection<LoanPaymentFromThirteenthMonthPay> thirteenthMonthPayLoanPayments = null)
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
                    if (bonusLoanPayments != null && bonusLoanPayments.Any())
                    {
                        deductionAmount = bonusLoanPayments.Sum(l => l.AmountPayment);
                        if (deductionAmount > loan.TotalBalanceLeft)
                        {
                            deductionAmount = loan.TotalBalanceLeft;
                        }
                    }
                }

                if (_policy.UseLoanDeductFromThirteenthMonthPay)
                {
                    if (thirteenthMonthPayLoanPayments != null && thirteenthMonthPayLoanPayments.Any())
                    {
                        var addedLoanDeduction = thirteenthMonthPayLoanPayments
                            .Where(l => l.LoanId == loan.RowID.Value)
                            .Sum(l => l.AmountPayment);

                        if ((addedLoanDeduction + deductionAmount) > loan.TotalBalanceLeft)
                        {
                            deductionAmount = loan.TotalBalanceLeft;
                        }

                        deductionAmount += addedLoanDeduction;
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
