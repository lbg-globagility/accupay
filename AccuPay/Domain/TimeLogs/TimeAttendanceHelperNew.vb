Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Helper.TimeLogsReader
Imports AccuPay.Tools

Public Class TimeAttendanceHelperNew
    Implements ITimeAttendanceHelper

    Private _importedTimeAttendanceLogs As New List(Of ImportTimeAttendanceLog)

    Private _employees As New List(Of Employee)

    Private _employeeShifts As New List(Of EmployeeDutySchedule)

    Private _logsGroupedByEmployee As New List(Of IGrouping(Of String, ImportTimeAttendanceLog))

    Private Const HOURS_BUFFER As Decimal = 4

    Sub New(
           importedTimeLogs As List(Of ImportTimeAttendanceLog),
           employees As List(Of Employee),
           employeeShifts As List(Of EmployeeDutySchedule))

        _importedTimeAttendanceLogs = importedTimeLogs
        _employees = employees
        _employeeShifts = employeeShifts

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

        ValidateLogs(dayLogRecords)

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

            For Each currentDate In Calendar.EachDay(earliestDate, lastDate)

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

            Dim employeeLogs = logGroup.ToList()

            Dim earliestDate = logGroup.FirstOrDefault().DateTime.Date.ToMinimumHourValue
            Dim lastDate = logGroup.LastOrDefault().DateTime.Date.ToMaximumHourValue

            For Each currentDate In Calendar.EachDay(earliestDate, lastDate)

                Dim dayLogRecord = GenerateDayLogRecord(employeeId.Value,
                                                        employeeLogs,
                                                        currentEmployeeShifts,
                                                        currentDate)

                If dayLogRecord IsNot Nothing Then
                    dayLogRecords.Add(dayLogRecord)
                End If

            Next

        Next

        Return dayLogRecords

    End Function

    Private Function GenerateDayLogRecord(
        employeeId As Integer,
        employeeLogs As List(Of ImportTimeAttendanceLog),
        currentEmployeeShifts As List(Of EmployeeDutySchedule),
        currentDate As Date) As DayLogRecord

        Dim currentShift = GetShift(currentEmployeeShifts, currentDate)

        Dim nextDate = currentDate.AddDays(1)
        Dim nextShift = GetShift(currentEmployeeShifts, nextDate)

        Dim timeInBounds = GetShiftBoundsForTimeIn(currentDate, currentShift)
        Dim timeOutBounds = GetShiftBoundsForTimeOut(currentDate, currentShift, nextShift)

        Dim dayBounds = New TimePeriod(timeInBounds, timeOutBounds)

        Return New DayLogRecord With {
            .EmployeeId = employeeId,
            .LogDate = currentDate,
            .LogRecords = GetTimeLogsFromBounds(dayBounds, employeeLogs),
            .ShiftTimeInBounds = dayBounds.Start,
            .ShiftTimeOutBounds = dayBounds.End,
            .ShiftSchedule = currentShift
        }

    End Function

    Private Function GetShift(currentEmployeeShifts As List(Of EmployeeDutySchedule), currentDate As Date) As EmployeeDutySchedule
        Return currentEmployeeShifts.
                        Where(Function(s) s.DateSched = currentDate).
                        FirstOrDefault()
    End Function

    Private Function GetTimeLogsFromBounds(
        shiftBounds As TimePeriod,
        timeAttendanceLogs As List(Of ImportTimeAttendanceLog)
    ) As List(Of ImportTimeAttendanceLog)

        Return timeAttendanceLogs.
            Where(Function(t)
                      Return t.DateTime >= shiftBounds.Start AndAlso
                            t.DateTime <= shiftBounds.End
                  End Function).
            ToList

    End Function

    Private Function GetShiftBoundsForTimeIn(
        currentDate As Date,
        currentShift As EmployeeDutySchedule) As Date

        Dim shiftMinBound As Date

        Dim shiftTimeFrom As Date

        If currentShift Is Nothing OrElse currentShift.StartTime Is Nothing Then

            'If walang shift, minimum bound should be 12:00 AM
            shiftTimeFrom = currentDate.ToMinimumHourValue
            shiftMinBound = shiftTimeFrom
        Else
            'If merong shift, minimum bound should be shift.TimeFrom - 4 hours
            '(ex. 9:00 AM - 5:00 PM shift -> 5:00 AM minimum bound)
            shiftTimeFrom = currentDate.Add(CType(currentShift.StartTime, TimeSpan))
            shiftMinBound = shiftTimeFrom.Add(TimeSpan.FromHours(-HOURS_BUFFER))

        End If

        Return shiftMinBound
    End Function

    Private Function GetShiftBoundsForTimeOut(
        currentDate As Date,
        currentShift As EmployeeDutySchedule,
        nextShift As EmployeeDutySchedule) As Date

        Dim shiftMaxBound As Date
        Dim maxBoundTime As TimeSpan

        If nextShift IsNot Nothing AndAlso nextShift.StartTime IsNot Nothing Then
            'If merong next shift, maximum bound should be
            '(nextShift.TimeFrom - 4 hours - 1 second) + 1 day
            'ex. 9:00 AM - 5:00 PM -> (next day) 4:59 AM maximum bound
            maxBoundTime = nextShift.StartTime.Value.
                                        Add(TimeSpan.FromHours(-HOURS_BUFFER)).
                                        Add(TimeSpan.FromSeconds(-1))

            shiftMaxBound = nextShift.DateSched.Add(maxBoundTime)

        ElseIf currentShift IsNot Nothing AndAlso currentShift.EndTime IsNot Nothing Then

            Dim dateTimeOut = currentShift.DateSched

            'check if shift is night shift by checking
            'if StartTime is greater than EndTime
            If currentShift.StartTime IsNot Nothing AndAlso
                currentShift.StartTime >= currentShift.EndTime Then

                'if night shift, then dateTimeOut should be in the next day
                dateTimeOut = dateTimeOut.AddDays(1)

                'maxBoundTime should be shift EndTime plus 4 hours
                '(ex. 9:00 PM - 5:00 AM -> (next day) 9:00 AM
                maxBoundTime = currentShift.EndTime.Value.Add(TimeSpan.FromHours(HOURS_BUFFER))
            Else

                'if not night shift, then dateTimeOut should be same day
                'and make the maxBoundTime the maximum hours for that day
                '(ex. 9:00 AM - 5:00 PM -> 11:59 PM maximum bound)
                maxBoundTime = New TimeSpan(23, 59, 59)

            End If

            shiftMaxBound = dateTimeOut.Add(maxBoundTime)
        Else

            'If walang next shift and walang current end time shift
            'maximum bound should be 11:59 PM
            shiftMaxBound = currentDate.ToMaximumHourValue

        End If

        Return shiftMaxBound
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