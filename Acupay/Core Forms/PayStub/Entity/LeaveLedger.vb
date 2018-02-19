Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("leaveledger")>
    Public Class LeaveLedger

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As DateTime

        Public Property CreatedBy As Integer?

        Public Property LastUpd As DateTime

        Public Property LastUpdBy As Integer?

        Public Property EmployeeID As Integer?

        Public Property ProductID As Integer?

        Public Property LastTransactionID As Integer?

        <ForeignKey("ProductID")>
        Public Overridable Property Product As Product

        <ForeignKey("LastTransactionID")>
        Public Overridable Property LastTransaction As LeaveTransaction

        <InverseProperty("LeaveLedger")>
        Public Overridable Property LeaveTransactions As IList(Of LeaveTransaction)

    End Class

End Namespace