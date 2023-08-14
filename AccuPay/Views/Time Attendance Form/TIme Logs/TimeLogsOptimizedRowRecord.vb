Imports AccuPay.Core.Interfaces.Excel
Imports AccuPay.Utilities.Attributes

Public Class TimeLogsOptimizedRowRecord
    Inherits ExcelEmployeeRowRecord
    Implements IExcelRowRecord

    <ColumnName("Employee ID")>
    Public Property EmployeeId As String

    <ColumnName("Date")>
    Public Property [Date] As Date?

    <ColumnName("Time IN")>
    Public Property TimeIn As TimeSpan?

    <ColumnName("Time OUT")>
    Public Property TimeOut As TimeSpan?

    <Ignore>
    Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

    <Ignore>
    Public Property EmployeeNumber As String

    <Ignore>
    Public Property EmployeeFullName As String

    <Ignore>
    Public Property OrganizationName As String

    <Ignore>
    Public Property OrganizationId As Integer

    <Ignore>
    Public ReadOnly Property HasNoDate As Boolean
        Get
            Return Not [Date].HasValue
        End Get
    End Property

    <Ignore>
    Public ReadOnly Property HasNoDateErrorText As String
        Get
            If HasNoDate Then Return "Invalid Date inputted"
            Return String.Empty
        End Get
    End Property

    <Ignore>
    Public ReadOnly Property HasNoTimeIn As Boolean
        Get
            Return Not TimeIn.HasValue
        End Get
    End Property

    <Ignore>
    Public ReadOnly Property HasNoTimeOut As Boolean
        Get
            Return Not TimeOut.HasValue
        End Get
    End Property

    <Ignore>
    Public ReadOnly Property HasNoTimeInAndOutErrorText As String
        Get
            If HasNoDate Then Return "Invalid Time IN and OUT"
            Return String.Empty
        End Get
    End Property

    <Ignore>
    Public ReadOnly Property ErrorMessage As String
        Get
            Dim errorTexts = {HasNoDateErrorText, HasNoTimeInAndOutErrorText}
            If errorTexts.Where(Function(t) Not String.IsNullOrEmpty(t)).Any() Then Return String.Join("; ", errorTexts)

            Return String.Empty
        End Get
    End Property

End Class
