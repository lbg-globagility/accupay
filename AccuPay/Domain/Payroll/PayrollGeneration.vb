Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Loans
Imports AccuPay.Payroll
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

    Private _paystubActual As PaystubActual

    Sub New(employee As Employee,
            resources As PayrollResources,
            Optional paystubForm As PayStubForm = Nothing)
        _formCaller = paystubForm
        _notifyMainWindow = AddressOf paystubForm.ProgressCounter

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
                New Result(_employee.EmployeeNo, _employee.Fullname, ResultStatus.Success, ""))
        Catch ex As Exception
            logger.Error("DoProcess", ex)

            _formCaller.BeginInvoke(
                _notifyMainWindow,
                New Result(_employee.EmployeeNo, _employee.Fullname, ResultStatus.Error, ex.Message))
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
            End If

            _paystubActual = New PaystubActual()

            _paystub.EmployeeID = _employee.RowID

            If _employee.IsMonthly Or _employee.IsFixed Then
                If _employee.WorkDaysPerYear > 0 Then
                    Dim workDaysPerPayPeriod = _employee.WorkDaysPerYear / CalendarConstants.MonthsInAYear / CalendarConstants.SemiMonthlyPayPeriodsPerMonth

                    _paystub.BasicHours = workDaysPerPayPeriod * 8
                End If

                _paystub.BasicPay = AccuMath.CommercialRound((_salary.BasicSalary / 2), 2)

            ElseIf _employee.IsDaily Then

                ComputeBasicHours()

                _paystub.BasicPay = _timeEntries.Sum(Function(t) t.BasicDayPay)
            End If

            ComputeHours()

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
                    _paystub.RegularHours =
                        _paystub.BasicHours - _paystub.LeaveHours

                    _paystub.RegularPay =
                        _paystub.BasicPay - _paystub.LeavePay

                    _paystub.TotalEarnings = (_paystub.BasicPay + _paystub.AdditionalPay) - _paystub.BasicDeductions
                End If

            ElseIf _employee.IsDaily Then
                _paystub.TotalEarnings =
                    _paystub.RegularPay +
                    _paystub.LeavePay +
                    _paystub.AdditionalPay
            End If

            CalculateAllowances()

            Dim socialSecurityCalculator = New SssCalculator(_settings, _resources.SocialSecurityBrackets)
            socialSecurityCalculator.Calculate(_paystub, _previousPaystub, _salary, _employee, _payPeriod)

            Dim philHealthCalculator = New PhilHealthCalculator(New PhilHealthPolicy(_settings), _resources.PhilHealthBrackets)
            philHealthCalculator.Calculate(_salary, _paystub, _previousPaystub, _employee, _payPeriod, _allowances)

            Dim hdmfCalculator = New HdmfCalculator()
            hdmfCalculator.Calculate(_salary, _paystub, _employee, _payPeriod)

            Dim withholdingTaxCalculator = New WithholdingTaxCalculator(_settings, _resources.FilingStatuses, _resources.WithholdingTaxBrackets, _resources.DivisionMinimumWages)
            withholdingTaxCalculator.Calculate(_paystub, _previousPaystub, _employee, _payPeriod, _salary)

            Dim newLoanTransactions = ComputeLoans()

            If _paystub.TotalEarnings < 0 Then
                _paystub.TotalEarnings = 0
            End If

            Dim grandTotalAllowance = _paystub.TotalAllowance + _paystub.TotalTaxableAllowance

            _paystub.GrossPay = _paystub.TotalEarnings + _paystub.TotalBonus + grandTotalAllowance
            _paystub.TotalAdjustments = If(_paystub?.Adjustments.Sum(Function(a) a.Amount), 0)
            _paystub.NetPay = AccuMath.CommercialRound(_paystub.GrossPay - _paystub.NetDeductions + _paystub.TotalAdjustments)

            Using context = New PayrollContext()
                UpdateLeaveLedger(context)

                If _paystub.RowID.HasValue Then
                    context.Entry(_paystub).State = EntityState.Modified

                    If _paystub.ThirteenthMonthPay IsNot Nothing Then
                        context.Entry(_paystub.ThirteenthMonthPay).State = EntityState.Modified
                    End If
                Else
                    context.Paystubs.Add(_paystub)
                End If

                context.Entry(_paystub).Collection(Function(p) p.AllowanceItems).Load()
                context.Set(Of AllowanceItem).RemoveRange(_paystub.AllowanceItems)

                _paystub.AllowanceItems = _allowanceItems

                UpdatePaystubItems(context)

                For Each newLoanTransaction In newLoanTransactions
                    context.LoanTransactions.Add(newLoanTransaction)
                Next

                ComputeThirteenthMonthPay()

                context.SaveChanges()
            End Using
        Catch ex As PayrollException
            Throw
        Catch ex As Exception
            Throw New Exception($"Failure to generate paystub for employee {_paystub.EmployeeID}", ex)
        End Try
    End Sub

    Private Sub ComputeBasicHours()
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
        _paystub.RegularHours = _timeEntries.Sum(Function(t) t.RegularHours)
        _paystub.RegularPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.RegularPay))
        _paystubActual.RegularPay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.RegularPay))

        _paystub.OvertimeHours = _timeEntries.Sum(Function(t) t.OvertimeHours)
        _paystub.OvertimePay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.OvertimePay))
        _paystubActual.OvertimePay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.RegularPay))

        _paystub.NightDiffHours = _timeEntries.Sum(Function(t) t.NightDiffHours)
        _paystub.NightDiffPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.NightDiffPay))
        _paystubActual.NightDiffPay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.NightDiffPay))

        _paystub.NightDiffOvertimeHours = _timeEntries.Sum(Function(t) t.NightDiffOTHours)
        _paystub.NightDiffOvertimePay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.NightDiffOTPay))
        _paystubActual.NightDiffOTPay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.NightDiffOTPay))

        _paystub.RestDayHours = _timeEntries.Sum(Function(t) t.RestDayHours)
        _paystub.RestDayPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.RestDayPay))
        _paystubActual.RestDayPay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.RestDayPay))

        _paystub.RestDayOTHours = _timeEntries.Sum(Function(t) t.RestDayOTHours)
        _paystub.RestDayOTPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.RestDayOTPay))
        _paystubActual.RestDayOTPay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.RestDayOTPay))

        _paystub.SpecialHolidayHours = _timeEntries.Sum(Function(t) t.SpecialHolidayHours)
        _paystub.SpecialHolidayPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.SpecialHolidayPay))
        _paystubActual.SpecialHolidayPay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.SpecialHolidayPay))

        _paystub.SpecialHolidayOTHours = _timeEntries.Sum(Function(t) t.SpecialHolidayOTHours)
        _paystub.SpecialHolidayOTPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.SpecialHolidayOTPay))
        _paystubActual.SpecialHolidayOTPay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.SpecialHolidayOTPay))

        _paystub.RegularHolidayHours = _timeEntries.Sum(Function(t) t.RegularHolidayHours)
        _paystub.RegularHolidayPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.RegularHolidayPay))
        _paystubActual.RegularHolidayPay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.RegularHolidayPay))

        _paystub.RegularHolidayOTHours = _timeEntries.Sum(Function(t) t.RegularHolidayOTHours)
        _paystub.RegularHolidayOTPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.RegularHolidayOTPay))
        _paystubActual.RegularHolidayOTPay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.RegularHolidayOTPay))

        _paystub.HolidayPay = _timeEntries.Sum(Function(t) t.HolidayPay)

        _paystub.LeaveHours = _timeEntries.Sum(Function(t) t.TotalLeaveHours)
        _paystub.LeavePay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.LeavePay))
        _paystubActual.LeavePay = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.LeavePay))

        _paystub.LateHours = _timeEntries.Sum(Function(t) t.LateHours)
        _paystub.LateDeduction = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.LateDeduction))
        _paystubActual.LateDeduction = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.LateDeduction))

        _paystub.UndertimeHours = _timeEntries.Sum(Function(t) t.UndertimeHours)
        _paystub.UndertimeDeduction = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.UndertimeDeduction))
        _paystubActual.UndertimeDeduction = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.UndertimeDeduction))

        _paystub.AbsentHours = _timeEntries.Sum(Function(t) t.AbsentHours)
        _paystub.AbsenceDeduction = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.AbsentDeduction))
        _paystubActual.AbsenceDeduction = AccuMath.CommercialRound(_actualtimeentries.Sum(Function(t) t.AbsentDeduction))
    End Sub

    Private Sub CalculateAllowances()
        Dim dailyCalculator = New DailyAllowanceCalculator(_settings, _payrateCalendar, _previousTimeEntries)
        Dim semiMonthlyCalculator = New SemiMonthlyAllowanceCalculator(New AllowancePolicy(_settings), _employee, _paystub, _payPeriod, _payrateCalendar, _timeEntries)

        For Each allowance In _allowances
            Dim item = New AllowanceItem() With {
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .LastUpdBy = z_User,
                .PayPeriodID = _payPeriod.RowID,
                .AllowanceID = allowance.RowID,
                .Paystub = _paystub,
                .IsTaxable = allowance.Product.IsTaxable,
                .IsThirteenthMonthPay = allowance.Product.IsThirteenthMonthPay
            }

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

        _paystub.TotalTaxableAllowance = AccuMath.CommercialRound(
            _allowanceItems.
                Where(Function(a) a.IsTaxable).
                Sum(Function(a) a.Amount))

        _paystub.TotalAllowance = AccuMath.CommercialRound(
            _allowanceItems.
                Where(Function(a) Not a.IsTaxable).
                Sum(Function(a) a.Amount))
    End Sub

    Private Function CalculateOneTimeAllowances(allowance As Allowance) As Decimal
        Return allowance.Amount
    End Function

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

                Dim newTransaction = New LeaveTransaction() With {
                    .OrganizationID = z_OrganizationID,
                    .Created = Date.Now,
                    .EmployeeID = leave.EmployeeID,
                    .PayPeriodID = _payPeriod.RowID,
                    .ReferenceID = leave.RowID,
                    .TransactionDate = transactionDate.Value,
                    .Type = LeaveTransactionType.Debit,
                    .Amount = totalLeaveHours,
                    .Balance = If(ledger?.LastTransaction?.Balance, 0) - totalLeaveHours
                }

                ledger.LeaveTransactions.Add(newTransaction)
                ledger.LastTransaction = newTransaction
            End If
        Next
    End Sub

    Private Function ComputeLoans() As IList(Of LoanTransaction)
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
        Dim vacationLeaveProduct = _products.Where(Function(p) p.PartNo = ProductConstant.VACATION_LEAVE_PART_NO).FirstOrDefault()
        Dim sickLeaveProduct = _products.Where(Function(p) p.PartNo = ProductConstant.SICK_LEAVE_PART_NO).FirstOrDefault()

        context.Entry(_paystub).Collection(Function(p) p.PaystubItems).Load()
        context.Set(Of PaystubItem).RemoveRange(_paystub.PaystubItems)

        Dim vacationLeaveBalance = context.PaystubItems.
            Where(Function(p) p.Product.PartNo = ProductConstant.VACATION_LEAVE_PART_NO).
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
            Where(Function(p) p.Product.PartNo = ProductConstant.SICK_LEAVE_PART_NO).
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

    Private Sub ComputeThirteenthMonthPay()
        If _paystub.ThirteenthMonthPay Is Nothing Then
            _paystub.ThirteenthMonthPay = New ThirteenthMonthPay() With {
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User
            }
        Else
            _paystub.ThirteenthMonthPay.LastUpdBy = z_User
        End If

        Dim contractualEmployementStatuses = New String() {"Contractual", "SERVICE CONTRACT"}

        Dim thirteenthMonthAmount = 0D

        If _employee.IsDaily Then

            If contractualEmployementStatuses.Contains(_employee.EmploymentStatus) Then

                thirteenthMonthAmount = _timeEntries.
                                            Where(Function(t) Not t.IsRestDay).
                                            Sum(Function(t) t.BasicDayPay + t.LeavePay)
            Else

                Dim thirteenthMonthAmountRunningTotal As Decimal = 0

                For Each actualTimeEntry In _actualtimeentries

                    Dim timeEntry = _timeEntries.Where(Function(t) t.Date = actualTimeEntry.Date).FirstOrDefault

                    If timeEntry Is Nothing OrElse timeEntry.IsRestDay Then Continue For

                    thirteenthMonthAmount += actualTimeEntry.BasicDayPay + actualTimeEntry.LeavePay

                Next

            End If

        ElseIf _employee.IsMonthly Or _employee.IsFixed Then

            Dim trueSalary = _salary.TotalSalary
            Dim basicPay = trueSalary / CalendarConstants.SemiMonthlyPayPeriodsPerMonth

            Dim totalDeductions = _actualtimeentries.Sum(Function(t) t.LateDeduction + t.UndertimeDeduction + t.AbsentDeduction)

            Dim additionalAmount As Decimal
            If (_settings.GetBoolean("ThirteenthMonthPolicy.IsAllowancePaid")) Then

                additionalAmount = _allowanceItems.Sum(Function(a) a.Amount)

            End If

            thirteenthMonthAmount = ((basicPay + additionalAmount) - totalDeductions)
        End If

        Dim allowanceAmount = _allowanceItems.Where(Function(a) a.IsThirteenthMonthPay).Sum(Function(a) a.Amount)
        thirteenthMonthAmount += allowanceAmount

        _paystub.ThirteenthMonthPay.BasicPay = thirteenthMonthAmount
        _paystub.ThirteenthMonthPay.Amount = thirteenthMonthAmount / CalendarConstants.MonthsInAYear
        _paystub.ThirteenthMonthPay.Paystub = _paystub
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

    Public Enum ResultStatus
        Success
        Warning
        [Error]
    End Enum

End Class