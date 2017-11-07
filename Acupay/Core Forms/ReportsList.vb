Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports OfficeOpenXml

Public Class ReportsList

    Private basic_alphabet =
        New String() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

    Public listReportsForm As New List(Of String)

    Sub ChangeForm(ByVal Formname As Form)

        Try
            Application.DoEvents()
            Dim FName As String = Formname.Name
            'Formname.WindowState = FormWindowState.Maximized
            Formname.TopLevel = False

            If listReportsForm.Contains(FName) Then
                Formname.Show()
                Formname.BringToFront()

            Else
                FormReports.PanelReport.Controls.Add(Formname)
                listReportsForm.Add(Formname.Name)

                Formname.Show()
                Formname.BringToFront()

                'Formname.Location = New Point((PanelGeneral.Width / 2) - (Formname.Width / 2), (PanelGeneral.Height / 2) - (Formname.Height / 2))
                'Formname.Anchor = AnchorStyles.Top And AnchorStyles.Bottom And AnchorStyles.Right And AnchorStyles.Left
                'Formname.WindowState = FormWindowState.Maximized
                Formname.Dock = DockStyle.Fill

            End If

        Catch ex As Exception

            Exit Sub

        End Try

    End Sub

    Private Sub ReportsList_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'LV_Item("Attendance sheet")

        'LV_Item("Alpha list")

        ''LV_Item("Employee's Dialect")

        'LV_Item("Employee's Employment Record")

        'LV_Item("Employee's History of Salary Increase")

        'LV_Item("Employee's Identification Number")

        'LV_Item("Employee's Offenses")

        'LV_Item("Employee's Payroll Ledger")

        ''LV_Item("Employee's Religion")

        ''LV_Item("Employee's Spouse Information")

        'LV_Item("Employee 13th Month Pay Report")

        ''LV_Item("Employee Citizenship")

        'LV_Item("Employee Leave Ledger")

        'LV_Item("Employee Loan Report")

        'LV_Item("Employee Personal Information")

        ''LV_Item("Employee Skills")

        'LV_Item("Loan Report")

        'LV_Item("PAGIBIG Monthly Report")

        'LV_Item("Payroll Summary Report")

        'LV_Item("PhilHealth Monthly Report")

        'LV_Item("SSS Monthly Report")

        'LV_Item("Tax Monthly Report")

        Dim report_list As New AutoCompleteStringCollection

        enlistTheLists("SELECT DisplayValue FROM listofval WHERE `Type`='Report List' AND `Active`='Yes' ORDER BY RowID;",
                        report_list)

        For Each strval In report_list
            LV_Item(strval)
        Next

    End Sub

    Sub LV_Item(ByVal m_MenuName As String, Optional m_Icon As Integer = -1)

        'Dim ls As New ListViewItem

        If m_Icon = -1 Then

            'ls = lvMainMenu.Items.Add(m_MenuName)

            lvMainMenu.Items.Add(m_MenuName)

        Else

            'ls = lvMainMenu.Items.Add(m_MenuName, m_Icon)

            lvMainMenu.Items.Add(m_MenuName, m_Icon)

        End If

    End Sub

    Private Sub lvMainMenu_KeyDown(sender As Object, e As KeyEventArgs) Handles lvMainMenu.KeyDown

        If lvMainMenu.Items.Count <> 0 Then

            If e.KeyCode = Keys.Enter Then

                report_maker()

            End If

        End If

    End Sub

    Dim date_from As String = Nothing

    Dim date_to As String = Nothing

    Private Sub lvMainMenu_MouseDown(sender As Object, e As MouseEventArgs) Handles lvMainMenu.MouseDown

    End Sub

    Private Sub lvMainMenu_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lvMainMenu.MouseDoubleClick

        date_from = Nothing

        date_to = Nothing

        If lvMainMenu.Items.Count > 0 _
            And e.Button = Windows.Forms.MouseButtons.Left Then

            report_maker()

        End If

    End Sub

    Private Sub lvMainMenu_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvMainMenu.SelectedIndexChanged

    End Sub

    Dim PayrollSummaChosenData As String = String.Empty

    Public reportname As String = String.Empty

    Sub report_maker()

        reportname = String.Empty

        PayrollSummaChosenData = String.Empty
        Dim n_listviewitem As New ListViewItem

        Try

            n_listviewitem = lvMainMenu.SelectedItems(0)

        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))

            Exit Sub

        End Try

        Dim lvi_index =
            lvMainMenu.Items.IndexOf(n_listviewitem)

        reportname = n_listviewitem.Text

        Select Case lvi_index

            Case 0 'Attendance sheet

                Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

                If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

                    Dim dat_tbl As New DataTable

                    Dim d_from = If(n_PayrollSummaDateSelection.DateFromstr = Nothing, Nothing,
                                    Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "yyyy-MM-dd"))

                    Dim d_to = If(n_PayrollSummaDateSelection.DateTostr = Nothing, Nothing,
                                    Format(CDate(n_PayrollSummaDateSelection.DateTostr), "yyyy-MM-dd"))


                    date_from = If(n_PayrollSummaDateSelection.DateFromstr = Nothing, Nothing,
                                    Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "MMM d,yyyy"))

                    date_to = If(n_PayrollSummaDateSelection.DateTostr = Nothing, Nothing,
                                    Format(CDate(n_PayrollSummaDateSelection.DateTostr), "MMM d,yyyy"))

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizationID"
                    params(1, 0) = "FromDate"
                    params(2, 0) = "ToDate"

                    params(0, 1) = orgztnID
                    params(1, 1) = d_from
                    params(2, 1) = d_to

                    dat_tbl = callProcAsDatTab(params,
                                               "RPT_attendance_sheet")

                    printReport(dat_tbl)

                End If

            Case 1 'Alpha list

            Case 2 'Employment Record

                '   Case 2 'Employee 13th Month Pay Report

            Case 3 'History of salary

                '   Case 3 'Official Business filing

            Case 4 'ID number

                '   Case 4 'Employee Loan Report

            Case 5 'Offenses

                '   Case 5 'PAGIBIG Monthly Report

            Case 6 'Payroll Ledger

                '   Case 6 'Payroll Summary Report

            Case 7 '13th month pay

                '   Case 7 'PhilHealth Monthly Report

                Dim n_promptyear As New promptyear

                If n_promptyear.ShowDialog = Windows.Forms.DialogResult.OK Then

                    Dim params(4, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramYear"

                    params(0, 1) = orgztnID
                    params(1, 1) = n_promptyear.YearValue

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_13thmonthpay")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 8 'Leave ledger

                '   Case 8 'SSS Monthly Report

            Case 9 'Loan report

                printLoanReports()

            Case 10 'Personal Information

                printEmployeeProfiles()

            Case 11 'PAGIBIG

                Dim n_selectMonth As New selectMonth

                If n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then

                    'MsgBox(Format(CDate(n_selectMonth.MonthValue), "MM"))

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramDate"

                    params(0, 1) = orgztnID

                    Dim thedatevalue = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

                    params(1, 1) = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

                    date_from = Format(CDate(n_selectMonth.MonthValue), "MMMM  yyyy")

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_PAGIBIG_Monthly")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 12 'Payroll summary

                Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

                n_PayrollSummaDateSelection.ReportIndex = lvi_index

                If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

                    Dim params(4, 2) As Object

                    params(0, 0) = "ps_OrganizationID"
                    params(1, 0) = "ps_PayPeriodID1"
                    params(2, 0) = "ps_PayPeriodID2"
                    params(3, 0) = "psi_undeclared"
                    params(4, 0) = "strSalaryDistrib"

                    params(0, 1) = orgztnID
                    params(1, 1) = n_PayrollSummaDateSelection.DateFromID
                    params(2, 1) = n_PayrollSummaDateSelection.DateToID

                    MessageBoxManager.OK = "Actual"

                    MessageBoxManager.Cancel = "Declared"

                    MessageBoxManager.Register()

                    Dim custom_prompt =
                        MessageBox.Show("Choose the payroll summary to be printed.", "Payroll Summary Data Option", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

                    If custom_prompt = Windows.Forms.DialogResult.OK Then

                        params(3, 1) = "1"

                        PayrollSummaChosenData = " (ACTUAL)"

                    Else

                        params(3, 1) = "0"

                        PayrollSummaChosenData = " (DECLARED)"

                    End If

                    params(4, 1) = n_PayrollSummaDateSelection.
                                   cboStringParameter.
                                   Text

                    MessageBoxManager.Unregister()

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "PAYROLLSUMMARY")

                    Dim AbsTardiUTNDifOTHolipay As New DataTable

                    Dim paramets(4, 2) As Object

                    paramets(0, 0) = "param_OrganizationID"
                    paramets(1, 0) = "param_EmployeeRowID"
                    paramets(2, 0) = "param_PayPeriodID1"
                    paramets(3, 0) = "param_PayPeriodID2"
                    paramets(4, 0) = "IsActual"
                    paramets(0, 1) = orgztnID
                    'paramets(1, 1) = drow("EmployeeRowID")R
                    paramets(2, 1) = n_PayrollSummaDateSelection.DateFromID
                    paramets(3, 1) = n_PayrollSummaDateSelection.DateToID
                    paramets(4, 1) = params(3, 1)
                    Dim newdatrow As DataRow

                    Dim dt_result As New DataTable

                    For i = 1 To 40
                        dt_result.Columns.Add("Column" & i)
                    Next

                    For Each drow As DataRow In datatab.Rows

                        newdatrow = dt_result.NewRow

                        newdatrow("Column1") = If(IsDBNull(drow(17)), "None", drow(17)) 'Division
                        newdatrow("Column2") = drow(11) 'Employee ID

                        newdatrow("Column3") = drow(14) & ", " & drow(12) & If(Trim(drow(13)) = "", "", ", " & drow(13)) 'Full name

                        newdatrow("Column4") = If(IsDBNull(drow(16)), "None", drow(16)) 'Position

                        newdatrow("Column20") = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "MMMM d, yyyy") &
            If(n_PayrollSummaDateSelection.DateFromstr = Nothing, "", " to " & Format(CDate(n_PayrollSummaDateSelection.DateTostr), "MMMM d, yyyy")) 'Pay period

                        newdatrow("Column21") = FormatNumber(Val(drow(0)), 2) 'Basic pay
                        newdatrow("Column22") = FormatNumber(Val(drow(1)), 2) 'Gross income
                        newdatrow("Column23") = FormatNumber(Val(drow(2)), 2) 'Net salary
                        newdatrow("Column24") = FormatNumber(Val(drow(3)), 2) 'Taxable income
                        newdatrow("Column25") = FormatNumber(Val(drow(4)), 2) 'SSS
                        newdatrow("Column26") = FormatNumber(Val(drow(5)), 2) 'Withholding tax
                        newdatrow("Column27") = FormatNumber(Val(drow(6)), 2) 'PhilHealth
                        newdatrow("Column28") = FormatNumber(Val(drow(7)), 2) 'PAGIBIG
                        newdatrow("Column29") = FormatNumber(Val(drow(8)), 2) 'Loans
                        newdatrow("Column30") = FormatNumber(Val(drow(9)), 2) 'Bonus
                        newdatrow("Column31") = FormatNumber(Val(drow(10)), 2) 'Allowance


                        paramets(1, 1) = drow("EmployeeRowID")

                        AbsTardiUTNDifOTHolipay = callProcAsDatTab(paramets,
                                                                   "GET_AbsTardiUTNDifOTHolipay")

                        Dim absentval = 0.0

                        Dim tardival = 0.0

                        Dim UTval = 0.0

                        Dim ndiffOTval = 0.0

                        Dim holidayval = 0.0

                        Dim overtimeval = 0.0

                        Dim ndiffval = 0.0


                        For Each ddrow As DataRow In AbsTardiUTNDifOTHolipay.Rows

                            If Trim(ddrow("PartNo")) = "Absent" Then

                                absentval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Tardiness" Then

                                tardival = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Undertime" Then

                                UTval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Night differential OT" Then

                                ndiffOTval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Holiday pay" Then

                                holidayval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Overtime" Then

                                overtimeval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Night differential" Then

                                ndiffval = Val(ddrow("PayAmount"))

                            End If

                        Next


                        newdatrow("Column32") = FormatNumber(absentval, 2) 'Absent

                        'newdatrow("Column33") = FormatNumber(tardival, 2) 'Tardiness

                        'newdatrow("Column34") = FormatNumber(UTval, 2) 'Undertime

                        'newdatrow("Column35") = FormatNumber(ndiffval, 2) 'Night differential

                        'newdatrow("Column36") = FormatNumber(holidayval, 2) 'Holiday pay

                        'newdatrow("Column37") = FormatNumber(overtimeval, 2) 'Overtime

                        'newdatrow("Column38") = FormatNumber(ndiffOTval, 2) 'Night differential OT

                        '***********************************************************************************

                        'newdatrow("DatCol33") = FormatNumber(Val(drow("Absent")), 2) 'Tardiness

                        newdatrow("Column33") = FormatNumber(Val(drow("Tardiness")), 2) 'Tardiness

                        newdatrow("Column34") = FormatNumber(Val(drow("UnderTime")), 2) 'Undertime

                        newdatrow("Column35") = FormatNumber(Val(drow("NightDifftl")), 2) 'Night differential

                        newdatrow("Column36") = FormatNumber(Val(drow("HolidayPay")), 2) 'Holiday pay

                        newdatrow("Column37") = FormatNumber(Val(drow("OverTime")), 2) 'Overtime

                        newdatrow("Column38") = FormatNumber(Val(drow("NightDifftlOT")), 2) 'Night differential OT

                        newdatrow("Column39") = FormatNumber(ValNoComma(drow("DatCol39")), 2) 'AGENCY FEE

                        AbsTardiUTNDifOTHolipay = Nothing

                        dt_result.Rows.Add(newdatrow)

                    Next

                    printReport(dt_result)

                    dt_result = Nothing

                End If

            Case 13 'PhilHealth

                Dim n_selectMonth As New selectMonth

                If n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then

                    'MsgBox(Format(CDate(n_selectMonth.MonthValue), "MM"))

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramDate"

                    params(0, 1) = orgztnID

                    Dim thedatevalue = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

                    params(1, 1) = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

                    date_from = Format(CDate(n_selectMonth.MonthValue), "MMMM  yyyy")

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_PhilHealth_Monthly")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 14 'SSS

                Dim n_selectMonth As New selectMonth

                If n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then

                    'MsgBox(Format(CDate(n_selectMonth.MonthValue), "MM"))

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramDate"

                    params(0, 1) = orgztnID

                    Dim thedatevalue = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

                    params(1, 1) = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

                    date_from = Format(CDate(n_selectMonth.MonthValue), "MMMM  yyyy")

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_SSS_Monthly")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 15 'Tax

                Dim n_selectMonth As New selectMonth

                If n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then

                    'MsgBox(Format(CDate(n_selectMonth.MonthValue), "MM"))

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramDateFrom"
                    params(2, 0) = "paramDateTo"

                    params(0, 1) = orgztnID
                    params(1, 1) = Format(CDate(n_selectMonth.MonthFirstDate), "yyyy-MM-dd")
                    params(2, 1) = Format(CDate(n_selectMonth.MonthLastDate), "yyyy-MM-dd")

                    date_from = Format(CDate(n_selectMonth.MonthValue), "MMMM  yyyy")

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_Tax_Monthly")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 16 'Post employement clearance

            Case 17 'Agency fee

            Case Else

        End Select

    End Sub

    Sub report_makerPreviousBuild()

        PayrollSummaChosenData = String.Empty
        Dim n_listviewitem As New ListViewItem

        n_listviewitem = lvMainMenu.SelectedItems(0)

        Dim lvi_index =
            lvMainMenu.Items.IndexOf(n_listviewitem)

        Select Case lvi_index

            Case 0 'Attendance sheet

                Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

                If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

                    Dim dat_tbl As New DataTable

                    Dim d_from = If(n_PayrollSummaDateSelection.DateFromstr = Nothing, Nothing,
                                    Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "yyyy-MM-dd"))

                    Dim d_to = If(n_PayrollSummaDateSelection.DateTostr = Nothing, Nothing,
                                    Format(CDate(n_PayrollSummaDateSelection.DateTostr), "yyyy-MM-dd"))


                    date_from = If(n_PayrollSummaDateSelection.DateFromstr = Nothing, Nothing,
                                    Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "MMMM d, yyyy"))

                    date_to = If(n_PayrollSummaDateSelection.DateTostr = Nothing, Nothing,
                                    Format(CDate(n_PayrollSummaDateSelection.DateTostr), "MMMM d, yyyy"))

                    '",IFNULL(CONCAT(TIME_FORMAT(sh.TimeFrom,'%l:%i'),'TO',TIME_FORMAT(sh.TimeTo,'%l:%i')),'') 'Shift'" & _

                    'retAsDatTbl("SELECT " & _
                    '                      "CONCAT(ee.EmployeeID,' / ',ee.LastName,',',ee.FirstName,IFNULL(IF(ee.MiddleName='','',CONCAT(',',INITIALS(ee.MiddleName,'. ','1'))),'')) 'Fullname'" & _
                    '                      ",UCASE(SUBSTRING(DATE_FORMAT(ete.Date,'%W'),1,3)) 'DayText'" & _
                    '                      ",DATE_FORMAT(ete.Date,'%m/%e/%Y') 'Date'" & _
                    '                      ",IFNULL(CONCAT(TIME_FORMAT(sh.TimeFrom,'%l'),IF(TIME_FORMAT(sh.TimeFrom,'%i') > 0, CONCAT(':',TIME_FORMAT(sh.TimeFrom,'%i')),''),'to',TIME_FORMAT(sh.TimeTo,'%l'),IF(TIME_FORMAT(sh.TimeTo,'%i') > 0, CONCAT(':',TIME_FORMAT(sh.TimeTo,'%i')),'')),'') 'Shift'" & _
                    '                      ",REPLACE(TIME_FORMAT(etd.TimeIn,'%l:%i %p'),'M','') 'TimeIn'" & _
                    '                      ",REPLACE(TIME_FORMAT(etd.TimeIn,'%l:%i %p'),'M','') 'BOut'" & _
                    '                      ",REPLACE(TIME_FORMAT(etd.TimeIn,'%l:%i %p'),'M','') 'BIn'" & _
                    '                      ",REPLACE(TIME_FORMAT(etd.TimeOut,'%l:%i %p'),'M','') 'TimeOut'" & _
                    '                      ",IFNULL(ete.TotalHoursWorked,0) 'TotalHoursWorked'" & _
                    '                      ",IFNULL(ete.HoursLate,0) 'HoursLate'" & _
                    '                      ",IFNULL(ete.UndertimeHours,0) 'UndertimeHours'" & _
                    '                      ",IFNULL(ete.NightDifferentialHours,0) 'NightDifferentialHours'" & _
                    '                      ",IFNULL(ete.OvertimeHoursWorked,0) 'OvertimeHoursWorked'" & _
                    '                      ",IFNULL(ete.NightDifferentialOTHours,0) 'NightDifferentialOTHours'" & _
                    '                      ",etd.TimeScheduleType" & _
                    '                      " FROM employeetimeentry ete" & _
                    '                      " LEFT JOIN employeeshift esh ON esh.RowID=ete.EmployeeShiftID" & _
                    '                      " LEFT JOIN shift sh ON sh.RowID=esh.ShiftID" & _
                    '                      " LEFT JOIN employeetimeentrydetails etd ON etd.EmployeeID=ete.EmployeeID AND etd.OrganizationID=ete.OrganizationID" & _
                    '                      " LEFT JOIN employee ee ON ee.RowID=ete.EmployeeID" & _
                    '                      " WHERE ete.DATE" & _
                    '                      " BETWEEN '" & d_from & "'" & _
                    '                      " AND '" & d_to & "'" & _
                    '                      " AND ete.OrganizationID='" & orgztnID & "'" & _
                    '                      " GROUP BY ete.RowID" & _
                    '                      " ORDER BY ete.Date;")

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizationID"
                    params(1, 0) = "FromDate"
                    params(2, 0) = "ToDate"

                    params(0, 1) = orgztnID
                    params(1, 1) = d_from
                    params(2, 1) = d_to

                    dat_tbl = callProcAsDatTab(params,
                                               "RPT_attendance_sheet")

                    printReport(dat_tbl)

                End If

            Case 1 'Alpha list

                '**********************

                'Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

                'If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

                '    Dim params(2, 1) As Object

                '    params(0, 0) = "OrganizID"
                '    params(1, 0) = "AnnualDateFrom"
                '    params(2, 0) = "AnnualDateTo"

                '    params(0, 1) = orgztnID
                '    params(1, 1) = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "yyyy-MM-dd")
                '    params(2, 1) = Format(CDate(n_PayrollSummaDateSelection.DateTostr), "yyyy-MM-dd")

                '    Dim AnnualizedWithholdingTax As New DataTable

                '    AnnualizedWithholdingTax = _
                '        callProcAsDatTab(params, _
                '                         "RPT_AnnualizedWithholdingTax")


                '    Dim paramet(2, 1) As Object

                '    paramet(0, 0) = "OrganizID"
                '    paramet(1, 0) = "LastDateOfFinancialYear"
                '    paramet(2, 0) = "FirstDateOfFinancialYear"

                '    paramet(0, 1) = orgztnID
                '    paramet(1, 1) = Format(CDate(n_PayrollSummaDateSelection.DateTostr), "yyyy-MM-dd")
                '    paramet(2, 1) = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "yyyy-MM-dd")

                '    'Dim the_lastdate = EXECQUER("SELECT `RPT_LastD ateOfFinancialYear`();")
                '    'paramet(1, 1) = Format(CDate(the_lastdate), "yyyy-MM-dd")
                '    'paramet(2, 1) = Format(CDate(the_lastdate), "yyyy-MM-dd")

                '    Dim getGrossCompensation As New DataTable

                '    getGrossCompensation = _
                '        callProcAsDatTab(paramet, _
                '                         "RPT_getGrossCompensation")


                '    Dim employee_RowID As New AutoCompleteStringCollection

                '    enlistTheLists("SELECT e.RowID FROM" & _
                '                    " employee e" & _
                '                    " WHERE e.OrganizationID='" & orgztnID & "'" & _
                '                    " ORDER BY e.LastName DESC;", _
                '                    employee_RowID) 'RowID


                '    For Each strval In employee_RowID

                '        Dim total_GrossCompen = Val(0)

                '        Dim total_OverTime = Val(0)

                '        Dim less_SSS_Philhealth_PagIbig = Val(0)

                '        Dim less_TaxableAllowance = Val(0)

                '        Dim less_PersonalExemption = Val(0)

                '        Dim less_AddtnallExemption = Val(0)

                '        Dim less_DeMinimisExemption = Val(0)

                '        Dim netTaxableCompensation = Val(0)


                '        Dim sel_AnnualizedWithholdingTax = AnnualizedWithholdingTax.Compute("SUM(AllowanceYesTax)", "EmployeeID=" & strval)

                '        Dim sel_AnnualizedWithholdingTax1 = AnnualizedWithholdingTax.Select("EmployeeID=" & strval)


                '        Dim sel_getGrossCompensation = getGrossCompensation.Select("EmployeeID=" & strval)


                '        Dim first_tax = Val(0)

                '        Dim plusOverExcess = Val(0)

                '        Dim taxdueValue = Val(0)


                '        For Each drow In sel_getGrossCompensation

                '            total_GrossCompen = ValNoComma(drow("TotalGrossCompensation")) '+ Val(drow("TotalAllowance"))

                '            less_SSS_Philhealth_PagIbig = ValNoComma(drow("EmployeeContributionAmount")) _
                '                                        + ValNoComma(drow("EmployeeShare")) _
                '                                        + ValNoComma(drow("HDMFAmount"))

                '            less_TaxableAllowance = ValNoComma(sel_AnnualizedWithholdingTax) 'ddrow("AllowanceYesTax")

                '            For Each ddrow In sel_AnnualizedWithholdingTax1

                '                total_OverTime = ValNoComma(ddrow("TotalOverTime"))

                '                less_PersonalExemption = ValNoComma(ddrow("PersonalExemption"))

                '                less_AddtnallExemption = ValNoComma(ddrow("AdditionalPersonalExemption"))

                '                less_DeMinimisExemption = ValNoComma(ddrow("DeMinimisExemption"))

                '                Exit For

                '            Next

                '            total_GrossCompen += total_OverTime

                '            total_GrossCompen += less_TaxableAllowance

                '            total_GrossCompen = total_GrossCompen - less_SSS_Philhealth_PagIbig

                '            'total_GrossCompen = total_GrossCompen - less_TaxableAllowance

                '            total_GrossCompen = total_GrossCompen - less_PersonalExemption

                '            total_GrossCompen = total_GrossCompen - less_AddtnallExemption

                '            total_GrossCompen = total_GrossCompen - less_DeMinimisExemption


                '            netTaxableCompensation = total_GrossCompen

                '            Dim tax_due As New DataTable

                '            tax_due = retAsDatTbl("SELECT *" & _
                '                                  " FROM taxdue " & _
                '                                  " WHERE " & netTaxableCompensation & "" & _
                '                                  " BETWEEN Over AND NotOver;")

                '            For Each d_row As DataRow In tax_due.Rows

                '                first_tax = Val(d_row("AmountRate"))

                '                plusOverExcess = (Val(d_row("NotOver")) - netTaxableCompensation) * Val(d_row("AdditionalPercent"))

                '                taxdueValue = first_tax + plusOverExcess

                '                Exit For

                '            Next

                '        Next

                '        'tax_amount = (Val(drowtax("ExemptionAmount")) + _
                '        '             ((emptaxabsal - Val(drowtax("TaxableIncomeFromAmount"))) * Val(drowtax("ExemptionInExcessAmount"))) _
                '        '             )

                '    Next

                'End If

            Case 2 'Employee's Employment Record

                Dim dt_prevemplyr As New DataTable


                Dim params(0, 1) As Object

                params(0, 0) = "OrganizatID"

                params(0, 1) = orgztnID

                dt_prevemplyr = callProcAsDatTab(params,
                                           "RPT_employment_record")

                printReport(dt_prevemplyr)

                dt_prevemplyr = Nothing

            Case 3 'Employee's History of Salary Increase

                Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

                If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

                    Dim dattab As New DataTable

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "PayPerDate1"
                    params(2, 0) = "PayPerDate2"

                    params(0, 1) = orgztnID
                    params(1, 1) = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "yyyy-MM-dd")
                    params(2, 1) = Format(CDate(n_PayrollSummaDateSelection.DateTostr), "yyyy-MM-dd")

                    dattab = callProcAsDatTab(params,
                                               "RPT_salary_increase_histo")

                    printReport(dattab)

                    dattab = Nothing

                End If

            Case 4 'Employee's Identification Number

            Case 5 'Employee's Offenses

            Case 6 'Employee's Payroll Ledger

                Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

                If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

                    Dim params(3, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "PayPerID1"
                    params(2, 0) = "PayPerID2"
                    params(3, 0) = "psi_undeclared"

                    params(0, 1) = orgztnID
                    params(1, 1) = n_PayrollSummaDateSelection.DateFromID
                    params(2, 1) = n_PayrollSummaDateSelection.DateToID

                    MessageBoxManager.OK = "Actual"

                    MessageBoxManager.Cancel = "Declared"

                    MessageBoxManager.Register()

                    Dim custom_prompt =
                        MessageBox.Show("Choose the payroll ledger to be printed.", "Payroll Ledger Data Option", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

                    If custom_prompt = Windows.Forms.DialogResult.OK Then

                        params(3, 1) = "1"

                        PayrollSummaChosenData = " (ACTUAL)"

                    Else

                        params(3, 1) = "0"

                        PayrollSummaChosenData = " (DECLARED)"

                    End If

                    MessageBoxManager.Unregister()

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_payroll_legder")

                    Dim AbsTardiUTNDifOTHolipay As New DataTable

                    Dim paramets(3, 2) As Object

                    paramets(0, 0) = "param_OrganizationID"
                    paramets(1, 0) = "param_EmployeeRowID"
                    paramets(2, 0) = "param_PayPeriodID1"
                    paramets(3, 0) = "param_PayPeriodID2"

                    paramets(0, 1) = orgztnID
                    'paramets(1, 1) = drow("EmployeeRowID")
                    paramets(2, 1) = n_PayrollSummaDateSelection.DateFromID
                    paramets(3, 1) = n_PayrollSummaDateSelection.DateToID

                    Dim newdatrow As DataRow

                    Dim dt_result As New DataTable

                    For i = 1 To 40
                        dt_result.Columns.Add("DatCol" & i)
                    Next

                    For Each drow As DataRow In datatab.Rows

                        newdatrow = dt_result.NewRow

                        newdatrow("DatCol1") = drow(1) 'EmployeeID

                        newdatrow("DatCol2") = drow(2) 'Fullname

                        newdatrow("DatCol3") = drow(3) 'PayFromDate

                        newdatrow("DatCol4") = drow(4) 'PayToDate

                        newdatrow("DatCol5") = FormatNumber(Val(drow(5)), 2) 'BasicPay






                        paramets(1, 1) = drow(0) 'Employee RowID

                        AbsTardiUTNDifOTHolipay = callProcAsDatTab(paramets,
                                                                   "GET_AbsTardiUTNDifOTHolipay")

                        Dim absentval = 0.0

                        Dim tardival = 0.0

                        Dim UTval = 0.0

                        Dim ndiffOTval = 0.0

                        Dim holidayval = 0.0

                        Dim overtimeval = 0.0

                        Dim ndiffval = 0.0


                        For Each ddrow As DataRow In AbsTardiUTNDifOTHolipay.Rows

                            If Trim(ddrow("PartNo")) = "Absent" Then

                                absentval = Val(ddrow("PayAmount"))

                                Exit For

                            ElseIf Trim(ddrow("PartNo")) = "Tardiness" Then

                                tardival = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Undertime" Then

                                UTval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Night differential OT" Then

                                ndiffOTval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Holiday pay" Then

                                holidayval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Overtime" Then

                                overtimeval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Night differential" Then

                                ndiffval = Val(ddrow("PayAmount"))

                            End If

                        Next

                        'absentval = ValNoComma(drow(""))

                        tardival = ValNoComma(drow("Tardiness"))

                        UTval = ValNoComma(drow("Undertime"))

                        ndiffOTval = ValNoComma(drow("NightDifftlOT"))

                        holidayval = ValNoComma(drow("HolidayPay"))

                        overtimeval = ValNoComma(drow("OverTime"))

                        ndiffval = ValNoComma(drow("NightDifftl"))


                        newdatrow("DatCol6") = FormatNumber(overtimeval, 2) 'Overtime

                        newdatrow("DatCol8") = FormatNumber(holidayval, 2) 'Holiday pay

                        newdatrow("DatCol9") = FormatNumber(ndiffval, 2) 'Night differential

                        newdatrow("DatCol10") = FormatNumber(absentval, 2) 'Absent

                        newdatrow("DatCol11") = FormatNumber(tardival, 2) 'Tardiness

                        'newdatrow("Column34") = FormatNumber(UTval, 2) 'Undertime


                        newdatrow("DatCol12") = 0.0 'Other income

                        newdatrow("DatCol13") = 0.0 'Adjustment

                        newdatrow("DatCol14") = FormatNumber(Val(drow(9)), 2) 'SSS

                        newdatrow("DatCol15") = FormatNumber(Val(drow(12)), 2) 'PhilHealth

                        newdatrow("DatCol16") = FormatNumber(Val(drow(14)), 2) 'PAGIBIG

                        newdatrow("DatCol17") = 0.0 'Union

                        newdatrow("DatCol18") = FormatNumber(Val(drow(10)), 2) 'Withholding tax

                        newdatrow("DatCol19") = FormatNumber(Val(drow(17)), 2) 'Loans

                        newdatrow("DatCol20") = FormatNumber(Val(drow(7)), 2) 'Net salary

                        newdatrow("DatCol21") = FormatNumber(ndiffOTval, 2) 'Night differential OT


                        AbsTardiUTNDifOTHolipay = Nothing


                        dt_result.Rows.Add(newdatrow)

                    Next

                    printReport(dt_result)

                    datatab = Nothing

                    dt_result = Nothing

                End If

            Case 7 'Employee 13th Month Pay Report

                Dim n_promptyear As New promptyear

                If n_promptyear.ShowDialog = Windows.Forms.DialogResult.OK Then

                    MsgBox(n_promptyear.YearValue)

                    Dim params(4, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramYear"

                    params(0, 1) = orgztnID
                    params(1, 1) = n_promptyear.YearValue

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_13thmonthpay")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 8 'Employee Leave Ledger

                Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

                If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

                    'MsgBox(Format(CDate(n_selectMonth.MonthValue), "MM"))

                    Dim params(4, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramDateFrom"
                    params(2, 0) = "paramDateTo"
                    params(3, 0) = "PayPeriodDateFromID"
                    params(4, 0) = "PayPeriodDateToID"

                    params(0, 1) = orgztnID
                    params(1, 1) = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "yyyy-MM-dd")
                    params(2, 1) = Format(CDate(n_PayrollSummaDateSelection.DateTostr), "yyyy-MM-dd")
                    params(3, 1) = n_PayrollSummaDateSelection.DateFromID
                    params(4, 1) = If(n_PayrollSummaDateSelection.DateToID = Nothing,
                                      n_PayrollSummaDateSelection.DateFromID,
                                      n_PayrollSummaDateSelection.DateToID)

                    date_from = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "MMMM  d, yyyy")

                    date_to = Format(CDate(n_PayrollSummaDateSelection.DateTostr), "MMMM  d, yyyy")

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_leave_ledger")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 9 'Or 11 'Employee Loan Report or Loan Report

                Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

                If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "PayDateFrom"
                    params(2, 0) = "PayDateTo"

                    params(0, 1) = orgztnID
                    params(1, 1) = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "yyyy-MM-dd")
                    params(2, 1) = Format(CDate(n_PayrollSummaDateSelection.DateTostr), "yyyy-MM-dd")

                    date_from = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "MMMM d, yyyy")

                    date_to = Format(CDate(n_PayrollSummaDateSelection.DateTostr), "MMMM d, yyyy")

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_loans")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 10 'Employee Personal Information

            Case 11 'PAGIBIG Monthly Report

                Dim n_selectMonth As New selectMonth

                If n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then

                    'MsgBox(Format(CDate(n_selectMonth.MonthValue), "MM"))

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramDate"

                    params(0, 1) = orgztnID
                    params(1, 1) = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

                    date_from = Format(CDate(n_selectMonth.MonthValue), "MMMM  yyyy")

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_PAGIBIG_Monthly")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 12 'Payroll Summary Report

                Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

                If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

                    Dim params(3, 2) As Object

                    params(0, 0) = "ps_OrganizationID"
                    params(1, 0) = "ps_PayPeriodID1"
                    params(2, 0) = "ps_PayPeriodID2"
                    params(3, 0) = "psi_undeclared"

                    params(0, 1) = orgztnID
                    params(1, 1) = n_PayrollSummaDateSelection.DateFromID
                    params(2, 1) = n_PayrollSummaDateSelection.DateToID

                    MessageBoxManager.OK = "Actual"

                    MessageBoxManager.Cancel = "Declared"

                    MessageBoxManager.Register()

                    Dim custom_prompt =
                        MessageBox.Show("Choose the payroll summary to be printed.", "Payroll Summary Data Option", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

                    If custom_prompt = Windows.Forms.DialogResult.OK Then

                        params(3, 1) = "1"

                        PayrollSummaChosenData = " (ACTUAL)"

                    Else

                        params(3, 1) = "0"

                        PayrollSummaChosenData = " (DECLARED)"

                    End If

                    MessageBoxManager.Unregister()

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "PAYROLLSUMMARY")

                    Dim AbsTardiUTNDifOTHolipay As New DataTable

                    Dim paramets(3, 2) As Object

                    paramets(0, 0) = "param_OrganizationID"
                    paramets(1, 0) = "param_EmployeeRowID"
                    paramets(2, 0) = "param_PayPeriodID1"
                    paramets(3, 0) = "param_PayPeriodID2"

                    paramets(0, 1) = orgztnID
                    'paramets(1, 1) = drow("EmployeeRowID")
                    paramets(2, 1) = n_PayrollSummaDateSelection.DateFromID
                    paramets(3, 1) = n_PayrollSummaDateSelection.DateToID

                    Dim newdatrow As DataRow

                    Dim dt_result As New DataTable

                    For i = 1 To 40
                        dt_result.Columns.Add("Column" & i)
                    Next

                    For Each drow As DataRow In datatab.Rows

                        newdatrow = dt_result.NewRow

                        newdatrow("Column1") = If(IsDBNull(drow(17)), "None", drow(17)) 'Division
                        newdatrow("Column2") = drow(11) 'Employee ID

                        newdatrow("Column3") = drow(14) & ", " & drow(12) & If(Trim(drow(13)) = "", "", ", " & drow(13)) 'Full name

                        newdatrow("Column4") = If(IsDBNull(drow(16)), "None", drow(16)) 'Position

                        newdatrow("Column20") = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "MMMM d, yyyy") &
            If(n_PayrollSummaDateSelection.DateFromstr = Nothing, "", " to " & Format(CDate(n_PayrollSummaDateSelection.DateTostr), "MMMM d, yyyy")) 'Pay period

                        newdatrow("Column21") = FormatNumber(Val(drow(0)), 2) 'Basic pay
                        newdatrow("Column22") = FormatNumber(Val(drow(1)), 2) 'Gross income
                        newdatrow("Column23") = FormatNumber(Val(drow(2)), 2) 'Net salary
                        newdatrow("Column24") = FormatNumber(Val(drow(3)), 2) 'Taxable income
                        newdatrow("Column25") = FormatNumber(Val(drow(4)), 2) 'SSS
                        newdatrow("Column26") = FormatNumber(Val(drow(5)), 2) 'Withholding tax
                        newdatrow("Column27") = FormatNumber(Val(drow(6)), 2) 'PhilHealth
                        newdatrow("Column28") = FormatNumber(Val(drow(7)), 2) 'PAGIBIG
                        newdatrow("Column29") = FormatNumber(Val(drow(8)), 2) 'Loans
                        newdatrow("Column30") = FormatNumber(Val(drow(9)), 2) 'Bonus
                        newdatrow("Column31") = FormatNumber(Val(drow(10)), 2) 'Allowance


                        paramets(1, 1) = drow("EmployeeRowID")

                        AbsTardiUTNDifOTHolipay = callProcAsDatTab(paramets,
                                                                   "GET_AbsTardiUTNDifOTHolipay")

                        Dim absentval = 0.0

                        Dim tardival = 0.0

                        Dim UTval = 0.0

                        Dim ndiffOTval = 0.0

                        Dim holidayval = 0.0

                        Dim overtimeval = 0.0

                        Dim ndiffval = 0.0


                        For Each ddrow As DataRow In AbsTardiUTNDifOTHolipay.Rows

                            If Trim(ddrow("PartNo")) = "Absent" Then

                                absentval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Tardiness" Then

                                tardival = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Undertime" Then

                                UTval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Night differential OT" Then

                                ndiffOTval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Holiday pay" Then

                                holidayval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Overtime" Then

                                overtimeval = Val(ddrow("PayAmount"))

                            ElseIf Trim(ddrow("PartNo")) = "Night differential" Then

                                ndiffval = Val(ddrow("PayAmount"))

                            End If '

                        Next


                        newdatrow("Column32") = FormatNumber(absentval, 2) 'Absent

                        'newdatrow("Column33") = FormatNumber(tardival, 2) 'Tardiness

                        'newdatrow("Column34") = FormatNumber(UTval, 2) 'Undertime

                        'newdatrow("Column35") = FormatNumber(ndiffval, 2) 'Night differential

                        'newdatrow("Column36") = FormatNumber(holidayval, 2) 'Holiday pay

                        'newdatrow("Column37") = FormatNumber(overtimeval, 2) 'Overtime

                        'newdatrow("Column38") = FormatNumber(ndiffOTval, 2) 'Night differential OT

                        '***********************************************************************************

                        'newdatrow("DatCol33") = FormatNumber(Val(drow("Absent")), 2) 'Tardiness

                        newdatrow("Column33") = FormatNumber(Val(drow("Tardiness")), 2) 'Tardiness

                        newdatrow("Column34") = FormatNumber(Val(drow("UnderTime")), 2) 'Undertime

                        newdatrow("Column35") = FormatNumber(Val(drow("NightDifftl")), 2) 'Night differential

                        newdatrow("Column36") = FormatNumber(Val(drow("HolidayPay")), 2) 'Holiday pay

                        newdatrow("Column37") = FormatNumber(Val(drow("OverTime")), 2) 'Overtime

                        newdatrow("Column38") = FormatNumber(Val(drow("NightDifftlOT")), 2) 'Night differential OT


                        AbsTardiUTNDifOTHolipay = Nothing


                        dt_result.Rows.Add(newdatrow)

                    Next





                    printReport(dt_result)

                    dt_result = Nothing

                End If

            Case 13 'PhilHealth Summary Report

                Dim n_selectMonth As New selectMonth

                If n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then

                    'MsgBox(Format(CDate(n_selectMonth.MonthValue), "MM"))

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramDate"

                    params(0, 1) = orgztnID
                    params(1, 1) = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

                    date_from = Format(CDate(n_selectMonth.MonthValue), "MMMM  yyyy")

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_PhilHealth_Monthly")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 14 'SSS Monthly Report

                Dim n_selectMonth As New selectMonth

                If n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then

                    'MsgBox(Format(CDate(n_selectMonth.MonthValue), "MM"))

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramDate"

                    params(0, 1) = orgztnID
                    params(1, 1) = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

                    date_from = Format(CDate(n_selectMonth.MonthValue), "MMMM  yyyy")

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_SSS_Monthly")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 15 'Tax Monthly Report

                Dim n_selectMonth As New selectMonth

                If n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then

                    'MsgBox(Format(CDate(n_selectMonth.MonthValue), "MM"))

                    Dim params(2, 2) As Object

                    params(0, 0) = "OrganizID"
                    params(1, 0) = "paramDateFrom"
                    params(2, 0) = "paramDateTo"

                    params(0, 1) = orgztnID
                    params(1, 1) = Format(CDate(n_selectMonth.MonthFirstDate), "yyyy-MM-dd")
                    params(2, 1) = Format(CDate(n_selectMonth.MonthLastDate), "yyyy-MM-dd")

                    date_from = Format(CDate(n_selectMonth.MonthValue), "MMMM  yyyy")

                    Dim datatab As DataTable

                    datatab = callProcAsDatTab(params,
                                               "RPT_Tax_Monthly")

                    printReport(datatab)

                    datatab = Nothing

                End If

            Case 16 'Post Employment Clearance

            Case Else

        End Select

    End Sub

    Dim rptdt As New DataTable

    Sub printReport(Optional param_dt As DataTable = Nothing)

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            With rptdt.Columns

                .Add("DatCol1") ', Type.GetType("System.Int32"))

                .Add("DatCol2") ', Type.GetType("System.String"))

                .Add("DatCol3") 'Employee Full NameS

                .Add("DatCol4") 'Gross Income

                .Add("DatCol5") 'Net Income

                .Add("DatCol6") 'Taxable salary

                .Add("DatCol7") 'Withholding Tax

                .Add("DatCol8") 'Total Allowance

                .Add("DatCol9") 'Total Loans

                .Add("DatCol10") 'Total Bonuses


                .Add("DatCol11") 'Basic Pay

                .Add("DatCol12") 'SSS Amount

                .Add("DatCol13") 'PhilHealth Amount

                .Add("DatCol14") 'PAGIBIG Amount

                .Add("DatCol15") 'Sub Total - Right side

                .Add("DatCol16") 'txthrsworkamt

                .Add("DatCol17") 'Regular hours worked

                .Add("DatCol18") 'Regular hours amount

                .Add("DatCol19") 'Overtime hours worked

                .Add("DatCol20") 'Overtime hours amount


                .Add("DatCol21") 'Night differential hours worked

                .Add("DatCol22") 'Night differential hours amount

                .Add("DatCol23") 'Night differential OT hours worked

                .Add("DatCol24") 'Night differential OT hours amount

                .Add("DatCol25") 'Total hours worked

                .Add("DatCol26") 'Undertime hours

                .Add("DatCol27") 'Undertime amount

                .Add("DatCol28") 'Late hours

                .Add("DatCol29") 'Late amount

                .Add("DatCol30") 'Leave type


                .Add("DatCol31") 'Leave count

                .Add("DatCol32")

                .Add("DatCol33")

                .Add("DatCol34") 'Allowance type

                .Add("DatCol35") 'Loan type

                .Add("DatCol36") 'Bonus type

                .Add("DatCol37") 'Allowance amount

                .Add("DatCol38") 'Loan amount

                .Add("DatCol39") 'Bonus amount

                .Add("DatCol40") '
                .Add("DatCol41") '
                .Add("DatCol42") '
                .Add("DatCol43") '
                .Add("DatCol44") '
                .Add("DatCol45") '
                .Add("DatCol46") '
                .Add("DatCol47") '
                .Add("DatCol48") '
                .Add("DatCol49") '

                .Add("DatCol50") '
                .Add("DatCol51") '
                .Add("DatCol52") '
                .Add("DatCol53") '
                .Add("DatCol54") '
                .Add("DatCol55")
                .Add("DatCol56") '
                .Add("DatCol57") '
                .Add("DatCol58") '
                .Add("DatCol59") '

                .Add("DatCol60") '

            End With

        Else

            rptdt.Rows.Clear()

        End If

        If param_dt Is Nothing Then

        Else

            Dim n_row As DataRow

            For Each drow As DataRow In param_dt.Rows

                n_row = rptdt.NewRow

                Dim ii = 0

                For Each dcol As DataColumn In param_dt.Columns

                    n_row(ii) = If(IsDBNull(drow(dcol.ColumnName)), Nothing,
                                   drow(dcol.ColumnName))

                    ii += 1

                Next

                rptdt.Rows.Add(n_row)

            Next

        End If

        Dim rptdoc = Nothing

        Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

        Dim n_listviewitem As New ListViewItem

        n_listviewitem = lvMainMenu.SelectedItems(0)

        Dim lvi_index =
            lvMainMenu.Items.IndexOf(n_listviewitem)

        Select Case lvi_index

            Case 0 'Attendance sheet

                rptdoc = New Attendance_Sheet

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("Text14")

                objText.Text = String.Concat("for the period of ", date_from, " to ", date_to)


                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")

                objText.Text = orgNam


                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgaddress")

                objText.Text = EXECQUER(
                    String.Concat("SELECT CONCAT_WS(', ', a.StreetAddress1, a.StreetAddress2, a.Barangay, a.CityTown, a.Country, a.State) `Result`",
                                  " FROM organization o LEFT JOIN address a ON a.RowID = o.PrimaryAddressID",
                                  " WHERE o.RowID=", orgztnID, ";"))


                Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                                        ",',',COALESCE(FaxNumber,'')" &
                                        ",',',COALESCE(EmailAddress,'')" &
                                        ",',',COALESCE(TINNo,''))" &
                                        " FROM organization WHERE RowID=" & orgztnID & ";")

                Dim contactdet = Split(contactdetails, ",")

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgcontactno")

                If Trim(contactdet(0).ToString) = "" Then
                Else
                    objText.Text = "Contact No. " & contactdet(0).ToString
                End If

            Case 1 'Alpha list

                ''Case 2 'Employee 13th Month Pay Report

            Case 2 'Employment Record

            Case 3 'History of salary

                ''Case 3 'Official Business filing

                ''    rptdoc = New OBFReport

                ''    objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("txtorgcontactno")

                ''    objText.Text = date_from

            Case 4 'ID number

                ''Case 4  'Loan Report

            Case 5 'Offenses

                ''Case 5 'PAGIBIG Monthly Report

            Case 6 'Payroll Ledger

                ''Case 6 'Payroll Summary Report

            Case 7 '13th month pay

                ''Case 7 'PhilHealth Monthly Report

                rptdoc = New Employee_13th_Month_Pay_Report

            Case 8 'Leave ledger

                ''Case 8 'SSS Monthly Report

            Case 9 'Loan report

                ''Case 9 'Tax Monthly Report

                rptdoc = New Loan_Report

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text14")

                objText.Text = "for the period of " & date_from & " to " & date_to & ""

            Case 10 'Personal Information

                ''Case 10 'Tardiness

                ''    rptdoc = New TardinessReport

                ''    objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("txtorgname")

                ''    objText.Text = orgNam

                ''    objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("txtorgcontactno")

                ''    objText.Text = date_from

            Case 11 'PAGIBIG

                rptdoc = New Pagibig_Monthly_Report

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

                objText.Text = "for the month of " & date_from

                InfoBalloon()

            Case 12 'Payroll summary

                rptdoc = New PayrollSumma 'PayrollSumaryRpt

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")

                objText.Text = orgNam

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgaddress")

                objText.Text = EXECQUER("SELECT CONCAT(IF(StreetAddress1 IS NULL,'',StreetAddress1)" &
                                            ",IF(StreetAddress2 IS NULL,'',CONCAT(', ',StreetAddress2))" &
                                            ",IF(Barangay IS NULL,'',CONCAT(', ',Barangay))" &
                                            ",IF(CityTown IS NULL,'',CONCAT(', ',CityTown))" &
                                            ",IF(Country IS NULL,'',CONCAT(', ',Country))" &
                                            ",IF(State IS NULL,'',CONCAT(', ',State)))" &
                                            " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" &
                                            " WHERE o.RowID=" & orgztnID & ";")

                Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                                        ",',',COALESCE(FaxNumber,'')" &
                                        ",',',COALESCE(EmailAddress,'')" &
                                        ",',',COALESCE(TINNo,''))" &
                                        " FROM organization WHERE RowID=" & orgztnID & ";")

                Dim contactdet = Split(contactdetails, ",")

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgcontactno")

                If Trim(contactdet(0).ToString) = "" Then
                Else
                    objText.Text = "Contact No. " & contactdet(0).ToString
                End If

            Case 13 'PhilHealth

                rptdoc = New Phil_Health_Monthly_Report

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

                objText.Text = "for the month of " & date_from

            Case 14 'SSS

                rptdoc = New SSS_Monthly_Report

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

                objText.Text = "for the month of " & date_from

            Case 15 'Tax

                rptdoc = New Tax_Monthly_Report

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

                objText.Text = "for the month of  " & date_from

            Case 16 'Post employement clearance

            Case 17 'Agency fee

            Case Else

        End Select

        Try

            rptdoc.SetDataSource(rptdt)

            Dim crvwr As New CrysRepForm

            crvwr.crysrepvwr.ReportSource = rptdoc

            Dim papy_string = ""

            crvwr.Text = papy_string & PayrollSummaChosenData

            crvwr.Show()

        Catch ex As Exception

        End Try

    End Sub

    Sub printReportPreviousBuild(Optional param_dt As DataTable = Nothing)

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            With rptdt.Columns

                .Add("DatCol1") ', Type.GetType("System.Int32"))

                .Add("DatCol2") ', Type.GetType("System.String"))

                .Add("DatCol3") 'Employee Full NameS

                .Add("DatCol4") 'Gross Income

                .Add("DatCol5") 'Net Income

                .Add("DatCol6") 'Taxable salary

                .Add("DatCol7") 'Withholding Tax

                .Add("DatCol8") 'Total Allowance

                .Add("DatCol9") 'Total Loans

                .Add("DatCol10") 'Total Bonuses


                .Add("DatCol11") 'Basic Pay

                .Add("DatCol12") 'SSS Amount

                .Add("DatCol13") 'PhilHealth Amount

                .Add("DatCol14") 'PAGIBIG Amount

                .Add("DatCol15") 'Sub Total - Right side

                .Add("DatCol16") 'txthrsworkamt

                .Add("DatCol17") 'Regular hours worked

                .Add("DatCol18") 'Regular hours amount

                .Add("DatCol19") 'Overtime hours worked

                .Add("DatCol20") 'Overtime hours amount


                .Add("DatCol21") 'Night differential hours worked

                .Add("DatCol22") 'Night differential hours amount

                .Add("DatCol23") 'Night differential OT hours worked

                .Add("DatCol24") 'Night differential OT hours amount

                .Add("DatCol25") 'Total hours worked

                .Add("DatCol26") 'Undertime hours

                .Add("DatCol27") 'Undertime amount

                .Add("DatCol28") 'Late hours

                .Add("DatCol29") 'Late amount

                .Add("DatCol30") 'Leave type


                .Add("DatCol31") 'Leave count

                .Add("DatCol32")

                .Add("DatCol33")

                .Add("DatCol34") 'Allowance type

                .Add("DatCol35") 'Loan type

                .Add("DatCol36") 'Bonus type

                .Add("DatCol37") 'Allowance amount

                .Add("DatCol38") 'Loan amount

                .Add("DatCol39") 'Bonus amount

                .Add("DatCol40") '
                .Add("DatCol41") '
                .Add("DatCol42") '
                .Add("DatCol43") '
                .Add("DatCol44") '
                .Add("DatCol45") '
                .Add("DatCol46") '
                .Add("DatCol47") '
                .Add("DatCol48") '
                .Add("DatCol49") '

                .Add("DatCol50") '
                .Add("DatCol51") '
                .Add("DatCol52") '
                .Add("DatCol53") '
                .Add("DatCol54") '
                .Add("DatCol55")
                .Add("DatCol56") '
                .Add("DatCol57") '
                .Add("DatCol58") '
                .Add("DatCol59") '

                .Add("DatCol60") '

            End With

        Else

            rptdt.Rows.Clear()

        End If


        If param_dt Is Nothing Then

        Else

            Dim n_row As DataRow

            For Each drow As DataRow In param_dt.Rows

                n_row = rptdt.NewRow

                Dim ii = 0

                For Each dcol As DataColumn In param_dt.Columns

                    n_row(ii) = If(IsDBNull(drow(dcol.ColumnName)), Nothing,
                                   drow(dcol.ColumnName))

                    ii += 1

                Next

                rptdt.Rows.Add(n_row)

            Next

        End If

        Dim rptdoc = Nothing

        Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

        Dim n_listviewitem As New ListViewItem

        n_listviewitem = lvMainMenu.SelectedItems(0)

        Dim lvi_index =
            lvMainMenu.Items.IndexOf(n_listviewitem)

        Select Case lvi_index

            Case 0 'Attendance sheet

                rptdoc = New Attendance_Sheet

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("Text14")

                objText.Text = "for the period of " & date_from & " to " & date_to & ""


                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")

                objText.Text = orgNam


                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgaddress")

                objText.Text = EXECQUER("SELECT CONCAT(IF(StreetAddress1 IS NULL,'',StreetAddress1)" &
                                            ",IF(StreetAddress2 IS NULL,'',CONCAT(', ',StreetAddress2))" &
                                            ",IF(Barangay IS NULL,'',CONCAT(', ',Barangay))" &
                                            ",IF(CityTown IS NULL,'',CONCAT(', ',CityTown))" &
                                            ",IF(Country IS NULL,'',CONCAT(', ',Country))" &
                                            ",IF(State IS NULL,'',CONCAT(', ',State)))" &
                                            " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" &
                                            " WHERE o.RowID=" & orgztnID & ";")


                Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                                        ",',',COALESCE(FaxNumber,'')" &
                                        ",',',COALESCE(EmailAddress,'')" &
                                        ",',',COALESCE(TINNo,''))" &
                                        " FROM organization WHERE RowID=" & orgztnID & ";")

                Dim contactdet = Split(contactdetails, ",")

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgcontactno")

                If Trim(contactdet(0).ToString) = "" Then
                Else
                    objText.Text = "Contact No. " & contactdet(0).ToString
                End If

            Case 1 'Alpha list

            Case 2 'Employee's Employment Record

                rptdoc = New Employees_Employment_Record

            Case 3 'Employee's History of Salary Increase

                rptdoc = New Employees_History_of_Salary_Increase

            Case 4 'Employee's Identification Number

            Case 5 'Employee's Offenses

            Case 6 'Employee's Payroll Ledger

                rptdoc = New Employees_Payroll_Ledger

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("Text14")

                objText.Text = "for the period of " & date_from & " to " & date_to & ""

            Case 7 'Employee 13th Month Pay Report

            Case 8 'Employee Leave Ledger

                rptdoc = New Employee_Leave_Ledger

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text14")

                objText.Text = "for the period of " & date_from & " to " & date_to & ""

            Case 9 'Employee Loan Report

                rptdoc = New Loan_Report

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text14")

                objText.Text = "for the period of " & date_from & " to " & date_to & ""

            Case 10 'Employee Personal Information

            Case 11 'PAGIBIG Monthly Report

                rptdoc = New Pagibig_Monthly_Report

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

                objText.Text = "for the month of " & date_from

                InfoBalloon()

            Case 12 'Payroll Summary Report

                rptdoc = New PayrollSumma 'PayrollSumaryRpt

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")

                objText.Text = orgNam



                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgaddress")

                objText.Text = EXECQUER("SELECT CONCAT(IF(StreetAddress1 IS NULL,'',StreetAddress1)" &
                                            ",IF(StreetAddress2 IS NULL,'',CONCAT(', ',StreetAddress2))" &
                                            ",IF(Barangay IS NULL,'',CONCAT(', ',Barangay))" &
                                            ",IF(CityTown IS NULL,'',CONCAT(', ',CityTown))" &
                                            ",IF(Country IS NULL,'',CONCAT(', ',Country))" &
                                            ",IF(State IS NULL,'',CONCAT(', ',State)))" &
                                            " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" &
                                            " WHERE o.RowID=" & orgztnID & ";")


                Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                                        ",',',COALESCE(FaxNumber,'')" &
                                        ",',',COALESCE(EmailAddress,'')" &
                                        ",',',COALESCE(TINNo,''))" &
                                        " FROM organization WHERE RowID=" & orgztnID & ";")

                Dim contactdet = Split(contactdetails, ",")

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgcontactno")

                If Trim(contactdet(0).ToString) = "" Then
                Else
                    objText.Text = "Contact No. " & contactdet(0).ToString
                End If

            Case 13 'PhilHealth Monthly Report

                rptdoc = New Phil_Health_Monthly_Report

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

                objText.Text = "for the month of " & date_from

            Case 14 'SSS Monthly Report

                rptdoc = New SSS_Monthly_Report

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

                objText.Text = "for the month of " & date_from

            Case 15 'Tax Monthly Report

                rptdoc = New Tax_Monthly_Report

                objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

                objText.Text = "for the month of  " & date_from

            Case 16 'Post Employment Clearance

                'Case 17 'BIR Form No. 2316

                '    'rptdoc = New BIR2316 'BIR2316'BIR2316_2 

            Case Else

        End Select

        'If lvMainMenu.Items(0).Selected = True Then 'Attendance sheet

        '    rptdoc = New Attendance_Sheet

        '    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("Text14")

        '    objText.Text = "for the period of " & date_from & " to " & date_to & ""


        '    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")

        '    objText.Text = orgNam


        '    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgaddress")

        '    objText.Text = EXECQUER("SELECT CONCAT(IF(StreetAddress1 IS NULL,'',StreetAddress1)" & _
        '                                ",IF(StreetAddress2 IS NULL,'',CONCAT(', ',StreetAddress2))" & _
        '                                ",IF(Barangay IS NULL,'',CONCAT(', ',Barangay))" & _
        '                                ",IF(CityTown IS NULL,'',CONCAT(', ',CityTown))" & _
        '                                ",IF(Country IS NULL,'',CONCAT(', ',Country))" & _
        '                                ",IF(State IS NULL,'',CONCAT(', ',State)))" & _
        '                                " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" & _
        '                                " WHERE o.RowID=" & orgztnID & ";")


        '    Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" & _
        '                            ",',',COALESCE(FaxNumber,'')" & _
        '                            ",',',COALESCE(EmailAddress,'')" & _
        '                            ",',',COALESCE(TINNo,''))" & _
        '                            " FROM organization WHERE RowID=" & orgztnID & ";")

        '    Dim contactdet = Split(contactdetails, ",")

        '    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgcontactno")

        '    If Trim(contactdet(0).ToString) = "" Then
        '    Else
        '        objText.Text = "Contact No. " & contactdet(0).ToString
        '    End If


        'ElseIf lvMainMenu.Items(3).Selected = True Then 'Employee's Employment Record

        '    rptdoc = New Employees_Employment_Record


        'ElseIf lvMainMenu.Items(4).Selected = True Then 'Employee's History of Salary Increase

        '    rptdoc = New Employees_History_of_Salary_Increase


        'ElseIf lvMainMenu.Items(7).Selected = True Then 'Employee's Payroll Ledger

        '    rptdoc = New Employees_Payroll_Ledger

        '    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("Text14")

        '    objText.Text = "for the period of " & date_from & " to " & date_to & ""


        'ElseIf lvMainMenu.Items(12).Selected = True Then 'Employee Leave Ledger

        '    rptdoc = New Employee_Leave_Ledger

        '    objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text14")

        '    objText.Text = "for the period of " & date_from & " to " & date_to & ""

        'ElseIf lvMainMenu.Items(16).Selected = True Then 'Loan Report

        '    rptdoc = New Loan_Report

        '    objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text14")

        '    objText.Text = "for the period of " & date_from & " to " & date_to & ""


        'ElseIf lvMainMenu.Items(17).Selected = True Then 'PAGIBIG Monthly Report

        '    rptdoc = New Pagibig_Monthly_Report

        '    objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

        '    objText.Text = "for the month of " & date_from



        'ElseIf lvMainMenu.Items(18).Selected = True Then 'Payroll Summary Report

        '    rptdoc = New PayrollSumma

        '    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")

        '    objText.Text = orgNam


        '    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgaddress")

        '    objText.Text = EXECQUER("SELECT CONCAT(IF(StreetAddress1 IS NULL,'',StreetAddress1)" & _
        '                                ",IF(StreetAddress2 IS NULL,'',CONCAT(', ',StreetAddress2))" & _
        '                                ",IF(Barangay IS NULL,'',CONCAT(', ',Barangay))" & _
        '                                ",IF(CityTown IS NULL,'',CONCAT(', ',CityTown))" & _
        '                                ",IF(Country IS NULL,'',CONCAT(', ',Country))" & _
        '                                ",IF(State IS NULL,'',CONCAT(', ',State)))" & _
        '                                " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" & _
        '                                " WHERE o.RowID=" & orgztnID & ";")


        '    Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" & _
        '                            ",',',COALESCE(FaxNumber,'')" & _
        '                            ",',',COALESCE(EmailAddress,'')" & _
        '                            ",',',COALESCE(TINNo,''))" & _
        '                            " FROM organization WHERE RowID=" & orgztnID & ";")

        '    Dim contactdet = Split(contactdetails, ",")

        '    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgcontactno")

        '    If Trim(contactdet(0).ToString) = "" Then
        '    Else
        '        objText.Text = "Contact No. " & contactdet(0).ToString
        '    End If


        'ElseIf lvMainMenu.Items(19).Selected = True Then 'PhilHealth Monthly Report

        '    rptdoc = New Phil_Health_Monthly_Report

        '    objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

        '    objText.Text = "for the month of " & date_from


        'ElseIf lvMainMenu.Items(20).Selected = True Then 'SSS Monthly Report

        '    rptdoc = New SSS_Monthly_Report

        '    objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

        '    objText.Text = "for the month of " & date_from


        'ElseIf lvMainMenu.Items(21).Selected = True Then 'Tax Monthly Report

        '    rptdoc = New Tax_Monthly_Report

        '    objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

        '    objText.Text = "for the month of " & date_from


        'End If

        rptdoc.SetDataSource(rptdt)

        Dim crvwr As New CrysRepForm

        crvwr.crysrepvwr.ReportSource = rptdoc

        Dim papy_string = ""

        crvwr.Text = papy_string & PayrollSummaChosenData

        crvwr.Show()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Button1.Enabled = False

        Dim rptdoc As ReportDocument

        rptdoc = New Crystal_Report11601C_BIR

        Static n_dt As New DataTable

        Static once As SByte = 0

        If once = 0 Then

            For ii = 1 To 120

                Dim n_dcol As New DataColumn

                n_dcol.ColumnName = "COL" & ii

                n_dt.Columns.Add(n_dcol)

            Next

        End If

        rptdoc.SetDataSource(n_dt)

        Dim crvwr As New CrysRepForm

        crvwr.crysrepvwr.ReportSource = rptdoc

        Dim papy_string = ""

        crvwr.Text = papy_string

        crvwr.Show()

        'Dim n_OverTimeForm As New OverTimeForm

        'n_OverTimeForm.Show()

        'Dim n_OBFForm As New OBFForm

        'n_OBFForm.Show()

        'Dim yeah = 1

        'Dim oops = 9

        'yeah = yeah Xor oops

        'oops = yeah Xor oops

        'yeah = yeah Xor oops

        'MsgBox(yeah & vbNewLine & _
        '       oops)

        '********************

        'Dim ii = 999

        'Dim count = 0

        'For i = 0 To ii 'ii To 0 Step -1

        '    Dim iii = i Mod 3

        '    If iii = 0 Then

        '        count += i

        '    End If

        '    iii = i Mod 5

        '    If iii = 0 Then

        '        count += i

        '    End If

        '    'ii -= 1000

        'Next

        'MsgBox(count)

        '********************

        'Dim iii = 0

        'Dim iv = 0

        'For i = 0 To Integer.MaxValue '2147483647

        '    For ii = 0 To Integer.MaxValue
        '        iii = i * ii

        '        If Trim(iii).Length = 3 Then

        '        End If

        '    Next

        '    'iv = i * ii

        '    'If Trim(iii).Length = 3 Then



        '    'End If

        'Next

        Button1.Enabled = True

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Process.Start(installerpath)
        'Open_Remote_Connection()

    End Sub

    Private Sub Open_Remote_Connection(Optional strComputer As String = "GLOBAL-A-PC\Users\Public\Downloads\Test1.txt",
                                       Optional strUsername As String = Nothing,
                                       Optional strPassword As String = Nothing)
        '//====================================================================================
        '//using NET USE to open a connection to the remote computer
        '//with the specified credentials. if we dont do this first, File.Copy will fail
        '//====================================================================================
        Dim ProcessStartInfo As New System.Diagnostics.ProcessStartInfo
        ProcessStartInfo.FileName = "net"
        ProcessStartInfo.Arguments = "use \\" & strComputer & "\c$ /USER:" & strUsername & " " & strPassword
        ProcessStartInfo.WindowStyle = ProcessWindowStyle.Maximized 'Hidden
        System.Diagnostics.Process.Start(ProcessStartInfo)

        '//============================================================================
        '//wait 2 seconds to let the above command complete or the copy will still fail
        '//============================================================================
        System.Threading.Thread.Sleep(2000)

    End Sub

    Private Sub printEmployeeProfiles()

        Dim sql_print_employee_profiles As New SQL("CALL PRINT_employee_profiles(?og_rowid);",
                                                   New Object() {orgztnID})

        Static one_value As Integer = 1

        Try

            Dim dt As New DataTable

            dt = sql_print_employee_profiles.GetFoundRows.Tables(0)

            If sql_print_employee_profiles.HasError Then

                Throw sql_print_employee_profiles.ErrorException

            Else

                Static report_name As String = "EmployeeProfiles"

                Static temp_path As String = Path.GetTempPath()

                Static temp_file As String = String.Concat(temp_path, report_name, "Report.xlsx")

                Dim newFile = New FileInfo(temp_file)

                If newFile.Exists Then
                    newFile.Delete()
                    newFile = New FileInfo(temp_file)
                End If

                Using excl_pkg = New ExcelPackage(newFile)

                    Dim worksheet As ExcelWorksheet =
                                excl_pkg.Workbook.Worksheets.Add(report_name)

                    Dim row_indx As Integer = one_value

                    Dim col_index As Integer = one_value

                    For Each dtcol As DataColumn In dt.Columns
                        worksheet.Cells(row_indx, col_index).Value = dtcol.ColumnName
                        col_index += one_value
                    Next

                    row_indx += one_value

                    For Each dtrow As DataRow In dt.Rows

                        Dim row_array = dtrow.ItemArray

                        Dim i = 0

                        For Each rowval In row_array

                            Dim excl_colrow As String =
                                        String.Concat(basic_alphabet(i),
                                                      row_indx)

                            worksheet.Cells(excl_colrow).Value = rowval

                            i += one_value

                        Next

                        row_indx += one_value

                    Next

                    worksheet.Cells.AutoFitColumns(0)

                    excl_pkg.Save()

                End Using

                Process.Start(temp_file)

            End If

        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))

        End Try

    End Sub

    Private Sub printLoanReports()

        Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

        If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

            Dim date_from, date_to As Object

            date_from = n_PayrollSummaDateSelection.DateFromstr
            date_to = n_PayrollSummaDateSelection.DateTostr

            Dim sql_print_employee_loanreports As _
                New SQL("CALL RPT_loans(?og_rowid, ?date_f, ?date_t, NULL);",
                        New Object() {orgztnID, date_from, date_to})

            Try

                Dim dt As New DataTable

                dt = sql_print_employee_loanreports.GetFoundRows.Tables(0)

                If sql_print_employee_loanreports.HasError Then

                    Throw sql_print_employee_loanreports.ErrorException

                Else

                    Dim rptdoc As New LoanReports

                    rptdoc.SetDataSource(dt)

                    Dim crvwr As New CrysRepForm

                    Dim objText As TextObject = Nothing

                    objText =
                        rptdoc.ReportDefinition.Sections(1).ReportObjects("PeriodDate")

                    objText.Text =
                        String.Concat("for the period of ",
                                      DirectCast(date_from, Date).ToShortDateString,
                                       " to ",
                                      DirectCast(date_to, Date).ToShortDateString)

                    crvwr.crysrepvwr.ReportSource = rptdoc

                    crvwr.Show()

                End If

            Catch ex As Exception

                MsgBox(getErrExcptn(ex, Me.Name))

            Finally

            End Try

        End If

    End Sub

End Class