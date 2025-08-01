Option Strict On

Imports System.Collections.Concurrent
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.ValueObjects
Imports Microsoft.Extensions.DependencyInjection

Public Class DeleteDefaultShiftAndTimeLogsGeneration
    Inherits ProgressGenerator

    Private ReadOnly _employees As IEnumerable(Of Employee)
    Private ReadOnly _currentPayPeriod As IPayPeriod

    Private _results As BlockingCollection(Of EmployeeResult)

    Public Sub New(employees As IEnumerable(Of Employee), currentPayPeriod As IPayPeriod)
        MyBase.New(employees.Where(Function(e) e IsNot Nothing).Count + 1) ' +1 for the resources loading

        _employees = employees.
            Where(Function(e) e IsNot Nothing).
            OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst)

        _currentPayPeriod = currentPayPeriod

        _results = New BlockingCollection(Of EmployeeResult)()
    End Sub

    Public Async Function Start() As Task

        SetCurrentMessage("Loading resources...")
        Dim result = Await GetEmployeeShiftAndTimeLogs()
        IncreaseProgress("Finished loading resources.")

        Dim shifts As List(Of Shift) = result.shifts
        Dim timeLogs As List(Of TimeLog) = result.timeLogs

        For Each employee In _employees

            _results.Add(Await DeleteDefaultShiftAndTimeLogs(shifts, timeLogs, employee))

        Next

        SetResults(_results.ToList())
    End Function

    Private Async Function GetEmployeeShiftAndTimeLogs() As Task(Of (shifts As List(Of Shift), timeLogs As List(Of TimeLog)))

        Dim shifts As New List(Of Shift)
        Dim timeLogs As New List(Of TimeLog)

        Dim shiftRepository = MainServiceProvider.GetRequiredService(Of IShiftRepository)
        Dim timeLogRepository = MainServiceProvider.GetRequiredService(Of ITimeLogRepository)

        Dim employeeIds = _employees.Select(Function(x) x.RowID.Value).ToList()

        Dim coveredPeriod = New TimePeriod(_currentPayPeriod.PayFromDate, _currentPayPeriod.PayToDate)

        shifts = (Await shiftRepository.GetByMultipleEmployeeAndBetweenDatePeriodAsync(
            z_OrganizationID,
            employeeIds,
            coveredPeriod)).
            ToList()

        timeLogs = (Await timeLogRepository.GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(
            employeeIds,
            coveredPeriod)).
            ToList()

        Return (shifts, timeLogs)
    End Function

    Private Async Function DeleteDefaultShiftAndTimeLogs(shifts As List(Of Shift), timeLogs As List(Of TimeLog), employee As Employee) As Task(Of EmployeeResult)

        Try

            Dim shiftService = MainServiceProvider.GetRequiredService(Of IShiftDataService)
            Dim timeLogService = MainServiceProvider.GetRequiredService(Of ITimeLogDataService)

            Dim employeeTimeLogs = timeLogs.Where(Function(t) t.EmployeeID.Value = employee.RowID.Value).ToList()

            Await timeLogService.SaveManyAsync(
                currentlyLoggedInUserId:=z_User,
                deleted:=employeeTimeLogs.ToList())

            Dim employeeShifts = shifts.Where(Function(s) s.EmployeeID.Value = employee.RowID.Value).ToList()

            Await shiftService.SaveManyAsync(
                currentlyLoggedInUserId:=z_User,
                deleted:=employeeShifts.ToList())

            SetCurrentMessage($"Finished deleting [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")

            Return EmployeeResult.Success(
                employeeNumber:=employee.EmployeeNo,
                employeeFullName:=employee.FullNameWithMiddleInitialLastNameFirst,
                employeeId:=employee.RowID.Value)
        Catch ex As Exception

            SetCurrentMessage($"Failure deleting [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")

            Return EmployeeResult.Error(
                employeeNumber:=employee.EmployeeNo,
                employeeFullName:=employee.FullNameWithMiddleInitialLastNameFirst,
                employeeId:=employee.RowID.Value,
                description:=$"Failed to delete shift and time logs for employee {employee.EmployeeNo} {ex.Message}")
        Finally

            IncreaseProgress()
        End Try

    End Function

End Class
