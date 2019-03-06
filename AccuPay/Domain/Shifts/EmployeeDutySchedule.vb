Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("shiftschedules")>
    Public Class EmployeeDutySchedule
        Implements IEmployeeDutySchedule

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer

        Public Property OrganizationID As Integer? Implements IEmployeeDutySchedule.OrganizationID

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As DateTime

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property LastUpd As DateTime

        Public Property LastUpdBy As Integer?

        <ForeignKey("Employee")>
        Public Property EmployeeID As Integer? Implements IEmployeeDutySchedule.EmployeeID

        <Column("Date")>
        Public Property DateSched As Date? Implements IEmployeeDutySchedule.DateSched

        Public Property StartTime As TimeSpan? Implements IEmployeeDutySchedule.StartTime
        Public Property EndTime As TimeSpan? Implements IEmployeeDutySchedule.EndTime
        Public Property BreakStartTime As TimeSpan? Implements IEmployeeDutySchedule.BreakStartTime
        Public Property BreakEndTime As TimeSpan? Implements IEmployeeDutySchedule.BreakEndTime
        Public Property IsRestDay As Boolean Implements IEmployeeDutySchedule.IsRestDay

        Public Overridable Property Employee As Employee

    End Class

End Namespace
