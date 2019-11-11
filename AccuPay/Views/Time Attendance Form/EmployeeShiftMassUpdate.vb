Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Imports Indigo.CollapsibleGroupBox
Imports AccuPay.DB

Public Class EmployeeShiftMassUpdate
    Dim IsNew As Integer = 0
    Dim rowid As Integer

    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Dim machineFirstDayOfWeek As String = RegKey.GetValue("iFirstDayOfWeek").ToString

    Dim machineShortTime As String = RegKey.GetValue("sShortTime").ToString

    Dim ArrayWeekFormat() As String

    Dim ParentDiv As New DataTable

    Dim ChiledDiv As New DataTable

    Private Sub fillemployeelist()
        'If dgvEmplist.Rows.Count = 0 Then
        'ElseCOALESCE(StreetAddress1,' ')
        Dim dt As New DataTable

        'If divisionRowID = 0 Then

        Dim concat_includedEmployees = String.Join(",", includedEmployees.ToArray())
        '                                                                                           e.EmployeeID,e.FirstName,e.LastName
        Dim minimum_search As String = If(txtEmpSearchBox.Text.Trim.Length = 0, "", " AND MATCH(" & TimeAttendForm.emp_ft_col & ") AGAINST('" & txtEmpSearchBox.Text.Trim & "' IN BOOLEAN MODE)")
        '                                                                       OR
        Dim addtnl_query As String = If(concat_includedEmployees.Length > 0, " AND e.RowID IN (" & concat_includedEmployees & ")", "")

        Dim orderby_query As String = " ORDER BY e.RowID DESC" 'If(addtnl_query.Length > 0, " ORDER BY FIELD(e.RowID," & concat_includedEmployees & ")", " ORDER BY e.RowID DESC")

        Dim search_quer1 As String = " SELECT e.RowID,CONCAT_WS(', ',e.Lastname,e.Firstname) as name" &
            ", e.EmployeeID" &
            ", (esh.esdRowID IS NOT NULL) AS IsByDayEncoding" &
            " FROM employee e" &
            " LEFT JOIN (SELECT RowID AS esdRowID,EmployeeID FROM employeeshiftbyday WHERE OrganizationID='" & orgztnID & "' GROUP BY EmployeeID) esh ON esh.EmployeeID=e.RowID" &
            " WHERE e.organizationID = '" & z_OrganizationID & "'" &
            minimum_search

        Dim search_quer2 As String = " SELECT e.RowID,CONCAT_WS(', ',e.Lastname,e.Firstname) as name" &
            ", e.EmployeeID" &
            ", (esh.esdRowID IS NOT NULL) AS IsByDayEncoding" &
            " FROM employee e" &
            " LEFT JOIN (SELECT RowID AS esdRowID,EmployeeID FROM employeeshiftbyday WHERE OrganizationID='" & orgztnID & "' GROUP BY EmployeeID) esh ON esh.EmployeeID=e.RowID" &
            " WHERE e.organizationID = '" & z_OrganizationID & "'" &
            addtnl_query

        If txtEmpSearchBox.Text.Trim.Length = 0 And addtnl_query.Length > 0 Then

            dt = getDataTableForSQL(search_quer2 &
                                    " UNION" &
                                    search_quer1 &
                                    ";")

        ElseIf txtEmpSearchBox.Text.Trim.Length > 0 And addtnl_query.Length = 0 Then
            dt = New SQLQueryToDatatable(search_quer1 & ";").ResultTable
        Else

            dt = New SQLQueryToDatatable(search_quer1 &
                                         " UNION" &
                                         search_quer2 &
                                         ";").ResultTable 'orderby_query &

        End If

        'Else

        '    Dim emp_id_logic_operator As String = "="

        '    'If TextBox4.Text.Trim.Length = 0 Then
        '    emp_id_logic_operator = "!="
        '    'End If

        '    Dim division_condition As String = " AND DivisionID='" & divisionRowID & "'"

        '    If divisionRowID = 0 Then
        '        division_condition = String.Empty
        '    End If

        '    'dt = getDataTableForSQL("Select concat(COALESCE(e.Lastname, ' '),' ', COALESCE(e.Firstname, ' '), ' ', COALESCE(e.MiddleName, ' ')) as name" & _
        '    'dt = getDataTableForSQL("Select CONCAT(e.LastName,',',e.FirstName,',',INITIALS(e.MiddleName,'.','1')) as name" & _
        '    '                        ", e.EmployeeID" & _
        '    '                        ", e.RowID" & _
        '    '                        ",(esh.esdRowID IS NOT NULL) AS IsByDayEncoding" &
        '    '                        " from employee e" & _
        '    '                        " LEFT JOIN (SELECT RowID AS esdRowID,EmployeeID FROM employeeshiftbyday WHERE OrganizationID='" & orgztnID & "' GROUP BY EmployeeID) esh ON esh.EmployeeID=e.RowID" &
        '    '                        " INNER JOIN (SELECT * FROM position WHERE OrganizationID='" & orgztnID & "'" & division_condition & ") pos ON pos.RowID=e.PositionID" &
        '    '                        " where e.organizationID = '" & z_OrganizationID & "'" & _
        '    '                        " AND e.EmployeeID " & emp_id_logic_operator & " '" & TextBox4.Text & "'" & _
        '    '                        " ORDER BY e.RowID DESC;")

        '    Dim n_ReadSQLProcedureToDatatable As _
        '        New ReadSQLProcedureToDatatable("SEARCH_employeeshift",
        '                                        orgztnID,
        '                                        divisionRowID,
        '                                        txtEmpSearchBox.Text)

        '    dt = n_ReadSQLProcedureToDatatable.ResultTable

        'End If

        dgvEmpList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvEmpList.Rows.Add()
            With drow

                dgvEmpList.Rows.Item(n).Cells(c_EmployeeID.Index).Value = .Item("EmployeeID").ToString
                'txtempid.Text = .Item("EmployeeID").ToString
                dgvEmpList.Rows.Item(n).Cells(c_EmployeeName.Index).Value = .Item("Name").ToString
                dgvEmpList.Rows.Item(n).Cells(c_ID.Index).Value = .Item("RowID").ToString
                dgvEmpList.Item("ShiftEncodingType", n).Value = CBool(.Item("IsByDayEncoding"))
                dgvEmpList.Item("ShiftEncodingTypeDisplayValue", n).Value = includedEmployees.Contains(.Item("RowID"))
                'If CBool(.Item("IsByDayEncoding")) Then
                '    dgvEmpList.Item("ShiftEncodingTypeDisplayValue", n).Value = "By day"
                'Else
                '    dgvEmpList.Item("ShiftEncodingTypeDisplayValue", n).Value = "By date"
                'End If

            End With
        Next
        'End If

        tslblRowCountFound.Text = "record(s) found : " & dgvEmpList.RowCount

    End Sub

    Private Sub EmployeeShiftEntryForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        e.Cancel = (Not btnSave.Enabled)

        'myBalloon(, , lblSaveMsg, , , 1)

        'If previousForm IsNot Nothing Then
        '    If previousForm.Name = Me.Name Then
        '        previousForm = Nothing
        '    End If
        'End If

        'dutyshift.Close()

        'TimeAttendForm.listTimeAttendForm.Remove(Me.Name)

        'ShiftList.Close()
        'ShiftList.Dispose()

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        MyBase.OnLoad(e)

        txtEmpSearchBox.Focus()

    End Sub

    Dim view_ID As Integer = Nothing

    Private Sub ShiftEntryForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        fillemployeelist()

        If dgvEmpList.RowCount <> 0 Then
            Dim dgvceleventarg As New DataGridViewCellEventArgs(c_EmployeeID.Index, 0)
            dgvEmpList_CellClick(sender, dgvceleventarg)
        End If

        'cboshiftlist.ContextMenu = New ContextMenu

        enlistToCboBox("SELECT CONCAT(TIME_FORMAT(TimeFrom,'%l:%i %p'), ' TO ', TIME_FORMAT(TimeTo,'%l:%i %p')) FROM shift WHERE OrganizationID='" & orgztnID & "' ORDER BY TimeFrom,TimeTo;",
                       cboshiftlist)

        view_ID = VIEW_privilege("Employee Shift", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then

            btnSave.Visible = 0
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    btnSave.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                    Else

                    End If

                    If drow("Deleting").ToString = "N" Then
                    Else

                    End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Dim includedEmployees As New List(Of String)

    Private Sub dgvEmpList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmpList.CellContentClick

        If e.ColumnIndex > -1 _
            And e.RowIndex > -1 Then

            With dgvEmpList

                If TypeOf .Item(e.ColumnIndex, e.RowIndex) Is DataGridViewCheckBoxCell Then

                    .Item("c_EmployeeID", e.RowIndex).Selected = True
                    .Item("ShiftEncodingTypeDisplayValue", e.RowIndex).Selected = True

                End If

            End With

        End If

    End Sub

    Private Sub dgvEmpList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmpList.CellClick

        chkbxNewShiftByDay.Checked = False

        chkNightShift.Checked = False

        chkrestday.Checked = False

        IsNew = 0

        If e.ColumnIndex > -1 _
            And e.RowIndex > -1 Then
            dgvEmpList.Tag = dgvEmpList.Item("c_ID", e.RowIndex).Value
        Else
            dgvEmpList.Tag = Nothing
        End If

    End Sub

    Dim isCellInEditMode As Boolean = False

    Private Sub dgvEmpList_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvEmpList.CellBeginEdit
        isCellInEditMode = True
    End Sub

    Private Sub dgvEmpList_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmpList.CellEndEdit

        If e.ColumnIndex > -1 _
            And e.RowIndex > -1 Then
            isCellInEditMode = False
            With dgvEmpList

                If TypeOf .Item(e.ColumnIndex, e.RowIndex) Is DataGridViewCheckBoxCell Then

                    Try

                        Dim checkBoxResult As Boolean = CBool(.Item(e.ColumnIndex, e.RowIndex).Value)

                        If checkBoxResult Then

                            If includedEmployees.Contains(.Item("c_ID", e.RowIndex).Value) = False Then

                                includedEmployees.Add(.Item("c_ID", e.RowIndex).Value)

                            ElseIf includedEmployees.Count = 0 Then

                                includedEmployees.Add(.Item("c_ID", e.RowIndex).Value)

                            End If
                        Else

                            If includedEmployees.Contains(.Item("c_ID", e.RowIndex).Value) Then

                                includedEmployees.Remove(.Item("c_ID", e.RowIndex).Value)

                                fillemployeelist()

                            End If

                        End If
                    Catch ex As Exception
                        MsgBox(getErrExcptn(ex, Me.Name))
                    Finally

                    End Try
                Else

                End If

            End With
        Else

        End If

        tslblSelectedEmployee.Text = "selected employee(s) : " & includedEmployees.Count

    End Sub

    Dim dontUpdate As SByte = 0

    Public dutyShiftRowID As Integer = Nothing

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim prompt = MessageBox.Show("Do you wish to continue ?", "Confirm mass update employee shift", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
        Dim user_said_yes As Boolean = (prompt = Windows.Forms.DialogResult.Yes)
        Dim concat_includedEmployees = String.Join(",", includedEmployees.ToArray())

        If concat_includedEmployees.Length > 0 And user_said_yes Then

            btnSave.Enabled = False

            Dim n_SQLQueryToDatatable As _
                New SQLQueryToDatatable("CALL BULK_INSUPD_employeeshift('" & orgztnID & "'" &
                                        ",'" & concat_includedEmployees & "'" &
                                        "," & If(shift_id.Length = 0, "NULL", "'" & shift_id & "'") &
                                        ",'" & MYSQLDateFormat(CDate(dtpDateFrom.Value)) & "'" &
                                        ",'" & MYSQLDateFormat(CDate(dtpDateTo.Value)) & "'" &
                                        ",'" & Convert.ToInt16(chkrestday.Checked) & "'" &
                                        ",'" & z_User & "');")

            If isShowDialog Then
                'show an affirmative prompt, then close this form/class

                If n_SQLQueryToDatatable.HasError = False Then
                    MsgBox("Mass update done successfully.", MsgBoxStyle.Information)
                    Me.DialogResult = Windows.Forms.DialogResult.OK

                End If

            End If

            includedEmployees.Clear()

            btnSave.Enabled = True

        End If

    End Sub

    Private Sub lblShiftEntry_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblShiftEntry.LinkClicked
        'ShiftEntryForm.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
        'ShiftEntryForm.Close()
        'ShiftEntryForm.ShowDialog()

        'dutyshift.Show()
        'dutyshift.BringToFront()

        Dim n_ShiftEntryForm As New ShiftEntryForm

        n_ShiftEntryForm.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog

        n_ShiftEntryForm.StartPosition = FormStartPosition.CenterScreen

        If n_ShiftEntryForm.ShowDialog = Windows.Forms.DialogResult.OK Then

            If n_ShiftEntryForm.ShiftRowID <> Nothing Then

                enlistToCboBox("SELECT CONCAT(TIME_FORMAT(TimeFrom,'%l:%i %p'), ' TO ', TIME_FORMAT(TimeTo,'%l:%i %p'))" &
                               " FROM shift" &
                               " WHERE OrganizationID='" & orgztnID & "'" &
                               " ORDER BY TimeFrom,TimeTo;",
                               cboshiftlist)

                cboshiftlist.Text = Format(CDate(n_ShiftEntryForm.ShiftTimeFrom), machineShortTime) & " TO " & Format(CDate(n_ShiftEntryForm.ShiftTimeTo), machineShortTime)

            End If

        End If

        'Dim newShiftEntryForm As New ShiftEntryForm
        'newShiftEntryForm.ShowDialog()

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        IsNew = 0
        dgvEmpList.Enabled = 1

        Dim dgvceleventarg As New DataGridViewCellEventArgs(c_EmployeeID.Index,
                                                            0) 'dgvEmpList.CurrentRow.Index

        If dgvEmpList.RowCount > -1 Then
            dgvEmpList_CellClick(sender, dgvceleventarg)
        End If

        dtpDateFrom.MinDate = CDate("1/1/1753").ToShortDateString

        chkbxNewShiftByDay.Checked = False

        includedEmployees.Clear()

    End Sub

    Private Sub btnAudittrail_Click(sender As Object, e As EventArgs)
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub dgvEmpList_GotFocus(sender As Object, e As EventArgs) Handles dgvEmpList.GotFocus

    End Sub

    Private Sub cboshiftlist_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboshiftlist.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        If chkrestday.Checked Then
            If e_asc = 8 Then
                e.Handled = False
            Else
                e.Handled = True
            End If
        Else
            e.Handled = True
        End If

    End Sub

    Dim shift_id As String = ""

    Private Sub cboshiftlist_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboshiftlist.SelectedIndexChanged

        If cboshiftlist.SelectedIndex > -1 Then
            Dim n_ExecuteQuery As _
                New ExecuteQuery("SELECT RowID" &
                                 " FROM shift" &
                                 " WHERE OrganizationID='" & orgztnID & "'" &
                                 " AND CONCAT(TIME_FORMAT(TimeFrom,'%l:%i %p'), ' TO ', TIME_FORMAT(TimeTo,'%l:%i %p'))='" & cboshiftlist.Text & "' LIMIT 1;")
            shift_id = ValNoComma(n_ExecuteQuery.Result)
        Else
            shift_id = ""
        End If

    End Sub

    Dim filepath As String = String.Empty

    Private Sub tsbtnImportEmpShift_Click(sender As Object, e As EventArgs)

        Dim browsefile As OpenFileDialog = New OpenFileDialog()

        browsefile.Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                                  "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"

        If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

            filepath = IO.Path.GetFullPath(browsefile.FileName)

            Panel1.Enabled = False

            MDIPrimaryForm.Showmainbutton.Enabled = False

            Panel1.Enabled = False

            bgEmpShiftImport.RunWorkerAsync()

        End If

    End Sub

    Private Sub bgEmpShiftImport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgEmpShiftImport.DoWork

        backgroundworking = 1

        'IMPORT_employeeshift

        'EmployeeID
        'OrganizID
        'CreatedLastUpdBy
        'i_TimeFrom
        'i_TimeTo
        'i_DateFrom
        'i_DateTo

        Dim catchDT =
                    getWorkBookAsDataSet(filepath,
                                         Me.Name)

        If catchDT Is Nothing Then
        Else

            'For Each dtbl As DataTable In catchDT.Tables
            '    MsgBox(dtbl.TableName)
            '    'MsgBox(dtbl.Rows.Count)

            '    Dim tblname = dtbl.TableName
            '    '"'Employee DependentsS'"
            '    '"'Employee Salary$'"
            '    'Employees$
            'Next

            'Dim input_box = InputBox("What is the ordinal number of worksheet to be use ?", _
            '                         "Select the worksheet", _
            '                         1)

            'Dim table_name = Nothing

            'If input_box = Nothing Then
            '    table_name = catchDT.Tables(Val(input_box)).Name
            'Else
            '    table_name = catchDT.Tables(Val(input_box)).Name
            'End If

            Dim dtEmpShift = catchDT.Tables("'Employee Shift$'") 'Employee Shift

            If dtEmpShift IsNot Nothing Then

                Dim i = 1

                For Each drow As DataRow In dtEmpShift.Rows

                    Dim time_from = Nothing

                    Try
                        time_from = Format(CDate(drow(1)), "HH:mm")
                    Catch ex As Exception
                        time_from = DBNull.Value
                    End Try

                    Dim time_to = Nothing

                    Try
                        time_to = Format(CDate(drow(2)), "HH:mm")
                    Catch ex As Exception
                        time_to = DBNull.Value
                    End Try

                    IMPORT_employeeshift(drow(0),
                                         time_from,
                                         time_to,
                                         drow(3),
                                         drow(4),
                                         drow(5))

                    Dim progressresult = (i / dtEmpShift.Rows.Count) * 100

                    bgEmpShiftImport.ReportProgress(CInt(progressresult))

                    i += 1

                Next

            End If

        End If

        EXECQUER("DELETE FROM shift WHERE OrganizationID='" & orgztnID & "' AND TimeFrom IS NULL AND TimeTo IS NULL;")

    End Sub

    Private Sub bgEmpShiftImport_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgEmpShiftImport.ProgressChanged

    End Sub

    Private Sub bgEmpShiftImport_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgEmpShiftImport.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MessageBox.Show("Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Background work cancelled.")
        Else

        End If

        Panel1.Enabled = True

        backgroundworking = 0

        enlistToCboBox("SELECT CONCAT(TIME_FORMAT(TimeFrom,'%l:%i %p'), ' TO ', IF(TimeTo IS NULL, '', TIME_FORMAT(TimeTo,'%l:%i %p')))" &
                       " FROM shift" &
                       " WHERE OrganizationID='" & orgztnID & "'" &
                       " ORDER BY TimeFrom,TimeTo;",
                       cboshiftlist)

        If dgvEmpList.RowCount <> 0 Then
            Dim dgvceleventarg As New DataGridViewCellEventArgs(c_EmployeeID.Index, 0)
            dgvEmpList_CellClick(sender, dgvceleventarg)
        End If

        MDIPrimaryForm.Showmainbutton.Enabled = True

        Panel1.Enabled = True

    End Sub

    Dim dataread As MySqlDataReader

    Private Sub IMPORT_employeeshift(Optional i_EmployeeID As Object = Nothing,
                                     Optional i_TimeFrom As Object = Nothing,
                                     Optional i_TimeTo As Object = Nothing,
                                     Optional i_DateFrom As Object = Nothing,
                                     Optional i_DateTo As Object = Nothing,
                                     Optional i_SchedType As Object = Nothing)

        Try

            If conn.State = ConnectionState.Open Then

                conn.Close()

            End If

            cmd = New MySqlCommand("IMPORT_employeeshift", conn)

            conn.Open()

            With cmd

                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.AddWithValue("i_EmployeeID", i_EmployeeID)

                .Parameters.AddWithValue("OrganizID", orgztnID)

                .Parameters.AddWithValue("CreatedLastUpdBy", z_User)

                Dim ii = MilitTime(i_TimeFrom)

                .Parameters.AddWithValue("i_TimeFrom", ii)

                Dim iii = MilitTime(i_TimeTo)

                .Parameters.AddWithValue("i_TimeTo", iii)

                .Parameters.AddWithValue("i_DateFrom", If(i_DateFrom = Nothing, DBNull.Value, Format(CDate(i_DateFrom), "yyyy-MM-dd")))

                .Parameters.AddWithValue("i_DateTo", If(i_DateTo = Nothing, DBNull.Value, Format(CDate(i_DateTo), "yyyy-MM-dd")))

                Dim schedtypevalue = Nothing

                Try
                    If i_SchedType Is Nothing Then
                        schedtypevalue = ""
                    ElseIf IsDBNull(i_SchedType) Then
                        schedtypevalue = ""
                    Else
                        schedtypevalue = If(i_SchedType = Nothing, DBNull.Value, CStr(i_SchedType))
                    End If
                Catch ex As Exception
                    schedtypevalue = ""
                End Try

                .Parameters.AddWithValue("i_SchedType", schedtypevalue)

                'dataread =
                .ExecuteNonQuery()

            End With
        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))

        End Try

    End Sub

    Function MilitTime(ByVal timeval As Object) As Object

        Dim retrnObj As Object

        retrnObj = New Object

        Try

            'If timeval Is Nothing Then
            '    retrnObj = DBNull.Value
            'Else
            If timeval = Nothing Then
                retrnObj = DBNull.Value
            ElseIf IsDBNull(timeval) Then
                retrnObj = DBNull.Value
            Else

                Dim endtime As Object = timeval

                If endtime.ToString.Contains("P") Then

                    Dim hrs As String = If(Val(getStrBetween(endtime, "", ":")) = 12, 12, Val(getStrBetween(endtime, "", ":")) + 12)

                    Dim mins As String = StrReverse(getStrBetween(StrReverse(endtime.ToString), "", ":"))

                    mins = getStrBetween(mins, "", " ")

                    retrnObj = hrs & ":" & mins

                ElseIf endtime.ToString.Contains("A") Then

                    Dim i As Integer = StrReverse(endtime).ToString.IndexOf(" ")

                    endtime = endtime.ToString.Replace("A", "")

                    'Dim i As Integer = StrReverse("3:15 AM").ToString.IndexOf(" ")

                    ''endtime = endtime.ToString.Replace("A", "")

                    'MsgBox(Trim(StrReverse(StrReverse("3:15 AM").ToString.Substring(i, ("3:15 AM").ToString.Length - i))).Length)

                    Dim amTime As String = Trim(StrReverse(StrReverse(endtime.ToString).Substring(i,
                                                                                      endtime.ToString.Length - i)
                                              )
                                   )

                    amTime = If(getStrBetween(amTime, "", ":") = "12",
                                24 & ":" & StrReverse(getStrBetween(StrReverse(amTime), "", ":")),
                                amTime)

                    retrnObj = amTime
                Else
                    retrnObj = endtime

                End If

            End If
        Catch ex As Exception
            retrnObj = DBNull.Value
        End Try

        Return retrnObj

    End Function

    Dim pagination As Integer = 0

    Dim n_ShiftList As New ShiftList

    Private Sub CustomColoredTabControl1_Selecting(sender As Object, e As TabControlCancelEventArgs) 'Handles CustomColoredTabControl1.Selecting
        e.Cancel = True
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        btnSearch.Enabled = False

        fillemployeelist()

        btnSearch.Enabled = True

    End Sub

    Private Sub txtEmpSearchBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEmpSearchBox.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then

            btnSearch_Click(btnSearch, New EventArgs)

        End If

    End Sub

    Private Sub txtEmpSearchBox_TextChanged(sender As Object, e As EventArgs) Handles txtEmpSearchBox.TextChanged

    End Sub

    Dim divisionRowID = Nothing

    Private Sub trvDepartment_AfterSelect(sender As Object, e As TreeViewEventArgs)

        Static once As SByte = 0

        divisionRowID = 0

        If e.Node IsNot Nothing Then

            If once = 0 Then

                once = 1
            Else

                divisionRowID = CInt(ValNoComma(e.Node.Tag))

                btnSearch_Click(btnSearch, New EventArgs)

            End If

        End If

    End Sub

    'Dim sdfsd As New Indigo.CollapsibleGroupBox '.CollapseBoxClickedEventHandler

    'Dim msd As sdfsd = AddressOf sdfsd.CollapseBoxClickedEventHandler

    Private Sub CollapsibleGroupBox1_Click(sender As Object,
                                           e As CollapseBoxClickedEventHandler)

    End Sub

    Private Sub CollapsibleGroupBox1_Enter(sender As Object, e As EventArgs)

    End Sub

    Private Sub CollapsibleGroupBox1_CollapseBoxClickedEvent(sender As Object)

    End Sub

    Private Sub cmsDeleteShift_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmsDeleteShift.Opening

    End Sub

    Private isShowDialog As Boolean = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult

        With Me

            btnClose.Visible = False

            .Text = someValue.Trim

            isShowDialog = True

        End With

        Return Me.ShowDialog

    End Function

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If isShowDialog And isCellInEditMode = False Then

            If keyData = Keys.Escape Then

                Me.Close()

                Return False
            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub dgvEmpList_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvEmpList.RowsAdded

        If e.RowIndex > -1 Then

            With dgvEmpList

                If CBool(.Item("ShiftEncodingTypeDisplayValue", e.RowIndex).Value) = True Then

                End If

            End With

        End If

    End Sub

    Private Sub chkrestday_CheckedChanged(sender As Object, e As EventArgs) Handles chkrestday.CheckedChanged
        cboshiftlist.SelectedIndex = -1
    End Sub

    Private Sub dtpDateFrom_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateFrom.ValueChanged

        If DateDiff(DateInterval.Day, CDate(dtpDateFrom.Value), CDate(dtpDateTo.Value)) < 0 Then
            dtpDateTo.Value = dtpDateFrom.Value
        End If

    End Sub

    Private Sub dtpDateTo_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateTo.ValueChanged

        If DateDiff(DateInterval.Day, CDate(dtpDateFrom.Value), CDate(dtpDateTo.Value)) < 0 Then
            dtpDateFrom.Value = dtpDateTo.Value
        End If

    End Sub

End Class