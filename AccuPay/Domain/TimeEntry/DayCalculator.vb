Option Strict On

Imports AccuPay
Imports AccuPay.Entity
Imports AccuPay.Utils
Imports PayrollSys

Public Class DayCalculator
    Private Const DEFAULT_WORK_HOURS As Integer = 8
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
                            employeeShift As ShiftSchedule,
                            shiftSched As EmployeeDutySchedule,
                            timeLog As TimeLog,
                            overtimes As IList(Of Overtime),
                            officialBusiness As OfficialBusiness,
                            leaves As IList(Of Leave),
                            timeAttendanceLogs As IList(Of TimeAttendanceLog),
                            breakTimeBrackets As IList(Of BreakTimeBracket)) As TimeEntry

        Dim timeEntry = oldTimeEntries.Where(Function(t) t.Date = currentDate).SingleOrDefault()

        If timeEntry Is Nothing Then
            timeEntry = New TimeEntry() With {
                .EmployeeID = _employee.RowID,
                .OrganizationID = _organization.RowID,
                .Date = currentDate
            }
        End If

        timeEntry.Reset()

        Dim isBetweenSalaryDates As Boolean = False
        If salary IsNot Nothing Then
            isBetweenSalaryDates = currentDate.Date >= salary.EffectiveFrom AndAlso
                                    (salary.EffectiveTo Is Nothing OrElse currentDate.Date <= salary.EffectiveTo.Value)
        End If

        If Not isBetweenSalaryDates Then
            Return timeEntry
        End If

        Dim currentShift = GetCurrentShift(currentDate, employeeShift, shiftSched, _policy.UseShiftSchedule, _policy.RespectDefaultRestDay, _employee.DayOfRest)

        timeEntry.IsRestDay = currentShift.IsRestDay
        timeEntry.HasShift = currentShift.HasShift

        If timeEntry.HasShift Then
            timeEntry.WorkHours = currentShift.WorkingHours
            timeEntry.ShiftHours = currentShift.ShiftHours
        End If

        Dim hasWorkedLastDay = PayrollTools.HasWorkedLastWorkingDay(currentDate, oldTimeEntries, _payrateCalendar)
        Dim payrate = _payrateCalendar.Find(currentDate)

        ComputeHours(currentDate, timeEntry, timeLog, officialBusiness, leaves, overtimes, oldTimeEntries, timeAttendanceLogs, breakTimeBrackets, currentShift, hasWorkedLastDay)
        ComputePay(timeEntry, currentDate, currentShift, salary, payrate, hasWorkedLastDay)

        Return timeEntry
    End Function

    Public Shared Function GetCurrentShift(
                                currentDate As Date,
                                employeeShift As ShiftSchedule,
                                shiftSched As EmployeeDutySchedule,
                                useShiftSchedule As Boolean,
                                respectDefaultRestDay As Boolean,
                                employeeDayOfRest As Integer?) As CurrentShift

        Dim currentShift = If(useShiftSchedule,
                    New CurrentShift(shiftSched, currentDate),
                    New CurrentShift(employeeShift, currentDate))

        If respectDefaultRestDay Then
            currentShift.SetDefaultRestDay(employeeDayOfRest)
        End If

        Return currentShift
    End Function

    Private Sub ComputeHours(currentDate As Date,
                             timeEntry As TimeEntry,
                             timeLog As TimeLog,
                             officialBusiness As OfficialBusiness,
                             leaves As IList(Of Leave),
                             overtimes As IList(Of Overtime),
                             oldTimeEntries As IList(Of TimeEntry),
                             timeAttendanceLogs As IList(Of TimeAttendanceLog),
                             breakTimeBrackets As IList(Of BreakTimeBracket),
                             currentShift As CurrentShift,
                             hasWorkedLastDay As Boolean)
        Dim previousDay = currentDate.AddDays(-1)
        Dim calculator = New TimeEntryCalculator()

        timeEntry.EmployeeShiftID = currentShift.ShiftSchedule?.RowID

        Dim hasTimeLog = (timeLog?.TimeIn IsNot Nothing And timeLog?.TimeOut IsNot Nothing) Or
            officialBusiness IsNot Nothing
        Dim payrate = _payrateCalendar.Find(currentDate)

        Dim logPeriod As TimePeriod = Nothing
        If hasTimeLog Then
            logPeriod = GetLogPeriod(timeLog, officialBusiness, currentShift, currentDate)
        End If

        If _policy.PaidAsLongAsPresent Then
            Dim atLeastHasTimeLogs = timeLog?.TimeIn IsNot Nothing Or timeLog?.TimeOut IsNot Nothing
            If atLeastHasTimeLogs Then timeEntry.RegularHours = DEFAULT_WORK_HOURS
        End If

        If logPeriod IsNot Nothing Then

            Dim dutyPeriod As TimePeriod = Nothing

            Dim shiftPeriod = currentShift.ShiftPeriod

            If shiftPeriod IsNot Nothing Then
                dutyPeriod = shiftPeriod.Overlap(logPeriod)
            End If

            If dutyPeriod IsNot Nothing Then
                timeEntry.RegularHours = calculator.ComputeRegularHours(dutyPeriod, currentShift, _policy.ComputeBreakTimeLate)

                Dim coveredPeriod = dutyPeriod

                Dim leavePeriod As TimePeriod = Nothing
                If leaves.Any() Then
                    Dim leave = leaves.FirstOrDefault()
                    leavePeriod = GetLeavePeriod(leave, currentShift)

                    coveredPeriod = New TimePeriod(
                        {dutyPeriod.Start, leavePeriod.Start}.Min(),
                        {dutyPeriod.End, leavePeriod.End}.Max())

                    Dim leaveHours = TimeEntryCalculator.
                        ComputeLeaveHours(leavePeriod, currentShift, _policy.ComputeBreakTimeLate)

                    timeEntry.SetLeaveHours(leave.LeaveType, leaveHours)
                End If

                timeEntry.LateHours = calculator.ComputeLateHours(coveredPeriod, currentShift, _policy.ComputeBreakTimeLate)

                timeEntry.UndertimeHours = calculator.ComputeUndertimeHours(coveredPeriod, currentShift, _policy.ComputeBreakTimeLate)

                OverrideLateAndUndertimeHoursComputations(timeEntry, currentShift, dutyPeriod, leavePeriod, _policy)

                If _policy.ComputeBreakTimeLate Then
                    timeEntry.LateHours += calculator.ComputeBreakTimeLateHours(coveredPeriod, currentShift, timeAttendanceLogs, breakTimeBrackets)
                End If

                If _policy.LateHoursRoundingUp Then
                    Dim lateHours = timeEntry.LateHours

                    If lateHours > 0.5 And lateHours <= 1 Then
                        timeEntry.LateHours = 1
                    ElseIf lateHours >= 2 And lateHours <= 4 Then
                        timeEntry.LateHours = 4
                    End If
                End If

                timeEntry = LateSchemeSkipCountHours(timeEntry, currentShift, calculator, coveredPeriod)

                timeEntry.RegularHours = currentShift.WorkingHours - (timeEntry.LateHours + timeEntry.UndertimeHours)

                If leavePeriod IsNot Nothing Then
                    Dim coveredLeavePeriod = New TimePeriod(
                        {currentShift.Start, leavePeriod.Start}.Max(),
                        {currentShift.End, leavePeriod.End}.Min())

                    timeEntry.RegularHours -= calculator.ComputeRegularHours(coveredLeavePeriod, currentShift, _policy.ComputeBreakTimeLate)
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

            End If

            ComputeHolidayHours(payrate, timeEntry)
            ComputeRestDayHours(currentShift, timeEntry, logPeriod)

            timeEntry.BasicHours =
                timeEntry.RegularHours +
                timeEntry.RestDayHours +
                timeEntry.RegularHolidayHours +
                timeEntry.SpecialHolidayHours
        End If

        ComputeAbsentHours(timeEntry, payrate, hasWorkedLastDay, currentShift, leaves)
        ComputeLeaveHours(hasTimeLog, leaves, currentShift, timeEntry)
    End Sub

    Private Sub OverrideLateAndUndertimeHoursComputations(
                    timeEntry As TimeEntry,
                    currentShift As CurrentShift,
                    dutyPeriod As TimePeriod,
                    leavePeriod As TimePeriod,
                    policy As TimeEntryPolicy)

        Dim output = ComputeLateAndUndertimeHours(
                        currentShift.ShiftPeriod,
                        dutyPeriod,
                        leavePeriod,
                        currentShift.BreakPeriod,
                        policy.ComputeBreakTimeLate)

        If output.Item1 IsNot Nothing AndAlso output.Item2 IsNot Nothing Then

            timeEntry.LateHours = output.Item1.Value
            timeEntry.UndertimeHours = output.Item2.Value

        End If

    End Sub

    Public Shared Function ComputeLateAndUndertimeHours(
                                shiftPeriod As TimePeriod,
                                dutyPeriod As TimePeriod,
                                leavePeriod As TimePeriod,
                                breakPeriod As TimePeriod,
                                computeBreakTimeLatePolicy As Boolean) As (Decimal?, Decimal?)

        Dim lateHours, undertimeHours As Decimal

        Dim shiftAndDutyDifference As New List(Of TimePeriod)

        If shiftPeriod Is Nothing Then Return (Nothing, Nothing)

        If dutyPeriod IsNot Nothing Then

            shiftAndDutyDifference = shiftPeriod.Difference(dutyPeriod).ToList
        Else

            shiftAndDutyDifference.Add(shiftPeriod)

        End If

        Dim latePeriods = GetLateAndUndertimePeriods(shiftPeriod, dutyPeriod, leavePeriod, shiftAndDutyDifference)

        Dim latePeriod As TimePeriod = latePeriods.Item1
        Dim undertimeHoursAfterLeaveBeforeDutyTimePeriod As TimePeriod = latePeriods.Item2
        Dim undertimePeriod As TimePeriod = latePeriods.Item3

        Dim inBetweenUndertimeHours As Decimal = 0

        If undertimeHoursAfterLeaveBeforeDutyTimePeriod IsNot Nothing Then

            inBetweenUndertimeHours = ComputeHoursNotCoveredByLeave(breakPeriod, computeBreakTimeLatePolicy, undertimeHoursAfterLeaveBeforeDutyTimePeriod, leavePeriod)

        End If

        lateHours = ComputeHoursNotCoveredByLeave(breakPeriod, computeBreakTimeLatePolicy, latePeriod, leavePeriod)
        undertimeHours = ComputeHoursNotCoveredByLeave(breakPeriod, computeBreakTimeLatePolicy, undertimePeriod, leavePeriod)

        undertimeHours += inBetweenUndertimeHours

        Return (lateHours, undertimeHours)

    End Function

    Private Shared Function GetLateAndUndertimePeriods(
                        shiftPeriod As TimePeriod,
                        dutyPeriod As TimePeriod,
                        leavePeriod As TimePeriod,
                        shiftAndDutyDifference As List(Of TimePeriod)) As _
                        (TimePeriod, TimePeriod, TimePeriod)

        Dim undertimePeriod As TimePeriod = Nothing
        Dim latePeriod As TimePeriod = Nothing
        Dim inBetweenUndertimePeriod As TimePeriod = Nothing

        If shiftAndDutyDifference.Any() Then

            If shiftAndDutyDifference.Count = 2 Then

                latePeriod = shiftAndDutyDifference(0)
                undertimePeriod = shiftAndDutyDifference(1)

                If leavePeriod IsNot Nothing Then
                    'check if there is undertime after leave
                    'ex 9am-6pm shift / 10am-3pm leave / 4pm-5pm duty period / 9am-10am Late - 3pm-4pm UT & 5pm-6pm UT
                    Dim latePeriods = latePeriod.Difference(leavePeriod)
                    If latePeriods.Count = 2 Then

                        latePeriod = latePeriods(0)

                        'this is an in between undertime and will
                        'be added on to the end of the day undertime
                        inBetweenUndertimePeriod = latePeriods(1)
                    ElseIf latePeriods.Count = 1 Then

                        'check if the late is a late or undertime
                        If leavePeriod.Start <= shiftPeriod.Start OrElse
                        (dutyPeriod IsNot Nothing AndAlso dutyPeriod.Start <= shiftPeriod.Start) Then

                            latePeriod = Nothing
                            'this is an in between undertime and will
                            'be added on to the end of the day undertime
                            inBetweenUndertimePeriod = latePeriods(0)
                        Else

                            latePeriod = latePeriods(0)
                        End If

                    End If
                End If
            ElseIf shiftAndDutyDifference.Count = 1 Then

                If (leavePeriod IsNot Nothing AndAlso leavePeriod.Start <= shiftPeriod.Start) OrElse
                    (dutyPeriod IsNot Nothing AndAlso dutyPeriod.Start <= shiftPeriod.Start) Then

                    'ex. 9am-6pm shift / 9am-12pm leave
                    undertimePeriod = shiftAndDutyDifference(0)
                Else

                    'ex. 9am-6pm shift / 3pm-6pm leave
                    latePeriod = shiftAndDutyDifference(0)

                    If leavePeriod IsNot Nothing Then

                        'check if there is undertime after leave
                        'ex 9am-6pm shift / 3pm-5pm leave / 9am-3pm Late - 5pm-6pm Undertime
                        Dim latePeriods = latePeriod.Difference(leavePeriod)
                        If latePeriods.Count = 2 Then

                            latePeriod = latePeriods(0)

                            undertimePeriod = latePeriods(1)

                        End If

                    End If

                End If

            End If

        End If

        Return (latePeriod, inBetweenUndertimePeriod, undertimePeriod)

    End Function

    Private Shared Function ComputeHoursNotCoveredByLeave(
                                breakPeriod As TimePeriod,
                                computeBreakTimeLatePolicy As Boolean,
                                notCoveredByLeavePeriod As TimePeriod,
                                leavePeriod As TimePeriod) As Decimal

        Dim hours As Decimal = 0

        If notCoveredByLeavePeriod Is Nothing Then Return hours

        Dim notCoveredByLeavePeriods As New List(Of TimePeriod)

        If leavePeriod IsNot Nothing Then

            notCoveredByLeavePeriods = notCoveredByLeavePeriod.
                                        Difference(leavePeriod).
                                        ToList
        Else

            notCoveredByLeavePeriods.Add(notCoveredByLeavePeriod)
        End If

        If notCoveredByLeavePeriods.Any() Then

            If notCoveredByLeavePeriods.Count = 2 Then

                Dim periodBeforeLeave = notCoveredByLeavePeriods(0)
                Dim periodAfterLeave = notCoveredByLeavePeriods(1)

                hours = ComputeHoursWithBreaktime(breakPeriod, computeBreakTimeLatePolicy, periodBeforeLeave)
                hours += ComputeHoursWithBreaktime(breakPeriod, computeBreakTimeLatePolicy, periodAfterLeave)
            Else

                hours = ComputeHoursWithBreaktime(breakPeriod, computeBreakTimeLatePolicy, notCoveredByLeavePeriods(0))

            End If

        End If

        Return hours
    End Function

    Private Shared Function ComputeHoursWithBreaktime(breakPeriod As TimePeriod, computeBreakTimeLatePolicy As Boolean, hoursPeriod As TimePeriod) As Decimal
        Dim hours As Decimal

        If breakPeriod IsNot Nothing AndAlso computeBreakTimeLatePolicy = False Then
            Dim hoursWithoutBreaktimes = hoursPeriod.Difference(breakPeriod)

            hours = hoursWithoutBreaktimes.Sum(Function(l) l.TotalHours)
        Else

            hours = hoursPeriod.TotalHours
        End If

        Return hours
    End Function

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
            Dim lateMinutes = calculator.ComputeLateMinutes(coveredPeriod, currentShift, _policy.ComputeBreakTimeLate)

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

            timeEntry.Reset()

            Return
        End If

        Dim dailyRate = PayrollTools.GetDailyRate(salary, _employee)
        Dim hourlyRate = PayrollTools.GetHourlyRateByDailyRate(dailyRate)

        timeEntry.BasicDayPay = timeEntry.BasicHours * hourlyRate
        timeEntry.RegularPay = timeEntry.RegularHours * hourlyRate

        timeEntry.LateDeduction = timeEntry.LateHours * hourlyRate
        timeEntry.UndertimeDeduction = timeEntry.UndertimeHours * hourlyRate
        timeEntry.AbsentDeduction = timeEntry.AbsentHours * hourlyRate

        timeEntry.OvertimePay = timeEntry.OvertimeHours * hourlyRate * payrate.OvertimeRate

        Dim restDayRate = payrate.RestDayRate
        If _policy.RestDayInclusive AndAlso _employee.IsPremiumInclusive Then
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

            Dim isHolidayPayInclusive = _employee.IsPremiumInclusive

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

                Dim isPaidToday = timeEntry.GetTotalDayPay() > 0

                Dim isEntitledToHolidayPay =
                    (Not isHolidayPayInclusive) AndAlso
                    (hasWorkedLastDay OrElse
                        isPaidToday OrElse
                        (Not _policy.RequiredToWorkLastDayForHolidayPay))

                If isEntitledToHolidayPay Then
                    'timeEntry.RegularHolidayPay += basicHolidayPay
                    timeEntry.BasicRegularHolidayPay = basicHolidayPay
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
                calculator.ComputeNightDiffHours(dutyPeriod, currentShift, nightDiffPeriod, _policy.HasNightBreaktime, _policy.ComputeBreakTimeLate) +
                calculator.ComputeNightDiffHours(dutyPeriod, currentShift, dawnDiffPeriod, _policy.HasNightBreaktime, _policy.ComputeBreakTimeLate)

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
        'Dim isCalculatingRegularHoliday = payrate.IsRegularHoliday And _employee.CalcHoliday
        'Dim isCalculatingSpecialHoliday = payrate.IsSpecialNonWorkingHoliday And _employee.CalcSpecialHoliday

        'Dim isExemptDueToHoliday =
        '    (payrate.IsHoliday And (Not _policy.RequiredToWorkLastDay Or hasWorkedLastDay)) And
        '    (isCalculatingRegularHoliday Or isCalculatingSpecialHoliday Or Not _policy.AbsencesOnHoliday)
        If leaves.Any() Then
            Return
        End If

        If timeEntry.BasicHours > 0 Then
            Return
        End If

        If payrate.IsRegularDay And (currentshift.IsRestDay Or (Not currentshift.HasShift)) Then
            Return
        End If

        If IsExemptDueToHoliday(payrate, hasWorkedLastDay) Then
            Return
        End If

        timeEntry.AbsentHours = currentshift.WorkingHours
    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="payrate"></param>
    ''' <param name="hasWorkedLastDay"></param>
    ''' <returns>True means no absence, false means possibly absent.</returns>
    Public Function IsExemptDueToHoliday(payrate As PayRate, hasWorkedLastDay As Boolean) As Boolean
        ' If it's not a holiday, then employee is not exempt
        If Not payrate.IsHoliday Then
            Return False
        End If

        If _employee.IsDaily Then
            Return True
        End If

        If IsHolidayExempt(payrate) Then
            Return True
        End If

        If Not _policy.AbsencesOnHoliday Then
            Return True
        End If

        Return ((Not _policy.RequiredToWorkLastDay) Or hasWorkedLastDay)
    End Function

    Public Function IsHolidayExempt(payrate As PayRate) As Boolean
        Dim isCalculatingRegularHoliday = payrate.IsRegularHoliday And _employee.CalcHoliday
        Dim isCalculatingSpecialHoliday = payrate.IsSpecialNonWorkingHoliday And _employee.CalcSpecialHoliday

        Return Not (isCalculatingRegularHoliday Or isCalculatingSpecialHoliday)
    End Function

    ''' <summary>
    ''' Updates timeentry. This sets the leave hours only if there is no time logs. This will also increment the undertime hours if its is a working day (with timelogs or not).
    ''' </summary>
    ''' <param name="leaves">The list of leave for this day.</param>
    ''' <param name="currentShift">The shift for this day.</param>
    ''' <param name="timeEntry">The timeEntry object that will be updated.</param>
    Private Sub ComputeLeaveHours(hasTimeLog As Boolean, leaves As IList(Of Leave), currentShift As CurrentShift, timeEntry As TimeEntry)
        Dim calculator = New TimeEntryCalculator()

        If Not hasTimeLog And leaves.Any() Then
            Dim leave = leaves.FirstOrDefault()
            Dim leaveHours As Decimal = ComputeLeaveHoursWithoutTimelog(
                                            currentShift,
                                            leave,
                                            _policy.ComputeBreakTimeLate)

            timeEntry.SetLeaveHours(leave.LeaveType, leaveHours)
        End If

        If leaves.Any() AndAlso currentShift.IsWorkingDay Then

            Dim requiredHours = currentShift.WorkingHours
            Dim missingHours = requiredHours - (timeEntry.TotalLeaveHours +
                                                timeEntry.RegularHours +
                                                timeEntry.LateHours +
                                                timeEntry.UndertimeHours)

            Dim payRate = _payrateCalendar.Find(currentShift.Date)

            If missingHours > 0 _
                And Not payRate.IsHoliday Then

                timeEntry.UndertimeHours += missingHours
            End If
        End If
    End Sub

    Public Shared Function ComputeLeaveHoursWithoutTimelog(
                        currentShift As CurrentShift,
                        leave As Leave,
                        computeBreakTimeLate As Boolean) As Decimal

        Dim leaveHours = 0D
        If currentShift.HasShift = False AndAlso
            (leave.StartTime Is Nothing OrElse leave.EndTime Is Nothing) Then

            leaveHours = 8
        Else
            Dim leavePeriod = GetLeavePeriod(leave, currentShift)
            leaveHours = TimeEntryCalculator.
                ComputeLeaveHours(leavePeriod, currentShift, computeBreakTimeLate)
        End If

        leaveHours = AccuMath.CommercialRound(leaveHours, 2)
        Return leaveHours
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

    Private Shared Function GetLeavePeriod(leave As Leave, currentShift As CurrentShift) As TimePeriod
        Dim startTime = If(leave.StartTime, currentShift.StartTime.Value)
        Dim endTime = If(leave.EndTime, currentShift.EndTime.Value)

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