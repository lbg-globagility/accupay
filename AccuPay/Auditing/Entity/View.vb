Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Auditing

    <Table("view")>
    Public Class View

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property ViewName As String

    End Class

End Namespace
