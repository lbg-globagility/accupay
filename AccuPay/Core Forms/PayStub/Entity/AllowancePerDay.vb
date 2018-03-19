Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("allowanceperday")>
    Public Class AllowancePerDay

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property AllowanceItemID As Integer?

        Public Property [Date] As Date

        Public Property Amount As Decimal

        <ForeignKey("AllowanceItemID")>
        Public Overridable Property AllowanceItem As AllowanceItem

    End Class

End Namespace