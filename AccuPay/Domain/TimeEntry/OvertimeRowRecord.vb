Option Strict On

Imports AccuPay.Attributes
Imports AccuPay.Data.Entities
Imports AccuPay.Utilities

Public Class OvertimeRowRecord
    Implements IExcelRowRecord

    <Ignore>
    Public Property EmployeeFullName As String

    <ColumnName("Employee ID")>
    Public Property EmployeeID As String

    <ColumnName("Type")>
    Public Property Type As String

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

    Public Function ToOvertime() As Overtime

        If StartDate Is Nothing OrElse EndDate Is Nothing Then
            Return Nothing
        End If

        Return New Overtime With {
            .OTStartDate = StartDate.Value,
            .OTEndDate = EndDate.Value,
            .OTStartTime = StartTime,
            .OTEndTime = EndTime,
            .EmployeeID = ObjectUtils.ToNullableInteger(EmployeeID)
        }
    End Function

End Class