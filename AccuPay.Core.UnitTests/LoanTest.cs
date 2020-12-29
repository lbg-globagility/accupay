using AccuPay.Data.Entities;
using NUnit.Framework;

namespace AccuPay.Core.UnitTests
{
    public class LoanTest
    {
        [TestCase(20_000, 2_000, 10)]
        [TestCase(20_000, 1_500, 14)]
        [TestCase(20_000, 11_000, 2)]
        [TestCase(20_000, 20_000, 1)]
        public void ShouldComputePayPeriodLeftToGreaterThanZero_WithTotalBalanceGreaterThanDeductionAmount(
            int totalBalanceLeft,
            int deductionAmount,
            int loanPayPeriodLeft)
        {
            var loan = new LoanSchedule()
            {
                TotalLoanAmount = 20_000,
                TotalBalanceLeft = totalBalanceLeft,
                DeductionAmount = deductionAmount
            };

            loan.RecomputePayPeriodLeft();

            Assert.AreEqual(loanPayPeriodLeft, loan.LoanPayPeriodLeft);
        }

        [TestCase(0, 2_000)]
        [TestCase(20_000, 0)]
        [TestCase(0, 0)]
        public void ShouldComputePayPeriodLeftToZero(
            int totalBalanceLeft,
            int deductionAmount)
        {
            var loan = new LoanSchedule()
            {
                TotalLoanAmount = 20_000,
                TotalBalanceLeft = totalBalanceLeft,
                DeductionAmount = deductionAmount
            };

            loan.RecomputePayPeriodLeft();

            Assert.AreEqual(0, loan.LoanPayPeriodLeft);
        }
    }
}
