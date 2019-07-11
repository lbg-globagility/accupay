Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.SimplifiedEntities
Imports AccuPay.Utils
Imports PayrollSys

Public Class BenchmarkPayrollForm

    Private _employeeRepository As EmployeeRepository
    Private _salaryRepository As SalaryRepository
    Private _currentPayPeriod As IPayPeriod
    Private _salaries As List(Of Salary)
    Private _employees As List(Of Employee)

    Private _textBoxDelayedAction As New DelayedAction(Of Boolean)

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _employeeRepository = New EmployeeRepository
        _salaryRepository = New SalaryRepository
        _salaries = New List(Of Salary)
        _employees = New List(Of Employee)
    End Sub

    Private Async Function ShowEmployees() As Task

        Dim payPeriodId = _currentPayPeriod?.RowID

        If payPeriodId IsNot Nothing Then

            _employees = Await _employeeRepository.
                                GetAllActiveWithoutPayrollAsync(_currentPayPeriod.RowID)
        Else
            _employees = New List(Of Employee)
        End If

        Await FilterEmployeeGridView()

    End Function

    Private Sub ShowEmployee()

        ClearEmployeeForms()

        If EmployeesGridView.CurrentRow IsNot Nothing Then

            Dim employee = CType(EmployeesGridView.CurrentRow.DataBoundItem, Employee)

            Dim salary = _salaries.
                            Where(Function(s) Nullable.Equals(s.EmployeeID, employee?.RowID)).
                            FirstOrDefault

            If employee Is Nothing OrElse salary Is Nothing Then Return

            Dim monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee, salary.BasicSalary)
            Dim dailyRate = PayrollTools.GetDailyRate(monthlyRate, employee.WorkDaysPerYear)
            Dim hourlyRate = PayrollTools.GetHourlyRateByDailyRate(dailyRate)

            EmployeeNumberLabel.Text = employee.EmployeeNo
            EmployeeNameLabel.Text = employee.FullNameLastNameFirst.ToUpper

            TinTextBox.Text = employee.TinNo
            SssNumberTextBox.Text = employee.SssNo
            PagibigNumberTextBox.Text = employee.HdmfNo
            PhilhealthNumberTextBox.Text = employee.PhilHealthNo
            BasicPayTextBox.Text = AccuMath.CommercialRound(monthlyRate, 4).ToString("#,##0.0000")
            PerDayTextBox.Text = AccuMath.CommercialRound(dailyRate, 4).ToString("#,##0.0000")
            PerHourTextBox.Text = AccuMath.CommercialRound(hourlyRate, 4).ToString("#,##0.0000")
            AllowanceTextBox.Text = AccuMath.CommercialRound(salary.AllowanceSalary, 4).ToString("#,##0.0000")

            EmployeeDetailsGroupBox.Enabled = True
            InputsTabControl.Enabled = True

        End If

    End Sub

    Private Sub ClearEmployeeForms()

        EmployeeDetailsGroupBox.Enabled = False
        InputsTabControl.Enabled = False
        SummaryGroupBox.Enabled = False

        EmployeeNumberLabel.Text = "000-0000"
        EmployeeNameLabel.Text = "EMPLOYEE NAME"

        TinTextBox.ResetText()
        SssNumberTextBox.ResetText()
        PagibigNumberTextBox.ResetText()
        PhilhealthNumberTextBox.ResetText()
        BasicPayTextBox.ResetText()
        PerDayTextBox.ResetText()
        PerHourTextBox.ResetText()
        AllowanceTextBox.ResetText()

        RegularDaysTextBox.ResetText()
        OvertimeTextBox.ResetText()
        LateTextBox.ResetText()
        NightDifferentialTextBox.ResetText()
        EcolaTextBox.ResetText()

        BasicPaySummaryTextBox.ResetText()
        PhilhealthAmountTextBox.ResetText()
        SssAmountTextBox.ResetText()
        PagibigNumberTextBox.ResetText()
        WithholdingTaxTextBox.ResetText()
        PagibigLoanTextBox.ResetText()
        SssLoanTextBox.ResetText()

        NightDifferentialAmountTextBox.ResetText()
        ThirteenthMonthPayTextBox.ResetText()
        LeaveBalanceTextBox.ResetText()

        GrossPayTextBox.ResetText()
        TotalLeaveTextBox.ResetText()
        TotalDeductionTextBox.ResetText()
        TotalOtherIncomeTextBox.ResetText()
        TotalOvertimeTextBox.ResetText()
        NetPayTextBox.ResetText()

    End Sub

    Private Async Sub BenchmarkPayrollForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        EmployeesGridView.AutoGenerateColumns = False

        Await GetCutOffPeriod()

        _salaries = Await _salaryRepository.
                        GetAllByCutOff(_currentPayPeriod.PayFromDate)

        Await ShowEmployees()

        ClearEmployeeForms()

    End Sub

    Private Async Function GetCutOffPeriod() As Task
        _currentPayPeriod = Await PayrollTools.
                                GetCurrentlyWorkedOnPayPeriodByCurrentYear()

        UpdateCutOffLabel()
    End Function

    Private Sub UpdateCutOffLabel()
        PayPeriodLabel.Text = $"For the Period:
            {_currentPayPeriod.PayFromDate.ToString("MMMM d")} - {_currentPayPeriod.PayToDate.ToString("MMMM d")}, {_currentPayPeriod.PayToDate.Year}"
    End Sub

    Private Async Function FilterEmployeeGridView() As Task

        Dim searchValue = SearchEmployeeTextBox.Text.Trim

        If String.IsNullOrWhiteSpace(searchValue) Then
            EmployeesGridView.DataSource = _employees
        Else

            Dim employees = Await _employeeRepository.
               SearchSimpleLocal(_employees, SearchEmployeeTextBox.Text)

            EmployeesGridView.DataSource = employees

        End If

    End Function

    Private Sub EmployeesGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles EmployeesGridView.CellDoubleClick

        ShowEmployee()

    End Sub

    Private Sub ResetPayrollButton_Click(sender As Object, e As EventArgs) Handles ResetPayrollButton.Click

        ClearEmployeeForms()

    End Sub

    Private Async Sub PayPeriodLabel_Click(sender As Object, e As EventArgs) Handles PayPeriodLabel.Click
        Dim form As New selectPayPeriod()
        form.GeneratePayroll = False
        form.ShowDialog()

        If form.PayPeriod IsNot Nothing Then

            _currentPayPeriod = form.PayPeriod

            UpdateCutOffLabel()

            Await ShowEmployees()

            ClearEmployeeForms()
        End If
    End Sub

    Private Sub SearchEmployeeTextBox_TextChanged(sender As Object, e As EventArgs) Handles SearchEmployeeTextBox.TextChanged

        _textBoxDelayedAction.ProcessAsync(Async Function()
                                               Await FilterEmployeeGridView()

                                               Return True
                                           End Function)

    End Sub

End Class