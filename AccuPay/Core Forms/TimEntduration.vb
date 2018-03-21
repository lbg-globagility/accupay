Imports MySql.Data.MySqlClient
Imports System.IO

Public Class TimEntduration

    Dim day_today As Integer = Val(CDate(EmpTimeEntry.today_date).Day)

    Dim month_today As Integer = Val(CDate(EmpTimeEntry.today_date).Month)

    Dim year_today As Integer = Val(CDate(EmpTimeEntry.today_date).Year)

    Dim new_conn As New MySqlConnection
    Dim new_cmd As New MySqlCommand

    Dim current_years = 0

    Public Event DoneGenerating()

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(dateToday As DateTime)
        Me.New()

        day_today = dateToday.Day
        month_today = dateToday.Month
        year_today = dateToday.Year
    End Sub

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

            Dim allOptionRow = n_SQLQueryToDatatable.ResultTable.NewRow()
            allOptionRow("RowID") = DBNull.Value
            allOptionRow("Name") = "All"

            n_SQLQueryToDatatable.ResultTable.Rows.InsertAt(allOptionRow, 0)

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

            date_month = Format(CDate(txtYr.Text & "-01-01").AddMonths(i),
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

    Dim quer_empPayFreq = "SEMI-MONTHLY"

    Sub PayFreq_Changed(sender As Object, e As EventArgs)

        quer_empPayFreq = "SEMI-MONTHLY"

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

        loadpayp(Trim(txtYr.Text) & "-" & sel_month & "-01",
                 quer_empPayFreq)

    End Sub

    Sub loadpayp(Optional param_Date As Object = Nothing,
                 Optional PayFreqType As Object = "SEMI-MONTHLY")
        Dim dt_payp As New DataTable

        Dim _params =
            New Object() {orgztnID,
                          If(param_Date = Nothing, Year(Now) & "-01-01", Format(CDate(param_Date), "yyyy-MM-dd")),
                          PayFreqType}

        dt_payp =
            New SQL("CALL VIEW_payperiod(?og_rowid, ?param_date, ?str_payfreq_type);",
                    _params).GetFoundRows.Tables(0)

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

        loadpayp(Trim(txtYr.Text) & "-" & sel_month & "-01",
                 quer_empPayFreq)

        dgvpayper.Focus()

    End Sub

    Private Sub TextBox5_KeyDown(sender As Object, e As KeyEventArgs) Handles txtYr.KeyDown,
                                                                              cbomonth.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button2_Click(sender, e)
            'ElseIf e.KeyCode = Keys.Escape Then
            '    Me.Close()
        End If

    End Sub

    Dim me_close As SByte = 0

    Private Sub TimEntduration_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If bgWork.IsBusy = False Then

            me_close = 1

            MDIPrimaryForm.Enabled = True

            MDIPrimaryForm.BringToFront()
        Else
            e.Cancel = True
            me_close = 0

        End If

        Dim this_class_count = 0

        For Each form_obj As Form In Application.OpenForms

            If form_obj.Name = "TimEntduration" Then
                this_class_count += 1
            End If

        Next

        If this_class_count = 1 Then
            EmpTimeEntry.tsbtnCloseempawar.Enabled = True
        End If
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If selectdayFrom = Nothing Or selectdayFrom = Nothing Then
            dgvpayper_SelectionChanged(sender, e)
        End If

        Dim listOfValues As ListOfValueCollection = Nothing
        Using context = New PayrollContext()
            Dim lovs = context.ListOfValues.Where(Function(l) l.Type = "Payroll Policy").ToList()
            listOfValues = New ListOfValueCollection(lovs)
        End Using

        If Not listOfValues.GetBoolean("Payroll Policy", "timeinonly") Then
            Dim isValid = CheckForBlankTimeLogs()

            If Not isValid Then
                Exit Sub
            End If
        End If

        progbar.Value = 0

        EmpTimeEntry.First.Enabled = False

        EmpTimeEntry.Prev.Enabled = False

        EmpTimeEntry.Nxt.Enabled = False

        EmpTimeEntry.Last.Enabled = False

        progbar.Visible = True

        Button1.Enabled = False

        RemoveHandler EmpTimeEntry.dgvEmployi.SelectionChanged, AddressOf EmpTimeEntry.dgvEmployi_SelectionChanged

        RemoveHandler EmpTimeEntry.dgvcalendar.CurrentCellChanged, AddressOf EmpTimeEntry.dgvcalendar_CurrentCellChanged

        TimeAttendForm.MenuStrip1.Enabled = False

        MDIPrimaryForm.Showmainbutton.Enabled = False

        dgvpayper.Focus()

        bgworkRECOMPUTE_employeeleave.RunWorkerAsync()

    End Sub

    Private Function CheckForBlankTimeLogs() As Boolean
        Dim divisionId As Integer? = If(IsDBNull(cboxDivisions.Tag), CType(Nothing, Integer?), ValNoComma(cboxDivisions.Tag))

        Dim dateFrom = CDate(selectdayFrom)
        Dim dateTo = CDate(selectdayTo)

        Dim blankSql = String.Empty
        If divisionId.HasValue Then
            blankSql = $"
                SELECT COUNT(etd.RowID)
                FROM employeetimeentrydetails etd
                INNER JOIN employee ee
                ON ee.RowID = etd.EmployeeID
                INNER JOIN position p
                ON p.RowID = ee.PositionID
                INNER JOIN division d
                ON d.RowID = p.DivisionID
                WHERE etd.OrganizationID = {orgztnID} AND
                    etd.EmployeeID IS NOT NULL AND
                    (IFNULL(etd.TimeIn,'')='' OR IFNULL(etd.TimeOut,'')='') AND
                    etd.`Date` BETWEEN '{dateFrom.ToString("s")}' AND '{dateTo.ToString("s")}' AND
                    d.RowID = {divisionId}
                LIMIT 1;
            "
        Else
            blankSql = $"
                SELECT COUNT(etd.RowID)
                FROM employeetimeentrydetails etd
                WHERE etd.OrganizationID = {orgztnID} AND
                    etd.EmployeeID IS NOT NULL AND
                    (IFNULL(etd.TimeIn,'')='' OR IFNULL(etd.TimeOut,'')='') AND
                    etd.`Date` BETWEEN '{dateFrom.ToString("s")}' AND '{dateTo.ToString("s")}'
                LIMIT 1;
            "
        End If

        Dim blankTimeOut = EXECQUER(blankSql)

        If blankTimeOut >= 1 Then

            Me.BringToFront()

            Dim n_BlankTimeEntryLogs As New BlankTimeEntryLogs(dayFrom, dayTo)

            If n_BlankTimeEntryLogs.ShowDialog("") = Windows.Forms.DialogResult.OK Then
                Button1_Click(Button1, New EventArgs)
            End If

            Return False

        ElseIf blankTimeOut = 0 Then

            Dim sql = String.Empty

            If divisionId.HasValue Then
                sql = ($"
                    SELECT EXISTS(
                        SELECT etd.RowID
                        FROM employeetimeentrydetails etd
                        INNER JOIN employee e ON e.RowID=etd.EmployeeID AND e.OrganizationID=etd.OrganizationID
                        INNER JOIN division dv ON dv.RowID={divisionId}
                        INNER JOIN `position` pos ON pos.RowID=e.PositionID AND pos.DivisionId=dv.RowID
                        WHERE etd.OrganizationID='{orgztnID}' AND
                            etd.`Date` BETWEEN '{dateFrom.ToString("s")}' AND '{dateTo.ToString("s")}'
                    );
                ")
            Else
                sql = ($"
                    SELECT EXISTS(
                        SELECT etd.RowID
                        FROM employeetimeentrydetails etd
                        INNER JOIN employee e ON e.RowID=etd.EmployeeID AND e.OrganizationID=etd.OrganizationID
                        INNER JOIN `position` pos ON pos.RowID=e.PositionID
                        WHERE etd.OrganizationID='{orgztnID}' AND
                            etd.`Date` BETWEEN '{dateFrom.ToString("s")}' AND '{dateTo.ToString("s")}'
                    );
                ")
            End If

            Dim hasTimeLogs As Boolean = EXECQUER(sql)

            If Not hasTimeLogs Then
                MsgBox("There are no time logs within this pay period." & vbNewLine &
                       "Please prepare the time logs first.",
                       MsgBoxStyle.Information,
                       "")

                Return False
            End If
        End If

        Return True
    End Function

    'FIRST_METHOD
    Private Sub bgWork_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgWork.DoWork

        backgroundworking = 1

        Dim lastbound = DateDiff(DateInterval.Day,
                                 CDate(selectdayFrom),
                                 CDate(selectdayTo)) 'CDate(selectdayTo).Day

        Try
            If IsDBNull(cboxDivisions.Tag) Then
                Dim n_ExecuteQuery As _
                        New ExecuteQuery("CALL MASS_generate_employeetimeentry('" & orgztnID & "'" &
                                         ",'" & quer_empPayFreq & "'" &
                                         ",NULL" &
                                         ",'" & z_User & "'" &
                                         ",'" & Format(CDate(selectdayFrom), "yyy-MM-dd") & "'" &
                                         ",'" & Format(CDate(selectdayTo), "yyy-MM-dd") & "');",
                                         999999)
            Else
                Dim n_ExecuteQuery As _
                    New ExecuteQuery("CALL MASS_generate_employeetimeentry('" & orgztnID & "'" &
                                     ",'" & quer_empPayFreq & "'" &
                                     ",'" & cboxDivisions.Tag & "'" &
                                     ",'" & z_User & "'" &
                                     ",'" & Format(CDate(selectdayFrom), "yyy-MM-dd") & "'" &
                                     ",'" & Format(CDate(selectdayTo), "yyy-MM-dd") & "');",
                                     999999)
            End If

            Dim n_ExecuteQuery2 =
                New ExecuteQuery("CALL RECOMPUTE_agencytotalbill('" & orgztnID & "', '" & dayFrom & "', '" & dayTo & "', '" & z_User & "');")
        Catch exception As Exception
            MsgBox("An unexpected error has occcured.", exception.Message)
        End Try

        bgWork.ReportProgress(100, "")

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
            MsgBox("Background work cancelled.",
                   MsgBoxStyle.Exclamation)
        Else
            MsgBox("Done computing the hours worked.",
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

        EmpTimeEntry.dgvEmployi_SelectionChanged(sender, e)

        EmpTimeEntry.dgvEmployi_SelectionChanged(sender, e)

        backgroundworking = 0

        RaiseEvent DoneGenerating()

        AddHandler EmpTimeEntry.dgvEmployi.SelectionChanged, AddressOf EmpTimeEntry.dgvEmployi_SelectionChanged

        AddHandler EmpTimeEntry.dgvcalendar.CurrentCellChanged, AddressOf EmpTimeEntry.dgvcalendar_CurrentCellChanged

    End Sub

    Function computehrswork_employeetimeentry(Optional etent_EmployeeID As Object = Nothing,
                                              Optional etent_Date As Object = Nothing,
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

    Dim selectdayFrom As Object = Nothing
    Dim selectdayTo As Object = Nothing

    Dim dayFrom As Object = Nothing
    Dim dayTo As Object = Nothing

    Private Sub dgvpaypers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpaypers.CellContentClick

    End Sub

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
            MsgBox("Background work cancelled.",
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

            i += 1

        Next

        Dim max_drp_downwidhth As Integer = drp_downwidhths.Max

        cboxDivisions.DropDownWidth = max_drp_downwidhth 'wiidth, cb_width

    End Sub

    Private Sub cboxDivisions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboxDivisions.SelectedIndexChanged
        division_selectedvalue = cboxDivisions.SelectedValue

        cboxDivisions.Tag = division_selectedvalue
    End Sub

End Class
