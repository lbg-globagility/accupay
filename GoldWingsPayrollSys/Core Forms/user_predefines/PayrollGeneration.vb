Imports MySql.Data.MySqlClient
Imports log4net

Public Class PayrollGeneration

    Private Shared logger As ILog = LogManager.GetLogger("PayrollLogger")

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

    Private Class PayStubObject
        Public Property RowID As Integer?
        Public Property OrganizationID As Integer?
        Public Property CreatedBy As Integer?
        Public Property LastUpdBy As Integer?
        Public Property PayPeriodID As Integer?
        Public Property EmployeeID As Integer?
        Public Property PayFromDate As Date
        Public Property PayToDate As Date
        Public Property RegularHours As Decimal
        Public Property RegularPay As Decimal
        Public Property OvertimeHours As Decimal
        Public Property OvertimePay As Decimal
        Public Property TotalGrossSalary As Decimal
        Public Property TotalNetSalary As Decimal
        Public Property TotalTaxableSalary As Decimal
        Public Property TotalEmpWithholdingTax As Decimal
        Public Property TotalEmpSSS As Decimal
        Public Property TotalCompSSS As Decimal
        Public Property TotalEmpPhilHealth As Decimal
        Public Property TotalCompPhilHealth As Decimal
        Public Property TotalEmpHDMF As Decimal
        Public Property TotalCompHDMF As Decimal
    End Class

    Private Class PayStubSummary
        Public Property RowID As Integer?
        Public Property PayStubID As Integer?
        Public Property RegularHours As Decimal
        Public Property RegularPay As Decimal
        Public Property OvertimeHours As Decimal
        Public Property OvertimePay As Decimal
        Public Property NightDiffHours As Decimal
        Public Property NightDiffPay As Decimal
        Public Property NightDiffOTHours As Decimal
        Public Property NightDiffOTPay As Decimal
        Public Property RestDayHours As Decimal
        Public Property RestDayPay As Decimal
        Public Property LeavePay As Decimal
        Public Property HolidayPay As Decimal
        Public Property LateHours As Decimal
        Public Property LateDeduction As Decimal
        Public Property UndertimeHours As Decimal
        Public Property UndertimeDeduction As Decimal
        Public Property AbsenceDeduction As Decimal
        Public Property BasicPay As Decimal
    End Class

    Public Property PayrollDateFrom As String
    Public Property PayrollDateTo As String
    Public Property PayPeriodID As String

    Private employee_dattab As DataTable
    Private isEndOfMonth2 As String
    Private _isFirstHalf As Boolean
    Private _isEndOfMonth As Boolean
    Private _allSalaries As DataTable
    Private emp_loans As DataTable
    Private emp_bonus As DataTable

    Private allOneTimeAllowances As DataTable
    Private allDailyAllowances As DataTable
    Private allSemiMonthlyAllowances As DataTable
    Private allMonthlyAllowances As DataTable
    Private allWeeklyAllowances As DataTable

    Private allNoTaxOneTimeAllowances As DataTable
    Private allNoTaxDailyAllowances As DataTable
    Private allNoTaxWeeklyAllowances As DataTable
    Private allNoTaxSemiMonthlyAllowances As DataTable
    Private allNoTaxMonthlyAllowances As DataTable

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

    Private numofdaypresent As DataTable
    Private allTimeEntries As DataTable
    Private dtemployeefirsttimesalary As DataTable
    Private previousTimeEntries As DataTable
    Private VeryFirstPayPeriodIDOfThisYear As Object
    Private withthirteenthmonthpay As SByte

    Private _withholdingTaxTable As DataTable

    Private _filingStatuses As DataTable

    Private fixedNonTaxableMonthlyAllowances As DataTable
    Private fixedTaxableMonthlyAllowances As DataTable

    Private Delegate Sub NotifyMainWindow(ByVal progress_index As Integer)
    Private m_NotifyMainWindow As NotifyMainWindow

    Private form_caller As Form

    Private payPeriod As DataRow
    Private annualUnusedLeaves As DataTable
    Private unusedLeaveProductID As String
    Private existingUnusedLeaveAdjustments As DataTable

    Private agencyFeeSummary As DataTable

    Private _employee As DataRow

    Private _payStub As PayStubObject

    Private _employeeSss As Decimal
    Private _employerSss As Decimal

    Private _employeePhilHealth As Decimal
    Private _employerPhilHealth As Decimal

    Private _employeeHdmf As Decimal
    Private _employerHdmf As Decimal

    Private _philHealthDeductionSchedule As String
    Private _sssDeductionSchedule As String
    Private _hdmfDeductionSchedule As String
    Private _withholdingTaxSchedule As String

    Private mysql_conn As MySqlConnection

    Dim myTrans As MySqlTransaction
    Private numberofweeksthismonth As MySQLExecuteQuery

    Sub New(_employee_dattab As DataTable,
            isEndOfMonth As String,
            _esal_dattab As DataTable,
            _emp_loans As DataTable,
            _emp_bonus As DataTable,
            _emp_allowanceDaily As DataTable,
            _emp_allowanceMonthly As DataTable,
            _emp_allowanceOnce As DataTable,
            _emp_allowanceSemiM As DataTable,
            _emp_allowanceWeekly As DataTable,
            _notax_allowanceDaily As DataTable,
            _notax_allowanceMonthly As DataTable,
            _notax_allowanceOnce As DataTable,
            _notax_allowanceSemiM As DataTable,
            _notax_allowanceWeekly As DataTable,
            _emp_bonusDaily As DataTable,
            _emp_bonusMonthly As DataTable,
            _emp_bonusOnce As DataTable,
            _emp_bonusSemiM As DataTable,
            _emp_bonusWeekly As DataTable,
            _notax_bonusDaily As DataTable,
            _notax_bonusMonthly As DataTable,
            _notax_bonusOnce As DataTable,
            _notax_bonusSemiM As DataTable,
            _notax_bonusWeekly As DataTable,
            _numofdaypresent As DataTable,
            _etent_totdaypay As DataTable,
            _dtemployeefirsttimesalary As DataTable,
            _prev_empTimeEntry As DataTable,
            _VeryFirstPayPeriodIDOfThisYear As Object,
            _withthirteenthmonthpay As SByte,
            filingStatuses As DataTable,
            withholdingTaxTable As DataTable,
            Optional pay_stub_frm As PayStub = Nothing)

        form_caller = pay_stub_frm

        employee_dattab = _employee_dattab
        isEndOfMonth2 = isEndOfMonth
        _allSalaries = _esal_dattab
        emp_loans = _emp_loans
        emp_bonus = _emp_bonus

        allOneTimeAllowances = _emp_allowanceOnce
        allDailyAllowances = _emp_allowanceDaily
        allWeeklyAllowances = _emp_allowanceWeekly
        allSemiMonthlyAllowances = _emp_allowanceSemiM
        allMonthlyAllowances = _emp_allowanceMonthly

        allNoTaxOneTimeAllowances = _notax_allowanceOnce
        allNoTaxDailyAllowances = _notax_allowanceDaily
        allNoTaxWeeklyAllowances = _notax_allowanceWeekly
        allNoTaxSemiMonthlyAllowances = _notax_allowanceSemiM
        allNoTaxMonthlyAllowances = _notax_allowanceMonthly

        allOneTimeBonuses = _emp_bonusOnce
        allDailyBonuses = _emp_bonusDaily
        allWeeklyBonuses = _emp_bonusWeekly
        allSemiMonthlyBonuses = _emp_bonusSemiM
        allMonthlyBonuses = _emp_bonusMonthly

        allNoTaxOneTimeBonuses = _notax_bonusOnce
        allNoTaxDailyBonuses = _notax_bonusDaily
        allNoTaxWeeklyBonuses = _notax_bonusWeekly
        allNoTaxSemiMonthlyBonuses = _notax_bonusSemiM
        allNoTaxMonthlyBonuses = _notax_bonusMonthly

        numofdaypresent = _numofdaypresent
        allTimeEntries = _etent_totdaypay
        dtemployeefirsttimesalary = _dtemployeefirsttimesalary
        previousTimeEntries = _prev_empTimeEntry
        VeryFirstPayPeriodIDOfThisYear = _VeryFirstPayPeriodIDOfThisYear
        withthirteenthmonthpay = _withthirteenthmonthpay

        _filingStatuses = filingStatuses
        _withholdingTaxTable = withholdingTaxTable

        m_NotifyMainWindow = AddressOf pay_stub_frm.ProgressCounter

        _isFirstHalf = (isEndOfMonth = "0")
        _isEndOfMonth = (isEndOfMonth = "1")
    End Sub

    Sub PayrollGeneration_BackgourndWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        DoProcess()
    End Sub

    Public Sub DoProcess()
        Console.WriteLine("PayrolLGeneration - DoProcess")
        Try
            Dim pause_process_message As String = String.Empty

            'LoadCurrentPayperiod()
            'LoadAnnualUnusedLeaves()
            'LoadProductIDForUnusedLeave()
            'LoadExistingUnusedLeaveAdjustments()
            'ResetUnusedLeaveAdjustments()

            Dim sel_employee_dattab = employee_dattab.Select("PositionID IS NULL")

            If sel_employee_dattab.Count > 0 Then
                For Each drow In sel_employee_dattab
                    pause_process_message = "Employee '" & CStr(drow("EmployeeID")) & "' has no position." &
                        vbNewLine & "Please supply his/her position before proceeding to payroll."
                    'e.Cancel = True
                    'If bgworkgenpayroll.CancellationPending Then
                    '    bgworkgenpayroll.CancelAsync()
                    'End If
                Next
            End If

            LoadFixedMonthlyAllowances()

            Dim date_to_use = If(CDate(PayrollDateFrom) > CDate(PayrollDateTo), CDate(PayrollDateFrom), CDate(PayrollDateTo))
            Dim dateStr_to_use = Format(date_to_use, "yyyy-MM-dd")
            numberofweeksthismonth = New MySQLExecuteQuery("SELECT `COUNTTHEWEEKS`('" & dateStr_to_use & "');")

            For Each employee As DataRow In employee_dattab.Rows
                CalculateEmployee(employee)
            Next
        Catch ex As Exception
            Console.WriteLine(getErrExcptn(ex, "PayrollGeneration - " & employee_dattab.TableName))
        Finally
            'employee_dattab.Dispose()
            '_allSalaries.Dispose()
            'emp_loans.Dispose()
            'emp_bonus.Dispose()
            'allDailyAllowances.Dispose()
            'allMonthlyAllowances.Dispose()
            'allOneTimeAllowances.Dispose()
            'allSemiMonthlyAllowances.Dispose()
            'allWeeklyAllowances.Dispose()
            'allNoTaxDailyAllowances.Dispose()
            'allNoTaxMonthlyAllowances.Dispose()
            'allNoTaxOneTimeAllowances.Dispose()
            'allNoTaxSemiMonthlyAllowances.Dispose()
            'allNoTaxWeeklyAllowances.Dispose()
            'allDailyBonuses.Dispose()
            'allMonthlyBonuses.Dispose()
            'allOneTimeBonuses.Dispose()
            'allSemiMonthlyBonuses.Dispose()
            'allWeeklyBonuses.Dispose()
            'allNoTaxDailyBonuses.Dispose()
            'allNoTaxMonthlyBonuses.Dispose()
            'allNoTaxOneTimeBonuses.Dispose()
            'allNoTaxSemiMonthlyBonuses.Dispose()
            'allNoTaxWeeklyBonuses.Dispose()
            'numofdaypresent.Dispose()
            'allTimeEntries.Dispose()
            'dtemployeefirsttimesalary.Dispose()
            'previousTimeEntries.Dispose()
            'withholdingTaxTable.Dispose()
            'filingStatus.Dispose()
            'fixedNonTaxableMonthlyAllowances.Dispose()
            'fixedTaxableMonthlyAllowances.Dispose()
        End Try
    End Sub

    Private Sub CalculateEmployee(employee As DataRow)
        Try
            _payStub = New PayStubObject()

            Dim totalVacationDaysLeft = 0D
            Dim pstub_TotalLoans = 0D
            Dim grossIncomeLastPayPeriod = 0D

            _employee = employee

            _sssDeductionSchedule = employee("SSSDeductSched").ToString
            _philHealthDeductionSchedule = employee("PhHealthDeductSched").ToString
            _hdmfDeductionSchedule = employee("HDMFDeductSched").ToString
            _withholdingTaxSchedule = employee("WTaxDeductSched").ToString

            Dim employeeID = Trim(employee("RowID"))

            Dim workDaysPerYear = employee("WorkDaysPerYear")

            Dim divisorMonthlys = If(
                employee("PayFrequencyID") = 1,
                2,
                If(
                    employee("PayFrequencyID") = 2,
                    1,
                    If(
                        employee("PayFrequencyID") = 3,
                        New MySQLExecuteQuery("SELECT COUNT(RowID) FROM employeetimeentry WHERE EmployeeID='" & employeeID & "' AND Date BETWEEN '" & PayrollDateFrom & "' AND '" & PayrollDateTo & "' AND IFNULL(TotalDayPay,0)!=0 AND OrganizationID='" & orgztnID & "';"),
                        numberofweeksthismonth
                    )
                )
            )


            Dim rowempsal = _allSalaries.Select($"EmployeeID = '{employeeID}'")
            Dim emp_loan = emp_loans.Select($"EmployeeID = '{employeeID}'")
            Dim emp_bon = emp_bonus.Select($"EmployeeID = '{employeeID}'")

            Dim oneTimeAllowances = allOneTimeAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim dailyAllowances = allDailyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim weeklyAllowances = allWeeklyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim semiMonthlyAllowances = allSemiMonthlyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim monthlyAllowances = allMonthlyAllowances.Select($"EmployeeID = '{employeeID}'")

            Dim noTaxOneTimeAllowances = allNoTaxOneTimeAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim noTaxDailyAllowances = allNoTaxDailyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim noTaxWeeklyAllowances = allNoTaxWeeklyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim noTaxSemiMonthlyAllowances = allNoTaxSemiMonthlyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim noTaxMonthlyAllowances = allNoTaxMonthlyAllowances.Select($"EmployeeID = '{employeeID}'")

            Dim oneTimeBonuses = allOneTimeBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim dailyBonuses = allDailyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim weeklyBonuses = allWeeklyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim semiMonthlyBonuses = allSemiMonthlyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim monthlyBonuses = allMonthlyBonuses.Select($"EmployeeID = '{employeeID}'")

            Dim noTaxOneBonuses = allNoTaxOneTimeBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim noTaxDailyBonuses = allNoTaxDailyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim noTaxWeeklyBonuses = allNoTaxWeeklyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim noTaxSemiMonthlyBonuses = allNoTaxSemiMonthlyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim noTaxMonthlyBonuses = allNoTaxMonthlyBonuses.Select($"EmployeeID = '{employeeID}'")

            Dim totalLoanDeduction = Val(0)
            For Each drowloan In emp_loan
                totalLoanDeduction = drowloan("DeductionAmount")
            Next

            Dim valday_allowance = ValNoComma(allDailyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{employeeID}'"))
            Dim valmonth_allowance = ValNoComma(allMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{employeeID}'"))

            Dim valonce_allowance = 0.0
            For Each drowonceallow In oneTimeAllowances
                valonce_allowance = drowonceallow("TotalAllowanceAmount")
            Next

            Dim valsemim_allowance = ValNoComma(allSemiMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{employeeID}'"))
            'For Each drowsemimallow In semim_allowance
            '    valonce_allowance = drowsemimallow("TotalAllowanceAmount")
            'Next

            Dim valweek_allowance = 0.0
            For Each drowweekallow In weeklyAllowances
                valonce_allowance = drowweekallow("TotalAllowanceAmount")
            Next

            Dim totalFixedTaxableMonthlyAllowance = ValNoComma(fixedTaxableMonthlyAllowances.Compute("SUM(AllowanceAmount)", $"EmployeeID = '{employeeID}'"))

            'If isEndOfMonth = 1 Then
            '    totalFixedTaxableMonthlyAllowance = 0.0
            'End If

            Dim totalTaxableAllowance = (
                valday_allowance +
                valmonth_allowance +
                valonce_allowance +
                valsemim_allowance +
                valweek_allowance +
                totalFixedTaxableMonthlyAllowance
            )

            Dim totalNoTaxDailyAllowance = ValNoComma(allNoTaxDailyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{employeeID}'"))
            Dim totalNoTaxMonthlyAllowance = ValNoComma(allNoTaxMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{employeeID}'"))

            'If isEndOfMonth = 1 Then

            '    For Each drowmonallow In monthnotax_allowance

            '        valmonthnotax_allowance = drowmonallow("TotalAllowanceAmount") ' / divisorMonthlys

            '        Exit For

            '    Next

            'End If

            Dim valoncenotax_allowance = ValNoComma(allNoTaxOneTimeAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{employeeID}'"))

            'For Each drowonceallow In oncenotax_allowance
            '    valoncenotax_allowance = drowonceallow("TotalAllowanceAmount")
            'Next

            Dim valsemimnotax_allowance = ValNoComma(allNoTaxSemiMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{employeeID}'"))
            'For Each drowsemimallow In semimnotax_allowance
            '    valoncenotax_allowance = drowsemimallow("TotalAllowanceAmount")
            'Next

            Dim valweeknotax_allowance = 0D
            'For Each drowweekallow In weeknotax_allowance
            '    valoncenotax_allowance = drowweekallow("TotalAllowanceAmount")
            'Next

            Dim totalFixedNonTaxableMonthlyAllowance = ValNoComma(fixedNonTaxableMonthlyAllowances.Compute("SUM(AllowanceAmount)", $"EmployeeID = '{employeeID}'"))

            Dim totalNoTaxAllowance = (
                totalNoTaxDailyAllowance +
                totalNoTaxMonthlyAllowance +
                valoncenotax_allowance +
                valsemimnotax_allowance +
                valweeknotax_allowance +
                totalFixedNonTaxableMonthlyAllowance
            )


            Dim valday_bon = 0.0
            For Each drowdaybon In dailyBonuses
                'valday_bon = drowdaybon("BonusAmount")

                'If drow("EmployeeType").ToString = "Fixed" Then
                '    valday_bon = valday_bon * numofweekdays 'numofweekends
                'Else
                '    Dim daymultiplier = numofdaypresent.Select("EmployeeID = '" & drow("RowID").ToString & "'")
                '    For Each drowdaymultip In daymultiplier
                '        Dim i_val = Val(drowdaymultip("DaysAttended"))
                '        valday_bon = valday_bon * i_val
                '        Exit For
                '    Next

                'End If

                Exit For
            Next

            Dim valmonth_bon = 0.0

            If isEndOfMonth2 = 1 Then

                For Each drowmonbon In monthlyBonuses
                    'valmonth_bon = drowmonbon("BonusAmount")
                Next

            End If

            Dim valonce_bon = 0.0
            For Each drowoncebon In oneTimeBonuses
                'valonce_bon = drowoncebon("BonusAmount")
            Next

            Dim valsemim_bon = 0.0
            For Each drowsemimbon In semiMonthlyBonuses
                'valonce_bon = drowsemimbon("BonusAmount")
            Next

            Dim valweek_bon = 0.0
            For Each drowweekbon In weeklyBonuses
                'valonce_bon = drowweekbon("BonusAmount")
            Next

            'this is taxable                        ' / divisorMonthlys
            Dim totalTaxableBonus = (
                valday_bon +
                valmonth_bon +
                valonce_bon +
                valsemim_bon +
                valweek_bon
            )

            Dim valdaynotax_bon = 0.0
            For Each drowdaybon In noTaxDailyBonuses
                valdaynotax_bon = drowdaybon("BonusAmount")

                If employee("EmployeeType").ToString = "Fixed" Then
                    valdaynotax_bon = valdaynotax_bon * workDaysPerYear 'numofweekends
                Else
                    Dim daymultiplier = numofdaypresent.Select($"EmployeeID = '{employeeID}'")
                    For Each drowdaymultip In daymultiplier
                        Dim i_val = Val(drowdaymultip("DaysAttended"))
                        valdaynotax_bon = valdaynotax_bon * i_val
                        Exit For
                    Next

                End If

                Exit For
            Next

            Dim valmonthnotax_bon = 0.0

            If isEndOfMonth2 = 1 Then

                For Each drowmonbon In noTaxMonthlyBonuses
                    'valmonthnotax_bon = drowmonbon("BonusAmount")
                Next

            End If

            Dim valoncenotax_bon = 0.0
            For Each drowoncebon In noTaxOneBonuses
                'valoncenotax_bon = drowoncebon("BonusAmount")
            Next

            Dim valsemimnotax_bon = 0.0
            For Each drowsemimbon In noTaxSemiMonthlyBonuses
                'valoncenotax_bon = drowsemimbon("BonusAmount")
            Next

            Dim valweeknotax_bon = 0.0
            For Each drowweekbon In noTaxWeeklyBonuses
                'valoncenotax_bon = drowweekbon("BonusAmount")
            Next

            'this is non-taxable
            Dim totalNoTaxBonus = (
                valoncenotax_bon +
                valdaynotax_bon +
                valweeknotax_bon +
                valsemimnotax_bon +
                (valmonthnotax_bon / divisorMonthlys)
            )

            Dim timeEntries = allTimeEntries.Select($"EmployeeID = '{employeeID}'")

            Dim grossIncome = 0D

            totalVacationDaysLeft = Val(0)
            pstub_TotalLoans = Val(0)
            Dim taxAmount = 0D
            Dim totalBonus = totalTaxableBonus + totalNoTaxBonus

            Dim totalAllowance = totalTaxableAllowance + totalNoTaxAllowance
            Dim governmentContributions = 0D

            Dim taxableSalary = 0.0

            For Each timeEntrySummary In timeEntries

                grossIncomeLastPayPeriod = Val(0)
                _employeeSss = 0D
                _employerSss = 0D
                _employeePhilHealth = 0D
                _employerPhilHealth = 0D
                _employeeHdmf = 0D
                _employerHdmf = 0D
                totalVacationDaysLeft = Val(0)
                pstub_TotalLoans = Val(0)

                For Each drowsal In rowempsal

                    Dim skipGovernmentDeductions = CStr(employee("IsFirstTimeSalary")) = "1"
                    Dim basicPay = ValNoComma(drowsal("BasicPay"))

                    Dim regularHours = Decimal.Parse(timeEntrySummary("RegularHours"))
                    Dim regularPay = Decimal.Parse(timeEntrySummary("RegularHoursAmount"))

                    Dim overtimeHours = Decimal.Parse(timeEntrySummary("OvertimeHours"))
                    Dim overtimePay = Decimal.Parse(timeEntrySummary("OvertimeHoursAmount"))

                    Dim nightDiffPay = Decimal.Parse(timeEntrySummary("NightDiffHoursAmount"))
                    Dim nightDiffOTPay = Decimal.Parse(timeEntrySummary("NightDiffOTHoursAmount"))

                    Dim sel_dtemployeefirsttimesalary = dtemployeefirsttimesalary.Select($"EmployeeID = '{employeeID}'")
                    Dim StartingAttendanceCount = ValNoComma(employee("StartingAttendanceCount"))
                    Dim employmentType = StrConv(employee("EmployeeType").ToString, VbStrConv.ProperCase)

                    If employmentType = SalaryType.Fixed Then

                        grossIncome = basicPay + (overtimePay + nightDiffPay + nightDiffOTPay)

                        Dim previousOvertimePay = ValNoComma(previousTimeEntries.Compute("SUM(OvertimeHoursAmount)", $"EmployeeID = '{employeeID}'"))
                        Dim previousNightDiffPay = ValNoComma(previousTimeEntries.Compute("SUM(NightDiffHoursAmount)", $"EmployeeID = '{employeeID}'"))
                        Dim previousNightDiffOTPay = ValNoComma(previousTimeEntries.Compute("SUM(NightDiffOTHoursAmount)", $"EmployeeID = '{employeeID}'"))

                        grossIncomeLastPayPeriod = basicPay + previousOvertimePay + previousNightDiffPay + previousNightDiffOTPay

                    ElseIf employmentType = SalaryType.Monthly Then

                        If skipGovernmentDeductions Then
                            grossIncome = ValNoComma(timeEntrySummary("TotalDayPay"))
                            grossIncomeLastPayPeriod = ValNoComma(previousTimeEntries.Compute("SUM(TotalDayPay)", $"EmployeeID = '{employeeID}'"))
                        Else
                            Dim lateDeduction = ValNoComma(timeEntrySummary("HoursLateAmount"))
                            Dim undertimeDeduction = ValNoComma(timeEntrySummary("UndertimeHoursAmount"))
                            Dim absenceDeduction = ValNoComma(timeEntrySummary("Absent"))
                            Dim totalDeduction = lateDeduction + undertimeDeduction + absenceDeduction

                            grossIncome = basicPay - totalDeduction

                            If previousTimeEntries.Select($"EmployeeID = '{employeeID}'").Count > 0 Then
                                grossIncomeLastPayPeriod = basicPay
                            End If

                            Dim previousLateDeduction = ValNoComma(previousTimeEntries.Compute("SUM(HoursLateAmount)", $"EmployeeID = '{employeeID}'"))
                            Dim previousUndertimeDeduction = ValNoComma(previousTimeEntries.Compute("SUM(UndertimeHoursAmount)", $"EmployeeID = '{employeeID}'"))
                            Dim previousAbsenceDeduction = ValNoComma(previousTimeEntries.Compute("SUM(Absent)", $"EmployeeID = '{employeeID}'"))
                            Dim totalPreviousDeduction = previousLateDeduction + previousUndertimeDeduction + previousAbsenceDeduction

                            grossIncomeLastPayPeriod -= totalPreviousDeduction
                            grossIncomeLastPayPeriod += ValNoComma(previousTimeEntries.Compute("MIN(emtAmount)", $"EmployeeID = '{employeeID}'"))
                        End If

                    ElseIf employmentType = SalaryType.Daily Then
                        grossIncome = ValNoComma(timeEntrySummary("TotalDayPay"))
                        grossIncomeLastPayPeriod = ValNoComma(previousTimeEntries.Compute("SUM(TotalDayPay)", $"EmployeeID = '{employeeID}'"))
                    End If

                    CalculateSss(drowsal)
                    CalculatePhilHealth(drowsal)
                    CalculateHdmf(drowsal)

                    governmentContributions = _payStub.TotalEmpSSS + _employeePhilHealth + _employeeHdmf

                    Dim dailyRate = ValNoComma(employee("EmpRatePerDay"))
                    Dim minimumWage = ValNoComma(employee("MinimumWageAmount"))
                    Dim isMinimumWageEarner = dailyRate <= minimumWage

                    If IsWithholdingTaxPaidOnFirstHalf() Or IsWithholdingTaxPaidOnEndOfTheMonth() Then
                        taxableSalary = (grossIncome + grossIncomeLastPayPeriod) - governmentContributions
                    Else
                        taxableSalary = grossIncome - governmentContributions
                    End If

                    If isMinimumWageEarner Then
                        taxAmount = 0
                    Else
                        Dim maritalStatus = employee("MaritalStatus").ToString
                        Dim noOfDependents = employee("NoOfDependents").ToString
                        Dim sel_filingStatus = _filingStatuses.Select($"MaritalStatus = '{maritalStatus}' AND Dependent = '{noOfDependents}'")
                        Dim filingStatusID = 1

                        For Each fstatrow In sel_filingStatus
                            filingStatusID = fstatrow("RowID")
                        Next

                        Dim sel_payWTax = _withholdingTaxTable.Select($"
                            FilingStatusID = '{filingStatusID}' AND
                            PayFrequencyID = 2 AND
                            TaxableIncomeFromAmount <= {taxableSalary} AND {taxableSalary} <= TaxableIncomeToAmount
                        ").FirstOrDefault()

                        If sel_payWTax IsNot Nothing Then
                            Dim exemptionAmount = ValNoComma(sel_payWTax("ExemptionAmount"))
                            Dim taxableIncomeFromAmount = ValNoComma(sel_payWTax("TaxableIncomeFromAmount"))
                            Dim exemptionInExcessAmount = ValNoComma(sel_payWTax("ExemptionInExcessAmount"))

                            taxAmount = exemptionAmount + ((taxableSalary - taxableIncomeFromAmount) * exemptionInExcessAmount)
                        End If
                    End If

                    Exit For
                Next

                Exit For
            Next


            Dim totalNetPay = (grossIncome + totalBonus + totalAllowance) - governmentContributions - totalLoanDeduction - taxAmount)

            mysql_conn = New MySqlConnection(mysql_conn_text)
            Dim command = New MySqlCommand("INSUPDPROC_paystub", mysql_conn, myTrans)

            If mysql_conn.State = ConnectionState.Closed Then
                mysql_conn.Open()
            End If

            myTrans = mysql_conn.BeginTransaction()
            command.CommandTimeout = 5000
            command.CommandType = CommandType.StoredProcedure

            With command.Parameters
                .Clear()
                .AddWithValue("pstub_RowID", DBNull.Value)
                .AddWithValue("pstub_OrganizationID", orgztnID)
                .AddWithValue("pstub_CreatedBy", z_User)
                .AddWithValue("pstub_LastUpdBy", z_User)
                .AddWithValue("pstub_PayPeriodID", PayPeriodID)
                .AddWithValue("pstub_EmployeeID", employeeID)

                .AddWithValue("pstub_TimeEntryID", DBNull.Value)

                .AddWithValue("pstub_PayFromDate", PayrollDateFrom)
                .AddWithValue("pstub_PayToDate", PayrollDateTo)

                .AddWithValue("pstub_TotalGrossSalary", grossIncome + totalBonus + totalAllowance)
                .AddWithValue("pstub_TotalNetSalary", totalNetPay)
                .AddWithValue("pstub_TotalTaxableSalary", taxableSalary)

                .AddWithValue("pstub_TotalEmpWithholdingTax", taxAmount)
                .AddWithValue("pstub_TotalEmpSSS", _payStub.TotalEmpSSS)
                .AddWithValue("pstub_TotalCompSSS", _payStub.TotalCompSSS)
                .AddWithValue("pstub_TotalEmpPhilhealth", _employeePhilHealth)
                .AddWithValue("pstub_TotalCompPhilhealth", _employerPhilHealth)
                .AddWithValue("pstub_TotalEmpHDMF", _employeeHdmf)
                .AddWithValue("pstub_TotalCompHDMF", _employerHdmf)
                .AddWithValue("pstub_TotalVacationDaysLeft", totalVacationDaysLeft)
                .AddWithValue("pstub_TotalLoans", totalLoanDeduction)
                .AddWithValue("pstub_TotalBonus", totalBonus)
                .AddWithValue("pstub_TotalAllowance", totalAllowance)
            End With

            command.ExecuteNonQuery()
            myTrans.Commit()
            command.Dispose()

            'If IsFirstPayperiodOfTheYear() Then
            '    ConvertLeaveToCash(drow)
            'End If

            form_caller.Invoke(m_NotifyMainWindow, 1)
            Dim my_cmd As String = String.Concat(Convert.ToString(employee("RowID")), "@", Convert.ToString(employee("EmployeeID")))
            Console.WriteLine(my_cmd)

        Catch ex As Exception
            myTrans.Rollback()

            Dim this_err As String = String.Concat(getErrExcptn(ex, "PayrollGeneration"), " -- ", Convert.ToString(employee_dattab.TableName),
                                                       ".Employee ID[", Convert.ToString(employee("EmployeeID")), "]")
            logger.Error($"Error calculting employee #{employee("EmployeeID")}.", ex)
            Console.WriteLine(this_err)
        Finally
            If mysql_conn IsNot Nothing Then
                mysql_conn.Close()
                mysql_conn.Dispose()
            End If
        End Try
    End Sub

    Private Sub LoadFixedMonthlyAllowances()
        ' Construct sql to load all fixed monthly allowances for all employees
        Dim fixedTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.EffectiveStartDate <= '" & PayrollDateFrom & "' " &
            "AND ea.EffectiveEndDate >= '" & PayrollDateTo & "' " &
            "AND ea.AllowanceFrequency = 'Monthly' " &
            "AND p.`Fixed` = 1 " &
            "AND p.`Status` = 1;"

        If isEndOfMonth2 = 0 Then 'means end of the month
            fixedTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.AllowanceFrequency IN ('Monthly','Semi-monthly') " &
            " AND (ea.EffectiveStartDate >= '" & PayrollDateFrom & "' OR ea.EffectiveEndDate >= '" & PayrollDateFrom & "')" &
            " AND (ea.EffectiveStartDate <= '" & PayrollDateTo & "' OR ea.EffectiveEndDate <= '" & PayrollDateTo & "')" &
            " AND p.`Fixed` = 1 " &
            "AND p.`Status` = 1;"
        Else '                      'means first half of the month
            fixedTaxableMonthlyAllowanceSql =
                "SELECT ea.* " &
                "FROM employeeallowance ea " &
                "INNER JOIN product p " &
                "ON p.RowID = ea.ProductID " &
                "WHERE ea.OrganizationID = '" & orgztnID & "' " &
                "AND ea.AllowanceFrequency = 'Semi-monthly' " &
                " AND (ea.EffectiveStartDate >= '" & PayrollDateFrom & "' OR ea.EffectiveEndDate >= '" & PayrollDateFrom & "')" &
                " AND (ea.EffectiveStartDate <= '" & PayrollDateTo & "' OR ea.EffectiveEndDate <= '" & PayrollDateTo & "')" &
                " AND p.`Fixed` = 1 " &
                "AND p.`Status` = 1;"
        End If

        fixedTaxableMonthlyAllowances = New MySQLQueryToDataTable(fixedTaxableMonthlyAllowanceSql).ResultTable

        Dim fixedNonTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.EffectiveStartDate <= '" & PayrollDateFrom & "' " &
            "AND ea.EffectiveEndDate >= '" & PayrollDateTo & "' " &
            "AND ea.AllowanceFrequency = 'Monthly' " &
            "AND p.`Fixed` = 1 " &
            "AND p.`Status` = 0;"

        If isEndOfMonth2 = 0 Then 'means end of the month
            fixedNonTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.AllowanceFrequency IN ('Monthly','Semi-monthly') " &
            " AND (ea.EffectiveStartDate >= '" & PayrollDateFrom & "' OR ea.EffectiveEndDate >= '" & PayrollDateFrom & "')" &
            " AND (ea.EffectiveStartDate <= '" & PayrollDateTo & "' OR ea.EffectiveEndDate <= '" & PayrollDateTo & "')" &
            " AND p.`Fixed` = 1 " &
            "AND p.`Status` = 0;"
        Else '                      'means first half of the month
            fixedNonTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.AllowanceFrequency = 'Semi-monthly' " &
            " AND (ea.EffectiveStartDate >= '" & PayrollDateFrom & "' OR ea.EffectiveEndDate >= '" & PayrollDateFrom & "')" &
            " AND (ea.EffectiveStartDate <= '" & PayrollDateTo & "' OR ea.EffectiveEndDate <= '" & PayrollDateTo & "')" &
            " AND p.`Fixed` = 1 " &
            "AND p.`Status` = 0;"
        End If

        fixedNonTaxableMonthlyAllowances = New MySQLQueryToDataTable(fixedNonTaxableMonthlyAllowanceSql).ResultTable
    End Sub

    Private Sub CalculateSss(salary As DataRow)
        Dim employeeSssPerMonth = CDec(salary("EmployeeContributionAmount"))
        Dim employerSssPerMonth = CDec(salary("EmployerContributionAmount"))
        Dim payPeriodsPerMonth = CDec(ValNoComma(_employee("PAYFREQUENCY_DIVISOR")))

        If IsSssPaidOnFirstHalf() Or IsSssPaidOnEndOfTheMonth() Then
            _payStub.TotalEmpSSS = employeeSssPerMonth
            _payStub.TotalCompSSS = employerSssPerMonth

            _employeeSss = employeeSssPerMonth
            _employerSss = employerSssPerMonth
        ElseIf IsSssPaidPerPayPeriod() Then
            _payStub.TotalEmpSSS = employeeSssPerMonth / payPeriodsPerMonth
            _payStub.TotalCompSSS = employerSssPerMonth / payPeriodsPerMonth

            _employeeSss = employeeSssPerMonth / payPeriodsPerMonth
            _employerSss = employerSssPerMonth / payPeriodsPerMonth
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
        Dim employeePhilHealthPerMonth = CDec(salary("EmployeeShare"))
        Dim employerPhilHealthPerMonth = CDec(salary("EmployerShare"))
        Dim payPeriodsPerMonth = CDec(ValNoComma(_employee("PAYFREQUENCY_DIVISOR")))

        If IsPhilHealthPaidOnFirstHalf() Or IsPhilHealthPaidOnEndOfTheMonth() Then
            _payStub.TotalEmpPhilHealth = employeePhilHealthPerMonth
            _payStub.TotalCompPhilHealth = employerPhilHealthPerMonth

            _employeePhilHealth = employeePhilHealthPerMonth
            _employerPhilHealth = employerPhilHealthPerMonth
        ElseIf IsPhilHealthPaidPerPayPeriod() Then
            _payStub.TotalEmpPhilHealth = employeePhilHealthPerMonth / payPeriodsPerMonth
            _payStub.TotalCompPhilHealth = employerPhilHealthPerMonth / payPeriodsPerMonth

            _employeePhilHealth = employeePhilHealthPerMonth / payPeriodsPerMonth
            _employerPhilHealth = employerPhilHealthPerMonth / payPeriodsPerMonth
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
        Dim employeeHdmfPerMonth = CDec(salary("HDMFAmount"))
        Dim employerHdmfPerMonth = 100D
        Dim payPeriodsPerMonth = CDec(ValNoComma(_employee("PAYFREQUENCY_DIVISOR")))

        If IsHdmfPaidOnFirstHalf() Or IsHdmfPaidOnEndOfTheMonth() Then
            _payStub.TotalEmpHDMF = employeeHdmfPerMonth
            _payStub.TotalCompHDMF = employerHdmfPerMonth

            _employeeHdmf = employeeHdmfPerMonth
            _employerHdmf = employerHdmfPerMonth
        ElseIf IsHdmfPaidPerPayPeriod() Then
            _payStub.TotalEmpHDMF = employeeHdmfPerMonth / payPeriodsPerMonth
            _payStub.TotalCompHDMF = employerHdmfPerMonth / payPeriodsPerMonth

            _employeeHdmf = employeeHdmfPerMonth / payPeriodsPerMonth
            _employerHdmf = employerHdmfPerMonth / payPeriodsPerMonth
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

    Private Function IsWithholdingTaxPaidOnFirstHalf() As Boolean
        Return _isFirstHalf And (_withholdingTaxSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsWithholdingTaxPaidOnEndOfTheMonth() As Boolean
        Return _isEndOfMonth And (_withholdingTaxSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsWithholdingTaxPaidPerPayPeriod() As Boolean
        Return _withholdingTaxSchedule = ContributionSchedule.PerPayPeriod
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
            .Replace("@OrganizationID", z_OrganizationID) _
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

        sql = sql.Replace("@OrganizationID", z_OrganizationID)

        Dim results = GetDatatable(sql)
        Dim row = results(0)
        Me.unusedLeaveProductID = row("RowID")
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
            .Replace("@OrganizationID", z_OrganizationID) _
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

        Dim totalLeaves = CDec(unusedLeave("TotalLeave"))

        If totalLeaves <= 0D Then
            Return
        End If

        ' Let's find this employee's salary to derive the hourly pay
        Dim salaryAgreement = _allSalaries.Select($"EmployeeID = '{employeeID}'") _
            .FirstOrDefault()

        Dim basicPay = CDec(salaryAgreement("BasicPay"))
        Dim hoursInAWorkDay = 8
        Dim workDaysPerYear = CDec(employee("WorkDaysPerYear"))
        Dim workDaysPerMonth = workDaysPerYear / 12

        Dim hourlySalary = 0D
        If employee("EmployeeType") = "Daily" Then
            hourlySalary = basicPay / hoursInAWorkDay
        ElseIf employee("EmployeeType") = "Monthly" Then
            hourlySalary = basicPay / workDaysPerMonth / hoursInAWorkDay
        End If

        Dim adjustmentAmount = hourlySalary * totalLeaves

        ' Make the custom remark for the adjustment
        Dim totalLeavesInDays = totalLeaves / hoursInAWorkDay
        Dim remarks = totalLeavesInDays & " unused leaves"

        Dim existingAdjustment = Me.existingUnusedLeaveAdjustments.Select("EmployeeID=" & employeeID) _
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

Friend Class MySQLQueryToDataTable

    Private priv_conn As New MySqlConnection

    Private priv_da As New MySqlDataAdapter

    Private priv_cmd As New MySqlCommand

    Sub New(SQLProcedureName As String,
            Optional cmd_time_out As Integer = 0)

        'Static mysql_conn_text As String = n_DataBaseConnection.GetStringMySQLConnectionString

        'Dim n_DataBaseConnection As New DataBaseConnection

        If cmd_time_out > 0 Then

            priv_conn.ConnectionString = mysql_conn_text &
                "default command timeout=" & cmd_time_out & ";"

        Else

            priv_conn.ConnectionString = mysql_conn_text

        End If

        SQLProcedureName = SQLProcedureName.Trim

        Try

            If priv_conn.State = ConnectionState.Open Then
                priv_conn.Close()

            End If

            priv_conn.Open()

            priv_cmd = New MySqlCommand

            With priv_cmd

                .Parameters.Clear()

                .Connection = priv_conn

                .CommandText = SQLProcedureName

                .CommandType = CommandType.Text

                priv_da.SelectCommand = priv_cmd

                priv_da.Fill(n_ResultTable)

            End With

        Catch ex As Exception
            _hasError = True
            'MsgBox()
            Console.WriteLine(getErrExcptn(ex, MyBase.ToString))
        Finally
            priv_da.Dispose()

            priv_conn.Close()

            priv_cmd.Dispose()

        End Try

    End Sub

    Dim n_ResultTable As New DataTable

    Property ResultTable As DataTable

        Get
            Return n_ResultTable

        End Get

        Set(value As DataTable)

            n_ResultTable = value

        End Set

    End Property

    Dim _hasError As Boolean = False

    Property HasError As Boolean

        Get
            Return _hasError

        End Get

        Set(value As Boolean)
            _hasError = value

        End Set

    End Property

End Class

Friend Class MySQLExecuteQuery

    Private priv_conn As New MySqlConnection

    Private priv_da As New MySqlDataAdapter

    Private priv_cmd As New MySqlCommand

    Private getResult = Nothing

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
