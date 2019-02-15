Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools

Public Class TimeAttendanceAnalyzer
    'TODO: ShiftSchedule
    Public Function GetLogsGroupByEmployee(
        logs As IList(Of TimeAttendanceLog)
    ) As List(Of IGrouping(Of String, TimeAttendanceLog))

        Return logs.GroupBy(Function(l) l.EmployeeNo).ToList()
    End Function

    Public Function Analyze(
        employees As List(Of Employee),
        logGroups As List(Of IGrouping(Of String, TimeAttendanceLog)),
        employeeShifts As IList(Of ShiftSchedule)
    ) As IList(Of TimeLog)

        Dim timeLogs = New List(Of TimeLog)

        For Each logGroup In logGroups
            Dim employee = employees.FirstOrDefault(Function(e) e.EmployeeNo = logGroup.Key)

            If employee Is Nothing Then
                'Maybe add here an error count and error reason/details
                Continue For
            End If

            Dim employeeLogs = logGroup.ToList()

            Dim currentEmployeeShifts = employeeShifts.
                                        Where(Function(s) s.EmployeeID.Value = employee.RowID.Value).
                                        ToList

            timeLogs.AddRange(
                GetEmployeeTimeLogs(employee, employeeLogs, currentEmployeeShifts))
        Next

        Return timeLogs
    End Function

    Private Function GetEmployeeTimeLogs(employee As Employee, logGroup As IList(Of TimeAttendanceLog), employeeShifts As IList(Of ShiftSchedule)) As IList(Of TimeLog)
        Dim earliestDate = logGroup.FirstOrDefault().DateTime.Date
        Dim lastDate = logGroup.LastOrDefault().DateTime.Date

        Dim sortedLogs = New SortedSet(Of Date)(logGroup.Select(Function(l) l.DateTime))

        Dim timeLogs = New List(Of TimeLog)

        For Each currentDate In Calendar.EachDay(earliestDate, lastDate)
            Dim currentShift = employeeShifts.
                Where(Function(s) s.EffectiveFrom <= currentDate And currentDate <= s.EffectiveTo).
                FirstOrDefault()

            Dim nextDate = currentDate.AddDays(1)

            Dim nextShift = employeeShifts.
                Where(Function(s) s.EffectiveFrom <= nextDate And nextDate <= s.EffectiveTo).
                FirstOrDefault()

            If nextShift Is Nothing Then
                nextShift = currentShift
            End If

            Dim timeLog = New TimeLog() With {
                .LogDate = currentDate,
                .OrganizationID = z_OrganizationID,
                .EmployeeID = employee.RowID
            }

            timeLog.TimeIn = GetTimeIn(currentDate, currentShift, sortedLogs)
            timeLog.TimeOut = GetTimeOut(currentDate, currentShift, nextShift, sortedLogs)

            timeLogs.Add(timeLog)
        Next

        Return timeLogs
    End Function

    Private Function GetTimeIn(currentDate As Date, currentShift As ShiftSchedule, sortedLogs As SortedSet(Of Date)) As TimeSpan?
        Dim timeInBounds = GetShiftBoundsForTimeIn(currentDate, currentShift)
        Dim logIns = sortedLogs.
            Where(Function(l) timeInBounds.Min <= l And l <= timeInBounds.Max)

        If logIns.Any() Then
            Dim timeIn = logIns.Min()
            sortedLogs.Remove(timeIn)
            Return timeIn.TimeOfDay
        Else
            Return Nothing
        End If
    End Function

    Private Function GetTimeOut(currentDate As Date, currentShift As ShiftSchedule, nextShift As ShiftSchedule, sortedLogs As SortedSet(Of Date)) As TimeSpan?
        Dim timeOutBounds = GetShiftBoundsForTimeOut(currentDate, currentShift, nextShift)

        Dim logOuts = sortedLogs.
                Where(Function(l) timeOutBounds.Min <= l And l <= timeOutBounds.Max)


        If logOuts.Any() Then
            Dim timeOut = logOuts.Max()
            sortedLogs.Remove(timeOut)
            Return timeOut.TimeOfDay
        Else
            Return Nothing
        End If
    End Function

    Private Function GetShiftBoundsForTimeIn(currentDate As Date, currentShift As ShiftSchedule) As (Min As Date, Max As Date)

        Dim shiftMinBound As Date
        Dim shiftMaxBound As Date

        If currentShift Is Nothing OrElse currentShift.Shift Is Nothing Then
            shiftMinBound = New DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0)
            shiftMaxBound = New DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59)

        Else
            Dim shiftTime = currentShift.Shift
            Dim timeFromMinBound = shiftTime.TimeFrom.Add(TimeSpan.FromHours(-4))
            Dim timeFromMaxBound = shiftTime.TimeFrom.Add(TimeSpan.FromHours(4))

            shiftMinBound = currentDate.Add(timeFromMinBound)
            shiftMaxBound = currentDate.Add(timeFromMaxBound)

        End If

        Return (shiftMinBound, shiftMaxBound)
    End Function

    Private Function GetShiftBoundsForTimeOut(currentDate As Date, currentShift As ShiftSchedule, nextShift As ShiftSchedule) As (Min As Date, Max As Date)
        'Nag eeror dito kapag walang shift ang employee
        'TODO: handle that error
        'Should we prompt 

        'if walang shift
        'min bound 12:00 am of current day
        'max bound 11:59 pm of current day

        Dim shiftMinBound As Date
        Dim shiftMaxBound As Date

        If currentShift Is Nothing OrElse currentShift.Shift Is Nothing Then
            shiftMinBound = New DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0)
            shiftMaxBound = New DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59)

        Else
            Dim shiftTime = currentShift.Shift
            Dim minBoundTime = shiftTime.TimeTo.Add(TimeSpan.FromHours(-4))
            shiftMinBound = currentDate.Add(minBoundTime)

            '(nextShift Is Nothing) is already handled by the caller of the caller
            If nextShift.Shift Is Nothing Then
                shiftMaxBound = shiftMinBound.AddDays(1)
            Else
                Dim nextShiftTime = nextShift.Shift
                Dim maxBoundTime = nextShiftTime.TimeFrom.Add(TimeSpan.FromHours(-4))
                shiftMaxBound = currentDate.AddDays(1).Add(maxBoundTime).AddSeconds(-1)
            End If
        End If

        Return (shiftMinBound, shiftMaxBound)
    End Function

End Class
