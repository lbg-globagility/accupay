Option Strict On
Imports AccuPay.Entity
Imports AccuPay.Tools

Public Class TimeAttendanceHelper

    Private _importedTimeAttendanceLogs As New List(Of Helper.TimeLogsReader.TimeAttendanceLog)

    Private _employees As New List(Of Employee)

    Private _employeeShifts As New List(Of ShiftSchedule)

    Private _logsGroupedByEmployee As New List(Of IGrouping(Of String, Helper.TimeLogsReader.TimeAttendanceLog))


    Sub New(
           importedTimeLogs As List(Of Helper.TimeLogsReader.TimeAttendanceLog),
           employees As List(Of Employee),
           employeeShifts As List(Of ShiftSchedule))

        _importedTimeAttendanceLogs = importedTimeLogs
        _employees = employees
        _employeeShifts = employeeShifts

        _logsGroupedByEmployee = Helper.TimeLogsReader.TimeAttendanceLog.GroupByEmployee(_importedTimeAttendanceLogs)

        GetEmployeeObjectOfLogs()

    End Sub

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

    Public Function Analyze() As List(Of Helper.TimeLogsReader.TimeAttendanceLog)

        Dim dayLogRecords = GenerateDayLogRecords()

        For Each dayLogRecord In dayLogRecords

            Dim timeAttendanceLogs = dayLogRecord.LogRecords.OrderBy(Function(d) d.DateTime).ToList

            Dim index = 0
            Dim lastIndex = timeAttendanceLogs.Count - 1

            For Each timeAttendanceLog In timeAttendanceLogs

                timeAttendanceLog.IsTimeIn = Nothing


                If index = 0 Then
                    timeAttendanceLog.IsTimeIn = True

                ElseIf index = lastIndex Then
                    timeAttendanceLog.IsTimeIn = False

                End If

                timeAttendanceLog.LogDate = dayLogRecord.LogDate

                timeAttendanceLog.ShiftTimeInBounds = dayLogRecord.ShiftTimeInBounds

                timeAttendanceLog.ShiftTimeOutBounds = dayLogRecord.ShiftTimeOutBounds

                timeAttendanceLog.ShiftSchedule = dayLogRecord.ShiftSchedule

                index += 1

            Next

        Next

        Return _importedTimeAttendanceLogs
    End Function

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

            Dim earliestDate = logGroup.FirstOrDefault().DateTime.Date
            Dim lastDate = logGroup.LastOrDefault().DateTime.Date

            For Each currentDate In Calendar.EachDay(earliestDate, lastDate)

                Dim dayLogRecord = GenerateDayLogRecord(employeeLogs, currentEmployeeShifts, currentDate)

                If dayLogRecord IsNot Nothing Then
                    dayLogRecords.Add(dayLogRecord)
                End If

            Next

        Next

        Return dayLogRecords

    End Function

    Private Function GenerateDayLogRecord(
        employeeLogs As List(Of Helper.TimeLogsReader.TimeAttendanceLog),
        currentEmployeeShifts As List(Of ShiftSchedule),
        currentDate As Date) As DayLogRecord

        Dim currentShift = GetCurrentShift(currentEmployeeShifts, currentDate)

        Dim nextDate = currentDate.AddDays(1)
        Dim nextShift = GetNextShift(currentEmployeeShifts, currentShift, nextDate)

        Dim timeInBounds = GetShiftBoundsForTimeIn(currentDate, currentShift)
        Dim timeOutBounds = GetShiftBoundsForTimeOut(currentDate, currentShift, nextShift)

        Dim dayBounds = New TimePeriod(timeInBounds, timeOutBounds)

        Return New DayLogRecord With {
            .LogDate = currentDate,
            .LogRecords = GetTimeLogsFromBounds(dayBounds, employeeLogs),
            .ShiftTimeInBounds = dayBounds.Start,
            .ShiftTimeOutBounds = dayBounds.End,
            .ShiftSchedule = currentShift
        }

        'Dim timeInLog = timeInLogs.OrderBy(Function(t) t.DateTime).FirstOrDefault
        'Dim timeOutLog = timeOutLogs.OrderByDescending(Function(t) t.DateTime).FirstOrDefault


        'Return New DayLog With {
        '    .LogDate = currentDate,
        '    .TimeInLog = timeInLog,
        '    .TimeOutLog = timeOutLog,
        '    .ShiftTimeIn = timeInBounds.Start,
        '    .ShiftTimeOut = timeOutBounds.End
        '}

    End Function

    Public Function GenerateTimeLogs() As List(Of TimeLog)

        Dim timeLogs As New List(Of TimeLog)

        For Each logGroups In _logsGroupedByEmployee

            Dim timeAttendanceLogOfEmployee = logGroups.ToList()

            If timeAttendanceLogOfEmployee.Count = 0 Then Continue For

            Dim currentEmployee = timeAttendanceLogOfEmployee(0).Employee

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

                Dim timeLog = New TimeLog() With {
                    .LogDate = logDate,
                    .OrganizationID = z_OrganizationID,
                    .EmployeeID = currentEmployee.RowID,
                    .TimeIn = firstTimeIn,
                    .TimeOut = finalTimeOut,
                    .TimeStampIn = firstTimeStampIn,
                    .TimeStampOut = finalTimeStampOut
                }

                timeLogs.Add(timeLog)

            Next

        Next

        Return timeLogs

    End Function

    Private Function GetNextShift(currentEmployeeShifts As List(Of ShiftSchedule), currentShift As ShiftSchedule, nextDate As Date) As ShiftSchedule
        Dim nextShift = currentEmployeeShifts.
        Where(Function(s) s.EffectiveFrom <= nextDate And nextDate <= s.EffectiveTo).
        FirstOrDefault()

        If nextShift Is Nothing Then
            nextShift = currentShift
        End If

        Return nextShift
    End Function

    Private Function GetCurrentShift(currentEmployeeShifts As List(Of ShiftSchedule), currentDate As Date) As ShiftSchedule
        Return currentEmployeeShifts.
                        Where(Function(s) s.EffectiveFrom <= currentDate And currentDate <= s.EffectiveTo).
                        FirstOrDefault()
    End Function

    Private Function GetTimeLogsFromBounds(
        shiftBounds As TimePeriod,
        timeAttendanceLogs As List(Of Helper.TimeLogsReader.TimeAttendanceLog)
    ) As List(Of Helper.TimeLogsReader.TimeAttendanceLog)


        Return timeAttendanceLogs.
            Where(Function(t)
                      Return t.DateTime >= shiftBounds.Start AndAlso
                            t.DateTime <= shiftBounds.End
                  End Function).
            ToList

    End Function

    Private Function GetShiftBoundsForTimeIn(
        currentDate As Date,
        currentShift As ShiftSchedule) As Date

        Dim shiftMinBound As Date

        Dim shiftTimeFrom As Date

        If currentShift Is Nothing OrElse currentShift.Shift Is Nothing Then

            shiftTimeFrom = New DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0)
            shiftMinBound = shiftTimeFrom

        Else
            shiftTimeFrom = currentDate.Add(currentShift.Shift.TimeFrom)
            shiftMinBound = shiftTimeFrom.Add(TimeSpan.FromHours(-4))

        End If

        Return shiftMinBound
    End Function

    Private Function GetShiftBoundsForTimeOut(
        currentDate As Date,
        currentShift As ShiftSchedule,
        nextShift As ShiftSchedule) As Date

        Dim shiftMinBound As Date
        Dim shiftMaxBound As Date

        Dim shiftTimeTo As Date

        If currentShift Is Nothing OrElse currentShift.Shift Is Nothing Then
            shiftTimeTo = New DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59)
            shiftMaxBound = shiftTimeTo

        Else
            shiftTimeTo = currentDate.Add(currentShift.Shift.TimeTo)
            shiftMinBound = shiftTimeTo.Add(TimeSpan.FromHours(-4))

            '(nextShift Is Nothing) is already handled by the caller of the caller
            If nextShift.Shift Is Nothing Then
                shiftMaxBound = shiftMinBound.AddDays(1)

            Else
                Dim maxBoundTime = nextShift.Shift.TimeFrom.Add(TimeSpan.FromHours(-4))
                shiftMaxBound = currentDate.AddDays(1).Add(maxBoundTime).AddSeconds(-1)
            End If
        End If

        Return shiftMaxBound
    End Function


    Private Class DayLogRecord

        Property LogDate As Date

        Property LogRecords As List(Of Helper.TimeLogsReader.TimeAttendanceLog)

        Property ShiftTimeInBounds As Date

        Property ShiftTimeOutBounds As Date

        Property ShiftSchedule As ShiftSchedule

    End Class

End Class
