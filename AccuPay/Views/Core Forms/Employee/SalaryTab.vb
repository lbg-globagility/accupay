Option Strict On

Imports System.Data.Entity
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class SalaryTab

    Private Const StandardPagIbigContribution As Decimal = 100

    Private _mode As FormMode = FormMode.Empty

    Private _employee As Employee

    Private _salaries As List(Of Salary)

    Private _currentSalary As Salary

    Private _socialSecurityBrackets As List(Of SocialSecurityBracket)

    Private _philHealthBrackets As List(Of PhilHealthBracket)

    Private _philHealthDeductionType As String

    Private _philHealthContributionRate As Decimal

    Private _philHealthMinimumContribution As Decimal

    Private _philHealthMaximumContribution As Decimal

    Public Property BasicSalary As Decimal
        Get
            Return TypeTools.ParseDecimal(txtAmount.Text)
        End Get
        Set(value As Decimal)
            txtAmount.Text = CStr(value)
        End Set
    End Property

    Public Property AllowanceSalary As Decimal
        Get
            Return TypeTools.ParseDecimal(txtAllowance.Text)
        End Get
        Set(value As Decimal)
            txtAllowance.Text = CStr(value)
        End Set
    End Property

    Public Property Sss As Decimal?
        Get
            Return TypeTools.ParseDecimalOrNull(txtSss.Text)
        End Get
        Set(value As Decimal?)
            If value.HasValue Then
                txtSss.Text = CStr(value)
            End If
        End Set
    End Property

    Public Property PhilHealth As Decimal?
        Get
            Return TypeTools.ParseDecimalOrNull(txtPhilHealth.Text)
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
    End Sub

    Public Sub SetEmployee(employee As Employee)
        If _mode = FormMode.Creating Then
            EnableSalaryGrid()
        End If

        _employee = employee
        txtPayFrequency.Text = employee.PayFrequency.Type
        txtSalaryType.Text = employee.EmployeeType
        txtFullname.Text = $"{employee.FirstName} {employee.LastName}"
        txtEmployeeID.Text = $"ID# {employee.EmployeeNo}, {employee.Position?.Name}, {employee.EmployeeType} Salary"

        pbEmployee.Image = ConvByteToImage(employee.Image)

        ChangeMode(FormMode.Empty)
        LoadSalaries()
    End Sub

    Private Sub SalaryTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If DesignMode Then
            Return
        End If

        LoadSocialSecurityBrackets()
        LoadPhilHealthBrackets()
        ChangeMode(FormMode.Disabled)
        LoadSalaries()

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
            _salaries = (From s In context.Salaries.Include(Function(s) s.SocialSecurityBracket)
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


    Private Sub LoadSocialSecurityBrackets()
        Using context = New PayrollContext()
            _socialSecurityBrackets = context.SocialSecurityBrackets.ToList()
        End Using
    End Sub

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
        txtBasicPay.Text = String.Empty
        txtSss.Text = String.Empty
        txtPhilHealth.Text = String.Empty
        txtPagIbig.Text = String.Empty
    End Sub

    Private Sub DisplaySalary()
        RemoveHandler txtAmount.TextChanged, AddressOf txtAmount_TextChanged
        dtpEffectiveFrom.Value = _currentSalary.EffectiveFrom
        dtpEffectiveTo.Value = If(_currentSalary.EffectiveTo, _currentSalary.EffectiveFrom.AddYears(100))
        txtAmount.Text = CStr(_currentSalary.BasicSalary)
        txtAllowance.Text = CStr(_currentSalary.AllowanceSalary)
        txtTotalSalary.Text = CStr(_currentSalary.TotalSalary)
        txtBasicPay.Text = CStr(_currentSalary.BasicPay)
        txtSss.Text = CStr(If(_currentSalary.SocialSecurityBracket?.EmployeeContributionAmount, 0))
        txtSss.Tag = _currentSalary.SocialSecurityBracket
        txtPhilHealth.Text = CStr(_currentSalary.PhilHealthDeduction)
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
            .EffectiveTo = .EffectiveFrom.AddYears(100)
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

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Using context = New PayrollContext()
            Try
                Dim socialSecurityBracket = DirectCast(txtSss.Tag, SocialSecurityBracket)

                If socialSecurityBracket IsNot Nothing Then
                    context.Entry(socialSecurityBracket).State = EntityState.Unchanged
                End If

                With _currentSalary
                    .BasicSalary = TypeTools.ParseDecimal(txtAmount.Text)
                    .AllowanceSalary = TypeTools.ParseDecimal(txtAllowance.Text)
                    .TotalSalary = (.BasicSalary + .AllowanceSalary)
                    .EffectiveFrom = dtpEffectiveFrom.Value
                    .EffectiveTo = dtpEffectiveTo.Value
                    .PhilHealthDeduction = TypeTools.ParseDecimal(txtPhilHealth.Text)
                    .PaySocialSecurityID = socialSecurityBracket?.RowID
                    .SocialSecurityBracket = socialSecurityBracket
                    .HDMFAmount = TypeTools.ParseDecimal(txtPagIbig.Text)
                End With

                If _currentSalary.RowID.HasValue Then
                    _currentSalary.LastUpdBy = z_User
                    context.Entry(_currentSalary).State = EntityState.Modified
                Else
                    context.Salaries.Add(_currentSalary)
                End If

                context.SaveChanges()
            Catch ex As Exception
                MsgBox("Something wrong occured.", MsgBoxStyle.Exclamation)
                Throw
            End Try
        End Using
        LoadSalaries()
        ChangeMode(FormMode.Editing)
    End Sub

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
        Dim salary = TypeTools.ParseDecimal(txtAmount.Text)

        Dim monthlyRate = 0D
        If _employee.EmployeeType = "Daily" Then
            monthlyRate = salary * PayrollTools.GetWorkDaysPerMonth(_employee.WorkDaysPerYear)
            txtBasicPay.Text = salary.ToString()
        ElseIf _employee.EmployeeType = "Monthly" Or _employee.EmployeeType = "Fixed" Then
            monthlyRate = salary

            If _employee.PayFrequency.Type = "SEMI-MONTHLY" Then

            End If
        End If

        If _currentSalary Is Nothing Then
            Return
        End If

        UpdateSss(monthlyRate)
        UpdatePhilHealth(monthlyRate)
    End Sub

    Private Sub UpdateBasicPay()

    End Sub

    Private Sub UpdateSss(monthlyRate As Decimal)
        Dim socialSecurityBracket = _socialSecurityBrackets?.FirstOrDefault(
                    Function(s) s.RangeFromAmount <= monthlyRate And monthlyRate <= s.RangeToAmount)

        txtSss.Tag = socialSecurityBracket
        txtSss.Text = socialSecurityBracket?.EmployeeContributionAmount.ToString()
    End Sub

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

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Using dialog = New ImportSalaryForm()
            dialog.ShowDialog()
        End Using
    End Sub

    Private Sub txtSss_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSss.KeyDown
        e.Handled = True

        Select Case e.KeyCode
            Case Keys.Back, Keys.D0, Keys.NumPad0
                txtSss.Tag = Nothing
                txtSss.Text = ""
        End Select
    End Sub

    Private Enum FormMode
        Disabled
        Empty
        Creating
        Editing
    End Enum

End Class
