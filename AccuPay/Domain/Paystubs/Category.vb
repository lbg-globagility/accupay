Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("category")>
    Public Class Category

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property RowID As Integer

        Public Overridable Property CategoryID As Integer?

        Public Overridable Property CategoryName As String

        Public Overridable Property OrganizationID As Integer

        Public Overridable Property Catalog As String

        Public Overridable Property CatalogID As Integer?

        'Public Overridable Property LastUpd As Date?
    End Class

End Namespace
