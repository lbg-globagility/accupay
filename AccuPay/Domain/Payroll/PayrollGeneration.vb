Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Loans
Imports AccuPay.Payroll
Imports AccuPay.Utils
Imports log4net
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class PayrollGeneration

    Private Delegate Sub NotifyMainWindow(result As Result)

    Private Shared ReadOnly logger As ILog = LogManager.GetLogger("PayrollLogger")

    Private ReadOnly _notifyMainWindow As NotifyMainWindow

    Private ReadOnly _employee As Employee

    Private ReadOnly _resources As PayrollResources

    Private ReadOnly _salary As Salary

    Private ReadOnly _loanSchedules As ICollection(Of LoanSchedule)

    Private ReadOnly _loanTransactions As ICollection(Of LoanTransaction)

    Private ReadOnly _previousTimeEntries As ICollection(Of TimeEntry)

    Private ReadOnly _formCaller As Form

    Private ReadOnly _payPeriod As PayPeriod

    Private ReadOnly _products As IEnumerable(Of Product)

    Private ReadOnly _settings As ListOfValueCollection

    Private ReadOnly _timeEntries As ICollection(Of TimeEntry)

    Private ReadOnly _employeeDutySchedules As ICollection(Of EmployeeDutySchedule)

    Private ReadOnly _payrateCalendar As PayratesCalendar

    Private ReadOnly _allowances As ICollection(Of Allowance)

    Private ReadOnly _allowanceItems As ICollection(Of AllowanceItem) = New List(Of AllowanceItem)

    Private ReadOnly _actualtimeentries As ICollection(Of ActualTimeEntry)

    Private ReadOnly _policy As TimeEntryPolicy

    Private ReadOnly _previousPaystub As Paystub

    Private _paystub As Paystub

    Sub New(employee As Employee,
            resources As PayrollResources,
            Optional paystubForm As PayStubForm = Nothing)
        _formCaller = paystubForm

        If paystubForm IsNot Nothing Then
            _notifyMainWindow = AddressOf paystubForm.ProgressCounter

        End If

        _employee = employee

        _resources = resources

        _loanSchedules = resources.LoanSchedules.
            Where(Function(l) Nullable.Equals(l.EmployeeID, _employee.RowID)).
            ToList()

        _loanTransactions = resources.LoanTransactions.
            Where(Function(t) Nullable.Equals(t.EmployeeID, _employee.RowID)).
            ToList()

        _salary = resources.Salaries.
            FirstOrDefault(Function(s) CBool(s.EmployeeID = _employee.RowID))

        _products = resources.Products

        _paystub = resources.Paystubs.FirstOrDefault(
            Function(p) Nullable.Equals(p.EmployeeID, _employee.RowID))

        _previousPaystub = resources.PreviousPaystubs.FirstOrDefault(
            Function(p) CBool(p.EmployeeID = _employee.RowID))

        _settings = New ListOfValueCollection(resources.ListOfValues)
        _policy = New TimeEntryPolicy(_settings)
        _payPeriod = resources.PayPeriod

        _previousTimeEntries = resources.TimeEntries.
            Where(Function(t) CBool(t.EmployeeID = _employee.RowID)).
            ToList()

        _timeEntries = resources.TimeEntries.
            Where(Function(t) CBool(t.EmployeeID = _employee.RowID)).
            Where(Function(t) _payPeriod.PayFromDate <= t.Date And t.Date <= _payPeriod.PayToDate).
            OrderBy(Function(t) t.Date).
            ToList()

        _employeeDutySchedules = resources.EmployeeDutySchedule.
            Where(Function(t) CBool(t.EmployeeID = _employee.RowID)).
            Where(Function(t) _payPeriod.PayFromDate <= t.DateSched AndAlso t.DateSched <= _payPeriod.PayToDate).
            OrderBy(Function(t) t.DateSched).
            ToList()

        _actualtimeentries = resources.ActualTimeEntries.
            Where(Function(t) CBool(t.EmployeeID = _employee.RowID)).
            ToList()

        _payrateCalendar = New PayratesCalendar(CType(resources.PayRates, IList(Of PayRate)))

        _allowances = resources.Allowances.
            Where(Function(a) CBool(a.EmployeeID = _employee.RowID)).
            ToList()
    End Sub

    Public Sub DoProcess()
        Try
            GeneratePayStub()

            _formCaller.BeginInvoke(
                _notifyMainWindow,
                New Result(_employee.EmployeeNo, _employee.FullNameWithMiddleInitialLastNameFirst, ResultStatus.Success, ""))
        Catch ex As Exception
            logger.Error("DoProcess", ex)

            _formCaller.BeginInvoke(
                _notifyMainWindow,
                New Result(_employee.EmployeeNo, _employee.FullNameWithMiddleInitialLastNameFirst, ResultStatus.Error, ex.Message))
        End Try
    End Sub

    Private Sub GeneratePayStub()
        Try
            If _salary Is Nothing Then
                Throw New PayrollException("Employee has no salary for this cutoff.")
            End If

            If (Not _timeEntries.Any()) And (_employee.IsDaily Or _employee.IsMonthly) Then
                Throw New PayrollException("No time entries.")
            End If

            If _employee.Position Is Nothing Then
                Throw New PayrollException("Employee has no job position set.")
            End If

            If _paystub Is Nothing Then
                _paystub = New Paystub() With {
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .LastUpdBy = z_User,
                .EmployeeID = _employee.RowID,
                .PayPeriodID = _payPeriod.RowID,
                .PayFromdate = _payPeriod.PayFromDate,
                .PayToDate = _payPeriod.PayToDate
            }

                _paystub.Actual = New PaystubActual With {
                .OrganizationID = z_OrganizationID,
                .EmployeeID = _employee.RowID,
                .PayPeriodID = _payPeriod.RowID,
                .PayFromDate = _payPeriod.PayFromDate,
                .PayToDate = _payPeriod.PayToDate
            }
            End If

            _paystub.EmployeeID = _employee.RowID

            Dim newLoanTransactions = ComputePayroll()

            SavePayroll(newLoanTransactions)
        Catch ex As PayrollException
            Throw
        Catch ex As Exception
            Throw New Exception($"Failure to generate paystub for employee {_paystub.EmployeeID}", ex)
        End Try
    End Sub

    Public Sub SavePayroll(newLoanTransactions As List(Of LoanTransaction))
        Using context = New PayrollContext()

            If _paystub.RowID.HasValue Then
                context.Entry(_paystub).State = EntityState.Modified
                context.Entry(_paystub.Actual).State = EntityState.Modified

                If _paystub.ThirteenthMonthPay IsNot Nothing Then
                    context.Entry(_paystub.ThirteenthMonthPay).State = EntityState.Modified
                End If
            Else
                context.Paystubs.Add(_paystub)
            End If

            context.Entry(_paystub).Collection(Function(p) p.AllowanceItems).Load()
            context.Set(Of AllowanceItem).RemoveRange(_paystub.AllowanceItems)

            _paystub.AllowanceItems = _allowanceItems

            For Each newLoanTransaction In newLoanTransactions
                context.LoanTransactions.Add(newLoanTransaction)
            Next

            If _resources.SystemOwner.CurrentSystemOwner <> SystemOwner.Benchmark Then
                UpdateLeaveLedger(context)
                UpdatePaystubItems(context)
            Else
                UpdateBenchmarkLeaveLedger(context)

            End If

            context.SaveChanges()
        End Using
    End Sub

    Public Function ComputePayroll(
                        Optional paystub As Paystub = Nothing,
                        Optional allowanceItems As ICollection(Of AllowanceItem) = Nothing) As List(Of LoanTransaction)

        Dim newLoanTransactions As New List(Of LoanTransaction)

        If paystub IsNot Nothing Then

            _paystub = paystub

        End If

        If _resources.SystemOwner.CurrentSystemOwner <> SystemOwner.Benchmark Then

            ComputeBasicHoursAndPay()

            ComputeHours()

            ComputeTotalEarnings()

        End If

        CalculateAllowances(allowanceItems)

        newLoanTransactions = ComputeLoans()

        If _paystub.TotalEarnings < 0 Then
            _paystub.TotalEarnings = 0
        End If

        Dim grandTotalAllowance = _paystub.TotalAllowance + _paystub.TotalTaxableAllowance

        'gross pay and total earnings should be higher than the goverment deduction calculators
        'since it is sometimes used in computing the basis pay for the deductions
        'depending on the organization's policy
        _paystub.GrossPay = _paystub.TotalEarnings + _paystub.TotalBonus + grandTotalAllowance
        _paystub.TotalAdjustments = _paystub.Adjustments.Sum(Function(a) a.Amount)

        Dim socialSecurityCalculator = New SssCalculator(_settings, _resources.SocialSecurityBrackets)
        socialSecurityCalculator.Calculate(_paystub, _previousPaystub, _salary, _employee, _payPeriod)

        Dim philHealthCalculator = New PhilHealthCalculator(New PhilHealthPolicy(_settings), _resources.PhilHealthBrackets)
        philHealthCalculator.Calculate(_salary, _paystub, _previousPaystub, _employee, _payPeriod, _allowances)

        Dim hdmfCalculator = New HdmfCalculator()
        hdmfCalculator.Calculate(_salary, _paystub, _employee, _payPeriod, _settings)

        Dim withholdingTaxCalculator = New WithholdingTaxCalculator(_settings, _resources.FilingStatuses, _resources.WithholdingTaxBrackets, _resources.DivisionMinimumWages)
        withholdingTaxCalculator.Calculate(_paystub, _previousPaystub, _employee, _payPeriod, _salary)

        _paystub.NetPay = AccuMath.CommercialRound(_paystub.GrossPay - _paystub.NetDeductions + _paystub.TotalAdjustments)

        Dim actualCalculator = New PaystubActualCalculator()
        actualCalculator.Compute(_employee, _salary, _settings, _payPeriod, _paystub)

        Dim thirteenthMonthPayCalculator = New ThirteenthMonthPayCalculator()
        thirteenthMonthPayCalculator.Calculate(_employee, _paystub, _timeEntries, _actualtimeentries, _salary, _settings, _allowanceItems)

        Return newLoanTransactions
    End Function

    Private Sub ComputeTotalEarnings()
        If _employee.IsFixed Then

            _paystub.TotalEarnings = _paystub.BasicPay + _paystub.AdditionalPay

        ElseIf _employee.IsMonthly Then

            Dim isFirstPayAsDailyRule = _settings.GetBoolean("Payroll Policy", "isfirstsalarydaily")

            Dim isFirstPay =
                _payPeriod.PayFromDate <= _employee.StartDate And
                _employee.StartDate <= _payPeriod.PayToDate

            If isFirstPay And isFirstPayAsDailyRule Then
                _paystub.TotalEarnings =
                    _paystub.RegularPay +
                    _paystub.LeavePay +
                    _paystub.AdditionalPay
            Else
                _paystub.RegularHours = _paystub.BasicHours - _paystub.LeaveHours

                _paystub.RegularPay = _paystub.BasicPay - _paystub.LeavePay

                _paystub.TotalEarnings = (_paystub.BasicPay + _paystub.AdditionalPay) - _paystub.BasicDeductions
            End If

        ElseIf _employee.IsDaily Then
            _paystub.TotalEarnings =
                _paystub.RegularPay +
                _paystub.LeavePay +
                _paystub.AdditionalPay
        End If
    End Sub

    Private Sub ComputeBasicHoursAndPay()
        If _employee.IsMonthly Or _employee.IsFixed Then
            If _employee.WorkDaysPerYear > 0 Then
                Dim workDaysPerPayPeriod = _employee.WorkDaysPerYear / CalendarConstants.MonthsInAYear / CalendarConstants.SemiMonthlyPayPeriodsPerMonth

                _paystub.BasicHours = workDaysPerPayPeriod * 8
            End If

            _paystub.BasicPay = AccuMath.CommercialRound((_salary.BasicSalary / 2), 2)

        ElseIf _employee.IsDaily Then

            ComputeBasicHoursForDay()

            _paystub.BasicPay = _timeEntries.Sum(Function(t) t.BasicDayPay)
        End If
    End Sub

    Private Sub ComputeBasicHoursForDay()
        Dim basicHours = 0D

        For Each timeEntry In _timeEntries

            Dim payrate = _payrateCalendar.Find(timeEntry.Date)
            If Not (timeEntry.IsRestDay Or (timeEntry.TotalLeaveHours > 0) Or
                payrate.IsRegularHoliday Or payrate.IsSpecialNonWorkingHoliday) Then

                basicHours += timeEntry.WorkHours
            End If
        Next

        _paystub.BasicHours = basicHours
    End Sub

    Private Sub ComputeHours()

        Dim paystubRate As New PaystubRate
        paystubRate.Compute(_timeEntries, _salary, _employee, _actualtimeentries)

        _paystub.RegularHours = paystubRate.RegularHours
        _paystub.RegularPay = paystubRate.RegularPay
        _paystub.Actual.RegularPay = paystubRate.ActualRegularPay

        _paystub.OvertimeHours = paystubRate.OvertimeHours
        _paystub.OvertimePay = paystubRate.OvertimePay
        _paystub.Actual.OvertimePay = paystubRate.ActualOvertimePay

        _paystub.NightDiffHours = paystubRate.NightDiffHours
        _paystub.NightDiffPay = paystubRate.NightDiffPay
        _paystub.Actual.NightDiffPay = paystubRate.ActualNightDiffPay

        _paystub.NightDiffOvertimeHours = paystubRate.NightDiffOvertimeHours
        _paystub.NightDiffOvertimePay = paystubRate.NightDiffOvertimePay
        _paystub.Actual.NightDiffOvertimePay = paystubRate.ActualNightDiffOvertimePay

        _paystub.RestDayHours = paystubRate.RestDayHours
        _paystub.RestDayPay = paystubRate.RestDayPay
        _paystub.Actual.RestDayPay = paystubRate.ActualRestDayPay

        _paystub.RestDayOTHours = paystubRate.RestDayOTHours
        _paystub.RestDayOTPay = paystubRate.RestDayOTPay
        _paystub.Actual.RestDayOTPay = paystubRate.ActualRestDayOTPay

        _paystub.SpecialHolidayHours = paystubRate.SpecialHolidayHours
        _paystub.SpecialHolidayPay = paystubRate.SpecialHolidayPay
        _paystub.Actual.SpecialHolidayPay = paystubRate.ActualSpecialHolidayPay

        _paystub.SpecialHolidayOTHours = paystubRate.SpecialHolidayOTHours
        _paystub.SpecialHolidayOTPay = paystubRate.SpecialHolidayOTPay
        _paystub.Actual.SpecialHolidayOTPay = paystubRate.ActualSpecialHolidayOTPay

        _paystub.RegularHolidayHours = paystubRate.RegularHolidayHours
        _paystub.RegularHolidayPay = paystubRate.RegularHolidayPay
        _paystub.Actual.RegularHolidayPay = paystubRate.ActualRegularHolidayPay

        _paystub.RegularHolidayOTHours = paystubRate.RegularHolidayOTHours
        _paystub.RegularHolidayOTPay = paystubRate.RegularHolidayOTPay
        _paystub.Actual.RegularHolidayOTPay = paystubRate.ActualRegularHolidayOTPay

        _paystub.HolidayPay = paystubRate.HolidayPay

        _paystub.LeaveHours = paystubRate.LeaveHours
        _paystub.LeavePay = paystubRate.LeavePay
        _paystub.Actual.LeavePay = paystubRate.ActualLeavePay

        _paystub.LateHours = paystubRate.LateHours
        _paystub.LateDeduction = paystubRate.LateDeduction
        _paystub.Actual.LateDeduction = paystubRate.ActualLateDeduction

        _paystub.UndertimeHours = paystubRate.UndertimeHours
        _paystub.UndertimeDeduction = paystubRate.UndertimeDeduction
        _paystub.Actual.UndertimeDeduction = paystubRate.ActualUndertimeDeduction

        _paystub.AbsentHours = paystubRate.AbsentHours
        _paystub.AbsenceDeduction = paystubRate.AbsenceDeduction
        _paystub.Actual.AbsenceDeduction = paystubRate.ActualAbsenceDeduction
    End Sub

    Private Sub CalculateAllowances(Optional allowanceItems As ICollection(Of AllowanceItem) = Nothing)

        If allowanceItems Is Nothing Then

            CreateAllowanceItems()
        Else

            _allowanceItems.Clear()

            For Each item In allowanceItems

                _allowanceItems.Add(item)

            Next

        End If

        ComputeTotalAllowances()
    End Sub

    Public Shared Function CreateBasicAllowanceItem(
                                paystub As Paystub,
                                payperiodId As Integer?,
                                allowanceId As Integer?) As AllowanceItem

        Return New AllowanceItem() With {
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .LastUpdBy = z_User,
                .Paystub = paystub,
                .PayPeriodID = payperiodId,
                .AllowanceID = allowanceId
            }

    End Function

    Private Sub CreateAllowanceItems()
        Dim dailyCalculator = New DailyAllowanceCalculator(_settings, _payrateCalendar, _previousTimeEntries)
        Dim semiMonthlyCalculator = New SemiMonthlyAllowanceCalculator(New AllowancePolicy(_settings), _employee, _paystub, _payPeriod, _payrateCalendar, _timeEntries)

        For Each allowance In _allowances

            Dim item = CreateBasicAllowanceItem(
                        paystub:=_paystub,
                        payperiodId:=_payPeriod.RowID,
                        allowanceId:=allowance.RowID
            )
            item.IsTaxable = allowance.Product.IsTaxable
            item.IsThirteenthMonthPay = allowance.Product.IsThirteenthMonthPay

            If allowance.IsOneTime Then

                item.Amount = allowance.Amount

            ElseIf allowance.IsDaily Then

                item = dailyCalculator.Compute(_payPeriod, allowance, _employee, _paystub, _timeEntries)

            ElseIf allowance.IsSemiMonthly Then

                item = semiMonthlyCalculator.Calculate(allowance)

            ElseIf allowance.IsMonthly Then

                If allowance.Product.Fixed And _payPeriod.IsEndOfTheMonth Then

                    item.Amount = allowance.Amount
                End If
            Else
                item = Nothing
            End If

            _allowanceItems.Add(item)
        Next
    End Sub

    Private Sub ComputeTotalAllowances()
        _paystub.TotalTaxableAllowance = AccuMath.CommercialRound(
                    _allowanceItems.
                        Where(Function(a) a.IsTaxable).
                        Sum(Function(a) a.Amount))

        _paystub.TotalAllowance = AccuMath.CommercialRound(
            _allowanceItems.
                Where(Function(a) Not a.IsTaxable).
                Sum(Function(a) a.Amount))
    End Sub

    Private Sub UpdateBenchmarkLeaveLedger(context As PayrollContext)
        Dim vacationLedger = context.LeaveLedgers.
                                        Include(Function(l) l.Product).
                                        Include(Function(l) l.LastTransaction).
                                        Where(Function(l) l.EmployeeID.Value = _employee.RowID.Value).
                                        Where(Function(l) l.Product.IsVacationLeave).
                                        FirstOrDefault

        vacationLedger.LeaveTransactions = New List(Of LeaveTransaction)

        If vacationLedger Is Nothing Then

            Throw New Exception($"Vacation ledger for Employee No.: {_employee.EmployeeNo}")

        End If

        'context.RemoveRange(
        '    context.LeaveTransactions.
        '        Where(Function(t) t.LeaveLedgerID.Value = vacationLedger.RowID.Value).
        '        Where(Function(t) t.PayPeriodID.Value = _payPeriod.RowID.Value))

        UpdateLedgerTransaction(
                    employeeId:=_employee.RowID,
                    leaveId:=Nothing,
                    ledger:=vacationLedger,
                    totalLeaveHours:=_paystub.LeaveHours,
                    transactionDate:=_payPeriod.PayToDate)

    End Sub

    Private Sub UpdateLeaveLedger(context As PayrollContext)
        Dim leaves = context.Leaves.
            Where(Function(l) _payPeriod.PayFromDate <= l.StartDate).
            Where(Function(l) l.StartDate <= _payPeriod.PayToDate).
            Where(Function(l) CBool(l.EmployeeID = _employee.RowID)).
            OrderBy(Function(l) l.StartDate).
            ToList()

        Dim leaveIds = leaves.Select(Function(l) l.RowID)

        Dim transactions = (From t In context.LeaveTransactions
                            Where leaveIds.Contains(t.ReferenceID)).ToList()

        Dim employeeId = _employee.RowID
        Dim ledgers = context.LeaveLedgers.
            Include(Function(x) x.Product).
            Include(Function(x) x.LeaveTransactions).
            Include(Function(x) x.LastTransaction).
            Where(Function(x) CBool(x.EmployeeID = employeeId)).
            ToList()

        Dim newLeaveTransactions = New List(Of LeaveTransaction)
        For Each leave In leaves
            ' If a transaction has already been made for the current leave, skip the current leave.
            If transactions.Any(Function(t) Nullable.Equals(t.ReferenceID, leave.RowID)) Then
                Continue For
            Else
                Dim ledger = ledgers.
                    FirstOrDefault(Function(l) l.Product.PartNo = leave.LeaveType)

                'retrieves the time entries within leave date range
                Dim timeEntry = _timeEntries.
                    Where(Function(t) CBool(t.Date >= leave.StartDate And t.Date <= leave.EndDate))

                If timeEntry Is Nothing Then
                    Continue For
                End If

                'summate the leave hours
                Dim totalLeaveHours = timeEntry.Sum(Function(t) t.TotalLeaveHours)

                Dim transactionDate = If(IsDBNull(leave.EndDate), leave.StartDate, leave.EndDate)

                UpdateLedgerTransaction(
                    employeeId:=leave.EmployeeID,
                    leaveId:=leave.RowID,
                    ledger:=ledger,
                    totalLeaveHours:=totalLeaveHours,
                    transactionDate:=transactionDate)
            End If
        Next
    End Sub

    Private Sub UpdateLedgerTransaction(
                    employeeId As Integer?,
                    leaveId As Integer?,
                    ledger As LeaveLedger,
                    totalLeaveHours As Decimal,
                    transactionDate As Date?)

        Dim newTransaction = New LeaveTransaction() With {
                            .OrganizationID = z_OrganizationID,
                            .Created = Date.Now,
                            .EmployeeID = employeeId,
                            .PayPeriodID = _payPeriod.RowID,
                            .ReferenceID = leaveId,
                            .TransactionDate = transactionDate.Value,
                            .Type = LeaveTransactionType.Debit,
                            .Amount = totalLeaveHours,
                            .Balance = If(ledger?.LastTransaction?.Balance, 0) - totalLeaveHours
                        }

        ledger.LeaveTransactions.Add(newTransaction)
        ledger.LastTransaction = newTransaction
    End Sub

    Private Function ComputeLoans() As List(Of LoanTransaction)
        Dim newLoanTransactions = New List(Of LoanTransaction)

        If _loanTransactions.Count > 0 Then
            _paystub.TotalLoans = _loanTransactions.Sum(Function(t) t.Amount)
        Else
            Dim acceptedLoans As String() = {}
            If _payPeriod.IsFirstHalf Then
                acceptedLoans = {"Per pay period", "First half"}
            ElseIf _payPeriod.IsEndOfTheMonth Then
                acceptedLoans = {"Per pay period", "End of the month"}
            End If

            Dim loanSchedules = _loanSchedules.
                Where(Function(l) acceptedLoans.Contains(l.DeductionSchedule)).
                ToList()

            For Each loanSchedule In loanSchedules
                Dim loanTransaction = New LoanTransaction() With {
                    .Created = Date.Now(),
                    .LastUpd = Date.Now(),
                    .OrganizationID = z_OrganizationID,
                    .EmployeeID = _paystub.EmployeeID,
                    .PayPeriodID = _payPeriod.RowID,
                    .LoanScheduleID = loanSchedule.RowID.Value,
                    .LoanPayPeriodLeft = If(loanSchedule.LoanPayPeriodLeft Is Nothing, 0, Convert.ToInt32(loanSchedule.LoanPayPeriodLeft) - 1)
                }

                If loanSchedule.DeductionAmount > loanSchedule.TotalBalanceLeft Then
                    loanTransaction.Amount = loanSchedule.TotalBalanceLeft
                Else
                    loanTransaction.Amount = loanSchedule.DeductionAmount
                End If

                loanTransaction.TotalBalance = loanSchedule.TotalBalanceLeft - loanTransaction.Amount

                newLoanTransactions.Add(loanTransaction)
            Next

            _paystub.TotalLoans = newLoanTransactions.Aggregate(0D, Function(total, x) x.Amount + total)
        End If

        Return newLoanTransactions
    End Function

    Private Sub UpdatePaystubItems(context As PayrollContext)
        Dim vacationLeaveProduct = _products.Where(Function(p) p.PartNo = ProductConstant.VACATION_LEAVE).FirstOrDefault()
        Dim sickLeaveProduct = _products.Where(Function(p) p.PartNo = ProductConstant.SICK_LEAVE).FirstOrDefault()

        context.Entry(_paystub).Collection(Function(p) p.PaystubItems).Load()
        context.Set(Of PaystubItem).RemoveRange(_paystub.PaystubItems)

        Dim vacationLeaveBalance = context.PaystubItems.
            Where(Function(p) p.Product.PartNo = ProductConstant.VACATION_LEAVE).
            Where(Function(p) CBool(p.Paystub.RowID = _paystub.RowID)).
            FirstOrDefault()

        Dim vacationLeaveUsed = _timeEntries.Sum(Function(t) t.VacationLeaveHours)
        Dim newBalance = _employee.LeaveBalance - vacationLeaveUsed

        vacationLeaveBalance = New PaystubItem() With {
            .OrganizationID = z_OrganizationID,
            .ProductID = vacationLeaveProduct.RowID,
            .PayAmount = newBalance,
            .Paystub = _paystub
        }

        _paystub.PaystubItems.Add(vacationLeaveBalance)

        Dim sickLeaveBalance = context.PaystubItems.
            Where(Function(p) p.Product.PartNo = ProductConstant.SICK_LEAVE).
            Where(Function(p) CBool(p.Paystub.RowID = _paystub.RowID)).
            FirstOrDefault()

        Dim sickLeaveUsed = _timeEntries.Sum(Function(t) t.SickLeaveHours)
        Dim newBalance2 = _employee.SickLeaveBalance - sickLeaveUsed

        sickLeaveBalance = New PaystubItem() With {
            .OrganizationID = z_OrganizationID,
            .ProductID = sickLeaveProduct.RowID,
            .PayAmount = newBalance2,
            .Paystub = _paystub
        }

        _paystub.PaystubItems.Add(sickLeaveBalance)
    End Sub

    Private Class PayFrequency
        Public Const SemiMonthly As Integer = 1
        Public Const Monthly As Integer = 2
    End Class

    Private Class SalaryType
        Public Const Fixed As String = "Fixed"
        Public Const Monthly As String = "Monthly"
        Public Const Daily As String = "Daily"
    End Class

    Public Class Result

        Public Property EmployeeNo As String

        Public Property FullName As String

        Public Property Status As ResultStatus

        Public Property Description As String

        Public Sub New(employeeNo As String, fullName As String, status As ResultStatus, description As String)
            Me.EmployeeNo = employeeNo
            Me.FullName = fullName
            Me.Status = status
            Me.Description = description
        End Sub

    End Class

    Private Class PaystubRate
        Implements IPaystubRate
        Public Property RegularHours As Decimal Implements IPaystubRate.RegularHours
        Public Property RegularPay As Decimal Implements IPaystubRate.RegularPay
        Public Property ActualRegularPay As Decimal Implements IPaystubRate.ActualRegularPay

        Public Property OvertimeHours As Decimal Implements IPaystubRate.OvertimeHours
        Public Property OvertimePay As Decimal Implements IPaystubRate.OvertimePay
        Public Property ActualOvertimePay As Decimal Implements IPaystubRate.ActualOvertimePay

        Public Property NightDiffHours As Decimal Implements IPaystubRate.NightDiffHours
        Public Property NightDiffPay As Decimal Implements IPaystubRate.NightDiffPay
        Public Property ActualNightDiffPay As Decimal Implements IPaystubRate.ActualNightDiffPay

        Public Property NightDiffOvertimeHours As Decimal Implements IPaystubRate.NightDiffOvertimeHours
        Public Property NightDiffOvertimePay As Decimal Implements IPaystubRate.NightDiffOvertimePay
        Public Property ActualNightDiffOvertimePay As Decimal Implements IPaystubRate.ActualNightDiffOvertimePay

        Public Property RestDayHours As Decimal Implements IPaystubRate.RestDayHours
        Public Property RestDayPay As Decimal Implements IPaystubRate.RestDayPay
        Public Property ActualRestDayPay As Decimal Implements IPaystubRate.ActualRestDayPay

        Public Property RestDayOTHours As Decimal Implements IPaystubRate.RestDayOTHours
        Public Property RestDayOTPay As Decimal Implements IPaystubRate.RestDayOTPay
        Public Property ActualRestDayOTPay As Decimal Implements IPaystubRate.ActualRestDayOTPay

        Public Property SpecialHolidayHours As Decimal Implements IPaystubRate.SpecialHolidayHours
        Public Property SpecialHolidayPay As Decimal Implements IPaystubRate.SpecialHolidayPay
        Public Property ActualSpecialHolidayPay As Decimal Implements IPaystubRate.ActualSpecialHolidayPay

        Public Property SpecialHolidayOTHours As Decimal Implements IPaystubRate.SpecialHolidayOTHours
        Public Property SpecialHolidayOTPay As Decimal Implements IPaystubRate.SpecialHolidayOTPay
        Public Property ActualSpecialHolidayOTPay As Decimal Implements IPaystubRate.ActualSpecialHolidayOTPay

        Public Property RegularHolidayHours As Decimal Implements IPaystubRate.RegularHolidayHours
        Public Property RegularHolidayPay As Decimal Implements IPaystubRate.RegularHolidayPay
        Public Property ActualRegularHolidayPay As Decimal Implements IPaystubRate.ActualRegularHolidayPay

        Public Property RegularHolidayOTHours As Decimal Implements IPaystubRate.RegularHolidayOTHours
        Public Property RegularHolidayOTPay As Decimal Implements IPaystubRate.RegularHolidayOTPay
        Public Property ActualRegularHolidayOTPay As Decimal Implements IPaystubRate.ActualRegularHolidayOTPay

        Public Property HolidayPay As Decimal Implements IPaystubRate.HolidayPay

        Public Property LeaveHours As Decimal Implements IPaystubRate.LeaveHours
        Public Property LeavePay As Decimal Implements IPaystubRate.LeavePay
        Public Property ActualLeavePay As Decimal Implements IPaystubRate.ActualLeavePay

        Public Property LateHours As Decimal Implements IPaystubRate.LateHours
        Public Property LateDeduction As Decimal Implements IPaystubRate.LateDeduction
        Public Property ActualLateDeduction As Decimal Implements IPaystubRate.ActualLateDeduction

        Public Property UndertimeHours As Decimal Implements IPaystubRate.UndertimeHours
        Public Property UndertimeDeduction As Decimal Implements IPaystubRate.UndertimeDeduction
        Public Property ActualUndertimeDeduction As Decimal Implements IPaystubRate.ActualUndertimeDeduction

        Public Property AbsentHours As Decimal Implements IPaystubRate.AbsentHours
        Public Property AbsenceDeduction As Decimal Implements IPaystubRate.AbsenceDeduction
        Public Property ActualAbsenceDeduction As Decimal Implements IPaystubRate.ActualAbsenceDeduction

        Public Sub Compute(timeEntries As ICollection(Of TimeEntry), salary As Salary, employee As Employee, actualtimeentries As ICollection(Of ActualTimeEntry))

            Me.RegularHours = timeEntries.Sum(Function(t) t.RegularHours)
            Me.RegularPay = PayrollTools.GetHourlyRateByDailyRate(salary, employee) * Me.RegularHours
            Me.ActualRegularPay = PayrollTools.GetHourlyRateByDailyRate(salary, employee, isActual:=True) * Me.RegularHours

            Me.OvertimeHours = timeEntries.Sum(Function(t) t.OvertimeHours)
            Me.OvertimePay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.OvertimePay))
            Me.ActualOvertimePay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.OvertimePay))

            Me.NightDiffHours = timeEntries.Sum(Function(t) t.NightDiffHours)
            Me.NightDiffPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.NightDiffPay))
            Me.ActualNightDiffPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.NightDiffPay))

            Me.NightDiffOvertimeHours = timeEntries.Sum(Function(t) t.NightDiffOTHours)
            Me.NightDiffOvertimePay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.NightDiffOTPay))
            Me.ActualNightDiffOvertimePay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.NightDiffOTPay))

            Me.RestDayHours = timeEntries.Sum(Function(t) t.RestDayHours)
            Me.RestDayPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.RestDayPay))
            Me.ActualRestDayPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.RestDayPay))

            Me.RestDayOTHours = timeEntries.Sum(Function(t) t.RestDayOTHours)
            Me.RestDayOTPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.RestDayOTPay))
            Me.ActualRestDayOTPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.RestDayOTPay))

            Me.SpecialHolidayHours = timeEntries.Sum(Function(t) t.SpecialHolidayHours)
            Me.SpecialHolidayPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.SpecialHolidayPay))
            Me.ActualSpecialHolidayPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.SpecialHolidayPay))

            Me.SpecialHolidayOTHours = timeEntries.Sum(Function(t) t.SpecialHolidayOTHours)
            Me.SpecialHolidayOTPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.SpecialHolidayOTPay))
            Me.ActualSpecialHolidayOTPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.SpecialHolidayOTPay))

            Me.RegularHolidayHours = timeEntries.Sum(Function(t) t.RegularHolidayHours)
            Me.RegularHolidayPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.RegularHolidayPay))
            Me.ActualRegularHolidayPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.RegularHolidayPay))

            Me.RegularHolidayOTHours = timeEntries.Sum(Function(t) t.RegularHolidayOTHours)
            Me.RegularHolidayOTPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.RegularHolidayOTPay))
            Me.ActualRegularHolidayOTPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.RegularHolidayOTPay))

            Me.HolidayPay = timeEntries.Sum(Function(t) t.HolidayPay)

            Me.LeaveHours = timeEntries.Sum(Function(t) t.TotalLeaveHours)
            Me.LeavePay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.LeavePay))
            Me.ActualLeavePay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.LeavePay))

            Me.LateHours = timeEntries.Sum(Function(t) t.LateHours)
            Me.LateDeduction = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.LateDeduction))
            Me.ActualLateDeduction = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.LateDeduction))

            Me.UndertimeHours = timeEntries.Sum(Function(t) t.UndertimeHours)
            Me.UndertimeDeduction = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.UndertimeDeduction))
            Me.ActualUndertimeDeduction = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.UndertimeDeduction))

            Me.AbsentHours = timeEntries.Sum(Function(t) t.AbsentHours)
            Me.AbsenceDeduction = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.AbsentDeduction))
            Me.ActualAbsenceDeduction = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.AbsentDeduction))

        End Sub

    End Class

    Public Enum ResultStatus
        Success
        Warning
        [Error]
    End Enum

End Class