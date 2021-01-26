Option Strict On

Imports AccuPay.Core.Entities
Imports Moq

Public Class LoanMother

    Public Shared Function Simple() As LoanSchedule

        Return New LoanSchedule() With {
            .RowID = It.IsAny(Of Integer),
            .EmployeeID = It.IsAny(Of Integer),
            .OrganizationID = It.IsAny(Of Integer),
            .LoanTypeID = It.IsAny(Of Integer),
            .LoanNumber = It.IsAny(Of String),
            .TotalLoanAmount = It.IsAny(Of Decimal),
            .TotalBalanceLeft = It.IsAny(Of Decimal),
            .DedEffectiveDateFrom = It.IsAny(Of Date),
            .DeductionAmount = It.IsAny(Of Decimal),
            .Status = It.IsAny(Of String),
            .DeductionPercentage = It.IsAny(Of Decimal),
            .DeductionSchedule = It.IsAny(Of String),
            .Comments = It.IsAny(Of String)
        }

    End Function

    Public Shared Function WithLoanAmountAndDeductionAmount(
        totalLoanAmount As Decimal,
        originalDeductionAmount As Decimal,
        Optional loanTransactions As List(Of LoanTransaction) = Nothing) As LoanSchedule

        Dim loan = Simple()

        loan.TotalLoanAmount = totalLoanAmount
        loan.TotalBalanceLeft = loan.TotalLoanAmount
        loan.OriginalDeductionAmount = originalDeductionAmount
        loan.DeductionAmount = loan.OriginalDeductionAmount

        loan.LoanTransactions = loanTransactions
        loan.TotalBalanceLeft -= If(loanTransactions?.Sum(Function(l) l.PrincipalAmount), 0)

        Return loan
    End Function

    Public Shared Function ForYearlyLoanInterest(
        totalLoanAmount As Decimal,
        deductionPercentage As Decimal,
        basicMonthlySalary As Decimal,
        originalDeductionAmount As Decimal,
        Optional loanTransactions As List(Of LoanTransaction) = Nothing) As LoanSchedule

        Dim loan = WithLoanAmountAndDeductionAmount(
            totalLoanAmount:=totalLoanAmount,
            originalDeductionAmount:=originalDeductionAmount,
            loanTransactions:=loanTransactions)

        loan.DeductionPercentage = deductionPercentage
        loan.BasicMonthlySalary = basicMonthlySalary

        Return loan

    End Function

End Class
