Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools

Public Class TimeAttendanceAnalyzer
    'TODO: ShiftSchedule
    Public Function Analyze(logs As IList(Of TimeAttendanceLog), employeeShifts As IList(Of ShiftSchedule)) As IList(Of TimeLog)
        Dim logGroups = logs.GroupBy(Function(l) l.EmployeeNo).ToList()

        Dim timeLogs = New List(Of TimeLog)

        For Each logGroup In logGroups
            Dim employeeNo = logGroup.Key
            Dim employeeLogs = logGroup.ToList()

            timeLogs.AddRange(
                GetEmployeeTimeLogs(employeeNo, employeeLogs, employeeShifts))
        Next

        Return timeLogs
    End Function

    Private Function GetEmployeeTimeLogs(employeeNo As String, logGroup As IList(Of TimeAttendanceLog), employeeShifts As IList(Of ShiftSchedule)) As IList(Of TimeLog)
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
                .LogDate = currentDate
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
            Return logIns.Min().TimeOfDay
        Else
            Return Nothing
        End If
    End Function

    Private Function GetTimeOut(currentDate As Date, currentShift As ShiftSchedule, nextShift As ShiftSchedule, sortedLogs As SortedSet(Of Date)) As TimeSpan?
        Dim timeOutBounds = GetShiftBoundsForTimeOut(currentDate, currentShift, nextShift)

        Dim logOuts = sortedLogs.
                Where(Function(l) timeOutBounds.Min <= l And l <= timeOutBounds.Max)

        If logOuts.Any() Then
            Return logOuts.Max().TimeOfDay
        Else
            Return Nothing
        End If
    End Function

    Private Function GetShiftBoundsForTimeIn(currentDate As Date, currentShift As ShiftSchedule) As (Min As Date, Max As Date)
        Dim shiftTime = currentShift.Shift
        Dim timeFromMinBound = shiftTime.TimeFrom.Add(TimeSpan.FromHours(-4))
        Dim timeFromMaxBound = shiftTime.TimeFrom.Add(TimeSpan.FromHours(4))

        Dim shiftMinBound = currentDate.Add(timeFromMinBound)
        Dim shiftMaxBound = currentDate.Add(timeFromMaxBound)

        Return (shiftMinBound, shiftMaxBound)
    End Function

    Private Function GetShiftBoundsForTimeOut(currentDate As Date, currentShift As ShiftSchedule, nextShift As ShiftSchedule) As (Min As Date, Max As Date)
        Dim shiftTime = currentShift.Shift
        Dim nextShiftTime = nextShift.Shift

        Dim minBoundTime = shiftTime.TimeTo.Add(TimeSpan.FromHours(-4))
        Dim maxBoundTime = nextShiftTime.TimeFrom.Add(TimeSpan.FromHours(-4))

        Dim shiftMinBound = currentDate.Add(minBoundTime)
        Dim shiftMaxBound = currentDate.AddDays(1).Add(maxBoundTime).AddSeconds(-1)

        Return (shiftMinBound, shiftMaxBound)
    End Function

End Class
