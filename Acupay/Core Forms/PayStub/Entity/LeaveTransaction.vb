Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    Public Class LeaveTransactionType

        Public Const Credit As String = "Credit"

        Public Const Debit As String = "Debit"

    End Class

    <Table("leavetransaction")>
    Public Class LeaveTransaction

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As DateTime

        Public Property CreatedBy As Integer?

        Public Property LastUpd As DateTime?

        Public Property LastUpdBy As Integer?

        Public Property EmployeeID As Integer?

        Public Property LeaveLedgerID As Integer?

        Public Property PayPeriodID As Integer?

        Public Property ReferenceID As Integer?

        Public Property TransactionDate As Date

        Public Property Type As String

        Public Property Balance As Decimal

        Public Property Amount As Decimal

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

        <ForeignKey("LeaveLedgerID")>
        Public Overridable Property LeaveLedger As LeaveLedger

        <ForeignKey("PayPeriodID")>
        Public Overridable Property PayPeriod As PayPeriod

    End Class

End Namespace