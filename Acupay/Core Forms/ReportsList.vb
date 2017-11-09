Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports OfficeOpenXml
Imports Acupay.DS1

Public Class ReportsList

    Private basic_alphabet =
        New String() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

    Public listReportsForm As New List(Of String)

    Sub ChangeForm(ByVal Formname As Form)
        Try
            Application.DoEvents()
            Dim FName As String = Formname.Name
            Formname.TopLevel = False

            If listReportsForm.Contains(FName) Then
                Formname.Show()
                Formname.BringToFront()
            Else
                FormReports.PanelReport.Controls.Add(Formname)
                listReportsForm.Add(Formname.Name)

                Formname.Show()
                Formname.BringToFront()

                Formname.Dock = DockStyle.Fill
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub ReportsList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim a = New Object() {
            "Attendance sheet",
            "Alpha List",
            "Employee's Employment Record",
            "Employee's History of Salary Increase",
            "Employee's Identification Number",
            "Employee's Offenses",
            "Employee's Payroll Ledger",
            "Employee's 13th Month Pay Report",
            "Employee Leave Ledger",
            "Employee Loan Report",
            "Employee Personal Information",
            "PAGIBIG Monthly Report",
            "Payroll Summary Report",
            New PhilHealthReportProvider(),
            "SSS Monthly Report",
            "Tax Monthly Report",
            "Post Employment Clearance",
            "Agency Fee"
        }

        For Each strval In a
            If TypeOf strval Is String Then
                LV_Item(strval)
            ElseIf TypeOf strval Is ReportProvider Then
                Dim provider = DirectCast(strval, ReportProvider)
                Dim newListItem = New ListViewItem(provider.Name)
                newListItem.Tag = provider

                lvMainMenu.Items.Add(newListItem)
            End If
        Next
    End Sub

    Sub LV_Item(ByVal m_MenuName As String, Optional m_Icon As Integer = -1)
        If m_Icon = -1 Then
            lvMainMenu.Items.Add(m_MenuName)
        Else
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

    Private Sub lvMainMenu_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lvMainMenu.MouseDoubleClick

        date_from = Nothing

        date_to = Nothing

        If lvMainMenu.Items.Count > 0 _
            And e.Button = Windows.Forms.MouseButtons.Left Then

            report_maker()

        End If

    End Sub

    Dim PayrollSummaChosenData As String = String.Empty

    Sub report_maker()
        PayrollSummaChosenData = String.Empty
        Dim n_listviewitem As New ListViewItem

        Try

            n_listviewitem = lvMainMenu.SelectedItems(0)
        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))

            Exit Sub

        End Try

        If TypeOf n_listviewitem.Tag Is ReportProvider Then
            Dim provider = DirectCast(n_listviewitem.Tag, ReportProvider)
            provider.Run()
            Return
        End If

        Dim lvi_index =
            lvMainMenu.Items.IndexOf(n_listviewitem)

        Select Case lvi_index
            Case ReportType.AttendanceSheet
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

            Case ReportType.Alphalist
            Case ReportType.EmploymentRecord
            Case ReportType.SalaryHistory
            Case ReportType.IDNumber
            Case ReportType.Offenses
            Case ReportType.PayrollLedger
            Case ReportType.ThirteenthMonthPay
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
            Case ReportType.PagIBIG
                Dim n_selectMonth As New selectMonth

                If n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then
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
            Case ReportType.SSS

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

            Case ReportType.Tax

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

            Case ReportType.PostEmploymentClearance

            Case ReportType.AgencyFee

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

        If param_dt IsNot Nothing Then
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

            Dim a As DataTable = rptdt
            rptdoc.SetDataSource(a)

            Dim crvwr As New CrysRepForm

            crvwr.crysrepvwr.ReportSource = rptdoc

            Dim papy_string = ""

            crvwr.Text = papy_string & PayrollSummaChosenData

            crvwr.Show()
        Catch ex As Exception

        End Try

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

        Button1.Enabled = True

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

                    objText =
                        rptdoc.ReportDefinition.Sections(1).ReportObjects("txtOrganizationName")

                    objText.Text = orgNam.ToUpper

                    crvwr.crysrepvwr.ReportSource = rptdoc

                    crvwr.Show()

                End If
            Catch ex As Exception

                MsgBox(getErrExcptn(ex, Me.Name))
            Finally

            End Try

        End If

    End Sub

    Public Enum ReportType As Integer
        AttendanceSheet = 0
        Alphalist = 1
        EmploymentRecord = 2
        SalaryHistory = 3
        IDNumber = 4
        Offenses = 5
        PayrollLedger = 6
        ThirteenthMonthPay = 7
        LeaveLedger = 8
        LoanReport = 9
        PersonalInformation = 10
        PagIBIG = 11
        PayrollSummary = 12
        PhilHealth = 13
        SSS = 14
        Tax = 15
        PostEmploymentClearance = 16
        AgencyFee = 17
    End Enum

End Class
