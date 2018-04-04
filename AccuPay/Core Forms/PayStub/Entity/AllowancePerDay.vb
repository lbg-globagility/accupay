Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("allowanceperday")>
    Public Class AllowancePerDay

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property RowID As Integer?

        Public Overridable Property [Date] As Date

        Public Overridable Property Amount As Decimal

        <ForeignKey("AllowanceItemID")>
        Public Overridable Property AllowanceItem As AllowanceItem

        Protected Sub New()
        End Sub

        Public Sub New([date] As Date, amount As Decimal)
            Me.Date = [date]
            Me.Amount = amount
        End Sub

    End Class

End Namespace