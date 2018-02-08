Imports System.Collections.ObjectModel
Imports AccuPay.Entity
Imports log4net
Imports MySql.Data.MySqlClient
Imports PayrollSys

Public Class PayrollGeneration

    Private Shared logger As ILog = LogManager.GetLogger("PayrollLogger")

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

    Public Property PayrollDateFrom As String

    Public Property PayrollDateTo As String

    Public Property PayPeriodID As String

    Private _employee As DataRow

    Private _employee2 As Employee

    Private isEndOfMonth2 As String
    Private _isFirstHalf As Boolean
    Private _isEndOfMonth As Boolean
    Private _allSalaries As DataTable

    Private _allLoanSchedules As ICollection(Of PayrollSys.LoanSchedule)

    Private _allLoanTransactions As ICollection(Of PayrollSys.LoanTransaction)

    Private allWeeklyAllowances As DataTable
    Private allNoTaxWeeklyAllowances As DataTable

    Private allOneTimeBonuses As DataTable
    Private allDailyBonuses As DataTable
    Private allWeeklyBonuses As DataTable
    Private allSemiMonthlyBonuses As DataTable
    Private allMonthlyBonuses As DataTable

    Private allNoTaxOneTimeBonuses As DataTable
    Private allNoTaxDailyBonuses As DataTable
    Private allNoTaxWeeklyBonuses As DataTable
    Private allNoTaxSemiMonthlyBonuses As DataTable
    Private allNoTaxMonthlyBonuses As DataTable

    Private allFixedTaxableMonthlyAllowances As DataTable
    Private allFixedNonTaxableMonthlyAllowances As DataTable

    Private _numOfDayPresent As DataTable
    Private _allTimeEntries As DataTable
    Private _employeeFirstTimeSalary As DataTable
    Private _VeryFirstPayPeriodIDOfThisYear As Object
    Private _withThirteenthMonthPay As SByte

    Private _previousTimeEntries2 As ICollection(Of TimeEntry)

    Private _withholdingTaxTable As DataTable

    Private _filingStatuses As DataTable

    Private Delegate Sub NotifyMainWindow(ByVal progress_index As Integer)

    Private _notifyMainWindow As NotifyMainWindow

    Private form_caller As Form

    Private payPeriod As DataRow

    Private _payPeriod As PayPeriod

    Private annualUnusedLeaves As DataTable
    Private unusedLeaveProductID As String
    Private existingUnusedLeaveAdjustments As DataTable

    Private agencyFeeSummary As DataTable

    Private _payStub As Paystub

    Private _products As IEnumerable(Of Product)

    Private _philHealthDeductionSchedule As String
    Private _sssDeductionSchedule As String
    Private _hdmfDeductionSchedule As String
    Private _withholdingTaxSchedule As String

    Private _connection As MySqlConnection

    Private numberofweeksthismonth As Integer

    Private _vacationLeaveBalance As Decimal

    Private _sickLeaveBalance As Decimal

    Private _otherLeaveBalance As Decimal

    Private _sickLeaveUsed As Decimal

    Private _vacationLeaveUsed As Decimal

    Private _otherLeaveUsed As Decimal

    Private _paystubs As IEnumerable(Of AccuPay.Entity.Paystub)

    Private _previousPaystubs As IEnumerable(Of Paystub)

    Private _socialSecurityBrackets As IEnumerable(Of SocialSecurityBracket)

    Private _philHealthBrackets As IEnumerable(Of PhilHealthBracket)

    Private _withholdingTaxBrackets As IEnumerable(Of WithholdingTaxBracket)

    Private _listOfValues As IList(Of ListOfValue)

    Private _settings As ListOfValueCollection

    Private _timeEntries2 As ICollection(Of TimeEntry)

    Private _payRates As IReadOnlyDictionary(Of Date, PayRate)

    Private _allowances As ICollection(Of Allowance)

    Sub New(employee As DataRow,
            payPeriodHalfNo As String,
            allSalaries As DataTable,
            allLoanSchedules As ICollection(Of PayrollSys.LoanSchedule),
            allLoanTransactions As ICollection(Of PayrollSys.LoanTransaction),
            allWeeklyAllowances As DataTable,
            allNoTaxWeeklyAllowances As DataTable,
            allDailyBonuses As DataTable,
            allMonthlyBonuses As DataTable,
            allOneTimeBonuses As DataTable,
            allSemiMonthlyBonuses As DataTable,
            allWeeklyBonuses As DataTable,
            allNoTaxDailyBonuses As DataTable,
            allNoTaxMonthlyBonuses As DataTable,
            allNoTaxOneTimeBonuses As DataTable,
            allNoTaxSemiMonthlyBonuses As DataTable,
            allNoTaxWeeklyBonuses As DataTable,
            numOfDayPresent As DataTable,
            allTimeEntries As DataTable,
            dtemployeefirsttimesalary As DataTable,
            VeryFirstPayPeriodIDOfThisYear As Object,
            withThirteenthMonthPay As SByte,
            filingStatuses As DataTable,
            withholdingTaxTable As DataTable,
            products As IEnumerable(Of Product),
            paystubs As IEnumerable(Of Paystub),
            resources As PayrollResources,
            Optional pay_stub_frm As PayStubForm = Nothing)
        form_caller = pay_stub_frm

        _employee = employee
        _employee2 = resources.Employees.
            FirstOrDefault(Function(e) e.RowID.ToString() = _employee("RowID"))

        isEndOfMonth2 = payPeriodHalfNo
        Me._allSalaries = allSalaries
        _allLoanSchedules = allLoanSchedules
        _allLoanTransactions = allLoanTransactions

        Me.allWeeklyAllowances = allWeeklyAllowances
        Me.allNoTaxWeeklyAllowances = allNoTaxWeeklyAllowances

        Me.allOneTimeBonuses = allOneTimeBonuses
        Me.allDailyBonuses = allDailyBonuses
        Me.allWeeklyBonuses = allWeeklyBonuses
        Me.allSemiMonthlyBonuses = allSemiMonthlyBonuses
        Me.allMonthlyBonuses = allMonthlyBonuses

        Me.allNoTaxOneTimeBonuses = allNoTaxOneTimeBonuses
        Me.allNoTaxDailyBonuses = allNoTaxDailyBonuses
        Me.allNoTaxWeeklyBonuses = allNoTaxWeeklyBonuses
        Me.allNoTaxSemiMonthlyBonuses = allNoTaxSemiMonthlyBonuses
        Me.allNoTaxMonthlyBonuses = allNoTaxMonthlyBonuses

        _numOfDayPresent = numOfDayPresent
        _allTimeEntries = allTimeEntries
        _employeeFirstTimeSalary = dtemployeefirsttimesalary
        _VeryFirstPayPeriodIDOfThisYear = VeryFirstPayPeriodIDOfThisYear
        _withThirteenthMonthPay = withThirteenthMonthPay

        _filingStatuses = filingStatuses
        _withholdingTaxTable = withholdingTaxTable

        _notifyMainWindow = AddressOf pay_stub_frm.ProgressCounter

        _products = products
        _paystubs = paystubs

        _isFirstHalf = (payPeriodHalfNo = "1")
        _isEndOfMonth = (payPeriodHalfNo = "0")
        _socialSecurityBrackets = resources.SocialSecurityBrackets
        _philHealthBrackets = resources.PhilHealthBrackets
        _withholdingTaxBrackets = resources.WithholdingTaxBrackets

        _listOfValues = resources.ListOfValues
        _previousPaystubs = resources.PreviousPaystubs
        _settings = New ListOfValueCollection(_listOfValues)
        _payPeriod = resources.PayPeriod

        _previousTimeEntries2 = resources.TimeEntries2.
            Where(Function(t) t.EmployeeID = _employee2.RowID).
            ToList()

        _timeEntries2 = resources.TimeEntries2.
            Where(Function(t) t.EmployeeID = _employee2.RowID).
            Where(Function(t) _payPeriod.PayFromDate <= t.EntryDate And t.EntryDate <= _payPeriod.PayToDate).
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
            Dim date_to_use = If(CDate(PayrollDateFrom) > CDate(PayrollDateTo), CDate(PayrollDateFrom), CDate(PayrollDateTo))
            Dim dateStr_to_use = Format(date_to_use, "yyyy-MM-dd")
            numberofweeksthismonth = CInt(New MySQLExecuteQuery("SELECT `COUNTTHEWEEKS`('" & dateStr_to_use & "');").Result)

            GeneratePayStub()
        Catch ex As Exception
            Console.WriteLine(getErrExcptn(ex, "PayrollGeneration - Error"))
            logger.Error("DoProcess", ex)
        End Try
    End Sub

    Private Sub GeneratePayStub()
        Dim transaction As MySqlTransaction = Nothing
        Dim newLoanTransactions = New Collection(Of PayrollSys.LoanTransaction)

        Try
            _payStub = New Paystub()

            Dim totalVacationDaysLeft = 0D

            _sssDeductionSchedule = _employee("SSSDeductSched").ToString
            _philHealthDeductionSchedule = _employee("PhHealthDeductSched").ToString
            _hdmfDeductionSchedule = _employee("HDMFDeductSched").ToString
            _withholdingTaxSchedule = _employee("WTaxDeductSched").ToString

            _payStub.EmployeeID = _employee2.RowID

            Dim salary = _allSalaries.Select($"EmployeeID = '{_payStub.EmployeeID}'").FirstOrDefault()

            Dim loanTransactions = _allLoanTransactions.Where(Function(t) t.EmployeeID = _payStub.EmployeeID)

            If loanTransactions.Count > 0 Then
                _payStub.TotalLoans = loanTransactions.Aggregate(0D, Function(total, t) total + t.DeductionAmount)
            Else
                Dim acceptedLoans As String() = {}
                If _isFirstHalf Then
                    acceptedLoans = {"Per pay period", "First half"}
                ElseIf _isEndOfMonth Then
                    acceptedLoans = {"Per pay period", "End of the month"}
                End If

                Dim loanSchedules = _allLoanSchedules.
                    Where(Function(l) l.EmployeeID = _payStub.EmployeeID).
                    Where(Function(l) acceptedLoans.Contains(l.DeductionSchedule)).
                    ToList()

                For Each loanSchedule In loanSchedules
                    Dim loanTransaction = New PayrollSys.LoanTransaction() With {
                        .Created = Date.Now(),
                        .LastUpd = Date.Now(),
                        .OrganizationID = z_OrganizationID,
                        .EmployeeID = _payStub.EmployeeID,
                        .PayPeriodID = PayPeriodID,
                        .LoanScheduleID = loanSchedule.RowID,
                        .LoanPayPeriodLeft = loanSchedule.LoanPayPeriodLeft - 1
                    }

                    If loanSchedule.DeductionAmount > loanSchedule.TotalBalanceLeft Then
                        loanTransaction.DeductionAmount = loanSchedule.TotalBalanceLeft
                    Else
                        loanTransaction.DeductionAmount = loanSchedule.DeductionAmount
                    End If

                    loanTransaction.TotalBalanceLeft = loanSchedule.TotalBalanceLeft - loanTransaction.DeductionAmount

                    newLoanTransactions.Add(loanTransaction)
                Next

                _payStub.TotalLoans = newLoanTransactions.Aggregate(0D, Function(total, x) x.DeductionAmount + total)
            End If

            totalVacationDaysLeft = 0D

            CalculateAllowances()
            CalculateBonuses()

            Dim governmentContributions = 0D
            Dim timeEntrySummary = _allTimeEntries.Select($"EmployeeID = '{_payStub.EmployeeID}'").FirstOrDefault()

            If timeEntrySummary IsNot Nothing Then

                totalVacationDaysLeft = 0D

                If salary IsNot Nothing Then

                    Dim isFirstPay =
                        _payPeriod.PayFromDate <= _employee2.StartDate And
                        _employee2.StartDate <= _payPeriod.PayToDate

                    Dim basicPay = ValNoComma(salary("BasicPay"))

                    ComputeHours()

                    _payStub.RegularPay = ValNoComma(timeEntrySummary("RegularHoursAmount"))

                    _payStub.OvertimePay = ValNoComma(timeEntrySummary("OvertimeHoursAmount"))

                    _payStub.NightDiffPay = ValNoComma(timeEntrySummary("NightDiffHoursAmount"))

                    _payStub.NightDiffOvertimePay = ValNoComma(timeEntrySummary("NightDiffOTHoursAmount"))

                    _payStub.RestDayPay = ValNoComma(timeEntrySummary("RestDayAmount"))

                    _payStub.LeavePay = ValNoComma(timeEntrySummary("Leavepayment"))
                    _payStub.HolidayPay = ValNoComma(timeEntrySummary("HolidayPayAmount"))

                    _payStub.LateDeduction = ValNoComma(timeEntrySummary("HoursLateAmount"))
                    _payStub.UndertimeDeduction = ValNoComma(timeEntrySummary("UndertimeHoursAmount"))
                    _payStub.AbsenceDeduction = ValNoComma(timeEntrySummary("Absent"))

                    _vacationLeaveBalance = _employee2.LeaveBalance
                    _sickLeaveBalance = _employee2.SickLeaveBalance
                    _otherLeaveBalance = _employee2.OtherLeaveBalance

                    _vacationLeaveUsed = CDec(timeEntrySummary("VacationLeaveHours"))
                    _sickLeaveUsed = CDec(timeEntrySummary("SickLeaveHours"))
                    _otherLeaveUsed = CDec(timeEntrySummary("OtherLeaveHours"))

                    Dim sel_dtemployeefirsttimesalary = _employeeFirstTimeSalary.Select($"EmployeeID = '{_payStub.EmployeeID}'")

                    Dim previousPaystub = _previousPaystubs?.
                        Where(Function(p) Nullable.Equals(p.EmployeeID, _payStub.EmployeeID)).
                        FirstOrDefault()

                    Dim currentTaxableIncome = 0D
                    If _employee2.EmployeeType = SalaryType.Fixed Then

                        _payStub.WorkPay = basicPay + (_payStub.HolidayPay + _payStub.OvertimePay + _payStub.NightDiffPay + _payStub.NightDiffOvertimePay)

                        currentTaxableIncome = basicPay

                    ElseIf _employee2.EmployeeType = SalaryType.Monthly Then

                        If isFirstPay Then
                            _payStub.WorkPay = ValNoComma(timeEntrySummary("TotalDayPay"))
                        Else
                            Dim totalDeduction = _payStub.LateDeduction + _payStub.UndertimeDeduction + _payStub.AbsenceDeduction
                            Dim extraPay = _payStub.OvertimePay + _payStub.NightDiffPay + _payStub.NightDiffOvertimePay + _payStub.RestDayPay + _payStub.HolidayPay
                            _payStub.WorkPay = (basicPay + extraPay) - totalDeduction

                            Dim taxablePolicy = If(_settings.GetString("Payroll Policy", "paystub.taxableincome"), "Basic Pay")

                            If taxablePolicy = "Gross Income" Then
                                currentTaxableIncome = _payStub.WorkPay
                            Else
                                currentTaxableIncome = basicPay
                            End If
                        End If

                    ElseIf _employee2.EmployeeType = SalaryType.Daily Then
                        _payStub.WorkPay = ValNoComma(timeEntrySummary("TotalDayPay"))
                    End If

                    CalculateSss(salary)
                    CalculatePhilHealth(salary)
                    CalculateHdmf(salary)

                    governmentContributions = _payStub.SssEmployeeShare + _payStub.PhilHealthEmployeeShare + _payStub.HdmfEmployeeShare
                    currentTaxableIncome = currentTaxableIncome - governmentContributions

                    _payStub.TaxableIncome = currentTaxableIncome

                    If IsWithholdingTaxPaidOnFirstHalf() Or IsWithholdingTaxPaidOnEndOfTheMonth() Then
                        _payStub.TaxableIncome = currentTaxableIncome + If(previousPaystub?.TaxableIncome, 0D)
                    ElseIf IsWithholdingTaxPaidPerPayPeriod() Then
                        _payStub.TaxableIncome = currentTaxableIncome
                    End If

                    CalculateWithholdingTax()
                End If
            End If

            _payStub.GrossPay = _payStub.WorkPay + _payStub.TotalBonus + _payStub.TotalAllowance
            _payStub.NetPay = _payStub.GrossPay - (governmentContributions + _payStub.TotalLoans + _payStub.WithholdingTax)

            Dim vacationLeaveProduct = _products.Where(Function(p) p.PartNo = "Vacation leave").FirstOrDefault()
            Dim sickLeaveProduct = _products.Where(Function(p) p.PartNo = "Sick leave").FirstOrDefault()

            _connection = New MySqlConnection(mysql_conn_text)
            Dim command = New MySqlCommand("SavePayStub", _connection, transaction)

            If _connection.State = ConnectionState.Closed Then
                _connection.Open()
            End If

            transaction = _connection.BeginTransaction()
            command.CommandTimeout = 5000
            command.CommandType = CommandType.StoredProcedure

            With command.Parameters
                .Clear()
                .AddWithValue("pstub_RowID", DBNull.Value)
                .AddWithValue("pstub_OrganizationID", orgztnID)
                .AddWithValue("pstub_CreatedBy", z_User)
                .AddWithValue("pstub_LastUpdBy", z_User)
                .AddWithValue("pstub_PayPeriodID", PayPeriodID)
                .AddWithValue("pstub_EmployeeID", _payStub.EmployeeID)
                .AddWithValue("pstub_TimeEntryID", DBNull.Value)
                .AddWithValue("pstub_PayFromDate", PayrollDateFrom)
                .AddWithValue("pstub_PayToDate", PayrollDateTo)
                .AddWithValue("$RegularHours", _payStub.RegularHours)
                .AddWithValue("$RegularPay", _payStub.RegularPay)
                .AddWithValue("$OvertimeHours", _payStub.OvertimeHours)
                .AddWithValue("$OvertimePay", _payStub.OvertimePay)
                .AddWithValue("$NightDiffHours", _payStub.NightDiffHours)
                .AddWithValue("$NightDiffPay", _payStub.NightDiffPay)
                .AddWithValue("$NightDiffOvertimeHours", _payStub.NightDiffOvertimeHours)
                .AddWithValue("$NightDiffOvertimePay", _payStub.NightDiffOvertimePay)
                .AddWithValue("$RestDayHours", _payStub.RestDayHours)
                .AddWithValue("$RestDayPay", _payStub.RestDayPay)
                .AddWithValue("$LeaveHours", _payStub.LeaveHours)
                .AddWithValue("$LeavePay", _payStub.LeavePay)
                .AddWithValue("$SpecialHolidayHours", _payStub.SpecialHolidayHours)
                .AddWithValue("$RegularHolidayHours", _payStub.RegularHolidayHours)
                .AddWithValue("$HolidayPay", _payStub.HolidayPay)
                .AddWithValue("$LateHours", _payStub.LateHours)
                .AddWithValue("$LateDeduction", _payStub.LateDeduction)
                .AddWithValue("$UndertimeHours", _payStub.UndertimeHours)
                .AddWithValue("$UndertimeDeduction", _payStub.UndertimeDeduction)
                .AddWithValue("$AbsentHours", _payStub.AbsentHours)
                .AddWithValue("$AbsenceDeduction", _payStub.AbsenceDeduction)
                .AddWithValue("$WorkPay", _payStub.WorkPay)
                .AddWithValue("pstub_TotalAllowance", _payStub.TotalAllowance)
                .AddWithValue("pstub_TotalBonus", _payStub.TotalBonus)
                .AddWithValue("pstub_TotalGrossSalary", _payStub.GrossPay)
                .AddWithValue("pstub_TotalNetSalary", _payStub.NetPay)
                .AddWithValue("pstub_TotalTaxableSalary", _payStub.TaxableIncome)
                .AddWithValue("pstub_TotalEmpWithholdingTax", _payStub.WithholdingTax)
                .AddWithValue("pstub_TotalEmpSSS", _payStub.SssEmployeeShare)
                .AddWithValue("pstub_TotalCompSSS", _payStub.SssEmployerShare)
                .AddWithValue("pstub_TotalEmpPhilhealth", _payStub.PhilHealthEmployeeShare)
                .AddWithValue("pstub_TotalCompPhilhealth", _payStub.PhilHealthEmployerShare)
                .AddWithValue("pstub_TotalEmpHDMF", _payStub.HdmfEmployeeShare)
                .AddWithValue("pstub_TotalCompHDMF", _payStub.HdmfEmployerShare)
                .AddWithValue("pstub_TotalVacationDaysLeft", totalVacationDaysLeft)
                .AddWithValue("pstub_TotalLoans", _payStub.TotalLoans)
                .Add("NewID", MySqlDbType.Int32)
                .Item("NewID").Direction = ParameterDirection.ReturnValue
            End With

            command.ExecuteNonQuery()
            _payStub.RowID = command.Parameters("NewID").Value

            transaction.Commit()
            transaction = Nothing
            command.Dispose()
            _connection.Close()

            Using context = New PayrollContext()
                Dim vacationLeaveBalance =
                    (From p In context.PaystubItems
                     Where p.Product.PartNo = "Vacation leave" And
                        p.PayStubID = _payStub.RowID).
                    FirstOrDefault()

                If vacationLeaveBalance Is Nothing Then
                    Dim newBalance = _vacationLeaveBalance - _vacationLeaveUsed

                    vacationLeaveBalance = New PaystubItem() With {
                        .OrganizationID = z_OrganizationID,
                        .Created = Date.Now,
                        .ProductID = vacationLeaveProduct?.RowID,
                        .PayAmount = newBalance,
                        .PayStubID = _payStub.RowID
                    }

                    context.PaystubItems.Add(vacationLeaveBalance)
                End If

                Dim sickLeaveBalance =
                    (From p In context.PaystubItems
                     Where p.Product.PartNo = "Sick leave" And
                        p.PayStubID = _payStub.RowID).
                    FirstOrDefault()

                If sickLeaveBalance Is Nothing Then
                    Dim newBalance = _sickLeaveBalance - _sickLeaveUsed

                    sickLeaveBalance = New PaystubItem() With {
                        .OrganizationID = z_OrganizationID,
                        .Created = Date.Now,
                        .ProductID = sickLeaveProduct?.RowID,
                        .PayAmount = newBalance,
                        .PayStubID = _payStub.RowID
                    }

                    context.PaystubItems.Add(sickLeaveBalance)
                End If

                context.LoanTransactions.AddRange(newLoanTransactions)

                context.SaveChanges()
            End Using

            form_caller.BeginInvoke(_notifyMainWindow, 1)
        Catch ex As Exception
            If transaction IsNot Nothing Then
                transaction.Rollback()
            End If

            Throw New Exception($"Failure to generate paystub for employee {_payStub.EmployeeID}", ex)
        Finally
            If _connection IsNot Nothing Then
                If _connection.State = ConnectionState.Open Then
                    _connection.Close()
                    _connection.Dispose()
                End If
            End If
        End Try
    End Sub

    Private Sub ComputeHours()
        For Each timeEntry In _timeEntries2
            Dim payRate = _payRates(timeEntry.EntryDate)

            If payRate.IsRegularDay Then
                _payStub.RegularHours += timeEntry.RegularHours
                _payStub.RestDayHours += timeEntry.RestDayHours
            ElseIf payRate.IsSpecialNonWorkingHoliday Then
                _payStub.SpecialHolidayHours += timeEntry.RegularHours
            ElseIf payRate.IsRegularHoliday Then
                _payStub.RegularHolidayHours += timeEntry.RegularHours
            End If

            _payStub.OvertimeHours += timeEntry.OvertimeHours
            _payStub.NightDiffHours += timeEntry.NightDiffHours
            _payStub.NightDiffOvertimeHours += timeEntry.NightDiffOvertimeHours
            _payStub.LeaveHours += timeEntry.TotalLeaveHours

            _payStub.LateHours += timeEntry.LateHours
            _payStub.UndertimeHours += timeEntry.UndertimeHours
            _payStub.AbsentHours += timeEntry.AbsentHours
        Next
    End Sub

    Private Function CalculateSemiMonthlyProratedAllowance(allowance As Allowance) As Decimal
        Dim workDaysPerYear = _employee2.WorkDaysPerYear
        Dim workingDays = CDec(workDaysPerYear / CalendarConstants.MonthsInAYear / CalendarConstants.SemiMonthlyPayPeriodsPerMonth)
        Dim dailyRate = allowance.Amount / workingDays

        Dim allowancesPerDay = New Collection(Of AllowancePerDay)
        For Each timeEntry In _timeEntries2
            Dim divisor = If(timeEntry.ShiftSchedule?.Shift?.DivisorToDailyRate, 8D)
            Dim hourlyRate = dailyRate / divisor

            Dim deductionHours =
                timeEntry.LateHours +
                timeEntry.UndertimeHours +
                timeEntry.AbsentHours
            Dim deductionAmount = -(hourlyRate * deductionHours)

            Dim additionalAmount = 0D
            Dim giveAllowanceDuringHolidays = _settings.GetBoolean("Payroll Policy", "allowances.holiday")
            If giveAllowanceDuringHolidays Then
                Dim payRate = _payRates(timeEntry.EntryDate)

                If (payRate.IsSpecialNonWorkingHoliday And _employee2.CalcSpecialHoliday) Or
                   (payRate.IsRegularHoliday And _employee2.CalcHoliday) Then
                    additionalAmount = timeEntry.RegularHours * hourlyRate * (payRate.CommonRate - 1D)
                End If
            End If

            Dim perDay = New AllowancePerDay()
            perDay.Amount = deductionAmount + additionalAmount
            allowancesPerDay.Add(perDay)
        Next

        Dim sumTotalOfDays = allowancesPerDay.Sum(Function(a) a.Amount)
        Dim baseAllowance = allowance.Amount
        Return baseAllowance + sumTotalOfDays
    End Function

    Private Function CalculateDailyProratedAllowance(allowance As Allowance) As Decimal
        Dim dailyRate = allowance.Amount

        Dim allowancesPerDay = New Collection(Of AllowancePerDay)
        For Each timeEntry In _timeEntries2
            Dim divisor = If(timeEntry.ShiftSchedule?.Shift?.DivisorToDailyRate, 8D)
            Dim hourlyRate = dailyRate / divisor

            Dim amount = 0D
            Dim payRate = _payRates(timeEntry.EntryDate)
            If payRate.IsRegularDay Then
                Dim isRestDay = If(timeEntry.ShiftSchedule?.IsRestDay, False)

                If isRestDay Then
                    amount = If(timeEntry.RegularHours > 0, dailyRate, 0)
                Else
                    amount = timeEntry.RegularHours * hourlyRate
                End If
            ElseIf payRate.IsSpecialNonWorkingHoliday Then
                Dim countableHours = timeEntry.RegularHours + timeEntry.TotalLeaveHours

                amount = If(countableHours > 0, dailyRate, 0D)
            ElseIf payRate.IsRegularHoliday Then
                amount = timeEntry.RegularHours * hourlyRate

                If HasWorkedLastWorkingDay(timeEntry) Then
                    amount += dailyRate
                End If
            End If

            Dim perDay = New AllowancePerDay()
            perDay.EntryDate = timeEntry.EntryDate
            perDay.Amount = amount
            allowancesPerDay.Add(perDay)
        Next

        Dim totalAmount = allowancesPerDay.Sum(Function(a) a.Amount)
        Return totalAmount
    End Function

    Private Function CalculateOneTimeAllowances(allowance As Allowance)
        Return allowance.Amount
    End Function

    Private Function HasWorkedLastWorkingDay(current As TimeEntry) As Boolean
        Dim lastPotentialEntry = current.EntryDate.AddDays(-3)

        Dim lastTimeEntries = _previousTimeEntries2.
            Where(Function(t) lastPotentialEntry <= t.EntryDate And t.EntryDate <= current.EntryDate).
            Reverse().
            ToList()

        For Each lastTimeEntry In lastTimeEntries
            ' If employee has no shift set for the day, it's not a working day.
            If lastTimeEntry?.ShiftSchedule?.Shift Is Nothing Then
                Continue For
            End If

            If lastTimeEntry?.ShiftSchedule.IsRestDay And (lastTimeEntry.TotalDayPay > 0D) Then
                Return True
            End If

            Dim payRate = _payRates(lastTimeEntry.EntryDate)
            If payRate.IsHoliday And (lastTimeEntry.TotalDayPay > 0D) Then
                Return True
            End If

            Return lastTimeEntry.RegularHours > 0 Or lastTimeEntry.TotalLeaveHours > 0
        Next

        Return False
    End Function

    Private Sub CalculateAllowances()
        Dim totalOneTimeAllowance = 0D
        Dim totalDailyAllowance = 0D
        Dim totalSemiMonthlyAllowance = 0D
        Dim totalMonthlyAllowance = 0D

        For Each allowance In _allowances
            If allowance.AllowanceFrequency = "One time" Then
                totalOneTimeAllowance += allowance.Amount
            ElseIf allowance.AllowanceFrequency = "Daily" Then
                totalDailyAllowance += CalculateDailyProratedAllowance(allowance)
            ElseIf allowance.AllowanceFrequency = "Semi-monthly" Then
                If allowance.Product.Fixed Then
                    totalSemiMonthlyAllowance += allowance.Amount
                Else
                    totalSemiMonthlyAllowance += CalculateSemiMonthlyProratedAllowance(allowance)
                End If
            ElseIf allowance.AllowanceFrequency = "Monthly" Then
                If allowance.Product.Fixed And _isEndOfMonth Then
                    totalMonthlyAllowance += allowance.Amount
                End If
            End If
        Next

        Dim totalAllowance = (
            totalOneTimeAllowance +
            totalDailyAllowance +
            totalSemiMonthlyAllowance +
            totalMonthlyAllowance
        )

        _payStub.TotalAllowance = totalAllowance
    End Sub

    Private Sub CalculateBonuses()
        Dim oneTimeBonuses = allOneTimeBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim dailyBonuses = allDailyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim weeklyBonuses = allWeeklyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim semiMonthlyBonuses = allSemiMonthlyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim monthlyBonuses = allMonthlyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")

        Dim noTaxOneBonuses = allNoTaxOneTimeBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim noTaxDailyBonuses = allNoTaxDailyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim noTaxWeeklyBonuses = allNoTaxWeeklyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim noTaxSemiMonthlyBonuses = allNoTaxSemiMonthlyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim noTaxMonthlyBonuses = allNoTaxMonthlyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim workDaysPerYear = _employee2.WorkDaysPerYear
        'Dim divisorMonthlys = If(
        '        CInt(_employee("PayFrequencyID")) = 1,
        '        2,
        '        If(
        '            CInt(_employee("PayFrequencyID")) = 2,
        '            1,
        '            If(
        '                CInt(_employee("PayFrequencyID")) = 3,
        '                CInt(New MySQLExecuteQuery("SELECT COUNT(RowID) FROM employeetimeentry WHERE EmployeeID='" & _payStub.EmployeeID & "' AND Date BETWEEN '" & PayrollDateFrom & "' AND '" & PayrollDateTo & "' AND IFNULL(TotalDayPay,0)!=0 AND OrganizationID='" & orgztnID & "';").Result),
        '                numberofweeksthismonth
        '            )
        '        )
        '    )

        Dim totalTaxableBonus = 0D

        Dim valdaynotax_bon = 0D
        For Each drowdaybon In noTaxDailyBonuses
            valdaynotax_bon = ValNoComma(drowdaybon("BonusAmount"))

            If _employee2.EmployeeType = "Fixed" Then
                valdaynotax_bon = valdaynotax_bon * workDaysPerYear 'numofweekends
            Else
                Dim daymultiplier = _numOfDayPresent.Select($"EmployeeID = '{_payStub.EmployeeID}'")
                For Each drowdaymultip In daymultiplier
                    Dim i_val = CInt(drowdaymultip("DaysAttended"))
                    valdaynotax_bon = valdaynotax_bon * i_val
                    Exit For
                Next

            End If

            Exit For
        Next

        Dim valmonthnotax_bon = 0D

        If isEndOfMonth2 = "1" Then

            For Each drowmonbon In noTaxMonthlyBonuses
                'valmonthnotax_bon = drowmonbon("BonusAmount")
            Next

        End If

        Dim valoncenotax_bon = 0D
        For Each drowoncebon In noTaxOneBonuses
            'valoncenotax_bon = drowoncebon("BonusAmount")
        Next

        Dim valsemimnotax_bon = 0D
        For Each drowsemimbon In noTaxSemiMonthlyBonuses
            'valoncenotax_bon = drowsemimbon("BonusAmount")
        Next

        Dim valweeknotax_bon = 0D
        For Each drowweekbon In noTaxWeeklyBonuses
            'valoncenotax_bon = drowweekbon("BonusAmount")
        Next

        'this is non-taxable
        Dim totalNoTaxBonus = (
            valoncenotax_bon +
            valdaynotax_bon +
            valweeknotax_bon +
            valsemimnotax_bon
        )

        _payStub.TotalBonus = totalTaxableBonus + totalNoTaxBonus
    End Sub

    Private Sub CalculateSss(salary As DataRow)
        Dim employeeSssPerMonth = ValNoComma(salary("EmployeeContributionAmount"))
        Dim employerSssPerMonth = ValNoComma(salary("EmployerContributionAmount"))
        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))

        If False Then
            Dim socialSecurityBracket = _socialSecurityBrackets.FirstOrDefault(
                Function(s)
                    Return s.RangeFromAmount <= _payStub.GrossPay And
                        s.RangeToAmount >= _payStub.GrossPay
                End Function
            )

            employeeSssPerMonth = socialSecurityBracket.EmployeeContributionAmount
            employerSssPerMonth = socialSecurityBracket.EmployerContributionAmount
        End If

        If IsSssPaidOnFirstHalf() Or IsSssPaidOnEndOfTheMonth() Then
            _payStub.SssEmployeeShare = employeeSssPerMonth
            _payStub.SssEmployerShare = employerSssPerMonth
        ElseIf IsSssPaidPerPayPeriod() Then
            _payStub.SssEmployeeShare = employeeSssPerMonth / payPeriodsPerMonth
            _payStub.SssEmployerShare = employerSssPerMonth / payPeriodsPerMonth
        End If
    End Sub

    Private Function IsSssPaidOnFirstHalf() As Boolean
        Return _isFirstHalf And (_sssDeductionSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsSssPaidOnEndOfTheMonth() As Boolean
        Return _isEndOfMonth And (_sssDeductionSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsSssPaidPerPayPeriod() As Boolean
        Return _sssDeductionSchedule = ContributionSchedule.PerPayPeriod
    End Function

    Private Sub CalculatePhilHealth(salary As DataRow)
        Dim totalContribution = ConvertToType(Of Decimal)(salary("PhilHealthDeduction"))

        If False Then
            Dim philHealthBracket = _philHealthBrackets.FirstOrDefault(
                Function(p)
                    Return p.SalaryRangeFrom <= _payStub.GrossPay And
                        p.SalaryRangeTo >= _payStub.GrossPay
                End Function
            )

            totalContribution = If(philHealthBracket?.TotalMonthlyPremium, 0D)
        End If

        Dim halfContribution = AccuMath.Truncate(totalContribution / 2, 2)

        ' Account for any division loss by putting the missing value to the employer share
        Dim expectedTotal = halfContribution * 2
        Dim remaining = 0D
        If expectedTotal < totalContribution Then
            remaining = totalContribution - expectedTotal
        End If

        Dim employeeShare = halfContribution
        Dim employerShare = halfContribution + remaining

        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))

        If IsPhilHealthPaidOnFirstHalf() Or IsPhilHealthPaidOnEndOfTheMonth() Then
            _payStub.PhilHealthEmployeeShare = employeeShare
            _payStub.PhilHealthEmployerShare = employerShare
        ElseIf IsPhilHealthPaidPerPayPeriod() Then
            _payStub.PhilHealthEmployeeShare = employeeShare / payPeriodsPerMonth
            _payStub.PhilHealthEmployerShare = employerShare / payPeriodsPerMonth
        End If
    End Sub

    Private Function IsPhilHealthPaidOnFirstHalf() As Boolean
        Return _isFirstHalf And (_philHealthDeductionSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsPhilHealthPaidOnEndOfTheMonth() As Boolean
        Return _isEndOfMonth And (_philHealthDeductionSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsPhilHealthPaidPerPayPeriod() As Boolean
        Return _philHealthDeductionSchedule = ContributionSchedule.PerPayPeriod
    End Function

    Private Sub CalculateHdmf(salary As DataRow)
        Dim employeeHdmfPerMonth = ValNoComma(salary("HDMFAmount"))
        Dim employerHdmfPerMonth = 100D
        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))

        If IsHdmfPaidOnFirstHalf() Or IsHdmfPaidOnEndOfTheMonth() Then
            _payStub.HdmfEmployeeShare = employeeHdmfPerMonth
            _payStub.HdmfEmployerShare = employerHdmfPerMonth
        ElseIf IsHdmfPaidPerPayPeriod() Then
            _payStub.HdmfEmployeeShare = employeeHdmfPerMonth / payPeriodsPerMonth
            _payStub.HdmfEmployerShare = employerHdmfPerMonth / payPeriodsPerMonth
        End If
    End Sub

    Private Function IsHdmfPaidOnFirstHalf() As Boolean
        Return _isFirstHalf And (_hdmfDeductionSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsHdmfPaidOnEndOfTheMonth() As Boolean
        Return _isEndOfMonth And (_hdmfDeductionSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsHdmfPaidPerPayPeriod() As Boolean
        Return _hdmfDeductionSchedule = ContributionSchedule.PerPayPeriod
    End Function

    Private Sub CalculateWithholdingTax()
        Dim payFrequencyID As Integer

        If IsWithholdingTaxPaidOnFirstHalf() Or IsWithholdingTaxPaidOnEndOfTheMonth() Then
            payFrequencyID = PayFrequency.Monthly
        ElseIf IsWithholdingTaxPaidPerPayPeriod() Then
            payFrequencyID = PayFrequency.SemiMonthly
        End If

        ' Round the daily rate to two decimal places since amounts in the 3rd decimal place
        ' isn't significant enough to warrant the employee to be taxable.
        Dim dailyRate = Math.Round(ValNoComma(_employee("EmpRatePerDay")), 2)

        Dim minimumWage = ValNoComma(_employee("MinimumWageAmount"))
        Dim isMinimumWageEarner = dailyRate <= minimumWage

        If isMinimumWageEarner Then
            _payStub.TaxableIncome = 0D
        End If

        If Not (_payStub.TaxableIncome > 0D And IsScheduledForTaxation()) Then
            Return
        End If

        Dim maritalStatus = _employee2.MaritalStatus
        Dim noOfDependents = _employee2.NoOfDependents

        Dim filingStatus = _filingStatuses.
            Select($"
                MaritalStatus = '{maritalStatus}' AND
                Dependent <= '{noOfDependents}'
            ").
            OrderByDescending(Function(f) CInt(f("Dependent"))).
            FirstOrDefault()

        Dim filingStatusID = 1
        If filingStatus IsNot Nothing Then
            filingStatusID = CInt(filingStatus("RowID"))
        End If

        Dim bracket = GetMatchingTaxBracket(payFrequencyID, filingStatusID)

        If bracket Is Nothing Then
            Return
        End If

        Dim exemptionAmount = bracket.ExemptionAmount
        Dim taxableIncomeFromAmount = bracket.TaxableIncomeFromAmount
        Dim exemptionInExcessAmount = bracket.ExemptionInExcessAmount

        Dim excessAmount = _payStub.TaxableIncome - taxableIncomeFromAmount
        _payStub.WithholdingTax = exemptionAmount + (excessAmount * exemptionInExcessAmount)
    End Sub

    Private Function GetMatchingTaxBracket(payFrequencyID As Integer?, filingStatusID As Integer?) As WithholdingTaxBracket
        Dim taxEffectivityDate = New Date(_payPeriod.Year, _payPeriod.Month, 1)

        Dim possibleBrackets =
            (From w In _withholdingTaxBrackets
             Where w.PayFrequencyID = payFrequencyID And
                 w.TaxableIncomeFromAmount <= _payStub.TaxableIncome And
                 _payStub.TaxableIncome <= w.TaxableIncomeToAmount And
                 w.EffectiveDateFrom <= taxEffectivityDate And
                 taxEffectivityDate <= w.EffectiveDateTo
             Select w).
             ToList()

        ' If there are more than one tax brackets that matches the previous list, filter by
        ' the tax filing status.
        If possibleBrackets.Count > 1 Then
            Return possibleBrackets.
                Where(Function(b) Nullable.Equals(b.FilingStatusID, filingStatusID)).
                FirstOrDefault()
        ElseIf possibleBrackets.Count = 1 Then
            Return possibleBrackets.First()
        End If

        Return Nothing
    End Function

    Private Function IsWithholdingTaxPaidOnFirstHalf() As Boolean
        Return _isFirstHalf And (_withholdingTaxSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsWithholdingTaxPaidOnEndOfTheMonth() As Boolean
        Return _isEndOfMonth And (_withholdingTaxSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsWithholdingTaxPaidPerPayPeriod() As Boolean
        Return _withholdingTaxSchedule = ContributionSchedule.PerPayPeriod
    End Function

    Private Function IsScheduledForTaxation() As Boolean
        Return (_isFirstHalf And IsWithholdingTaxPaidOnFirstHalf()) Or
            (_isEndOfMonth And IsWithholdingTaxPaidOnEndOfTheMonth()) Or
            IsWithholdingTaxPaidPerPayPeriod()
    End Function

    Private Function IsFirstPayperiodOfTheYear() As Boolean
        If Me.payPeriod Is Nothing Then
            Return False
        End If

        Return CInt(Me.payPeriod("OrdinalValue")) = 1
    End Function

    Private Sub LoadAnnualUnusedLeaves()
        Dim sql = <![CDATA[
            SELECT RowID, OrganizationID, EmployeeID, TotalLeave, Year
            FROM annualunusedleave
            WHERE OrganizationID = @OrganizationID
                AND Year = @Year;
        ]]>.Value

        Dim currentYear = CInt(Me.payPeriod("Year"))
        Dim previousYear = currentYear - 1

        sql = sql.Replace("@OrganizationID", CStr(z_OrganizationID)) _
            .Replace("@Year", CStr(previousYear))

        Me.annualUnusedLeaves = GetDatatable(sql)
    End Sub

    Private Sub LoadExistingUnusedLeaveAdjustments()
        Dim sql = <![CDATA[
            SELECT
                paystubadjustment.*,
                paystub.EmployeeID
            FROM paystubadjustment
            LEFT JOIN paystub
                ON paystub.RowID = paystubadjustment.PayStubID
            WHERE
                paystubadjustment.ProductID = @ProductID AND
                paystub.PayPeriodID = @PayPeriodID AND
                paystubadjustment.OrganizationID = @OrganizationID;
        ]]>.Value

        sql = sql.Replace("@ProductID", unusedLeaveProductID) _
            .Replace("@OrganizationID", CStr(z_OrganizationID)) _
            .Replace("@PayPeriodID", PayPeriodID)

        Me.existingUnusedLeaveAdjustments = GetDatatable(sql)
    End Sub

    Private Sub LoadCurrentPayperiod()
        Dim sql = <![CDATA[
            SELECT Year, OrdinalValue
            FROM payperiod
            WHERE RowID = @PayPeriodID;
        ]]>.Value

        sql = sql.Replace("@PayPeriodID", Me.PayPeriodID)

        Dim results = GetDatatable(sql)
        Me.payPeriod = results(0)
    End Sub

    Private Sub LoadProductIDForUnusedLeave()
        Dim sql = <![CDATA[
            SELECT RowID
            FROM product
            WHERE PartNo = 'Unused leaves'
                AND OrganizationID = @OrganizationID
            LIMIT 1;
        ]]>.Value

        sql = sql.Replace("@OrganizationID", CStr(z_OrganizationID))

        Dim results = GetDatatable(sql)
        Dim row = results(0)
        Me.unusedLeaveProductID = CStr(row("RowID"))
    End Sub

    Private Sub ResetUnusedLeaveAdjustments()
        Dim sql = <![CDATA[
            DELETE paystubadjustment
            FROM paystubadjustment
            LEFT JOIN paystub
                ON paystub.RowID = paystubadjustment.PayStubID
            WHERE paystubadjustment.ProductID = @ProductID AND
                paystub.PayPeriodID = @PayPeriodID AND
                paystubadjustment.OrganizationID = @OrganizationID;
        ]]>.Value

        sql = sql.Replace("@ProductID", unusedLeaveProductID) _
            .Replace("@OrganizationID", CStr(z_OrganizationID)) _
            .Replace("@PayPeriodID", PayPeriodID)

        EXECQUER(sql)
    End Sub

    Private Sub ConvertLeaveToCash(employee As DataRow)
        Dim employeeID = employee("RowID")
        Dim employeeType = employee("EmployeeType")
        Dim employeeOID = employee("EmployeeID")
        Dim unusedLeave = annualUnusedLeaves.Select($"EmployeeID = '{employeeID}'") _
            .FirstOrDefault()

        ' Perform basic checks to see if employee has any unused leaves
        If unusedLeave Is Nothing Then
            Return
        End If

        Dim totalLeaves = ValNoComma(unusedLeave("TotalLeave"))

        If totalLeaves <= 0D Then
            Return
        End If

        ' Let's find this employee's salary to derive the hourly pay
        Dim salaryAgreement = _allSalaries.Select($"EmployeeID = '{employeeID}'") _
            .FirstOrDefault()

        Dim basicPay = ConvertToType(Of Decimal)(salaryAgreement("BasicPay"))
        Dim hoursInAWorkDay = 8
        Dim workDaysPerYear = ValNoComma(employee("WorkDaysPerYear"))
        Dim workDaysPerMonth = workDaysPerYear / 12

        Dim hourlySalary = 0D
        If CStr(employee("EmployeeType")) = SalaryType.Daily Then
            hourlySalary = basicPay / hoursInAWorkDay
        ElseIf CStr(employee("EmployeeType")) = SalaryType.Monthly Then
            hourlySalary = basicPay / workDaysPerMonth / hoursInAWorkDay
        End If

        Dim adjustmentAmount = hourlySalary * totalLeaves

        ' Make the custom remark for the adjustment
        Dim totalLeavesInDays = totalLeaves / hoursInAWorkDay
        Dim remarks = totalLeavesInDays & " unused leaves"

        Dim existingAdjustment = existingUnusedLeaveAdjustments.Select($"EmployeeID = '{employeeID}'") _
            .FirstOrDefault()

        Dim payStubAdjustmentID = If(existingAdjustment IsNot Nothing, existingAdjustment("RowID"), Nothing)

        Dim a = New ReadSQLFunction(
            "I_paystubadjustment",
            "returnvalue",
            orgztnID,
            z_User,
            unusedLeaveProductID,
            adjustmentAmount,
            remarks,
            employeeOID,
            Me.PayPeriodID,
            payStubAdjustmentID
        )
    End Sub

End Class

Friend Class MySQLExecuteQuery

    Private priv_conn As New MySqlConnection

    Private priv_da As New MySqlDataAdapter

    Private priv_cmd As New MySqlCommand

    Private getResult As Object

    Dim dr As MySqlDataReader

    Sub New(ByVal cmdsql As String,
            Optional cmd_time_out As Integer = 0)

        Static except_this_string() As String = {"CALL", "UPDATE", "DELETE"}

        'Dim n_DataBaseConnection As New DataBaseConnection

        If cmd_time_out > 0 Then
            'n_DataBaseConnection.GetStringMySQLConnectionString
            priv_conn.ConnectionString = mysql_conn_text &
                "default command timeout=" & cmd_time_out & ";"
        Else
            'n_DataBaseConnection.GetStringMySQLConnectionString
            priv_conn.ConnectionString = mysql_conn_text

        End If

        Try

            If priv_conn.State = ConnectionState.Open Then : priv_conn.Close() : End If

            priv_conn.Open()

            priv_cmd = New MySqlCommand

            With priv_cmd

                .CommandType = CommandType.Text

                .Connection = priv_conn

                .CommandText = cmdsql

                If cmd_time_out > 0 Then
                    .CommandTimeout = cmd_time_out
                End If

                If cmdsql.Contains("CALL") Then

                    .ExecuteNonQuery()

                ElseIf FindingWordsInString(cmdsql,
                                            except_this_string) Then

                    .ExecuteNonQuery()
                Else

                    dr = .ExecuteReader()

                End If

            End With

            If cmdsql.Contains("CALL") Then
                getResult = Nothing
            ElseIf FindingWordsInString(cmdsql,
                                        except_this_string) Then
                getResult = Nothing
            Else

                If dr.Read = True Then

                    If IsDBNull(dr(0)) Then
                        getResult = String.Empty
                    Else
                        getResult = dr(0)

                    End If
                Else
                    getResult = Nothing

                End If

            End If
        Catch ex As Exception

            _hasError = True
            _error_message = getErrExcptn(ex, MyBase.ToString)
            'MsgBox(_error_message, , cmdsql)
            Console.WriteLine(_error_message)
        Finally

            If dr IsNot Nothing Then
                dr.Close()
                dr.Dispose()
            End If

            priv_conn.Close()

            priv_cmd.Dispose()

        End Try

    End Sub

    Property Result As Object

        Get
            Return getResult

        End Get

        Set(value As Object)
            getResult = value

        End Set

    End Property

    Sub ExecuteQuery()

    End Sub

    Dim _hasError As Boolean = False

    Property HasError As Boolean

        Get
            Return _hasError

        End Get

        Set(value As Boolean)
            _hasError = value

        End Set

    End Property

    Dim _error_message As String = String.Empty

    Property ErrorMessage As String

        Get
            Return _error_message

        End Get

        Set(value As String)
            _error_message = value

        End Set

    End Property

End Class
