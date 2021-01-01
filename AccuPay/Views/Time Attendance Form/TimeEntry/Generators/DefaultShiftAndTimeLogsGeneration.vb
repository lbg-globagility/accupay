Option Strict On

Imports System.Collections.Concurrent
Imports System.Threading.Tasks
Imports AccuPay.Core
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Services
Imports AccuPay.DefaultShiftAndTimeLogsForm
Imports AccuPay.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class DefaultShiftAndTimeLogsGeneration
    Inherits ProgressGenerator

    Private ReadOnly _employees As IEnumerable(Of Employee)
    Private ReadOnly _currentPayPeriod As IPayPeriod
    Private ReadOnly _defaultValue As DefaultValue

    Private _results As BlockingCollection(Of EmployeeResult)

    Public Sub New(employees As IEnumerable(Of Employee), currentPayPeriod As IPayPeriod, defaultValue As DefaultValue)
        MyBase.New(employees.Where(Function(e) e IsNot Nothing).Count + 1) ' +1 for the resources loading

        _employees = employees.
            Where(Function(e) e IsNot Nothing).
            OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst)

        _currentPayPeriod = currentPayPeriod
        _defaultValue = defaultValue

        _results = New BlockingCollection(Of EmployeeResult)()
    End Sub

    Public Async Function Start() As Task

        SetCurrentMessage("Loading resources...")
        Dim resultData = CreateEmployeeShiftAndTimeLogs()
        IncreaseProgress("Finished loading resources.")

        Dim shifts As List(Of ShiftModel) = resultData.shifts
        Dim timeLogs As List(Of TimeLog) = resultData.timeLogs

        For Each employee In _employees

            _results.Add(Await SaveDefaultShiftAndTimeLogs(shifts, timeLogs, employee))

        Next

        SetResults(_results.ToList())
    End Function

    Private Function CreateEmployeeShiftAndTimeLogs() As (shifts As List(Of ShiftModel), timeLogs As List(Of TimeLog))

        Dim shifts As New List(Of ShiftModel)
        Dim timeLogs As New List(Of TimeLog)

        For Each currentDate In Calendar.EachDay(_currentPayPeriod.PayFromDate, _currentPayPeriod.PayToDate)

            For Each employee In _employees

                If SkippableDate(currentDate, employee) = False Then

                    shifts.Add(CreateShift(currentDate, employee))
                    timeLogs.Add(CreateTimeLogs(currentDate, employee))

                End If

            Next
        Next

        Return (shifts, timeLogs)
    End Function

    Private Async Function SaveDefaultShiftAndTimeLogs(shifts As List(Of ShiftModel), timeLogs As List(Of TimeLog), employee As Employee) As Task(Of EmployeeResult)

        Try

            Dim shiftService = MainServiceProvider.GetRequiredService(Of ShiftDataService)
            Dim timeLogService = MainServiceProvider.GetRequiredService(Of TimeLogDataService)

            Dim employeeTimeLogs = timeLogs.Where(Function(t) t.EmployeeID.Value = employee.RowID.Value)
            Await timeLogService.SaveManyAsync(
                currentlyLoggedInUserId:=z_User,
                added:=employeeTimeLogs.ToList())

            Dim employeeShifts = shifts.Where(Function(s) s.EmployeeId.Value = employee.RowID.Value)
            Await shiftService.BatchApply(employeeShifts, z_OrganizationID, z_User)

            SetCurrentMessage($"Finished generating [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")

            Return EmployeeResult.Success(
                employeeNumber:=employee.EmployeeNo,
                employeeFullName:=employee.FullNameWithMiddleInitialLastNameFirst,
                employeeId:=employee.RowID.Value)
        Catch ex As Exception

            SetCurrentMessage($"Failure generating [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")

            Return EmployeeResult.Error(
                employeeNumber:=employee.EmployeeNo,
                employeeFullName:=employee.FullNameWithMiddleInitialLastNameFirst,
                employeeId:=employee.RowID.Value,
                description:=$"Failed to generate shift and time logs for employee {employee.EmployeeNo} {ex.Message}")
        Finally

            IncreaseProgress()
        End Try

    End Function

    Private Shared Function SkippableDate(currentDate As Date, employee As Employee) As Boolean
        Return employee.IsDaily = False AndAlso
            (
                currentDate.DayOfWeek = DayOfWeek.Saturday OrElse
                currentDate.DayOfWeek = DayOfWeek.Sunday)
    End Function

    Private Function CreateTimeLogs(currentDate As Date, employee As Employee) As TimeLog
        Return New TimeLog() With {
            .OrganizationID = z_OrganizationID,
            .EmployeeID = employee.RowID,
            .LogDate = currentDate,
            .TimeIn = _defaultValue.StartTime,
            .TimeOut = _defaultValue.EndTime,
            .BranchID = employee.BranchID
        }
    End Function

    Private Function CreateShift(currentDate As Date, employee As Employee) As ShiftModel

        Dim shift = New ShiftModel With {
            .EmployeeId = employee.RowID,
            .Date = currentDate,
            .StartTime = _defaultValue.StartTime,
            .EndTime = _defaultValue.EndTime,
            .BreakTime = _defaultValue.ShiftBreakTime,
            .BreakLength = _defaultValue.ShiftBreakLength,
            .IsRestDay = False
        }

        Return shift

    End Function

End Class
