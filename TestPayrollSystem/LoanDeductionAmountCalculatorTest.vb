Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Services
Imports Moq

Public Class LoanDeductionAmountCalculatorTest

    Private _loan As LoanSchedule
    Private _payPeriod As PayPeriod
    Private _loanTransactions As List(Of LoanTransaction)
    Private _yearlyLoanInterest As YearlyLoanInterest

    <SetUp>
    Public Sub SetUp()
        _loan = LoanMother.Simple()

        _payPeriod = PayPeriodMother.StartDateOnly(New Date(2020, 10, 15))

        _loanTransactions = New List(Of LoanTransaction)
    End Sub

    <Test>
    Public Sub ShouldReturnLoanDeductionAmountWithInterest_FirstDeduction_WithInterestAndLoanAmountGreaterThanBasicSalary()

        Dim policyHelper As New Mock(Of IPolicyHelper)
        policyHelper.Setup(Function(x) x.UseGoldwingsLoanInterest).Returns(True)

        Dim calculator = New LoanDeductionAmountCalculator(policyHelper.Object)

        'Short Term
        _loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=25_000,
            deductionPercentage:=4,
            basicMonthlySalary:=20_000,
            originalDeductionAmount:=1_500
        )

        Dim deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1508.33D, deductionAmount)

        'Long Term
        _loan = LoanMother.ForYearlyLoanInterest(
            totalLoanAmount:=180_000,
            deductionPercentage:=4,
            basicMonthlySalary:=15_000,
            originalDeductionAmount:=1_500
        )

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1775D, deductionAmount)

    End Sub

    <Test>
    Public Sub ShouldReturnLoanDeductionAmountWithInterest_ShortTerm()

        Dim policyHelper As New Mock(Of IPolicyHelper)
        policyHelper.Setup(Function(x) x.UseGoldwingsLoanInterest).Returns(True)

        Dim calculator = New LoanDeductionAmountCalculator(policyHelper.Object)

        _loanTransactions = New List(Of LoanTransaction)
        Dim principalAmount = 1500D

        '1st month - 1st cut off
        _loan = CreateShortTermLoan(principalAmount)

        Dim deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1508.33D, deductionAmount)

        '1st month - 2nd cut off
        'Created after the first cut off
        _yearlyLoanInterest = New YearlyLoanInterest(_loan, _payPeriod, _loanTransactions)

        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=8.33D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1508.33D, deductionAmount)

        '2nd month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=8.33D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1508.33D, deductionAmount)

        '2nd month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=8.33D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(508.34D, deductionAmount)

        '3rd month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=8.34D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '3rd month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '4th month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '4th month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '5th month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '5th month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '6th month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '6th month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '7th month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '7th month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '8th month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '8th month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '9th month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(1500D, deductionAmount)

        '9th month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(500D, deductionAmount)

        '10th month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(0D, deductionAmount)

        '10th month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(0D, deductionAmount)

        '11th month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(0D, deductionAmount)

        '11th month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(0D, deductionAmount)

        '12th month - 1st cut off
        UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)
        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(0D, deductionAmount)

        '12th month - 2nd cut off
        UpdatePayPeriodAndLoanTransactionsOfFirstHalf(
            interestAmount:=0D,
            deductionAmount:=deductionAmount)

        _loan = CreateShortTermLoan(principalAmount)

        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(0D, deductionAmount)

    End Sub

    Private Sub UpdatePayPeriodAndLoanTransactionsOfFirstHalf(deductionAmount As Decimal, interestAmount As Decimal)
        _payPeriod = PayPeriodMother.StartDateOnly(_payPeriod.PayFromDate.AddDays(15))
        _loanTransactions.Add(LoanTransactionMother.WithInterest(
            interestAmount:=interestAmount,
            deductionAmount:=deductionAmount,
            _payPeriod))
    End Sub

    Private Sub UpdatePayPeriodAndLoanTransactionsOfEndOfTheMonth(deductionAmount As Decimal, interestAmount As Decimal)

        _payPeriod = PayPeriodMother.StartDateOnly(New Date(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month, Date.DaysInMonth(_payPeriod.PayFromDate.Year, _payPeriod.PayFromDate.Month)))
        _loanTransactions.Add(LoanTransactionMother.WithInterest(
            interestAmount:=interestAmount,
            deductionAmount:=deductionAmount,
            _payPeriod))
    End Sub

    Private Function CreateShortTermLoan(principalAmount As Decimal) As LoanSchedule
        Return LoanMother.ForYearlyLoanInterest(
                    totalLoanAmount:=25_000,
                    deductionPercentage:=4,
                    basicMonthlySalary:=20_000,
                    originalDeductionAmount:=principalAmount,
                    loanTransactions:=_loanTransactions)
    End Function

#Region "Normal"

    <TestCase(True)>
    <TestCase(False)>
    Public Sub ShouldReturnLoanDeductionAmount_WithLoanBalanceGreaterThanOrEqualDeductionAmount(useLoanDeductFromBonus As Boolean)

        _loan.DeductionAmount = 1000

        Dim policyHelper As New Mock(Of IPolicyHelper)
        policyHelper.
            Setup(Function(x) x.UseLoanDeductFromBonus).
            Returns(useLoanDeductFromBonus)
        policyHelper.
            Setup(Function(x) x.UseGoldwingsLoanInterest).
            Returns(False)

        Dim calculator = New LoanDeductionAmountCalculator(policyHelper.Object)

        _loan.TotalBalanceLeft = 1000
        Dim deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(_loan.DeductionAmount, deductionAmount)

        _loan.TotalBalanceLeft = 1001
        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(_loan.DeductionAmount, deductionAmount)

        _loan.TotalBalanceLeft = 100000
        deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)
        Assert.AreEqual(_loan.DeductionAmount, deductionAmount)

    End Sub

    <TestCase(True)>
    <TestCase(False)>
    Public Sub ShouldReturnTotalBalanceLeft_WithLoanBalanceLessThanDeductionAmount(useLoanDeductFromBonus As Boolean)

        _loan.DeductionAmount = 1000
        _loan.TotalBalanceLeft = 500

        Dim policyHelper As New Mock(Of IPolicyHelper)
        policyHelper.
            Setup(Function(x) x.UseLoanDeductFromBonus).
            Returns(useLoanDeductFromBonus)
        policyHelper.
            Setup(Function(x) x.UseGoldwingsLoanInterest).
            Returns(False)

        Dim calculator = New LoanDeductionAmountCalculator(policyHelper.Object)
        Dim deductionAmount = calculator.Calculate(_loan, _payPeriod, _yearlyLoanInterest, _loanTransactions)

        Assert.AreEqual(_loan.TotalBalanceLeft, deductionAmount)

    End Sub

#End Region

End Class
