Option Strict On

Imports AccuPay
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class DayCalculator

    Private ReadOnly _payrateCalendar As PayratesCalendar
    Private ReadOnly _settings As ListOfValueCollection
    Private ReadOnly _organization As Organization
    Private ReadOnly _employee As Employee
    Private ReadOnly _policy As TimeEntryPolicy

    Public Sub New(organization As Organization, settings As ListOfValueCollection, payrateCalendar As PayratesCalendar, employee As Employee)
        _payrateCalendar = payrateCalendar
        _settings = settings
        _organization = organization
        _employee = employee
        _policy = New TimeEntryPolicy(settings)
    End Sub

    Public Function Compute(currentDate As DateTime,
                            salary As Salary,
                            oldTimeEntries As IList(Of TimeEntry),
                            shiftSchedule As ShiftSchedule,
                            timeLog As TimeLog,
                            overtimes As IList(Of Overtime),
                            leaves As IList(Of Leave)) As TimeEntry
        Dim timeEntry = oldTimeEntries.Where(Function(t) t.Date = currentDate).SingleOrDefault()

        If timeEntry Is Nothing Then
            timeEntry = New TimeEntry() With {
                .EmployeeID = _employee.RowID,
                .OrganizationID = _organization.RowID,
                .Date = currentDate
            }
        End If

        timeEntry.ResetHours()
        timeEntry.ResetPay()

        Dim previousDay = currentDate.AddDays(-1)
        Dim calculator = New TimeEntryCalculator()

        timeEntry.EmployeeShiftID = shiftSchedule?.RowID

        Dim currentShift = New CurrentShift(shiftSchedule, currentDate)
        If _policy.RespectDefaultRestDay Then
            currentShift.SetDefaultRestDay(_employee.DayOfRest)
        End If

        Dim hasTimeLog = timeLog IsNot Nothing
        Dim payrate = _payrateCalendar.Find(currentDate)

        If hasTimeLog And currentShift.HasShift Then
            Dim logPeriod = GetLogPeriod(timeLog, currentShift, currentDate)

            Dim shiftPeriod = currentShift.ShiftPeriod
            Dim dutyPeriod = shiftPeriod.Overlap(logPeriod)

            If dutyPeriod IsNot Nothing Then
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

                If _policy.LateHoursRoundingUp Then
                    Dim lateHours = timeEntry.LateHours

                    If lateHours > 0.5 And lateHours <= 1 Then
                        timeEntry.LateHours = 1
                    ElseIf lateHours >= 2 And lateHours <= 4 Then
                        timeEntry.LateHours = 4
                    End If
                End If

                timeEntry.UndertimeHours = calculator.ComputeUndertimeHours(coveredPeriod, currentShift)

                timeEntry.RegularHours = currentShift.WorkingHours - timeEntry.LateHours - timeEntry.UndertimeHours

                timeEntry.OvertimeHours = overtimes.Sum(
                Function(o) calculator.ComputeOvertimeHours(logPeriod, o, currentShift))

                If _employee.CalcNightDiff And currentShift.IsNightShift Then
                    Dim nightDiffPeriod = GetNightDiffPeriod(currentDate)
                    Dim dawnDiffPeriod = GetNightDiffPeriod(previousDay)

                    timeEntry.NightDiffHours =
                    calculator.ComputeNightDiffHours(dutyPeriod, currentShift, nightDiffPeriod) +
                    calculator.ComputeNightDiffHours(dutyPeriod, currentShift, dawnDiffPeriod)

                    timeEntry.NightDiffOTHours = overtimes.Sum(
                    Function(o) calculator.ComputeNightDiffOTHours(logPeriod, o, currentShift, nightDiffPeriod) +
                        calculator.ComputeNightDiffOTHours(logPeriod, o, currentShift, dawnDiffPeriod))
                End If

                If (_employee.CalcHoliday And payrate.IsRegularHoliday) Or
               (_employee.CalcSpecialHoliday And payrate.IsSpecialNonWorkingHoliday) Then

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

                If currentShift.IsRestDay And _employee.CalcRestDay Then
                    timeEntry.RestDayHours = timeEntry.RegularHours
                    timeEntry.RegularHours = 0

                    timeEntry.RestDayOTHours = timeEntry.OvertimeHours
                    timeEntry.OvertimeHours = 0

                    timeEntry.UndertimeHours = 0
                    timeEntry.LateHours = 0
                End If
            End If
        End If

        Dim isCalculatingRegularHoliday = payrate.IsRegularHoliday And _employee.CalcHoliday
        Dim isCalculatingSpecialHoliday = payrate.IsSpecialNonWorkingHoliday And _employee.CalcSpecialHoliday

        Dim isExemptDueToHoliday =
            (payrate.IsHoliday And (Not _policy.RequiredToWorkLastDay Or HasWorkedLastWorkingDay(currentDate, oldTimeEntries))) And
            (isCalculatingRegularHoliday Or isCalculatingSpecialHoliday Or Not _policy.AbsencesOnHoliday)

        If (Not currentShift.HasShift) Or (timeEntry.RegularHours > 0) Or isExemptDueToHoliday Or currentShift.IsRestDay Or leaves.Any() Then
            timeEntry.AbsentHours = 0
        Else
            timeEntry.AbsentHours = currentShift.WorkingHours
        End If

        Dim dailyRate = 0D
        If _employee.IsDaily Then
            dailyRate = If(salary?.BasicSalary, 0)
        ElseIf _employee.IsMonthly Or _employee.IsFixed Then
            dailyRate = If(salary?.BasicSalary, 0) / (_employee.WorkDaysPerYear / 12)
        End If

        Dim hourlyRate = dailyRate / 8

        Dim nightDiffRate = payrate.NightDifferentialRate - payrate.CommonRate
        Dim nightDiffOTRate = payrate.NightDifferentialOTRate - payrate.OvertimeRate

        Dim restDayNDRate = payrate.RestDayNDRate - payrate.RestDayRate
        Dim restDayNDOTRate = payrate.RestDayNDOTRate - payrate.RestDayOvertimeRate

        timeEntry.BasicDayPay = (
            timeEntry.RegularHours +
            timeEntry.RestDayHours +
            timeEntry.RegularHolidayHours +
            timeEntry.SpecialHolidayHours) * hourlyRate

        If currentDate < _employee.StartDate Then

            timeEntry.ResetHours()
            timeEntry.ResetPay()

        ElseIf Not payrate.IsHoliday Then

            If currentShift.IsWorkingDay Then
                timeEntry.RegularPay = timeEntry.RegularHours * hourlyRate
                timeEntry.OvertimePay = timeEntry.OvertimeHours * hourlyRate * payrate.OvertimeRate
                timeEntry.NightDiffPay = timeEntry.NightDiffHours * hourlyRate * nightDiffRate
                timeEntry.NightDiffOTPay = timeEntry.NightDiffOTHours * hourlyRate * nightDiffOTRate
                timeEntry.LateDeduction = timeEntry.LateHours * hourlyRate
                timeEntry.UndertimeDeduction = timeEntry.UndertimeHours * hourlyRate
            ElseIf currentShift.IsRestDay Then
                If _policy.RestDayInclusive And (_employee.IsMonthly Or _employee.IsFixed) Then
                    timeEntry.RestDayPay = timeEntry.RestDayHours * hourlyRate * (payrate.RestDayRate - 1)
                Else
                    timeEntry.RestDayPay = timeEntry.RestDayHours * hourlyRate * payrate.RestDayRate
                End If

                timeEntry.RestDayOTPay = timeEntry.RestDayOTHours * hourlyRate * payrate.RestDayOvertimeRate
                timeEntry.NightDiffPay = timeEntry.NightDiffHours * hourlyRate * restDayNDRate
                timeEntry.NightDiffOTPay = timeEntry.NightDiffOTHours * hourlyRate * restDayNDOTRate
            End If

            timeEntry.ComputeTotalHours()
            timeEntry.ComputeTotalPay()

        ElseIf payrate.IsHoliday Then

            Dim isHolidayPayInclusive = _employee.IsMonthly Or _employee.IsFixed
            Dim holidayRate = 0D

            If currentShift.IsWorkingDay Then
                holidayRate = payrate.CommonRate

                timeEntry.NightDiffPay = timeEntry.NightDiffHours * hourlyRate * nightDiffRate
                timeEntry.NightDiffOTPay = timeEntry.NightDiffOTHours * hourlyRate * nightDiffOTRate
                timeEntry.SpecialHolidayOTPay = timeEntry.SpecialHolidayOTHours * hourlyRate * payrate.OvertimeRate
                timeEntry.RegularHolidayOTPay = timeEntry.RegularHolidayOTHours * hourlyRate * payrate.OvertimeRate
            ElseIf currentShift.IsRestDay Then
                holidayRate = payrate.RestDayRate

                timeEntry.NightDiffPay = timeEntry.NightDiffHours * hourlyRate * restDayNDRate
                timeEntry.NightDiffOTPay = timeEntry.NightDiffOTHours * hourlyRate * restDayNDOTRate
                timeEntry.SpecialHolidayOTPay = timeEntry.SpecialHolidayOTHours * hourlyRate * payrate.RestDayOvertimeRate
                timeEntry.RegularHolidayOTPay = timeEntry.RegularHolidayOTHours * hourlyRate * payrate.RestDayOvertimeRate
            End If

            timeEntry.RegularPay = timeEntry.RegularHours * hourlyRate
            timeEntry.RestDayPay = timeEntry.RestDayHours * hourlyRate * payrate.RestDayRate
            timeEntry.RestDayOTPay = timeEntry.RestDayOTHours * hourlyRate * payrate.RestDayOvertimeRate

            If _employee.CalcSpecialHoliday And payrate.IsSpecialNonWorkingHoliday Then
                holidayRate = If(isHolidayPayInclusive, holidayRate - 1, holidayRate)

                timeEntry.SpecialHolidayPay = timeEntry.SpecialHolidayHours * hourlyRate * holidayRate
                timeEntry.LeavePay = timeEntry.TotalLeaveHours * hourlyRate * holidayRate
            ElseIf _employee.CalcHoliday And payrate.IsRegularHoliday Then
                timeEntry.RegularHolidayPay = timeEntry.RegularHolidayHours * hourlyRate * (holidayRate - 1)
                timeEntry.LeavePay = timeEntry.TotalLeaveHours * hourlyRate

                Dim holidayCalculationType = _settings.GetStringOrDefault("Payroll Policy.HolidayPay", "Daily")

                Dim basicHolidayPay = 0D
                If holidayCalculationType = "Hourly" Then
                    basicHolidayPay = currentShift.WorkingHours * hourlyRate
                ElseIf holidayCalculationType = "Daily" Then
                    basicHolidayPay = dailyRate
                End If

                If Not isHolidayPayInclusive Then
                    timeEntry.RegularHolidayPay = timeEntry.RegularHolidayPay + If(HasWorkedLastWorkingDay(currentDate, oldTimeEntries), basicHolidayPay, 0)
                End If
            End If

            timeEntry.ComputeTotalHours()
            timeEntry.ComputeTotalPay()
        End If

        Return timeEntry
    End Function

    Private Function HasWorkedLastWorkingDay(current As Date, currentTimeEntries As IList(Of TimeEntry)) As Boolean
        Dim lastPotentialEntry = current.Date.AddDays(-3)

        Dim lastTimeEntries = currentTimeEntries.
            Where(Function(t) lastPotentialEntry <= t.Date And t.Date <= current.Date).
            OrderByDescending(Function(t) t.Date).
            ToList()

        For Each lastTimeEntry In lastTimeEntries
            ' If employee has no shift set for the day, it's not a working day.
            If lastTimeEntry?.ShiftSchedule?.Shift Is Nothing Then
                Continue For
            End If

            If lastTimeEntry?.ShiftSchedule.IsRestDay Then
                If lastTimeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Dim payRate = _payrateCalendar.Find(lastTimeEntry.Date)
            If payRate.IsHoliday Then
                If lastTimeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Return lastTimeEntry.RegularHours > 0 Or lastTimeEntry.TotalLeaveHours > 0
        Next

        Return False
    End Function

    Public Function GetLogPeriod(timeLog As TimeLog, currentShift As CurrentShift, currentDate As Date) As TimePeriod
        Dim logPeriod = TimePeriod.FromTime(timeLog.TimeIn.Value, timeLog.TimeOut.Value, currentDate)

        If currentShift.HasShift Then
            Dim graceTime = _employee.LateGracePeriod

            Dim shiftStart = currentShift.ShiftPeriod.Start
            Dim gracePeriod = New TimePeriod(shiftStart, shiftStart.AddMinutes(graceTime))

            If gracePeriod.Contains(logPeriod.Start) Then
                logPeriod = TimePeriod.FromTime(shiftStart.TimeOfDay, timeLog.TimeOut.Value, currentDate)
            End If
        End If

        Return logPeriod
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

    Public Function GetNightDiffPeriod([date] As DateTime) As TimePeriod
        Dim nightDiffTimeFrom = _organization.NightDifferentialTimeFrom
        Dim nightDiffTimeTo = _organization.NightDifferentialTimeTo

        Return TimePeriod.FromTime(nightDiffTimeFrom, nightDiffTimeTo, [date])
    End Function

End Class
