Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Benchmark
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class SalaryTab

    Private Const FormEntityName As String = "Salary"

    Private _mode As FormMode = FormMode.Empty

    Private _employee As Employee

    Private _salaries As List(Of Salary)

    Private _currentSalary As Salary

    Private _isSystemOwnerBenchMark As Boolean

    Private _ecolaAllowance As Allowance

    Private _currentRolePermission As RolePermission

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Private ReadOnly _systemOwnerService As SystemOwnerService

    Public Sub New()
        InitializeComponent()
        dgvSalaries.AutoGenerateColumns = False

        If MainServiceProvider IsNot Nothing Then

            _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

            _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)
        End If

    End Sub

    Public Property BasicSalary As Decimal
        Get
            Return txtAmount.Text.ToDecimal
        End Get
        Set(value As Decimal)
            txtAmount.Text = CStr(value)
        End Set
    End Property

    Public Property AllowanceSalary As Decimal
        Get
            Return txtAllowance.Text.ToDecimal
        End Get
        Set(value As Decimal)
            txtAllowance.Text = CStr(value)
        End Set
    End Property

    Public Property PhilHealth As Decimal?
        Get
            Return txtPhilHealth.Text.ToNullableDecimal
        End Get
        Set(value As Decimal?)
            If value.HasValue Then
                txtPhilHealth.Text = CStr(value)
            End If
        End Set
    End Property

    Public Async Function SetEmployee(employee As Employee) As Task

        pbEmployee.Focus()
        _employee = employee

        If Await InitializeBenchmarkData() = False Then
            Return
        Else

            txtEcola.Text = _ecolaAllowance?.Amount.ToString
        End If

        txtPayFrequency.Text = employee.PayFrequency?.Type
        txtSalaryType.Text = employee.EmployeeType
        txtFullname.Text = employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = employee.EmployeeIdWithPositionAndEmployeeType

        pbEmployee.Image = ConvByteToImage(employee.Image)

        ChangeMode(FormMode.Empty)
        LoadSalaries()
    End Function

    Private Async Function InitializeBenchmarkData() As Task(Of Boolean)
        If _isSystemOwnerBenchMark Then

            Dim employeeId = _employee?.RowID

            Dim errorMessage = "Cannot retrieve ECOLA data. Please contact Globagility Inc. to fix this."

            Dim currentPayPeriod = Await _payPeriodRepository.GetCurrentPayPeriodAsync(z_OrganizationID)

            If currentPayPeriod Is Nothing OrElse employeeId Is Nothing Then
                MessageBoxHelper.ErrorMessage(errorMessage)

                Return False
            End If

            'If we are going to enable this as a policy, check its employee type.
            'If Daily then its allowance frequency should also be Daily.
            'Else allowance frequency should be semi-monthly (for Fixed and Monthly)
            'If _employee.IsDaily Then
            'End If
            _ecolaAllowance = Await BenchmarkPayrollHelper.GetEcola(
                employeeId.Value,
                payDateFrom:=currentPayPeriod.PayFromDate,
                payDateTo:=currentPayPeriod.PayToDate)

            If _ecolaAllowance Is Nothing Then
                MessageBoxHelper.ErrorMessage(errorMessage)
                Return False
            End If

        End If

        Return True

    End Function

    Private Sub ToggleBenchmarkEcola()

        lblTotalSalary.Visible = Not _isSystemOwnerBenchMark
        lblTotalSalaryPeroSign.Visible = Not _isSystemOwnerBenchMark
        txtTotalSalary.Visible = Not _isSystemOwnerBenchMark

        lblEcola.Visible = _isSystemOwnerBenchMark
        lblEcolaPeroSign.Visible = _isSystemOwnerBenchMark
        txtEcola.Visible = _isSystemOwnerBenchMark

    End Sub

    Private Async Sub SalaryTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If DesignMode Then
            Return
        End If

        ChangeMode(FormMode.Disabled)
        LoadSalaries()

        Await CheckRolePermissions()

        _isSystemOwnerBenchMark = _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark

        ToggleBenchmarkEcola()

        AddHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
    End Sub

    Private Async Function CheckRolePermissions() As Task
        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.SALARY)

        btnNew.Visible = False
        btnImport.Visible = False
        btnSave.Visible = False
        btnCancel.Visible = False
        btnDelete.Visible = False
        grpSalary.Enabled = False

        If role.Success Then

            _currentRolePermission = role.RolePermission

            If role.RolePermission.Create Then
                btnNew.Visible = True
                btnImport.Visible = True

            End If

            If role.RolePermission.Update Then
                btnSave.Visible = True
                btnCancel.Visible = True
                grpSalary.Enabled = True
            End If

            If role.RolePermission.Delete Then
                btnDelete.Visible = True

            End If

        End If
    End Function

    Private Sub ChangeMode(mode As FormMode)
        _mode = mode

        Select Case _mode
            Case FormMode.Disabled
                btnNew.Enabled = False
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case FormMode.Empty
                btnNew.Enabled = True
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case FormMode.Editing
                btnNew.Enabled = True
                btnSave.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
        End Select
    End Sub

    Private Sub LoadSalaries()

        If _employee?.RowID Is Nothing Then
            Return
        End If

        Dim salaryRepository = MainServiceProvider.GetRequiredService(Of SalaryRepository)

        _salaries = salaryRepository.GetByEmployee(_employee.RowID.Value).
            OrderByDescending(Function(s) s.EffectiveFrom).
            ToList()

        RemoveHandler dgvSalaries.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
        dgvSalaries.DataSource = _salaries

        If _currentSalary IsNot Nothing Then
            Dim oldSalary = _currentSalary
            _currentSalary = Nothing

            For Each row As DataGridViewRow In dgvSalaries.Rows
                Dim salary = DirectCast(row.DataBoundItem, Salary)
                If oldSalary.RowID = salary.RowID Then
                    _currentSalary = oldSalary
                    dgvSalaries.CurrentCell = row.Cells(0)
                    row.Selected = True
                    FormToolsControl(True)
                    ChangeMode(FormMode.Editing)
                    Exit For
                End If
            Next
        End If

        If _currentSalary Is Nothing Then
            SelectSalary(_salaries.FirstOrDefault())
        End If

        AddHandler dgvSalaries.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
    End Sub

    Private Sub FormToolsControl(enabled As Boolean)

        If enabled Then
            txtPhilHealth.Enabled = Not chkPayPhilHealth.Checked
            txtPagIbig.Enabled = Not ChkPagIbig.Checked

            If _currentRolePermission IsNot Nothing AndAlso _currentRolePermission.Update Then

                grpSalary.Enabled = True

            End If
        Else
            grpSalary.Enabled = False
        End If

    End Sub

    Private Sub ClearForm()
        dtpEffectiveFrom.Value = Date.Today
        txtAmount.Text = String.Empty
        txtAllowance.Text = String.Empty
        txtTotalSalary.Text = String.Empty
        txtPhilHealth.Text = String.Empty
        txtPagIbig.Text = String.Empty
        chkPaySSS.Checked = True
    End Sub

    Private Sub DisplaySalary()
        RemoveHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
        dtpEffectiveFrom.Value = _currentSalary.EffectiveFrom

        txtAmount.Text = CStr(_currentSalary.BasicSalary)
        txtAllowance.Text = CStr(_currentSalary.AllowanceSalary)
        txtTotalSalary.Text = CStr(_currentSalary.TotalSalary)

        chkPaySSS.Checked = _currentSalary.DoPaySSSContribution

        chkPayPhilHealth.Checked = _currentSalary.AutoComputePhilHealthContribution
        txtPhilHealth.Text = CStr(_currentSalary.PhilHealthDeduction)

        ChkPagIbig.Checked = _currentSalary.AutoComputeHDMFContribution
        txtPagIbig.Text = CStr(_currentSalary.HDMFAmount)

        AddHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If _employee Is Nothing Then
            MessageBoxHelper.ErrorMessage("Please select an employee first.")
            Return
        End If

        Dim form As New AddSalaryForm(_employee)
        form.ShowDialog()

        If form.isSaved Then
            LoadSalaries()
            FormToolsControl(True)
            If form.showBalloon Then
                ShowBalloonInfo("Salary successfuly added.", "Saved")
            End If

        End If

    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        'Focus other control to commit the changes on some control
        'ex. Change Start Date then clicking the save button immediately
        'does not change the Start DatePicker value unless you press tab first
        'or focus other control manually
        pbEmployee.Focus()

        If _currentSalary Is Nothing Then

            MessageBoxHelper.Warning("No selected salary.")
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save Salary",
            Async Function()

                Dim salary = _currentSalary.CloneJson()

                With salary
                    .EffectiveFrom = dtpEffectiveFrom.Value
                    .BasicSalary = txtAmount.Text.ToDecimal
                    .AllowanceSalary = txtAllowance.Text.ToDecimal
                    .DoPaySSSContribution = chkPaySSS.Checked
                    .AutoComputePhilHealthContribution = chkPayPhilHealth.Checked
                    .PhilHealthDeduction = txtPhilHealth.Text.ToDecimal
                    .AutoComputeHDMFContribution = ChkPagIbig.Checked
                    .HDMFAmount = txtPagIbig.Text.ToDecimal
                    .LastUpdBy = z_User
                End With

                salary.UpdateTotalSalary()

                Dim repository = MainServiceProvider.GetRequiredService(Of SalaryRepository)
                Dim oldsalary = Await repository.GetByIdAsync(salary.RowID.Value)

                Dim dataService = MainServiceProvider.GetRequiredService(Of SalaryDataService)
                Await dataService.SaveAsync(salary)

                If _isSystemOwnerBenchMark Then

                    Await SaveEcola()

                End If

                _currentSalary = salary

                Dim messageTitle = "Update Salary"
                RecordUpdateSalary(oldsalary)

                ShowBalloonInfo("Salary successfuly saved.", messageTitle)
                LoadSalaries()
                ChangeMode(FormMode.Editing)
            End Function)
    End Sub

    Private Function RecordUpdateSalary(oldSalary As Salary) As Boolean

        If oldSalary Is Nothing Then Return False

        Dim changes = New List(Of UserActivityItem)

        Dim entityName = FormEntityName.ToLower()

        If _currentSalary.EffectiveFrom <> oldSalary.EffectiveFrom Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = CInt(oldSalary.RowID),
                .Description = $"Updated {entityName} start date from '{oldSalary.EffectiveFrom.ToShortDateString}' to '{_currentSalary.EffectiveFrom.ToShortDateString}'."
            })
        End If
        If _currentSalary.BasicSalary <> oldSalary.BasicSalary Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = CInt(oldSalary.RowID),
                .Description = $"Updated basic salary from '{oldSalary.BasicSalary.ToString}' to '{_currentSalary.BasicSalary.ToString}'."
            })
        End If
        If _currentSalary.AllowanceSalary <> oldSalary.AllowanceSalary Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = CInt(oldSalary.RowID),
                .Description = $"Updated allowance salary from '{oldSalary.AllowanceSalary.ToString}' to '{_currentSalary.AllowanceSalary.ToString}'."
            })
        End If
        If _currentSalary.TotalSalary <> oldSalary.TotalSalary Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = CInt(oldSalary.RowID),
                .Description = $"Updated total salary from '{oldSalary.TotalSalary.ToString}' to '{_currentSalary.TotalSalary.ToString}'."
            })
        End If
        If _currentSalary.AutoComputePhilHealthContribution <> oldSalary.AutoComputePhilHealthContribution Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = CInt(oldSalary.RowID),
                .Description = $"Updated {entityName} PhilHealth auto from '{oldSalary.AutoComputePhilHealthContribution.ToString}' to '{_currentSalary.AutoComputePhilHealthContribution.ToString}'."
            })
        End If
        If _currentSalary.PhilHealthDeduction <> oldSalary.PhilHealthDeduction Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = CInt(oldSalary.RowID),
                .Description = $"Updated {entityName} PhilHealth deduction from '{oldSalary.PhilHealthDeduction.ToString}' to '{_currentSalary.PhilHealthDeduction.ToString}'."
            })
        End If
        If _currentSalary.DoPaySSSContribution <> oldSalary.DoPaySSSContribution Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = CInt(oldSalary.RowID),
                .Description = $"Updated {entityName} SSS deduction from '{oldSalary.DoPaySSSContribution.ToString}' to '{_currentSalary.DoPaySSSContribution.ToString}'."
            })
        End If
        If _currentSalary.AutoComputeHDMFContribution <> oldSalary.AutoComputeHDMFContribution Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = CInt(oldSalary.RowID),
                .Description = $"Updated {entityName} PagIbig auto from '{oldSalary.AutoComputeHDMFContribution.ToString}' to '{_currentSalary.AutoComputeHDMFContribution.ToString}'."
            })
        End If
        If _currentSalary.HDMFAmount <> oldSalary.HDMFAmount Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = CInt(oldSalary.RowID),
                .Description = $"Updated {entityName} PagIbig deduction from '{oldSalary.HDMFAmount.ToString}' to '{_currentSalary.HDMFAmount.ToString}'."
            })
        End If

        If changes.Count > 0 Then
            Dim userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
            userActivityRepository.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)
        End If

        Return False
    End Function

    Private Async Function SaveEcola() As Task
        Dim _ecolaAllowanceId = _ecolaAllowance?.RowID

        If _ecolaAllowanceId IsNot Nothing Then

            Dim repository = MainServiceProvider.GetRequiredService(Of AllowanceRepository)
            Dim dataService = MainServiceProvider.GetRequiredService(Of AllowanceDataService)

            Dim ecolaAllowance = Await repository.GetByIdAsync(_ecolaAllowanceId.Value)

            ecolaAllowance.Amount = txtEcola.Text.ToDecimal
            ecolaAllowance.LastUpdBy = z_User

            Await dataService.SaveAsync(ecolaAllowance)

        End If
    End Function

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If _currentSalary.RowID.HasValue = False Then

            MessageBoxHelper.Warning("No selected salary!")
            Return

        End If

        Dim result = MsgBox("Are you sure you want to delete this salary?", MsgBoxStyle.YesNo, "Delete Salary")

        If result <> MsgBoxResult.Yes Then Return

        Await FunctionUtils.TryCatchFunctionAsync("Delete Salary",
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of SalaryDataService)
                Await dataService.DeleteAsync(_currentSalary.RowID.Value)

                Dim userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
                userActivityRepository.RecordDelete(z_User, FormEntityName, CInt(_currentSalary.RowID), z_OrganizationID)

                LoadSalaries()
            End Function)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _mode = FormMode.Editing Then
            LoadSalaries()
        End If

        If _currentSalary Is Nothing Then
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
            DisplaySalary()
        End If
    End Sub

    Private Sub SelectSalary(salary As Salary)
        _currentSalary = salary

        If _currentSalary Is Nothing Then
            ClearForm()
            ChangeMode(FormMode.Empty)
            FormToolsControl(False)
        Else
            ChangeMode(FormMode.Editing)
            DisplaySalary()
            FormToolsControl(True)
        End If
    End Sub

    Private Sub dgvSalaries_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSalaries.SelectionChanged
        Dim salary = DirectCast(dgvSalaries.CurrentRow?.DataBoundItem, Salary)

        If salary Is Nothing Then
            Return
        End If

        SelectSalary(salary)
    End Sub

    Private Sub txtAmount_TextChanged(sender As Object, e As EventArgs)
        Dim basicSalary = txtAmount.Text.ToDecimal

        If _currentSalary Is Nothing Then
            Return
        End If

        Dim monthlyRate = Data.Helpers.PayrollTools.GetEmployeeMonthlyRate(_employee, _currentSalary)

        UpdateTotalSalary()
    End Sub

    Private Sub txtAllowance_TextChanged(sender As Object, e As EventArgs) Handles txtAllowance.TextChanged

        UpdateTotalSalary()

    End Sub

    Private Sub UpdateTotalSalary()
        Dim totalSalary = txtAmount.Text.ToDecimal + txtAllowance.Text.ToDecimal

        txtTotalSalary.Text = totalSalary.ToString
    End Sub

    Private Sub ChkPayPhilHealth_CheckedChanged(sender As Object, e As EventArgs) Handles chkPayPhilHealth.CheckedChanged
        txtPhilHealth.Enabled = Not chkPayPhilHealth.Checked
    End Sub

    Private Sub ChkPagIbig_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPagIbig.CheckedChanged
        txtPagIbig.Enabled = Not ChkPagIbig.Checked
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Using dialog = New ImportSalaryForm()
            dialog.ShowDialog()

            If dialog.IsSaved Then

                ShowBalloonInfo("Salaries Successfully Imported.", "Import Salary")
                LoadSalaries()
                ChangeMode(FormMode.Editing)

            End If
        End Using
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        EmployeeForm.Close()

    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 100, -20)
    End Sub

    Private Sub UserActivitySalaryToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivitySalaryToolStripButton.Click
        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

End Class