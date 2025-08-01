Option Strict On

Imports System.ComponentModel
Imports System.Threading.Tasks
Imports AccuPay.Benchmark
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Core.ValueObjects
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports log4net
Imports Microsoft.Extensions.DependencyInjection

Public Class BenchmarkPayrollForm

    Private Shared ReadOnly logger As ILog = LogManager.GetLogger("BenchmarkPayrollLogger")

    Private _currentPayPeriod As PayPeriod
    Private _salaries As List(Of Salary)
    Private _employees As List(Of Employee)
    Private _actualSalaryPolicy As ActualTimeEntryPolicy

    Private _selectedDeductions As List(Of AdjustmentInput)
    Private _selectedIncomes As List(Of AdjustmentInput)

    Private _overtimeRate As OvertimeRate

    Private _benchmarkPayrollHelper As BenchmarkPayrollHelper

    Private _textBoxDelayedAction As New DelayedAction(Of Boolean)

    Private _payrollResources As IPayrollResources

    Private _employeeRate As BenchmarkPaystubRate

    Private _overtimes As List(Of OvertimeInput)

    Private _ecola As Allowance

    Private _pagibigLoan As Loan

    Private _sssLoan As Loan

    Private _leaveBalance As Decimal

    Private Const MoneyFormat As String = "#,##0.0000"

    Private ReadOnly _employeeRepository As IEmployeeRepository

    Private ReadOnly _loanRepository As ILoanRepository

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Private ReadOnly _salaryRepository As ISalaryRepository

    Private ReadOnly _listOfValueService As IListOfValueService

    Private ReadOnly _overtimeRateService As IOvertimeRateService

    Private _benchmarkPayrollGenerationOutput As BenchmarkPayrollGeneration.DoProcessOutput

    Sub New()

        InitializeComponent()

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _loanRepository = MainServiceProvider.GetRequiredService(Of ILoanRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _salaryRepository = MainServiceProvider.GetRequiredService(Of ISalaryRepository)

        _listOfValueService = MainServiceProvider.GetRequiredService(Of IListOfValueService)

        _overtimeRateService = MainServiceProvider.GetRequiredService(Of IOvertimeRateService)

        _salaries = New List(Of Salary)
        _employees = New List(Of Employee)

        _overtimes = New List(Of OvertimeInput)

        _selectedDeductions = New List(Of AdjustmentInput)
        _selectedIncomes = New List(Of AdjustmentInput)

    End Sub

    Private Async Sub BenchmarkPayrollForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        Await RefreshForm()

    End Sub

    Private Async Function RefreshForm(Optional refreshPayPeriod As Boolean = True) As Task
        InitializeControls()

        _benchmarkPayrollHelper = Await BenchmarkPayrollHelper.GetInstance(logger)

        If _benchmarkPayrollHelper Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot initialize the payroll form properly. Please contact Globagility Inc.")

            Return

        End If

        DeductionComboBox.DataSource = _benchmarkPayrollHelper.DeductionList
        DeductionComboBox.SelectedIndex = -1
        OtherIncomeComboBox.DataSource = _benchmarkPayrollHelper.IncomeList
        OtherIncomeComboBox.SelectedIndex = -1

        If refreshPayPeriod Then

            Await GetCutOffPeriod()

        End If

        If _currentPayPeriod Is Nothing Then
            MessageBoxHelper.ErrorMessage("Cannot identify the selected pay period. Please close then reopen this form and try again.")
            Return
        End If

        Await LoadPayrollDetails()
    End Function

    Private Async Function LoadPayrollDetails() As Task

        Await LoadPayrollResourcesAsync()

        _salaries = (Await _salaryRepository.
            GetByCutOffAsync(z_OrganizationID, _currentPayPeriod.PayToDate)).
            ToList()

        Await ShowEmployees()

        ClearEmployeeForms()
    End Function

    Private Sub InitializeControls()
        EmployeesGridView.AutoGenerateColumns = False
        DeductionsGridView.AutoGenerateColumns = False
        OtherIncomeGridView.AutoGenerateColumns = False

        DeductionComboBox.DisplayMember = "PartNo"
        OtherIncomeComboBox.DisplayMember = "PartNo"

        TotalDeductionTextBox.Clear()
        TotalOtherIncomeTextBox.Clear()
    End Sub

    Private Async Function LoadPayrollResourcesAsync() As Task

        _overtimeRate = Await _overtimeRateService.GetOvertimeRates()

        Dim settings = _listOfValueService.Create()

        _actualSalaryPolicy = New ActualTimeEntryPolicy(settings)

        'TODO: Add loading bar

        Dim payPeriodId = _currentPayPeriod.RowID.Value
        Dim paypFrom = _currentPayPeriod.PayFromDate
        Dim paypTo = _currentPayPeriod.PayToDate

        Dim resources = MainServiceProvider.GetRequiredService(Of IPayrollResources)

        Dim loadTask = Task.Factory.StartNew(
            Function()
                If paypFrom = Nothing And paypTo = Nothing Then
                    Return Nothing
                End If

                Dim resourcesTask = resources.Load(
                    payPeriodId:=payPeriodId,
                    organizationId:=z_OrganizationID,
                    userId:=z_User)

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

            _employees = (Await _employeeRepository.GetAllActiveWithoutPayrollAsync(
                    _currentPayPeriod.RowID.Value,
                    z_OrganizationID)).
                OrderBy(Function(e) e.FullNameLastNameFirst).
                ToList()
        Else
            _employees = New List(Of Employee)
        End If

        Await FilterEmployeeGridView()

    End Function

    Private Async Function ShowEmployee() As Task

        ClearEmployeeForms()

        If EmployeesGridView.CurrentRow IsNot Nothing Then

            If CheckIfGridViewHasValue(EmployeesGridView) = False Then Return

            Dim employeeId = CType(EmployeesGridView.CurrentRow.DataBoundItem, Employee)?.RowID

            If employeeId Is Nothing Then Return

            Dim employee = Await _employeeRepository.GetActiveEmployeeWithDivisionAndPositionAsync(employeeId.Value)

            If employee Is Nothing Then Return

            Await _benchmarkPayrollHelper.CleanEmployee(employeeId.Value)

            Dim salary = _salaries.
                Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID.Value)).
                FirstOrDefault

            If salary Is Nothing Then

                MessageBoxHelper.Warning("Selected employee currently has no active salary. Please add one before computing the employees payroll.")
                Return
            End If

            If (Await FetchOtherPayrollData(employeeId) = False) Then Return

            _employeeRate = New BenchmarkPaystubRate(employee, salary)

            Dim employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)
            _leaveBalance = Await employeeRepository.GetVacationLeaveBalance(employee.RowID.Value)

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

            EcolaTextBox.Text = _ecola.Amount.ToString(MoneyFormat)

            _selectedDeductions.Clear()
            DeductionsGridView.Rows.Clear()
            _selectedIncomes.Clear()
            OtherIncomeGridView.Rows.Clear()

            _overtimes = New List(Of OvertimeInput)

            EmployeeDetailsGroupBox.Enabled = True
            InputsTabControl.Enabled = True

        End If

    End Function

    Private Async Function FetchOtherPayrollData(employeeId As Integer?) As Task(Of Boolean)
        _ecola = Await BenchmarkPayrollHelper.GetEcola(
            employeeId.Value,
            payDateFrom:=_currentPayPeriod.PayFromDate)

        If _ecola Is Nothing Then

            MessageBoxHelper.Warning("Cannot retrieve the ECOLA data for this employee. Please contact Globagility Inc. to help fix this.")
            Return False
        End If

        Dim pagibigLoans = Await _loanRepository.GetActiveLoansByLoanNameAsync(ProductConstant.PAG_IBIG_LOAN, employeeId.Value)

        If pagibigLoans.Count > 1 Then

            MessageBoxHelper.Warning("Selected employee currently has multiple active PAGIBIG LOANs. Please delete one in the loan schedule form first.")
            Return False
        Else
            _pagibigLoan = pagibigLoans.FirstOrDefault

        End If

        Dim sssLoans = Await _loanRepository.GetActiveLoansByLoanNameAsync(ProductConstant.SSS_LOAN, employeeId.Value)

        If sssLoans.Count > 1 Then

            MessageBoxHelper.Warning("Selected employee currently has multiple active PAGIBIG LOANs. Please delete one in the loan schedule form first.")
            Return False
        Else
            _sssLoan = sssLoans.FirstOrDefault

        End If

        Return True
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
        LeaveTextBox.ResetText()
        EcolaTextBox.ResetText()

        BasicPaySummaryTextBox.ResetText()
        PhilhealthAmountTextBox.ResetText()
        SssAmountTextBox.ResetText()
        PagibigAmountTextBox.ResetText()
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

    End Sub

    Private Async Function GetCutOffPeriod() As Task
        _currentPayPeriod = Await _payPeriodRepository.GetOrCreateCurrentPayPeriodAsync(
            organizationId:=z_OrganizationID,
            currentUserId:=z_User)

        UpdateCutOffLabel()
    End Function

    Private Sub UpdateCutOffLabel()
        PayPeriodLabel.Text = $"For the Period:
            {_currentPayPeriod.PayFromDate:MMMM d} - {_currentPayPeriod.PayToDate:MMMM d}, {_currentPayPeriod.PayToDate.Year}"
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

        If CheckIfGridViewHasValue(EmployeesGridView) = False Then Return Nothing

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

    Private Async Sub EmployeesGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles EmployeesGridView.CellDoubleClick

        Await ShowEmployee()

    End Sub

    Private Sub ResetPayrollButton_Click(sender As Object, e As EventArgs) Handles ResetPayrollButton.Click

        ClearEmployeeForms()

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

    Private Sub SearchEmployeeTextBox_TextChanged(sender As Object, e As EventArgs) Handles SearchEmployeeTextBox.TextChanged

        _textBoxDelayedAction.ProcessAsync(
            Async Function()
                Await FilterEmployeeGridView()

                Return True
            End Function)

    End Sub

    Private Sub ComputeSalaryButton_Click(sender As Object, e As EventArgs) Handles ComputeSalaryButton.Click

        GenerateSummary()

    End Sub

    Private Sub GenerateSummary()

        Dim employee = GetSelectedEmployee()

        If employee Is Nothing Then

            MessageBoxHelper.ErrorMessage("No employee selected.")
            Return

        End If

        If _currentPayPeriod Is Nothing Then
            MessageBoxHelper.ErrorMessage("Cannot identify the selected pay period. Please close then reopen this form and try again.")
            Return
        End If

        Dim regularDays = AccuMath.CommercialRound(RegularDaysTextBox.Text.ToDecimal, 4)
        RegularDaysTextBox.Text = regularDays.ToString
        Dim lateDays = AccuMath.CommercialRound(LateTextBox.Text.ToDecimal, 4)
        LateTextBox.Text = lateDays.ToString
        Dim leaveDays = AccuMath.CommercialRound(LeaveTextBox.Text.ToDecimal)
        LeaveTextBox.Text = leaveDays.ToString

        If leaveDays > _leaveBalance Then

            MessageBoxHelper.ErrorMessage("No remaining SL/VL for this employee.")
            Return
        End If

        Dim output = BenchmarkPayrollGeneration.DoProcess(
            employee,
            _currentPayPeriod,
            _payrollResources,
            _currentPayPeriod,
            _employeeRate,
            regularDays:=regularDays,
            lateDays:=lateDays,
            leaveDays:=leaveDays,
            overtimeRate:=_overtimeRate,
            actualSalaryPolicy:=_actualSalaryPolicy,
            selectedDeductions:=_selectedDeductions,
            selectedIncomes:=_selectedIncomes,
            overtimes:=_overtimes,
            ecola:=_ecola)

        'TODO
        '#1. Resign employee

        _benchmarkPayrollGenerationOutput = output
        Dim currentPaystub = output.Paystub

        'Get loans data
        Dim loans As New List(Of LoanTransaction)

        For Each loan In output.LoanTransanctions
            loans.Add(loan.Clone())
        Next

        Dim pagIbigLoan As Decimal? = AccuMath.NullableDecimalTernaryOperator(_pagibigLoan Is Nothing, 0, Nothing)
        Dim sssLoan As Decimal? = AccuMath.NullableDecimalTernaryOperator(_sssLoan Is Nothing, 0, Nothing)

        Dim loanIndex As Integer = 0

        While (loans.Any())

            Dim loan = loans(loanIndex)

            If _pagibigLoan?.RowID IsNot Nothing AndAlso loan.LoanID = _pagibigLoan.RowID.Value Then

                If pagIbigLoan Is Nothing Then

                    pagIbigLoan = loan.DeductionAmount
                    loans.Remove(loan)
                    Continue While
                Else
                    'This most likely happens when there are multiple active pagibig loans which is not allowed.
                    MessageBoxHelper.ErrorMessage("There is a problem fetching the data for loans. Please contact Globagility Inc. to help fix this.")
                    Return
                End If

            ElseIf _sssLoan?.RowID IsNot Nothing AndAlso loan.LoanID = _sssLoan.RowID.Value Then

                If sssLoan Is Nothing Then

                    sssLoan = loan.DeductionAmount
                    loans.Remove(loan)
                    Continue While
                Else
                    'This most likely happens when there are multiple active SSS loans which is not allowed.
                    MessageBoxHelper.ErrorMessage("There is a problem fetching the data for loans. Please contact Globagility Inc. to help fix this.")
                    Return
                End If
            Else

                'This most likely happens when there are multiple active SSS loans which is not allowed.
                MessageBoxHelper.ErrorMessage("There is a problem fetching the data for loans. Please contact Globagility Inc. to help fix this.")
                Return
            End If

        End While

        'Show data from paystub
        TotalDeductionsLabel.Text = "Php " & Math.Abs(currentPaystub.TotalDeductionAdjustments).RoundToString()
        TotalOtherIncomeLabel.Text = "Php " & currentPaystub.TotalAdditionAdjustments.RoundToString()

        BasicPaySummaryTextBox.Text = currentPaystub.BasicPay.RoundToString()
        PhilhealthAmountTextBox.Text = currentPaystub.PhilHealthEmployeeShare.RoundToString()
        SssAmountTextBox.Text = currentPaystub.SssEmployeeShare.RoundToString()
        PagibigAmountTextBox.Text = currentPaystub.HdmfEmployeeShare.RoundToString()
        WithholdingTaxTextBox.Text = currentPaystub.WithholdingTax.RoundToString()
        PagibigLoanTextBox.Text = If(pagIbigLoan, 0).RoundToString()
        SssLoanTextBox.Text = If(sssLoan, 0).RoundToString()

        EcolaAmountTextBox.Text = currentPaystub.Ecola.RoundToString()
        ThirteenthMonthPayTextBox.Text = currentPaystub.ThirteenthMonthPay?.Amount.RoundToString()
        LeaveBalanceTextBox.Text = BenchmarkPayrollHelper.ConvertHoursToDays((_leaveBalance - currentPaystub.LeaveHours)).ToString

        GrossPayTextBox.Text = currentPaystub.GrossPay.RoundToString()
        TotalLeaveTextBox.Text = currentPaystub.LeavePay.RoundToString()

        TotalDeductionTextBox.Text = (
                currentPaystub.NetDeductions +
                Math.Abs(currentPaystub.TotalDeductionAdjustments)).
            RoundToString()

        TotalOtherIncomeTextBox.Text = currentPaystub.TotalAdditionAdjustments.RoundToString()
        TotalOvertimeTextBox.Text = BenchmarkPayrollHelper.GetTotalOvertimePay(currentPaystub).RoundToString()
        NetPayTextBox.Text = currentPaystub.NetPay.RoundToString()

        SummaryGroupBox.Enabled = True
    End Sub

    Private Function GetSelectedDeduction() As Product

        If DeductionComboBox.SelectedIndex < 0 OrElse
            DeductionComboBox.SelectedIndex >= _benchmarkPayrollHelper.DeductionList.Count Then

            Return Nothing

        End If

        Return _benchmarkPayrollHelper.DeductionList(DeductionComboBox.SelectedIndex)

    End Function

    Private Function GetSelectedIncome() As Product

        If OtherIncomeComboBox.SelectedIndex < 0 OrElse
            OtherIncomeComboBox.SelectedIndex >= _benchmarkPayrollHelper.IncomeList.Count Then

            Return Nothing

        End If

        Return _benchmarkPayrollHelper.IncomeList(OtherIncomeComboBox.SelectedIndex)

    End Function

    Private Sub RefreshDeductionGridView()

        DeductionComboBox.SelectedIndex = -1

        Try

            DeductionsGridView.DataSource = New BindingList(Of AdjustmentInput)(_selectedDeductions)
        Catch ex As Exception
            'usually because the cell is not yet committed (maybe because there is error)
        End Try

    End Sub

    Private Sub RefreshOtherIncomeGridView()

        OtherIncomeComboBox.SelectedIndex = -1

        Try
            OtherIncomeGridView.DataSource = New BindingList(Of AdjustmentInput)(_selectedIncomes)
        Catch ex As Exception
            'usually because the cell is not yet committed (maybe because there is error)
        End Try

    End Sub

    Private Sub SetOvertimeButton_Click(sender As Object, e As EventArgs) Handles SetOvertimeButton.Click

        Dim form As New SetOvertimeForm(
            _employeeRate.HourlyRate,
            _overtimeRate.OvertimeRateList,
            _overtimes,
            _employeeRate.Employee.IsPremiumInclusive)

        form.ShowDialog()

        _overtimes = form.Overtimes

        OvertimeTextBox.Text = _overtimes.Sum(Function(o) o.Hours).ToString

    End Sub

    Private Async Sub SavePayrollButton_Click(sender As Object, e As EventArgs) Handles SavePayrollButton.Click

        Await FunctionUtils.TryCatchFunctionAsync($"Payroll Generation for employee {_employeeRate.Employee.FullNameLastNameFirst} [{_employeeRate.Employee.EmployeeNo}]",
            Async Function()

                Await BenchmarkPayrollGeneration.Save(_benchmarkPayrollGenerationOutput)

                Await RefreshForm(refreshPayPeriod:=False)

                MessageBoxHelper.Information($"Payroll for employee {_employeeRate.Employee.FullNameLastNameFirst} [{_employeeRate.Employee.EmployeeNo}] was generated successfully.", "Payroll Generation")

            End Function)

    End Sub

    Private Sub AddDeductionButton_Click(sender As Object, e As EventArgs) Handles AddDeductionButton.Click

        Dim adjustment = GetSelectedDeduction()

        If adjustment Is Nothing Then

            MessageBoxHelper.ErrorMessage("Select a deduction type first.")
            Return
        End If

        If _selectedDeductions.
            Where(Function(a) a.Adjustment.RowID.Value = adjustment.RowID.Value).
            Any Then

            MessageBoxHelper.Warning("You have already added this deduction. You can edit the existing deduction amount instead.")
            Return

        End If

        _selectedDeductions.Add(New AdjustmentInput(adjustment))

        RefreshDeductionGridView()

    End Sub

    Private Sub RemoveDeductionButton_Click(sender As Object, e As EventArgs) Handles RemoveDeductionButton.Click

        If CheckIfGridViewHasValue(DeductionsGridView) = False Then Return

        Dim adjustment = CType(DeductionsGridView.CurrentRow?.DataBoundItem, AdjustmentInput)

        If adjustment Is Nothing Then

            MessageBoxHelper.Warning("No selected deduction.")
            Return
        End If

        _selectedDeductions.Remove(adjustment)

        RefreshDeductionGridView()

        GenerateSummary()

    End Sub

    Private Function CheckIfGridViewHasValue(gridView As DataGridView) As Boolean
        Return gridView.Rows.
            Cast(Of DataGridViewRow).
            Any(Function(r) r.Cells.Cast(Of DataGridViewCell).
                Any(Function(c) c.Value IsNot Nothing))
    End Function

    Private Sub AddIncomeButton_Click(sender As Object, e As EventArgs) Handles AddIncomeButton.Click

        Dim adjustment = GetSelectedIncome()

        If adjustment Is Nothing Then

            MessageBoxHelper.ErrorMessage("Select an income type first.")
            Return
        End If

        If _selectedIncomes.
            Where(Function(a) a.Adjustment.RowID.Value = adjustment.RowID.Value).
            Any Then

            MessageBoxHelper.Warning("You have already added this income. You can edit the existing income amount instead.")
            Return

        End If

        _selectedIncomes.Add(New AdjustmentInput(adjustment))

        RefreshOtherIncomeGridView()

    End Sub

    Private Sub RemoveIncomeButton_Click(sender As Object, e As EventArgs) Handles RemoveIncomeButton.Click

        If CheckIfGridViewHasValue(OtherIncomeGridView) = False Then Return

        Dim adjustment = CType(OtherIncomeGridView.CurrentRow?.DataBoundItem, AdjustmentInput)

        If adjustment Is Nothing Then

            MessageBoxHelper.Warning("No selected income.")
            Return
        End If

        _selectedIncomes.Remove(adjustment)

        RefreshOtherIncomeGridView()

        GenerateSummary()

    End Sub

    Private Sub GridView_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) _
        Handles OtherIncomeGridView.CellEndEdit, DeductionsGridView.CellEndEdit

        If e.RowIndex >= 0 Then

            Dim adjustmentInput As AdjustmentInput = Nothing

            If sender Is DeductionsGridView Then

                If e.RowIndex >= _selectedDeductions.Count Then Return

                adjustmentInput = _selectedDeductions(e.RowIndex)

            ElseIf sender Is OtherIncomeGridView Then

                If e.RowIndex >= _selectedIncomes.Count Then Return

                adjustmentInput = _selectedIncomes(e.RowIndex)
            End If

            If adjustmentInput Is Nothing Then

                Return
            Else

                If adjustmentInput.Amount < 0 Then

                    adjustmentInput.Amount = Math.Abs(adjustmentInput.Amount)

                End If

            End If

            adjustmentInput.Amount = AccuMath.CommercialRound(adjustmentInput.Amount)

            GenerateSummary()

        End If

    End Sub

    Private Sub GridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) _
        Handles OtherIncomeGridView.DataError, DeductionsGridView.DataError

        Beep()
        e.Cancel = True

    End Sub

    Private Async Sub RefreshFormButton_Click(sender As Object, e As EventArgs) Handles RefreshFormButton.Click

        Await RefreshForm(refreshPayPeriod:=False)

    End Sub

    Private Sub BenchmarkPayrollForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        PayrollForm.listPayrollForm.Remove(Me.Name)

    End Sub

End Class
