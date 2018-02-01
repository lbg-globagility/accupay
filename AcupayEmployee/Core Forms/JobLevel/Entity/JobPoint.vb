Option Strict On

Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations
Imports Acupay
Imports AccuPay.Entity

Namespace Global.AccuPay.JobLevels

    <Table("jobpoint")>
    Public Class JobPoint

        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property EmployeeID As Integer?

        Public Property OccurredOn As Date

        Public Property Points As Integer

        Public Property Comments As String

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

    End Class

End Namespace
