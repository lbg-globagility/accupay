Option Strict On

Imports AccuPay.Entity

Public Class TimeEntryCalculator

    Private _timeLog As TimeLog

    Private _dutyStart As Date

    Private _dutyEnd As Date

    Private _shift As Shift

    Public _timeEntry As TimeEntry

    Private _shiftToday As CurrentShift

    Public Sub ComputeAllHours()
        ComputeRegularHours()
    End Sub

    Public Sub ComputeRegularHours()
        Dim regularHours As TimeSpan

        If _shiftToday.HasBreaktime Then
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

    Public Function ComputeLateHours(workBegin As Date, workEnd As Date, shiftToday As CurrentShift) As Decimal
        If workBegin < shiftToday.Start Then
            Return 0D
        End If

        Dim lateHours As TimeSpan

        If shiftToday.HasBreaktime Then
            Dim hoursBeforeBreak As TimeSpan
            If shiftToday.Start < shiftToday.BreaktimeStart Then
                Dim latePeriodEndBeforeBreaktime = {workBegin, shiftToday.BreaktimeStart.Value}.Min
                hoursBeforeBreak = latePeriodEndBeforeBreaktime - shiftToday.Start
            End If

            Dim hoursAfterBreak As TimeSpan
            If workBegin > shiftToday.BreaktimeEnd Then
                hoursAfterBreak = workBegin - shiftToday.BreaktimeEnd.Value
            End If

            lateHours = hoursBeforeBreak + hoursAfterBreak
        Else
            lateHours = workBegin - shiftToday.Start
        End If

        Return CDec(lateHours.TotalHours)
    End Function

    Public Sub ComputeUndertimeHours()
        If _timeEntry.DutyEnd > _shiftToday.End Then
            Return
        End If

        Dim undertimeHours As TimeSpan

        If _shiftToday.HasBreaktime Then
            Dim hoursBeforeBreak As TimeSpan
            Dim hoursAfterBreak As TimeSpan

            If _timeEntry.DutyEnd < _shiftToday.BreaktimeStart Then
                hoursBeforeBreak = _shiftToday.BreaktimeStart.Value - _timeEntry.DutyEnd
            End If

            Dim undertimePeriodStartAfterBreaktime = {_timeEntry.DutyEnd, _shiftToday.BreaktimeEnd}.Max
            hoursAfterBreak = _shiftToday.End - undertimePeriodStartAfterBreaktime.Value

            undertimeHours = hoursBeforeBreak + hoursAfterBreak
        Else
            undertimeHours = _shiftToday.End - _timeEntry.DutyEnd
        End If

        _timeEntry.UndertimeHours = CDec(undertimeHours.TotalHours)
    End Sub

    Public Function ComputeUndertimeHours2(workBegin As Date, workEnd As Date, shift As CurrentShift) As Decimal
        Dim periodWorked = New TimeInterval(workBegin, workEnd)
        Dim shiftPeriod = New TimeInterval(shift.Start, shift.End)



        Return 0
    End Function

    Public Function ComputeOvertimeHours(workBegin As Date, workEnd As Date, overtime As Overtime, shift As CurrentShift) As Decimal
        Dim overtimeStart = If(overtime.Start, shift.End)
        Dim overtimeEnd = If(overtime.End, shift.Start)

        Dim overtimeRecognized = New TimeInterval(overtimeStart, overtimeEnd)
        Dim worked = New TimeInterval(workBegin, workEnd)

        Dim overtimeWorked = worked.Intersect(overtimeRecognized)

        Return CDec(overtimeWorked.Length.TotalHours)
    End Function

End Class
