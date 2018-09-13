Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay

Namespace Global.AccuPay.Loans

    <Table("employeeloanschedule")>
    Public Class LoanSchedule

        <Key>
        Public Overridable Property RowID As Integer?

        Public Overridable Property OrganizationID As Integer?

        Public Overridable Property EmployeeID As Integer?

        Public Overridable Property LoanTypeID As Integer?

        Public Overridable Property BonusID As Integer?

        Public Overridable Property LoanNumber As String

        Public Overridable Property DedEffectiveDateFrom As Date

        Public Overridable Property DedEffectiveDateTo As Date?

        Public Overridable Property TotalLoanAmount As Decimal

        Public Overridable Property DeductionSchedule As String

        Public Overridable Property DeductionAmount As Decimal

        Public Overridable Property TotalBalanceLeft As Decimal

        Public Overridable Property Status As String

        Public Overridable Property DeductionPercentage As Decimal

        Public Overridable Property NoOfPayPeriod As Decimal

        Public Overridable Property LoanPayPeriodLeft As Integer

        Public Overridable Property Comments As String

        Public Overridable Property LoanName As String

        <NotMapped>
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
