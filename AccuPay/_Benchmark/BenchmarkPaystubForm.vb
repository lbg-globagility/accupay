Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.SimplifiedEntities
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class BenchmarkPaystubForm

    Private _currentPayPeriod As IPayPeriod

    Private _employeeRepository As EmployeeRepository

    Private _salaryRepository As SalaryRepository

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

        '_overtimes = New List(Of OvertimeInput)

        '_selectedDeductions = New List(Of AdjustmentInput)
        '_selectedIncomes = New List(Of AdjustmentInput)

        '_loanScheduleRepository = New LoanScheduleRepository

    End Sub

    Private Sub BenchmarkPaystubForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        PayrollForm.listPayrollForm.Remove(Me.Name)
    End Sub

    Private Async Sub BenchmarkPaystubForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        Await RefreshForm()

    End Sub

    Private Async Function RefreshForm() As Task
        InitializeControls()

        Await GetCutOffPeriod()

        If _currentPayPeriod Is Nothing Then
            MessageBoxHelper.ErrorMessage("Cannot identify the selected pay period. Please close then reopen this form and try again.")
            Return
        End If

        'Await LoadPayrollDetails()

        '_overtimeRate = Await OvertimeRateService.GetOvertimeRates()

        Await LoadPayrollDetails()
    End Function

    Private Async Function LoadPayrollDetails() As Task
        _salaries = Await _salaryRepository.
                                        GetAllByCutOff(_currentPayPeriod.PayFromDate)

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
        _currentPayPeriod = Await PayrollTools.
                                GetCurrentlyWorkedOnPayPeriodByCurrentYear()

        UpdateCutOffLabel()
    End Function

    Private Sub UpdateCutOffLabel()
        PayPeriodLabel.Text = $"For the Period:
            {_currentPayPeriod.PayFromDate.ToString("MMMM d")} - {_currentPayPeriod.PayToDate.ToString("MMMM d")}, {_currentPayPeriod.PayToDate.Year}"
    End Sub

    Private Async Function ShowEmployees() As Task

        Dim payPeriodId = _currentPayPeriod?.RowID

        If payPeriodId IsNot Nothing Then

            _employees = Await _employeeRepository.
                                GetAllWithPayrollAsync(_currentPayPeriod.RowID)
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

        DeductionsGridView.DataSource = New List(Of Adjustment)
        OtherIncomeGridView.DataSource = New List(Of Adjustment)

        RegularDaysTextBox.ResetText()
        OvertimeTextBox.ResetText()
        LateTextBox.ResetText()
        LeaveTextBox.ResetText()
        EcolaTextBox.ResetText()

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

        DeletePaystubButton.Enabled = False

    End Sub

    Private Async Function ShowEmployee() As Task

        ClearEmployeeForms()

        DeletePaystubButton.Enabled = True 'move this at the bottom after you finish the codes below

        If EmployeesGridView.CurrentRow IsNot Nothing Then

            If CheckIfGridViewHasValue(EmployeesGridView) = False Then Return

            Dim employee = CType(EmployeesGridView.CurrentRow.DataBoundItem, Employee)

            If employee?.RowID Is Nothing Then Return

            Dim employeeId = employee.RowID.Value

            EmployeeNumberLabel.Text = employee.EmployeeNo
            EmployeeNameLabel.Text = employee.FullNameLastNameFirst.ToUpper

            Return

            'TODO
            '4. get undeclared/actual salary
            '5. populate summary

            Using context As New PayrollContext

                Dim payStub = Await context.Paystubs.
                        FirstOrDefaultAsync(Function(p) p.EmployeeID.Value = employeeId AndAlso p.PayPeriodID.Value = _currentPayPeriod.RowID.Value)

                Dim payStubId = payStub?.RowID

                If payStubId Is Nothing Then

                    MessageBoxHelper.ErrorMessage("This employee has no paystub for this cut off. It may have been deleted earlier. Please refresh the form.")
                    Return

                End If

                Dim payStubAdjustments = Await context.Adjustments.
                                                Include(Function(a) a.Product).
                                                Where(Function(a) a.PaystubID = payStubId).
                                                Select(Function(p) New Adjustments With {
                                                    .Amount = p.Amount,
                                                    .Code = p.Product.Comments,
                                                    .Description = p.Product.PartNo
                                                    }).
                                                ToListAsync

                Dim deductions = payStubAdjustments.Where(Function(p) p.Amount < 0).ToList
                Dim otherIncomes = payStubAdjustments.Where(Function(p) p.Amount > 0).ToList

                Dim loanRecords = context.LoanTransactions.
                                            Where(Function(l) l.EmployeeID = employeeId).
                                            Where(Function(l) l.PayPeriodID = _currentPayPeriod.RowID.Value).
                                            ToListAsync

                DeductionsGridView.DataSource = deductions
                OtherIncomeGridView.DataSource = otherIncomes

            End Using

            '1.

            'Dim salary = _salaries.
            '                Where(Function(s) Nullable.Equals(s.EmployeeID, employee?.RowID)).
            '                FirstOrDefault

            'If salary Is Nothing Then

            '    MessageBoxHelper.Warning("Cannot retrieve the employee salary. It may have been edited or deleted after this payroll was generated.")
            'End If

            'If (Await FetchOtherPayrollData(employeeId) = False) Then Return

            '_employeeRate = New BenchmarkPaystubRate(employee, salary)

            '_leaveBalance = Await EmployeeData.GetVacationLeaveBalance(employee.RowID)

            'If _employeeRate.IsInvalid Then Return

            'EmployeeNumberLabel.Text = _employeeRate.Employee.EmployeeNo
            'EmployeeNameLabel.Text = _employeeRate.Employee.FullNameLastNameFirst.ToUpper

            'TinTextBox.Text = _employeeRate.Employee.TinNo
            'SssNumberTextBox.Text = _employeeRate.Employee.SssNo
            'PagibigNumberTextBox.Text = _employeeRate.Employee.HdmfNo
            'PhilhealthNumberTextBox.Text = _employeeRate.Employee.PhilHealthNo
            'BasicPayTextBox.Text = _employeeRate.MonthlyRate.ToString(MoneyFormat)
            'PerDayTextBox.Text = _employeeRate.DailyRate.ToString(MoneyFormat)
            'PerHourTextBox.Text = _employeeRate.HourlyRate.ToString(MoneyFormat)
            'AllowanceTextBox.Text = _employeeRate.AllowanceSalary.ToString(MoneyFormat)

            'EcolaTextBox.Text = _ecola.Amount.ToString(MoneyFormat)

            '_selectedDeductions.Clear()
            'DeductionsGridView.Rows.Clear()
            '_selectedIncomes.Clear()
            'OtherIncomeGridView.Rows.Clear()

            '_overtimes = New List(Of OvertimeInput)

        End If

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

    Private Sub DeletePaystubButton_Click(sender As Object, e As EventArgs) Handles DeletePaystubButton.Click

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

        If MessageBoxHelper.Confirm(Of Boolean) _
               (confirmMessage, "Delete Paystub", messageBoxIcon:=MessageBoxIcon.Warning) = False Then Return

        PayrollTools.DeletePaystub(employeeId, _currentPayPeriod.RowID.Value)

        MessageBoxHelper.Information("Done! " & vbNewLine + vbNewLine & $"Go to Payroll transactions to process the payslip of {employee.FullNameLastNameFirst} [{employee.EmployeeNo}] again.", "Delete Paystub")

    End Sub

    Private Sub SearchEmployeeTextBox_TextChanged(sender As Object, e As EventArgs) Handles SearchEmployeeTextBox.TextChanged

        _textBoxDelayedAction.ProcessAsync(Async Function()
                                               Await FilterEmployeeGridView()

                                               Return True
                                           End Function)

    End Sub

    Private Async Sub RefreshFormButton_Click(sender As Object, e As EventArgs) Handles RefreshFormButton.Click

        Await RefreshForm()

    End Sub

    Private Async Sub PayPeriodLabel_Click(sender As Object, e As EventArgs) Handles PayPeriodLabel.Click
        Dim form As New selectPayPeriod()
        form.GeneratePayroll = False
        form.ShowDialog()

        If form.PayPeriod IsNot Nothing Then

            _currentPayPeriod = form.PayPeriod

            If _currentPayPeriod Is Nothing Then
                MessageBoxHelper.ErrorMessage("Cannot identify the selected pay period. Please close then reopen this form and try again.")
                Return
            End If

            UpdateCutOffLabel()

            Await LoadPayrollDetails()
        End If
    End Sub

    Private Class Adjustments

        Public Property Code As String
        Public Property Description As String
        Public Property Amount As Decimal

    End Class

End Class