Imports System.Threading.Tasks
Imports AccuPay.Benchmark
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.ModelData
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

    Private _productRepository As ProductRepository

    Private _pagibigLoanId As Integer?

    Private _sssLoanId As Integer?

    Private _overtimeRate As OvertimeRate

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _employeeRepository = New EmployeeRepository
        _salaryRepository = New SalaryRepository
        _productRepository = New ProductRepository
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

        Dim govermentLoans = Await _productRepository.GetGovernmentLoanTypes()

        _pagibigLoanId = govermentLoans.FirstOrDefault(Function(l) l.IsPagibigLoan)?.RowID
        _sssLoanId = govermentLoans.FirstOrDefault(Function(l) l.IsSssLoan)?.RowID

        Await RefreshForm()

    End Sub

    Private Async Function RefreshForm() As Task
        InitializeControls()

        Await GetCutOffPeriod()

        If _currentPayPeriod Is Nothing Then
            MessageBoxHelper.ErrorMessage("Cannot identify the selected pay period. Please close then reopen this form and try again.")
            Return
        End If

        _overtimeRate = Await OvertimeRateService.GetOvertimeRates()

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

        OvertimeGridView.DataSource = New List(Of OvertimeInput)
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

        TotalOtherIncomeLabel.Text = "00.00"
        TotalDeductionsLabel.Text = "00.00"
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

            'TODO
            '2. Ecola
            '5. get undeclared/actual salary

            Using context As New PayrollContext

                'paystub
                Dim payStub As Paystub = Await GetPayStub(employeeId, context)
                Dim payStubId = payStub?.RowID

                If payStubId Is Nothing Then

                    MessageBoxHelper.ErrorMessage("This employee has no paystub for this cut off. It may have been deleted earlier. Please refresh the form.")
                    Return

                End If

                'adjustments (deductions and other incomes)
                Dim payStubAdjustments = Await GetAdjustments(context, payStubId)
                Dim otherIncomes = payStubAdjustments.Where(Function(p) p.Amount > 0).ToList
                Dim deductions = GetAdjustmentDeductions(payStubAdjustments)

                DeductionsGridView.DataSource = deductions
                OtherIncomeGridView.DataSource = otherIncomes

                'loans
                Dim loanAmounts = Await GetGovernmentLoanAmounts(context, employeeId)
                Dim pagIbigLoan = loanAmounts.Item1
                Dim sssLoan = loanAmounts.Item2

                'overtime list
                Dim overtimeInputs = GetOvertimeInputs(payStub, employee)
                If overtimeInputs Is Nothing Then Return
                OvertimeGridView.DataSource = overtimeInputs

                'Show summary
                TotalDeductionsLabel.Text = "Php " & Math.Abs(payStub.TotalDeductionAdjustments).RoundToString()
                TotalOtherIncomeLabel.Text = "Php " & payStub.TotalAdditionAdjustments.RoundToString()

                BasicPaySummaryTextBox.Text = payStub.BasicPay.RoundToString()
                PhilhealthAmountTextBox.Text = payStub.PhilHealthEmployeeShare.RoundToString()
                SssAmountTextBox.Text = payStub.SssEmployeeShare.RoundToString()
                PagibigAmountTextBox.Text = payStub.HdmfEmployeeShare.RoundToString()
                WithholdingTaxTextBox.Text = payStub.WithholdingTax.RoundToString()
                PagibigLoanTextBox.Text = If(pagIbigLoan, 0).RoundToString()
                SssLoanTextBox.Text = If(sssLoan, 0).RoundToString()

                payStub.Ecola = payStub.AllowanceItems.Sum(Function(a) a.Amount)
                EcolaAmountTextBox.Text = payStub.Ecola.RoundToString()
                ThirteenthMonthPayTextBox.Text = payStub.ThirteenthMonthPay?.Amount.RoundToString()
                LeaveBalanceTextBox.Text = (Await EmployeeData.GetVacationLeaveBalance(employee.RowID)).ToString

                GrossPayTextBox.Text = payStub.GrossPay.RoundToString()
                TotalLeaveTextBox.Text = payStub.LeavePay.RoundToString()

                TotalDeductionTextBox.Text = (payStub.NetDeductions +
                                            Math.Abs(payStub.TotalDeductionAdjustments)).RoundToString()

                TotalOtherIncomeTextBox.Text = payStub.TotalAdditionAdjustments.RoundToString()
                TotalOvertimeTextBox.Text = BenchmarkPayrollHelper.GetTotalOvertimePay(payStub).RoundToString()
                NetPayTextBox.Text = payStub.NetPay.RoundToString()

            End Using
        End If

    End Function

    Private Function GetOvertimeInputs(paystub As Paystub, employee As Employee) As List(Of OvertimeInput)

        Dim overtimeInputs As New List(Of OvertimeInput)

        Dim salary = _salaries.
                            Where(Function(s) Nullable.Equals(s.EmployeeID, employee?.RowID)).
                            FirstOrDefault

        If salary Is Nothing Then

            MessageBoxHelper.Warning("Selected employee currently has no active salary. Please add one before computing the employees payroll.")
            Return Nothing
        End If

        Dim employeeRate = New BenchmarkPaystubRate(employee, salary)

        If employeeRate.IsInvalid Then

            MessageBoxHelper.Warning("Cannot retrieve details of the employee. Please try again and refresh the form or contact Globagility Inc.")
            Return Nothing
        End If

        Dim payPerHour = employeeRate.HourlyRate

        If paystub.OvertimePay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.Overtime,
                                    paystub.OvertimeHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.NightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.NightDifferential,
                                    paystub.NightDiffHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.NightDiffOvertimePay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.NightDifferentialOvertime,
                                    paystub.NightDiffOvertimeHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RestDayPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RestDay,
                                    paystub.RestDayHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RestDayOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RestDayOvertime,
                                    paystub.RestDayOTHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RestDayNightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RestDayNightDifferential,
                                    paystub.RestDayNightDiffHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RestDayNightDiffOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RestDayNightDifferentialOvertime,
                                    paystub.RestDayNightDiffOTHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.SpecialHolidayPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.SpecialHoliday,
                                    paystub.SpecialHolidayHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.SpecialHolidayOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.SpecialHolidayOvertime,
                                    paystub.SpecialHolidayOTHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.SpecialHolidayNightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.SpecialHolidayNightDifferential,
                                    paystub.SpecialHolidayNightDiffHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.SpecialHolidayNightDiffOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.SpecialHolidayNightDifferentialOvertime,
                                    paystub.SpecialHolidayNightDiffOTHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.SpecialHolidayRestDayPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.SpecialHolidayRestDay,
                                    paystub.SpecialHolidayRestDayHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.SpecialHolidayRestDayOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.SpecialHolidayRestDayOvertime,
                                    paystub.SpecialHolidayRestDayOTHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.SpecialHolidayRestDayNightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.SpecialHolidayRestDayNightDifferential,
                                    paystub.SpecialHolidayRestDayNightDiffHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.SpecialHolidayRestDayNightDiffOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.SpecialHolidayRestDayNightDifferentialOvertime,
                                    paystub.SpecialHolidayRestDayNightDiffOTHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RegularHolidayPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RegularHoliday,
                                    paystub.RegularHolidayHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RegularHolidayOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RegularHolidayOvertime,
                                    paystub.RegularHolidayOTHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RegularHolidayNightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RegularHolidayNightDifferential,
                                    paystub.RegularHolidayNightDiffHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RegularHolidayNightDiffOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RegularHolidayNightDifferentialOvertime,
                                    paystub.RegularHolidayNightDiffOTHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RegularHolidayRestDayPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RegularHolidayRestDay,
                                    paystub.RegularHolidayRestDayHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RegularHolidayRestDayOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RegularHolidayRestDayOvertime,
                                    paystub.RegularHolidayRestDayOTHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RegularHolidayRestDayNightDiffPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RegularHolidayRestDayNightDifferential,
                                    paystub.RegularHolidayRestDayNightDiffHours,
                                    False,
                                    payPerHour))
        End If

        If paystub.RegularHolidayRestDayNightDiffOTPay <> 0 Then

            overtimeInputs.Add(New OvertimeInput(
                                    _overtimeRate.RegularHolidayRestDayNightDifferentialOvertime,
                                    paystub.RegularHolidayRestDayNightDiffOTHours,
                                    False,
                                    payPerHour))

        End If

        Return overtimeInputs

    End Function

    Private Async Function GetGovernmentLoanAmounts(context As PayrollContext, employeeId As Integer) As Task(Of (Decimal?, Decimal?))

        Dim loanRecords = Await context.LoanTransactions.
                                            Include(Function(t) t.LoanSchedule).
                                            Include(Function(t) t.LoanSchedule.LoanType).
                                            Where(Function(t) t.EmployeeID = employeeId).
                                            Where(Function(t) t.PayPeriodID = _currentPayPeriod.RowID.Value).
                                            ToListAsync

        Dim pagIbigLoan = loanRecords.
            FirstOrDefault(Function(l) l.LoanSchedule.LoanTypeID = _pagibigLoanId)?.Amount
        Dim sssLoan = loanRecords.
            FirstOrDefault(Function(l) l.LoanSchedule.LoanTypeID = _sssLoanId)?.Amount

        Return (pagIbigLoan, sssLoan)

    End Function

    Private Shared Function GetAdjustmentDeductions(payStubAdjustments As List(Of Adjustments)) As List(Of Adjustments)
        Dim deductions = payStubAdjustments.Where(Function(p) p.Amount < 0).ToList
        For Each deduction In deductions
            deduction.Amount = Math.Abs(deduction.Amount)
        Next

        Return deductions
    End Function

    Private Shared Async Function GetAdjustments(context As PayrollContext, payStubId As Integer?) As Task(Of List(Of Adjustments))
        Return Await context.Adjustments.
                                                        Include(Function(a) a.Product).
                                                        Where(Function(a) a.PaystubID = payStubId).
                                                        Select(Function(p) New Adjustments With {
                                                            .Amount = p.Amount,
                                                            .Code = p.Product.Comments,
                                                            .Description = p.Product.PartNo
                                                            }).
                                                        ToListAsync
    End Function

    Private Async Function GetPayStub(employeeId As Integer, context As PayrollContext) As Task(Of Paystub)
        Return Await context.Paystubs.
                                Include(Function(p) p.ThirteenthMonthPay).
                                Include(Function(a) a.Adjustments).
                                Include(Function(a) a.ActualAdjustments).
                                Include(Function(a) a.AllowanceItems).
                                FirstOrDefaultAsync(Function(p) p.EmployeeID.Value = employeeId AndAlso p.PayPeriodID.Value = _currentPayPeriod.RowID.Value)
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

        If MessageBoxHelper.Confirm(Of Boolean) _
               (confirmMessage, "Delete Paystub", messageBoxIcon:=MessageBoxIcon.Warning) = False Then Return

        PayrollTools.DeletePaystub(employeeId, _currentPayPeriod.RowID.Value)

        Await RefreshForm()

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