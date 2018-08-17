Option Strict On

Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class TimeEntryCalculator

    Public Function ComputeRegularHours(workPeriod As TimePeriod, currentShift As CurrentShift) As Decimal
        Dim shiftPeriod = currentShift.ShiftPeriod

        Dim coveredPeriod = workPeriod.Overlap(shiftPeriod)

        If currentShift.HasBreaktime() Then
            Dim breakPeriod = currentShift.BreakPeriod
            Dim coveredPeriods = coveredPeriod.Difference(breakPeriod)

            Return coveredPeriods.Sum(Function(c) c.TotalHours)
        End If

        Return coveredPeriod.TotalHours
    End Function

    Public Function ComputeLateHours(workPeriod As TimePeriod, currentShift As CurrentShift) As Decimal
        Dim shiftPeriod = currentShift.ShiftPeriod

        If workPeriod.EarlierThan(shiftPeriod) Then
            Return 0
        End If

        Dim latePeriod = New TimePeriod(shiftPeriod.Start, workPeriod.Start)

        If currentShift.HasBreaktime Then
            Dim breakPeriod = currentShift.BreakPeriod
            Dim latePeriods = latePeriod.Difference(breakPeriod)

            Return latePeriods.Sum(Function(l) l.TotalHours)
        End If

        Return latePeriod.TotalHours
    End Function

    Public Function ComputeUndertimeHours(workPeriod As TimePeriod, currentShift As CurrentShift) As Decimal
        Dim shiftPeriod = currentShift.ShiftPeriod

        If workPeriod.LaterThan(shiftPeriod) Then
            Return 0
        End If

        Dim undertimePeriod = New TimePeriod(workPeriod.End, shiftPeriod.End)

        If currentShift.HasBreaktime Then
            Dim breakPeriod = currentShift.BreakPeriod
            Dim undertimePeriods = undertimePeriod.Difference(breakPeriod)

            Return undertimePeriods.Sum(Function(u) u.TotalHours)
        End If

        Return undertimePeriod.TotalHours
    End Function

    Public Function ComputeNightDiffHours(workPeriod As TimePeriod, currentShift As CurrentShift, nightDiffPeriod As TimePeriod) As Decimal
        If Not workPeriod.Intersects(nightDiffPeriod) Then
            Return 0D
        End If

        Dim nightWorked = workPeriod.Overlap(nightDiffPeriod)

        Dim nightDiffHours = 0D
        If currentShift.HasBreaktime Then
            Dim breakPeriod = currentShift.BreakPeriod
            Dim nightWorkedPeriods = nightWorked.Difference(breakPeriod)

            nightDiffHours = nightWorkedPeriods.Sum(Function(n) n.TotalHours)
        Else
            nightDiffHours = nightWorked.TotalHours
        End If

        Return nightDiffHours
    End Function

    Public Function ComputeOvertimeHours(workPeriod As TimePeriod, overtime As Overtime, shift As CurrentShift) As Decimal
        Dim otStartTime = If(overtime.OTStartTime, shift.End.TimeOfDay)
        Dim otEndTime = If(overtime.OTEndTime, shift.Start.TimeOfDay)

        Dim overtimeStart = overtime.OTStartDate.Add(otStartTime)
        Dim nextDay = overtime.OTStartDate.AddDays(1)

        Dim overtimeEnd = If(
            otEndTime > otStartTime,
            overtime.OTStartDate.Add(otEndTime),
            nextDay.Add(otEndTime))

        Dim overtimeRecognized = New TimePeriod(overtimeStart, overtimeEnd)

        Dim overtimeWorked = workPeriod.Overlap(overtimeRecognized)

        ' If overtime worked is nothing, perhaps the overtime was meant to happen in the next day past midnight.
        If overtimeWorked Is Nothing Then
            overtimeStart = nextDay.Add(otStartTime)
            overtimeEnd = nextDay.Add(otEndTime)

            overtimeRecognized = New TimePeriod(overtimeStart, overtimeEnd)

            overtimeWorked = workPeriod.Overlap(overtimeRecognized)
        End If

        Return If(overtimeWorked?.TotalHours, 0)
    End Function

    Public Function ComputeLeaveHours(leavePeriod As TimePeriod, currentShift As CurrentShift) As Decimal
        If currentShift.HasBreaktime() Then
            Dim breakPeriod = currentShift.BreakPeriod

            Return leavePeriod.Difference(breakPeriod).Sum(Function(l) l.TotalHours)
        End If

        Return leavePeriod.TotalHours
    End Function

End Class
