Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.AccuPay.Desktop.Helpers
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class SalaryTab

    Private _mode As FormMode = FormMode.Empty

    Private _employee As Employee

    Private _salaries As List(Of Salary)

    Private _currentSalary As Salary

    Private _isSystemOwnerBenchMark As Boolean

    Private _ecolaAllowances As IReadOnlyCollection(Of Allowance)

    Private _ecolaAllowance As Allowance

    Private _currentRolePermission As RolePermission

    Private ReadOnly _systemOwnerService As ISystemOwnerService

    Private ReadOnly _organizationRepository As IOrganizationRepository

    Private ReadOnly _listOfValueService As IListOfValueService

    Public Sub New()
        InitializeComponent()
        dgvSalaries.AutoGenerateColumns = False

        If MainServiceProvider IsNot Nothing Then

            _systemOwnerService = MainServiceProvider.GetRequiredService(Of ISystemOwnerService)

            _organizationRepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)

            _listOfValueService = MainServiceProvider.GetRequiredService(Of IListOfValueService)
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

        txtPayFrequency.Text = employee.PayFrequency?.Type
        txtSalaryType.Text = employee.EmployeeType
        txtFullname.Text = employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = employee.EmployeeIdWithPositionAndEmployeeType

        pbEmployee.Image = ConvByteToImage(employee.Image)

        ChangeMode(FormMode.Empty)
        Await LoadSalaries()
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
        Await LoadSalaries()

        Await ShowOrHideGovernmentFields()

        Await CheckRolePermissions()

        _isSystemOwnerBenchMark = _systemOwnerService.GetCurrentSystemOwner() = SystemOwner.Benchmark

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

    Private Async Function ShowOrHideGovernmentFields() As Task
        Dim currentSystemOwner = Await _systemOwnerService.GetCurrentSystemOwnerEntityAsync()
        If Not currentSystemOwner.IsMorningSun Then Return

        Dim settings = Await _listOfValueService.CreateAsync()
        Dim hdmfPolicy = New Core.Services.Policies.HdmfPolicy(settings)
        Dim hdmfBool = Not hdmfPolicy.HdmfCalculationBasis(z_OrganizationID) = Core.Enums.HdmfCalculationBasis.BasedOnLoan
        Label11.Visible = hdmfBool
        ChkPagIbig.Visible = hdmfBool
        Label217.Visible = hdmfBool
        txtPagIbig.Visible = hdmfBool

        Dim philHealthPolicy = New Core.Services.PhilHealthPolicy(settings)
        Dim philHealthBool = Not philHealthPolicy.CalculationBasis(z_OrganizationID) = Core.Enums.PhilHealthCalculationBasis.BasedOnLoan
        Label8.Visible = philHealthBool
        chkPayPhilHealth.Visible = philHealthBool
        Label215.Visible = philHealthBool
        txtPhilHealth.Visible = philHealthBool

        Dim sssPolicy = New Core.Services.Policies.SssPolicy(settings)
        Dim sssBool = Not sssPolicy.SssCalculationBasis(z_OrganizationID) = Core.Enums.SssCalculationBasis.BasedOnLoan
        Label9.Visible = sssBool
        chkPaySSS.Visible = sssBool
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

    Private Async Function LoadSalaries() As Task

        If _employee?.RowID Is Nothing Then
            Return
        End If

        Dim salaryRepository = MainServiceProvider.GetRequiredService(Of ISalaryRepository)
        _salaries = (Await salaryRepository.GetByEmployeeAsync(_employee.RowID.Value)).
            OrderByDescending(Function(s) s.EffectiveFrom).
            ToList()

        Dim allowanceRepository = MainServiceProvider.GetRequiredService(Of IAllowanceRepository)
        _ecolaAllowances = (Await allowanceRepository.
            GetEmployeeEcolaAsync(_employee.RowID.Value, z_OrganizationID)).
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
    End Function

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
        chkIsMinimumWage.Checked = False

        txtAmount.Text = String.Empty
        txtAllowance.Text = String.Empty
        txtTotalSalary.Text = String.Empty

        chkPayPhilHealth.Checked = True
        txtPhilHealth.Text = String.Empty
        ChkPagIbig.Checked = True
        txtPagIbig.Text = String.Empty
        chkPaySSS.Checked = True
    End Sub

    Private Sub DisplaySalary()
        RemoveHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
        dtpEffectiveFrom.Value = _currentSalary.EffectiveFrom
        chkIsMinimumWage.Checked = _currentSalary.IsMinimumWage

        txtAmount.Text = _currentSalary.BasicSalary.ToString()
        txtAllowance.Text = _currentSalary.AllowanceSalary.ToString()
        txtTotalSalary.Text = _currentSalary.TotalSalary.ToString()

        chkPaySSS.Checked = _currentSalary.DoPaySSSContribution

        chkPayPhilHealth.Checked = _currentSalary.AutoComputePhilHealthContribution
        txtPhilHealth.Text = _currentSalary.PhilHealthDeduction.ToString()

        ChkPagIbig.Checked = _currentSalary.AutoComputeHDMFContribution
        txtPagIbig.Text = _currentSalary.HDMFAmount.ToString()

        If _isSystemOwnerBenchMark Then

            _ecolaAllowance = _ecolaAllowances.
                Where(Function(a) a.EffectiveStartDate.Date <= _currentSalary.EffectiveFrom.Date).
                OrderByDescending(Function(a) a.EffectiveStartDate).
                FirstOrDefault()

            txtEcola.Text = If(_ecolaAllowance?.Amount.ToString(), "0.00")
        End If

        AddHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
    End Sub

    Private Async Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If _employee Is Nothing Then
            MessageBoxHelper.ErrorMessage("Please select an employee first.")
            Return
        End If

        Dim form As New AddSalaryForm(_employee)
        form.ShowDialog()

        If form.IsSaved Then
            Await LoadSalaries()
            FormToolsControl(True)
            If form.ShowBalloon Then
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
                    .IsMinimumWage = chkIsMinimumWage.Checked
                    .BasicSalary = txtAmount.Text.ToDecimal
                    .AllowanceSalary = txtAllowance.Text.ToDecimal
                    .DoPaySSSContribution = chkPaySSS.Checked
                    .AutoComputePhilHealthContribution = chkPayPhilHealth.Checked
                    .PhilHealthDeduction = txtPhilHealth.Text.ToDecimal
                    .AutoComputeHDMFContribution = ChkPagIbig.Checked
                    .HDMFAmount = txtPagIbig.Text.ToDecimal
                End With

                Dim dataService = MainServiceProvider.GetRequiredService(Of ISalaryDataService)
                Await dataService.SaveAsync(salary, z_User)

                If _isSystemOwnerBenchMark AndAlso _ecolaAllowance IsNot Nothing Then

                    Await EcolaHelper.UpdateEcola(
                        _ecolaAllowance.RowID.Value,
                        salary.EffectiveFrom.Date,
                        txtEcola.Text.ToDecimal)

                End If

                _currentSalary = salary

                Dim messageTitle = "Update Salary"

                ShowBalloonInfo("Salary successfuly saved.", messageTitle)
                Await LoadSalaries()
                ChangeMode(FormMode.Editing)
            End Function)
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If _currentSalary.RowID.HasValue = False Then

            MessageBoxHelper.Warning("No selected salary!")
            Return

        End If

        Dim result = MsgBox("Are you sure you want to delete this salary?", MsgBoxStyle.YesNo, "Delete Salary")

        If result <> MsgBoxResult.Yes Then Return

        Await FunctionUtils.TryCatchFunctionAsync("Delete Salary",
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of ISalaryDataService)
                Await dataService.DeleteAsync(
                    id:=_currentSalary.RowID.Value,
                    currentlyLoggedInUserId:=z_User)

                Await LoadSalaries()

                ShowBalloonInfo($"Salary successfully deleted.", "Delete Salary")
            End Function)
    End Sub

    Private Async Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _mode = FormMode.Editing Then
            Await LoadSalaries()
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

        Dim monthlyRate = PayrollTools.GetEmployeeMonthlyRate(_employee, _currentSalary)

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

    Private Async Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Using dialog = New ImportSalaryForm()
            dialog.ShowDialog()

            If dialog.IsSaved Then

                ShowBalloonInfo("Salaries Successfully Imported.", "Import Salary")
                Await LoadSalaries()
                ChangeMode(FormMode.Editing)

            End If
        End Using
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        EmployeeForm.Close()

    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 70, -50)
    End Sub

    Private Sub UserActivitySalaryToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivitySalaryToolStripButton.Click

        Dim formEntityName As String = "Salary"

        Dim userActivity As New UserActivityForm(formEntityName)
        userActivity.ShowDialog()
    End Sub

End Class
