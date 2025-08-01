Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces.Excel
Imports AccuPay.Utilities.Attributes

Public Class OBRowRecord
    Implements IExcelRowRecord

    <Ignore>
    Public Property EmployeeFullName As String

    <ColumnName("Employee ID")>
    Public Property EmployeeNumber As String

    <ColumnName("Start Date")>
    Public Property StartDate As Date?

    <ColumnName("Start Time")>
    Public Property StartTime As TimeSpan?

    <ColumnName("End Time")>
    Public Property EndTime As TimeSpan?

    <Ignore>
    Public Property ErrorMessage As String

    <Ignore>
    Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

    Friend Function ToOfficialBusiness(employeeId As Integer) As OfficialBusiness

        If StartDate Is Nothing Then Return Nothing

        Dim newOfficialBusiness = New OfficialBusiness With {
            .RowID = Nothing,
            .OrganizationID = z_OrganizationID,
            .EmployeeID = employeeId,
            .StartDate = StartDate.Value,
            .StartTime = StartTime,
            .EndTime = EndTime,
            .Status = OfficialBusiness.StatusApproved
        }

        newOfficialBusiness.UpdateEndDate()

        Return newOfficialBusiness
    End Function

End Class
