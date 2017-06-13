Imports MySql.Data.MySqlClient
Imports System.IO

Public Class TimEntduration

    Dim day_today As Integer = Val(CDate(EmpTimeEntry.today_date).Day)

    Dim month_today As Integer = Val(CDate(EmpTimeEntry.today_date).Month)

    Dim year_today As Integer = Val(CDate(EmpTimeEntry.today_date).Year)

    Dim new_conn As New MySqlConnection
    Dim new_cmd As New MySqlCommand

    Dim employee_dattab As New DataTable

    Dim current_years = 0

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable(String.Concat("SELECT d.RowID",
                                    ",CONCAT_WS(' - ', dd.Name, d.Name) `Name`",
                                    " FROM `division` d",
                                    " INNER JOIN `division` dd ON dd.RowID=d.ParentDivisionID AND dd.RowID != d.RowID",
                                    " WHERE d.OrganizationID='", orgztnID, "'",
                                    " AND d.ParentDivisionID IS NOT NULL",
                                    " ORDER BY dd.Name, d.Name;"))

        With n_SQLQueryToDatatable.ResultTable

            cboxDivisions.ValueMember = .Columns(0).ColumnName

            cboxDivisions.DisplayMember = .Columns(1).ColumnName

            cboxDivisions.DataSource = n_SQLQueryToDatatable.ResultTable

        End With

        EmpTimeEntry.tsbtnCloseempawar.Enabled = False

        MyBase.OnLoad(e)

    End Sub

    Private Sub TimEntduration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        new_conn.ConnectionString = db_connectinstring

        Me.Text = "Today's date is " & Format(CDate(EmpTimeEntry.today_date), "MMMM dd, yyyy")

        current_years = Format(CDate(EmpTimeEntry.today_date), "yyyy")

        linkPrev.Text = "← " & (current_years - 1)
        linkNxt.Text = (current_years + 1) & " →"



        txtYr.Text = CDate(EmpTimeEntry.today_date).Year

        cbomonth.Items.Clear()

        Dim date_month = Nothing

        For i = 0 To 11
            'EXECQUER("SELECT DATE_FORMAT(DATE_ADD(MAKEDATE(YEAR(NOW()),1), INTERVAL " & i & " MONTH),'%M');")

            date_month = Format(CDate(txtYr.Text & "-01-01").AddMonths(i), _
                                "MMMM")

            cbomonth.Items.Add(date_month)

        Next

        cbomonth.SelectedIndex = CDate(EmpTimeEntry.today_date).Month - 1


        Dim payfrqncy As New AutoCompleteStringCollection


        Dim sel_query = ""

        Dim hasAnEmployee = EXECQUER("SELECT EXISTS(SELECT RowID FROM employee WHERE OrganizationID=" & orgztnID & " LIMIT 1);")

        If hasAnEmployee = 1 Then
            sel_query = "SELECT pp.PayFrequencyType FROM payfrequency pp INNER JOIN employee e ON e.PayFrequencyID=pp.RowID GROUP BY pp.RowID;"
        Else
            sel_query = "SELECT PayFrequencyType FROM payfrequency;"
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

            AddHandler new_tsbtn.Click, AddressOf PayFreq_Changed 'Button2_Click

        Next

        tstrip.PerformLayout()

        If first_sender IsNot Nothing Then
            PayFreq_Changed(first_sender, New EventArgs)
        End If


        'With dgvpayper
        '    .Columns("RowID").Visible = False
        '    .Columns("TotalGrossSalary").Visible = False
        '    .Columns("TotalNetSalary").Visible = False
        '    .Columns("TotalEmpSSS").Visible = False
        '    .Columns("TotalEmpWithholdingTax").Visible = False
        '    .Columns("TotalCompSSS").Visible = False
        '    .Columns("TotalEmpPhilhealth").Visible = False
        '    .Columns("TotalCompPhilhealth").Visible = False
        '    .Columns("TotalEmpHDMF").Visible = False
        '    .Columns("TotalCompHDMF").Visible = False
        '    .Columns("now_origin").Visible = False

        'End With

        For Each c As DataGridViewColumn In dgvpayper.Columns
            c.Visible = False
            If c.Index = 1 _
                Or c.Index = 2 Then
                c.Visible = True
            End If
        Next

    End Sub

    Dim selectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim unselectedButtonFont = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Dim quer_empPayFreq = ""

    Sub PayFreq_Changed(sender As Object, e As EventArgs)

        quer_empPayFreq = ""

        Dim senderObj As New ToolStripButton

        Static prevObj As New ToolStripButton

        Static once As SByte = 0

        senderObj = DirectCast(sender, ToolStripButton)

        If once = 0 Then

            once = 1

            prevObj = senderObj

            senderObj.BackColor = Color.FromArgb(194, 228, 255)

            senderObj.Font = selectedButtonFont

            quer_empPayFreq = senderObj.Text

            loadpayp(CDate(EmpTimeEntry.today_date), quer_empPayFreq)

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

        quer_empPayFreq = senderObj.Text

        Dim sel_month = CStr(cbomonth.SelectedIndex + 1)

        If sel_month.ToString.Length = 1 Then
            sel_month = "0" & sel_month
        End If

        'loadpayp(CDate(EmpTimeEntry.today_date), quer_empPayFreq)

        loadpayp(Trim(txtYr.Text) & "-" & sel_month & "-01", _
                 quer_empPayFreq)

    End Sub

    Sub loadpayp(Optional param_Date As Object = Nothing, _
                 Optional PayFreqType As Object = "SEMI-MONTHLY")

        'Dim param(3, 2) As Object

        'param(0, 0) = "payp_OrganizationID"
        'param(1, 0) = "param_Date"
        'param(2, 0) = "PayFreqType"

        'param(0, 1) = orgztnID
        'param(1, 1) = param_Date ' If(param_Date = Nothing, _
        ''                 CObj(Year(Now) & "-01-01"), _
        ''                 param_Date)
        'param(2, 1) = PayFreqType

        'filltable(dgvpayper, _
        '          "VIEW_payperiod", _
        '          param, _
        '          1) 'VIEW_payp

        '****************************************

        'Dim params(3, 2) As Object

        'params(0, 0) = "payp_OrganizationID"
        'params(1, 0) = "param_Date"
        'params(2, 0) = "PayFreqType"

        'params(0, 1) = orgztnID
        'params(1, 1) = If(param_Date = Nothing, Year(Now) & "-01-01", param_Date)
        'params(2, 1) = PayFreqType

        'EXEC_VIEW_PROCEDURE(params, _
        '                    "VIEW_payperiod", _
        '                    dgvpaypers, _
        '                    1)

        Dim dt_payp As New DataTable

        dt_payp = New ReadSQLProcedureToDatatable("VIEW_payperiod",
                                                  orgztnID,
                                                  If(param_Date = Nothing, Year(Now) & "-01-01", Format(CDate(param_Date), "yyyy-MM-dd")),
                                                  PayFreqType).ResultTable

        dgvpaypers.Rows.Clear()

        For Each drow As DataRow In dt_payp.Rows

            Dim row_array = drow.ItemArray

            dgvpaypers.Rows.Add(row_array)

        Next

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim sel_month = CStr(cbomonth.SelectedIndex + 1)

        If sel_month.ToString.Length = 1 Then
            sel_month = "0" & sel_month
        End If

        loadpayp(Trim(txtYr.Text) & "-" & sel_month & "-01", _
                 quer_empPayFreq)

        dgvpayper.Focus()

    End Sub

    Private Sub TextBox5_KeyDown(sender As Object, e As KeyEventArgs) Handles txtYr.KeyDown, _
                                                                              cbomonth.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button2_Click(sender, e)
            'ElseIf e.KeyCode = Keys.Escape Then
            '    Me.Close()
        End If

    End Sub

    Dim me_close As SByte = 0

    Private Sub TimEntduration_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        'If Application.OpenForms().OfType(Of EmpTimeEntry).Any Then
        '    e.Cancel = False
        'End If

        'If EmpTimeEntry.me_close = 1 Then
        '    If bgWork.IsBusy Then
        '        e.Cancel = True
        '        Me.Hide()
        '        me_close = 0
        '    Else

        '        e.Cancel = False
        '        me_close = 1

        '    End If

        'Else
        '    e.Cancel = True

        '    If bgWork.IsBusy = False Then

        '        'MDIPrimaryForm.Enabled = True

        '        Me.SendToBack()

        '        Me.Hide()

        '        MDIPrimaryForm.BringToFront()

        '    End If
        '    me_close = 0

        'End If

        If bgWork.IsBusy = False Then

            me_close = 1

            MDIPrimaryForm.Enabled = True

            'Me.SendToBack()

            'Me.Hide()

            MDIPrimaryForm.BringToFront()

        Else
            e.Cancel = True
            me_close = 0

        End If

        'MDIPrimaryForm.BringToFront()

        'If bgWork.IsBusy Then
        '    'Me.WindowState = FormWindowState.Minimized
        'Else
        'End If

        Dim this_class_count = 0

        For Each form_obj As Form In Application.OpenForms

            If form_obj.Name = "TimEntduration" Then
                this_class_count += 1
            End If

        Next

        If this_class_count = 1 Then
            EmpTimeEntry.tsbtnCloseempawar.Enabled = True
        End If

        'If Application.OpenForms().OfType(Of TimEntduration).Any = False Then

        '    EmpTimeEntry.tsbtnCloseempawar.Enabled = True

        'End If

    End Sub

    Private Sub dgvpayper_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpayper.CellContentClick

    End Sub

    Private Sub dgvpayper_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvpayper.ColumnAdded
        e.Column.SortMode = DataGridViewColumnSortMode.NotSortable
    End Sub

    Private Sub dgvpayper_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvpayper.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Hide()
        End If
    End Sub

    Private Sub txtYr_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtYr.KeyPress
        e.Handled = TrapNumKey(Asc(e.KeyChar))
    End Sub

    Private Sub txtYr_TextChanged(sender As Object, e As EventArgs) Handles txtYr.TextChanged

    End Sub

    Dim empsh_EffFrom As Object
    Dim empsh_EffTo As Object

    Dim day_paypFrom As Integer
    Dim day_paypTo As Integer
    Dim month_paypFrom As String = Nothing
    Dim month_paypTo As Integer
    Dim year_payp As Integer

    Dim date_paypFrom As Date
    Dim date_paypTo As Date

    Sub compute_employeetimeentry()

        If dgvpayper.RowCount <> 0 Then

            With dgvpayper
                .Focus()

                With .CurrentRow

                    year_payp = If(.Cells("Pay period from").Value = Nothing, _
                                                    CDate(EmpTimeEntry.today_date).Year, _
                                                    CDate(.Cells("Pay period from").Value).Year)


                    day_paypFrom = If(.Cells("Pay period from").Value = Nothing, _
                                      CDate(.Cells("Pay period to").Value).Day, _
                                      CDate(.Cells("Pay period from").Value).Day)

                    day_paypTo = If(.Cells("Pay period to").Value = Nothing, _
                                    CDate(.Cells("Pay period from").Value).Day, _
                                    CDate(.Cells("Pay period to").Value).Day)

                    month_paypFrom = If(.Cells("Pay period from").Value = Nothing, _
                                        CDate(.Cells("Pay period to").Value).Month, _
                                        CDate(.Cells("Pay period from").Value).Month)

                    month_paypTo = If(.Cells("Pay period to").Value = Nothing, _
                                      CDate(.Cells("Pay period from").Value).Month, _
                                      CDate(.Cells("Pay period to").Value).Month)

                    date_paypFrom = If(.Cells("Pay period from").Value = Nothing, _
                                       CDate(EmpTimeEntry.today_date), _
                                       CDate(.Cells("Pay period from").Value))

                    date_paypTo = If(.Cells("Pay period to").Value = Nothing, _
                                     CDate(EmpTimeEntry.today_date), _
                                     CDate(.Cells("Pay period to").Value))

                    If day_today = day_paypFrom Then

                    ElseIf day_today = 1 Then

                    ElseIf day_today = 15 Then

                    ElseIf day_today = 16 Then

                    ElseIf day_today = day_paypTo Then

                    Else

                    End If

                    'tingnan ko muna si employee at ang date sa employeetimeentry,
                    'if exist recalculate with regards sa employeeshift, and then update employeetimeentry
                    'if not exist sa employeetimeentry,
                    '   check if employee has employeeshift
                    '       if exist sa employeeshift,
                    '       check now on employeetimeentrydetails then insert into employeetimeentry (absent, late, undertime, overtime)
                    '   if not exist sa employeeshift
                    '       ignore insert into employeetimeentry

                    'kapag nagcompute ako ng hour(s) worked,
                    'consider ko yung date ngayon at yung employeeshift.EffectiveFrom & employeeshift.EffectiveTo
                    '
                    '

                    If .Cells("Pay period to").Value = Nothing Then
                        EmpTimeEntry.dtppayperiod.Value = Format(CDate(.Cells("Pay period from").Value), "MMMM-dd-yyyy")
                    Else
                        EmpTimeEntry.dtppayperiod.Value = Format(CDate(.Cells("Pay period to").Value), "MMMM-dd-yyyy")
                    End If

                End With

                With EmpTimeEntry

                    If .currEShiftID <> Nothing _
                        Or .currNightEShiftID <> Nothing Then

                        '.currShiftID = .currShiftID

                        '.curr_TimeFromMILIT = .curr_TimeFromMILIT
                        '.curr_TimeToMILIT = .curr_TimeToMILIT

                        '.currNightEShiftID = .currNightEShiftID
                        '.currNightShiftID = .currNightShiftID

                        '.curr_NightTimeFromMILIT = .curr_NightTimeFromMILIT
                        '.curr_NightTimeToMILIT = .curr_NightTimeToMILIT

                        empsh_EffFrom = CDate(EmpTimeEntry.txtshftEffFrom.Text)

                        empsh_EffTo = CDate(EmpTimeEntry.txtshftEffTo.Text)

                        Dim dt_etent As New DataTable

                        Dim hrswrkd As Double

                        Static firstrow As SByte = 0

                        'timeoutlastval = Nothing

                        'txthrsworkd.Text = 0
                        'txthrsOT.Text = 0
                        'txthrsUT.Text = 0

                        'txthrsabsent.Text = 0
                        'txtnightdiff.Text = 0
                        'txtnightdiffOT.Text = 0

                        'hrswrkd = 0

                        'cbolateabsent.SelectedIndex = -1
                        ''curr_TimeFromMILIT'curr_TimeToMILIT
                        ''curr_NightTimeFromMILIT'curr_NightTimeToMILIT

                        ''*******KUNG MAY GRACE TIME, THE HERE...**********
                        ''SELECT ADDTIME('08:00:00', '00:15:00');' '00:15:00' sample ng grace time
                        ''*************************************************

                        'MsgBox(EmpTimeEntry.employeetimeentry.Rows.Count.ToString & vbNewLine & _
                        '       " employeetimeentry row count")

                        'For i = day_paypFrom To day_paypTo

                        'Next

                        If DateDiff(DateInterval.Day, _
                                    CDate(empsh_EffTo), _
                                    CDate(EmpTimeEntry.today_date)) <= 0 Then 'the date today is
                            '                                                 'within the effecitivity
                            '                                                 'of the employee's shift

                            Dim drowExists() As DataRow = EmpTimeEntry.employeetimeentry.Select("Date>='" & year_payp & _
                                                                                                "-" & month_paypFrom & _
                                                                                                "-" & If(day_paypFrom.ToString.Length = 1, "0" & day_paypFrom, day_paypFrom) & "'" & _
                                                                                                " AND Date<='" & year_payp & _
                                                                                                "-" & month_paypFrom & _
                                                                                                "-" & If(day_paypTo.ToString.Length = 1, "0" & day_paypTo, day_paypTo) & "'")

                            'For Each r As DataRow In drowExists
                            '    For c = 0 To EmpTimeEntry.employeetimeentry.Columns.Count - 1
                            '        MsgBox(r(c).ToString)
                            '    Next
                            'Next

                            'MsgBox(drowExists.Count.ToString)

                            'If drowExists.Count <> 0 Then
                            '    MsgBox(drowExists(0).ToString)
                            'End If

                            Dim UTval, _
                                UTNightval, _
                                OTval, _
                                OTNightval, _
                                Lateval As Object
                            UTNightval = ValNoComma(0) : OTNightval = ValNoComma(0)
                            Dim ot_hrsworkd = ValNoComma(0)

                            If dgvpayper.CurrentRow.Cells("Pay period from").Value <> Nothing _
                                And dgvpayper.CurrentRow.Cells("Pay period to").Value <> Nothing Then

                                Dim dateFrom = Format(CDate(dgvpayper.CurrentRow.Cells("Pay period from").Value), "yyyy-MM-dd")
                                Dim dateTo = Format(CDate(dgvpayper.CurrentRow.Cells("Pay period to").Value), "yyyy-MM-dd")

                                Dim firstbound = If(dgvpayper.CurrentRow.Cells("Pay period from").Value = Nothing, _
                                                    CDate(dgvpayper.CurrentRow.Cells("Pay period to").Value).Day, _
                                                    CDate(dgvpayper.CurrentRow.Cells("Pay period from").Value).Day)

                                Dim lastbound = If(dgvpayper.CurrentRow.Cells("Pay period to").Value = Nothing, _
                                                    CDate(dgvpayper.CurrentRow.Cells("Pay period from").Value).Day, _
                                                    CDate(dgvpayper.CurrentRow.Cells("Pay period to").Value).Day)

                                Dim month = If(month_paypFrom.ToString.Length = 1, "0" & month_paypFrom, month_paypFrom)

                                Dim dt_esh As New DataTable

                                For i_date = firstbound To lastbound

                                    'month & "-" & i_dateDay & "-" & year_payp

                                    Dim i_dateDay = If(i_date.ToString.Length = 1, "0" & i_date, i_date)

                                    dt_esh = New DataTable
                                    dt_esh = retAsDatTbl("SELECT " & _
                                     "esh.RowID 'esh_RowID'" & _
                                     ",COALESCE(DATE_FORMAT(esh.EffectiveFrom,'%m-%d-%Y'),MAKEDATE(YEAR(NOW()),1)) 'EffFrom'" & _
                                     ",COALESCE(DATE_FORMAT(esh.EffectiveTo,'%m-%d-%Y'),DATE_FORMAT(MAKEDATE(YEAR(NOW()),DAYOFYEAR(DATE(CONCAT(YEAR(NOW()),'-12-',DAY(LAST_DAY(DATE(CONCAT(YEAR(NOW()),'-12-00')))))))),'%m-%d-%Y')) 'EffTo'" & _
                                     ",COALESCE(sh.RowID,'') 'sh_RowID'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeFrom,'%r'),'') 'sh_TimeFrom'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeTo,'%r'),'') 'sh_TimeTo'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeFrom,'%H:%i:%s'),'') 'sh_TimeFromMILIT'" & _
                                     ",COALESCE(TIME_FORMAT(sh.TimeTo,'%H:%i:%s'),'') 'sh_TimeToMILIT'" & _
                                     ",COALESCE(esh.NightShift,'') 'NightShift'" & _
                                     ",COALESCE((CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(IF(sh.TimeFrom>sh.TimeTo,ADDTIME(sh.TimeTo, '24:00:00'),sh.TimeTo),'%H:%i:%s'),TIME_FORMAT(sh.TimeFrom,'%H:%i:%s')),':',1) AS INT)) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(IF(sh.TimeFrom>sh.TimeTo,ADDTIME(sh.TimeTo, '24:00:00'),sh.TimeTo),'%H:%i:%s'),TIME_FORMAT(sh.TimeFrom,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),0) 'RegularHrsWork'" & _
                                     " FROM employeeshift esh" & _
                                     " LEFT JOIN shift sh ON sh.RowID=esh.ShiftID" & _
                                     " WHERE esh.EmployeeID=" & .dgvEmployi.CurrentRow.Cells("RowID").Value & _
                                     " AND esh.OrganizationID=" & orgztnID & _
                                     " AND CAST('" & year_payp & "-" & month & "-" & i_dateDay & "' AS DATE)" & _
                                     " BETWEEN COALESCE(esh.EffectiveFrom,DATE_ADD(DATE('" & year_payp & "-" & month & "-" & i_dateDay & "'), INTERVAL -1 MONTH))" & _
                                     " AND COALESCE(esh.EffectiveTo,DATE_ADD(DATE('" & year_payp & "-" & month & "-" & i_dateDay & "'), INTERVAL 1 MONTH));")

                                    '*******KUNG MAY GRACE TIME, THEN HERE...**********
                                    'SELECT ADDTIME('08:00:00', '00:15:00');' '00:15:00' sample ng grace period
                                    '*************************************************

                                    '*******AT SAKA KUNG MERON BANG ALLOWABLE MINUTE(S) LATE**********
                                    '*************************************************

                                    .currEShiftID = Nothing
                                    .currShiftID = Nothing

                                    .curr_TimeFromMILIT = Nothing
                                    .curr_TimeToMILIT = Nothing

                                    .currNightEShiftID = Nothing
                                    .currNightShiftID = Nothing

                                    .curr_NightTimeFromMILIT = Nothing
                                    .curr_NightTimeToMILIT = Nothing

                                    .curr_RegHrsWork = Nothing

                                    For Each drow As DataRow In dt_esh.Rows

                                        If Val(drow("NightShift")) = 0 Then 'DAY SHIFT

                                            .currEShiftID = drow("esh_RowID")
                                            .currShiftID = drow("sh_RowID")

                                            .curr_TimeFromMILIT = drow("sh_TimeFromMILIT")
                                            .curr_TimeToMILIT = drow("sh_TimeToMILIT")

                                            .curr_RegHrsWork = drow("RegularHrsWork")

                                            'isNightShift = 0 'DAY SHIFT

                                        Else '                              'NIGHT SHIFT

                                            .currNightEShiftID = drow("esh_RowID")
                                            .currNightShiftID = drow("sh_RowID")

                                            .curr_NightTimeFromMILIT = drow("sh_TimeFromMILIT")
                                            .curr_NightTimeToMILIT = drow("sh_TimeToMILIT")

                                            .curr_RegHrsWork = drow("RegularHrsWork")

                                        End If

                                    Next

                                    If .dgvEmployi.RowCount <> 0 Then

                                        dt_etent = retAsDatTbl("SELECT RowID" & _
                                                               ", COALESCE(TIME_FORMAT(TimeIn,'%H:%i:%s'),'') 'TimeIn'" & _
                                                               ", COALESCE(TIME_FORMAT(TimeOut,'%H:%i:%s'),'') 'TimeOut'" & _
                                                               ", COALESCE(DATE_FORMAT(Date,'%m-%d-%Y'),'') 'Date'" & _
                                                               ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF('" & .curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF('" & .curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),'') 'IsLate'" & _
                                                               ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_TimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_TimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'IsUT'" & _
                                                               ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'17:00:00'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'17:00:00'),':',-2),':',1) AS DECIMAL) / 60),'') 'OT'" & _
                                                               ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(IF(TimeIn>TimeOut,ADDTIME(TimeOut, '24:00:00'),TimeOut),'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(IF(TimeIn>TimeOut,ADDTIME(TimeOut, '24:00:00'),TimeOut),'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),0) 'HRS_workd'" & _
                                                               ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF('" & .curr_NightTimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF('" & .curr_NightTimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),'') 'IsLateNight'" & _
                                                               ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_NightTimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_NightTimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'IsUTNight'" & _
                                                               ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_NightTimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_NightTimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'OTNight'" & _
                                                               " FROM employeetimeentrydetails" & _
                                                               " WHERE EmployeeID=" & .dgvEmployi.CurrentRow.Cells("RowID").Value & _
                                                               " AND DATE BETWEEN DATE('" & dateFrom & "') AND DATE('" & dateTo & "')" & _
                                                               " AND OrganizationID=" & orgztnID & _
                                                               " ORDER BY RowID ASC;") '" ORDER BY DATE_FORMAT(Created,'%H') ASC;")
                                    Else
                                        dt_etent = Nothing
                                    End If

                                    Dim hrs_workd As Object = dt_etent.Compute("SUM(HRS_workd)", _
                                                                               "Date='" & month & "-" & i_dateDay & "-" & year_payp & "'")

                                    hrswrkd = If(IsDBNull(hrs_workd), _
                                                 0, _
                                                 hrs_workd)

                                    OTval = Nothing
                                    UTval = Nothing

                                    If Val(.curr_RegHrsWork) <> 0 Then
                                        '                                     'itong - 1 dito ay para sa 1 hour lunch break
                                        Dim hrswokd_OT = hrswrkd - (.curr_RegHrsWork - 1)

                                        If hrswokd_OT >= 0 Then
                                            OTval = hrswokd_OT
                                        Else
                                            UTval = hrswokd_OT
                                        End If

                                    End If

                                    Dim lateDayNight() As DataRow = _
                                        dt_etent.Select("Date='" & month & "-" & i_dateDay & "-" & year_payp & "'", _
                                                        "TimeIn ASC")

                                    Lateval = Nothing

                                    For Each drw In lateDayNight 'kapag negative value, lagpas ang time in ni employee sa tamang TimeIn
                                        'For c = 0 To dt_etent.Columns.Count - 1
                                        'If drw("IsLate").ToString <> "" Then
                                        '    'Lateval = If(drw("IsLate").ToString.Contains("-"), _
                                        '    '             Val(drw("IsLate")) * -1, _
                                        '    '             0)
                                        Lateval = DateDiff(DateInterval.Minute, _
                                                           CDate(drw("TimeIn")), _
                                                           If(.curr_NightTimeFromMILIT = Nothing, CDate(.curr_TimeFromMILIT), CDate(.curr_NightTimeFromMILIT)))

                                        Lateval = If(Lateval < 0, _
                                                     (Lateval * -1) / 60, _
                                                     0)
                                        'Else
                                        '    Lateval = If(drw("IsLateNight").ToString.Contains("-"), _
                                        '                 Val(drw("IsLateNight")) * -1, _
                                        '                 0)
                                        'End If
                                        'MsgBox(Lateval.ToString)
                                        'Next
                                        'MsgBox(drw("RowID").ToString)
                                        Exit For
                                    Next

                                    'Dim undrtimeDayNight() As DataRow = _
                                    '    dt_etent.Select("Date='" & month & "-" & i_dateDay & "-" & year_payp & "'", _
                                    '                    "TimeIn DESC")

                                    'UTval = Nothing

                                    ''MsgBox(undrtimeDayNight.Count.ToString)

                                    'For Each d_row In undrtimeDayNight 'kapag negative value, maagang nag-TimeOut si employee
                                    '    'If undrtimeDayNight.Count <> 0 Then
                                    '    If d_row("IsUT").ToString <> "" Then
                                    '        UTval = If(d_row("IsUT").ToString.Contains("-"), Val(d_row("IsUT")), 0)
                                    '    Else
                                    '        UTval = If(d_row("IsUTNight").ToString.Contains("-"), Val(d_row("IsUTNight")), 0)
                                    '    End If
                                    '    'End If

                                    '    'MsgBox(d_row("RowID").ToString)
                                    '    Exit For
                                    'Next

                                    'Dim OverTime() As DataRow = _
                                    '    dt_etent.Select("Date='" & month & "-" & i_dateDay & "-" & year_payp & "'", _
                                    '                    "TimeIn ASC")

                                    'OTval = Nothing

                                    ''MsgBox(undrtimeDayNight.Count.ToString)

                                    'ot_hrsworkd = 0

                                    'For Each d_row In OverTime
                                    '    Dim OTmins = DateDiff(DateInterval.Minute, _
                                    '                          CDate(d_row("TimeOut")), _
                                    '                          CDate(.curr_NightTimeToMILIT))

                                    '    Dim OTminsTO

                                    '    Dim OTminsTI

                                    '    If d_row("IsUT").ToString <> "" Then
                                    '        OTminsTO = DateDiff(DateInterval.Minute, _
                                    '                            CDate(d_row("TimeOut")), _
                                    '                            CDate(.curr_TimeToMILIT))

                                    '        OTminsTI = DateDiff(DateInterval.Minute, _
                                    '                            CDate(d_row("TimeIn")), _
                                    '                            CDate(.curr_TimeToMILIT))

                                    '        If OTminsTI >= 0 And OTminsTO >= 0 Then
                                    '            ot_hrsworkd += (DateDiff(DateInterval.Minute, _
                                    '                            CDate(d_row("TimeIn")), _
                                    '                            CDate(d_row("TimeIn"))) / 60)

                                    '        ElseIf OTminsTO >= 0 Then
                                    '            ot_hrsworkd += (OTmins / 60)
                                    '        End If
                                    '        'OTval
                                    '    Else
                                    '        OTminsTO = DateDiff(DateInterval.Minute, _
                                    '                            CDate(d_row("TimeOut")), _
                                    '                            CDate(.curr_NightTimeToMILIT))

                                    '        OTminsTI = DateDiff(DateInterval.Minute, _
                                    '                            CDate(d_row("TimeIn")), _
                                    '                            CDate(.curr_NightTimeToMILIT))

                                    '        If OTminsTI >= 0 And OTminsTO >= 0 Then
                                    '            ot_hrsworkd += (DateDiff(DateInterval.Minute, _
                                    '                            CDate(d_row("TimeIn")), _
                                    '                            CDate(d_row("TimeIn"))) / 60)

                                    '        ElseIf OTminsTO >= 0 Then
                                    '            ot_hrsworkd += (OTmins / 60)
                                    '        End If
                                    '        'OTval

                                    '    End If
                                    'Next

                                    'OTval = ot_hrsworkd
                                    ''CDate("17:50:00")

                                    Dim the_paypdate = year_payp & "-" & month & "-" & i_dateDay

                                    If .dgvEmployi.RowCount <> 0 Then
                                        '.dgvEmployi.Focus()

                                        Dim nightshiftstart = EXECQUER("SELECT COALESCE(NightShiftTimeFrom,'') 'NightShiftTimeFrom' FROM organization WHERE RowID=" & orgztnID & ";")

                                        nightshiftstart = If(Trim(nightshiftstart) = "", "20:00:00", nightshiftstart)

                                        Dim isNightShift = If(.curr_TimeFromMILIT = Nothing, _
                                                              DateDiff(DateInterval.Minute, CDate(.curr_NightTimeFromMILIT), CDate(nightshiftstart)) / 60, _
                                                              DateDiff(DateInterval.Minute, CDate(.curr_TimeFromMILIT), CDate(nightshiftstart)))

                                        isNightShift = If(isNightShift <= 0, 1, 0)

                                        EmpTimeEntry.INSUPD_employeetimeentry(, _
                                            the_paypdate, _
                                            If(.currNightEShiftID <> Nothing, .currNightEShiftID, .currEShiftID), _
                                            .dgvEmployi.CurrentRow.Cells("RowID").Value, _
                                            , , hrswrkd, OTval, UTval, , , Lateval, , , , , isNightShift)

                                    End If

                                    '    '*****************DAY SHIFT****************
                                    '    If drows("IsUT").ToString.Contains("-") Then
                                    '        drows("IsUT") = drows("IsUT").ToString.Replace("-", "")
                                    '        UTval = Val(drows("IsUT"))
                                    '    End If

                                    '    If drows("OT").ToString.Contains("-") = False Then
                                    '        drows("OT") = drows("OT").ToString.Replace("-", "")
                                    '        OTval += Val(drows("OT"))
                                    '    End If
                                    '    '*****************DAY SHIFT****************

                                    '    '*****************NIGHT SHIFT****************
                                    '    If drows("IsUTNight").ToString.Contains("-") Then
                                    '        drows("IsUTNight") = drows("IsUTNight").ToString.Replace("-", "")
                                    '        UTNightval = Val(drows("IsUTNight"))
                                    '    End If

                                    '    If drows("OT").ToString.Contains("-") Then
                                    '        drows("OTNight") = drows("OTNight").ToString.Replace("-", "")
                                    '        OTNightval += Val(drows("OTNight"))
                                    '    End If
                                    '    '*****************NIGHT SHIFT****************

                                Next

                            Else

                                'dt_etent = retAsDatTbl("SELECT RowID" & _
                                '                       ", COALESCE(TIME_FORMAT(TimeIn,'%H:%i:%s'),'') 'TimeIn'" & _
                                '                       ", COALESCE(TIME_FORMAT(TimeOut,'%H:%i:%s'),'') 'TimeOut'" & _
                                '                       ", COALESCE(DATE_FORMAT(DATE,'%m-%d-%Y'),'') 'Date'" & _
                                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF('" & .curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF('" & .curr_TimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),'') 'IsLate'" & _
                                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_TimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_TimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'IsUT'" & _
                                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'17:00:00'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'17:00:00'),':',-2),':',1) AS DECIMAL) / 60),'') 'OT'" & _
                                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'), TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),0) 'HRS_workd'" & _
                                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF('" & .curr_NightTimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF('" & .curr_NightTimeFromMILIT & "', TIME_FORMAT(TimeIn,'%H:%i:%s')),':',-2),':',1) AS DECIMAL) / 60),'') 'IsLateNight'" & _
                                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_NightTimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_NightTimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'IsUTNight'" & _
                                '                       ", COALESCE(CAST(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_NightTimeToMILIT & "'),':',1) AS DECIMAL) + (CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TIMEDIFF(TIME_FORMAT(TimeOut,'%H:%i:%s'),'" & .curr_NightTimeToMILIT & "'),':',-2),':',1) AS DECIMAL) / 60),'') 'OTNight'" & _
                                '                       " FROM employeetimeentrydetails" & _
                                '                       " WHERE EmployeeID=" & EmpTimeEntry.dgvEmployi.CurrentRow.Cells("RowID").Value & _
                                '                       " AND DATE= DATE('" & year_payp & "-" & month_paypFrom & "-" & i & "') " & _
                                '                       " AND OrganizationID=" & orgztnID & _
                                '                       " ORDER BY RowID ASC;") '" ORDER BY DATE_FORMAT(Created,'%H') ASC;")

                            End If

                            'For Each drows As DataRow In dt_etent.Rows

                            '    hrswrkd += Val(drows("HRS_workd")) 'Total Hour(s) worked

                            '    If firstrow = 0 Then
                            '        firstrow = 1

                            '        'txthrsUT.Text = UTval.ToString 'Total Undertime hour(s) 

                            '        If Val(drows("IsLateNight")) <= 0 Then
                            '            drows("IsLateNight") = drows("IsLateNight").ToString.Replace("-", "")
                            '            Lateval = Val(drows("IsLateNight"))
                            '        Else

                            '            If drows("IsLate").ToString.Contains("-") Then
                            '                'cbolateabsent.SelectedIndex = 1
                            '                'drows("IsLate") = drows("IsLate").ToString.Replace("-", "")
                            '                'Lateval = Val(drows("IsLate"))
                            '            Else
                            '                'cbolateabsent.SelectedIndex = -1
                            '            End If

                            '            'drows("IsLate") = drows("IsLate").ToString.Replace("-", "")
                            '            'UTval = Val(drows("IsLate"))
                            '        End If

                            '    End If
                            '    '*****************DAY SHIFT****************
                            '    If drows("IsUT").ToString.Contains("-") Then
                            '        drows("IsUT") = drows("IsUT").ToString.Replace("-", "")
                            '        UTval = Val(drows("IsUT"))
                            '    End If

                            '    If drows("OT").ToString.Contains("-") = False Then
                            '        drows("OT") = drows("OT").ToString.Replace("-", "")
                            '        OTval += Val(drows("OT"))
                            '    End If
                            '    '*****************DAY SHIFT****************

                            '    '*****************NIGHT SHIFT****************
                            '    If drows("IsUTNight").ToString.Contains("-") Then
                            '        drows("IsUTNight") = drows("IsUTNight").ToString.Replace("-", "")
                            '        UTNightval = Val(drows("IsUTNight"))
                            '    End If

                            '    If drows("OT").ToString.Contains("-") Then
                            '        drows("OTNight") = drows("OTNight").ToString.Replace("-", "")
                            '        OTNightval += Val(drows("OTNight"))
                            '    End If
                            '    '*****************NIGHT SHIFT****************

                            'Next

                            'firstrow = 0

                            'txthrsworkd.Text = hrswrkd

                            'If curr_TimeFromMILIT = Nothing _
                            '                    And curr_TimeToMILIT = Nothing Then 'SHIFT IS NOT DEFINED

                            '    txthrsworkd.Text = hrswrkd + OTval

                            '    If curr_NightTimeFromMILIT <> Nothing _
                            '                    And curr_NightTimeToMILIT <> Nothing Then 'NIGHT SHIFT

                            '        txthrsworkd.Text = hrswrkd

                            '        txtnightdiffOT.Text = OTNightval

                            '        txthrsUT.Text = UTNightval

                            '    End If
                            'ElseIf curr_TimeFromMILIT <> Nothing _
                            '                    And curr_TimeToMILIT <> Nothing Then 'DAY SHIFT

                            '    txthrsworkd.Text = hrswrkd

                            '    txthrsOT.Text = OTval

                            '    txthrsUT.Text = UTval

                            'End If

                            ''txthrsUT.Text = If(OTval > 0, 0, UTval)

                            ''********************
                            'cboOverUnderTime.SelectedIndex = If(OTval > 0 Or OTNightval > 0, 0, If(dt_etent.Rows.Count = 0, -1, 1))
                            ''DEPENDE PA ANG OVER TIME, 
                            ''EXAMPLE KUNG MAY APPROVAL PA BA? 
                            ''NO OVER TIME
                            '',KUNG MAY ALLOWABLE HOUR(S) FOR OVER TIME
                            ''********************

                        Else

                        End If

                    End If

                End With

                'EmpTimeEntry.dtppayperiod_ValueChanged(sender, e)

            End With

        End If

        Me.Hide()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        employee_dattab = retAsDatTbl("SELECT e.RowID" & _
                                      ",e.StartDate" & _
                                      " FROM employee e" & _
                                      " INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID" & _
                                      " INNER JOIN position ps ON ps.RowID=e.PositionID AND ps.DivisionId IN (" & cboxDivisions.Tag & ")" & _
                                      " WHERE e.OrganizationID=" & orgztnID & _
                                      " AND pf.PayFrequencyType='" & quer_empPayFreq & _
                                      "' GROUP BY e.RowID" & _
                                      " ORDER BY RowID;")

        If selectdayFrom = Nothing Or selectdayFrom = Nothing Then
            dgvpayper_SelectionChanged(sender, e)
        End If

        Dim blankTimeOut = _
            EXECQUER("SELECT COUNT(etd.RowID)" &
                     " FROM employeetimeentrydetails etd" &
                     " WHERE etd.OrganizationID=" & orgztnID & "" &
                     " AND etd.EmployeeID IS NOT NULL" &
                     " AND (IFNULL(etd.TimeIn,'')='' OR IFNULL(etd.TimeOut,'')='')" &
                     " AND etd.`Date`" &
                     " BETWEEN '" & MYSQLDateFormat(CDate(selectdayFrom)) & "'" &
                     " AND '" & MYSQLDateFormat(CDate(selectdayTo)) & "'" &
                     " LIMIT 1;")

        If blankTimeOut >= 1 Then
            
            ' ''Dim prompt = MessageBox.Show("It seems that there were blank time logs." & vbNewLine & _
            ' ''                             "Would you to correct it first ?", _
            ' ''                             "Blank Time Logs", MessageBoxButtons.YesNo, MessageBoxIcon.Information)

            ' ''If prompt = Windows.Forms.DialogResult.Yes Then

            ' ''    'MDIPrimaryForm.ToolStripButton3_Click(sender, e)

            ' ''    'TimeAttendForm.TimeEntryToolStripMenuItem_Click(sender, e)

            Me.BringToFront()

            Dim n_BlankTimeEntryLogs As New BlankTimeEntryLogs(dayFrom, dayTo)

            If n_BlankTimeEntryLogs.ShowDialog("") = Windows.Forms.DialogResult.OK Then

                Button1_Click(Button1, New EventArgs)

            End If

            ' ''    'Me.Close()

            Exit Sub

            ' ''Else 'If prompt = Windows.Forms.DialogResult.Cancel Then

            ' ''    Exit Sub

            ' ''End If

        ElseIf blankTimeOut = 0 Then

            Dim timelogs =
                EXECQUER(
                    String.Concat("SELECT EXISTS(SELECT etd.RowID",
                         " FROM employeetimeentrydetails etd",
                         " INNER JOIN employee e ON e.RowID=etd.EmployeeID AND e.OrganizationID=etd.OrganizationID",
                         " INNER JOIN division dv ON dv.RowID=", ValNoComma(cboxDivisions.Tag),
                         " INNER JOIN `position` pos ON pos.RowID=e.PositionID AND pos.DivisionId=dv.RowID",
                         " WHERE etd.OrganizationID=", orgztnID,
                         " AND etd.`Date`",
                         " BETWEEN '", Format(CDate(selectdayFrom), "yyyy-MM-dd"), "'",
                         " AND '", Format(CDate(selectdayTo), "yyyy-MM-dd"), "');"))

            If timelogs = 1 Then

            Else

                MsgBox("There are no time logs within this pay period." & vbNewLine & _
                       "Please prepare the time logs first.",
                       MsgBoxStyle.Information,
                       "")

                Exit Sub

            End If

        End If

        'compute_employeetimeentry()

        progbar.Value = 0

        EmpTimeEntry.First.Enabled = False

        EmpTimeEntry.Prev.Enabled = False

        EmpTimeEntry.Nxt.Enabled = False

        EmpTimeEntry.Last.Enabled = False



        'EmpTimeEntry.ToolStrip1.Enabled = False

        progbar.Visible = True

        'MDIPrimaryForm.systemprogressbar.Visible = True

        Button1.Enabled = False

        'Dim etentdet_for_this_payp As New DataTable

        'etentdet_for_this_payp = retAsDatTbl("SELECT * FROM employeetimeentrydetails WHERE Date BETWEEN '" & _
        '                                     Format(CDate(dgvpayper.CurrentRow.Cells("Pay period from").Value), "yyyy-MM-dd") & _
        '                                     "' AND '" & Format(CDate(dgvpayper.CurrentRow.Cells("Pay period to").Value), "yyyy-MM-dd") & _
        '                                     "' GROUP BY Created;")

        'If etentdet_for_this_payp.Rows.Count >= 2 Then 

        'End If

        RemoveHandler EmpTimeEntry.dgvEmployi.SelectionChanged, AddressOf EmpTimeEntry.dgvEmployi_SelectionChanged

        RemoveHandler EmpTimeEntry.dgvcalendar.CurrentCellChanged, AddressOf EmpTimeEntry.dgvcalendar_CurrentCellChanged

        TimeAttendForm.MenuStrip1.Enabled = False

        MDIPrimaryForm.Showmainbutton.Enabled = False

        dgvpayper.Focus()

        bgworkRECOMPUTE_employeeleave.RunWorkerAsync()

    End Sub

    'FIRST_METHOD
    Private Sub bgWork_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgWork.DoWork

        backgroundworking = 1

        'compute_employeetimeentry()
        '" AND EmploymentStatus IS NOT IN ('Resigned','Terminated')" & _

        'employee_dattab

        'Dim firstbound = If(selectdayFrom = Nothing, _
        '                    CDate(selectdayTo).Day, _
        '                    CDate(selectdayFrom).Day)

        'Dim lastbound = If(selectdayTo = Nothing, _
        '                    CDate(selectdayFrom).Day, _
        '                    CDate(selectdayTo).Day - CDate(selectdayFrom).Day)

        'firstbound = 1

        Dim lastbound = DateDiff(DateInterval.Day, _
                                 CDate(selectdayFrom), _
                                 CDate(selectdayTo)) 'CDate(selectdayTo).Day

        'If(selectdayTo = Nothing, _
        '                    firstbound, _
        '                    CDate(selectdayTo).Day - CDate(selectdayFrom).Day)

        'month_paypFrom = If(selectdayFrom = Nothing, _
        '                    CDate(selectdayTo).Month, _
        '                    CDate(selectdayFrom).Month)

        'Dim month = If(month_paypFrom.ToString.Length = 1, "0" & month_paypFrom, month_paypFrom)

        'year_payp = If(selectdayFrom = Nothing, _
        '               current_years, _
        '               CDate(selectdayFrom).Year)

        'CDate(EmpTimeEntry.today_date).Year
        'firstbound
        'lastbound -= 1

        '*******************************************************************************

        'For i_date = 0 To lastbound

        '    'Dim i_dateDay = If(i_date.ToString.Length = 1, "0" & i_date, i_date)

        '    Dim indx_date As Object = CDate(selectdayFrom).AddDays(i_date) 'year_payp & "-" & month & "-" & i_dateDay

        '    indx_date = Format(indx_date, "yyyy-MM-dd")

        '    For Each drow As DataRow In employee_dattab.Rows
        '        computehrswork_employeetimeentry(drow("RowID"), _
        '                                         indx_date, _
        '                                         drow("StartDate"))

        '    Next

        '    bgWork.ReportProgress(CInt(100 * i_date / lastbound), "")

        'Next













        'Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT CONCAT('SELECT GENERATE_employeetimeentry(', e.RowID,',', e.OrganizationID,',\'', d.DateValue,'\'," & z_User & ");')" & _
        '                                                     " FROM dates d" & _
        '                                                     " INNER JOIN employee e ON e.OrganizationID=" & orgztnID & _
        '                                                     " INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID AND pf.PayFrequencyType='" & quer_empPayFreq & "'" & _
        '                                                     " INNER JOIN position ps ON ps.RowID=e.PositionID AND ps.DivisionId='" & division_selectedvalue & "'" & _
        '                                                     " WHERE d.DateValue" & _
        '                                                     " BETWEEN '" & Format(CDate(selectdayFrom), "yyy-MM-dd") & "' AND '" & Format(CDate(selectdayTo), "yyyy-MM-dd") & "';")

        'Dim output_filepath = Path.GetTempPath & Convert.ToString("\sqldumpfile.txt")

        'If File.Exists(output_filepath) Then
        '    File.Delete(output_filepath)
        'End If

        ''Using outputFile As New StreamWriter(output_filepath)

        ''End Using

        'For Each drow As DataRow In n_SQLQueryToDatatable.ResultTable.Rows
        '    'outputFile.WriteLine(drow(0))
        '    'File.AppendAllText(output_filepath,
        '    '                   drow(0))

        '    'Dim n_ExecuteQuery As New ExecuteQuery(drow(0))

        'Next

        'Dim sql_script = Nothing

        'sql_script = File.ReadAllText(output_filepath)

        'Dim n_ExecuteQuery As New ExecuteQuery("CALL EXEC_sql_from_file('" & sql_script & "');")
        '",'" & division_selectedvalue & "'" & _
        Dim n_ExecuteQuery As _
                New ExecuteQuery("CALL MASS_generate_employeetimeentry('" & orgztnID & "'" & _
                                 ",'" & quer_empPayFreq & "'" & _
                                 ",'" & cboxDivisions.Tag & "'" & _
                                 ",'" & z_User & "'" & _
                                 ",'" & Format(CDate(selectdayFrom), "yyy-MM-dd") & "'" & _
                                 ",'" & Format(CDate(selectdayTo), "yyy-MM-dd") & "');",
                                 999999)


        n_ExecuteQuery = _
            New ExecuteQuery("CALL RECOMPUTE_agencytotalbill('" & orgztnID & "', '" & dayFrom & "', '" & dayTo & "', '" & z_User & "');")

        bgWork.ReportProgress(100, "")

        '*******************************************************************************

        'Dim LeftPartString = "UPDATE employeetimeentry SET "

        'Dim RightPartString = "=0.0 WHERE OrganizationID='" & orgztnID & "'"

        'Dim eteColNames As New AutoCompleteStringCollection

        'enlistTheLists("SELECT ii.COLUMN_NAME" & _
        '               " FROM information_schema.`COLUMNS` ii" & _
        '               " WHERE ii.TABLE_SCHEMA='" & sys_db & "'" & _
        '               " AND ii.TABLE_NAME='employeetimeentry'" & _
        '               " AND ii.DATA_TYPE='decimal';",
        '               eteColNames)

        'Dim strResult = String.Empty

        'For Each strval In eteColNames

        '    strResult &= LeftPartString & strval & RightPartString & " AND " & strval & " IS NULL;"

        'Next

        'EXECQUER(strResult)

    End Sub

    'BEFORE_LAST_METHOD
    Private Sub bgWork_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgWork.ProgressChanged

        progbar.Value = CType(e.ProgressPercentage, Integer)

        MDIPrimaryForm.systemprogressbar.Value = CType(e.ProgressPercentage, Integer)

    End Sub

    'LAST_METHOD
    Private Sub bgWork_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgWork.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox("Error: " & vbNewLine & e.Error.Message)
            'MessageBox.Show
        ElseIf e.Cancelled Then
            MsgBox("Background work cancelled.", _
                   MsgBoxStyle.Exclamation)
        Else
            MsgBox("Done computing the hours worked.", _
                   MsgBoxStyle.Information)

            If me_close = 1 Then
                me_close = 0
            End If

            Me.Close()
        End If

        EmpTimeEntry.First.Enabled = True

        EmpTimeEntry.Prev.Enabled = True

        EmpTimeEntry.Nxt.Enabled = True

        EmpTimeEntry.Last.Enabled = True

        EmpTimeEntry.ToolStrip1.Enabled = True

        TimeAttendForm.MenuStrip1.Enabled = True

        MDIPrimaryForm.Showmainbutton.Enabled = True

        progbar.Visible = False
        MDIPrimaryForm.systemprogressbar.Visible = False
        MDIPrimaryForm.systemprogressbar.Value = Nothing
        Button1.Enabled = True

        employee_dattab = Nothing

        EmpTimeEntry.dgvEmployi_SelectionChanged(sender, e)

        EmpTimeEntry.dgvEmployi_SelectionChanged(sender, e)

        backgroundworking = 0

        AddHandler EmpTimeEntry.dgvEmployi.SelectionChanged, AddressOf EmpTimeEntry.dgvEmployi_SelectionChanged

        AddHandler EmpTimeEntry.dgvcalendar.CurrentCellChanged, AddressOf EmpTimeEntry.dgvcalendar_CurrentCellChanged

    End Sub

    Function computehrswork_employeetimeentry(Optional etent_EmployeeID As Object = Nothing, _
                                              Optional etent_Date As Object = Nothing, _
                                              Optional employee_startdate As Object = Nothing) As Object
        Dim return_value = Nothing
        Try
            If new_conn.State = ConnectionState.Open Then : new_conn.Close() : End If

            new_cmd = New MySqlCommand("COMPUTE_employeetimeentry", new_conn)

            new_conn.Open()

            With new_cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("etentID", MySqlDbType.Int32)

                .Parameters.AddWithValue("etent_EmployeeID", etent_EmployeeID)

                .Parameters.AddWithValue("etent_OrganizationID", orgztnID)

                .Parameters.AddWithValue("etent_Date", etent_Date)

                .Parameters.AddWithValue("etent_CreatedBy", z_User)

                .Parameters.AddWithValue("etent_LastUpdBy", z_User)

                .Parameters.AddWithValue("EmployeeStartDate", employee_startdate)

                .Parameters("etentID").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()
                'computehrswork_employeetimeentry
                return_value = datread(0)

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " " & "COMPUTE_employeetimeentry", , "Error")
        Finally
            new_conn.Close()
            new_cmd.Dispose()
        End Try
        Return return_value
    End Function

    Private Sub cbomonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbomonth.SelectedIndexChanged
        Button2_Click(sender, e)

    End Sub

    Private Sub dgvpaypers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpaypers.CellContentClick

    End Sub

    Dim selectdayFrom As Object = Nothing
    Dim selectdayTo As Object = Nothing

    Dim dayFrom As Object = Nothing
    Dim dayTo As Object = Nothing

    Private Sub dgvpayper_SelectionChanged(sender As Object, e As EventArgs) Handles dgvpaypers.SelectionChanged 'dgvpayper

        If dgvpaypers.RowCount <> 0 Then 'dgvpayper

            If dgvpaypers.CurrentRow IsNot Nothing Then
                With dgvpaypers.CurrentRow
                    selectdayFrom = .Cells("DataGridViewTextBoxColumn2").Value 'Pay period from
                    selectdayTo = .Cells("DataGridViewTextBoxColumn3").Value 'Pay period to

                    dayFrom = Format(CDate(selectdayFrom), "yyyy-MM-dd")
                    dayTo = Format(CDate(selectdayTo), "yyyy-MM-dd")

                End With

            Else
                selectdayFrom = Nothing
                selectdayTo = Nothing

                dayFrom = Nothing
                dayTo = Nothing

            End If

        Else
            selectdayFrom = Nothing
            selectdayTo = Nothing

            dayFrom = Nothing
            dayTo = Nothing

        End If

    End Sub

    Private Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        Panel2.Enabled = False
        current_years = Val(current_years) - 1

        txtYr.Text = current_years

        Button2_Click(sender, e)

        linkPrev.Text = "← " & (current_years - 1)
        linkNxt.Text = (current_years + 1) & " →"
        Panel2.Enabled = True
    End Sub

    Private Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        Panel2.Enabled = False
        current_years = Val(current_years) + 1

        txtYr.Text = current_years

        Button2_Click(sender, e)

        linkPrev.Text = "← " & (current_years - 1)
        linkNxt.Text = (current_years + 1) & " →"
        Panel2.Enabled = True
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkRECOMPUTE_employeeleave.DoWork

        Dim n_ExecuteQuery As _
            New ExecuteQuery("CALL RESET_employeeleave_duplicate('" & orgztnID & "','" & dayFrom & "','" & dayTo & "'" &
                             ",'" & quer_empPayFreq & "','" & cboxDivisions.Tag & "');", 999999)


        n_ExecuteQuery = New ExecuteQuery("CALL RECOMPUTE_employeeleave('" & orgztnID & "','" & dayFrom & "','" & dayTo & "');", 999999)


        bgworkRECOMPUTE_employeeleave.ReportProgress(50, "")
    End Sub

    Private Sub bgworkRECOMPUTE_employeeleave_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkRECOMPUTE_employeeleave.ProgressChanged

        progbar.Value = CType(e.ProgressPercentage, Integer)

        MDIPrimaryForm.systemprogressbar.Value = CType(e.ProgressPercentage, Integer)

    End Sub

    Private Sub bgworkRECOMPUTE_employeeleave_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkRECOMPUTE_employeeleave.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox("Error: " & vbNewLine & e.Error.Message & vbNewLine & "bgworkRECOMPUTE_employeeleave_RunWorkerCompleted")

        ElseIf e.Cancelled Then
            MsgBox("Background work cancelled.", _
                   MsgBoxStyle.Exclamation)
        Else


            bgWork.RunWorkerAsync()


        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        Dim n_link As New System.Windows.Forms.LinkLabel.Link

        If keyData = Keys.Escape Then

            If Panel1.Enabled Then

                Me.Close()

            End If

            Return True

        ElseIf keyData = Keys.Left Then

            If Panel1.Enabled Then

                n_link.Name = "linkPrev"

                linkPrev_LinkClicked(linkPrev, New LinkLabelLinkClickedEventArgs(n_link))

            End If

            Return True

        ElseIf keyData = Keys.Right Then

            If Panel1.Enabled Then

                n_link.Name = "linkNxt"

                linkNxt_LinkClicked(linkNxt, New LinkLabelLinkClickedEventArgs(n_link))

            End If

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub Button1_EnabledChanged(sender As Object, e As EventArgs) Handles Button1.EnabledChanged
        Dim enable_property = Button1.Enabled

        Panel1.Enabled = enable_property

        cboxDivisions.Enabled = enable_property

    End Sub

    Dim division_selectedvalue = Nothing

    Private Sub cboxDivisions_DropDown(sender As Object, e As EventArgs) Handles cboxDivisions.DropDown

        Static cb_font As Font = cboxDivisions.Font

        'Dim cb_width As Integer = cboxDivisions.DropDownWidth

        Dim grp As Graphics = cboxDivisions.CreateGraphics()

        Dim vertScrollBarWidth As Integer = If(cboxDivisions.Items.Count > cboxDivisions.MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0)

        Dim wiidth As Integer = 0

        Dim data_source As New DataTable

        data_source = cboxDivisions.DataSource

        Dim i = 0

        Dim drp_downwidhths As Integer()

        ReDim drp_downwidhths(data_source.Rows.Count - 1)

        For Each strRow As DataRow In data_source.Rows

            wiidth = CInt(grp.MeasureString(CStr(strRow(1)), cb_font).Width) + vertScrollBarWidth

            drp_downwidhths(i) = wiidth

            'If cb_width < wiidth Then
            '    wiidth = wiidth
            'End If

            i += 1

        Next

        Dim max_drp_downwidhth As Integer = drp_downwidhths.Max

        cboxDivisions.DropDownWidth = max_drp_downwidhth 'wiidth, cb_width

    End Sub

    Private Sub cboxDivisions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboxDivisions.SelectedIndexChanged
        division_selectedvalue = cboxDivisions.SelectedValue

        cboxDivisions.Tag = division_selectedvalue
        'New ExecuteQuery("SELECT GROUP_CONCAT(RowID)" &
        '                 " FROM `division`" &
        '                 " WHERE OrganizationID='" & orgztnID & "'" &
        '                 " AND Name='" & cboxDivisions.Text & "'" &
        '                 " AND ParentDivisionID IS NOT NULL;").Result

    End Sub

End Class