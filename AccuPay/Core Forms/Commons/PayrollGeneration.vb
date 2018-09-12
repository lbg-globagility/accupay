Imports AccuPay.Entity
Imports AccuPay.Loans
Imports log4net
Imports Microsoft.EntityFrameworkCore
Imports AccuPay.Payroll
Imports PayrollSys

Public Class PayrollGeneration

    Private Const PagibigEmployerAmount As Decimal = 100

    Private Shared logger As ILog = LogManager.GetLogger("PayrollLogger")

    Private _employee As DataRow

    Private _employee2 As Employee

    Private _salary As Salary

    Private _allLoanSchedules As ICollection(Of LoanSchedule)

    Private _allLoanTransactions As ICollection(Of LoanTransaction)

    Private allWeeklyAllowances As DataTable
    Private allNoTaxWeeklyAllowances As DataTable

    Private _previousTimeEntries2 As ICollection(Of TimeEntry)

    Private _filingStatuses As DataTable

    Private Delegate Sub NotifyMainWindow(success As Boolean)

    Private _notifyMainWindow As NotifyMainWindow

    Private form_caller As Form

    Private _payPeriod As PayPeriod

    Private _paystub As Paystub

    Private _paystubActual As PaystubActual

    Private _previousPaystub As Paystub

    Private _products As IEnumerable(Of Product)

    Private _socialSecurityBrackets As IEnumerable(Of SocialSecurityBracket)

    Private _philHealthBrackets As IEnumerable(Of PhilHealthBracket)

    Private _withholdingTaxBrackets As IEnumerable(Of WithholdingTaxBracket)

    Private _settings As ListOfValueCollection

    Private _timeEntries As ICollection(Of TimeEntry)

    Private _payRates As IReadOnlyDictionary(Of Date, PayRate)

    Private _allowances As ICollection(Of Allowance)

    Private _allowanceItems As ICollection(Of AllowanceItem) = New List(Of AllowanceItem)

    Private _actualtimeentries As ICollection(Of ActualTimeEntry)

    Sub New(employee As DataRow,
            payPeriodHalfNo As String,
            allWeeklyAllowances As DataTable,
            allNoTaxWeeklyAllowances As DataTable,
            filingStatuses As DataTable,
            resources As PayrollResources,
            Optional pay_stub_frm As PayStubForm = Nothing)
        form_caller = pay_stub_frm

        _employee = employee
        _employee2 = resources.Employees.
            FirstOrDefault(Function(e) e.RowID.ToString() = _employee("RowID"))

        _allLoanSchedules = resources.LoanSchedules
        _allLoanTransactions = resources.LoanTransactions

        Me.allWeeklyAllowances = allWeeklyAllowances
        Me.allNoTaxWeeklyAllowances = allNoTaxWeeklyAllowances

        _filingStatuses = filingStatuses

        _notifyMainWindow = AddressOf pay_stub_frm.ProgressCounter

        _salary = resources.Salaries.FirstOrDefault(Function(s) s.EmployeeID = _employee2.RowID)

        _products = resources.Products

        _paystub = resources.Paystubs.FirstOrDefault(
            Function(p) Nullable.Equals(p.EmployeeID, _employee2.RowID))

        _socialSecurityBrackets = resources.SocialSecurityBrackets
        _philHealthBrackets = resources.PhilHealthBrackets
        _withholdingTaxBrackets = resources.WithholdingTaxBrackets

        _previousPaystub = resources.PreviousPaystubs.FirstOrDefault(
            Function(p) p.EmployeeID = _employee2.RowID)

        _settings = New ListOfValueCollection(resources.ListOfValues)
        _payPeriod = resources.PayPeriod

        _previousTimeEntries2 = resources.TimeEntries.
            Where(Function(t) t.EmployeeID = _employee2.RowID).
            ToList()

        _timeEntries = resources.TimeEntries.
            Where(Function(t) t.EmployeeID = _employee2.RowID).
            Where(Function(t) _payPeriod.PayFromDate <= t.Date And t.Date <= _payPeriod.PayToDate).
            ToList()

        _actualtimeentries = resources.ActualTimeEntries.
            Where(Function(t) t.EmployeeID = _employee2.RowID).
            ToList()

        _payRates = resources.PayRates.ToDictionary(Function(p) p.RateDate)

        _allowances = resources.Allowances.
            Where(Function(a) a.EmployeeID = _employee2.RowID).
            ToList()
    End Sub

    Sub PayrollGeneration_BackgroundWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        DoProcess()
    End Sub

    Public Sub DoProcess()
        Try
            Console.WriteLine($"Generate Paystub for #{_employee2.RowID} - {_employee2.LastName}, {_employee2.FirstName}")

            GeneratePayStub()
            form_caller.BeginInvoke(_notifyMainWindow, True)
        Catch ex As Exception
            logger.Error("DoProcess", ex)
            form_caller.BeginInvoke(_notifyMainWindow, False)
        End Try
    End Sub

    Private Sub GeneratePayStub()
        Try
            If _paystub Is Nothing Then
                _paystub = New Paystub() With {
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User,
                    .LastUpdBy = z_User,
                    .EmployeeID = _employee2.RowID,
                    .PayPeriodID = _payPeriod.RowID,
                    .PayFromdate = _payPeriod.PayFromDate,
                    .PayToDate = _payPeriod.PayToDate
                }
            End If

            _paystubActual = New PaystubActual()

            _paystub.EmployeeID = _employee2.RowID

            CalculateAllowances()

            _paystub.TotalAdjustments = If(_paystub?.Adjustments.Sum(Function(a) a.Amount), 0)

            If (_timeEntries.Count > 0) And (_salary IsNot Nothing) Then

                Dim basicPay = _salary.BasicPay

                ComputeHours()

                If _employee2.IsMonthly Or _employee2.IsFixed Then
                    If _employee2.WorkDaysPerYear > 0 Then
                        Dim workDaysPerPayPeriod = _employee2.WorkDaysPerYear / CalendarConstants.MonthsInAYear / CalendarConstants.SemiMonthlyPayPeriodsPerMonth

                        _paystub.BasicHours = workDaysPerPayPeriod * 8
                    End If

                    _paystub.BasicPay = basicPay
                End If

                If _employee2.EmployeeType = SalaryType.Fixed Then

                    Dim extraPay =
                        _paystub.OvertimePay +
                        _paystub.NightDiffPay +
                        _paystub.NightDiffOvertimePay +
                        _paystub.RestDayPay +
                        _paystub.RestDayOTPay +
                        _paystub.SpecialHolidayPay +
                        _paystub.SpecialHolidayOTPay +
                        _paystub.RegularHolidayPay +
                        _paystub.RegularHolidayOTPay

                    _paystub.TotalEarnings = basicPay + extraPay

                ElseIf _employee2.EmployeeType = SalaryType.Monthly Then

                    Dim isFirstPayAsDailyRule = _settings.GetBoolean("Payroll Policy", "isfirstsalarydaily")

                    Dim isFirstPay =
                        _payPeriod.PayFromDate <= _employee2.StartDate And
                        _employee2.StartDate <= _payPeriod.PayToDate

                    If isFirstPay And isFirstPayAsDailyRule Then
                        _paystub.TotalEarnings = _timeEntries.Sum(Function(t) t.TotalDayPay)
                    Else
                        Dim totalDeduction = _paystub.LateDeduction + _paystub.UndertimeDeduction + _paystub.AbsenceDeduction
                        Dim extraPay =
                            _paystub.OvertimePay +
                            _paystub.NightDiffPay +
                            _paystub.NightDiffOvertimePay +
                            _paystub.RestDayPay +
                            _paystub.RestDayOTPay +
                            _paystub.SpecialHolidayPay +
                            _paystub.SpecialHolidayOTPay +
                            _paystub.RegularHolidayPay +
                            _paystub.RegularHolidayOTPay

                        _paystub.TotalEarnings = (basicPay + extraPay) - totalDeduction

                        Dim taxablePolicy = If(_settings.GetString("Payroll Policy.paystub.taxableincome"), "Basic Pay")
                    End If

                ElseIf _employee2.EmployeeType = SalaryType.Daily Then
                    _paystub.BasicHours = _timeEntries.
                        Where(Function(t) Not If(t.ShiftSchedule?.IsRestDay, True)).
                        Sum(Function(t) t.ShiftSchedule?.Shift?.WorkHours)

                    _paystub.BasicPay = _timeEntries.Sum(Function(t) t.BasicDayPay)

                    _paystub.TotalEarnings =
                        _paystub.RegularPay +
                        _paystub.OvertimePay +
                        _paystub.NightDiffPay +
                        _paystub.NightDiffOvertimePay +
                        _paystub.RestDayPay +
                        _paystub.RestDayOTPay +
                        _paystub.SpecialHolidayPay +
                        _paystub.SpecialHolidayOTPay +
                        _paystub.RegularHolidayPay +
                        _paystub.RegularHolidayOTPay +
                        _paystub.LeavePay
                End If

                Dim socialSecurityCalculator = New SssCalculator(_socialSecurityBrackets)
                socialSecurityCalculator.Calculate(_settings, _paystub, _previousPaystub, _salary, _employee, _employee2, _payPeriod)

                Dim philHealthCalculator = New PhilHealthCalculator(_philHealthBrackets)
                philHealthCalculator.Calculate(_settings, _salary, _paystub, _previousPaystub, _employee, _employee2, _payPeriod)

                Dim hdmfCalculator = New HdmfCalculator()
                hdmfCalculator.Calculate(_salary, _paystub, _employee, _employee2, _payPeriod)

                Dim withholdingTaxCalculator = New WithholdingTaxCalculator(_filingStatuses, _withholdingTaxBrackets)
                withholdingTaxCalculator.Calculate(_settings, _employee, _paystub, _previousPaystub, _employee2, _payPeriod)
            End If

            Dim newLoanTransactions = ComputeLoans()

            _paystub.GrossPay = _paystub.TotalEarnings + _paystub.TotalBonus + _paystub.TotalAllowance

            Dim governmentContributions = _paystub.SssEmployeeShare + _paystub.PhilHealthEmployeeShare + _paystub.HdmfEmployeeShare
            _paystub.NetPay = AccuMath.CommercialRound(_paystub.GrossPay - (governmentContributions + _paystub.TotalLoans + _paystub.WithholdingTax) + _paystub.TotalAdjustments)

            Dim vacationLeaveProduct = _products.Where(Function(p) p.PartNo = "Vacation leave").FirstOrDefault()
            Dim sickLeaveProduct = _products.Where(Function(p) p.PartNo = "Sick leave").FirstOrDefault()

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
        Catch ex As Exception
            Throw New Exception($"Failure to generate paystub for employee {_paystub.EmployeeID}", ex)
        End Try
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
        For Each allowance In _allowances
            Dim item = New AllowanceItem() With {
                .OrganizationID = z_OrganizationID,
                .Created = Date.Now,
                .LastUpd = Date.Now,
                .CreatedBy = z_User,
                .LastUpdBy = z_User,
                .PayPeriodID = _payPeriod.RowID,
                .AllowanceID = allowance.RowID,
                .Paystub = _paystub
            }

            If allowance.AllowanceFrequency = "One time" Then
                item.Amount = allowance.Amount
            ElseIf allowance.AllowanceFrequency = "Daily" Then
                item = CalculateDailyProratedAllowance(allowance)
            ElseIf allowance.AllowanceFrequency = "Semi-monthly" Then
                If allowance.Product.Fixed Then
                    item.Amount = allowance.Amount
                Else
                    item = CalculateSemiMonthlyProratedAllowance(allowance)
                End If
            ElseIf allowance.AllowanceFrequency = "Monthly" Then
                If allowance.Product.Fixed And _payPeriod.IsEndOfTheMonth Then
                    item.Amount = allowance.Amount
                End If
            Else
                item = Nothing
            End If

            _allowanceItems.Add(item)
        Next

        _paystub.TotalAllowance = AccuMath.CommercialRound(_allowanceItems.Sum(Function(a) a.Amount))
    End Sub

    Private Function CalculateSemiMonthlyProratedAllowance(allowance As Allowance) As AllowanceItem
        Dim workDaysPerYear = _employee2.WorkDaysPerYear
        Dim workingDays = CDec(workDaysPerYear / CalendarConstants.MonthsInAYear / CalendarConstants.SemiMonthlyPayPeriodsPerMonth)
        Dim dailyRate = allowance.Amount / workingDays

        Dim allowanceItem = New AllowanceItem() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .LastUpdBy = z_User,
            .Paystub = _paystub,
            .PayPeriodID = _payPeriod.RowID,
            .AllowanceID = allowance.RowID,
            .Amount = allowance.Amount
        }

        For Each TimeEntry In _timeEntries
            Dim divisor = If(TimeEntry.ShiftSchedule?.Shift?.DivisorToDailyRate, 8D)
            Dim hourlyRate = dailyRate / divisor

            Dim deductionHours =
                TimeEntry.LateHours +
                TimeEntry.UndertimeHours +
                TimeEntry.AbsentHours
            Dim deductionAmount = -(hourlyRate * deductionHours)

            Dim additionalAmount = 0D
            Dim giveAllowanceDuringHolidays = _settings.GetBoolean("Payroll Policy", "allowances.holiday")
            If giveAllowanceDuringHolidays Then
                Dim payRate = _payRates(TimeEntry.Date)

                If (payRate.IsSpecialNonWorkingHoliday And _employee2.CalcSpecialHoliday) Or
                   (payRate.IsRegularHoliday And _employee2.CalcHoliday) Then
                    additionalAmount = TimeEntry.RegularHours * hourlyRate * (payRate.CommonRate - 1D)
                End If
            End If

            allowanceItem.AddPerDay(TimeEntry.Date, deductionAmount + additionalAmount)
        Next

        Return allowanceItem
    End Function

    Private Function CalculateOneTimeAllowances(allowance As Allowance) As Decimal
        Return allowance.Amount
    End Function

    Private Function CalculateDailyProratedAllowance(allowance As Allowance) As AllowanceItem
        Dim dailyRate = allowance.Amount

        Dim allowanceItem = New AllowanceItem() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .LastUpdBy = z_User,
            .Paystub = _paystub,
            .PayPeriodID = _payPeriod.RowID,
            .AllowanceID = allowance.RowID
        }

        For Each timeEntry In _timeEntries
            If Not (allowance.EffectiveStartDate <= timeEntry.Date And timeEntry.Date <= allowance.EffectiveEndDate) Then
                Continue For
            End If

            Dim divisor = If(timeEntry.ShiftSchedule?.Shift?.DivisorToDailyRate, 8D)
            Dim hourlyRate = dailyRate / divisor

            Dim amount = 0D
            Dim payRate = _payRates(timeEntry.Date)
            If payRate.IsRegularDay Then
                Dim isRestDay = timeEntry.RestDayHours > 0

                If isRestDay Then
                    amount = dailyRate
                Else
                    amount = (timeEntry.RegularHours + timeEntry.TotalLeaveHours) * hourlyRate
                End If
            ElseIf payRate.IsSpecialNonWorkingHoliday Then
                Dim countableHours = timeEntry.RegularHours + timeEntry.SpecialHolidayHours + timeEntry.TotalLeaveHours

                amount = If(countableHours > 0, dailyRate, 0D)
            ElseIf payRate.IsRegularHoliday Then
                amount = (timeEntry.RegularHours + timeEntry.RegularHolidayHours) * hourlyRate

                Dim exemption = _settings.GetBoolean("AllowancePolicy.HolidayAllowanceForMonthly")

                Dim giveAllowance =
                    HasWorkedLastWorkingDay(timeEntry) Or
                    ((_employee2.IsFixed Or _employee2.IsMonthly) And exemption)

                If giveAllowance Then
                    If _settings.GetString("AllowancePolicy.CalculationType") = "Hourly" Then
                        Dim workHours = If(timeEntry.ShiftSchedule?.Shift?.WorkHours, 8D)

                        amount += {workHours * hourlyRate, dailyRate}.Max()
                    Else
                        amount += dailyRate
                    End If
                End If
            End If

            allowanceItem.AddPerDay(timeEntry.Date, amount)
        Next

        Return allowanceItem
    End Function

    Private Function HasWorkedLastWorkingDay(current As TimeEntry) As Boolean
        Dim lastPotentialEntry = current.Date.AddDays(-3)

        Dim lastTimeEntries = _previousTimeEntries2.
            Where(Function(t) lastPotentialEntry <= t.Date And t.Date <= current.Date).
            Reverse().
            ToList()

        For Each lastTimeEntry In lastTimeEntries
            ' If employee has no shift set for the day, it's not a working day.
            If lastTimeEntry?.ShiftSchedule?.Shift Is Nothing Then
                Continue For
            End If

            If lastTimeEntry?.ShiftSchedule.IsRestDay Then
                If lastTimeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Dim payRate = _payRates(lastTimeEntry.Date)
            If payRate.IsHoliday Then
                If lastTimeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Return lastTimeEntry.RegularHours > 0 Or lastTimeEntry.TotalLeaveHours > 0
        Next

        Return False
    End Function

    Private Sub UpdateLeaveLedger(context As PayrollContext)
        Dim leaves = context.Leaves.
            Where(Function(l) _payPeriod.PayFromDate <= l.StartDate).
            Where(Function(l) l.StartDate <= _payPeriod.PayToDate).
            Where(Function(l) CBool(l.EmployeeID = _employee2.RowID)).
            ToList()

        Dim leaveIds = leaves.Select(Function(l) l.RowID)

        Dim transactions = (From t In context.LeaveTransactions
                            Where leaveIds.Contains(t.ReferenceID)).ToList()

        Dim employeeId = _employee2.RowID
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

                Dim timeEntry = _timeEntries.
                    FirstOrDefault(Function(t) t.Date = leave.StartDate)

                If timeEntry Is Nothing Then
                    Continue For
                End If

                Dim newTransaction = New LeaveTransaction() With {
                    .OrganizationID = z_OrganizationID,
                    .Created = Date.Now,
                    .EmployeeID = leave.EmployeeID,
                    .PayPeriodID = _payPeriod.RowID,
                    .ReferenceID = leave.RowID,
                    .TransactionDate = Date.Today,
                    .Type = LeaveTransactionType.Debit,
                    .Amount = timeEntry.TotalLeaveHours,
                    .Balance = If(ledger?.LastTransaction?.Balance, 0) - timeEntry.TotalLeaveHours
                }

                ledger.LeaveTransactions.Add(newTransaction)
                ledger.LastTransaction = newTransaction
            End If
        Next
    End Sub

    Private Function ComputeLoans() As IList(Of LoanTransaction)
        Dim existingLoanTransactions = _allLoanTransactions.
               Where(Function(t) Nullable.Equals(t.EmployeeID, _paystub.EmployeeID))
        Dim newLoanTransactions = New List(Of LoanTransaction)

        If existingLoanTransactions.Count > 0 Then
            _paystub.TotalLoans = existingLoanTransactions.Sum(Function(t) t.Amount)
        Else
            Dim acceptedLoans As String() = {}
            If _payPeriod.IsFirstHalf Then
                acceptedLoans = {"Per pay period", "First half"}
            ElseIf _payperiod.IsEndOfTheMonth Then
                acceptedLoans = {"Per pay period", "End of the month"}
            End If

            Dim loanSchedules = _allLoanSchedules.
                Where(Function(l) Nullable.Equals(l.EmployeeID, _paystub.EmployeeID)).
                Where(Function(l) acceptedLoans.Contains(l.DeductionSchedule)).
                ToList()

            For Each loanSchedule In loanSchedules
                Dim loanTransaction = New LoanTransaction() With {
                    .Created = Date.Now(),
                    .LastUpd = Date.Now(),
                    .OrganizationID = z_OrganizationID,
                    .EmployeeID = _paystub.EmployeeID,
                    .PayPeriodID = _payPeriod.RowID,
                    .LoanScheduleID = loanSchedule.RowID,
                    .LoanPayPeriodLeft = loanSchedule.LoanPayPeriodLeft - 1
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
        Dim vacationLeaveProduct = _products.Where(Function(p) p.PartNo = "Vacation leave").FirstOrDefault()
        Dim sickLeaveProduct = _products.Where(Function(p) p.PartNo = "Sick leave").FirstOrDefault()

        context.Entry(_paystub).Collection(Function(p) p.PaystubItems).Load()
        context.Set(Of PaystubItem).RemoveRange(_paystub.PaystubItems)

        Dim vacationLeaveBalance = context.PaystubItems.
            Where(Function(p) p.Product.PartNo = "Vacation leave").
            Where(Function(p) CBool(p.Paystub.RowID = _paystub.RowID)).
            FirstOrDefault()

        Dim vacationLeaveUsed = _timeEntries.Sum(Function(t) t.VacationLeaveHours)
        Dim newBalance = _employee2.LeaveBalance - vacationLeaveUsed

        vacationLeaveBalance = New PaystubItem() With {
            .OrganizationID = z_OrganizationID,
            .ProductID = vacationLeaveProduct.RowID,
            .PayAmount = newBalance,
            .Paystub = _paystub
        }

        _paystub.PaystubItems.Add(vacationLeaveBalance)

        Dim sickLeaveBalance = context.PaystubItems.
            Where(Function(p) p.Product.PartNo = "Sick leave").
            Where(Function(p) CBool(p.Paystub.RowID = _paystub.RowID)).
            FirstOrDefault()

        Dim sickLeaveUsed = _timeEntries.Sum(Function(t) t.SickLeaveHours)
        Dim newBalance2 = _employee2.SickLeaveBalance - sickLeaveUsed

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

        Dim contractual_employment_statuses = New String() {"Contractual", "SERVICE CONTRACT"}

        Dim basicpay_13month = 0D

        If _employee2.IsDaily Then
            basicpay_13month = If(
                contractual_employment_statuses.Contains(_employee2.EmploymentStatus),
                _timeEntries.
                    Where(Function(t) Not If(t.ShiftSchedule?.IsRestDay, False)).
                    Sum(Function(t) t.BasicDayPay + t.LeavePay),
                _actualtimeentries.
                    Where(Function(t) Not If(t.ShiftSchedule?.IsRestDay, False)).
                    Sum(Function(t) t.BasicDayPay + t.LeavePay))

        ElseIf _employee2.IsMonthly Or _employee2.IsFixed Then
            Dim trueSalary = _salary.TotalSalary
            Dim basicPay = trueSalary / CalendarConstants.SemiMonthlyPayPeriodsPerMonth

            Dim totalDeductions = _actualtimeentries.Sum(Function(t) t.LateDeduction + t.UndertimeDeduction + t.AbsentDeduction)

            basicpay_13month = (basicPay - totalDeductions)
        End If

        _paystub.ThirteenthMonthPay.BasicPay = basicpay_13month
        _paystub.ThirteenthMonthPay.Amount = (basicpay_13month / CalendarConstants.MonthsInAYear)
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

    Private Class ContributionSchedule
        Public Const FirstHalf As String = "First half"
        Public Const EndOfTheMonth As String = "End of the month"
        Public Const PerPayPeriod As String = "Per pay period"
    End Class

End Class
