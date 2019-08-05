Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Enums
Imports AccuPay.Extensions
Imports AccuPay.Repository
Imports AccuPay.SimplifiedEntities
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class SalaryTab

    Dim sys_ownr As New SystemOwner

    Private Const StandardPagIbigContribution As Decimal = 100

    Private _mode As FormMode = FormMode.Empty

    Private _employee As Employee

    Private _salaries As List(Of Salary)

    Private _currentSalary As Salary

    Private _philHealthBrackets As List(Of PhilHealthBracket)

    Private _philHealthDeductionType As String

    Private _philHealthContributionRate As Decimal

    Private _philHealthMinimumContribution As Decimal

    Private _philHealthMaximumContribution As Decimal

    Private _isSystemOwnerBenchMark As Boolean

    Private _allowanceRepository As AllowanceRepository

    Private _ecolaAllowance As Allowance

    Private _currentPayPeriod As IPayPeriod

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

    Public Sub New()
        InitializeComponent()
        dgvSalaries.AutoGenerateColumns = False

        _allowanceRepository = New AllowanceRepository
    End Sub

    Public Async Function SetEmployee(employee As Employee) As Task

        _employee = employee

        If Await InitializeBenchmarkData() = False Then
            Return
        Else

            txtEcola.Text = _ecolaAllowance.Amount.ToString
        End If

        If _mode = FormMode.Creating Then
            EnableSalaryGrid()
        End If

        txtPayFrequency.Text = employee.PayFrequency.Type
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

            If _currentPayPeriod Is Nothing OrElse employeeId Is Nothing Then
                MessageBoxHelper.ErrorMessage(errorMessage)

                Return False
            End If

            'If we are going to enable this as a policy, check its employee type.
            'If Daily then its allowance frequency should also be Daily.
            'Else allowance frequency should be semi-monthly (for Fixed and Monthly)
            'If _employee.IsDaily Then
            'End If

            _ecolaAllowance = Await PayrollTools.GetOrCreateEmployeeEcola(
                                                employeeId.Value,
                                                payDateFrom:=_currentPayPeriod.PayFromDate,
                                                payDateTo:=_currentPayPeriod.PayToDate,
                                                allowanceFrequency:=Allowance.FREQUENCY_DAILY,
                                                amount:=0,
                                                effectiveEndDateShouldBeNull:=True)

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

        LoadPhilHealthBrackets()
        ChangeMode(FormMode.Disabled)
        LoadSalaries()

        _isSystemOwnerBenchMark = sys_ownr.CurrentSystemOwner = SystemOwner.Benchmark

        _currentPayPeriod = Await PayrollTools.GetCurrentlyWorkedOnPayPeriodByCurrentYear()

        ToggleBenchmarkEcola()

        AddHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
    End Sub

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
            Case FormMode.Creating
                btnNew.Enabled = False
                btnSave.Enabled = True
                btnDelete.Enabled = False
                btnCancel.Enabled = True
            Case FormMode.Editing
                btnNew.Enabled = True
                btnSave.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
        End Select
    End Sub

    Private Sub LoadSalaries()
        If _employee Is Nothing Then
            Return
        End If

        Using context = New PayrollContext()
            _salaries = (From s In context.Salaries
                         Where CBool(s.EmployeeID = _employee.RowID)
                         Order By s.EffectiveFrom Descending).
                         ToList()
        End Using

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
                    Exit For
                End If
            Next
        End If

        If _currentSalary Is Nothing Then
            SelectSalary(_salaries.FirstOrDefault())
        End If

        AddHandler dgvSalaries.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
    End Sub

    Private Sub ValidateSalaryRanges(salaries As List(Of PayrollSys.Salary))
        If salaries.Count <= 1 Then
            'lblWarning.Visible = False
        End If

        For i = 0 To salaries.Count - 1
            Dim salary = salaries.Item(i)

            For j = i + 1 To salaries.Count - 1
                Dim comparedSalary = salaries.Item(j)
                If salary.RowID = comparedSalary.RowID Then
                    Continue For
                End If

                If SalariesOverlap(salary, comparedSalary) Then
                    'TODO make the overlapping salaries show in the form as warnings
                    'lblWarning.Text = "Warning: One or more of the employee's salary history is overlapping with another salary's date."
                    'lblWarning.Visible = True
                    'WarnBalloon("You have input a date range overlaps to employee's existing salary.", "Overlapping dates", lblforballoon, 0, -69)
                Else
                    'lblWarning.Visible = False
                End If
            Next
        Next
    End Sub

    Private Function SalariesOverlap(salaryA As PayrollSys.Salary, salaryB As PayrollSys.Salary) As Boolean
        'If (Not salaryA.IsIndefinite) And (Not salaryB.IsIndefinite) Then
        '    Return salaryA.EffectiveFrom <= salaryB.EffectiveTo And
        '        salaryB.EffectiveFrom <= salaryA.EffectiveTo
        'End If

        'If salaryA.IsIndefinite And (Not salaryB.IsIndefinite) Then
        '    Return salaryB.EffectiveTo >= salaryA.EffectiveFrom
        'End If

        'If salaryB.IsIndefinite And (Not salaryA.IsIndefinite) Then
        '    Return salaryA.EffectiveTo >= salaryB.EffectiveFrom
        'End If

        Return True
    End Function

    Private Sub LoadPhilHealthBrackets()
        Using context = New PayrollContext()
            Dim listOfValues = context.ListOfValues.
                Where(Function(l) l.Type = "PhilHealth").
                ToList()

            Dim values = New ListOfValueCollection(listOfValues)

            _philHealthDeductionType = If(values.GetValue("DeductionType"), "Bracket")
            _philHealthContributionRate = values.GetDecimal("Rate")
            _philHealthMinimumContribution = values.GetDecimal("MinimumContribution")
            _philHealthMaximumContribution = values.GetDecimal("MaximumContribution")

            _philHealthBrackets = context.PhilHealthBrackets.ToList()
        End Using
    End Sub

    Private Sub ClearForm()
        dtpEffectiveFrom.Value = Date.Today
        dtpEffectiveTo.Value = Date.Today
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

        dtpEffectiveTo.Checked = _currentSalary.EffectiveTo.HasValue
        If _currentSalary.EffectiveTo.HasValue Then
            dtpEffectiveTo.Value = _currentSalary.EffectiveTo.Value
        End If

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
        Dim latestSalary = _salaries.
            OrderBy(Function(s) s.EffectiveTo).
            LastOrDefault()

        _currentSalary = New Salary() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .EmployeeID = _employee.RowID,
            .PositionID = _employee.PositionID,
            .HDMFAmount = StandardPagIbigContribution,
            .EffectiveFrom = Date.Today,
            .EffectiveTo = Nothing
        }

        DisableSalaryGrid()
        ChangeMode(FormMode.Creating)
        DisplaySalary()
    End Sub

    Private Sub DisableSalaryGrid()
        RemoveHandler dgvSalaries.SelectionChanged, AddressOf dgvSalaries_SelectionChanged
        dgvSalaries.ClearSelection()
        dgvSalaries.CurrentCell = Nothing
    End Sub

    Private Sub EnableSalaryGrid()
        AddHandler dgvSalaries.SelectionChanged, AddressOf dgvSalaries_SelectionChanged

        If dgvSalaries.Rows.Count > 0 Then
            dgvSalaries.Item(0, 0).Selected = True
            SelectSalary(DirectCast(dgvSalaries.CurrentRow.DataBoundItem, Salary))
        End If
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Using context = New PayrollContext()
            Try
                With _currentSalary
                    .EffectiveFrom = dtpEffectiveFrom.Value
                    .EffectiveTo = If(dtpEffectiveTo.Checked, dtpEffectiveTo.Value, New DateTime?)
                    .BasicSalary = txtAmount.Text.ToDecimal
                    .AllowanceSalary = txtAllowance.Text.ToDecimal
                    .TotalSalary = (.BasicSalary + .AllowanceSalary)
                    .DoPaySSSContribution = chkPaySSS.Checked
                    .AutoComputePhilHealthContribution = chkPayPhilHealth.Checked
                    .PhilHealthDeduction = txtPhilHealth.Text.ToDecimal
                    .AutoComputeHDMFContribution = ChkPagIbig.Checked
                    .HDMFAmount = txtPagIbig.Text.ToDecimal
                End With

                If _currentSalary.RowID.HasValue Then
                    _currentSalary.LastUpdBy = z_User
                    context.Entry(_currentSalary).State = EntityState.Modified
                Else
                    context.Salaries.Add(_currentSalary)
                End If

                If _isSystemOwnerBenchMark Then

                    Await SaveEcola(context)

                End If

                Await context.SaveChangesAsync()

                Dim messageTitle = ""
                If _mode = FormMode.Creating Then

                    messageTitle = "New Salary"

                ElseIf _mode = FormMode.Editing Then

                    messageTitle = "Update Salary"

                End If

                ShowBalloonInfo("Salary successfuly saved.", messageTitle)
            Catch ex As Exception
                MsgBox("Something wrong occured.", MsgBoxStyle.Exclamation)
                Throw
            End Try
        End Using
        LoadSalaries()
        ChangeMode(FormMode.Editing)
    End Sub

    Private Async Function SaveEcola(context As PayrollContext) As Task
        Dim _ecolaAllowanceId = _ecolaAllowance?.RowID

        If _ecolaAllowanceId IsNot Nothing Then

            Dim ecolaAllowance = Await context.Allowances.
                    FirstOrDefaultAsync(Function(a) a.RowID.Value = _ecolaAllowanceId.Value)

            ecolaAllowance.Amount = txtEcola.Text.ToDecimal
            ecolaAllowance.LastUpdBy = z_User

        End If
    End Function

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim result = MsgBox("Are you sure you want to delete this salary?", MsgBoxStyle.YesNo, "Delete Salary")

        If result = MsgBoxResult.Yes Then
            Using context = New PayrollContext()
                context.Salaries.Attach(_currentSalary)
                context.Salaries.Remove(_currentSalary)
                context.SaveChanges()
            End Using

            LoadSalaries()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _mode = FormMode.Creating Then
            SelectSalary(Nothing)
            EnableSalaryGrid()
        ElseIf _mode = FormMode.Editing Then
            LoadSalaries()
        End If

        If _currentSalary Is Nothing Then
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
        End If
    End Sub

    Private Sub SelectSalary(salary As Salary)
        _currentSalary = salary

        If _currentSalary Is Nothing Then
            ClearForm()
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
            DisplaySalary()
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
        Dim totalSalary = txtAmount.Text.ToDecimal +
                    txtAllowance.Text.ToDecimal

        txtTotalSalary.Text = totalSalary.ToString
    End Sub

    <Obsolete>
    Private Sub UpdatePhilHealth(monthlyRate As Decimal)
        Dim philHealthContribution = 0D
        If _philHealthDeductionType = "Formula" Then
            philHealthContribution = monthlyRate * (_philHealthContributionRate / 100)

            philHealthContribution = {philHealthContribution, _philHealthMinimumContribution}.Max()
            philHealthContribution = {philHealthContribution, _philHealthMaximumContribution}.Min()
            philHealthContribution = AccuMath.Truncate(philHealthContribution, 2)
        Else
            Dim philHealthBracket = _philHealthBrackets?.FirstOrDefault(
                Function(p) p.SalaryRangeFrom <= monthlyRate And monthlyRate <= p.SalaryRangeTo)

            _currentSalary.PayPhilHealthID = philHealthBracket?.RowID

            philHealthContribution = If(philHealthBracket?.TotalMonthlyPremium, 0)
        End If

        txtPhilHealth.Text = CStr(philHealthContribution)
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

End Class