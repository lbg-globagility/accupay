using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using AccuPay.Core.UnitTests.Mothers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AccuPay.Core.UnitTests
{
    public class LoanDeductionAmountCalculatorTest
    {
        private Loan _loan;
        private PayPeriod _payPeriod;
        private List<LoanTransaction> _loanTransactions;
        private YearlyLoanInterest _yearlyLoanInterest;

        [SetUp]
        public void SetUp()
        {
            _loan = LoanMother.Simple();

            _payPeriod = PayPeriodMother.StartDateOnly(new DateTime(2020, 11, 15));

            _loanTransactions = new List<LoanTransaction>();
        }

        [Test]
        public void InterestAmountShouldBeZero_WithTotalLoanAmountLessThanOrEqualBasicMonthlySalary()
        {
            Mock<IPolicyHelper> policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.UseGoldwingsLoanInterest).Returns(true);

            var calculator = new LoanDeductionAmountCalculator(policyHelper.Object);

            // Short Term

            var loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 4,
                basicMonthlySalary: 25_000,
                originalDeductionAmount: 1_500
            );

            var calculation = calculator.Calculate(loan, _payPeriod, null, new List<LoanTransaction>());
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1_500M, calculation.deductionAmount);
            Assert.IsNull(calculation.newYearlyLoanInterest);

            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 4,
                basicMonthlySalary: 25_001,
                originalDeductionAmount: 1_500
            );

            calculation = calculator.Calculate(loan, _payPeriod, null, new List<LoanTransaction>());
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1_500M, calculation.deductionAmount);
            Assert.IsNull(calculation.newYearlyLoanInterest);

            // Long Term
            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 4,
                basicMonthlySalary: 180_000,
                originalDeductionAmount: 1_500
            );

            calculation = calculator.Calculate(loan, _payPeriod, null, new List<LoanTransaction>());
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1_500M, calculation.deductionAmount);
            Assert.IsNull(calculation.newYearlyLoanInterest);

            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 4,
                basicMonthlySalary: 180_001,
                originalDeductionAmount: 1_500
            );

            calculation = calculator.Calculate(loan, _payPeriod, null, new List<LoanTransaction>());
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1_500M, calculation.deductionAmount);
            Assert.IsNull(calculation.newYearlyLoanInterest);
        }

        [Test]
        public void InterestAmountShouldBeZero_WithInterestPercentageLessThanZero()
        {
            Mock<IPolicyHelper> policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.UseGoldwingsLoanInterest).Returns(true);

            var calculator = new LoanDeductionAmountCalculator(policyHelper.Object);

            var loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 0,
                basicMonthlySalary: 20_000,
                originalDeductionAmount: 1_500
            );

            // Short Term
            var calculation = calculator.Calculate(loan, _payPeriod, null, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1_500M, calculation.deductionAmount);
            Assert.IsNull(calculation.newYearlyLoanInterest);

            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 0,
                basicMonthlySalary: 20_001,
                originalDeductionAmount: 1_500
            );

            calculation = calculator.Calculate(loan, _payPeriod, null, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1_500M, calculation.deductionAmount);
            Assert.IsNull(calculation.newYearlyLoanInterest);

            // Long Term
            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 0,
                basicMonthlySalary: 100_000,
                originalDeductionAmount: 1_500
            );

            calculation = calculator.Calculate(loan, _payPeriod, null, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1_500M, calculation.deductionAmount);
            Assert.IsNull(calculation.newYearlyLoanInterest);

            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 0,
                basicMonthlySalary: 100_001,
                originalDeductionAmount: 1_500
            );

            calculation = calculator.Calculate(loan, _payPeriod, null, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1_500M, calculation.deductionAmount);
            Assert.IsNull(calculation.newYearlyLoanInterest);
        }

        [Test]
        public void ShouldReturnLoanDeductionAmountWithInterest_FirstDeduction_WithInterestAndLoanAmountGreaterThanBasicSalary()
        {
            Mock<IPolicyHelper> policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.UseGoldwingsLoanInterest).Returns(true);

            var calculator = new LoanDeductionAmountCalculator(policyHelper.Object);

            // Short Term
            _loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 4,
                basicMonthlySalary: 20_000,
                originalDeductionAmount: 1_500);

            var calculation = calculator.Calculate(_loan, _payPeriod, null, _loanTransactions);
            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            // Long Term
            _loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 4,
                basicMonthlySalary: 15_000,
                originalDeductionAmount: 1_500);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);
        }

        [Test]
        public void ShouldReturnLoanDeductionAmountWithInterest_ShortTerm()
        {
            Mock<IPolicyHelper> policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.UseGoldwingsLoanInterest).Returns(true);

            var calculator = new LoanDeductionAmountCalculator(policyHelper.Object);

            _loanTransactions = new List<LoanTransaction>();
            var principalAmount = 1500M;

            // 1st month - 1st cut off
            _loan = CreateShortTermLoan(principalAmount);

            var calculation = calculator.Calculate(_loan, _payPeriod, null, _loanTransactions);
            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);
            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(8.34M, calculation.interestAmount);
            Assert.AreEqual(508.34M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateShortTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;
        }

        [Test]
        public void ShouldReturnLoanDeductionAmountWithInterest_LongTerm()
        {
            Mock<IPolicyHelper> policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.UseGoldwingsLoanInterest).Returns(true);

            var calculator = new LoanDeductionAmountCalculator(policyHelper.Object);

            _loanTransactions = new List<LoanTransaction>();
            var principalAmount = 1500M;

            // 1st year - 1st month - 1st cut off
            _loan = CreateLongTermLoan(principalAmount);

            var calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 1st month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 1st month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 1st month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 1st month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(35M, calculation.interestAmount);
            Assert.AreEqual(1535M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;
        }

        [Test]
        public void ShouldReturnLoanDeductionAmountWithInterest_LongTermWithOverPayment()
        {
            Mock<IPolicyHelper> policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.UseGoldwingsLoanInterest).Returns(true);

            var calculator = new LoanDeductionAmountCalculator(policyHelper.Object);

            _loanTransactions = new List<LoanTransaction>();
            var principalAmount = 1500M;

            // 1st year - 1st month - 1st cut off
            _loan = CreateLongTermLoan(principalAmount);

            var calculation = calculator.Calculate(_loan, _payPeriod, null, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 1st year - 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(275M, calculation.interestAmount);
            Assert.AreEqual(1775M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 1st month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 2nd year - 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(215M, calculation.interestAmount);
            Assert.AreEqual(1715M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 1st month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 3rd year - 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(155M, calculation.interestAmount);
            Assert.AreEqual(1655M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 1st month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);
            _loan.DeductionAmount = 3000;

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(3095M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 4th year - 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(95M, calculation.interestAmount);
            Assert.AreEqual(1595M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 1st month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 1st month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 2nd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount); ;

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 2nd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 3rd month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 3rd month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 4th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 4th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 5th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 5th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 6th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 6th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 7th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount); ;

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 7th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 8th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 8th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 9th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 9th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 10th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 10th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 11th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 11th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 12th month - 1st cut off
            UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;

            // 5th year - 12th month - 2nd cut off
            UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount);

            _loan = CreateLongTermLoan(principalAmount);

            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(0M, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;
        }

        private void UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            decimal deductionAmount,
            decimal interestAmount)
        {
            // this loan transaction is the one saved by the previous payroll
            // so its payperiod should be the previous one
            _loanTransactions.Add(LoanTransactionMother.WithInterest(
                interestAmount: interestAmount,
                deductionAmount: deductionAmount,
                _payPeriod));

            _payPeriod = PayPeriodMother.StartDateOnly(_payPeriod.PayFromDate.AddDays(15));
        }

        private void UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(decimal deductionAmount, decimal interestAmount)
        {
            // this loan transaction is the one saved by the previous payroll
            // so its payperiod should be the previous one
            _loanTransactions.Add(LoanTransactionMother.WithInterest(
                interestAmount: interestAmount,
                deductionAmount: deductionAmount,
                _payPeriod));

            _payPeriod = PayPeriodMother.StartDateOnly(new DateTime(
                _payPeriod.PayFromDate.Year,
                _payPeriod.PayFromDate.Month,
                DateTime.DaysInMonth(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month)));
        }

        private Loan CreateShortTermLoan(decimal principalAmount)
        {
            return LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 4,
                basicMonthlySalary: 20_000,
                originalDeductionAmount: principalAmount,
                loanTransactions: _loanTransactions);
        }

        private Loan CreateLongTermLoan(decimal principalAmount)
        {
            return LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 4,
                basicMonthlySalary: 15_000,
                originalDeductionAmount: principalAmount,
                loanTransactions: _loanTransactions);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnLoanDeductionAmount_WithLoanBalanceGreaterThanOrEqualDeductionAmount(bool useLoanDeductFromBonus)
        {
            _loan.DeductionAmount = 1000;

            Mock<IPolicyHelper> policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.UseLoanDeductFromBonus).Returns(useLoanDeductFromBonus);
            policyHelper.Setup(x => x.UseGoldwingsLoanInterest).Returns(false);

            var calculator = new LoanDeductionAmountCalculator(policyHelper.Object);

            _loan.TotalBalanceLeft = 1000;
            var calculation = calculator.Calculate(_loan, _payPeriod, null, _loanTransactions);
            Assert.AreEqual(_loan.DeductionAmount, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;
            _loan.TotalBalanceLeft = 1001;
            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(_loan.DeductionAmount, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;
            _loan.TotalBalanceLeft = 100000;
            calculation = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions);
            Assert.AreEqual(_loan.DeductionAmount, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnTotalBalanceLeft_WithLoanBalanceLessThanDeductionAmount(bool useLoanDeductFromBonus)
        {
            _loan.DeductionAmount = 1000;
            _loan.TotalBalanceLeft = 500;

            Mock<IPolicyHelper> policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.UseLoanDeductFromBonus).Returns(useLoanDeductFromBonus);
            policyHelper.Setup(x => x.UseGoldwingsLoanInterest).Returns(false);

            var calculator = new LoanDeductionAmountCalculator(policyHelper.Object);
            var calculation = calculator.Calculate(_loan, _payPeriod, null, _loanTransactions);

            Assert.AreEqual(_loan.TotalBalanceLeft, calculation.deductionAmount);

            if (calculation.newYearlyLoanInterest != null) _yearlyLoanInterest = calculation.newYearlyLoanInterest;
        }
    }
}
