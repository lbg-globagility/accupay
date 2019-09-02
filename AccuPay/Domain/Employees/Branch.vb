Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("branch")>
    Public Class Branch

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As DateTime?

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As DateTime?

        Public Property LastUpdBy As Integer?

        Public Property OrganizationID As Integer

        <Column("BranchCode")>
        Public Property Code As String

        <Column("BranchName")>
        Public Property Name As String

    End Class

End Namespace