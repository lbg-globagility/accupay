Option Strict On

Imports AccuPay.Entity

Public Class TimeEntryCalculator

    Private _timeLog As TimeLog

    Private _dutyStart As Date

    Private _dutyEnd As Date

    Private _shift As Shift

    Public _timeEntry As TimeEntry

    Private _shiftToday As CurrentShift

    Public Sub ComputeAllHours(computeBreakTimeLate As Boolean)
        ComputeRegularHours(computeBreakTimeLate)
    End Sub

    Public Sub ComputeRegularHours(computeBreakTimeLate As Boolean)
        Dim regularHours As TimeSpan

        If _shiftToday.HasBreaktime AndAlso computeBreakTimeLate = False Then
            Dim hoursBeforeBreak As TimeSpan
            Dim hoursAfterBreak As TimeSpan

            If _timeEntry.DutyStart < _shiftToday.BreaktimeStart Then
                Dim lastWorkBeforeBreaktimeStarts = {_timeEntry.DutyEnd, _shiftToday.BreaktimeStart.Value}.Min()

                hoursBeforeBreak = lastWorkBeforeBreaktimeStarts - _timeEntry.DutyStart
            End If

            If _timeEntry.DutyEnd > _shiftToday.BreaktimeEnd Then
                Dim workStartAfterBreaktime = {_timeEntry.DutyStart, _shiftToday.BreaktimeEnd}.Max()

                hoursAfterBreak = _timeEntry.DutyEnd - workStartAfterBreaktime.Value
            End If

            regularHours = hoursBeforeBreak + hoursAfterBreak
        Else
            regularHours = _timeEntry.DutyEnd - _timeEntry.DutyStart
        End If

        _timeEntry.RegularHours = CDec(regularHours.TotalHours)
    End Sub

    Public Function ComputeLateHours(workBegin As Date, workEnd As Date, shiftToday As CurrentShift, computeBreakTimeLate As Boolean) As Decimal
        If workBegin < shiftToday.RangeStart Then
            Return 0D
        End If

        Dim lateHours As TimeSpan

        If shiftToday.HasBreaktime AndAlso computeBreakTimeLate = False Then
            Dim hoursBeforeBreak As TimeSpan
            If shiftToday.RangeStart < shiftToday.BreaktimeStart Then
                Dim latePeriodEndBeforeBreaktime = {workBegin, shiftToday.BreaktimeStart.Value}.Min
                hoursBeforeBreak = latePeriodEndBeforeBreaktime - shiftToday.RangeStart
            End If

            Dim hoursAfterBreak As TimeSpan
            If workBegin > shiftToday.BreaktimeEnd Then
                hoursAfterBreak = workBegin - shiftToday.BreaktimeEnd.Value
            End If

            lateHours = hoursBeforeBreak + hoursAfterBreak
        Else
            lateHours = workBegin - shiftToday.RangeStart
        End If

        Return CDec(lateHours.TotalHours)
    End Function

    Public Sub ComputeUndertimeHours(computeBreakTimeLate As Boolean)
        If _timeEntry.DutyEnd > _shiftToday.RangeEnd Then
            Return
        End If

        Dim undertimeHours As TimeSpan

        If _shiftToday.HasBreaktime AndAlso computeBreakTimeLate = False Then
            Dim hoursBeforeBreak As TimeSpan
            Dim hoursAfterBreak As TimeSpan

            If _timeEntry.DutyEnd < _shiftToday.BreaktimeStart Then
                hoursBeforeBreak = _shiftToday.BreaktimeStart.Value - _timeEntry.DutyEnd
            End If

            Dim undertimePeriodStartAfterBreaktime = {_timeEntry.DutyEnd, _shiftToday.BreaktimeEnd}.Max
            hoursAfterBreak = _shiftToday.RangeEnd - undertimePeriodStartAfterBreaktime.Value

            undertimeHours = hoursBeforeBreak + hoursAfterBreak
        Else
            undertimeHours = _shiftToday.RangeEnd - _timeEntry.DutyEnd
        End If

        _timeEntry.UndertimeHours = CDec(undertimeHours.TotalHours)
    End Sub

End Class
