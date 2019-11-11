Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeeleave")>
    Public Class Leave

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property RowID As Integer?

        Public Overridable Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Overridable Property Created As DateTime

        Public Overridable Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Overridable Property LastUpd As DateTime?

        Public Overridable Property LastUpdBy As Integer?

        Public Overridable Property EmployeeID As Integer?

        Public Overridable Property LeaveType As String

        Public Overridable Property LeaveHours As Decimal

        <Column("LeaveStartTime")>
        Public Overridable Property StartTime As TimeSpan?

        <Column("LeaveEndTime")>
        Public Overridable Property EndTime As TimeSpan?

        <Column("LeaveStartDate")>
        Public Overridable Property StartDate As Date

        <Column("LeaveEndDate")>
        Public Overridable Property EndDate As Date?

        Public Overridable Property Reason As String

        Public Overridable Property Comments As String

        Public Overridable Property Image As Byte()

        Public Overridable Property Status As String

    End Class

End Namespace