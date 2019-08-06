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
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property RowID As Integer?

        Public Overridable Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property Created As DateTime

        Public Overridable Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Overridable Property LastUpd As DateTime?

        Public Overridable Property LastUpdBy As Integer?

        Public Overridable Property EmployeeID As Integer?

        Public Overridable Property LeaveLedgerID As Integer?

        Public Overridable Property PayPeriodID As Integer?

        Public Overridable Property ReferenceID As Integer?

        Public Overridable Property TransactionDate As Date

        Public Overridable Property Type As String

        Public Overridable Property Balance As Decimal

        Public Overridable Property Amount As Decimal

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

        <ForeignKey("LeaveLedgerID")>
        Public Overridable Property LeaveLedger As LeaveLedger

        <ForeignKey("PayPeriodID")>
        Public Overridable Property PayPeriod As PayPeriod

        Public ReadOnly Property IsCredit As Boolean
            Get
                Return Type.Trim.ToUpper = LeaveTransactionType.Credit.ToUpper
            End Get
        End Property

        Public ReadOnly Property IsDebit As Boolean
            Get
                Return Type.Trim.ToUpper = LeaveTransactionType.Debit.ToUpper
            End Get
        End Property

    End Class

End Namespace