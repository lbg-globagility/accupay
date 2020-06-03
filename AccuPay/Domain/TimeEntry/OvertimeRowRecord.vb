Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Infrastructure.Services.Excel

Public Class OvertimeRowRecord
    Implements IExcelRowRecord

    <Ignore>
    Public Property EmployeeFullName As String

    <ColumnName("Employee ID")>
    Public Property EmployeeNumber As String

    <ColumnName("Effective start date")>
    Public Property StartDate As Date?

    <ColumnName("Effective Start Time")>
    Public Property StartTime As TimeSpan?

    <ColumnName("Effective End Time")>
    Public Property EndTime As TimeSpan?

    <Ignore>
    Public Property ErrorMessage As String

    <Ignore>
    Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

    Public Function ToOvertime(employeeId As Integer) As Overtime

        If StartDate Is Nothing Then Return Nothing

        Dim newOvertime = New Overtime With {
            .RowID = Nothing,
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .EmployeeID = employeeId,
            .OTStartDate = StartDate.Value,
            .OTStartTime = StartTime,
            .OTEndTime = EndTime,
            .Status = Overtime.StatusApproved
        }

        newOvertime.UpdateEndDate()

        Return newOvertime
    End Function

End Class