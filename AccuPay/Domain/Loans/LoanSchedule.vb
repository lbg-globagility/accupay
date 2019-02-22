Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay

Namespace Global.AccuPay.Loans

    <Table("employeeloanschedule")>
    Public Class LoanSchedule

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property EmployeeID As Integer?

        Public Property LoanTypeID As Integer?

        Public Property BonusID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property LoanNumber As String

        Public Property DedEffectiveDateFrom As Date

        Public Property DedEffectiveDateTo As Date?

        Public Property TotalLoanAmount As Decimal

        Public Property DeductionSchedule As String

        Public Property DeductionAmount As Decimal

        Public Property TotalBalanceLeft As Decimal

        Public Property Status As String

        Public Property DeductionPercentage As Decimal

        Public Property NoOfPayPeriod As Decimal

        Public Property LoanPayPeriodLeft As Integer

        Public Property Comments As String

        Public Property LoanName As String

        Public Overridable Property LoanTransactions As ICollection(Of LoanTransaction)

        Public Overridable Sub Credit(payPeriodID As Integer?)
            Dim currentDeductionAmount = If(DeductionAmount > TotalBalanceLeft, TotalBalanceLeft, DeductionAmount)
            Dim newBalance = If(LastEntry()?.TotalBalance, TotalBalanceLeft) - currentDeductionAmount

            Dim transaction = New LoanTransaction() With {
                .Created = Date.Now,
                .LastUpd = Date.Now,
                .OrganizationID = z_OrganizationID,
                .EmployeeID = EmployeeID,
                .PayPeriodID = payPeriodID,
                .LoanPayPeriodLeft = LoanPayPeriodLeft - 1,
                .TotalBalance = newBalance,
                .Amount = currentDeductionAmount
            }

            LoanTransactions.Add(transaction)
        End Sub

        Public Overridable Function LastEntry() As LoanTransaction
            Return LoanTransactions.LastOrDefault()
        End Function

        Public Overridable Sub Rollback()
            LoanTransactions.Remove(LastEntry())
        End Sub

    End Class

End Namespace
