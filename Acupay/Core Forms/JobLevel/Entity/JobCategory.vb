Option Strict On

Imports System.Collections.ObjectModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.JobLevels

    <Table("jobcategory")>
    Public Class JobCategory

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property Name As String

        Public Overridable Property JobLevels As ICollection(Of JobLevel)

        Public Sub New()
            JobLevels = New Collection(Of JobLevel)
            Created = Date.Now
        End Sub

    End Class

End Namespace
