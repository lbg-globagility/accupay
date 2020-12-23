using AccuPay.Data.Helpers;
using AccuPay.Utilities;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("yearlyloaninterest")]
    public class YearlyLoanInterest : IAuditableEntity
    {
        public int LoanScheduleId { get; private set; }

        public int Year { get; private set; }

        public DateTime Date { get; private set; }

        public LoanSchedule LoanSchedule { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; private set; }

        public int? CreatedBy { get; internal set; }

        public int? LastUpdBy { get; internal set; }

        /// <summary>
        /// The loan amount this year that will be computed with interest.
        /// </summary>
        public decimal LoanAmountWithInterest { get; set; }

        /// <summary>
        /// The interest needed to be paid for this year. = LoanAmountWithInterest * Loan Interest Percentage
        /// </summary>
        public decimal LoanInterestAmount { get; set; }

        /// <summary>
        /// The interest amount that is going to be included to the total deduction amount that the employee pays every cut off.
        /// </summary>
        public decimal LoanInterestPerCutOff { get; set; }

        // default constructor for EF Core.
        private YearlyLoanInterest() { }

        public YearlyLoanInterest(LoanSchedule loan, PayPeriod payPeriod, IReadOnlyCollection<LoanTransaction> loanTransactions)
        {
            if (loan == null)
                throw new ArgumentNullException($"{nameof(loan)} cannot be null.");

            if (payPeriod == null)
                throw new ArgumentNullException($"{nameof(payPeriod)} cannot be null.");

            if (!loan.IsEligibleForGoldwingsInterest())
                return;

            LoanScheduleId = loan.RowID.Value;
            Year = payPeriod.PayFromDate.Year;
            Date = payPeriod.PayFromDate;

            decimal requestedLoan = AccuMath.CommercialRound(loan.TotalLoanAmount); // 25_000, 180_000
            decimal interestRate = AccuMath.CommercialRound(loan.DeductionPercentage); // 4, 4
            decimal maximumLoanAmount = AccuMath.CommercialRound(loan.BasicMonthlySalary); // 20_000, 15_000
            decimal principalDeductionAmount = AccuMath.CommercialRound(loan.OriginalDeductionAmount); // 1_500, 1_500

            decimal amountWithOutInterest = maximumLoanAmount; // 20_000, 15_000
            decimal amountWithInterest = requestedLoan - maximumLoanAmount; // 5_000, 165_000

            decimal totalPaidPrincipal = loanTransactions?.Sum(x => x.PrincipalAmount) ?? 0; // 0, 0

            decimal amountWithInterestForThisYear = amountWithInterest - totalPaidPrincipal; // 5_000, 165_000~21_000
            if (amountWithInterestForThisYear < 0) amountWithInterestForThisYear = 0;

            decimal interestForThisYear = GetInterestForThisYear(
                principalDeductionAmount: principalDeductionAmount,
                amountWithInterestForThisYear: amountWithInterestForThisYear,
                interestRate: interestRate); // 33.33, 6_600~490

            LoanAmountWithInterest = amountWithInterestForThisYear; // 5_000, 165_000~21_000

            if (LoanAmountWithInterest == 0)
            {
                LoanInterestAmount = 0;
                LoanInterestPerCutOff = 0;
            }
            else
            {
                LoanInterestAmount = interestForThisYear; // 33.33, 6_600~490
                LoanInterestPerCutOff = AccuMath.CommercialRound(
                    LoanInterestAmount /
                    CountSemiMonthlyCutOffInterestPaymentOfTheYear(
                        principalDeductionAmount: principalDeductionAmount,
                        amountWithInterest: LoanAmountWithInterest)); // 8.33, 275~35
            }
        }

        public (decimal deductionAmount, decimal interestAmount) CalculateCutOffDeduction(decimal deductionAmount, LoanSchedule loan, IReadOnlyCollection<LoanTransaction> allLoanTransactions)
        {
            if (IsZero)
            {
                return (deductionAmount: deductionAmount, interestAmount: 0);
            }

            var currentDate = Date.ToMinimumHourValue();

            allLoanTransactions = allLoanTransactions ?? new List<LoanTransaction>();

            var currentLoanTransactions = allLoanTransactions?
                .Where(x => x.PayPeriod.PayFromDate.ToMinimumHourValue() >= currentDate)
                .Where(x => x.PayPeriod.PayFromDate.ToMinimumHourValue() <= currentDate.AddYears(1).AddDays(-1))
                .Where(x => x.InterestAmount != 0);

            var interestPaymentCount = currentLoanTransactions.Count();

            var maxInterestPaymentCount = CountSemiMonthlyCutOffInterestPayment(
                principalDeductionAmount: loan.OriginalDeductionAmount,
                amountWithInterest: LoanAmountWithInterest);

            if (interestPaymentCount >= maxInterestPaymentCount)
            {
                // interests are already paid for this year
                return (deductionAmount: deductionAmount, interestAmount: 0);
            }

            bool nextDeductionIsLastInterest = maxInterestPaymentCount - 1 == interestPaymentCount;

            if (!nextDeductionIsLastInterest)
            {
                // interests are still not fully paid for this year
                return (
                    deductionAmount: deductionAmount + LoanInterestPerCutOff,
                    interestAmount: LoanInterestPerCutOff);
            }
            else
            {
                // this is the last interest payment for this year
                decimal interestThisCutOff =
                    LoanInterestAmount -
                    currentLoanTransactions.Sum(x => x.InterestAmount);

                decimal principalThisCutOff = deductionAmount;

                var totalPaidPrincipalAmount = allLoanTransactions
                    .Where(x => x.InterestAmount != 0)
                    .Sum(x => x.PrincipalAmount);

                decimal totalAmountWithInterest = AccuMath.CommercialRound(loan.TotalLoanAmount) - AccuMath.CommercialRound(loan.BasicMonthlySalary); // 5_000, 165_000

                if (totalAmountWithInterest - totalPaidPrincipalAmount <= loan.OriginalDeductionAmount)
                {
                    // this is the last principal payment for the whole loan
                    principalThisCutOff = totalAmountWithInterest - totalPaidPrincipalAmount;
                }

                return (
                    deductionAmount: principalThisCutOff + interestThisCutOff,
                    interestAmount: interestThisCutOff);
            }
        }

        public bool IsZero =>
            LoanAmountWithInterest == 0 &&
            LoanInterestAmount == 0 &&
            LoanInterestPerCutOff == 0;

        public bool IsNewEntity => Created == DateTime.MinValue;

        public bool IsWithInPayPeriod(PayPeriod payPeriod) =>
            payPeriod.PayFromDate.ToMinimumHourValue() >= Date.ToMinimumHourValue() &&
            payPeriod.PayFromDate.ToMinimumHourValue() < Date.AddYears(1).ToMinimumHourValue();

        #region Private Methods

        private decimal GetInterestForThisYear(
            decimal principalDeductionAmount /* 1_500, 1_500 */,
            decimal amountWithInterestForThisYear /* 5_000, 165_000~21_000 */,
            decimal interestRate /* 4, 4 */)
        {
            if (amountWithInterestForThisYear <= 0 || interestRate <= 0)
            {
                return 0;
            }

            decimal interestForThisYear = AccuMath.CommercialRound(amountWithInterestForThisYear * interestRate / 100); // 200, 6_600~840

            decimal remainingInterestSemiMonthlyCutOffs =
                    CountSemiMonthlyCutOffInterestPaymentOfTheYear(principalDeductionAmount, amountWithInterestForThisYear); // 4, 24~14

            if (remainingInterestSemiMonthlyCutOffs < CalendarConstant.SemiMonthlyPayPeriodsPerYear)
            {
                interestForThisYear = // 33.33, 490
                    interestForThisYear *
                    (GetRemainingCutOffPercentage(remainingInterestSemiMonthlyCutOffs)); // (200 * 4 / 24), (840 * 14 / 24)
            }

            return AccuMath.CommercialRound(interestForThisYear);
        }

        private static decimal GetRemainingCutOffPercentage(decimal remainingInterestSemiMonthlyCutOffs)
        {
            return remainingInterestSemiMonthlyCutOffs / CalendarConstant.SemiMonthlyPayPeriodsPerYear;
        }

        private static decimal CountSemiMonthlyCutOffInterestPaymentOfTheYear(decimal principalDeductionAmount, decimal amountWithInterest)
        {
            decimal remainingInterestSemiMonthlyCutOffs = CountSemiMonthlyCutOffInterestPayment(
                principalDeductionAmount: principalDeductionAmount,
                amountWithInterest: amountWithInterest);

            return remainingInterestSemiMonthlyCutOffs > CalendarConstant.SemiMonthlyPayPeriodsPerYear ?
                CalendarConstant.SemiMonthlyPayPeriodsPerYear :
                remainingInterestSemiMonthlyCutOffs;
        }

        private static decimal CountSemiMonthlyCutOffInterestPayment(decimal principalDeductionAmount, decimal amountWithInterest)
        {
            if (principalDeductionAmount == 0) return 0;

            return Math.Ceiling(amountWithInterest / principalDeductionAmount);
        }

        #endregion Private Methods
    }
}
