Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Benchmark
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Repository
Imports AccuPay.SetOvertimeForm
Imports AccuPay.SimplifiedEntities
Imports AccuPay.Utils
Imports log4net
Imports PayrollSys

Public Class BenchmarkPayrollForm

    Private Shared ReadOnly logger As ILog = LogManager.GetLogger("BenchmarkPayrollLogger")

    Private _employeeRepository As EmployeeRepository
    Private _salaryRepository As SalaryRepository
    Private _currentPayPeriod As IPayPeriod
    Private _salaries As List(Of Salary)
    Private _employees As List(Of Employee)
    Private _actualSalaryPolicy As ActualTimeEntryPolicy

    Private _overtimeRate As OvertimeRate

    Private _currentPaystub As Paystub

    Private _benchmarkPayrollHelper As BenchmarkPayrollHelper

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

    Private Async Sub BenchmarkPayrollForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        EmployeesGridView.AutoGenerateColumns = False

        _benchmarkPayrollHelper = Await BenchmarkPayrollHelper.GetInstance(logger)

        If _benchmarkPayrollHelper Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot initialize the payroll form properly. Please contact Globagility Inc.")

            Return

        End If

        Await GetCutOffPeriod()

        _salaries = Await _salaryRepository.
                        GetAllByCutOff(_currentPayPeriod.PayFromDate)

        Await ShowEmployees()

        ClearEmployeeForms()

        Await LoadPayrollResourcesAsync()

    End Sub

    Private Async Function LoadPayrollResourcesAsync() As Task

        _overtimeRate = Await OvertimeRateService.GetOvertimeRates()

        Using context As New PayrollContext

            Dim settings As New ListOfValueCollection(context.ListOfValues.ToList())

            _actualSalaryPolicy = New ActualTimeEntryPolicy(settings)

        End Using

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

    End Function

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

    Private Async Function ShowEmployee() As Task

        ClearEmployeeForms()

        If EmployeesGridView.CurrentRow IsNot Nothing Then

            Dim employeeId = CType(EmployeesGridView.CurrentRow.DataBoundItem, Employee)?.RowID

            If employeeId Is Nothing Then Return

            Dim employee = Await _employeeRepository.GetEmployeeWithDivisionAsync(employeeId)

            Await _benchmarkPayrollHelper.CleanEmployee(employeeId.Value)

            _employeeRate = New BenchmarkPaystubRate(employee, _salaries)

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

    End Function

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

        If _employeeRate.Employee Is Nothing OrElse Nullable.Equals(_employeeRate.Employee.RowID, employee.RowID) = False Then

            MessageBoxHelper.ErrorMessage("No employee selected.")
            Return Nothing

        End If

        Return _employeeRate.Employee

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

        ComputeBasicHoursAndBasicPay(paystub, employee)
        ComputeHoursAndPay(paystub)

        generator.ComputePayroll(paystub)
        Return paystub
    End Function

    Private Sub ComputeBasicHoursAndBasicPay(paystub As Paystub, employee As Employee)

        Dim cutOffsPerMonth As Integer = 2

        Dim workDaysThisCutOff = PayrollTools.
                GetWorkDaysPerMonth(employee.WorkDaysPerYear) / cutOffsPerMonth

        paystub.BasicHours = workDaysThisCutOff * PayrollTools.WorkHoursPerDay

        paystub.BasicPay = paystub.BasicHours * _employeeRate.HourlyRate

    End Sub

    Private Function ConvertDaysToHours(days As Decimal) As Decimal

        Return days * BenchmarkPaystubRate.WorkHoursPerDay

    End Function

    Private Function GetOvertime(overtimeDescription As String) As Decimal

        Dim overtime = _overtimes.
            Where(Function(o) o.OvertimeType.Name = overtimeDescription).
            FirstOrDefault

        If overtime Is Nothing Then
            Return 0
        Else
            Return overtime.Hours
        End If

    End Function

    Private Sub ComputeHoursAndPay(paystub As Paystub)

        Dim regularHours As Decimal = ConvertDaysToHours(RegularDaysTextBox.Text.ToDecimal)
        Dim overtimeHours As Decimal = GetOvertime(OvertimeRate.OvertimeDescription)

        Dim nightDiffHours As Decimal = GetOvertime(OvertimeRate.NightDifferentialDescription)
        Dim nightDiffOvertimeHours As Decimal = GetOvertime(OvertimeRate.NightDifferentialOvertimeDescription)
        Dim restDayHours As Decimal = GetOvertime(OvertimeRate.RestDayDescription)
        Dim restDayOTHours As Decimal = GetOvertime(OvertimeRate.RestDayOvertimeDescription)
        Dim specialHolidayHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayDescription)
        Dim specialHolidayOTHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayOvertimeDescription)
        Dim regularHolidayHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayDescription)
        Dim regularHolidayOTHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayOvertimeDescription)

        Dim leaveHours As Decimal = 0
        Dim lateHours As Decimal = 0
        Dim undertimeHours As Decimal = 0
        Dim absentHours As Decimal = 0

        Dim restDayNightDiffHours As Decimal = GetOvertime(OvertimeRate.RestDayNightDifferentialDescription)
        Dim restDayNightDiffOTHours As Decimal = GetOvertime(OvertimeRate.RestDayNightDifferentialOvertimeDescription)
        Dim specialHolidayNightDiffHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayNightDifferentialDescription)
        Dim specialHolidayNightDiffOTHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayNightDifferentialOvertimeDescription)
        Dim specialHolidayRestDayHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayRestDayDescription)
        Dim specialHolidayRestDayOTHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayRestDayOvertimeDescription)
        Dim specialHolidayRestDayNightDiffHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayRestDayNightDifferentialDescription)
        Dim specialHolidayRestDayNightDiffOTHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayRestDayNightDifferentialOvertimeDescription)
        Dim regularHolidayNightDiffHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayNightDifferentialDescription)
        Dim regularHolidayNightDiffOTHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayNightDifferentialOvertimeDescription)
        Dim regularHolidayRestDayHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayRestDayDescription)
        Dim regularHolidayRestDayOTHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayRestDayOvertimeDescription)
        Dim regularHolidayRestDayNightDiffHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayRestDayNightDifferentialDescription)
        Dim regularHolidayRestDayNightDiffOTHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayRestDayNightDifferentialOvertimeDescription)

        _employeeRate.Compute(
            _overtimeRate,
            employeeEntitledForNightDifferentialPay:=_employeeRate.Employee.CalcNightDiff,
            employeeEntitledForSpecialHolidayPay:=_employeeRate.Employee.CalcSpecialHoliday,
            employeeEntitledForRegularHolidayPay:=_employeeRate.Employee.CalcHoliday,
            employeeEntitledForRestDayPay:=_employeeRate.Employee.CalcRestDay,
            allowanceForOvertimePolicy:=_actualSalaryPolicy.AllowanceForOvertime,
            allowanceForNightDiffPolicy:=_actualSalaryPolicy.AllowanceForNightDiff,
            allowanceForNightDiffOTPolicy:=_actualSalaryPolicy.AllowanceForNightDiffOT,
            allowanceForHolidayPolicy:=_actualSalaryPolicy.AllowanceForHoliday,
            allowanceForRestDayPolicy:=_actualSalaryPolicy.AllowanceForRestDay,
            allowanceForRestDayOTPolicy:=_actualSalaryPolicy.AllowanceForRestDayOT,
            regularHours:=regularHours,
            overtimeHours:=overtimeHours,
            nightDiffHours:=nightDiffHours,
            nightDiffOvertimeHours:=nightDiffOvertimeHours,
            restDayHours:=restDayHours,
            restDayOTHours:=restDayOTHours,
            specialHolidayHours:=specialHolidayHours,
            specialHolidayOTHours:=specialHolidayOTHours,
            regularHolidayHours:=regularHolidayHours,
            regularHolidayOTHours:=regularHolidayOTHours,
            leaveHours:=leaveHours,
            lateHours:=lateHours,
            undertimeHours:=undertimeHours,
            absentHours:=absentHours,
            restDayNightDiffHours:=restDayNightDiffHours,
            restDayNightDiffOTHours:=restDayNightDiffOTHours,
            specialHolidayNightDiffHours:=specialHolidayNightDiffHours,
            specialHolidayNightDiffOTHours:=specialHolidayNightDiffOTHours,
            specialHolidayRestDayHours:=specialHolidayRestDayHours,
            specialHolidayRestDayOTHours:=specialHolidayRestDayOTHours,
            specialHolidayRestDayNightDiffHours:=specialHolidayRestDayNightDiffHours,
            specialHolidayRestDayNightDiffOTHours:=specialHolidayRestDayNightDiffOTHours,
            regularHolidayNightDiffHours:=regularHolidayNightDiffHours,
            regularHolidayNightDiffOTHours:=regularHolidayNightDiffOTHours,
            regularHolidayRestDayHours:=regularHolidayRestDayHours,
            regularHolidayRestDayOTHours:=regularHolidayRestDayOTHours,
            regularHolidayRestDayNightDiffHours:=regularHolidayRestDayNightDiffHours,
            regularHolidayRestDayNightDiffOTHours:=regularHolidayRestDayNightDiffOTHours
        )

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
        paystub.Actual.NightDiffOvertimePay = _employeeRate.ActualNightDiffOvertimePay

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

        'new
        paystub.RestDayNightDiffHours = _employeeRate.RestDayNightDiffHours
        paystub.RestDayNightDiffPay = _employeeRate.RestDayNightDiffPay
        paystub.Actual.RestDayNightDiffPay = _employeeRate.ActualRestDayNightDiffPay

        paystub.RestDayNightDiffOTHours = _employeeRate.RestDayNightDiffOTHours
        paystub.RestDayNightDiffOTPay = _employeeRate.RestDayNightDiffOTPay
        paystub.Actual.RestDayNightDiffOTPay = _employeeRate.ActualRestDayNightDiffOTPay

        paystub.SpecialHolidayNightDiffHours = _employeeRate.SpecialHolidayNightDiffHours
        paystub.SpecialHolidayNightDiffPay = _employeeRate.SpecialHolidayNightDiffPay
        paystub.Actual.SpecialHolidayNightDiffPay = _employeeRate.ActualSpecialHolidayNightDiffPay

        paystub.SpecialHolidayNightDiffOTHours = _employeeRate.SpecialHolidayNightDiffOTHours
        paystub.SpecialHolidayNightDiffOTPay = _employeeRate.SpecialHolidayNightDiffOTPay
        paystub.Actual.SpecialHolidayNightDiffOTPay = _employeeRate.ActualSpecialHolidayNightDiffOTPay

        paystub.SpecialHolidayRestDayHours = _employeeRate.SpecialHolidayRestDayHours
        paystub.SpecialHolidayRestDayPay = _employeeRate.SpecialHolidayRestDayPay
        paystub.Actual.SpecialHolidayRestDayPay = _employeeRate.ActualSpecialHolidayRestDayPay

        paystub.SpecialHolidayRestDayOTHours = _employeeRate.SpecialHolidayRestDayOTHours
        paystub.SpecialHolidayRestDayOTPay = _employeeRate.SpecialHolidayRestDayOTPay
        paystub.Actual.SpecialHolidayRestDayOTPay = _employeeRate.ActualSpecialHolidayRestDayOTPay

        paystub.SpecialHolidayRestDayNightDiffHours = _employeeRate.SpecialHolidayRestDayNightDiffHours
        paystub.SpecialHolidayRestDayNightDiffPay = _employeeRate.SpecialHolidayRestDayNightDiffPay
        paystub.Actual.SpecialHolidayRestDayNightDiffPay = _employeeRate.ActualSpecialHolidayRestDayNightDiffPay

        paystub.SpecialHolidayRestDayNightDiffOTHours = _employeeRate.SpecialHolidayRestDayNightDiffOTHours
        paystub.SpecialHolidayRestDayNightDiffOTPay = _employeeRate.SpecialHolidayRestDayNightDiffOTPay
        paystub.Actual.SpecialHolidayRestDayNightDiffOTPay = _employeeRate.ActualSpecialHolidayRestDayNightDiffOTPay

        paystub.RegularHolidayNightDiffHours = _employeeRate.RegularHolidayNightDiffHours
        paystub.RegularHolidayNightDiffPay = _employeeRate.RegularHolidayNightDiffPay
        paystub.Actual.RegularHolidayNightDiffPay = _employeeRate.ActualRegularHolidayNightDiffPay

        paystub.RegularHolidayNightDiffOTHours = _employeeRate.RegularHolidayNightDiffOTHours
        paystub.RegularHolidayNightDiffOTPay = _employeeRate.RegularHolidayNightDiffOTPay
        paystub.Actual.RegularHolidayNightDiffOTPay = _employeeRate.ActualRegularHolidayNightDiffOTPay

        paystub.RegularHolidayRestDayHours = _employeeRate.RegularHolidayRestDayHours
        paystub.RegularHolidayRestDayPay = _employeeRate.RegularHolidayRestDayPay
        paystub.Actual.RegularHolidayRestDayPay = _employeeRate.ActualRegularHolidayRestDayPay

        paystub.RegularHolidayRestDayOTHours = _employeeRate.RegularHolidayRestDayOTHours
        paystub.RegularHolidayRestDayOTPay = _employeeRate.RegularHolidayRestDayOTPay
        paystub.Actual.RegularHolidayRestDayOTPay = _employeeRate.ActualRegularHolidayRestDayOTPay

        paystub.RegularHolidayRestDayNightDiffHours = _employeeRate.RegularHolidayRestDayNightDiffHours
        paystub.RegularHolidayRestDayNightDiffPay = _employeeRate.RegularHolidayRestDayNightDiffPay
        paystub.Actual.RegularHolidayRestDayNightDiffPay = _employeeRate.ActualRegularHolidayRestDayNightDiffPay

        paystub.RegularHolidayRestDayNightDiffOTHours = _employeeRate.RegularHolidayRestDayNightDiffOTHours
        paystub.RegularHolidayRestDayNightDiffOTPay = _employeeRate.RegularHolidayRestDayNightDiffOTPay
        paystub.Actual.RegularHolidayRestDayNightDiffOTPay = _employeeRate.ActualRegularHolidayRestDayNightDiffOTPay

    End Sub

    Private Async Sub EmployeesGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles EmployeesGridView.CellDoubleClick

        Await ShowEmployee()

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

        _currentPaystub = CreatePaystub(employee, generator)

        BasicPaySummaryTextBox.Text = _currentPaystub.BasicPay.RoundToString()
        PhilhealthAmountTextBox.Text = _currentPaystub.PhilHealthEmployeeShare.RoundToString()
        SssAmountTextBox.Text = _currentPaystub.SssEmployeeShare.RoundToString()
        PagibigAmountTextBox.Text = _currentPaystub.HdmfEmployeeShare.RoundToString()
        WithholdingTaxTextBox.Text = _currentPaystub.WithholdingTax.RoundToString()
        PagibigLoanTextBox.Text = 0D.RoundToString()
        SssLoanTextBox.Text = 0D.RoundToString()

        NightDifferentialAmountTextBox.Text = _currentPaystub.NightDiffHours.RoundToString()
        ThirteenthMonthPayTextBox.Text = _currentPaystub.ThirteenthMonthPay?.Amount.RoundToString()
        LeaveBalanceTextBox.Text = 0D.RoundToString()

        GrossPayTextBox.Text = _currentPaystub.GrossPay.RoundToString()
        TotalLeaveTextBox.Text = _currentPaystub.LeaveHours.RoundToString()
        TotalDeductionTextBox.Text = _currentPaystub.NetDeductions.RoundToString()
        TotalOtherIncomeTextBox.Text = _currentPaystub.TotalAllowance.RoundToString()
        TotalOvertimeTextBox.Text = GetTotalOvertimePay(_currentPaystub).RoundToString()
        NetPayTextBox.Text = _currentPaystub.NetPay.RoundToString()

        SummaryGroupBox.Enabled = True
    End Sub

    Private Function GetTotalOvertimePay(paystub As Paystub) As Decimal

        Return paystub.OvertimePay +
                paystub.NightDiffPay +
                paystub.NightDiffOvertimePay +
                paystub.RestDayPay +
                paystub.RestDayOTPay +
                paystub.RestDayNightDiffPay +
                paystub.RestDayNightDiffOTPay +
                paystub.SpecialHolidayPay +
                paystub.SpecialHolidayOTPay +
                paystub.SpecialHolidayNightDiffPay +
                paystub.SpecialHolidayNightDiffOTPay +
                paystub.SpecialHolidayRestDayPay +
                paystub.SpecialHolidayRestDayOTPay +
                paystub.SpecialHolidayRestDayNightDiffPay +
                paystub.SpecialHolidayRestDayNightDiffOTPay +
                paystub.RegularHolidayPay +
                paystub.RegularHolidayOTPay +
                paystub.RegularHolidayNightDiffPay +
                paystub.RegularHolidayNightDiffOTPay +
                paystub.RegularHolidayRestDayPay +
                paystub.RegularHolidayRestDayOTPay +
                paystub.RegularHolidayRestDayNightDiffPay +
                paystub.RegularHolidayRestDayNightDiffOTPay

    End Function

    Private Sub SetOvertimeButton_Click(sender As Object, e As EventArgs) Handles SetOvertimeButton.Click

        Dim form As New SetOvertimeForm(_employeeRate.HourlyRate, _overtimeRate.OvertimeRateList, _overtimes)
        form.ShowDialog()

        _overtimes = form.Overtimes

        OvertimeTextBox.Text = _overtimes.Sum(Function(o) o.Hours).ToString

    End Sub

    Private Sub SavePayrollButton_Click(sender As Object, e As EventArgs) Handles SavePayrollButton.Click

    End Sub

End Class