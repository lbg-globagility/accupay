Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore

Public Class DayCalculator

    Public Sub Compute(employee As Employee, currentDate As DateTime, organization As Organization)
        Dim timeLog As TimeLog = Nothing
        Dim shift As ShiftSchedule = Nothing

        Using context = New PayrollContext()
            timeLog = context.TimeLogs.
                SingleOrDefault(Function(t) t.LogDate = currentDate)

            shift = context.ShiftSchedules.
                Include(Function(s) s.Shift).
                Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID)).
                FirstOrDefault(Function(s) s.EffectiveFrom = currentDate)
        End Using

        Dim nextDay = currentDate.AddDays(1)

        Dim fullTimeIn = Calendar.Create(currentDate, TimeLog.TimeIn)
        Dim fullTimeOut = Calendar.Create(If(TimeLog.TimeOut > TimeLog.TimeIn, currentDate, nextDay), TimeLog.TimeOut)
        Dim logPeriod = New TimePeriod(fullTimeIn, fullTimeOut)

        Dim currentShift = New CurrentShift(Shift.Shift, currentDate)

        Dim shiftPeriod = currentShift.ShiftPeriod
        Dim dutyPeriod = shiftPeriod.Overlap(logPeriod)

        Dim nightDiffTimeFrom = organization.NightDifferentialTimeFrom
        Dim nightDiffTimeTo = organization.NightDifferentialTimeTo
        Dim nightDiffStart = Calendar.Create(currentDate, nightDiffTimeFrom)
        Dim nightDiffEnd = Calendar.Create(If(nightDiffTimeTo > nightDiffTimeTo, currentDate, nextDay), nightDiffTimeTo)
        Dim nightDiffPeriod = New TimePeriod(nightDiffStart, nightDiffEnd)


    End Sub

End Class
