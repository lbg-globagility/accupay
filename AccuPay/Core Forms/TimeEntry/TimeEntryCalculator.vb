Option Strict On

Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class TimeEntryCalculator

    Public Sub Calculate(employee As Employee, dateToday As Date)
        Using context = New PayrollContext()
            Dim timeLog = context.TimeLogs.
                Where(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID)).FirstOrDefault()

            Dim shiftSchedule = context.ShiftSchedules.
                Include(Function(s) s.Shift).
                Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID)).
                Where(Function(s) s.EffectiveFrom <= dateToday And dateToday <= s.EffectiveTo).
                FirstOrDefault()

            Dim payRate = context.PayRates.
                Where(Function(p) p.RateDate = dateToday).
                Where(Function(p) Nullable.Equals(p.OrganizationID, employee.OrganizationID)).
                FirstOrDefault()

            Dim dateTomorrow = dateToday.AddDays(1)
            Dim timeIn = timeLog.TimeIn
            Dim timeOut = timeLog.TimeOut

            Dim workStart = CombineDateAndTime(dateToday, timeLog.TimeIn.Value)
            Dim workEnd = CombineDateAndTime(If(timeOut > timeIn, dateToday, dateTomorrow), timeOut.Value)

            Dim shiftTimeStart = shiftSchedule.Shift.TimeFrom
            Dim shiftTimeTo = shiftSchedule.Shift.TimeTo

            Dim shiftStart = CombineDateAndTime(dateToday, shiftTimeStart)
            Dim shiftEnd = CombineDateAndTime(If(shiftTimeTo > shiftTimeStart, dateToday, dateTomorrow), shiftTimeTo)

            Dim breakTimeStart = shiftSchedule.Shift.BreaktimeFrom
            Dim breakTimeEnd = shiftSchedule.Shift.BreaktimeTo
            Dim hasBreaktime = (breakTimeStart IsNot Nothing) And (breakTimeEnd IsNot Nothing)

            Dim breakStart = CombineDateAndTime(If(breakTimeStart > shiftTimeStart, dateToday, dateTomorrow), breakTimeStart.Value)
            Dim breakEnd = CombineDateAndTime(If(breakTimeEnd > shiftTimeStart, dateToday, dateTomorrow), breakTimeEnd.Value)

            Dim dutyStart = {workStart, shiftStart}.Max()
            Dim dutyEnd = {workEnd, shiftEnd}.Min()

            Dim lateHours = 0D

            If dutyStart > shiftStart Then
                If hasBreaktime Then
                    Dim hoursLateBeforeBreak = 0D
                    Dim hoursLateAfterBreak = 0D

                    If shiftStart < breakStart Then
                        hoursLateBeforeBreak = ComputeHours(shiftStart, {dutyStart, breakStart}.Min())
                    End If

                    If dutyStart > breakEnd Then
                        hoursLateAfterBreak = ComputeHours(breakEnd, dutyStart)
                    End If

                    lateHours = hoursLateBeforeBreak + hoursLateAfterBreak
                Else
                    lateHours = ComputeHours(shiftStart, dutyStart)
                End If
            End If
        End Using
    End Sub

    Private Function ComputeHours(start As Date, [end] As Date) As Decimal
        Return CDec(([end] - start).TotalHours)
    End Function

    Private Function CombineDateAndTime([date] As DateTime, time As TimeSpan) As DateTime
        Return [date].Add(time)
    End Function

End Class
