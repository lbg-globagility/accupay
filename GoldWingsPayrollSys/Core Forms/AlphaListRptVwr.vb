Imports Femiani.Forms.UI.Input
Imports CrystalDecisions.CrystalReports.Engine

Public Class AlphaListRptVwr

#Region "INITIALIZE VARIABLE"

    Public TaxDateFrom As Object = Nothing

    Public TaxDateTo As Object = Nothing

    Dim AlphaListDataTable As New DataTable

    Dim AnnualizedWithholdingTax As New DataTable

    Dim getGrossCompensation As New DataTable

#End Region

    Sub New(Optional date_From As Object = Nothing, _
            Optional date_To As Object = Nothing)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        For Each obj As Control In GroupBox1.Controls
            If TypeOf obj Is TextBox Then

                OjbAssignNoContextMenu(obj)

            End If
        Next

        For Each obj As Control In GroupBox2.Controls
            If TypeOf obj Is TextBox Then

                OjbAssignNoContextMenu(obj)

            End If
        Next

        For Each obj As Control In GroupBox3.Controls
            If TypeOf obj Is TextBox Then

                OjbAssignNoContextMenu(obj)

            End If
        Next

        For Each obj As Control In GroupBox4.Controls
            If TypeOf obj Is TextBox Then

                OjbAssignNoContextMenu(obj)

            End If
        Next

        If date_From = String.Empty _
            And date_To = String.Empty Then

            Me.Text = orgNam & " - BIR Form No. 2316"

        Else
            
            TaxDateFrom = date_From

            TaxDateTo = date_To

            Me.Text = orgNam & " - BIR Form No. 2316 for period of " & _
                Format(CDate(TaxDateFrom), "MMM dd, yyyy") & _
                " to " & Format(CDate(TaxDateTo), "MMM dd, yyyy")

            txtForTheYear.Text = CDate(TaxDateTo).Year


            TextBox1.Text = Format(CDate(TaxDateFrom), "MM")

            TextBox2.Text = Format(CDate(TaxDateFrom), "dd")


            TextBox3.Text = Format(CDate(TaxDateTo), "MM")

            TextBox4.Text = Format(CDate(TaxDateTo), "dd")

        End If

    End Sub

    Sub AlphaListRptVwr()

        Dim params(3, 1) As Object

        params(0, 0) = "OrganizID"
        params(1, 0) = "AnnualDateFrom"
        params(2, 0) = "AnnualDateTo"
        params(3, 0) = "IsActual"

        params(0, 1) = orgztnID
        params(1, 1) = Format(CDate(TaxDateFrom), "yyyy-MM-dd")
        params(2, 1) = Format(CDate(TaxDateTo), "yyyy-MM-dd")
        params(3, 1) = "0"

        AnnualizedWithholdingTax = _
            callProcAsDatTab(params, _
                             "RPT_AnnualizedWithholdingTax")


        Dim paramet(2, 1) As Object

        paramet(0, 0) = "OrganizID"
        paramet(1, 0) = "LastDateOfFinancialYear"
        paramet(2, 0) = "FirstDateOfFinancialYear"

        paramet(0, 1) = orgztnID
        paramet(1, 1) = Format(CDate(TaxDateTo), "yyyy-MM-dd")
        paramet(2, 1) = Format(CDate(TaxDateFrom), "yyyy-MM-dd")

        'Dim the_lastdate = EXECQUER("SELECT `RPT_LastDateOfFinancialYear`();")
        'paramet(1, 1) = Format(CDate(the_lastdate), "yyyy-MM-dd")
        'paramet(2, 1) = Format(CDate(the_lastdate), "yyyy-MM-dd")

        getGrossCompensation = _
            callProcAsDatTab(paramet, _
                             "RPT_getGrossCompensation")


        Dim employee_RowID As New AutoCompleteStringCollection

        enlistTheLists("SELECT e.RowID FROM" & _
                        " employee e" & _
                        " WHERE e.OrganizationID='" & orgztnID & "'" & _
                        " ORDER BY e.LastName DESC;", _
                        employee_RowID) 'RowID


        For Each strval In employee_RowID

            Dim total_GrossCompen = Val(0)

            Dim total_OverTime = Val(0)

            Dim less_SSS_Philhealth_PagIbig = Val(0)

            Dim less_TaxableAllowance = Val(0)

            Dim less_PersonalExemption = Val(0)

            Dim less_AddtnallExemption = Val(0)

            Dim less_DeMinimisExemption = Val(0)

            Dim netTaxableCompensation = Val(0)


            Dim sel_AnnualizedWithholdingTax = AnnualizedWithholdingTax.Compute("SUM(AllowanceYesTax)", "EmployeeID=" & strval)

            Dim sel_AnnualizedWithholdingTax1 = AnnualizedWithholdingTax.Select("EmployeeID=" & strval)


            Dim sel_getGrossCompensation = getGrossCompensation.Select("EmployeeID=" & strval)


            Dim first_tax = Val(0)

            Dim plusOverExcess = Val(0)

            Dim taxdueValue = Val(0)


            For Each drow In sel_getGrossCompensation

                total_GrossCompen = ValNoComma(drow("TotalGrossCompensation")) '+ Val(drow("TotalAllowance"))

                less_SSS_Philhealth_PagIbig = ValNoComma(drow("EmployeeContributionAmount")) _
                                            + ValNoComma(drow("EmployeeShare")) _
                                            + ValNoComma(drow("HDMFAmount"))

                less_TaxableAllowance = ValNoComma(sel_AnnualizedWithholdingTax) 'ddrow("AllowanceYesTax")

                For Each ddrow In sel_AnnualizedWithholdingTax1

                    total_OverTime = ValNoComma(ddrow("TotalOverTime"))

                    less_PersonalExemption = ValNoComma(ddrow("PersonalExemption"))

                    less_AddtnallExemption = ValNoComma(ddrow("AdditionalPersonalExemption"))

                    less_DeMinimisExemption = ValNoComma(ddrow("DeMinimisExemption"))

                    Exit For

                Next

                total_GrossCompen += total_OverTime

                total_GrossCompen += less_TaxableAllowance

                total_GrossCompen = total_GrossCompen - less_SSS_Philhealth_PagIbig

                'total_GrossCompen = total_GrossCompen - less_TaxableAllowance

                total_GrossCompen = total_GrossCompen - less_PersonalExemption

                total_GrossCompen = total_GrossCompen - less_AddtnallExemption

                total_GrossCompen = total_GrossCompen - less_DeMinimisExemption


                netTaxableCompensation = total_GrossCompen

                Dim tax_due As New DataTable

                tax_due = retAsDatTbl("SELECT *" & _
                                      " FROM taxdue " & _
                                      " WHERE " & netTaxableCompensation & "" & _
                                      " BETWEEN Over AND NotOver;")

                For Each d_row As DataRow In tax_due.Rows

                    first_tax = ValNoComma(d_row("AmountRate"))

                    plusOverExcess = (ValNoComma(d_row("NotOver")) - netTaxableCompensation) * ValNoComma(d_row("AdditionalPercent"))

                    taxdueValue = first_tax + plusOverExcess

                    Exit For

                Next

            Next

            'tax_amount = (Val(drowtax("ExemptionAmount")) + _
            '             ((emptaxabsal - Val(drowtax("TaxableIncomeFromAmount"))) * Val(drowtax("ExemptionInExcessAmount"))) _
            '             )

        Next

    End Sub

    Private Sub AlphaListRptVwr_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        Dim prompt = MessageBox.Show("Do you want close this ?", "Exit Alphalist", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

        If prompt = MsgBoxResult.Yes Then

            e.Cancel = False

            If crvAlphaList.ReportSource IsNot Nothing Then

                crvAlphaList.ReportSource.Dispose()

            End If

        Else

            e.Cancel = True

        End If

    End Sub

    Dim collectEmployeeID = Nothing

    Private Sub AlphaListRptVwr_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        collectEmployeeID = EXECQUER("SELECT GROUP_CONCAT(EmployeeID) FROM employee WHERE OrganizationID='" & orgztnID & "';")

        bgwEmployeeID.RunWorkerAsync()

        AlphaListRptVwr()

        bgReport.RunWorkerAsync()

    End Sub

    Private Sub AutoCompleteTextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles AutoCompleteTextBox1.KeyPress
        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then
            btnSearch_Click(sender, e)
        End If

    End Sub

    Private Sub AutoCompleteTextBox1_TextChanged(sender As Object, e As EventArgs) Handles AutoCompleteTextBox1.TextChanged

    End Sub

    Private Sub bgwEmployeeID_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwEmployeeID.DoWork

        'Dim EmployeeIDCollection As New AutoCompleteStringCollection

        'enlistTheLists("SELECT EmployeeID FROM employee WHERE OrganizationID='" & orgztnID & "';", _
        '                EmployeeIDCollection)

        'For Each strval As String In EmployeeIDCollection

        '    AutoCompleteTextBox1.Items.Add(New AutoCompleteEntry(strval, StringToArray(strval)))

        'Next

        Dim arr_collectEmployeeID = Split(collectEmployeeID, ",")

        For Each strval In arr_collectEmployeeID

            AutoCompleteTextBox1.Items.Add(New AutoCompleteEntry(strval, StringToArray(strval)))

        Next

    End Sub

    Private Sub bgwEmployeeID_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwEmployeeID.ProgressChanged

    End Sub

    Private Sub bgwEmployeeID_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwEmployeeID.RunWorkerCompleted

        AutoCompleteTextBox1.Enabled = True

    End Sub

    Dim emp_RowIDValue = Nothing

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        If dt_catchtable IsNot Nothing Then

            Dim emp_RowID = _
                EXECQUER("SELECT" & _
                         " CONCAT(" & _
                         "e.RowID" & _
                         ",';',e.FirstName" & _
                         ",';',INITIALS(e.MiddleName,'.','1')" & _
                         ",';',e.LastName" & _
                         ",';',po.PositionName" & _
                         ",';',CONCAT(e.EmployeeType,' salary'))" & _
                         " FROM employee e" & _
                         " LEFT JOIN position po ON po.RowID=e.PositionID" & _
                         " WHERE e.EmployeeID='" & AutoCompleteTextBox1.Text.Trim & "'" & _
                         " AND e.OrganizationID='" & orgztnID & "';")

            If emp_RowID = String.Empty Then

                emp_RowIDValue = String.Empty

                txtFName.Text = Nothing

                txtEmpID.Text = Nothing

            Else

                pnlEmployee.AutoScrollPosition = New Point(0, 0)

                'emp_RowID = _
                '    EXECQUER("SELECT RowID" & _
                '             " FROM employee" & _
                '             " WHERE EmployeeID='" & AutoCompleteTextBox1.Text.Trim & "'" & _
                '             " AND OrganizationID='" & orgztnID & "';")

                Dim empDataRow = Split(emp_RowID, ";")

                emp_RowIDValue = empDataRow(0)

                txtFName.Text = empDataRow(1) & If(empDataRow(2) = Nothing, Nothing, " " & empDataRow(2)) & " " & empDataRow(3)

                txtEmpID.Text = "ID# " & AutoCompleteTextBox1.Text.Trim & ", " & empDataRow(4) & ", " & empDataRow(5)

            End If

        Else

            emp_RowIDValue = String.Empty

            txtFName.Text = Nothing

            txtEmpID.Text = Nothing

        End If

    End Sub

    Dim rptdoc As New ReportDocument

    Dim dt_catchtable As New DataTable

    Private Sub bgReport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgReport.DoWork

        If crvAlphaList.ReportSource IsNot Nothing Then

            crvAlphaList.ReportSource.Dispose()

        End If

        rptdoc = New Alphalist


        Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

        objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text1")

        objText.Text = CDate(TaxDateTo).Year


        objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text2")

        Dim MMDD_From = Format(CDate(TaxDateFrom), "MMdd")

        objText.Text = MMDD_From


        objText = rptdoc.ReportDefinition.Sections(1).ReportObjects("Text3")

        Dim MMDD_To = Format(CDate(TaxDateTo), "MMdd")

        objText.Text = MMDD_To




        Dim all_employee As New DataTable

        Dim org_details As New DataTable

        Dim emp_prevEmployer As New DataTable

        Dim emp_dependent As New DataTable


        all_employee = retAsDatTbl("CALL `RPT_AlphaListemployee`('" & orgztnID & "');")

        emp_dependent = retAsDatTbl("CALL `RPT_AlphaListempdependents`('" & orgztnID & "');")

        emp_prevEmployer = retAsDatTbl("CALL `RPT_AlphaListemppreviousemployer`('" & orgztnID & "');")

        org_details = retAsDatTbl("SELECT IFNULL(og.TINNo,'') TINNo" & _
                                  ",og.Name" & _
                                  ",'ü' AS MainEmployer" & _
                                  ",ad.StreetAddress1" & _
                                  ",ad.StreetAddress2" & _
                                  ",ad.Barangay" & _
                                  ",ad.CityTown" & _
                                  ",ad.State" & _
                                  ",ad.Country" & _
                                  ",ad.ZipCode" & _
                                  ",og.RDOCode AS oRDOCode" & _
                                  ",og.ZIPCode AS oZIPCode" & _
                                  ",SUM(1)" & _
                                  " FROM organization og" & _
                                  " LEFT JOIN address ad ON ad.RowID=og.PrimaryAddressID" & _
                                  " WHERE og.RowID='" & orgztnID & "';")



        If dt_catchtable.Columns.Count = 0 Then

            For i = 1 To 70

                dt_catchtable.Columns.Add("COL" & i) 'this the main table for alphalist

                dt_catchChanges.Columns.Add("COL" & i) 'this the table that catches the changes applied by the user and will be use to implement changes to the main table

            Next

        Else

            dt_catchtable.Rows.Clear()

        End If


        Dim n_row As DataRow


        For Each e_row As DataRow In all_employee.Rows

            n_row = dt_catchtable.NewRow

            n_row("COL1") = e_row("EmployeeID") '

            n_row("COL5") = e_row("TINNo")

            n_row("COL6") = e_row("LastName") & e_row("FirstName") & e_row("MiddleName")

            n_row("COL7") = org_details.Rows(0)("oRDOCode")

            n_row("COL8") = e_row("HomeAddress")

            'n_row("COL9") = String.Empty

            n_row("COL10") = e_row("HomeAddress")

            'n_row("COL11") = "1900"

            'n_row("COL12") = "516 Swan st. Anakpawis Brgy. San Jaun Cainta, Rizal"

            'n_row("COL13") = "1900"

            n_row("COL14") = e_row("BirthDate")

            n_row("COL15") = e_row("MobilePhone")

            n_row("COL16") = e_row("ExemptionStatus")



            Dim sel_emp_dependent = emp_dependent.Select("ParentEmployeeID = " & e_row("RowID"))

            Dim collect_dependent = String.Empty

            Dim collect_dependentBDate = String.Empty

            For Each ed_row As DataRow In sel_emp_dependent

                collect_dependent = ed_row("FirstName") & ed_row("MiddleName") & ed_row("LastName") & vbNewLine

                collect_dependentBDate = ed_row("Birthdate") & vbNewLine

            Next

            n_row("COL18") = collect_dependent

            n_row("COL19") = collect_dependentBDate






            'this is the be the BASIS for numeric values




            Dim Seldt_catchChanges = dt_catchChanges.Select("COL1 = '" & e_row("RowID") & "'")

            If Seldt_catchChanges.Count <> 0 Then

                n_row("COL17") = Seldt_catchChanges(0)("COL17")

                n_row("COL22") = Seldt_catchChanges(0)("COL22")

            Else

                n_row("COL17") = "ü        ü"

                n_row("COL22") = "ü"

            End If


            '************PART IV-B*************START

            '****A****

            Dim sel_getGrossCompensation = getGrossCompensation.Select("EmployeeID = " & e_row("RowID"))

            If n_row("COL22") = "ü" Then 'Employee is a minimum wage earner

                'AnnualizedWithholdingTax'getGrossCompensation

                If e_row("EmployeeType").ToString = "Daily" Then


                    'A. NON-TAXABLE/EXEMPT COMPENSATION INCOME

                    Dim val_basicpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalDayPay)", "EmployeeID = " & e_row("RowID")))

                    Dim val_ndiffotpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(NightDiffOT)", "EmployeeID = " & e_row("RowID")))

                    Dim value_holiday = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(HolidayPay)", "EmployeeID = " & e_row("RowID")))
                    
                    Dim value_otpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(OvertimePay)", "EmployeeID = " & e_row("RowID")))

                    Dim val_ndiffpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(NightDiffPay)", "EmployeeID = " & e_row("RowID")))

                    val_basicpay = ValNoComma(val_basicpay) -
                                    (ValNoComma(value_otpay) + ValNoComma(value_holiday) + ValNoComma(val_ndiffpay) + ValNoComma(val_ndiffotpay))

                    n_row("COL44") = Format(ValNoComma(val_basicpay), "###0.00") 'Basic pay

                    n_row("COL45") = Format(ValNoComma(value_holiday), "###0.00") 'Holiday Pay'

                    n_row("COL46") = Format(ValNoComma(value_otpay), "###0.00") 'Overtime Pay

                    n_row("COL47") = Format(ValNoComma(val_ndiffpay), "###0.00") 'Night Differential Pay

                    n_row("COL48") = Nothing 'Hazard Pay

                    Dim val_tmpay = If(sel_getGrossCompensation.Count = 0, 0, sel_getGrossCompensation(0)("ThirteenthMonthPay"))

                    n_row("COL49") = ValNoComma(val_tmpay) '13th month pay

                    n_row("COL50") = Nothing 'De Minimis & Other Benefits
                    
                    Dim val_sss = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalEmpSSS)", "EmployeeID = " & e_row("RowID")))

                    Dim val_phh = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalEmpPhilhealth)", "EmployeeID = " & e_row("RowID")))

                    Dim val_hdmf = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalEmpHDMF)", "EmployeeID = " & e_row("RowID")))

                    Dim val_col51 = ValNoComma(val_sss) + ValNoComma(val_phh) + ValNoComma(val_hdmf)

                    n_row("COL51") = Format(ValNoComma(val_col51), "###0.00") 'SSS, GSIS, PHIC & Pag-ibig Contribs, & Union Dues (Employee share only)

                    n_row("COL52") = Nothing 'Salaries & Other Forms of Compensation

                    n_row("COL53") = Nothing 'Total Non-Taxable/Exempt Compensation Income

                    n_row("COL54") = Nothing 'Basic Salary

                ElseIf e_row("EmployeeType").ToString = "Fixed" _
                    Or e_row("EmployeeType").ToString = "Monthly" Then


                    'A. NON-TAXABLE/EXEMPT COMPENSATION INCOME

                    Dim val_tmpay = If(sel_getGrossCompensation.Count = 0, 0, sel_getGrossCompensation(0)("ThirteenthMonthPay"))

                    Dim val_ndiffotpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(NightDiffOT)", "EmployeeID = " & e_row("RowID")))

                    Dim value_holiday = If(AnnualizedWithholdingTax.Rows.Count =0,0,AnnualizedWithholdingTax.Compute("SUM(HolidayPay)", "EmployeeID = " & e_row("RowID")))
                    
                    Dim value_otpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(OvertimePay)", "EmployeeID = " & e_row("RowID")))
                    
                    Dim val_ndiffpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(NightDiffPay)", "EmployeeID = " & e_row("RowID")))

                    val_tmpay = ValNoComma(val_tmpay) -
                        (ValNoComma(value_otpay) + ValNoComma(value_holiday) + ValNoComma(val_ndiffpay) + ValNoComma(val_ndiffotpay))

                    n_row("COL44") = Format(ValNoComma(val_tmpay), "###0.00") 'Basic Pay

                    n_row("COL45") = Format(ValNoComma(value_holiday), "###0.00") 'Holiday Pay'

                    n_row("COL46") = Format(ValNoComma(value_otpay), "###0.00") 'Overtime Pay

                    n_row("COL47") = Format(ValNoComma(val_ndiffpay), "###0.00") 'Night Differential Pay

                    n_row("COL48") = Nothing 'Hazard Pay
                    
                    n_row("COL49") = If(sel_getGrossCompensation.Count = 0, 0, ValNoComma(sel_getGrossCompensation(0)("ThirteenthMonthPay"))) '13th month pay

                    n_row("COL50") = Nothing 'De Minimis & Other Benefits
                    
                    Dim val_sss = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalEmpSSS)", "EmployeeID = " & e_row("RowID")))

                    Dim val_phh = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalEmpPhilhealth)", "EmployeeID = " & e_row("RowID")))

                    Dim val_hdmf = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalEmpHDMF)", "EmployeeID = " & e_row("RowID")))

                    Dim val_col51 = ValNoComma(val_sss) + ValNoComma(val_phh) + ValNoComma(val_hdmf)

                    n_row("COL51") = Format(ValNoComma(val_col51), "###0.00") 'SSS, GSIS, PHIC & Pag-ibig Contribs, & Union Dues (Employee share only)

                    n_row("COL52") = Nothing 'Salaries & Other Forms of Compensation

                    n_row("COL53") = Nothing 'Total Non-Taxable/Exempt Compensation Income

                    n_row("COL54") = Nothing 'Basic Salary


                    'B. TAXABLE COMPENSATION INCOME REGULAR

                End If

            Else '                              'Employee is a ABOVE minimum wage earner


                'A. NON-TAXABLE/EXEMPT COMPENSATION INCOME

                Dim val_basicpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalDayPay)", "EmployeeID = " & e_row("RowID")))

                Dim val_ndiffotpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(NightDiffOT)", "EmployeeID = " & e_row("RowID")))

                Dim value_holiday = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(HolidayPay)", "EmployeeID = " & e_row("RowID")))

                Dim value_otpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(OvertimePay)", "EmployeeID = " & e_row("RowID")))

                Dim val_ndiffpay = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(NightDiffPay)", "EmployeeID = " & e_row("RowID")))

                val_basicpay = ValNoComma(val_basicpay) -
                                (ValNoComma(value_otpay) + ValNoComma(value_holiday) + ValNoComma(val_ndiffpay) + ValNoComma(val_ndiffotpay))

                n_row("COL44") = Format(ValNoComma(val_basicpay), "###0.00") 'Basic Pay

                n_row("COL45") = Format(ValNoComma(value_holiday), "###0.00") 'Holiday Pay

                n_row("COL46") = Format(ValNoComma(value_otpay), "###0.00") 'Overtime Pay

                n_row("COL47") = Format(ValNoComma(val_ndiffpay), "###0.00") 'Night Differential Pay

                n_row("COL48") = Nothing 'Hazard Pay

                n_row("COL49") = ValNoComma(If(sel_getGrossCompensation.Count = 0, 0, sel_getGrossCompensation(0)("ThirteenthMonthPay"))) '13th month pay

                n_row("COL50") = Nothing 'De Minimis & Other Benefits
                
                Dim val_sss = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalEmpSSS)", "EmployeeID = " & e_row("RowID")))

                Dim val_phh = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalEmpPhilhealth)", "EmployeeID = " & e_row("RowID")))

                Dim val_hdmf = If(AnnualizedWithholdingTax.Rows.Count = 0, 0, AnnualizedWithholdingTax.Compute("SUM(TotalEmpHDMF)", "EmployeeID = " & e_row("RowID")))

                Dim val_col51 = ValNoComma(val_sss) + ValNoComma(val_phh) + ValNoComma(val_hdmf)

                n_row("COL51") = Format(ValNoComma(val_col51), "###0.00") 'SSS, GSIS, PHIC & Pag-ibig Contribs, & Union Dues (Employee share only)

                n_row("COL52") = Nothing 'Salaries & Other Forms of Compensation

                n_row("COL53") = Nothing 'Total Non-Taxable/Exempt Compensation Income

                n_row("COL54") = Nothing 'Basic Salary


                'B. TAXABLE COMPENSATION INCOME REGULAR

            End If

            '****A****

            '************PART IV-B*************FINISH









            n_row("COL23") = org_details.Rows(0)("TINNo")

            n_row("COL24") = org_details.Rows(0)("Name")

            n_row("COL25") = org_details.Rows(0)("StreetAddress1") & _
                org_details.Rows(0)("StreetAddress2") & _
                org_details.Rows(0)("Barangay") & _
                org_details.Rows(0)("CityTown") & _
                org_details.Rows(0)("State") & _
                org_details.Rows(0)("Country")

            n_row("COL26") = org_details.Rows(0)("oZIPCode")

            n_row("COL27") = org_details.Rows(0)("MainEmployer")


            Dim sel_emp_prevEmployer = emp_prevEmployer.Select("EmployeeID = " & e_row("RowID"))

            For Each epe_row As DataRow In sel_emp_prevEmployer

                n_row("COL28") = epe_row("TINNo")

                n_row("COL29") = epe_row("Name")

                n_row("COL30") = epe_row("BusinessAddress")

                'n_row("COL31") = org_details.Rows(0)("MainEmployer")
            Next

            
            dt_catchtable.Rows.Add(n_row)

        Next











        'If dt_catchChanges Is Nothing Then

        'Else

        '    For Each e_row As DataRow In all_employee.Rows

        '        Dim Seldt_catchChanges = dt_catchChanges.Select("COL1" & e_row("RowID"))

        '    Next

        'End If

        
    End Sub

    Private Sub bgReport_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgReport.ProgressChanged

    End Sub

    Private Sub bgReport_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgReport.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox("Error: " & vbNewLine & e.Error.Message)

        ElseIf e.Cancelled Then
            MsgBox("Background work cancelled.", _
                   MsgBoxStyle.Exclamation)
        Else

        End If

        rptdoc.SetDataSource(dt_catchtable)

        crvAlphaList.ReportSource = rptdoc

        tsbtnReload.Enabled = True

    End Sub

    Private Sub txtForTheYear_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtForTheYear.KeyPress, _
                                                                                            TextBox1.KeyPress, _
                                                                                            TextBox2.KeyPress, _
                                                                                            TextBox3.KeyPress, _
                                                                                            TextBox4.KeyPress, _
                                                                                            txtPrevEmployerZipCode.KeyPress

        e.Handled = TrapNumKey(Asc(e.KeyChar))

    End Sub

    Private Sub txtForTheYear_TextChanged(sender As Object, e As EventArgs) Handles txtForTheYear.TextChanged

    End Sub

    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMinWageDay.KeyPress

        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtMinWageDay.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtMinWageDay.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If

    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles txtMinWageDay.TextChanged

    End Sub

    Private Sub TextBox6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMinWageMonth.KeyPress

        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtMinWageMonth.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtMinWageMonth.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If

    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles txtMinWageMonth.TextChanged

    End Sub

    Private Sub TextBox12_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTaxabIncomePrevEmployer.KeyPress

        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtTaxabIncomePrevEmployer.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtTaxabIncomePrevEmployer.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If

    End Sub

    Private Sub TextBox12_TextChanged(sender As Object, e As EventArgs) Handles txtTaxabIncomePrevEmployer.TextChanged

    End Sub

    Private Sub TextBox11_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPaidOnHealth.KeyPress

        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then
                onedot += 1
                If onedot >= 2 Then
                    If txtPaidOnHealth.Text.Contains(".") Then
                        e.Handled = True
                        onedot = 2
                    Else
                        e.Handled = False
                        onedot = 0
                    End If
                Else
                    If txtPaidOnHealth.Text.Contains(".") Then
                        e.Handled = True
                    Else
                        e.Handled = False
                    End If
                End If
            Else
                e.Handled = False
            End If

        Else
            e.Handled = True
        End If

    End Sub

    Private Sub TextBox11_TextChanged(sender As Object, e As EventArgs) Handles txtPaidOnHealth.TextChanged

    End Sub

    Dim dt_catchChanges As New DataTable

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        'ü

        If dt_catchChanges IsNot Nothing _
            And emp_RowIDValue <> String.Empty Then


            Dim sel_dt_catchtable = dt_catchChanges.Select("COL1 = '" & emp_RowIDValue & "'")


            If sel_dt_catchtable.Count <> 0 Then
                
                If rdbYes.Checked Then

                    sel_dt_catchtable(0)("COL17") = "ü"

                End If

                If rdbNo.Checked Then
                    
                    sel_dt_catchtable(0)("COL17") = "         ü"

                End If


                If rdbYesMinimum.Checked Then

                    sel_dt_catchtable(0)("COL22") = "ü"

                End If

                If rdbNoMinimum.Checked Then

                    sel_dt_catchtable(0)("COL22") = String.Empty

                End If

            Else

                Dim Newdt_catchChanges As DataRow

                Newdt_catchChanges = dt_catchChanges.NewRow

                Newdt_catchChanges("COL1") = emp_RowIDValue

                If rdbYes.Checked Then

                    Newdt_catchChanges("COL17") = "ü"

                End If

                If rdbNo.Checked Then

                    Newdt_catchChanges("COL17") = "         ü"

                End If


                If rdbYesMinimum.Checked Then

                    Newdt_catchChanges("COL22") = "ü"

                End If

                If rdbNoMinimum.Checked Then

                    Newdt_catchChanges("COL22") = String.Empty

                End If


                dt_catchChanges.Rows.Add(Newdt_catchChanges)

            End If

        End If

    End Sub

    Private Sub btnDiscard_Click(sender As Object, e As EventArgs) Handles btnDiscard.Click

    End Sub

    Private Sub tsbtnReload_Click(sender As Object, e As EventArgs) Handles tsbtnReload.Click

        If tsbtnReload.Enabled And bgReport.IsBusy = False Then

            tsbtnReload.Enabled = False

            bgReport.RunWorkerAsync()

        End If

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.F5 Then

            tsbtnReload_Click(tsbtnReload, New EventArgs)

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub bgLoadAlphaList_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgLoadAlphaList.DoWork

    End Sub

    Private Sub crvAlphaList_Load(sender As Object, e As EventArgs) Handles crvAlphaList.Load

    End Sub

End Class