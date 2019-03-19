Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("shiftschedules")>
    Public Class EmployeeDutySchedule

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer

        Public Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As DateTime

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property LastUpd As DateTime

        Public Property LastUpdBy As Integer?

        <ForeignKey("Employee")>
        Public Property EmployeeID As Integer?

        <Column("Date")>
        Public Property DateSched As Date

        Public Property StartTime As TimeSpan?
        Public Property EndTime As TimeSpan?
        Public Property BreakStartTime As TimeSpan?
        Public Property BreakLength As Decimal
        Public Property IsRestDay As Boolean
        Public Property ShiftHours As Decimal
        Public Property WorkHours As Decimal

        Public Overridable Property Employee As Employee

    End Class

End Namespace
