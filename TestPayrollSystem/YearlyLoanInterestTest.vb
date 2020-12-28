Option Strict On

Imports AccuPay.Data.Entities

Public Class YearlyLoanInterestTest

    Private _payPeriod As PayPeriod

    <SetUp>
    Public Sub SetUp()

        _payPeriod = PayPeriodMother.StartDateOnly(New Date(2020, 11, 30))

    End Sub

    <Test>
    Public Sub ShouldBeZero_WithTotalLoanAmountLessThanOrEqualBasicMonthlySalary()

        Dim loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=25_000,
            deductionPercentage:=4,
            basicMonthlySalary:=25_000,
            originalDeductionAmount:=1_500
        )

        'Short Term
        Dim yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, New List(Of LoanTransaction))

        Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsTrue(yearlyLoanInterest.IsZero)

        loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=25_000,
            deductionPercentage:=4,
            basicMonthlySalary:=25_001,
            originalDeductionAmount:=1_500
        )

        yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, New List(Of LoanTransaction))

        Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsTrue(yearlyLoanInterest.IsZero)

        'Long Term
        loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=180_000,
            deductionPercentage:=4,
            basicMonthlySalary:=180_000,
            originalDeductionAmount:=1_500
        )

        yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, New List(Of LoanTransaction))

        Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsTrue(yearlyLoanInterest.IsZero)

        loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=180_000,
            deductionPercentage:=4,
            basicMonthlySalary:=180_001,
            originalDeductionAmount:=1_500
        )

        yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, New List(Of LoanTransaction))

        Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsTrue(yearlyLoanInterest.IsZero)

    End Sub

    <Test>
    Public Sub ShouldComputeCorrectValues_WithNoLoanTransactions()

        Dim loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=25_000,
            deductionPercentage:=4,
            basicMonthlySalary:=20_000,
            originalDeductionAmount:=1_500
        )

        'Short Term
        Dim yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, New List(Of LoanTransaction))

        Assert.AreEqual(5_000, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(33.33, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(8.33, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsFalse(yearlyLoanInterest.IsZero)

        'Long Term
        loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=180_000,
            deductionPercentage:=4,
            basicMonthlySalary:=15_000,
            originalDeductionAmount:=1_500
        )

        yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, New List(Of LoanTransaction))

        Assert.AreEqual(165_000, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(6_600, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(275, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsFalse(yearlyLoanInterest.IsZero)

    End Sub

    <Test>
    Public Sub ShouldComputeCorrectValues_WithLoanTransactions()

        Dim loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=180_000,
            deductionPercentage:=4,
            basicMonthlySalary:=15_000,
            originalDeductionAmount:=1_500
        )

        'Long Term - 2nd year
        Dim firstYearLoanTransactions = LoanTransactionMother.ListWithInterest(
            listCount:=24,
            interestAmount:=275,
            deductionAmount:=1775,
            payPeriod:=_payPeriod)

        Dim yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, firstYearLoanTransactions)

        Assert.AreEqual(129_000, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(5_160, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(215, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsFalse(yearlyLoanInterest.IsZero)

        'Long Term - 3rd year
        Dim secondYearLoanTransactions = LoanTransactionMother.ListWithInterest(
            listCount:=24,
            interestAmount:=215,
            deductionAmount:=1715,
            payPeriod:=_payPeriod)

        Dim loanTransactions = (New List(Of LoanTransaction))
        loanTransactions.AddRange(firstYearLoanTransactions)
        loanTransactions.AddRange(secondYearLoanTransactions)
        yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, loanTransactions)

        Assert.AreEqual(93_000, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(3_720, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(155, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsFalse(yearlyLoanInterest.IsZero)

        'Long Term - 4th Year
        Dim thirdYearLoanTransactions = LoanTransactionMother.ListWithInterest(
            listCount:=24,
            interestAmount:=155,
            deductionAmount:=1655,
            payPeriod:=_payPeriod)

        loanTransactions = (New List(Of LoanTransaction))
        loanTransactions.AddRange(firstYearLoanTransactions)
        loanTransactions.AddRange(secondYearLoanTransactions)
        loanTransactions.AddRange(thirdYearLoanTransactions)
        yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, loanTransactions)

        Assert.AreEqual(57_000, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(2_280, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(95, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsFalse(yearlyLoanInterest.IsZero)

        'Long Term - 5th Year
        Dim fourthYearLoanTransactions = LoanTransactionMother.ListWithInterest(
            listCount:=24,
            interestAmount:=95,
            deductionAmount:=1595,
            payPeriod:=_payPeriod)

        loanTransactions = (New List(Of LoanTransaction))
        loanTransactions.AddRange(firstYearLoanTransactions)
        loanTransactions.AddRange(secondYearLoanTransactions)
        loanTransactions.AddRange(thirdYearLoanTransactions)
        loanTransactions.AddRange(fourthYearLoanTransactions)
        yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, loanTransactions)

        Assert.AreEqual(21_000, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(490, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(35, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsFalse(yearlyLoanInterest.IsZero)

        'Long Term - 5th Year (Overpaid on 4th year)
        Dim fourthYearLoanTransactionsOverPayment = LoanTransactionMother.ListWithInterest(
            listCount:=14,
            interestAmount:=95,
            deductionAmount:=3095,
            payPeriod:=_payPeriod)

        Dim fourthYearLoanTransactionsRemainingPayments = LoanTransactionMother.ListWithInterest(
            listCount:=10,
            interestAmount:=95,
            deductionAmount:=1595,
            payPeriod:=_payPeriod)

        loanTransactions = (New List(Of LoanTransaction))
        loanTransactions.AddRange(firstYearLoanTransactions)
        loanTransactions.AddRange(secondYearLoanTransactions)
        loanTransactions.AddRange(thirdYearLoanTransactions)
        loanTransactions.AddRange(fourthYearLoanTransactionsOverPayment)
        loanTransactions.AddRange(fourthYearLoanTransactionsRemainingPayments)
        yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, loanTransactions)

        Assert.AreEqual(0, yearlyLoanInterest.LoanAmountWithInterest)
        Assert.AreEqual(0, yearlyLoanInterest.LoanInterestAmount)
        Assert.AreEqual(0, yearlyLoanInterest.LoanInterestPerCutOff)
        Assert.IsTrue(yearlyLoanInterest.IsZero)

    End Sub

    <Test>
    Public Sub ShouldCorrectlyCalculateCutOffDeduction_WithNoLoanTransaction()

        Dim originalDeductionAmount = 1500D
        Dim loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=25_000,
            deductionPercentage:=4,
            basicMonthlySalary:=20_000,
            originalDeductionAmount:=originalDeductionAmount
        )

        Dim loanTransactions As New List(Of LoanTransaction)

        Dim yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, loanTransactions)

        Dim deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1508.33D, deductionAmount)

        loanTransactions = Nothing

        yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, loanTransactions)

        deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1508.33D, deductionAmount)

    End Sub

    <Test>
    Public Sub ShouldCorrectlyCalculateCutOffDeduction_WithLoanTransactions()

        Dim originalDeductionAmount = 1500D
        Dim loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=25_000,
            deductionPercentage:=4,
            basicMonthlySalary:=20_000,
            originalDeductionAmount:=originalDeductionAmount
        )

        Dim loanTransactions As New List(Of LoanTransaction)
        Dim yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, loanTransactions)

        '1st month - 1st cut off
        Dim deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1508.33D, deductionAmount)

        '1st month - 2nd cut off
        loanTransactions.Add(LoanTransactionMother.WithInterest(
            interestAmount:=8.33D,
            deductionAmount:=deductionAmount,
            payPeriod:=PayPeriodMother.StartDateOnly(_payPeriod.PayFromDate.AddDays(15))))

        deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1508.33D, deductionAmount)

        '2nd month - 1st cut off
        loanTransactions.Add(LoanTransactionMother.WithInterest(
            interestAmount:=8.33D,
            deductionAmount:=deductionAmount,
            payPeriod:=PayPeriodMother.StartDateOnly(New Date(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month, Date.DaysInMonth(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month)))))

        deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1508.33D, deductionAmount)

        '2nd month - 2nd cut off
        loanTransactions.Add(LoanTransactionMother.WithInterest(
            interestAmount:=8.33D,
            deductionAmount:=deductionAmount,
            payPeriod:=PayPeriodMother.StartDateOnly(_payPeriod.PayFromDate.AddDays(15))))

        deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(508.34D, deductionAmount)

        '3rd month - 1st cut off
        loanTransactions.Add(LoanTransactionMother.WithInterest(
            interestAmount:=8.34D,
            deductionAmount:=deductionAmount,
            payPeriod:=PayPeriodMother.StartDateOnly(New Date(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month, Date.DaysInMonth(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month)))))

        deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1500D, deductionAmount)

    End Sub

    <Test>
    Public Sub ShouldCorrectlyCalculateCutOffDeduction_WithLoanTransactionsButPayPeriodIsNotRight()

        Dim originalDeductionAmount = 1500D
        Dim loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=25_000,
            deductionPercentage:=4,
            basicMonthlySalary:=20_000,
            originalDeductionAmount:=originalDeductionAmount
        )

        Dim loanTransactions As New List(Of LoanTransaction)
        Dim yearlyLoanInterest = New YearlyLoanInterest(loan, _payPeriod, loanTransactions)

        '1st month - 1st cut off
        Dim deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1508.33D, deductionAmount)

        '1st month - 2nd cut off
        loanTransactions.Add(LoanTransactionMother.WithInterest(
            interestAmount:=8.33D,
            deductionAmount:=deductionAmount,
            payPeriod:=PayPeriodMother.StartDateOnly(_payPeriod.PayFromDate.AddDays(15).AddYears(-1))))

        deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1508.33D, deductionAmount)

        '2nd month - 1st cut off
        loanTransactions.Add(LoanTransactionMother.WithInterest(
            interestAmount:=8.33D,
            deductionAmount:=deductionAmount,
            payPeriod:=PayPeriodMother.StartDateOnly((New Date(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month, Date.DaysInMonth(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month)).AddYears(-1)))))

        deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1508.33D, deductionAmount)

        '2nd month - 2nd cut off
        loanTransactions.Add(LoanTransactionMother.WithInterest(
            interestAmount:=8.33D,
            deductionAmount:=deductionAmount,
            payPeriod:=PayPeriodMother.StartDateOnly(_payPeriod.PayFromDate.AddDays(15).AddYears(-1))))

        deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1508.33D, deductionAmount)

        '3rd month - 1st cut off
        loanTransactions.Add(LoanTransactionMother.WithInterest(
            interestAmount:=8.34D,
            deductionAmount:=deductionAmount,
            payPeriod:=PayPeriodMother.StartDateOnly((New Date(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month, Date.DaysInMonth(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month)).AddYears(-1)))))

        deductionAmount = yearlyLoanInterest.
            CalculateCutOffDeduction(originalDeductionAmount, loan, loanTransactions)

        Assert.AreEqual(1508.33D, deductionAmount)

    End Sub

End Class
