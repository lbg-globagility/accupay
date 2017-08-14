Option Strict On

Public Class TimeEntryGenerator

    Private timeLog As TimeLog

    Private shift As Shift

    Public timeEntry As TimeEntry

    Private shiftToday As ShiftToday

    Public Sub New(shift As Shift, timeLog As TimeLog)
        Me.shift = shift
        Me.timeLog = timeLog

        shiftToday = New ShiftToday(Me.shift, Me.timeLog.LogDate)
        timeEntry = New TimeEntry(Me.timeLog, shiftToday)
    End Sub

    Public Sub ComputeAllHours()
        ComputeRegularHours()
    End Sub

    Public Sub ComputeRegularHours()
        Dim regularHours As TimeSpan

        If shiftToday.HasBreaktime Then
            Dim hoursBeforeBreak As TimeSpan
            Dim hoursAfterBreak As TimeSpan

            If timeEntry.DutyStart < shiftToday.BreaktimeStart Then
                Dim lastWorkBeforeBreaktimeStarts = {timeEntry.DutyEnd, shiftToday.BreaktimeStart}.Min()

                hoursBeforeBreak = lastWorkBeforeBreaktimeStarts - timeEntry.DutyStart
            End If

            If timeEntry.DutyEnd > shiftToday.BreaktimeEnd Then
                Dim workStartAfterBreaktime = {timeEntry.DutyStart, shiftToday.BreaktimeEnd}.Max()

                hoursAfterBreak = timeEntry.DutyEnd - workStartAfterBreaktime
            End If

            regularHours = hoursBeforeBreak + hoursAfterBreak
        Else
            regularHours = timeEntry.DutyEnd - timeEntry.DutyStart
        End If

        timeEntry.RegularHours = CDec(regularHours.TotalHours)
    End Sub

End Class
