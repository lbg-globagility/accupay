Option Strict On

Imports AccuPay.Entity
Imports PayrollSys

Public Class DayCalculator

    Private ReadOnly _payrateCalendar As PayratesCalendar
    Private ReadOnly _settings As ListOfValueCollection
    Private ReadOnly _organization As Organization
    Private ReadOnly _employee As Employee
    Private ReadOnly _policy As TimeEntryPolicy

    Private ReadOnly _minutesPerHour As Decimal = 60
    Private _lateSkipCountRounding As Boolean = False
    Private _lateSkipCount As Decimal = 0
    Private _overtimeSkipCountRounding As Boolean = False
    Private _overtimeSkipCount As Decimal = 0

    Public Sub New(organization As Organization, settings As ListOfValueCollection, payrateCalendar As PayratesCalendar, employee As Employee)
        _payrateCalendar = payrateCalendar
        _settings = settings
        _organization = organization
        _employee = employee
        _policy = New TimeEntryPolicy(settings)

        _lateSkipCountRounding = _policy.LateSkipCountRounding
        If _lateSkipCountRounding Then
            _lateSkipCount = Convert.ToDecimal(settings.GetValue("SkipCount"))
        End If

        _overtimeSkipCountRounding = _policy.OvertimeSkipCountRounding
        If _overtimeSkipCountRounding Then
            _overtimeSkipCount = Convert.ToDecimal(settings.GetValue("OvertimeSkipCount"))
        End If
    End Sub

    Public Function Compute(currentDate As DateTime,
                            salary As Salary,
                            oldTimeEntries As IList(Of TimeEntry),
                            shiftSchedule As ShiftSchedule,
                            timeLog As TimeLog,
                            overtimes As IList(Of Overtime),
                            officialBusiness As OfficialBusiness,
                            leaves As IList(Of Leave),
                            timeAttendanceLogs As IList(Of TimeAttendanceLog)) As TimeEntry
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

        Dim isBetweenSalaryDates As Boolean = False
        If salary IsNot Nothing Then
            isBetweenSalaryDates = currentDate.Date >= salary.EffectiveFrom AndAlso
                                    (salary.EffectiveTo Is Nothing OrElse currentDate.Date <= salary.EffectiveTo.Value)
        End If

        If Not isBetweenSalaryDates Then
            Return timeEntry
        End If

        Dim currentShift = New CurrentShift(shiftSchedule, currentDate)
        If _policy.RespectDefaultRestDay Then
            currentShift.SetDefaultRestDay(_employee.DayOfRest)
        End If

        Dim hasWorkedLastDay = HasWorkedLastWorkingDay(currentDate, oldTimeEntries)
        Dim payrate = _payrateCalendar.Find(currentDate)

        ComputeHours(currentDate, timeEntry, timeLog, officialBusiness, leaves, overtimes, oldTimeEntries, timeAttendanceLogs, currentShift, hasWorkedLastDay)
        ComputePay(timeEntry, currentDate, currentShift, salary, payrate, hasWorkedLastDay)

        Return timeEntry
    End Function

    Private Sub ComputeHours(currentDate As Date,
                             timeEntry As TimeEntry,
                             timeLog As TimeLog,
                             officialBusiness As OfficialBusiness,
                             leaves As IList(Of Leave),
                             overtimes As IList(Of Overtime),
                             oldTimeEntries As IList(Of TimeEntry),
                             timeAttendanceLogs As IList(Of TimeAttendanceLog),
                             currentShift As CurrentShift,
                             hasWorkedLastDay As Boolean)
        Dim previousDay = currentDate.AddDays(-1)
        Dim calculator = New TimeEntryCalculator()

        timeEntry.EmployeeShiftID = currentShift.ShiftSchedule?.RowID

        Dim hasTimeLog = (timeLog?.TimeIn IsNot Nothing And timeLog?.TimeOut IsNot Nothing) Or
            officialBusiness IsNot Nothing
        Dim payrate = _payrateCalendar.Find(currentDate)

        Dim logPeriod As TimePeriod = Nothing
        If hasTimeLog And currentShift.HasShift Then
            logPeriod = GetLogPeriod(timeLog, officialBusiness, currentShift, currentDate)
        End If

        If logPeriod IsNot Nothing Then
            Dim shiftPeriod = currentShift.ShiftPeriod
            Dim dutyPeriod = shiftPeriod.Overlap(logPeriod)

            If dutyPeriod IsNot Nothing Then
                timeEntry.RegularHours = calculator.ComputeRegularHours(dutyPeriod, currentShift)

                Dim coveredPeriod = dutyPeriod

                Dim leavePeriod As TimePeriod = Nothing
                If leaves.Any() Then
                    Dim leave = leaves.FirstOrDefault()
                    leavePeriod = GetLeavePeriod(leave, currentShift)

                    coveredPeriod = New TimePeriod(
                        {dutyPeriod.Start, leavePeriod.Start}.Min(),
                        {dutyPeriod.End, leavePeriod.End}.Max())

                    Dim leaveHours = calculator.ComputeLeaveHours(leavePeriod, currentShift)
                    timeEntry.SetLeaveHours(leave.LeaveType, leaveHours)
                End If

                timeEntry.LateHours = calculator.ComputeLateHours(coveredPeriod, currentShift)
                timeEntry.LateHours += calculator.ComputeBreakTimeLateHours(coveredPeriod, currentShift, timeAttendanceLogs)

                If _policy.LateHoursRoundingUp Then
                    Dim lateHours = timeEntry.LateHours

                    If lateHours > 0.5 And lateHours <= 1 Then
                        timeEntry.LateHours = 1
                    ElseIf lateHours >= 2 And lateHours <= 4 Then
                        timeEntry.LateHours = 4
                    End If
                End If

                timeEntry = LateSchemeSkipCountHours(timeEntry, currentShift, calculator, coveredPeriod)

                timeEntry.UndertimeHours = calculator.ComputeUndertimeHours(coveredPeriod, currentShift)

                timeEntry.RegularHours = currentShift.WorkingHours - (timeEntry.LateHours + timeEntry.UndertimeHours)

                If leavePeriod IsNot Nothing Then
                    Dim coveredLeavePeriod = New TimePeriod(
                        {currentShift.Start, leavePeriod.Start}.Max(),
                        {currentShift.End, leavePeriod.End}.Min())

                    timeEntry.RegularHours -= coveredLeavePeriod.TotalHours
                End If

                Dim nightBreaktime As TimePeriod = Nothing
                If _policy.HasNightBreaktime Then
                    nightBreaktime = New TimePeriod(
                        currentDate.Add(TimeSpan.Parse("21:00")),
                        currentDate.Add(TimeSpan.Parse("22:00")))
                End If

                timeEntry.OvertimeHours = overtimes.Sum(
                    Function(o) calculator.ComputeOvertimeHours(logPeriod, o, currentShift, nightBreaktime))

                timeEntry = OvertimeSchemeSkipCountHours(timeEntry, overtimes, currentShift, calculator, logPeriod, nightBreaktime)

                ComputeNightDiffHours(timeEntry, currentShift, dutyPeriod, logPeriod, currentDate, previousDay, overtimes, nightBreaktime)
                ComputeHolidayHours(payrate, timeEntry)
                ComputeRestDayHours(currentShift, timeEntry, logPeriod)

                timeEntry.BasicHours =
                    timeEntry.RegularHours +
                    timeEntry.RestDayHours +
                    timeEntry.RegularHolidayHours +
                    timeEntry.SpecialHolidayHours
            End If
        End If

        ComputeAbsentHours(timeEntry, payrate, hasWorkedLastDay, currentShift, leaves)
        ComputeLeaveHours(hasTimeLog, leaves, currentShift, timeEntry)
    End Sub

    'Private Function fsdfsd(currentShift As CurrentShift, leavePeriod As TimePeriod) As Decimal
    '    If leavePeriod.Start Then
    '        Return 0
    'End Function

    Private Function OvertimeSchemeSkipCountHours(timeEntry As TimeEntry, overtimes As IList(Of Overtime), currentShift As CurrentShift, calculator As TimeEntryCalculator, logPeriod As TimePeriod, nightBreaktime As TimePeriod) As TimeEntry
        If _overtimeSkipCountRounding Then
            Dim overtimeMinutes = overtimes.Sum(
            Function(o) calculator.ComputeOvertimeMinutes(logPeriod, o, currentShift, nightBreaktime))

            Dim empGracePeriod = _employee.LateGracePeriod
            Dim empGracePeriodHrs = empGracePeriod / _minutesPerHour
            Dim hasNoGracePeriod = empGracePeriod = 0
            Dim hasGracePeriod = Not hasNoGracePeriod

            If hasGracePeriod _
                And overtimeMinutes > empGracePeriod Then

                Dim properValue = Math.Ceiling(overtimeMinutes / empGracePeriod)
                Dim lessOne = properValue - 1
                Dim properLateHours = lessOne * _overtimeSkipCount

                timeEntry.OvertimeHours = properLateHours / _minutesPerHour
            ElseIf hasGracePeriod _
                And overtimeMinutes <= empGracePeriod Then

                timeEntry.OvertimeHours = 0
            End If
        End If

        Return timeEntry
    End Function

    Private Function LateSchemeSkipCountHours(timeEntry As TimeEntry,
                                              currentShift As CurrentShift,
                                              calculator As TimeEntryCalculator,
                                              coveredPeriod As TimePeriod) As TimeEntry
        If _lateSkipCountRounding Then
            Dim lateMinutes = calculator.ComputeLateMinutes(coveredPeriod, currentShift)

            Dim empGracePeriod = _employee.LateGracePeriod
            Dim empGracePeriodHrs = empGracePeriod / _minutesPerHour
            Dim hasNoGracePeriod = empGracePeriod = 0
            If Not hasNoGracePeriod _
                And lateMinutes > empGracePeriod Then

                Dim properValue = Math.Ceiling(lateMinutes / empGracePeriod)
                Dim lessOne = properValue - 1
                Dim properLateHours = lessOne * _lateSkipCount

                timeEntry.LateHours = properLateHours / _minutesPerHour
            End If
        End If

        Return timeEntry
    End Function

    Private Function Skipper(repeater As Integer, startCount As Decimal) As Decimal
        Dim initialValaue = startCount + 1

        Dim index = repeater - 1
        Dim i = 0
        While i < index
            initialValaue += startCount
            i += 1
        End While

        Return initialValaue
    End Function

    Private Sub ComputePay(timeEntry As TimeEntry,
                           currentDate As Date,
                           currentShift As CurrentShift,
                           salary As Salary,
                           payrate As PayRate,
                           hasWorkedLastDay As Boolean)
        If currentDate < _employee.StartDate Then
            timeEntry.ResetHours()
            timeEntry.ResetPay()

            Return
        End If

        Dim dailyRate = GetDailyRate(salary)
        Dim hourlyRate = dailyRate / 8

        timeEntry.BasicDayPay = timeEntry.BasicHours * hourlyRate
        timeEntry.RegularPay = timeEntry.RegularHours * hourlyRate

        timeEntry.LateDeduction = timeEntry.LateHours * hourlyRate
        timeEntry.UndertimeDeduction = timeEntry.UndertimeHours * hourlyRate
        timeEntry.AbsentDeduction = timeEntry.AbsentHours * hourlyRate

        timeEntry.OvertimePay = timeEntry.OvertimeHours * hourlyRate * payrate.OvertimeRate

        Dim restDayRate = payrate.RestDayRate
        If _policy.RestDayInclusive And (_employee.IsMonthly Or _employee.IsFixed) Then
            restDayRate -= 1
        End If
        timeEntry.RestDayPay = timeEntry.RestDayHours * hourlyRate * restDayRate

        timeEntry.RestDayOTPay = timeEntry.RestDayOTHours * hourlyRate * payrate.RestDayOvertimeRate
        timeEntry.LeavePay = timeEntry.TotalLeaveHours * hourlyRate

        If currentShift.IsWorkingDay Then
            Dim nightDiffRate = payrate.NightDifferentialRate - payrate.CommonRate
            Dim nightDiffOTRate = payrate.NightDifferentialOTRate - payrate.OvertimeRate

            Dim notEntitledForLegalHolidayRate = Not _employee.CalcHoliday And payrate.IsSpecialNonWorkingHoliday
            Dim notEntitledForSpecialNonWorkingHolidayRate = Not _employee.CalcSpecialHoliday And payrate.IsSpecialNonWorkingHoliday

            If notEntitledForLegalHolidayRate Or
                notEntitledForSpecialNonWorkingHolidayRate Then

                nightDiffRate = (payrate.NightDifferentialRate / payrate.CommonRate) Mod 1
                nightDiffOTRate = (payrate.NightDifferentialOTRate / payrate.OvertimeRate) Mod 1
            End If

            timeEntry.NightDiffPay = timeEntry.NightDiffHours * hourlyRate * nightDiffRate
            timeEntry.NightDiffOTPay = timeEntry.NightDiffOTHours * hourlyRate * nightDiffOTRate
        ElseIf currentShift.IsRestDay Then
            Dim restDayNDRate = payrate.RestDayNDRate - payrate.RestDayRate
            Dim restDayNDOTRate = payrate.RestDayNDOTRate - payrate.RestDayOvertimeRate

            timeEntry.NightDiffPay = timeEntry.NightDiffHours * hourlyRate * restDayNDRate
            timeEntry.NightDiffOTPay = timeEntry.NightDiffOTHours * hourlyRate * restDayNDOTRate
        End If

        If payrate.IsHoliday Then
            Dim holidayRate = 0D
            Dim holidayOTRate = 0D

            If currentShift.IsWorkingDay Then
                holidayRate = payrate.CommonRate
                holidayOTRate = payrate.OvertimeRate
            ElseIf currentShift.IsRestDay Then
                holidayRate = payrate.RestDayRate
                holidayOTRate = payrate.RestDayOvertimeRate
            End If

            timeEntry.SpecialHolidayOTPay = timeEntry.SpecialHolidayOTHours * hourlyRate * holidayOTRate
            timeEntry.RegularHolidayOTPay = timeEntry.RegularHolidayOTHours * hourlyRate * holidayOTRate

            Dim isHolidayPayInclusive = _employee.IsMonthly Or _employee.IsFixed

            If _employee.CalcSpecialHoliday And payrate.IsSpecialNonWorkingHoliday Then
                holidayRate = If(isHolidayPayInclusive, holidayRate - 1, holidayRate)

                timeEntry.SpecialHolidayPay = timeEntry.SpecialHolidayHours * hourlyRate * holidayRate
                timeEntry.LeavePay = timeEntry.TotalLeaveHours * hourlyRate * holidayRate
            ElseIf _employee.CalcHoliday And payrate.IsRegularHoliday Then
                timeEntry.RegularHolidayPay = timeEntry.RegularHolidayHours * hourlyRate * (holidayRate - 1)

                Dim holidayCalculationType = _settings.GetStringOrDefault("Payroll Policy.HolidayPay", "Daily")

                Dim basicHolidayPay = 0D
                If holidayCalculationType = "Hourly" Then
                    basicHolidayPay = currentShift.WorkingHours * hourlyRate
                ElseIf holidayCalculationType = "Daily" Then
                    basicHolidayPay = dailyRate
                End If

                Dim isEntitledToHolidayPay =
                    (Not isHolidayPayInclusive) AndAlso
                    (hasWorkedLastDay Or (Not _policy.RequiredToWorkLastDayForHolidayPay))

                If isEntitledToHolidayPay Then
                    timeEntry.RegularHolidayPay += basicHolidayPay
                End If
            End If
        End If

        timeEntry.ComputeTotalHours()
        timeEntry.ComputeTotalPay()
    End Sub

    Private Sub ComputeNightDiffHours(timeEntry As TimeEntry,
                                      currentShift As CurrentShift,
                                      dutyPeriod As TimePeriod,
                                      logPeriod As TimePeriod,
                                      currentDate As Date,
                                      previousDay As Date,
                                      overtimes As IList(Of Overtime),
                                      nightBreaktime As TimePeriod)
        Dim calculator = New TimeEntryCalculator()

        If _employee.CalcNightDiff And currentShift.IsNightShift Then
            Dim nightDiffPeriod = GetNightDiffPeriod(currentDate)
            Dim dawnDiffPeriod = GetNightDiffPeriod(previousDay)

            timeEntry.NightDiffHours =
                calculator.ComputeNightDiffHours(dutyPeriod, currentShift, nightDiffPeriod, _policy.HasNightBreaktime) +
                calculator.ComputeNightDiffHours(dutyPeriod, currentShift, dawnDiffPeriod, _policy.HasNightBreaktime)

            timeEntry.NightDiffOTHours = overtimes.Sum(
                Function(o) calculator.ComputeNightDiffOTHours(logPeriod, o, currentShift, nightDiffPeriod, nightBreaktime) +
                    calculator.ComputeNightDiffOTHours(logPeriod, o, currentShift, dawnDiffPeriod, nightBreaktime))
        End If
    End Sub

    Private Sub ComputeHolidayHours(payrate As PayRate, timeEntry As TimeEntry)
        If Not (
            (_employee.CalcHoliday And payrate.IsRegularHoliday) Or
            (_employee.CalcSpecialHoliday And payrate.IsSpecialNonWorkingHoliday)) Then
            Return
        End If

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
    End Sub

    Private Sub ComputeRestDayHours(currentShift As CurrentShift, timeEntry As TimeEntry, logPeriod As TimePeriod)
        If Not (currentShift.IsRestDay And _employee.CalcRestDay) Then
            Return
        End If

        If _policy.IgnoreShiftOnRestDay Then
            timeEntry.RegularHours = logPeriod.TotalHours
        End If

        timeEntry.RestDayHours = timeEntry.RegularHours
        timeEntry.RegularHours = 0

        timeEntry.RestDayOTHours = timeEntry.OvertimeHours
        timeEntry.OvertimeHours = 0

        timeEntry.UndertimeHours = 0
        timeEntry.LateHours = 0
    End Sub

    Private Sub ComputeAbsentHours(timeEntry As TimeEntry,
                                   payrate As PayRate,
                                   hasWorkedLastDay As Boolean,
                                   currentshift As CurrentShift,
                                   leaves As IList(Of Leave))
        Dim isCalculatingRegularHoliday = payrate.IsRegularHoliday And _employee.CalcHoliday
        Dim isCalculatingSpecialHoliday = payrate.IsSpecialNonWorkingHoliday And _employee.CalcSpecialHoliday

        Dim isExemptDueToHoliday =
            (payrate.IsHoliday And (Not _policy.RequiredToWorkLastDay Or hasWorkedLastDay)) And
            (isCalculatingRegularHoliday Or isCalculatingSpecialHoliday Or Not _policy.AbsencesOnHoliday)

        If (Not currentshift.HasShift) Or (timeEntry.RegularHours > 0) Or isExemptDueToHoliday Or currentshift.IsRestDay Or leaves.Any() Then
            timeEntry.AbsentHours = 0
        Else
            timeEntry.AbsentHours = currentshift.WorkingHours
        End If
    End Sub

    Private Sub ComputeLeaveHours(hasTimeLog As Boolean, leaves As IList(Of Leave), currentShift As CurrentShift, timeEntry As TimeEntry)
        Dim calculator = New TimeEntryCalculator()

        If Not hasTimeLog And leaves.Any() Then
            Dim leave = leaves.FirstOrDefault()
            Dim leaveHours = 0D
            If leave.StartTime Is Nothing And leave.EndTime Is Nothing And Not currentShift.HasShift Then
                leaveHours = 8
            Else
                Dim leavePeriod = GetLeavePeriod(leave, currentShift)
                leaveHours = calculator.ComputeLeaveHours(leavePeriod, currentShift)
            End If

            timeEntry.SetLeaveHours(leave.LeaveType, leaveHours)
        End If

        Dim hasNoTimeLog = Not hasTimeLog

        If leaves.Any() Then
            If hasNoTimeLog Then
                timeEntry.UndertimeHours = 0
                Return
            End If

            Dim requiredHours = currentShift.WorkingHours
            Dim missingHours = requiredHours - (timeEntry.TotalLeaveHours + timeEntry.RegularHours)

            Dim payRate = _payrateCalendar.Find(currentShift.Date)

            If missingHours > 0 _
                And Not payRate.IsHoliday Then

                timeEntry.UndertimeHours += missingHours
            End If
        End If
    End Sub

    Private Function GetDailyRate(salary As Salary) As Decimal
        Dim dailyRate = 0D

        If salary Is Nothing Then
            Return 0
        End If

        If _employee.IsDaily Then
            dailyRate = salary.BasicSalary
        ElseIf _employee.IsMonthly Or _employee.IsFixed Then
            dailyRate = salary.BasicSalary / (_employee.WorkDaysPerYear / 12)
        End If

        Return dailyRate
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

            Dim isDefaultDayOff = _employee.DayOfRest = CInt(lastTimeEntry?.Date.DayOfWeek) + 1
            Dim isShiftRestDay = If(lastTimeEntry?.ShiftSchedule.IsRestDay, False)

            If isShiftRestDay Xor isDefaultDayOff Then

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

            Dim isAbsent = lastTimeEntry.AbsentDeduction > 0

            Return lastTimeEntry.RegularHours > 0 Or lastTimeEntry.TotalLeaveHours > 0
        Next

        Return False
    End Function

    Public Function GetLogPeriod(timeLog As TimeLog, officialBusiness As OfficialBusiness, currentShift As CurrentShift, currentDate As Date) As TimePeriod
        Dim appliedIn = {timeLog?.TimeIn, officialBusiness?.StartTime}.
            Where(Function(i) i.HasValue).
            Min()

        Dim appliedOut = {timeLog?.TimeOut, officialBusiness?.EndTime}.
            Where(Function(i) i.HasValue).
            Max()

        If Not appliedIn.HasValue Or Not appliedOut.HasValue Then
            Return Nothing
        End If
        Dim logPeriod = TimePeriod.FromTime(appliedIn.Value, appliedOut.Value, currentDate)

        If currentShift.HasShift Then
            Dim graceTime = _employee.LateGracePeriod

            Dim shiftStart = currentShift.ShiftPeriod.Start
            Dim gracePeriod = New TimePeriod(shiftStart, shiftStart.AddMinutes(graceTime))

            If gracePeriod.Contains(logPeriod.Start) Then
                logPeriod = TimePeriod.FromTime(shiftStart.TimeOfDay, appliedOut.Value, currentDate)
            End If
        End If

        Return logPeriod
    End Function

    Public Function GetLeavePeriod(leave As Leave, currentShift As CurrentShift) As TimePeriod
        Dim startTime = If(leave.StartTime, currentShift.Shift.TimeFrom)
        Dim endTime = If(leave.EndTime, currentShift.Shift.TimeTo)

        Dim leavePeriod = TimePeriod.FromTime(startTime, endTime, currentShift.Date)

        If Not currentShift.HasShift Then
            Return leavePeriod
        End If

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
