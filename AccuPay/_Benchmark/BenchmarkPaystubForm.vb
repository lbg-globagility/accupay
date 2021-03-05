Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Benchmark
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Entities.Paystub
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.ValueObjects
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class BenchmarkPaystubForm

    Private _currentPayPeriod As IPayPeriod

    Private _salaries As List(Of Salary)

    Private _employees As List(Of Employee)

    Private _textBoxDelayedAction As New DelayedAction(Of Boolean)

    Private _pagibigLoanId As Integer?

    Private _sssLoanId As Integer?

    Private _overtimeRate As OvertimeRate

    Private ReadOnly _employeeRepository As IEmployeeRepository

    Private ReadOnly _paystubRepository As IPaystubRepository

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Private ReadOnly _productRepository As IProductRepository

    Private ReadOnly _salaryRepository As ISalaryRepository

    Private ReadOnly _overtimeRateService As IOvertimeRateService

    Sub New()

        InitializeComponent()

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _salaryRepository = MainServiceProvider.GetRequiredService(Of ISalaryRepository)

        _paystubRepository = MainServiceProvider.GetRequiredService(Of IPaystubRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)

        _overtimeRateService = MainServiceProvider.GetRequiredService(Of IOvertimeRateService)

        _salaries = New List(Of Salary)
        _employees = New List(Of Employee)

        '_overtimes = New List(Of OvertimeInput)

        '_selectedDeductions = New List(Of AdjustmentInput)
        '_selectedIncomes = New List(Of AdjustmentInput)

        '_loanScheduleRepository = New LoanScheduleRepository

    End Sub

    Private Sub BenchmarkPaystubForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        PayrollForm.listPayrollForm.Remove(Me.Name)
    End Sub

    Private Async Sub BenchmarkPaystubForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim govermentLoans = Await _productRepository.GetGovernmentLoanTypesAsync(z_OrganizationID)

        _pagibigLoanId = govermentLoans.FirstOrDefault(Function(l) l.IsPagibigLoan)?.RowID
        _sssLoanId = govermentLoans.FirstOrDefault(Function(l) l.IsSssLoan)?.RowID

        If _pagibigLoanId Is Nothing OrElse _sssLoanId Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot initialize the payroll form properly. Please contact Globagility Inc.")
            Return

        End If

        Await RefreshForm()

    End Sub

    Private Async Function RefreshForm(Optional refreshPayPeriod As Boolean = True) As Task
        InitializeControls()

        If refreshPayPeriod Then

            Await GetCutOffPeriod()

        End If

        If _currentPayPeriod Is Nothing Then
            MessageBoxHelper.ErrorMessage("Cannot identify the selected pay period. Please close then reopen this form and try again.")
            Return
        End If

        _overtimeRate = Await _overtimeRateService.GetOvertimeRates()

        Await LoadPayrollDetails()
    End Function

    Private Async Function LoadPayrollDetails() As Task
        _salaries = (Await _salaryRepository.
            GetByCutOffAsync(z_OrganizationID, _currentPayPeriod.PayToDate)).
            ToList()

        Await ShowEmployees()

        ClearEmployeeForms()
    End Function

    Private Sub InitializeControls()
        OvertimeGridView.AutoGenerateColumns = False
        EmployeesGridView.AutoGenerateColumns = False
        DeductionsGridView.AutoGenerateColumns = False
        OtherIncomeGridView.AutoGenerateColumns = False

        TotalDeductionTextBox.Clear()
        TotalOtherIncomeTextBox.Clear()

    End Sub

    Private Async Function GetCutOffPeriod() As Task
        _currentPayPeriod = Await _payPeriodRepository.GetOrCreateCurrentPayPeriodAsync(
            organizationId:=z_OrganizationID,
            currentUserId:=z_User)

        UpdateCutOffLabel()
    End Function

    Private Sub UpdateCutOffLabel()
        PayPeriodLabel.Text = $"For the Period:
            {_currentPayPeriod?.PayFromDate:MMMM d} - {_currentPayPeriod?.PayToDate:MMMM d}, {_currentPayPeriod?.PayToDate.Year}"
    End Sub

    Private Async Function ShowEmployees() As Task

        Dim payPeriodId = _currentPayPeriod?.RowID

        If payPeriodId IsNot Nothing Then

            _employees = (Await _employeeRepository.GetAllWithPayrollAsync(
                    payPeriodId:=_currentPayPeriod.RowID.Value,
                    organizationId:=z_OrganizationID)).
                OrderBy(Function(e) e.FullNameLastNameFirst).
                ToList()
        Else
            _employees = New List(Of Employee)
        End If

        Await FilterEmployeeGridView()

    End Function

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

    Private Sub ClearEmployeeForms()

        EmployeeNumberLabel.Text = "000-0000"
        EmployeeNameLabel.Text = "EMPLOYEE NAME"

        OvertimeGridView.DataSource = New List(Of OvertimeInput)
        DeductionsGridView.DataSource = New List(Of Adjustment)
        OtherIncomeGridView.DataSource = New List(Of Adjustment)

        RegularDaysTextBox.ResetText()
        UndeclaredPerDayTextBox.ResetText()
        UndeclaredGrossPayTextBox.ResetText()
        HolidayAndLeaveDaysTextBox.ResetText()
        HolidayAndLeaveAmountTextBox.ResetText()

        BasicPaySummaryTextBox.ResetText()
        PhilhealthAmountTextBox.ResetText()
        SssAmountTextBox.ResetText()
        WithholdingTaxTextBox.ResetText()
        PagibigLoanTextBox.ResetText()
        SssLoanTextBox.ResetText()

        EcolaAmountTextBox.ResetText()
        ThirteenthMonthPayTextBox.ResetText()
        LeaveBalanceTextBox.ResetText()

        GrossPayTextBox.ResetText()
        TotalLeaveTextBox.ResetText()
        TotalDeductionTextBox.ResetText()
        TotalOtherIncomeTextBox.ResetText()
        TotalOvertimeTextBox.ResetText()
        NetPayTextBox.ResetText()

        TotalOtherIncomeLabel.Text = "00.00"
        TotalDeductionsLabel.Text = "00.00"
        DeletePaystubButton.Enabled = False

    End Sub

    Private Async Function ShowEmployee() As Task

        ClearEmployeeForms()

        If EmployeesGridView.CurrentRow IsNot Nothing Then

            If CheckIfGridViewHasValue(EmployeesGridView) = False Then Return

            Dim employee = CType(EmployeesGridView.CurrentRow.DataBoundItem, Employee)

            If employee?.RowID Is Nothing Then Return

            Dim employeeId = employee.RowID.Value

            EmployeeNumberLabel.Text = employee.EmployeeNo
            EmployeeNameLabel.Text = employee.FullNameLastNameFirst.ToUpper

            'paystub
            Dim payStub = Await GetPayStub(employeeId)
            Dim payStubId = payStub?.RowID

            If payStubId Is Nothing Then

                MessageBoxHelper.ErrorMessage("This employee has no paystub for this cut off. It may have been deleted earlier. Please refresh the form.")
                Return

            End If

            Dim employeeRate = GetEmployeeRate(employee)
            If employeeRate Is Nothing Then Return

            PopulateOvertimeGridView(payStub, employeeRate)
            PopulateAdjustmentsGridView(payStub) 'database
            ShowUndeclaredSalaryBreakdown(payStub, employeeRate)

            'Show summary
            payStub.Ecola = payStub.AllowanceItems.Sum(Function(a) a.Amount)
            Await ShowSummaryData(employee, payStub) 'database

            DeletePaystubButton.Enabled = True 'move this at the bottom after you finish the codes below

        End If

    End Function

    Private Async Function ShowSummaryData(employee As Employee, payStub As Paystub) As Task
        'loans
        Dim loanAmounts = GetGovernmentLoanAmounts(payStub)
        Dim pagIbigLoan = loanAmounts.Item1
        Dim sssLoan = loanAmounts.Item2

        TotalDeductionsLabel.Text = "Php " & Math.Abs(payStub.TotalDeductionAdjustments).RoundToString()
        TotalOtherIncomeLabel.Text = "Php " & payStub.TotalAdditionAdjustments.RoundToString()

        BasicPaySummaryTextBox.Text = payStub.BasicPay.RoundToString()
        PhilhealthAmountTextBox.Text = payStub.PhilHealthEmployeeShare.RoundToString()
        SssAmountTextBox.Text = payStub.SssEmployeeShare.RoundToString()
        PagibigAmountTextBox.Text = payStub.HdmfEmployeeShare.RoundToString()
        WithholdingTaxTextBox.Text = payStub.WithholdingTax.RoundToString()
        PagibigLoanTextBox.Text = If(pagIbigLoan, 0).RoundToString()
        SssLoanTextBox.Text = If(sssLoan, 0).RoundToString()

        EcolaAmountTextBox.Text = payStub.Ecola.RoundToString()
        ThirteenthMonthPayTextBox.Text = payStub.ThirteenthMonthPay?.Amount.RoundToString()

        Dim employeeLeave = Await _employeeRepository.GetVacationLeaveBalance(employee.RowID.Value)
        LeaveBalanceTextBox.Text = BenchmarkPayrollHelper.ConvertHoursToDays(employeeLeave).ToString

        GrossPayTextBox.Text = payStub.GrossPay.RoundToString()
        TotalLeaveTextBox.Text = payStub.LeavePay.RoundToString()

        TotalDeductionTextBox.Text = (payStub.NetDeductions +
                                    Math.Abs(payStub.TotalDeductionAdjustments)).RoundToString()

        TotalOtherIncomeTextBox.Text = payStub.TotalAdditionAdjustments.RoundToString()
        TotalOvertimeTextBox.Text = BenchmarkPayrollHelper.GetTotalOvertimePay(payStub).RoundToString()
        NetPayTextBox.Text = payStub.NetPay.RoundToString()
    End Function

    Private Sub ShowUndeclaredSalaryBreakdown(payStub As Paystub, employeeRate As BenchmarkPaystubRate)
        Dim rawUndeclaredRate = employeeRate.ActualDailyRate - employeeRate.DailyRate
        Dim regularDays = BenchmarkPayrollHelper.ConvertHoursToDays(payStub.RegularHours)
        Dim holidayAndLeaveDays = BenchmarkPayrollHelper.ConvertHoursToDays((payStub.TotalWorkedHoursWithoutOvertimeAndLeave - payStub.RegularHours) + payStub.LeaveHours)

        Dim undeclaredRegularPay = rawUndeclaredRate * regularDays
        Dim undeclaredHolidayAndLeavePay = ComputeHolidayAndLeavePays(payStub, rawUndeclaredRate)

        RegularDaysTextBox.Text = regularDays.RoundToString
        UndeclaredPerDayTextBox.Text = rawUndeclaredRate.RoundToString
        UndeclaredGrossPayTextBox.Text = undeclaredRegularPay.RoundToString
        HolidayAndLeaveDaysTextBox.Text = holidayAndLeaveDays.RoundToString
        HolidayAndLeaveAmountTextBox.Text = undeclaredHolidayAndLeavePay.RoundToString
        TotalUnderclaredTextBox.Text = (undeclaredRegularPay + undeclaredHolidayAndLeavePay).RoundToString
    End Sub

    Private Function ComputeHolidayAndLeavePays(payStub As Paystub, rawUndeclaredRate As Decimal) As Decimal

        If rawUndeclaredRate = 0 Then Return 0

        Dim rawUndeclaredRatePerHour = rawUndeclaredRate / 8

        Dim total As Decimal = 0

        If payStub.LeaveHours <> 0 Then

            total += payStub.LeaveHours * rawUndeclaredRatePerHour

        End If

        If payStub.RestDayHours <> 0 Then

            total += payStub.RestDayHours * (rawUndeclaredRatePerHour * _overtimeRate.RestDay.CurrentRate)

        End If

        If payStub.SpecialHolidayHours <> 0 Then

            total += payStub.SpecialHolidayHours * (rawUndeclaredRatePerHour * _overtimeRate.SpecialHoliday.CurrentRate)

        End If

        If payStub.SpecialHolidayRestDayHours <> 0 Then

            total += payStub.SpecialHolidayRestDayHours * (rawUndeclaredRatePerHour * _overtimeRate.SpecialHolidayRestDay.CurrentRate)

        End If

        If payStub.RegularHolidayHours <> 0 Then

            total += payStub.RegularHolidayHours * (rawUndeclaredRatePerHour * _overtimeRate.RegularHoliday.CurrentRate)

        End If

        If payStub.RegularHolidayRestDayHours <> 0 Then

            total += payStub.RegularHolidayRestDayHours * (rawUndeclaredRatePerHour * _overtimeRate.RegularHolidayRestDay.CurrentRate)

        End If

        Return total

    End Function

    Private Sub PopulateAdjustmentsGridView(paystub As Paystub)
        If paystub?.Adjustments Is Nothing Then

            DeductionsGridView.DataSource = Nothing
            OtherIncomeGridView.DataSource = Nothing

            Return
        End If

        Dim otherIncomes = paystub.Adjustments.
            Where(Function(p) p.Amount > 0).
            Select(Function(p) New Adjustments With {
                    .Amount = p.Amount,
                    .Code = p.Product.Comments,
                    .Description = p.Product.PartNo
                }).
            ToList()

        Dim deductions = paystub.Adjustments.
            Where(Function(p) p.Amount < 0).
            Select(Function(p) New Adjustments With {
                    .Amount = Math.Abs(p.Amount),
                    .Code = p.Product.Comments,
                    .Description = p.Product.PartNo
                }).
            ToList()

        DeductionsGridView.DataSource = deductions
        OtherIncomeGridView.DataSource = otherIncomes
    End Sub

    Private Sub PopulateOvertimeGridView(payStub As Paystub, employeeRate As BenchmarkPaystubRate)
        Dim overtimeInputs = GetOvertimeInputs(payStub, employeeRate)
        If overtimeInputs Is Nothing Then Return
        OvertimeGridView.DataSource = overtimeInputs
    End Sub

    Private Function GetEmployeeRate(employee As Employee) As BenchmarkPaystubRate

        Dim salary = _salaries.
            Where(Function(s) Nullable.Equals(s.EmployeeID, employee?.RowID)).
            FirstOrDefault()

        If salary Is Nothing Then

            MessageBoxHelper.Warning("Selected employee currently has no active salary. Please add one before computing the employees payroll.")
            Return Nothing
        End If

        Dim employeeRate = New BenchmarkPaystubRate(employee, salary)

        If employeeRate.IsInvalid Then

            MessageBoxHelper.Warning("Cannot retrieve details of the employee. Please try again and refresh the form or contact Globagility Inc.")
            Return Nothing
        End If

        Return employeeRate

    End Function

    Private Function GetOvertimeInputs(paystub As Paystub, employeeRate As BenchmarkPaystubRate) As List(Of OvertimeInput)

        Dim overtimeInputs As New List(Of OvertimeInput)

        Dim payPerHour = employeeRate.HourlyRate

        If paystub.OvertimePay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.Overtime,
                input:=paystub.OvertimeHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.NightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.NightDifferential,
                input:=paystub.NightDiffHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.NightDiffOvertimePay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.NightDifferentialOvertime,
                input:=paystub.NightDiffOvertimeHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RestDayPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RestDay,
                input:=paystub.RestDayHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=False))
        End If

        If paystub.RestDayOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RestDayOvertime,
                input:=paystub.RestDayOTHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RestDayNightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RestDayNightDifferential,
                input:=paystub.RestDayNightDiffHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RestDayNightDiffOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RestDayNightDifferentialOvertime,
                input:=paystub.RestDayNightDiffOTHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.SpecialHolidayPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.SpecialHoliday,
                input:=paystub.SpecialHolidayHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.SpecialHolidayOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.SpecialHolidayOvertime,
                input:=paystub.SpecialHolidayOTHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.SpecialHolidayNightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.SpecialHolidayNightDifferential,
                input:=paystub.SpecialHolidayNightDiffHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.SpecialHolidayNightDiffOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.SpecialHolidayNightDifferentialOvertime,
                input:=paystub.SpecialHolidayNightDiffOTHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.SpecialHolidayRestDayPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.SpecialHolidayRestDay,
                input:=paystub.SpecialHolidayRestDayHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.SpecialHolidayRestDayOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.SpecialHolidayRestDayOvertime,
                input:=paystub.SpecialHolidayRestDayOTHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.SpecialHolidayRestDayNightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.SpecialHolidayRestDayNightDifferential,
                input:=paystub.SpecialHolidayRestDayNightDiffHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.SpecialHolidayRestDayNightDiffOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.SpecialHolidayRestDayNightDifferentialOvertime,
                input:=paystub.SpecialHolidayRestDayNightDiffOTHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RegularHolidayPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RegularHoliday,
                input:=paystub.RegularHolidayHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RegularHolidayOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RegularHolidayOvertime,
                input:=paystub.RegularHolidayOTHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RegularHolidayNightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RegularHolidayNightDifferential,
                input:=paystub.RegularHolidayNightDiffHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RegularHolidayNightDiffOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RegularHolidayNightDifferentialOvertime,
                input:=paystub.RegularHolidayNightDiffOTHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RegularHolidayRestDayPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RegularHolidayRestDay,
                input:=paystub.RegularHolidayRestDayHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RegularHolidayRestDayOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RegularHolidayRestDayOvertime,
                input:=paystub.RegularHolidayRestDayOTHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RegularHolidayRestDayNightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RegularHolidayRestDayNightDifferential,
                input:=paystub.RegularHolidayRestDayNightDiffHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))
        End If

        If paystub.RegularHolidayRestDayNightDiffOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                overtimeType:=_overtimeRate.RegularHolidayRestDayNightDifferentialOvertime,
                input:=paystub.RegularHolidayRestDayNightDiffOTHours,
                isDay:=False,
                payperHour:=payPerHour,
                isHolidayInclusive:=employeeRate.Employee.IsPremiumInclusive))

        End If

        Return overtimeInputs

    End Function

    Private Function GetGovernmentLoanAmounts(payStub As Paystub) As (Decimal?, Decimal?)

        Dim loanRecords = payStub.LoanTransactions

        Dim pagIbigLoan = loanRecords.
            FirstOrDefault(Function(l) l.Loan.LoanTypeID.Value = _pagibigLoanId.Value)?.DeductionAmount
        Dim sssLoan = loanRecords.
            FirstOrDefault(Function(l) l.Loan.LoanTypeID.Value = _sssLoanId.Value)?.DeductionAmount

        Return (pagIbigLoan, sssLoan)

    End Function

    Private Async Function GetPayStub(employeeId As Integer) As Task(Of Paystub)

        If _currentPayPeriod?.RowID Is Nothing Then Return Nothing

        Return Await _paystubRepository.
            GetByCompositeKeyFullPaystubAsync(
                New EmployeeCompositeKey(
                        employeeId:=employeeId,
                        payPeriodId:=_currentPayPeriod.RowID.Value
                ))
    End Function

    Private Function CheckIfGridViewHasValue(gridView As DataGridView) As Boolean
        Return gridView.Rows.
            Cast(Of DataGridViewRow).
            Any(Function(r) r.Cells.Cast(Of DataGridViewCell).
                Any(Function(c) c.Value IsNot Nothing))
    End Function

    Private Function GetSelectedEmployee() As Employee

        If EmployeesGridView.CurrentRow IsNot Nothing Then

            If CheckIfGridViewHasValue(EmployeesGridView) = False Then Return Nothing

            Return CType(EmployeesGridView.CurrentRow.DataBoundItem, Employee)

        End If

        Return Nothing

    End Function

    Private Async Sub EmployeesGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles EmployeesGridView.CellDoubleClick

        Await ShowEmployee()

    End Sub

    Private Async Sub DeletePaystubButton_Click(sender As Object, e As EventArgs) Handles DeletePaystubButton.Click

        Dim employee = GetSelectedEmployee()
        Dim employeeId = employee?.RowID

        If employeeId Is Nothing Then

            MessageBoxHelper.ErrorMessage("No employee selected!")
            Return
        End If

        If _currentPayPeriod?.RowID Is Nothing Then

            MessageBoxHelper.ErrorMessage("Pay period not set up properly. Please refresh the form.")
            Return
        End If

        Dim confirmMessage = $"Are you sure you want to delete paystub of '{employee.FullNameLastNameFirst} [{employee.EmployeeNo}]'?"
        Const MessageTitle As String = "Delete Paystub"

        If MessageBoxHelper.Confirm(Of Boolean) _
               (confirmMessage, MessageTitle, messageBoxIcon:=MessageBoxIcon.Warning) = False Then Return

        Await FunctionUtils.TryCatchFunctionAsync("",
            Async Function()
                Dim paystubDataService = MainServiceProvider.GetRequiredService(Of IPaystubDataService)
                Await paystubDataService.DeleteAsync(
                    New EmployeeCompositeKey(
                            employeeId:=employeeId.Value,
                            payPeriodId:=_currentPayPeriod.RowID.Value),
                    currentlyLoggedInUserId:=z_User,
                    organizationId:=z_OrganizationID)

                Await RefreshForm(refreshPayPeriod:=False)

                MessageBoxHelper.Information("Done! " & vbNewLine + vbNewLine & $"Go to Payroll transactions to process the payslip of {employee.FullNameLastNameFirst} [{employee.EmployeeNo}] again.", MessageTitle)
            End Function)

    End Sub

    Private Sub SearchEmployeeTextBox_TextChanged(sender As Object, e As EventArgs) Handles SearchEmployeeTextBox.TextChanged

        _textBoxDelayedAction.ProcessAsync(
            Async Function()
                Await FilterEmployeeGridView()

                Return True
            End Function)

    End Sub

    Private Async Sub RefreshFormButton_Click(sender As Object, e As EventArgs) Handles RefreshFormButton.Click

        Await RefreshForm(refreshPayPeriod:=False)

    End Sub

    Private Async Sub PayPeriodLabel_Click(sender As Object, e As EventArgs) Handles PayPeriodLabel.Click
        Dim form As New SelectPayPeriodDialog()

        If form.ShowDialog() <> DialogResult.OK OrElse form.SelectedPayPeriod Is Nothing Then Return

        _currentPayPeriod = form.SelectedPayPeriod

        If _currentPayPeriod Is Nothing Then
            MessageBoxHelper.ErrorMessage("Cannot identify the selected pay period. Please close then reopen this form and try again.")
            Return
        End If

        UpdateCutOffLabel()

        Await LoadPayrollDetails()
    End Sub

    Private Class Adjustments

        Public Property Code As String
        Public Property Description As String
        Public Property Amount As Decimal

    End Class

End Class
