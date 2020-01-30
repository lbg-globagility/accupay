Option Strict On

Imports AccuPay.Attributes
Imports AccuPay.Utils

Public Class OvertimeRowRecord
    Implements IExcelRowRecord

    <Ignore>
    Public Property EmployeeFullName As String

    <ColumnName("Employee ID")>
    Public Property EmployeeID As String

    <ColumnName("Type")>
    Public Property Type As String

    <ColumnName("Effective start date")>
    Public Property EffectiveStartDate As Date?

    <ColumnName("Effective Start Time")>
    Public Property EffectiveStartTime As TimeSpan?

    <ColumnName("Effective end date")>
    Public Property EffectiveEndDate As Date?

    <ColumnName("Effective End Time")>
    Public Property EffectiveEndTime As TimeSpan?

    <Ignore>
    Public Property ErrorMessage As String

    <Ignore>
    Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

    Public Function ToOvertime() As Overtime

        If EffectiveStartDate Is Nothing OrElse EffectiveEndDate Is Nothing Then
            Return Nothing
        End If

        Return New Overtime With {
            .OTStartDate = EffectiveStartDate.Value,
            .OTEndDate = EffectiveEndDate.Value,
            .OTStartTime = EffectiveStartTime,
            .OTEndTime = EffectiveEndTime,
            .EmployeeID = ObjectUtils.ToNullableInteger(EmployeeID)
        }
    End Function

End Class