Option Strict On
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Helper.TimeLogsReader
Imports AccuPay.Tools

Public Class TimeAttendanceHelper
    Implements ITimeAttendanceHelper

    Private _importedTimeAttendanceLogs As New List(Of ImportTimeAttendanceLog)

    Private _employees As New List(Of Employee)

    Private _employeeShifts As New List(Of ShiftSchedule)

    Private _logsGroupedByEmployee As New List(Of IGrouping(Of String, ImportTimeAttendanceLog))

    Private Const HOURS_BUFFER As Decimal = 4

    Sub New(
           importedTimeLogs As List(Of ImportTimeAttendanceLog),
           employees As List(Of Employee),
           employeeShifts As List(Of ShiftSchedule))

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

                timeAttendanceLog.ShiftSchedule = dayLogRecord.ShiftSchedule

                index += 1

            Next

        Next

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
        employeeLogs As List(Of ImportTimeAttendanceLog),
        currentEmployeeShifts As List(Of ShiftSchedule),
        currentDate As Date) As DayLogRecord

        Dim currentShift = GetShift(currentEmployeeShifts, currentDate)

        Dim nextDate = currentDate.AddDays(1)
        Dim nextShift = GetShift(currentEmployeeShifts, nextDate)

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

    End Function

    Private Function GetShift(currentEmployeeShifts As List(Of ShiftSchedule), currentDate As Date) As ShiftSchedule
        Return currentEmployeeShifts.
                        Where(Function(s) s.EffectiveFrom <= currentDate And currentDate <= s.EffectiveTo).
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
        currentShift As ShiftSchedule) As Date

        Dim shiftMinBound As Date

        Dim shiftTimeFrom As Date

        If currentShift Is Nothing OrElse currentShift.Shift Is Nothing Then

            'If walang shift, minimum bound should be 12:00 AM
            shiftTimeFrom = currentDate.ToMinimumHourValue
            shiftMinBound = shiftTimeFrom

        Else
            'If merong shift, minimum bound should be shift.TimeFrom - 4 hours 
            '(ex. 9:00 AM - 5:00 PM shift -> 5:00 AM minimum bound)
            shiftTimeFrom = currentDate.Add(currentShift.Shift.TimeFrom)
            shiftMinBound = shiftTimeFrom.Add(TimeSpan.FromHours(-HOURS_BUFFER))

        End If

        Return shiftMinBound
    End Function

    Private Function GetShiftBoundsForTimeOut(
        currentDate As Date,
        currentShift As ShiftSchedule,
        nextShift As ShiftSchedule) As Date

        Dim shiftMaxBound As Date
        Dim maxBoundTime As TimeSpan

        If nextShift IsNot Nothing AndAlso nextShift.Shift IsNot Nothing Then
            'If merong next shift, maximum bound should be
            '(nextShift.TimeFrom - 4 hours - 1 second) + 1 day
            'ex. 9:00 AM - 5:00 PM -> (next day) 4:59 AM maximum bound
            maxBoundTime = nextShift.Shift.TimeFrom.
                                        Add(TimeSpan.FromHours(-HOURS_BUFFER)).
                                        Add(TimeSpan.FromSeconds(-1))

            shiftMaxBound = currentDate.AddDays(1).Add(maxBoundTime)

        ElseIf currentShift IsNot Nothing AndAlso currentShift.Shift IsNot Nothing Then

            Dim dateTimeOut = currentDate

            'check if shift is night shift by checking 
            'if TimeFrom is greater than TimeTo
            If currentShift.Shift.TimeFrom >= currentShift.Shift.TimeTo Then

                'if night shift, then dateTimeOut should be in the next day
                dateTimeOut = dateTimeOut.AddDays(1)

                'maxBoundTime should be shift TimeTo plus 4 hours
                '(ex. 9:00 PM - 5:00 AM -> (next day) 9:00 AM
                maxBoundTime = currentShift.Shift.TimeTo.Add(TimeSpan.FromHours(HOURS_BUFFER))

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

#End Region

    Private Class DayLogRecord

        Property LogDate As Date

        Property LogRecords As List(Of ImportTimeAttendanceLog)

        Property ShiftTimeInBounds As Date

        Property ShiftTimeOutBounds As Date

        Property ShiftSchedule As ShiftSchedule

        Public Overrides Function ToString() As String
            Return $"({LogDate.ToShortDateString}) {ShiftTimeInBounds.ToString("yyyy-MM-dd hh:mm tt")} - {ShiftTimeOutBounds.ToString("yyyy-MM-dd hh:mm tt")}"
        End Function

    End Class

End Class