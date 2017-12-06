Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("listofval")>
    Public Class ListOfValue

        <Key>
        Public Property RowID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property LIC As String

        Public Property DisplayValue As String

        Public Property Type As String

        Public Property Active As String

        Public Property Description As String

    End Class

End Namespace
