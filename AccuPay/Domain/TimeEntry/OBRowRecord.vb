Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Infrastructure.Services.Excel
Imports AccuPay.Utilities

Public Class OBRowRecord
    Implements IExcelRowRecord

    <Ignore>
    Public Property EmployeeFullName As String

    <ColumnName("Employee ID")>
    Public Property EmployeeID As String

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

    <Ignore>
    Public Property Status As String

    Public ReadOnly Property EndDate As Date?
        Get
            If StartDate.HasValue = False Then
                Return Nothing
            ElseIf StartTime.HasValue = False OrElse EndTime.HasValue = False Then
                Return StartDate
            ElseIf StartTime.HasValue = False AndAlso EndTime.HasValue = False Then
                Return StartDate
            Else
                Return If(EndTime < StartTime, StartDate.Value.AddDays(1), StartDate)
            End If
        End Get
    End Property

    Friend Function ToOfficialBusiness() As OfficialBusiness

        If StartDate Is Nothing OrElse StartDate Is Nothing Then
            Return Nothing
        End If

        Return New OfficialBusiness With {
            .StartDate = StartDate.Value,
            .EndDate = EndDate.Value,
            .StartTime = StartTime,
            .EndTime = EndTime,
            .EmployeeID = ObjectUtils.ToNullableInteger(EmployeeID),
            .Status = Status
        }
    End Function

End Class