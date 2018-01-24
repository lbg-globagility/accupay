Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("allowanceperday")>
    Public Class AllowancePerDay

        <Key>
        Public Property RowID As Integer?

        Public Property AllowanceID As Integer?

        Public Property EntryDate As Date

        Public Property Amount As Decimal

    End Class

End Namespace