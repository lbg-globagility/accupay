Option Strict On

Public Class TimeEntryGenerator

    Private timeEntry As TimeEntry

    Private timeLog As TimeLog

    Private shift As Shift

    Private dutyStart As Date

    Private dutyEnd As Date

    Private shiftToday As ShiftToday

    Public Function GetFullTimeIn() As Date
        Return Date.Parse(timeEntry.EntryDate & timeEntry.TimeIn.ToString())
    End Function

    Public Function GetFullTimeOut() As Date
        If timeEntry.TimeIn > timeEntry.TimeOut Then
            Return Date.Parse(timeEntry.EntryDate & timeEntry.TimeOut.ToString())
        Else
            Return Date.Parse(timeEntry.DateTomorrow() & timeEntry.TimeOut.ToString())
        End If
    End Function

    Public Sub ComputeRegularHours()
        Dim regularHours As TimeSpan

        If shiftToday.HasBreaktime Then
            Dim hoursBeforeBreak As TimeSpan
            Dim hoursAfterBreak As TimeSpan

            If dutyStart < shiftToday.BreaktimeStart Then
                Dim lastWorkBeforeBreaktimeStarts = {dutyEnd, shiftToday.BreaktimeStart}.Min()

                hoursBeforeBreak = lastWorkBeforeBreaktimeStarts - dutyStart
            End If

            If dutyEnd > shiftToday.BreaktimeEnd Then
                Dim workStartAfterBreaktime = {dutyStart, shiftToday.BreaktimeEnd}.Max()

                hoursAfterBreak = dutyEnd - workStartAfterBreaktime
            End If

            regularHours = hoursBeforeBreak + hoursAfterBreak
        Else
            regularHours = dutyEnd - dutyStart
        End If

        timeEntry.RegularHours = CDec(regularHours.TotalHours)
    End Sub

End Class
