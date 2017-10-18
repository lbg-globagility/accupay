Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Threading.Tasks
Imports log4net

Public Class PayStub

    Private _logger As ILog = LogManager.GetLogger("PayrollLogger")

    Public q_employee As String = "SELECT e.RowID," &
        "e.EmployeeID 'Employee ID'," &
        "e.FirstName 'First Name'," &
        "e.MiddleName 'Middle Name'," &
        "e.LastName 'Last Name'," &
        "e.Surname," &
        "e.Nickname," &
        "e.MaritalStatus 'Marital Status'," &
        "COALESCE(e.NoOfDependents,0) 'No. Of Dependents'," &
        "DATE_FORMAT(e.Birthdate,'%m-%d-%Y') 'Birthdate'," &
        "DATE_FORMAT(e.Startdate,'" & custom_mysqldateformat & "') AS Startdate," &
        "e.JobTitle 'Job Title'," &
        "COALESCE(pos.PositionName,'') 'Position'," &
        "e.Salutation," &
        "e.TINNo 'TIN'," &
        "e.SSSNo 'SSS No.'," &
        "e.HDMFNo 'PAGIBIG No.'," &
        "e.PhilHealthNo 'PhilHealth No.'," &
        "e.WorkPhone 'Work Phone No.'," &
        "e.HomePhone 'Home Phone No.'," &
        "e.MobilePhone 'Mobile Phone No.'," &
        "e.HomeAddress 'Home address'," &
        "e.EmailAddress 'Email address'," &
        "IF(e.Gender='M','Male','Female') 'Gender'," &
        "e.EmploymentStatus 'Employment Status'," &
        "IFNULL(pf.PayFrequencyType,'') 'Pay Frequency'," &
        "e.UndertimeOverride," &
        "e.OvertimeOverride," &
        "COALESCE(pos.RowID,'pos.RowID') 'PositionID'" &
        ",IFNULL(e.PayFrequencyID,'') 'PayFrequencyID'" &
        ",e.EmployeeType" &
        ",e.LeaveBalance" &
        ",e.SickLeaveBalance" &
        ",e.MaternityLeaveBalance" &
        ",e.LeaveAllowance" &
        ",e.SickLeaveAllowance" &
        ",e.MaternityLeaveAllowance" &
        ",e.LeavePerPayPeriod" &
        ",e.SickLeavePerPayPeriod" &
        ",e.MaternityLeavePerPayPeriod" &
        ",COALESCE(fstat.RowID,3) 'fstatRowID'" &
        ",'' 'Image'" &
        ",DATE_FORMAT(e.Created,'%m-%d-%Y') 'Creation Date'," &
        "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) 'Created by'," &
        "COALESCE(DATE_FORMAT(e.LastUpd,'%m-%d-%Y'),'') 'Last Update'," &
        "(SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))  FROM user WHERE RowID=e.LastUpdBy) 'LastUpdate by'" &
        " " &
        "FROM employee e " &
        "LEFT JOIN user u ON e.CreatedBy=u.RowID " &
        "LEFT JOIN position pos ON e.PositionID=pos.RowID " &
        "LEFT JOIN payfrequency pf ON e.PayFrequencyID=pf.RowID " &
        "LEFT JOIN filingstatus fstat ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents " &
        "WHERE e.OrganizationID=" & orgztnID

    '",COALESCE(LEFT(Image,256),'') 'Image'" &

    Public current_year As String = CDate(dbnow).Year

    Dim new_conn As New MySqlConnection
    Dim new_cmd As New MySqlCommand

    Dim pagination As Integer = 0

    Dim orgpayfreqID As String

    Dim isorgPHHdeductsched As SByte = 0
    Dim isorgSSSdeductsched As SByte = 0
    Dim isorgHDMFdeductsched As SByte = 0
    Dim isorgWTaxdeductsched As SByte = 0

    Dim strPHHdeductsched As String = String.Empty
    Dim strSSSdeductsched As String = String.Empty
    Dim strHDMFdeductsched As String = String.Empty

    Dim govdeducsched As New AutoCompleteStringCollection

    Dim allowfreq As New AutoCompleteStringCollection

    Dim prodPartNo As New DataTable

    Dim employeepicture As New DataTable

    Dim viewID As Integer = Nothing

    Dim employeecolumnname As New AutoCompleteStringCollection

    'josh
    Dim currentTotal As Double

    Dim annualUnusedLeaves As DataTable

    Dim n_VeryFirstPayPeriodIDOfThisYear As Object = Nothing

    Private _loanSchedules As ICollection(Of PayrollSys.LoanSchedule)

    Private _loanTransactions As ICollection(Of PayrollSys.LoanTransaction)

    Public paypFrom As String = Nothing
    Public paypTo As String = Nothing
    Public paypRowID As String = Nothing
    Public paypPayFreqID As String = Nothing
    Public isEndOfMonth As String = 0

    Const max_count_per_page As Integer = 1

    Dim currentEmployeeID As String = Nothing

    Dim LastFirstMidName As String = Nothing

    Dim employee_dattab As New DataTable

    Dim esal_dattab As New DataTable

    Dim etent_holidaypay As New DataTable

    Dim etent_totdaypay As New DataTable

    Dim emp_bonusDaily As New DataTable

    Dim notax_bonusDaily As New DataTable

    Dim emp_bonusMonthly As New DataTable

    Dim notax_bonusMonthly As New DataTable

    Dim emp_bonusOnce As New DataTable

    Dim notax_bonusOnce As New DataTable

    Dim emp_allowanceDaily As New DataTable

    Dim notax_allowanceDaily As New DataTable

    Dim emp_allowanceMonthly As New DataTable

    Dim notax_allowanceMonthly As New DataTable

    Dim emp_allowanceOnce As New DataTable

    Dim notax_allowanceOnce As New DataTable

    Private _fixedTaxableMonthlyAllowances As DataTable

    Private _fixedNonTaxableMonthlyAllowances As DataTable

    Dim emp_bonusSemiM As New DataTable

    Dim notax_bonusSemiM As New DataTable

    Dim emp_allowanceSemiM As New DataTable

    Dim notax_allowanceSemiM As New DataTable

    Dim emp_allowanceWeekly As New DataTable

    Dim notax_allowanceWeekly As New DataTable

    Dim emp_bonusWeekly As New DataTable

    Dim notax_bonusWeekly As New DataTable

    Dim prev_empTimeEntry As New DataTable

    Dim numofdaypresent As New DataTable

    Dim emp_TardinessUndertime As New DataTable

    Private _withholdingTaxTable As DataTable
    Private _filingStatuses As DataTable

    Public numofweekdays As Integer

    Public numofweekends As Integer

    Public withthirteenthmonthpay As SByte = 0

    Dim empthirteenmonthtable As New DataTable

    Dim dtemployeefirsttimesalary As New DataTable

    Private thread_max As Integer = 5

    Const max_rec_perpage As Integer = 1

    Dim emp_list_batcount As Integer = 0

    <Obsolete>
    Public Prior_PayPeriodID As String = String.Empty

    <Obsolete>
    Public Current_PayPeriodID As String = String.Empty

    <Obsolete>
    Public Next_PayPeriodID As String = String.Empty

    <Obsolete>
    Public paypSSSContribSched As String = Nothing

    <Obsolete>
    Public paypPhHContribSched As String = Nothing

    <Obsolete>
    Public paypHDMFContribSched As String = Nothing

    Dim pause_process_message = String.Empty

    Dim IsUserPressEnterToSearch As Boolean = False

    Dim dtempalldistrib As New DataTable

    Dim rptdattab As New DataTable

    Dim PayrollSummaChosenData As String = String.Empty

    Dim selectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim unselectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim quer_empPayFreq = ""

    Dim dtJosh As DataTable
    Dim da As New MySqlDataAdapter()

    Dim dtprintAllPaySlip As New DataTable

    Dim rptdocAll As New rptAllDecUndecPaySlip

    Dim multi_threads(0) As Thread

    Dim array_bgwork(1) As System.ComponentModel.BackgroundWorker

    Dim payroll_emp_count As Integer = 0

    Dim progress_precentage As Integer = 0

    Dim indxStartBatch As Integer = 0

    Property VeryFirstPayPeriodIDOfThisYear As Object
        Get
            Return n_VeryFirstPayPeriodIDOfThisYear
        End Get
        Set(value As Object)
            n_VeryFirstPayPeriodIDOfThisYear = value
        End Set
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)

        SplitContainer1.SplitterWidth = 6

        Dim dgvVisibleRows = dgvpayper.Columns.Cast(Of DataGridViewColumn).Where(Function(ii) ii.Visible = True)

        Dim scrollbarwidth = 19

        Dim mincolwidth As Integer = (dgvpayper.Width - (dgvpayper.RowHeadersWidth + scrollbarwidth)) / dgvVisibleRows.Count

        For Each dgvcol In dgvVisibleRows
            dgvcol.Width = mincolwidth
            dgvcol.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
        MyBase.OnLoad(e)

    End Sub

    Private Sub PayStub_EnabledChanged(sender As Object, e As EventArgs) Handles Me.EnabledChanged

        Dim _bool As Boolean = Me.Enabled

        ''###### CONTROL DISABLER ######

        PayrollForm.MenuStrip1.Enabled = _bool
        MDIPrimaryForm.Showmainbutton.Enabled = _bool
        ToolStrip1.Enabled = _bool

        Panel5.Enabled = _bool

        MDIPrimaryForm.systemprogressbar.Value = 0
        MDIPrimaryForm.systemprogressbar.Visible = CBool(Not _bool)

    End Sub

    Private Sub PayStub_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'customTabControl3.DrawMode = TabDrawMode.Normal
        'customTabControl3.DisplayStyle = TabStyle.Rounded

        'dbconn()

        viewID = VIEW_privilege("Employee Pay Slip", orgztnID)

        new_conn.ConnectionString = db_connectinstring 'conn.ConnectionString

        'new_conn.Open()

        VIEW_payperiodofyear()

        'loademployees()
        'loademployee()

        dgvpayper.Focus()

        orgpayfreqID = EXECQUER("SELECT COALESCE(PayFrequencyID,'') FROM organization WHERE RowID=" & orgztnID & ";")

        ''dgvpayper_SelectionChanged(sender, e)

        enlistTheLists("SELECT DisplayValue FROM listofval WHERE `Type`='Government deduction schedule' AND Active='Yes' ORDER BY OrderBy;", govdeducsched)

        Dim dattabl_deductsched As New DataTable
        'dattabl_deductsched = retAsDatTbl("SELECT IF(COALESCE(PhilhealthDeductionSchedule,'" & govdeducsched.Item(1).ToString & "') = '" & govdeducsched.Item(1).ToString & "',1,0) 'PhilhealthDeductionSchedule'" &
        '                                  ",IF(COALESCE(SSSDeductionSchedule,'" & govdeducsched.Item(1).ToString & "') = '" & govdeducsched.Item(1).ToString & "',1,0) 'SSSDeductionSchedule'" &
        '                                  ",IF(COALESCE(PagIbigDeductionSchedule,'" & govdeducsched.Item(1).ToString & "') = '" & govdeducsched.Item(1).ToString & "',1,0) 'PagIbigDeductionSchedule'" &
        '                                  " FROM organization WHERE RowID=" & orgztnID & ";")

        dattabl_deductsched = retAsDatTbl("SELECT COALESCE(PhilhealthDeductionSchedule,'" & govdeducsched.Item(2).ToString & "') 'PhilhealthDeductionSchedule'" &
                                          ",COALESCE(SSSDeductionSchedule,'" & govdeducsched.Item(2).ToString & "') 'SSSDeductionSchedule'" &
                                          ",COALESCE(PagIbigDeductionSchedule,'" & govdeducsched.Item(2).ToString & "') 'PagIbigDeductionSchedule'" &
                                          " FROM organization WHERE RowID=" & orgztnID & ";")

        'For Each drown As DataRow In dattabl_deductsched.Rows
        '    isorgPHHdeductsched = CSByte(drown("PhilhealthDeductionSchedule"))
        '    isorgSSSdeductsched = CSByte(drown("SSSDeductionSchedule"))
        '    isorgHDMFdeductsched = CSByte(drown("PagIbigDeductionSchedule"))
        '    Exit For
        'Next

        Dim strdeductsched = dattabl_deductsched.Rows(0)("PhilhealthDeductionSchedule")

        'PhilHealth deduction schedule

        If govdeducsched.Item(0).ToString = strdeductsched Then 'End of the month

            isorgPHHdeductsched = 0

        ElseIf govdeducsched.Item(1).ToString = strdeductsched Then 'First half

            isorgPHHdeductsched = 2

        ElseIf govdeducsched.Item(2).ToString = strdeductsched Then 'Per pay period

            isorgPHHdeductsched = 1

        End If

        strdeductsched = dattabl_deductsched.Rows(0)("SSSDeductionSchedule")

        'SSS deduction schedule

        If govdeducsched.Item(0).ToString = strdeductsched Then 'End of the month

            isorgSSSdeductsched = 0

        ElseIf govdeducsched.Item(1).ToString = strdeductsched Then 'First half

            isorgSSSdeductsched = 2

        ElseIf govdeducsched.Item(2).ToString = strdeductsched Then 'Per pay period

            isorgSSSdeductsched = 1

        End If

        strdeductsched = dattabl_deductsched.Rows(0)("PagIbigDeductionSchedule")

        'PAGIBIG deduction schedule

        If govdeducsched.Item(0).ToString = strdeductsched Then 'End of the month

            isorgHDMFdeductsched = 0

        ElseIf govdeducsched.Item(1).ToString = strdeductsched Then 'First half

            isorgHDMFdeductsched = 2

        ElseIf govdeducsched.Item(2).ToString = strdeductsched Then 'Per pay period

            isorgHDMFdeductsched = 1

        End If

        linkPrev.Text = "← " & (current_year - 1)
        linkNxt.Text = (current_year + 1) & " →"

        prodPartNo = retAsDatTbl("SELECT PartNo FROM product WHERE OrganizationID=" & orgztnID & ";")

        If dgvemployees.RowCount <> 0 Then
            dgvemployees.Item("EmployeeID", 0).Selected = 1
        End If

        enlistTheLists("SELECT DisplayValue FROM listofval WHERE Type='Allowance Frequency' AND Active='Yes' ORDER BY OrderBy;",
                       allowfreq) 'Daily'Monthly'One time'Semi-monthly'Weely

        ''tsbtnSearch.Image = ImageList1.Images(0)

        '        enlistTheLists("SELECT column_name" &
        '" FROM information_schema.`COLUMNS`" &
        '" WHERE table_schema = '" & sys_db & "'" &
        '" AND table_name = 'employee';", _
        '                       employeecolumnname)

        '        For Each strval In employeecolumnname
        '            MsgBox(strval.ToString)
        '        Next

        Dim formuserprivilege = position_view_table.Select("ViewID = " & viewID)

        If formuserprivilege.Count = 0 Then

            tsbtngenpayroll.Visible = 0
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    tsbtngenpayroll.Visible = 0

                    'dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        tsbtngenpayroll.Visible = 0
                    Else
                        tsbtngenpayroll.Visible = 1
                    End If

                    'If drow("Deleting").ToString = "N" Then
                    '    tsbtndel.Visible = 0
                    'Else
                    '    tsbtndel.Visible = 1
                    'End If

                    'If drow("Updates").ToString = "N" Then
                    '    dontUpdate = 1
                    'Else
                    '    dontUpdate = 0
                    'End If

                End If

            Next

        End If

        'Josh
        cboProducts.ValueMember = "ProductID"
        cboProducts.DisplayMember = "ProductName"

        cboProducts.DataSource = New SQLQueryToDatatable("SELECT RowID AS 'ProductID', Name AS 'ProductName', Category FROM product WHERE Category IN ('Allowance Type', 'Bonus', 'Adjustment Type')" &
                                                  " AND OrganizationID='" & orgztnID & "';").ResultTable

        'cboProducts.DataSource = New SQLQueryToDatatable("SELECT RowID AS 'ProductID', Name AS 'ProductName', Category FROM product WHERE Category IN ('Allowance Type', 'Bonus')" &
        '                                          " AND OrganizationID='" & orgztnID & "';").ResultTable

        dgvAdjustments.AutoGenerateColumns = False

    End Sub

    Sub VIEW_payperiodofyear(Optional param_Date As Object = Nothing)

        'Dim params(2, 2) As Object

        'params(0, 0) = "payp_OrganizationID"
        'params(1, 0) = "param_Date"
        'params(2, 0) = "FormatNumber"

        'params(0, 1) = orgztnID
        'params(1, 1) = If(param_Date = Nothing, DBNull.Value, param_Date & "-01-01")
        'params(2, 1) = 0

        'EXEC_VIEW_PROCEDURE(params, _
        '                    "VIEW_payperiodofyear", _
        '                    dgvpayper)
        Dim hasValue = (MDIPrimaryForm.systemprogressbar.Value > 0)
        If hasValue Then
            For i = 0 To 5
                RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
            Next
        End If
        Dim date_param = If(param_Date = Nothing, "NULL", "'" & param_Date & "-01-01'")
        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("CALL VIEW_payperiodofyear('" & orgztnID & "'," & date_param & ",'0');")
        Dim catchdt As New DataTable : catchdt = n_SQLQueryToDatatable.ResultTable
        dgvpayper.Rows.Clear()
        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvpayper.Rows.Add(row_array)
        Next
        catchdt.Dispose()
        If hasValue Then
            AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
        End If
    End Sub

    Sub loademployees(Optional searchquery As String = Nothing)

        If searchquery = Nothing Then
            Dim param(2, 2) As Object

            param(0, 0) = "e_OrganizationID"
            param(1, 0) = "pagination"

            param(0, 1) = orgztnID
            param(1, 1) = pagination

            filltable(dgvemployees, "VIEW_employee", param, 1)
            'filltable(dgvEmp, q_employee)
            'filltable(dgvEmp, "VIEW_employee", "e_OrganizationID", 1, 1)
        Else 'q_employee &
            filltable(dgvemployees, searchquery) ' & " ORDER BY e.RowID DESC")
        End If

        Static x As SByte = 0

        If x = 0 Then
            x = 1

            With dgvemployees
                .Columns("RowID").Visible = False
                .Columns("UndertimeOverride").Visible = False
                .Columns("OvertimeOverride").Visible = False
                .Columns("PositionID").Visible = False
                .Columns("PayFrequencyID").Visible = False

                .Columns("LeaveBalance").Visible = False
                .Columns("SickLeaveBalance").Visible = False
                .Columns("MaternityLeaveBalance").Visible = False

                .Columns("LeaveAllowance").Visible = False
                .Columns("SickLeaveAllowance").Visible = False
                .Columns("MaternityLeaveAllowance").Visible = False

                .Columns("LeavePerPayPeriod").Visible = False
                .Columns("SickLeavePerPayPeriod").Visible = False
                .Columns("MaternityLeavePerPayPeriod").Visible = False

                .Columns("fstatRowID").Visible = False
                .Columns("Image").Visible = False

                'For Each r As DataGridViewRow In .Rows
                '    empcolcount = 0
                '    For Each c As DataGridViewColumn In .Columns
                '        If c.Visible Then
                '            If TypeOf r.Cells(c.Index).Value Is Byte() Then
                '                Simple.Add("")
                '            Else
                '                Simple.Add(CStr(r.Cells(c.Index).Value))
                '            End If
                '            empcolcount += 1
                '        End If
                '    Next
                'Next

            End With

        End If

    End Sub

    Sub loademployee(Optional q_empsearch As String = Nothing)
        Dim full_query As String = String.Empty
        If q_empsearch = Nothing Then
            full_query = (q_employee & " ORDER BY e.RowID DESC LIMIT " & pagination & ",20;") ', dgvemployees
        Else
            If pagination <= 0 Then
                pagination = 0
            End If

            full_query = (q_employee & q_empsearch & " ORDER BY e.RowID DESC LIMIT " & pagination & ",20;") ', dgvemployees', Simple)

        End If
        Dim catchdt As New DataTable
        'catchdt = New SQLQueryToDatatable(full_query).ResultTable

        Dim param_array = New Object() {orgztnID,
                                        tsSearch.Text,
                                        pagination}

        Dim n_ReadSQLProcedureToDatatable As New _
            ReadSQLProcedureToDatatable("SEARCH_employee_paystub",
                                        param_array)

        catchdt = n_ReadSQLProcedureToDatatable.ResultTable

        dgvemployees.Rows.Clear()
        'For Each drow As DataRow In catchdt.Rows
        '    Dim row_array = drow.ItemArray
        '    dgvemployees.Rows.Add(row_array)
        'Next
        PopulateDGVEmployee(catchdt)
        Static x As SByte = 0

        If x = 0 Then
            x = 1

            With dgvemployees
                .Columns("RowID").Visible = False
                .Columns("UndertimeOverride").Visible = False
                .Columns("OvertimeOverride").Visible = False
                .Columns("PositionID").Visible = False
                .Columns("PayFreqID").Visible = False

                .Columns("LeaveBal").Visible = False
                .Columns("SickBal").Visible = False
                .Columns("MaternBal").Visible = False

                .Columns("LeaveAllow").Visible = False
                .Columns("SickAllow").Visible = False
                .Columns("MaternAllow").Visible = False

                .Columns("Leavepayp").Visible = False
                .Columns("Sickpayp").Visible = False
                .Columns("Maternpayp").Visible = False

                .Columns("fstatRowID").Visible = False
                .Columns("Image").Visible = False

                'For Each r As DataGridViewRow In .Rows
                '    empcolcount = 0
                '    For Each c As DataGridViewColumn In .Columns
                '        If c.Visible Then
                '            If TypeOf r.Cells(c.Index).Value Is Byte() Then
                '                Simple.Add("")
                '            Else
                '                Simple.Add(CStr(r.Cells(c.Index).Value))
                '            End If
                '            empcolcount += 1
                '        End If
                '    Nexte
                'Next

            End With
            If dgvemployees.RowCount > 0 Then
                dgvemployees.Item("EmployeeID", 0).Selected = True
            End If
            employeepicture = New SQLQueryToDatatable("SELECT RowID,Image FROM employee WHERE Image IS NOT NULL AND OrganizationID=" & orgztnID & ";").ResultTable 'retAsDatTbl("SELECT RowID,Image FROM employee WHERE OrganizationID=" & orgztnID & ";")

        End If

    End Sub

    Private Sub dgvpayper_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvpayper.SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
        If dgvpayper.RowCount > 0 Then
            With dgvpayper.CurrentRow

                paypPayFreqID = .Cells("Column4").Value

                paypRowID = .Cells("Column1").Value
                paypFrom = MYSQLDateFormat(CDate(.Cells("Column2").Value)) 'Format(CDate(.Cells("Column2").Value), "yyyy-MM-dd")

                'If .Cells("Column3").Value = Nothing Then
                '    paypTo = Format(CDate(dbnow), "yyyy-MM-dd")

                '    If DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo)) < 0 Then
                '        Dim payptoval = EXECQUER("SELECT LAST_DAY('" & paypFrom & "');")

                '        paypTo = Format(CDate(dbnow), "yyyy-MM-dd")
                '    ElseIf DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo)) < 0 Then
                '        paypTo = Format(CDate(dbnow), "yyyy-MM-dd")
                '    End If
                'Else
                paypTo = MYSQLDateFormat(CDate(.Cells("Column3").Value)) 'Format(CDate(.Cells("Column3").Value), "yyyy-MM-dd")
                'End If

                isEndOfMonth = Trim(.Cells("Column14").Value)

                Dim date_diff = DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo))

                numofweekdays = 0

                For i = 0 To date_diff

                    Dim DayOfWeek = CDate(paypFrom).AddDays(i)

                    If DayOfWeek.DayOfWeek = 0 Then 'System.DayOfWeek.Sunday
                        numofweekends += 1

                    ElseIf DayOfWeek.DayOfWeek = 6 Then 'System.DayOfWeek.Saturday
                        numofweekends += 1
                    Else
                        numofweekdays += 1

                    End If

                Next

                'For Each tsitem As ToolStripItem In tstrip.Items

                '    If tsitem.Text = Trim(.Cells("Column12").Value) Then
                '        tsitem.PerformClick()
                '        PayFreq_Changed(tsitem, New EventArgs)
                '        Exit For
                '    End If

                'Next

            End With
            If tsSearch.Text.Trim.Length > 0 Then
                tsbtnSearch_Click(sender, New EventArgs)
            Else
                dgvemployees_SelectionChanged(dgvemployees, New EventArgs)
            End If
        Else

            numofweekdays = 0

            paypRowID = Nothing
            paypFrom = Nothing
            paypTo = Nothing
            isEndOfMonth = 0
            paypPayFreqID = String.Empty

            dgvemployees_SelectionChanged(sender, e)

        End If
        AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
        'RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        current_year = current_year - 1

        linkPrev.Text = "← " & (current_year - 1)
        linkNxt.Text = (current_year + 1) & " →"

        VIEW_payperiodofyear(current_year)

        AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        'AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        dgvpayper_SelectionChanged(sender, e)
    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged
        'RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        current_year = current_year + 1

        linkNxt.Text = (current_year + 1) & " →"
        linkPrev.Text = "← " & (current_year - 1)

        VIEW_payperiodofyear(current_year)

        AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        'AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        dgvpayper_SelectionChanged(sender, e)
    End Sub

    Private Sub First_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked, Prev.LinkClicked,
                                                                                                Nxt.LinkClicked, Last.LinkClicked,
                                                                                                LinkLabel4.LinkClicked, LinkLabel3.LinkClicked,
                                                                                                LinkLabel2.LinkClicked, LinkLabel1.LinkClicked

        ''RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "First" Or sendrname = "LinkLabel1" Then
            pagination = 0
        ElseIf sendrname = "Prev" Or sendrname = "LinkLabel2" Then
            'If pagination - max_count_per_page < 0 Then
            '    pagination = 0
            'Else
            '    pagination -= max_count_per_page
            'End If

            Dim modcent = pagination Mod max_count_per_page

            If modcent = 0 Then

                pagination -= max_count_per_page
            Else

                pagination -= modcent

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sendrname = "Nxt" Or sendrname = "LinkLabel4" Then

            Dim modcent = pagination Mod max_count_per_page

            If modcent = 0 Then
                pagination += max_count_per_page
            Else
                pagination -= modcent

                pagination += max_count_per_page

            End If
        ElseIf sendrname = "Last" Or sendrname = "LinkLabel3" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / " & max_count_per_page & " FROM employee WHERE OrganizationID=" & orgztnID & ";"))

            Dim remender = lastpage Mod 1

            pagination = (lastpage - remender) * max_count_per_page

            If pagination - max_count_per_page < max_count_per_page Then
                'pagination = 0

            End If

            'pagination = If(lastpage - max_count_per_page >= max_count_per_page , _
            '                lastpage - max_count_per_page , _
            '                lastpage)

        End If

        'loademployees()
        'If Trim(tsSearch.Text) = "" Then
        'Else
        'End If

        Dim pay_freqString = String.Empty

        For Each ctrl In tstrip.Items
            If TypeOf ctrl Is ToolStripButton Then

                If DirectCast(ctrl, ToolStripButton).BackColor =
                        Color.FromArgb(194, 228, 255) Then

                    pay_freqString =
                            DirectCast(ctrl, ToolStripButton).Text

                    Exit For
                Else
                    Continue For
                End If
            Else
                Continue For
            End If

        Next

        quer_empPayFreq = " AND pf.PayFrequencyType='" & pay_freqString & "' "

        loademployee(quer_empPayFreq)
        '# ############################# #
        'dgvemployees_SelectionChanged(sender, e)

        ''AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        dgvemployees_SelectionChanged(sender, e)

        AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

    Private Sub dgvemployees_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvemployees.KeyDown
        'Dim dgv_DataGridViewCellEventArgs As New DataGridViewCellEventArgs(EmployeeID.Index, _
        '                                                                   dgvemployees.CurrentRow.Index)
        'dgvemployees_CellClick(sender, dgv_DataGridViewCellEventArgs)
    End Sub

    Private Sub dgvemployees_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvemployees.SelectionChanged

        btnSaveAdjustments.Enabled = False

        Dim employeetype = ""

        Static sameEmpID As String = 0

        dgvemployees.Tag = Nothing

        If dgvemployees.RowCount > 0 Then 'And dgvemployees.CurrentRow IsNot Nothing
            With dgvemployees.CurrentRow

                'If .Cells("RowID").Value <> sameEmpID Then
                employeetype = Trim(.Cells("EmployeeType").Value)
                sameEmpID = .Cells("RowID").Value

                dgvemployees.Tag = .Cells("RowID").Value

                'empPix = employeepix.Select("RowID=" & .Cells("RowID").Value)
                'For Each drow In empPix
                '    EmployeeImage = ConvByteToImage(DirectCast(drow("Image"), Byte()))
                '    makefileGetPath(drow("Image"))
                'Next

                txtFName.Text = .Cells("FirstName").Value

                'txtFName.Text = txtFName.Text & If(.Cells("MiddleName").Value = Nothing, _
                '                                         "", _
                '                                         " " & StrConv(Microsoft.VisualBasic.Left(.Cells("MiddleName").Value.ToString, 1), _
                'VbStrConv.ProperCase) & ".")

                Dim addtlWord = Nothing

                If .Cells("MiddleName").Value = Nothing Then
                Else

                    Dim midNameTwoWords = Split(.Cells("MiddleName").Value.ToString, " ")

                    addtlWord = " "

                    For Each s In midNameTwoWords

                        addtlWord &= (StrConv(Microsoft.VisualBasic.Left(s, 1), VbStrConv.ProperCase) & ".")

                    Next

                    txtFName.Text &= addtlWord

                End If

                LastFirstMidName = .Cells("LastName").Value & ", " & .Cells("FirstName").Value &
                    If(Trim(addtlWord) = Nothing, "", If(Trim(addtlWord) = ".", "", ", " & addtlWord))

                txtFName.Text = txtFName.Text & " " & .Cells("LastName").Value

                txtFName.Text = txtFName.Text & If(.Cells("Surname").Value = Nothing,
                                                         "",
                                                         "-" & StrConv(.Cells("Surname").Value,
                                                                       VbStrConv.ProperCase))

                'Microsoft.VisualBasic.Left(.Cells("Surname").Value.ToString, 1)

                currentEmployeeID = .Cells("EmployeeID").Value

                txtEmpID.Text = "ID# " & .Cells("EmployeeID").Value &
                            If(.Cells("Position").Value = Nothing,
                                                               "",
                                                               ", " & .Cells("Position").Value) &
                            If(.Cells("EmployeeType").Value = Nothing,
                                                               "",
                                                               ", " & .Cells("EmployeeType").Value & " salary")

                'If IsDBNull(.Cells("Image").Value) Then
                '    pbEmpPicChk.Image = Nothing
                'Else
                '    pbEmpPicChk.Image = ConvByteToImage(DirectCast(.Cells("Image").Value, Byte()))
                'End If

                pbEmpPicChk.Image = Nothing
                Try
                    Dim selemppic = employeepicture.Select("RowID=" & ValNoComma(.Cells("RowID").Value))

                    For Each drow In selemppic
                        If IsDBNull(drow("Image")) Then
                            pbEmpPicChk.Image = Nothing
                        Else
                            pbEmpPicChk.Image = ConvByteToImage(DirectCast(drow("Image"), Byte()))
                        End If
                        Exit For
                    Next
                Catch ex As Exception

                End Try

                'End If

                Gender_Label(.Cells("Gender").Value)

                txtvlallow.Text = .Cells("LeaveAllow").Value
                txtslallow.Text = .Cells("SickAllow").Value
                txtmlallow.Text = .Cells("MaternAllow").Value

                txtvlpayp.Text = .Cells("Leavepayp").Value
                txtslpayp.Text = .Cells("Sickpayp").Value
                txtmlpayp.Text = .Cells("Maternpayp").Value

                txttotabsentamt.Text = "0.00"
                txttottardiamt.Text = "0.00"
                txttotutamt.Text = "0.00"

                If ValNoComma(paypRowID) > 0 Then
                    '# ####################################################### #
                    Try
                        For Each txtbxctrl In SplitContainer1.Panel2.Controls.OfType(Of TextBox).ToList()
                            txtbxctrl.Text = "0.00"
                        Next
                    Catch ex As Exception
                        MsgBox(getErrExcptn(ex, Me.Name))
                    End Try
                    If tabEarned.SelectedIndex = 0 Then
                        TabPage1_Enter1(TabPage1, New EventArgs)
                    ElseIf tabEarned.SelectedIndex = 1 Then
                        TabPage4_Enter(TabPage4, New EventArgs)
                    End If
                Else
                    dgvpaystub.Rows.Clear()

                    txtgrosssal.Text = ""
                    txtnetsal.Text = ""
                    txttaxabsal.Text = ""

                    dgvpaystub.Rows.Clear()
                    dgvpaystubitem.Rows.Clear()
                    dgvempsal.Rows.Clear()
                    dgvetent.Rows.Clear()
                    dgvpaystubitm.Rows.Clear()
                    dgvempbon.Rows.Clear()
                    dgvLoanList.Rows.Clear()
                    dgvempallowance.Rows.Clear()
                    Try
                        For Each txtbxctrl In SplitContainer1.Panel2.Controls.OfType(Of TextBox).ToList()
                            txtbxctrl.Text = "0.00"
                        Next
                    Catch ex As Exception
                        MsgBox(getErrExcptn(ex, Me.Name))
                    End Try
                    If tabEarned.SelectedIndex = 0 Then
                        TabPage1_Enter1(TabPage1, New EventArgs)
                    ElseIf tabEarned.SelectedIndex = 1 Then
                        TabPage4_Enter(TabPage4, New EventArgs)
                    End If
                End If

            End With
        Else
            sameEmpID = -1

            LastFirstMidName = Nothing

            currentEmployeeID = Nothing

            pbEmpPicChk.Image = Nothing
            txtFName.Text = ""
            txtEmpID.Text = ""

            txtBasicPay.Text = ""

            txttotreghrs.Text = ""
            txttotregamt.Text = ""

            txtOvertimeHours.Text = ""
            txtOvertimePay.Text = ""

            txtNightDiffHours.Text = ""
            txtNightDiffPay.Text = ""

            txtNightDiffOvertimeHours.Text = ""
            txtNightDiffOvertimePay.Text = ""

            txtHolidayHours.Text = ""
            txtHolidayPay.Text = ""

            txtRegularHours.Text = ""
            txtRegularPay.Text = ""

            lblSubtotal.Text = ""

            txtemptotallow.Text = ""

            txtgrosssal.Text = ""

            txtvlbal.Text = ""
            txtslbal.Text = ""
            txtmlbal.Text = ""

            txtvlallow.Text = ""
            txtslallow.Text = ""
            txtmlallow.Text = ""

            txtvlpayp.Text = ""
            txtslpayp.Text = ""
            txtmlpayp.Text = ""

            txttotabsent.Text = ""
            txttotabsentamt.Text = ""

            txttottardi.Text = ""
            txttottardiamt.Text = ""

            txttotut.Text = ""
            txttotutamt.Text = ""

            lblsubtotmisc.Text = ""

            txtempsss.Text = ""
            txtempphh.Text = ""
            txtemphdmf.Text = ""

            txtemptotloan.Text = ""
            txtemptotbon.Text = ""

            txttaxabsal.Text = ""
            txtempwtax.Text = ""
            txtnetsal.Text = ""

            dgvpaystub.Rows.Clear()
            dgvpaystubitem.Rows.Clear()
            dgvempsal.Rows.Clear()
            dgvetent.Rows.Clear()
            dgvpaystubitm.Rows.Clear()
            dgvempbon.Rows.Clear()
            dgvLoanList.Rows.Clear()
            dgvempallowance.Rows.Clear()

        End If

    End Sub

    Sub VIEW_paystub(Optional EmpID As Object = Nothing,
                     Optional PayPeriodID As Object = Nothing)

        Dim params(2, 2) As Object

        params(0, 0) = "paystb_OrganizationID"
        params(1, 0) = "paystb_EmployeeID"
        params(2, 0) = "paystb_PayPeriodID"

        params(0, 1) = orgztnID
        params(1, 1) = EmpID
        params(2, 1) = PayPeriodID

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_paystub",
                             dgvpaystub, , 1)

    End Sub

    Sub VIEW_paystubitem(ByVal paystitm_PayStubID As Object)

        Dim params(2, 2) As Object

        params(0, 0) = "paystitm_PayStubID"

        params(0, 1) = paystitm_PayStubID

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_paystubitem",
                             dgvpaystubitm, , 1)

    End Sub

    Sub VIEW_specificemployeesalary(Optional esal_EmployeeID As Object = Nothing,
                                    Optional esal_Date As Object = Nothing)

        Dim params(2, 2) As Object

        params(0, 0) = "esal_EmployeeID"
        params(1, 0) = "esal_OrganizationID"
        params(2, 0) = "esal_Date"

        params(0, 1) = esal_EmployeeID
        params(1, 1) = orgztnID
        params(2, 1) = esal_Date

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_specificemployeesalary",
                             dgvempsal, , 1)

    End Sub

    Sub VIEW_employeetimeentry_SUM(Optional etent_EmployeeID As Object = Nothing,
                                   Optional etent_Date As Object = Nothing,
                                   Optional etent_DateTo As Object = Nothing)

        Dim params(3, 2) As Object

        params(0, 0) = "etent_OrganizationID"
        params(1, 0) = "etent_EmployeeID"
        params(2, 0) = "etent_Date"
        params(3, 0) = "etent_DateTo"

        params(0, 1) = orgztnID
        params(1, 1) = etent_EmployeeID
        params(2, 1) = etent_Date
        params(3, 1) = etent_DateTo

        EXEC_VIEW_PROCEDURE(params,
                            "VIEW_employeetimeentry_SUM",
                            dgvetent, , 1)

    End Sub

    Function COUNT_employeeabsent(Optional EmpID As Object = Nothing,
                                  Optional EmpStartDate As Object = Nothing,
                                  Optional payperiodDateFrom As Object = Nothing,
                                  Optional payperiodDateTo As Object = Nothing) As Object

        Dim returnval As Object = Nothing

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand("COUNT_employeeabsent", conn)
            conn.Open()
            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("absentcount", MySqlDbType.Decimal)

                .Parameters.AddWithValue("EmpID", EmpID)
                .Parameters.AddWithValue("OrgID", orgztnID)
                '.Parameters.AddWithValue("EmpStartDate", Format(CDate(EmpStartDate), "yyyy-MM-dd"))
                .Parameters.AddWithValue("EmpStartDate", New ExecuteQuery("SELECT StartDate FROM employee WHERE RowID='" & EmpID & "';").Result)
                .Parameters.AddWithValue("payperiodDateFrom", Format(CDate(payperiodDateFrom), "yyyy-MM-dd"))
                .Parameters.AddWithValue("payperiodDateTo", Format(CDate(payperiodDateTo), "yyyy-MM-dd"))

                .Parameters("absentcount").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()

                returnval = If(datread.Read = True, If(IsDBNull(datread.GetString(0)), "0.00", datread.GetString(0).ToString), "0.00") 'dr.GetString(0).ToString

            End With
        Catch ex As Exception
            MsgBox(ex.Message, , "Error : COUNT_employeeabsent")

        End Try

        Return returnval

    End Function

    Function GET_employeerateperhour(Optional EmpID As Object = Nothing,
                                     Optional paramDate As Object = Nothing) As Object

        Dim rate_perhour As Object = Nothing

        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand("GET_employeerateperhour", conn)
            conn.Open()
            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("rateperhour", MySqlDbType.Int32)

                .Parameters.AddWithValue("EmpID", EmpID)
                .Parameters.AddWithValue("OrgID", orgztnID)
                .Parameters.AddWithValue("paramDate", Format(CDate(paramDate), "yyyy-MM-dd"))

                .Parameters("rateperhour").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()

                rate_perhour = If(datread.Read = True, If(IsDBNull(datread.GetString(0)), "0", datread.GetString(0).ToString), "0") 'dr.GetString(0).ToString

            End With
        Catch ex As Exception
            MsgBox(ex.Message, , "Error : GET_employeerateperhour")

        End Try

        Return rate_perhour

    End Function

    Private Sub tsbtngenpayroll_Click(sender As Object, e As EventArgs) Handles tsbtngenpayroll.Click

        Me.VeryFirstPayPeriodIDOfThisYear = Nothing

        With selectPayPeriod

            'If dgvpayper.RowCount <> 0 Then
            '    .PayFreqType = Trim(dgvpayper.CurrentRow.Cells("Column12").Value)
            'Else
            '.PayFreqType = ""
            'End If

            'For Each tsitem As ToolStripItem In tstrip.Items
            '    If tsitem.Font Is selectedButtonFont Then
            '        .PayFreqType = tsitem.Text
            '        Exit For
            '    End If
            'Next

            .Show()
            .BringToFront()
            .dgvpaypers.Focus()

        End With

    End Sub

    Sub genpayroll(Optional PayFreqRowID As Object = Nothing)
        Dim loadTask = Task.Factory.StartNew(
            Sub()
                If paypFrom = Nothing And paypTo = Nothing Then
                    Exit Sub
                End If

                Dim resources = New PayrollResources(paypRowID, CDate(paypFrom), CDate(paypTo))
                Dim resourcesTask = resources.Load()
                resourcesTask.Wait()
                etent_totdaypay = resources.TimeEntries
                _loanSchedules = resources.LoanSchedules
                _loanTransactions = resources.LoanTransactions
                _fixedNonTaxableMonthlyAllowances = resources.FixedNonTaxableMonthlyAllowances
                _fixedTaxableMonthlyAllowances = resources.FixedTaxableMonthlyAllowances

                Dim dailyallowfreq = "Daily"

                If allowfreq.Count <> 0 Then
                    dailyallowfreq = If(allowfreq.Item(0).ToString = "", "Daily", allowfreq.Item(0).ToString)
                    'allowfreq : Daily'Monthly'One time'Semi-monthly'Weely
                End If

                emp_allowanceDaily = New ReadSQLProcedureToDatatable("GET_employee_allowanceofthisperiod",
                                                                                       orgztnID,
                                                                                       "Daily",
                                                                                       "1",
                                                                                       paypFrom,
                                                                                       paypTo).ResultTable

                notax_allowanceDaily = New ReadSQLProcedureToDatatable("GET_employee_allowanceofthisperiod",
                                                                                       orgztnID,
                                                                                       "Daily",
                                                                                       "0",
                                                                                       paypFrom,
                                                                                       paypTo).ResultTable

                emp_bonusDaily = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                                ",EmployeeID" &
                                                                ",(DATEDIFF('" & paypTo & "',EffectiveStartDate) + 1) 'bonusmultiplier'" &
                                                                " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & dailyallowfreq & "'" &
                                                                " AND IF(EffectiveStartDate > '" & paypFrom & "' AND EffectiveEndDate > '" & paypTo & "'" &
                                                                ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveStartDate < '" & paypFrom & "' AND EffectiveEndDate < '" & paypTo & "'" &
                                                                ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveStartDate <= '" & paypFrom & "' AND EffectiveEndDate >= '" & paypTo & "'" &
                                                                ", '" & paypTo & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                                                ", IF(EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                                                ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveEndDate IS NULL" &
                                                                ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                                                ")))))" &
                                                                " GROUP BY EmployeeID;")

                notax_bonusDaily = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                                ",EmployeeID" &
                                                                ",(DATEDIFF('" & paypTo & "',EffectiveStartDate) + 1) 'bonusmultiplier'" &
                                                                " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & dailyallowfreq & "'" &
                                                                " AND IF(EffectiveStartDate > '" & paypFrom & "' AND EffectiveEndDate > '" & paypTo & "'" &
                                                                ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveStartDate < '" & paypFrom & "' AND EffectiveEndDate < '" & paypTo & "'" &
                                                                ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveStartDate <= '" & paypFrom & "' AND EffectiveEndDate >= '" & paypTo & "'" &
                                                                ", '" & paypTo & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                                                ", IF(EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                                                ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveEndDate IS NULL" &
                                                                ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                                                ")))))" &
                                                                " GROUP BY EmployeeID;")

                Dim monthlyallowfreq = "Monthly"

                If allowfreq.Count <> 0 Then
                    monthlyallowfreq = If(allowfreq.Item(1).ToString = "", "Monthly", allowfreq.Item(1).ToString)
                End If

                emp_allowanceMonthly = New ReadSQLProcedureToDatatable("GET_employee_allowanceofthisperiod",
                                                                                       orgztnID,
                                                                                       "Monthly",
                                                                                       "1",
                                                                                       paypFrom,
                                                                                       paypTo).ResultTable

                notax_allowanceMonthly = New ReadSQLProcedureToDatatable("GET_employee_allowanceofthisperiod",
                                                                                       orgztnID,
                                                                                       "Monthly",
                                                                                       "0",
                                                                                       paypFrom,
                                                                                       paypTo).ResultTable

                emp_bonusMonthly = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                                 ",EmployeeID" &
                                                                " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & monthlyallowfreq & "'" &
                                                                " AND IF(EffectiveStartDate > '" & paypFrom & "' AND EffectiveEndDate > '" & paypTo & "'" &
                                                                ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveStartDate < '" & paypFrom & "' AND EffectiveEndDate < '" & paypTo & "'" &
                                                                ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveStartDate <= '" & paypFrom & "' AND EffectiveEndDate >= '" & paypTo & "'" &
                                                                ", '" & paypTo & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                                                ", IF(EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                                                ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveEndDate IS NULL" &
                                                                ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                                                ")))))" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                notax_bonusMonthly = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                                 ",EmployeeID" &
                                                                " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & monthlyallowfreq & "'" &
                                                                " AND IF(EffectiveStartDate > '" & paypFrom & "' AND EffectiveEndDate > '" & paypTo & "'" &
                                                                ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveStartDate < '" & paypFrom & "' AND EffectiveEndDate < '" & paypTo & "'" &
                                                                ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveStartDate <= '" & paypFrom & "' AND EffectiveEndDate >= '" & paypTo & "'" &
                                                                ", '" & paypTo & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                                                ", IF(EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                                                ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", IF(EffectiveEndDate IS NULL" &
                                                                ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                ", EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                                                ")))))" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                '" AND DATEDIFF(CURRENT_DATE(),EffectiveStartDate)>=0" &

                Dim onceallowfreq = "One time"

                If allowfreq.Count <> 0 Then
                    onceallowfreq = If(allowfreq.Item(2).ToString = "", "One time", allowfreq.Item(2).ToString)
                End If

                emp_allowanceOnce = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                                                " FROM employeeallowance" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & onceallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                notax_allowanceOnce = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                                                " FROM employeeallowance" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & onceallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                emp_bonusOnce = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & onceallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                notax_bonusOnce = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & onceallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                Dim semimallowfreq = "Semi-monthly"

                If allowfreq.Count <> 0 Then
                    semimallowfreq = If(allowfreq.Item(3).ToString = "", "Semi-monthly", allowfreq.Item(3).ToString)
                End If

                emp_bonusSemiM = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                notax_bonusSemiM = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                emp_allowanceSemiM = New ReadSQLProcedureToDatatable("GET_employee_allowanceofthisperiod",
                                                                                       orgztnID,
                                                                                       "Semi-monthly",
                                                                                       "1",
                                                                                       paypFrom,
                                                                                       paypTo).ResultTable

                notax_allowanceSemiM = New ReadSQLProcedureToDatatable("GET_employee_allowanceofthisperiod",
                                                                                       orgztnID,
                                                                                       "Semi-monthly",
                                                                                       "0",
                                                                                       paypFrom,
                                                                                       paypTo).ResultTable

                Dim weeklyallowfreq = "Weekly"

                If allowfreq.Count <> 0 Then
                    weeklyallowfreq = If(allowfreq.Item(4).ToString = "", "Weekly", allowfreq.Item(4).ToString)
                End If

                emp_bonusWeekly = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                notax_bonusWeekly = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                emp_allowanceWeekly = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                                                " FROM employeeallowance" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='Weekly'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                notax_allowanceWeekly = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                                                " FROM employeeallowance" &
                                                                " WHERE OrganizationID=" & orgztnID &
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='Weekly'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                _withholdingTaxTable = New SqlToDataTable("SELECT * FROM paywithholdingtax;").Read()
                _filingStatuses = New SqlToDataTable("SELECT * FROM filingstatus;").Read()

                esal_dattab = resources.Salaries

                DebugUtility.DumpTable(esal_dattab, "salaries")

                numofdaypresent = retAsDatTbl("SELECT COUNT(RowID) 'DaysAttended'" &
                                                                ",SUM((TIME_TO_SEC(TIMEDIFF(TimeOut,TimeIn)) / 60) / 60) 'SumHours'" &
                                                                ",EmployeeID" &
                                                                " FROM employeetimeentrydetails" &
                                                                " WHERE OrganizationID=" & orgztnID & "" &
                                                                " AND Date BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID;")

                If withthirteenthmonthpay = 1 Then

                    Dim params(1, 2) As Object

                    params(0, 0) = "param_OrganizationID"
                    params(1, 0) = "param_year"

                    params(0, 1) = orgztnID
                    params(1, 1) = paypTo

                    empthirteenmonthtable =
                                                  GetAsDataTable("GET_Attended_Months",
                                                                 CommandType.StoredProcedure,
                                                                 params)
                Else
                    empthirteenmonthtable = Nothing

                End If

                Dim param(2, 2) As Object

                param(0, 0) = "OrganizID"
                param(1, 0) = "etentDateFrom"
                param(2, 0) = "etentDateTo"

                param(0, 1) = orgztnID
                param(1, 1) = paypFrom
                param(2, 1) = paypTo

                etent_holidaypay = callProcAsDatTab(param,
                                                                      "GET_employeeholidaypay")

                Dim paramets(2, 2) As Object

                paramets(0, 0) = "OrganizID"
                paramets(1, 0) = "payp_FromDate"
                paramets(2, 0) = "payp_ToDate"

                paramets(0, 1) = orgztnID
                paramets(1, 1) = paypFrom
                paramets(2, 1) = paypTo

                emp_TardinessUndertime =
                                      callProcAsDatTab(paramets,
                                                       "GETVIEW_employeeTardinessUndertime")
                prev_empTimeEntry = New ReadSQLProcedureToDatatable("GETVIEW_previousemployeetimeentry",
                                                                                      orgztnID,
                                                                                      paypRowID,
                                                                                      paypRowID).ResultTable
                'prev_empTimeEntry

                'RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

                ''RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

                dtemployeefirsttimesalary =
                                      New SQLQueryToDatatable("SELECT COUNT(ete.RowID)" &
                                                              ",ete.EmployeeID" &
                                                              " FROM employeetimeentry ete" &
                                                              " INNER JOIN employee e ON ete.EmployeeID=e.RowID AND e.EmployeeType='Daily'" &
                                                              " WHERE ete.TotalDayPay!=0" &
                                                              " AND ete.OrganizationID='" & orgztnID & "'" &
                                                              " AND ete.`Date`" &
                                                              " BETWEEN '" & paypFrom & "'" &
                                                              " AND '" & paypTo & "'" &
                                                              " GROUP BY ete.EmployeeID" &
                                                              " HAVING COUNT(ete.RowID) < 5;").ResultTable
            End Sub,
            0
        )

        loadTask.ContinueWith(
            AddressOf LoadingPayrollDataOnSuccess,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        loadTask.ContinueWith(
            AddressOf LoadingPayrollDataOnError,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )
    End Sub

    Private Sub LoadingPayrollDataOnSuccess()
        indxStartBatch = 0

        Dim n_lov_mxthread As New ExecuteQuery("SELECT CAST(DisplayValue AS INT) `Result` FROM listofval WHERE `Type`='Max thread count' AND LIC='Max thread count' AND Active='Yes' LIMIT 1;")
        If ValNoComma(n_lov_mxthread.Result) > 0 Then
            thread_max = ValNoComma(n_lov_mxthread.Result)
        Else
            n_lov_mxthread = New _
                                                                   ExecuteQuery(String.Concat("INSERT INTO `listofval` (`DisplayValue`, `LIC`, `Type`, `ParentLIC`, `Active`, `Description`, `Created`, `CreatedBy`, `LastUpd`, `OrderBy`, `LastUpdBy`) VALUES ('5', 'Max thread count', 'Max thread count', '', 'Yes', 'max thread count when generating payroll', CURRENT_TIMESTAMP(), ", z_User, ", CURRENT_TIMESTAMP(), 1, ", z_User, ") ON DUPLICATE KEY UPDATE LastUpd = CURRENT_TIMESTAMP(), LastUpdBy = IFNULL(LastUpdBy,CreatedBy);"))
        End If

        progress_precentage = 0

        ThreadingPayrollGeneration(thread_max)
    End Sub

    Private Sub LoadingPayrollDataOnError(t As Task)
        _logger.Error("Error loading one of the payroll data.", t.Exception)
        MsgBox("Error loading", "Sorry, but something went wrong while loading the payroll data for computation.")
        Me.Enabled = True
    End Sub

    Private Sub ThreadingPayrollGeneration(Optional starting_batchindex As Integer = 0)
        Timer1.Stop()
        Timer1.Enabled = False

        Try
            Dim employees As DataTable = Nothing

            Using connection = New MySqlConnection(mysql_conn_text),
                command = New MySqlCommand("GetEmployees", connection)

                If Me.Enabled Then
                    Dim dataSet = New DataSet()

                    With command
                        .Connection.Open()
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Clear()
                        .Parameters.AddWithValue("$OrganizationID", orgztnID)
                        .Parameters.AddWithValue("$PayDateFrom", paypFrom)
                        .Parameters.AddWithValue("$PayDateTo", paypTo)
                    End With

                    Dim adapter = New MySqlDataAdapter(command)
                    adapter.Fill(dataSet)

                    employees = dataSet.Tables(0)

                    payroll_emp_count = employees.Rows.Count

                    Me.Enabled = False
                End If
            End Using

            Dim employeeRows = employees.Rows.Cast(Of DataRow)

            Timer1.Enabled = True
            Timer1.Start()

            Parallel.ForEach(
                employeeRows,
                Sub(employeeRow)
                    Dim generator = New PayrollGeneration(
                        employeeRow,
                        isEndOfMonth,
                        esal_dattab,
                        _loanSchedules,
                        _loanTransactions,
                        emp_allowanceDaily,
                        emp_allowanceMonthly,
                        emp_allowanceOnce,
                        emp_allowanceSemiM,
                        emp_allowanceWeekly,
                        notax_allowanceDaily,
                        notax_allowanceMonthly,
                        notax_allowanceOnce,
                        notax_allowanceSemiM,
                        notax_allowanceWeekly,
                        _fixedTaxableMonthlyAllowances,
                        _fixedNonTaxableMonthlyAllowances,
                        emp_bonusDaily,
                        emp_bonusMonthly,
                        emp_bonusOnce,
                        emp_bonusSemiM,
                        emp_bonusWeekly,
                        notax_bonusDaily,
                        notax_bonusMonthly,
                        notax_bonusOnce,
                        notax_bonusSemiM,
                        notax_bonusWeekly,
                        numofdaypresent,
                        etent_totdaypay,
                        dtemployeefirsttimesalary,
                        prev_empTimeEntry,
                        VeryFirstPayPeriodIDOfThisYear,
                        withthirteenthmonthpay,
                        _filingStatuses,
                        _withholdingTaxTable,
                        Me
                    )

                    With generator
                        .PayrollDateFrom = paypFrom
                        .PayrollDateTo = paypTo
                        .PayPeriodID = paypRowID
                    End With

                    generator.DoProcess()
                End Sub
            )
        Catch ex As Exception
            _logger.Error("Error loading the employees", ex)
        End Try
    End Sub

    Private Sub SinglePaySlip_Click(sender As Object, e As EventArgs) Handles DeclaredToolStripMenuItem.Click,
                                                                                ActualToolStripMenuItem.Click

        Dim IsActualFlag = Convert.ToInt16(DirectCast(sender, ToolStripMenuItem).Tag)

        Dim n_PrintSinglePaySlipOfficialFormat As _
            New PrintSinglePaySlipOfficialFormat(ValNoComma(paypRowID),
                                                 IsActualFlag,
                                                 ValNoComma(dgvemployees.Tag))

    End Sub

    Private Sub tsbtnprintpayslip_Click(sender As Object, e As EventArgs) 'Handles DeclaredToolStripMenuItem.Click 'tsbtnprintpayslip.Click

        tsbtnprintpayslip.Enabled = False

        If paypRowID <> Nothing Then

            If dgvemployees.RowCount > 0 Then

                Dim n_PrintSoloPaySlipThisPayPeriod As _
                    New PrintSoloPaySlipThisPayPeriod(paypFrom, paypTo,
                                                      dgvemployees.CurrentRow.Cells("RowID").Value, 0)

            End If

        End If

        tsbtnprintpayslip.Enabled = True

        '## ############################## ##

        Dim papy_str As String = Nothing

        If papy_str = Nothing Then 'If paypRowID = Nothing Then

            Exit Sub

        End If

        Try

            Dim rptdoc As New HalfPaySlip 'prntPaySlip

            With rptdoc.ReportDefinition.Sections(2)

                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("txtempbasicpay")
                objText.Text = " ₱ " & txtBasicPay.Text

                objText = .ReportObjects("OrgName")
                objText.Text = orgNam

                objText = .ReportObjects("OrgAddress")
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

                objText = .ReportObjects("OrgContact")
                'If Trim(contactdet(0).ToString) = "" Then
                'Else
                '    objText.Text = "Contact No. " & contactdet(0).ToString
                'End If

                objText.Text = String.Empty

                objText = .ReportObjects("payperiod")
                papy_str = "Payroll slip for the period of   " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))
                objText.Text = papy_str

                objText = .ReportObjects("txtFName")
                objText.Text = StrConv(LastFirstMidName, VbStrConv.Uppercase) 'txtFName.Text

                objText = .ReportObjects("txtEmpID")
                objText.Text = currentEmployeeID 'txtEmpID.Text

                objText = .ReportObjects("txttotreghrs")
                objText.Text = txtRegularHours.Text

                objText = .ReportObjects("txttotregamt")
                objText.Text = "₱ " & txttotregamt.Text

                objText = .ReportObjects("txttotothrs")
                objText.Text = txtOvertimeHours.Text

                objText = .ReportObjects("txttototamt")
                objText.Text = "₱ " & txtOvertimePay.Text

                objText = .ReportObjects("txttotnightdiffhrs")
                objText.Text = txtNightDiffHours.Text

                objText = .ReportObjects("txttotnightdiffamt")
                objText.Text = "₱ " & txtNightDiffPay.Text

                objText = .ReportObjects("txttotnightdiffothrs")
                objText.Text = txtNightDiffOvertimeHours.Text

                objText = .ReportObjects("txttotnightdiffotamt")
                objText.Text = "₱ " & txtNightDiffOvertimePay.Text

                objText = .ReportObjects("txttotholidayhrs")
                objText.Text = txtHolidayHours.Text

                objText = .ReportObjects("txttotholidayamt")
                objText.Text = "₱ " & txtHolidayPay.Text
                '₱
                objText = .ReportObjects("txthrswork")
                objText.Text = txttotreghrs.Text

                objText = .ReportObjects("txthrsworkamt")
                objText.Text = "₱ " & txtRegularPay.Text

                objText = .ReportObjects("lblsubtot")
                objText.Text = "₱ " & lblSubtotal.Text

                objText = .ReportObjects("txtemptotallow")
                objText.Text = "₱ " & txtemptotallow.Text

                objText = .ReportObjects("txtgrosssal")
                objText.Text = "₱ " & txtgrosssal.Text

                objText = .ReportObjects("txtvlbal")
                objText.Text = txtvlbal.Text

                objText = .ReportObjects("txtslbal")
                objText.Text = txtslbal.Text

                objText = .ReportObjects("txtmlbal")
                objText.Text = txtmlbal.Text

                objText = .ReportObjects("txtothlbal")
                objText.Text = 0

                For Each dgvrow As DataGridViewRow In dgvpaystubitem.Rows

                    If dgvrow.Cells("paystitm_Item").Value = "Others" Then

                        objText.Text = Val(dgvrow.Cells("paystitm_PayAmount").Value)

                        Exit For

                    End If

                Next

                objText = .ReportObjects("txttotabsent")
                objText.Text = txttotabsent.Text

                objText = .ReportObjects("txttotabsentamt")
                objText.Text = "₱ " & txttotabsentamt.Text

                objText = .ReportObjects("txttottardi")
                objText.Text = txttottardi.Text

                objText = .ReportObjects("txttottardiamt")
                objText.Text = "₱ " & txttottardiamt.Text

                objText = .ReportObjects("txttotut")
                objText.Text = txttotut.Text

                objText = .ReportObjects("txttotutamt")
                objText.Text = "₱ " & txttotutamt.Text

                Dim misc_subtot = Val(txttottardiamt.Text.Replace(",", "")) + Val(txttotutamt.Text.Replace(",", ""))

                objText = .ReportObjects("lblsubtotmisc")
                objText.Text = "₱ " & FormatNumber(Val(misc_subtot), 2) '.ToString.Replace(",", "")

                objText = .ReportObjects("txtempsss")
                objText.Text = "₱ " & txtempsss.Text

                objText = .ReportObjects("txtempphh")
                objText.Text = "₱ " & txtempphh.Text

                objText = .ReportObjects("txtemphdmf")
                objText.Text = "₱ " & txtemphdmf.Text

                objText = .ReportObjects("txtemptotloan")
                objText.Text = "₱ " & txtemptotloan.Text

                objText = .ReportObjects("txtemptotbon")
                objText.Text = "₱ " & txtemptotbon.Text

                objText = .ReportObjects("txttaxabsal")
                objText.Text = "₱ " & txttaxabsal.Text

                objText = .ReportObjects("txtempwtax")
                objText.Text = "₱ " & txtempwtax.Text

                objText = .ReportObjects("txtnetsal")
                objText.Text = "₱ " & txtnetsal.Text

                objText = .ReportObjects("allowsubdetails")

                If dgvemployees.RowCount <> 0 Then

                    VIEW_eallow_indate(dgvemployees.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                    VIEW_eloan_indate(dgvemployees.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                    VIEW_ebon_indate(dgvemployees.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                    Dim allowvalues As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("allowvalues")

                    'dgvpaystubitem

                    For Each dgvrow As DataGridViewRow In dgvempallowance.Rows
                        If dgvrow.Index = 0 Then
                            objText.Text = dgvrow.Cells("eall_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                            allowvalues.Text = "₱ " & FormatNumber(Val(dgvrow.Cells("eall_Amount").Value), 2)
                        Else
                            objText.Text &= vbNewLine & dgvrow.Cells("eall_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                            allowvalues.Text &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("eall_Amount").Value), 2)

                            Dim strtxt = dgvrow.Cells("eall_Type").Value & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                            If strtxt.ToString.Length < objText.Text.Length Then
                                Dim lengthdiff = strtxt.ToString.Length - objText.Text.Length
                            Else

                            End If

                        End If
                    Next

                    'objText.Text &= vbNewLine
                    'allowvalues.Text &= vbNewLine

                    objText = .ReportObjects("loansubdetails")

                    Dim loanvalues As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("loanvalues")

                    Dim tabchar = "	"

                    Dim resultloandetails = String.Empty

                    Dim resultloanvalues = String.Empty

                    For Each dgvrow As DataGridViewRow In dgvLoanList.Rows
                        If dgvrow.Index = 0 Then

                            objText.Text = dgvrow.Cells("c_loantype").Value & " loan " & tabchar & dgvrow.Cells("c_totballeft").Value ' & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

                            resultloandetails = dgvrow.Cells("c_loantype").Value & " loan "

                            loanvalues.Text = "₱ " & FormatNumber(Val(dgvrow.Cells("c_dedamt").Value), 2)

                            resultloanvalues = dgvrow.Cells("c_totballeft").Value
                        Else
                            objText.Text &= vbNewLine & dgvrow.Cells("c_loantype").Value & " loan " & tabchar & dgvrow.Cells("c_totballeft").Value ' & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

                            resultloandetails &= vbNewLine & dgvrow.Cells("c_loantype").Value & " loan "

                            loanvalues.Text &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("c_dedamt").Value), 2)

                            resultloanvalues &= dgvrow.Cells("c_totballeft").Value

                            Dim strtxt = dgvrow.Cells("c_loantype").Value & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

                            If strtxt.ToString.Length < objText.Text.Length Then
                                Dim lengthdiff = strtxt.ToString.Length - objText.Text.Length
                            Else

                            End If

                        End If
                    Next

                    objText = .ReportObjects("loansubdetails2")
                    objText.Text = resultloandetails

                    loanvalues = .ReportObjects("loanvalues2")
                    loanvalues.Text = resultloanvalues

                    'objText.Text &= vbNewLine
                    'loanvalues.Text &= vbNewLine

                    ''dgvempbon'bonsubdetails

                    objText = .ReportObjects("bonsubdetails")

                    Dim bonvalues As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("bonvalues")

                    For Each dgvrow As DataGridViewRow In dgvempbon.Rows
                        If dgvrow.Index = 0 Then
                            objText.Text = dgvrow.Cells("bon_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                            bonvalues.Text = "₱ " & FormatNumber(Val(dgvrow.Cells("bon_Amount").Value), 2)
                        Else
                            objText.Text &= vbNewLine & dgvrow.Cells("bon_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                            bonvalues.Text &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("bon_Amount").Value), 2)

                            Dim strtxt = dgvrow.Cells("bon_Type").Value & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                            If strtxt.ToString.Length < objText.Text.Length Then
                                Dim lengthdiff = strtxt.ToString.Length - objText.Text.Length
                            Else

                            End If

                        End If

                    Next

                    'objText.Text &= vbNewLine
                    'bonvalues.Text &= vbNewLine

                End If

            End With

            Dim crvwr As New CrysVwr
            crvwr.CrystalReportViewer1.ReportSource = rptdoc

            crvwr.Text = papy_str & ", ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value & ", " & txtFName.Text
            crvwr.Refresh()
            crvwr.Show() '
            'TINNo

            'rptdoc = Nothing
            'rptdoc.Dispose()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")

        End Try

    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Me.Close()

    End Sub

    Public genpayselyear As String = Nothing

    Sub btnrefresh_Click(sender As Object, e As EventArgs) Handles btnrefresh.Click
        'For Each c As DataGridViewColumn In dgvpayper.Columns
        '    File.AppendAllText(Path.GetTempPath() & "dgvpayper.txt", c.Name & "@" & c.HeaderText & "&" & c.Visible.ToString & Environment.NewLine)
        'Next

        'RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        ''RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        If TabControl2.SelectedIndex = 0 Then
            '
            Dim searchdate = Nothing
            If MaskedTextBox1.Text = "  /  /" Then
                searchdate = Format(CDate(dbnow), "yyyy")
            Else
                searchdate = Format(CDate(Trim(MaskedTextBox1.Text)), "yyyy")

            End If

            VIEW_payperiodofyear() 'searchdate

            'loademployee()

            'employeepicture = retAsDatTbl("SELECT RowID,Image FROM employee WHERE OrganizationID=" & orgztnID & ";")
        Else

        End If

        'AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        ''AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem
        TabControlColor(TabControl1, e)

    End Sub

    Private Sub btntotallow_Click(sender As Object, e As EventArgs) Handles btntotallow.Click

        viewtotloan.Close()

        viewtotbon.Close()
        'viewtotallow

        With viewtotallow
            .Show()
            .BringToFront()
            If dgvemployees.RowCount <> 0 Then

                'If tabEarned.SelectedIndex = 1 Then

                '    .VIEW_employeeallowance_indate(dgvemployees.CurrentRow.Cells("RowID").Value, _
                '                            paypFrom, _
                '                            paypTo, _
                '                            numofweekdays,
                '                            "Ecola")

                'Else

                .VIEW_employeeallowance_indate(dgvemployees.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo,
                                        numofweekdays)

                'End If

                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value

            End If

        End With

    End Sub

    Sub VIEW_eallow_indate(Optional eallow_EmployeeID As Object = Nothing,
                               Optional datefrom As Object = Nothing,
                               Optional dateto As Object = Nothing,
                               Optional AllowanceExcept As String = Nothing)

        Dim param(4, 2) As Object

        'param(0, 0) = "eallow_EmployeeID"
        'param(1, 0) = "eallow_OrganizationID"
        'param(2, 0) = "effectivedatefrom"
        'param(3, 0) = "effectivedateto"
        'param(4, 0) = "numweekdays"

        param(0, 0) = "eallow_EmployeeID"
        param(1, 0) = "eallow_OrganizationID"
        param(2, 0) = "effective_datefrom"
        param(3, 0) = "effective_dateto"
        param(4, 0) = "ExceptThisAllowance"

        param(0, 1) = eallow_EmployeeID
        param(1, 1) = orgztnID
        param(2, 1) = datefrom
        param(3, 1) = If(dateto = Nothing, DBNull.Value, dateto)
        param(4, 1) = If(AllowanceExcept = Nothing, String.Empty, AllowanceExcept)

        'param(4, 1) = Val(num_weekdays)

        EXEC_VIEW_PROCEDURE(param,
                           "VIEW_employeeallowances",
                           dgvempallowance, , 1) 'VIEW_employeeallowance_indate

    End Sub

    Private Sub btntotloan_Click(sender As Object, e As EventArgs) Handles btntotloan.Click
        viewtotallow.Close()
        viewtotbon.Close()
        'viewtotallow

        With viewtotloan
            .Show()
            .BringToFront()
            If dgvemployees.RowCount <> 0 Then
                .VIEW_employeeloan_indate(dgvemployees.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value
            End If
        End With

    End Sub

    Sub VIEW_eloan_indate(Optional eloan_EmployeeID As Object = Nothing,
                               Optional datefrom As Object = Nothing,
                               Optional dateto As Object = Nothing)

        Dim params(3, 2) As Object

        params(0, 0) = "eloan_EmployeeID"
        params(1, 0) = "eloan_OrganizationID"
        params(2, 0) = "effectivedatefrom"
        params(3, 0) = "effectivedateto"

        params(0, 1) = eloan_EmployeeID
        params(1, 1) = orgztnID
        params(2, 1) = datefrom
        params(3, 1) = dateto

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_employeeloan_indate",
                             dgvLoanList)

    End Sub

    Private Sub btntotbon_Click(sender As Object, e As EventArgs) Handles btntotbon.Click

        viewtotallow.Close()

        viewtotloan.Close()
        'viewtotallow

        With viewtotbon
            .Show()
            .BringToFront()
            If dgvemployees.RowCount <> 0 Then
                .VIEW_employeebonus_indate(dgvemployees.CurrentRow.Cells("RowID").Value,
                                        paypFrom,
                                        paypTo)

                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value
            End If
        End With

    End Sub

    Sub VIEW_ebon_indate(Optional ebon_EmployeeID As Object = Nothing,
                               Optional datefrom As Object = Nothing,
                               Optional dateto As Object = Nothing)

        Dim params(3, 2) As Object

        params(0, 0) = "ebon_EmployeeID"
        params(1, 0) = "ebon_OrganizationID"
        params(2, 0) = "effectivedatefrom"
        params(3, 0) = "effectivedateto"

        params(0, 1) = ebon_EmployeeID
        params(1, 1) = orgztnID
        params(2, 1) = datefrom
        params(3, 1) = dateto

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_employeebonus_indate",
                             dgvempbon)

    End Sub

    Private Sub PayStub_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        Dim alive_bgworks = array_bgwork.Cast(Of System.ComponentModel.BackgroundWorker).Where(Function(y) y IsNot Nothing)

        Dim busy_bgworks = alive_bgworks.Cast(Of System.ComponentModel.BackgroundWorker).Where(Function(y) y.IsBusy)

        Dim bool_result As Boolean = (Convert.ToInt16(busy_bgworks.Count) > 0)

        e.Cancel = False

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        showAuditTrail.Close()
        selectPayPeriod.Close()
        Dim open_forms = My.Application.OpenForms.Cast(Of Form).Where(Function(i) i.Name = "CrysVwr")
        If open_forms.Count > 0 Then
            Dim array_open_forms = open_forms.ToArray
            For Each frm In array_open_forms
                frm.Close()
            Next
        End If
        viewtotallow.Close()
        viewtotloan.Close()
        viewtotbon.Close()

        PayrollForm.listPayrollForm.Remove(Me.Name)
    End Sub

    Private Sub tsSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tsSearch.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)
        IsUserPressEnterToSearch = (e_asc = 13)
        If IsUserPressEnterToSearch Then
            Dim search_string_len = tsSearch.Text.Trim.Length
            IsUserPressEnterToSearch = (search_string_len > 0)
            tsbtnSearch_Click(sender, e)
        Else
            Dim keypressresult = TrapCharKey(e_asc) And TrapNumKey(e_asc)

            e.Handled = keypressresult

        End If

    End Sub

    Private Sub tsbtnSearch_Click(sender As Object, e As EventArgs) Handles tsbtnSearch.Click

        IsUserPressEnterToSearch = False
        ''RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        'loademployee(Trim(tsSearch.Text))
        If tsSearch.Text.Trim.Length = 0 Then
            First_LinkClicked(First, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link())) 'loademployee(quer_empPayFreq)'quer_empPayFreq
            'loademployee(String.Empty)
        Else
            Dim dattabsearch As New DataTable

            pagination = 0

            'dattabsearch = retAsDatTbl("SELECT e.*" &
            '                           ",pos.PositionName" &
            '                           ",pf.PayFrequencyType" &
            '                           ",fstat.FilingStatus" &
            '                           " FROM employee e" &
            '                           " LEFT JOIN user u ON e.CreatedBy=u.RowID" &
            '                           " LEFT JOIN position pos ON e.PositionID=pos.RowID" &
            '                           " LEFT JOIN payfrequency pf ON e.PayFrequencyID=pf.RowID" &
            '                           " LEFT JOIN filingstatus fstat ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents" &
            '                           " WHERE (e.FirstName LIKE '%" & Trim(tsSearch.Text) & "%'" &
            '                           " OR e.MiddleName LIKE '%" & Trim(tsSearch.Text) & "%'" &
            '                           " OR e.LastName LIKE '%" & Trim(tsSearch.Text) & "%'" &
            '                           " OR e.Surname LIKE '%" & Trim(tsSearch.Text) & "%'" &
            '                           " OR e.EmployeeID LIKE '%" & Trim(tsSearch.Text) & "%'" &
            '                           " OR e.TINNo LIKE '%" & Trim(tsSearch.Text) & "%'" &
            '                           " OR e.SSSNo LIKE '%" & Trim(tsSearch.Text) & "%'" &
            '                           " OR e.HDMFNo LIKE '%" & Trim(tsSearch.Text) & "%'" &
            '                           " OR e.PhilHealthNo LIKE '%" & Trim(tsSearch.Text) & "%')" &
            '                           " AND e.OrganizationID=" & orgztnID & " ORDER BY e.RowID DESC LIMIT " & pagination & ",100;")

            Dim param_array = New Object() {orgztnID,
                                            tsSearch.Text,
                                            pagination}

            Dim n_ReadSQLProcedureToDatatable As New _
                ReadSQLProcedureToDatatable("SEARCH_employee_paystub",
                                            param_array)

            dattabsearch = n_ReadSQLProcedureToDatatable.ResultTable

            '" WHERE MATCH (e.FirstName,e.MiddleName,e.LastName,e.Surname,e.EmployeeID,e.TINNo,e.SSSNo,e.HDMFNo,e.PhilHealthNo)" &
            '" AGAINST ('" & Trim(tsSearch.Text) & "') AND e.OrganizationID=" & orgztnID & " ORDER BY e.RowID DESC LIMIT " & pagination & ",100;")

            dgvemployees.Rows.Clear()
            PopulateDGVEmployee(dattabsearch)
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
            RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            dgvemployees_SelectionChanged(sender, e)

            AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        End If

        'dgvemployees_SelectionChanged(sender, e)

    End Sub

    Private Sub MaskedTextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MaskedTextBox1.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        If e_asc = 13 Then
            Try
                If MaskedTextBox1.Text = "  /  /" Then
                    'MsgBox(Format(CDate(dbnow), machineShortDateFormat))
                    MaskedTextBox1.Text = Format(CDate(dbnow), machineShortDateFormat)
                Else
                    If MaskedTextBox1.Text.Contains("_") Then
                        MaskedTextBox1.Text = Format(CDate(Trim(MaskedTextBox1.Text.Replace("_", ""))), machineShortDateFormat)

                    End If

                End If

                btnrefresh_Click(sender, e)
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            End Try
        End If

    End Sub

    Private Sub SplitContainer1_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        'Label61.Text = "SplitX=" & e.SplitX & " and " & "SplitY=" & e.SplitY

        SplitContainer1.Panel2.Focus()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'For Each c As DataGridViewColumn In dgvemployees.Columns
        '    File.AppendAllText(Path.GetTempPath() & "dgvemployees.txt", c.Name & "@" & c.HeaderText & "&" & c.Visible.ToString & Environment.NewLine)
        'Next

        If Button3.Image.Tag = 1 Then
            Button3.Image = Nothing
            Button3.Image = My.Resources.r_arrow
            Button3.Image.Tag = 0

            TabControl1.Show()
            dgvpayper.Width = 350

            dgvpayper_SelectionChanged(sender, e)
        Else
            Button3.Image = Nothing
            Button3.Image = My.Resources.l_arrow
            Button3.Image.Tag = 1

            TabControl1.Hide()
            Dim pointX As Integer = Width_resolution - (Width_resolution * 0.15)

            dgvpayper.Width = pointX
        End If

    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(viewID)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        'If dgvpayper.RowCount <> 0 Then
        Button1.Enabled = False
        bgwPrintAllPaySlip.RunWorkerAsync()
        'End If

        MsgBox(GET_employeeallowance(4,
                                     "Semi-monthly",
                                     "Fixed",
                                     "1"))

        'MsgBox(GET_employeeallowance(1, _
        '                             "Daily", _
        '                             "Monthly", _
        '                             "1"))

        'MsgBox(GET_employeeallowance(4, _
        '                             "Semi-monthly", _
        '                             "Fixed", _
        '                             "0"))

        'MsgBox(GET_employeeallowance(1, _
        '                             "Daily", _
        '                             "Monthly", _
        '                             "0"))

    End Sub

    'Dim empalldistrib As New List(Of String)

    'gingamit ko lang an 'GET_employeeallowance' sa allowance
    'na may Daily at semi-monthly
    'na allowance frequency

    Function GET_employeeallowance(Optional employeeRowID = Nothing,
                                   Optional allowancefrequensi = Nothing,
                                   Optional emppaytype = Nothing,
                                   Optional istaxab = Nothing,
                                   Optional emp_hiredate = Nothing) As Object

        Static ECOLA_RowID = Nothing

        Dim emphire_date = Format(CDate(emp_hiredate), "yyyy-MM-dd")

        Static once As SByte = 0

        If once = 0 Then
            once = 1

            ECOLA_RowID = EXECQUER("SELECT RowID FROM product WHERE PartNo='Ecola' AND OrganizationID='" & orgztnID & "' LIMIT 1;")

            dtempalldistrib.Columns.Add("ProductID")

            dtempalldistrib.Columns.Add("Value", Type.GetType("System.Double"))

        End If

        Static employ_rowid As String = Nothing

        If employ_rowid <> employeeRowID Then

            employ_rowid = employeeRowID

            If dtempalldistrib.Rows.Count <> 0 Then

                dtempalldistrib.Rows.Clear()

            End If

        End If

        'If empalldistrib.Count <> 0 Then

        '    empalldistrib.Clear()

        'End If

        Dim returningval = Nothing

        Dim totalAllowanceWork = 0.0

        Try

            If allowancefrequensi = "Daily" _
                    And emppaytype = "Fixed" Then

                Dim n_datatab As New DataTable

                Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount'" &
                                              ",ProductID" &
                                              " FROM employeeallowance" &
                                              " WHERE OrganizationID=" & orgztnID &
                                              " AND EmployeeID='" & employeeRowID & "'" &
                                              " AND TaxableFlag='" & istaxab & "'" &
                                              " AND AllowanceFrequency='" & allowancefrequensi & "'" &
                                              " AND IF(EffectiveStartDate > '" & paypFrom & "' AND EffectiveEndDate > '" & paypTo & "'" &
                                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                              ", IF(EffectiveStartDate < '" & paypFrom & "' AND EffectiveEndDate < '" & paypTo & "'" &
                                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                              ", IF(EffectiveStartDate <= '" & paypFrom & "' AND EffectiveEndDate >= '" & paypTo & "'" &
                                              ", '" & paypTo & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                              ", IF(EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                              ", IF(EffectiveEndDate IS NULL" &
                                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                              ", EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                              ")))))" &
                                              " GROUP BY ProductID;")

                n_datatab = n_SQLQueryToDatatable.ResultTable

                'numofweekdays = EXECQUER("SELECT `GET_numOfDaysSemiMonFixed`('" & orgztnID & "');")

                numofweekdays = EXECQUER("SELECT ROUND(`GET_empworkdaysperyear`('" & employeeRowID & "') / 12 / 2);")

                If CDate(emphire_date) >= CDate(paypFrom) _
                    And CDate(emphire_date) <= CDate(paypTo) Then

                    'Dim payperiod_diff = DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo))

                    Dim work_dayscount = DateDiff(DateInterval.Day, CDate(emphire_date), CDate(paypTo)) + 1

                    'work_dayscount = work_dayscount / (payperiod_diff + 1)

                    If numofweekdays >= 310 And numofweekdays <= 320 Then 'six days a week

                        numofweekdays = 0

                        For i = 0 To work_dayscount

                            Dim DayOfWeek = CDate(emphire_date).AddDays(i)

                            If DayOfWeek.DayOfWeek = 0 Then 'System.DayOfWeek.Sunday
                                '    PayStub.numofweekends += 1

                                'ElseIf DayOfWeek.DayOfWeek = 6 Then 'System.DayOfWeek.Saturday
                                '    PayStub.numofweekends += 1
                            Else
                                numofweekdays += 1

                            End If

                        Next

                        'numofweekdays = numofweekdays * work_dayscount
                    Else '                              'five days a week

                        numofweekdays = 0

                        For i = 0 To work_dayscount

                            Dim DayOfWeek = CDate(emphire_date).AddDays(i)

                            If DayOfWeek.DayOfWeek = 0 Then 'System.DayOfWeek.Sunday
                                '    PayStub.numofweekends += 1

                            ElseIf DayOfWeek.DayOfWeek = 6 Then 'System.DayOfWeek.Saturday
                                '    PayStub.numofweekends += 1
                            Else
                                numofweekdays += 1

                            End If

                        Next

                    End If

                End If

                If n_datatab IsNot Nothing Then

                    If n_datatab.Rows.Count <> 0 Then

                        For Each drrow As DataRow In n_datatab.Rows

                            Dim eal_val = drrow("TotalAllowanceAmount") * numofweekdays

                            totalAllowanceWork = totalAllowanceWork + eal_val

                            dtempalldistrib.Rows.Add(drrow("ProductID"), eal_val)

                        Next

                    End If

                    n_datatab = Nothing

                End If

            ElseIf allowancefrequensi = "Semi-monthly" _
                        And emppaytype = "Fixed" Then

                Dim n_datatab As New DataTable

                Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount'" &
                                              ",ProductID" &
                                              " FROM employeeallowance" &
                                              " WHERE OrganizationID=" & orgztnID &
                                              " AND EmployeeID='" & employeeRowID & "'" &
                                              " AND TaxableFlag='" & istaxab & "'" &
                                              " AND AllowanceFrequency='" & allowancefrequensi & "'" &
                                              " AND IF(EffectiveStartDate > '" & paypFrom & "' AND EffectiveEndDate > '" & paypTo & "'" &
                                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                              ", IF(EffectiveStartDate < '" & paypFrom & "' AND EffectiveEndDate < '" & paypTo & "'" &
                                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                              ", IF(EffectiveStartDate <= '" & paypFrom & "' AND EffectiveEndDate >= '" & paypTo & "'" &
                                              ", '" & paypTo & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                              ", IF(EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                              ", IF(EffectiveEndDate IS NULL" &
                                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                              ", EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                              ")))))" &
                                              " GROUP BY ProductID;")

                n_datatab = n_SQLQueryToDatatable.ResultTable

                If n_datatab IsNot Nothing Then

                    If n_datatab.Rows.Count <> 0 Then

                        For Each drrow As DataRow In n_datatab.Rows

                            Dim eal_val = drrow("TotalAllowanceAmount")

                            totalAllowanceWork = totalAllowanceWork + eal_val

                            dtempalldistrib.Rows.Add(drrow("ProductID"), eal_val)

                        Next

                    End If

                    n_datatab = Nothing

                End If
            Else

                Dim date_diff = DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo))

                Dim employee_timeentries As New DataTable

                Dim payperiod_fromdate = Format(CDate(paypFrom), "yyyy-MM-dd")

                Dim payperiod_todate = Format(CDate(paypTo), "yyyy-MM-dd")

                Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT ete.*" &
                                                 ",pr.PayType" &
                                                 ",IFNULL(((TIME_TO_SEC(TIMEDIFF(IF(etd.TimeIn>etd.TimeOut, ADDTIME(etd.TimeOut,'24:00:00'),etd.TimeOut),etd.TimeIn)) / 60) / 60),ete.TotalHoursWorked) AS HrsWorked" &
                                                 ",e.WorkDaysPerYear" &
                                                 ",IF(COMPUTE_TimeDifference(sh.TimeFrom, sh.TimeTo) IN (4,5), COMPUTE_TimeDifference(sh.TimeFrom, sh.TimeTo), (COMPUTE_TimeDifference(sh.TimeFrom, sh.TimeTo) - 1)) AS PerfectHours" &
                                                 " FROM payrate pr" &
                                                 " LEFT JOIN employeetimeentry ete ON ete.Date=pr.Date AND ete.OrganizationID=pr.OrganizationID AND ete.EmployeeID='" & employeeRowID & "'" &
                                                 " LEFT JOIN employeetimeentrydetails etd ON etd.Date=pr.Date AND etd.OrganizationID=pr.OrganizationID AND etd.EmployeeID='" & employeeRowID & "'" &
                                                 " LEFT JOIN (SELECT * FROM employeeshift WHERE OrganizationID='" & orgztnID & "' AND EmployeeID='" & employeeRowID & "') esh ON esh.RowID=ete.EmployeeShiftID" &
                                                 " LEFT JOIN shift sh ON sh.RowID=esh.ShiftID" &
                                                 " INNER JOIN employee e ON e.RowID=ete.EmployeeID AND e.OrganizationID=pr.OrganizationID" &
                                                 " WHERE pr.OrganizationID='" & orgztnID & "'" &
                                                 " AND ete.TotalDayPay != 0" &
                                                 " AND pr.`Date` BETWEEN '" & payperiod_fromdate & "' AND '" & payperiod_todate & "';")

                employee_timeentries = n_SQLQueryToDatatable.ResultTable

                Dim count_employee_timeentries = employee_timeentries.Rows.Count

                For i = 0 To date_diff

                    Dim DayOfWeek = CDate(paypFrom).AddDays(i)

                    Dim dateloop = Format(CDate(DayOfWeek), "yyyy-MM-dd")

                    'Dim emphourworked = EXECQUER("SELECT ((TIME_TO_SEC(TIMEDIFF(IF(TimeIn>TimeOut,ADDTIME(TimeOut,'24:00:00'),TimeOut),TimeIn)) / 60) / 60) 'HrsWorked'" &
                    '                             " FROM employeetimeentrydetails" &
                    '                             " WHERE EmployeeID='" & employeeRowID & "'" &
                    '                             " AND OrganizationID='" & orgztnID & "'" &
                    '                             " AND Date='" & dateloop & "'" &
                    '                             " ORDER BY Date DESC;")

                    'Dim sel_employee_timeentry = employee_timeentries.Select("Date = '" & dateloop & "'")

                    Dim emphourworked = employee_timeentries.Compute("SUM(HrsWorked)", "Date = '" & dateloop & "'")

                    Dim empTotDayPay = employee_timeentries.Compute("SUM(TotalDayPay)", "Date = '" & dateloop & "'")

                    Dim perfecthours = employee_timeentries.Compute("SUM(PerfectHours)", "Date = '" & dateloop & "'")

                    Dim regularhours = employee_timeentries.Compute("SUM(RegularHoursWorked)", "Date = '" & dateloop & "'")

                    Dim pro_ratedhrs = ValNoComma(regularhours) / ValNoComma(perfecthours)

                    'If IsDBNull(emphourworked) Then
                    '    emphourworked = 0
                    'Else
                    emphourworked = ValNoComma(emphourworked)
                    'End If

                    emphourworked = If(Val(emphourworked) > 8, (Val(emphourworked) - 1), Val(emphourworked))

                    Dim sel_employee_timeentries = employee_timeentries.Select("Date = '" & dateloop & "'")

                    Dim DayPayType = String.Empty

                    'If sel_employee_timeentries.Count = 0 Then
                    '    Continue For
                    'Else

                    For Each drow As DataRow In sel_employee_timeentries
                        DayPayType = drow("PayType")
                    Next

                    'End If

                    If emphourworked = 0 Then

                        If emppaytype = "Fixed" Or DayPayType = "Regular Holiday" Then

                            emphourworked = 8

                        ElseIf emppaytype = "Monthly" And numofweekdays = 15 Then

                            emphourworked = 8

                        End If

                    End If

                    If emphourworked = 0 Then
                    Else

                        'If emppaytype = "Monthly" And numofweekdays = 15 Then

                        '    emphourworked = 8

                        'End If

                        Dim dutyhours = EXECQUER("SELECT ((TIME_TO_SEC(TIMEDIFF(IF(sh.TimeFrom>sh.TimeTo,ADDTIME(sh.TimeTo,'24:00:00'),sh.TimeTo),sh.TimeFrom)) / 60) / 60) 'DutyHrs'" &
                                                 " FROM employeeshift esh" &
                                                 " LEFT JOIN shift sh ON sh.RowID=esh.ShiftID" &
                                                 " WHERE esh.EmployeeID='" & employeeRowID & "'" &
                                                 " AND esh.OrganizationID='" & orgztnID & "'" &
                                                 " AND '" & dateloop & "'" &
                                                 " BETWEEN DATE(COALESCE(esh.EffectiveFrom, DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d')))" &
                                                 " AND DATE(COALESCE(esh.EffectiveTo, ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH)))" &
                                                 " AND DATEDIFF('" & dateloop & "',esh.EffectiveFrom) >= 0" &
                                                 " AND COALESCE(esh.RestDay,0)='0'" &
                                                 " ORDER BY DATEDIFF(DATE_FORMAT('" & dateloop & "','%Y-%m-%d'),esh.EffectiveFrom)" &
                                                 " LIMIT 1;")

                        dutyhours = If(Val(dutyhours) > 8, (dutyhours - 1), Val(dutyhours))

                        If dutyhours = 0 Then

                            If emppaytype = "Fixed" Or DayPayType = "Regular Holiday" Then

                                dutyhours = 8

                            ElseIf emppaytype = "Monthly" And numofweekdays = 15 Then

                                dutyhours = dutyhours '8

                            End If

                        End If

                        If Val(dutyhours) = 0 Then

                            If allowancefrequensi = "Daily" Then

                                Dim empallowanceamounts As New DataTable

                                n_SQLQueryToDatatable = New SQLQueryToDatatable("SELECT IFNULL(AllowanceAmount,0) 'AllowanceAmount'" &
                                                                  ",ProductID" &
                                                                  " FROM employeeallowance" &
                                                                  " WHERE EmployeeID='" & employeeRowID & "'" &
                                                                  " AND OrganizationID='" & orgztnID & "'" &
                                                                  " AND TaxableFlag='" & istaxab & "'" &
                                                                  " AND AllowanceFrequency='" & allowancefrequensi & "'" &
                                                                  " AND ProductID='" & ECOLA_RowID & "'" &
                                                                  " AND '" & dateloop & "'" &
                                                                  " BETWEEN EffectiveStartDate" &
                                                                  " AND EffectiveEndDate;")

                                empallowanceamounts = n_SQLQueryToDatatable.ResultTable

                                For Each ddrow As DataRow In empallowanceamounts.Rows

                                    If ValNoComma(ddrow("ProductID")) = ECOLA_RowID Then
                                        'If ValNoComma(empTotDayPay) > 0 Then

                                        totalAllowanceWork += (ValNoComma(ddrow("AllowanceAmount")) * pro_ratedhrs)

                                        dtempalldistrib.Rows.Add(ddrow("ProductID"), (ValNoComma(ddrow("AllowanceAmount")) * pro_ratedhrs))

                                        'Else
                                        '    dtempalldistrib.Rows.Add(ddrow("ProductID"), 0.0)

                                        'End If

                                        Exit For

                                    End If

                                Next

                            End If
                        Else

                            Dim empallowanceamounts As New DataTable

                            n_SQLQueryToDatatable = New SQLQueryToDatatable("SELECT IFNULL(AllowanceAmount,0) 'AllowanceAmount'" &
                                                              ",ProductID" &
                                                              " FROM employeeallowance" &
                                                              " WHERE EmployeeID='" & employeeRowID & "'" &
                                                              " AND OrganizationID='" & orgztnID & "'" &
                                                              " AND TaxableFlag='" & istaxab & "'" &
                                                              " AND AllowanceFrequency='" & allowancefrequensi & "'" &
                                                              " AND '" & dateloop & "'" &
                                                              " BETWEEN EffectiveStartDate" &
                                                              " AND EffectiveEndDate;")

                            empallowanceamounts = n_SQLQueryToDatatable.ResultTable

                            If allowancefrequensi = "Daily" Then

                                'Dim distcount = empallowanceamounts.Select.Distinct.Count

                                For Each ddrow As DataRow In empallowanceamounts.Rows

                                    If ValNoComma(ddrow("ProductID")) = ECOLA_RowID Then
                                        If ValNoComma(empTotDayPay) > 0 Then

                                            totalAllowanceWork += (ValNoComma(ddrow("AllowanceAmount")) * pro_ratedhrs)

                                            dtempalldistrib.Rows.Add(ddrow("ProductID"), (ValNoComma(ddrow("AllowanceAmount")) * pro_ratedhrs))
                                        Else
                                            dtempalldistrib.Rows.Add(ddrow("ProductID"), 0.0)

                                        End If
                                    Else

                                        Dim valamount = Val(ddrow("AllowanceAmount")) ' / dutyhours

                                        totalAllowanceWork = totalAllowanceWork + (valamount) ' * emphourworked)

                                        Dim allowaval = valamount ' * emphourworked

                                        'empalldistrib.Add(allowaval & "@" & ddrow("ProductID"))

                                        dtempalldistrib.Rows.Add(ddrow("ProductID"), allowaval)

                                    End If

                                Next

                                'ElseIf allowancefrequensi = "Monthly" Then

                                'ElseIf allowancefrequensi = "Once" Then

                            ElseIf allowancefrequensi = "Semi-monthly" Then

                                If emppaytype = "Daily" Then

                                    'numofweekdays = EXECQUER("SELECT ROUND(`GET_empworkdaysperyear`('" & employeeRowID & "') / 12 / 2);")

                                    For Each ddrow As DataRow In empallowanceamounts.Rows

                                        Dim val_allowa = ValNoComma(ddrow("AllowanceAmount"))

                                        Dim allowaval = val_allowa 'valamount * emphourworked

                                        totalAllowanceWork = totalAllowanceWork + val_allowa 'totalAllowanceWork + (valamount * emphourworked)

                                        dtempalldistrib.Rows.Add(ddrow("ProductID"), allowaval)

                                    Next
                                Else

                                    Dim emp_late = employee_timeentries.Compute("SUM(HoursLate)", "EmployeeID = '" & employeeRowID & "'")

                                    emp_late = ValNoComma(emp_late)

                                    Dim emp_undtime = employee_timeentries.Compute("SUM(UnderTimeHours)", "EmployeeID = '" & employeeRowID & "'")

                                    emp_undtime = ValNoComma(emp_undtime)

                                    'For Each drow As DataRow In sel_employee_timeentries
                                    '    emp_late = ValNoComma(drow("HoursLate")) _
                                    '        + ValNoComma(drow("UnderTimeHours"))
                                    'Next

                                    Dim empRowID = employeeRowID

                                    For Each ddrow As DataRow In employee_timeentries.Rows

                                        Dim splitthisvalue = ValNoComma(ddrow("WorkDaysPerYear")) / 12

                                        splitthisvalue = splitthisvalue / 2

                                        Dim splitval = Split(splitthisvalue.ToString, ".")

                                        numofweekdays = ValNoComma(splitval(0))

                                        Exit For

                                    Next

                                    'changes made here Lambert
                                    'Dim num_weekdays = _
                                    'EXECQUER("SELECT AVG(IF(GET_employeerateperday('" & employeeRowID & "','" & orgztnID & "',d.DateValue) / ete.RegularHoursAmount > 1,1,GET_employeerateperday('" & employeeRowID & "','" & orgztnID & "',d.DateValue) / ete.RegularHoursAmount)) AS AvgAmount" &
                                    '        " FROM dates d" &
                                    '        " INNER JOIN employeeshift esh ON esh.EmployeeID='" & employeeRowID & "' AND OrganizationID='" & orgztnID & "' AND d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo" &
                                    '        " INNER JOIN shift sh ON sh.RowID=esh.ShiftID" &
                                    '        " INNER JOIN employeetimeentry ete ON ete.EmployeeID='" & employeeRowID & "' AND ete.OrganizationID='" & orgztnID & "' AND ete.`Date`=d.DateValue" &
                                    '        " INNER JOIN payrate pr ON pr.RowID=ete.PayRateID" &
                                    '        " WHERE d.DateValue BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                    '        " ORDER BY d.DateValue;")

                                    For Each ddrow As DataRow In empallowanceamounts.Rows

                                        Dim val_allowa = ValNoComma(ddrow("AllowanceAmount"))

                                        Dim valamount = val_allowa '(val_allowa / numofweekdays)

                                        ''valamount = valamount * (numofweekdays / (date_diff + 1))

                                        'valamount = valamount * count_employee_timeentries
                                        '                       'val_allowa
                                        '                                   'numofweekdays
                                        Dim less_toallowance = ((valamount / count_employee_timeentries) / 8) * (emp_late + emp_undtime)

                                        valamount = valamount - less_toallowance

                                        totalAllowanceWork = valamount 'totalAllowanceWork + (valamount * emphourworked)

                                        Dim allowaval = totalAllowanceWork 'valamount * emphourworked

                                        dtempalldistrib.Rows.Add(ddrow("ProductID"), allowaval)

                                    Next

                                End If

                                Exit For

                            ElseIf allowancefrequensi = "Monthly" Then

                                For Each ddrow As DataRow In empallowanceamounts.Rows

                                    Dim valamount = Val(ddrow("AllowanceAmount")) / numofweekdays

                                    valamount = valamount * (numofweekdays / (date_diff + 1))

                                    totalAllowanceWork = totalAllowanceWork + (valamount) ' * emphourworked)

                                    Dim allowaval = valamount ' * emphourworked

                                    'empalldistrib.Add(allowaval & "@" & ddrow("ProductID"))

                                    dtempalldistrib.Rows.Add(ddrow("ProductID"), allowaval)

                                Next

                            End If

                            empallowanceamounts = Nothing

                        End If

                    End If

                Next

            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally

            returningval = totalAllowanceWork

        End Try

        Return returningval

    End Function

    Private Sub tsbtnPayrollSumma_Click1(sender As Object, e As EventArgs) Handles _
        DeclaredToolStripMenuItem2.Click,
        ActualToolStripMenuItem2.Click
        '###########################################################################

        Try
            Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

            n_PayrollSummaDateSelection.ReportIndex = 6

            If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim obj_sender = DirectCast(sender, ToolStripMenuItem)

                Dim is_actual As Boolean = (obj_sender.Name = "ActualToolStripMenuItem2")

                Dim str_saldistrib As String = n_PayrollSummaDateSelection.cboStringParameter.Text

                Dim param_array = New Object() {orgztnID,
                                                n_PayrollSummaDateSelection.DateFromID,
                                                n_PayrollSummaDateSelection.DateToID,
                                                Convert.ToInt16(is_actual),
                                                str_saldistrib}

                Dim n_ReadSQLProcedureToDatatable As New _
                    ReadSQLProcedureToDatatable("PAYROLLSUMMARY",
                                                param_array)

                Dim datatab As New DataTable

                datatab = n_ReadSQLProcedureToDatatable.ResultTable

                If datatab Is Nothing Then
                Else

                    Dim rptdoc As New PayrollSumma

                    rptdoc.SetDataSource(datatab)

                    Dim crvwr As New CrysRepForm

                    crvwr.crysrepvwr.ReportSource = rptdoc

                    Dim papy_string = ""

                    papy_string = "for the period of " & Format(CDate(n_PayrollSummaDateSelection.DateFromstr), machineShortDateFormat) &
                        If(paypTo = Nothing, "", " to " & Format(CDate(n_PayrollSummaDateSelection.DateTostr), machineShortDateFormat))

                    Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

                    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")

                    objText.Text = orgNam

                    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgaddress")

                    Dim obj_value = EXECQUER(String.Concat("SELECT CONCAT_WS(', ', ad.StreetAddress1, ad.StreetAddress2, ad.CityTown, ad.Country, ad.State, ad.ZipCode, ad.Barangay) `Result`",
                                                           " FROM organization og",
                                                           " LEFT JOIN address ad ON ad.RowID=og.PrimaryAddressID",
                                                           " WHERE og.RowID=", orgztnID, ";"))

                    If obj_value = Nothing Then
                    Else
                        objText.Text = CStr(obj_value)
                    End If

                    PayrollSummaChosenData = obj_sender.Text

                    crvwr.Text = papy_string & " (" & PayrollSummaChosenData.ToUpper & ")"

                    crvwr.Refresh()

                    crvwr.Show()

                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub tsbtnPayrollSumma_Click(sender As Object, e As EventArgs) 'Handles DeclaredToolStripMenuItem2.Click, _
        'ActualToolStripMenuItem2.Click 'tsbtnPayrollSumma.Click

        Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

        n_PayrollSummaDateSelection.ReportIndex = 6

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

            Dim obj_sender = DirectCast(sender, ToolStripMenuItem)

            If obj_sender.Name = "DeclaredToolStripMenuItem2" Then

                params(3, 1) = "0"

                PayrollSummaChosenData = DeclaredToolStripMenuItem2.Text
            Else 'If obj_sender.Name = "ActualToolStripMenuItem2" Then

                params(3, 1) = "1"

                PayrollSummaChosenData = ActualToolStripMenuItem2.Text

            End If

            params(4, 1) = n_PayrollSummaDateSelection.
                           cboStringParameter.
                           Text

            Dim datatab As DataTable

            datatab = callProcAsDatTab(params,
                                       "PAYROLLSUMMARY")

            Static once As SByte = 0

            If once = 0 Then

                once = 1

                With rptdattab.Columns

                    .Add("DatCol1") ', Type.GetType("System.Int32"))

                    .Add("DatCol2") ', Type.GetType("System.String"))

                    .Add("DatCol3") 'Employee Full Name

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
                rptdattab.Rows.Clear()

            End If

            Dim newdatrow As DataRow

            Dim AbsTardiUTNDifOTHolipay As New DataTable

            Dim paramets(4, 2) As Object

            paramets(0, 0) = "param_OrganizationID"
            paramets(1, 0) = "param_EmployeeRowID"
            paramets(2, 0) = "param_PayPeriodID1"
            paramets(3, 0) = "param_PayPeriodID2"
            paramets(4, 0) = "IsActual"

            paramets(0, 1) = orgztnID
            'paramets(1, 1) = drow("EmployeeRowID")
            paramets(2, 1) = n_PayrollSummaDateSelection.DateFromID
            paramets(3, 1) = n_PayrollSummaDateSelection.DateToID

            paramets(4, 1) = params(3, 1)

            For Each drow As DataRow In datatab.Rows

                newdatrow = rptdattab.NewRow

                newdatrow("DatCol1") = If(IsDBNull(drow(17)), "None", drow(17)) 'Division
                newdatrow("DatCol2") = drow(11) 'Employee ID

                newdatrow("DatCol3") = drow(14) & ", " & drow(12) & If(Trim(drow(13)) = "", "", ", " & drow(13)) 'Full name

                newdatrow("DatCol4") = If(IsDBNull(drow(16)), "None", drow(16)) 'Position
                'newdatrow("DatCol5") = "0"
                'newdatrow("DatCol6") = "0"
                'newdatrow("DatCol7") = "0"
                'newdatrow("DatCol8") = "0"
                'newdatrow("DatCol9") = "0"
                'newdatrow("DatCol10") = "0"

                'newdatrow("DatCol1") = "0"
                'newdatrow("DatCol2") = "0"
                'newdatrow("DatCol3") = "0"
                'newdatrow("DatCol4") = "0"
                'newdatrow("DatCol5") = "0"
                'newdatrow("DatCol6") = "0"
                'newdatrow("DatCol7") = "0"
                'newdatrow("DatCol8") = "0"
                'newdatrow("DatCol9") = "0"
                newdatrow("DatCol20") = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "MMMM d, yyyy") &
                If(paypTo = Nothing, "", " to " & Format(CDate(n_PayrollSummaDateSelection.DateTostr), "MMMM d, yyyy")) 'Pay period

                newdatrow("DatCol21") = FormatNumber(Val(drow(0)), 2) 'Basic pay
                newdatrow("DatCol22") = FormatNumber(Val(drow(1)), 2) 'Gross income
                newdatrow("DatCol23") = FormatNumber(Val(drow(2)), 2) 'Net salary
                newdatrow("DatCol24") = FormatNumber(Val(drow(3)), 2) 'Taxable income
                newdatrow("DatCol25") = FormatNumber(Val(drow(4)), 2) 'SSS
                newdatrow("DatCol26") = FormatNumber(Val(drow(5)), 2) 'Withholding tax
                newdatrow("DatCol27") = FormatNumber(Val(drow(6)), 2) 'PhilHealth
                newdatrow("DatCol28") = FormatNumber(Val(drow(7)), 2) 'PAGIBIG
                newdatrow("DatCol29") = FormatNumber(Val(drow(8)), 2) 'Loans
                newdatrow("DatCol30") = FormatNumber(Val(drow(9)), 2) 'Bonus
                newdatrow("DatCol31") = FormatNumber(Val(drow(10)), 2) 'Allowance

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

                'For Each ddrow As DataRow In AbsTardiUTNDifOTHolipay.Rows

                '    If Trim(ddrow("PartNo")) = "Absent" Then

                absentval = ValNoComma(AbsTardiUTNDifOTHolipay.Compute("SUM(PayAmount)", "PartNo='Absent' AND Undeclared='" & params(3, 1) & "'"))

                '    ElseIf Trim(ddrow("PartNo")) = "Tardiness" Then

                tardival = ValNoComma(AbsTardiUTNDifOTHolipay.Compute("SUM(PayAmount)", "PartNo='Tardiness' AND Undeclared='" & params(3, 1) & "'"))

                '    ElseIf Trim(ddrow("PartNo")) = "Undertime" Then

                UTval = ValNoComma(AbsTardiUTNDifOTHolipay.Compute("SUM(PayAmount)", "PartNo='Undertime' AND Undeclared='" & params(3, 1) & "'"))

                '    ElseIf Trim(ddrow("PartNo")) = "Night differential OT" Then

                ndiffOTval = ValNoComma(AbsTardiUTNDifOTHolipay.Compute("SUM(PayAmount)", "PartNo='Night differential OT' AND Undeclared='" & params(3, 1) & "'"))

                '    ElseIf Trim(ddrow("PartNo")) = "Holiday pay" Then

                holidayval = ValNoComma(AbsTardiUTNDifOTHolipay.Compute("SUM(PayAmount)", "PartNo='Holiday pay' AND Undeclared='" & params(3, 1) & "'"))

                '    ElseIf Trim(ddrow("PartNo")) = "Overtime" Then

                overtimeval = ValNoComma(AbsTardiUTNDifOTHolipay.Compute("SUM(PayAmount)", "PartNo='Overtime' AND Undeclared='" & params(3, 1) & "'"))

                '    ElseIf Trim(ddrow("PartNo")) = "Night differential" Then

                ndiffval = ValNoComma(AbsTardiUTNDifOTHolipay.Compute("SUM(PayAmount)", "PartNo='Night differential' AND Undeclared='" & params(3, 1) & "'"))

                '    End If '

                'Next

                'newdatrow("DatCol32") = FormatNumber(absentval, 2) 'Absent

                'newdatrow("DatCol33") = FormatNumber(tardival, 2) 'Tardiness

                'newdatrow("DatCol34") = FormatNumber(UTval, 2) 'Undertime

                'newdatrow("DatCol35") = FormatNumber(ndiffval, 2) 'Night differential

                'newdatrow("DatCol36") = FormatNumber(holidayval, 2) 'Holiday pay

                'newdatrow("DatCol37") = FormatNumber(overtimeval, 2) 'Overtime

                'newdatrow("DatCol38") = FormatNumber(ndiffOTval, 2) 'Night differential OT

                '***********************************************************************************

                'newdatrow("DatCol32") = FormatNumber(Val(drow("Absent")), 2) 'Absent
                newdatrow("DatCol32") = FormatNumber(ValNoComma(drow("DatCol32")), 2) 'Absent

                newdatrow("DatCol33") = FormatNumber(Val(drow("Tardiness")), 2) 'Tardiness

                newdatrow("DatCol34") = FormatNumber(Val(drow("UnderTime")), 2) 'Undertime

                newdatrow("DatCol35") = FormatNumber(Val(drow("NightDifftl")), 2) 'Night differential

                newdatrow("DatCol36") = FormatNumber(Val(drow("HolidayPay")), 2) 'Holiday pay

                newdatrow("DatCol37") = FormatNumber(Val(drow("OverTime")), 2) 'Overtime

                newdatrow("DatCol38") = FormatNumber(Val(drow("NightDifftlOT")), 2) 'Night differential OT

                newdatrow("DatCol39") = FormatNumber(ValNoComma(drow("DatCol39")), 2) 'AGENCY FEE
                newdatrow("DatCol40") = FormatNumber(ValNoComma(drow("DatCol40")), 2) '13th month pay
                '***********************************************************************************

                AbsTardiUTNDifOTHolipay = Nothing

                'newdatrow("DatCol39") = 0
                'newdatrow("DatCol40") = 0

                'newdatrow("DatCol40") = 0
                'newdatrow("DatCol41") = 0
                'newdatrow("DatCol42") = 0
                'newdatrow("DatCol43") = 0
                'newdatrow("DatCol44") = 0
                'newdatrow("DatCol45") = 0
                'newdatrow("DatCol46") = 0
                'newdatrow("DatCol47") = 0
                'newdatrow("DatCol48") = 0
                'newdatrow("DatCol49") = 0
                'newdatrow("DatCol50") = 0

                'newdatrow("DatCol50") = 0
                'newdatrow("DatCol51") = 0
                'newdatrow("DatCol52") = 0
                'newdatrow("DatCol53") = 0
                'newdatrow("DatCol54") = 0
                'newdatrow("DatCol55") = 0
                'newdatrow("DatCol56") = 0
                'newdatrow("DatCol57") = 0
                'newdatrow("DatCol58") = 0
                'newdatrow("DatCol59") = 0

                'newdatrow("DatCol60") = 0

                rptdattab.Rows.Add(newdatrow)

            Next

            If rptdattab Is Nothing Then
            Else

                Dim rptdoc As New PayrollSumma

                rptdoc.SetDataSource(rptdattab)

                Dim crvwr As New CrysVwr 'With {.ShowSubControls = True}

                crvwr.CrystalReportViewer1.ReportSource = rptdoc

                Dim papy_string = ""

                'If n_PayrollSummaDateSelection.DateFromID = _
                '    n_PayrollSummaDateSelection.DateToID Then

                '    papy_string = "for the period of " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))

                'Else
                papy_string = "for the period of " & Format(CDate(n_PayrollSummaDateSelection.DateFromstr), machineShortDateFormat) &
                    If(paypTo = Nothing, "", " to " & Format(CDate(n_PayrollSummaDateSelection.DateTostr), machineShortDateFormat))

                'End If

                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")

                objText.Text = orgNam

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgaddress")

                Dim obj_value = EXECQUER("SELECT CONCAT(IF(StreetAddress1 IS NULL,'',StreetAddress1)" &
                                         ",IF(StreetAddress2 IS NULL,'',CONCAT(', ',StreetAddress2))" &
                                         ",IF(Barangay IS NULL,'',CONCAT(', ',Barangay))" &
                                         ",IF(CityTown IS NULL,'',CONCAT(', ',CityTown))" &
                                         ",IF(Country IS NULL,'',CONCAT(', ',Country))" &
                                         ",IF(State IS NULL,'',CONCAT(', ',State)))" &
                                         " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" &
                                         " WHERE o.RowID=" & orgztnID &
                                         " AND o.PrimaryAddressID IS NOT NULL;")
                If obj_value = Nothing Then
                Else
                    objText.Text = CStr(obj_value)
                End If

                'Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                '                        ",',',COALESCE(FaxNumber,'')" &
                '                        ",',',COALESCE(EmailAddress,'')" &
                '                        ",',',COALESCE(TINNo,''))" &
                '                        " FROM organization WHERE RowID=" & orgztnID & ";")

                'Dim contactdet = Split(contactdetails, ",")

                'objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgcontactno")

                'If Trim(contactdet(0).ToString) = "" Then
                'Else
                '    objText.Text = "Contact No. " & contactdet(0).ToString
                'End If

                crvwr.Text = papy_string & " (" & PayrollSummaChosenData.ToUpper & ")"

                crvwr.Refresh()

                crvwr.Show()

            End If

        End If

        'Try

        '    'DatCol1

        '    'callProcAsDatTab

        'Catch ex As Exception
        '    MsgBox(getErrExcptn(ex, Me.Name))

        'End Try

    End Sub

    Private Sub tbppayroll_Enter(sender As Object, e As EventArgs) Handles tbppayroll.Enter

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Dim payfrqncy As New AutoCompleteStringCollection

            Dim sel_query = ""

            Dim hasAnEmployee = EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE OrganizationID=" & orgztnID & " LIMIT 1);")

            If hasAnEmployee = 1 Then
                sel_query = "SELECT pp.PayFrequencyType FROM payfrequency pp INNER JOIN employee e ON e.PayFrequencyID=pp.RowID GROUP BY pp.RowID;"
            Else
                sel_query = "SELECT PayFrequencyType FROM payfrequency WHERE PayFrequencyType IN ('SEMI-MONTHLY','WEEKLY');"
            End If

            enlistTheLists(sel_query, payfrqncy)

            Dim first_sender As New ToolStripButton

            Dim indx = 0

            For Each strval In payfrqncy

                Dim new_tsbtn As New ToolStripButton

                With new_tsbtn

                    .AutoSize = False
                    .BackColor = Color.FromArgb(255, 255, 255)
                    .ImageTransparentColor = System.Drawing.Color.Magenta
                    .Margin = New System.Windows.Forms.Padding(0, 1, 0, 1)
                    .Name = String.Concat("tsbtn" & strval)
                    .Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
                    .Size = New System.Drawing.Size(110, 30)
                    .Text = strval
                    .TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                    .TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
                    .ToolTipText = strval

                End With

                tstrip.Items.Add(new_tsbtn)

                If indx = 0 Then
                    indx = 1
                    first_sender = new_tsbtn
                End If

                AddHandler new_tsbtn.Click, AddressOf PayFreq_Changed

            Next

            tstrip.PerformLayout()

            If first_sender IsNot Nothing Then
                PayFreq_Changed(first_sender, New EventArgs)
            End If

            For Each tsItem As ToolStripItem In tstrip.Items
                tsItem.PerformClick()
                Exit For
            Next

            'RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

            dgvpayper_SelectionChanged(dgvpayper, New EventArgs)

            AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        End If

    End Sub

    Sub PayFreq_Changed(sender As Object, e As EventArgs)

        quer_empPayFreq = ""

        Dim senderObj As New ToolStripButton

        Static prevObj As New ToolStripButton

        Static once As SByte = 0

        senderObj = DirectCast(sender, ToolStripButton)

        If once = 0 Then

            once += 1

            prevObj = senderObj

            senderObj.BackColor = Color.FromArgb(194, 228, 255)

            senderObj.Font = selectedButtonFont

            ''RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            quer_empPayFreq = " AND pf.PayFrequencyType='" & senderObj.Text & "' "

            loademployee(quer_empPayFreq)

            ''AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            Exit Sub

        End If

        If prevObj.Name = Nothing Then
        Else

            If prevObj.Name <> senderObj.Name Then

                prevObj.BackColor = Color.FromArgb(255, 255, 255)

                prevObj.Font = unselectedButtonFont

                prevObj = senderObj

            End If

        End If

        senderObj.BackColor = Color.FromArgb(194, 228, 255)

        senderObj.Font = selectedButtonFont

        Dim prev_selRowIndex = -1

        If dgvemployees.RowCount <> 0 Then
            Try
                prev_selRowIndex = dgvemployees.CurrentRow.Index
            Catch ex As Exception
                prev_selRowIndex = -1
            End Try
        End If

        quer_empPayFreq = String.Empty

        If tsSearch.Text.Trim.Length = 0 Then
            'quer_empPayFreq = " AND pf.PayFrequencyType='" & senderObj.Text & "' "
            tsbtnSearch_Click(sender, e)

        ElseIf 1 = 2 Then 'ElseIf IsUserPressEnterToSearch Then

            ''RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            quer_empPayFreq = " AND pf.PayFrequencyType='" & senderObj.Text & "' AND e.EmployeeID='" & tsSearch.Text & "' "

            loademployee(quer_empPayFreq)

            If prev_selRowIndex <> -1 Then
                If dgvemployees.RowCount > prev_selRowIndex Then
                    dgvemployees.Item("EmployeeID", prev_selRowIndex).Selected = True
                End If
            End If

            ''AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            'dgvemployees_SelectionChanged(sender, e)

            Static twice As SByte = 0

            If twice < 1 Then

                twice += 1

            ElseIf twice = 1 Then
                twice = 2
                ''RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

                'dgvpayper_SelectionChanged(sender, e)

                ''AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

            End If

        End If

    End Sub

    '********************************************************
    '*********             CONTEXT MENU             *********
    '********************************************************

    Private Sub cms1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cms1.Opening

        If dgvpayper.RowCount <> 0 Then
            ToolStripMenuItem1.Enabled = True
        Else
            ToolStripMenuItem1.Enabled = False

        End If

    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

        If dgvpayper.RowCount <> 0 Then

            With dgvpayper.CurrentRow

                paypRowID = .Cells("Column1").Value

                paypFrom = Format(CDate(.Cells("Column2").Value), "yyyy-MM-dd")

                paypTo = Format(CDate(.Cells("Column3").Value), "yyyy-MM-dd")

                'Dim sel_yearDateFrom = CDate(PayStub.paypFrom).Year

                'Dim sel_yearDateTo = CDate(PayStub.paypTo).Year

                'Dim sel_year = If(sel_yearDateFrom > sel_yearDateTo, _
                '                  sel_yearDateFrom, _
                '                  sel_yearDateTo)

                isEndOfMonth = Trim(.Cells("Column14").Value)

                genpayselyear = Format(CDate(.Cells("Column2").Value), "yyyy")

                numofweekdays = 0

                numofweekends = 0

                Dim date_diff = DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo))

                For i = 0 To date_diff

                    Dim DayOfWeek = CDate(paypFrom).AddDays(i)

                    If DayOfWeek.DayOfWeek = 0 Then 'System.DayOfWeek.Sunday
                        numofweekends += 1

                    ElseIf DayOfWeek.DayOfWeek = 6 Then 'System.DayOfWeek.Saturday
                        numofweekends += 1
                    Else
                        numofweekdays += 1

                    End If

                Next

                withthirteenthmonthpay = 0

                If Format(CDate(.Cells("Column3").Value), "MM") = "12" Then

                    Dim prompt = MessageBox.Show("Do you want to include the calculation of Thirteenth month pay ?", "Thirteenth month pay calculation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information)

                    If prompt = Windows.Forms.DialogResult.Yes Then

                        'withthirteenthmonthpay = 1

                    ElseIf prompt = Windows.Forms.DialogResult.No Then

                    ElseIf prompt = Windows.Forms.DialogResult.Cancel Then
                        'Exit Sub
                    End If
                Else

                End If

                Dim PayFreqRowID = EXECQUER("SELECT RowID FROM payfrequency WHERE PayFrequencyType='" & Trim(.Cells("Column12").Value) & "';")

                'genpayroll(PayFreqRowID)

            End With

        End If

    End Sub

    Sub InsertPaystubAdjustment(paystubID As Integer, productID As Integer, payAmount As Double)
        Try

            If conn.State = ConnectionState.Open Then
                conn.Close()
                'Else
            End If

            new_cmd = New MySqlCommand("INSUPD_paystub", conn)

            conn.Open()

            With new_cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("paystubID", MySqlDbType.Int32)

                .Parameters.AddWithValue("pstub_RowID", DBNull.Value)
                .Parameters.AddWithValue("pstub_OrganizationID", orgztnID)
                .Parameters.AddWithValue("pstub_CreatedBy", z_User)
                .Parameters.AddWithValue("pstub_LastUpdBy", z_User)
                '.Parameters.AddWithValue("pstub_PayPeriodID", pstub_PayPeriodID)
                '.Parameters.AddWithValue("pstub_EmployeeID", etent_EmployeeID)

                '.Parameters.AddWithValue("pstub_TimeEntryID", DBNull.Value)

                '.Parameters.AddWithValue("pstub_PayFromDate", If(pstub_PayFromDate = Nothing, DBNull.Value, Format(CDate(pstub_PayFromDate), "yyyy-MM-dd")))
                '.Parameters.AddWithValue("pstub_PayToDate", If(pstub_PayToDate = Nothing, DBNull.Value, Format(CDate(pstub_PayToDate), "yyyy-MM-dd")))

                '.Parameters.AddWithValue("pstub_TotalGrossSalary", pstub_TotalGrossSalary)
                '.Parameters.AddWithValue("pstub_TotalNetSalary", pstub_TotalNetSalary)
                '.Parameters.AddWithValue("pstub_TotalTaxableSalary", pstub_TotalTaxableSalary)
                '.Parameters.AddWithValue("pstub_TotalEmpWithholdingTax", pstub_TotalEmpWithholdingTax)

                '.Parameters.AddWithValue("pstub_TotalEmpSSS", pstub_TotalEmpSSS) 'DBNull.Value
                '.Parameters.AddWithValue("pstub_TotalCompSSS", pstub_TotalCompSSS)
                '.Parameters.AddWithValue("pstub_TotalEmpPhilhealth", pstub_TotalEmpPhilhealth)
                '.Parameters.AddWithValue("pstub_TotalCompPhilhealth", pstub_TotalCompPhilhealth)
                '.Parameters.AddWithValue("pstub_TotalEmpHDMF", pstub_TotalEmpHDMF)
                '.Parameters.AddWithValue("pstub_TotalCompHDMF", pstub_TotalCompHDMF)
                '.Parameters.AddWithValue("pstub_TotalVacationDaysLeft", pstub_TotalVacationDaysLeft)
                '.Parameters.AddWithValue("pstub_TotalLoans", pstub_TotalLoans)
                '.Parameters.AddWithValue("pstub_TotalBonus", pstub_TotalBonus)
                '.Parameters.AddWithValue("pstub_TotalAllowance", pstub_TotalAllowance)

                .Parameters("paystubID").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()

                'INSUPD_paystub = datread(0)

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " " & "INSUPD_paystub", , "Error")
        Finally
            new_conn.Close()
            conn.Close()
            new_cmd.Dispose()
        End Try

    End Sub

    Private Sub dgAdjustments_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAdjustments.CellContentClick
        If TypeOf dgvAdjustments.Columns(e.ColumnIndex) Is DataGridViewLinkColumn _
            AndAlso e.RowIndex >= 0 Then
            Dim n_ExecuteQuery As New ExecuteQuery("SELECT PartNo FROM product WHERE RowID='" & dgvAdjustments.Item("cboProducts", e.RowIndex).Value & "' LIMIT 1;")
            Dim item_name As String = n_ExecuteQuery.Result
            Dim prompt = MessageBox.Show("Are you sure you want to delete '" & item_name & "'" & If(item_name.ToLower.Contains("adjustment"), "", " adjustment") & " ?",
                                         "Delete adjustment", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If prompt = Windows.Forms.DialogResult.Yes Then
                Dim SQLTableName As String = If(dgvAdjustments.Item("IsAdjustmentActual", e.RowIndex).Value = 0, "paystubadjustment", "paystubadjustmentactual")
                Dim del_quer As String =
                    "DELETE FROM " & SQLTableName & " WHERE RowID='" & dgvAdjustments.Item("psaRowID", e.RowIndex).Value & "';" &
                    "ALTER TABLE " & SQLTableName & " AUTO_INCREMENT = 0;"
                n_ExecuteQuery = New ExecuteQuery(del_quer)
                dgvAdjustments.Rows.Remove(dgvAdjustments.Rows(e.RowIndex))
                'btndiscardchanges_Click(btndiscardchanges, New EventArgs)
                dgvemployees_SelectionChanged(dgvemployees, New EventArgs)
                btnSaveAdjustments.Enabled = False
            End If
            'dgvAdjustments.Rows.RemoveAt(e.RowIndex)'UpdateSaveAdjustmentButtonDisable()
        End If
    End Sub

    Sub btnSaveAdjustments_Click(sender As Object, e As EventArgs) Handles btnSaveAdjustments.Click
        dgvAdjustments.EndEdit(True)
        'MessageBox.Show(Me.currentEmployeeID & " " & dgvpayper.SelectedRows(0).Cells(0).Value)
        '
        'Dim comboBox As New ComboBox
        'Dim amountTextBox As New TextBox

        Dim hasError As Boolean = False
        Dim errorRow As New DataGridViewRow
        Dim lastRow As Integer = dgvAdjustments.Rows.Count

        Dim rowCount As Integer = 0
        'For Each dgvRow As DataGridViewRow In dgvAdjustments.Rows
        '    rowCount += 1
        '    If (IsNothing(dgvRow.Cells(0).Value) Or IsDBNull(dgvRow.Cells(0).Value)) AndAlso
        '        (IsNothing(dgvRow.Cells(1).Value) Or IsDBNull(dgvRow.Cells(1).Value)) AndAlso
        '         (IsNothing(dgvRow.Cells(2).Value) Or IsDBNull(dgvRow.Cells(2).Value)) Then
        '        If rowCount <> lastRow Then
        '            dgvRow.Selected = True
        '            hasError = True
        '            MsgBox("Complete the form first")
        '            Exit For
        '        End If
        '    Else
        '        If Not (IsNothing(dgvRow.Cells(0).Value) Or IsDBNull(dgvRow.Cells(0).Value)) AndAlso
        '            Not (IsNothing(dgvRow.Cells(1).Value) Or IsDBNull(dgvRow.Cells(1).Value)) Then
        '            If IsNumeric(dgvRow.Cells(1).Value) Then
        '                ' MessageBox.Show("EmployeeID: (" & Me.currentEmployeeID & ") PayPeriod: (" & dgvpayper.SelectedRows(0).Cells(0).Value & ")")
        '            Else
        '                dgvRow.Selected = True
        '                hasError = True
        '                MsgBox("To save a row, a product and an amount must be provided")
        '                Exit For
        '            End If
        '        Else
        '            dgvRow.Selected = True
        '            hasError = True
        '            MsgBox("To save a row, a product and an amount must be provided")
        '            Exit For
        '        End If

        '    End If
        'Next
        Dim comment_columnname = "DataGridViewTextBoxColumn64"
        If Not hasError Then
            Try
                'EXECQUER("DELETE FROM paystubadjustment WHERE PayStubID = FN_GetPayStubIDByEmployeeIDAndPayPeriodID('" & Me.currentEmployeeID & "', " & dgvpayper.SelectedRows(0).Cells(0).Value & ",'" & orgztnID & "');ALTER TABLE paystubadjustment AUTO_INCREMENT = 0;")
                Dim SQLFunctionName As String = If(ValNoComma(tabEarned.SelectedIndex) = 0, "I_paystubadjustment", "I_paystubadjustmentactual")
                For Each dgvRow As DataGridViewRow In dgvAdjustments.Rows
                    Dim productRowID = dgvRow.Cells("cboProducts").Value
                    If productRowID IsNot Nothing And dgvRow.IsNewRow = False Then 'If Not dgvRow.Cells(0).Value Is Nothing AndAlso Not dgvRow.Cells(1).Value Is Nothing AndAlso IsNumeric(dgvRow.Cells(1).Value) Then
                        If TypeOf dgvRow.Cells("cboProducts").Value Is String Then
                            productRowID =
                            EXECQUER("SELECT RowID FROM product WHERE OrganizationID='" & orgztnID & "' AND PartNo='" & dgvRow.Cells("cboProducts").Value & "' LIMIT 1;")
                        End If
                        'SavePaystubAdjustments(productRowID,ValNoComma(dgvRow.Cells("DataGridViewTextBoxColumn66").Value),If(IsNothing(dgvRow.Cells(comment_columnname).Value) Or IsDBNull(dgvRow.Cells(comment_columnname).Value), "", dgvRow.Cells(comment_columnname).Value),dgvRow.Cells("psaRowID").Value)
                        Dim n_ReadSQLFunction As _
                            New ReadSQLFunction(SQLFunctionName,
                                                "returnvalue",
                                                orgztnID,
                                                z_User,
                                                productRowID,
                                                ValNoComma(dgvRow.Cells("DataGridViewTextBoxColumn66").Value),
                                                dgvRow.Cells("DataGridViewTextBoxColumn64").Value,
                                                Me.currentEmployeeID,
                                                paypRowID,
                                                dgvRow.Cells("psaRowID").Value)
                        If ValNoComma(dgvRow.Cells("psaRowID").Value) = 0 And n_ReadSQLFunction.HasError = False Then
                            dgvRow.Cells("psaRowID").Value = n_ReadSQLFunction.ReturnValue
                        End If
                    Else : Continue For
                    End If
                Next
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Me.Name))
            Finally
                UpdatePaystubAdjustmentColumn()
                MsgBox("Adjustments were saved!", MsgBoxStyle.Information)
                dgvemployees_SelectionChanged(sender, e)
            End Try
        End If
    End Sub

    Private Sub UpdatePaystubAdjustmentColumn()
        Dim _conn As New MySqlConnection
        _conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString
        Try
            If _conn.State = ConnectionState.Open Then : _conn.Close() : End If
            _conn.Open()

            cmd = New MySqlCommand("SP_UpdatePaystubAdjustment", _conn)

            With cmd
                .Parameters.Clear()
                .CommandType = CommandType.StoredProcedure

                .Parameters.AddWithValue("pa_EmployeeID", Me.currentEmployeeID)
                .Parameters.AddWithValue("pa_PayPeriodID", dgvpayper.SelectedRows(0).Cells(0).Value)
                .Parameters.AddWithValue("User_RowID", z_User)
                .Parameters.AddWithValue("Og_RowID", z_OrganizationID)
            End With

            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally
            _conn.Close()
            cmd.Dispose()
        End Try
    End Sub

    Private Sub UpdateAdjustmentDetails(Optional IsActual As Boolean = False)
        Try
            dtJosh = New DataTable
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            conn.Open()

            cmd = New MySqlCommand("VIEW_paystubadjustment", conn)
            With cmd
                .Parameters.Clear()
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("pa_EmployeeID", Me.currentEmployeeID)
                If dgvpayper.RowCount > 0 Then
                    .Parameters.AddWithValue("pa_PayPeriodID", dgvpayper.SelectedRows(0).Cells(0).Value)
                Else
                    .Parameters.AddWithValue("pa_PayPeriodID", DBNull.Value)
                End If
                .Parameters.AddWithValue("pa_OrganizationID", orgztnID)
                .Parameters.AddWithValue("pa_IsActual", Convert.ToInt16(IsActual))
            End With
            da = New MySqlDataAdapter(cmd)
            da.Fill(dtJosh)
            dgvAdjustments.DataSource = dtJosh

            'txtnetsal.Text = FormatNumber(currentTotal + Convert.ToDouble(GET_SumPayStubAdjustments()), 2)
        Catch ex As Exception
            dgvAdjustments.Rows.Clear()
        Finally
            conn.Close()

            cmd.Dispose()

            da.Dispose()

        End Try

    End Sub

    Private Sub dgvAdjustments_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvAdjustments.DataError
        btnSaveAdjustments.Enabled = False
    End Sub

    Private Sub dgvAdjustments_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAdjustments.CellEndEdit
        UpdateSaveAdjustmentButtonDisable()
    End Sub

    Sub SavePaystubAdjustments(productID As String, payAmount As String, comment As String)
        Try
            dtJosh = New DataTable
            If conn.State = ConnectionState.Open Then : conn.Close() : End If
            conn.Open()

            cmd = New MySqlCommand("I_paystubadjustment", conn)
            With cmd
                .Parameters.Clear()
                .CommandType = CommandType.StoredProcedure

                .Parameters.AddWithValue("pa_OrganizationID", z_OrganizationID)
                .Parameters.AddWithValue("pa_CurrentUser", z_User)
                .Parameters.AddWithValue("pa_ProductID", productID)
                .Parameters.AddWithValue("pa_PayAmount", payAmount)
                .Parameters.AddWithValue("pa_Comment", comment)
                .Parameters.AddWithValue("pa_EmployeeID", Me.currentEmployeeID)

                If dgvpayper.RowCount <> 0 Then
                    .Parameters.AddWithValue("pa_PayPeriodID", dgvpayper.SelectedRows(0).Cells(0).Value)
                Else
                    .Parameters.AddWithValue("pa_PayPeriodID", DBNull.Value)
                End If

            End With

            cmd.ExecuteNonQuery()
        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))
        Finally

            conn.Close()

            cmd.Dispose()

        End Try

    End Sub

    Private Sub UpdateSaveAdjustmentButtonDisable()
        Dim hasError As Boolean = False
        Dim lastRow As Integer = dgvAdjustments.Rows.Count

        Dim rowCount As Integer = 0
        'For Each dgvRow As DataGridViewRow In dgvAdjustments.Rows
        '    rowCount += 1
        '    If (IsNothing(dgvRow.Cells(0).Value) Or IsDBNull(dgvRow.Cells(0).Value)) AndAlso
        '        (IsNothing(dgvRow.Cells(1).Value) Or IsDBNull(dgvRow.Cells(1).Value)) AndAlso
        '         (IsNothing(dgvRow.Cells(2).Value) Or IsDBNull(dgvRow.Cells(2).Value)) Then
        '        If rowCount <> lastRow Then
        '            dgvRow.Selected = True
        '            hasError = True
        '            ' MsgBox("Complete the form first")
        '            Exit For
        '        End If
        '    Else
        '        If Not (IsNothing(dgvRow.Cells(0).Value) Or IsDBNull(dgvRow.Cells(0).Value)) AndAlso
        '            Not (IsNothing(dgvRow.Cells(1).Value) Or IsDBNull(dgvRow.Cells(1).Value)) Then
        '            If IsNumeric(dgvRow.Cells(1).Value) Then
        '                ' MessageBox.Show("EmployeeID: (" & Me.currentEmployeeID & ") PayPeriod: (" & dgvpayper.SelectedRows(0).Cells(0).Value & ")")
        '            Else
        '                dgvRow.Selected = True
        '                hasError = True
        '                ' MsgBox("To save a row, a product and an amount must be provided")
        '                Exit For
        '            End If
        '        Else
        '            dgvRow.Selected = True
        '            hasError = True
        '            'MsgBox("To save a row, a product and an amount must be provided")
        '            Exit For
        '        End If

        '    End If
        'Next

        btnSaveAdjustments.Enabled = Not hasError
    End Sub

    Private Sub tabEarned_DrawItem(sender As Object, e As DrawItemEventArgs) Handles tabEarned.DrawItem

        TabControlColor(tabEarned, e)

    End Sub

    Private Sub TabPage1_Enter1(sender As Object, e As EventArgs) Handles TabPage1.Enter 'DECLARED
        For Each txtbxctrl In TabPage1.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag

        Dim n_SQLQueryToDatatable As _
            New ReadSQLProcedureToDatatable("VIEW_paystubitem_declared",
                                            orgztnID,
                                            EmployeeRowID,
                                            paypFrom,
                                            paypTo)

        Dim paystubactual = n_SQLQueryToDatatable.ResultTable

        For Each drow As DataRow In paystubactual.Rows
            Dim psaItems = New SQLQueryToDatatable("CALL VIEW_paystubitem('" & ValNoComma(drow("RowID")) & "');").ResultTable

            Dim strdouble = ValNoComma(drow("TrueSalary")) / ValNoComma(drow("PAYFREQUENCYDIVISOR")) 'BasicPay

            txtBasicPay.Text = FormatNumber(ValNoComma(strdouble), 2)

            txtRegularHours.Text = ValNoComma(drow("RegularHours"))

            If drow("EmployeeType").ToString = "Fixed" Then
                txtRegularPay.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then
                Dim basicPay = ValNoComma(drow("BasicPay"))
                Dim deductions = 0.0

                If drow("FirstTimeSalary").ToString = "1" Then
                    basicPay = ValNoComma(drow("RegularPay"))
                Else
                    basicPay = ValNoComma(drow("BasicPay"))
                    deductions = ValNoComma(drow("LateDeduction")) +
                        ValNoComma(drow("UndertimeDeduction")) +
                        ValNoComma(drow("Absent")) +
                        ValNoComma(drow("HolidayPay"))
                End If

                txtRegularPay.Text = FormatNumber(basicPay - deductions, 2)
            ElseIf drow("EmployeeType").ToString = "Daily" Then
                txtRegularPay.Text = FormatNumber(ValNoComma(drow("RegularPay")), 2)
            End If

            txtOvertimeHours.Text = ValNoComma(drow("OvertimeHours"))
            txtOvertimePay.Text = FormatNumber(ValNoComma(drow("OvertimePay")), 2)

            txtNightDiffHours.Text = ValNoComma(drow("NightDiffHours"))
            txtNightDiffPay.Text = FormatNumber(ValNoComma(drow("NightDiffPay")), 2)

            txtNightDiffOvertimeHours.Text = ValNoComma(drow("NightDiffOvertimeHours"))
            txtNightDiffOvertimePay.Text = FormatNumber(ValNoComma(drow("NightDiffOvertimePay")), 2)

            txtRestDayHours.Text = ValNoComma(drow("RestDayHours"))
            txtRestDayAmount.Text = ValNoComma(drow("RestDayPay"))

            txtHolidayHours.Text = 0.0
            txtHolidayPay.Text = FormatNumber(ValNoComma(drow("HolidayPay")), 2)

            Dim sumallbasic = ValNoComma(drow("RegularPay")) +
                              ValNoComma(drow("OvertimePay")) +
                              ValNoComma(drow("NightDiffPay")) +
                              ValNoComma(drow("NightDiffOvertimePay")) +
                              ValNoComma(drow("HolidayPay"))
            'lblsubtot.Text = FormatNumber((ValNoComma(sumallbasic)), 2)
            'lblsubtot.Text = FormatNumber((ValNoComma(drow("TotalDayPay"))), 2)
            If drow("EmployeeType").ToString = "Fixed" Then
                lblSubtotal.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then
                'Dim thebasicpay = ValNoComma(drow("BasicPay"))
                'Dim thelessamounts = ValNoComma(drow("HoursLateAmount")) + ValNoComma(drow("UndertimeHoursAmount")) + ValNoComma(drow("Absent"))

                'txthrsworkamt.Text = FormatNumber((thebasicpay - thelessamounts), 2)
                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "1" Then
                    thebasicpay = ValNoComma(drow("RegularPay"))
                    lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
                Else
                    thebasicpay = ValNoComma(drow("BasicPay"))
                    thelessamounts = ValNoComma(drow("LateDeduction")) + ValNoComma(drow("UndertimeDeduction")) + ValNoComma(drow("Absent"))
                    Dim all_regular = (thebasicpay - (thelessamounts + ValNoComma(drow("HolidayPay"))))
                    lblSubtotal.Text = FormatNumber(all_regular + ValNoComma(drow("HolidayPay")), 2)
                End If

                'lblsubtot.Text = FormatNumber((thebasicpay - thelessamounts), 2)
            Else
                lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)

            End If

            'Absent
            txttotabsent.Text = 0.0
            txttotabsentamt.Text = FormatNumber(ValNoComma((drow("Absent"))), 2)
            'Tardiness / late
            txttottardi.Text = ValNoComma(drow("LateHours"))
            txttottardiamt.Text = FormatNumber(ValNoComma((drow("LateDeduction"))), 2)
            'Undertime
            txttotut.Text = ValNoComma(drow("UndertimeHours"))
            txttotutamt.Text = FormatNumber(ValNoComma((drow("UndertimeDeduction"))), 2)

            Dim miscsubtotal = ValNoComma(drow("Absent")) + ValNoComma(drow("LateDeduction")) + ValNoComma(drow("UndertimeDeduction"))
            lblsubtotmisc.Text = FormatNumber(ValNoComma((miscsubtotal)), 2)

            'Allowance
            txtemptotallow.Text = FormatNumber(ValNoComma((drow("TotalAllowance"))), 2)
            'Bonus
            txtemptotbon.Text = FormatNumber(ValNoComma((drow("TotalBonus"))), 2)
            'Gross
            txtgrosssal.Text = FormatNumber(ValNoComma((drow("TotalGrossSalary"))), 2)

            'SSS
            txtempsss.Text = FormatNumber(ValNoComma((drow("TotalEmpSSS"))), 2)
            'PhilHealth
            txtempphh.Text = FormatNumber(ValNoComma((drow("TotalEmpPhilhealth"))), 2)
            'PAGIBIG
            txtemphdmf.Text = FormatNumber(ValNoComma((drow("TotalEmpHDMF"))), 2)

            'Taxable salary
            txttaxabsal.Text = FormatNumber(ValNoComma((drow("TotalTaxableSalary"))), 2)
            'Withholding taxS
            txtempwtax.Text = FormatNumber(ValNoComma((drow("TotalEmpWithholdingTax"))), 2)
            'Loans
            txtemptotloan.Text = FormatNumber(ValNoComma((drow("TotalLoans"))), 2)
            'Adjustments
            txtTotalAdjustments.Text = FormatNumber(ValNoComma((drow("TotalAdjustments"))), 2)

            Dim totalAgencyFee = ValNoComma(drow("TotalAgencyFee"))
            txtAgencyFee.Text = FormatNumber(totalAgencyFee, 2)

            Dim thirteenthMonthPay = ValNoComma(drow("ThirteenthMonthPay"))
            txtThirteenthMonthPay.Text = FormatNumber(thirteenthMonthPay, 2)

            Dim totalNetSalary = ValNoComma(drow("TotalNetSalary")) + totalAgencyFee
            'Net
            txtnetsal.Text = FormatNumber(totalNetSalary, 2)

            Dim totalNetPay = totalNetSalary + thirteenthMonthPay
            txtTotalNetPay.Text = FormatNumber(totalNetPay, 2)

            'LEAVE BALANCES
            txtvlbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Vacation leave'")) ' -
            'ValNoComma(drow("VacationLeaveHours"))

            txtslbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Sick leave'")) ' -
            'ValNoComma(drow("SickLeaveHours"))

            txtmlbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Maternity/paternity leave'")) ' -

            txtPaidLeave.Text = FormatNumber(ValNoComma(drow("PaidLeaveAmount")), 2)
            'txtPaidLeaveHrs.Text = FormatNumber(ValNoComma(drow("PaidLeaveHours")), 2)

            Exit For
        Next

        paystubactual.Dispose()

        UpdateAdjustmentDetails(Convert.ToInt16(DirectCast(sender, TabPage).Tag))

    End Sub

    Private Sub TabPage1_Enter(sender As Object, e As EventArgs) Handles TabPage1.Enter 'DECLARED
        TabPage1.Text = TabPage1.Text.Trim

        TabPage1.Text = TabPage1.Text & Space(15)

        TabPage4.Text = TabPage4.Text.Trim

        'dgvemployees_SelectionChanged(dgvemployees, New EventArgs)

    End Sub

    Private Function UnroundDecimal(DoubleValue As Object,
                                    Optional DecimalPlace As Integer = 2) As Double

        Dim strdouble = ValNoComma(DoubleValue).ToString

        Dim dot_index = strdouble.LastIndexOf(".")

        If dot_index > 0 Then
            dot_index += DecimalPlace
        ElseIf dot_index < 0 Then
            dot_index = strdouble.Length
        End If

        If dot_index > strdouble.Length Then
            dot_index = strdouble.Length
        End If

        Dim strresult = strdouble.Substring(0, dot_index)

        Return ValNoComma(strresult)

    End Function

    Private Sub TabPage4_Enter1(sender As Object, e As EventArgs) Handles TabPage4.Enter 'UNDECLARED / ACTUAL

        TabPage4.Text = TabPage4.Text.Trim

        TabPage4.Text = TabPage4.Text & Space(15)

        TabPage1.Text = TabPage1.Text.Trim

        'For Each ctrl As Control In TabPage4.Controls
        '    If TypeOf ctrl Is TextBox Then

        '        'Dim contentstring = DirectCast(ctrl, TextBox).Text & "@" & ctrl.Name & ".Text = 0" & Environment.NewLine

        '        File.AppendAllText("C:\Users\GLOBAL-D\Desktop\UNDECLAREDTextBoxObject.txt",
        '                           ctrl.Name & ".Text = 0" & Environment.NewLine)

        '        File.AppendAllText("C:\Users\GLOBAL-D\Desktop\UNDECLAREDTextBoxObject.txt",
        '                           DirectCast(ctrl, TextBox).Text & Environment.NewLine)

        '    Else
        '        Continue For
        '    End If
        'Next

        For Each txtbxctrl In TabPage4.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag

        'Dim n_SQLQueryToDatatable As _
        '    New SQLQueryToDatatable("SELECT psa.*" &
        '                            ",(es.BasicPay + (es.BasicPay * GET_employeeundeclaredsalarypercent(es.EmployeeID,es.OrganizationID,'" & paypFrom & "','" & paypTo & "'))) AS BasicPay" &
        '                            ",es.TrueSalary" &
        '                            ",IF(e.EmployeeType='Daily',PAYFREQUENCY_DIVISOR(e.EmployeeType),PAYFREQUENCY_DIVISOR(pf.PayFrequencyType)) AS PAYFREQUENCYDIVISOR" &
        '                            ",ete.*" &
        '                            ",e.EmployeeType" &
        '                            ",(e.StartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "') AS FirstTimeSalary" &
        '                            " FROM paystubactual psa" &
        '                            " INNER JOIN employee e ON e.RowID=psa.EmployeeID AND e.OrganizationID=psa.OrganizationID" &
        '                            " INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID" &
        '                            " INNER JOIN employeesalary es" &
        '                                " ON es.EmployeeID=psa.EmployeeID" &
        '                                " AND es.OrganizationID=psa.OrganizationID" &
        '                                " AND (es.EffectiveDateFrom >= psa.PayFromDate OR IFNULL(es.EffectiveDateTo,CURDATE()) >= psa.PayFromDate)" &
        '                                " AND (es.EffectiveDateFrom <= psa.PayToDate OR IFNULL(es.EffectiveDateTo,CURDATE()) <= psa.PayToDate)" &
        '                            " INNER JOIN (SELECT RowID AS eteRowID" &
        '                                        ",SUM(RegularHoursWorked) AS RegularHoursWorked" &
        '                                        ",SUM(RegularHoursAmount) AS RegularHoursAmount" &
        '                                        ",SUM(TotalHoursWorked) AS TotalHoursWorked" &
        '                                        ",SUM(OvertimeHoursWorked) AS OvertimeHoursWorked" &
        '                                        ",SUM(OvertimeHoursAmount) AS OvertimeHoursAmount" &
        '                                        ",SUM(UndertimeHours) AS UndertimeHours" &
        '                                        ",SUM(UndertimeHoursAmount) AS UndertimeHoursAmount" &
        '                                        ",SUM(NightDifferentialHours) AS NightDifferentialHours" &
        '                                        ",SUM(NightDiffHoursAmount) AS NightDiffHoursAmount" &
        '                                        ",SUM(NightDifferentialOTHours) AS NightDifferentialOTHours" &
        '                                        ",SUM(NightDiffOTHoursAmount) AS NightDiffOTHoursAmount" &
        '                                        ",SUM(HoursLate) AS HoursLate" &
        '                                        ",SUM(HoursLateAmount) AS HoursLateAmount" &
        '                                        ",SUM(VacationLeaveHours) AS VacationLeaveHours" &
        '                                        ",SUM(SickLeaveHours) AS SickLeaveHours" &
        '                                        ",SUM(MaternityLeaveHours) AS MaternityLeaveHours" &
        '                                        ",SUM(OtherLeaveHours) AS OtherLeaveHours" &
        '                                        ",SUM(TotalDayPay) AS TotalDayPay" &
        '                                        ",SUM(Absent) AS Absent" &
        '                                        " FROM employeetimeentryactual" &
        '                                        " WHERE EmployeeID='" & EmployeeRowID & "' AND OrganizationID='" & orgztnID & "' AND `Date` BETWEEN '" & paypFrom & "' AND '" & paypTo & "') ete ON ete.eteRowID > 0" &
        '                            " WHERE psa.EmployeeID='" & EmployeeRowID & "'" &
        '                            " AND psa.OrganizationID='" & orgztnID & "'" &
        '                            " AND psa.PayFromDate='" & paypFrom & "'" &
        '                            " AND psa.PayToDate='" & paypTo & "'" &
        '                            " ORDER BY es.EffectiveDateFrom DESC" &
        '                            " LIMIT 1;")

        Dim n_SQLQueryToDatatable As _
            New ReadSQLProcedureToDatatable("VIEW_paystubitem_actual",
                                            orgztnID,
                                            EmployeeRowID,
                                            paypFrom,
                                            paypTo)

        Dim paystubactual As New DataTable

        paystubactual = n_SQLQueryToDatatable.ResultTable

        Dim psaItems As New DataTable

        For Each drow As DataRow In paystubactual.Rows

            psaItems = New SQLQueryToDatatable("CALL VIEW_paystubitemundeclared('" & ValNoComma(drow("RowID")) & "');").ResultTable

            Dim strdouble = ValNoComma(drow("TrueSalary")) / ValNoComma(drow("PAYFREQUENCYDIVISOR")) 'BasicPay

            'Basic Pay
            txtempbasicpay_U.Text = FormatNumber(ValNoComma(strdouble), 2)
            'Regular
            txthrswork_U.Text = ValNoComma(drow("RegularHoursWorked"))
            If drow("EmployeeType").ToString = "Fixed" Then

                txthrsworkamt_U.Text = FormatNumber(ValNoComma(strdouble), 2)

            ElseIf drow("EmployeeType").ToString = "Monthly" Then

                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "1" Then
                    thebasicpay = ValNoComma(drow("RegularHoursAmount"))
                Else
                    thebasicpay = ValNoComma(drow("BasicPay"))
                    thelessamounts = ValNoComma(drow("HoursLateAmount")) + ValNoComma(drow("UndertimeHoursAmount")) + ValNoComma(drow("Absent")) _
                        + ValNoComma(drow("HolidayPay"))
                End If

                txthrsworkamt_U.Text = FormatNumber((thebasicpay - thelessamounts), 2)
            Else
                txthrsworkamt_U.Text = FormatNumber(ValNoComma((drow("RegularHoursAmount"))), 2)
            End If
            'Over time
            txttotothrs_U.Text = ValNoComma(drow("OvertimeHoursWorked"))
            txttototamt_U.Text = FormatNumber(ValNoComma((drow("OvertimeHoursAmount"))), 2)
            'Night differential
            txttotnightdiffhrs_U.Text = ValNoComma(drow("NightDifferentialHours"))
            txttotnightdiffamt_U.Text = FormatNumber(ValNoComma((drow("NightDiffHoursAmount"))), 2)
            'Night differential Overtime
            txttotnightdiffothrs_U.Text = ValNoComma(drow("NightDifferentialOTHours"))
            txttotnightdiffotamt_U.Text = FormatNumber(ValNoComma((drow("NightDiffOTHoursAmount"))), 2)
            'Holiday pay
            txttotholidayhrs_U.Text = 0.0
            Dim holidaybayad = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Holiday pay'"))
            txttotholidayamt_U.Text = FormatNumber(holidaybayad, 2)

            txttotholidayamt_U.Text = FormatNumber(ValNoComma(drow("HolidayPay")), 2)

            Dim sumallbasic = ValNoComma(txthrsworkamt_U.Text) _
                              + ValNoComma(drow("OvertimeHoursAmount")) _
                              + ValNoComma(drow("NightDiffHoursAmount")) _
                              + ValNoComma(drow("NightDiffOTHoursAmount")) _
                              + holidaybayad
            'lblsubtot.Text = FormatNumber(ValNoComma(sumallbasic), 2)
            'lblsubtot.Text = FormatNumber((ValNoComma(drow("TotalDayPay"))), 2)
            If drow("EmployeeType").ToString = "Fixed" Then
                lblSubtotal.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then
                'Dim thebasicpay = ValNoComma(drow("BasicPay"))
                'Dim thelessamounts = ValNoComma(drow("HoursLateAmount")) + ValNoComma(drow("UndertimeHoursAmount")) + ValNoComma(drow("Absent"))

                'txthrsworkamt.Text = FormatNumber((thebasicpay - thelessamounts), 2)

                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "1" Then
                    thebasicpay = ValNoComma(drow("RegularHoursAmount"))
                    lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
                Else
                    thebasicpay = ValNoComma(drow("BasicPay"))
                    thelessamounts = ValNoComma(drow("HoursLateAmount")) + ValNoComma(drow("UndertimeHoursAmount")) + ValNoComma(drow("Absent"))
                    Dim all_regular = (thebasicpay - (thelessamounts + ValNoComma(drow("HolidayPay"))))
                    lblSubtotal.Text = FormatNumber(all_regular + ValNoComma(drow("HolidayPay")), 2)
                End If

                'lblsubtot.Text = FormatNumber((thebasicpay - thelessamounts), 2)
            Else
                lblSubtotal.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
            End If

            'Absent
            txttotabsent_U.Text = 0.0
            txttotabsentamt_U.Text = FormatNumber(ValNoComma((drow("Absent"))), 2)
            'Tardiness / late
            txttottardi_U.Text = ValNoComma(drow("HoursLate"))
            txttottardiamt_U.Text = FormatNumber(ValNoComma((drow("HoursLateAmount"))), 2)
            'Undertime
            txttotut_U.Text = ValNoComma(drow("UndertimeHours"))
            txttotutamt_U.Text = FormatNumber(ValNoComma((drow("UndertimeHoursAmount"))), 2)

            txtRestDayAmount.Text = FormatNumber(ValNoComma(drow("RestDayPay")), 0)

            Dim miscsubtotal = ValNoComma(drow("Absent")) + ValNoComma(drow("HoursLateAmount")) + ValNoComma(drow("UndertimeHoursAmount"))
            lblsubtotmisc.Text = FormatNumber(ValNoComma((miscsubtotal)), 2)

            'Allowance
            txtemptotallow.Text = FormatNumber(ValNoComma((drow("TotalAllowance"))), 2)
            'Bonus
            txtemptotbon.Text = FormatNumber(ValNoComma((drow("TotalBonus"))), 2)
            'Gross
            txtgrosssal.Text = FormatNumber(ValNoComma((drow("TotalGrossSalary"))), 2)

            'SSS
            txtempsss.Text = FormatNumber(ValNoComma((drow("TotalEmpSSS"))), 2)
            'PhilHealth
            txtempphh.Text = FormatNumber(ValNoComma((drow("TotalEmpPhilhealth"))), 2)
            'PAGIBIG
            txtemphdmf.Text = FormatNumber(ValNoComma((drow("TotalEmpHDMF"))), 2)

            'Taxable salary
            txttaxabsal.Text = FormatNumber(ValNoComma((drow("TotalTaxableSalary"))), 2)
            'Withholding taxS
            txtempwtax.Text = FormatNumber(ValNoComma((drow("TotalEmpWithholdingTax"))), 2)
            'Loans
            txtemptotloan.Text = FormatNumber(ValNoComma((drow("TotalLoans"))), 2)
            'Adjustments
            txtTotalAdjustments.Text = FormatNumber(ValNoComma((drow("TotalAdjustments"))), 2)

            Dim totalAgencyFee = ValNoComma(drow("TotalAgencyFee"))
            txtAgencyFee.Text = FormatNumber(totalAgencyFee, 2)

            Dim thirteenthMonthPay = ValNoComma(drow("ThirteenthMonthPay"))
            txtThirteenthMonthPay.Text = FormatNumber(thirteenthMonthPay, 2)

            Dim totalNetSalary = ValNoComma(drow("TotalNetSalary")) + totalAgencyFee
            'Net
            txtnetsal.Text = FormatNumber(totalNetSalary, 2)

            Dim totalNetPay = totalNetSalary + thirteenthMonthPay
            txtTotalNetPay.Text = FormatNumber(totalNetPay, 2)

            'LEAVE BALANCES
            txtvlbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Vacation leave'")) ' -
            'ValNoComma(drow("VacationLeaveHours"))

            txtslbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Sick leave'")) ' -
            'ValNoComma(drow("SickLeaveHours"))

            txtmlbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Maternity/paternity leave'")) ' -
            'ValNoComma(drow("MaternityLeaveHours"))

            'txtothrbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Others'")) -
            '    ValNoComma(drow("OtherLeaveHours"))

            txtPaidLeave.Text = FormatNumber(ValNoComma(drow("PaidLeaveAmount")), 2)
            'txtPaidLeaveHrs.Text = FormatNumber(ValNoComma(drow("PaidLeaveHours")), 2)

            Exit For
        Next

        paystubactual.Dispose()

        UpdateAdjustmentDetails(Convert.ToInt16(DirectCast(sender, TabPage).Tag))

    End Sub

    Private Sub TabPage4_Enter(sender As Object, e As EventArgs) 'Handles TabPage4.Enter 'UNDECLARED

        TabPage4_Enter1(TabPage4, New EventArgs)

        Dim i = 1

        If i = 1 Then
            Exit Sub
        End If

        TabPage4.Text = TabPage4.Text.Trim

        TabPage4.Text = TabPage4.Text & Space(15)

        TabPage1.Text = TabPage1.Text.Trim

        If dgvpayper.RowCount <> 0 And
            dgvemployees.RowCount <> 0 Then

            Dim undeclaredSalPercent =
            EXECQUER("SELECT `GET_employeeundeclaredsalarypercent`('" & dgvemployees.CurrentRow.Cells("RowID").Value & "'" &
                     ", '" & orgztnID & "'" &
                     ", '" & paypFrom & "'" &
                     ", '" & paypTo & "');")

            undeclaredSalPercent = ValNoComma(undeclaredSalPercent)

            Dim computed_val = Val(0)

            computed_val = ValNoComma(txtBasicPay.Text) * undeclaredSalPercent

            txtempbasicpay_U.Text = ValNoComma(txtBasicPay.Text) + (computed_val)
            txtempbasicpay_U.Text = FormatNumber(ValNoComma(txtempbasicpay_U.Text), 2)

            txthrswork_U.Text = txtRegularHours.Text

            computed_val = ValNoComma(txtRegularPay.Text) * undeclaredSalPercent

            txthrsworkamt_U.Text = ValNoComma(txtRegularPay.Text) + (computed_val)
            txthrsworkamt_U.Text = FormatNumber(ValNoComma(txthrsworkamt_U.Text), 2)

            txttotothrs_U.Text = txtOvertimeHours.Text

            computed_val = ValNoComma(txtOvertimePay.Text) * undeclaredSalPercent

            txttototamt_U.Text = ValNoComma(txtOvertimePay.Text) + (computed_val)
            txttototamt_U.Text = FormatNumber(ValNoComma(txttototamt_U.Text), 2)

            txttotnightdiffhrs_U.Text = txtNightDiffHours.Text

            computed_val = ValNoComma(txtNightDiffPay.Text) * undeclaredSalPercent

            txttotnightdiffamt_U.Text = ValNoComma(txtNightDiffPay.Text) + (computed_val)
            txttotnightdiffamt_U.Text = FormatNumber(ValNoComma(txttotnightdiffamt_U.Text), 2)

            txttotnightdiffothrs_U.Text = txtNightDiffOvertimeHours.Text

            computed_val = ValNoComma(txtNightDiffOvertimePay.Text) * undeclaredSalPercent

            txttotnightdiffotamt_U.Text = ValNoComma(txtNightDiffOvertimePay.Text) + (computed_val)
            txttotnightdiffotamt_U.Text = FormatNumber(ValNoComma(txttotnightdiffotamt_U.Text), 2)

            txttotholidayhrs_U.Text = txtHolidayHours.Text

            computed_val = ValNoComma(txtHolidayPay.Text) * undeclaredSalPercent

            txttotholidayamt_U.Text = ValNoComma(txtHolidayPay.Text) + (computed_val)
            txttotholidayamt_U.Text = FormatNumber(ValNoComma(txttotholidayamt_U.Text), 2)

            txttotabsent_U.Text = txttotabsent.Text

            computed_val = ValNoComma(txttotabsentamt.Text) * undeclaredSalPercent

            txttotabsentamt_U.Text = ValNoComma(txttotabsentamt.Text) + (computed_val)
            txttotabsentamt_U.Text = FormatNumber(ValNoComma(txttotabsentamt_U.Text), 2)

            txttottardi_U.Text = txttottardi.Text

            computed_val = ValNoComma(txttottardiamt.Text) * undeclaredSalPercent

            txttottardiamt_U.Text = ValNoComma(txttottardiamt.Text) + (computed_val)
            txttottardiamt_U.Text = FormatNumber(ValNoComma(txttottardiamt_U.Text), 2)

            txttotut_U.Text = txttotut.Text

            computed_val = ValNoComma(txttotutamt.Text) * undeclaredSalPercent

            txttotutamt_U.Text = ValNoComma(txttotutamt.Text) + (computed_val)
            txttotutamt_U.Text = FormatNumber(ValNoComma(txttotutamt_U.Text), 2)

            Static same_rowindex As Integer = -1

            Static same_rowindexPapPerd As Integer = -1

            Static keep_declaredvalues(20) As String

            If same_rowindex <> dgvemployees.CurrentRow.Index _
                Or same_rowindexPapPerd <> dgvpayper.CurrentRow.Index Then

                same_rowindex = dgvemployees.CurrentRow.Index

                same_rowindexPapPerd = dgvpayper.CurrentRow.Index

                keep_declaredvalues(0) = ValNoComma(lblSubtotal.Text)

                keep_declaredvalues(1) = ValNoComma(lblsubtotmisc.Text)

                keep_declaredvalues(2) = ValNoComma(txtgrosssal.Text)

                keep_declaredvalues(3) =
                EXECQUER("SELECT GET_paystubitemallowanceecola('" & orgztnID & "'" &
                         ", '" & dgvemployees.CurrentRow.Cells("RowID").Value & "'" &
                         ", '" & dgvpayper.CurrentRow.Cells("Column1").Value & "');")

                keep_declaredvalues(2) -= ValNoComma(keep_declaredvalues(3))

                keep_declaredvalues(4) = ValNoComma(txtnetsal.Text)

                keep_declaredvalues(5) = ValNoComma(txttaxabsal.Text)

            End If

            computed_val = ValNoComma(keep_declaredvalues(0)) * undeclaredSalPercent

            lblSubtotal.Text = ValNoComma(keep_declaredvalues(0)) + (computed_val)
            lblSubtotal.Text = FormatNumber(ValNoComma(lblSubtotal.Text), 2)

            computed_val = ValNoComma(keep_declaredvalues(1)) * undeclaredSalPercent

            lblsubtotmisc.Text = ValNoComma(keep_declaredvalues(1)) + (computed_val)
            lblsubtotmisc.Text = FormatNumber(ValNoComma(lblsubtotmisc.Text), 2)

            'txtemptotallow.Text = _
            '    EXECQUER("SELECT GET_paystubitemallowancenotecola('" & orgztnID & "'" &
            '             ", '" & dgvemployees.CurrentRow.Cells("RowID").Value & "'" &
            '             ", '" & dgvpayper.CurrentRow.Cells("Column1").Value & "');")

            computed_val = ValNoComma(keep_declaredvalues(2)) * undeclaredSalPercent

            txtgrosssal.Text = ValNoComma(keep_declaredvalues(2)) + (computed_val)
            txtgrosssal.Text = FormatNumber(ValNoComma(txtgrosssal.Text), 2)

            'computed_val = ValNoComma(keep_declaredvalues(5)) * undeclaredSalPercent

            'txttaxabsal.Text = ValNoComma(keep_declaredvalues(5)) + (computed_val)
            'txttaxabsal.Text = FormatNumber(ValNoComma(txttaxabsal.Text), 2)

            computed_val = ValNoComma(keep_declaredvalues(4)) * undeclaredSalPercent

            txtnetsal.Text = ValNoComma(keep_declaredvalues(4)) + (computed_val)
            txtnetsal.Text = FormatNumber(ValNoComma(txtnetsal.Text), 2)
        Else

            txtempbasicpay_U.Text = txtBasicPay.Text

            txthrswork_U.Text = txthrswork_U.Text
            txthrsworkamt_U.Text = txtRegularPay.Text

            txttotothrs_U.Text = txtOvertimeHours.Text
            txttototamt_U.Text = txtOvertimePay.Text

            txttotnightdiffhrs_U.Text = txtNightDiffHours.Text
            txttotnightdiffamt_U.Text = txtNightDiffPay.Text

            txttotnightdiffothrs_U.Text = txtNightDiffOvertimeHours.Text
            txttotnightdiffotamt_U.Text = txtNightDiffOvertimePay.Text

            txttotholidayhrs_U.Text = txtHolidayHours.Text
            txttotholidayamt_U.Text = txtHolidayPay.Text

            txttotabsent_U.Text = txttotabsent.Text
            txttotabsentamt_U.Text = txttotabsentamt.Text

            txttottardi_U.Text = txttottardi.Text
            txttottardiamt_U.Text = txttottardiamt.Text

            txttotut_U.Text = txttotut.Text
            txttotutamt_U.Text = txttotutamt.Text

            txttotut_U.Text = txttotut.Text
            txttotutamt_U.Text = txttotutamt.Text

        End If

    End Sub

    Private Sub bgwPrintAllPaySlip_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwPrintAllPaySlip.DoWork

        If dtprintAllPaySlip.Columns.Count = 0 Then

            For i = 1 To 120

                Dim n_col As New DataColumn

                n_col.ColumnName = "COL" & i

                dtprintAllPaySlip.Columns.Add(n_col)

            Next

        End If

        employee_dattab = retAsDatTbl("SELECT e.*" &
                                      ",CONCAT(UCASE(e.LastName),', ',UCASE(e.FirstName),', ',INITIALS(e.MiddleName,'.','1')) AS FullName" &
                                      " FROM employee e LEFT JOIN employeesalary esal ON e.RowID=esal.EmployeeID" &
                                      " WHERE e.OrganizationID=" & orgztnID &
                                      " AND CURDATE() BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,CURDATE())" &
                                      " GROUP BY e.RowID" &
                                      " ORDER BY e.LastName;") 'RowID DESC

        '" AND '" & paypTo & "' BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,'" & paypTo & "')" &

        Dim n_row As DataRow

        For Each drow As DataRow In employee_dattab.Rows

            n_row = dtprintAllPaySlip.NewRow

            n_row("COL1") = drow("EmployeeID")

            n_row("COL2") = drow("FullName")

            For ii = 3 To 120

                Dim datacol_name = "COL" & ii

                n_row(datacol_name) = "0.0"

            Next

            dtprintAllPaySlip.Rows.Add(n_row)

        Next

        rptdocAll = New rptAllDecUndecPaySlip

        With rptdocAll.ReportDefinition.Sections(2)

            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("OrgName")

            objText.Text = orgNam

            objText = .ReportObjects("OrgAddress")

            Dim orgaddress = EXECQUER("SELECT CONCAT(IF(StreetAddress1 IS NULL,'',StreetAddress1)" &
                                    ",IF(StreetAddress2 IS NULL,'',CONCAT(', ',StreetAddress2))" &
                                    ",IF(Barangay IS NULL,'',CONCAT(', ',Barangay))" &
                                    ",IF(CityTown IS NULL,'',CONCAT(', ',CityTown))" &
                                    ",IF(Country IS NULL,'',CONCAT(', ',Country))" &
                                    ",IF(State IS NULL,'',CONCAT(', ',State)))" &
                                    " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" &
                                    " WHERE o.RowID=" & orgztnID & ";")

            objText.Text = orgaddress

            Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                                    ",',',COALESCE(FaxNumber,'')" &
                                    ",',',COALESCE(EmailAddress,'')" &
                                    ",',',COALESCE(TINNo,''))" &
                                    " FROM organization WHERE RowID=" & orgztnID & ";")

            Dim contactdet = Split(contactdetails, ",")

            objText = .ReportObjects("OrgContact")

            Dim contactdets As String = Nothing

            If Trim(contactdet(0).ToString) = "" Then

                contactdets = ""
            Else

                contactdets = "Contact No. " & contactdet(0).ToString

            End If

            objText.Text = contactdets

            objText = .ReportObjects("payperiod")

            'Dim papy_str = "Payroll slip for the period of   " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))
            Dim papy_str = "Payroll slip for the period of   " & Format(CDate(Now), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(Now), machineShortDateFormat))

            objText.Text = papy_str

        End With

        rptdocAll.SetDataSource(dtprintAllPaySlip)

    End Sub

    Private Sub bgwPrintAllPaySlip_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwPrintAllPaySlip.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox("Error: " & vbNewLine & e.Error.Message)

        ElseIf e.Cancelled Then
            MsgBox("Background work cancelled.",
                   MsgBoxStyle.Exclamation)
        Else

            Dim crvwr As New CrysVwr

            crvwr.CrystalReportViewer1.ReportSource = rptdocAll

            'Dim papy_string = "Print all pay slip for the period of " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))
            Dim papy_string = "Payroll slip for the period of   " & Format(CDate(Now), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(Now), machineShortDateFormat))

            crvwr.Text = papy_string

            crvwr.Refresh()

            crvwr.Show()

        End If

        Button1.Enabled = True

    End Sub

    Private Sub btndiscardchanges_Click(sender As Object, e As EventArgs) Handles btndiscardchanges.Click
        UpdateAdjustmentDetails(Convert.ToInt16(tabEarned.SelectedIndex)) 'Josh

        btnSaveAdjustments.Enabled = False

    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked

        Dim n_ProdCtrlForm As New ProdCtrlForm

        With n_ProdCtrlForm

            .Status.HeaderText = "Taxable Flag"

            .PartNo.HeaderText = "Item Name"

            .NameOfCategory = "Adjustment Type"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                cboProducts.DataSource = Nothing

                cboProducts.ValueMember = "ProductID"
                cboProducts.DisplayMember = "ProductName"

                cboProducts.DataSource = New SQLQueryToDatatable("SELECT RowID AS 'ProductID', Name AS 'ProductName', Category FROM product WHERE Category IN ('Allowance Type', 'Bonus', 'Adjustment Type')" &
                                                                 " AND OrganizationID='" & orgztnID & "';").ResultTable

            End If

        End With

    End Sub

    Private Sub Gender_Label(ByVal strGender As String)

        If strGender.Trim.Length > 0 Then

            Dim label_output As String = ""

            If strGender = "Male" Then
                label_output = "Paternity"
            Else
                label_output = "Maternity"
            End If

            Label149.Text = label_output

            Label148.Text = label_output

            Label152.Text = label_output

        End If

    End Sub

    Private Sub tsbtnDelEmpPayroll_Click(sender As Object, e As EventArgs) Handles tsbtnDelEmpPayroll.Click

        If currentEmployeeID = Nothing Then
        Else

            tsbtnDelEmpPayroll.Enabled = False

            Dim prompt = MessageBox.Show("Do you want to delete this '" & CDate(paypFrom).ToShortDateString &
                                         "' to '" & CDate(paypTo).ToShortDateString &
                                         "' payroll of employee '" & currentEmployeeID & "' ?",
                                         "Delete employee payroll",
                                         MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            If prompt = Windows.Forms.DialogResult.Yes Then

                Dim n_ExecuteQuery As New ExecuteQuery("SELECT RowID" &
                                                       " FROM paystub" &
                                                       " WHERE EmployeeID='" & dgvemployees.Tag & "'" &
                                                       " AND OrganizationID='" & orgztnID & "'" &
                                                       " AND PayPeriodID='" & paypRowID & "'" &
                                                       " LIMIT 1;")

                Dim paystubRowID As Object = Nothing

                paystubRowID = n_ExecuteQuery.Result

                n_ExecuteQuery = New ExecuteQuery("CALL DEL_specificpaystub('" & paystubRowID & "');")

            End If

            tsbtnDelEmpPayroll.Enabled = True

        End If

    End Sub

    Private Sub dgvemployees_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvemployees.RowsRemoved
        RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged
    End Sub

    'Imports System.Threading
    'Dim threadlist(1) As Thread
    Private Sub PrintAllPaySlip_Click(sender As Object, e As EventArgs) Handles DeclaredToolStripMenuItem1.Click,
                                                                                ActualToolStripMenuItem1.Click

        'MsgBox(threadlist.GetUpperBound(0))

        Dim IsActualFlag = Convert.ToInt16(DirectCast(sender, ToolStripMenuItem).Tag)

        Dim n_PrintAllPaySlipOfficialFormat As _
            New PrintAllPaySlipOfficialFormat(ValNoComma(paypRowID),
                                              IsActualFlag)

        'ReDim threadlist(threadlist.GetUpperBound(0) + 1)

        'MsgBox(threadlist.GetUpperBound(0))

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

        Dim array_bgworks = array_bgwork.Cast(Of System.ComponentModel.BackgroundWorker).Where(Function(y) y.IsBusy)

        ToolStripButton1.Text = ValNoComma(array_bgworks.Count)

    End Sub

    Sub ProgressCounter(ByVal cnt As Integer)
        Interlocked.Increment(progress_precentage)
        Dim compute_percentage As Integer = (progress_precentage / payroll_emp_count) * 100
        MDIPrimaryForm.systemprogressbar.Value = compute_percentage

        Select Case compute_percentage
            Case 100
                'Me.Enabled = True
                Dim param_array = New Object() {orgztnID, paypRowID, z_User}

                Dim n_ExecSQLProcedure As New _
                    ExecSQLProcedure("RECOMPUTE_thirteenthmonthpay",
                                     param_array)

                dgvpayper_SelectionChanged(dgvpayper, New EventArgs)
            Case Else
        End Select

        Console.WriteLine(String.Concat("#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@ ", compute_percentage, "% complete"))
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        indxStartBatch += thread_max
        Console.WriteLine("batch has finished...")

        If progress_precentage = payroll_emp_count Then
            Timer1.Stop()
            Me.Enabled = True
            Timer1.Enabled = False
        End If
    End Sub

    Private Sub dgvemployees_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployees.CellContentClick

    End Sub

    Private Sub PopulateDGVEmployee(dat_tbl As DataTable)

        For Each drow As DataRow In dat_tbl.Rows
            dgvemployees.Rows.Add(drow("RowID"),
                                  drow("EmployeeID"),
                                  drow("FirstName"),
                                  drow("MiddleName"),
                                  drow("LastName"),
                                  drow("Surname"),
                                  drow("Nickname"),
                                  drow("MaritalStatus"),
                                  drow("NoOfDependents"),
                                  Format(CDate(drow("Birthdate")), machineShortDateFormat),
                                  Format(CDate(drow("StartDate")), machineShortDateFormat),
                                  drow("JobTitle"),
                                  If(IsDBNull(drow("PositionName")), "", drow("PositionName")),
                                  drow("Salutation"),
                                  drow("TINNo"),
                                  drow("SSSNo"),
                                  drow("HDMFNo"),
                                  drow("PhilHealthNo"),
                                  drow("WorkPhone"),
                                  drow("HomePhone"),
                                  drow("MobilePhone"),
                                  drow("HomeAddress"),
                                  drow("EmailAddress"),
                                  If(Trim(drow("Gender")) = "M", "Male", "Female"),
                                  drow("EmploymentStatus"),
                                  drow("PayFrequencyType"),
                                  drow("UndertimeOverride"),
                                  drow("OvertimeOverride"),
                                  If(IsDBNull(drow("PositionID")), "", drow("PositionID")),
                                  drow("PayFrequencyID"),
                                  drow("EmployeeType"),
                                  drow("LeaveBalance"),
                                  drow("SickLeaveBalance"),
                                  drow("MaternityLeaveBalance"),
                                  drow("LeaveAllowance"),
                                  drow("SickLeaveAllowance"),
                                  drow("MaternityLeaveAllowance"),
                                  drow("LeavePerPayPeriod"),
                                  drow("SickLeavePerPayPeriod"),
                                  drow("MaternityLeavePerPayPeriod"),
                                  drow("fstatRowID"),
                                  Nothing,
                                  drow("Created"),
                                  drow("CreatedBy"),
                                  If(IsDBNull(drow("LastUpd")), "", drow("LastUpd")),
                                  If(IsDBNull(drow("LastUpdBy")), "", drow("LastUpdBy")))
        Next

    End Sub

End Class

Friend Class PrintAllPaySlipOfficialFormat

    Private n_PayPeriodRowID As Object = Nothing

    Private n_IsPrintingAsActual As SByte = 0

    Sub New(PayPeriodRowID As Object,
            IsPrintingAsActual As SByte)

        n_PayPeriodRowID = PayPeriodRowID

        n_IsPrintingAsActual = IsPrintingAsActual

        DoProcess()

    End Sub

    Const customDateFormat As String = "'%c/%e/%Y'"

    Private crvwr As New CrysRepForm

    Private catchdt As New DataTable

    Sub DoProcess()

        Dim rptdoc As Object = Nothing

        Dim sql As New SQL("SELECT Name FROM systemowner WHERE IsCurrentOwner='1' LIMIT 1;")

        Dim current_system_owner As String = Convert.ToString(sql.GetFoundRow)

        If SystemOwner.Goldwings = current_system_owner Then

            Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL paystub_payslip(" & orgztnID & "," & n_PayPeriodRowID & "," & n_IsPrintingAsActual & ");")
            'New SQLQueryToDatatable("CALL RPT_solopayslip(" & orgztnID & ",'2016-10-06','2016-10-20',9,0);")

            catchdt = n_SQLQueryToDatatable.ResultTable

            rptdoc = New OfficialPaySlipFormat

            With rptdoc.ReportDefinition.Sections(2)
                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("txtOrganizName")
                objText.Text = orgNam.ToUpper

                objText = .ReportObjects("txtPayPeriod")

                If ValNoComma(n_PayPeriodRowID) > 0 Then
                    objText.Text =
                New ExecuteQuery("SELECT CONCAT(DATE_FORMAT(PayFromDate," & customDateFormat & "),' to ',DATE_FORMAT(PayToDate," & customDateFormat & ")) `Result`" &
                                 " FROM payperiod WHERE RowID=" & ValNoComma(n_PayPeriodRowID) & ";").Result
                End If
            End With

        ElseIf SystemOwner.Hyundai = current_system_owner Then

            Dim params =
                New Object() {orgztnID, n_PayPeriodRowID}

            Dim _sql As New SQL("CALL `HyundaiPayslip`(?og_rowid, ?pp_rowid, TRUE, NULL);",
                               params)

            catchdt = _sql.GetFoundRows.Tables(0)

            rptdoc = New HyundaiPayslip

            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")
            objText.Text = orgNam.ToUpper

        End If

        rptdoc.SetDataSource(catchdt)

        crvwr.crysrepvwr.ReportSource = rptdoc

        crvwr.Show()

    End Sub

End Class

Friend Class PrintSinglePaySlipOfficialFormat

    Private n_PayPeriodRowID As Object = Nothing

    Private n_IsPrintingAsActual As SByte = 0

    Private n_EmployeeRowID As Object = Nothing

    Sub New(PayPeriodRowID As Object,
            IsPrintingAsActual As SByte,
            EmployeeRow_ID As Object)

        n_PayPeriodRowID = PayPeriodRowID

        n_IsPrintingAsActual = IsPrintingAsActual

        n_EmployeeRowID = EmployeeRow_ID

        DoProcess()

    End Sub

    Const customDateFormat As String = "'%c/%e/%Y'"

    Private crvwr As New CrysRepForm

    Private catchdt As New DataTable

    Sub DoProcess()

        Dim rptdoc = New OfficialPaySlipFormat

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL paystub_singlepayslip(" & orgztnID & "," & n_PayPeriodRowID & "," & n_IsPrintingAsActual &
                                    "," & n_EmployeeRowID & ");")
        'New SQLQueryToDatatable("CALL RPT_solopayslip(" & orgztnID & ",'2016-10-06','2016-10-20',9,0);")

        catchdt = n_SQLQueryToDatatable.ResultTable

        With rptdoc.ReportDefinition.Sections(2)
            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("txtOrganizName")
            objText.Text = orgNam.ToUpper

            objText = .ReportObjects("txtPayPeriod")

            If ValNoComma(n_PayPeriodRowID) > 0 Then
                objText.Text =
                    New ExecuteQuery("SELECT CONCAT(DATE_FORMAT(PayFromDate," & customDateFormat & "),' to ',DATE_FORMAT(PayToDate," & customDateFormat & ")) `Result`" &
                                     " FROM payperiod WHERE RowID=" & ValNoComma(n_PayPeriodRowID) & ";").Result
            End If
        End With

        rptdoc.SetDataSource(catchdt)

        crvwr.crysrepvwr.ReportSource = rptdoc

        crvwr.Show()

    End Sub

End Class

Friend Class PrintSoloPaySlipThisPayPeriod

    Sub New(ByVal payp_From As Object, ByVal payp_To As Object, ByVal emp_RowID As Object,
            Optional Is_Actual As Int16 = 0)

        Dim rptdoc = New TwoEmpIn1PaySlip

        Dim rptdt = New DataTable

        rptdt = retAsDatTbl("CALL `RPT_solopayslip`('" & orgztnID & "', '" & payp_From & "', '" & payp_To & "', '" & emp_RowID & "', '" & Is_Actual & "');")

        Dim pay_periodstring = String.Empty

        With rptdoc.ReportDefinition.Sections(2)
            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("OrgName")

            objText.Text = orgNam
            'objText = .ReportObjects("OrgName1")
            'objText.Text = orgNam

            Dim orgaddress = EXECQUER("SELECT CONCAT_WS(', ',a.StreetAddress1,a.StreetAddress2,a.Barangay,a.CityTown,a.Country,a.State) AS Result" &
                                      " FROM organization o LEFT JOIN address a ON a.RowID=o.PrimaryAddressID" &
                                      " WHERE o.RowID=" & orgztnID & ";")

            objText = .ReportObjects("OrgAddress")
            objText.Text = orgaddress
            'objText = .ReportObjects("OrgAddress1")
            'objText.Text = orgaddress

            Dim og_contact = String.Empty

            objText = .ReportObjects("OrgContact")
            objText.Text = New ExecuteQuery("SELECT CONCAT_WS(', ',c.FirstName,c.LastName,c.MainPhone,a.StreetAddress1,a.StreetAddress2,a.Barangay,a.CityTown,a.Country,a.State) AS Result" &
                                            " FROM organization o LEFT JOIN contact c ON c.RowID=o.PrimaryContactID" &
                                            " LEFT JOIN address a ON a.RowID=c.AddressID" &
                                            " WHERE o.RowID=" & orgztnID & ";").Result
            'objText = .ReportObjects("OrgContact1")
            'objText.Text = ""

            pay_periodstring = "for the period of  " & CDate(payp_From).ToShortDateString & " to " & CDate(payp_To).ToShortDateString

            objText = .ReportObjects("payperiod")
            objText.Text = pay_periodstring
            'objText = .ReportObjects("payperiod1")
            'objText.Text = pay_periodstring

        End With

        rptdoc.SetDataSource(rptdt)

        Dim crvwr As New CrysRepForm

        crvwr.crysrepvwr.ReportSource = rptdoc

        crvwr.Text = "Print payslip\" & orgNam & "\" & pay_periodstring

        crvwr.Show()

    End Sub

End Class
