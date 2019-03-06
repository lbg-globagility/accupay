Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Extensions
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

    Public Function ComputeBreakTimeLateHours(workPeriod As TimePeriod, currentShift As CurrentShift, timeAttendanceLogs As IList(Of TimeAttendanceLog)) As Decimal
        Dim shiftPeriod = currentShift.ShiftPeriod

        Dim startTime = If(workPeriod.Start >= shiftPeriod.Start, workPeriod.Start, shiftPeriod.Start)
        Dim endTime = If(workPeriod.End >= shiftPeriod.End, shiftPeriod.End, workPeriod.End)

        Dim logs = timeAttendanceLogs.
                    Where(Function(l) l.TimeStamp >= startTime).
                    Where(Function(l) l.TimeStamp <= endTime).
                    OrderBy(Function(l) l.TimeStamp).
                    ToList

        'get the late periods
        Dim latePeriods As New List(Of TimePeriod)

        Dim lastOut As Date
        Dim firstIn As Date
        Dim isLookingForBreakTimeOut As Boolean = True

        For Each log In logs
            If isLookingForBreakTimeOut Then
                If log.IsTimeIn = False Then
                    lastOut = log.TimeStamp

                    isLookingForBreakTimeOut = False
                End If
            Else
                'still get the time out to get the last OUT
                If log.IsTimeIn = False Then
                    lastOut = log.TimeStamp

                Else
                    'here in else, we found the first IN since the employee OUT for breaktime
                    'this will be the late timeperiod
                    firstIn = log.TimeStamp

                    isLookingForBreakTimeOut = True

                    latePeriods.Add(New TimePeriod(lastOut, firstIn))
                End If
            End If

        Next

        'compute the late hours
        Return latePeriods.Sum(Function(l) l.TotalHours)

    End Function

    Public Function ComputeLateMinutes(workPeriod As TimePeriod, currentShift As CurrentShift) As Decimal
        Dim shiftPeriod = currentShift.ShiftPeriod

        If workPeriod.EarlierThan(shiftPeriod) Then
            Return 0
        End If

        Dim latePeriod = New TimePeriod(shiftPeriod.Start, workPeriod.Start)

        If currentShift.HasBreaktime Then
            Dim breakPeriod = currentShift.BreakPeriod
            Dim latePeriods = latePeriod.Difference(breakPeriod)

            Return latePeriods.Sum(Function(l) l.TotalMinutes)
        End If

        Return latePeriod.TotalMinutes
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

    Public Function ComputeNightDiffHours(workPeriod As TimePeriod, currentShift As CurrentShift, nightDiffPeriod As TimePeriod, shouldBreakTime As Boolean) As Decimal
        If Not workPeriod.Intersects(nightDiffPeriod) Then
            Return 0D
        End If

        Dim nightWorked = workPeriod.Overlap(nightDiffPeriod)

        Dim nightDiffHours = 0D

        If shouldBreakTime AndAlso currentShift.HasBreaktime Then
            Dim breakPeriod = currentShift.BreakPeriod
            Dim nightWorkedPeriods = nightWorked.Difference(breakPeriod)

            nightDiffHours = nightWorkedPeriods.Sum(Function(n) n.TotalHours)
        Else
            nightDiffHours = nightWorked.TotalHours
        End If

        Return nightDiffHours
    End Function

    Public Function ComputeOvertimeHours(workPeriod As TimePeriod, overtime As Overtime, shift As CurrentShift, breaktime As TimePeriod) As Decimal
        Dim overtimeWorked = GetOvertimeWorked(workPeriod, overtime, shift)

        Dim overtimeHours As Decimal?
        If breaktime IsNot Nothing Then
            overtimeHours = overtimeWorked?.Difference(breaktime).Sum(Function(o) o.TotalHours)
        Else
            overtimeHours = overtimeWorked?.TotalHours
        End If

        Return If(overtimeHours, 0)
    End Function

    Public Function ComputeOvertimeMinutes(workPeriod As TimePeriod, overtime As Overtime, shift As CurrentShift, breaktime As TimePeriod) As Decimal
        Dim overtimeWorked = GetOvertimeWorked(workPeriod, overtime, shift)

        Dim overtimeHours As Decimal?
        If breaktime IsNot Nothing Then
            overtimeHours = overtimeWorked?.Difference(breaktime).Sum(Function(o) o.TotalMinutes)
        Else
            overtimeHours = overtimeWorked?.TotalMinutes
        End If

        Return If(overtimeHours, 0)
    End Function

    Public Function ComputeNightDiffOTHours(workPeriod As TimePeriod, overtime As Overtime, shift As CurrentShift, nightDiffPeriod As TimePeriod, breaktime As TimePeriod) As Decimal
        Dim overtimeWorked = GetOvertimeWorked(workPeriod, overtime, shift)

        Dim nightOvertimeWorked = overtimeWorked?.Overlap(nightDiffPeriod)

        Dim nightDiffOTHours As Decimal?
        If breaktime IsNot Nothing Then
            nightDiffOTHours = nightOvertimeWorked?.Difference(breaktime).Sum(Function(o) o.TotalHours)
        Else
            nightDiffOTHours = nightOvertimeWorked?.TotalHours
        End If

        Return If(nightDiffOTHours, 0)
    End Function

    Private Function GetOvertimeWorked(workPeriod As TimePeriod, overtime As Overtime, shift As CurrentShift) As TimePeriod
        Dim otStartTime = If(overtime.OTStartTime, shift.End.TimeOfDay)
        Dim otEndTime = If(overtime.OTEndTime, shift.Start.TimeOfDay)

        Dim overtimeStart = overtime.OTStartDate.Add(otStartTime)
        Dim nextDay = overtime.OTStartDate.AddDays(1)

        If otEndTime = otStartTime Then
            Return Nothing
        End If

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

        Return overtimeWorked
    End Function

    Public Function ComputeLeaveHours(leavePeriod As TimePeriod, currentShift As CurrentShift) As Decimal
        If currentShift.HasBreaktime() Then
            Dim breakPeriod = currentShift.BreakPeriod

            Return leavePeriod.Difference(breakPeriod).Sum(Function(l) l.TotalHours)
        End If

        Return leavePeriod.TotalHours
    End Function

End Class
