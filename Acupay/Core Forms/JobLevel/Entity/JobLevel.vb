Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.JobLevel

    <Table("joblevel")>
    Public Class JobLevel

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property JobCategoryID As Integer?

        Public Property Name As String

        Public Property Points As Integer

        <ForeignKey("JobCategoryID")>
        Public Overridable Property JobCategory As JobCategory

    End Class

End Namespace
