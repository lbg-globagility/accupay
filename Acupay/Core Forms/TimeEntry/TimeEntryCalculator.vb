Option Strict On

Imports AccuPay.Entity

Public Class TimeEntryCalculator

    Private _timeLog As TimeLog

    Private _dutyStart As Date

    Private _dutyEnd As Date

    Private _shift As Shift

    Public _timeEntry As TimeEntry

    Private _shiftToday As ShiftToday

    Public Sub New(shift As Shift, timeLog As TimeLog)
        _shift = shift
        _timeLog = timeLog

        _shiftToday = New ShiftToday(_shift, _timeLog.LogDate)
        _timeEntry = New TimeEntry(_timeLog, _shiftToday)
    End Sub

    Public Sub ComputeAllHours()
        ComputeRegularHours()
    End Sub

    Public Sub ComputeRegularHours()
        Dim regularHours As TimeSpan

        If _shiftToday.HasBreaktime Then
            Dim hoursBeforeBreak As TimeSpan
            Dim hoursAfterBreak As TimeSpan

            If _timeEntry.DutyStart < _shiftToday.BreaktimeStart Then
                Dim lastWorkBeforeBreaktimeStarts = {_timeEntry.DutyEnd, _shiftToday.BreaktimeStart}.Min()

                hoursBeforeBreak = lastWorkBeforeBreaktimeStarts - _timeEntry.DutyStart
            End If

            If _timeEntry.DutyEnd > _shiftToday.BreaktimeEnd Then
                Dim workStartAfterBreaktime = {_timeEntry.DutyStart, _shiftToday.BreaktimeEnd}.Max()

                hoursAfterBreak = _timeEntry.DutyEnd - workStartAfterBreaktime
            End If

            regularHours = hoursBeforeBreak + hoursAfterBreak
        Else
            regularHours = _timeEntry.DutyEnd - _timeEntry.DutyStart
        End If

        _timeEntry.RegularHours = CDec(regularHours.TotalHours)
    End Sub

    Public Sub ComputerLateHours()
        If _timeEntry.DutyStart < _shiftToday.RangeStart Then
            Return
        End If

        Dim lateHours As TimeSpan

        If _shiftToday.HasBreaktime Then
            Dim hoursBeforeBreak As TimeSpan
            Dim hoursAfterBreak As TimeSpan

            If _shiftToday.RangeStart < _shiftToday.BreaktimeStart Then
                Dim latePeriodEndBeforeBreaktime = {_timeEntry.DutyStart, _shiftToday.BreaktimeStart}.Min
                hoursBeforeBreak = latePeriodEndBeforeBreaktime - _shiftToday.RangeStart
            End If

            If _timeEntry.DutyStart > _shiftToday.BreaktimeEnd Then
                hoursAfterBreak = _timeEntry.DutyStart - _shiftToday.BreaktimeEnd
            End If

            lateHours = hoursBeforeBreak + hoursAfterBreak
        Else
            lateHours = _timeEntry.DutyStart - _shiftToday.RangeStart
        End If

        _timeEntry.LateHours = CDec(lateHours.TotalHours)
    End Sub

    Public Sub ComputeUndertimeHours()
        If _timeEntry.DutyEnd > _shiftToday.RangeEnd Then
            Return
        End If

        Dim undertimeHours As TimeSpan

        If _shiftToday.HasBreaktime Then
            Dim hoursBeforeBreak As TimeSpan
            Dim hoursAfterBreak As TimeSpan

            If _timeEntry.DutyEnd < _shiftToday.BreaktimeStart Then
                hoursBeforeBreak = _shiftToday.BreaktimeStart - _timeEntry.DutyEnd
            End If

            Dim undertimePeriodStartAfterBreaktime = {_timeEntry.DutyEnd, _shiftToday.BreaktimeEnd}.Max
            hoursAfterBreak = _shiftToday.RangeEnd - undertimePeriodStartAfterBreaktime

            undertimeHours = hoursBeforeBreak + hoursAfterBreak
        Else
            undertimeHours = _shiftToday.RangeEnd - _timeEntry.DutyEnd
        End If

        _timeEntry.UndertimeHours = CDec(undertimeHours.TotalHours)
    End Sub

End Class
