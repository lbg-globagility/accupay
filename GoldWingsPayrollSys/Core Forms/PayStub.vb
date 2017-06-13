Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Threading

Imports System.Threading.Tasks

Public Class PayStub
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
        _uiTasks = New TaskFactory(TaskScheduler.FromCurrentSynchronizationContext())
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

        enlistTheLists("SELECT DisplayValue FROM listofval WHERE Type='Allowance Frequency' AND Active='Yes' ORDER BY OrderBy;", _
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

        Dim n_ReadSQLProcedureToDatatable As New  _
            ReadSQLProcedureToDatatable("SEARCH_employee_paystub",
                                        param_array)

        catchdt = n_ReadSQLProcedureToDatatable.ResultTable

        dgvemployees.Rows.Clear()
        For Each drow As DataRow In catchdt.Rows
            Dim row_array = drow.ItemArray
            dgvemployees.Rows.Add(row_array)
        Next
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

    Private Sub dgvpayper_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpayper.CellContentClick

    End Sub

    Public paypFrom As String = Nothing
    Public paypTo As String = Nothing
    Public paypRowID As String = Nothing
    Public paypPayFreqID As String = Nothing
    Public isEndOfMonth As String = 0

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

    Const max_count_per_page As Integer = 20

    Private Sub First_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked, Prev.LinkClicked, _
                                                                                                Nxt.LinkClicked, Last.LinkClicked, _
                                                                                                LinkLabel4.LinkClicked, LinkLabel3.LinkClicked, _
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

        If bgworkgenpayroll.IsBusy Then

        Else

            Dim pay_freqString = String.Empty

            For Each ctrl In tstrip.Items
                If TypeOf ctrl Is ToolStripButton Then

                    If DirectCast(ctrl, ToolStripButton).BackColor = _
                        Color.FromArgb(194, 228, 255) Then

                        pay_freqString = _
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

        End If

    End Sub

    Private Sub dgvemployees_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvemployees.KeyDown
        'Dim dgv_DataGridViewCellEventArgs As New DataGridViewCellEventArgs(EmployeeID.Index, _
        '                                                                   dgvemployees.CurrentRow.Index)
        'dgvemployees_CellClick(sender, dgv_DataGridViewCellEventArgs)
    End Sub

    Private Sub dgvemployees_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployees.CellClick
        'MsgBox("dgvemployees_CellClick " & dgvemployees.CurrentRow.Index)
    End Sub

    Private Sub dgvemployees_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployees.CellContentClick

    End Sub

    Dim currentEmployeeID As String = Nothing

    Dim LastFirstMidName As String = Nothing

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


                LastFirstMidName = .Cells("LastName").Value & ", " & .Cells("FirstName").Value & _
                    If(Trim(addtlWord) = Nothing, "", If(Trim(addtlWord) = ".", "", ", " & addtlWord))


                txtFName.Text = txtFName.Text & " " & .Cells("LastName").Value

                txtFName.Text = txtFName.Text & If(.Cells("Surname").Value = Nothing, _
                                                         "", _
                                                         "-" & StrConv(.Cells("Surname").Value, _
                                                                       VbStrConv.ProperCase))

                'Microsoft.VisualBasic.Left(.Cells("Surname").Value.ToString, 1)

                currentEmployeeID = .Cells("EmployeeID").Value

                txtEmpID.Text = "ID# " & .Cells("EmployeeID").Value & _
                            If(.Cells("Position").Value = Nothing, _
                                                               "", _
                                                               ", " & .Cells("Position").Value) & _
                            If(.Cells("EmployeeType").Value = Nothing, _
                                                               "", _
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
                    'UpdateAdjustmentDetails()

                    '# ####################################################### #
                    'VIEW_paystub(.Cells("RowID").Value, _
                    '             paypRowID)
                    ''"₱ " & 
                    'txtgrosssal.Text = "0.00"
                    'txtnetsal.Text = "0.00"
                    'txtTotalAdjustments.Text = "0.00" 'Josh
                    'txttaxabsal.Text = "0.00"
                    'txtempwtax.Text = "0.00"

                    'txtempsss.Text = "0.00"
                    'txtempphh.Text = "0.00"
                    'txtemphdmf.Text = "0.00"

                    'txtemptotallow.Text = "0.00"
                    'txtemptotloan.Text = "0.00"
                    'txtemptotbon.Text = "0.00"

                    'For Each dgvrow As DataGridViewRow In dgvpaystub.Rows
                    '    With dgvrow
                    '        txtgrosssal.Text = FormatNumber(Val(.Cells("paystb_TotalGrossSalary").Value), 2)

                    '        txtnetsal.Text = FormatNumber(Val(.Cells("paystb_TotalNetSalary").Value), 2)

                    '        Me.currentTotal = .Cells("paystb_TotalNetSalary").Value


                    '        txttaxabsal.Text = FormatNumber(Val(.Cells("paystb_TotalTaxableSalary").Value), 2)

                    '        txtempwtax.Text = FormatNumber(Val(.Cells("paystb_TotalEmpWithholdingTax").Value), 2)

                    '        txtTotalAdjustments.Text = FormatNumber(Val(.Cells("paystb_TotalAdjustments").Value), 2)

                    '        'txtersss.Text = .Cells("paystb_TotalCompSSS").Value
                    '        'txterphh.Text = .Cells("paystb_TotalCompPhilhealth").Value
                    '        'txterhdmf.Text = .Cells("paystb_TotalCompHDMF").Value

                    '        txtempsss.Text = FormatNumber(Val(.Cells("paystb_TotalEmpSSS").Value), 2)
                    '        txtempphh.Text = FormatNumber(Val(.Cells("paystb_TotalEmpPhilhealth").Value), 2)
                    '        txtemphdmf.Text = FormatNumber(Val(.Cells("paystb_TotalEmpHDMF").Value), 2)

                    '        txtemptotallow.Text = FormatNumber(Val(.Cells("paystb_TotalAllowance").Value), 2)

                    '        'txtotpay.Text = .Cells("paystb_TotalAllowance").Value
                    '        'txtnightdiffpay.Text = .Cells("paystb_TotalAllowance").Value
                    '        'txtnightdiffotpay.Text = .Cells("paystb_TotalAllowance").Value
                    '        'txtholidaypay.Text = .Cells("paystb_TotalAllowance").Value

                    '        'txttardi.Text = .Cells("paystb_TotalAllowance").Value
                    '        'txtut.Text = .Cells("paystb_TotalAllowance").Value

                    '        txtemptotloan.Text = FormatNumber(Val(.Cells("paystb_TotalLoans").Value), 2)
                    '        txtemptotbon.Text = FormatNumber(Val(.Cells("paystb_TotalBonus").Value), 2)

                    '        'txtvacleft.Text = .Cells("paystb_TotalVacationDaysLeft").Value

                    '        VIEW_paystubitem(.Cells("paystb_RowID").Value)

                    '        For Each dgvrw As DataGridViewRow In dgvpaystubitm.Rows
                    '            With dgvrw
                    '                If .Cells("Item").Value.ToString = "Vacation leave" Then
                    '                    txtvlbal.Text = FormatNumber(Val(.Cells("PayAmount").Value), 2)
                    '                    Continue For
                    '                ElseIf .Cells("Item").Value.ToString = "Sick leave" Then
                    '                    txtslbal.Text = FormatNumber(Val(.Cells("PayAmount").Value), 2)
                    '                    Continue For
                    '                ElseIf .Cells("Item").Value.ToString = "Maternity leave" Then '/paternity 
                    '                    'If Microsoft.VisualBasic.Left(dgvemployees.CurrentRow.Cells("Gender").Value, 1) = "M" Then
                    '                    txtmlbal.Text = FormatNumber(Val(.Cells("PayAmount").Value), 2)
                    '                    'Else
                    '                    '    txtmlbal.Text = FormatNumber(Val(.Cells("PayAmount").Value), 2)
                    '                    'End If
                    '                    Continue For
                    '                ElseIf .Cells("Item").Value.ToString = "Tardiness" Then
                    '                    txttottardiamt.Text = FormatNumber(Val(.Cells("PayAmount").Value), 2)
                    '                    Continue For
                    '                ElseIf .Cells("Item").Value.ToString = "Undertime" Then
                    '                    txttotutamt.Text = FormatNumber(Val(.Cells("PayAmount").Value), 2)
                    '                    Continue For
                    '                ElseIf .Cells("Item").Value.ToString = "Absent" Then
                    '                    txttotabsentamt.Text = FormatNumber(Val(.Cells("PayAmount").Value), 2)
                    '                    Continue For
                    '                Else
                    '                    Continue For
                    '                End If

                    '            End With

                    '        Next

                    '    End With

                    '    Exit For
                    'Next

                    'VIEW_specificemployeesalary(.Cells("RowID").Value, _
                    '                            paypTo)

                    'For Each dgvrow As DataGridViewRow In dgvempsal.Rows
                    '    txtempbasicpay.Text = "0.00"

                    '    With dgvrow
                    '        txtempbasicpay.Text = FormatNumber(Val(.Cells("esal_BasicPay").Value), 2)

                    '        'If isorgSSSdeductsched = 1 Then 'Per pay period
                    '        '    txtempsss.Text = Val(.Cells("esal_EmployeeContributionAmount").Value) / 2
                    '        '    txtempsss.Text = FormatNumber(Val(txtempsss.Text), 2).ToString '.Replace(",", "")

                    '        'ElseIf isorgSSSdeductsched = 2 Then 'First half
                    '        '    If isEndOfMonth = "0" Then
                    '        '        txtempsss.Text = Val(.Cells("esal_EmployeeContributionAmount").Value)
                    '        '        txtempsss.Text = FormatNumber(Val(txtempsss.Text), 2).ToString '.Replace(",", "")

                    '        '    Else
                    '        '        txtempsss.Text = "0.00"

                    '        '    End If

                    '        'Else '                          'End of the month
                    '        '    If isEndOfMonth = "0" Then
                    '        '        txtempsss.Text = "0.00"
                    '        '    Else
                    '        '        txtempsss.Text = Val(.Cells("esal_EmployeeContributionAmount").Value)
                    '        '        txtempsss.Text = FormatNumber(Val(txtempsss.Text), 2).ToString '.Replace(",", "")
                    '        '    End If

                    '        'End If

                    '        'If isorgPHHdeductsched = 1 Then 'Per pay period
                    '        '    txtempphh.Text = Val(.Cells("esal_EmployeeShare").Value) / 2
                    '        '    txtempphh.Text = FormatNumber(Val(txtempphh.Text), 2).ToString '.Replace(",", "")

                    '        'ElseIf isorgPHHdeductsched = 2 Then 'First half
                    '        '    If isEndOfMonth = "0" Then
                    '        '        txtempphh.Text = .Cells("esal_EmployeeShare").Value
                    '        '        txtempphh.Text = FormatNumber(Val(txtempphh.Text), 2).ToString '.Replace(",", "")

                    '        '    Else
                    '        '        txtempphh.Text = "0.00"

                    '        '    End If

                    '        'Else '                              'End of the month
                    '        '    If isEndOfMonth = "0" Then
                    '        '        txtempphh.Text = "0.00"
                    '        '    Else
                    '        '        txtempphh.Text = .Cells("esal_EmployeeShare").Value
                    '        '        txtempphh.Text = FormatNumber(Val(txtempphh.Text), 2).ToString '.Replace(",", "")
                    '        '    End If

                    '        'End If

                    '        'If isorgHDMFdeductsched = 1 Then 'Per pay period
                    '        '    txtemphdmf.Text = Val(.Cells("esal_HDMFAmount").Value) / 2
                    '        '    txtemphdmf.Text = FormatNumber(Val(txtemphdmf.Text), 2).ToString '.Replace(",", "")

                    '        'ElseIf isorgHDMFdeductsched = 2 Then 'First half
                    '        '    If isEndOfMonth = "0" Then
                    '        '        txtemphdmf.Text = .Cells("esal_HDMFAmount").Value
                    '        '        txtemphdmf.Text = FormatNumber(Val(txtemphdmf.Text), 2).ToString '.Replace(",", "")

                    '        '    Else
                    '        '        txtemphdmf.Text = "0.00"

                    '        '    End If

                    '        'Else
                    '        '    If isEndOfMonth = "0" Then
                    '        '        txtemphdmf.Text = "0.00"
                    '        '    Else
                    '        '        txtemphdmf.Text = .Cells("esal_HDMFAmount").Value
                    '        '        txtemphdmf.Text = FormatNumber(Val(txtemphdmf.Text), 2).ToString '.Replace(",", "")
                    '        '    End If

                    '        'End If

                    '        Exit For
                    '    End With
                    'Next

                    'VIEW_employeetimeentry_SUM(.Cells("RowID").Value, _
                    '                            paypFrom, _
                    '                            paypTo)

                    'txttotreghrs.Text = "0.00"
                    'txttotregamt.Text = "0.00"

                    'txttotothrs.Text = "0.00"
                    'txttototamt.Text = "0.00"

                    'txttotnightdiffhrs.Text = "0.00"
                    'txttotnightdiffamt.Text = "0.00"

                    'txttotnightdiffothrs.Text = "0.00"
                    'txttotnightdiffotamt.Text = "0.00"

                    'txttotholidayhrs.Text = "0.00"
                    'txttotholidayamt.Text = "0.00"

                    'txthrswork.Text = "0.00"
                    'txthrsworkamt.Text = "0.00"

                    'lblsubtot.Text = "0.00"

                    'For Each dgvrow As DataGridViewRow In dgvetent.Rows
                    '    With dgvrow
                    '        If employeetype = "Fixed" Then
                    '            If dgvemployees.CurrentRow.Cells("PayFreqID").Value = 1 Then
                    '                If txtgrosssal.Text = "0.00" Then
                    '                    txtgrosssal.Text = FormatNumber(Val(txtempbasicpay.Text.Replace(",", "")) + _
                    '                    (.Cells("etent_OvertimeHoursAmount").Value) + _
                    '                    (.Cells("etent_NightDiffOTHoursAmount").Value), _
                    '                                            2)

                    '                End If

                    '                lblsubtot.Text = txtempbasicpay.Text 'txtgrosssal

                    '                'txtgrosssal.Text = txtgrosssal.Text '.Replace(",", "")

                    '                txthrsworkamt.Text = lblsubtot.Text

                    '            Else
                    '                If txtgrosssal.Text = "0.00" Then
                    '                    txtgrosssal.Text = FormatNumber(Val(txtemptotallow.Text.Replace(",", "")) + _
                    '                    (.Cells("etent_OvertimeHoursAmount").Value) + _
                    '                    (.Cells("etent_NightDiffOTHoursAmount").Value), _
                    '                                                2)

                    '                End If

                    '                lblsubtot.Text = txtempbasicpay.Text 'txtgrosssal'.Replace(",", "")

                    '                txthrsworkamt.Text = lblsubtot.Text

                    '            End If


                    '        ElseIf employeetype = "Monthly" Then

                    '            txttotregamt.Text = ValNoComma(txtempbasicpay.Text)
                    '            txttotregamt.Text = ValNoComma(txttotregamt.Text) _
                    '                - (ValNoComma(.Cells("etent_HoursLateAmount").Value) _
                    '                + ValNoComma(.Cells("etent_UndertimeHoursAmount").Value) _
                    '                + ValNoComma(txttotabsentamt.Text))

                    '            txttotregamt.Text = FormatNumber(ValNoComma(txttotregamt.Text), 2)

                    '            txthrsworkamt.Text = txttotregamt.Text

                    '            lblsubtot.Text = txthrsworkamt.Text

                    '        Else
                    '            lblsubtot.Text = FormatNumber(ValNoComma(.Cells("etent_TotalDayPay").Value), 2)

                    '            'txthrsworkamt.Text = FormatNumber(Val(.Cells("etent_TotalDayPay").Value), 2)

                    '            txthrsworkamt.Text = FormatNumber(ValNoComma(.Cells("etent_RegularHoursAmount").Value), 2)

                    '        End If

                    '        txttotreghrs.Text = Val(.Cells("etent_TotalHoursWorked").Value) 'etent_RegularHoursWorked
                    '        txttotregamt.Text = If(IsDBNull(.Cells("etent_RegularHoursAmount").Value), "0.00", FormatNumber(Val(.Cells("etent_RegularHoursAmount").Value), 2))

                    '        txttotothrs.Text = .Cells("etent_OvertimeHoursWorked").Value
                    '        txttototamt.Text = FormatNumber(Val(.Cells("etent_OvertimeHoursAmount").Value), 2)

                    '        txttotnightdiffhrs.Text = .Cells("etent_NightDifferentialHours").Value
                    '        txttotnightdiffamt.Text = FormatNumber(Val(.Cells("etent_NightDiffHoursAmount").Value), 2)

                    '        txttotnightdiffothrs.Text = .Cells("etent_NightDifferentialOTHours").Value
                    '        txttotnightdiffotamt.Text = FormatNumber(Val(.Cells("etent_NightDiffOTHoursAmount").Value), 2)

                    '        txttotut.Text = .Cells("etent_UndertimeHours").Value
                    '        'txttotutamt.Text = FormatNumber(Val(.Cells("etent_UndertimeHoursAmount").Value), 2)

                    '        'txttotholidayhrs.Text = .Cells("esal_BasicPay").Value
                    '        'txttotholidayamt.Text = .Cells("esal_BasicPay").Value

                    '        txthrswork.Text = ValNoComma(.Cells("etent_TotalHoursWorked").Value) _
                    '                        - ValNoComma(.Cells("etent_OvertimeHoursWorked").Value) _
                    '                        - ValNoComma(.Cells("etent_NightDifferentialOTHours").Value)
                    '        'txthrswork.Text = .Cells("etent_RegularHoursWorked").Value

                    '        txttottardi.Text = .Cells("etent_HoursLate").Value
                    '        'txttottardiamt.Text = FormatNumber(Val(.Cells("etent_HoursLateAmount").Value), 2)

                    '        Exit For
                    '    End With
                    'Next

                    'txttotabsent.Text = COUNT_employeeabsent(.Cells("RowID").Value, _
                    '                                         .Cells("Startdate").Value, _
                    '                                         paypFrom, _
                    '                                         paypTo)

                    ''Dim param_date = If(paypTo = Nothing, paypFrom, paypTo)

                    ''Dim rateper_hour = GET_employeerateperhour(.Cells("RowID").Value, param_date)

                    ''txttottardi.Text = Val(rateper_hour) * Val(txttottardi.Text)

                    'Dim misc_subtot = Val(txttottardiamt.Text.Replace(",", "")) + Val(txttotutamt.Text.Replace(",", ""))

                    'lblsubtotmisc.Text = FormatNumber(Val(misc_subtot), 2).ToString '.Replace(",", "")

                    ''VIEW_eallow_indate(.Cells("RowID").Value, _
                    ''                    paypFrom, _
                    ''                    paypTo)

                    ''VIEW_eloan_indate(.Cells("RowID").Value, _
                    ''                    paypFrom, _
                    ''                    paypTo)

                    ''VIEW_ebon_indate(.Cells("RowID").Value, _
                    ''                    paypFrom, _
                    ''                    paypTo)
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

            txtempbasicpay.Text = ""

            txttotreghrs.Text = ""
            txttotregamt.Text = ""

            txttotothrs.Text = ""
            txttototamt.Text = ""

            txttotnightdiffhrs.Text = ""
            txttotnightdiffamt.Text = ""

            txttotnightdiffothrs.Text = ""
            txttotnightdiffotamt.Text = ""

            txttotholidayhrs.Text = ""
            txttotholidayamt.Text = ""

            txthrswork.Text = ""
            txthrsworkamt.Text = ""

            lblsubtot.Text = ""

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

    Sub VIEW_paystub(Optional EmpID As Object = Nothing, _
                     Optional PayPeriodID As Object = Nothing)

        Dim params(2, 2) As Object

        params(0, 0) = "paystb_OrganizationID"
        params(1, 0) = "paystb_EmployeeID"
        params(2, 0) = "paystb_PayPeriodID"

        params(0, 1) = orgztnID
        params(1, 1) = EmpID
        params(2, 1) = PayPeriodID

        EXEC_VIEW_PROCEDURE(params, _
                             "VIEW_paystub", _
                             dgvpaystub, , 1)

    End Sub

    Sub VIEW_paystubitem(ByVal paystitm_PayStubID As Object)

        Dim params(2, 2) As Object

        params(0, 0) = "paystitm_PayStubID"

        params(0, 1) = paystitm_PayStubID

        EXEC_VIEW_PROCEDURE(params, _
                             "VIEW_paystubitem", _
                             dgvpaystubitm, , 1)

    End Sub

    Sub VIEW_specificemployeesalary(Optional esal_EmployeeID As Object = Nothing, _
                                    Optional esal_Date As Object = Nothing)

        Dim params(2, 2) As Object

        params(0, 0) = "esal_EmployeeID"
        params(1, 0) = "esal_OrganizationID"
        params(2, 0) = "esal_Date"

        params(0, 1) = esal_EmployeeID
        params(1, 1) = orgztnID
        params(2, 1) = esal_Date

        EXEC_VIEW_PROCEDURE(params, _
                             "VIEW_specificemployeesalary", _
                             dgvempsal, , 1)

    End Sub

    Sub VIEW_employeetimeentry_SUM(Optional etent_EmployeeID As Object = Nothing, _
                                   Optional etent_Date As Object = Nothing, _
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

        EXEC_VIEW_PROCEDURE(params, _
                            "VIEW_employeetimeentry_SUM", _
                            dgvetent, , 1)

    End Sub

    Function COUNT_employeeabsent(Optional EmpID As Object = Nothing, _
                                  Optional EmpStartDate As Object = Nothing, _
                                  Optional payperiodDateFrom As Object = Nothing, _
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

    Function GET_employeerateperhour(Optional EmpID As Object = Nothing, _
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

    Dim employee_dattab As New DataTable

    Dim esal_dattab As New DataTable

    Dim etent_dattab As New DataTable

    Dim etent_holidaypay As New DataTable

    Dim etent_totdaypay As New DataTable

    Dim emp_loans As New DataTable

    Dim emp_bonus As New DataTable

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

    Public numofweekdays As Integer

    Public numofweekends As Integer

    Dim eloans As New DataTable

    Dim empleave As New DataTable

    Dim allowtyp As Object = Nothing
    Dim allow_type() As String

    Dim deductions As Object = Nothing
    Dim arraydeduction() As String

    Dim loan_type As Object = Nothing
    Dim loantyp() As String

    Dim misc As Object = Nothing
    Dim miscs() As String

    Dim totals As Object = Nothing
    Dim emp_totals() As String

    Dim leavetype As Object = Nothing
    Dim leavtyp() As String


    Dim allowkind As Object = Nothing
    Dim allow_kind() As String

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

    Public withthirteenthmonthpay As SByte = 0

    Dim empthirteenmonthtable As New DataTable

    Dim MinimumWageAmount = Val(0)

    Dim dtemployeefirsttimesalary As New DataTable

    Sub genpayroll(Optional PayFreqRowID As Object = Nothing)

        Task.Factory.StartNew(Sub()

                                  'RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

                                  ''RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

                                  'If dgvpayper.RowCount = 0 Then
                                  '    'Exit Sub
                                  'End If

                                  If paypFrom = Nothing _
                                      And paypTo = Nothing Then
                                      Exit Sub
                                  End If

                                  Dim _bool As Boolean = False

                                  ''###### CONTROL DISABLER ######
                                  'Me.Enabled = _bool
                                  'PayrollForm.MenuStrip1.Enabled = _bool
                                  'MDIPrimaryForm.Showmainbutton.Enabled = _bool
                                  'ToolStrip1.Enabled = _bool
                                  'MDIPrimaryForm.systemprogressbar.Value = 0
                                  'MDIPrimaryForm.systemprogressbar.Visible = CBool(Not _bool)
                                  'Panel5.Enabled = _bool


                                  'Dim prompt = MessageBox.Show("Begin generate payroll from " & Format(CDate(paypFrom), "MMM-d-yyyy") & " to " & Format(CDate(paypTo), "MMM-d-yyyy") & " ?", "Generate payroll", MessageBoxButtons.YesNoCancel)
                                  ''MsgBox("Employee time entry from " & paypFrom & " to " & paypTo & " is not yet prepared.", MsgBoxStyle.Information)


                                  'If prompt = Windows.Forms.DialogResult.Yes Then

                                  'numofweekdays = 0

                                  'Dim date_diff = DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo))

                                  etent_dattab = New SQLQueryToDatatable("SELECT RowID,OrganizationID,Created,CreatedBy,COALESCE(LastUpd,'') 'LastUpd',COALESCE(LastUpdBy,'') 'LastUpdBy',Date,COALESCE(EmployeeShiftID,'') 'EmployeeShiftID',COALESCE(EmployeeID,'') 'EmployeeID',COALESCE(EmployeeSalaryID,'') 'EmployeeSalaryID',COALESCE(EmployeeFixedSalaryFlag,0) '',COALESCE(RegularHoursWorked,0) 'RegularHoursWorked',COALESCE(OvertimeHoursWorked,0) 'OvertimeHoursWorked',COALESCE(UndertimeHours,0) 'UndertimeHours',COALESCE(NightDifferentialHours,0) 'NightDifferentialHours',COALESCE(NightDifferentialOTHours,0) 'NightDifferentialOTHours',COALESCE(HoursLate,0) 'HoursLate',COALESCE(LateFlag,0) 'LateFlag',COALESCE(PayRateID,'') 'PayRateID',COALESCE(VacationLeaveHours,0) 'VacationLeaveHours',COALESCE(SickLeaveHours,0) 'SickLeaveHours',COALESCE(TotalDayPay,0) 'TotalDayPay'" &
                                                              " FROM employeetimeentry" &
                                                              " WHERE OrganizationID=" & orgztnID & "" &
                                                              " AND Date" &
                                                              " BETWEEN '" & paypFrom & "'" &
                                                              " AND '" & paypTo & "'" &
                                                              " ORDER BY Date;").ResultTable

                                  'OvertimeHoursAmount,NightDiffHoursAmount,NightDiffOTHoursAmount


                                  'employee
                                  'EMPLOYEE_payrollgen
                                  'Dim ib = InputBox("Please input Employee ID", "title", 0)
                                  'NULL, '" & ib.Trim & "'
                                  '#############################################
                                  '#############################################
                                  'employee_dattab = New SQLQueryToDatatable("CALL EMPLOYEE_payrollgen('" & orgztnID & "', '" & paypFrom & "', '" & paypTo & "', NULL);").ResultTable
                                  '#############################################
                                  '#############################################
                                  'employee_dattab = New SQLQueryToDatatable("SELECT e.*" &
                                  '                                ",IF(e.AgencyID IS NOT NULL, IFNULL(d.PhHealthDeductSchedAgency,d.PhHealthDeductSched), d.PhHealthDeductSched) AS PhHealthDeductSched" &
                                  '                                ",IF(e.AgencyID IS NOT NULL, IFNULL(d.HDMFDeductSchedAgency,d.HDMFDeductSched), d.HDMFDeductSched) AS HDMFDeductSched" &
                                  '                                ",IF(e.AgencyID IS NOT NULL, IFNULL(d.SSSDeductSchedAgency,d.SSSDeductSched), d.SSSDeductSched) AS SSSDeductSched" &
                                  '                                ",IF(e.AgencyID IS NOT NULL, IFNULL(d.WTaxDeductSchedAgency,d.WTaxDeductSched), d.WTaxDeductSched) AS WTaxDeductSched" &
                                  '                                ",PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) 'PAYFREQUENCY_DIVISOR'" &
                                  '                                ",GET_employeerateperday(e.RowID, e.OrganizationID, '" & paypTo & "') 'EmpRatePerDay'" &
                                  '                                ",d.MinimumWageAmount" &
                                  '                                ",(e.StartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "') AS IsFirstTimeSalary" &
                                  '                                ",GET_employeeStartingAttendanceCount(e.RowID,'" & paypFrom & "','" & paypTo & "') AS StartingAttendanceCount" &
                                  '                                " FROM" &
                                  '                                " employee e LEFT JOIN employeesalary esal ON e.RowID=esal.EmployeeID" &
                                  '                                " LEFT JOIN position p ON p.RowID=e.PositionID" &
                                  '                                " LEFT JOIN `division` d ON d.RowID=p.DivisionId" &
                                  '                                " LEFT JOIN agency ag ON ag.RowID=e.AgencyID" &
                                  '                                " INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID" &
                                  '                                " WHERE e.OrganizationID=" & orgztnID & _
                                  '                                " AND '" & paypTo & "' BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,'" & paypTo & "')" &
                                  '                                " GROUP BY e.RowID" &
                                  '                                " ORDER BY e.RowID DESC;").ResultTable
                                  '",IFNULL(dmw.Amount,481.0) AS MinimumWageAmount" &
                                  '" LEFT JOIN divisionminimumwage dmw ON dmw.OrganizationID=e.OrganizationID AND dmw.DivisionID=d.RowID AND '" & paypTo & "' BETWEEN dmw.EffectiveDateFrom AND dmw.EffectiveDateTo" &
                                  Dim double_value = ValNoComma(0)
                                  ''employeetimeentry - ORIGINAL
                                  '    etent_totdaypay = retAsDatTbl("SELECT SUM(COALESCE(TotalDayPay,0)) 'TotalDayPay'" &
                                  '                                  ",EmployeeID" &
                                  '                                  ",Date" &
                                  '                                  ",SUM(COALESCE(RegularHoursAmount,0)) 'RegularHoursAmount'" &
                                  '                                  ",SUM(COALESCE(OvertimeHoursAmount,0)) 'OvertimeHoursAmount'" &
                                  '                                  ",SUM(COALESCE(UndertimeHoursAmount,0)) 'UndertimeHoursAmount'" &
                                  '                                  ",SUM(COALESCE(NightDiffHoursAmount,0)) 'NightDiffHoursAmount'" &
                                  '                                  ",SUM(COALESCE(NightDiffOTHoursAmount,0)) 'NightDiffOTHoursAmount'" &
                                  '                                  ",SUM(COALESCE(HoursLateAmount,0)) 'HoursLateAmount'" &
                                  '                                  " FROM employeetimeentry" &
                                  '                                  " WHERE OrganizationID=" & orgztnID & _
                                  '                                  " AND Date" &
                                  '                                  " BETWEEN '" & paypFrom & "'" &
                                  '                                  " AND '" & paypTo & "'" &
                                  '                                  " GROUP BY EmployeeID" &
                                  '                                  " ORDER BY EmployeeID;")

                                  'employeetimeentry - EXPERIMENT
                                  etent_totdaypay = New SQLQueryToDatatable("SELECT SUM(COALESCE(ete.TotalDayPay,0)) 'TotalDayPay'" &
                                                                ",ete.EmployeeID" &
                                                                ",ete.Date" &
                                                                ",SUM(COALESCE(ete.RegularHoursAmount,0)) 'RegularHoursAmount'" &
                                                                ",SUM(COALESCE(ete.OvertimeHoursAmount,0)) 'OvertimeHoursAmount'" &
                                                                ",SUM(COALESCE(ete.UndertimeHoursAmount,0)) 'UndertimeHoursAmount'" &
                                                                ",SUM(COALESCE(ete.NightDiffHoursAmount,0)) 'NightDiffHoursAmount'" &
                                                                ",SUM(COALESCE(ete.NightDiffOTHoursAmount,0)) 'NightDiffOTHoursAmount'" &
                                                                ",SUM(COALESCE(ete.HoursLateAmount,0)) 'HoursLateAmount'" &
                                                                ",SUM(ete.Absent) AS Absent" &
                                                                ",SUM(ete.RegularHoursAmount) AS HolidayPay" &
                                                                ",IFNULL(emt.emtAmount,0) AS emtAmount" &
                                                                " FROM employeetimeentry ete" &
                                                                " LEFT JOIN employee e ON e.RowID=ete.EmployeeID" &
                                                                " LEFT JOIN payrate pr ON pr.RowID=ete.PayRateID AND pr.OrganizationID=ete.OrganizationID" &
                                                                " LEFT JOIN (SELECT ete.RowID" &
                                                                              ",e.RowID AS eRowID" &
                                                                              ",(SUM(ete.RegularHoursAmount) * (pr.`PayRate` - 1.0)) AS emtAmount" &
                                                                              " FROM employeetimeentry ete" &
                                                                              " INNER JOIN employee e ON e.RowID=ete.EmployeeID" &
                                                                              " AND e.OrganizationID=ete.OrganizationID" &
                                                                              " AND (e.CalcSpecialHoliday='1' OR e.CalcHoliday='1')" &
                                                                              " INNER JOIN payrate pr ON pr.RowID=ete.PayRateID AND pr.PayType!='Regular Day'" &
                                                                              " WHERE ete.OrganizationID='" & orgztnID & "' AND ete.`Date` BETWEEN '" & paypFrom & "' AND '" & paypTo & "') emt ON emt.RowID IS NOT NULL AND emt.eRowID=ete.EmployeeID" &
                                                                " WHERE ete.OrganizationID=" & orgztnID & _
                                                                " AND ete.Date" &
                                                                " BETWEEN IF('" & paypFrom & "' < e.StartDate, e.StartDate, '" & paypFrom & "')" &
                                                                " AND '" & paypTo & "'" &
                                                                " GROUP BY ete.EmployeeID" &
                                                                " ORDER BY ete.EmployeeID;").ResultTable

                                  ''SELECT SUM(COALESCE(ete.TotalDayPay,0)) 'TotalDayPay',ete.EmployeeID , ete.DATE, SUM(COALESCE(ete.RegularHoursAmount,0)) 'RegularHoursAmount', SUM(COALESCE(ete.OvertimeHoursAmount,0)) 'OvertimeHoursAmount', SUM(COALESCE(ete.UndertimeHoursAmount,0)) 'UndertimeHoursAmount', SUM(COALESCE(ete.NightDiffHoursAmount,0)) 'NightDiffHoursAmount', SUM(COALESCE(ete.NightDiffOTHoursAmount,0)) 'NightDiffOTHoursAmount', SUM(COALESCE(ete.HoursLateAmount,0)) 'HoursLateAmount' FROM employeetimeentry ete LEFT JOIN employee e ON e.RowID=ete.EmployeeID WHERE ete.OrganizationID=2 AND ete.DATE BETWEEN IF('2015-06-01' < e.StartDate, e.StartDate, '2015-06-01') AND '2015-06-15' GROUP BY ete.EmployeeID ORDER BY ete.EmployeeID;

                                  ''employeeloans
                                  'emp_loans = retAsDatTbl("SELECT SUM((COALESCE(TotalLoanAmount,0) - COALESCE(TotalBalanceLeft,0))) 'TotalLoanAmount'" &
                                  '                        ",SUM(DeductionAmount) 'DeductionAmount'" &
                                  '                        ",EmployeeID" &
                                  '                        " FROM employeeloanschedule" &
                                  '                        " WHERE OrganizationID=" & orgztnID & _
                                  '                        " AND IF('" & paypFrom & "' < DedEffectiveDateFrom AND '" & paypTo & "' < DedEffectiveDateTo, DedEffectiveDateFrom>='" & paypFrom & "' AND DedEffectiveDateTo>='" & paypTo & "', DedEffectiveDateFrom<='" & paypFrom & "' AND DedEffectiveDateTo>='" & paypTo & "')" &
                                  '                        " AND Status='In Progress'" &
                                  '                        " GROUP BY EmployeeID" &
                                  '                        " ORDER BY EmployeeID;")

                                  'employeeloans

                                  Dim sum_emp_loans = String.Empty

                                  Select Case PayFreqRowID

                                      Case 1

                                          If isEndOfMonth = "1" Then 'means, first half of the month

                                              sum_emp_loans = "SELECT SUM((COALESCE(TotalLoanAmount,0) - COALESCE(TotalBalanceLeft,0))) 'TotalLoanAmount'" &
                                                          ",SUM(DeductionAmount) 'DeductionAmount'" &
                                                          ",EmployeeID" &
                                                          " FROM employeeloanschedule" &
                                                          " WHERE OrganizationID=" & orgztnID & _
                                                          " AND BonusID IS NULL AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                                          " ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                                          " ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                                          " AND `Status`='In Progress'" &
                                                          " AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                                          " AND DeductionSchedule IN ('First half','Per pay period')" &
                                                          " GROUP BY EmployeeID" &
                                                          " ORDER BY EmployeeID;"

                                          Else 'If isEndOfMonth = "1" Then                'means, end of the month

                                              sum_emp_loans = "SELECT SUM((COALESCE(TotalLoanAmount,0) - COALESCE(TotalBalanceLeft,0))) 'TotalLoanAmount'" &
                                                          ",SUM(DeductionAmount) 'DeductionAmount'" &
                                                          ",EmployeeID" &
                                                          " FROM employeeloanschedule" &
                                                          " WHERE OrganizationID=" & orgztnID & _
                                                          " AND BonusID IS NULL AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                                          " ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                                          " ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                                          " AND `Status`='In Progress'" &
                                                          " AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                                          " AND DeductionSchedule IN ('End of the month','Per pay period')" &
                                                          " GROUP BY EmployeeID" &
                                                          " ORDER BY EmployeeID;"

                                          End If

                                      Case 2

                                      Case 3

                                      Case 4

                                          If isEndOfMonth = "2" Then 'means, first half of the monthS

                                              sum_emp_loans = "SELECT SUM((COALESCE(TotalLoanAmount,0) - COALESCE(TotalBalanceLeft,0))) 'TotalLoanAmount'" &
                                                          ",SUM(DeductionAmount) 'DeductionAmount'" &
                                                          ",EmployeeID" &
                                                          " FROM employeeloanschedule" &
                                                          " WHERE OrganizationID=" & orgztnID & _
                                                          " AND BonusID IS NULL AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                                          " ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                                          " ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                                          " AND Status='In Progress'" &
                                                          " AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                                          " AND DeductionSchedule IN ('First half','Per pay period')" &
                                                          " GROUP BY EmployeeID" &
                                                          " ORDER BY EmployeeID;"

                                          ElseIf isEndOfMonth = "1" Then 'means, end of the month

                                              sum_emp_loans = "SELECT SUM((COALESCE(TotalLoanAmount,0) - COALESCE(TotalBalanceLeft,0))) 'TotalLoanAmount'" &
                                                          ",SUM(DeductionAmount) 'DeductionAmount'" &
                                                          ",EmployeeID" &
                                                          " FROM employeeloanschedule" &
                                                          " WHERE OrganizationID=" & orgztnID & _
                                                          " AND BonusID IS NULL AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                                          " ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                                          " ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                                          " AND Status='In Progress'" &
                                                          " AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                                          " AND DeductionSchedule IN ('End of the month','Per pay period')" &
                                                          " GROUP BY EmployeeID" &
                                                          " ORDER BY EmployeeID;"

                                          ElseIf isEndOfMonth = "0" Then 'means, per pay period

                                              sum_emp_loans = "SELECT SUM((COALESCE(TotalLoanAmount,0) - COALESCE(TotalBalanceLeft,0))) 'TotalLoanAmount'" &
                                                          ",SUM(DeductionAmount) 'DeductionAmount'" &
                                                          ",EmployeeID" &
                                                          " FROM employeeloanschedule" &
                                                          " WHERE OrganizationID=" & orgztnID & _
                                                          " AND BonusID IS NULL AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                                          " ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                                          " ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                                          " AND Status='In Progress'" &
                                                          " AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                                          " AND DeductionSchedule = 'Per pay period'" &
                                                          " GROUP BY EmployeeID" &
                                                          " ORDER BY EmployeeID;"

                                          End If

                                  End Select
                                  '                                   sum_emp_loans
                                  emp_loans = New SQLQueryToDatatable("CALL PAYSTUB_prepare_loans('" & orgztnID & "','" & paypFrom & "','" & paypTo & "','" & paypRowID & "');").ResultTable

                                  '"SELECT SUM((COALESCE(TotalLoanAmount,0) - COALESCE(TotalBalanceLeft,0))) 'TotalLoanAmount'" &
                                  '",SUM(DeductionAmount) 'DeductionAmount'" &
                                  '",EmployeeID" &
                                  '" FROM employeeloanschedule" &
                                  '" WHERE OrganizationID=" & orgztnID & _
                                  '" AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                  '" ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                  '" ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                  '" AND Status='In Progress'" &
                                  '" AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                  '" GROUP BY EmployeeID" &
                                  '" ORDER BY EmployeeID;"




                                  '" AND IF('" & paypFrom & "' < DedEffectiveDateFrom AND '" & paypTo & "' < DedEffectiveDateTo, DedEffectiveDateFrom>='" & paypFrom & "' AND DedEffectiveDateTo>='" & paypTo & "', DedEffectiveDateFrom<='" & paypFrom & "' AND DedEffectiveDateTo<='" & paypTo & "')" &

                                  '" AND DedEffectiveDateFrom>='" & paypFrom & "'" &
                                  '" AND DedEffectiveDateTo<='" & paypTo & "'" &

                                  'employeeloans

                                  Dim segregate_emp_loan = String.Empty

                                  Select Case PayFreqRowID

                                      Case 1

                                          If isEndOfMonth = "0" Then 'means, first half of the month

                                              segregate_emp_loan = "SELECT LoanTypeID,DeductionAmount,DeductionPercentage,EmployeeID,IF(LoanPayPeriodLeft BETWEEN 1 AND 1.99, '1', '0') 'LoanDueDate',TotalLoanAmount,DeductionAmount,NoOfPayPeriod" &
                                                                  " FROM employeeloanschedule" &
                                                                  " WHERE OrganizationID=" & orgztnID & _
                                                                  " AND BonusID IS NULL AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                                                  " ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                                                  " ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                                                  " AND Status='In Progress'" &
                                                                  " AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                                                  " AND DeductionSchedule IN ('First half','Per pay period')" &
                                                                  " ORDER BY LoanTypeID;"

                                          Else '                      'means, end of the month

                                              segregate_emp_loan = "SELECT LoanTypeID,DeductionAmount,DeductionPercentage,EmployeeID,IF(LoanPayPeriodLeft BETWEEN 1 AND 1.99, '1', '0') 'LoanDueDate',TotalLoanAmount,DeductionAmount,NoOfPayPeriod" &
                                                                  " FROM employeeloanschedule" &
                                                                  " WHERE OrganizationID=" & orgztnID & _
                                                                  " AND BonusID IS NULL AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                                                  " ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                                                  " ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                                                  " AND Status='In Progress'" &
                                                                  " AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                                                  " AND DeductionSchedule IN ('End of the month','Per pay period')" &
                                                                  " ORDER BY LoanTypeID;"

                                          End If

                                      Case 2

                                      Case 3

                                      Case 4

                                          If isEndOfMonth = "2" Then 'means, first half of the month

                                              segregate_emp_loan = "SELECT LoanTypeID,DeductionAmount,DeductionPercentage,EmployeeID,IF(LoanPayPeriodLeft BETWEEN 1 AND 1.99, '1', '0') 'LoanDueDate',TotalLoanAmount,DeductionAmount,NoOfPayPeriod" &
                                                                  " FROM employeeloanschedule" &
                                                                  " WHERE OrganizationID=" & orgztnID & _
                                                                  " AND BonusID IS NULL AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                                                  " ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                                                  " ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                                                  " AND Status='In Progress'" &
                                                                  " AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                                                  " AND DeductionSchedule IN ('First half','Per pay period')" &
                                                                  " ORDER BY LoanTypeID;"

                                          ElseIf isEndOfMonth = "1" Then 'means, end of the month

                                              segregate_emp_loan = "SELECT LoanTypeID,DeductionAmount,DeductionPercentage,EmployeeID,IF(LoanPayPeriodLeft BETWEEN 1 AND 1.99, '1', '0') 'LoanDueDate',TotalLoanAmount,DeductionAmount,NoOfPayPeriod" &
                                                                  " FROM employeeloanschedule" &
                                                                  " WHERE OrganizationID=" & orgztnID & _
                                                                  " AND BonusID IS NULL AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                                                  " ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                                                  " ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                                                  " AND Status='In Progress'" &
                                                                  " AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                                                  " AND DeductionSchedule IN ('End of the month','Per pay period')" &
                                                                  " ORDER BY LoanTypeID;"

                                          ElseIf isEndOfMonth = "0" Then 'means, per pay period

                                              segregate_emp_loan = "SELECT LoanTypeID,DeductionAmount,DeductionPercentage,EmployeeID,IF(LoanPayPeriodLeft BETWEEN 1 AND 1.99, '1', '0') 'LoanDueDate',TotalLoanAmount,DeductionAmount,NoOfPayPeriod" &
                                                                  " FROM employeeloanschedule" &
                                                                  " WHERE OrganizationID=" & orgztnID & _
                                                                  " AND BonusID IS NULL AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                                                  " ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                                                  " ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                                                  " AND Status='In Progress'" &
                                                                  " AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                                                  " AND DeductionSchedule = 'Per pay period'" &
                                                                  " ORDER BY LoanTypeID;"

                                          End If

                                  End Select

                                  eloans = retAsDatTbl(segregate_emp_loan) 'LoanPayPeriodLeft

                                  '"SELECT LoanTypeID,DeductionAmount,DeductionPercentage,EmployeeID,IF(LoanPayPeriodLeft BETWEEN 1 AND 1.99, '1', '0') 'LoanDueDate',TotalLoanAmount,DeductionAmount,NoOfPayPeriod" &
                                  '" FROM employeeloanschedule" &
                                  '" WHERE OrganizationID=" & orgztnID & _
                                  '" AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                                  '" ,IF(MONTH(DedEffectiveDateFrom) = MONTH('" & paypTo & "'), IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "'), DedEffectiveDateFrom<='" & paypTo & "')" &
                                  '" ,DedEffectiveDateFrom<='" & paypTo & "')" &
                                  '" AND Status='In Progress'" &
                                  '" AND COALESCE(LoanPayPeriodLeft,0)!=0" &
                                  '" ORDER BY LoanTypeID;"



                                  '" ,IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "')" &

                                  '" AND IF('" & paypFrom & "' < DedEffectiveDateFrom AND '" & paypTo & "' < DedEffectiveDateTo, DedEffectiveDateFrom>='" & paypFrom & "' AND DedEffectiveDateTo>='" & paypTo & "', DedEffectiveDateFrom<='" & paypFrom & "' AND DedEffectiveDateTo>='" & paypTo & "')" &

                                  '" AND DedEffectiveDateFrom>='" & paypFrom & "'" &
                                  '" AND DedEffectiveDateTo<='" & paypTo & "'" &

                                  'employeebonus
                                  emp_bonus = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                          ",EmployeeID" &
                                                          " FROM employeebonus" &
                                                          " WHERE OrganizationID=" & orgztnID & _
                                                          " AND EffectiveStartDate>='" & paypFrom & "'" &
                                                          " AND EffectiveEndDate<='" & paypTo & "'" &
                                                          " AND TaxableFlag='1'" &
                                                          " GROUP BY EmployeeID" &
                                                          " ORDER BY EmployeeID;")

                                  Dim dailyallowfreq = "Daily"

                                  If allowfreq.Count <> 0 Then
                                      dailyallowfreq = If(allowfreq.Item(0).ToString = "", "Daily", allowfreq.Item(0).ToString)
                                      'allowfreq : Daily'Monthly'One time'Semi-monthly'Weely
                                  End If

                                  'employeeallownce - Daily
                                  'emp_allowanceDaily = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='1'" &
                                  '                              " AND AllowanceFrequency='" & dailyallowfreq & "'" &
                                  '                              " AND EffectiveStartDate>='" & paypFrom & "'" &
                                  '                              " AND EffectiveEndDate<='" & paypTo & "'" &
                                  '                              " GROUP BY EmployeeID;")

                                  'emp_allowanceDaily = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount'" &
                                  '                              ",EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='1'" &
                                  '                              " AND AllowanceFrequency='" & dailyallowfreq & "'" &
                                  '                              " AND IF(DATEDIFF(COALESCE(EffectiveEndDate,EffectiveStartDate),EffectiveStartDate) > DATEDIFF('" & paypTo & "','" & paypFrom & "')" &
                                  '                              ",EffectiveStartDate<='" & paypFrom & "' AND EffectiveEndDate>='" & paypTo & "'" &
                                  '                              ",EffectiveStartDate>='" & paypFrom & "' AND EffectiveEndDate<='" & paypTo & "')" &
                                  '                              " GROUP BY EmployeeID;")

                                  '",(DATEDIFF('" & paypTo & "',EffectiveStartDate) + 1) 'allowmultiplier'" &

                                  'emp_allowanceDaily = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount'" &
                                  '                              ",EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='1'" &
                                  '                              " AND AllowanceFrequency='" & dailyallowfreq & "'" &
                                  '                              " AND IF(EffectiveStartDate > '" & paypFrom & "' AND EffectiveEndDate > '" & paypTo & "'" &
                                  '                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveStartDate < '" & paypFrom & "' AND EffectiveEndDate < '" & paypTo & "'" &
                                  '                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveStartDate <= '" & paypFrom & "' AND EffectiveEndDate >= '" & paypTo & "'" &
                                  '                              ", '" & paypTo & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                  '                              ", IF(EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                  '                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveEndDate IS NULL" &
                                  '                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                  '                              ")))))" &
                                  '                              " GROUP BY EmployeeID;")

                                  emp_allowanceDaily = New ReadSQLProcedureToDatatable("GET_employee_allowanceofthisperiod",
                                                                                       orgztnID,
                                                                                       "Daily",
                                                                                       "1",
                                                                                       paypFrom,
                                                                                       paypTo).ResultTable

                                  'notax_allowanceDaily = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount'" &
                                  '                              ",EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='0'" &
                                  '                              " AND AllowanceFrequency='" & dailyallowfreq & "'" &
                                  '                              " AND IF(EffectiveStartDate > '" & paypFrom & "' AND EffectiveEndDate > '" & paypTo & "'" &
                                  '                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveStartDate < '" & paypFrom & "' AND EffectiveEndDate < '" & paypTo & "'" &
                                  '                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveStartDate <= '" & paypFrom & "' AND EffectiveEndDate >= '" & paypTo & "'" &
                                  '                              ", '" & paypTo & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                  '                              ", IF(EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                  '                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveEndDate IS NULL" &
                                  '                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                  '                              ")))))" &
                                  '                              " GROUP BY EmployeeID;")

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
                                                                " WHERE OrganizationID=" & orgztnID & _
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
                                                                " WHERE OrganizationID=" & orgztnID & _
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

                                  'employeeallownce - Monthly
                                  'emp_allowanceMonthly = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='1'" &
                                  '                              " AND AllowanceFrequency='" & monthlyallowfreq & "'" &
                                  '                              " AND EffectiveStartDate>='" & paypFrom & "'" &
                                  '                              " AND EffectiveEndDate<='" & paypTo & "'" &
                                  '                              " AND DATEDIFF(CURRENT_DATE(),EffectiveStartDate)>=0" &
                                  '                              " GROUP BY EmployeeID" &
                                  '                              " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  'emp_allowanceMonthly = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='1'" &
                                  '                              " AND AllowanceFrequency='" & monthlyallowfreq & "'" &
                                  '                              " AND '" & If(paypTo = Nothing, paypFrom, paypTo) & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                  '                              " GROUP BY EmployeeID" &
                                  '                              " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  'emp_allowanceMonthly = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='1'" &
                                  '                              " AND AllowanceFrequency='" & monthlyallowfreq & "'" &
                                  '                              " AND IF(EffectiveStartDate > '" & paypFrom & "' AND EffectiveEndDate > '" & paypTo & "'" &
                                  '                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveStartDate < '" & paypFrom & "' AND EffectiveEndDate < '" & paypTo & "'" &
                                  '                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveStartDate <= '" & paypFrom & "' AND EffectiveEndDate >= '" & paypTo & "'" &
                                  '                              ", '" & paypTo & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                  '                              ", IF(EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                  '                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveEndDate IS NULL" &
                                  '                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                  '                              ")))))" &
                                  '                              " GROUP BY EmployeeID" &
                                  '                              " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  emp_allowanceMonthly = New ReadSQLProcedureToDatatable("GET_employee_allowanceofthisperiod",
                                                                                       orgztnID,
                                                                                       "Monthly",
                                                                                       "1",
                                                                                       paypFrom,
                                                                                       paypTo).ResultTable

                                  'notax_allowanceMonthly = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='0'" &
                                  '                              " AND AllowanceFrequency='" & monthlyallowfreq & "'" &
                                  '                              " AND IF(EffectiveStartDate > '" & paypFrom & "' AND EffectiveEndDate > '" & paypTo & "'" &
                                  '                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveStartDate < '" & paypFrom & "' AND EffectiveEndDate < '" & paypTo & "'" &
                                  '                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveStartDate <= '" & paypFrom & "' AND EffectiveEndDate >= '" & paypTo & "'" &
                                  '                              ", '" & paypTo & "' BETWEEN EffectiveStartDate AND EffectiveEndDate" &
                                  '                              ", IF(EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                  '                              ", EffectiveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", IF(EffectiveEndDate IS NULL" &
                                  '                              ", EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              ", EffectiveStartDate >= '" & paypFrom & "' AND EffectiveEndDate <= '" & paypTo & "'" &
                                  '                              ")))))" &
                                  '                              " GROUP BY EmployeeID" &
                                  '                              " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  notax_allowanceMonthly = New ReadSQLProcedureToDatatable("GET_employee_allowanceofthisperiod",
                                                                                       orgztnID,
                                                                                       "Monthly",
                                                                                       "0",
                                                                                       paypFrom,
                                                                                       paypTo).ResultTable

                                  emp_bonusMonthly = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                                 ",EmployeeID" &
                                                                " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID & _
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
                                                                " WHERE OrganizationID=" & orgztnID & _
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

                                  'employeeallownce - One time
                                  'emp_allowanceOnce = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='1'" &
                                  '                              " AND AllowanceFrequency='" & onceallowfreq & "'" &
                                  '                              " AND EffectiveStartDate='" & paypFrom & "'" &
                                  '                              " AND DATEDIFF(CURRENT_DATE(),EffectiveStartDate)>=0" &
                                  '                              " GROUP BY EmployeeID" &
                                  '                              " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  emp_allowanceOnce = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                                                " FROM employeeallowance" &
                                                                " WHERE OrganizationID=" & orgztnID & _
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & onceallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  notax_allowanceOnce = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                                                " FROM employeeallowance" &
                                                                " WHERE OrganizationID=" & orgztnID & _
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & onceallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  emp_bonusOnce = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID & _
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & onceallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  notax_bonusOnce = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID & _
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & onceallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")




                                  'allowfreq : Daily'Monthly'One time'Semi-monthly'Weekly

                                  Dim semimallowfreq = "Semi-monthly"

                                  If allowfreq.Count <> 0 Then
                                      semimallowfreq = If(allowfreq.Item(3).ToString = "", "Semi-monthly", allowfreq.Item(3).ToString)
                                  End If

                                  'emp_allowanceSemiM
                                  'notax_allowanceSemiM
                                  'emp_bonusSemiM
                                  'notax_bonusSemiM

                                  emp_bonusSemiM = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID & _
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  notax_bonusSemiM = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID & _
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")


                                  'emp_allowanceSemiM = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='1'" &
                                  '                              " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                  '                              " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              " GROUP BY EmployeeID" &
                                  '                              " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  emp_allowanceSemiM = New ReadSQLProcedureToDatatable("GET_employee_allowanceofthisperiod",
                                                                                       orgztnID,
                                                                                       "Semi-monthly",
                                                                                       "1",
                                                                                       paypFrom,
                                                                                       paypTo).ResultTable

                                  'notax_allowanceSemiM = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                  '                              " FROM employeeallowance" &
                                  '                              " WHERE OrganizationID=" & orgztnID & _
                                  '                              " AND TaxableFlag='0'" &
                                  '                              " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                  '                              " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                  '                              " GROUP BY EmployeeID" &
                                  '                              " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

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

                                  'emp_allowanceWeekly
                                  'notax_allowanceWeekly
                                  'emp_bonusWeekly
                                  'notax_bonusWeekly

                                  emp_bonusWeekly = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID & _
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  notax_bonusWeekly = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                                              ",EmployeeID" &
                                                              " FROM employeebonus" &
                                                                " WHERE OrganizationID=" & orgztnID & _
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")


                                  emp_allowanceWeekly = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                                                " FROM employeeallowance" &
                                                                " WHERE OrganizationID=" & orgztnID & _
                                                                " AND TaxableFlag='1'" &
                                                                " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

                                  notax_allowanceWeekly = retAsDatTbl("SELECT SUM(COALESCE(AllowanceAmount,0)) 'TotalAllowanceAmount',EmployeeID" &
                                                                " FROM employeeallowance" &
                                                                " WHERE OrganizationID=" & orgztnID & _
                                                                " AND TaxableFlag='0'" &
                                                                " AND AllowanceFrequency='" & semimallowfreq & "'" &
                                                                " AND EffectiveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveStartDate);")

















                                  ''employeebonus
                                  'emp_bonus = retAsDatTbl("SELECT SUM(COALESCE(BonusAmount,0)) 'BonusAmount'" &
                                  '                        ",EmployeeID" &
                                  '                        " FROM employeebonus" &
                                  '                        " WHERE OrganizationID=" & orgztnID & _
                                  '                        " AND EffectiveStartDate>='" & paypFrom & "'" &
                                  '                        " AND EffectiveEndDate<='" & paypTo & "'" &
                                  '                        " AND TaxableFlag='1'" &
                                  '                        " GROUP BY EmployeeID" &
                                  '                        " ORDER BY EmployeeID;")

                                  'esal_dattab = retAsDatTbl("SELECT RowID,EmployeeID,Created,CreatedBy,COALESCE(LastUpd,'') 'LastUpd',COALESCE(LastUpdBy,'') 'LastUpdBy',OrganizationID,COALESCE(FilingStatusID,'') 'FilingStatusID',COALESCE(PaySocialSecurityID,'') 'PaySocialSecurityID',COALESCE(PayPhilhealthID,'') 'PayPhilhealthID',COALESCE(HDMFAmount,'') 'HDMFAmount',BasicPay,Salary,BasicDailyPay,BasicHourlyPay,NoofDependents,MaritalStatus,PositionID,EffectiveDateFrom,COALESCE(EffectiveDateTo,'') 'EffectiveDateTo'" &

                                  'employeesalary
                                  'esal_dattab = retAsDatTbl("SELECT *,COALESCE((SELECT EmployeeShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployeeShare'" &
                                  '                            ",COALESCE((SELECT EmployerShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployerShare'" &
                                  '                            ",COALESCE((SELECT EmployeeContributionAmount FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployeeContributionAmount'" &
                                  '                            ",COALESCE((SELECT (EmployerContributionAmount + EmployeeECAmount) FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployerContributionAmount'" &
                                  '                            " FROM employeesalary" &
                                  '                            " WHERE OrganizationID=" & orgztnID & "" &
                                  '                            " AND (EffectiveDateFrom >= '" & paypFrom & "' AND IFNULL(EffectiveDateTo,CURDATE()) >= '" & paypFrom & "')" &
                                  '                            " AND (EffectiveDateFrom <= '" & paypTo & "' AND IFNULL(EffectiveDateTo,CURDATE()) <= '" & paypTo & "')" &
                                  '                        " UNION" &
                                  '                            " SELECT *,COALESCE((SELECT EmployeeShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployeeShare'" &
                                  '                            ",COALESCE((SELECT EmployerShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployerShare'" &
                                  '                            ",COALESCE((SELECT EmployeeContributionAmount FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployeeContributionAmount'" &
                                  '                            ",COALESCE((SELECT (EmployerContributionAmount + EmployeeECAmount) FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployerContributionAmount'" &
                                  '                            " FROM employeesalary" &
                                  '                            " WHERE OrganizationID=" & orgztnID & "" &
                                  '                            " AND '" & paypFrom & "' BETWEEN EffectiveDateFrom AND EffectiveDateTo" &
                                  '                        " UNION" &
                                  '                            " SELECT *,COALESCE((SELECT EmployeeShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployeeShare'" &
                                  '                            ",COALESCE((SELECT EmployerShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployerShare'" &
                                  '                            ",COALESCE((SELECT EmployeeContributionAmount FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployeeContributionAmount'" &
                                  '                            ",COALESCE((SELECT (EmployerContributionAmount + EmployeeECAmount) FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployerContributionAmount'" &
                                  '                            " FROM employeesalary" &
                                  '                            " WHERE OrganizationID=" & orgztnID & "" &
                                  '                            " AND '" & paypTo & "' BETWEEN EffectiveDateFrom AND EffectiveDateTo" &
                                  '                        " UNION" &
                                  '                            " SELECT *,COALESCE((SELECT EmployeeShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployeeShare'" &
                                  '                            ",COALESCE((SELECT EmployerShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployerShare'" &
                                  '                            ",COALESCE((SELECT EmployeeContributionAmount FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployeeContributionAmount'" &
                                  '                            ",COALESCE((SELECT (EmployerContributionAmount + EmployeeECAmount) FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployerContributionAmount'" &
                                  '                            " FROM employeesalary" &
                                  '                            " WHERE OrganizationID=" & orgztnID & "" &
                                  '                            " AND (EffectiveDateFrom >= '" & paypFrom & "' AND EffectiveDateTo <= '" & paypTo & "')" &
                                  '                            " GROUP BY EmployeeID" &
                                  '                            " ORDER BY DATEDIFF(DATE_FORMAT(CURDATE(),'%Y-%m-%d'),EffectiveDateFrom)" &
                                  '                            ";")

                                  esal_dattab = New SQLQueryToDatatable(" SELECT *,COALESCE((SELECT EmployeeShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployeeShare'" &
                                                              ",COALESCE((SELECT EmployerShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployerShare'" &
                                                              ",COALESCE((SELECT EmployeeContributionAmount FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployeeContributionAmount'" &
                                                              ",COALESCE((SELECT (EmployerContributionAmount + EmployeeECAmount) FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployerContributionAmount'" &
                                                              " FROM employeesalary" &
                                                              " WHERE OrganizationID=" & orgztnID & "" &
                                                              " AND (EffectiveDateFrom >= '" & paypFrom & "' OR IFNULL(EffectiveDateTo,CURDATE()) >= '" & paypFrom & "')" &
                                                              " AND (EffectiveDateFrom <= '" & paypTo & "' OR IFNULL(EffectiveDateTo,CURDATE()) <= '" & paypTo & "')" &
                                                              " GROUP BY EmployeeID" &
                                                              " ORDER BY DATEDIFF(DATE_FORMAT(CURDATE(),'%Y-%m-%d'),EffectiveDateFrom)" &
                                                              ";").ResultTable

                                  '" AND EffectiveDateFrom>='" & paypFrom & "'" &
                                  '" AND COALESCE(EffectiveDateTo,CURRENT_DATE())<='" & paypTo & "'" &

                                  '" AND DATEDIFF(CURRENT_DATE(),EffectiveDateFrom) >= 0" &

                                  numofdaypresent = retAsDatTbl("SELECT COUNT(RowID) 'DaysAttended'" &
                                                                ",SUM((TIME_TO_SEC(TIMEDIFF(TimeOut,TimeIn)) / 60) / 60) 'SumHours'" &
                                                                ",EmployeeID" &
                                                                " FROM employeetimeentrydetails" &
                                                                " WHERE OrganizationID=" & orgztnID & "" &
                                                                " AND Date BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                                " GROUP BY EmployeeID;")

                                  'Clothing,Meal,Rice,Transportation
                                  allowtyp = EXECQUER("SELECT GROUP_CONCAT(RowID) FROM product WHERE OrganizationID='" & orgztnID & "' AND Category='Allowance Type' ORDER BY PartNo;")
                                  'CategoryName
                                  allow_type = Split(allowtyp, ",")

                                  'Absent,Tardiness,Undertime,.PAGIBIG,.PhilHealth,.SSS
                                  deductions = EXECQUER("SELECT GROUP_CONCAT(RowID) FROM product WHERE OrganizationID='" & orgztnID & "' AND Category='Deductions' ORDER BY PartNo;")

                                  arraydeduction = Split(deductions, ",")

                                  'PhilHealth,SSS,PAGIBIG
                                  loan_type = EXECQUER("SELECT GROUP_CONCAT(RowID) FROM product WHERE OrganizationID='" & orgztnID & "' AND Category='Loan Type' ORDER BY PartNo;")

                                  loantyp = Split(loan_type, ",")

                                  'Miscellaneous - Overtime,Night differential OT,Holiday pay
                                  misc = EXECQUER("SELECT GROUP_CONCAT(RowID) FROM product WHERE OrganizationID='" & orgztnID & "' AND Category='Miscellaneous' ORDER BY PartNo;")

                                  miscs = Split(misc, ",")

                                  'Totals - Withholding Tax,Gross Income,Net Income,Taxable Income
                                  totals = EXECQUER("SELECT GROUP_CONCAT(CONCAT(PartNo,';',RowID)) FROM product WHERE OrganizationID='" & orgztnID & "' AND Category='Totals' ORDER BY PartNo;") 'BusinessUnitID
                                  'GROUP_CONCAT(RowID)
                                  emp_totals = Split(totals, ",")

                                  'Leave Type - Vacation leave,Sick leave,Maternity/paternity leave,Others
                                  leavetype = EXECQUER("SELECT GROUP_CONCAT(RowID) FROM product WHERE OrganizationID='" & orgztnID & "' AND Category='Leave Type' ORDER BY PartNo;")

                                  leavtyp = Split(leavetype, ",")

                                  'allowkind

                                  'SELECT * FROM product WHERE CategoryID='33' AND OrganizationID=2;

                                  'allow_kind()

                                  'employeeleave
                                  'empleave = retAsDatTbl("SELECT elv.*" &
                                  '                        ",SUM((DATEDIFF(COALESCE(elv.LeaveEndDate,elv.LeaveStartDate),elv.LeaveStartDate) + 1)) 'NumOfDaysLeave'" &
                                  '                        ",COALESCE(((TIME_TO_SEC(TIMEDIFF(elv.LeaveEndTime,elv.LeaveStartTime)) / 60) / 60),0) 'NumOfHoursLeave'" &
                                  '                        ",e.LeavePerPayPeriod" &
                                  '                        ",COALESCE((SELECT RowID FROM product WHERE PartNo=elv.LeaveType AND OrganizationID=" & orgztnID & "),'') 'ProductID'" &
                                  '                        " FROM employeeleave elv LEFT JOIN employee e ON e.RowID=elv.EmployeeID" &
                                  '                        " WHERE elv.OrganizationID=" & orgztnID & _
                                  '                        " AND elv.LeaveStartDate>='" & paypFrom & "'" &
                                  '                        " AND elv.LeaveEndDate<='" & paypTo & "'" &
                                  '                        " GROUP BY elv.LeaveType" &
                                  '                        " ORDER BY elv.EmployeeID;")

                                  empleave = retAsDatTbl("SELECT elv.*" &
                                                          ",SUM((DATEDIFF(COALESCE(elv.LeaveEndDate,elv.LeaveStartDate),elv.LeaveStartDate) + 1)) 'NumOfDaysLeave'" &
                                                          ",COALESCE(((TIME_TO_SEC(TIMEDIFF(elv.LeaveEndTime,elv.LeaveStartTime)) / 60) / 60),0) 'NumOfHoursLeave'" &
                                                          ",e.LeavePerPayPeriod" &
                                                          ",COALESCE((SELECT RowID FROM product WHERE PartNo=elv.LeaveType AND OrganizationID=" & orgztnID & "),'') 'ProductID'" &
                                                          " FROM employeeleave elv LEFT JOIN employee e ON e.RowID=elv.EmployeeID" &
                                                          " WHERE elv.OrganizationID=" & orgztnID & _
                                                          " AND IF(elv.LeaveStartDate > '" & paypFrom & "' AND elv.LeaveEndDate > '" & paypTo & "'" &
                                                          ", elv.LeaveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                          ", IF(elv.LeaveStartDate < '" & paypFrom & "' AND elv.LeaveEndDate < '" & paypTo & "'" &
                                                          ", elv.LeaveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                          ", IF(elv.LeaveStartDate <= '" & paypFrom & "' AND elv.LeaveEndDate >= '" & paypTo & "'" &
                                                          ", '" & paypTo & "' BETWEEN elv.LeaveStartDate AND elv.LeaveEndDate" &
                                                          ", IF(elv.LeaveStartDate >= '" & paypFrom & "' AND elv.LeaveEndDate <= '" & paypTo & "'" &
                                                          ", elv.LeaveEndDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                          ", IF(elv.LeaveEndDate IS NULL" &
                                                          ", elv.LeaveStartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "'" &
                                                          ", elv.LeaveStartDate >= '" & paypFrom & "' AND elv.LeaveEndDate <= '" & paypTo & "'" &
                                                          ")))))" &
                                                          " GROUP BY elv.LeaveType" &
                                                          " ORDER BY elv.EmployeeID;")

                                  If withthirteenthmonthpay = 1 Then

                                      Dim params(1, 2) As Object

                                      params(0, 0) = "param_OrganizationID"
                                      params(1, 0) = "param_year"

                                      params(0, 1) = orgztnID
                                      params(1, 1) = paypTo

                                      empthirteenmonthtable = _
                                                  GetAsDataTable("GET_Attended_Months", _
                                                                 CommandType.StoredProcedure, _
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

                                  etent_holidaypay = callProcAsDatTab(param, _
                                                                      "GET_employeeholidaypay")

                                  Dim paramets(2, 2) As Object

                                  paramets(0, 0) = "OrganizID"
                                  paramets(1, 0) = "payp_FromDate"
                                  paramets(2, 0) = "payp_ToDate"

                                  paramets(0, 1) = orgztnID
                                  paramets(1, 1) = paypFrom
                                  paramets(2, 1) = paypTo

                                  emp_TardinessUndertime = _
                                      callProcAsDatTab(paramets, _
                                                       "GETVIEW_employeeTardinessUndertime")

                                  prev_empTimeEntry = New ReadSQLProcedureToDatatable("GETVIEW_previousemployeetimeentry",
                                                                                      orgztnID,
                                                                                      paypRowID,
                                                                                      paypRowID).ResultTable
                                  'prev_empTimeEntry

                                  'RemoveHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

                                  ''RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

                                  Dim MinimumWage_Amount = _
                                      EXECQUER("SELECT `GET_MinimumWageRate`();")

                                  MinimumWageAmount = ValNoComma(MinimumWage_Amount)

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

                                  'bgworkgenpayroll.RunWorkerAsync()

                                  'Thread_Method(ToolStripButton2, New EventArgs)

                                  'If etent_dattab.Rows.Count <> 0 Then

                                  'Else
                                  '    MsgBox("Employee time entry from " & Format(CDate(paypFrom), "MMM-d-yyyy") & " to " & Format(CDate(paypTo), "MMM-d-yyyy") & " is not yet prepared.", MsgBoxStyle.Information)

                                  'End If

                                  'End If

                              End Sub, 0).ContinueWith(Sub()

                                                           indxStartBatch = 0

                                                           Dim n_lov_mxthread As New ExecuteQuery("SELECT CAST(DisplayValue AS INT) `Result` FROM listofval WHERE `Type`='Max thread count' AND LIC='Max thread count' AND Active='Yes' LIMIT 1;")
                                                           If ValNoComma(n_lov_mxthread.Result) > 0 Then
                                                               thread_max = ValNoComma(n_lov_mxthread.Result)
                                                           Else
                                                               n_lov_mxthread = New  _
                                                                   ExecuteQuery(String.Concat("INSERT INTO `listofval` (`DisplayValue`, `LIC`, `Type`, `ParentLIC`, `Active`, `Description`, `Created`, `CreatedBy`, `LastUpd`, `OrderBy`, `LastUpdBy`) VALUES ('5', 'Max thread count', 'Max thread count', '', 'Yes', 'max thread count when generating payroll', CURRENT_TIMESTAMP(), ", z_User, ", CURRENT_TIMESTAMP(), 1, ", z_User, ") ON DUPLICATE KEY UPDATE LastUpd = CURRENT_TIMESTAMP(), LastUpdBy = IFNULL(LastUpdBy,CreatedBy);"))
                                                           End If

                                                           progress_precentage = 0

                                                           ThreadingPayrollGeneration(thread_max)

                                                       End Sub, TaskScheduler.FromCurrentSynchronizationContext)

    End Sub

    Private thread_max As Integer = 5

    Dim SpDataSet As DataSet = New DataSet

    Const max_rec_perpage As Integer = 20

    Dim emp_list_batcount As Integer = 0

    Private Sub ThreadingPayrollGeneration(Optional starting_batchindex As Integer = 0)

        '# ################################################################################################################################################ #

        Timer1.Stop()
        Timer1.Enabled = False

        Dim emp_count = employee_dattab.Rows.Count

        Dim process_seconds As Integer = max_rec_perpage * 1000
        Dim loop_max_ctr = emp_count / max_rec_perpage
        loop_max_ctr = (loop_max_ctr - (loop_max_ctr Mod 1)) + 1

        ReDim multi_threads(loop_max_ctr)
        multithreads.Clear()
        Dim i = 0

        Dim erro_msg_length As Integer = 0

        Dim SpCmd As MySqlCommand = New MySqlCommand
        Dim conn_bool As Boolean = False

        Try

            SpCmd = New MySqlCommand("EMPLOYEE_payrollgen_paginate",
                                      New MySql.Data.MySqlClient.MySqlConnection(mysql_conn_text))

            If Me.Enabled Then 'tsbtngenpayroll
                SpDataSet = New DataSet
                With SpCmd
                    conn_bool = (.Connection.State = ConnectionState.Closed)
                    If conn_bool Then
                        .Connection.Open()
                    End If
                    .CommandTimeout = 5000
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Clear()
                    .Parameters.AddWithValue("OrganizID", orgztnID)
                    .Parameters.AddWithValue("Pay_Date_From", paypFrom)
                    .Parameters.AddWithValue("Pay_Date_To", paypTo)
                    .Parameters.AddWithValue("max_rec_perpage", max_rec_perpage)
                End With

                Dim MyAdapter As MySqlDataAdapter = New MySqlDataAdapter(SpCmd)
                MyAdapter.Fill(SpDataSet)
                
                payroll_emp_count = 0

                Dim valid_table = SpDataSet.Tables.Cast(Of DataTable).Where(Function(dta) Convert.ToInt16(dta.Rows.Count) > 0)

                emp_list_batcount = valid_table.Count

                For Each dt As DataTable In valid_table 'SpDataSet.Tables
                    payroll_emp_count += Convert.ToInt16(dt.Rows.Count)
                Next

                Me.Enabled = False 'tsbtngenpayroll

            End If

        Catch ex As Exception
            Dim err_msg As String = getErrExcptn(ex, Me.Name)
            erro_msg_length = err_msg.Length
            MsgBox(err_msg)
        Finally
            SpCmd.Connection.Close()
            SpCmd.Dispose()

            If erro_msg_length = 0 Then
                'BusyBGWorkChecker.RunWorkerAsync()

                Dim tasks As New List(Of Task)

                'Task.Factory.StartNew(Sub()
                Dim tblcount As Integer = Convert.ToInt16(SpDataSet.Tables.Count)

                ReDim array_bgwork(tblcount - 1)

                Dim min_index As Integer = starting_batchindex - thread_max

                For Each dt As DataTable In SpDataSet.Tables

                    If dt.Rows.Count > 0 Then

                        If min_index <= i Then

                            Dim n_PayrollGeneration As New PayrollGeneration(dt,
                                                                              isEndOfMonth,
                                                                              esal_dattab,
                                                                              emp_loans,
                                                                              emp_bonus,
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
                                                                              withthirteenthmonthpay, Me)

                            With n_PayrollGeneration
                                .PayrollDateFrom = paypFrom
                                .PayrollDateTo = paypTo
                                .PayrollRecordID = paypRowID

                                Dim n_bgwork As New System.ComponentModel.BackgroundWorker() With {.WorkerReportsProgress = True, .WorkerSupportsCancellation = True}

                                array_bgwork(i) = n_bgwork

                                AddHandler array_bgwork(i).DoWork, AddressOf .PayrollGeneration_BackgourndWork
                                If i = 0 Then
                                    Console.WriteLine(String.Concat("PROCESS STARTS @ ", Now.ToShortTimeString, "....."))
                                End If
                                array_bgwork(i).RunWorkerAsync()
                                'Thread.Sleep(3500) 'process_seconds

                            End With

                            'tasks.Add(
                            '    Task.Factory.StartNew(Sub()
                            '                              n_PayrollGeneration.DoProcess()

                            '                              _uiTasks.StartNew(Sub()
                            '                                                    'Thread.Sleep(1000)
                            '                                                End Sub)

                            '                          End Sub)
                            '                      )
                            ''Thread.Sleep(3000)
                            ''ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf n_PayrollGeneration.DoProcess))
                            'Dim objNewThread As Thread = New Thread(AddressOf n_PayrollGeneration.DoProcess)

                            'objNewThread.IsBackground = True

                            'objNewThread.Start()

                            'multi_threads(i) = objNewThread
                            'multithreads.Add(objNewThread)

                            'Thread.Sleep(3500)

                            i += 1

                            'If (i Mod thread_max) = 0 Then
                            If (i Mod starting_batchindex) = 0 Then
                                indxStartBatch = starting_batchindex
                                'Dim ii = 5000
                                'Console.WriteLine(String.Concat("...batch ", batch, " already running, wait ", (ii / 1000), " sec for next batch"))
                                'Thread.Sleep(ii)
                                Exit For
                            Else
                                Continue For

                            End If

                        Else

                            i += 1

                            Continue For

                        End If

                    Else
                        Continue For

                    End If

                Next

                Timer1.Enabled = True
                Timer1.Start()

                '    _uiTasks.StartNew(Sub()

                '                      End Sub)

                'End Sub)

                'Task.WaitAll(tasks.ToArray())

                Dim callthem = "Hey, over here"

                'Dim valid_threads = multi_threads.Cast(Of Thread).Where(Function(x) x IsNot Nothing)

                'Dim thread_counter = valid_threads.Count

                'Dim thereAreThreadStillRunning As Boolean = Recursive(True)

                'callthem = "Hey, over here"

                ''            If Thread.CurrentThread.ThreadState = ThreadState.Running Then
                ''                GoTo fsfsdf
                ''            End If

                ''fsfsdf:     If Thread.CurrentThread.ThreadState = ThreadState.Running Then : GoTo fsfsdf : End If

                ''Dim n_ImportLoans As New ImportLoans(catchDatSet, Me)

                ''Dim objNewThread As New Thread(AddressOf n_ImportLoans.StartProcess)

                ''objNewThread.IsBackground = True

                ''objNewThread.Start()

                ''threadArrayList.Add(objNewThread)

                '# ################################################################################################################################################ #

            End If

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
                objText.Text = " ₱ " & txtempbasicpay.Text

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
                objText.Text = txthrswork.Text

                objText = .ReportObjects("txttotregamt")
                objText.Text = "₱ " & txttotregamt.Text

                objText = .ReportObjects("txttotothrs")
                objText.Text = txttotothrs.Text

                objText = .ReportObjects("txttototamt")
                objText.Text = "₱ " & txttototamt.Text

                objText = .ReportObjects("txttotnightdiffhrs")
                objText.Text = txttotnightdiffhrs.Text

                objText = .ReportObjects("txttotnightdiffamt")
                objText.Text = "₱ " & txttotnightdiffamt.Text

                objText = .ReportObjects("txttotnightdiffothrs")
                objText.Text = txttotnightdiffothrs.Text

                objText = .ReportObjects("txttotnightdiffotamt")
                objText.Text = "₱ " & txttotnightdiffotamt.Text

                objText = .ReportObjects("txttotholidayhrs")
                objText.Text = txttotholidayhrs.Text

                objText = .ReportObjects("txttotholidayamt")
                objText.Text = "₱ " & txttotholidayamt.Text
                '₱
                objText = .ReportObjects("txthrswork")
                objText.Text = txttotreghrs.Text

                objText = .ReportObjects("txthrsworkamt")
                objText.Text = "₱ " & txthrsworkamt.Text

                objText = .ReportObjects("lblsubtot")
                objText.Text = "₱ " & lblsubtot.Text

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

                    VIEW_eallow_indate(dgvemployees.CurrentRow.Cells("RowID").Value, _
                                        paypFrom, _
                                        paypTo)

                    VIEW_eloan_indate(dgvemployees.CurrentRow.Cells("RowID").Value, _
                                        paypFrom, _
                                        paypTo)

                    VIEW_ebon_indate(dgvemployees.CurrentRow.Cells("RowID").Value, _
                                        paypFrom, _
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

    Private Sub tsbtnprintall_Click(sender As Object, e As EventArgs) 'Handles DeclaredToolStripMenuItem1.Click 'tsbtnprintall.Click

        Dim sssProductID = Nothing

        Dim phhProductID = Nothing

        Dim hdmfProductID = Nothing

        Dim dtprod = retAsDatTbl("SELECT p.RowID" &
                                 ",p1.RowID AS p1RowID" &
                                 ",p2.RowID AS p2RowID" &
                                 " FROM product p" &
                                 " INNER JOIN product p1 ON p1.OrganizationID='" & orgztnID & "' AND p1.PartNo='.PhilHealth'" &
                                 " INNER JOIN product p2 ON p2.OrganizationID='" & orgztnID & "' AND p2.PartNo='.PAGIBIG'" &
                                 " WHERE p.OrganizationID='" & orgztnID & "'" &
                                 " AND p.PartNo='.SSS';")

        For Each drrow As DataRow In dtprod.Rows

            sssProductID = drrow("RowID")

            phhProductID = drrow("p1RowID")

            hdmfProductID = drrow("p2RowID")

        Next

        ''RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        If paypRowID = Nothing Then

            ''AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            Exit Sub

        End If

        Dim pay_stbitem As New DataTable 'this is for leave balances

        pay_stbitem = retAsDatTbl("SELECT" &
                                  " pi.PayStubID" &
                                  ",pi.ProductID" &
                                  ",p.PartNo" &
                                  ",pi.PayAmount" &
                                  " FROM paystubitem pi" &
                                  " LEFT JOIN product p ON p.RowID = pi.ProductID" &
                                  " LEFT JOIN paystub ps ON ps.RowID = pi.PayStubID" &
                                  " WHERE p.Category='Leave Type'" &
                                  " AND p.OrganizationID=" & orgztnID & _
                                  " AND ps.PayPeriodID='" & paypRowID & "';") 'this is for leave balances

        Dim rptdattab As New DataTable

        With rptdattab.Columns

            .Add("Column1", Type.GetType("System.Int32"))
            .Add("Column2", Type.GetType("System.String"))
            .Add("Column3") 'Employee Full Name

            .Add("Column4") 'Gross Income

            .Add("Column5") 'Net Income
            .Add("Column6") 'Taxable salary

            .Add("Column7") 'Withholding Tax
            .Add("Column8") 'Total Allowance

            .Add("Column9") 'Total Loans
            .Add("Column10") 'Total Bonuses

            .Add("Column11") 'Basic Pay
            .Add("Column12") 'SSS Amount

            .Add("Column13") 'PhilHealth Amount
            .Add("Column14") 'PAGIBIG Amount

            .Add("Column15") 'Sub Total - Right side
            .Add("Column16") 'txthrsworkamt

            .Add("Column17") 'Regular hours worked

            .Add("Column18") 'Regular hours amount

            .Add("Column19") 'Overtime hours worked

            .Add("Column20") 'Overtime hours amount
            .Add("Column21") 'Night differential hours worked
            .Add("Column22") 'Night differential hours amount

            .Add("Column23") 'Night differential OT hours worked
            .Add("Column24") 'Night differential OT hours amount

            .Add("Column25") 'Total hours worked

            .Add("Column26") 'Undertime hours

            .Add("Column27") 'Undertime amount
            .Add("Column28") 'Late hours

            .Add("Column29") 'Late amount

            .Add("Column30") 'Leave type
            .Add("Column31") 'Leave count
            .Add("Column32")

            .Add("Column33")

            .Add("Column34") 'Allowance type
            .Add("Column35") 'Loan type
            .Add("Column36") 'Bonus type





            .Add("Column37") 'Allowance amount
            .Add("Column38") 'Loan amount
            .Add("Column39") 'Bonus amount

        End With

        'employee_dattab = retAsDatTbl("SELECT e.* FROM" &
        '                              " employee e LEFT JOIN employeesalary esal ON e.RowID=esal.EmployeeID" &
        '                              " WHERE e.OrganizationID=" & orgztnID & _
        '                              " AND '" & paypTo & "' BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,'" & paypTo & "')" &
        '                              " GROUP BY e.RowID" &
        '                              " ORDER BY e.LastName;") 'RowID DESC

        employee_dattab = retAsDatTbl("SELECT e.*" &
                                      ",ps.RowID as psRowID" &
                                      " FROM employee e LEFT JOIN employeesalary esal ON e.RowID=esal.EmployeeID" &
                                      " LEFT JOIN paystub ps ON ps.EmployeeID=e.RowID AND ps.OrganizationID=e.OrganizationID AND ps.PayPeriodID='" & paypRowID & "'" &
                                      " WHERE e.OrganizationID=" & orgztnID & _
                                      " AND e.PayFrequencyID='" & paypPayFreqID & "'" &
                                      " AND '" & paypTo & "' BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,'" & paypTo & "')" &
                                      " GROUP BY e.RowID" &
                                      " ORDER BY e.LastName;") 'RowID DESC  

        Dim employeeloanfulldetails As New DataTable

        employeeloanfulldetails = retAsDatTbl("CALL employeeloanfulldetails('" & orgztnID & "'" &
                                              ",'" & paypPayFreqID & "'" &
                                              ",'" & paypFrom & "'" &
                                              ",'" & paypTo & "');")

        Dim newdatrow As DataRow

        For Each drow As DataRow In employee_dattab.Rows
            newdatrow = rptdattab.NewRow

            newdatrow("Column1") = drow("RowID") 'Employee RowID
            newdatrow("Column2") = drow("EmployeeID") 'Employee ID

            Dim LasFirMidName As String = Nothing


            Dim midNameTwoWords = Split(drow("MiddleName").ToString, " ")

            Dim midnameinitial As String = Nothing

            For Each strmidname In midNameTwoWords

                midnameinitial &= (StrConv(Microsoft.VisualBasic.Left(strmidname, 1), VbStrConv.ProperCase) & ".")

            Next

            midnameinitial = If(Trim(midnameinitial) = Nothing, "", _
                                If(Trim(midnameinitial) = ".", "", ", " & midnameinitial))

            LasFirMidName = drow("LastName").ToString & ", " & drow("FirstName").ToString & midnameinitial

            LasFirMidName = StrConv(LasFirMidName, VbStrConv.Uppercase)

            Dim full_name = drow("FirstName").ToString & If(drow("MiddleName").ToString = Nothing, _
                                                        "", _
                                                        " " & StrConv(Microsoft.VisualBasic.Left(drow("MiddleName").ToString, 1), _
                                                        VbStrConv.ProperCase) & ".")

            full_name = full_name & " " & drow("LastName").ToString

            full_name = full_name & If(drow("Surname").ToString = Nothing, _
                                    "", _
                                    "-" & StrConv(Microsoft.VisualBasic.Left(drow("Surname").ToString, 1), _
                                    VbStrConv.ProperCase))

            newdatrow("Column3") = LasFirMidName 'full_name 'Employee Full Name


            VIEW_paystub(drow("RowID").ToString, _
                                 paypRowID)

            Dim undeclared_psi As New DataTable

            Dim declared_psi As New DataTable

            Dim its_value = drow("psRowID").ToString

            If its_value <> "" Then

                undeclared_psi = retAsDatTbl("CALL `VIEW_paystubitemundeclared`('" & drow("psRowID") & "');")

                declared_psi = retAsDatTbl("CALL `VIEW_paystubitem`('" & drow("psRowID") & "');")

            End If

            Dim totamountallow = 0.0
            Dim totamountbon = 0.0

            For Each dgvrow As DataGridViewRow In dgvpaystub.Rows
                With dgvrow

                    newdatrow("Column4") = "₱ " & FormatNumber(ValNoComma(.Cells("paystb_TotalGrossSalary").Value), 2) 'Gross Income

                    Dim gros_inc = ValNoComma(newdatrow("Column4"))

                    newdatrow("Column5") = "₱ " & FormatNumber(ValNoComma(.Cells("paystb_TotalNetSalary").Value), 2) 'Net Income

                    newdatrow("Column6") = "₱ " & FormatNumber(ValNoComma(.Cells("paystb_TotalTaxableSalary").Value), 2) 'Taxable salary

                    newdatrow("Column7") = "₱ " & FormatNumber(ValNoComma(.Cells("paystb_TotalEmpWithholdingTax").Value), 2) 'Withholding Tax

                    'newdatrow("Column3") = .Cells("paystb_TotalEmpSSS").Value
                    'newdatrow("Column3") = .Cells("paystb_TotalEmpPhilhealth").Value   
                    'newdatrow("Column3") = .Cells("paystb_TotalEmpHDMF").Value

                    newdatrow("Column8") = "₱ " & FormatNumber(ValNoComma(.Cells("paystb_TotalAllowance").Value), 2) 'Total Allowance

                    totamountallow = ValNoComma(.Cells("paystb_TotalAllowance").Value)

                    'txtholidaypay.Text = .Cells("paystb_TotalAllowance").Value

                    'lblsubtotmisc.Text = .Cells("paystb_TotalAllowance").Value

                    newdatrow("Column9") = "₱ " & FormatNumber(ValNoComma(.Cells("paystb_TotalLoans").Value), 2) 'Total Loans

                    newdatrow("Column10") = "₱ " & FormatNumber(ValNoComma(.Cells("paystb_TotalBonus").Value), 2) 'Total Bonuses

                    totamountbon = ValNoComma(.Cells("paystb_TotalBonus").Value)

                    'txtvacleft.Text = .Cells("paystb_TotalVacationDaysLeft").Value

                    'VIEW_paystubitem(.Cells("paystb_RowID").Value)

                    'paystb_RowID

                    Dim selpay_stbitem = pay_stbitem.Select("PayStubID = " & .Cells("paystb_RowID").Value)

                    Dim firstRow = 0

                    Dim isStrListed As New List(Of String)

                    For Each datrow In selpay_stbitem 'this is for leave balances

                        '.Add("Column30") 'Leave type
                        '.Add("Column31") 'Leave count
                        Dim leavebalance = ValNoComma(datrow("PayAmount"))

                        If firstRow = 0 Then
                            If isStrListed.Contains(datrow("PartNo")) Then
                            Else
                                newdatrow("Column30") = datrow("PartNo")

                                newdatrow("Column31") = ValNoComma(datrow("PayAmount"))

                                isStrListed.Add(datrow("PartNo"))
                            End If
                        Else
                            If isStrListed.Contains(datrow("PartNo")) Then
                            Else
                                newdatrow("Column30") &= vbNewLine & datrow("PartNo")

                                newdatrow("Column31") &= vbNewLine & ValNoComma(datrow("PayAmount"))

                                isStrListed.Add(datrow("PartNo"))
                            End If
                        End If

                        firstRow += 1

                    Next 'this is for leave balances

                    isStrListed.Clear()

                End With

                Exit For
            Next

            VIEW_specificemployeesalary(drow("RowID").ToString, _
                                        paypTo)

            Dim theEmpBasicPayFix = 0.0

            For Each dgvrow As DataGridViewRow In dgvempsal.Rows
                With dgvrow
                    newdatrow("Column11") = "₱ " & FormatNumber(ValNoComma(.Cells("esal_BasicPay").Value), 2) 'Basic Pay

                    theEmpBasicPayFix = ValNoComma(.Cells("esal_BasicPay").Value) 'Basic Pay

                    Dim val_sss = If(declared_psi.Rows.Count = 0, 0, ValNoComma(declared_psi.Compute("SUM(PayAmount)", "ProductID = '" & sssProductID & "'")))

                    Dim val_phh = If(declared_psi.Rows.Count = 0, 0, ValNoComma(declared_psi.Compute("SUM(PayAmount)", "ProductID = '" & phhProductID & "'")))

                    Dim val_hdmf = If(declared_psi.Rows.Count = 0, 0, ValNoComma(declared_psi.Compute("SUM(PayAmount)", "ProductID = '" & hdmfProductID & "'")))

                    If isorgSSSdeductsched = 1 Then
                        newdatrow("Column12") = "₱ " & FormatNumber((val_sss / 2), 2)

                    Else
                        If isEndOfMonth = "0" Then
                            newdatrow("Column12") = "₱ " & FormatNumber(val_sss, 2) 'SSS Amount
                        Else
                            newdatrow("Column12") = "₱ " & "0.00" 'SSS Amount
                        End If
                        'SSS Amount

                    End If

                    If isorgPHHdeductsched = 1 Then
                        newdatrow("Column13") = "₱ " & FormatNumber((val_phh / 2), 2) 'PhilHealth Amount

                    Else
                        If isEndOfMonth = "0" Then
                            newdatrow("Column13") = "₱ " & FormatNumber(val_phh, 2) 'PhilHealth Amount
                        Else
                            newdatrow("Column13") = "₱ " & "0.00" 'PhilHealth Amount
                        End If

                    End If

                    If isorgHDMFdeductsched = 1 Then
                        newdatrow("Column14") = "₱ " & FormatNumber((val_hdmf / 2), 2) 'PAGIBIG Amount

                    Else
                        If isEndOfMonth = "0" Then
                            newdatrow("Column14") = "₱ " & FormatNumber((val_hdmf), 2) 'PAGIBIG Amount
                        Else
                            newdatrow("Column14") = "₱ " & "0.00" 'PAGIBIG Amount
                        End If

                    End If

                    Exit For

                End With

            Next

            VIEW_employeetimeentry_SUM(drow("RowID").ToString, _
                                       paypFrom, _
                                       paypTo)

            For Each dgvrow As DataGridViewRow In dgvetent.Rows
                With dgvrow

                    If drow("EmployeeType").ToString = "Fixed" Then

                        Dim validgrossinc = ValNoComma(newdatrow("Column4"))

                        If ValNoComma(.Cells("etent_TotalDayPay").Value) = 0 Then
                            If drow("PayFrequencyID").ToString = 1 Then
                                ''------------------------------------------------ITO UNG BASIC PAY

                                'newdatrow("Column4") = "₱ " & FormatNumber(valnocomma(newdatrow("Column11")) + _
                                '(.Cells("etent_OvertimeHoursAmount").Value) + _
                                '(.Cells("etent_NightDiffOTHoursAmount").Value), _
                                '2)

                                newdatrow("Column4") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") '.ToString.Replace(",", "") 'Gross Income

                                newdatrow("Column15") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'Sub Total - Right side

                                newdatrow("Column16") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'txthrsworkamt
                            Else
                                Dim totbasicpay = ValNoComma(totamountallow) + _
                                ValNoComma(.Cells("etent_OvertimeHoursAmount").Value) + _
                                ValNoComma(.Cells("etent_NightDiffOTHoursAmount").Value)

                                newdatrow("Column4") = "₱ " & FormatNumber(totbasicpay, 2) 'newdatrow("Column4") '.ToString.Replace(",", "") 'Gross Income

                                newdatrow("Column16") = "₱ " & FormatNumber(totbasicpay, 2) 'newdatrow("Column4") 'txthrsworkamt
                            End If

                        Else

                            'newdatrow("Column4") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") '.ToString.Replace(",", "") 'Gross Income

                            newdatrow("Column15") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'Sub Total - Right side

                            newdatrow("Column16") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'txthrsworkamt

                        End If

                    Else
                        newdatrow("Column15") = "₱ " & FormatNumber(ValNoComma(.Cells("etent_TotalDayPay").Value), 2) 'Sub Total - Right side

                        newdatrow("Column16") = "₱ " & FormatNumber(ValNoComma(.Cells("etent_TotalDayPay").Value), 2) 'txthrsworkamt

                    End If

                    Dim regular_hours_worked = ValNoComma(0)

                    regular_hours_worked = ValNoComma(.Cells("etent_TotalHoursWorked").Value) _
                                           - ValNoComma(.Cells("etent_OvertimeHoursWorked").Value) _
                                           - ValNoComma(.Cells("etent_TotalHoursWorked").Value)

                    newdatrow("Column17") = "" 'FormatNumber(ValNoComma(.Cells("etent_TotalHoursWorked").Value), 2) 'Regular hours worked

                    Dim et_RegHrsAmt = If(IsDBNull(.Cells("etent_RegularHoursAmount").Value), 0, .Cells("etent_RegularHoursAmount").Value)

                    newdatrow("Column18") = "₱ " & FormatNumber(ValNoComma(et_RegHrsAmt), 2) 'Regular hours amount

                    newdatrow("Column19") = "" 'FormatNumber(ValNoComma(.Cells("etent_OvertimeHoursWorked").Value), 2) 'Overtime hours worked
                    newdatrow("Column20") = "₱ " & FormatNumber(ValNoComma(.Cells("etent_OvertimeHoursAmount").Value), 2) 'Overtime hours amount

                    newdatrow("Column21") = "" 'FormatNumber(ValNoComma(.Cells("etent_NightDifferentialHours").Value), 2) 'Night differential hours worked
                    newdatrow("Column22") = "₱ " & FormatNumber(ValNoComma(.Cells("etent_NightDiffHoursAmount").Value), 2) 'Night differential hours amount

                    newdatrow("Column23") = "" 'FormatNumber(ValNoComma(.Cells("etent_NightDifferentialOTHours").Value), 2) 'Night differential OT hours worked
                    newdatrow("Column24") = "₱ " & FormatNumber(ValNoComma(.Cells("etent_NightDiffOTHoursAmount").Value), 2) 'Night differential OT hours amount

                    'txttotholidayhrs.Text = .Cells("esal_BasicPay").Value
                    'txttotholidayamt.Text = .Cells("esal_BasicPay").Value

                    newdatrow("Column25") = "" 'FormatNumber(ValNoComma(.Cells("etent_TotalHoursWorked").Value), 2) 'Total hours worked

                    Dim strtab = "					"

                    'newdatrow("Column26") = "₱ " & FormatNumber(valnocomma(.Cells("etent_UndertimeHours").Value), 2) 'Undertime hours
                    'newdatrow("Column27") = "₱ " & FormatNumber(valnocomma(.Cells("etent_UndertimeHoursAmount").Value), 2) 'Undertime amount

                    '*******************************************

                    Dim str_length = 0

                    'str_length = ("₱ " & FormatNumber(ValNoComma(.Cells("etent_HoursLateAmount").Value), 2)).ToString.Length 'Absent

                    If undeclared_psi.Rows.Count = 0 Then
                        newdatrow("Column26") = "₱ " & FormatNumber(0, 2)
                    Else

                        Dim undeclaredpercent = ValNoComma(EXECQUER("SELECT `GET_employeeundeclaredsalarypercent`('" & drow("RowID") & "'" &
                                                                    ", '" & orgztnID & "'" &
                                                                    ", '" & paypFrom & "'" &
                                                                    ", '" & paypTo & "');"))

                        Dim val_absent = If(undeclared_psi.Rows.Count = 0, 0, ValNoComma(undeclared_psi.Compute("SUM(PayAmount)", "Item = 'Absent'")))
                        val_absent = val_absent - (val_absent * undeclaredpercent)
                        newdatrow("Column26") = "₱ " & FormatNumber(val_absent, 2)

                    End If

                    str_length = ("₱ " & FormatNumber(ValNoComma(.Cells("etent_HoursLateAmount").Value), 2)).ToString.Length 'Tardiness

                    'ValNoComma(.Cells("etent_HoursLate").Value) & Space(21 - str_length) & _
                    newdatrow("Column27") = "₱ " & FormatNumber(ValNoComma(.Cells("etent_HoursLateAmount").Value), 2) 'Tardiness

                    str_length = ("₱ " & FormatNumber(ValNoComma(.Cells("etent_UndertimeHoursAmount").Value), 2)).ToString.Length 'Undertime

                    'ValNoComma(.Cells("etent_UndertimeHours").Value) & Space(21 - str_length) & _
                    newdatrow("Column28") = "₱ " & FormatNumber(ValNoComma(.Cells("etent_UndertimeHoursAmount").Value), 2) 'Undertime

                    '*******************************************

                    txttotabsent.Text = COUNT_employeeabsent(drow("RowID").ToString, _
                                                             drow("StartDate").ToString, _
                                                             paypFrom, _
                                                             paypTo)

                    'Dim param_date = If(paypTo = Nothing, paypFrom, paypTo)

                    'Dim rateper_hour = GET_employeerateperhour(.Cells("RowID").Value, param_date)

                    'newdatrow("Column28") = "₱ " & FormatNumber(valnocomma(.Cells("etent_HoursLate").Value), 2)
                    'newdatrow("Column29") = "₱ " & FormatNumber(valnocomma(.Cells("etent_HoursLateAmount").Value), 2)

                    Dim misc_subtot = ValNoComma(newdatrow("Column29")) + ValNoComma(newdatrow("Column27"))

                    Exit For

                End With

            Next



            VIEW_eallow_indate(drow("RowID"), _
                                paypFrom, _
                                paypTo)

            VIEW_eloan_indate(drow("RowID"), _
                                paypFrom, _
                                paypTo)

            VIEW_ebon_indate(drow("RowID"), _
                                paypFrom, _
                                paypTo)



            For Each dgvrow As DataGridViewRow In dgvempallowance.Rows 'Allowances
                If dgvrow.Index = 0 Then
                    newdatrow("Column34") = dgvrow.Cells("eall_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                    newdatrow("Column37") = "₱ " & FormatNumber(ValNoComma(dgvrow.Cells("eall_Amount").Value), 2)
                Else
                    newdatrow("Column34") &= vbNewLine & dgvrow.Cells("eall_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                    newdatrow("Column37") &= vbNewLine & "₱ " & FormatNumber(ValNoComma(dgvrow.Cells("eall_Amount").Value), 2)

                    'Dim strtxt = dgvrow.Cells("eall_Type").Value & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                End If
            Next

            'objText.Text &= vbNewLine
            'allowvalues.Text &= vbNewLine

            Dim totalamountofloan = ValNoComma(0)

            Dim first_indx = 0

            Dim sel_employeeloanfulldetails = employeeloanfulldetails.Select("EmpRowID = '" & drow("RowID") & "'")

            For Each loanrow As DataRow In sel_employeeloanfulldetails

                If first_indx = 0 Then

                    newdatrow("Column35") = loanrow("PartNo").ToString & " loan"

                    newdatrow("Column38") = "₱ " & FormatNumber(ValNoComma(loanrow("DeductionAmount")), 2)

                    newdatrow("Column33") = "₱ " & FormatNumber(ValNoComma(loanrow("CurrentBalance")), 2)

                    totalamountofloan += ValNoComma(loanrow("DeductionAmount"))

                Else

                    newdatrow("Column35") &= vbNewLine & loanrow("PartNo").ToString & " loan" '& vbTab & " - " & dgvrow.Cells("c_totloanamt").Value _
                    '                                                                     '& vbTab & "/" & dgvrow.Cells("c_totballeft").Value

                    newdatrow("Column38") &= vbNewLine & "₱ " & FormatNumber(ValNoComma(loanrow("DeductionAmount")), 2)

                    newdatrow("Column33") &= vbNewLine & "₱ " & FormatNumber(ValNoComma(loanrow("CurrentBalance")), 2)

                    totalamountofloan += ValNoComma(loanrow("DeductionAmount"))

                End If

                first_indx += 1

            Next

            'For Each dgvrow As DataGridViewRow In dgvLoanList.Rows 'Loans

            '    If dgvrow.Index = 0 Then
            '        newdatrow("Column35") = dgvrow.Cells("c_loantype").Value & " loan" '& vbTab & " - " & dgvrow.Cells("c_totloanamt").Value _
            '        '                                                        '& vbTab & "/" & dgvrow.Cells("c_totballeft").Value

            '        newdatrow("Column38") = "₱ " & FormatNumber(ValNoComma(dgvrow.Cells("c_dedamt").Value), 2)

            '        newdatrow("Column10") = "₱ " & FormatNumber(ValNoComma(dgvrow.Cells("c_totballeft").Value), 2)

            '        totalamountofloan += ValNoComma(dgvrow.Cells("c_dedamt").Value)

            '    Else
            '        newdatrow("Column35") &= vbNewLine & dgvrow.Cells("c_loantype").Value & " loan" '& vbTab & " - " & dgvrow.Cells("c_totloanamt").Value _
            '        '                                                                     '& vbTab & "/" & dgvrow.Cells("c_totballeft").Value

            '        newdatrow("Column38") &= vbNewLine & "₱ " & FormatNumber(ValNoComma(dgvrow.Cells("c_dedamt").Value), 2)

            '        newdatrow("Column10") &= "₱ " & FormatNumber(ValNoComma(dgvrow.Cells("c_totballeft").Value), 2)

            '        totalamountofloan += ValNoComma(dgvrow.Cells("c_dedamt").Value)

            '        'Dim strtxt = dgvrow.Cells("c_loantype").Value & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

            '    End If

            'Next

            newdatrow("Column9") = "₱ " & FormatNumber(totalamountofloan, 2)

            'objText.Text &= vbNewLine
            'loanvalues.Text &= vbNewLine

            ''dgvempbon'bonsubdetails

            For Each dgvrow As DataGridViewRow In dgvempbon.Rows 'Bonuses
                If dgvrow.Index = 0 Then
                    newdatrow("Column36") = dgvrow.Cells("bon_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                    newdatrow("Column39") = "₱ " & FormatNumber(ValNoComma(dgvrow.Cells("bon_Amount").Value), 2)
                Else
                    newdatrow("Column36") &= vbNewLine & dgvrow.Cells("bon_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                    newdatrow("Column39") &= vbNewLine & "₱ " & FormatNumber(ValNoComma(dgvrow.Cells("bon_Amount").Value), 2)

                End If
            Next

            'objText.Text &= vbNewLine
            'bonvalues.Text &= vbNewLine

            rptdattab.Rows.Add(newdatrow)

        Next

        'For Each dgvrow As DataGridViewRow In dgvemployees.Rows
        '    newdatrow = rptdattab.NewRow

        '    dgvrow.Selected = 1
        '    'dgvemployees_SelectionChanged(sender, e)

        '    With dgvrow
        '        newdatrow("Column1") = .Cells("RowID").Value
        '        newdatrow("Column2") = .Cells("EmployeeID").Value

        '        Dim full_name = .Cells("FirstName").Value & If(.Cells("MiddleName").Value = Nothing, _
        '                                                 "", _
        '                                                 " " & StrConv(Microsoft.VisualBasic.Left(.Cells("MiddleName").Value.ToString, 1), _
        '                                                                                                VbStrConv.ProperCase) & ".")
        '        full_name = full_name & " " & .Cells("LastName").Value

        '        full_name = full_name & If(.Cells("Surname").Value = Nothing, _
        '                                                 "", _
        '                                                 "-" & StrConv(Microsoft.VisualBasic.Left(.Cells("Surname").Value.ToString, 1), _
        '                                                                                                VbStrConv.ProperCase))

        '        newdatrow("Column3") = full_name

        '        'txtempbasicpay.Text

        '        'txttotreghrs.Text
        '        'txttotregamt.Text

        '        'txttotothrs.Text
        '        'txttototamt.Text

        '        'txttotnightdiffhrs.Text
        '        'txttotnightdiffamt.Text

        '        'txttotnightdiffothrs.Text
        '        'txttotnightdiffotamt.Text

        '        'txttotholidayhrs.Text
        '        'txttotholidayamt.Text

        '        'txthrswork.Text
        '        'txthrsworkamt.Text

        '        'lblsubtot.Text

        '        'txtemptotallow.Text

        '        'txtgrosssal.Text

        '        'txtvlbal.Text
        '        'txtslbal.Text
        '        'txtmlbal.Text

        '        'txttotabsent.Text
        '        'txttotabsentamt.Text

        '        'txttottardi.Text
        '        'txttottardiamt.Text

        '        'txttotut.Text
        '        'txttotutamt.Text

        '        'lblsubtotmisc.Text

        '        'txtempsss.Text
        '        'txtempphh.Text
        '        'txtemphdmf.Text

        '        'txtemptotloan.Text
        '        'txtemptotbon.Text

        '        'txttaxabsal.Text
        '        'txtempwtax.Text
        '        'txtnetsal.Text

        '    End With

        '    rptdattab.Rows.Add(newdatrow)

        'Next

        'onepagefouremployeepayslip,prntAllPaySlip
        Dim rptdoc As New printallpayslipotherformat

        With rptdoc.ReportDefinition.Sections(2)
            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("OrgName1")

            objText.Text = orgNam

            objText = .ReportObjects("OrgName")

            objText.Text = orgNam

            objText = .ReportObjects("OrgAddress1")

            Dim orgaddress = EXECQUER("SELECT CONCAT(IF(IFNULL(StreetAddress1,'')='','',StreetAddress1)" &
                                    ",IF(IFNULL(StreetAddress2,'')='','',CONCAT(', ',StreetAddress2))" &
                                    ",IF(IFNULL(Barangay,'')='','',CONCAT(', ',Barangay))" &
                                    ",IF(IFNULL(CityTown,'')='','',CONCAT(', ',CityTown))" &
                                    ",IF(IFNULL(Country,'')='','',CONCAT(', ',Country))" &
                                    ",IF(IFNULL(State,'')='','',CONCAT(', ',State)))" &
                                    " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" &
                                    " WHERE o.RowID=" & orgztnID & ";")

            objText.Text = orgaddress

            objText = .ReportObjects("OrgAddress")

            objText.Text = orgaddress

            Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                                    ",',',COALESCE(FaxNumber,'')" &
                                    ",',',COALESCE(EmailAddress,'')" &
                                    ",',',COALESCE(TINNo,''))" &
                                    " FROM organization WHERE RowID=" & orgztnID & ";")

            Dim contactdet = Split(contactdetails, ",")

            objText = .ReportObjects("OrgContact1")

            Dim contactdets As String = String.Empty

            'If Trim(contactdet(0).ToString) = "" Then
            '    contactdets = ""
            'Else
            '    contactdets = "Contact No. " & contactdet(0).ToString
            'End If

            objText.Text = contactdets

            objText = .ReportObjects("OrgContact")

            objText.Text = contactdets

            objText = .ReportObjects("payperiod1")

            Dim papy_str = "Payroll slip for the period of   " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))

            objText.Text = papy_str

            objText = .ReportObjects("payperiod")

            objText.Text = papy_str

        End With

        rptdoc.SetDataSource(rptdattab)

        Dim crvwr As New CrysVwr
        crvwr.CrystalReportViewer1.ReportSource = rptdoc

        Dim papy_string = "Print all pay slip for the period of " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))

        crvwr.Text = papy_string
        crvwr.Refresh()
        crvwr.Show()

        employeeloanfulldetails.Dispose()

        'rptdattab = Nothing
        'rptdattab.Dispose()

        ''AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        'dgvemployees_SelectionChanged(sender, e)

    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Me.Close()

    End Sub

    Dim pstub_TotalEmpSSS = Val(0)

    Dim pstub_TotalCompSSS = Val(0)

    Dim pstub_TotalEmpPhilhealth = Val(0)

    Dim pstub_TotalCompPhilhealth = Val(0)

    Dim pstub_TotalEmpHDMF = Val(0)

    Dim pstub_TotalCompHDMF = Val(0)

    Dim pstub_TotalVacationDaysLeft = Val(0)

    Dim pstub_TotalLoans = Val(0)

    Dim pstub_TotalBonus = Val(0)

    Dim pstub_TotalAllowance = Val(0)

    Dim OTAmount = Val(0)

    Dim NightDiffOTAmount = Val(0)

    Dim NightDiffAmount = Val(0)

    Dim leavebalances As Object = Nothing
    Dim leavebalan() As String

    Dim thirteenthmoval = 0.0

    Dim org_WorkDaysPerYear As Integer = 0

    Public Prior_PayPeriodID As String = String.Empty

    Public Current_PayPeriodID As String = String.Empty

    Public Next_PayPeriodID As String = String.Empty

    Dim EcolaProductID = Nothing


    Public paypSSSContribSched As String = Nothing

    Public paypPhHContribSched As String = Nothing

    Public paypHDMFContribSched As String = Nothing

    Dim pause_process_message = String.Empty

    Private Sub bgworkgenpayroll_DoWorks(sender As Object, e As System.ComponentModel.DoWorkEventArgs) ' Handles bgworkgenpayroll.DoWork

        backgroundworking = 1

        Dim dtGovtDeductSched As New DataTable

        dtGovtDeductSched = retAsDatTbl("SELECT" &
                                        " PhilHealthDeductionSchedule" &
                                        ",SSSDeductionSchedule" &
                                        ",PagIbigDeductionSchedule" &
                                        ",MinWageEmpSSSContrib" &
                                        ",MinWageEmpPhHContrib" &
                                        ",MinWageEmpHDMFContrib" &
                                        " FROM organization" &
                                        " WHERE RowID='" & orgztnID & "';")

        'Dim MinWageEmpSSSContrib = ValNoComma(0)
        'Dim MinWageEmplyrSSSContrib = ValNoComma(0)


        'Dim MinWageEmpPhHContrib = ValNoComma(0)

        'Dim MinWageEmpHDMFContrib = ValNoComma(0)

        'For Each ogrow As DataRow In dtGovtDeductSched.Rows

        '    If IsDBNull(ogrow("PhilHealthDeductionSchedule")) Then
        '        strPHHdeductsched = "End of the month"
        '    Else
        '        strPHHdeductsched = ogrow("PhilHealthDeductionSchedule")
        '    End If

        '    If IsDBNull(ogrow("SSSDeductionSchedule")) Then
        '        strSSSdeductsched = "End of the month"
        '    Else
        '        strSSSdeductsched = ogrow("SSSDeductionSchedule")
        '    End If

        '    If IsDBNull(ogrow("PagIbigDeductionSchedule")) Then
        '        strHDMFdeductsched = "End of the month"
        '    Else
        '        strHDMFdeductsched = ogrow("PagIbigDeductionSchedule")
        '    End If

        '    MinWageEmpSSSContrib = ValNoComma(ogrow("MinWageEmpSSSContrib"))

        '    MinWageEmpPhHContrib = ValNoComma(ogrow("MinWageEmpPhHContrib"))

        '    MinWageEmpHDMFContrib = ValNoComma(ogrow("MinWageEmpHDMFContrib"))

        'Next

        'MinWageEmplyrSSSContrib = EXECQUER("SELECT pss.EmployerContributionAmount" &
        '                                   " FROM paysocialsecurity pss" &
        '                                   " WHERE pss.EmployeeContributionAmount=" & MinWageEmpSSSContrib & ";")

        'MinWageEmplyrSSSContrib = ValNoComma(MinWageEmplyrSSSContrib)

        backgroundworking = 1

        EcolaProductID = EXECQUER("SELECT RowID FROM product WHERE PartNo='Ecola' AND OrganizationID='" & orgztnID & "' AND Category='Allowance Type' LIMIT 1;")

        'Dim date_differ = DateDiff(DateInterval.Day, CDate(paypFrom), CDate(paypTo))

        Dim emptaxabsal As Decimal = 0
        Dim empnetsal As Decimal = 0
        Dim emp_taxabsal = Val(0)

        Dim tax_amount = Val(0)

        Dim grossincome = Val(0)

        Dim emp_count = employee_dattab.Rows.Count

        Dim progress_index As Integer = 1

        Dim date_to_use = If(CDate(paypFrom) > CDate(paypTo), CDate(paypFrom), CDate(paypTo))

        Dim dateStr_to_use = Format(CDate(date_to_use), "yyyy-MM-dd")

        Dim numberofweeksthismonth = _
            EXECQUER("SELECT `COUNTTHEWEEKS`('" & dateStr_to_use & "');")

        Dim sel_employee_dattab = employee_dattab.Select("PositionID IS NULL")

        If sel_employee_dattab.Count > 0 Then

            For Each drow In sel_employee_dattab

                pause_process_message = "Employee '" & drow("EmployeeID") & "' has no position." &
                    vbNewLine & "Please supply his/her position before proceeding to payroll."

                e.Cancel = True

                If bgworkgenpayroll.CancellationPending Then

                    bgworkgenpayroll.CancelAsync()

                End If

            Next

        End If

        If e.Cancel = False Then

            For Each drow As DataRow In employee_dattab.Rows

                strPHHdeductsched = drow("PhHealthDeductSched").ToString
                If drow("PhHealthDeductSched").ToString = "End of the month" Then

                    isorgPHHdeductsched = 0

                ElseIf drow("PhHealthDeductSched").ToString = "Per pay period" Then

                    isorgPHHdeductsched = 1

                ElseIf drow("PhHealthDeductSched").ToString = "First half" Then

                    isorgPHHdeductsched = 2

                End If



                ',,strHDMFdeductsched
                strSSSdeductsched = drow("SSSDeductSched").ToString
                If drow("SSSDeductSched").ToString = "End of the month" Then

                    isorgSSSdeductsched = 0

                ElseIf drow("SSSDeductSched").ToString = "Per pay period" Then

                    isorgSSSdeductsched = 1

                ElseIf drow("SSSDeductSched").ToString = "First half" Then

                    isorgSSSdeductsched = 2

                End If




                strHDMFdeductsched = drow("HDMFDeductSched").ToString
                If drow("HDMFDeductSched").ToString = "End of the month" Then

                    isorgHDMFdeductsched = 0

                ElseIf drow("HDMFDeductSched").ToString = "Per pay period" Then

                    isorgHDMFdeductsched = 1

                ElseIf drow("HDMFDeductSched").ToString = "First half" Then

                    isorgHDMFdeductsched = 2

                End If



                'If drow("WTaxDeductSched").ToString = "End of the month" Then

                '    isorgWTaxdeductsched = 0

                'ElseIf drow("WTaxDeductSched").ToString = "Per pay period" Then

                '    isorgWTaxdeductsched = 1

                'ElseIf drow("WTaxDeductSched").ToString = "First half" Then

                '    isorgWTaxdeductsched = 2

                'End If


                Dim employee_ID = Trim(drow("RowID"))

                org_WorkDaysPerYear = drow("WorkDaysPerYear")

                Dim divisorMonthlys = If(drow("PayFrequencyID") = 1, 2, _
                                         If(drow("PayFrequencyID") = 2, 1, _
                                            If(drow("PayFrequencyID") = 3, EXECQUER("SELECT COUNT(RowID) FROM employeetimeentry WHERE EmployeeID='" & employee_ID & "' AND Date BETWEEN '" & paypFrom & "' AND '" & paypTo & "' AND IFNULL(TotalDayPay,0)!=0 AND OrganizationID='" & orgztnID & "';"), _
                                               numberofweeksthismonth)))

                Dim rowempsal = esal_dattab.Select("EmployeeID = " & drow("RowID").ToString)

                Dim emp_loan = emp_loans.Select("EmployeeID = " & drow("RowID").ToString)

                Dim emp_bon = emp_bonus.Select("EmployeeID = " & drow("RowID").ToString)








                Dim day_allowance = emp_allowanceDaily.Select("EmployeeID = " & drow("RowID").ToString)

                Dim month_allowance = emp_allowanceMonthly.Select("EmployeeID = " & drow("RowID").ToString)

                Dim once_allowance = emp_allowanceOnce.Select("EmployeeID = " & drow("RowID").ToString)

                Dim semim_allowance = emp_allowanceSemiM.Select("EmployeeID = " & drow("RowID").ToString)

                Dim week_allowance = emp_allowanceWeekly.Select("EmployeeID = " & drow("RowID").ToString)

                'emp_allowanceSemiM

                'emp_allowanceWeekly




                Dim daynotax_allowance = notax_allowanceDaily.Select("EmployeeID = " & drow("RowID").ToString)

                Dim monthnotax_allowance = notax_allowanceMonthly.Select("EmployeeID = " & drow("RowID").ToString)

                Dim oncenotax_allowance = notax_allowanceOnce.Select("EmployeeID = " & drow("RowID").ToString)

                Dim semimnotax_allowance = notax_allowanceSemiM.Select("EmployeeID = " & drow("RowID").ToString)

                Dim weeknotax_allowance = notax_allowanceWeekly.Select("EmployeeID = " & drow("RowID").ToString)

                'notax_allowanceSemiM

                'notax_allowanceWeekly




                Dim day_bon = emp_bonusDaily.Select("EmployeeID = " & drow("RowID").ToString)

                Dim month_bon = emp_bonusMonthly.Select("EmployeeID = " & drow("RowID").ToString)

                Dim once_bon = emp_bonusOnce.Select("EmployeeID = " & drow("RowID").ToString)

                Dim semim_bon = emp_bonusSemiM.Select("EmployeeID = " & drow("RowID").ToString)

                Dim week_bon = emp_bonusWeekly.Select("EmployeeID = " & drow("RowID").ToString)

                'emp_bonusSemiM

                'emp_bonusWeekly



                Dim daynotax_bon = notax_bonusDaily.Select("EmployeeID = " & drow("RowID").ToString)

                Dim monthnotax_bon = notax_bonusMonthly.Select("EmployeeID = " & drow("RowID").ToString)

                Dim oncenotax_bon = notax_bonusOnce.Select("EmployeeID = " & drow("RowID").ToString)

                Dim semimnotax_bon = notax_bonusSemiM.Select("EmployeeID = " & drow("RowID").ToString)

                Dim weeknotax_bon = notax_bonusWeekly.Select("EmployeeID = " & drow("RowID").ToString)

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



                Dim valday_allowance = _
                    GET_employeeallowance(drow("RowID").ToString, _
                                      "Daily", _
                                      drow("EmployeeType").ToString, _
                                      "1")

                'valday_allowance = FormatNumber(Val(valday_allowance), 2).Replace(",", "")

                'For Each drowdayallow In day_allowance
                '    valday_allowance = drowdayallow("TotalAllowanceAmount")

                '    If drow("EmployeeType").ToString = "Fixed" Then
                '        valday_allowance = valday_allowance * numofweekdays 'numofweekends
                '    Else
                '        Dim daymultiplier = numofdaypresent.Select("EmployeeID = " & drow("RowID").ToString)

                '        For Each drowdaymultip In daymultiplier

                '            Dim i_val = Val(drowdaymultip("SumHours"))
                '            'DaysAttended

                '            valday_allowance = valday_allowance * i_val

                '            Exit For

                '        Next

                '    End If

                '    Exit For

                'Next

                Dim valmonth_allowance = 0.0

                If isEndOfMonth = 1 Then

                    For Each drowmonallow In month_allowance

                        'Dim daymultiplier = numofdaypresent.Select("EmployeeID = " & drow("RowID").ToString)

                        'For Each drowdaymultip In daymultiplier

                        valmonth_allowance = drowmonallow("TotalAllowanceAmount") ' / divisorMonthlys

                        'Dim i_val = Val(drowdaymultip("SumHours"))
                        'DaysAttended

                        'usual number of working hours = 8

                        'usual number of working days of semi-monthly = 10

                        '80 = 8 * 10

                        'valmonth_allowance = valmonth_allowance / 80

                        'valmonth_allowance = valmonth_allowance * i_val

                        '    Exit For

                        'Next

                        Exit For

                    Next

                End If



                'Dim valsemimon_allowance = _
                '    GET_employeeallowance(drow("RowID").ToString, _
                '                      "Semi-monthly", _
                '                      drow("EmployeeType").ToString, _
                '                      "1")


                Dim valonce_allowance = 0.0
                For Each drowonceallow In once_allowance
                    valonce_allowance = drowonceallow("TotalAllowanceAmount")
                Next

                Dim valsemim_allowance = 0.0
                For Each drowsemimallow In semim_allowance
                    valonce_allowance = drowsemimallow("TotalAllowanceAmount")
                Next

                Dim valweek_allowance = 0.0
                For Each drowweekallow In week_allowance
                    valonce_allowance = drowweekallow("TotalAllowanceAmount")
                Next


                'this is taxable                                ' / divisorMonthlys
                Dim totalemployeeallownce = (valday_allowance _
                                             + (valmonth_allowance) _
                                             + valonce_allowance _
                                             + valsemim_allowance _
                                             + valweek_allowance)



                Dim valdaynotax_allowance = _
                    GET_employeeallowance(drow("RowID").ToString, _
                                      "Daily", _
                                      drow("EmployeeType").ToString, _
                                      "0")

                'For Each drowdayallow In daynotax_allowance
                '    valdaynotax_allowance = drowdayallow("TotalAllowanceAmount")

                '    If drow("EmployeeType").ToString = "Fixed" Then
                '        valdaynotax_allowance = valdaynotax_allowance * numofweekdays 'numofweekends
                '    Else
                '        Dim daymultiplier = numofdaypresent.Select("EmployeeID = " & drow("RowID").ToString)
                '        For Each drowdaymultip In daymultiplier
                '            Dim i_val = Val(drowdaymultip("SumHours"))
                '            'DaysAttended
                '            valdaynotax_allowance = valdaynotax_allowance * i_val
                '            Exit For
                '        Next

                '    End If

                '    Exit For
                'Next

                Dim valmonthnotax_allowance = 0.0

                If isEndOfMonth = 1 Then

                    For Each drowmonallow In monthnotax_allowance

                        'Dim daymultiplier = numofdaypresent.Select("EmployeeID = " & drow("RowID").ToString)

                        'For Each drowdaymultip In daymultiplier

                        valmonthnotax_allowance = drowmonallow("TotalAllowanceAmount") ' / divisorMonthlys

                        '    Dim i_val = Val(drowdaymultip("SumHours"))
                        '    'DaysAttended

                        '    'usual number of working hours = 8

                        '    'usual number of working days of semi-monthly = 10

                        '    '80 = 8 * 10

                        '    valmonthnotax_allowance = valmonthnotax_allowance / 80

                        '    valmonthnotax_allowance = valmonthnotax_allowance * i_val

                        '    Exit For

                        'Next

                        Exit For

                    Next

                End If

                'Dim valsemimonnotax_allowance = _
                '    GET_employeeallowance(drow("RowID").ToString, _
                '                      "Semi-monthly", _
                '                      drow("EmployeeType").ToString, _
                '                      "0")

                Dim valoncenotax_allowance = 0.0
                For Each drowonceallow In oncenotax_allowance
                    valoncenotax_allowance = drowonceallow("TotalAllowanceAmount")
                Next

                Dim valsemimnotax_allowance = 0.0
                For Each drowsemimallow In semimnotax_allowance
                    valoncenotax_allowance = drowsemimallow("TotalAllowanceAmount")
                Next

                Dim valweeknotax_allowance = 0.0
                For Each drowweekallow In weeknotax_allowance
                    valoncenotax_allowance = drowweekallow("TotalAllowanceAmount")
                Next





                'this is non-taxable                                        ' / divisorMonthlys
                '+ valsemimonnotax_allowance _
                Dim totalnotaxemployeeallownce = (valdaynotax_allowance _
                                                  + (valmonthnotax_allowance) _
                                                  + valoncenotax_allowance _
                                                  + valsemimnotax_allowance _
                                                  + valweeknotax_allowance)





                Dim valday_bon = 0.0
                For Each drowdaybon In day_bon
                    valday_bon = drowdaybon("BonusAmount")

                    If drow("EmployeeType").ToString = "Fixed" Then
                        valday_bon = valday_bon * numofweekdays 'numofweekends
                    Else
                        Dim daymultiplier = numofdaypresent.Select("EmployeeID = " & drow("RowID").ToString)
                        For Each drowdaymultip In daymultiplier
                            Dim i_val = Val(drowdaymultip("DaysAttended"))
                            valday_bon = valday_bon * i_val
                            Exit For
                        Next

                    End If

                    Exit For
                Next

                Dim valmonth_bon = 0.0

                If isEndOfMonth = 1 Then

                    For Each drowmonbon In month_bon
                        valmonth_bon = drowmonbon("BonusAmount")
                    Next

                End If

                Dim valonce_bon = 0.0
                For Each drowoncebon In once_bon
                    valonce_bon = drowoncebon("BonusAmount")
                Next

                Dim valsemim_bon = 0.0
                For Each drowsemimbon In semim_bon
                    valonce_bon = drowsemimbon("BonusAmount")
                Next

                Dim valweek_bon = 0.0
                For Each drowweekbon In week_bon
                    valonce_bon = drowweekbon("BonusAmount")
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
                        valdaynotax_bon = valdaynotax_bon * numofweekdays 'numofweekends
                    Else
                        Dim daymultiplier = numofdaypresent.Select("EmployeeID = " & drow("RowID").ToString)
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
                        valmonthnotax_bon = drowmonbon("BonusAmount")
                    Next

                End If

                Dim valoncenotax_bon = 0.0
                For Each drowoncebon In oncenotax_bon
                    valoncenotax_bon = drowoncebon("BonusAmount")
                Next

                Dim valsemimnotax_bon = 0.0
                For Each drowsemimbon In semimnotax_bon
                    valoncenotax_bon = drowsemimbon("BonusAmount")
                Next

                Dim valweeknotax_bon = 0.0
                For Each drowweekbon In weeknotax_bon
                    valoncenotax_bon = drowweekbon("BonusAmount")
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







                Dim emptotdaypay = etent_totdaypay.Select("EmployeeID = " & drow("RowID").ToString)

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

                Dim pstub_TotalAllowance = Val(0)

                'If emptotdaypay.Count = 0 Then
                '    If drow("EmployeeType").ToString = "Fixed" Then

                '    End If
                'End If

                'For Each drowsal In rowempsal
                '    If drow("EmployeeType").ToString = "Fixed" Then
                '        grossincome = CDec(drowsal("Salary"))
                '    End If
                '    Exit For
                'Next
                OTAmount = 0
                NightDiffOTAmount = 0
                NightDiffAmount = 0

                '==================================================Ito ay para kay Fixed-employee, kahit wala siyang record sa employeetimeentry meron pa rin siyang sasahurin

                If drow("EmployeeType").ToString = "Fixed" And drow("EmploymentStatus").ToString = "Regular" Then

                    For Each drowsal In rowempsal
                        emptaxabsal = 0
                        empnetsal = 0
                        emp_taxabsal = 0
                        'paywithholdingtax
                        'PayFrequencyID'FilingStatusID'EffectiveDateFrom'EffectiveDateTo'ExemptionAmount'ExemptionInExcessAmount'TaxableIncomeFromAmount
                        'orgpayfreqID
                        OTAmount = 0.0
                        NightDiffOTAmount = 0.0
                        NightDiffAmount = 0

                        grossincome = CDec(drowsal("BasicPay"))

                        grossincome += totalemployeeallownce

                        If isorgPHHdeductsched = 1 Then 'Per pay period
                            pstub_TotalEmpPhilhealth = (CDec(drowsal("EmployeeShare")) / 2)
                            pstub_TotalCompPhilhealth = (CDec(drowsal("EmployerShare")) / 2)

                            emptaxabsal = grossincome - (pstub_TotalEmpPhilhealth) '- (CDec(drowsal("EmployeeContributionAmount")) / 2))

                        ElseIf isorgPHHdeductsched = 2 Then 'First half

                            pstub_TotalEmpPhilhealth = CDec(drowsal("EmployeeShare"))
                            pstub_TotalCompPhilhealth = CDec(drowsal("EmployerShare"))

                            If isEndOfMonth = 1 Or isEndOfMonth = 0 Then

                                emptaxabsal = grossincome

                                If drow("PayFrequencyID") <> 1 Then

                                    pstub_TotalEmpPhilhealth = 0

                                    pstub_TotalCompPhilhealth = 0

                                End If

                            Else

                                emptaxabsal = grossincome - pstub_TotalEmpPhilhealth '(CDec(drowsal("EmployeeContributionAmount")))

                            End If

                        Else '                          'End of the month
                            pstub_TotalEmpPhilhealth = CDec(drowsal("EmployeeShare"))
                            pstub_TotalCompPhilhealth = CDec(drowsal("EmployerShare"))

                            If isEndOfMonth = 1 Then
                                emptaxabsal = grossincome - pstub_TotalEmpPhilhealth '(CDec(drowsal("EmployeeContributionAmount")))

                            Else

                                emptaxabsal = grossincome

                                pstub_TotalEmpPhilhealth = 0

                                pstub_TotalCompPhilhealth = 0

                            End If

                        End If

                        Dim str_TotalEmpPhilhealth As String = pstub_TotalEmpPhilhealth & " " & pstub_TotalCompPhilhealth

                        If isorgSSSdeductsched = 1 Then 'Per pay period
                            pstub_TotalEmpSSS = (CDec(drowsal("EmployeeContributionAmount")) / 2)
                            pstub_TotalCompSSS = (CDec(drowsal("EmployerContributionAmount")) / 2)

                            emptaxabsal -= pstub_TotalEmpSSS

                        ElseIf isorgSSSdeductsched = 2 Then 'First half

                            pstub_TotalEmpSSS = CDec(drowsal("EmployeeContributionAmount"))
                            pstub_TotalCompSSS = CDec(drowsal("EmployerContributionAmount"))

                            If isEndOfMonth = 1 Or isEndOfMonth = 0 Then

                                emptaxabsal = emptaxabsal

                                If drow("PayFrequencyID") <> 1 Then

                                    pstub_TotalEmpSSS = 0

                                    pstub_TotalCompSSS = 0

                                End If

                            Else
                                emptaxabsal = emptaxabsal - pstub_TotalEmpSSS

                            End If

                        Else
                            pstub_TotalEmpSSS = CDec(drowsal("EmployeeContributionAmount"))
                            pstub_TotalCompSSS = CDec(drowsal("EmployerContributionAmount"))

                            If isEndOfMonth = 1 Then
                                emptaxabsal = emptaxabsal - pstub_TotalEmpSSS
                            Else
                                emptaxabsal = emptaxabsal

                                pstub_TotalEmpSSS = 0

                                pstub_TotalCompSSS = 0

                            End If

                        End If

                        Dim str_TotalEmpSSS As String = pstub_TotalEmpSSS & " " & pstub_TotalCompSSS

                        If isorgHDMFdeductsched = 1 Then 'Per pay period
                            pstub_TotalEmpHDMF = (CDec(drowsal("HDMFAmount")) / 2)
                            pstub_TotalCompHDMF = (100 / 2) 'CDec(drowsal("HDMFAmount"))

                            emptaxabsal -= pstub_TotalEmpHDMF

                        ElseIf isorgHDMFdeductsched = 2 Then 'First half

                            pstub_TotalEmpHDMF = CDec(drowsal("HDMFAmount"))
                            pstub_TotalCompHDMF = 100 'CDec(drowsal("HDMFAmount"))

                            If isEndOfMonth = 1 Or isEndOfMonth = 0 Then

                                emptaxabsal = emptaxabsal

                                If drow("PayFrequencyID") <> 1 Then

                                    pstub_TotalEmpHDMF = 0

                                    pstub_TotalCompHDMF = 0

                                End If

                            Else
                                emptaxabsal = emptaxabsal - pstub_TotalEmpHDMF

                            End If

                        Else
                            'If drow("PayFrequencyID").ToString = 1 Then
                            '    pstub_TotalEmpHDMF = (CDec(drowsal("HDMFAmount")) / 2)
                            '    pstub_TotalCompHDMF = (CDec(drowsal("HDMFAmount")) / 2)
                            'Else
                            'End If

                            pstub_TotalEmpHDMF = CDec(drowsal("HDMFAmount"))
                            pstub_TotalCompHDMF = 100 'CDec(drowsal("HDMFAmount"))

                            If isEndOfMonth = 1 Then
                                emptaxabsal = emptaxabsal - pstub_TotalEmpHDMF
                            Else
                                emptaxabsal = emptaxabsal

                                pstub_TotalEmpHDMF = 0

                                pstub_TotalCompHDMF = 0

                            End If

                        End If

                        Dim str_TotalEmpHDMF As String = pstub_TotalEmpHDMF & " " & pstub_TotalCompHDMF

                        'If drow("UndertimeOverride").ToString = 1 Then
                        '    emptaxabsal = (emptaxabsal) - (Val(drowtotdaypay("UndertimeHoursAmount"))) 'Val(emptotdaypay("HoursLateAmount"))
                        '    'Else
                        '    '    emptaxabsal = emptaxabsal + totalemployeeallownce
                        'End If

                        'If drow("OvertimeOverride").ToString = 1 Then
                        '    If drow("EmployeeType").ToString = "Fixed" Then
                        '        emptaxabsal = emptaxabsal + (Val(drowtotdaypay("OvertimeHoursAmount")) + Val(drowtotdaypay("NightDiffOTHoursAmount"))) 'Val(emptotdaypay("HoursLateAmount"))
                        '        'ElseIf drow("EmployeeType").ToString = "Daily" Then
                        '        '    grossincome = CDec(drowtotdaypay("TotalDayPay"))
                        '        'ElseIf drow("EmployeeType").ToString = "Hourly" Then
                        '        '    grossincome = CDec(drowtotdaypay("TotalDayPay"))
                        '    End If
                        '    emptaxabsal = emptaxabsal + (Val(drowtotdaypay("OvertimeHoursAmount")) + Val(drowtotdaypay("NightDiffOTHoursAmount"))) 'Val(emptotdaypay("HoursLateAmount"))
                        'End If

                        '17550.00 - (2403.85 + ((17550.00 - 10577.00) * 0.32)) = 12914.79
                        '14471.85 - (2083.33 + ((14471.85 - 13542) * 0.3))
                        'ExemptionAmount'ExemptionInExcessAmount'TaxableIncomeFromAmount
                        'emp_taxabsal = emptaxabsal - Val(drowtax("ExemptionInExcessAmount"))
                        'emp_taxabsal = 1
                        'emp_taxabsal = 1

                        Dim taxab_salval = Trim(emptaxabsal) 'drowsal("Salary").ToString

                        '" AND CURRENT_DATE() BETWEEN COALESCE(EffectiveDateFrom,CURRENT_DATE()) AND COALESCE(EffectiveDateTo,COALESCE(ADDDATE(EffectiveDateFrom, INTERVAL 1 DAY),ADDDATE(CURRENT_DATE(), INTERVAL 1 DAY)))" &

                        'payphilhealth = EmployeeShare
                        'paysocialsecurity = EmployeeContributionAmount

                        'isEndOfMonth
                        'isorgPHHdeductsched'isorgSSSdeductsched'isorgHDMFdeductsched

                        tax_amount = 0

                        If isEndOfMonth = 1 Then

                            Dim paywithholdingtax = retAsDatTbl("SELECT ExemptionAmount,TaxableIncomeFromAmount,ExemptionInExcessAmount" &
                                                                " FROM paywithholdingtax" &
                                                                " WHERE FilingStatusID=(SELECT RowID FROM filingstatus WHERE MaritalStatus='" & drow("MaritalStatus").ToString & "' AND Dependent=" & drow("NoOfDependents").ToString & ")" &
                                                                " AND " & taxab_salval & " BETWEEN TaxableIncomeFromAmount AND TaxableIncomeToAmount" &
                                                                " AND DATEDIFF(CURRENT_DATE(),COALESCE(EffectiveDateTo,COALESCE(EffectiveDateFrom,CURRENT_DATE()))) >= 0" &
                                                                " AND PayFrequencyID='" & drow("PayFrequencyID").ToString & "'" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),COALESCE(EffectiveDateTo,COALESCE(EffectiveDateFrom,CURRENT_DATE())))" &
                                                                " LIMIT 1;")

                            Dim GET_employeetaxableincome = EXECQUER("SELECT `GET_employeetaxableincome`('" & drow("RowID") & "', '" & orgztnID & "', '" & paypFrom & "','" & taxab_salval & "');")

                            For Each drowtax As DataRow In paywithholdingtax.rows

                                emp_taxabsal = emptaxabsal - (Val(drowtax("ExemptionAmount")) + _
                                             ((emptaxabsal - Val(drowtax("TaxableIncomeFromAmount"))) * Val(drowtax("ExemptionInExcessAmount"))) _
                                                             )

                                emp_taxabsal = taxab_salval

                                'tax_amount = (Val(drowtax("ExemptionAmount")) + _
                                '             ((emptaxabsal - Val(drowtax("TaxableIncomeFromAmount"))) * Val(drowtax("ExemptionInExcessAmount"))) _
                                '             )

                                Dim the_values = Split(GET_employeetaxableincome, ";")

                                tax_amount = the_values(1)

                                'If drow("EmployeeType").ToString = "Fixed" Then
                                '    grossincome = CDec(drowsal("Salary"))
                                'ElseIf drow("EmployeeType").ToString = "Daily" Then
                                '    grossincome = CDec(drowtotdaypay("TotalDayPay"))
                                'ElseIf drow("EmployeeType").ToString = "Hourly" Then
                                '    grossincome = CDec(drowtotdaypay("TotalDayPay"))
                                'End If

                                Exit For

                            Next

                        End If

                        Exit For
                    Next

                End If

                '==================================================Ito ay para kay Fixed-employee, kahit wala siyang record sa employeetimeentry meron pa rin siyang sasahurin





                For Each drowtotdaypay In emptotdaypay 'drowtotdaypay("TotalDayPay").ToString

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
                    OTAmount = 0
                    NightDiffOTAmount = 0
                    NightDiffAmount = 0

                    For Each drowsal In rowempsal
                        emptaxabsal = 0
                        empnetsal = 0
                        emp_taxabsal = 0
                        'paywithholdingtax
                        'PayFrequencyID'FilingStatusID'EffectiveDateFrom'EffectiveDateTo'ExemptionAmount'ExemptionInExcessAmount'TaxableIncomeFromAmount
                        'orgpayfreqID
                        OTAmount = ValNoComma(etent_totdaypay.Compute("SUM(OvertimeHoursAmount)", "EmployeeID = " & drow("RowID").ToString))
                        'drowtotdaypay("OvertimeHoursAmount")
                        NightDiffOTAmount = drowtotdaypay("NightDiffOTHoursAmount")

                        NightDiffAmount = drowtotdaypay("NightDiffHoursAmount")

                        Dim fullmonthSalaryTaxableSalary = ValNoComma(0)

                        If drow("EmployeeType").ToString = "Fixed" Then
                            grossincome = CDec(drowsal("BasicPay"))
                            grossincome = grossincome + (Val(drowtotdaypay("OvertimeHoursAmount")) + Val(drowtotdaypay("NightDiffOTHoursAmount"))) 'Val(emptotdaypay("HoursLateAmount"))

                            'ElseIf drow("EmployeeType").ToString = "Monthly" Then
                            '    grossincome = CDec(drowtotdaypay("TotalDayPay"))

                            '    grossincome = grossincome + (Val(drowtotdaypay("OvertimeHoursAmount")) + Val(drowtotdaypay("NightDiffOTHoursAmount"))) 'Val(emptotdaypay("HoursLateAmount"))

                            'ElseIf drow("EmployeeType").ToString = "Hourly" Then
                            '    grossincome = CDec(drowtotdaypay("TotalDayPay"))

                            'ElseIf drow("EmployeeType").ToString = "Weekly" Then
                            '    grossincome = CDec(drowtotdaypay("TotalDayPay"))

                        ElseIf drow("EmployeeType").ToString = "Daily" _
                            Or drow("EmployeeType").ToString = "Monthly" _
                            Or drow("EmployeeType").ToString = "Hourly" _
                            Or drow("EmployeeType").ToString = "Weekly" Then

                            Dim employee_keyRowID = drow("RowID").ToString

                            Dim stremployee_type = String.Empty

                            stremployee_type = drow("EmployeeType").ToString

                            grossincome = CDec(drowtotdaypay("TotalDayPay"))

                            'CurrentPayPeriodID'PriorPayPeriodID'NextPayPeriodID

                            'strSSSdeductsched'strPHHdeductsched'strHDMFdeductsched

                            If drow("EmployeeType").ToString = "Daily" _
                                And drow("PayFrequencyID").ToString = "4" Then 'Daily employee type & Weekly pay frequency

                                'isEndOfMonth = 0

                            End If

                            If (isEndOfMonth = 1 Or isEndOfMonth = 0) And _
                                drow("EmploymentStatus").ToString = "Regular" Then

                            End If

                            Dim prev_payperiodID = Prior_PayPeriodID

                            Dim fullSemiMonthSalary = Val(0) 'ValNoComma(drowsal("BasicPay")) * (org_WorkDaysPerYear / 12)

                            'Prior_PayPeriodID'Current_PayPeriodID'Next_PayPeriodID


                            'Dim prev_GrossIncome = EXECQUER("SELECT IFNULL(TotalGrossSalary,0)" &
                            '                                  " FROM paystub" &
                            '                                  " WHERE OrganizationID='" & orgztnID & "'" &
                            '                                  " AND EmployeeID='" & employee_ID & "'" &
                            '                                  " AND PayPeriodID='" & prev_payperiodID & "';")


                            '"SELECT IFNULL(psi.PayAmount,0)" &
                            '" FROM paystubitem psi" &
                            '" INNER JOIN paystub ps ON ps.RowID=psi.PayStubID" &
                            '" AND ps.OrganizationID='" & orgztnID & "'" &
                            '" AND ps.PayPeriodID='" & prev_payperiodID & "'" &
                            '" AND ps.EmployeeID='" & employee_ID & "'" &
                            '" INNER JOIN product p ON p.RowID=psi.ProductID" &
                            '" WHERE p.PartNo='Taxable Income';"

                            'fullSemiMonthSalary + OT Pay + Holiday Pay + Rest Day Pay + Nigth Diff



                            'Dim getEcolaAmountOfThisPayPeriod = Nothing

                            'getEcolaAmountOfThisPayPeriod = ValNoComma(0)

                            'If dtempalldistrib IsNot Nothing Then 'get Ecola allowance amount of this pay period

                            '    If dtempalldistrib.Rows.Count <> 0 Then

                            '        Dim GroupedSum = From userentry In dtempalldistrib
                            '                         Group userentry By key = userentry.Field(Of String)("ProductID") Into Group _
                            '                         Select ProductID = key, SumVal = Group.Sum(Function(p As Object) p("Value"))

                            '        For Each strr In GroupedSum.ToList

                            '            'EcolaProductID

                            '            If strr.ProductID = EcolaProductID Then

                            '                getEcolaAmountOfThisPayPeriod = ValNoComma(strr.SumVal)
                            '                'INSUPD_paystubitem(, paystubID, strr.ProductID, strr.SumVal)

                            '                Exit For

                            '            Else
                            '                Continue For
                            '            End If

                            '        Next

                            '    End If

                            'End If '                                get Ecola allowance amount of this pay period




                            'Dim fullmonthSalary = fullSemiMonthSalary _
                            '                      + ValNoComma(drowtotdaypay("OvertimeHoursAmount")) _
                            '                      + ValNoComma(drowtotdaypay("NightDiffHoursAmount")) _
                            '                      + ValNoComma(drowtotdaypay("NightDiffOTHoursAmount"))

                            '

                            'SSS

                            Dim fullmonthSalary = grossincome _
                                                  - (ValNoComma(drowtotdaypay("OvertimeHoursAmount")) _
                                                  + ValNoComma(drowtotdaypay("NightDiffOTHoursAmount")))

                            Dim sel_prev_empTimeEntry = prev_empTimeEntry.Select("EmployeeID = '" & employee_ID & "'")

                            If strSSSdeductsched = "End of the month" Then

                                If sel_prev_empTimeEntry.Count <> 0 Then 'PREVIOUS TIME ENTRY

                                    fullmonthSalary = fullmonthSalary _
                                        + ValNoComma(prev_empTimeEntry.Compute("SUM(TotalDayPay)", "EmployeeID = '" & employee_ID & "'")) _
                                        - (ValNoComma(prev_empTimeEntry.Compute("SUM(OvertimeHoursAmount)", "EmployeeID = '" & employee_ID & "'")) _
                                        + ValNoComma(prev_empTimeEntry.Compute("SUM(NightDiffOTHoursAmount)", "EmployeeID = '" & employee_ID & "'")))

                                End If

                            ElseIf strSSSdeductsched = "Per pay period" Then

                            End If





                            Dim employeeContribution As New DataTable


                            'SSS

                            employeeContribution = retAsDatTbl("SELECT" &
                                                               " SUM(EmployeeShare) AS EmployeeShare" &
                                                               ",SUM(EmployerShare) AS EmployerShare" &
                                                               " FROM payphilhealth" &
                                                               " WHERE " & fullmonthSalary & " BETWEEN SalaryRangeFrom AND SalaryRangeTo" &
                                                               " UNION" &
                                                               " SELECT SUM(EmployeeContributionAmount) AS EmployeeShare" &
                                                               ",SUM(EmployerContributionAmount + EmployeeECAmount) AS EmployerShare" &
                                                               " FROM paysocialsecurity" &
                                                               " WHERE " & fullmonthSalary & " BETWEEN RangeFromAmount AND RangeToAmount;")

                            If drow("EmployeeType").ToString = "Daily" _
                            And drow("PayFrequencyID").ToString = "4" Then

                                If paypSSSContribSched = "1" Then
                                    If employeeContribution.Rows.Count <> 0 _
                                        And drow("EmploymentStatus").ToString = "Regular" Then

                                        'SSS
                                        'MinWageEmpSSSContrib'MinWageEmpPhHContrib'MinWageEmpHDMFContrib

                                        'pstub_TotalEmpSSS = MinWageEmpSSSContrib
                                        If employeeContribution.Rows.Count = 2 Then
                                            pstub_TotalEmpSSS = ValNoComma(employeeContribution.Rows(1)("EmployeeShare"))
                                        Else
                                            pstub_TotalEmpSSS = ValNoComma(0)
                                        End If

                                        Try
                                            pstub_TotalCompSSS = ValNoComma(employeeContribution.Rows(1)("EmployerShare"))
                                        Catch ex As Exception
                                            pstub_TotalCompSSS = 0 'MinWageEmplyrSSSContrib
                                        End Try

                                    End If

                                End If

                            Else
                                If (isEndOfMonth = 1 And strSSSdeductsched = "End of the month") _
                                Or (isEndOfMonth = 0 And strSSSdeductsched = "Per pay period") Then

                                    If employeeContribution.Rows.Count <> 0 _
                                        And drow("EmploymentStatus").ToString = "Regular" Then

                                        'SSS

                                        pstub_TotalEmpSSS = ValNoComma(employeeContribution.Rows(1)("EmployeeShare"))

                                        pstub_TotalCompSSS = ValNoComma(employeeContribution.Rows(1)("EmployerShare"))

                                    End If

                                End If

                            End If





                            'PHILHEALTH

                            fullmonthSalary = grossincome _
                                - (ValNoComma(drowtotdaypay("OvertimeHoursAmount")) _
                                   + ValNoComma(drowtotdaypay("NightDiffOTHoursAmount")))

                            If strPHHdeductsched = "End of the month" Then

                                If sel_prev_empTimeEntry.Count <> 0 Then 'PREVIOUS TIME ENTRY

                                    fullmonthSalary = fullmonthSalary _
                                        + ValNoComma(prev_empTimeEntry.Compute("SUM(TotalDayPay)", "EmployeeID = '" & employee_ID & "'")) _
                                        - (ValNoComma(prev_empTimeEntry.Compute("SUM(OvertimeHoursAmount)", "EmployeeID = '" & employee_ID & "'")) _
                                        + ValNoComma(prev_empTimeEntry.Compute("SUM(NightDiffOTHoursAmount)", "EmployeeID = '" & employee_ID & "'")))

                                End If

                            ElseIf strPHHdeductsched = "Per pay period" Then

                            End If


                            'PHILHEALTH

                            employeeContribution = retAsDatTbl("SELECT" &
                                                               " SUM(EmployeeShare) AS EmployeeShare" &
                                                               ",SUM(EmployerShare) AS EmployerShare" &
                                                               " FROM payphilhealth" &
                                                               " WHERE " & fullmonthSalary & " BETWEEN SalaryRangeFrom AND SalaryRangeTo" &
                                                               " UNION" &
                                                               " SELECT SUM(EmployeeContributionAmount) AS EmployeeShare" &
                                                               ",SUM(EmployerContributionAmount + EmployeeECAmount) AS EmployerShare" &
                                                               " FROM paysocialsecurity" &
                                                               " WHERE " & fullmonthSalary & " BETWEEN RangeFromAmount AND RangeToAmount;")

                            If drow("EmployeeType").ToString = "Daily" _
                            And drow("PayFrequencyID").ToString = "4" Then

                                If paypPhHContribSched = "1" Then
                                    If employeeContribution.Rows.Count <> 0 _
                                        And drow("EmploymentStatus").ToString = "Regular" Then

                                        'PHILHEALTH
                                        'MinWageEmpSSSContrib'MinWageEmpPhHContrib'MinWageEmpHDMFContrib

                                        pstub_TotalEmpPhilhealth = ValNoComma(employeeContribution.Rows(0)("EmployeeShare")) 'MinWageEmpPhHContrib

                                        pstub_TotalCompPhilhealth = ValNoComma(employeeContribution.Rows(0)("EmployerShare"))

                                    End If
                                End If
                                'paypHDMFContribSched

                            Else

                                If (isEndOfMonth = 1 And strSSSdeductsched = "End of the month") _
                                    Or (isEndOfMonth = 0 And strSSSdeductsched = "Per pay period") Then

                                    If employeeContribution.Rows.Count <> 0 _
                                        And drow("EmploymentStatus").ToString = "Regular" Then

                                        'PHILHEALTH

                                        pstub_TotalEmpPhilhealth = ValNoComma(employeeContribution.Rows(0)("EmployeeShare"))

                                        pstub_TotalCompPhilhealth = ValNoComma(employeeContribution.Rows(0)("EmployerShare"))

                                    End If

                                End If

                            End If

                            fullmonthSalaryTaxableSalary = fullmonthSalary

                            'ElseIf isEndOfMonth = 2 And _
                            '    strSSSdeductsched = "First half" And _
                            '    strPHHdeductsched = "First half" And _
                            '    drow("EmploymentStatus").ToString = "Regular" Then 'strSSSdeductsched'strPHHdeductsched'strHDMFdeductsched

                        End If



                        'CurrentPayPeriodID'PriorPayPeriodID'NextPayPeriodID

                        grossincome += totalemployeeallownce

                        If drow("EmployeeType").ToString = "Daily" _
                        And drow("PayFrequencyID").ToString = "4" Then

                            emptaxabsal = grossincome - (pstub_TotalEmpPhilhealth) '- (CDec(drowsal("EmployeeContributionAmount")) / 2))

                            fullmonthSalaryTaxableSalary -= pstub_TotalEmpPhilhealth

                        Else

                            If isorgPHHdeductsched = 1 Then 'Per pay period

                                'If drow("EmploymentStatus").ToString = "Regular" Then
                                '    pstub_TotalEmpPhilhealth = (CDec(drowsal("EmployeeShare")) / 2)
                                '    pstub_TotalCompPhilhealth = (CDec(drowsal("EmployerShare")) / 2)
                                'End If

                                emptaxabsal = grossincome - (pstub_TotalEmpPhilhealth) '- (CDec(drowsal("EmployeeContributionAmount")) / 2))

                                fullmonthSalaryTaxableSalary -= pstub_TotalEmpPhilhealth

                            ElseIf isorgPHHdeductsched = 2 Then 'First half

                                'If drow("EmploymentStatus").ToString = "Regular" Then
                                '    pstub_TotalEmpPhilhealth = CDec(drowsal("EmployeeShare"))
                                '    pstub_TotalCompPhilhealth = CDec(drowsal("EmployerShare"))
                                'End If

                                If isEndOfMonth = 1 Or isEndOfMonth = 0 Then

                                    emptaxabsal = grossincome

                                    If drow("PayFrequencyID") <> 1 Then

                                        pstub_TotalEmpPhilhealth = 0

                                        pstub_TotalCompPhilhealth = 0

                                    Else
                                        emptaxabsal = emptaxabsal - pstub_TotalEmpPhilhealth

                                        fullmonthSalaryTaxableSalary -= pstub_TotalEmpPhilhealth

                                    End If

                                Else

                                    emptaxabsal = grossincome - pstub_TotalEmpPhilhealth '(CDec(drowsal("EmployeeContributionAmount")))

                                    fullmonthSalaryTaxableSalary -= pstub_TotalEmpPhilhealth

                                End If

                            Else '                          'End of the month
                                'If drow("EmploymentStatus").ToString = "Probationary" Then
                                '    pstub_TotalEmpPhilhealth = CDec(drowsal("EmployeeShare"))
                                '    pstub_TotalCompPhilhealth = CDec(drowsal("EmployerShare"))
                                'End If

                                If isEndOfMonth = 1 Then
                                    emptaxabsal = grossincome - pstub_TotalEmpPhilhealth '(CDec(drowsal("EmployeeContributionAmount")))

                                    fullmonthSalaryTaxableSalary -= pstub_TotalEmpPhilhealth

                                Else

                                    emptaxabsal = grossincome

                                    pstub_TotalEmpPhilhealth = 0

                                    pstub_TotalCompPhilhealth = 0

                                End If

                            End If

                        End If

                        Dim str_TotalEmpPhilhealth As String = pstub_TotalEmpPhilhealth & " " & pstub_TotalCompPhilhealth

                        If drow("EmployeeType").ToString = "Daily" _
                        And drow("PayFrequencyID").ToString = "4" Then

                            emptaxabsal -= pstub_TotalEmpSSS

                            fullmonthSalaryTaxableSalary -= pstub_TotalEmpSSS

                        Else

                            If isorgSSSdeductsched = 1 Then 'Per pay period
                                'If drow("EmploymentStatus").ToString = "Regular" Then
                                '    pstub_TotalEmpSSS = (CDec(drowsal("EmployeeContributionAmount")) / 2)
                                '    pstub_TotalCompSSS = (CDec(drowsal("EmployerContributionAmount")) / 2)
                                'End If

                                emptaxabsal -= pstub_TotalEmpSSS

                                fullmonthSalaryTaxableSalary -= pstub_TotalEmpSSS

                            ElseIf isorgSSSdeductsched = 2 Then 'First half

                                'If drow("EmploymentStatus").ToString = "Regular" Then
                                '    pstub_TotalEmpSSS = CDec(drowsal("EmployeeContributionAmount"))
                                '    pstub_TotalCompSSS = CDec(drowsal("EmployerContributionAmount"))
                                'End If

                                If isEndOfMonth = 1 Or isEndOfMonth = 0 Then

                                    emptaxabsal = emptaxabsal

                                    If drow("PayFrequencyID") <> 1 Then

                                        pstub_TotalEmpSSS = 0

                                        pstub_TotalCompSSS = 0

                                    Else
                                        emptaxabsal = emptaxabsal - pstub_TotalEmpSSS

                                        fullmonthSalaryTaxableSalary -= pstub_TotalEmpSSS

                                    End If

                                Else
                                    emptaxabsal = emptaxabsal - pstub_TotalEmpSSS

                                    fullmonthSalaryTaxableSalary -= pstub_TotalEmpSSS

                                End If

                            Else
                                'If drow("EmploymentStatus").ToString = "Regular" Then
                                '    pstub_TotalEmpSSS = CDec(drowsal("EmployeeContributionAmount"))
                                '    pstub_TotalCompSSS = CDec(drowsal("EmployerContributionAmount"))
                                'End If

                                If isEndOfMonth = 1 Then
                                    emptaxabsal = emptaxabsal - pstub_TotalEmpSSS

                                    fullmonthSalaryTaxableSalary -= pstub_TotalEmpSSS

                                Else
                                    emptaxabsal = emptaxabsal

                                    pstub_TotalEmpSSS = 0

                                    pstub_TotalCompSSS = 0

                                End If

                            End If

                        End If

                        Dim str_TotalEmpSSS As String = pstub_TotalEmpSSS & " " & pstub_TotalCompSSS

                        If drow("EmployeeType").ToString = "Daily" _
                        And drow("PayFrequencyID").ToString = "4" _
                        And paypPhHContribSched = "1" Then

                            'MinWageEmpSSSContrib'MinWageEmpPhHContrib'MinWageEmpHDMFContrib

                            pstub_TotalEmpHDMF = (CDec(drowsal("HDMFAmount")) / 2) ' MinWageEmpHDMFContrib
                            pstub_TotalCompHDMF = (CDec(drowsal("HDMFAmount") / 2)) 'MinWageEmpHDMFContrib 'CDec(drowsal("HDMFAmount"))

                            emptaxabsal -= pstub_TotalEmpHDMF

                            fullmonthSalaryTaxableSalary -= pstub_TotalEmpHDMF

                        Else 'If 1 > 1 Then

                            If isorgHDMFdeductsched = 1 Then 'Per pay period
                                If drow("EmploymentStatus").ToString = "Regular" Then
                                    pstub_TotalEmpHDMF = (CDec(drowsal("HDMFAmount")) / 2)
                                    pstub_TotalCompHDMF = (100 / 2) 'CDec(drowsal("HDMFAmount"))
                                End If

                                emptaxabsal -= pstub_TotalEmpHDMF

                                fullmonthSalaryTaxableSalary -= pstub_TotalEmpHDMF

                            ElseIf isorgHDMFdeductsched = 2 Then 'First half

                                If drow("EmploymentStatus").ToString = "Regular" Then
                                    pstub_TotalEmpHDMF = CDec(drowsal("HDMFAmount"))
                                    pstub_TotalCompHDMF = 100 'CDec(drowsal("HDMFAmount"))
                                End If

                                If isEndOfMonth = 1 Or isEndOfMonth = 0 Then

                                    emptaxabsal = emptaxabsal

                                    If drow("PayFrequencyID") <> 1 Then

                                        pstub_TotalEmpHDMF = 0

                                        pstub_TotalCompHDMF = 0

                                    Else
                                        emptaxabsal = emptaxabsal - pstub_TotalEmpHDMF

                                        fullmonthSalaryTaxableSalary -= pstub_TotalEmpHDMF

                                    End If

                                Else
                                    emptaxabsal = emptaxabsal - pstub_TotalEmpHDMF

                                    fullmonthSalaryTaxableSalary -= pstub_TotalEmpHDMF

                                End If

                            Else
                                'If drow("PayFrequencyID").ToString = 1 Then
                                '    pstub_TotalEmpHDMF = (CDec(drowsal("HDMFAmount")) / 2)
                                '    pstub_TotalCompHDMF = (CDec(drowsal("HDMFAmount")) / 2)
                                'Else
                                'End If
                                If drow("EmploymentStatus").ToString = "Regular" Then
                                    pstub_TotalEmpHDMF = CDec(drowsal("HDMFAmount"))
                                    pstub_TotalCompHDMF = 100 'CDec(drowsal("HDMFAmount"))
                                End If

                                If isEndOfMonth = 1 Then
                                    emptaxabsal = emptaxabsal - pstub_TotalEmpHDMF

                                    fullmonthSalaryTaxableSalary -= pstub_TotalEmpHDMF

                                Else
                                    emptaxabsal = emptaxabsal

                                    pstub_TotalEmpHDMF = 0

                                    pstub_TotalCompHDMF = 0

                                End If

                            End If

                        End If

                        Dim str_TotalEmpHDMF As String = pstub_TotalEmpHDMF & " " & pstub_TotalCompHDMF

                        'If drow("UndertimeOverride").ToString = 1 Then
                        '    emptaxabsal = (emptaxabsal) - (Val(drowtotdaypay("UndertimeHoursAmount"))) 'Val(emptotdaypay("HoursLateAmount"))
                        '    'Else
                        '    '    emptaxabsal = emptaxabsal + totalemployeeallownce
                        'End If

                        'If drow("OvertimeOverride").ToString = 1 Then
                        '    If drow("EmployeeType").ToString = "Fixed" Then
                        '        emptaxabsal = emptaxabsal + (Val(drowtotdaypay("OvertimeHoursAmount")) + Val(drowtotdaypay("NightDiffOTHoursAmount"))) 'Val(emptotdaypay("HoursLateAmount"))
                        '        'ElseIf drow("EmployeeType").ToString = "Daily" Then
                        '        '    grossincome = CDec(drowtotdaypay("TotalDayPay"))
                        '        'ElseIf drow("EmployeeType").ToString = "Hourly" Then
                        '        '    grossincome = CDec(drowtotdaypay("TotalDayPay"))
                        '    End If
                        '    emptaxabsal = emptaxabsal + (Val(drowtotdaypay("OvertimeHoursAmount")) + Val(drowtotdaypay("NightDiffOTHoursAmount"))) 'Val(emptotdaypay("HoursLateAmount"))
                        'End If

                        '17550.00 - (2403.85 + ((17550.00 - 10577.00) * 0.32)) = 12914.79
                        '14471.85 - (2083.33 + ((14471.85 - 13542) * 0.3))
                        'ExemptionAmount'ExemptionInExcessAmount'TaxableIncomeFromAmount
                        'emp_taxabsal = emptaxabsal - Val(drowtax("ExemptionInExcessAmount"))
                        'emp_taxabsal = 1
                        'emp_taxabsal = 1

                        Dim taxab_salval = Trim(emptaxabsal) 'drowsal("Salary").ToString

                        '" AND CURRENT_DATE() BETWEEN COALESCE(EffectiveDateFrom,CURRENT_DATE()) AND COALESCE(EffectiveDateTo,COALESCE(ADDDATE(EffectiveDateFrom, INTERVAL 1 DAY),ADDDATE(CURRENT_DATE(), INTERVAL 1 DAY)))" &

                        'payphilhealth = EmployeeShare
                        'paysocialsecurity = EmployeeContributionAmount

                        'isEndOfMonth
                        'isorgPHHdeductsched'isorgSSSdeductsched'isorgHDMFdeductsched

                        tax_amount = 0

                        Dim empDailyRate = _
                            EXECQUER("SELECT `GET_employeerateperday`('" & drow("RowID") & "', '" & orgztnID & "', '" & paypTo & "');")

                        If isEndOfMonth = 1 And MinimumWageAmount < ValNoComma(empDailyRate) Then

                            Dim paywithholdingtax = retAsDatTbl("SELECT ExemptionAmount,TaxableIncomeFromAmount,ExemptionInExcessAmount" &
                                                                " FROM paywithholdingtax" &
                                                                " WHERE FilingStatusID=(SELECT RowID FROM filingstatus WHERE MaritalStatus='" & drow("MaritalStatus").ToString & "' AND Dependent=" & drow("NoOfDependents").ToString & ")" &
                                                                " AND " & taxab_salval & " BETWEEN TaxableIncomeFromAmount AND TaxableIncomeToAmount" &
                                                                " AND DATEDIFF(CURRENT_DATE(),COALESCE(EffectiveDateTo,COALESCE(EffectiveDateFrom,CURRENT_DATE()))) >= 0" &
                                                                " AND PayFrequencyID='" & drow("PayFrequencyID").ToString & "'" &
                                                                " ORDER BY DATEDIFF(CURRENT_DATE(),COALESCE(EffectiveDateTo,COALESCE(EffectiveDateFrom,CURRENT_DATE())))" &
                                                                " LIMIT 1;")

                            Dim GET_employeetaxableincome = EXECQUER("SELECT `GET_employeetaxableincome`('" & drow("RowID") & "', '" & orgztnID & "', '" & paypFrom & "','" & taxab_salval & "');")

                            For Each drowtax As DataRow In paywithholdingtax.rows

                                emp_taxabsal = emptaxabsal - (Val(drowtax("ExemptionAmount")) + _
                                             ((emptaxabsal - Val(drowtax("TaxableIncomeFromAmount"))) * Val(drowtax("ExemptionInExcessAmount"))) _
                                                             )

                                emp_taxabsal = taxab_salval

                                'tax_amount = (Val(drowtax("ExemptionAmount")) + _
                                '             ((emptaxabsal - Val(drowtax("TaxableIncomeFromAmount"))) * Val(drowtax("ExemptionInExcessAmount"))) _
                                '             )

                                Dim the_values = Split(GET_employeetaxableincome, ";")

                                tax_amount = the_values(1)

                                'If drow("EmployeeType").ToString = "Fixed" Then
                                '    grossincome = CDec(drowsal("Salary"))
                                'ElseIf drow("EmployeeType").ToString = "Daily" Then
                                '    grossincome = CDec(drowtotdaypay("TotalDayPay"))
                                'ElseIf drow("EmployeeType").ToString = "Hourly" Then
                                '    grossincome = CDec(drowtotdaypay("TotalDayPay"))
                                'End If

                                Exit For

                            Next

                        End If

                        Exit For
                    Next

                    Exit For
                Next

                If withthirteenthmonthpay = 0 Then

                    thirteenthmoval = 0.0

                Else

                    If empthirteenmonthtable IsNot Nothing Then

                        Dim selempthirteenmonthtable = empthirteenmonthtable.Select("EmployeeRowID = " & drow("RowID").ToString)

                        For Each thirtrow In selempthirteenmonthtable

                            If Trim(thirtrow("EmployeeType")) = "Fixed" Then

                                thirteenthmoval = Val(thirtrow("CompleteMonthAttended")) * _
                                    Val(thirtrow("multiplicand")) + _
                                    Val(thirtrow("firstmonthpay"))

                            ElseIf Trim(thirtrow("EmployeeType")) = "Daily" Then

                                thirteenthmoval = Val(thirtrow("multiplicand")) * _
                                    Val(thirtrow("HourlyDailyDaysAttended"))

                            ElseIf Trim(thirtrow("EmployeeType")) = "Hourly" Then

                                thirteenthmoval = Val(thirtrow("multiplicand")) * _
                                    Val(thirtrow("ShiftHoursCount")) * _
                                    Val(thirtrow("HourlyDailyDaysAttended"))

                            End If

                            thirteenthmoval = thirteenthmoval / 12

                        Next

                    Else

                        thirteenthmoval = 0.0

                    End If

                End If

                Dim isexisting = EXECQUER("SELECT EXISTS(SELECT RowID FROM paystub WHERE EmployeeID='" & drow("RowID").ToString & _
                                       "' AND OrganizationID=" & orgztnID & _
                                       " AND PayPeriodID='" & paypRowID & "');")

                Dim isexist = 0

                If emp_taxabsal = 0 Then
                    emp_taxabsal = emptaxabsal

                End If

                Dim loopEmpID = drow("RowID").ToString

                Dim tot_net_pay = emp_taxabsal - valemp_loan - tax_amount

                Dim paystubID = _
                INSUPD_paystub(paypRowID, _
                               drow("RowID").ToString, _
                               paypFrom, _
                               paypTo, _
                               grossincome + totalemployeebonus + totalnotaxemployeebonus + totalnotaxemployeeallownce, _
                               tot_net_pay + totalnotaxemployeebonus + totalnotaxemployeeallownce + thirteenthmoval, _
                               emptaxabsal, _
                               tax_amount, _
                               pstub_TotalEmpSSS, _
                               pstub_TotalCompSSS, _
                               pstub_TotalEmpPhilhealth, _
                               pstub_TotalCompPhilhealth, _
                               pstub_TotalEmpHDMF, _
                               pstub_TotalCompHDMF, _
                               valemp_loan, _
                               totalemployeebonus + totalnotaxemployeebonus, _
                               totalemployeeallownce + totalnotaxemployeeallownce) 'totalemployeebonus + totalemployeeallownce +

                If isexist = 0 Then

                    Dim groupallow As New DataTable
                    'groupallow = retAsDatTbl("SELECT *,SUM(AllowanceAmount) 'sum_AllowanceAmount'" &
                    '                         " FROM employeeallowance" &
                    '                         " WHERE EmployeeID='" & drow("RowID").ToString & "'" &
                    '                         " AND TaxableFlag='1'" &
                    '                         " AND IF(EffectiveEndDate IS NULL" &
                    '                         ", DATEDIFF('" & paypTo & "',EffectiveStartDate) >=0 AND EffectiveStartDate>='" & paypFrom & "'" &
                    '                            ", EffectiveStartDate<='" & paypFrom & "' AND EffectiveEndDate>='" & paypTo & "')" &
                    '                         " GROUP BY ProductID;")

                    groupallow = retAsDatTbl("SELECT *,SUM(AllowanceAmount) 'sum_AllowanceAmount'" &
                                             " FROM employeeallowance" &
                                             " WHERE EmployeeID='" & drow("RowID").ToString & "'" &
                                             " AND IF(EffectiveEndDate IS NULL" &
                                             ", DATEDIFF('" & paypTo & "',EffectiveStartDate) >=0 AND EffectiveStartDate>='" & paypFrom & "'" &
                                             ", IF(DATEDIFF(COALESCE(EffectiveEndDate,EffectiveStartDate),EffectiveStartDate) > DATEDIFF('" & paypTo & "','" & paypFrom & "')" &
                    ",EffectiveStartDate<='" & paypFrom & "' AND EffectiveEndDate>='" & paypTo & "'" &
                    ",EffectiveStartDate>='" & paypFrom & "' AND EffectiveEndDate<='" & paypTo & "'))" &
                                             " GROUP BY ProductID;")

                    'IF(DATEDIFF(COALESCE(EffectiveEndDate,EffectiveStartDate),EffectiveStartDate) > DATEDIFF('" & paypTo & "','" & paypFrom & "')" &
                    '",EffectiveStartDate<='" & paypFrom & "' AND EffectiveEndDate>='" & paypTo & "'" &
                    '",EffectiveStartDate>='" & paypFrom & "' AND EffectiveEndDate<='" & paypTo & "'"  & _

                    If groupallow.Rows.Count = 0 Then

                        'Clothing,Meal,Rice,Transportation
                        '312,313,314,315
                        For Each strval In allow_type
                            INSUPD_paystubitem(, paystubID, strval, 0)
                        Next
                        'INSUPD_paystubitem(, paystubID, allow_type(1).ToString, 0)
                        'INSUPD_paystubitem(, paystubID, allow_type(2).ToString, 0)
                        'INSUPD_paystubitem(, paystubID, allow_type(3).ToString, 0)

                    Else
                        For Each gallow As DataRow In groupallow.Rows
                            INSUPD_paystubitem(, paystubID, gallow("ProductID"), gallow("sum_AllowanceAmount"))
                        Next

                    End If

                    'numofdaypresent

                    'Dim dailyallowfreq = "Daily"

                    'If allowfreq.Count <> 0 Then
                    '    dailyallowfreq = If(allowfreq.Item(0).ToString = "", "Daily", allowfreq.Item(0).ToString)
                    'End If

                    ' ''employeeallownce - Daily
                    'Dim eallowdaily As New DataTable
                    'eallowdaily = retAsDatTbl("SELECT ProductID,AllowanceAmount" &
                    '                              " FROM employeeallowance" &
                    '                              " WHERE OrganizationID=" & orgztnID & _
                    '                              " AND TaxableFlag='1'" &
                    '                              " AND EmployeeID='" & drow("RowID").ToString & _
                    '                              "' AND AllowanceFrequency='" & dailyallowfreq & "'" &
                    '                              " AND EffectiveStartDate<='" & paypFrom & "'" &
                    '                              " AND EffectiveEndDate>='" & paypTo & "'" &
                    '                              " GROUP BY ProductID;")

                    'For Each eallowday As DataRow In eallowdaily.Rows
                    '    INSUPD_paystubitem(, paystubID, eallowday("ProductID"), eallowday("AllowanceAmount"))
                    'Next

                    'Dim monthlyallowfreq = "Monthly"

                    'If allowfreq.Count <> 0 Then
                    '    monthlyallowfreq = If(allowfreq.Item(1).ToString = "", "Monthly", allowfreq.Item(1).ToString)
                    'End If

                    ''employeeallownce - Monthly
                    'Dim eallowmonthly As New DataTable
                    'eallowmonthly = retAsDatTbl("SELECT ProductID,AllowanceAmount" &
                    '                              " FROM employeeallowance" &
                    '                              " WHERE OrganizationID=" & orgztnID & _
                    '                              " AND TaxableFlag='1'" &
                    '                              " AND EmployeeID='" & drow("RowID").ToString & _
                    '                              "' AND AllowanceFrequency='" & monthlyallowfreq & "'" &
                    '                              " AND EffectiveStartDate<='" & paypFrom & "'" &
                    '                              " AND EffectiveEndDate>='" & paypTo & "'" &
                    '                              " AND DATEDIFF(CURRENT_DATE(),EffectiveStartDate)>=0" &
                    '                              " GROUP BY ProductID;")

                    'For Each eallowmon As DataRow In eallowmonthly.Rows
                    '    INSUPD_paystubitem(, paystubID, eallowmon("ProductID"), eallowmon("AllowanceAmount"))
                    'Next

                    'Dim onceallowfreq = "One time"

                    'If allowfreq.Count <> 0 Then
                    '    onceallowfreq = If(allowfreq.Item(2).ToString = "", "One time", allowfreq.Item(2).ToString)
                    'End If

                    ''employeeallownce - One time
                    'Dim eallowonce As New DataTable
                    'eallowonce = retAsDatTbl("SELECT ProductID,AllowanceAmount" &
                    '                              " FROM employeeallowance" &
                    '                              " WHERE OrganizationID=" & orgztnID & _
                    '                              " AND TaxableFlag='1'" &
                    '                              " AND EmployeeID='" & drow("RowID").ToString & _
                    '                              "' AND AllowanceFrequency='" & onceallowfreq & "'" &
                    '                              " AND EffectiveStartDate='" & paypFrom & "'" &
                    '                              " AND DATEDIFF(CURRENT_DATE(),EffectiveStartDate)>=0" &
                    '                              " GROUP BY ProductID;")

                    'For Each eallowone As DataRow In eallowonce.Rows
                    '    INSUPD_paystubitem(, paystubID, eallowone("ProductID"), eallowone("AllowanceAmount"))
                    'Next

                    'employeebonus
                    Dim e_bon As New DataTable
                    e_bon = retAsDatTbl("SELECT ProductID,BonusAmount" &
                                            " FROM employeebonus" &
                                            " WHERE OrganizationID=" & orgztnID & _
                                            " AND EmployeeID='" & drow("RowID").ToString & _
                                            "' AND TaxableFlag='1'" &
                                            " AND IF(EffectiveEndDate IS NULL" &
                                            ", DATEDIFF('" & paypTo & "',EffectiveStartDate) >=0 AND EffectiveStartDate>='" & paypFrom & "'" &
                                            ", EffectiveStartDate<='" & paypFrom & "' AND EffectiveEndDate >='" & paypTo & "')" &
                                            " GROUP BY ProductID;")

                    For Each ebon As DataRow In e_bon.Rows
                        INSUPD_paystubitem(, paystubID, ebon("ProductID"), ebon("BonusAmount"))
                    Next




                    'Absent,Tardiness,Undertime,.PAGIBIG,.PhilHealth,.SSS

                    '338,339,340,344,345,346

                    INSUPD_paystubitem(, paystubID, arraydeduction(0).ToString, pstub_TotalEmpHDMF)
                    INSUPD_paystubitem(, paystubID, arraydeduction(1).ToString, pstub_TotalEmpPhilhealth)
                    INSUPD_paystubitem(, paystubID, arraydeduction(2).ToString, pstub_TotalEmpSSS)

                    Dim sel_TardinessUndertime = emp_TardinessUndertime.Select("EmployeeID=" & drow("RowID").ToString)

                    If sel_TardinessUndertime.Count = 0 Then

                        INSUPD_paystubitem(, paystubID, arraydeduction(3).ToString, 0) 'Absent
                        INSUPD_paystubitem(, paystubID, arraydeduction(4).ToString, 0) 'Tardiness
                        INSUPD_paystubitem(, paystubID, arraydeduction(5).ToString, 0) 'Undertime

                    Else

                        For Each d_row In sel_TardinessUndertime

                            INSUPD_paystubitem(, paystubID, arraydeduction(3).ToString, 0) 'Absent

                            If IsDBNull(d_row("HoursLateAmount")) Then
                                INSUPD_paystubitem(, paystubID, arraydeduction(4).ToString, 0) 'Tardiness
                            Else
                                INSUPD_paystubitem(, paystubID, arraydeduction(4).ToString, d_row("HoursLateAmount")) 'Tardiness
                            End If

                            If IsDBNull(d_row("UndertimeHoursAmount")) Then
                                INSUPD_paystubitem(, paystubID, arraydeduction(5).ToString, 0) 'Undertime
                            Else
                                INSUPD_paystubitem(, paystubID, arraydeduction(5).ToString, d_row("UndertimeHoursAmount")) 'Undertime
                            End If

                        Next

                    End If

                    'Leave


                    'employeeloans

                    Dim sel_eloans = eloans.Select("EmployeeID=" & drow("RowID").ToString)

                    If sel_eloans.Count = 0 Then
                        'PhilHealth,SSS,PAGIBIG

                        For Each strval In loantyp
                            INSUPD_paystubitem(, paystubID, strval, 0)
                        Next

                    Else
                        Dim idloanalreadyinsert As New List(Of String)

                        For Each val_loan In sel_eloans
                            If loantyp.Contains(Trim(val_loan("LoanTypeID"))) Then

                                If Trim(val_loan("LoanDueDate")) = "1" Then

                                    Dim loanbutal = (Val(val_loan("TotalLoanAmount")) - (Val(val_loan("DeductionAmount")) * Val(val_loan("NoOfPayPeriod"))))

                                    'loanbutal = If(loanbutal < 0, loanbutal, loanbutal)

                                    INSUPD_paystubitem(, paystubID, val_loan("LoanTypeID"), val_loan("DeductionAmount") + loanbutal)

                                Else

                                    INSUPD_paystubitem(, paystubID, val_loan("LoanTypeID"), val_loan("DeductionAmount"))

                                End If

                                idloanalreadyinsert.Add(Trim(val_loan("LoanTypeID")))

                            Else

                            End If

                        Next

                        For Each strval In loantyp
                            If idloanalreadyinsert.Contains(strval) Then
                            Else
                                If idloanalreadyinsert.Contains(strval) Then
                                Else
                                    idloanalreadyinsert.Add(strval)

                                    INSUPD_paystubitem(, paystubID, strval, 0)

                                End If

                            End If

                        Next

                        'If eloans.Rows.Count = 0 Then

                        'Else
                        '    For Each e_loan As DataRow In eloans.Rows
                        '        INSUPD_paystubitem(, paystubID, e_loan("LoanTypeID"), e_loan("DeductionAmount"))
                        '    Next
                        'End If

                    End If





                    '341,342,343,361

                    'Overtime,Night differential OT,Holiday pay,Night differential

                    'Holiday pay,Night differential,Night differential OT,Overtime

                    INSUPD_paystubitem(, paystubID, miscs(3).ToString, OTAmount) 'Overtime

                    INSUPD_paystubitem(, paystubID, miscs(2).ToString, NightDiffOTAmount) 'Night differential OT

                    INSUPD_paystubitem(, paystubID, miscs(1).ToString, NightDiffAmount) 'Night differential

                    Dim holidaypayresult = etent_holidaypay.Select("EmployeeID=" & drow("RowID").ToString)

                    If holidaypayresult.Count = 0 Then
                        INSUPD_paystubitem(, paystubID, miscs(0).ToString, 0) 'Holiday pay
                    Else

                        For Each drowval In holidaypayresult

                            INSUPD_paystubitem(, paystubID, miscs(0).ToString, If(IsDBNull(drowval(1)), 0, Val(drowval(1)))) 'Holiday pay

                        Next

                    End If

                    'Withholding Tax,Gross Income,Net Income,Taxable Income

                    '347,348,349,350

                    Array.Sort(emp_totals)

                    Dim getstrvalue = String.Empty

                    getstrvalue = getStrBetween(StrReverse(emp_totals(0).ToString), "", ";")
                    INSUPD_paystubitem(, paystubID, StrReverse(getstrvalue), grossincome) 'Gross Income

                    getstrvalue = getStrBetween(StrReverse(emp_totals(1).ToString), "", ";")
                    INSUPD_paystubitem(, paystubID, StrReverse(getstrvalue), tot_net_pay + totalnotaxemployeebonus + totalnotaxemployeeallownce + thirteenthmoval) 'Net Income

                    getstrvalue = getStrBetween(StrReverse(emp_totals(2).ToString), "", ";")
                    INSUPD_paystubitem(, paystubID, StrReverse(getstrvalue), emptaxabsal) 'Taxable Income
                    'pstub_TotalEmpSSS,pstub_TotalEmpPhilhealth,pstub_TotalEmpHDMF
                    getstrvalue = getStrBetween(StrReverse(emp_totals(3).ToString), "", ";")
                    INSUPD_paystubitem(, paystubID, StrReverse(getstrvalue), tax_amount) 'Withholding Tax


                    If isexisting = 0 Then 'the paystub not yet exists

                        If drow("EmploymentStatus").ToString = "Regular" Then 'accruance of leave are for regular employees only

                            Dim sel_empleave = empleave.Select("EmployeeID=" & drow("RowID").ToString)

                            Dim leavebal, sickbal, maternbal, othersbal As Double

                            leavebal = 0.0
                            sickbal = 0.0
                            maternbal = 0.0
                            othersbal = 0.0

                            Dim lst_leaveinsert As New List(Of String)

                            lst_leaveinsert.Clear()

                            If sel_empleave.Count = 0 Then
                                'Leave Type - Vacation leave,Sick leave,Maternity/paternity leave,Others
                                'Dim paypindex = 43

                                'For Each strval In leavtyp
                                '    INSUPD_paystubitem(, paystubID, strval, 0) 'drow(paypindex))
                                '    'paypindex += 1
                                'Next

                                EXECQUER("UPDATE employee SET" &
                                         " LeaveBalance=COALESCE(LeaveBalance,0) + LeavePerPayPeriod" &
                                         ",SickLeaveBalance=COALESCE(SickLeaveBalance,0) + SickLeavePerPayPeriod" &
                                         ",MaternityLeaveBalance=COALESCE(MaternityLeaveBalance,0) + MaternityLeavePerPayPeriod" &
                                         ",OtherLeaveBalance=COALESCE(OtherLeaveBalance,0) + OtherLeavePerPayPeriod" &
                                         ",LastUpdBy=" & z_User & _
                                         " WHERE RowID='" & drow("RowID").ToString & "';") 'OrganizationID=" & orgztnID & " AND 

                                'leavebalances'leavebalan


                                leavebalances = EXECQUER("SELECT CONCAT(COALESCE(MaternityLeaveBalance,0)" &
                                                         ",',0'" &
                                                         ",',',COALESCE(SickLeaveBalance,0)" &
                                                         ",',',COALESCE(OtherLeaveBalance,0)" &
                                                         ",',',COALESCE(LeaveBalance,0))" &
                                                         " FROM employee WHERE OrganizationID=" & orgztnID & " AND RowID='" & drow("RowID").ToString & "';")

                                leavebalan = Split(leavebalances, ",")

                                '353,354,352,351
                                '351,352,353,354

                                Dim leavetypecount = leavtyp.GetUpperBound(0) ' - 1

                                '389,397,401,402,410

                                'Maternity leave
                                'Paternity leave
                                'Sick leave
                                'Single Parent leave
                                'Vacation leave

                                For i = 0 To leavetypecount

                                    Try
                                        INSUPD_paystubitem(, paystubID, leavtyp(i).ToString, leavebalan(i).ToString)

                                    Catch ex As Exception
                                        'INSUPD_paystubitem(, paystubID, leavtyp(i).ToString, 0)
                                        lst_leaveinsert.Add(leavtyp(i).ToString)

                                    End Try

                                Next


                                If lst_leaveinsert.Count <> 0 Then
                                    For Each leave_id In leavtyp

                                        If lst_leaveinsert.Contains(leave_id) Then

                                            INSUPD_paystubitem(, paystubID, leave_id, 0)

                                        End If

                                    Next

                                End If



                                'INSUPD_paystubitem(, paystubID, leavtyp(0).ToString, leavebalan(0).ToString) 'Vacation
                                'INSUPD_paystubitem(, paystubID, leavtyp(1).ToString, leavebalan(1).ToString) 'Sick
                                'INSUPD_paystubitem(, paystubID, leavtyp(2).ToString, leavebalan(2).ToString) 'Maternity
                                'INSUPD_paystubitem(, paystubID, leavtyp(3).ToString, 0)

                            Else
                                For Each elvrow In sel_empleave
                                    If elvrow("LeaveType").ToString = "Vacation leave" Then
                                        leavebal = Val(elvrow("NumOfHoursLeave")) 'FormatNumber(Val(elvrow("NumOfHoursLeave")), 2)

                                        'INSUPD_paystubitem(, paystubID, elvrow("ProductID"), (elvrow("NumOfDaysLeave") * leavebal))

                                    ElseIf elvrow("LeaveType").ToString = "Sick leave" Then
                                        sickbal = Val(elvrow("NumOfHoursLeave")) 'FormatNumber(Val(elvrow("NumOfHoursLeave")), 2)

                                        'INSUPD_paystubitem(, paystubID, elvrow("ProductID"), (elvrow("NumOfDaysLeave") * sickbal))

                                    ElseIf elvrow("LeaveType").ToString = "Maternity/paternity leave" Then
                                        maternbal = Val(elvrow("NumOfHoursLeave")) 'FormatNumber(Val(elvrow("NumOfHoursLeave")), 2)

                                        'INSUPD_paystubitem(, paystubID, elvrow("ProductID"), (elvrow("NumOfDaysLeave") * maternbal))

                                    Else '
                                        If elvrow("LeaveType").ToString = "Others" Then
                                            othersbal = Val(elvrow("NumOfHoursLeave"))
                                        ElseIf elvrow("LeaveType").ToString = "Single Parent leave" Then
                                            othersbal = Val(elvrow("NumOfHoursLeave"))
                                        End If
                                        'FormatNumber(Val(elvrow("NumOfHoursLeave")), 2)

                                        'INSUPD_paystubitem(, paystubID, elvrow("ProductID"), (elvrow("NumOfDaysLeave") * othersbal))

                                    End If

                                Next

                                'EXECQUER("UPDATE employee SET" &
                                '         " LeaveBalance=IF(" & othersbal & _
                                '         ",SickLeaveBalance=" & sickbal & _
                                '         ",MaternityLeaveBalance=" & maternbal & _
                                '         " WHERE OrganizationID=" & orgztnID & " AND RowID='" & drow("RowID").ToString & "';")

                                'EXECQUER("UPDATE employee SET" &
                                '         " LeaveBalance=IF(" & leavebal & "=0, (COALESCE(LeaveBalance,0) + LeavePerPayPeriod), " & leavebal & ")" &
                                '         ",SickLeaveBalance=IF(" & sickbal & "=0, (COALESCE(SickLeaveBalance,0) + SickLeavePerPayPeriod), " & sickbal & ")" &
                                '         ",MaternityLeaveBalance=IF(" & maternbal & "=0, (COALESCE(MaternityLeaveBalance,0) + MaternityLeavePerPayPeriod), " & maternbal & ")" &
                                '         " WHERE OrganizationID=" & orgztnID & " AND RowID='" & drow("RowID").ToString & "';")

                                'Employee will accrue the LeavePerPayPeriod, SickLeavePerPayPeriod and MaternityLeavePerPayPeriod
                                ' and will deduct any leaves filed within this pay period of the pay stub

                                EXECQUER("UPDATE employee SET" &
                                         " LeaveBalance=(COALESCE(LeaveBalance,0) + LeavePerPayPeriod) - " & leavebal & "" &
                                         ",SickLeaveBalance=(COALESCE(SickLeaveBalance,0) + SickLeavePerPayPeriod) - " & sickbal & "" &
                                         ",MaternityLeaveBalance=(COALESCE(MaternityLeaveBalance,0) + MaternityLeavePerPayPeriod) - " & maternbal & "" &
                                         ",OtherLeaveBalance=(COALESCE(OtherLeaveBalance,0) + OtherLeavePerPayPeriod) - " & othersbal & "" &
                                         " WHERE OrganizationID=" & orgztnID & " AND RowID='" & drow("RowID").ToString & "';")

                                'leavebalances = EXECQUER("SELECT CONCAT(COALESCE(LeaveBalance,0),',',COALESCE(SickLeaveBalance,0),',',COALESCE(MaternityLeaveBalance,0)) FROM employee WHERE OrganizationID=" & orgztnID & " AND RowID='" & drow("RowID").ToString & "';")

                                leavebalances = EXECQUER("SELECT CONCAT(COALESCE(MaternityLeaveBalance,0)" &
                                                         ",',0'" &
                                                         ",',',COALESCE(SickLeaveBalance,0)" &
                                                         ",',',COALESCE(OtherLeaveBalance,0)" &
                                                         ",',',COALESCE(LeaveBalance,0))" &
                                                         " FROM employee WHERE OrganizationID=" & orgztnID & " AND RowID='" & drow("RowID").ToString & "';")

                                leavebalan = Split(leavebalances, ",")

                                '353,354,352,351
                                '351,352,353,354

                                'INSUPD_paystubitem(, paystubID, leavtyp(0).ToString, leavebalan(0).ToString) 'Vacation
                                'INSUPD_paystubitem(, paystubID, leavtyp(1).ToString, leavebalan(1).ToString) 'Sick
                                'INSUPD_paystubitem(, paystubID, leavtyp(2).ToString, leavebalan(2).ToString) 'Maternity
                                'INSUPD_paystubitem(, paystubID, leavtyp(3).ToString, 0)

                                Dim leavetypecount = leavtyp.GetUpperBound(0) ' - 1

                                '389,397,401,402,410

                                'Maternity leave
                                'Paternity leave
                                'Sick leave
                                'Single Parent leave
                                'Vacation leave

                                For i = 0 To leavetypecount

                                    Try
                                        INSUPD_paystubitem(, paystubID, leavtyp(i).ToString, leavebalan(i).ToString)

                                    Catch ex As Exception
                                        'INSUPD_paystubitem(, paystubID, leavtyp(i).ToString, 0)
                                        lst_leaveinsert.Add(leavtyp(i).ToString)

                                    End Try

                                Next

                                If lst_leaveinsert.Count <> 0 Then
                                    For Each leave_id In leavtyp

                                        If lst_leaveinsert.Contains(leave_id) Then

                                            INSUPD_paystubitem(, paystubID, leave_id, 0)

                                        End If

                                    Next

                                End If

                            End If

                            'EXECQUER("UPDATE employee SET" &
                            '         " LeaveBalance=COALESCE(LeaveBalance,0) + LeavePerPayPeriod" &
                            '         ",SickLeaveBalance=COALESCE(SickLeaveBalance,0) + SickLeavePerPayPeriod" &
                            '         ",MaternityLeaveBalance=COALESCE(MaternityLeaveBalance,0) + MaternityLeavePerPayPeriod" &
                            '         " WHERE OrganizationID=" & orgztnID & " AND RowID='" & drow("RowID").ToString & "';")

                            'INSUPD_paystubitem(, paystubID, elvrow("ProductID"), (elvrow("NumOfDaysLeave") * elvrow("LeavePerPayPeriod")))

                            'employeeloanschedule - will trigger this...
                            'LoanPayPeriodLeft = LoanPayPeriodLeft - 1 AND LoanPayPeriodLeft != 0


                            '" AND IF('" & paypFrom & "' < DedEffectiveDateFrom AND '" & paypTo & "' < DedEffectiveDateTo" &
                            '", DedEffectiveDateFrom>='" & paypFrom & "' AND DedEffectiveDateTo>='" & paypTo & "'" &
                            '", DedEffectiveDateFrom<='" & paypFrom & "' AND DedEffectiveDateTo>='" & paypTo & "')")

                            '" AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                            '" ,IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "')" &
                            '" ,DedEffectiveDateFrom<='" & paypTo & "')" &


                            'EXECQUER("UPDATE employeeloanschedule SET" &
                            '         " LoanPayPeriodLeft = (LoanPayPeriodLeft - 1)" &
                            '         " WHERE OrganizationID=" & orgztnID & _
                            '         " AND LoanPayPeriodLeft != 0" &
                            '         " AND Status='In Progress'" &
                            '         " AND IF(DedEffectiveDateFrom < '" & paypTo & "'" &
                            '         " ,IF(DAY(DedEffectiveDateFrom) BETWEEN DAY('" & paypFrom & "') AND DAY('" & paypTo & "'), DedEffectiveDateFrom BETWEEN '" & paypFrom & "' AND '" & paypTo & "', DedEffectiveDateFrom<='" & paypTo & "')" &
                            '         " ,DedEffectiveDateFrom<='" & paypTo & "');")


                        Else 'zero accruance of leave for probationary employees only

                            For Each strval In leavtyp
                                INSUPD_paystubitem(, paystubID, strval.ToString, 0)
                            Next

                        End If

                    Else 'the paystub already exists

                    End If


                    'For Each strval In empalldistrib

                    '    Dim splitstr = Split(strval, "@")

                    '    'empalldistrib.Add(allowaval & "@" & ddrow("ProductID"))

                    '    INSUPD_paystubitem(, paystubID, splitstr(1).ToString, splitstr(0).ToString)

                    'Next

                    If dtempalldistrib IsNot Nothing Then

                        If dtempalldistrib.Rows.Count <> 0 Then

                            Dim GroupedSum = From userentry In dtempalldistrib
                                             Group userentry By key = userentry.Field(Of String)("ProductID") Into Group _
                                             Select ProductID = key, SumVal = Group.Sum(Function(p As Object) ValNoComma(p("Value")))

                            Dim includedallowance As New List(Of String)

                            includedallowance.Clear()

                            For Each strr In GroupedSum.ToList
                                'MsgBox(strr.ProductID & vbNewLine & strr.SumVal)

                                INSUPD_paystubitem(, paystubID, strr.ProductID, strr.SumVal)

                                includedallowance.Add(strr.ProductID)

                            Next

                            'allow_type

                            For Each strr In allow_type

                                If includedallowance.Contains(strr) Then

                                Else

                                    INSUPD_paystubitem(, paystubID, strr, 0)

                                End If

                            Next

                        Else

                            'SELECT * FROM product WHERE CategoryID='33' AND OrganizationID=2;

                        End If

                    End If

                End If

                ConvertLeaveToCash(employee_ID, drow)

                'employee_dattab.Rows
                'bgworkgenpayroll.ReportProgress()

                bgworkgenpayroll.ReportProgress(CInt((100 * progress_index) / emp_count), "")

                progress_index += 1

                employee_ID = Trim(drow("RowID"))

            Next

            EXECQUER("CALL `RECOMPUTE_thirteenthmonthpay`('" & orgztnID & "','" & paypRowID & "','" & z_User & "');")

        End If


        'EXECQUER("CALL `RECOMPUTE_weeklyemployeeSSSContribAmount`('" & orgztnID & "','" & Current_PayPeriodID & "');")

    End Sub

    Private Sub bgworkgenpayroll_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkgenpayroll.ProgressChanged
        MDIPrimaryForm.systemprogressbar.Value = CType(e.ProgressPercentage, Integer)

    End Sub

    Public genpayselyear As String = Nothing

    Private Sub bgworkgenpayroll_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkgenpayroll.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            'MsgBox("Error : bgworkgenpayroll" & vbNewLine & e.Error.Message)
            MsgBox(getErrExcptn(e.Error, Me.Name), , "bgworkgenpayroll_RunWorkerCompleted")

        ElseIf e.Cancelled Then

            If pause_process_message = String.Empty Then

                MsgBox("Background work cancelled.", _
                       MsgBoxStyle.Exclamation)

            Else

                MsgBox(pause_process_message, _
                       MsgBoxStyle.Information,
                       "Payroll process skipped")

            End If

        Else
            
            VIEW_payperiodofyear(genpayselyear)

            loademployee(quer_empPayFreq)

            employee_dattab = Nothing

            esal_dattab = Nothing

            etent_dattab = Nothing

            etent_totdaypay = Nothing

            emp_loans = Nothing

            emp_bonus = Nothing

            emp_bonusDaily = Nothing

            notax_bonusDaily = Nothing

            emp_bonusMonthly = Nothing

            notax_bonusMonthly = Nothing

            emp_bonusOnce = Nothing

            notax_bonusOnce = Nothing

            emp_allowanceDaily = Nothing

            notax_allowanceDaily = Nothing

            emp_allowanceMonthly = Nothing

            notax_allowanceMonthly = Nothing

            emp_allowanceOnce = Nothing

            notax_allowanceOnce = Nothing

            numofdaypresent = Nothing

            eloans = Nothing

            empleave = Nothing

            allowtyp = Nothing

            deductions = Nothing

            loan_type = Nothing

            misc = Nothing

            totals = Nothing

            leavetype = Nothing

            empthirteenmonthtable = Nothing

            dtempalldistrib.Rows.Clear()

            MsgBox("Done generating payroll", _
                   MsgBoxStyle.Information)

        End If

        PayrollForm.MenuStrip1.Enabled = True

        MDIPrimaryForm.Showmainbutton.Enabled = True

        Me.Enabled = True

        MDIPrimaryForm.systemprogressbar.Visible = False

        ToolStrip1.Enabled = True

        Panel5.Enabled = True

        MDIPrimaryForm.systemprogressbar.Value = Nothing

        'AddHandler dgvpayper.SelectionChanged, AddressOf dgvpayper_SelectionChanged

        ''AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        dgvpayper_SelectionChanged(sender, e)

        backgroundworking = 0

        Me.Enabled = True

    End Sub

    Function INSUPD_paystub(Optional pstub_PayPeriodID As Object = Nothing, _
                                              Optional etent_EmployeeID As Object = Nothing, _
                                              Optional pstub_PayFromDate As Object = Nothing, _
                                              Optional pstub_PayToDate As Object = Nothing, _
                                              Optional pstub_TotalGrossSalary As Object = Nothing, _
                                              Optional pstub_TotalNetSalary As Object = Nothing, _
                                              Optional pstub_TotalTaxableSalary As Object = Nothing, _
                                              Optional pstub_TotalEmpWithholdingTax As Object = Nothing, _
                                              Optional pstub_TotalEmpSSS As Object = Nothing, _
                                              Optional pstub_TotalCompSSS As Object = Nothing, _
                                              Optional pstub_TotalEmpPhilhealth As Object = Nothing, _
                                              Optional pstub_TotalCompPhilhealth As Object = Nothing, _
                                              Optional pstub_TotalEmpHDMF As Object = Nothing, _
                                              Optional pstub_TotalCompHDMF As Object = Nothing, _
                                              Optional pstub_TotalLoans As Object = Nothing, _
                                              Optional pstub_TotalBonus As Object = Nothing, _
                                              Optional pstub_TotalAllowance As Object = Nothing) As Object
        Dim return_value = Nothing
        Try
            'If new_conn.State = ConnectionState.Open Then
            '    new_conn.Close()
            'Else
            '    conn.Open()
            'End If

            If conn.State = ConnectionState.Open Then
                conn.Close()
                'Else
            End If

            new_cmd = New MySqlCommand("INSUPD_paystub", conn)

            conn.Open()

            With new_cmd
                .Parameters.Clear()
                .CommandTimeout = 5000
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("paystubID", MySqlDbType.Int32)

                .Parameters.AddWithValue("pstub_RowID", DBNull.Value)
                .Parameters.AddWithValue("pstub_OrganizationID", orgztnID)
                .Parameters.AddWithValue("pstub_CreatedBy", z_User)
                .Parameters.AddWithValue("pstub_LastUpdBy", z_User)
                .Parameters.AddWithValue("pstub_PayPeriodID", pstub_PayPeriodID)
                .Parameters.AddWithValue("pstub_EmployeeID", etent_EmployeeID)

                .Parameters.AddWithValue("pstub_TimeEntryID", DBNull.Value)

                .Parameters.AddWithValue("pstub_PayFromDate", If(pstub_PayFromDate = Nothing, DBNull.Value, Format(CDate(pstub_PayFromDate), "yyyy-MM-dd")))
                .Parameters.AddWithValue("pstub_PayToDate", If(pstub_PayToDate = Nothing, DBNull.Value, Format(CDate(pstub_PayToDate), "yyyy-MM-dd")))

                .Parameters.AddWithValue("pstub_TotalGrossSalary", pstub_TotalGrossSalary)
                .Parameters.AddWithValue("pstub_TotalNetSalary", pstub_TotalNetSalary)
                .Parameters.AddWithValue("pstub_TotalTaxableSalary", pstub_TotalTaxableSalary)
                .Parameters.AddWithValue("pstub_TotalEmpWithholdingTax", pstub_TotalEmpWithholdingTax)

                .Parameters.AddWithValue("pstub_TotalEmpSSS", pstub_TotalEmpSSS) 'DBNull.Value
                .Parameters.AddWithValue("pstub_TotalCompSSS", pstub_TotalCompSSS)
                .Parameters.AddWithValue("pstub_TotalEmpPhilhealth", pstub_TotalEmpPhilhealth)
                .Parameters.AddWithValue("pstub_TotalCompPhilhealth", pstub_TotalCompPhilhealth)
                .Parameters.AddWithValue("pstub_TotalEmpHDMF", pstub_TotalEmpHDMF)
                .Parameters.AddWithValue("pstub_TotalCompHDMF", pstub_TotalCompHDMF)
                .Parameters.AddWithValue("pstub_TotalVacationDaysLeft", pstub_TotalVacationDaysLeft)
                .Parameters.AddWithValue("pstub_TotalLoans", pstub_TotalLoans)
                .Parameters.AddWithValue("pstub_TotalBonus", pstub_TotalBonus)
                .Parameters.AddWithValue("pstub_TotalAllowance", pstub_TotalAllowance)

                .Parameters("paystubID").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()
                'INSUPD_paystub
                return_value = datread(0)

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " " & "INSUPD_paystub", , "Error")
        Finally
            new_conn.Close()
            conn.Close()
            new_cmd.Dispose()
        End Try
        Return return_value
    End Function

    Function INSUPD_paystubitem(Optional pstubitm_RowID As Object = Nothing, _
                                Optional pstubitm_PayStubID As Object = Nothing, _
                                Optional pstubitm_ProductID As Object = Nothing, _
                                Optional pstubitm_PayAmount As Object = Nothing) As Object
        Dim return_value = Nothing
        Try

            If conn.State = ConnectionState.Open Then
                conn.Close()
                'Else
            End If

            new_cmd = New MySqlCommand("INSUPD_paystubitem", conn)

            conn.Open()

            With new_cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("pstubtimID", MySqlDbType.Int32)

                .Parameters.AddWithValue("pstubitm_RowID", If(pstubitm_RowID = Nothing, DBNull.Value, pstubitm_RowID))
                .Parameters.AddWithValue("pstubitm_OrganizationID", orgztnID)
                .Parameters.AddWithValue("pstubitm_CreatedBy", z_User)
                .Parameters.AddWithValue("pstubitm_LastUpdBy", z_User)
                .Parameters.AddWithValue("pstubitm_PayStubID", pstubitm_PayStubID)
                .Parameters.AddWithValue("pstubitm_ProductID", pstubitm_ProductID)
                .Parameters.AddWithValue("pstubitm_PayAmount", ValNoComma(pstubitm_PayAmount))

                .Parameters("pstubtimID").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()
                'INSUPD_paystubitem
                return_value = datread(0)

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " " & "INSUPD_paystubitem", , "Error")
        Finally
            new_conn.Close()
            conn.Close()
            new_cmd.Dispose()
        End Try
        Return return_value
    End Function

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

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs)

        'Static once As SByte = 0

        'Dim searchitem = prodPartNo.Select("PartNo = '" & Trim(ComboBox1.Text) & "'")

        'With ComboBox1
        '    .DataSource = searchitem

        '    If once = 0 Then
        '        once = 1
        '        '.DisplayMember = "PartNo"
        '        '.ValueMember = "PartNo"
        '    End If

        '    '.AutoCompleteMode = AutoCompleteMode.SuggestAppend ' This is necessary
        '    '.AutoCompleteSource = AutoCompleteSource.ListItems

        'End With

    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem
        TabControlColor(TabControl1, e)

    End Sub

    Private Sub dgvpaystub_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpaystub.CellContentClick

    End Sub

    Private Sub dgvpaystub_SelectionChanged(sender As Object, e As EventArgs) Handles dgvpaystub.SelectionChanged

    End Sub

    Private Sub dgvpaystubitem_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpaystubitem.CellContentClick

    End Sub

    Private Sub dgvpaystubitem_SelectionChanged(sender As Object, e As EventArgs) Handles dgvpaystubitem.SelectionChanged

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

                .VIEW_employeeallowance_indate(dgvemployees.CurrentRow.Cells("RowID").Value, _
                                        paypFrom, _
                                        paypTo, _
                                        numofweekdays)

                'End If

                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value

            End If

        End With

    End Sub

    Sub VIEW_eallow_indate(Optional eallow_EmployeeID As Object = Nothing, _
                               Optional datefrom As Object = Nothing, _
                               Optional dateto As Object = Nothing, _
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

        EXEC_VIEW_PROCEDURE(param, _
                           "VIEW_employeeallowances", _
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
                .VIEW_employeeloan_indate(dgvemployees.CurrentRow.Cells("RowID").Value, _
                                        paypFrom, _
                                        paypTo)

                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value
            End If
        End With

    End Sub

    Sub VIEW_eloan_indate(Optional eloan_EmployeeID As Object = Nothing, _
                               Optional datefrom As Object = Nothing, _
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

        EXEC_VIEW_PROCEDURE(params, _
                             "VIEW_employeeloan_indate", _
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
                .VIEW_employeebonus_indate(dgvemployees.CurrentRow.Cells("RowID").Value, _
                                        paypFrom, _
                                        paypTo)

                .Text = .Text & " - ID# " & dgvemployees.CurrentRow.Cells("EmployeeID").Value
            End If
        End With

    End Sub
    Sub VIEW_ebon_indate(Optional ebon_EmployeeID As Object = Nothing, _
                               Optional datefrom As Object = Nothing, _
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

        EXEC_VIEW_PROCEDURE(params, _
                             "VIEW_employeebonus_indate", _
                             dgvempbon)

    End Sub
    Private Sub PayStub_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        Dim alive_bgworks = array_bgwork.Cast(Of System.ComponentModel.BackgroundWorker).Where(Function(y) y IsNot Nothing)

        Dim busy_bgworks = alive_bgworks.Cast(Of System.ComponentModel.BackgroundWorker).Where(Function(y) y.IsBusy)

        Dim bool_result As Boolean = (Convert.ToInt16(busy_bgworks.Count) > 0)

        If bgworkgenpayroll.IsBusy Then 'bgworkgenpayroll.IsBusy, bool_result
            e.Cancel = True

        Else
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

        End If

    End Sub

    Private Sub tsSearch_Click(sender As Object, e As EventArgs) Handles tsSearch.Click

    End Sub

    Dim IsUserPressEnterToSearch As Boolean = False

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

            Dim n_ReadSQLProcedureToDatatable As New  _
                ReadSQLProcedureToDatatable("SEARCH_employee_paystub",
                                            param_array)

            dattabsearch = n_ReadSQLProcedureToDatatable.ResultTable

            '" WHERE MATCH (e.FirstName,e.MiddleName,e.LastName,e.Surname,e.EmployeeID,e.TINNo,e.SSSNo,e.HDMFNo,e.PhilHealthNo)" &
            '" AGAINST ('" & Trim(tsSearch.Text) & "') AND e.OrganizationID=" & orgztnID & " ORDER BY e.RowID DESC LIMIT " & pagination & ",100;")

            dgvemployees.Rows.Clear()

            For Each drow As DataRow In dattabsearch.Rows
                dgvemployees.Rows.Add(drow("RowID"), _
                                      drow("EmployeeID"), _
                                      drow("FirstName"), _
                                      drow("MiddleName"), _
                                      drow("LastName"), _
                                      drow("Surname"), _
                                      drow("Nickname"), _
                                      drow("MaritalStatus"), _
                                      drow("NoOfDependents"), _
                                      Format(CDate(drow("Birthdate")), machineShortDateFormat), _
                                      Format(CDate(drow("StartDate")), machineShortDateFormat), _
                                      drow("JobTitle"), _
                                      If(IsDBNull(drow("PositionName")), "", drow("PositionName")), _
                                      drow("Salutation"), _
                                      drow("TINNo"), _
                                      drow("SSSNo"), _
                                      drow("HDMFNo"), _
                                      drow("PhilHealthNo"), _
                                      drow("WorkPhone"), _
                                      drow("HomePhone"), _
                                      drow("MobilePhone"), _
                                      drow("HomeAddress"), _
                                      drow("EmailAddress"), _
                                      If(Trim(drow("Gender")) = "M", "Male", "Female"), _
                                      drow("EmploymentStatus"), _
                                      drow("PayFrequencyType"), _
                                      drow("UndertimeOverride"), _
                                      drow("OvertimeOverride"), _
                                      If(IsDBNull(drow("PositionID")), "", drow("PositionID")), _
                                      drow("PayFrequencyID"), _
                                      drow("EmployeeType"), _
                                      drow("LeaveBalance"), _
                                      drow("SickLeaveBalance"), _
                                      drow("MaternityLeaveBalance"), _
                                      drow("LeaveAllowance"), _
                                      drow("SickLeaveAllowance"), _
                                      drow("MaternityLeaveAllowance"), _
                                      drow("FilingStatus"), _
                                      Nothing, _
                                      drow("Created"), _
                                      drow("CreatedBy"), _
                                      If(IsDBNull(drow("LastUpd")), "", drow("LastUpd")), _
                                      If(IsDBNull(drow("LastUpdBy")), "", drow("LastUpdBy")))

            Next

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

    Private Sub MaskedTextBox1_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles MaskedTextBox1.MaskInputRejected

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

        MsgBox(GET_employeeallowance(4, _
                                     "Semi-monthly", _
                                     "Fixed", _
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

    Dim dtempalldistrib As New DataTable

    'gingamit ko lang an 'GET_employeeallowance' sa allowance
    'na may Daily at semi-monthly
    'na allowance frequency

    Function GET_employeeallowance(Optional employeeRowID = Nothing, _
                                   Optional allowancefrequensi = Nothing, _
                                   Optional emppaytype = Nothing, _
                                   Optional istaxab = Nothing, _
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
                                              " WHERE OrganizationID=" & orgztnID & _
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
                                              " WHERE OrganizationID=" & orgztnID & _
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

    Dim rptdattab As New DataTable

    Dim PayrollSummaChosenData As String = String.Empty

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

                Dim n_ReadSQLProcedureToDatatable As New  _
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

                    papy_string = "for the period of " & Format(CDate(n_PayrollSummaDateSelection.DateFromstr), machineShortDateFormat) & _
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
            Dim log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
            log.Error("Error", ex)
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

            datatab = callProcAsDatTab(params, _
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
                newdatrow("DatCol20") = Format(CDate(n_PayrollSummaDateSelection.DateFromstr), "MMMM d, yyyy") & _
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

                AbsTardiUTNDifOTHolipay = callProcAsDatTab(paramets, _
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
                papy_string = "for the period of " & Format(CDate(n_PayrollSummaDateSelection.DateFromstr), machineShortDateFormat) & _
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

    Private Sub ToolStripButton0_Click(sender As Object, e As EventArgs)
        'ToolStripButton0
    End Sub

    Private Sub tbppayroll_Click(sender As Object, e As EventArgs) Handles tbppayroll.Click

    End Sub

    Dim selectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim unselectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

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

    Dim quer_empPayFreq = ""

    Sub PayFreq_Changed(sender As Object, e As EventArgs)

        quer_empPayFreq = ""

        If bgworkgenpayroll.IsBusy Then

        Else

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


    Dim dtJosh As DataTable
    Dim da As New MySqlDataAdapter()

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

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click 'DECLARED

    End Sub

    Private Sub TabPage1_Enter1(sender As Object, e As EventArgs) Handles TabPage1.Enter 'DECLARED

        For Each txtbxctrl In TabPage1.Controls.OfType(Of TextBox).ToList()
            txtbxctrl.Text = "0.00"
        Next

        Dim EmployeeRowID = dgvemployees.Tag
        '",(es.BasicPay + (es.BasicPay * GET_employeeundeclaredsalarypercent(es.EmployeeID,es.OrganizationID,'" & paypFrom & "','" & paypTo & "'))) AS BasicPay" &

        'Dim n_SQLQueryToDatatable As _
        '    New SQLQueryToDatatable("SELECT psa.*" &
        '                            ",es.BasicPay" &
        '                            ",es.Salary AS TrueSalary" &
        '                            ",IF(e.EmployeeType='Daily',PAYFREQUENCY_DIVISOR(e.EmployeeType),PAYFREQUENCY_DIVISOR(pf.PayFrequencyType)) AS PAYFREQUENCYDIVISOR" &
        '                            ",ete.*" &
        '                            ",e.EmployeeType" &
        '                            ",(e.StartDate BETWEEN '" & paypFrom & "' AND '" & paypTo & "') AS FirstTimeSalary" &
        '                            " FROM paystub psa" &
        '                            " INNER JOIN employee e ON e.RowID=psa.EmployeeID AND e.OrganizationID=psa.OrganizationID" &
        '                            " INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID" &
        '                            " INNER JOIN employeesalary es" &
        '                                " ON es.EmployeeID=psa.EmployeeID" &
        '                                " AND es.OrganizationID=psa.OrganizationID" &
        '                                " AND (es.EffectiveDateFrom >= psa.PayFromDate OR IFNULL(es.EffectiveDateTo,CURDATE()) >= psa.PayFromDate)" &
        '                                " AND (es.EffectiveDateFrom <= psa.PayToDate OR IFNULL(es.EffectiveDateTo,CURDATE()) <= psa.PayToDate)" &
        '                            " INNER JOIN (SELECT te.RowID AS eteRowID" &
        '                                        ",SUM(RegularHoursWorked) AS RegularHoursWorked" &
        '                                        ",SUM(RegularHoursAmount / pr.PayRate) AS RegularHoursAmount" &
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
        '                                        " FROM employeetimeentry te" &
        '                                        " INNER JOIN payrate pr ON pr.RowID=te.PayRateID" &
        '                                        " WHERE te.EmployeeID='" & EmployeeRowID & "' AND te.OrganizationID='" & orgztnID & "' AND te.`Date` BETWEEN '" & paypFrom & "' AND '" & paypTo & "') ete ON ete.eteRowID > 0" &
        '                            " WHERE psa.EmployeeID='" & EmployeeRowID & "'" &
        '                            " AND psa.OrganizationID='" & orgztnID & "'" &
        '                            " AND psa.PayFromDate='" & paypFrom & "'" &
        '                            " AND psa.PayToDate='" & paypTo & "'" &
        '                            " ORDER BY es.EffectiveDateFrom DESC" &
        '                            " LIMIT 1;")

        Dim n_SQLQueryToDatatable As _
            New ReadSQLProcedureToDatatable("VIEW_paystubitem_declared",
                                            orgztnID,
                                            EmployeeRowID,
                                            paypFrom,
                                            paypTo)

        Dim paystubactual As New DataTable

        paystubactual = n_SQLQueryToDatatable.ResultTable


        Dim psaItems As New DataTable

        For Each drow As DataRow In paystubactual.Rows

            psaItems = New SQLQueryToDatatable("CALL VIEW_paystubitem('" & ValNoComma(drow("RowID")) & "');").ResultTable

            Dim strdouble = ValNoComma(drow("TrueSalary")) / ValNoComma(drow("PAYFREQUENCYDIVISOR")) 'BasicPay

            'Basic Pay
            txtempbasicpay.Text = FormatNumber(ValNoComma(strdouble), 2)
            'Regular
            txthrswork.Text = ValNoComma(drow("RegularHoursWorked"))
            If drow("EmployeeType").ToString = "Fixed" Then

                txthrsworkamt.Text = FormatNumber(ValNoComma(strdouble), 2)

            ElseIf drow("EmployeeType").ToString = "Monthly" Then

                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "1" Then
                    thebasicpay = ValNoComma(drow("RegularHoursAmount"))
                Else
                    thebasicpay = ValNoComma(drow("BasicPay"))
                    thelessamounts = ValNoComma(drow("HoursLateAmount")) + ValNoComma(drow("UndertimeHoursAmount")) + ValNoComma(drow("Absent")) _
                        + ValNoComma(drow("HolidayPayment"))
                End If


                txthrsworkamt.Text = FormatNumber((thebasicpay - thelessamounts), 2)

            Else
                txthrsworkamt.Text = FormatNumber(ValNoComma((drow("RegularHoursAmount"))), 2)
            End If
            'Over time
            txttotothrs.Text = ValNoComma(drow("OvertimeHoursWorked"))
            txttototamt.Text = FormatNumber(ValNoComma((drow("OvertimeHoursAmount"))), 2)
            'Night differential
            txttotnightdiffhrs.Text = ValNoComma(drow("NightDifferentialHours"))
            txttotnightdiffamt.Text = FormatNumber(ValNoComma((drow("NightDiffHoursAmount"))), 2)
            'Night differential Overtime
            txttotnightdiffothrs.Text = ValNoComma(drow("NightDifferentialOTHours"))
            txttotnightdiffotamt.Text = FormatNumber(ValNoComma((drow("NightDiffOTHoursAmount"))), 2)
            'Holiday pay
            txttotholidayhrs.Text = 0.0
            Dim holidaybayad = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Holiday pay'"))
            txttotholidayamt.Text = FormatNumber(holidaybayad, 2)

            Dim sumallbasic = ValNoComma(txthrsworkamt.Text) _
                              + ValNoComma(drow("OvertimeHoursAmount")) _
                              + ValNoComma(drow("NightDiffHoursAmount")) _
                              + ValNoComma(drow("NightDiffOTHoursAmount")) _
                              + holidaybayad
            'lblsubtot.Text = FormatNumber((ValNoComma(sumallbasic)), 2)
            'lblsubtot.Text = FormatNumber((ValNoComma(drow("TotalDayPay"))), 2)
            If drow("EmployeeType").ToString = "Fixed" Then
                lblsubtot.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then
                'Dim thebasicpay = ValNoComma(drow("BasicPay"))
                'Dim thelessamounts = ValNoComma(drow("HoursLateAmount")) + ValNoComma(drow("UndertimeHoursAmount")) + ValNoComma(drow("Absent"))

                'txthrsworkamt.Text = FormatNumber((thebasicpay - thelessamounts), 2)

                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "1" Then
                    thebasicpay = ValNoComma(drow("RegularHoursAmount"))
                    lblsubtot.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
                Else
                    thebasicpay = ValNoComma(drow("BasicPay"))
                    thelessamounts = ValNoComma(drow("HoursLateAmount")) + ValNoComma(drow("UndertimeHoursAmount")) + ValNoComma(drow("Absent"))
                    Dim all_regular = (thebasicpay - (thelessamounts + ValNoComma(drow("HolidayPayment"))))
                    lblsubtot.Text = FormatNumber(all_regular + ValNoComma(drow("HolidayPayment")), 2)
                End If


                'lblsubtot.Text = FormatNumber((thebasicpay - thelessamounts), 2)

            Else
                lblsubtot.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)

            End If

            'Absent
            txttotabsent.Text = 0.0
            txttotabsentamt.Text = FormatNumber(ValNoComma((drow("Absent"))), 2)
            'Tardiness / late
            txttottardi.Text = ValNoComma(drow("HoursLate"))
            txttottardiamt.Text = FormatNumber(ValNoComma((drow("HoursLateAmount"))), 2)
            'Undertime
            txttotut.Text = ValNoComma(drow("UndertimeHours"))
            txttotutamt.Text = FormatNumber(ValNoComma((drow("UndertimeHoursAmount"))), 2)

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

            ''txtothrbal.Text = ValNoComma(psaItems.Compute("SUM(PayAmount)", "Item = 'Others'")) -
            ''    ValNoComma(drow("OtherLeaveHours"))

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

    Private Sub TabPage4_Click(sender As Object, e As EventArgs) Handles TabPage4.Click 'UNDECLARED

    End Sub

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
                        + ValNoComma(drow("HolidayPayment"))
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

            Dim sumallbasic = ValNoComma(txthrsworkamt_U.Text) _
                              + ValNoComma(drow("OvertimeHoursAmount")) _
                              + ValNoComma(drow("NightDiffHoursAmount")) _
                              + ValNoComma(drow("NightDiffOTHoursAmount")) _
                              + holidaybayad
            'lblsubtot.Text = FormatNumber(ValNoComma(sumallbasic), 2)
            'lblsubtot.Text = FormatNumber((ValNoComma(drow("TotalDayPay"))), 2)
            If drow("EmployeeType").ToString = "Fixed" Then
                lblsubtot.Text = FormatNumber(ValNoComma(strdouble), 2)
            ElseIf drow("EmployeeType").ToString = "Monthly" Then
                'Dim thebasicpay = ValNoComma(drow("BasicPay"))
                'Dim thelessamounts = ValNoComma(drow("HoursLateAmount")) + ValNoComma(drow("UndertimeHoursAmount")) + ValNoComma(drow("Absent"))

                'txthrsworkamt.Text = FormatNumber((thebasicpay - thelessamounts), 2)

                Dim thebasicpay = ValNoComma(drow("BasicPay"))
                Dim thelessamounts = ValNoComma(0)

                If drow("FirstTimeSalary").ToString = "1" Then
                    thebasicpay = ValNoComma(drow("RegularHoursAmount"))
                    lblsubtot.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
                Else
                    thebasicpay = ValNoComma(drow("BasicPay"))
                    thelessamounts = ValNoComma(drow("HoursLateAmount")) + ValNoComma(drow("UndertimeHoursAmount")) + ValNoComma(drow("Absent"))
                    Dim all_regular = (thebasicpay - (thelessamounts + ValNoComma(drow("HolidayPayment"))))
                    lblsubtot.Text = FormatNumber(all_regular + ValNoComma(drow("HolidayPayment")), 2)
                End If


                'lblsubtot.Text = FormatNumber((thebasicpay - thelessamounts), 2)

            Else
                lblsubtot.Text = FormatNumber(ValNoComma(drow("TotalDayPay")), 2)
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

        If dgvpayper.RowCount <> 0 And _
            dgvemployees.RowCount <> 0 Then

            Dim undeclaredSalPercent = _
            EXECQUER("SELECT `GET_employeeundeclaredsalarypercent`('" & dgvemployees.CurrentRow.Cells("RowID").Value & "'" &
                     ", '" & orgztnID & "'" &
                     ", '" & paypFrom & "'" &
                     ", '" & paypTo & "');")

            undeclaredSalPercent = ValNoComma(undeclaredSalPercent)

            Dim computed_val = Val(0)

            computed_val = ValNoComma(txtempbasicpay.Text) * undeclaredSalPercent

            txtempbasicpay_U.Text = ValNoComma(txtempbasicpay.Text) + (computed_val)
            txtempbasicpay_U.Text = FormatNumber(ValNoComma(txtempbasicpay_U.Text), 2)

            txthrswork_U.Text = txthrswork.Text

            computed_val = ValNoComma(txthrsworkamt.Text) * undeclaredSalPercent

            txthrsworkamt_U.Text = ValNoComma(txthrsworkamt.Text) + (computed_val)
            txthrsworkamt_U.Text = FormatNumber(ValNoComma(txthrsworkamt_U.Text), 2)

            txttotothrs_U.Text = txttotothrs.Text

            computed_val = ValNoComma(txttototamt.Text) * undeclaredSalPercent

            txttototamt_U.Text = ValNoComma(txttototamt.Text) + (computed_val)
            txttototamt_U.Text = FormatNumber(ValNoComma(txttototamt_U.Text), 2)

            txttotnightdiffhrs_U.Text = txttotnightdiffhrs.Text

            computed_val = ValNoComma(txttotnightdiffamt.Text) * undeclaredSalPercent

            txttotnightdiffamt_U.Text = ValNoComma(txttotnightdiffamt.Text) + (computed_val)
            txttotnightdiffamt_U.Text = FormatNumber(ValNoComma(txttotnightdiffamt_U.Text), 2)

            txttotnightdiffothrs_U.Text = txttotnightdiffothrs.Text

            computed_val = ValNoComma(txttotnightdiffotamt.Text) * undeclaredSalPercent

            txttotnightdiffotamt_U.Text = ValNoComma(txttotnightdiffotamt.Text) + (computed_val)
            txttotnightdiffotamt_U.Text = FormatNumber(ValNoComma(txttotnightdiffotamt_U.Text), 2)

            txttotholidayhrs_U.Text = txttotholidayhrs.Text

            computed_val = ValNoComma(txttotholidayamt.Text) * undeclaredSalPercent

            txttotholidayamt_U.Text = ValNoComma(txttotholidayamt.Text) + (computed_val)
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


                keep_declaredvalues(0) = ValNoComma(lblsubtot.Text)

                keep_declaredvalues(1) = ValNoComma(lblsubtotmisc.Text)

                keep_declaredvalues(2) = ValNoComma(txtgrosssal.Text)

                keep_declaredvalues(3) = _
                EXECQUER("SELECT GET_paystubitemallowanceecola('" & orgztnID & "'" &
                         ", '" & dgvemployees.CurrentRow.Cells("RowID").Value & "'" &
                         ", '" & dgvpayper.CurrentRow.Cells("Column1").Value & "');")

                keep_declaredvalues(2) -= ValNoComma(keep_declaredvalues(3))


                keep_declaredvalues(4) = ValNoComma(txtnetsal.Text)

                keep_declaredvalues(5) = ValNoComma(txttaxabsal.Text)

            End If



            computed_val = ValNoComma(keep_declaredvalues(0)) * undeclaredSalPercent

            lblsubtot.Text = ValNoComma(keep_declaredvalues(0)) + (computed_val)
            lblsubtot.Text = FormatNumber(ValNoComma(lblsubtot.Text), 2)



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

            txtempbasicpay_U.Text = txtempbasicpay.Text

            txthrswork_U.Text = txthrswork_U.Text
            txthrsworkamt_U.Text = txthrsworkamt.Text

            txttotothrs_U.Text = txttotothrs.Text
            txttototamt_U.Text = txttototamt.Text

            txttotnightdiffhrs_U.Text = txttotnightdiffhrs.Text
            txttotnightdiffamt_U.Text = txttotnightdiffamt.Text

            txttotnightdiffothrs_U.Text = txttotnightdiffothrs.Text
            txttotnightdiffotamt_U.Text = txttotnightdiffotamt.Text

            txttotholidayhrs_U.Text = txttotholidayhrs.Text
            txttotholidayamt_U.Text = txttotholidayamt.Text

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

    Dim dtprintAllPaySlip As New DataTable

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
                                      " WHERE e.OrganizationID=" & orgztnID & _
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

    Private Sub bgwPrintAllPaySlip_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwPrintAllPaySlip.ProgressChanged

    End Sub

    Dim rptdocAll As New rptAllDecUndecPaySlip

    Private Sub bgwPrintAllPaySlip_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwPrintAllPaySlip.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox("Error: " & vbNewLine & e.Error.Message)

        ElseIf e.Cancelled Then
            MsgBox("Background work cancelled.", _
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

    Private Sub ActualToolStripMenuItem_Click(sender As Object, e As EventArgs) 'Handles ActualToolStripMenuItem.Click

        tsbtnprintpayslip.Enabled = False

        If paypRowID <> Nothing Then

            If dgvemployees.RowCount > 0 Then

                Dim n_PrintSoloPaySlipThisPayPeriod As _
                    New PrintSoloPaySlipThisPayPeriod(paypFrom, paypTo,
                                                      dgvemployees.CurrentRow.Cells("RowID").Value, 1)

            End If

        End If

        tsbtnprintpayslip.Enabled = True

        '## ############################## ##

        Dim papy_str As String = Nothing

        If papy_str = Nothing Then 'If paypRowID = Nothing Then

            Exit Sub

        End If

        Dim undeclaredpercent = ValNoComma(EXECQUER("SELECT `GET_employeeundeclaredsalarypercent`('" & dgvemployees.CurrentRow.Cells("RowID").Value & "'" &
                                                    ", '" & orgztnID & "'" &
                                                    ", '" & paypFrom & "'" &
                                                    ", '" & paypTo & "');"))

        Try

            Dim rptdoc As New HalfPaySlip 'prntPaySlip

            With rptdoc.ReportDefinition.Sections(2)

                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("txtempbasicpay")
                objText.Text = " ₱ " & txtempbasicpay.Text

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
                objText.Text = txthrswork.Text

                Dim objval = Nothing

                objText = .ReportObjects("txttotregamt")
                objval = ValNoComma(txttotregamt.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("txttotothrs")
                objText.Text = txttotothrs.Text

                objText = .ReportObjects("txttototamt")
                objval = ValNoComma(txttototamt.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("txttotnightdiffhrs")
                objText.Text = txttotnightdiffhrs.Text

                objText = .ReportObjects("txttotnightdiffamt")
                objval = ValNoComma(txttotnightdiffamt.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("txttotnightdiffothrs")
                objText.Text = txttotnightdiffothrs.Text

                objText = .ReportObjects("txttotnightdiffotamt")
                objval = ValNoComma(txttotnightdiffotamt.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("txttotholidayhrs")
                objText.Text = txttotholidayhrs.Text

                objText = .ReportObjects("txttotholidayamt")
                objval = ValNoComma(txttotholidayamt.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("txthrswork")
                objText.Text = txttotreghrs.Text

                objText = .ReportObjects("txthrsworkamt")
                objval = ValNoComma(txthrsworkamt.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("lblsubtot")
                objval = ValNoComma(lblsubtot.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("txtemptotallow")
                objText.Text = "₱ " & txtemptotallow.Text

                objText = .ReportObjects("txtgrosssal")
                objval = ValNoComma(txtgrosssal.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

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
                objval = ValNoComma(txttotabsentamt.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("txttottardi")
                objText.Text = txttottardi.Text

                objText = .ReportObjects("txttottardiamt")
                objval = ValNoComma(txttottardiamt.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("txttotut")
                objText.Text = txttotut.Text

                objText = .ReportObjects("txttotutamt")
                objval = ValNoComma(txttotutamt.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                Dim misc_subtot = Val(txttottardiamt.Text.Replace(",", "")) + Val(txttotutamt.Text.Replace(",", ""))

                objText = .ReportObjects("lblsubtotmisc")
                objval = ValNoComma(misc_subtot)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2) '.ToString.Replace(",", "")

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
                objval = ValNoComma(txtempwtax.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("txtnetsal")
                objval = ValNoComma(txtnetsal.Text)
                objval = objval + (objval * undeclaredpercent)
                objText.Text = "₱ " & FormatNumber(ValNoComma(objval), 2)

                objText = .ReportObjects("allowsubdetails")

                If dgvemployees.RowCount <> 0 Then

                    VIEW_eallow_indate(dgvemployees.CurrentRow.Cells("RowID").Value, _
                                        paypFrom, _
                                        paypTo)

                    VIEW_eloan_indate(dgvemployees.CurrentRow.Cells("RowID").Value, _
                                        paypFrom, _
                                        paypTo)

                    VIEW_ebon_indate(dgvemployees.CurrentRow.Cells("RowID").Value, _
                                        paypFrom, _
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

    Private Sub ActualToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ActualToolStripMenuItem2.Click

    End Sub

    Private Sub ActualToolStripMenuItem1_Click(sender As Object, e As EventArgs) 'Handles ActualToolStripMenuItem1.Click

        Dim sssProductID = Nothing

        Dim phhProductID = Nothing

        Dim hdmfProductID = Nothing

        Dim dtprod = retAsDatTbl("SELECT p.RowID" &
                                 ",p1.RowID AS p1RowID" &
                                 ",p2.RowID AS p2RowID" &
                                 " FROM product p" &
                                 " INNER JOIN product p1 ON p1.OrganizationID='" & orgztnID & "' AND p1.PartNo='.PhilHealth'" &
                                 " INNER JOIN product p2 ON p2.OrganizationID='" & orgztnID & "' AND p2.PartNo='.PAGIBIG'" &
                                 " WHERE p.OrganizationID='" & orgztnID & "'" &
                                 " AND p.PartNo='.SSS';")

        For Each drrow As DataRow In dtprod.Rows

            sssProductID = drrow("RowID")

            phhProductID = drrow("p1RowID")

            hdmfProductID = drrow("p2RowID")

        Next

        ''RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        If paypRowID = Nothing Then

            ''AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

            Exit Sub

        End If

        Dim pay_stbitem As New DataTable 'this is for leave balances

        pay_stbitem = retAsDatTbl("SELECT" &
                                  " pi.PayStubID" &
                                  ",pi.ProductID" &
                                  ",p.PartNo" &
                                  ",pi.PayAmount" &
                                  " FROM paystubitem pi" &
                                  " LEFT JOIN product p ON p.RowID = pi.ProductID" &
                                  " LEFT JOIN paystub ps ON ps.RowID = pi.PayStubID" &
                                  " WHERE p.Category='Leave Type'" &
                                  " AND p.OrganizationID=" & orgztnID & _
                                  " AND ps.PayPeriodID='" & paypRowID & "';") 'this is for leave balances

        Dim rptdattab As New DataTable

        With rptdattab.Columns

            .Add("Column1", Type.GetType("System.Int32"))
            .Add("Column2", Type.GetType("System.String"))
            .Add("Column3") 'Employee Full Name

            .Add("Column4") 'Gross Income

            .Add("Column5") 'Net Income
            .Add("Column6") 'Taxable salary

            .Add("Column7") 'Withholding Tax
            .Add("Column8") 'Total Allowance

            .Add("Column9") 'Total Loans
            .Add("Column10") 'Total Bonuses

            .Add("Column11") 'Basic Pay
            .Add("Column12") 'SSS Amount

            .Add("Column13") 'PhilHealth Amount
            .Add("Column14") 'PAGIBIG Amount

            .Add("Column15") 'Sub Total - Right side
            .Add("Column16") 'txthrsworkamt

            .Add("Column17") 'Regular hours worked

            .Add("Column18") 'Regular hours amount

            .Add("Column19") 'Overtime hours worked

            .Add("Column20") 'Overtime hours amount
            .Add("Column21") 'Night differential hours worked
            .Add("Column22") 'Night differential hours amount

            .Add("Column23") 'Night differential OT hours worked
            .Add("Column24") 'Night differential OT hours amount

            .Add("Column25") 'Total hours worked

            .Add("Column26") 'Undertime hours

            .Add("Column27") 'Undertime amount
            .Add("Column28") 'Late hours

            .Add("Column29") 'Late amount

            .Add("Column30") 'Leave type
            .Add("Column31") 'Leave count
            .Add("Column32")

            .Add("Column33")

            .Add("Column34") 'Allowance type
            .Add("Column35") 'Loan type
            .Add("Column36") 'Bonus type





            .Add("Column37") 'Allowance amount
            .Add("Column38") 'Loan amount
            .Add("Column39") 'Bonus amount

        End With

        employee_dattab = retAsDatTbl("SELECT e.*" &
                                      ",ps.RowID as psRowID" &
                                      " FROM employee e LEFT JOIN employeesalary esal ON e.RowID=esal.EmployeeID" &
                                      " LEFT JOIN paystub ps ON ps.EmployeeID=e.RowID AND ps.OrganizationID=e.OrganizationID AND ps.PayPeriodID='" & paypRowID & "'" &
                                      " WHERE e.OrganizationID=" & orgztnID & _
                                      " AND e.PayFrequencyID='" & paypPayFreqID & "'" &
                                      " AND '" & paypTo & "' BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,'" & paypTo & "')" &
                                      " GROUP BY e.RowID" &
                                      " ORDER BY e.LastName;") 'RowID DESC

        Dim employeeloanfulldetails As New DataTable

        employeeloanfulldetails = retAsDatTbl("CALL employeeloanfulldetails('" & orgztnID & "'" &
                                              ",'" & paypPayFreqID & "'" &
                                              ",'" & paypFrom & "'" &
                                              ",'" & paypTo & "');")

        Dim newdatrow As DataRow

        For Each drow As DataRow In employee_dattab.Rows
            newdatrow = rptdattab.NewRow

            newdatrow("Column1") = drow("RowID") 'Employee RowID
            newdatrow("Column2") = drow("EmployeeID") 'Employee ID

            Dim LasFirMidName As String = Nothing


            Dim midNameTwoWords = Split(drow("MiddleName").ToString, " ")

            Dim midnameinitial As String = Nothing

            For Each strmidname In midNameTwoWords

                midnameinitial &= (StrConv(Microsoft.VisualBasic.Left(strmidname, 1), VbStrConv.ProperCase) & ".")

            Next

            midnameinitial = If(Trim(midnameinitial) = Nothing, "", _
                                If(Trim(midnameinitial) = ".", "", ", " & midnameinitial))

            LasFirMidName = drow("LastName").ToString & ", " & drow("FirstName").ToString & midnameinitial

            LasFirMidName = StrConv(LasFirMidName, VbStrConv.Uppercase)

            Dim full_name = drow("FirstName").ToString & If(drow("MiddleName").ToString = Nothing, _
                                                        "", _
                                                        " " & StrConv(Microsoft.VisualBasic.Left(drow("MiddleName").ToString, 1), _
                                                        VbStrConv.ProperCase) & ".")

            full_name = full_name & " " & drow("LastName").ToString

            full_name = full_name & If(drow("Surname").ToString = Nothing, _
                                    "", _
                                    "-" & StrConv(Microsoft.VisualBasic.Left(drow("Surname").ToString, 1), _
                                    VbStrConv.ProperCase))

            newdatrow("Column3") = LasFirMidName 'full_name 'Employee Full Name


            VIEW_paystub(drow("RowID").ToString, _
                                 paypRowID)

            Dim undeclared_psi As New DataTable

            Dim declared_psi As New DataTable

            If IsDBNull(drow("RowID")) Then

            Else

                Dim its_value = drow("psRowID").ToString

                If its_value <> "" Then

                    undeclared_psi = retAsDatTbl("CALL `VIEW_paystubitemundeclared`('" & drow("psRowID") & "');")

                    declared_psi = retAsDatTbl("CALL `VIEW_paystubitem`('" & drow("psRowID") & "');")

                End If

            End If

            Dim totamountallow = 0.0
            Dim totamountbon = 0.0

            For Each dgvrow As DataGridViewRow In dgvpaystub.Rows
                With dgvrow

                    Dim val_gross = If(undeclared_psi.Rows.Count = 0, 0, ValNoComma(undeclared_psi.Compute("SUM(PayAmount)", "Item = 'Gross Income'")))

                    newdatrow("Column4") = "₱ " & FormatNumber(ValNoComma(val_gross), 2) 'Gross Income

                    Dim gros_inc = Val(newdatrow("Column4"))


                    Dim val_netincome = If(undeclared_psi.Rows.Count = 0, 0, ValNoComma(undeclared_psi.Compute("SUM(PayAmount)", "Item = 'Net Income'")))

                    newdatrow("Column5") = "₱ " & FormatNumber(ValNoComma(val_netincome), 2) 'Net Income


                    'Dim val_taxableincome = If(undeclared_psi.Rows.Count = 0, 0, ValNoComma(undeclared_psi.Compute("SUM(PayAmount)", "Item = 'Taxable Income'")))

                    'newdatrow("Column6") = "₱ " & FormatNumber(ValNoComma(val_taxableincome), 2) 'Taxable salary

                    newdatrow("Column6") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalTaxableSalary").Value), 2) 'Taxable salary


                    Dim val_wtax = If(undeclared_psi.Rows.Count = 0, 0, ValNoComma(undeclared_psi.Compute("SUM(PayAmount)", "Item = 'Withholding Tax'")))

                    newdatrow("Column7") = "₱ " & FormatNumber(ValNoComma(val_wtax), 2) 'Withholding Tax

                    'newdatrow("Column3") = .Cells("paystb_TotalEmpSSS").Value
                    'newdatrow("Column3") = .Cells("paystb_TotalEmpPhilhealth").Value
                    'newdatrow("Column3") = .Cells("paystb_TotalEmpHDMF").Value

                    newdatrow("Column8") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalAllowance").Value), 2) 'Total Allowance

                    totamountallow = Val(.Cells("paystb_TotalAllowance").Value)

                    'txtholidaypay.Text = .Cells("paystb_TotalAllowance").Value

                    'lblsubtotmisc.Text = .Cells("paystb_TotalAllowance").Value

                    newdatrow("Column9") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalLoans").Value), 2) 'Total Loans

                    newdatrow("Column10") = "₱ " & FormatNumber(Val(.Cells("paystb_TotalBonus").Value), 2) 'Total Bonuses

                    totamountbon = Val(.Cells("paystb_TotalBonus").Value)

                    'txtvacleft.Text = .Cells("paystb_TotalVacationDaysLeft").Value

                    'VIEW_paystubitem(.Cells("paystb_RowID").Value)

                    'paystb_RowID

                    Dim selpay_stbitem = pay_stbitem.Select("PayStubID = " & .Cells("paystb_RowID").Value)

                    Dim firstRow = 0

                    Dim isStrListed As New List(Of String)

                    For Each datrow In selpay_stbitem 'this is for leave balances

                        '.Add("Column30") 'Leave type
                        '.Add("Column31") 'Leave count
                        Dim leavebalance = Val(datrow("PayAmount"))

                        If firstRow = 0 Then
                            If isStrListed.Contains(datrow("PartNo")) Then
                            Else
                                newdatrow("Column30") = datrow("PartNo")

                                newdatrow("Column31") = Val(datrow("PayAmount"))

                                isStrListed.Add(datrow("PartNo"))
                            End If
                        Else
                            If isStrListed.Contains(datrow("PartNo")) Then
                            Else
                                newdatrow("Column30") &= vbNewLine & datrow("PartNo")

                                newdatrow("Column31") &= vbNewLine & Val(datrow("PayAmount"))

                                isStrListed.Add(datrow("PartNo"))
                            End If
                        End If

                        firstRow += 1

                    Next 'this is for leave balances

                    isStrListed.Clear()

                End With

                Exit For

            Next

            VIEW_specificemployeesalary(drow("RowID").ToString, _
                                        paypTo)

            Dim theEmpBasicPayFix = 0.0

            Dim undeclaredpercent = ValNoComma(EXECQUER("SELECT `GET_employeeundeclaredsalarypercent`('" & drow("RowID") & "'" &
                                                        ", '" & orgztnID & "'" &
                                                        ", '" & paypFrom & "'" &
                                                        ", '" & paypTo & "');"))

            For Each dgvrow As DataGridViewRow In dgvempsal.Rows
                With dgvrow

                    Dim thebasicpay = ValNoComma(.Cells("esal_BasicPay").Value)

                    thebasicpay = thebasicpay + (thebasicpay * undeclaredpercent)

                    newdatrow("Column11") = "₱ " & FormatNumber(thebasicpay, 2) 'Basic Pay

                    theEmpBasicPayFix = ValNoComma(.Cells("esal_BasicPay").Value) 'Basic Pay

                    theEmpBasicPayFix = theEmpBasicPayFix + (theEmpBasicPayFix * undeclaredpercent)

                    Dim val_sss = If(declared_psi.Rows.Count = 0, 0, ValNoComma(declared_psi.Compute("SUM(PayAmount)", "ProductID = '" & sssProductID & "'")))

                    Dim val_phh = If(declared_psi.Rows.Count = 0, 0, ValNoComma(declared_psi.Compute("SUM(PayAmount)", "ProductID = '" & phhProductID & "'")))

                    Dim val_hdmf = If(declared_psi.Rows.Count = 0, 0, ValNoComma(declared_psi.Compute("SUM(PayAmount)", "ProductID = '" & hdmfProductID & "'")))

                    If isorgSSSdeductsched = 1 Then
                        newdatrow("Column12") = "₱ " & FormatNumber((val_sss / 2), 2)

                    Else
                        If isEndOfMonth = "0" Then
                            newdatrow("Column12") = "₱ " & FormatNumber(val_sss, 2) 'SSS Amount
                        Else
                            newdatrow("Column12") = "₱ " & "0.00" 'SSS Amount
                        End If
                        'SSS Amount

                    End If

                    If isorgPHHdeductsched = 1 Then
                        newdatrow("Column13") = "₱ " & FormatNumber((val_phh / 2), 2) 'PhilHealth Amount

                    Else
                        If isEndOfMonth = "0" Then
                            newdatrow("Column13") = "₱ " & FormatNumber(val_phh, 2) 'PhilHealth Amount
                        Else
                            newdatrow("Column13") = "₱ " & "0.00" 'PhilHealth Amount
                        End If

                    End If

                    If isorgHDMFdeductsched = 1 Then
                        newdatrow("Column14") = "₱ " & FormatNumber((val_hdmf / 2), 2) 'PAGIBIG Amount

                    Else
                        If isEndOfMonth = "0" Then
                            newdatrow("Column14") = "₱ " & FormatNumber((val_hdmf), 2) 'PAGIBIG Amount
                        Else
                            newdatrow("Column14") = "₱ " & "0.00" 'PAGIBIG Amount
                        End If

                    End If

                    Exit For

                End With

            Next

            VIEW_employeetimeentry_SUM(drow("RowID").ToString, _
                                       paypFrom, _
                                       paypTo)

            For Each dgvrow As DataGridViewRow In dgvetent.Rows
                With dgvrow

                    If drow("EmployeeType").ToString = "Fixed" Then

                        Dim validgrossinc = Val(newdatrow("Column4"))

                        If Val(.Cells("etent_TotalDayPay").Value) = 0 Then
                            If drow("PayFrequencyID").ToString = 1 Then
                                ''------------------------------------------------ITO UNG BASIC PAY

                                'newdatrow("Column4") = "₱ " & FormatNumber(Val(newdatrow("Column11")) + _
                                '(.Cells("etent_OvertimeHoursAmount").Value) + _
                                '(.Cells("etent_NightDiffOTHoursAmount").Value), _
                                '2)

                                newdatrow("Column4") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") '.ToString.Replace(",", "") 'Gross Income

                                newdatrow("Column15") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'Sub Total - Right side

                                newdatrow("Column16") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'txthrsworkamt
                            Else
                                Dim totbasicpay = ValNoComma(totamountallow) + _
                                ValNoComma(.Cells("etent_OvertimeHoursAmount").Value) + _
                                ValNoComma(.Cells("etent_NightDiffOTHoursAmount").Value)

                                totbasicpay = totbasicpay + (totbasicpay * undeclaredpercent)

                                newdatrow("Column4") = "₱ " & FormatNumber(totbasicpay, 2) 'newdatrow("Column4") '.ToString.Replace(",", "") 'Gross Income

                                newdatrow("Column16") = "₱ " & FormatNumber(totbasicpay, 2) 'newdatrow("Column4") 'txthrsworkamt
                            End If

                        Else

                            'newdatrow("Column4") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") '.ToString.Replace(",", "") 'Gross Income

                            newdatrow("Column15") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'Sub Total - Right side

                            newdatrow("Column16") = "₱ " & FormatNumber(theEmpBasicPayFix, 2) 'newdatrow("Column4") 'txthrsworkamt

                        End If

                    Else

                        Dim sumtotdaypay = ValNoComma(.Cells("etent_TotalDayPay").Value)

                        sumtotdaypay = sumtotdaypay + (sumtotdaypay * undeclaredpercent)

                        newdatrow("Column15") = "₱ " & FormatNumber(sumtotdaypay, 2) 'Sub Total - Right side

                        newdatrow("Column16") = "₱ " & FormatNumber(sumtotdaypay, 2) 'txthrsworkamt

                    End If

                    Dim regular_hours_worked = Val(0)

                    regular_hours_worked = ValNoComma(.Cells("etent_TotalHoursWorked").Value) _
                                           - ValNoComma(.Cells("etent_OvertimeHoursWorked").Value) _
                                           - ValNoComma(.Cells("etent_TotalHoursWorked").Value)

                    newdatrow("Column17") = "" 'FormatNumber(Val(.Cells("etent_TotalHoursWorked").Value), 2) 'Regular hours worked

                    Dim et_RegHrsAmt = ValNoComma(.Cells("etent_RegularHoursAmount").Value)

                    et_RegHrsAmt = et_RegHrsAmt + (et_RegHrsAmt * undeclaredpercent)

                    newdatrow("Column18") = "₱ " & FormatNumber(Val(et_RegHrsAmt), 2) 'Regular hours amount

                    newdatrow("Column19") = "" 'FormatNumber(Val(.Cells("etent_OvertimeHoursWorked").Value), 2) 'Overtime hours worked
                    Dim val_ot = If(undeclared_psi.Rows.Count = 0, 0, ValNoComma(undeclared_psi.Compute("SUM(PayAmount)", "Item = 'Overtime'")))
                    newdatrow("Column20") = "₱ " & FormatNumber(ValNoComma(val_ot), 2) 'Overtime hours amount

                    newdatrow("Column21") = "" 'FormatNumber(Val(.Cells("etent_NightDifferentialHours").Value), 2) 'Night differential hours worked
                    Dim val_ndiff = If(undeclared_psi.Rows.Count = 0, 0, ValNoComma(undeclared_psi.Compute("SUM(PayAmount)", "Item = 'Night differential'")))
                    newdatrow("Column22") = "₱ " & FormatNumber(ValNoComma(val_ndiff), 2) 'Night differential hours amount

                    newdatrow("Column23") = "" 'FormatNumber(Val(.Cells("etent_NightDifferentialOTHours").Value), 2) 'Night differential OT hours worked
                    Dim val_ndiffot = If(undeclared_psi.Rows.Count = 0, 0, ValNoComma(undeclared_psi.Compute("SUM(PayAmount)", "Item = 'Night differential OT'")))
                    newdatrow("Column24") = "₱ " & FormatNumber(ValNoComma(val_ndiffot), 2) 'Night differential OT hours amount

                    'txttotholidayhrs.Text = .Cells("esal_BasicPay").Value
                    'txttotholidayamt.Text = .Cells("esal_BasicPay").Value

                    newdatrow("Column25") = "" 'FormatNumber(Val(.Cells("etent_TotalHoursWorked").Value), 2) 'Total hours worked

                    Dim strtab = "					"

                    'newdatrow("Column26") = "₱ " & FormatNumber(Val(.Cells("etent_UndertimeHours").Value), 2) 'Undertime hours
                    'newdatrow("Column27") = "₱ " & FormatNumber(Val(.Cells("etent_UndertimeHoursAmount").Value), 2) 'Undertime amount

                    '*******************************************

                    Dim str_length = 0

                    'str_length = ("₱ " & FormatNumber(ValNoComma(.Cells("etent_HoursLateAmount").Value), 2)).ToString.Length 'Absent

                    Dim val_absent = If(undeclared_psi.Rows.Count = 0, 0, ValNoComma(undeclared_psi.Compute("SUM(PayAmount)", "Item = 'Absent'")))
                    newdatrow("Column26") = "₱ " & FormatNumber(val_absent, 2)


                    str_length = ("₱ " & FormatNumber(ValNoComma(.Cells("etent_HoursLateAmount").Value), 2)).ToString.Length 'Tardiness

                    Dim val_tardi = ValNoComma(.Cells("etent_HoursLateAmount").Value) + (ValNoComma(.Cells("etent_HoursLateAmount").Value) * undeclaredpercent)

                    'ValNoComma(.Cells("etent_HoursLate").Value) & Space(21 - str_length) & _
                    newdatrow("Column27") = "₱ " & FormatNumber(val_tardi, 2) 'Tardiness

                    str_length = ("₱ " & FormatNumber(ValNoComma(.Cells("etent_UndertimeHoursAmount").Value), 2)).ToString.Length 'Undertime

                    'ValNoComma(.Cells("etent_UndertimeHours").Value) & Space(21 - str_length) & _
                    newdatrow("Column28") = "₱ " & FormatNumber(ValNoComma(.Cells("etent_UndertimeHoursAmount").Value), 2) 'Undertime

                    '*******************************************

                    txttotabsent.Text = COUNT_employeeabsent(drow("RowID").ToString, _
                                                             drow("StartDate").ToString, _
                                                             paypFrom, _
                                                             paypTo)

                    'Dim param_date = If(paypTo = Nothing, paypFrom, paypTo)

                    'Dim rateper_hour = GET_employeerateperhour(.Cells("RowID").Value, param_date)

                    'newdatrow("Column28") = "₱ " & FormatNumber(Val(.Cells("etent_HoursLate").Value), 2)
                    'newdatrow("Column29") = "₱ " & FormatNumber(Val(.Cells("etent_HoursLateAmount").Value), 2)

                    Dim misc_subtot = ValNoComma(newdatrow("Column29")) + ValNoComma(newdatrow("Column27"))

                    Exit For
                End With
            Next



            VIEW_eallow_indate(drow("RowID"), _
                                paypFrom, _
                                paypTo)

            VIEW_eloan_indate(drow("RowID"), _
                                paypFrom, _
                                paypTo)

            VIEW_ebon_indate(drow("RowID"), _
                                paypFrom, _
                                paypTo)



            For Each dgvrow As DataGridViewRow In dgvempallowance.Rows 'Allowances
                If dgvrow.Index = 0 Then
                    newdatrow("Column34") = dgvrow.Cells("eall_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                    newdatrow("Column37") = "₱ " & FormatNumber(Val(dgvrow.Cells("eall_Amount").Value), 2)
                Else
                    newdatrow("Column34") &= vbNewLine & dgvrow.Cells("eall_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                    newdatrow("Column37") &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("eall_Amount").Value), 2)

                    'Dim strtxt = dgvrow.Cells("eall_Type").Value & vbTab & "₱ " & dgvrow.Cells("eall_Amount").Value

                End If
            Next

            'objText.Text &= vbNewLine
            'allowvalues.Text &= vbNewLine

            Dim totalamountofloan = Val(0)

            'For Each dgvrow As DataGridViewRow In dgvLoanList.Rows 'Loans


            '    If dgvrow.Index = 0 Then
            '        newdatrow("Column35") = dgvrow.Cells("c_loantype").Value & " loan" ' & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

            '        newdatrow("Column38") = "₱ " & FormatNumber(Val(dgvrow.Cells("c_dedamt").Value), 2)

            '        newdatrow("Column10") = "₱ " & FormatNumber(Val(dgvrow.Cells("c_totballeft").Value), 2)

            '        totalamountofloan += ValNoComma(dgvrow.Cells("c_dedamt").Value)

            '    Else
            '        newdatrow("Column35") &= vbNewLine & dgvrow.Cells("c_loantype").Value & " loan" ' & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

            '        newdatrow("Column38") &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("c_dedamt").Value), 2)

            '        newdatrow("Column10") &= "₱ " & FormatNumber(Val(dgvrow.Cells("c_totballeft").Value), 2)

            '        totalamountofloan += ValNoComma(dgvrow.Cells("c_dedamt").Value)

            '        'Dim strtxt = dgvrow.Cells("c_loantype").Value & vbTab & "₱ " & dgvrow.Cells("c_dedamt").Value

            '    End If
            'Next

            Dim first_indx = 0

            Dim sel_employeeloanfulldetails = employeeloanfulldetails.Select("EmpRowID = '" & drow("RowID") & "'")

            For Each loanrow As DataRow In sel_employeeloanfulldetails

                If first_indx = 0 Then

                    newdatrow("Column35") = loanrow("PartNo").ToString & " loan"

                    newdatrow("Column38") = "₱ " & FormatNumber(ValNoComma(loanrow("DeductionAmount")), 2)

                    newdatrow("Column33") = "₱ " & FormatNumber(ValNoComma(loanrow("CurrentBalance")), 2)

                    totalamountofloan += ValNoComma(loanrow("DeductionAmount"))

                Else

                    newdatrow("Column35") &= vbNewLine & loanrow("PartNo").ToString & " loan" '& vbTab & " - " & dgvrow.Cells("c_totloanamt").Value _
                    '                                                                     '& vbTab & "/" & dgvrow.Cells("c_totballeft").Value

                    newdatrow("Column38") &= vbNewLine & "₱ " & FormatNumber(ValNoComma(loanrow("DeductionAmount")), 2)

                    newdatrow("Column33") &= vbNewLine & "₱ " & FormatNumber(ValNoComma(loanrow("CurrentBalance")), 2)

                    totalamountofloan += ValNoComma(loanrow("DeductionAmount"))

                End If

                first_indx += 1

            Next

            newdatrow("Column9") = "₱ " & FormatNumber(totalamountofloan, 2)

            'objText.Text &= vbNewLine
            'loanvalues.Text &= vbNewLine

            ''dgvempbon'bonsubdetails

            For Each dgvrow As DataGridViewRow In dgvempbon.Rows 'Bonuses
                If dgvrow.Index = 0 Then
                    newdatrow("Column36") = dgvrow.Cells("bon_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                    newdatrow("Column39") = "₱ " & FormatNumber(Val(dgvrow.Cells("bon_Amount").Value), 2)
                Else
                    newdatrow("Column36") &= vbNewLine & dgvrow.Cells("bon_Type").Value ' & vbTab & "₱ " & dgvrow.Cells("bon_Amount").Value

                    newdatrow("Column39") &= vbNewLine & "₱ " & FormatNumber(Val(dgvrow.Cells("bon_Amount").Value), 2)

                End If
            Next

            'objText.Text &= vbNewLine
            'bonvalues.Text &= vbNewLine

            rptdattab.Rows.Add(newdatrow)

        Next

        'For Each dgvrow As DataGridViewRow In dgvemployees.Rows
        '    newdatrow = rptdattab.NewRow

        '    dgvrow.Selected = 1
        '    'dgvemployees_SelectionChanged(sender, e)

        '    With dgvrow
        '        newdatrow("Column1") = .Cells("RowID").Value
        '        newdatrow("Column2") = .Cells("EmployeeID").Value

        '        Dim full_name = .Cells("FirstName").Value & If(.Cells("MiddleName").Value = Nothing, _
        '                                                 "", _
        '                                                 " " & StrConv(Microsoft.VisualBasic.Left(.Cells("MiddleName").Value.ToString, 1), _
        '                                                                                                VbStrConv.ProperCase) & ".")
        '        full_name = full_name & " " & .Cells("LastName").Value

        '        full_name = full_name & If(.Cells("Surname").Value = Nothing, _
        '                                                 "", _
        '                                                 "-" & StrConv(Microsoft.VisualBasic.Left(.Cells("Surname").Value.ToString, 1), _
        '                                                                                                VbStrConv.ProperCase))

        '        newdatrow("Column3") = full_name

        '        'txtempbasicpay.Text

        '        'txttotreghrs.Text
        '        'txttotregamt.Text

        '        'txttotothrs.Text
        '        'txttototamt.Text

        '        'txttotnightdiffhrs.Text
        '        'txttotnightdiffamt.Text

        '        'txttotnightdiffothrs.Text
        '        'txttotnightdiffotamt.Text

        '        'txttotholidayhrs.Text
        '        'txttotholidayamt.Text

        '        'txthrswork.Text
        '        'txthrsworkamt.Text

        '        'lblsubtot.Text

        '        'txtemptotallow.Text

        '        'txtgrosssal.Text

        '        'txtvlbal.Text
        '        'txtslbal.Text
        '        'txtmlbal.Text

        '        'txttotabsent.Text
        '        'txttotabsentamt.Text

        '        'txttottardi.Text
        '        'txttottardiamt.Text

        '        'txttotut.Text
        '        'txttotutamt.Text

        '        'lblsubtotmisc.Text

        '        'txtempsss.Text
        '        'txtempphh.Text
        '        'txtemphdmf.Text

        '        'txtemptotloan.Text
        '        'txtemptotbon.Text

        '        'txttaxabsal.Text
        '        'txtempwtax.Text
        '        'txtnetsal.Text

        '    End With

        '    rptdattab.Rows.Add(newdatrow)

        'Next

        Dim rptdoc As New printallpayslipotherformat 'prntAllPaySlip

        With rptdoc.ReportDefinition.Sections(2)
            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("OrgName1")

            objText.Text = orgNam

            objText = .ReportObjects("OrgName")

            objText.Text = orgNam

            objText = .ReportObjects("OrgAddress1")

            Dim orgaddress = EXECQUER("SELECT CONCAT(IF(IFNULL(StreetAddress1,'')='','',StreetAddress1)" &
                                    ",IF(IFNULL(StreetAddress2,'')='','',CONCAT(', ',StreetAddress2))" &
                                    ",IF(IFNULL(Barangay,'')='','',CONCAT(', ',Barangay))" &
                                    ",IF(IFNULL(CityTown,'')='','',CONCAT(', ',CityTown))" &
                                    ",IF(IFNULL(Country,'')='','',CONCAT(', ',Country))" &
                                    ",IF(IFNULL(State,'')='','',CONCAT(', ',State)))" &
                                    " FROM address a LEFT JOIN organization o ON o.PrimaryAddressID=a.RowID" &
                                    " WHERE o.RowID=" & orgztnID & ";")

            objText.Text = orgaddress

            objText = .ReportObjects("OrgAddress")

            objText.Text = orgaddress

            Dim contactdetails = EXECQUER("SELECT GROUP_CONCAT(COALESCE(MainPhone,'')" &
                                    ",',',COALESCE(FaxNumber,'')" &
                                    ",',',COALESCE(EmailAddress,'')" &
                                    ",',',COALESCE(TINNo,''))" &
                                    " FROM organization WHERE RowID=" & orgztnID & ";")

            Dim contactdet = Split(contactdetails, ",")

            objText = .ReportObjects("OrgContact1")

            Dim contactdets As String = String.Empty

            'If Trim(contactdet(0).ToString) = "" Then
            '    contactdets = ""
            'Else
            '    contactdets = "Contact No. " & contactdet(0).ToString
            'End If

            objText.Text = contactdets

            objText = .ReportObjects("OrgContact")

            objText.Text = contactdets

            objText = .ReportObjects("payperiod1")

            Dim papy_str = "Payroll slip for the period of   " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))

            objText.Text = papy_str

            objText = .ReportObjects("payperiod")

            objText.Text = papy_str

        End With

        rptdoc.SetDataSource(rptdattab)

        Dim crvwr As New CrysVwr
        crvwr.CrystalReportViewer1.ReportSource = rptdoc

        Dim papy_string = "Print all pay slip for the period of " & Format(CDate(paypFrom), machineShortDateFormat) & If(paypTo = Nothing, "", " to " & Format(CDate(paypTo), machineShortDateFormat))

        crvwr.Text = papy_string
        crvwr.Refresh()
        crvwr.Show()

        employeeloanfulldetails.Dispose()

        'rptdattab = Nothing
        'rptdattab.Dispose()

        ''AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        'dgvemployees_SelectionChanged(sender, e)

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

    Dim payWTax As New DataTable

    Dim filingStatus As New DataTable

    'Dim MinimumWage_Amount = New ExecuteQuery("SELECT MinWageValue FROM payperiod WHERE RowID='" & paypRowID & "';").Result

    Function Recursive(ByRef isThereStillRunning As Boolean) As Boolean
        'Console.WriteLine("Recursive({0})", isThereStillRunning.ToString)

        'count = count + 1
        'If value >= 100 Then
        '    Return value
        'End If
        'Return Recursive(value * 2, count)

        Dim still_running = multi_threads.Cast(Of Thread).Where(Function(ii) ii.ThreadState = (ThreadState.Background _
                                                                                               Or ThreadState.Running) _
                                                                                           And ii.IsAlive = True)

        isThereStillRunning = (still_running IsNot Nothing)

        'If still_running.Count > 0 Then
        '    isThereStillRunning = True
        'Else
        '    isThereStillRunning = False
        'End If

        Return If(isThereStillRunning, Recursive(isThereStillRunning), False)

    End Function

    Dim multi_threads(0) As Thread

    Dim multithreads As New List(Of Thread)

    Private _uiTasks As TaskFactory

    Dim array_bgwork(1) As System.ComponentModel.BackgroundWorker

    Private Sub bgworkgenpayroll_DoBackGroundWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkgenpayroll.DoWork


        Dim sel_employee_dattab = employee_dattab.Select("PositionID IS NULL")

        If sel_employee_dattab.Count > 0 Then

            For Each drow In sel_employee_dattab

                pause_process_message = "Employee '" & drow("EmployeeID") & "' has no position." &
                    vbNewLine & "Please supply his/her position before proceeding to payroll."

                e.Cancel = True

                If bgworkgenpayroll.CancellationPending Then

                    bgworkgenpayroll.CancelAsync()

                End If

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
            "AND ea.EffectiveStartDate <= '" & paypFrom & "' " &
            "AND ea.EffectiveEndDate >= '" & paypTo & "' " &
            "AND ea.AllowanceFrequency = 'Monthly' " &
            "AND p.Fixed = TRUE " &
            "AND p.Status = TRUE"

        If isEndOfMonth = 0 Then 'means end of the month
            fixedTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.AllowanceFrequency IN ('Monthly','Semi-monthly') " &
            " AND (ea.EffectiveStartDate >= '" & paypFrom & "' OR ea.EffectiveEndDate >= '" & paypFrom & "')" &
            " AND (ea.EffectiveStartDate <= '" & paypTo & "' OR ea.EffectiveEndDate <= '" & paypTo & "')" &
            "AND p.Fixed = TRUE " &
            "AND p.Status = TRUE;"
        Else '                      'means first half of the month
            fixedTaxableMonthlyAllowanceSql =
                "SELECT ea.* " &
                "FROM employeeallowance ea " &
                "INNER JOIN product p " &
                "ON p.RowID = ea.ProductID " &
                "WHERE ea.OrganizationID = '" & orgztnID & "' " &
                "AND ea.AllowanceFrequency = 'Semi-monthly' " &
                " AND (ea.EffectiveStartDate >= '" & paypFrom & "' OR ea.EffectiveEndDate >= '" & paypFrom & "')" &
                " AND (ea.EffectiveStartDate <= '" & paypTo & "' OR ea.EffectiveEndDate <= '" & paypTo & "')" &
                "AND p.Fixed = TRUE " &
                "AND p.Status = TRUE;"
        End If

        Dim fixedTaxableMonthlyAllowances = New SQLQueryToDatatable(fixedTaxableMonthlyAllowanceSql).ResultTable

        Dim fixedNonTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.EffectiveStartDate <= '" & paypFrom & "' " &
            "AND ea.EffectiveEndDate >= '" & paypTo & "' " &
            "AND ea.AllowanceFrequency = 'Monthly' " &
            "AND p.Fixed = TRUE " &
            "AND p.status = FALSE"

        If isEndOfMonth = 0 Then 'means end of the month
            fixedNonTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.AllowanceFrequency IN ('Monthly','Semi-monthly') " &
            " AND (ea.EffectiveStartDate >= '" & paypFrom & "' OR ea.EffectiveEndDate >= '" & paypFrom & "')" &
            " AND (ea.EffectiveStartDate <= '" & paypTo & "' OR ea.EffectiveEndDate <= '" & paypTo & "')" &
            "AND p.`Fixed` = TRUE " &
            "AND p.`Status` = FALSE;"
        Else '                      'means first half of the month
            fixedNonTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.AllowanceFrequency = 'Semi-monthly' " &
            " AND (ea.EffectiveStartDate >= '" & paypFrom & "' OR ea.EffectiveEndDate >= '" & paypFrom & "')" &
            " AND (ea.EffectiveStartDate <= '" & paypTo & "' OR ea.EffectiveEndDate <= '" & paypTo & "')" &
            "AND p.`Fixed` = TRUE " &
            "AND p.`Status` = FALSE;"
        End If

        Dim fixedNonTaxableMonthlyAllowances = New SQLQueryToDatatable(fixedNonTaxableMonthlyAllowanceSql).ResultTable

        If e.Cancel = False Then

            '    '# ######################################################################################################################## #

            'Else

            '    '# ######################################################################################################################## #

            payWTax = New SQLQueryToDatatable("SELECT * FROM paywithholdingtax;" &
                                              "").ResultTable

            filingStatus = New SQLQueryToDatatable("SELECT * FROM filingstatus;" &
                                              "").ResultTable

            Dim emp_count = employee_dattab.Rows.Count

            Dim progress_index As Integer = 1

            Dim emptaxabsal As Decimal = 0
            Dim empnetsal As Decimal = 0
            Dim emp_taxabsal = Val(0)

            Dim tax_amount = Val(0)

            Dim grossincome = Val(0)

            Dim grossincome_firsthalf = Val(0)

            Dim date_to_use = If(CDate(paypFrom) > CDate(paypTo), CDate(paypFrom), CDate(paypTo))

            Dim dateStr_to_use = Format(CDate(date_to_use), "yyyy-MM-dd")

            Dim numberofweeksthismonth = _
                EXECQUER("SELECT `COUNTTHEWEEKS`('" & dateStr_to_use & "');")

            For Each drow As DataRow In employee_dattab.Rows

                strPHHdeductsched = drow("PhHealthDeductSched").ToString
                If drow("PhHealthDeductSched").ToString = "End of the month" Then

                    isorgPHHdeductsched = 0

                ElseIf drow("PhHealthDeductSched").ToString = "First half" Then

                    isorgPHHdeductsched = 1

                ElseIf drow("PhHealthDeductSched").ToString = "Per pay period" Then

                    isorgPHHdeductsched = 2

                End If


                strSSSdeductsched = drow("SSSDeductSched").ToString
                If drow("SSSDeductSched").ToString = "End of the month" Then

                    isorgSSSdeductsched = 0

                ElseIf drow("SSSDeductSched").ToString = "First half" Then

                    isorgSSSdeductsched = 1

                ElseIf drow("SSSDeductSched").ToString = "Per pay period" Then

                    isorgSSSdeductsched = 2

                End If


                strHDMFdeductsched = drow("HDMFDeductSched").ToString
                If drow("HDMFDeductSched").ToString = "End of the month" Then

                    isorgHDMFdeductsched = 0

                ElseIf drow("HDMFDeductSched").ToString = "First half" Then

                    isorgHDMFdeductsched = 1

                ElseIf drow("HDMFDeductSched").ToString = "Per pay period" Then

                    isorgHDMFdeductsched = 2

                End If


                If drow("WTaxDeductSched").ToString = "End of the month" Then

                    isorgWTaxdeductsched = 0

                ElseIf drow("WTaxDeductSched").ToString = "First half" Then

                    isorgWTaxdeductsched = 1

                ElseIf drow("WTaxDeductSched").ToString = "Per pay period" Then

                    isorgWTaxdeductsched = 2

                End If


                Dim employee_ID = Trim(drow("RowID"))

                org_WorkDaysPerYear = drow("WorkDaysPerYear")

                Dim divisorMonthlys = If(drow("PayFrequencyID") = 1, 2, _
                                         If(drow("PayFrequencyID") = 2, 1, _
                                            If(drow("PayFrequencyID") = 3, EXECQUER("SELECT COUNT(RowID) FROM employeetimeentry WHERE EmployeeID='" & employee_ID & "' AND Date BETWEEN '" & paypFrom & "' AND '" & paypTo & "' AND IFNULL(TotalDayPay,0)!=0 AND OrganizationID='" & orgztnID & "';"), _
                                               numberofweeksthismonth)))


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

                Dim valoncenotax_allowance = 0.0
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



                If ValNoComma(drow("RowID")) = 661 Then '218 - EmployeeID
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
                        valdaynotax_bon = valdaynotax_bon * numofweekdays 'numofweekends
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

                Dim pstub_TotalAllowance = Val(0)

                Dim the_taxable_salary = ValNoComma(0)

                employee_ID = Trim(drow("RowID"))

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
                    pstub_TotalBonus = Val(0)
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

                        employee_ID = Trim(drow("RowID"))

                        Dim employment_type = StrConv(drow("EmployeeType").ToString, VbStrConv.ProperCase)

                        Dim sel_dtemployeefirsttimesalary = dtemployeefirsttimesalary.Select("EmployeeID = '" & drow("RowID") & "'")

                        Dim StartingAttendanceCount = ValNoComma(drow("StartingAttendanceCount"))
                        If employee_ID = 9 Then : Dim call_lambert = "Over here" : End If
                        If employment_type = "Fixed" Then
                            grossincome = ValNoComma(drowsal("BasicPay"))
                            grossincome = grossincome + (OTAmount + NightDiffAmount + NightDiffOTAmount)

                            grossincome_firsthalf = ValNoComma(drowsal("BasicPay")) + _
                                ValNoComma(prev_empTimeEntry.Compute("SUM(OvertimeHoursAmount)", "EmployeeID = '" & drow("RowID").ToString & "'")) + _
                                ValNoComma(prev_empTimeEntry.Compute("SUM(NightDiffOTHoursAmount)", "EmployeeID = '" & drow("RowID").ToString & "'")) + _
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

                        '**************************************************************************************
                        'Below is the condition that if the employee will receive his/her salary for first time
                        'and his/her attended work days is less than five
                        'the system will skip the gov't contributions for this period
                        '**************************************************************************************
                        If skipgovtdeduct = "1" _
                            And StartingAttendanceCount < 5.0 _
                            And sel_dtemployeefirsttimesalary.Count <> 0 Then

                            pstub_TotalEmpSSS = 0
                            pstub_TotalCompSSS = 0

                            pstub_TotalEmpPhilhealth = 0
                            pstub_TotalCompPhilhealth = 0

                            pstub_TotalEmpHDMF = 0
                            pstub_TotalCompHDMF = 0

                        End If

                        Dim the_EmpRatePerDay = ValNoComma(drow("EmpRatePerDay"))

                        Dim the_MinimumWageAmount = ValNoComma(drow("MinimumWageAmount"))

                        Dim isMinimumWage = (ValNoComma(drow("EmpRatePerDay")) <= ValNoComma(drow("MinimumWageAmount")))

                        Dim _eRowID = drow("RowID")

                        If isEndOfMonth = isorgWTaxdeductsched Then

                            emp_taxabsal = grossincome - _
                                            (pstub_TotalEmpSSS + pstub_TotalEmpPhilhealth + pstub_TotalEmpHDMF)

                            the_taxable_salary = (grossincome + grossincome_firsthalf) - _
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

                                Dim GET_employeetaxableincome = EXECQUER("SELECT `GET_employeetaxableincome`('" & drow("RowID") & "', '" & orgztnID & "', '" & paypFrom & "','" & grossincome & "');")


                                'For Each drowtax As DataRow In paywithholdingtax.rows
                                For Each drowtax In sel_payWTax

                                    'emp_taxabsal = emptaxabsal - (Val(drowtax("ExemptionAmount")) + _
                                    '             ((emptaxabsal - Val(drowtax("TaxableIncomeFromAmount"))) * Val(drowtax("ExemptionInExcessAmount"))) _
                                    '                             )

                                    Dim the_values = Split(GET_employeetaxableincome, ";")

                                    tax_amount = _
                                        ValNoComma(drowtax("ExemptionAmount")) _
                                        + ((the_taxable_salary - ValNoComma(drowtax("TaxableIncomeFromAmount"))) * ValNoComma(drowtax("ExemptionInExcessAmount")))
                                    'ValNoComma(the_values(1))

                                    Exit For

                                Next

                            End If

                        Else
                            'PAYFREQUENCY_DIVISOR

                            emp_taxabsal = grossincome - _
                                            (pstub_TotalEmpSSS + pstub_TotalEmpPhilhealth + pstub_TotalEmpHDMF)

                            the_taxable_salary = grossincome - _
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

                                Dim GET_employeetaxableincome = EXECQUER("SELECT `GET_employeetaxableincome`('" & drow("RowID") & "', '" & orgztnID & "', '" & paypFrom & "','" & grossincome & "');")

                                'For Each drowtax As DataRow In paywithholdingtax.rows
                                For Each drowtax In sel_payWTax

                                    'emp_taxabsal = emptaxabsal - (Val(drowtax("ExemptionAmount")) + _
                                    '             ((emptaxabsal - Val(drowtax("TaxableIncomeFromAmount"))) * Val(drowtax("ExemptionInExcessAmount"))) _
                                    '                             )

                                    Dim the_values = Split(GET_employeetaxableincome, ";")

                                    tax_amount = _
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
                    New ExecuteQuery("SELECT EXISTS(SELECT RowID" &
                                     " FROM paystub" &
                                     " WHERE EmployeeID='" & drow("RowID") & "'" &
                                     " AND OrganizationID='" & orgztnID & "'" &
                                     " AND PayFromDate='" & paypFrom & "'" &
                                     " AND PayToDate='" & paypTo & "');")

                If isPayStubExists.Result = "2" Then

                    If ValNoComma(Me.VeryFirstPayPeriodIDOfThisYear) = ValNoComma(paypRowID) Then 'this means, the very first cut off of this year falls here
                        '                                                                           'so system should regain leave balance equals to leave allowance

                        Dim new_ExecuteQuery As _
                            New ExecuteQuery("UPDATE employee e" &
                                             " INNER JOIN payperiod pp ON pp.RowID='" & paypRowID & "'" &
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
                        '" AND (ADDDATE(e.StartDate, INTERVAL 1 YEAR) <= '" & paypFrom & "'" &
                        '" OR ADDDATE(e.StartDate, INTERVAL 1 YEAR) <= '" & paypTo & "');")

                        'typical regaining of leave balance at the start of the year

                    End If

                    'gaining of employee leave balance whos year of service reaches one(1)
                    'the values that employee will gain here are prorated
                    Dim n_ExecuteQuery As _
                        New ExecuteQuery("UPDATE employee e" &
                                         " INNER JOIN payperiod pp ON pp.RowID='" & paypRowID & "'" &
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
                                         " AND ADDDATE(e.StartDate, INTERVAL 1 YEAR) BETWEEN '" & paypFrom & "' AND '" & paypTo & "';")

                    'Dim n_ExecuteQuery As _
                    '    New ExecuteQuery("UPDATE employee e SET" &
                    '                     " e.LeaveBalance=e.LeaveBalance + e.LeavePerPayPeriod" &
                    '                     ",e.SickLeaveBalance=e.SickLeaveBalance + e.SickLeavePerPayPeriod" &
                    '                     ",e.MaternityLeaveBalance=e.MaternityLeaveBalance + e.MaternityLeavePerPayPeriod" &
                    '                     ",e.OtherLeaveBalance=e.OtherLeaveBalance + e.OtherLeavePerPayPeriod" &
                    '                     ",LastUpd=CURRENT_TIMESTAMP()" &
                    '                     ",LastUpdBy='" & z_User & "'" &
                    '                     " WHERE e.RowID='" & drow("RowID") & "'" &
                    '                     " AND e.OrganizationID='" & orgztnID & "'" &
                    '                     " AND (ADDDATE(e.StartDate, INTERVAL 1 YEAR) <= '" & paypFrom & "'" &
                    '                     " OR ADDDATE(e.StartDate, INTERVAL 1 YEAR) <= '" & paypTo & "');")

                End If

                If ValNoComma(drow("RowID")) = 661 Then '218 - EmployeeID
                    Dim call_lambert = "Over here"
                End If

                Dim paystubID = _
                INSUPD_paystub(paypRowID,
                               drow("RowID").ToString,
                               paypFrom,
                               paypTo,
                               grossincome + totalemployeebonus + totalnotaxemployeebonus + totalnotaxemployeeallownce + totalemployeeallownce,
                               tot_net_pay + totalemployeebonus + totalnotaxemployeebonus + totalnotaxemployeeallownce + totalemployeeallownce + thirteenthmoval,
                               the_taxable_salary,
                               tax_amount,
                               pstub_TotalEmpSSS,
                               pstub_TotalCompSSS,
                               pstub_TotalEmpPhilhealth,
                               pstub_TotalCompPhilhealth,
                               pstub_TotalEmpHDMF,
                               pstub_TotalCompHDMF,
                               valemp_loan,
                               totalemployeebonus + totalnotaxemployeebonus,
                               totalemployeeallownce + totalnotaxemployeeallownce) 'totalemployeebonus + totalemployeeallownce +

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
                '        New ExecuteQuery("INSERT INTO paystubitem(OrganizationID,Created,CreatedBy,PayStubID,ProductID,PayAmount,Undeclared)" &
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

                ConvertLeaveToCash(employee_ID, drow)

                If isPayStubExists.Result = "2" Then
                    'the decrement of leave balances happens here
                    Dim new_ExecuteQuery As _
                        New ExecuteQuery("UPDATE employee e" &
                                         " LEFT JOIN (SELECT RowID" &
                                                     ",SUM(VacationLeaveHours) AS VacationLeaveHours" &
                                                     ",SUM(SickLeaveHours) AS SickLeaveHours" &
                                                     ",SUM(MaternityLeaveHours) AS MaternityLeaveHours" &
                                                     ",SUM(OtherLeaveHours) AS OtherLeaveHours" &
                                                     " FROM employeetimeentry" &
                                                     " WHERE EmployeeID='" & drow("RowID") & "'" &
                                                     " AND OrganizationID='" & orgztnID & "'" &
                                                     " AND `Date` BETWEEN '" & paypFrom & "' AND '" & paypTo & "') ete ON ete.RowID IS NOT NULL" &
                                        " SET" &
                                        " e.LeaveBalance = e.LeaveBalance - IFNULL(ete.VacationLeaveHours, 0)" &
                                        ",e.SickLeaveBalance = e.SickLeaveBalance - IFNULL(ete.SickLeaveHours,0)" &
                                        ",e.MaternityLeaveBalance = e.MaternityLeaveBalance - IFNULL(ete.MaternityLeaveHours,0)" &
                                        ",e.OtherLeaveBalance = e.OtherLeaveBalance - IFNULL(ete.OtherLeaveHours,0)" &
                                        ",LastUpd=CURRENT_TIMESTAMP()" &
                                        ",LastUpdBy='" & z_User & "'" &
                                        " WHERE e.RowID='" & drow("RowID") & "' AND e.OrganizationID='" & orgztnID & "';")

                End If

                Dim newer_ExecuteQuery As _
                    New ExecuteQuery("CALL `SP_UpdatePaystubAdjustment`('" & drow("RowID") & "'" &
                                     ",'" & paypRowID & "'" &
                                     ",'" & z_User & "'" &
                                     ",'" & orgztnID & "');")

                bgworkgenpayroll.ReportProgress(CInt((100 * progress_index) / emp_count), "")

                progress_index += 1

            Next

            Dim nyu_ExecuteQuery As New ExecuteQuery("CALL `RECOMPUTE_thirteenthmonthpay`('" & orgztnID & "','" & paypRowID & "','" & z_User & "');")

            If withthirteenthmonthpay = 1 Then

                nyu_ExecuteQuery = New ExecuteQuery("CALL `RELEASE_thirteenthmonthpay`('" & orgztnID & "','" & paypRowID & "','" & z_User & "');")

            End If

            nyu_ExecuteQuery = New ExecuteQuery("UPDATE employeeloanschedule SET LastUpdBy=IFNULL(LastUpdBy,CreatedBy),`Status`='Completed' WHERE LoanPayPeriodLeft <= 0 AND OrganizationID=" & orgztnID & ";")

            payWTax.Dispose()

            filingStatus.Dispose()

        End If

    End Sub

    Private Sub ConvertLeaveToCash(employeeID As String, employee As DataRow)
        Dim sql = <![CDATA[
            SELECT Year, OrdinalValue
            FROM payperiod
            WHERE RowID = @PayPeriodID;
        ]]>.Value

        Trace.Write(sql)

        sql = Replace(sql, "@PayPeriodID", Me.paypRowID)

        Dim results = GetDatatable(sql)
        Dim payPeriod = results(0)
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

    Dim payroll_emp_count As Integer = 0

    Private Sub Thread_Method(sender As Object, e As EventArgs) Handles ToolStripButton2.Click

        SpDataSet = New DataSet("ds")

        payroll_emp_count = 0

        SpDataSet = New DataSet("ds")

        Dim SpCmd As New MySqlCommand("EMPLOYEE_payrollgen_paginate",
                                      New MySql.Data.MySqlClient.MySqlConnection(mysql_conn_text))

        Try

            SpCmd.CommandTimeout = 1000
            SpCmd.CommandType = CommandType.StoredProcedure
            SpCmd.Parameters.Clear()
            SpCmd.Parameters.AddWithValue("OrganizID", 3)
            SpCmd.Parameters.AddWithValue("Pay_Date_From", paypFrom) '"2016-11-06")
            SpCmd.Parameters.AddWithValue("Pay_Date_To", paypTo) '"2016-11-20")
            SpCmd.Parameters.AddWithValue("max_rec_perpage", 20)

            Dim MyAdapter As New MySqlDataAdapter(SpCmd)

            MyAdapter.Fill(SpDataSet)
        Catch ex As Exception
            MessageBox.Show(String.Concat("ERROR @: Thread_Method", ex.Message))

        Finally

            Dim tbl_cnt As Integer = SpDataSet.Tables.Count

            Console.WriteLine(String.Concat("Number of tables : ", Convert.ToString(tbl_cnt)))

            ' This is not run on the UI thread.

            'mysql_cmd.Connection.Open();
            'mysql_cmd.Connection.Close();
            Dim tbl_count As Integer = SpDataSet.Tables.Count
            Console.WriteLine(String.Concat("Starts @ ", Now))
            '.Factory.StartNew

            'Dim taskWithFactoryAndState1 As Task =
            '    Task.Run(Sub()

            '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

            For Each dt As DataTable In SpDataSet.Tables

                Try

                Catch ex As Exception
                    Console.WriteLine(String.Concat("Error @ ", dt.TableName, " : ", ex.Message))

                End Try

                'Task.Factory.StartNew()
                'Dim inner_task As Task =
                '    Task.Run(Sub()
                'New Task
                Try

                    Dim n_PayrollGeneration As New PayrollGeneration(dt,
                                                                      isEndOfMonth,
                                                                      esal_dattab,
                                                                      emp_loans,
                                                                      emp_bonus,
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
                                                                      withthirteenthmonthpay, Me)

                    With n_PayrollGeneration
                        .PayrollDateFrom = paypFrom
                        .PayrollDateTo = paypTo
                        .PayrollRecordID = paypRowID

                        Dim innr_task As Task =
                            Task.Run(Sub()
                                         'New Task
                                         Console.WriteLine("From Task.RUN")
                                     End Sub)

                        'innr_task.RunSynchronously()
                        innr_task.Wait()
                    End With

                    payroll_emp_count += Convert.ToInt16(dt.Rows.Count)

                Catch ex As Exception
                    Console.WriteLine(String.Concat("INNER_Task_Error @ ",
                                                    dt.TableName,
                                                    " :#: ", ex.Message))

                Finally
                    'Thread.Sleep(1000)

                End Try

                'End Sub)

                '.ContinueWith(Sub()

                '              End Sub)
                'inner_task.Wait()

                'inner_task.RunSynchronously()
                'inner_task.Start()

            Next

            '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

            'End Sub)

            '.ContinueWith(Function(ant)
            '                  'updates UI no problem as we are using correct SynchronizationContext
            Console.WriteLine(String.Concat("Total number of records : ", payroll_emp_count))
            Console.WriteLine("Done @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@")
            Console.WriteLine(String.Concat("Finished @ ", Now))
            '                  Return True

            '              End Function, TaskScheduler.FromCurrentSynchronizationContext())
            'taskWithFactoryAndState1.Start()
            'taskWithFactoryAndState1.RunSynchronously()
            'taskWithFactoryAndState1.Wait()

        End Try

    End Sub

    Dim progress_precentage As Integer = 0

    Sub ProgressCounter(ByVal cnt As Integer)

        progress_precentage += 1

        Dim compute_percentage As Integer = (progress_precentage / payroll_emp_count) * 100
        MDIPrimaryForm.systemprogressbar.Value = compute_percentage

        Select Case compute_percentage

            Case 100
                'Me.Enabled = True
                dgvpayper_SelectionChanged(dgvpayper, New EventArgs)

                Dim param_array = New Object() {orgztnID, paypRowID, z_User}

                Dim n_ExecSQLProcedure As New  _
                    ExecSQLProcedure("RECOMPUTE_thirteenthmonthpay",
                                     param_array)

            Case Else

        End Select

        Console.WriteLine(String.Concat("#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@ ", compute_percentage, "% complete"))

    End Sub

    Dim indxStartBatch As Integer = 0

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Dim alive_bgworks = array_bgwork.Cast(Of System.ComponentModel.BackgroundWorker).Where(Function(y) y IsNot Nothing)

        Dim busy_bgworks = alive_bgworks.Cast(Of System.ComponentModel.BackgroundWorker).Where(Function(y) y.IsBusy)

        Dim bool_result As Boolean = (Convert.ToInt16(busy_bgworks.Count) > 0)

        If bool_result = False Then

            indxStartBatch += thread_max

            'If payroll_emp_count >= max_rec_perpage Then
            If emp_list_batcount > thread_max Then
                ThreadingPayrollGeneration(indxStartBatch)
            End If

            Console.WriteLine("batch has finished...")

            If progress_precentage = payroll_emp_count Then
                Timer1.Stop()
                Me.Enabled = True
                Timer1.Enabled = False

            Else

            End If

        Else

            Console.WriteLine("batch still in process...")

        End If
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

        Dim rptdoc = New OfficialPaySlipFormat

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL paystub_payslip(" & orgztnID & "," & n_PayPeriodRowID & "," & n_IsPrintingAsActual & ");")
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