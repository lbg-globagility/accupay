using AccuPay.Core.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AccuPay.Core.UnitTests.Loans
{
    public class YearlyLoanInterestTest
    {
        private PayPeriod _payPeriod;

        [SetUp]
        public void SetUp()
        {
            _payPeriod = PayPeriodMother.StartDateOnly(new DateTime(2020, 11, 30));
        }

        [Test]
        public void ShouldBeZero_WithTotalLoanAmountLessThanOrEqualBasicMonthlySalary()
        {
            var loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 4,
                basicMonthlySalary: 25_000,
                originalDeductionAmount: 1_500
            );

            // Short Term
            var yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, new List<LoanTransaction>());

            Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsTrue(yearlyLoanInterest.IsZero);

            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 4,
                basicMonthlySalary: 25_001,
                originalDeductionAmount: 1_500
            );

            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, new List<LoanTransaction>());

            Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsTrue(yearlyLoanInterest.IsZero);

            // Long Term
            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 4,
                basicMonthlySalary: 180_000,
                originalDeductionAmount: 1_500
            );

            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, new List<LoanTransaction>());

            Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsTrue(yearlyLoanInterest.IsZero);

            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 4,
                basicMonthlySalary: 180_001,
                originalDeductionAmount: 1_500
            );

            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, new List<LoanTransaction>());

            Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsTrue(yearlyLoanInterest.IsZero);
        }

        [Test]
        public void ShouldBeZero_WithInterestPercentageLessThanZero()
        {
            var loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 0,
                basicMonthlySalary: 20_000,
                originalDeductionAmount: 1_500
            );

            // Short Term
            var yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, new List<LoanTransaction>());

            Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsTrue(yearlyLoanInterest.IsZero);

            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 0,
                basicMonthlySalary: 20_001,
                originalDeductionAmount: 1_500
            );

            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, new List<LoanTransaction>());

            Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsTrue(yearlyLoanInterest.IsZero);

            // Long Term
            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 0,
                basicMonthlySalary: 100_000,
                originalDeductionAmount: 1_500
            );

            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, new List<LoanTransaction>());

            Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsTrue(yearlyLoanInterest.IsZero);

            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 0,
                basicMonthlySalary: 100_001,
                originalDeductionAmount: 1_500
            );

            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, new List<LoanTransaction>());

            Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsTrue(yearlyLoanInterest.IsZero);
        }

        [Test]
        public void ShouldComputeCorrectValues_WithNoLoanTransactions()
        {
            var loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 4,
                basicMonthlySalary: 20_000,
                originalDeductionAmount: 1_500);

            // Short Term
            var yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, new List<LoanTransaction>());

            Assert.AreEqual(5_000, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(33.33, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(8.33, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsFalse(yearlyLoanInterest.IsZero);

            // Long Term
            loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 180_000,
                deductionPercentage: 4, basicMonthlySalary: 15_000, originalDeductionAmount: 1_500
    );

            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, new List<LoanTransaction>());

            Assert.AreEqual(165_000, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(6_600, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(275, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsFalse(yearlyLoanInterest.IsZero);
        }

        [Test]
        public void ShouldComputeCorrectValues_WithLoanTransactions()
        {
            var loan = LoanMother.ForYearlyLoanInterest(totalLoanAmount: 180_000, deductionPercentage: 4, basicMonthlySalary: 15_000, originalDeductionAmount: 1_500
    );

            // Long Term - 2nd year
            var firstYearLoanTransactions = LoanTransactionMother.ListWithInterest(listCount: 24, interestAmount: 275, deductionAmount: 1775, payPeriod: _payPeriod);

            var yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, firstYearLoanTransactions);

            Assert.AreEqual(129_000, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(5_160, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(215, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsFalse(yearlyLoanInterest.IsZero);

            // Long Term - 3rd year
            var secondYearLoanTransactions = LoanTransactionMother.ListWithInterest(listCount: 24, interestAmount: 215, deductionAmount: 1715, payPeriod: _payPeriod);

            var loanTransactions = (new List<LoanTransaction>());
            loanTransactions.AddRange(firstYearLoanTransactions);
            loanTransactions.AddRange(secondYearLoanTransactions);
            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, loanTransactions);

            Assert.AreEqual(93_000, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(3_720, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(155, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsFalse(yearlyLoanInterest.IsZero);

            // Long Term - 4th Year
            var thirdYearLoanTransactions = LoanTransactionMother.ListWithInterest(listCount: 24, interestAmount: 155, deductionAmount: 1655, payPeriod: _payPeriod);

            loanTransactions = (new List<LoanTransaction>());
            loanTransactions.AddRange(firstYearLoanTransactions);
            loanTransactions.AddRange(secondYearLoanTransactions);
            loanTransactions.AddRange(thirdYearLoanTransactions);
            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, loanTransactions);

            Assert.AreEqual(57_000, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(2_280, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(95, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsFalse(yearlyLoanInterest.IsZero);

            // Long Term - 5th Year
            var fourthYearLoanTransactions = LoanTransactionMother.ListWithInterest(listCount: 24, interestAmount: 95, deductionAmount: 1595, payPeriod: _payPeriod);

            loanTransactions = (new List<LoanTransaction>());
            loanTransactions.AddRange(firstYearLoanTransactions);
            loanTransactions.AddRange(secondYearLoanTransactions);
            loanTransactions.AddRange(thirdYearLoanTransactions);
            loanTransactions.AddRange(fourthYearLoanTransactions);
            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, loanTransactions);

            Assert.AreEqual(21_000, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(490, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(35, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsFalse(yearlyLoanInterest.IsZero);

            // Long Term - 5th Year (Overpaid on 4th year)
            var fourthYearLoanTransactionsOverPayment = LoanTransactionMother.ListWithInterest(listCount: 14, interestAmount: 95, deductionAmount: 3095, payPeriod: _payPeriod);

            var fourthYearLoanTransactionsRemainingPayments = LoanTransactionMother.ListWithInterest(listCount: 10, interestAmount: 95, deductionAmount: 1595, payPeriod: _payPeriod);

            loanTransactions = (new List<LoanTransaction>());
            loanTransactions.AddRange(firstYearLoanTransactions);
            loanTransactions.AddRange(secondYearLoanTransactions);
            loanTransactions.AddRange(thirdYearLoanTransactions);
            loanTransactions.AddRange(fourthYearLoanTransactionsOverPayment);
            loanTransactions.AddRange(fourthYearLoanTransactionsRemainingPayments);
            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, loanTransactions);

            Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount);
            Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff);
            Assert.IsTrue(yearlyLoanInterest.IsZero);
        }

        [Test]
        public void ShouldCorrectlyCalculateCutOffDeduction_WithNoLoanTransaction()
        {
            var originalDeductionAmount = 1500M;
            var loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 4,
                basicMonthlySalary: 20_000,
                originalDeductionAmount: originalDeductionAmount);

            List<LoanTransaction> loanTransactions = new List<LoanTransaction>();

            var yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, loanTransactions);

            var calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            loanTransactions = null;

            yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, loanTransactions);

            calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);
        }

        [Test]
        public void ShouldCorrectlyCalculateCutOffDeduction_WithLoanTransactions()
        {
            var originalDeductionAmount = 1500M;
            var loan = LoanMother.ForYearlyLoanInterest(
                totalLoanAmount: 25_000,
                deductionPercentage: 4,
                basicMonthlySalary: 20_000,
                originalDeductionAmount: originalDeductionAmount);

            List<LoanTransaction> loanTransactions = new List<LoanTransaction>();
            var yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, loanTransactions);

            // 1st month - 1st cut off
            var calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            // 1st month - 2nd cut off
            loanTransactions.Add(LoanTransactionMother.WithInterest(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount,
                payPeriod: PayPeriodMother.StartDateOnly(_payPeriod.PayFromDate.AddDays(15))));

            calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            // 2nd month - 1st cut off
            loanTransactions.Add(LoanTransactionMother.WithInterest(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount,
                payPeriod: PayPeriodMother.StartDateOnly(new DateTime(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month, DateTime.DaysInMonth(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month)))));

            calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            // 2nd month - 2nd cut off
            loanTransactions.Add(LoanTransactionMother.WithInterest(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount,
                payPeriod: PayPeriodMother.StartDateOnly(_payPeriod.PayFromDate.AddDays(15))));

            calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.34M, calculation.interestAmount);
            Assert.AreEqual(508.34M, calculation.deductionAmount);

            // 3rd month - 1st cut off
            loanTransactions.Add(LoanTransactionMother.WithInterest(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount,
                payPeriod: PayPeriodMother.StartDateOnly(new DateTime(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month, DateTime.DaysInMonth(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month)))));

            calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(0M, calculation.interestAmount);
            Assert.AreEqual(1500M, calculation.deductionAmount);
        }

        [Test]
        public void ShouldCorrectlyCalculateCutOffDeduction_WithLoanTransactionsButPayPeriodIsNotRight()
        {
            var originalDeductionAmount = 1500M;
            var loan = LoanMother.ForYearlyLoanInterest(totalLoanAmount: 25_000, deductionPercentage: 4, basicMonthlySalary: 20_000, originalDeductionAmount: originalDeductionAmount
    );

            List<LoanTransaction> loanTransactions = new List<LoanTransaction>();
            var yearlyLoanInterest = new YearlyLoanInterest(loan, _payPeriod, loanTransactions);

            // 1st month - 1st cut off
            var calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            // 1st month - 2nd cut off
            loanTransactions.Add(LoanTransactionMother.WithInterest(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount,
                payPeriod: PayPeriodMother.StartDateOnly(_payPeriod.PayFromDate.AddDays(15).AddYears(-1))));

            calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            // 2nd month - 1st cut off
            loanTransactions.Add(LoanTransactionMother.WithInterest(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount,
                payPeriod: PayPeriodMother.StartDateOnly((new DateTime(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month, DateTime.DaysInMonth(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month)).AddYears(-1)))));

            calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            // 2nd month - 2nd cut off
            loanTransactions.Add(LoanTransactionMother.WithInterest(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount,
                payPeriod: PayPeriodMother.StartDateOnly(_payPeriod.PayFromDate.AddDays(15).AddYears(-1))));

            calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);

            // 3rd month - 1st cut off
            loanTransactions.Add(LoanTransactionMother.WithInterest(
                interestAmount: calculation.interestAmount,
                deductionAmount: calculation.deductionAmount,
                payPeriod: PayPeriodMother.StartDateOnly((new DateTime(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month, DateTime.DaysInMonth(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month)).AddYears(-1)))));

            calculation = yearlyLoanInterest.CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions);

            Assert.AreEqual(8.33M, calculation.interestAmount);
            Assert.AreEqual(1508.33M, calculation.deductionAmount);
        }
    }
}
