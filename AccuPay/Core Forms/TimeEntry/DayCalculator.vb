Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore

Public Class DayCalculator

    Public Function Compute(employee As Employee, currentDate As DateTime, organization As Organization) As TimeEntry
        Dim timeEntry = New TimeEntry() With {
            .EmployeeID = employee.RowID,
            .OrganizationID = organization.RowID,
            .Date = currentDate
        }

        Dim timeLog As TimeLog = Nothing
        Dim shift As ShiftSchedule = Nothing
        Dim overtimes As IList(Of Overtime) = Nothing

        Using context = New PayrollContext()
            timeLog = context.TimeLogs.
                SingleOrDefault(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID) And t.LogDate = currentDate)

            shift = context.ShiftSchedules.
                Include(Function(s) s.Shift).
                Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID)).
                FirstOrDefault(Function(s) s.EffectiveFrom <= currentDate And currentDate <= s.EffectiveTo)

            overtimes = context.Overtimes.
                Where(Function(o) Nullable.Equals(o.EmployeeID, employee.RowID)).
                Where(Function(o) o.OTStartDate <= currentDate And currentDate <= o.OTEndDate).
                ToList()
        End Using

        Dim hasShift = shift IsNot Nothing

        Dim previousDay = currentDate.AddDays(-1)
        Dim calculator = New TimeEntryCalculator()

        Dim hasTimeLog = timeLog IsNot Nothing

        If hasTimeLog And hasShift Then
            Dim logPeriod = TimePeriod.FromTime(timeLog.TimeIn.Value, timeLog.TimeOut.Value, currentDate)
            Dim currentShift = New CurrentShift(shift.Shift, currentDate)

            Dim shiftPeriod = currentShift.ShiftPeriod
            Dim dutyPeriod = shiftPeriod.Overlap(logPeriod)

            timeEntry.RegularHours = calculator.ComputeRegularHours(dutyPeriod, currentShift)
            timeEntry.LateHours = calculator.ComputeLateHours(dutyPeriod, currentShift)
            timeEntry.UndertimeHours = calculator.ComputeUndertimeHours(dutyPeriod, currentShift)

            timeEntry.OvertimeHours = overtimes.Sum(
                Function(o) calculator.ComputeOvertimeHours(logPeriod, o, currentShift))
        End If

        Dim nightDiffPeriod = GetNightDiffPeriod(organization, currentDate)
        Dim dawnDiffPeriod = GetNightDiffPeriod(organization, previousDay)

        Return timeEntry
    End Function

    Public Function GetNightDiffPeriod(organization As Organization, [date] As DateTime) As TimePeriod
        Dim nightDiffTimeFrom = organization.NightDifferentialTimeFrom
        Dim nightDiffTimeTo = organization.NightDifferentialTimeTo

        Return TimePeriod.FromTime(nightDiffTimeFrom, nightDiffTimeTo, [date])
    End Function

End Class
