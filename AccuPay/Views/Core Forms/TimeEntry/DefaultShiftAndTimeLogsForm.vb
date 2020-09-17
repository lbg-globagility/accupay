Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class DefaultShiftAndTimeLogsForm

    Private ReadOnly _currentPayPeriod As IPayPeriod

    Private ReadOnly DefaultStartTime As New TimeSpan(8, 0, 0)

    Private ReadOnly DefaultEndTime As New TimeSpan(17, 0, 0)

    Private ReadOnly DefaultShiftBreakTime As New TimeSpan(12, 0, 0)

    Private ReadOnly DefaultShiftBreakLength As Integer = 1

    Sub New(currentPayPeriod As IPayPeriod)

        InitializeComponent()

        Me._currentPayPeriod = currentPayPeriod

    End Sub

    Private Sub DefaultShiftAndTimeLogsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        EmployeeDataGrid.AutoGenerateColumns = False

        Dim currentDate = Date.Now.ToMinimumHourValue()
        DefaultStartTimePicker.Value = currentDate.AddSeconds(DefaultStartTime.TotalSeconds)
        DefaultEndTimePicker.Value = currentDate.AddSeconds(DefaultEndTime.TotalSeconds)
        DefaultBreakTimePicker.Value = currentDate.AddSeconds(DefaultShiftBreakTime.TotalSeconds)
        DefaultBreakLengthNumeric.Value = DefaultShiftBreakLength

        Me.Text &= $" ({GetPayPeriodDescription()})"

    End Sub

    Private Function GetPayPeriodDescription() As String
        Return $"{_currentPayPeriod.PayFromDate.ToShortDateString()} - {_currentPayPeriod.PayToDate.ToShortDateString()}"
    End Function

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Sub EmployeeTreeView_TickedEmployee(s As Object, e As EventArgs) Handles EmployeeTreeView.EmployeeTicked

        RefreshDataGrid()

    End Sub

    Private Sub RefreshDataGrid()
        Dim selectedEmployees = EmployeeTreeView.GetTickedEmployees.ToList()

        EmployeeDataGrid.DataSource = selectedEmployees
        EmployeeDataGrid.Refresh()

        SaveButton.Enabled = selectedEmployees.Count > 0
        DeleteButton.Enabled = selectedEmployees.Count > 0

        SaveButton.Text = $"&Create ({selectedEmployees.Count()})"
        DeleteButton.Text = $"&Delete ({selectedEmployees.Count()})"
    End Sub

    Private Sub EmployeeDataGrid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles EmployeeDataGrid.DataError
        'this event is needed even if it is empty to override the handing of data error
        Console.WriteLine("EmployeeDataGrid_DataError")
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Const messageTitle As String = "Default Shift & Time Logs"

        If _currentPayPeriod Is Nothing Then
            MessageBoxHelper.Warning("Please select a pay period first.")
            Return
        End If

        Dim selectedEmployees = EmployeeTreeView.GetTickedEmployees.ToList()

        Dim employeeCount = selectedEmployees.Count

        If employeeCount = 0 Then Return

        If MessageBoxHelper.
            Confirm(Of Boolean)($"This will override the shift and time logs of { employeeCount } employee(s). Are you sure you want to create default data from {GetPayPeriodDescription()}?",
            messageTitle) Then

            Dim payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)
            Dim currentPayPeriod = Await payPeriodRepository.GetByIdAsync(_currentPayPeriod.RowID.Value)

            If currentPayPeriod.Status = Enums.PayPeriodStatus.Closed Then

                MessageBoxHelper.Warning("Data cannot be added/updated since it is within a ""Closed"" pay period.")

                Return
            End If

            GenerateDefaultShiftAndTimeLogs(messageTitle, selectedEmployees)
        End If

    End Sub

    Private Sub GenerateDefaultShiftAndTimeLogs(messageTitle As String, selectedEmployees As IReadOnlyCollection(Of Employee))

        Me.Cursor = Cursors.WaitCursor

        FunctionUtils.TryCatchFunction(messageTitle,
             Sub()

                 Dim shifts As New List(Of ShiftModel)
                 Dim timeLogs As New List(Of TimeLog)

                 For Each currentDate In Calendar.EachDay(_currentPayPeriod.PayFromDate, _currentPayPeriod.PayToDate)

                     For Each employee In selectedEmployees

                         If SkippableDate(currentDate, employee) = False Then

                             shifts.Add(CreateShift(currentDate, employee))
                             timeLogs.Add(CreateTimeLogs(currentDate, employee))

                         End If

                     Next
                 Next

                 'TODO: Add a result form to show if there are errors
                 Parallel.ForEach(selectedEmployees,
                    Async Sub(employee)

                        Dim shiftService = MainServiceProvider.GetRequiredService(Of EmployeeDutyScheduleDataService)
                        Dim timeLogService = MainServiceProvider.GetRequiredService(Of TimeLogDataService)

                        Dim employeeTimeLogs = timeLogs.Where(Function(t) t.EmployeeID.Value = employee.RowID.Value)
                        Await timeLogService.ChangeManyAsync(z_OrganizationID, added:=employeeTimeLogs.ToList())

                        Dim employeeShifts = shifts.Where(Function(s) s.EmployeeId.Value = employee.RowID.Value)
                        Await shiftService.BatchApply(employeeShifts, z_OrganizationID, z_User)

                    End Sub)

                 MessageBoxHelper.Information("Default shift and time logs were sucessfully created.", messageTitle)
                 Me.Close()
             End Sub
        )

        Me.Cursor = Cursors.Default

    End Sub

    Private Shared Function SkippableDate(currentDate As Date, employee As Employee) As Boolean
        Return employee.IsDaily = False AndAlso
            (
                currentDate.DayOfWeek = DayOfWeek.Saturday OrElse
                currentDate.DayOfWeek = DayOfWeek.Sunday)
    End Function

    Private Function CreateTimeLogs(currentDate As Date, employee As Employee) As TimeLog
        Return New TimeLog() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .EmployeeID = employee.RowID,
            .LogDate = currentDate,
            .TimeIn = DefaultStartTimePicker.Value.TimeOfDay,
            .TimeOut = DefaultEndTimePicker.Value.TimeOfDay,
            .BranchID = employee.BranchID
        }
    End Function

    Private Function CreateShift(currentDate As Date, employee As Employee) As ShiftModel

        Dim shift = New ShiftModel With {
            .EmployeeId = employee.RowID,
            .Date = currentDate,
            .StartTime = DefaultStartTimePicker.Value.TimeOfDay,
            .EndTime = DefaultEndTimePicker.Value.TimeOfDay,
            .BreakTime = DefaultBreakTimePicker.Value.TimeOfDay,
            .BreakLength = DefaultBreakLengthNumeric.Value.Round(),
            .IsRestDay = False
        }

        Return shift

    End Function

    Private Sub DefaultBreakLengthNumeric_Leave(sender As Object, e As EventArgs) Handles DefaultBreakLengthNumeric.Leave
        DefaultBreakLengthNumeric.Value = DefaultBreakLengthNumeric.Value.Round()
    End Sub

    Private Async Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

        Const messageTitle As String = "Delete Multiple Shift and Time Logs"

        If _currentPayPeriod Is Nothing Then
            MessageBoxHelper.Warning("Please select a pay period first.")
            Return
        End If

        Dim selectedEmployees = EmployeeTreeView.GetTickedEmployees.ToList()

        Dim employeeCount = selectedEmployees.Count

        If employeeCount = 0 Then Return

        If MessageBoxHelper.
            Confirm(Of Boolean)($"This will delete the shift and time logs of { employeeCount } employee(s). Are you sure you want to delete shift and time logs from {GetPayPeriodDescription()}?",
            messageTitle) Then

            Dim payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)
            Dim currentPayPeriod = Await payPeriodRepository.GetByIdAsync(_currentPayPeriod.RowID.Value)

            If currentPayPeriod.Status = Enums.PayPeriodStatus.Closed Then

                MessageBoxHelper.Warning("Data cannot be deleted since it is within a ""Closed"" pay period.")

                Return
            End If

            Await DeleteMultipleShiftAndTImelogs(messageTitle, selectedEmployees)
        End If

    End Sub

    Private Async Function DeleteMultipleShiftAndTImelogs(messageTitle As String, selectedEmployees As List(Of Employee)) As Task
        Dim shiftRepository = MainServiceProvider.GetRequiredService(Of EmployeeDutyScheduleRepository)
        Dim timeLogRepository = MainServiceProvider.GetRequiredService(Of TimeLogRepository)

        Dim employeeIds = selectedEmployees.Select(Function(x) x.RowID.Value).ToList()

        Dim coveredPeriod = New TimePeriod(_currentPayPeriod.PayFromDate, _currentPayPeriod.PayToDate)

        Dim shifts = Await shiftRepository.GetByMultipleEmployeeAndBetweenDatePeriodAsync(
            z_OrganizationID,
            employeeIds,
            coveredPeriod)

        Dim timeLogs = Await timeLogRepository.GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(
            employeeIds,
            coveredPeriod)

        Me.Cursor = Cursors.WaitCursor

        FunctionUtils.TryCatchFunction(messageTitle,
             Sub()

                 'TODO: Add a result form to show if there are errors
                 Parallel.ForEach(selectedEmployees,
                    Async Sub(employee)

                        Dim shiftService = MainServiceProvider.GetRequiredService(Of EmployeeDutyScheduleDataService)
                        Dim timeLogService = MainServiceProvider.GetRequiredService(Of TimeLogDataService)

                        Dim employeeTimeLogs = timeLogs.Where(Function(t) t.EmployeeID.Value = employee.RowID.Value)
                        Await timeLogService.ChangeManyAsync(z_OrganizationID, deleted:=employeeTimeLogs.ToList())

                        Dim employeeShifts = shifts.Where(Function(s) s.EmployeeID.Value = employee.RowID.Value)
                        Await shiftService.ChangeManyAsync(z_OrganizationID, deleted:=employeeShifts.ToList())

                    End Sub)

                 MessageBoxHelper.Information("Shift and time logs were sucessfully deleted.", messageTitle)
                 Me.Close()
             End Sub
        )

        Me.Cursor = Cursors.Default
    End Function

End Class