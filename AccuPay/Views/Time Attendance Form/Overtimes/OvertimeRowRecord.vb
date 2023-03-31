Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces.Excel
Imports AccuPay.Utilities.Attributes

Public Class OvertimeRowRecord
    Inherits ExcelEmployeeRowRecord
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

    Public Function ToOvertime(employee As Employee) As Overtime
        Dim employeeId = employee.RowID.Value
        Dim organizationId = employee.OrganizationID.Value

        If StartDate Is Nothing Then Return Nothing

        Return Overtime.NewOvertime(
            organizationId:=organizationId,
            employeeId:=employeeId,
            startDate:=StartDate.Value,
            startTime:=StartTime,
            endTime:=EndTime)

    End Function

End Class
