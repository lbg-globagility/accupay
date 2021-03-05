Option Strict On

Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Core
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Exceptions
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class DefaultShiftAndTimeLogsForm

    Private _currentPayPeriod As IPayPeriod

    Private ReadOnly DefaultStartTime As New TimeSpan(8, 0, 0)

    Private ReadOnly DefaultEndTime As New TimeSpan(17, 0, 0)

    Private ReadOnly DefaultShiftBreakTime As New TimeSpan(12, 0, 0)

    Private ReadOnly DefaultShiftBreakLength As Integer = 1

    Private ReadOnly _roleRepository As IRoleRepository

    Public Property NewPayPeriod As PayPeriod

    Sub New(currentPayPeriod As IPayPeriod)

        InitializeComponent()

        _currentPayPeriod = currentPayPeriod

        _roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)

        NewPayPeriod = Nothing

    End Sub

    Private Async Sub DefaultShiftAndTimeLogsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If _currentPayPeriod Is Nothing Then

            MessageBoxHelper.Warning("Invalid pay period.")
            Me.Close()

        End If

        If _currentPayPeriod?.RowID Is Nothing Then

            Dim dataService = MainServiceProvider.GetRequiredService(Of IPayPeriodDataService)

            NewPayPeriod = Await dataService.CreateAsync(
                organizationId:=z_OrganizationID,
                month:=_currentPayPeriod.Month,
                year:=_currentPayPeriod.Year,
                isFirstHalf:=_currentPayPeriod.IsFirstHalf,
                currentlyLoggedInUserId:=z_User
            )

            _currentPayPeriod = NewPayPeriod

        End If

        If _currentPayPeriod Is Nothing Then

            MessageBoxHelper.Warning("Invalid pay period.")
            Me.Close()

        End If

        EmployeeDataGrid.AutoGenerateColumns = False

        Dim currentDate = Date.Now.ToMinimumHourValue()
        DefaultStartTimePicker.Value = currentDate.AddSeconds(DefaultStartTime.TotalSeconds)
        DefaultEndTimePicker.Value = currentDate.AddSeconds(DefaultEndTime.TotalSeconds)
        DefaultBreakTimePicker.Value = currentDate.AddSeconds(DefaultShiftBreakTime.TotalSeconds)
        DefaultBreakLengthNumeric.Value = DefaultShiftBreakLength

        Me.Text &= $" ({GetPayPeriodDescription()})"

        USER_ROLE = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=z_OrganizationID)
        Dim shiftPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.SHIFT).FirstOrDefault()
        Dim timeLogPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.TIMELOG).FirstOrDefault()

        Dim deleteShiftPermission = shiftPermission IsNot Nothing AndAlso shiftPermission.Delete
        Dim deleteTimeLogPermission = timeLogPermission IsNot Nothing AndAlso timeLogPermission.Delete

        If Not deleteShiftPermission OrElse Not deleteTimeLogPermission Then

            DeleteButton.Visible = False

        End If

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

            Dim payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)
            Dim currentPayPeriod = Await payPeriodRepository.GetByIdAsync(_currentPayPeriod.RowID.Value)

            If currentPayPeriod.Status = Enums.PayPeriodStatus.Closed Then

                MessageBoxHelper.Warning("Data cannot be added/updated since it is within a ""Closed"" pay period.")

                Return
            End If

            GenerateDefaultShiftAndTimeLogs(selectedEmployees)
        End If

    End Sub

    Private Sub GenerateDefaultShiftAndTimeLogs(selectedEmployees As List(Of Employee))

        Const messageTitle As String = "Default Shift & Time Logs"
        Const defaultErrorMessage As String = "Something went wrong while creating default shift and time logs. Please contact Globagility Inc. for assistance."

        Dim defaultValue As New DefaultValue(
            DefaultStartTimePicker.Value.TimeOfDay,
            DefaultEndTimePicker.Value.TimeOfDay,
            DefaultBreakTimePicker.Value.TimeOfDay,
            DefaultBreakLengthNumeric.Value.Round())

        Dim generator As New DefaultShiftAndTimeLogsGeneration(selectedEmployees, _currentPayPeriod, defaultValue)
        Dim progressDialog = New ProgressDialog(generator, "Creating Default shift and time logs...")

        Me.Enabled = False
        progressDialog.Show()

        Dim generationTask = Task.Run(
                Async Function()
                    Await generator.Start()
                End Function
            )

        generationTask.ContinueWith(
            Sub() SaveGenerationOnSuccess(generator.Results, progressDialog),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        generationTask.ContinueWith(
            Sub(t As Task) GenerationOnError(t, progressDialog, messageTitle, defaultErrorMessage),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )

    End Sub

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

            Dim payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)
            Dim currentPayPeriod = Await payPeriodRepository.GetByIdAsync(_currentPayPeriod.RowID.Value)

            If currentPayPeriod.Status = Enums.PayPeriodStatus.Closed Then

                MessageBoxHelper.Warning("Data cannot be deleted since it is within a ""Closed"" pay period.")

                Return
            End If

            DeleteMultipleShiftAndTImelogs(messageTitle, selectedEmployees)
        End If

    End Sub

    Private Sub DeleteMultipleShiftAndTImelogs(messageTitle As String, selectedEmployees As List(Of Employee))
        Const defaultErrorMessage As String = "Something went wrong while deleting default shift and time logs. Please contact Globagility Inc. for assistance."

        Dim defaultValue As New DefaultValue(
            DefaultStartTimePicker.Value.TimeOfDay,
            DefaultEndTimePicker.Value.TimeOfDay,
            DefaultBreakTimePicker.Value.TimeOfDay,
            DefaultBreakLengthNumeric.Value.Round())

        Dim generator As New DeleteDefaultShiftAndTimeLogsGeneration(selectedEmployees, _currentPayPeriod)
        Dim progressDialog = New ProgressDialog(generator, "Deleting multiple shift and time logs...")

        Me.Enabled = False
        progressDialog.Show()

        Dim generationTask = Task.Run(
                Async Function()
                    Await generator.Start()
                End Function
            )

        generationTask.ContinueWith(
            Sub() DeleteGenerationOnSuccess(generator.Results, progressDialog),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        generationTask.ContinueWith(
            Sub(t As Task) GenerationOnError(t, progressDialog, messageTitle, defaultErrorMessage),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )

    End Sub

    Private Sub SaveGenerationOnSuccess(results As IReadOnlyCollection(Of ProgressGenerator.IResult), progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        Dim saveResults = results.Select(Function(r) CType(r, EmployeeResult)).ToList()

        Dim resultDialog = New EmployeeResultsDialog(
            saveResults,
            title:="Default Shift & Time Logs Results",
            generationDescription:="Default shift and time logs generation",
            entityDescription:="employees") With {
            .Owner = Me
        }

        resultDialog.ShowDialog()

        Me.Close()
    End Sub

    Private Sub DeleteGenerationOnSuccess(results As IReadOnlyCollection(Of ProgressGenerator.IResult), progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        Dim saveResults = results.Select(Function(r) CType(r, EmployeeResult)).ToList()

        Dim resultDialog = New EmployeeResultsDialog(
            saveResults,
            title:="Delete Multiple Shift & Time Logs Results",
            generationDescription:="Deleting multiple shift and time logs",
            entityDescription:="employees") With {
            .Owner = Me
        }

        resultDialog.ShowDialog()

        Me.Close()
    End Sub

    Private Sub GenerationOnError(t As Task, progressDialog As ProgressDialog, messageTitle As String, defaultErrorMessage As String)

        CloseProgressDialog(progressDialog)

        If t.Exception?.InnerException.GetType() Is GetType(BusinessLogicException) Then

            MessageBoxHelper.ErrorMessage(t.Exception?.InnerException.Message, messageTitle)
        Else
            Debugger.Break()
            MessageBoxHelper.ErrorMessage(defaultErrorMessage, messageTitle)
        End If

    End Sub

    Private Sub CloseProgressDialog(progressDialog As ProgressDialog)

        Me.Enabled = True

        If progressDialog Is Nothing Then Return

        progressDialog.Close()
        progressDialog.Dispose()
    End Sub

    Public Class DefaultValue

        Public ReadOnly Property StartTime As TimeSpan
        Public ReadOnly Property EndTime As TimeSpan
        Public ReadOnly Property ShiftBreakTime As TimeSpan
        Public ReadOnly Property ShiftBreakLength As Decimal

        Public Sub New(startTime As TimeSpan, endTime As TimeSpan, shiftBreakTime As TimeSpan, shiftBreakLength As Decimal)
            Me.StartTime = startTime
            Me.EndTime = endTime
            Me.ShiftBreakTime = shiftBreakTime
            Me.ShiftBreakLength = shiftBreakLength
        End Sub

    End Class

End Class
