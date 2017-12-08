Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.JobLevels

    <Table("joblevel")>
    Public Class JobLevel

        <Key, Column(Order:=1)>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property JobCategoryID As Integer?

        Public Property Name As String

        Public Property Points As Integer

        Public Property SalaryRangeFrom As Decimal

        Public Property SalaryRangeTo As Decimal

        <ForeignKey("JobCategoryID")>
        Public Overridable Property JobCategory As JobCategory

    End Class

End Namespace
