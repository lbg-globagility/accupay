Option Strict On

Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Entities.LeaveReset
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Interfaces.Domain_Services
Imports AccuPay.Core.Interfaces.Repositories
Imports AccuPay.Core.Services.LeaveBalanceReset
Imports AccuPay.Core.ValueObjects
Imports AccuPay.Desktop.Utilities
Imports log4net
Imports Microsoft.Extensions.DependencyInjection

Public Class LeaveResetForm
    Private _logger As ILog = LogManager.GetLogger("PayrollLogger")
    Private _employeeRepository As IEmployeeRepository
    Private _employees As List(Of Employee)
    Private _timePeriod As TimePeriod
    Private ReadOnly _leaveResetDataService As ILeaveResetDataService
    Private ReadOnly _cashoutUnusedLeaveRepository As ICashoutUnusedLeaveRepository

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        '_listOfValueService = MainServiceProvider.GetRequiredService(Of IListOfValueService)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _leaveResetDataService = MainServiceProvider.GetRequiredService(Of ILeaveResetDataService)

        _cashoutUnusedLeaveRepository = MainServiceProvider.GetRequiredService(Of ICashoutUnusedLeaveRepository)
    End Sub

    Private Async Sub PreviewLeaveBalanceForm_LoadAsync(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvEmployees.AutoGenerateColumns = False

        _timePeriod = MakeTimePeriod(Date.Now, Date.Now)
        Dim leaveReset = Await GetLeaveReset(_timePeriod)
        _timePeriod = leaveReset.GetTimePeriod(Date.Now.Year)

        Dim isVacationLeaveSupported = leaveReset.IsVacationLeaveSupported
        eVacationLeaveAllowance.Visible = isVacationLeaveSupported
        eVacationLeaveBalance.Visible = isVacationLeaveSupported

        Dim isSickLeaveSupported = leaveReset.IsSickLeaveSupported
        eSickLeaveAllowance.Visible = isSickLeaveSupported
        eSickLeaveBalance.Visible = isSickLeaveSupported

        Dim isOthersLeaveSupported = leaveReset.IsOthersLeaveSupported
        eOthersLeaveAllowance.Visible = isOthersLeaveSupported
        eOthersLeaveBalance.Visible = isOthersLeaveSupported

        Dim isParentalLeaveSupported = leaveReset.IsParentalLeaveSupported
        eParentalLeaveAllowance.Visible = isParentalLeaveSupported
        eParentalLeaveBalance.Visible = isParentalLeaveSupported

        Dim leaveBasisStartDates = {leaveReset.VacationLeaveBasisStartDate,
            leaveReset.SickLeaveBasisStartDate,
            leaveReset.OthersLeaveBasisStartDate,
            leaveReset.ParentalLeaveBasisStartDate}
        Dim hasDateRegularized = leaveBasisStartDates.Where(Function(x) x = BasisStartDateEnum.DateRegularized).Any()
        eDateRegularized.Visible = hasDateRegularized

        leaveReset.ChangeDateAccordingToYear()

        UpdateLeaveResetPeriodRange(leaveReset)

        Await LoadEmployees(leaveReset:=leaveReset)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub UpdateLeaveResetPeriodRange(leaveReset As LeaveReset)
        LinkLabel1.Text = $"For the period of {leaveReset.StartPeriodDate.ToShortDateString()} to {leaveReset.EndPeriodDate.ToShortDateString()}"
    End Sub

    Private Async Function GetLeaveReset(timePeriod As TimePeriod) As Task(Of LeaveReset)
        Return Await _leaveResetDataService.GetByOrganizationIdAndDate(
            organizationId:=z_OrganizationID,
            timePeriod:=timePeriod)
    End Function

    Private Function MakeTimePeriod(start As Date, [end] As Date) As TimePeriod
        Return New TimePeriod(start:=start, [end]:=[end])
    End Function

    Private Function GetPayrollSelector() As MultiplePayPeriodSelectionDialog
        Dim payrollSelector = New MultiplePayPeriodSelectionDialog With {
            .ShowPayrollSummaryPanel = False,
            .ShowDeclaredOrActualOptionsPanel = False}

        If payrollSelector.ShowDialog() <> DialogResult.OK Then
            Return Nothing
        End If

        Return payrollSelector
    End Function

    Private Async Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        'Handles LinkLabel1.LinkClicked
        Dim payrollSelector = GetPayrollSelector()
        If payrollSelector Is Nothing Then Return

        _timePeriod = MakeTimePeriod(payrollSelector.PayPeriodFrom.PayFromDate,
            payrollSelector.PayPeriodTo.PayToDate)

        Dim leaveReset = Await GetLeaveReset(_timePeriod)

        leaveReset.ChangeDateAccordingToYear(_timePeriod.Start.Year)

        UpdateLeaveResetPeriodRange(leaveReset)
    End Sub

    Private Async Function LoadEmployees(leaveReset As LeaveReset) As Task
        Dim activeEmployees = Await _employeeRepository.
            GetAllWithinServicePeriodAsync(z_OrganizationID, leaveReset.EndPeriodDate)

        _employees = activeEmployees.
            OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst).
            ToList()

        Dim _employeeModels = _employees.
            Select(Function(e) New EmployeeModel(e)).
            ToList()

        Dim cashoutUnusedLeaves = Await _cashoutUnusedLeaveRepository.GetFromLatestPeriodAsync(organizationId:=z_OrganizationID)

        dgvEmployees.DataSource = _employeeModels

        'DataGridViewX1.DataSource = Nothing
    End Function

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Dim confirm = MessageBoxHelper.Confirm(Of Boolean)(
            $"Reset leave {LinkLabel1.Text.ToLower()}?",
            "Reset leave")

        If Not confirm Then Return

        Dim generator = New LeaveBalanceResetGeneration(_employees, additionalProgressCount:=1)
        Dim progressDialog = New ProgressDialog(generator, "Resetting leeave...")

        Enabled = False
        progressDialog.Show()

        generator.SetCurrentMessage("Loading resources...")
        GetResources(progressDialog,
            Sub(resourcesTask)
                If resourcesTask Is Nothing Then

                    HandleErrorLoadingResources(progressDialog)
                    Return
                End If

                generator.IncreaseProgress("Finished loading resources.")

                Dim generationTask = Task.Run(
                    Async Function()
                        Dim leaveReset = Await GetLeaveReset(_timePeriod)

                        Await generator.Start(leaveReset, resourcesTask.Result)
                    End Function
                )

                generationTask.ContinueWith(
                    Sub() GeneratePayrollOnSuccess(generator.Results, progressDialog),
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext
                )

                generationTask.ContinueWith(
                    Sub(t) GeneratePayrollOnError(t, progressDialog),
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnFaulted,
                    TaskScheduler.FromCurrentSynchronizationContext
                )
            End Sub)
    End Sub

    Private Sub GetResources(progressDialog As ProgressDialog,
            callBackAfterLoadResources As Action(Of Task(Of ILeaveResetResources)))
        Dim resources = MainServiceProvider.GetRequiredService(Of ILeaveResetResources)

        Dim loadTask = Task.Run(
            Function()
                Dim resourcesTask = resources.Load(
                    organizationId:=z_OrganizationID,
                    userId:=z_User,
                    timePeriod:=_timePeriod)

                resourcesTask.Wait()

                Return resources
            End Function)

        loadTask.ContinueWith(
            callBackAfterLoadResources,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        loadTask.ContinueWith(
            Sub(t) LoadingResourcesOnError(t, progressDialog),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )
    End Sub

    Private Sub LoadingResourcesOnError(t As Task, progressDialog As ProgressDialog)

        _logger.Error("Error loading one of the payroll data.", t.Exception)

        HandleErrorLoadingResources(progressDialog)

    End Sub

    Private Shared Sub HandleErrorLoadingResources(progressDialog As ProgressDialog)
        CloseProgressDialog(progressDialog)

        MsgBox("Something went wrong while loading the payroll data needed for computation. Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Resources")
    End Sub

    Private Shared Sub CloseProgressDialog(progressDialog As ProgressDialog)

        MDIPrimaryForm.Enabled = True

        If progressDialog Is Nothing Then Return

        progressDialog.Close()
        progressDialog.Dispose()
    End Sub

    Private Async Sub GeneratePayrollOnSuccess(results As IReadOnlyCollection(Of ProgressGenerator.IResult), progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        Dim saveResults = results.Select(Function(r) CType(r, LeaveResetResult)).ToList()

        Dim dialog = New EmployeeResultsDialog(
            saveResults,
            title:="Leave Reset Results",
            generationDescription:="Leave Reset",
            entityDescription:="leavereset(s)") With {
            .Owner = Me
        }

        dialog.ShowDialog()

        Dim leaveReset = Await GetLeaveReset(_timePeriod)
        Await LoadEmployees(leaveReset)

        Enabled = True
        MDIPrimaryForm.Enabled = True
    End Sub

    Private Sub GeneratePayrollOnError(t As Task, progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        _logger.Error("Error on generating payroll.", t.Exception)
        MsgBox("Something went wrong while generating the payroll . Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Generation")

        Enabled = True
        MDIPrimaryForm.Enabled = True
    End Sub

    Private Class EmployeeModel

        Private _employee As Employee

        Sub New(employee As Employee)
            _employee = employee
        End Sub

        Public ReadOnly Property RowID As Integer
            Get
                Return _employee.RowID.Value
            End Get
        End Property

        Public ReadOnly Property EmployeeNo As String
            Get
                Return _employee.EmployeeNo
            End Get
        End Property

        Public ReadOnly Property LastName As String
            Get
                Return _employee.LastName
            End Get
        End Property

        Public ReadOnly Property FirstName As String
            Get
                Return _employee.FirstName
            End Get
        End Property

        Public ReadOnly Property StartDate As Date
            Get
                Return _employee.StartDate
            End Get
        End Property

        Public ReadOnly Property DateRegularized As Date?
            Get
                Return _employee.DateRegularized
            End Get
        End Property

        Public ReadOnly Property VacationLeaveAllowance As Decimal
            Get
                Return _employee.VacationLeaveAllowance
            End Get
        End Property

        Public ReadOnly Property VacationLeaveBalance As Decimal
            Get
                Return _employee.LeaveBalance
            End Get
        End Property

        Public ReadOnly Property SickLeaveAllowance As Decimal
            Get
                Return _employee.SickLeaveAllowance
            End Get
        End Property

        Public ReadOnly Property SickLeaveBalance As Decimal
            Get
                Return _employee.SickLeaveBalance
            End Get
        End Property

        Public ReadOnly Property OthersLeaveAllowance As Decimal
            Get
                Return _employee.OtherLeaveAllowance
            End Get
        End Property

        Public ReadOnly Property OthersLeaveBalance As Decimal
            Get
                Return _employee.OtherLeaveBalance
            End Get
        End Property

        Public ReadOnly Property ParentalLeaveAllowance As Decimal
            Get
                Return _employee.MaternityLeaveAllowance
            End Get
        End Property

        Public ReadOnly Property ParentalLeaveBalance As Decimal
            Get
                Return _employee.MaternityLeaveBalance
            End Get
        End Property

    End Class

End Class
