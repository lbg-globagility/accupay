Option Strict On

Public Class TimeEntryGenerator

    Private _timeEntry As TimeEntry

    Private _timeLog As Object

    Private _shift As Shift

    Public Sub ComputeRegularHours()
        Dim dutyStart = New Date()
        Dim dutyEnd = New Date()

        Dim regularHours As TimeSpan

        Dim breaktimeStart = New Date()
        Dim breaktimeEnd = New Date()

        If _shift.HasBreaktime Then
            Dim hoursBeforeBreak As TimeSpan
            Dim hoursAfterBreak As TimeSpan

            If dutyStart < breaktimeStart Then
                hoursBeforeBreak = breaktimeStart - dutyStart
            End If

            If dutyEnd > breaktimeEnd Then
                hoursAfterBreak = dutyEnd - breaktimeEnd
            End If

            regularHours = hoursBeforeBreak + hoursAfterBreak
        Else
            regularHours = dutyEnd - dutyStart
        End If

        _timeEntry.RegularHours = CDec(regularHours.TotalHours)
    End Sub

    Public Function GetFullTimeIn() As Date
        Return Date.Parse(_timeEntry.EntryDate & _timeEntry.TimeIn.ToString())
    End Function

    Public Function GetFullTimeOut() As Date
        If _timeEntry.TimeIn > _timeEntry.TimeOut Then
            Return Date.Parse(_timeEntry.EntryDate & _timeEntry.TimeOut.ToString())
        Else
            Return Date.Parse(_timeEntry.DateTomorrow() & _timeEntry.TimeOut.ToString())
        End If
    End Function

    Public Function GetFullBreaktimeStart() As Date
        Return Date.Parse(_timeEntry.EntryDate & _shift.BreaktimeFrom.ToString())
    End Function

    Public Function GetFullBreaktimeEnd() As Date
        Return Date.Parse(_timeEntry.EntryDate & _shift.BreaktimeTo.ToString())
    End Function

End Class
