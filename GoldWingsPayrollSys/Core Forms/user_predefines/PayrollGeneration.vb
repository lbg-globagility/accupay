Imports MySql.Data.MySqlClient

Public Class PayrollGeneration

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

    Private withholdingTaxTable As DataTable

    Private filingStatus As DataTable

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

    Private _employeeSSS As Decimal
    Private _employerSSS As Decimal

    Private _employeePhilHealth As Decimal
    Private _employerPhilHealth As Decimal

    Private _employeeHdmf As Decimal
    Private _employerHdmf As Decimal

    'ByVal _isorgPHHdeductsched As SByte,
    'ByVal _isorgSSSdeductsched As SByte,
    'ByVal _isorgHDMFdeductsched As SByte,
    'ByVal _isorgWTaxdeductsched As SByte,

    Public Property PayrollDateFrom As String

    Private n_PayrollDateTo As Object = Nothing

    Property PayrollDateTo As Object
        Get
            Return n_PayrollDateTo
        End Get
        Set(value As Object)
            n_PayrollDateTo = value
        End Set
    End Property

    Private n_PayrollRecordID As Object = Nothing

    Property PayrollRecordID As Object
        Get
            Return n_PayrollRecordID
        End Get
        Set(value As Object)
            n_PayrollRecordID = value
        End Set
    End Property

    Private _philHealthDeductionSchedule As String
    Private _sssDeductionSchedule As String
    Private _hdmfDeductionSchedule As String
    Private _withholdingTaxSchedule As String

    Private n_PayStub As New PayStub

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

        withholdingTaxTable = New MySQLQueryToDataTable("SELECT * FROM paywithholdingtax;").ResultTable
        filingStatus = New MySQLQueryToDataTable("SELECT * FROM filingstatus;").ResultTable

        m_NotifyMainWindow = AddressOf pay_stub_frm.ProgressCounter

        _isFirstHalf = (isEndOfMonth = "0")
        _isEndOfMonth = (isEndOfMonth = "1")
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

            'Dim str_pay_freq As New AutoCompleteStringCollection

            'enlistTheLists("SELECT CONCAT('\'',DisplayValue,'\'') AS DisplayValue FROM listofval WHERE LOCATE('monthly',DisplayValue) > 0 AND `Type`='Allowance Frequency' AND Active='Yes' ORDER BY OrderBy;",
            '                str_pay_freq)

            'Dim new_array As New List(Of String)

            'For Each strval In str_pay_freq
            '    new_array.Add(strval)
            'Next

            'Dim n_array = new_array.ToArray()

            '[String].Join(",", n_array)
            LoadFixedMonthlyAllowances()

            Dim date_to_use = If(CDate(PayrollDateFrom) > CDate(n_PayrollDateTo), CDate(PayrollDateFrom), CDate(n_PayrollDateTo))
            Dim dateStr_to_use = Format(date_to_use, "yyyy-MM-dd")
            numberofweeksthismonth = New MySQLExecuteQuery("SELECT `COUNTTHEWEEKS`('" & dateStr_to_use & "');")

            For Each employee As DataRow In employee_dattab.Rows
                CalculateEmployee(employee)
            Next
        Catch ex As Exception
            Console.WriteLine(getErrExcptn(ex, "PayrollGeneration - " & employee_dattab.TableName))
        Finally
            employee_dattab.Dispose()
            _allSalaries.Dispose()
            emp_loans.Dispose()
            emp_bonus.Dispose()
            allDailyAllowances.Dispose()
            allMonthlyAllowances.Dispose()
            allOneTimeAllowances.Dispose()
            allSemiMonthlyAllowances.Dispose()
            allWeeklyAllowances.Dispose()
            allNoTaxDailyAllowances.Dispose()
            allNoTaxMonthlyAllowances.Dispose()
            allNoTaxOneTimeAllowances.Dispose()
            allNoTaxSemiMonthlyAllowances.Dispose()
            allNoTaxWeeklyAllowances.Dispose()
            allDailyBonuses.Dispose()
            allMonthlyBonuses.Dispose()
            allOneTimeBonuses.Dispose()
            allSemiMonthlyBonuses.Dispose()
            allWeeklyBonuses.Dispose()
            allNoTaxDailyBonuses.Dispose()
            allNoTaxMonthlyBonuses.Dispose()
            allNoTaxOneTimeBonuses.Dispose()
            allNoTaxSemiMonthlyBonuses.Dispose()
            allNoTaxWeeklyBonuses.Dispose()
            numofdaypresent.Dispose()
            allTimeEntries.Dispose()
            dtemployeefirsttimesalary.Dispose()
            previousTimeEntries.Dispose()
            withholdingTaxTable.Dispose()
            filingStatus.Dispose()
            fixedNonTaxableMonthlyAllowances.Dispose()
            fixedTaxableMonthlyAllowances.Dispose()

        End Try

    End Sub

    Private Sub CalculateEmployee(drow As DataRow)
        Try
            Dim payStub = New PayStubObject()

            Dim pstub_TotalVacationDaysLeft As Decimal
            Dim pstub_TotalLoans As Decimal
            Dim emp_taxabsal = 0D
            Dim tax_amount = 0D
            Dim grossIncome = 0D
            Dim grossincome_firsthalf = Val(0)

            _employee = drow

            _sssDeductionSchedule = drow("SSSDeductSched").ToString
            _philHealthDeductionSchedule = drow("PhHealthDeductSched").ToString
            _hdmfDeductionSchedule = drow("HDMFDeductSched").ToString
            _withholdingTaxSchedule = drow("WTaxDeductSched").ToString

            Dim employeeID = Trim(drow("RowID"))

            Dim workDaysPerYear = drow("WorkDaysPerYear")

            Dim divisorMonthlys = If(
                drow("PayFrequencyID") = 1,
                2,
                If(
                    drow("PayFrequencyID") = 2,
                    1,
                    If(
                        drow("PayFrequencyID") = 3,
                        New MySQLExecuteQuery("SELECT COUNT(RowID) FROM employeetimeentry WHERE EmployeeID='" & employeeID & "' AND Date BETWEEN '" & PayrollDateFrom & "' AND '" & n_PayrollDateTo & "' AND IFNULL(TotalDayPay,0)!=0 AND OrganizationID='" & orgztnID & "';"),
                        numberofweeksthismonth)
                    )
                )


            Dim rowempsal = _allSalaries.Select($"EmployeeID = '{employeeID}'")
            Dim emp_loan = emp_loans.Select($"EmployeeID = '{employeeID}'")
            Dim emp_bon = emp_bonus.Select($"EmployeeID = '{employeeID}'")

            Dim once_allowance = allOneTimeAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim day_allowance = allDailyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim week_allowance = allWeeklyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim semim_allowance = allSemiMonthlyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim month_allowance = allMonthlyAllowances.Select($"EmployeeID = '{employeeID}'")

            Dim oncenotax_allowance = allNoTaxOneTimeAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim daynotax_allowance = allNoTaxDailyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim weeknotax_allowance = allNoTaxWeeklyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim semimnotax_allowance = allNoTaxSemiMonthlyAllowances.Select($"EmployeeID = '{employeeID}'")
            Dim monthnotax_allowance = allNoTaxMonthlyAllowances.Select($"EmployeeID = '{employeeID}'")

            Dim once_bon = allOneTimeBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim day_bon = allDailyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim week_bon = allWeeklyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim semim_bon = allSemiMonthlyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim month_bon = allMonthlyBonuses.Select($"EmployeeID = '{employeeID}'")

            Dim oncenotax_bon = allNoTaxOneTimeBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim daynotax_bon = allNoTaxDailyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim weeknotax_bon = allNoTaxWeeklyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim semimnotax_bon = allNoTaxSemiMonthlyBonuses.Select($"EmployeeID = '{employeeID}'")
            Dim monthnotax_bon = allNoTaxMonthlyBonuses.Select($"EmployeeID = '{employeeID}'")

            Dim valemp_loan = Val(0)
            For Each drowloan In emp_loan
                valemp_loan = drowloan("DeductionAmount")
            Next

            'Dim valemp_bon = Val(0)
            'For Each drowbon In emp_bon
            '    valemp_bon = drowbon("BonusAmount")
            'Next

            Dim valday_allowance = ValNoComma(allDailyAllowances.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & employeeID & "'"))

            Dim valmonth_allowance = ValNoComma(allMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & employeeID & "'"))

            'If isEndOfMonth = 1 Then

            '    For Each drowmonallow In month_allowance

            '        valmonth_allowance = drowmonallow("TotalAllowanceAmount") ' / divisorMonthlys

            '        Exit For

            '    Next

            'End If

            Dim valonce_allowance = 0.0
            For Each drowonceallow In once_allowance
                valonce_allowance = drowonceallow("TotalAllowanceAmount")
            Next

            Dim valsemim_allowance = ValNoComma(allSemiMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & employeeID & "'"))
            'For Each drowsemimallow In semim_allowance
            '    valonce_allowance = drowsemimallow("TotalAllowanceAmount")
            'Next

            Dim valweek_allowance = 0.0
            For Each drowweekallow In week_allowance
                valonce_allowance = drowweekallow("TotalAllowanceAmount")
            Next

            Dim totalFixedTaxableMonthlyAllowance = ValNoComma(fixedTaxableMonthlyAllowances.Compute("SUM(AllowanceAmount)", "EmployeeID = '" & employeeID & "'"))

            'If isEndOfMonth = 1 Then
            '    totalFixedTaxableMonthlyAllowance = 0.0
            'End If

            'this is taxable                                ' / divisorMonthlys
            Dim totalemployeeallownce = (valday_allowance _
                                             + (valmonth_allowance) _
                                             + valonce_allowance _
                                             + valsemim_allowance _
                                             + valweek_allowance _
                                             + totalFixedTaxableMonthlyAllowance)


            Dim valdaynotax_allowance = ValNoComma(allNoTaxDailyAllowances.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & employeeID & "'"))
            'GET_employeeallowance(drow("RowID").ToString, _
            '                  "Daily", _
            '                  drow("EmployeeType").ToString, _
            '                  "0")

            Dim valmonthnotax_allowance = ValNoComma(allNoTaxMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & employeeID & "'"))

            'If isEndOfMonth = 1 Then

            '    For Each drowmonallow In monthnotax_allowance

            '        valmonthnotax_allowance = drowmonallow("TotalAllowanceAmount") ' / divisorMonthlys

            '        Exit For

            '    Next

            'End If

            Dim valoncenotax_allowance = ValNoComma(allNoTaxOneTimeAllowances.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & employeeID & "'"))

            'For Each drowonceallow In oncenotax_allowance
            '    valoncenotax_allowance = drowonceallow("TotalAllowanceAmount")
            'Next

            Dim valsemimnotax_allowance = ValNoComma(allNoTaxSemiMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & employeeID & "'"))
            'For Each drowsemimallow In semimnotax_allowance
            '    valoncenotax_allowance = drowsemimallow("TotalAllowanceAmount")
            'Next

            Dim valweeknotax_allowance = 0D
            'For Each drowweekallow In weeknotax_allowance
            '    valoncenotax_allowance = drowweekallow("TotalAllowanceAmount")
            'Next



            If ValNoComma(drow("RowID")) = 36 Then '218 - EmployeeID
                Dim call_lambert = "Over here"
            End If

            Dim totalFixedNonTaxableMonthlyAllowance = ValNoComma(fixedNonTaxableMonthlyAllowances.Compute("SUM(AllowanceAmount)", "EmployeeID = '" & employeeID & "'"))

            Dim totalnotaxemployeeallownce = (
                valdaynotax_allowance +
                valmonthnotax_allowance +
                valoncenotax_allowance +
                valsemimnotax_allowance +
                valweeknotax_allowance +
                totalFixedNonTaxableMonthlyAllowance
            )


            Dim valday_bon = 0.0
            For Each drowdaybon In day_bon
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

                For Each drowmonbon In month_bon
                    'valmonth_bon = drowmonbon("BonusAmount")
                Next

            End If

            Dim valonce_bon = 0.0
            For Each drowoncebon In once_bon
                'valonce_bon = drowoncebon("BonusAmount")
            Next

            Dim valsemim_bon = 0.0
            For Each drowsemimbon In semim_bon
                'valonce_bon = drowsemimbon("BonusAmount")
            Next

            Dim valweek_bon = 0.0
            For Each drowweekbon In week_bon
                'valonce_bon = drowweekbon("BonusAmount")
            Next

            'this is taxable                        ' / divisorMonthlys
            Dim totalTaxableEmployeeBonus = (
                valday_bon +
                valmonth_bon +
                valonce_bon +
                valsemim_bon +
                valweek_bon
            )

            Dim valdaynotax_bon = 0.0
            For Each drowdaybon In daynotax_bon
                valdaynotax_bon = drowdaybon("BonusAmount")

                If drow("EmployeeType").ToString = "Fixed" Then
                    valdaynotax_bon = valdaynotax_bon * workDaysPerYear 'numofweekends
                Else
                    Dim daymultiplier = numofdaypresent.Select("EmployeeID = '" & drow("RowID").ToString & "'")
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

                For Each drowmonbon In monthnotax_bon
                    'valmonthnotax_bon = drowmonbon("BonusAmount")
                Next

            End If

            Dim valoncenotax_bon = 0.0
            For Each drowoncebon In oncenotax_bon
                'valoncenotax_bon = drowoncebon("BonusAmount")
            Next

            Dim valsemimnotax_bon = 0.0
            For Each drowsemimbon In semimnotax_bon
                'valoncenotax_bon = drowsemimbon("BonusAmount")
            Next

            Dim valweeknotax_bon = 0.0
            For Each drowweekbon In weeknotax_bon
                'valoncenotax_bon = drowweekbon("BonusAmount")
            Next



            'this is non-taxable
            Dim totalNoTaxEmployeeBonus = Val(0)

            '(valdaynotax_bon _
            '+ (valmonthnotax_bon / divisorMonthlys) _
            '+ valoncenotax_bon _
            '+ valsemimnotax_bon _
            '+ valweeknotax_bon)

            totalNoTaxEmployeeBonus = valdaynotax_bon
            totalNoTaxEmployeeBonus += valoncenotax_bon
            totalNoTaxEmployeeBonus += valsemimnotax_bon
            totalNoTaxEmployeeBonus += valweeknotax_bon

            totalNoTaxEmployeeBonus += valmonthnotax_bon / divisorMonthlys

            Dim timeEntries = allTimeEntries.Select("EmployeeID = '" & employeeID & "'")

            grossIncome = Val(0)

            pstub_TotalVacationDaysLeft = Val(0)
            pstub_TotalLoans = Val(0)
            emp_taxabsal = Val(0)
            tax_amount = Val(0)
            Dim totalBonus As Decimal

            Dim pstub_TotalAllowance = totalemployeeallownce + totalnotaxemployeeallownce

            Dim the_taxable_salary = 0.0

            For Each timeEntrySummary In timeEntries

                grossIncome = 0D
                grossincome_firsthalf = Val(0)
                _employeeSSS = 0D
                _employerSSS = 0D
                _employeePhilHealth = 0D
                _employerPhilHealth = 0D
                _employeeHdmf = 0D
                _employerHdmf = 0D
                pstub_TotalVacationDaysLeft = Val(0)
                pstub_TotalLoans = Val(0)
                totalBonus = totalTaxableEmployeeBonus + totalNoTaxEmployeeBonus
                emp_taxabsal = Val(0)
                tax_amount = Val(0)

                For Each drowsal In rowempsal

                    Dim skipGovernmentDeductions = CStr(drow("IsFirstTimeSalary")) = "1"

                    emp_taxabsal = 0D

                    Dim overtimePay = ValNoComma(timeEntrySummary("OvertimeHoursAmount"))
                    Dim nightDiffPay = ValNoComma(timeEntrySummary("NightDiffHoursAmount"))
                    Dim nightDiffOTPay = ValNoComma(timeEntrySummary("NightDiffOTHoursAmount"))

                    Dim employment_type = StrConv(drow("EmployeeType").ToString, VbStrConv.ProperCase)

                    Dim sel_dtemployeefirsttimesalary = dtemployeefirsttimesalary.Select("EmployeeID = '" & employeeID & "'")

                    Dim StartingAttendanceCount = ValNoComma(drow("StartingAttendanceCount"))
                    If employment_type = "Fixed" Then
                        Dim basicPay = ValNoComma(drowsal("BasicPay"))
                        grossIncome = basicPay + (overtimePay + nightDiffPay + nightDiffOTPay)

                        grossincome_firsthalf = ValNoComma(drowsal("BasicPay")) +
                                ValNoComma(previousTimeEntries.Compute("SUM(OvertimeHoursAmount)", $"EmployeeID = '{employeeID}'")) +
                                ValNoComma(previousTimeEntries.Compute("SUM(NightDiffOTHoursAmount)", $"EmployeeID = '{employeeID}'")) +
                                ValNoComma(previousTimeEntries.Compute("SUM(NightDiffHoursAmount)", $"EmployeeID = '{employeeID}'"))

                    ElseIf employment_type = "Monthly" Then

                        If skipGovernmentDeductions Then
                            grossIncome = ValNoComma(timeEntrySummary("TotalDayPay"))
                            grossincome_firsthalf = ValNoComma(previousTimeEntries.Compute("SUM(TotalDayPay)", $"EmployeeID = '{employeeID}'"))
                        Else
                            Dim basicPay = ValNoComma(drowsal("BasicPay"))

                            Dim lateDeduction = ValNoComma(timeEntrySummary("HoursLateAmount"))
                            Dim undertimeDeduction = ValNoComma(timeEntrySummary("UndertimeHoursAmount"))
                            Dim absenceDeduction = ValNoComma(timeEntrySummary("Absent"))
                            Dim totalDeduction = lateDeduction + undertimeDeduction + absenceDeduction

                            grossIncome = basicPay - totalDeduction

                            If previousTimeEntries.Select($"EmployeeID = '{employeeID}'").Count > 0 Then
                                grossincome_firsthalf = basicPay
                            End If

                            Dim previousLateDeduction = ValNoComma(previousTimeEntries.Compute("SUM(HoursLateAmount)", "EmployeeID = '" & employeeID & "'"))
                            Dim previousUndertimeDeduction = ValNoComma(previousTimeEntries.Compute("SUM(UndertimeHoursAmount)", "EmployeeID = '" & employeeID & "'"))
                            Dim previousAbsenceDeduction = ValNoComma(previousTimeEntries.Compute("SUM(Absent)", "EmployeeID = '" & employeeID & "'"))
                            Dim totalPreviousDeduction = previousLateDeduction + previousUndertimeDeduction + previousAbsenceDeduction

                            grossincome_firsthalf -= totalPreviousDeduction
                            grossincome_firsthalf += ValNoComma(previousTimeEntries.Compute("MIN(emtAmount)", "EmployeeID = '" & employeeID & "'"))
                        End If

                    ElseIf employment_type = "Daily" Then
                        grossIncome = ValNoComma(timeEntrySummary("TotalDayPay"))
                        grossincome_firsthalf = ValNoComma(previousTimeEntries.Compute("SUM(TotalDayPay)", "EmployeeID = '" & employeeID & "'"))
                    End If

                    CalculateSSS(drowsal)
                    CalculatePhilHealth(drowsal)
                    CalculateHDMF(drowsal)

                    Dim dailyRate = ValNoComma(drow("EmpRatePerDay"))
                    Dim minimumWage = ValNoComma(drow("MinimumWageAmount"))
                    Dim isMinimumWage = dailyRate <= minimumWage
                    Dim governmentContributions = _employeeSSS + _employeePhilHealth + _employeeHdmf

                    If IsWithholdingTaxPaidOnFirstHalf() Or IsWithholdingTaxPaidOnEndOfTheMonth() Then
                        emp_taxabsal = grossIncome - governmentContributions
                        the_taxable_salary = (grossIncome + grossincome_firsthalf) - governmentContributions

                        If isMinimumWage Then
                            tax_amount = 0
                        Else

                            'Dim paywithholdingtax = retAsDatTbl("SELECT ExemptionAmount,TaxableIncomeFromAmount,ExemptionInExcessAmount" &
                            '                                    " FROM paywithholdingtax" &
                            '                                    " WHERE FilingStatusID=(SELECT RowID FROM filingstatus WHERE MaritalStatus='" & drow("MaritalStatus").ToString & "' AND Dependent=" & drow("NoOfDependents").ToString & ")" &
                            '                                    " AND " & emp_taxabsal & " BETWEEN TaxableIncomeFromAmount AND TaxableIncomeToAmount" &
                            '                                    " AND DATEDIFF(CURRENT_DATE(),COALESCE(EffectiveDateTo,COALESCE(EffectiveDateFrom,CURRENT_DATE()))) >= 0" &
                            '                                    " AND PayFrequencyID='" & drow("PayFrequencyID").ToString & "'" &
                            '                                    " ORDER BY DATEDIFF(CURRENT_DATE(),COALESCE(EffectiveDateTo,COALESCE(EffectiveDateFrom,CURRENT_DATE())))" &
                            '                                    " LIMIT 1;")

                            'payWTax,filingStatus

                            Dim sel_filingStatus = filingStatus.Select("MaritalStatus = '" & drow("MaritalStatus").ToString & "' AND Dependent = " & drow("NoOfDependents").ToString)
                            Dim fstat_id = 1

                            For Each fstatrow In sel_filingStatus
                                fstat_id = fstatrow("RowID")
                            Next

                            Dim sel_payWTax = withholdingTaxTable.Select($"
                                FilingStatusID = {fstat_id} AND
                                PayFrequencyID = 2 AND
                                TaxableIncomeFromAmount >= {the_taxable_salary} And {the_taxable_salary} <= TaxableIncomeToAmount
                            ")

                            For Each drowtax In sel_payWTax
                                tax_amount =
                                        ValNoComma(drowtax("ExemptionAmount")) _
                                        + ((the_taxable_salary - ValNoComma(drowtax("TaxableIncomeFromAmount"))) * ValNoComma(drowtax("ExemptionInExcessAmount")))
                                Exit For
                            Next
                        End If
                    Else
                        'PAYFREQUENCY_DIVISOR

                        emp_taxabsal = grossIncome - governmentContributions
                        the_taxable_salary = grossIncome - governmentContributions

                        If isMinimumWage Then
                            tax_amount = 0
                        ElseIf IsWithholdingTaxPaidPerPayPeriod() Then

                            'Dim paywithholdingtax = retAsDatTbl("Select ExemptionAmount, TaxableIncomeFromAmount, ExemptionInExcessAmount" &
                            '                                    " FROM paywithholdingtax" &
                            '                                    " WHERE FilingStatusID=(SELECT RowID FROM filingstatus WHERE MaritalStatus='" & drow("MaritalStatus").ToString & "' AND Dependent=" & drow("NoOfDependents").ToString & ")" &
                            '                                    " AND " & emp_taxabsal & " BETWEEN TaxableIncomeFromAmount AND TaxableIncomeToAmount" &
                            '                                    " AND DATEDIFF(CURRENT_DATE(),COALESCE(EffectiveDateTo,COALESCE(EffectiveDateFrom,CURRENT_DATE()))) >= 0" &
                            '                                    " AND PayFrequencyID='" & drow("PayFrequencyID").ToString & "'" &
                            '                                    " ORDER BY DATEDIFF(CURRENT_DATE(),COALESCE(EffectiveDateTo,COALESCE(EffectiveDateFrom,CURRENT_DATE())))" &
                            '                                    " LIMIT 1;")

                            'payWTax,filingStatus

                            Dim sel_filingStatus = filingStatus.Select("MaritalStatus = '" & drow("MaritalStatus").ToString & "' AND Dependent = " & drow("NoOfDependents").ToString)

                            Dim fstat_id = 1

                            For Each fstatrow In sel_filingStatus
                                fstat_id = fstatrow("RowID")
                            Next

                            Dim sel_payWTax = withholdingTaxTable.Select($"
                                FilingStatusID = '{fstat_id}' And
                                PayFrequencyID = 2 And
                                TaxableIncomeFromAmount <= {the_taxable_salary} And {the_taxable_salary} <= TaxableIncomeToAmount
                            ")

                            Dim GET_employeetaxableincome = New MySQLExecuteQuery("Select `GET_employeetaxableincome`('" & drow("RowID") & "', '" & orgztnID & "', '" & PayrollDateFrom & "','" & grossIncome & "');").Result

                            'For Each drowtax As DataRow In paywithholdingtax.rows
                            For Each drowtax In sel_payWTax

                                'emp_taxabsal = emptaxabsal - (Val(drowtax("ExemptionAmount")) + _
                                '             ((emptaxabsal - Val(drowtax("TaxableIncomeFromAmount"))) * Val(drowtax("ExemptionInExcessAmount"))) _
                                '                             )

                                Dim the_values = Split(GET_employeetaxableincome, ";")

                                tax_amount =
                                        ValNoComma(drowtax("ExemptionAmount")) _
                                        + ((the_taxable_salary - ValNoComma(drowtax("TaxableIncomeFromAmount"))) * ValNoComma(drowtax("ExemptionInExcessAmount")))
                                'ValNoComma(the_values(1))

                                Exit For

                            Next

                        End If

                    End If

                    Exit For

                Next

                Exit For

            Next


            Dim totalNetPay = emp_taxabsal - valemp_loan - tax_amount

            'emptaxabsal

            Dim isPayStubExists As _
                    New MySQLExecuteQuery("SELECT 0 `Result`;")
            'New MySQLExecuteQuery("SELECT EXISTS(SELECT RowID" &
            '                 " FROM paystub" &
            '                 " WHERE EmployeeID='" & drow("RowID") & "'" &
            '                 " AND OrganizationID='" & orgztnID & "'" &
            '                 " AND PayFromDate='" & n_PayrollDateFrom & "'" &
            '                 " AND PayToDate='" & n_PayrollDateTo & "');")

            If isPayStubExists.Result = "2" Then

                If ValNoComma(VeryFirstPayPeriodIDOfThisYear) = ValNoComma(n_PayrollRecordID) Then 'this means, the very first cut off of this year falls here
                    '                                                                           'so system should regain leave balance equals to leave allowance

                    Dim new_ExecuteQuery As _
                            New MySQLExecuteQuery("UPDATE employee e" &
                                             " INNER JOIN payperiod pp ON pp.RowID='" & n_PayrollRecordID & "'" &
                                             " SET" &
                                             " e.LeaveBalance=e.LeaveAllowance" &
                                             ",e.SickLeaveBalance=e.SickLeaveAllowance" &
                                             ",e.MaternityLeaveBalance=e.MaternityLeaveAllowance" &
                                             ",e.OtherLeaveBalance=e.OtherLeaveAllowance" &
                                             ",e.LastUpd=CURRENT_TIMESTAMP()" &
                                             ",e.LastUpdBy='" & z_User & "'" &
                                             " WHERE e.RowID='" & drow("RowID") & "'" &
                                             " AND e.OrganizationID='" & orgztnID & "'" &
                                             " AND ADDDATE(e.StartDate, INTERVAL 2 YEAR) <= pp.`Year`;")
                    '" AND (ADDDATE(e.StartDate, INTERVAL 1 YEAR) <= '" & n_PayrollDateFrom & "'" &
                    '" OR ADDDATE(e.StartDate, INTERVAL 1 YEAR) <= '" & n_PayrollDateTo & "');")

                    'typical regaining of leave balance at the start of the year

                End If

                'gaining of employee leave balance whos year of service reaches one(1)
                'the values that employee will gain here are prorated
                Dim updateLeaveBalanceSqlTemplate = <![CDATA[
                            UPDATE employee e
                            INNER JOIN payperiod pp ON pp.RowID='@PayPeriodID'
                            SET
                                e.LeaveBalance = IF(
                                    e.LeaveAllowance - (e.LeavePerPayPeriod * pp.OrdinalValue) <= 0,
                                    e.LeavePerPayPeriod,
                                    e.LeaveAllowance - (e.LeavePerPayPeriod * pp.OrdinalValue)
                                ),
                                e.SickLeaveBalance = IF(
                                    e.SickLeaveAllowance - (e.SickLeavePerPayPeriod * pp.OrdinalValue) <= 0,
                                    e.SickLeavePerPayPeriod,
                                    e.SickLeaveAllowance - (e.SickLeavePerPayPeriod * pp.OrdinalValue)
                                ),
                                e.MaternityLeaveBalance = e.MaternityLeaveAllowance,
                                e.OtherLeaveBalance = IF(
                                    e.OtherLeaveAllowance - (e.OtherLeavePerPayPeriod * pp.OrdinalValue) <= 0,
                                    e.OtherLeavePerPayPeriod,
                                    e.OtherLeaveAllowance - (e.OtherLeavePerPayPeriod * pp.OrdinalValue)
                                ),
                                e.LastUpd = CURRENT_TIMESTAMP(),
                                e.LastUpdBy = '@UserID'
                             WHERE e.RowID = '@EmployeeID'
                                 AND e.OrganizationID = '@OrganizationID'
                                 AND e.PayFrequencyID = pp.TotalGrossSalary
                                 AND ADDDATE(e.StartDate, INTERVAL 1 YEAR) BETWEEN '@DateFrom' AND '@DateTo';
                        ]]>.Value

                Dim updateLeaveBalanceSql = updateLeaveBalanceSqlTemplate _
                    .Replace("@PayPeriodID", n_PayrollRecordID) _
                    .Replace("@UserID", z_User) _
                    .Replace("@EmployeeID", employeeID) _
                    .Replace("@OrganizationID", orgztnID) _
                    .Replace("@DateFrom", PayrollDateFrom) _
                    .Replace("@DateTo", n_PayrollDateTo)

                Dim n_ExecuteQuery = New MySQLExecuteQuery(updateLeaveBalanceSql)
            End If

            mysql_conn = New MySqlConnection(mysql_conn_text)
            Dim new_cmd = New MySqlCommand("INSUPDPROC_paystub", mysql_conn, myTrans)

            If mysql_conn.State = ConnectionState.Closed Then
                mysql_conn.Open()
            End If

            myTrans = mysql_conn.BeginTransaction()
            With new_cmd

                .Parameters.Clear()
                .CommandTimeout = 5000
                .CommandType = CommandType.StoredProcedure

                .Parameters.AddWithValue("pstub_RowID", DBNull.Value)
                .Parameters.AddWithValue("pstub_OrganizationID", orgztnID)
                .Parameters.AddWithValue("pstub_CreatedBy", z_User)
                .Parameters.AddWithValue("pstub_LastUpdBy", z_User)
                .Parameters.AddWithValue("pstub_PayPeriodID", n_PayrollRecordID)
                .Parameters.AddWithValue("pstub_EmployeeID", employeeID)

                .Parameters.AddWithValue("pstub_TimeEntryID", DBNull.Value)

                .Parameters.AddWithValue("pstub_PayFromDate", PayrollDateFrom) 'If(pstub_PayFromDate = Nothing, DBNull.Value, Format(CDate(pstub_PayFromDate), "yyyy-MM-dd")))
                .Parameters.AddWithValue("pstub_PayToDate", n_PayrollDateTo) 'If(pstub_PayToDate = Nothing, DBNull.Value, Format(CDate(pstub_PayToDate), "yyyy-MM-dd")))

                .Parameters.AddWithValue("pstub_TotalGrossSalary", grossIncome + totalTaxableEmployeeBonus + totalNoTaxEmployeeBonus + totalnotaxemployeeallownce + totalemployeeallownce)
                .Parameters.AddWithValue("pstub_TotalNetSalary", totalNetPay + totalTaxableEmployeeBonus + totalNoTaxEmployeeBonus + totalnotaxemployeeallownce + totalemployeeallownce)
                .Parameters.AddWithValue("pstub_TotalTaxableSalary", the_taxable_salary)
                .Parameters.AddWithValue("pstub_TotalEmpWithholdingTax", tax_amount)

                .Parameters.AddWithValue("pstub_TotalEmpSSS", _employeeSSS) 'DBNull.Value
                .Parameters.AddWithValue("pstub_TotalCompSSS", _employerSSS)
                .Parameters.AddWithValue("pstub_TotalEmpPhilhealth", _employeePhilHealth)
                .Parameters.AddWithValue("pstub_TotalCompPhilhealth", _employerPhilHealth)
                .Parameters.AddWithValue("pstub_TotalEmpHDMF", _employeeHdmf)
                .Parameters.AddWithValue("pstub_TotalCompHDMF", _employerHdmf)
                .Parameters.AddWithValue("pstub_TotalVacationDaysLeft", pstub_TotalVacationDaysLeft)
                .Parameters.AddWithValue("pstub_TotalLoans", valemp_loan) 'pstub_TotalLoans
                .Parameters.AddWithValue("pstub_TotalBonus", totalBonus)
                .Parameters.AddWithValue("pstub_TotalAllowance", pstub_TotalAllowance)

                '.Parameters.Add("paystubID", MySqlDbType.Int32)
                '.Parameters("paystubID").Direction = ParameterDirection.ReturnValue
                'Dim d_reader As MySqlDataReader
                'd_reader = .ExecuteReader()

                .ExecuteNonQuery()
                myTrans.Commit()
            End With
            new_cmd.Dispose()

            If isPayStubExists.Result = "2" Then
                'the decrement of leave balances happens here
                Dim new_ExecuteQuery As _
                        New MySQLExecuteQuery("UPDATE employee e" &
                                         " LEFT JOIN (SELECT RowID" &
                                                     ",SUM(VacationLeaveHours) AS VacationLeaveHours" &
                                                     ",SUM(SickLeaveHours) AS SickLeaveHours" &
                                                     ",SUM(MaternityLeaveHours) AS MaternityLeaveHours" &
                                                     ",SUM(OtherLeaveHours) AS OtherLeaveHours" &
                                                     " FROM employeetimeentry" &
                                                     " WHERE EmployeeID='" & drow("RowID") & "'" &
                                                     " AND OrganizationID='" & orgztnID & "'" &
                                                     " AND `Date` BETWEEN '" & PayrollDateFrom & "' AND '" & n_PayrollDateTo & "') ete ON ete.RowID IS NOT NULL" &
                                        " SET" &
                                        " e.LeaveBalance = e.LeaveBalance - IFNULL(ete.VacationLeaveHours, 0)" &
                                        ",e.SickLeaveBalance = e.SickLeaveBalance - IFNULL(ete.SickLeaveHours,0)" &
                                        ",e.MaternityLeaveBalance = e.MaternityLeaveBalance - IFNULL(ete.MaternityLeaveHours,0)" &
                                        ",e.OtherLeaveBalance = e.OtherLeaveBalance - IFNULL(ete.OtherLeaveHours,0)" &
                                        ",LastUpd=CURRENT_TIMESTAMP()" &
                                        ",LastUpdBy='" & z_User & "'" &
                                        " WHERE e.RowID='" & drow("RowID") & "' AND e.OrganizationID='" & orgztnID & "';")

            End If

            'If IsFirstPayperiodOfTheYear() Then
            '    ConvertLeaveToCash(drow)
            'End If

            form_caller.Invoke(m_NotifyMainWindow, 1)
            Dim my_cmd As String = String.Concat(Convert.ToString(drow("RowID")), "@", Convert.ToString(drow("EmployeeID")))
            Console.WriteLine(my_cmd)

        Catch ex As Exception

            myTrans.Rollback()

            Dim this_err As String = String.Concat(getErrExcptn(ex, "PayrollGeneration"), " -- ", Convert.ToString(employee_dattab.TableName),
                                                       ".Employee ID[", Convert.ToString(drow("EmployeeID")), "]")
            Console.WriteLine(this_err)

        Finally
            'Continue For
            If mysql_conn IsNot Nothing Then
                mysql_conn.Close()
                mysql_conn.Dispose()
            End If
            'Thread.Sleep(500)
        End Try
    End Sub

    Sub PayrollGeneration_BackgourndWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        DoProcess()
        'For i = 0 To 19
        '    Thread.Sleep(3000)
        'Next

        'Dim dfsd As String = DirectCast(sender, System.ComponentModel.BackgroundWorker).ToString
        'Console.WriteLine(String.Concat(dfsd, " @@ ", employee_dattab.TableName))

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
            "AND ea.EffectiveEndDate >= '" & n_PayrollDateTo & "' " &
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
            " AND (ea.EffectiveStartDate <= '" & n_PayrollDateTo & "' OR ea.EffectiveEndDate <= '" & n_PayrollDateTo & "')" &
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
                " AND (ea.EffectiveStartDate <= '" & n_PayrollDateTo & "' OR ea.EffectiveEndDate <= '" & n_PayrollDateTo & "')" &
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
            "AND ea.EffectiveEndDate >= '" & n_PayrollDateTo & "' " &
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
            " AND (ea.EffectiveStartDate <= '" & n_PayrollDateTo & "' OR ea.EffectiveEndDate <= '" & n_PayrollDateTo & "')" &
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
            " AND (ea.EffectiveStartDate <= '" & n_PayrollDateTo & "' OR ea.EffectiveEndDate <= '" & n_PayrollDateTo & "')" &
            " AND p.`Fixed` = 1 " &
            "AND p.`Status` = 0;"
        End If

        fixedNonTaxableMonthlyAllowances = New MySQLQueryToDataTable(fixedNonTaxableMonthlyAllowanceSql).ResultTable
    End Sub

    Private Sub CalculateSSS(salary As DataRow)
        Dim employeeSSSPerMonth = CDec(salary("EmployeeContributionAmount"))
        Dim employerSSSPerMonth = CDec(salary("EmployerContributionAmount"))
        Dim payPeriodsPerMonth = CDec(ValNoComma(_employee("PAYFREQUENCY_DIVISOR")))

        If IsSssPaidOnFirstHalf() Or IsSssPaidOnEndOfTheMonth() Then
            _employeeSSS = employeeSSSPerMonth
            _employerSSS = employerSSSPerMonth
        ElseIf IsSssPaidPerPayPeriod() Then
            _employeeSSS = employeeSSSPerMonth / payPeriodsPerMonth
            _employerSSS = employerSSSPerMonth / payPeriodsPerMonth
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
            _employeePhilHealth = employeePhilHealthPerMonth
            _employerPhilHealth = employerPhilHealthPerMonth
        ElseIf IsPhilHealthPaidPerPayPeriod() Then
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

    Private Sub CalculateHDMF(salary As DataRow)
        Dim employeeHdmfPerMonth = CDec(salary("HDMFAmount"))
        Dim employerHdmfPerMonth = 100D
        Dim payPeriodsPerMonth = CDec(ValNoComma(_employee("PAYFREQUENCY_DIVISOR")))

        If IsHdmfPaidOnFirstHalf() Or IsHdmfPaidOnEndOfTheMonth() Then
            _employeeHdmf = employeeHdmfPerMonth
            _employerHdmf = employerHdmfPerMonth
        ElseIf IsHdmfPaidPerPayPeriod() Then
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
            .Replace("@PayPeriodID", n_PayrollRecordID)

        Me.existingUnusedLeaveAdjustments = GetDatatable(sql)
    End Sub

    Private Sub LoadCurrentPayperiod()
        Dim sql = <![CDATA[
            SELECT Year, OrdinalValue
            FROM payperiod
            WHERE RowID = @PayPeriodID;
        ]]>.Value

        sql = sql.Replace("@PayPeriodID", Me.PayrollRecordID)

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
            .Replace("@PayPeriodID", n_PayrollRecordID)

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
            Me.PayrollRecordID,
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
