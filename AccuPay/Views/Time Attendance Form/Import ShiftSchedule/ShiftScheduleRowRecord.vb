Option Strict On

Imports AccuPay.Attributes

Namespace Global.Globagility.AccuPay.ShiftSchedules

    Public Class ShiftScheduleRowRecord
        Implements IShiftScheduleRowRecord

        <ColumnName("Employee No")>
        Public Property EmployeeNo As String Implements IShiftScheduleRowRecord.EmployeeNo

        <ColumnName("Effective Date From")>
        Public Property StartDate As Date Implements IShiftScheduleRowRecord.StartDate

        <ColumnName("Effective Date To (Optional)")>
        Public Property EndDate As Date Implements IShiftScheduleRowRecord.EndDate

        <ColumnName("Time From")>
        Public Property StartTime As TimeSpan? Implements IShiftScheduleRowRecord.StartTime

        <ColumnName("Time To")>
        Public Property EndTime As TimeSpan? Implements IShiftScheduleRowRecord.EndTime

        <ColumnName("Break Start Time (Optional)")>
        Public Property BreakStartTime As TimeSpan? Implements IShiftScheduleRowRecord.BreakStartTime

        <ColumnName("Break Length (Optional)")>
        Public Property BreakLength As Decimal Implements IShiftScheduleRowRecord.BreakLength

        <ColumnName("Offset")>
        Public Property IsRestDay As String Implements IShiftScheduleRowRecord.IsRestDay

    End Class

    Public Interface IShiftScheduleRowRecord
        Property EmployeeNo As String
        Property StartDate As Date
        Property EndDate As Date
        Property StartTime As TimeSpan?
        Property EndTime As TimeSpan?
        Property BreakStartTime As TimeSpan?
        Property BreakLength As Decimal
        Property IsRestDay As String
    End Interface

End Namespace
