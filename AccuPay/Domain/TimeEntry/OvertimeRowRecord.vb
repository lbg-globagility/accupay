Imports AccuPay.Attributes

Public Class OvertimeRowRecord
    <Ignore>
    Public Property EmployeeFullName As String

    <ColumnName("Employee ID")>
    Public Property EmployeeID As String

    <ColumnName("Type")>
    Public Property Type As String

    <ColumnName("Effective start date")>
    Public Property EffectiveStartDate As Date

    <ColumnName("Effective Start Time")>
    Public Property EffectiveStartTime As TimeSpan?

    <ColumnName("Effective end date")>
    Public Property EffectiveEndDate As Date

    <ColumnName("Effective End Time")>
    Public Property EffectiveEndTime As TimeSpan?

    <Ignore>
    Public Property ErrorMessage As String

    <Ignore>
    Public Property LineNumber As Integer
End Class
