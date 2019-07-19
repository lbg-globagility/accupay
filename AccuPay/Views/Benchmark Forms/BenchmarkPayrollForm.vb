Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Repository
Imports AccuPay.SetOvertimeForm
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

    Private _payrollResources As PayrollResources

    Private _employeeRate As BenchmarkPaystubRate

    Private _overtimes As List(Of OvertimeInput)

    Private Const MoneyFormat As String = "#,##0.0000"

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _employeeRepository = New EmployeeRepository
        _salaryRepository = New SalaryRepository
        _salaries = New List(Of Salary)
        _employees = New List(Of Employee)

        _overtimes = New List(Of OvertimeInput)
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

            _employeeRate = New BenchmarkPaystubRate(CType(EmployeesGridView.CurrentRow.DataBoundItem, Employee), _salaries)

            If _employeeRate.IsInvalid Then Return

            EmployeeNumberLabel.Text = _employeeRate.Employee.EmployeeNo
            EmployeeNameLabel.Text = _employeeRate.Employee.FullNameLastNameFirst.ToUpper

            TinTextBox.Text = _employeeRate.Employee.TinNo
            SssNumberTextBox.Text = _employeeRate.Employee.SssNo
            PagibigNumberTextBox.Text = _employeeRate.Employee.HdmfNo
            PhilhealthNumberTextBox.Text = _employeeRate.Employee.PhilHealthNo
            BasicPayTextBox.Text = _employeeRate.MonthlyRate.ToString(MoneyFormat)
            PerDayTextBox.Text = _employeeRate.DailyRate.ToString(MoneyFormat)
            PerHourTextBox.Text = _employeeRate.HourlyRate.ToString(MoneyFormat)
            AllowanceTextBox.Text = _employeeRate.AllowanceSalary.ToString(MoneyFormat)

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

        LoadPayrollResources()

    End Sub

    Private Sub LoadPayrollResources()
        'TODO: Add loading bar

        Dim paypRowID = _currentPayPeriod.RowID.Value
        Dim paypFrom = _currentPayPeriod.PayFromDate
        Dim paypTo = _currentPayPeriod.PayToDate

        Dim loadTask = Task.Factory.StartNew(
            Function()
                If paypFrom = Nothing And paypTo = Nothing Then
                    Return Nothing
                End If

                Dim resources = New PayrollResources(paypRowID, CDate(paypFrom), CDate(paypTo))
                Dim resourcesTask = resources.Load()
                resourcesTask.Wait()

                Return resources
            End Function,
            0
        )

        _payrollResources = loadTask.Result

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

    Private Function GetSelectedEmployee() As Employee

        If EmployeesGridView.CurrentRow Is Nothing Then

            MessageBoxHelper.ErrorMessage("No employee selected.")
            Return Nothing

        End If

        Dim employee = CType(EmployeesGridView.CurrentRow.DataBoundItem, Employee)

        If employee Is Nothing Then

            MessageBoxHelper.ErrorMessage("No employee selected.")
            Return Nothing

        End If

        Return employee

    End Function

    Private Function CreatePaystub(employee As Employee, generator As PayrollGeneration) As Paystub
        Dim paystub = New Paystub() With {
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User,
                    .LastUpdBy = z_User,
                    .EmployeeID = employee.RowID,
                    .PayPeriodID = _currentPayPeriod.RowID,
                    .PayFromdate = _currentPayPeriod.PayFromDate,
                    .PayToDate = _currentPayPeriod.PayToDate
                }

        paystub.Actual = New PaystubActual With {
            .OrganizationID = z_OrganizationID,
            .EmployeeID = employee.RowID,
            .PayPeriodID = _currentPayPeriod.RowID,
            .PayFromDate = _currentPayPeriod.PayFromDate,
            .PayToDate = _currentPayPeriod.PayToDate
        }

        paystub.EmployeeID = employee.RowID

        ComputeBasicHoursAndPay(paystub, employee)
        ComputeHours(paystub)

        generator.ComputePayroll(paystub)
        Return paystub
    End Function

    Private Sub ComputeBasicHoursAndPay(paystub As Paystub, employee As Employee)

        Dim cutOffsPerMonth As Integer = 2

        Dim workDaysThisCutOff = PayrollTools.
                GetWorkDaysPerMonth(employee.WorkDaysPerYear) / cutOffsPerMonth

        paystub.BasicHours = workDaysThisCutOff * PayrollTools.WorkHoursPerDay

        paystub.BasicPay = paystub.BasicHours * _employeeRate.HourlyRate

    End Sub

    Private Function ConvertDaysToHours(days As Decimal) As Decimal

        Return days / BenchmarkPaystubRate.WorkHoursPerDay

    End Function

    Private Sub ComputeHours(paystub As Paystub)

        'Dim payRate As New OvertimeRate
        'Dim actualSalaryPolicy As New ActualTimeEntryPolicy(New ListOfValueCollection)

        'Dim regularDays As Decimal = RegularDaysTextBox.Text.ToDecimal()

        Dim regularHours As Decimal = ConvertDaysToHours(RegularDaysTextBox.Text.ToDecimal)
        Dim overtimeHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)

        Dim nightDiffHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim nightDiffOvertimeHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim restDayHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim restDayOTHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim specialHolidayHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim specialHolidayOTHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim regularHolidayHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim regularHolidayOTHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim leaveHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim lateHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim undertimeHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)
        Dim absentHours As Decimal = ConvertDaysToHours(OvertimeTextBox.Text.ToDecimal)

        '_employeeRate.Compute(
        '    payRate,
        '    actualSalaryPolicy,
        ')
        Dim str = RegularDaysTextBox.Text.ToDecimal
        paystub.RegularHours = _employeeRate.RegularHours
        paystub.RegularPay = _employeeRate.RegularPay
        paystub.Actual.RegularPay = _employeeRate.ActualRegularPay

        paystub.OvertimeHours = _employeeRate.OvertimeHours
        paystub.OvertimePay = _employeeRate.OvertimePay
        paystub.Actual.OvertimePay = _employeeRate.ActualOvertimePay

        paystub.NightDiffHours = _employeeRate.NightDiffHours
        paystub.NightDiffPay = _employeeRate.NightDiffPay
        paystub.Actual.NightDiffPay = _employeeRate.ActualNightDiffPay

        paystub.NightDiffOvertimeHours = _employeeRate.NightDiffOvertimeHours
        paystub.NightDiffOvertimePay = _employeeRate.NightDiffOvertimePay
        paystub.Actual.NightDiffOTPay = _employeeRate.ActualNightDiffOTPay

        paystub.RestDayHours = _employeeRate.RestDayHours
        paystub.RestDayPay = _employeeRate.RestDayPay
        paystub.Actual.RestDayPay = _employeeRate.ActualRestDayPay

        paystub.RestDayOTHours = _employeeRate.RestDayOTHours
        paystub.RestDayOTPay = _employeeRate.RestDayOTPay
        paystub.Actual.RestDayOTPay = _employeeRate.ActualRestDayOTPay

        paystub.SpecialHolidayHours = _employeeRate.SpecialHolidayHours
        paystub.SpecialHolidayPay = _employeeRate.SpecialHolidayPay
        paystub.Actual.SpecialHolidayPay = _employeeRate.ActualSpecialHolidayPay

        paystub.SpecialHolidayOTHours = _employeeRate.SpecialHolidayOTHours
        paystub.SpecialHolidayOTPay = _employeeRate.SpecialHolidayOTPay
        paystub.Actual.SpecialHolidayOTPay = _employeeRate.ActualSpecialHolidayOTPay

        paystub.RegularHolidayHours = _employeeRate.RegularHolidayHours
        paystub.RegularHolidayPay = _employeeRate.RegularHolidayPay
        paystub.Actual.RegularHolidayPay = _employeeRate.ActualRegularHolidayPay

        paystub.RegularHolidayOTHours = _employeeRate.RegularHolidayOTHours
        paystub.RegularHolidayOTPay = _employeeRate.RegularHolidayOTPay
        paystub.Actual.RegularHolidayOTPay = _employeeRate.ActualRegularHolidayOTPay

        paystub.HolidayPay = _employeeRate.HolidayPay

        paystub.LeaveHours = _employeeRate.LeaveHours
        paystub.LeavePay = _employeeRate.LeavePay
        paystub.Actual.LeavePay = _employeeRate.ActualLeavePay

        paystub.LateHours = _employeeRate.LateHours
        paystub.LateDeduction = _employeeRate.LateDeduction
        paystub.Actual.LateDeduction = _employeeRate.ActualLateDeduction

        paystub.UndertimeHours = _employeeRate.UndertimeHours
        paystub.UndertimeDeduction = _employeeRate.UndertimeDeduction
        paystub.Actual.UndertimeDeduction = _employeeRate.ActualUndertimeDeduction

        paystub.AbsentHours = _employeeRate.AbsentHours
        paystub.AbsenceDeduction = _employeeRate.AbsenceDeduction
        paystub.Actual.AbsenceDeduction = _employeeRate.ActualAbsenceDeduction

    End Sub

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

    Private Sub ComputeSalaryButton_Click(sender As Object, e As EventArgs) Handles ComputeSalaryButton.Click

        Dim employee = GetSelectedEmployee()

        If employee Is Nothing Then

            MessageBoxHelper.ErrorMessage("No employee selected.")
            Return

        End If

        Dim generator = New PayrollGeneration(
                                employee,
                                _payrollResources
                            )

        Dim paystub As Paystub = CreatePaystub(employee, generator)

        BasicPaySummaryTextBox.Text = paystub.BasicPay.RoundToString()
        PhilhealthAmountTextBox.Text = paystub.PhilHealthEmployeeShare.RoundToString()
        SssAmountTextBox.Text = paystub.SssEmployeeShare.RoundToString()
        PagibigNumberTextBox.Text = paystub.HdmfEmployeeShare.RoundToString()
        WithholdingTaxTextBox.Text = paystub.WithholdingTax.RoundToString()
        PagibigLoanTextBox.Text = 0D.RoundToString()
        SssLoanTextBox.Text = 0D.RoundToString()

        NightDifferentialAmountTextBox.Text = paystub.NightDiffHours.RoundToString()
        ThirteenthMonthPayTextBox.Text = paystub.ThirteenthMonthPay?.Amount.RoundToString()
        LeaveBalanceTextBox.Text = 0D.RoundToString()

        GrossPayTextBox.Text = paystub.GrossPay.RoundToString()
        TotalLeaveTextBox.Text = paystub.LeaveHours.RoundToString()
        TotalDeductionTextBox.Text = paystub.NetDeductions.RoundToString()
        TotalOtherIncomeTextBox.Text = paystub.TotalAllowance.RoundToString()
        TotalOvertimeTextBox.Text = paystub.OvertimeHours.RoundToString()
        NetPayTextBox.Text = paystub.NetPay.RoundToString()

    End Sub

    Private Sub SetOvertimeButton_Click(sender As Object, e As EventArgs) Handles SetOvertimeButton.Click

        Dim form As New SetOvertimeForm(_employeeRate.HourlyRate, _overtimes)
        form.ShowDialog()

        _overtimes = form.Overtimes

        OvertimeTextBox.Text = _overtimes.Sum(Function(o) o.Hours).ToString

    End Sub

End Class