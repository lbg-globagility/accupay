Option Strict On

Imports AccuPay.Data.Interfaces.Excel
Imports AccuPay.Utilities.Attributes

Namespace Global.Globagility.AccuPay.ShiftSchedules

    Public Class ShiftScheduleRowRecord
        Implements IExcelRowRecord

        <ColumnName("Employee No")>
        Public Property EmployeeNo As String

        <ColumnName("Effective Date From")>
        Public Property StartDate As Date

        <ColumnName("Effective Date To (Optional)")>
        Public Property EndDate As Date?

        <ColumnName("Time From")>
        Public Property StartTime As TimeSpan?

        <ColumnName("Time To")>
        Public Property EndTime As TimeSpan?

        <ColumnName("Break Start Time (Optional)")>
        Public Property BreakStartTime As TimeSpan?

        <ColumnName("Break Length (Optional)")>
        Public Property BreakLength As Decimal

        <ColumnName("Offset (Optional) (true/false)")>
        Public Property IsRestDay As Boolean

        Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber
    End Class

End Namespace