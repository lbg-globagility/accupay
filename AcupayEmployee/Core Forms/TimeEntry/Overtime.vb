Option Strict On

Public Class Overtime

    Public Property OvertimeDate As Date

    Public Property StartTime As TimeSpan

    Public Property EndTime As TimeSpan

    Public Property RangeStart As Date

    Public Property RangeEnd As Date

    Public Sub New(overtimeDate As Date, startTime As TimeSpan, endTime As TimeSpan)
        Me.OvertimeDate = overtimeDate
        Me.StartTime = startTime
        Me.EndTime = endTime

        RangeStart = TimeUtility.RangeStart(overtimeDate, startTime)
        RangeEnd = TimeUtility.RangeEnd(overtimeDate, startTime, endTime)
    End Sub

End Class