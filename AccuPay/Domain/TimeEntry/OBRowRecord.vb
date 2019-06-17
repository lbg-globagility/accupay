Imports AccuPay.Attributes

Public Class OBRowRecord
    Implements IExcelRowRecord

    <Ignore>
    Public Property EmployeeFullName As String

    <ColumnName("Employee ID")>
    Public Property EmployeeID As String

    <ColumnName("Type (Official business)")>
    Public Property Type As String

    <ColumnName("Start Date")>
    Public Property StartDate As Date?

    <ColumnName("Start Time")>
    Public Property StartTime As TimeSpan?

    <ColumnName("End Date")>
    Public Property EndDate As Date?

    <ColumnName("End Time")>
    Public Property EndTime As TimeSpan?

    <Ignore>
    Public Property ErrorMessage As String

    <Ignore>
    Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

End Class