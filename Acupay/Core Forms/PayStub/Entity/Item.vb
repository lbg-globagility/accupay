Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.Item.Entity

    <Table("paystubitem")>
    Public Class Item

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date

        Public Property LastUpdBy As Integer

        Public Property PayStubID As Integer?

        Public Property ProductID As Integer?

        Public Property PayAmount As Decimal

        Public Property Undeclared As Char

    End Class

End Namespace