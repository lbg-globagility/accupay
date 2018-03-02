Option Strict On

Imports System.Linq
Imports AccuPay.Entity
Imports PayrollSys

Public Class SalaryTab

    Private Const StandardPagIbigContribution As Decimal = 100

    Private _mode As FormMode = FormMode.NoneSelected

    Private _employee As Employee

    Private _salaries As List(Of Salary)

    Private _currentSalary As Salary

    Private _socialSecurityBrackets As List(Of SocialSecurityBracket)

    Private _philHealthBrackets As List(Of PhilHealthBracket)

    Private _philHealthDeductionType As String

    Private _philHealthContributionRate As Decimal

    Private _philHealthMinimumContribution As Decimal

    Private _philHealthMaximumContribution As Decimal

    Public Sub SetEmployee(employee As Employee)
        _employee = employee
        txtPayFrequency.Text = employee.PayFrequency.Type
        txtSalaryType.Text = employee.EmployeeType
        txtFullname.Text = $"{employee.LastName}, {employee.FirstName}"
        pbEmployee.Image = ConvByteToImage(employee.Image)

        ChangeMode(FormMode.NoneSelected)
        LoadSalaries()
    End Sub

    Private Sub SalaryTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvSalaries.AutoGenerateColumns = False
        LoadSocialSecurityBrackets()
        LoadPhilHealthBrackets()
        ChangeMode(FormMode.Disabled)
    End Sub

    Private Sub ChangeMode(mode As FormMode)
        _mode = mode

        Select Case _mode
            Case FormMode.Disabled
                btnNew.Enabled = False
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case FormMode.NoneSelected
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
        Using context = New PayrollContext()
            _salaries = context.
                Salaries.Where(Function(s) Nullable.Equals(s.EmployeeID, _employee.RowID)).
                ToList()
        End Using

        dgvSalaries.DataSource = _salaries
    End Sub

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
            _philHealthContributionRate = If(values.GetDecimal("Rate"), 0)
            _philHealthMinimumContribution = If(values.GetDecimal("MinimumContribution"), 0)
            _philHealthMaximumContribution = If(values.GetDecimal("MaximumContribution"), 0)

            _philHealthBrackets = context.PhilHealthBrackets.ToList()
        End Using
    End Sub

    Private Sub DisplaySalary()
        txtAmount.Text = CStr(_currentSalary.BasicSalary)
        txtAllowance.Text = CStr(_currentSalary.AllowanceSalary)
        txtBasicPay.Text = CStr(_currentSalary.BasicPay)
        txtPhilHealth.Text = CStr(_currentSalary.PhilHealthDeduction)
        txtPagIbig.Text = CStr(_currentSalary.HDMFAmount)
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        _currentSalary = New Salary() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .EmployeeID = _employee.RowID,
            .PositionID = _employee.PositionID,
            .HDMFAmount = StandardPagIbigContribution
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
        With _currentSalary
            .BasicSalary = TypeTools.ParseDecimal(txtAmount.Text)
            .AllowanceSalary = TypeTools.ParseDecimal(txtAllowance.Text)
            .TotalSalary = (.BasicSalary + .AllowanceSalary)
            .EffectiveFrom = dtpEffectiveFrom.Value
            .EffectiveTo = dtpEffectiveTo.Value
            .PhilHealthDeduction = TypeTools.ParseDecimal(txtPhilHealth.Text)
            .HDMFAmount = TypeTools.ParseDecimal(txtPagIbig.Text)
        End With

        Using context = New PayrollContext()
            Try
                If _currentSalary.RowID.HasValue Then
                    _currentSalary.LastUpdBy = z_User
                    context.Entry(_currentSalary).State = Entity.EntityState.Modified
                Else
                    context.Salaries.Add(_currentSalary)
                End If

                context.SaveChanges()
            Catch ex As Exception

            End Try
        End Using

        LoadSalaries()
        ChangeMode(FormMode.Editing)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Throw New NotImplementedException()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        EnableSalaryGrid()
    End Sub

    Private Sub SelectSalary(salary As Salary)
        _currentSalary = salary

        ChangeMode(FormMode.Editing)
        DisplaySalary()
    End Sub

    Private Sub dgvSalaries_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSalaries.SelectionChanged
        Dim salary = DirectCast(dgvSalaries.CurrentRow?.DataBoundItem, Salary)

        If salary Is Nothing Then
            Return
        End If

        SelectSalary(salary)
    End Sub

    Private Sub txtAmount_TextChanged(sender As Object, e As EventArgs) Handles txtAmount.TextChanged
        Dim salary = TypeTools.ParseDecimal(txtAmount.Text)

        Dim monthlyRate = 0D
        If _employee.EmployeeType = "Daily" Then
            monthlyRate = salary * PayrollTools.GetWorkDaysPerMonth(_employee.WorkDaysPerYear)
        ElseIf _employee.EmployeeType = "Monthly" Then
            monthlyRate = salary
        ElseIf _employee.EmployeeType = "Fixed" Then
            monthlyRate = salary
        End If

        UpdateSss(monthlyRate)
        UpdatePhilHealth(monthlyRate)
    End Sub

    Private Sub UpdateSss(monthlyRate As Decimal)
        Dim socialSecurityBracket = _socialSecurityBrackets.FirstOrDefault(
                    Function(s) s.RangeFromAmount <= monthlyRate And monthlyRate <= s.RangeToAmount)

        _currentSalary.PaySocialSecurityID = socialSecurityBracket?.RowID
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
            Dim philHealthBracket = _philHealthBrackets.FirstOrDefault(
                Function(p) p.SalaryRangeFrom <= monthlyRate And monthlyRate <= p.SalaryRangeTo)

            _currentSalary.PayPhilHealthID = philHealthBracket?.RowID

            philHealthContribution = If(philHealthBracket?.TotalMonthlyPremium, 0)
        End If

        txtPhilHealth.Text = CStr(philHealthContribution)
    End Sub

    Private Sub txtSss_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSss.KeyDown
        e.Handled = True

        Select Case e.KeyCode
            Case Keys.Back, Keys.D0, Keys.NumPad0
                _currentSalary.PaySocialSecurityID = Nothing
                txtSss.Text = ""
        End Select
    End Sub

    Private Enum FormMode
        Disabled
        NoneSelected
        Creating
        Editing
    End Enum

End Class
