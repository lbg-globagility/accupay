Option Strict On

Imports AccuPay.Data
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Entity
Imports AccuPay.Helper.TimeLogsReader
Imports AccuPay.Tools
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions

Public Class TimeAttendanceHelperNew
    Implements ITimeAttendanceHelper

    Private Enum OvertimeType
        [Nothing]
        Pre
        Post
    End Enum

    Private _importedTimeAttendanceLogs As New List(Of ImportTimeAttendanceLog)

    Private ReadOnly _employees As New List(Of Entities.Employee)
    Private ReadOnly _employeeShifts As New List(Of EmployeeDutySchedule)

    Private ReadOnly _employeeOvertimes As List(Of Entities.Overtime)

    Private _logsGroupedByEmployee As New List(Of IGrouping(Of String, ImportTimeAttendanceLog))

    Private Const HOURS_BUFFER As Decimal = 4

    Sub New(
           importedTimeLogs As List(Of ImportTimeAttendanceLog),
           employees As List(Of Entities.Employee),
           employeeShifts As List(Of EmployeeDutySchedule),
           employeeOvertimes As List(Of Entities.Overtime))

        _importedTimeAttendanceLogs = importedTimeLogs
        _employees = employees
        _employeeShifts = employeeShifts
        _employeeOvertimes = employeeOvertimes

        _logsGroupedByEmployee = ImportTimeAttendanceLog.GroupByEmployee(_importedTimeAttendanceLogs)

        GetEmployeeObjectOfLogs()

    End Sub

    Public Function Analyze() As List(Of ImportTimeAttendanceLog) _
                    Implements ITimeAttendanceHelper.Analyze

        Dim dayLogRecords = GenerateDayLogRecords()

        For Each dayLogRecord In dayLogRecords

            Dim timeAttendanceLogs = dayLogRecord.LogRecords.
                                        Where(Function(d) d.IsTimeIn Is Nothing).
                                        OrderBy(Function(d) d.DateTime).
                                        ToList

            Dim index = 0
            Dim lastIndex = timeAttendanceLogs.Count - 1

            Dim isPreviouslyTimeIn = False

            For Each timeAttendanceLog In timeAttendanceLogs

                timeAttendanceLog.IsTimeIn = Nothing

                If index = 0 Then
                    timeAttendanceLog.IsTimeIn = True

                ElseIf index = lastIndex Then
                    timeAttendanceLog.IsTimeIn = False
                Else
                    timeAttendanceLog.IsTimeIn = Not isPreviouslyTimeIn
                End If

                isPreviouslyTimeIn = If(timeAttendanceLog.IsTimeIn, False)

                timeAttendanceLog.LogDate = dayLogRecord.LogDate

                timeAttendanceLog.ShiftTimeInBounds = dayLogRecord.ShiftTimeInBounds

                timeAttendanceLog.ShiftTimeOutBounds = dayLogRecord.ShiftTimeOutBounds

                timeAttendanceLog.EmployeeDutySchedule = dayLogRecord.ShiftSchedule

                index += 1

            Next

        Next

        Return _importedTimeAttendanceLogs
    End Function

    Public Function Validate() As List(Of ImportTimeAttendanceLog) _
        Implements ITimeAttendanceHelper.Validate

        '#1 Convert it back to DayLogRecords
        Dim dayLogRecords As New List(Of DayLogRecord)

        For Each logGroup In _logsGroupedByEmployee

            If logGroup.Count = 0 Then Continue For

            Dim employeeId = logGroup(0).Employee?.RowID

            If employeeId Is Nothing Then Continue For

            Dim employeeLogs = logGroup.ToList()

            Dim earliestLog = logGroup.FirstOrDefault().DateTime.ToMinimumHourValue
            Dim earliestLogDate = logGroup.FirstOrDefault().LogDate.ToMinimumHourValue
            Dim lastlog = logGroup.LastOrDefault().DateTime.ToMaximumHourValue
            Dim lastLogDate = logGroup.LastOrDefault().LogDate.ToMaximumHourValue

            Dim earliestDate = {earliestLog, earliestLogDate}.Min
            Dim lastDate = {lastlog, lastLogDate}.Max

            If earliestDate.HasValue = False OrElse lastDate.HasValue = False Then
                Continue For
            End If

            For Each currentDate In Calendar.EachDay(earliestDate.Value, lastDate.Value)

                Dim currentEmployeeLogs = employeeLogs.
                                            Where(Function(l) Nullable.Equals(l.LogDate, currentDate)).
                                            Where(Function(l) l.HasError = False).
                                            ToList

                If currentEmployeeLogs.Any() = False Then Continue For

                For Each log In currentEmployeeLogs

                    log.WarningMessage = Nothing

                Next

                dayLogRecords.Add(New DayLogRecord With {
                    .EmployeeId = employeeId.Value,
                    .LogDate = currentDate,
                    .LogRecords = currentEmployeeLogs.OrderBy(Function(l) l.DateTime).ToList
                })

            Next

        Next

        '#2 revalidate the logs. '[Check for succeeding logs that are both INs Or OUTs]
        ValidateLogs(dayLogRecords)

        Return _importedTimeAttendanceLogs
    End Function

    Public Function GenerateTimeLogs() As List(Of TimeLog) _
        Implements ITimeAttendanceHelper.GenerateTimeLogs

        Dim timeLogs As New List(Of TimeLog)

        For Each logGroups In _logsGroupedByEmployee

            Dim timeAttendanceLogOfEmployee = logGroups.ToList()

            If timeAttendanceLogOfEmployee.Count = 0 Then Continue For

            Dim currentEmployee = timeAttendanceLogOfEmployee(0).Employee

            If currentEmployee Is Nothing Then Continue For

            Dim logsByDayGroup = timeAttendanceLogOfEmployee.GroupBy(Function(l) l.LogDate).ToList()

            For Each logsByDay In logsByDayGroup

                Dim logsByDayList = logsByDay.OrderBy(Function(l) l.DateTime).ToList()

                If logsByDay.Key Is Nothing Then Continue For

                Dim logDate = CType(logsByDay.Key, Date)

                Dim firstTimeStampIn = logsByDayList.FirstOrDefault(Function(l) Nullable.Equals(l.IsTimeIn, True))?.DateTime
                Dim finalTimeStampOut = logsByDayList.LastOrDefault(Function(l) Nullable.Equals(l.IsTimeIn, False))?.DateTime

                Dim firstTimeIn, finalTimeOut As TimeSpan?

                'using timestampin with timeOfDay does not result to nothing
                If firstTimeStampIn Is Nothing Then
                    firstTimeIn = Nothing
                Else
                    firstTimeIn = firstTimeStampIn.Value.TimeOfDay
                End If
                If finalTimeStampOut Is Nothing Then
                    finalTimeOut = Nothing
                Else
                    finalTimeOut = finalTimeStampOut.Value.TimeOfDay
                End If

                timeLogs.Add(New TimeLog() With {
                    .LogDate = logDate,
                    .CreatedBy = z_User,
                    .OrganizationID = z_OrganizationID,
                    .EmployeeID = currentEmployee.RowID,
                    .TimeIn = firstTimeIn,
                    .TimeOut = finalTimeOut,
                    .TimeStampIn = firstTimeStampIn,
                    .TimeStampOut = finalTimeStampOut
                })

            Next

        Next

        Return timeLogs

    End Function

    Public Function GenerateTimeAttendanceLogs() As List(Of TimeAttendanceLog) _
        Implements ITimeAttendanceHelper.GenerateTimeAttendanceLogs

        Dim timeAttendanceLogs As New List(Of TimeAttendanceLog)

        For Each log In _importedTimeAttendanceLogs

            If log.LogDate Is Nothing Then Continue For

            Dim employeeId = log.Employee?.RowID

            If employeeId Is Nothing Then Continue For

            timeAttendanceLogs.Add(New TimeAttendanceLog() With {
                .CreatedBy = z_User,
                .OrganizationID = z_OrganizationID,
                .TimeStamp = log.DateTime,
                .IsTimeIn = log.IsTimeIn,
                .WorkDay = CType(log.LogDate, Date),
                .EmployeeID = Convert.ToInt32(employeeId)
            })

        Next

        Return timeAttendanceLogs

    End Function

#Region "Private Functions"

    Private Function GenerateDayLogRecords() As List(Of DayLogRecord)

        Dim dayLogRecords As New List(Of DayLogRecord)

        For Each logGroup In _logsGroupedByEmployee

            If logGroup.Count = 0 Then Continue For

            Dim employeeId = logGroup(0).Employee?.RowID

            If employeeId Is Nothing Then Continue For

            Dim currentEmployeeShifts = _employeeShifts.
                                        Where(Function(s) Nullable.Equals(s.EmployeeID, employeeId)).
                                        ToList

            Dim currentEmployeeOvertimes = _employeeOvertimes.
                                            Where(Function(o) Nullable.Equals(o.EmployeeID, employeeId)).
                                            ToList

            Dim employeeLogs = logGroup.ToList()

            Dim earliestDate = logGroup.FirstOrDefault().DateTime.Date.ToMinimumHourValue
            Dim lastDate = logGroup.LastOrDefault().DateTime.Date.ToMaximumHourValue

            Dim lastDayLogRecord As DayLogRecord = Nothing

            For Each currentDate In Calendar.EachDay(earliestDate, lastDate)

                Dim dayLogRecord = GenerateDayLogRecord(employeeId.Value,
                                                        employeeLogs,
                                                        currentEmployeeShifts,
                                                        currentEmployeeOvertimes,
                                                        currentDate,
                                                        lastDayLogRecord)

                If dayLogRecord IsNot Nothing Then

                    dayLogRecords.Add(dayLogRecord)
                    lastDayLogRecord = dayLogRecord
                Else
                    lastDayLogRecord = Nothing
                End If

            Next

        Next

        Return dayLogRecords

    End Function

    Private Function GenerateDayLogRecord(
        employeeId As Integer,
        employeeLogs As List(Of ImportTimeAttendanceLog),
        currentEmployeeShifts As List(Of EmployeeDutySchedule),
        currentEmployeeOvertimes As List(Of Entities.Overtime),
        currentDate As Date,
        lastDayLogRecord As DayLogRecord) As DayLogRecord

        Dim currentShift = GetShift(currentEmployeeShifts, currentDate)

        Dim nextDate = currentDate.AddDays(1)
        Dim nextShift = GetShift(currentEmployeeShifts, nextDate)

        Dim earliestOvertime = GetEarliestOvertime(
                                currentEmployeeOvertimes,
                                currentDate,
                                currentShift)

        Dim lastOvertime = GetLastOvertime(
                                currentEmployeeOvertimes,
                                currentDate,
                                currentShift)

        Dim earliestOvertimeNextDay = GetEarliestOvertime(
                                        currentEmployeeOvertimes,
                                        nextDate,
                                        nextShift)

        Dim timeInBounds = GetShiftBoundsForTimeIn(
                                lastDayLogRecord?.ShiftTimeOutBounds,
                                currentDate,
                                currentShift,
                                earliestOvertime)

        Dim timeOutBounds = GetShiftBoundsForTimeOut(
                                    currentDate,
                                    currentShift,
                                    lastOvertime,
                                    nextShift,
                                    earliestOvertimeNextDay)

        Dim dayBounds = New TimePeriod(timeInBounds, timeOutBounds)

        Return New DayLogRecord With {
            .EmployeeId = employeeId,
            .LogDate = currentDate,
            .LogRecords = GetTimeLogsFromBounds(dayBounds, employeeLogs, lastDayLogRecord),
            .ShiftTimeInBounds = dayBounds.Start,
            .ShiftTimeOutBounds = dayBounds.End,
            .ShiftSchedule = currentShift
        }

    End Function

    ''' <summary>
    ''' Check if the OT is really a pre OT or a .
    ''' </summary>
    ''' <param name="overtime"></param>
    ''' <param name="shift"></param>
    ''' <returns></returns>
    Private Function CheckOvertimeType(overtime As Entities.Overtime, shift As EmployeeDutySchedule) As OvertimeType

        'Sometimes the OT StartTime is less than the shift StartTime
        'but it can be a post OT.

        'FOR NIGHT SHIFT
        'for post shift
        'Example 1:   7PM-4AM shift / 4:30AM - 8:30AM OT
        'OT Is Post OT

        'For pre shift
        '(Example 2:   7PM-4AM shift / 2:30PM - 6:30PM OT
        'OT Is Pre OT)

        'FOR DAY SHIFT
        'for post shift
        'Example 1:   9AM-6PM shift / 6:30PM - 10:30PM OT
        'OT Is Post OT

        'For pre shift
        '(Example 2:  9AM-6PM shift / 4:30AM - 8:30AM OT
        'OT Is Pre OT)

        If shift Is Nothing OrElse shift.StartTime Is Nothing OrElse shift.EndTime Is Nothing OrElse
            overtime Is Nothing OrElse overtime.OTStartTime Is Nothing OrElse overtime.OTEndTime Is Nothing Then

            Return OvertimeType.Nothing

        End If

        'ex1: 08:30AM to 7PM = 11.5 hours
        'ex2: 10:30PM to 9AM = 10.5 hours (negative)
        Dim preOvertimeBreakSpan = ComputeHoursDifference(overtime.OTEndTime.Value, shift.StartTime.Value)

        'ex1: 4AM to 4:30AM = 0.5 hours
        'ex2: 6PM to 6:30PM = 0.5 hours
        Dim postOvertimeBreakSpan = ComputeHoursDifference(shift.EndTime.Value, overtime.OTStartTime.Value)

        'check where is the OT closest. Is it on the end shift time or start shift time
        If preOvertimeBreakSpan < postOvertimeBreakSpan Then

            Return OvertimeType.Pre
        Else
            'prioritize Post OT that is why it is the one on the else block
            Return OvertimeType.Post

        End If

    End Function

    Private Function ComputeHoursDifference(start As TimeSpan, [end] As TimeSpan) As Decimal

        'No difference
        If start = [end] Then Return 0

        Return TimePeriod.FromTime(start, [end], Date.Now.ToMinimumHourValue).TotalHours

    End Function

    Private Function GetShift(currentEmployeeShifts As List(Of EmployeeDutySchedule), currentDate As Date) As EmployeeDutySchedule
        Return currentEmployeeShifts.
                        Where(Function(s) s.DateSched = currentDate).
                        FirstOrDefault()
    End Function

    Private Function GetEarliestOvertime(
                        currentEmployeeOvertimes As List(Of Entities.Overtime),
                        currentDate As Date,
                        currentShift As EmployeeDutySchedule) As Entities.Overtime

        Return currentEmployeeOvertimes.
                Where(Function(o)

                          If Not IsTodaysOvertime(o, currentDate, currentShift) Then
                              Return False
                          End If

                          Return CheckOvertimeType(o, currentShift) = OvertimeType.Pre

                      End Function).
                OrderBy(Function(o) o.OTStartTime).
                FirstOrDefault()
    End Function

    Private Function GetLastOvertime(
                        currentEmployeeOvertimes As List(Of Entities.Overtime),
                        currentDate As Date,
                        currentShift As EmployeeDutySchedule) As Entities.Overtime

        Return currentEmployeeOvertimes.
                Where(Function(o)

                          If Not IsTodaysOvertime(o, currentDate, currentShift) Then
                              Return False
                          End If

                          Return CheckOvertimeType(o, currentShift) = OvertimeType.Post

                      End Function).
                OrderBy(Function(o) o.OTStartTime).
                LastOrDefault()
    End Function

    ''' <summary>
    ''' Checks if overtime and currentShift is not null then check if OT Startdate is equal to currentDate.
    ''' </summary>
    ''' <param name="overtime"></param>
    ''' <param name="currentDate"></param>
    ''' <param name="currentShift"></param>
    ''' <returns></returns>
    Private Function IsTodaysOvertime(
                        overtime As Entities.Overtime,
                        currentDate As Date,
                        currentShift As EmployeeDutySchedule) As Boolean

        If overtime Is Nothing OrElse
            currentShift Is Nothing OrElse
            overtime.OTStartDate <> currentDate OrElse
            overtime.OTStartTime Is Nothing OrElse
            overtime.OTEndTime Is Nothing OrElse
            currentShift.StartTime Is Nothing OrElse
            currentShift.EndTime Is Nothing Then

            Return False
        Else

            Return True
        End If

    End Function

    Private Function GetTimeLogsFromBounds(
                        shiftBounds As TimePeriod,
                        timeAttendanceLogs As List(Of ImportTimeAttendanceLog),
                        lastDayLogRecord As DayLogRecord
    ) As List(Of ImportTimeAttendanceLog)

        Dim logRecords = timeAttendanceLogs.
                            Where(Function(t) t.DateTime >= shiftBounds.Start).
                            Where(Function(t) t.DateTime <= shiftBounds.End).
                            ToList

        If lastDayLogRecord?.ShiftTimeOutBounds IsNot Nothing AndAlso
            lastDayLogRecord?.ShiftTimeOutBounds = shiftBounds.Start AndAlso
            lastDayLogRecord?.LogRecords IsNot Nothing Then

            'if this happens, this maybe caused by an employee that after previous overtime, [A1]
            'he continued his current shift right after
            'ex: previous day OT end = 9:00 AM | current day shift start = 9:00 AM

            'if there are multiple logs in the previous day log record
            'that are the same as the start shift bounds
            'ex: multiple logs of 9:00 AM, maybe caused by employee
            'logging out for previous OT then logging in again for current shift
            'we need to get at least one same log from previous day log record
            'And transfer it to current logs
            Dim shiftBoundsStartLogs = lastDayLogRecord.LogRecords.
                                            Where(Function(l) l.DateTime = shiftBounds.Start).
                                            ToList

            If shiftBoundsStartLogs.Count > 1 Then

                'get 1 same log from previous day log record
                Dim shiftBoundStartLog = shiftBoundsStartLogs(shiftBoundsStartLogs.Count - 1)

                logRecords.Add(shiftBoundStartLog)

                lastDayLogRecord?.LogRecords.Remove(shiftBoundStartLog)

            End If

        End If

        Return logRecords

    End Function

    Private Function GetShiftBoundsForTimeIn(
                        previousTimeOutBounds As Date?,
                        currentDate As Date,
                        currentShift As EmployeeDutySchedule,
                        earliestOvertime As Entities.Overtime) As Date

        Dim currentDayStartDateTime = GetStartDateTime(currentShift, earliestOvertime)

        If previousTimeOutBounds IsNot Nothing Then

            If currentDayStartDateTime IsNot Nothing AndAlso
                    currentDayStartDateTime = previousTimeOutBounds Then

                'if this happens, this maybe caused by an employee that after previous overtime, [A1]
                'he continued his current shift right after
                'ex: previous day OT end = 9:00 AM | current day shift start = 9:00 AM

                Return previousTimeOutBounds.Value
            End If

            Return previousTimeOutBounds.Value.AddSeconds(1)

        End If

        Dim shiftMinBound As Date

        If currentDayStartDateTime IsNot Nothing Then

            'If merong shift or OT, minimum bound should be earliest shift or OT - HOURS_BUFFER (4 hours)
            '(ex. 9:00 AM - 5:00 PM shift -> 5:00 AM minimum bound)
            shiftMinBound = currentDayStartDateTime.Value.Add(TimeSpan.FromHours(-HOURS_BUFFER))
        Else

            'If walang shift and OT, minimum bound should be 12:00 AM
            shiftMinBound = currentDate.ToMinimumHourValue

        End If

        Return shiftMinBound
    End Function

    Private Function GetShiftBoundsForTimeOut(
                        currentDate As Date,
                        currentShift As EmployeeDutySchedule,
                        lastOvertime As Entities.Overtime,
                        nextShift As EmployeeDutySchedule,
                        earliestOvertimeNextDay As Entities.Overtime) As Date

        Dim shiftMaxBound As Date

        Dim hoursBuffer As Decimal = HOURS_BUFFER

        Dim currentDayEndTime As Date? = GetEndDateTime(currentShift, lastOvertime)

        If nextShift Is Nothing Then

            'if there is no next shift, disregard the next day overtime
            'scenario: should be next shift is (6am-3pm), OT is (3pm-4pm)
            'if no shift, the nextDayStartDateTime would be 3pm which would be wrong
            earliestOvertimeNextDay = Nothing
        End If

        Dim nextDayStartDateTime As Date? = GetStartDateTime(nextShift, earliestOvertimeNextDay)

        If currentDayEndTime Is Nothing AndAlso nextDayStartDateTime Is Nothing Then
            'no current day shift or overtime
            'and no next day shift or overtime
            '= maximum bound should be 11:59 PM
            shiftMaxBound = currentDate.ToMaximumHourValue

        ElseIf currentDayEndTime IsNot Nothing AndAlso nextDayStartDateTime IsNot Nothing Then
            'has current day shift or overtime
            'and has next day shift or overtime

            If currentDayEndTime > nextDayStartDateTime Then

                'this may happen when there is input error
                'if current shift or late overtime end time is greater than
                'next day shift or early overtime start time

                shiftMaxBound = nextDayStartDateTime.Value.
                                Add(TimeSpan.FromHours(-hoursBuffer)).
                                Add(TimeSpan.FromSeconds(-1))

            ElseIf currentDayEndTime = nextDayStartDateTime Then

                'if this happens, this maybe caused by an employee that after overtime, [A1]
                'he continued his next shift right after
                'ex: current OT end = 9:00 AM | next day shift start = 9:00 AM

                shiftMaxBound = nextDayStartDateTime.Value
            Else
                'maximum bound should be halfway of the
                'current day end time and next day start date
                'ex 1: current day end time = 3:00 AM, next day start date = 9:00 AM (difference is 6 hours)
                '= maximum bound should be 5:59 AM (next day -3 hours -1 second)
                'if difference is greater HOURS_BUFFER * 2, use the default HOURS_BUFFER
                'ex 2: current day end time = 10:00 PM, next day start date = 9:00 AM (difference is 11 hours)
                '= maximum bound should be 4:59 AM (next day -4 hours -1 second)

                Dim restPeriodHours = New TimePeriod(currentDayEndTime.Value, nextDayStartDateTime.Value).TotalHours

                If restPeriodHours < (HOURS_BUFFER * 2) Then

                    hoursBuffer = AccuMath.CommercialRound(restPeriodHours / 2)

                End If

                shiftMaxBound = nextDayStartDateTime.Value.
                                Add(TimeSpan.FromHours(-hoursBuffer)).
                                Add(TimeSpan.FromSeconds(-1))

            End If

        ElseIf currentDayEndTime IsNot Nothing Then
            'currentDayEndTime IsNot Nothing AndAlso nextDayStartDateTime Is Nothing

            shiftMaxBound = currentDayEndTime.ToMaximumHourValue.Value

            ''''OLD CODE BELOW
            'if no next day shift or over time but has
            'current shift or overtime, set max bound time = currentDayEndTime + HOURS_BUFFER
            'ex: end time is 6:00 PM - max bound time = 10:00 PM
            'ex: end time is 4:00 PM - max bound time = 10:00 PM
            'shiftMaxBound = currentDayEndTime.Value.Add(TimeSpan.FromHours(HOURS_BUFFER))
        Else
            'nextDayStartDateTime IsNot Nothing AndAlso currentDayEndTime Is Nothing

            'if no next day shift or over time but has
            'current shift or overtime, set max bound time = currentDayEndTime - HOURS_BUFFER - 1 second
            'ex: next day start time is 9:00 AM - max bound time = 4:59 AM
            'ex: next day start end time is 5:00 AM - max bound time = 12:59 AM
            shiftMaxBound = nextDayStartDateTime.Value.
                                Add(TimeSpan.FromHours(-HOURS_BUFFER)).
                                Add(TimeSpan.FromSeconds(-1))
        End If

        Return shiftMaxBound
    End Function

    Private Function GetStartDateTime(currentShift As EmployeeDutySchedule, earliestOvertime As Entities.Overtime) As Date?
        Dim currentShiftStartTime = CreateDateTime(
                                        currentShift?.DateSched,
                                        currentShift?.StartTime)

        Dim earliestOvertimeStartTime = CreateDateTime(
                                            earliestOvertime?.OTStartDate,
                                            earliestOvertime?.OTStartTime)

        Dim startDateTime As Date? = CompareShiftToOvertime(
                                        currentShiftStartTime,
                                        earliestOvertimeStartTime,
                                        getEarliest:=True)
        Return startDateTime
    End Function

    Private Function GetEndDateTime(currentShift As EmployeeDutySchedule, lastOvertime As Entities.Overtime) As Date?
        Dim currentShiftEndTime = CreateDateTime(
                                            currentShift?.DateSched,
                                            currentShift?.EndTime,
                                            currentShift?.StartTime)
        Dim lastOvertimeEndTime As Date? = GetLastOvertime(currentShift, lastOvertime)

        Dim endTime As Date? = CompareShiftToOvertime(
                                        currentShiftEndTime,
                                        lastOvertimeEndTime,
                                        getEarliest:=False)
        Return endTime
    End Function

    Private Function GetLastOvertime(currentShift As EmployeeDutySchedule, lastOvertime As Entities.Overtime) As Date?
        Dim lastOvertimeEndTime = CreateDateTime(
                                        lastOvertime?.OTStartDate,
                                        lastOvertime?.OTEndTime,
                                        lastOvertime?.OTStartTime)

        'if OT start time < shift start time,
        'the OT is most likely a night shift Post OT
        If currentShift?.StartTime IsNot Nothing AndAlso
            lastOvertime?.OTStartTime IsNot Nothing AndAlso
            lastOvertimeEndTime IsNot Nothing AndAlso
            lastOvertime?.OTStartTime < currentShift?.StartTime Then

            lastOvertimeEndTime = lastOvertimeEndTime.Value.AddDays(1)

        End If

        Return lastOvertimeEndTime
    End Function

    Private Function CompareShiftToOvertime(
                        shiftDateTime As Date?,
                        overtimeDateTime As Date?,
                        getEarliest As Boolean) As Date?

        If overtimeDateTime Is Nothing AndAlso shiftDateTime Is Nothing Then

            Return Nothing

        ElseIf overtimeDateTime IsNot Nothing AndAlso shiftDateTime IsNot Nothing Then

            If getEarliest Then

                Return {overtimeDateTime.Value, shiftDateTime.Value}.Min
            Else

                Return {overtimeDateTime.Value, shiftDateTime.Value}.Max

            End If

        ElseIf overtimeDateTime IsNot Nothing Then

            Return overtimeDateTime.Value
        Else
            Return shiftDateTime.Value

        End If

    End Function

    Private Function CreateDateTime(
                        [date] As Date?,
                        time As TimeSpan?,
                        Optional startTime As TimeSpan? = Nothing) As Date?

        If [date] Is Nothing OrElse time Is Nothing Then

            Return Nothing
        End If

        'for night shift, add 1 day
        If startTime IsNot Nothing AndAlso startTime >= time Then

            Return [date].Value.
                AddDays(1).
                ToMinimumHourValue.
                AddTicks(time.Value.Ticks)
        Else

            Return [date].Value.
                ToMinimumHourValue.
                AddTicks(time.Value.Ticks)

        End If

    End Function

    Private Sub GetEmployeeObjectOfLogs()

        For Each logGroup In _logsGroupedByEmployee
            Dim employee = _employees.FirstOrDefault(Function(e) e.EmployeeNo = logGroup.Key)

            For Each log In logGroup

                If log.HasError Then Continue For

                If employee Is Nothing Then
                    log.Employee = Nothing

                    log.ErrorMessage = "Employee not found in the database."
                Else
                    log.Employee = employee
                End If

            Next

        Next

    End Sub

    Private Sub ValidateLogs(dayLogRecords As List(Of DayLogRecord))

        Dim succeedingInOrOutLogsWarningMessage = "This day has succeeding logs that are both IN or OUT."

        For Each dayLogRecord In dayLogRecords

            'check if logs for the day was ok
            Dim logsWithTimeStatus = dayLogRecord.LogRecords.
                                        Where(Function(l) l.IsTimeIn IsNot Nothing).
                                        OrderBy(Function(l) l.DateTime).
                                        ToList

            Dim warningMessage As String = Nothing

            'if the total count for that day is even, it means it is ok
            'because it means its logs are alternating IN then OUT
            'if it is odd then there are succeeding logs that are either both IN or OUT
            If (logsWithTimeStatus.Count Mod 2 <> 0) Then

                warningMessage = succeedingInOrOutLogsWarningMessage
            Else
                'Check if there are succeeding logs that are either both IN or OUT
                'and it should start with Time In
                Dim isTimeIn = True

                For Each log In logsWithTimeStatus

                    If Nullable.Equals(log.IsTimeIn, isTimeIn) = False Then

                        warningMessage = succeedingInOrOutLogsWarningMessage

                        Exit For

                    End If

                    isTimeIn = Not isTimeIn

                Next
            End If

            If warningMessage IsNot Nothing Then

                For Each log In logsWithTimeStatus
                    log.WarningMessage = warningMessage
                Next

            End If

        Next

        'add warning message for logs that were not including in a "day".
        Dim logsWithNoDateOrTimeStatus = _importedTimeAttendanceLogs.
                                            Where(Function(l) l.LogDate Is Nothing).
                                            Where(Function(l) l.IsTimeIn Is Nothing).
                                            ToList
        For Each log In logsWithNoDateOrTimeStatus
            log.WarningMessage = "The analyzer were not able to figure out this log's date."
        Next
    End Sub

#End Region

    Private Class DayLogRecord

        Property EmployeeId As Integer
        Property LogDate As Date
        Property LogRecords As List(Of ImportTimeAttendanceLog)
        Property ShiftTimeInBounds As Date
        Property ShiftTimeOutBounds As Date
        Property ShiftSchedule As EmployeeDutySchedule

        Public Overrides Function ToString() As String
            Return $"({LogDate.ToShortDateString}) {ShiftTimeInBounds.ToString("yyyy-MM-dd hh:mm tt")} - {ShiftTimeOutBounds.ToString("yyyy-MM-dd hh:mm tt")}"
        End Function

    End Class

End Class