Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("leavetransaction")>
    Public Class LeaveTransaction

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As DateTime

        Public Property CreatedBy As Integer?

        Public Property LastUpd As DateTime

        Public Property LastUpdBy As Integer?

        Public Property EmployeeID As Integer?

        Public Property LeaveLedgerID As Integer?

        Public Property PayPeriodID As Integer?

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