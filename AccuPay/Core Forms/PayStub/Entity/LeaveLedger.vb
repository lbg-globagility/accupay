Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("leaveledger")>
    Public Class LeaveLedger

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

        Public Overridable Property ProductID As Integer?

        Public Overridable Property LastTransactionID As Integer?

        <ForeignKey("ProductID")>
        Public Overridable Property Product As Product

        <ForeignKey("LastTransactionID")>
        Public Overridable Property LastTransaction As LeaveTransaction

        <InverseProperty("LeaveLedger")>
        Public Overridable Property LeaveTransactions As IList(Of LeaveTransaction)

    End Class

End Namespace