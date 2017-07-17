Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Threading.Tasks

Public Class PayrollGeneration

    Private employee_dattab As DataTable
    Private isEndOfMonth As String
    Private isorgPHHdeductsched As SByte
    Private isorgSSSdeductsched As SByte
    Private isorgHDMFdeductsched As SByte
    Private isorgWTaxdeductsched As SByte
    Private esal_dattab As DataTable
    Private emp_loans As DataTable
    Private emp_bonus As DataTable
    Private emp_allowanceDaily As DataTable
    Private emp_allowanceMonthly As DataTable
    Private emp_allowanceOnce As DataTable
    Private emp_allowanceSemiM As DataTable
    Private emp_allowanceWeekly As DataTable
    Private notax_allowanceDaily As DataTable
    Private notax_allowanceMonthly As DataTable
    Private notax_allowanceOnce As DataTable
    Private notax_allowanceSemiM As DataTable
    Private notax_allowanceWeekly As DataTable
    Private emp_bonusDaily As DataTable
    Private emp_bonusMonthly As DataTable
    Private emp_bonusOnce As DataTable
    Private emp_bonusSemiM As DataTable
    Private emp_bonusWeekly As DataTable
    Private notax_bonusDaily As DataTable
    Private notax_bonusMonthly As DataTable
    Private notax_bonusOnce As DataTable
    Private notax_bonusSemiM As DataTable
    Private notax_bonusWeekly As DataTable
    Private numofdaypresent As DataTable
    Private etent_totdaypay As DataTable
    Private dtemployeefirsttimesalary As DataTable
    Private prev_empTimeEntry As DataTable
    Private VeryFirstPayPeriodIDOfThisYear As Object
    Private withthirteenthmonthpay As SByte

    Private payWTax As DataTable

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

    Private _employeeHDMF As Decimal
    Private _employerHDMF As Decimal

    'ByVal _isorgPHHdeductsched As SByte,
    'ByVal _isorgSSSdeductsched As SByte,
    'ByVal _isorgHDMFdeductsched As SByte,
    'ByVal _isorgWTaxdeductsched As SByte,
    Sub New(ByVal _employee_dattab As DataTable,
                  ByVal _isEndOfMonth As String,
                  ByVal _esal_dattab As DataTable,
                  ByVal _emp_loans As DataTable,
                  ByVal _emp_bonus As DataTable,
                  ByVal _emp_allowanceDaily As DataTable,
                  ByVal _emp_allowanceMonthly As DataTable,
                  ByVal _emp_allowanceOnce As DataTable,
                  ByVal _emp_allowanceSemiM As DataTable,
                  ByVal _emp_allowanceWeekly As DataTable,
                  ByVal _notax_allowanceDaily As DataTable,
                  ByVal _notax_allowanceMonthly As DataTable,
                  ByVal _notax_allowanceOnce As DataTable,
                  ByVal _notax_allowanceSemiM As DataTable,
                  ByVal _notax_allowanceWeekly As DataTable,
                  ByVal _emp_bonusDaily As DataTable,
                  ByVal _emp_bonusMonthly As DataTable,
                  ByVal _emp_bonusOnce As DataTable,
                  ByVal _emp_bonusSemiM As DataTable,
                  ByVal _emp_bonusWeekly As DataTable,
                  ByVal _notax_bonusDaily As DataTable,
                  ByVal _notax_bonusMonthly As DataTable,
                  ByVal _notax_bonusOnce As DataTable,
                  ByVal _notax_bonusSemiM As DataTable,
                  ByVal _notax_bonusWeekly As DataTable,
                  ByVal _numofdaypresent As DataTable,
                  ByVal _etent_totdaypay As DataTable,
                  ByVal _dtemployeefirsttimesalary As DataTable,
                  ByVal _prev_empTimeEntry As DataTable,
                  ByVal _VeryFirstPayPeriodIDOfThisYear As Object,
                  ByVal _withthirteenthmonthpay As SByte,
                  Optional pay_stub_frm As PayStub = Nothing)

        form_caller = pay_stub_frm

        employee_dattab = _employee_dattab
        isEndOfMonth = _isEndOfMonth
        'isorgPHHdeductsched = _isorgPHHdeductsched
        'isorgSSSdeductsched = _isorgSSSdeductsched
        'isorgHDMFdeductsched = _isorgHDMFdeductsched
        'isorgWTaxdeductsched = _isorgWTaxdeductsched
        esal_dattab = _esal_dattab
        emp_loans = _emp_loans
        emp_bonus = _emp_bonus
        emp_allowanceDaily = _emp_allowanceDaily
        emp_allowanceMonthly = _emp_allowanceMonthly
        emp_allowanceOnce = _emp_allowanceOnce
        emp_allowanceSemiM = _emp_allowanceSemiM
        emp_allowanceWeekly = _emp_allowanceWeekly
        notax_allowanceDaily = _notax_allowanceDaily
        notax_allowanceMonthly = _notax_allowanceMonthly
        notax_allowanceOnce = _notax_allowanceOnce
        notax_allowanceSemiM = _notax_allowanceSemiM
        notax_allowanceWeekly = _notax_allowanceWeekly
        emp_bonusDaily = _emp_bonusDaily
        emp_bonusMonthly = _emp_bonusMonthly
        emp_bonusOnce = _emp_bonusOnce
        emp_bonusSemiM = _emp_bonusSemiM
        emp_bonusWeekly = _emp_bonusWeekly
        notax_bonusDaily = _notax_bonusDaily
        notax_bonusMonthly = _notax_bonusMonthly
        notax_bonusOnce = _notax_bonusOnce
        notax_bonusSemiM = _notax_bonusSemiM
        notax_bonusWeekly = _notax_bonusWeekly
        numofdaypresent = _numofdaypresent
        etent_totdaypay = _etent_totdaypay
        dtemployeefirsttimesalary = _dtemployeefirsttimesalary
        prev_empTimeEntry = _prev_empTimeEntry
        VeryFirstPayPeriodIDOfThisYear = _VeryFirstPayPeriodIDOfThisYear
        withthirteenthmonthpay = _withthirteenthmonthpay

        payWTax = New MySQLQueryToDataTable("SELECT * FROM paywithholdingtax;" &
                                          "").ResultTable

        filingStatus = New MySQLQueryToDataTable("SELECT * FROM filingstatus;" &
                                          "").ResultTable

        m_NotifyMainWindow = AddressOf pay_stub_frm.ProgressCounter

    End Sub

    Private n_PayrollDateFrom As Object = Nothing

    Property PayrollDateFrom As Object

        Get
            Return n_PayrollDateFrom
        End Get

        Set(value As Object)
            n_PayrollDateFrom = value

        End Set

    End Property

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

    'paypRowID

    Private Const FirstHalf As String = "First half"
    Private Const EndOfTheMonth As String = "End of the month"
    Private Const PerPayPeriod As String = "Per pay period"

    Private _philHealthDeductionSchedule As String
    Private _sssDeductionSchedule As String
    Private _hdmfDeductionSchedule As String

    Private n_PayStub As New PayStub

    Private mysql_conn As MySqlConnection

    Dim myTrans As MySqlTransaction

    Sub DoProcess()
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

                    pause_process_message = "Employee '" & drow("EmployeeID") & "' has no position." &
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

            ' Construct sql to load all fixed monthly allowances for all employees
            Dim fixedTaxableMonthlyAllowanceSql =
                "SELECT ea.* " &
                "FROM employeeallowance ea " &
                "INNER JOIN product p " &
                "ON p.RowID = ea.ProductID " &
                "WHERE ea.OrganizationID = '" & orgztnID & "' " &
                "AND ea.EffectiveStartDate <= '" & n_PayrollDateFrom & "' " &
                "AND ea.EffectiveEndDate >= '" & n_PayrollDateTo & "' " &
                "AND ea.AllowanceFrequency = 'Monthly' " &
                "AND p.`Fixed` = 1 " &
                "AND p.`Status` = 1;"

            If isEndOfMonth = 0 Then 'means end of the month
                fixedTaxableMonthlyAllowanceSql =
                "SELECT ea.* " &
                "FROM employeeallowance ea " &
                "INNER JOIN product p " &
                "ON p.RowID = ea.ProductID " &
                "WHERE ea.OrganizationID = '" & orgztnID & "' " &
                "AND ea.AllowanceFrequency IN ('Monthly','Semi-monthly') " &
                " AND (ea.EffectiveStartDate >= '" & n_PayrollDateFrom & "' OR ea.EffectiveEndDate >= '" & n_PayrollDateFrom & "')" &
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
                    " AND (ea.EffectiveStartDate >= '" & n_PayrollDateFrom & "' OR ea.EffectiveEndDate >= '" & n_PayrollDateFrom & "')" &
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
                "AND ea.EffectiveStartDate <= '" & n_PayrollDateFrom & "' " &
                "AND ea.EffectiveEndDate >= '" & n_PayrollDateTo & "' " &
                "AND ea.AllowanceFrequency = 'Monthly' " &
                "AND p.`Fixed` = 1 " &
                "AND p.`Status` = 0;"

            If isEndOfMonth = 0 Then 'means end of the month
                fixedNonTaxableMonthlyAllowanceSql =
                "SELECT ea.* " &
                "FROM employeeallowance ea " &
                "INNER JOIN product p " &
                "ON p.RowID = ea.ProductID " &
                "WHERE ea.OrganizationID = '" & orgztnID & "' " &
                "AND ea.AllowanceFrequency IN ('Monthly','Semi-monthly') " &
                " AND (ea.EffectiveStartDate >= '" & n_PayrollDateFrom & "' OR ea.EffectiveEndDate >= '" & n_PayrollDateFrom & "')" &
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
                " AND (ea.EffectiveStartDate >= '" & n_PayrollDateFrom & "' OR ea.EffectiveEndDate >= '" & n_PayrollDateFrom & "')" &
                " AND (ea.EffectiveStartDate <= '" & n_PayrollDateTo & "' OR ea.EffectiveEndDate <= '" & n_PayrollDateTo & "')" &
                " AND p.`Fixed` = 1 " &
                "AND p.`Status` = 0;"
            End If

            fixedNonTaxableMonthlyAllowances = New MySQLQueryToDataTable(fixedNonTaxableMonthlyAllowanceSql).ResultTable

            Dim pstub_TotalEmpSSS As Decimal
            Dim pstub_TotalCompSSS As Decimal
            Dim pstub_TotalEmpPhilhealth As Decimal
            Dim pstub_TotalCompPhilhealth As Decimal
            Dim pstub_TotalEmpHDMF As Decimal
            Dim pstub_TotalCompHDMF As Decimal
            Dim pstub_TotalVacationDaysLeft As Decimal
            Dim pstub_TotalLoans As Decimal
            Dim pstub_TotalBonus As Decimal
            Dim OTAmount As Decimal
            Dim NightDiffOTAmount As Decimal
            Dim NightDiffAmount As Decimal
            Dim thirteenthmoval As Decimal

            Dim emp_count As Integer = employee_dattab.Rows.Count

            Dim progress_index As Integer = 0

            Dim emptaxabsal As Decimal = 0
            Dim empnetsal As Decimal = 0
            Dim emp_taxabsal = Val(0)

            Dim tax_amount = Val(0)

            Dim grossincome = Val(0)

            Dim grossincome_firsthalf = Val(0)

            Dim date_to_use = If(CDate(n_PayrollDateFrom) > CDate(n_PayrollDateTo), CDate(n_PayrollDateFrom), CDate(n_PayrollDateTo))

            Dim dateStr_to_use = Format(CDate(date_to_use), "yyyy-MM-dd")

            Dim numberofweeksthismonth =
                    New MySQLExecuteQuery("SELECT `COUNTTHEWEEKS`('" & dateStr_to_use & "');")

            For Each drow As DataRow In employee_dattab.Rows
                Try
                    _philHealthDeductionSchedule = drow("PhHealthDeductSched").ToString

                    If _philHealthDeductionSchedule = EndOfTheMonth Then
                        isorgPHHdeductsched = 0
                    ElseIf _philHealthDeductionSchedule = "First half" Then
                        isorgPHHdeductsched = 1
                    ElseIf _philHealthDeductionSchedule = "Per pay period" Then
                        isorgPHHdeductsched = 2
                    End If

                    _sssDeductionSchedule = drow("SSSDeductSched").ToString
                    If _sssDeductionSchedule = "End of the month" Then
                        isorgSSSdeductsched = 0
                    ElseIf _sssDeductionSchedule = "First half" Then
                        isorgSSSdeductsched = 1
                    ElseIf _sssDeductionSchedule = "Per pay period" Then
                        isorgSSSdeductsched = 2
                    End If

                    _hdmfDeductionSchedule = drow("HDMFDeductSched").ToString
                    If _hdmfDeductionSchedule = "End of the month" Then
                        isorgHDMFdeductsched = 0
                    ElseIf _hdmfDeductionSchedule = "First half" Then
                        isorgHDMFdeductsched = 1
                    ElseIf _hdmfDeductionSchedule = "Per pay period" Then
                        isorgHDMFdeductsched = 2
                    End If

                    If drow("WTaxDeductSched").ToString = "End of the month" Then
                        isorgWTaxdeductsched = 0
                    ElseIf drow("WTaxDeductSched").ToString = "First half" Then
                        isorgWTaxdeductsched = 1
                    ElseIf drow("WTaxDeductSched").ToString = "Per pay period" Then
                        isorgWTaxdeductsched = 2
                    End If


                    Dim employeeID = Trim(drow("RowID"))

                    Dim org_WorkDaysPerYear = drow("WorkDaysPerYear")

                    Dim divisorMonthlys = If(
                        drow("PayFrequencyID") = 1,
                        2,
                        If(
                            drow("PayFrequencyID") = 2,
                            1,
                            If(
                                drow("PayFrequencyID") = 3,
                                New MySQLExecuteQuery("SELECT COUNT(RowID) FROM employeetimeentry WHERE EmployeeID='" & employeeID & "' AND Date BETWEEN '" & n_PayrollDateFrom & "' AND '" & n_PayrollDateTo & "' AND IFNULL(TotalDayPay,0)!=0 AND OrganizationID='" & orgztnID & "';"),
                                numberofweeksthismonth)
                            )
                        )


                    Dim rowempsal = esal_dattab.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim emp_loan = emp_loans.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim emp_bon = emp_bonus.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim day_allowance = emp_allowanceDaily.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim month_allowance = emp_allowanceMonthly.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim once_allowance = emp_allowanceOnce.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim semim_allowance = emp_allowanceSemiM.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim week_allowance = emp_allowanceWeekly.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    'emp_allowanceSemiM

                    'emp_allowanceWeekly

                    Dim daynotax_allowance = notax_allowanceDaily.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim monthnotax_allowance = notax_allowanceMonthly.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim oncenotax_allowance = notax_allowanceOnce.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim semimnotax_allowance = notax_allowanceSemiM.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim weeknotax_allowance = notax_allowanceWeekly.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    'notax_allowanceSemiM

                    'notax_allowanceWeekly




                    Dim day_bon = emp_bonusDaily.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim month_bon = emp_bonusMonthly.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim once_bon = emp_bonusOnce.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim semim_bon = emp_bonusSemiM.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim week_bon = emp_bonusWeekly.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    'emp_bonusSemiM

                    'emp_bonusWeekly



                    Dim daynotax_bon = notax_bonusDaily.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim monthnotax_bon = notax_bonusMonthly.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim oncenotax_bon = notax_bonusOnce.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim semimnotax_bon = notax_bonusSemiM.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    Dim weeknotax_bon = notax_bonusWeekly.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    'notax_bonusSemiM

                    'notax_bonusWeekly




                    Dim valemp_loan = Val(0)
                    For Each drowloan In emp_loan
                        valemp_loan = drowloan("DeductionAmount")
                    Next

                    'Dim valemp_bon = Val(0)
                    'For Each drowbon In emp_bon
                    '    valemp_bon = drowbon("BonusAmount")
                    'Next

                    Dim valday_allowance = ValNoComma(emp_allowanceDaily.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & drow("RowID") & "'"))
                    'GET_employeeallowance(drow("RowID").ToString, _
                    '                  "Daily", _
                    '                  drow("EmployeeType").ToString, _
                    '                  "1")

                    Dim valmonth_allowance = ValNoComma(emp_allowanceMonthly.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & drow("RowID") & "'"))

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

                    Dim valsemim_allowance = ValNoComma(emp_allowanceSemiM.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & drow("RowID") & "'"))
                    'For Each drowsemimallow In semim_allowance
                    '    valonce_allowance = drowsemimallow("TotalAllowanceAmount")
                    'Next

                    Dim valweek_allowance = 0.0
                    For Each drowweekallow In week_allowance
                        valonce_allowance = drowweekallow("TotalAllowanceAmount")
                    Next

                    Dim totalFixedTaxableMonthlyAllowance = ValNoComma(fixedTaxableMonthlyAllowances.Compute("SUM(AllowanceAmount)", "EmployeeID = '" & drow("RowID") & "'"))

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


                    Dim valdaynotax_allowance = ValNoComma(notax_allowanceDaily.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & drow("RowID") & "'"))
                    'GET_employeeallowance(drow("RowID").ToString, _
                    '                  "Daily", _
                    '                  drow("EmployeeType").ToString, _
                    '                  "0")

                    Dim valmonthnotax_allowance = ValNoComma(notax_allowanceMonthly.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & drow("RowID") & "'"))

                    'If isEndOfMonth = 1 Then

                    '    For Each drowmonallow In monthnotax_allowance

                    '        valmonthnotax_allowance = drowmonallow("TotalAllowanceAmount") ' / divisorMonthlys

                    '        Exit For

                    '    Next

                    'End If

                    Dim valoncenotax_allowance = ValNoComma(notax_allowanceOnce.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & drow("RowID") & "'"))

                    'For Each drowonceallow In oncenotax_allowance
                    '    valoncenotax_allowance = drowonceallow("TotalAllowanceAmount")
                    'Next

                    Dim valsemimnotax_allowance = ValNoComma(notax_allowanceSemiM.Compute("SUM(TotalAllowanceAmount)", "EmployeeID = '" & drow("RowID") & "'"))
                    'For Each drowsemimallow In semimnotax_allowance
                    '    valoncenotax_allowance = drowsemimallow("TotalAllowanceAmount")
                    'Next

                    Dim valweeknotax_allowance = 0.0
                    'For Each drowweekallow In weeknotax_allowance
                    '    valoncenotax_allowance = drowweekallow("TotalAllowanceAmount")
                    'Next



                    If ValNoComma(drow("RowID")) = 36 Then '218 - EmployeeID
                        Dim call_lambert = "Over here"
                    End If

                    Dim totalFixedNonTaxableMonthlyAllowance = ValNoComma(fixedNonTaxableMonthlyAllowances.Compute("SUM(AllowanceAmount)", "EmployeeID = '" & drow("RowID") & "'"))

                    'If isEndOfMonth = 1 Then
                    '    totalFixedNonTaxableMonthlyAllowance = 0.0
                    'End If

                    'this is non-taxable                                        ' / divisorMonthlys
                    '+ valsemimonnotax_allowance _
                    Dim totalnotaxemployeeallownce = (valdaynotax_allowance _
                                                          + (valmonthnotax_allowance) _
                                                          + valoncenotax_allowance _
                                                          + valsemimnotax_allowance _
                                                          + valweeknotax_allowance _
                                                          + totalFixedNonTaxableMonthlyAllowance)


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

                    If isEndOfMonth = 1 Then

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
                    Dim totalemployeebonus = (valday_bon _
                                                  + (valmonth_bon) _
                                                  + valonce_bon _
                                                  + valsemim_bon _
                                                  + valweek_bon)




                    Dim valdaynotax_bon = 0.0
                    For Each drowdaybon In daynotax_bon
                        valdaynotax_bon = drowdaybon("BonusAmount")

                        If drow("EmployeeType").ToString = "Fixed" Then
                            valdaynotax_bon = valdaynotax_bon * org_WorkDaysPerYear 'numofweekends
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

                    If isEndOfMonth = 1 Then

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
                    Dim totalnotaxemployeebonus = Val(0)

                    '(valdaynotax_bon _
                    '+ (valmonthnotax_bon / divisorMonthlys) _
                    '+ valoncenotax_bon _
                    '+ valsemimnotax_bon _
                    '+ valweeknotax_bon)

                    totalnotaxemployeebonus = valdaynotax_bon
                    totalnotaxemployeebonus += valoncenotax_bon
                    totalnotaxemployeebonus += valsemimnotax_bon
                    totalnotaxemployeebonus += valweeknotax_bon

                    totalnotaxemployeebonus += valmonthnotax_bon / divisorMonthlys







                    Dim emptotdaypay = etent_totdaypay.Select("EmployeeID = '" & drow("RowID").ToString & "'")

                    grossincome = Val(0)

                    pstub_TotalEmpSSS = Val(0)
                    pstub_TotalCompSSS = Val(0)
                    pstub_TotalEmpPhilhealth = Val(0)
                    pstub_TotalCompPhilhealth = Val(0)
                    pstub_TotalEmpHDMF = Val(0)
                    pstub_TotalCompHDMF = Val(0)
                    pstub_TotalVacationDaysLeft = Val(0)
                    pstub_TotalLoans = Val(0)
                    pstub_TotalBonus = Val(0)
                    emp_taxabsal = Val(0)
                    emptaxabsal = Val(0)
                    empnetsal = Val(0)
                    tax_amount = Val(0)

                    Dim pstub_TotalAllowance = totalemployeeallownce + totalnotaxemployeeallownce

                    Dim the_taxable_salary = ValNoComma(0)

                    employeeID = Trim(drow("RowID"))

                    For Each drowtotdaypay In emptotdaypay

                        grossincome = Val(0)
                        grossincome_firsthalf = Val(0)
                        pstub_TotalEmpSSS = Val(0)
                        pstub_TotalCompSSS = Val(0)
                        pstub_TotalEmpPhilhealth = Val(0)
                        pstub_TotalCompPhilhealth = Val(0)
                        pstub_TotalEmpHDMF = Val(0)
                        pstub_TotalCompHDMF = Val(0)
                        pstub_TotalVacationDaysLeft = Val(0)
                        pstub_TotalLoans = Val(0)
                        pstub_TotalBonus = totalemployeebonus + totalnotaxemployeebonus
                        emp_taxabsal = Val(0)
                        emptaxabsal = Val(0)
                        empnetsal = Val(0)
                        tax_amount = Val(0)
                        OTAmount = 0
                        NightDiffOTAmount = 0
                        NightDiffAmount = 0

                        For Each drowsal In rowempsal

                            Dim skipgovtdeduct = drow("IsFirstTimeSalary")

                            emptaxabsal = 0
                            empnetsal = 0
                            emp_taxabsal = 0

                            OTAmount = ValNoComma(drowtotdaypay("OvertimeHoursAmount"))

                            NightDiffOTAmount = ValNoComma(drowtotdaypay("NightDiffOTHoursAmount"))

                            NightDiffAmount = ValNoComma(drowtotdaypay("NightDiffHoursAmount"))

                            employeeID = Trim(drow("RowID"))

                            Dim employment_type = StrConv(drow("EmployeeType").ToString, VbStrConv.ProperCase)

                            Dim sel_dtemployeefirsttimesalary = dtemployeefirsttimesalary.Select("EmployeeID = '" & drow("RowID") & "'")

                            Dim StartingAttendanceCount = ValNoComma(drow("StartingAttendanceCount"))
                            If employeeID = 9 Then : Dim call_lambert = "Over here" : End If
                            If employment_type = "Fixed" Then
                                grossincome = ValNoComma(drowsal("BasicPay"))
                                grossincome = grossincome + (OTAmount + NightDiffAmount + NightDiffOTAmount)

                                grossincome_firsthalf = ValNoComma(drowsal("BasicPay")) +
                                        ValNoComma(prev_empTimeEntry.Compute("SUM(OvertimeHoursAmount)", "EmployeeID = '" & drow("RowID").ToString & "'")) +
                                        ValNoComma(prev_empTimeEntry.Compute("SUM(NightDiffOTHoursAmount)", "EmployeeID = '" & drow("RowID").ToString & "'")) +
                                        ValNoComma(prev_empTimeEntry.Compute("SUM(NightDiffHoursAmount)", "EmployeeID = '" & drow("RowID").ToString & "'"))

                            ElseIf employment_type = "Monthly" Then

                                If skipgovtdeduct = "1" Then
                                    grossincome = ValNoComma(drowtotdaypay("TotalDayPay"))

                                    grossincome_firsthalf = ValNoComma(prev_empTimeEntry.Compute("SUM(TotalDayPay)", "EmployeeID = '" & drow("RowID").ToString & "'"))

                                Else

                                    grossincome = ValNoComma(drowsal("BasicPay"))
                                    'grossincome = grossincome + (OTAmount + NightDiffAmount + NightDiffOTAmount)

                                    grossincome -= (ValNoComma(drowtotdaypay("HoursLateAmount")) _
                                                        + ValNoComma(drowtotdaypay("UndertimeHoursAmount")) _
                                                        + ValNoComma(drowtotdaypay("Absent")))
                                    'grossincome += ValNoComma(drowtotdaypay("emtAmount"))

                                    If prev_empTimeEntry.Select("EmployeeID = '" & drow("RowID").ToString & "'").Count > 0 Then
                                        grossincome_firsthalf = ValNoComma(drowsal("BasicPay")) '+ _
                                    End If
                                    'ValNoComma(prev_empTimeEntry.Compute("SUM(OvertimeHoursAmount)", "EmployeeID = " & drow("RowID").ToString)) + _
                                    'ValNoComma(prev_empTimeEntry.Compute("SUM(NightDiffOTHoursAmount)", "EmployeeID = " & drow("RowID").ToString)) + _
                                    'ValNoComma(prev_empTimeEntry.Compute("SUM(NightDiffHoursAmount)", "EmployeeID = " & drow("RowID").ToString))

                                    grossincome_firsthalf -=
                                            (ValNoComma(prev_empTimeEntry.Compute("SUM(HoursLateAmount)", "EmployeeID = '" & drow("RowID").ToString & "'")) _
                                            + ValNoComma(prev_empTimeEntry.Compute("SUM(UndertimeHoursAmount)", "EmployeeID = '" & drow("RowID").ToString & "'")) _
                                            + ValNoComma(prev_empTimeEntry.Compute("SUM(Absent)", "EmployeeID = '" & drow("RowID").ToString & "'")))
                                    grossincome_firsthalf += ValNoComma(prev_empTimeEntry.Compute("MIN(emtAmount)", "EmployeeID = '" & drow("RowID").ToString & "'"))
                                End If

                            ElseIf employment_type = "Daily" Then
                                grossincome = ValNoComma(drowtotdaypay("TotalDayPay"))

                                grossincome_firsthalf = ValNoComma(prev_empTimeEntry.Compute("SUM(TotalDayPay)", "EmployeeID = '" & drow("RowID").ToString & "'"))

                            End If


                            If isEndOfMonth = isorgSSSdeductsched Then

                                pstub_TotalEmpSSS = CDec(drowsal("EmployeeContributionAmount"))
                                pstub_TotalCompSSS = CDec(drowsal("EmployerContributionAmount"))

                            Else
                                If isorgSSSdeductsched = 2 Then 'Per pay period
                                    pstub_TotalEmpSSS = CDec(drowsal("EmployeeContributionAmount"))
                                    pstub_TotalCompSSS = CDec(drowsal("EmployerContributionAmount"))

                                    pstub_TotalEmpSSS = pstub_TotalEmpSSS / ValNoComma(drow("PAYFREQUENCY_DIVISOR"))
                                    pstub_TotalCompSSS = pstub_TotalCompSSS / ValNoComma(drow("PAYFREQUENCY_DIVISOR"))

                                End If

                            End If

                            CalculateSSS(drowsal)

                            If isEndOfMonth = isorgPHHdeductsched Then
                                pstub_TotalEmpPhilhealth = CDec(drowsal("EmployeeShare"))
                                pstub_TotalCompPhilhealth = CDec(drowsal("EmployerShare"))

                            Else
                                If isorgPHHdeductsched = 2 Then 'Per pay period
                                    pstub_TotalEmpPhilhealth = CDec(drowsal("EmployeeShare"))
                                    pstub_TotalCompPhilhealth = CDec(drowsal("EmployerShare"))

                                    pstub_TotalEmpPhilhealth = pstub_TotalEmpPhilhealth / ValNoComma(drow("PAYFREQUENCY_DIVISOR"))
                                    pstub_TotalCompPhilhealth = pstub_TotalCompPhilhealth / ValNoComma(drow("PAYFREQUENCY_DIVISOR"))

                                End If

                            End If

                            CalculatePhilHealth(drowsal)

                            If isEndOfMonth = isorgHDMFdeductsched Then
                                pstub_TotalEmpHDMF = CDec(drowsal("HDMFAmount"))
                                pstub_TotalCompHDMF = 100 'CDec(drowsal("HDMFAmount"))

                            Else
                                If isorgHDMFdeductsched = 2 Then 'Per pay period
                                    pstub_TotalEmpHDMF = CDec(drowsal("HDMFAmount"))
                                    pstub_TotalCompHDMF = 100 'CDec(drowsal("HDMFAmount"))

                                    pstub_TotalEmpHDMF = pstub_TotalEmpHDMF / ValNoComma(drow("PAYFREQUENCY_DIVISOR"))
                                    pstub_TotalCompHDMF = 100 / ValNoComma(drow("PAYFREQUENCY_DIVISOR"))

                                End If

                            End If

                            CalculateHDMF(drowsal)

                            '**************************************************************************************
                            'Below is the condition that if the employee will receive his/her salary for first time
                            'and his/her attended work days is less than five
                            'the system will skip the gov't contributions for this period
                            '**************************************************************************************
                            'If skipgovtdeduct = "1" _
                            '    And StartingAttendanceCount < 5.0 _
                            '    And sel_dtemployeefirsttimesalary.Count <> 0 Then

                            '    pstub_TotalEmpSSS = 0
                            '    pstub_TotalCompSSS = 0

                            '    pstub_TotalEmpPhilhealth = 0
                            '    pstub_TotalCompPhilhealth = 0

                            '    pstub_TotalEmpHDMF = 0
                            '    pstub_TotalCompHDMF = 0

                            'End If

                            Dim the_EmpRatePerDay = ValNoComma(drow("EmpRatePerDay"))

                            Dim the_MinimumWageAmount = ValNoComma(drow("MinimumWageAmount"))

                            Dim isMinimumWage = (ValNoComma(drow("EmpRatePerDay")) <= ValNoComma(drow("MinimumWageAmount")))

                            Dim _eRowID = drow("RowID")

                            If isEndOfMonth = isorgWTaxdeductsched Then

                                emp_taxabsal = grossincome -
                                                    (pstub_TotalEmpSSS + pstub_TotalEmpPhilhealth + pstub_TotalEmpHDMF)

                                the_taxable_salary = (grossincome + grossincome_firsthalf) -
                                                    (pstub_TotalEmpSSS + pstub_TotalEmpPhilhealth + pstub_TotalEmpHDMF)

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

                                    Dim sel_payWTax = payWTax.Select("FilingStatusID = " & fstat_id &
                                                                         " AND PayFrequencyID = 2 AND " &
                                                                         "TaxableIncomeFromAmount >= " & the_taxable_salary & " AND " & the_taxable_salary & " <= TaxableIncomeToAmount" &
                                                                         "")

                                    Dim GET_employeetaxableincome = New MySQLExecuteQuery("SELECT `GET_employeetaxableincome`('" & drow("RowID") & "', '" & orgztnID & "', '" & n_PayrollDateFrom & "','" & grossincome & "');").Result


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

                            Else
                                'PAYFREQUENCY_DIVISOR

                                emp_taxabsal = grossincome -
                                                    (pstub_TotalEmpSSS + pstub_TotalEmpPhilhealth + pstub_TotalEmpHDMF)

                                the_taxable_salary = grossincome -
                                                    (pstub_TotalEmpSSS + pstub_TotalEmpPhilhealth + pstub_TotalEmpHDMF)

                                If isMinimumWage Then

                                    tax_amount = 0

                                ElseIf isorgWTaxdeductsched = 2 Then

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

                                    Dim sel_payWTax = payWTax.Select("FilingStatusID = " & fstat_id &
                                                                         " AND PayFrequencyID = 2 AND " &
                                                                         "TaxableIncomeFromAmount >= " & the_taxable_salary & " AND " & the_taxable_salary & " <= TaxableIncomeToAmount" &
                                                                         "")

                                    Dim GET_employeetaxableincome = New MySQLExecuteQuery("SELECT `GET_employeetaxableincome`('" & drow("RowID") & "', '" & orgztnID & "', '" & n_PayrollDateFrom & "','" & grossincome & "');").Result

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


                    Dim tot_net_pay = emp_taxabsal - valemp_loan - tax_amount

                    'emptaxabsal

                    the_taxable_salary = the_taxable_salary

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
                        Dim n_ExecuteQuery As _
                                New MySQLExecuteQuery("UPDATE employee e" &
                                                 " INNER JOIN payperiod pp ON pp.RowID='" & n_PayrollRecordID & "'" &
                                                 " SET" &
                                                 " e.LeaveBalance=IF(e.LeaveAllowance - (e.LeavePerPayPeriod * pp.OrdinalValue) <= 0, e.LeavePerPayPeriod, e.LeaveAllowance - (e.LeavePerPayPeriod * pp.OrdinalValue))" &
                                                 ",e.SickLeaveBalance=IF(e.SickLeaveAllowance - (e.SickLeavePerPayPeriod * pp.OrdinalValue) <= 0, e.SickLeavePerPayPeriod, e.SickLeaveAllowance - (e.SickLeavePerPayPeriod * pp.OrdinalValue))" &
                                                 "" &
                                                 ",e.MaternityLeaveBalance=e.MaternityLeaveAllowance" &
                                                 "" &
                                                 ",e.OtherLeaveBalance=IF(e.OtherLeaveAllowance - (e.OtherLeavePerPayPeriod * pp.OrdinalValue) <= 0, e.OtherLeavePerPayPeriod, e.OtherLeaveAllowance - (e.OtherLeavePerPayPeriod * pp.OrdinalValue))" &
                                                 ",e.LastUpd=CURRENT_TIMESTAMP()" &
                                                 ",e.LastUpdBy='" & z_User & "'" &
                                                 " WHERE e.RowID='" & drow("RowID") & "'" &
                                                 " AND e.OrganizationID='" & orgztnID & "'" &
                                                 " AND e.PayFrequencyID=pp.TotalGrossSalary" &
                                                 " AND ADDDATE(e.StartDate, INTERVAL 1 YEAR) BETWEEN '" & n_PayrollDateFrom & "' AND '" & n_PayrollDateTo & "';")

                        'Dim n_ExecuteQuery As _
                        '    New MySQLExecuteQuery("UPDATE employee e SET" &
                        '                     " e.LeaveBalance=e.LeaveBalance + e.LeavePerPayPeriod" &
                        '                     ",e.SickLeaveBalance=e.SickLeaveBalance + e.SickLeavePerPayPeriod" &
                        '                     ",e.MaternityLeaveBalance=e.MaternityLeaveBalance + e.MaternityLeavePerPayPeriod" &
                        '                     ",e.OtherLeaveBalance=e.OtherLeaveBalance + e.OtherLeavePerPayPeriod" &
                        '                     ",LastUpd=CURRENT_TIMESTAMP()" &
                        '                     ",LastUpdBy='" & z_User & "'" &
                        '                     " WHERE e.RowID='" & drow("RowID") & "'" &
                        '                     " AND e.OrganizationID='" & orgztnID & "'" &
                        '                     " AND (ADDDATE(e.StartDate, INTERVAL 1 YEAR) <= '" & n_PayrollDateFrom & "'" &
                        '                     " OR ADDDATE(e.StartDate, INTERVAL 1 YEAR) <= '" & n_PayrollDateTo & "');")

                    End If

                    If ValNoComma(drow("RowID")) = 661 Then '218 - EmployeeID
                        Dim call_lambert = "Over here"
                    End If
                    'SET innodb_lock_wait_timeout = 5000; SET autocommit = 0;
                    '"UPDATE paystub SET LastUpd=CURRENT_TIMESTAMP(),LastUpdBy='" & z_User & "'" &
                    '" WHERE EmployeeID='" & Convert.ToString(drow("RowID")) & "' AND OrganizationID=" & orgztnID &
                    '" AND PayFromDate='" & n_PayrollDateFrom & "' AND PayToDate='" & n_PayrollDateTo & "';"
                    'Dim dsfsd As New MySQLExecuteQuery(my_cmd,
                    '                                   5000)
                    'n_PayStub, PayStub
                    'Dim paystubID = _
                    'n_PayStub.
                    'INSUPD_paystub(n_PayrollRecordID,
                    '               drow("RowID").ToString,
                    '               n_PayrollDateFrom,
                    '               n_PayrollDateTo,
                    '               grossincome + totalemployeebonus + totalnotaxemployeebonus + totalnotaxemployeeallownce + totalemployeeallownce,
                    '               tot_net_pay + totalemployeebonus + totalnotaxemployeebonus + totalnotaxemployeeallownce + totalemployeeallownce + thirteenthmoval,
                    '               the_taxable_salary,
                    '               tax_amount,
                    '               pstub_TotalEmpSSS,
                    '               pstub_TotalCompSSS,
                    '               pstub_TotalEmpPhilhealth,
                    '               pstub_TotalCompPhilhealth,
                    '               pstub_TotalEmpHDMF,
                    '               pstub_TotalCompHDMF,
                    '               valemp_loan,
                    '               totalemployeebonus + totalnotaxemployeebonus,
                    '               totalemployeeallownce + totalnotaxemployeeallownce) 'totalemployeebonus + totalemployeeallownce +

                    '############ # # # # # # # # ##############
                    mysql_conn = New MySqlConnection
                    mysql_conn.ConnectionString = mysql_conn_text
                    Dim new_cmd As New MySqlCommand
                    'new_cmd = New MySqlCommand("INSUPD_paystub", mysql_conn, myTrans)
                    new_cmd = New MySqlCommand("INSUPDPROC_paystub",
                                                   mysql_conn,
                                                   myTrans)

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
                        .Parameters.AddWithValue("pstub_EmployeeID", drow("RowID"))

                        .Parameters.AddWithValue("pstub_TimeEntryID", DBNull.Value)

                        .Parameters.AddWithValue("pstub_PayFromDate", n_PayrollDateFrom) 'If(pstub_PayFromDate = Nothing, DBNull.Value, Format(CDate(pstub_PayFromDate), "yyyy-MM-dd")))
                        .Parameters.AddWithValue("pstub_PayToDate", n_PayrollDateTo) 'If(pstub_PayToDate = Nothing, DBNull.Value, Format(CDate(pstub_PayToDate), "yyyy-MM-dd")))

                        .Parameters.AddWithValue("pstub_TotalGrossSalary", grossincome + totalemployeebonus + totalnotaxemployeebonus + totalnotaxemployeeallownce + totalemployeeallownce)
                        .Parameters.AddWithValue("pstub_TotalNetSalary", tot_net_pay + totalemployeebonus + totalnotaxemployeebonus + totalnotaxemployeeallownce + totalemployeeallownce + thirteenthmoval)
                        .Parameters.AddWithValue("pstub_TotalTaxableSalary", the_taxable_salary)
                        .Parameters.AddWithValue("pstub_TotalEmpWithholdingTax", tax_amount)

                        .Parameters.AddWithValue("pstub_TotalEmpSSS", pstub_TotalEmpSSS) 'DBNull.Value
                        .Parameters.AddWithValue("pstub_TotalCompSSS", pstub_TotalCompSSS)
                        .Parameters.AddWithValue("pstub_TotalEmpPhilhealth", pstub_TotalEmpPhilhealth)
                        .Parameters.AddWithValue("pstub_TotalCompPhilhealth", pstub_TotalCompPhilhealth)
                        .Parameters.AddWithValue("pstub_TotalEmpHDMF", pstub_TotalEmpHDMF)
                        .Parameters.AddWithValue("pstub_TotalCompHDMF", pstub_TotalCompHDMF)
                        .Parameters.AddWithValue("pstub_TotalVacationDaysLeft", pstub_TotalVacationDaysLeft)
                        .Parameters.AddWithValue("pstub_TotalLoans", valemp_loan) 'pstub_TotalLoans
                        .Parameters.AddWithValue("pstub_TotalBonus", pstub_TotalBonus)
                        .Parameters.AddWithValue("pstub_TotalAllowance", pstub_TotalAllowance)

                        '.Parameters.Add("paystubID", MySqlDbType.Int32)
                        '.Parameters("paystubID").Direction = ParameterDirection.ReturnValue
                        'Dim d_reader As MySqlDataReader
                        'd_reader = .ExecuteReader()

                        .ExecuteNonQuery()
                        myTrans.Commit()
                    End With
                    new_cmd.Dispose()
                    '############ # # # # # # # # ##############

                    'For Each strvalID In allow_type

                    '    'INSERT INTO paystubitem(RowID,OrganizationID,Created,CreatedBy,PayStubID,ProductID,PayAmount,Undeclared)

                    '    Dim totalallowance_amount = ValNoComma(0)

                    '    totalallowance_amount = ValNoComma(emp_allowanceDaily.Compute("SUM(TotalAllowanceAmount)",
                    '                                                                  "ProductID = '" & strvalID & "' AND EmployeeID = '" & drow("RowID") & "'"))

                    '    totalallowance_amount += ValNoComma(notax_allowanceDaily.Compute("SUM(TotalAllowanceAmount)",
                    '                                                                  "ProductID = '" & strvalID & "' AND EmployeeID = '" & drow("RowID") & "'"))

                    '    totalallowance_amount += ValNoComma(emp_allowanceMonthly.Compute("SUM(TotalAllowanceAmount)",
                    '                                                                  "ProductID = '" & strvalID & "' AND EmployeeID = '" & drow("RowID") & "'"))

                    '    totalallowance_amount += ValNoComma(notax_allowanceMonthly.Compute("SUM(TotalAllowanceAmount)",
                    '                                                                  "ProductID = '" & strvalID & "' AND EmployeeID = '" & drow("RowID") & "'"))

                    '    totalallowance_amount += ValNoComma(emp_allowanceSemiM.Compute("SUM(TotalAllowanceAmount)",
                    '                                                                  "ProductID = '" & strvalID & "' AND EmployeeID = '" & drow("RowID") & "'"))

                    '    totalallowance_amount += ValNoComma(notax_allowanceSemiM.Compute("SUM(TotalAllowanceAmount)",
                    '                                                                  "ProductID = '" & strvalID & "' AND EmployeeID = '" & drow("RowID") & "'"))

                    '    Dim n_ExecuteQuery As _
                    '        New MySQLExecuteQuery("INSERT INTO paystubitem(OrganizationID,Created,CreatedBy,PayStubID,ProductID,PayAmount,Undeclared)" &
                    '                         " VALUES('" & orgztnID & "'" &
                    '                             ",CURRENT_TIMESTAMP()" &
                    '                             ",'" & z_User & "'" &
                    '                             ",'" & paystubID & "'" &
                    '                             ",'" & strvalID & "'" &
                    '                             "," & totalallowance_amount &
                    '                             ",'0'" &
                    '                         ") ON" &
                    '                         " DUPLICATE" &
                    '                         " KEY" &
                    '                         " UPDATE" &
                    '                             " LastUpd=CURRENT_TIMESTAMP()" &
                    '                             ",LastUpdBy='" & z_User & "'" &
                    '                             ",PayAmount=" & totalallowance_amount & ";")

                    'Next

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
                                                             " AND `Date` BETWEEN '" & n_PayrollDateFrom & "' AND '" & n_PayrollDateTo & "') ete ON ete.RowID IS NOT NULL" &
                                                " SET" &
                                                " e.LeaveBalance = e.LeaveBalance - IFNULL(ete.VacationLeaveHours, 0)" &
                                                ",e.SickLeaveBalance = e.SickLeaveBalance - IFNULL(ete.SickLeaveHours,0)" &
                                                ",e.MaternityLeaveBalance = e.MaternityLeaveBalance - IFNULL(ete.MaternityLeaveHours,0)" &
                                                ",e.OtherLeaveBalance = e.OtherLeaveBalance - IFNULL(ete.OtherLeaveHours,0)" &
                                                ",LastUpd=CURRENT_TIMESTAMP()" &
                                                ",LastUpdBy='" & z_User & "'" &
                                                " WHERE e.RowID='" & drow("RowID") & "' AND e.OrganizationID='" & orgztnID & "';")

                    End If

                    ''Dim nn As New MySQLQueryToDataTable("CALL `SP_UpdatePaystubAdjustment`('" & drow("RowID") & "'" &
                    'Dim newer_ExecuteQuery As _
                    'New MySQLExecuteQuery("CALL `SP_UpdatePaystubAdjustment`('" & drow("RowID") & "'" &
                    '                                  ",'" & n_PayrollRecordID & "'" &
                    '                                  ",'" & z_User & "'" &
                    '                                  ",'" & orgztnID & "');", 5000)

                    progress_index += 1

                    'If IsFirstPayperiodOfTheYear() Then
                    '    ConvertLeaveToCash(drow)
                    'End If

                    'Dim f_args(1) As Object

                    ''bgworkgenpayroll.ReportProgress(CInt((100 * progress_index) / emp_count), "")

                    'f_args(1) = Convert.ToInt32(progress_index / emp_count) 'Convert.ToInt32(drow("counter"))

                    form_caller.Invoke(m_NotifyMainWindow,
                                           1)
                    'Convert.ToInt16(progress_index / emp_count))

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

            Next

        Catch ex As Exception
            'MsgBox
            Console.WriteLine(getErrExcptn(ex, "PayrollGeneration - " & employee_dattab.TableName))

        Finally
            employee_dattab.Dispose()
            esal_dattab.Dispose()
            emp_loans.Dispose()
            emp_bonus.Dispose()
            emp_allowanceDaily.Dispose()
            emp_allowanceMonthly.Dispose()
            emp_allowanceOnce.Dispose()
            emp_allowanceSemiM.Dispose()
            emp_allowanceWeekly.Dispose()
            notax_allowanceDaily.Dispose()
            notax_allowanceMonthly.Dispose()
            notax_allowanceOnce.Dispose()
            notax_allowanceSemiM.Dispose()
            notax_allowanceWeekly.Dispose()
            emp_bonusDaily.Dispose()
            emp_bonusMonthly.Dispose()
            emp_bonusOnce.Dispose()
            emp_bonusSemiM.Dispose()
            emp_bonusWeekly.Dispose()
            notax_bonusDaily.Dispose()
            notax_bonusMonthly.Dispose()
            notax_bonusOnce.Dispose()
            notax_bonusSemiM.Dispose()
            notax_bonusWeekly.Dispose()
            numofdaypresent.Dispose()
            etent_totdaypay.Dispose()
            dtemployeefirsttimesalary.Dispose()
            prev_empTimeEntry.Dispose()
            payWTax.Dispose()
            filingStatus.Dispose()
            fixedNonTaxableMonthlyAllowances.Dispose()
            fixedTaxableMonthlyAllowances.Dispose()

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

    Private Sub CalculateSSS(salary As DataRow)
        Dim monthlyEmployeeSSS = CDec(salary("EmployeeContributionAmount"))
        Dim monthlyEmployerSSS = CDec(salary("EmployerContributionAmount"))

        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))

        If isEndOfMonth = isorgSSSdeductsched Then

            _employeeSSS = monthlyEmployeeSSS
            _employerSSS = monthlyEmployerSSS

        ElseIf isorgSSSdeductsched = 2 Then 'Per pay period

            _employeeSSS = monthlyEmployeeSSS / payPeriodsPerMonth
            _employerSSS = monthlyEmployerSSS / payPeriodsPerMonth

        End If
    End Sub

    Private Sub CalculatePhilHealth(salary As DataRow)
        Dim employeePhilHealthPerMonth = CDec(salary("EmployeeShare"))
        Dim monthlyEmployerPhilHealth = CDec(salary("EmployerShare"))
        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))

        If isEndOfMonth = isorgPHHdeductsched Then

            _employeePhilHealth = employeePhilHealthPerMonth
            _employerPhilHealth = monthlyEmployerPhilHealth

        ElseIf isorgPHHdeductsched = 2 Then 'Per pay period

            _employeePhilHealth = employeePhilHealthPerMonth / payPeriodsPerMonth
            _employerPhilHealth = monthlyEmployerPhilHealth / payPeriodsPerMonth

        End If
    End Sub

    Private Sub CalculateHDMF(salary As DataRow)
        Dim employeeHdmfPerMonth = CDec(salary("HDMFAmount"))
        Dim employerHdmfPerMonth = 100D
        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))

        If isEndOfMonth = isorgHDMFdeductsched Then
            _employeeHDMF = employeeHdmfPerMonth
            _employerHDMF = employerHdmfPerMonth
        ElseIf isorgHDMFdeductsched = 2 Then 'Per pay period
            _employeeHDMF = employeeHdmfPerMonth / payPeriodsPerMonth
            _employerHDMF = employerHdmfPerMonth / payPeriodsPerMonth
        End If
    End Sub

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

        sql = sql.Replace("@OrganizationID", z_OrganizationID) _
            .Replace("@Year", previousYear)

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
        Dim unusedLeave = annualUnusedLeaves.Select("EmployeeID=" & employeeID) _
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
        Dim salaryAgreement = esal_dattab.Select("EmployeeID=" & employeeID) _
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