Option Strict On

Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class TimeEntryCalculator

    Public Function ComputeRegularHours(workStart As Date, workEnd As Date, currentShift As CurrentShift) As Decimal
        Dim workPeriod = New TimePeriod(workStart, workEnd)
        Dim shiftPeriod = New TimePeriod(currentShift.Start, currentShift.End)

        Dim coveredPeriod = workPeriod.Overlap(shiftPeriod)

        If currentShift.HasBreaktime() Then
            Dim breakPeriod = New TimePeriod(currentShift.BreaktimeStart.Value, currentShift.BreaktimeEnd.Value)
            Dim coveredPeriods = coveredPeriod.Difference(breakPeriod)

            Return coveredPeriods.Sum(Function(c) CDec(c.Length.TotalHours))
        End If

        Return CDec(coveredPeriod.Length.TotalHours)
    End Function

    Public Function ComputeLateHours(workStart As Date, workEnd As Date, currentShift As CurrentShift) As Decimal
        Dim workPeriod = New TimePeriod(workStart, workEnd)
        Dim shiftPeriod = New TimePeriod(currentShift.Start, currentShift.End)

        If workPeriod.Start <= shiftPeriod.Start Then
            Return 0
        End If

        Dim latePeriod = New TimePeriod(shiftPeriod.Start, workPeriod.Start)

        If currentShift.HasBreaktime Then
            Dim breakPeriod = New TimePeriod(currentShift.BreaktimeStart.Value, currentShift.BreaktimeEnd.Value)
            Dim latePeriods = latePeriod.Difference(breakPeriod)

            Return latePeriods.Sum(Function(l) CDec(l.Length.TotalHours))
        End If

        Return CDec(latePeriod.Length.TotalHours)
    End Function

    Public Function ComputeUndertimeHours(workBegin As Date, workEnd As Date, shift As CurrentShift) As Decimal
        Dim periodWorked = New TimePeriod(workBegin, workEnd)
        Dim shiftPeriod = New TimePeriod(shift.Start, shift.End)

        If periodWorked.End >= shiftPeriod.End Then
            Return 0
        End If

        Dim undertimePeriod = New TimePeriod(periodWorked.End, shiftPeriod.End)

        If shift.HasBreaktime Then
            Dim breakPeriod = New TimePeriod(shift.BreaktimeStart.Value, shift.BreaktimeEnd.Value)
            Dim undertimePeriods = undertimePeriod.Difference(breakPeriod)

            Return undertimePeriods.Sum(Function(u) CDec(u.Length.TotalHours))
        End If

        Return CDec(undertimePeriod.Length.TotalHours)
    End Function

    Public Function ComputeOvertimeHours(workBegin As Date, workEnd As Date, overtime As Overtime, shift As CurrentShift) As Decimal
        Dim otStartTime = If(overtime.OTStartTime, shift.End.TimeOfDay)
        Dim otEndTime = If(overtime.OTEndTime, shift.Start.TimeOfDay)

        Dim overtimeStart = overtime.OTStartDate.Add(otStartTime)
        Dim nextDay = overtime.OTStartDate.AddDays(1)

        Dim overtimeEnd = If(
            otEndTime > otStartTime,
            overtime.OTStartDate.Add(otEndTime),
            nextDay.Add(otEndTime))

        Dim overtimeRecognized = New TimePeriod(overtimeStart, overtimeEnd)
        Dim worked = New TimePeriod(workBegin, workEnd)

        Dim overtimeWorked = worked.Overlap(overtimeRecognized)

        ' If overtime worked is nothing, perhaps the overtime was meant to happen in the next day past midnight.
        If overtimeWorked Is Nothing Then
            overtimeStart = nextDay.Add(otStartTime)
            overtimeEnd = nextDay.Add(otEndTime)

            overtimeRecognized = New TimePeriod(overtimeStart, overtimeEnd)

            overtimeWorked = worked.Overlap(overtimeRecognized)
        End If

        Return CDec(overtimeWorked.Length.TotalHours)
    End Function

End Class
