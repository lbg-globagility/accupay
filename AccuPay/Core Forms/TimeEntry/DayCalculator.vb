Option Strict On

Imports AccuPay
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class DayCalculator

    Private ReadOnly _payrateCalendar As PayratesCalendar
    Private ReadOnly _settings As ListOfValueCollection

    Public Sub New(settings As ListOfValueCollection, payrateCalendar As PayratesCalendar)
        _payrateCalendar = payrateCalendar
        _settings = settings
    End Sub

    Public Function Compute(employee As Employee, currentDate As DateTime, organization As Organization, salary As Salary) As TimeEntry
        Dim timeEntry = New TimeEntry() With {
            .EmployeeID = employee.RowID,
            .OrganizationID = organization.RowID,
            .Date = currentDate
        }

        Dim timeLog As TimeLog = Nothing
        Dim employeeShift As ShiftSchedule = Nothing
        Dim overtimes As IList(Of Overtime) = Nothing
        Dim leaves As IList(Of Leave) = Nothing

        Using context = New PayrollContext()
            timeLog = context.TimeLogs.
                SingleOrDefault(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID) And t.LogDate = currentDate)

            employeeShift = context.ShiftSchedules.
                Include(Function(s) s.Shift).
                Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID)).
                FirstOrDefault(Function(s) s.EffectiveFrom <= currentDate And currentDate <= s.EffectiveTo)

            overtimes = context.Overtimes.
                Where(Function(o) Nullable.Equals(o.EmployeeID, employee.RowID)).
                Where(Function(o) o.OTStartDate <= currentDate And currentDate <= o.OTEndDate).
                ToList()

            leaves = context.Leaves.
                Where(Function(l) Nullable.Equals(l.EmployeeID, employee.RowID)).
                Where(Function(l) l.StartDate <= currentDate).
                Where(Function(l) l.Status = "Approved").
                ToList()
        End Using

        Dim hasShift = employeeShift IsNot Nothing

        Dim previousDay = currentDate.AddDays(-1)
        Dim calculator = New TimeEntryCalculator()

        Dim hasTimeLog = timeLog IsNot Nothing
        Dim payrate = _payrateCalendar.Find(currentDate)
        If hasTimeLog And hasShift Then
            Dim logPeriod = TimePeriod.FromTime(timeLog.TimeIn.Value, timeLog.TimeOut.Value, currentDate)
            Dim currentShift = New CurrentShift(employeeShift.Shift, currentDate)

            Dim shiftPeriod = currentShift.ShiftPeriod
            Dim dutyPeriod = shiftPeriod.Overlap(logPeriod)

            timeEntry.RegularHours = calculator.ComputeRegularHours(dutyPeriod, currentShift)

            Dim coveredPeriod = dutyPeriod

            If leaves.Any() Then
                Dim leave = leaves.FirstOrDefault()
                Dim leavePeriod = GetLeavePeriod(leave, currentShift)

                coveredPeriod = New TimePeriod(
                    {dutyPeriod.Start, leavePeriod.Start}.Min(),
                    {dutyPeriod.End, leavePeriod.End}.Max())

                Dim leaveHours = calculator.ComputeLeaveHours(leavePeriod, currentShift)
                Select Case leave.LeaveType
                    Case LeaveType.Sick
                        timeEntry.SickLeaveHours = leaveHours
                    Case LeaveType.Vacation
                        timeEntry.VacationLeaveHours = leaveHours
                    Case LeaveType.Maternity
                        timeEntry.MaternityLeaveHours = leaveHours
                    Case LeaveType.Others
                        timeEntry.OtherLeaveHours = leaveHours
                End Select
            End If

            timeEntry.LateHours = calculator.ComputeLateHours(coveredPeriod, currentShift)
            timeEntry.UndertimeHours = calculator.ComputeUndertimeHours(coveredPeriod, currentShift)

            timeEntry.OvertimeHours = overtimes.Sum(
                Function(o) calculator.ComputeOvertimeHours(logPeriod, o, currentShift))

            If employee.CalcNightDiff And employeeShift.IsNightShift Then
                Dim nightDiffPeriod = GetNightDiffPeriod(organization, currentDate)
                Dim dawnDiffPeriod = GetNightDiffPeriod(organization, previousDay)

                timeEntry.NightDiffHours =
                    calculator.ComputeNightDiffHours(dutyPeriod, currentShift, nightDiffPeriod) +
                    calculator.ComputeNightDiffHours(dutyPeriod, currentShift, dawnDiffPeriod)
            End If

            If (employee.CalcHoliday And payrate.IsRegularHoliday) Or
               (employee.CalcSpecialHoliday And payrate.IsSpecialNonWorkingHoliday) Then

                If payrate.IsRegularHoliday Then
                    timeEntry.RegularHolidayHours = timeEntry.RegularHours
                    timeEntry.RegularHolidayOTHours = timeEntry.OvertimeHours
                ElseIf payrate.IsSpecialNonWorkingHoliday Then
                    timeEntry.SpecialHolidayHours = timeEntry.RegularHours
                    timeEntry.SpecialHolidayOTHours = timeEntry.OvertimeHours
                End If

                timeEntry.RegularHours = 0
                timeEntry.OvertimeHours = 0

                timeEntry.LateHours = 0
                timeEntry.UndertimeHours = 0
            End If

            If employeeShift.IsRestDay And employee.CalcRestDay Then
                timeEntry.RestDayHours = timeEntry.RegularHours
                timeEntry.RegularHours = 0

                timeEntry.RestDayOTHours = timeEntry.OvertimeHours
                timeEntry.OvertimeHours = 0

                timeEntry.UndertimeHours = 0
                timeEntry.LateHours = 0
            End If
        End If

        Dim hasWorkedLastWorkingDay = False
        Dim requiredToWorkLastWorkingDay = _settings.GetBoolean("Payroll Policy.HolidayLastWorkingDayOrAbsent", False)
        Dim allowanceAbsenceOnHoliday = _settings.GetBoolean("Payroll Policy.holiday.allowabsence", False)
        Dim isCalculatingRegularHoliday = payrate.IsRegularHoliday And employee.CalcHoliday
        Dim isCalculatingSpecialHoliday = payrate.IsSpecialNonWorkingHoliday And employee.CalcSpecialHoliday

        Dim isExemptDueToHoliday =
            (payrate.IsHoliday And (Not requiredToWorkLastWorkingDay Or hasWorkedLastWorkingDay)) And
            (isCalculatingRegularHoliday Or isCalculatingSpecialHoliday Or Not allowanceAbsenceOnHoliday)

        If (Not hasShift) Or (timeEntry.RegularHours > 0) Or isExemptDueToHoliday Or employeeShift.IsRestDay Or leaves.Any() Then
            timeEntry.AbsentHours = 0
        Else
            timeEntry.AbsentHours = employeeShift.Shift.WorkHours
        End If

        Dim dailyRate = 0D
        If employee.IsDaily Then
            dailyRate = salary.BasicSalary
        ElseIf employee.IsMonthly Or employee.IsFixed Then
            dailyRate = salary.BasicSalary / (employee.WorkDaysPerYear / 12)
        End If

        Dim hourlyRate = dailyRate / 8

        If currentDate < employee.StartDate Then
            timeEntry.RegularHours = 0
            timeEntry.OvertimeHours = 0
            timeEntry.NightDiffHours = 0
            timeEntry.NightDiffOTHours = 0
            timeEntry.RegularHolidayHours = 0
            timeEntry.RegularHolidayOTHours = 0
            timeEntry.SpecialHolidayHours = 0
            timeEntry.SpecialHolidayOTHours = 0
            timeEntry.RestDayHours = 0
            timeEntry.RestDayOTHours = 0
            timeEntry.VacationLeaveHours = 0
            timeEntry.SickLeaveHours = 0
            timeEntry.MaternityLeaveHours = 0
            timeEntry.OtherLeaveHours = 0
            timeEntry.UndertimeHours = 0
            timeEntry.LateHours = 0
            timeEntry.AbsentHours = 0
        ElseIf Not payrate.IsHoliday Then

            If employeeShift.IsRestDay Then

                Dim isRestDayInclusive = _settings.GetBoolean("Payroll Policy.restday.inclusiveofbasicpay")

                If isRestDayInclusive And employee.IsMonthly Or employee.IsFixed Then
                    timeEntry.RestDayPay = timeEntry.RestDayHours * hourlyRate * (payrate.RestDayRate - 1)
                Else
                    timeEntry.RestDayPay = timeEntry.RestDayHours * hourlyRate * payrate.RestDayRate
                End If

                timeEntry.RestDayOTPay = timeEntry.RestDayOTHours * hourlyRate * payrate.RestDayOvertimeRate
            Else
                timeEntry.RegularPay = timeEntry.RegularHours * hourlyRate
                timeEntry.OvertimeHours = timeEntry.OvertimeHours * hourlyRate * payrate.OvertimeRate
                timeEntry.NightDiffHours = timeEntry.NightDiffHours * hourlyRate * payrate.NightDifferentialRate
                timeEntry.NightDiffOTHours = timeEntry.NightDiffOTHours * hourlyRate * payrate.NightDifferentialOTRate

                timeEntry.LateDeduction = timeEntry.LateHours * hourlyRate
                timeEntry.UndertimeDeduction = timeEntry.UndertimeHours * hourlyRate
            End If

        ElseIf payrate.IsHoliday Then

        End If

        Return timeEntry
    End Function

    Public Function GetLeavePeriod(leave As Leave, currentShift As CurrentShift) As TimePeriod
        Dim startTime = If(leave.StartTime, currentShift.Shift.TimeFrom)
        Dim endTime = If(leave.EndTime, currentShift.Shift.TimeTo)

        Dim leavePeriod = TimePeriod.FromTime(startTime, endTime, currentShift.Date)

        If Not currentShift.ShiftPeriod.Intersects(leavePeriod) Then
            Dim nextDay = currentShift.Date.AddDays(1)
            leavePeriod = TimePeriod.FromTime(startTime, endTime, nextDay)
        End If

        Return leavePeriod
    End Function

    Public Function GetNightDiffPeriod(organization As Organization, [date] As DateTime) As TimePeriod
        Dim nightDiffTimeFrom = organization.NightDifferentialTimeFrom
        Dim nightDiffTimeTo = organization.NightDifferentialTimeTo

        Return TimePeriod.FromTime(nightDiffTimeFrom, nightDiffTimeTo, [date])
    End Function

End Class
