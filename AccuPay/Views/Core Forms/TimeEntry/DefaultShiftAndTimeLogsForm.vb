Option Strict On

Imports AccuPay.Data
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utilities
Imports AccuPay.Utils
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

        Me.Text = Me.Text & $" ({GetPayPeriodDescription()})"

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

        SaveButton.Text = $"&Save ({selectedEmployees.Count()})"
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

            Await GenerateDefaultShiftAndTimeLogs(messageTitle, selectedEmployees)
        End If

    End Sub

    Private Async Function GenerateDefaultShiftAndTimeLogs(messageTitle As String, selectedEmployees As IReadOnlyCollection(Of Employee)) As Threading.Tasks.Task

        Me.Cursor = Cursors.WaitCursor

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()

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

                Dim shiftService = MainServiceProvider.GetRequiredService(Of EmployeeDutyScheduleDataService)
                Dim timeLogService = MainServiceProvider.GetRequiredService(Of TimeLogDataService)

                Await timeLogService.ChangeManyAsync(addedTimeLogs:=timeLogs)
                Await shiftService.BatchApply(shifts, z_OrganizationID, z_User)

                MessageBoxHelper.Information("Default shift and time logs were sucessfully created.", messageTitle)
                Me.Close()
            End Function
        )

        Me.Cursor = Cursors.Default

    End Function

    Private Shared Function SkippableDate(currentDate As Date, employee As Employee) As Boolean
        Return employee.IsDaily = False AndAlso (
                            currentDate.DayOfWeek = DayOfWeek.Saturday OrElse
                            currentDate.DayOfWeek = DayOfWeek.Sunday)
    End Function

    Private Function CreateTimeLogs(currentDate As Date, employee As Employee) As TimeLog
        Return New TimeLog() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .EmployeeID = employee.RowID,
            .LogDate = currentDate,
            .TimeIn = DefaultStartTime,
            .TimeOut = DefaultEndTime
        }
    End Function

    Private Function CreateShift(currentDate As Date, employee As Employee) As ShiftModel
        Dim shift = New ShiftModel With {
            .EmployeeId = employee.RowID,
            .Date = currentDate,
            .StartTime = DefaultStartTime,
            .EndTime = DefaultEndTime,
            .BreakTime = DefaultShiftBreakTime,
            .BreakLength = DefaultShiftBreakLength,
            .IsRestDay = False
        }

        Return shift

    End Function

End Class