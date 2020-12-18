Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Interfaces.Excel
Imports AccuPay.Utilities.Attributes

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

        Return Overtime.NewOvertime(
            organizationId:=z_OrganizationID,
            employeeId:=employeeId,
            startDate:=StartDate.Value,
            startTime:=StartTime,
            endTime:=EndTime)

    End Function

End Class
