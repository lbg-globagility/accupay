Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore

Public Class DayCalculator

    Public Sub Compute(employee As Employee, currentDate As DateTime)
        Using context = New PayrollContext()
            Dim timeLog = context.TimeLogs.
                SingleOrDefault(Function(t) t.LogDate = currentDate)

            Dim shift = context.ShiftSchedules.
                Include(Function(s) s.Shift).
                Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID)).
                FirstOrDefault(Function(s) s.EffectiveFrom = currentDate)

            Dim nextDay = currentDate.AddDays(1)

            Dim fullTimeIn = Calendar.Create(currentDate, timeLog.TimeIn)
            Dim fullTimeOut = Calendar.Create(If(timeLog.TimeOut > timeLog.TimeIn, currentDate, nextDay), timeLog.TimeOut)
            Dim logPeriod = New TimePeriod(fullTimeIn, fullTimeOut)

            Dim currentShift = New CurrentShift(shift.Shift, currentDate)

            Dim shiftPeriod = currentShift.ShiftPeriod
            Dim dutyPeriod = shiftPeriod.Overlap(logPeriod)
        End Using
    End Sub

End Class
