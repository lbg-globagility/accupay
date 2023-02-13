Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Interfaces.Repositories
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection
Imports AccuPay.Core.Services.Imports.ResetLeaveCredits
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Core.Helpers

Public Class ResetLeaveCreditsForm
    Private _resetLeaveCreditModels As List(Of ResetLeaveCreditModel)
    Private _policy As IPolicyHelper
    Private _currentRolePermission As RolePermission

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _resetLeaveCreditModels = New List(Of ResetLeaveCreditModel)

        gridPeriods.AutoGenerateColumns = False
        gridEmployees.AutoGenerateColumns = False

        _policy = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

    End Sub

    Private Async Sub ResetLeaveCreditsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await PopulateResetLeaveCredits()

        Await CheckRolePermissions()

        AddHandlerGridPeriodsSelectionChanged()

        gridPeriods_SelectionChanged(gridPeriods, New EventArgs)
    End Sub

    Private Async Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Dim payrollSelector = GetPayrollSelector()
        If payrollSelector Is Nothing Then Return

        Dim selectedPeriods = {payrollSelector.PayPeriodFrom, payrollSelector.PayPeriodTo}

        Dim startPeriod = selectedPeriods.FirstOrDefault()
        Dim endPeriod = selectedPeriods.LastOrDefault()

        Dim dateFrom = startPeriod.PayFromDate
        Dim dateTo = endPeriod.PayToDate

        Dim newResetLeaveCredit = ResetLeaveCredit.NewResetLeaveCredit(organizationId:=z_OrganizationID, userId:=z_User, periodId:=startPeriod.RowID, period:=startPeriod)

        Dim employees = (Await GetEmployees()).
            OrderBy(Function(x) x.FullNameLastNameFirst).
            ToList()
        newResetLeaveCredit.ResetLeaveCreditItems = employees.
            Select(Function(x) ResetLeaveCreditItem.NewResetLeaveCreditItem(organizationId:=z_OrganizationID, userId:=z_User, employeeId:=x.RowID, employee:=x)).
            ToList()

        RemoveHandlerGridPeriodsSelectionChanged()

        _resetLeaveCreditModels.Add(New ResetLeaveCreditModel(newResetLeaveCredit))

        gridPeriods.DataSource = _resetLeaveCreditModels.ToList()

        gridPeriods.Refresh()

        AddHandlerGridPeriodsSelectionChanged()

        gridPeriods_SelectionChanged(gridPeriods, New EventArgs)
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        gridEmployees.EndEdit()

        Dim messageTitle = "Save ResetLeaveCredits"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of IResetLeaveCreditDataService)

                Await dataService.SaveManyAsync2(organizationId:=z_OrganizationID,
                    userId:=z_User,
                    resetLeaveCreditModels:=_resetLeaveCreditModels)

                Await PopulateResetLeaveCredits()

                gridPeriods_SelectionChanged(gridPeriods, New EventArgs)
            End Function)
    End Sub

    Private Async Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Await PopulateResetLeaveCredits()

        gridPeriods_SelectionChanged(gridPeriods, New EventArgs)
    End Sub

    Private Sub btnUserActivities_Click(sender As Object, e As EventArgs) Handles btnUserActivities.Click
        Dim formEntityName As String = "ResetLeaveCredit"

        Dim userActivity As New UserActivityForm(formEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
        TimeAttendForm.listTimeAttendForm.Remove(Name)
    End Sub

    Private Sub gridPeriods_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridPeriods.CellContentClick

    End Sub

    Private Sub gridPeriods_SelectionChanged(sender As Object, e As EventArgs)
        btnApply.Enabled = False
        If gridPeriods.Rows.Count > 0 AndAlso gridPeriods.CurrentRow IsNot Nothing Then
            Dim resetLeaveCreditModel = DirectCast(gridPeriods.CurrentRow.DataBoundItem, ResetLeaveCreditModel)

            gridEmployees.DataSource = resetLeaveCreditModel.Items.
                OrderBy(Function(i) i.LastName).
                ThenBy(Function(i) i.FirstName).
                ToList()

            btnApply.Enabled = Not resetLeaveCreditModel.IsNew
            If Not resetLeaveCreditModel.IsNew Then
                btnApply.Enabled = resetLeaveCreditModel.HasValues
            End If
        Else
            gridEmployees.DataSource = Enumerable.Empty(Of ResetLeaveCreditModel)().ToList()
        End If
    End Sub

    Private Sub gridEmployees_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridEmployees.CellContentClick

    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Dim dataSource = gridEmployees.Rows.
            OfType(Of DataGridViewRow).
            Select(Function(r) DirectCast(r.DataBoundItem, ResetLeaveCreditItemModel)).
            ToList()
        If Not dataSource.Any() Then Return

        Dim form As New ResetLeaveCreditApplyPreviewForm(dataSource:=dataSource)
        If form.ShowDialog() = DialogResult.OK Then
            btnCancel_Click(btnCancel, New EventArgs)
        End If
    End Sub

    Private Async Function LoadResetLeaveCredits() As Task(Of List(Of ResetLeaveCredit))
        Dim repository = MainServiceProvider.GetRequiredService(Of IResetLeaveCreditRepository)

        Return Await repository.GetResetLeaveCredits(z_OrganizationID)
    End Function

    Private Async Function GetEmployees() As Task(Of List(Of Employee))
        Dim repository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        Return Await repository.GetAllAsync(z_OrganizationID)
    End Function

    Private Async Function PopulateResetLeaveCredits() As Task
        _resetLeaveCreditModels = (Await LoadResetLeaveCredits()).
            Select(Function(i) New ResetLeaveCreditModel(i)).
            ToList()

        gridPeriods.DataSource = _resetLeaveCreditModels.ToList()
    End Function

    Private Function GetPayrollSelector() As MultiplePayPeriodSelectionDialog
        Dim payrollSelector = New MultiplePayPeriodSelectionDialog With {
            .ShowPayrollSummaryPanel = False,
            .ShowDeclaredOrActualOptionsPanel = _policy.ShowActual
        }

        If payrollSelector.ShowDialog() <> DialogResult.OK Then
            Return Nothing
        End If

        Return payrollSelector
    End Function

    Private Sub AddHandlerGridPeriodsSelectionChanged()
        AddHandler gridPeriods.SelectionChanged, AddressOf gridPeriods_SelectionChanged
    End Sub

    Private Sub RemoveHandlerGridPeriodsSelectionChanged()
        RemoveHandler gridPeriods.SelectionChanged, AddressOf gridPeriods_SelectionChanged
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If gridPeriods.Rows.Count > 0 AndAlso gridPeriods.CurrentRow IsNot Nothing Then
            Dim prompt = MessageBox.Show(
            $"Are you sure you want delete this Reset Leave Credit?",
            "Delete Reset Leave Credits",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            If Not prompt = Windows.Forms.DialogResult.Yes Then Return

            Dim resetLeaveCreditModel = DirectCast(gridPeriods.CurrentRow.DataBoundItem, ResetLeaveCreditModel)

            Dim messageTitle = "Delete ResetLeaveCredits"

            Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of IResetLeaveCreditDataService)

                Await dataService.DeleteAsync(id:=resetLeaveCreditModel.Id.Value, currentlyLoggedInUserId:=z_User)

                Await PopulateResetLeaveCredits()

                gridPeriods_SelectionChanged(gridPeriods, New EventArgs)
            End Function)
        End If
    End Sub

    Private Sub gridEmployees_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles gridEmployees.CellValueChanged

        If gridEmployees.Rows.Count > 0 AndAlso gridEmployees.CurrentRow IsNot Nothing Then
            Dim resetLeaveCreditItemModel = DirectCast(gridEmployees.CurrentRow.DataBoundItem, ResetLeaveCreditItemModel)
            resetLeaveCreditItemModel.ChangeLastUpdBy(z_User)

            If resetLeaveCreditItemModel.IsApplied Then

            End If
        End If
    End Sub

    Private Sub gridEmployees_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles gridEmployees.CellBeginEdit
        If e.RowIndex < 0 Then Return

        Dim gridRow = gridEmployees.Rows(e.RowIndex)

        Dim resetLeaveCreditItemModel = DirectCast(gridRow.DataBoundItem, ResetLeaveCreditItemModel)
        If resetLeaveCreditItemModel IsNot Nothing AndAlso resetLeaveCreditItemModel.IsApplied Then
            gridEmployees.Rows(e.RowIndex).ReadOnly = resetLeaveCreditItemModel.IsApplied
            e.Cancel = True
        End If
    End Sub

    Private Async Function CheckRolePermissions() As Task
        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.RESET_LEAVE_CREDIT)

        btnNew.Visible = False
        'btnImport.Visible = False
        btnSave.Visible = False
        btnCancel.Visible = False
        btnDelete.Visible = False
        'DetailsTabLayout.Enabled = False

        If role.Success Then

            _currentRolePermission = role.RolePermission

            If _currentRolePermission.Create Then
                btnNew.Visible = True
                'btnImport.Visible = True

            End If

            If _currentRolePermission.Update Then
                btnSave.Visible = True
                btnCancel.Visible = True
                'DetailsTabLayout.Enabled = True
            End If

            If _currentRolePermission.Delete Then
                btnDelete.Visible = True

            End If

        End If
    End Function
End Class
