Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Entities.LeaveReset
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Interfaces.Domain_Services
Imports AccuPay.Core.Interfaces.Reports
Imports AccuPay.Core.Interfaces.Repositories
Imports AccuPay.Core.ValueObjects
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class CashOutUnusedLeavesForm
    Private ReadOnly _payPerioId As Integer
    Private ReadOnly _payStubForm As PayStubForm
    Private _employeeRepository As IEmployeeRepository
    Private _employees As List(Of Employee)
    Private _timePeriod As TimePeriod
    Private ReadOnly _leaveResetDataService As ILeaveResetDataService
    Private ReadOnly _alphaListReportDataService As IAlphaListReportDataService
    Private ReadOnly _salaryDataService As ISalaryDataService
    Private ReadOnly _adjustmentDataService As IAdjustmentDataService
    Private ReadOnly _productRepository As IProductRepository
    Private ReadOnly _leaveLedgerRepository As ILeaveLedgerRepository
    Private ReadOnly _cashoutUnusedLeaveRepository As ICashoutUnusedLeaveRepository
    Private ReadOnly _paystubRepository As IPaystubRepository

    Sub New(payPerioId As Integer, payStubForm As PayStubForm)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _payPerioId = payPerioId

        _payStubForm = payStubForm

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _leaveResetDataService = MainServiceProvider.GetRequiredService(Of ILeaveResetDataService)

        _alphaListReportDataService = MainServiceProvider.GetRequiredService(Of IAlphaListReportDataService)

        _salaryDataService = MainServiceProvider.GetRequiredService(Of ISalaryDataService)

        _adjustmentDataService = MainServiceProvider.GetRequiredService(Of IAdjustmentDataService)

        _productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)

        _leaveLedgerRepository = MainServiceProvider.GetRequiredService(Of ILeaveLedgerRepository)

        _cashoutUnusedLeaveRepository = MainServiceProvider.GetRequiredService(Of ICashoutUnusedLeaveRepository)

        _paystubRepository = MainServiceProvider.GetRequiredService(Of IPaystubRepository)
    End Sub

    Private Async Sub PreviewLeaveBalanceForm_LoadAsync(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvEmployees.AutoGenerateColumns = False
        DataGridViewX1.AutoGenerateColumns = False

        _timePeriod = MakeTimePeriod(Date.Now, Date.Now)
        Dim leaveReset = Await GetLeaveReset(_timePeriod)
        _timePeriod = leaveReset.GetTimePeriod(Date.Now.Year)

        Dim isVacationLeaveSupported = leaveReset.IsVacationLeaveSupported
        eVacationLeaveAllowance.Visible = isVacationLeaveSupported
        eVacationLeaveBalance.Visible = isVacationLeaveSupported
        eCashoutVacation.Visible = isVacationLeaveSupported
        '
        cVacationLeaveAllowance.Visible = isVacationLeaveSupported
        cVacationLeaveBalance.Visible = isVacationLeaveSupported
        cCashoutVacation.Visible = isVacationLeaveSupported

        Dim isSickLeaveSupported = leaveReset.IsSickLeaveSupported
        eSickLeaveAllowance.Visible = isSickLeaveSupported
        eSickLeaveBalance.Visible = isSickLeaveSupported
        eCashoutSick.Visible = isSickLeaveSupported
        '
        cSickLeaveAllowance.Visible = isSickLeaveSupported
        cSickLeaveBalance.Visible = isSickLeaveSupported
        cCashoutSick.Visible = isSickLeaveSupported

        Dim isOthersLeaveSupported = leaveReset.IsOthersLeaveSupported
        eOthersLeaveAllowance.Visible = isOthersLeaveSupported
        eOthersLeaveBalance.Visible = isOthersLeaveSupported
        eCashoutOthers.Visible = isOthersLeaveSupported
        '
        cOthersLeaveAllowance.Visible = isOthersLeaveSupported
        cOthersLeaveBalance.Visible = isOthersLeaveSupported
        cCashoutOthers.Visible = isOthersLeaveSupported

        Dim isParentalLeaveSupported = leaveReset.IsParentalLeaveSupported
        eParentalLeaveAllowance.Visible = isParentalLeaveSupported
        eParentalLeaveBalance.Visible = isParentalLeaveSupported
        eCashoutParental.Visible = isParentalLeaveSupported
        '
        cParentalLeaveAllowance.Visible = isParentalLeaveSupported
        cParentalLeaveBalance.Visible = isParentalLeaveSupported
        cCashoutParental.Visible = isParentalLeaveSupported

        eIsTicked.HeaderCell = New CheckBoxDataGridViewColumnHeaderCell

        Dim leaveBasisStartDates = {leaveReset.VacationLeaveBasisStartDate,
            leaveReset.SickLeaveBasisStartDate,
            leaveReset.OthersLeaveBasisStartDate,
            leaveReset.ParentalLeaveBasisStartDate}
        Dim hasDateRegularized = leaveBasisStartDates.Where(Function(x) x = BasisStartDateEnum.DateRegularized).Any()
        eDateRegularized.Visible = hasDateRegularized
        cDateRegularized.Visible = hasDateRegularized

        leaveReset.ChangeDateAccordingToYear()

        UpdateLeaveResetPeriodRange(leaveReset)

        Await LoadEmployees(leaveReset:=leaveReset)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub UpdateLeaveResetPeriodRange(leaveReset As LeaveReset)
        LinkLabel1.Text = $"For the period of {leaveReset.StartPeriodDate.ToShortDateString()} to {leaveReset.EndPeriodDate.ToShortDateString()}"
    End Sub

    Private Async Function GetLeaveReset(timePeriod As TimePeriod) As Task(Of LeaveReset)
        Return Await _leaveResetDataService.GetByOrganizationIdAndDate(
            organizationId:=z_OrganizationID,
            timePeriod:=timePeriod)
    End Function

    Private Function MakeTimePeriod(start As Date, [end] As Date) As TimePeriod
        Return New TimePeriod(start:=start, [end]:=[end])
    End Function

    Private Function GetPayrollSelector() As MultiplePayPeriodSelectionDialog
        Dim payrollSelector = New MultiplePayPeriodSelectionDialog With {
            .ShowPayrollSummaryPanel = False,
            .ShowDeclaredOrActualOptionsPanel = False}

        If payrollSelector.ShowDialog() <> DialogResult.OK Then
            Return Nothing
        End If

        Return payrollSelector
    End Function

    Private Async Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        'Handles LinkLabel1.LinkClicked
        Dim payrollSelector = GetPayrollSelector()
        If payrollSelector Is Nothing Then Return

        _timePeriod = MakeTimePeriod(payrollSelector.PayPeriodFrom.PayFromDate,
            payrollSelector.PayPeriodTo.PayToDate)

        Dim leaveReset = Await GetLeaveReset(_timePeriod)

        leaveReset.ChangeDateAccordingToYear(_timePeriod.Start.Year)

        UpdateLeaveResetPeriodRange(leaveReset)
    End Sub

    Private Async Function LoadEmployees(leaveReset As LeaveReset) As Task
        Dim activeEmployees = Await _employeeRepository.
            GetAllWithinServicePeriodAsync(z_OrganizationID, leaveReset.EndPeriodDate)

        _employees = activeEmployees.
            OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst).
            ToList()

        Dim salaries0 = _alphaListReportDataService.GetLatestSalaries2(organizationId:=z_OrganizationID)
        Dim salaryIds = salaries0.Select(Function(s) s.RowID).ToArray()
        Dim salaries = Await _salaryDataService.GetSalariesByIds(rowIds:=salaryIds)

        Dim leaveLedgers = Await _leaveLedgerRepository.GetAll(organizationId:=z_OrganizationID)

        Dim cashoutLeaves = Await _cashoutUnusedLeaveRepository.GetByPeriodAsync(payPeriodId:=_payPerioId)

        Dim paystubs = Await _paystubRepository.GetByPayPeriodFullPaystubAsync(payPeriodId:=_payPerioId)

        Dim _employeeModels = _employees.
            Select(Function(e) New EmployeeModel(payPeriodId:=_payPerioId,
                Employee:=e,
                salaries:=salaries.Where(Function(s) CBool(s.EmployeeID = e.RowID)).ToList(),
                leaveLedgers:=leaveLedgers.Where(Function(l) CBool(l.EmployeeID = e.RowID)).ToList(),
                cashoutLeaves:=cashoutLeaves.Where(Function(c) CBool(c.EmployeeID = e.RowID)).ToList(),
                paystub:=paystubs.FirstOrDefault(Function(l) CBool(l.EmployeeID = e.RowID)))).
            ToList()

        Dim toCashout = _employeeModels.Where(Function(t) t.IsCashedOut = False).ToList()
        dgvEmployees.DataSource = toCashout
        TabPage1.Text = $"{TabPage1.Text} ({toCashout.Count})"
        Dim cashedout = _employeeModels.Where(Function(t) t.IsCashedOut).ToList()
        DataGridViewX1.DataSource = cashedout
        TabPage2.Text = $"{TabPage2.Text} ({cashedout.Count})"
    End Function

    Private Async Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Dim models = dgvEmployees.Rows.
            OfType(Of DataGridViewRow).
            Select(Function(r) DirectCast(r.DataBoundItem, EmployeeModel)).
            Where(Function(r) r.IsTicked).
            ToList()

        If Not models.Any() Then
            MessageBox.Show("Pleas select one or more employee.", "No employee(s) selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirm = MessageBoxHelper.Confirm(Of Boolean)(
            $"{models.Count} employee(s) will cashout unused leave(s) {LinkLabel1.Text.ToLower()}?",
            "Cashout Unused Leaves")

        If Not confirm Then Return
        'btnReset.Enabled = False
        'btnClose.Enabled = False

        Dim modelEmployeeIds = models.Select(Function(m) m.RowID).ToArray()

        Dim leaveReset = Await GetLeaveReset(_timePeriod)

        Dim paystubs = (Await _payStubForm.PaystubRepository.GetByPayPeriodFullPaystubAsync(payPeriodId:=_payPerioId)).
            Where(Function(p) modelEmployeeIds.Contains(p.EmployeeID.Value)).
            ToList()

        Dim cashoutUnusedLeaves = New List(Of CashoutUnusedLeave)

        If leaveReset.IsVacationLeaveSupported Then
            Dim cashoutVacationLeaveType = Await _productRepository.GetOrCreateAdjustmentTypeAsync(adjustmentTypeName:=$"Cashout {LeaveTypeEnum.Vacation.Type}",
                organizationId:=z_OrganizationID,
                userId:=z_User)
            Dim adjustments = paystubs.Select(Function(p) ActualAdjustment.NewActualAdjustment(organizationId:=z_OrganizationID, userId:=z_User, amount:=models.FirstOrDefault(Function(m) CBool(m.RowID = p.EmployeeID)).CashoutVacation,
                comment:=String.Empty,
                productId:=cashoutVacationLeaveType.RowID,
                paystubId:=p.RowID)).
                ToList()
            Await _adjustmentDataService.AppendManyAsync(organizationId:=z_OrganizationID,
                userId:=z_User,
                payPeriodId:=_payPerioId,
                actualAdjustments:=adjustments)

            cashoutUnusedLeaves.AddRange(models.Select(Function(m) m.CashoutVacationLeave))
        End If

        If leaveReset.IsSickLeaveSupported Then
            Dim cashoutSickLeaveType = Await _productRepository.GetOrCreateAdjustmentTypeAsync(adjustmentTypeName:=$"Cashout {LeaveTypeEnum.Sick.Type}",
                organizationId:=z_OrganizationID,
                userId:=z_User)
            Dim adjustments = paystubs.Select(Function(p) ActualAdjustment.NewActualAdjustment(organizationId:=z_OrganizationID, userId:=z_User, amount:=models.FirstOrDefault(Function(m) CBool(m.RowID = p.EmployeeID)).CashoutSick,
                comment:=String.Empty,
                productId:=cashoutSickLeaveType.RowID,
                paystubId:=p.RowID)).
                ToList()
            Await _adjustmentDataService.AppendManyAsync(organizationId:=z_OrganizationID,
                userId:=z_User,
                payPeriodId:=_payPerioId,
                actualAdjustments:=adjustments)

            cashoutUnusedLeaves.AddRange(models.Select(Function(m) m.CashoutSickLeave))
        End If

        If leaveReset.IsOthersLeaveSupported Then
            Dim cashoutOthersLeaveType = Await _productRepository.GetOrCreateAdjustmentTypeAsync(adjustmentTypeName:=$"Cashout {LeaveTypeEnum.Others.Type}",
                organizationId:=z_OrganizationID,
                userId:=z_User)
            Dim adjustments = paystubs.Select(Function(p) ActualAdjustment.NewActualAdjustment(organizationId:=z_OrganizationID, userId:=z_User, amount:=models.FirstOrDefault(Function(m) CBool(m.RowID = p.EmployeeID)).CashoutOthers,
                comment:=String.Empty,
                productId:=cashoutOthersLeaveType.RowID,
                paystubId:=p.RowID)).
                ToList()
            Await _adjustmentDataService.AppendManyAsync(organizationId:=z_OrganizationID,
                userId:=z_User,
                payPeriodId:=_payPerioId,
                actualAdjustments:=adjustments)

            cashoutUnusedLeaves.AddRange(models.Select(Function(m) m.CashoutOthersLeave))
        End If

        If leaveReset.IsParentalLeaveSupported Then
            Dim cashoutParentalLeaveType = Await _productRepository.GetOrCreateAdjustmentTypeAsync(adjustmentTypeName:=$"Cashout {LeaveTypeEnum.Parental.Type}",
                organizationId:=z_OrganizationID,
                userId:=z_User)
            Dim adjustments = paystubs.Select(Function(p) ActualAdjustment.NewActualAdjustment(organizationId:=z_OrganizationID, userId:=z_User, amount:=models.FirstOrDefault(Function(m) CBool(m.RowID = p.EmployeeID)).CashoutParental,
                comment:=String.Empty,
                productId:=cashoutParentalLeaveType.RowID,
                paystubId:=p.RowID)).
                ToList()
            Await _adjustmentDataService.AppendManyAsync(organizationId:=z_OrganizationID,
                userId:=z_User,
                payPeriodId:=_payPerioId,
                actualAdjustments:=adjustments)

            cashoutUnusedLeaves.AddRange(models.Select(Function(m) m.CashoutParentalLeave))
        End If

        Await _payStubForm.GeneratePayroll(payPeriodId:=_payPerioId)

        'Await _cashoutUnusedLeaveRepository.SaveManyAsync(data:=cashoutUnusedLeaves)

        Await _cashoutUnusedLeaveRepository.ConsumeLeaveBalanceAsync(organizationId:=z_OrganizationID,
            userId:=z_User,
            payPeriodId:=_payPerioId,
            data:=cashoutUnusedLeaves)

        Dim isEnable = MDIPrimaryForm.Enabled
        'btnReset.Enabled = isEnable
        'btnClose.Enabled = isEnable
    End Sub

    Private Class EmployeeModel

        Private ReadOnly _employee As Employee
        Private ReadOnly _salary As Salary
        Private _isTicked As Boolean

        Sub New(payPeriodId As Integer,
            Employee As Employee,
            salaries As List(Of Salary),
            cashoutLeaves As List(Of CashoutUnusedLeave),
            leaveLedgers As ICollection(Of LeaveLedger),
            paystub As Paystub)
            _employee = Employee

            _salary = salaries.FirstOrDefault(Function(s) CBool(s.EmployeeID = Employee.RowID))

            If _salary IsNot Nothing Then
                Dim actualRate = _salary.TotalSalary / _salary.BasicSalary
                Dim monthlyRate1 = actualRate * PayrollTools.GetEmployeeMonthlyRate(employee:=_employee, salary:=_salary, isActual:=True)
                MonthlyRate = monthlyRate1
                DailyRate = PayrollTools.GetDailyRate(monthlyRate:=monthlyRate1, workDaysPerYear:=_employee.WorkDaysPerYear)
                HourlyRate = DailyRate / 8
                CashoutVacation = _employee.LeaveBalance * HourlyRate
                CashoutSick = _employee.SickLeaveBalance * HourlyRate
                CashoutOthers = _employee.OtherLeaveBalance * HourlyRate
                CashoutParental = _employee.MaternityLeaveBalance * HourlyRate
            End If

            HasCashoutLeaves = cashoutLeaves.Any()
            IsTicked = HasCashoutLeaves

            Dim cashoutUnusedVacation = cashoutLeaves.
                Where(Function(c) c.IsVacation).
                FirstOrDefault(Function(c) c.EmployeeID = _employee.RowID.Value)
            If cashoutUnusedVacation IsNot Nothing Then
                CashoutVacationLeave = cashoutUnusedVacation
                VacationLeaveAllowance = cashoutUnusedVacation.LeaveHours
                VacationLeaveBalance = cashoutUnusedVacation.LeaveHours
                CashoutVacation = cashoutUnusedVacation.Amount
            Else
                CashoutVacationLeave = CashoutUnusedLeave.NewCashoutUnusedLeave(organizationId:=z_OrganizationID,
                    employeeId:=_employee.RowID.Value,
                    leaveLedgerId:=leaveLedgers.FirstOrDefault(Function(l) l.IsVacation).RowID.Value,
                    paystubId:=paystub?.RowID,
                    payPeriodId:=payPeriodId,
                    leaveHours:=_employee.LeaveBalance,
                    amount:=CashoutVacation)
                VacationLeaveAllowance = _employee.VacationLeaveAllowance
                VacationLeaveBalance = _employee.LeaveBalance
            End If

            Dim cashoutUnusedSick = cashoutLeaves.
                Where(Function(c) c.IsSick).
                FirstOrDefault(Function(c) c.EmployeeID = _employee.RowID.Value)
            If cashoutUnusedSick IsNot Nothing Then
                CashoutSickLeave = cashoutUnusedSick
                SickLeaveAllowance = cashoutUnusedSick.LeaveHours
                SickLeaveBalance = cashoutUnusedSick.LeaveHours
                CashoutSick = cashoutUnusedSick.Amount
            Else
                CashoutSickLeave = CashoutUnusedLeave.NewCashoutUnusedLeave(organizationId:=z_OrganizationID,
                    employeeId:=_employee.RowID.Value,
                    leaveLedgerId:=leaveLedgers.FirstOrDefault(Function(l) l.IsSick).RowID.Value,
                    paystubId:=paystub?.RowID,
                    payPeriodId:=payPeriodId,
                    leaveHours:=_employee.LeaveBalance,
                    amount:=CashoutSick)
                SickLeaveAllowance = _employee.SickLeaveAllowance
                SickLeaveBalance = _employee.SickLeaveBalance
            End If

            Dim cashoutUnusedOthers = cashoutLeaves.
                Where(Function(c) c.IsOthers).
                FirstOrDefault(Function(c) c.EmployeeID = _employee.RowID.Value)
            If cashoutUnusedOthers IsNot Nothing Then
                CashoutOthersLeave = cashoutUnusedOthers
                OthersLeaveAllowance = cashoutUnusedOthers.LeaveHours
                OthersLeaveBalance = cashoutUnusedOthers.LeaveHours
                CashoutOthers = cashoutUnusedOthers.Amount
            Else
                CashoutOthersLeave = CashoutUnusedLeave.NewCashoutUnusedLeave(organizationId:=z_OrganizationID,
                    employeeId:=_employee.RowID.Value,
                    leaveLedgerId:=leaveLedgers.FirstOrDefault(Function(l) l.IsOthers).RowID.Value,
                    paystubId:=paystub?.RowID,
                    payPeriodId:=payPeriodId,
                    leaveHours:=_employee.LeaveBalance,
                    amount:=CashoutOthers)
                OthersLeaveAllowance = _employee.OtherLeaveAllowance
                OthersLeaveBalance = _employee.OtherLeaveBalance
            End If

            Dim cashoutUnusedParental = cashoutLeaves.
                Where(Function(c) c.IsParental).
                FirstOrDefault(Function(c) c.EmployeeID = _employee.RowID.Value)
            If cashoutUnusedParental IsNot Nothing Then
                CashoutParentalLeave = cashoutUnusedParental
                ParentalLeaveAllowance = cashoutUnusedParental.LeaveHours
                ParentalLeaveBalance = cashoutUnusedParental.LeaveHours
                CashoutParental = cashoutUnusedParental.Amount
            Else
                CashoutParentalLeave = CashoutUnusedLeave.NewCashoutUnusedLeave(organizationId:=z_OrganizationID,
                    employeeId:=_employee.RowID.Value,
                    leaveLedgerId:=leaveLedgers.FirstOrDefault(Function(l) l.IsParental).RowID.Value,
                    paystubId:=paystub?.RowID,
                    payPeriodId:=payPeriodId,
                    leaveHours:=_employee.LeaveBalance,
                    amount:=CashoutParental)
                ParentalLeaveAllowance = _employee.MaternityLeaveAllowance
                ParentalLeaveBalance = _employee.MaternityLeaveBalance
            End If

            Dim oneCashoutLeaves = cashoutLeaves.FirstOrDefault
            If oneCashoutLeaves IsNot Nothing Then PeriodDateText = $"{oneCashoutLeaves.PayPeriod.PayFromDate.ToShortDateString} - {oneCashoutLeaves.PayPeriod.PayToDate.ToShortDateString}"

            IsCashedOut = If(PeriodDateText Is Nothing, False, PeriodDateText.Trim().Length > 0)
        End Sub

        Public ReadOnly Property RowID As Integer
            Get
                Return _employee.RowID.Value
            End Get
        End Property

        Public ReadOnly Property EmployeeNo As String
            Get
                Return _employee.EmployeeNo
            End Get
        End Property

        Public ReadOnly Property LastName As String
            Get
                Return _employee.LastName
            End Get
        End Property

        Public ReadOnly Property FirstName As String
            Get
                Return _employee.FirstName
            End Get
        End Property

        Public ReadOnly Property StartDate As Date
            Get
                Return _employee.StartDate
            End Get
        End Property

        Public ReadOnly Property DateRegularized As Date?
            Get
                Return _employee.DateRegularized
            End Get
        End Property

        Public ReadOnly Property VacationLeaveAllowance As Decimal
        Public ReadOnly Property VacationLeaveBalance As Decimal
        Public ReadOnly Property SickLeaveAllowance As Decimal
        Public ReadOnly Property SickLeaveBalance As Decimal
        Public ReadOnly Property OthersLeaveAllowance As Decimal
        Public ReadOnly Property OthersLeaveBalance As Decimal
        Public ReadOnly Property ParentalLeaveAllowance As Decimal
        Public ReadOnly Property ParentalLeaveBalance As Decimal

        Public ReadOnly Property MonthlyRate As Decimal
        Public ReadOnly Property DailyRate As Decimal
        Public ReadOnly Property HourlyRate As Decimal
        Public ReadOnly Property CashoutVacation As Decimal
        Public ReadOnly Property CashoutSick As Decimal
        Public ReadOnly Property CashoutOthers As Decimal
        Public ReadOnly Property CashoutParental As Decimal

        Public Property IsTicked As Boolean
            Get
                Return _isTicked
            End Get
            Set(value As Boolean)
                _isTicked = value
                If HasCashoutLeaves Then _isTicked = True
            End Set
        End Property

        Public ReadOnly Property CashoutVacationLeave As CashoutUnusedLeave
        Public ReadOnly Property CashoutSickLeave As CashoutUnusedLeave
        Public ReadOnly Property CashoutOthersLeave As CashoutUnusedLeave
        Public ReadOnly Property CashoutParentalLeave As CashoutUnusedLeave
        Public ReadOnly Property HasCashoutLeaves As Boolean
        Public ReadOnly Property PeriodDateText As String
        Public ReadOnly Property IsCashedOut As Boolean
    End Class

End Class
