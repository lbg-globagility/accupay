Option Strict On

Imports AccuPay.Core.Entities

Public Class LoanTransactionMother

    Public Shared Function Simple(
        Optional loanScheduleId As Integer = Nothing,
        Optional employeeId As Integer = Nothing,
        Optional payPeriod As PayPeriod = Nothing) As LoanTransaction

        Dim transaction As New LoanTransaction() With
        {
            .LoanScheduleID = loanScheduleId,
            .EmployeeID = employeeId
        }

        If payPeriod IsNot Nothing Then

            transaction.PayPeriod = payPeriod
            transaction.PayPeriodID = payPeriod.RowID

        End If

        Return transaction

    End Function

    Public Shared Function WithInterest(
        interestAmount As Decimal,
        deductionAmount As Decimal,
        payPeriod As PayPeriod,
        Optional loanScheduleId As Integer = Nothing,
        Optional employeeId As Integer = Nothing) As LoanTransaction

        Dim loanTransaction = Simple(loanScheduleId, employeeId, payPeriod)

        loanTransaction.InterestAmount = interestAmount
        loanTransaction.DeductionAmount = deductionAmount

        Return loanTransaction

    End Function

    Public Shared Function ListWithInterest(
        listCount As Integer,
        interestAmount As Decimal,
        deductionAmount As Decimal,
        payPeriod As PayPeriod,
        Optional loanScheduleId As Integer = Nothing,
        Optional employeeId As Integer = Nothing) As List(Of LoanTransaction)

        Dim list As New List(Of LoanTransaction)

        For index = 1 To listCount

            Dim loanTransaction = Simple(loanScheduleId, employeeId, payPeriod)

            loanTransaction.InterestAmount = interestAmount
            loanTransaction.DeductionAmount = deductionAmount

            list.Add(loanTransaction)
        Next

        Return list

    End Function

End Class
