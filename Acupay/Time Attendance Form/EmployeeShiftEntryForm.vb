Imports System.Collections.ObjectModel
Imports System.IO
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports OfficeOpenXml

Public Class EmployeeShiftEntryForm

    Class ImportedShiftEntry

        Public Property EmployeeID As Integer

        Public Property TimeFrom As String

        Public Property TimeTo As String

        Public Property DateFrom As Date

        Public Property DateTo As Date

        Public Property SchedType As String

    End Class

    Dim IsNew As Integer = 0
    Dim rowid As Integer

    Private _shiftModels As ICollection(Of ShiftModel)

    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Dim machineFirstDayOfWeek As String = RegKey.GetValue("iFirstDayOfWeek").ToString

    Dim machineShortTime As String = RegKey.GetValue("sShortTime").ToString

    Dim ArrayWeekFormat() As String

    Private sys_ownr As New SystemOwner

    Protected Overrides Sub OnLoad(e As EventArgs)
        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL `MACHINE_WEEKFORMAT`(@@default_week_format);")
        Dim dtweek As New DataTable

        dtweek = n_SQLQueryToDatatable.ResultTable

        Dim i = 0

        ReDim ArrayWeekFormat(dtweek.Rows.Count - 1)

        For Each drow As DataRow In dtweek.Rows
            ArrayWeekFormat(i) = drow("DayFullName").ToString
            dgvWeek.Columns(i).HeaderText = drow("DayName").ToString
            i += 1
        Next

        dgvWeek.Rows.Clear()
        dgvWeek.Rows.Add()
        LoadDivision()

        setProperInterfaceBaseOnSystemOwner()

        MyBase.OnLoad(e)
    End Sub

    Dim ParentDiv As New DataTable

    Dim ChiledDiv As New DataTable

    Private Sub LoadDivision()
        trvDepartment.Nodes.Clear()

        ParentDiv = New SQLQueryToDatatable("SELECT * FROM `division` WHERE OrganizationID='" & orgztnID & "' AND ParentDivisionID IS NULL;").ResultTable

        ChiledDiv = New SQLQueryToDatatable("SELECT d.*" &
                                            ",dd.Name AS ParentDivisionName" &
                                            ",pf.PayFrequencyType" &
                                            " FROM `division` d" &
                                            " INNER JOIN `division` dd ON dd.RowID=d.ParentDivisionID" &
                                            " INNER JOIN payfrequency pf ON pf.RowID=d.PayFrequencyID" &
                                            " WHERE d.OrganizationID='" & orgztnID & "' AND d.ParentDivisionID IS NOT NULL;").ResultTable

        For Each pdiv As DataRow In ParentDiv.Rows
            ChildDivision(pdiv("RowID"), pdiv("Name"))
        Next

        trvDepartment.ExpandAll()

        For Each tnod As TreeNode In trvDepartment.Nodes
            trvDepartment.SelectedNode = tnod
            Exit For
        Next
    End Sub

    Private Sub ChildDivision(DivisionRowID As Object,
                              NodeText As String,
                              Optional trvnod As TreeNode = Nothing)
        Dim n_nod As TreeNode = Nothing

        n_nod = trvDepartment.Nodes.Add(NodeText & Space(5))

        With n_nod
            .Tag = DivisionRowID
            .NodeFont =
                New System.Drawing.Font("Segoe UI", 8.75!, System.Drawing.FontStyle.Bold)
        End With

        Dim sel_ChiledDiv = ChiledDiv.Select("ParentDivisionID = " & DivisionRowID)

        For Each divrow In sel_ChiledDiv
            Dim childnod = n_nod.Nodes.Add(divrow("Name"))

            childnod.Tag = divrow("RowID") 'divrow
            childnod.NodeFont =
                New System.Drawing.Font("Segoe UI", 8.75!, System.Drawing.FontStyle.Regular)
        Next
    End Sub

    Private Sub fillemplyeelist()
        Dim dt As New DataTable
        Dim lastName = txtsrchEmpLName.Text.Trim
        Dim shouldIgnoreLastName = String.IsNullOrWhiteSpace(lastName)

        Dim emp_id_logic_operator As String = "="

        If TextBox4.Text.Trim.Length = 0 Then
            emp_id_logic_operator = "!="
        End If

        Dim division_condition As String = " AND DivisionID='" & divisionRowID & "'"

        If divisionRowID = 0 Then
            division_condition = String.Empty
        End If

        Dim n_ReadSQLProcedureToDatatable As _
            New ReadSQLProcedureToDatatable("SEARCH_employeeshift",
                                            orgztnID,
                                            divisionRowID,
                                            TextBox4.Text, txtsrchEmpLName.Text.Trim)

        dt = n_ReadSQLProcedureToDatatable.ResultTable

        dgvEmpList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvEmpList.Rows.Add()
            With drow
                dgvEmpList.Rows.Item(n).Cells(c_EmployeeID.Index).Value = .Item("EmployeeID").ToString
                dgvEmpList.Rows.Item(n).Cells(c_EmployeeName.Index).Value = .Item("Name").ToString
                dgvEmpList.Rows.Item(n).Cells(c_ID.Index).Value = .Item("RowID").ToString
                dgvEmpList.Item("ShiftEncodingType", n).Value = CBool(.Item("IsByDayEncoding"))

                If CBool(.Item("IsByDayEncoding")) Then
                    dgvEmpList.Item("ShiftEncodingTypeDisplayValue", n).Value = "By day"
                Else
                    dgvEmpList.Item("ShiftEncodingTypeDisplayValue", n).Value = "By date"
                End If
            End With
        Next
    End Sub

    Private Sub fillemplyeelistselected()
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select concat(COALESCE(Lastname, ' '),' ', COALESCE(Firstname, ' '), ' ', COALESCE(MiddleName, ' ')) as name, EmployeeID, RowID from employee where organizationID = '" & z_OrganizationID & "' And RowID = '" & dgvEmpList.CurrentRow.Cells(c_ID.Index).Value & "'")

        For Each drow As DataRow In dt.Rows
            With drow
                txtEmpID.Text = .Item("EmployeeID").ToString
                txtEmpName.Text = .Item("Name").ToString
                rowid = .Item("RowID").ToString
            End With
        Next
    End Sub

    Private Sub fillemployeeshift()
        Dim new_link = New System.Windows.Forms.LinkLabel.Link()

        new_link.Name = "First"

        Link_Paging(First, New LinkLabelLinkClickedEventArgs(new_link))
    End Sub

    Private Sub fillemployeeshiftSelected(Optional esh_RowID As Object = Nothing)
        cboshiftlist.SelectedIndex = -1
        cboshiftlist.Text = ""

        If Not dgvEmpShiftList.Rows.Count = 0 Then
            Dim dt As New DataTable
            Dim selRowID = If(esh_RowID = Nothing, dgvEmpShiftList.CurrentRow.Cells(c_RowIDShift.Index).Value, esh_RowID)
            dt = getDataTableForSQL("select concat(COALESCE(ee.Lastname, ' '),' ', COALESCE(ee.Firstname, ' '), ' ', COALESCE(ee.MiddleName, ' ')) as name" &
                                    ",ee.EmployeeID" &
                                    ",es.EffectiveFrom" &
                                    ",es.EffectiveTo" &
                                    ",COALESCE(es.ShiftID,'') 'ShiftID'" &
                                    ",es.ShiftID AS ShiftRowID" &
                                    ",es.NightShift, es.RestDay" &
                                    ",IFNULL(TIME_FORMAT(s.TimeFrom, '%l:%i %p'),'') timef" &
                                    ",IFNULL(TIME_FORMAT(s.TimeTo, '%l:%i %p'),'') timet" &
                                    ",es.RowID from employeeshift es " &
                                    "left join shift s on es.ShiftID = s.RowID " &
                                    "inner join employee ee on es.EmployeeID = ee.RowID " &
                                    "where es.OrganizationID = '" & z_OrganizationID & "' And es.RowID = '" & selRowID & "';")

            For Each drow As DataRow In dt.Rows
                With drow
                    txtEmpID.Text = .Item("EmployeeID").ToString
                    txtEmpName.Text = .Item("Name").ToString

                    Dim shiftId = ConvertToType(Of Integer?)(drow("ShiftRowID"))

                    Dim selectedShift = _shiftModels.
                        FirstOrDefault(
                            Function(s) s.Shift.RowID = shiftId
                        )

                    If selectedShift Is Nothing Then
                        cboshiftlist.SelectedIndex = -1
                    Else
                        cboshiftlist.SelectedItem = selectedShift
                    End If

                    dtpDateFrom.Value = CDate(.Item("EffectiveFrom")).ToString(machineShortDateFormat)
                    dtpDateTo.Value = CDate(.Item("Effectiveto")).ToString(machineShortDateFormat)
                    lblShiftID.Text = .Item("ShiftID").ToString
                    chkNightShift.Checked = IIf(If(IsDBNull(.Item("NightShift")), 0, .Item("NightShift")) = "1", True, False)
                    chkrestday.Checked = If(.Item("RestDay") = 1, 1, 0)
                End With
            Next
        End If
    End Sub

    Private Sub EmployeeShiftEntryForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        myBalloon(, , lblSaveMsg, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        dutyshift.Close()

        TimeAttendForm.listTimeAttendForm.Remove(Me.Name)

        ShiftList.Close()
        ShiftList.Dispose()
    End Sub

    Dim view_ID As Integer = Nothing

    Private Sub ShiftEntryForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        fillemplyeelist()
        fillemployeeshift()
        fillemployeeshiftSelected()

        Using context = New PayrollContext()
            Dim shifts = context.Shifts.
                Where(Function(s) s.OrganizationID = z_OrganizationID).
                OrderBy(Function(s) s.TimeFrom).
                ThenBy(Function(s) s.TimeTo).
                ToList()

            _shiftModels = shifts.
                Select(Function(s) New ShiftModel(s)).
                ToList()

            cboshiftlist.DataSource = _shiftModels
        End Using

        view_ID = VIEW_privilege("Employee Shift", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then

            btnNew.Visible = 0
            btnSave.Visible = 0
            btnDelete.Visible = 0
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    btnNew.Visible = 0
                    btnSave.Visible = 0
                    btnDelete.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        btnNew.Visible = 0
                    Else
                        btnNew.Visible = 1
                    End If

                    If drow("Deleting").ToString = "N" Then
                        btnDelete.Visible = 0
                    Else
                        btnDelete.Visible = 1
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

    Private Sub dgvEmpList_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgvEmpList.CurrentCellChanged
        If dgvEmpList.RowCount > 0 Then
            chkbxNewShiftByDay.Checked = False
            chkNightShift.Checked = False
            chkrestday.Checked = False
            IsNew = 0

            If dgvEmpList.CurrentRow IsNot Nothing Then
                dgvEmpList.Tag = dgvEmpList.CurrentRow.Cells("c_ID").Value

                Try
                    fillemplyeelistselected()
                    fillemployeeshift()
                    fillemployeeshiftSelected()
                Catch ex As Exception
                    MsgBox(getErrExcptn(ex, Me.Name))
                Finally
                    If CustomColoredTabControl1.SelectedIndex = 1 Then
                        TabPage2_Enter(TabPage2, New EventArgs)
                    End If
                End Try
            End If
        End If
    End Sub

    Private Sub dgvEmpShiftList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmpShiftList.CellClick
        Try

            fillemployeeshiftSelected()
            btnDelete.Enabled = True
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, ByDateToolStripMenuItem.Click
        If TypeOf CObj(sender) Is ToolStripDropDownButton Then
        Else
            CustomColoredTabControlActivateSelecting(True)
            CustomColoredTabControl1.SelectedIndex = 0

            IsNew = 1
            lblShiftID.Text = 0
            dgvEmpList.Enabled = False
            dtpDateFrom.Value = Date.Now.ToString(machineShortDateFormat)
            dtpDateTo.Value = Date.Now.ToString(machineShortDateFormat)
            btnNew.Enabled = False

            chkNightShift.Checked = False
            chkrestday.Checked = False

            If dgvEmpList.RowCount <> 0 Then

                Dim empshiftmaxdate =
                    EXECQUER("SELECT IFNULL(ADDDATE(MAX(EffectiveTo), INTERVAL 1 DAY),'') 'empshiftmaxdate'" &
                             " FROM employeeshift" &
                             " WHERE EmployeeID=" & dgvEmpList.CurrentRow.Cells("c_ID").Value &
                             " AND RestDay='0'" &
                             " LIMIT 1;")

                If empshiftmaxdate = Nothing Then
                    empshiftmaxdate =
                    EXECQUER("SELECT IFNULL(StartDate,CURRENT_DATE()) 'StartDate'" &
                             " FROM employee" &
                             " WHERE RowID='" & dgvEmpList.CurrentRow.Cells("c_ID").Value &
                             "';")

                    dtpDateFrom.MinDate = CDate(empshiftmaxdate).ToShortDateString

                End If

                Try
                    dtpDateFrom.Value = CDate(empshiftmaxdate).ToShortDateString
                Catch ex As Exception
                    Try
                        dtpDateFrom.Value = Date.Now.ToString(machineShortDateFormat)
                    Catch ex1 As Exception
                        dtpDateFrom.Value = dtpDateFrom.MinDate.ToShortDateString
                    End Try
                End Try

            End If
            CustomColoredTabControlActivateSelecting(False)
        End If
    End Sub

    Dim dontUpdate As SByte = 0

    Public dutyShiftRowID As Integer = Nothing

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If dgvEmpList.RowCount = 0 Then
            Exit Sub
        End If

        Dim isrestday = If(chkrestday.Checked, "1", "0")

        ' A hack to make datepickers commit change to their values.
        dgvEmpShiftList.Focus()

        Select Case CustomColoredTabControl1.SelectedIndex
            Case 0
                Dim shiftModel = DirectCast(cboshiftlist.SelectedItem, ShiftModel)

                Dim shiftId = If(shiftModel?.Shift.RowID Is Nothing, 0, shiftModel.Shift.RowID)

                If chkrestday.Checked = 0 Then

                    Dim dt As New DataTable
                    dt = getDataTableForSQL("Select * From employeeshift where " &
                                            " EmployeeID = '" & dgvEmpList.CurrentRow.Cells(c_ID.Index).Value & "' And OrganizationID = '" & z_OrganizationID & "'")

                    For Each drow As DataRow In dt.Rows
                        With drow
                            Dim startdate, enddate As Date
                            startdate = CDate(.Item("EffectiveFrom")).ToString(machineShortDateFormat)
                            enddate = CDate(.Item("EffectiveTo")).ToString(machineShortDateFormat)

                            Dim sdate, edate As Date

                            sdate = dtpDateFrom.Value.ToString(machineShortDateFormat)
                            edate = dtpDateTo.Value.ToString(machineShortDateFormat)

                            If sdate > startdate And edate < enddate Then
                                MsgBox("Date between " & startdate & " to " & edate & " is not allowed", MsgBoxStyle.Exclamation, "System Message.")
                                Exit Sub
                            End If

                        End With

                    Next

                End If

                If shiftId = 0 And chkrestday.Checked = False Then
                    MsgBox("Please select a shift.", MsgBoxStyle.Exclamation, "System Message.")
                    cboshiftlist.Focus()
                    Exit Sub
                End If

                Dim nightshift As Integer
                If chkNightShift.Checked = True Then
                    nightshift = 1
                Else
                    nightshift = 0
                End If

                If IsNew = 1 Then
                    '                                                                                                                                                                 'Val(lblShiftID.Text)
                    sp_employeeshiftentry(z_datetime, z_User, z_datetime, z_OrganizationID, z_User, dtpDateFrom.Value, dtpDateTo.Value, dgvEmpList.CurrentRow.Cells(c_ID.Index).Value, shiftId, nightshift, isrestday)

                    dtpDateFrom.MinDate = CDate("1/1/1753").ToShortDateString

                    dtpDateTo.MinDate = CDate("1/1/1753").ToShortDateString

                    fillemployeeshift()

                    fillemployeeshiftSelected()

                    myBalloon("Successfully Save", "Saving...", lblSaveMsg, , -100)
                Else
                    If dontUpdate = 1 Then
                        Exit Sub
                    ElseIf dgvEmpShiftList.RowCount = 0 Then
                        Exit Sub
                    End If

                    Dim n_ExecuteQuery As New ExecuteQuery("CALL UPD_employeeshift('" & dgvEmpShiftList.CurrentRow.Cells(c_RowIDShift.Index).Value & "'" &
                                                           ",'" & z_User & "'" &
                                                           ",'" & dtpDateFrom.Value.ToString("yyyy-MM-dd") & "'" &
                                                           ",'" & dtpDateTo.Value.ToString("yyyy-MM-dd") & "'" &
                                                           ",'" & shiftId & "'" &
                                                           ",'" & nightshift & "'" &
                                                           ",'" & isrestday & "'" &
                                                           ");")

                    dtpDateFrom.MinDate = CDate("1/1/1753").ToShortDateString

                    dtpDateTo.MinDate = CDate("1/1/1753").ToShortDateString
                    fillemployeeshift()
                    Dim selRowID = ValNoComma(dgvEmpShiftList.CurrentRow.Cells(c_RowIDShift.Index).Value)
                    fillemployeeshiftSelected(selRowID)

                    myBalloon("Successfully Updated", "Updating...", lblSaveMsg, , -100)
                End If

            Case 1

                Dim customArrayWeekFormat As String = ""

                For Each strval In ArrayWeekFormat
                    customArrayWeekFormat &= ",'" & strval & "'"
                Next

                Dim trimcustomArrayWeekFormat = customArrayWeekFormat.Substring(1, (customArrayWeekFormat.Length - 1))
                'the result of this variable is the concatenation of the day names in a week

                Dim EmployeeStartingDate = New ExecuteQuery("SELECT StartDate FROM employee WHERE RowID='" & dgvEmpList.Tag & "' AND OrganizationID='" & orgztnID & "';").Result

                Dim customEmployeeShift As New DataTable

                With customEmployeeShift.Columns
                    .Add("EmpStartDate")
                    .Add("AddDay")
                    .Add("ShiftRowID")
                    .Add("NameOfDay")
                End With

                Dim shiftrowIDs As New List(Of String)

                Dim shiftbydayIsExists =
                    New ExecuteQuery("SELECT EXISTS(SELECT RowID" &
                                     " FROM employeeshiftbyday" &
                                     " WHERE EmployeeID='" & dgvEmpList.Tag & "'" &
                                     " AND OrganizationID='" & orgztnID & "'" &
                                     " LIMIT 1);").Result

                If shiftbydayIsExists = "1" Then

                End If

                If chkbxNewShiftByDay.Checked Then
                    Dim nothing_value =
                        New ExecuteQuery("DELETE FROM employeeshiftbyday" &
                                         " WHERE EmployeeID='" & dgvEmpList.Tag & "'" &
                                         " AND OrganizationID='" & orgztnID & "';" &
                                         " ALTER TABLE employeeshiftbyday AUTO_INCREMENT = 0;").Result
                End If

                For Each dgvcol As DataGridViewColumn In dgvWeek.Columns
                    For Each dgvrow As DataGridViewRow In dgvWeek.Rows

                        Dim IDShift = dgvWeek.Item(dgvcol.Index, dgvrow.Index).Tag

                        Dim n_ReadSQLFunction As _
                            New ReadSQLFunction("INSUPD_employeeshiftbyday", "returnvalue",
                                                orgztnID,
                                                z_User,
                                                dgvEmpList.Tag,
                                                If(IDShift = Nothing, DBNull.Value, IDShift),
                                                ArrayWeekFormat(dgvcol.Index),
                                                "0",
                                                If(IDShift = Nothing, 1, 0),
                                                dgvcol.Index,
                                                Convert.ToInt16(chkbxNewShiftByDay.Checked))

                        Exit For
                    Next
                Next

                customEmployeeShift.Dispose()

                If shiftbydayIsExists = "0" Then
                    Dim n_ExecuteQuery As _
                        New ExecuteQuery("CALL AUTOMATICUPD_employeeshiftbyday('" & orgztnID & "','" & dgvEmpList.Tag & "');")
                Else
                    If chkbxNewShiftByDay.Checked Then
                        Dim n_ExecuteQuery As _
                            New ExecuteQuery("CALL AUTOMATICUPD_employeeshiftbyday('" & orgztnID & "','" & dgvEmpList.Tag & "');")
                    End If
                End If

                chkbxNewShiftByDay.Checked = False

                Dim dgvceleventarg As New DataGridViewCellEventArgs(c_EmployeeID.Index,
                                                                    0) 'dgvEmpList.CurrentRow.Index

        End Select

        IsNew = 0
        lblShiftID.Text = 0
        dgvEmpList.Enabled = True
        btnNew.Enabled = True

        dtpDateFrom.MinDate = CDate("1/1/1753").ToShortDateString
    End Sub

    Private Sub lblShiftEntry_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblShiftEntry.LinkClicked
        Dim n_ShiftEntryForm As New ShiftEntryForm
        n_ShiftEntryForm.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
        n_ShiftEntryForm.StartPosition = FormStartPosition.CenterScreen

        If n_ShiftEntryForm.ShowDialog = Windows.Forms.DialogResult.OK Then
            If n_ShiftEntryForm.ShiftRowID <> Nothing Then

                enlistToCboBox("SELECT CONCAT(TIME_FORMAT(TimeFrom,'%l:%i %p'), ' TO ', TIME_FORMAT(TimeTo,'%l:%i %p'))" &
                               " FROM shift" &
                               " WHERE OrganizationID='" & orgztnID & "' AND TimeFrom IS NOT NULL  AND TimeTo IS NOT NULL " &
                               " ORDER BY TimeFrom,TimeTo;",
                               cboshiftlist)

                cboshiftlist.Text = Format(CDate(n_ShiftEntryForm.ShiftTimeFrom), machineShortTime) & " TO " & Format(CDate(n_ShiftEntryForm.ShiftTimeTo), machineShortTime)
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvEmpShiftList.RowCount <> 0 Then
            btnDelete.Enabled = False

            Dim prompt = MessageBox.Show("Do you want to delete this employee shift ?", "Confirm deleting shift", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

            If prompt = Windows.Forms.DialogResult.Yes Then
                Dim n_ExecSQLProcedure As New _
                    ExecSQLProcedure("DEL_employeeshift",
                                     Convert.ToInt32(dgvEmpShiftList.CurrentRow.Cells("c_RowIDShift").Value),
                                     z_User)

                dgvEmpShiftList.Rows.Remove(dgvEmpShiftList.CurrentRow)
            End If
        End If

        btnDelete.Enabled = True
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        IsNew = 0
        dgvEmpList.Enabled = 1

        Dim dgvceleventarg As New DataGridViewCellEventArgs(c_EmployeeID.Index,
                                                            0)

        btnNew.Enabled = True

        dtpDateFrom.MinDate = CDate("1/1/1753").ToShortDateString
        CustomColoredTabControlActivateSelecting(True)
        chkbxNewShiftByDay.Checked = False
    End Sub

    Private Sub btnAudittrail_Click(sender As Object, e As EventArgs) Handles btnAudittrail.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_ID)
        showAuditTrail.BringToFront()
    End Sub

    Private Sub dgvEmpList_GotFocus(sender As Object, e As EventArgs) Handles dgvEmpList.GotFocus
        btnDelete.Enabled = False
    End Sub

    Private Sub dgvEmpShiftList_GotFocus(sender As Object, e As EventArgs) Handles dgvEmpShiftList.GotFocus
        btnDelete.Enabled = True
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

    Dim filepath As String = String.Empty

    Private Sub tsbtnImportEmpShift_Click(sender As Object, e As EventArgs) Handles tsbtnImportEmpShift.Click

        Dim browsefile As OpenFileDialog = New OpenFileDialog()

        browsefile.Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                                  "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"

        If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then

            filepath = IO.Path.GetFullPath(browsefile.FileName)

            Panel1.Enabled = False

            Panel2.Enabled = False

            ToolStripProgressBar1.Visible = True

            MDIPrimaryForm.Showmainbutton.Enabled = False

            Panel2.Enabled = False

            Panel1.Enabled = False

            bgEmpShiftImport.RunWorkerAsync()

        End If

    End Sub

    Private Function ParseTime(value As String) As String
        If String.IsNullOrEmpty(value) Then
            Return Nothing
        End If

        Return Nothing
    End Function

    Private Function ParseDate(value As String) As Date?
        If String.IsNullOrEmpty(value) Then
            Return Nothing
        End If

        Dim separator As String

        If value.Contains("/") Then
            separator = "/"
        ElseIf value.Contains("-") Then
            separator = "-"
        Else
            Return Nothing
        End If

        Dim dateParts = value.Split(separator)
        Dim month = CInt(dateParts(0))
        Dim day = CInt(dateParts(1))
        Dim year = CInt(dateParts(2))

        Return New Date(year, month, day)
    End Function

    Private Function ImportShiftFromExcelFile(fileName As String) As Collection(Of ImportedShiftEntry)
        Dim shiftEntries = New Collection(Of ImportedShiftEntry)

        Dim excelFileName = fileName
        If excelFileName.EndsWith(".xls") Then
            excelFileName = ConvertToXlsx(fileName)
        End If

        Try
            Using excel As New ExcelPackage(New FileInfo(excelFileName))
                Dim worksheet = excel.Workbook.Worksheets("Employee Shift")

                If worksheet Is Nothing Then
                    Throw New Exception("Sorry, but we can't find the `Employee Shift` worksheet.")
                End If

                Dim startRow = worksheet.Dimension.Start
                Dim endRow = worksheet.Dimension.End

                For row = startRow.Row To endRow.Row
                    Dim employeeID = worksheet.Cells(row, 1).Text

                    If String.IsNullOrEmpty(employeeID) Or (Not IsNumeric(employeeID)) Then
                        Continue For
                    End If

                    Dim timeIn = worksheet.Cells(row, 2).Text
                    Dim timeOut = worksheet.Cells(row, 3).Text
                    Dim dateFrom = ParseDate(worksheet.Cells(row, 4).Text)
                    Dim dateTo = ParseDate(worksheet.Cells(row, 5).Text)
                    Dim schedType = worksheet.Cells(row, 6).Text

                    Dim shiftEntry = New ImportedShiftEntry() With {
                        .EmployeeID = employeeID,
                        .TimeFrom = timeIn,
                        .TimeTo = timeOut,
                        .DateFrom = dateFrom,
                        .DateTo = dateTo
                    }

                    shiftEntries.Add(shiftEntry)
                Next
            End Using
        Catch exception As Exception
            MsgBox(exception.Message)
        End Try

        Return shiftEntries
    End Function

    Public Function ConvertToXlsx(fileName As String) As String
        'Dim files = Directory.GetFiles(filesFolder)
        Dim tempFileName = Path.GetTempFileName() + ".xslx"

        Dim app = New Microsoft.Office.Interop.Excel.Application()
        Dim workbook = app.Workbooks.Open(fileName)
        workbook.SaveAs(Filename:=tempFileName, FileFormat:=Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook)
        workbook.Close()
        app.Quit()

        Return tempFileName
    End Function

    Private Sub bgEmpShiftImport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgEmpShiftImport.DoWork
        backgroundworking = 1

        Dim shiftEntries = ImportShiftFromExcelFile(filepath)

        For Each shiftEntry In shiftEntries
            IMPORT_employeeshift(
                shiftEntry.EmployeeID,
                shiftEntry.TimeFrom,
                shiftEntry.TimeTo,
                shiftEntry.DateFrom,
                shiftEntry.DateTo,
                shiftEntry.SchedType
            )
        Next

        Return
        EXECQUER("DELETE FROM shift WHERE OrganizationID='" & orgztnID & "' AND TimeFrom IS NULL AND TimeTo IS NULL;" &
                 "ALTER TABLE shift AUTO_INCREMENT = 0;")
    End Sub

    Private Sub bgEmpShiftImport_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgEmpShiftImport.ProgressChanged
        ToolStripProgressBar1.Value = CType(e.ProgressPercentage, Integer)
    End Sub

    Private Sub bgEmpShiftImport_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgEmpShiftImport.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MessageBox.Show("Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Background work cancelled.")
        End If

        Panel1.Enabled = True
        Panel2.Enabled = True

        ToolStripProgressBar1.Visible = False

        backgroundworking = 0

        enlistToCboBox("SELECT CONCAT(TIME_FORMAT(TimeFrom,'%l:%i %p'), ' TO ', IF(TimeTo IS NULL, '', TIME_FORMAT(TimeTo,'%l:%i %p')))" &
                       " FROM shift" &
                       " WHERE OrganizationID='" & orgztnID & "' AND TimeFrom IS NOT NULL  AND TimeTo IS NOT NULL " &
                       " ORDER BY TimeFrom,TimeTo;",
                       cboshiftlist)

        MDIPrimaryForm.Showmainbutton.Enabled = True
        Panel2.Enabled = True
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

    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then
            fillemplyeelist()
        End If
    End Sub

    Dim pagination As Integer = 0

    Private Sub Link_Paging(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked, Prev.LinkClicked,
                                                                                            Nxt.LinkClicked, Last.LinkClicked
        dgvEmpShiftList.Rows.Clear()

        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "First" Then
            pagination = 0
        ElseIf sendrname = "Prev" Then
            If pagination - 50 < 0 Then
                pagination = 0
            Else : pagination -= 50
            End If
        ElseIf sendrname = "Nxt" Then
            pagination += 50
        ElseIf sendrname = "Last" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 50 FROM employeeshift WHERE OrganizationID=" & orgztnID & " AND EmployeeID='" & dgvEmpList.Tag & "';"))
            Dim remender = lastpage Mod 1

            pagination = (lastpage - remender) * 50
        End If

        Dim n_ReadSQLProcedureToDatatable As _
            New ReadSQLProcedureToDatatable("VIEW_employeeshift",
                                            orgztnID,
                                            ValNoComma(dgvEmpList.Tag),
                                            pagination)

        Dim empshifttable As New DataTable

        empshifttable = n_ReadSQLProcedureToDatatable.ResultTable

        For Each drow As DataRow In empshifttable.Rows
            Dim rowArray = drow.ItemArray
            dgvEmpShiftList.Rows.Add(rowArray)
        Next

        dgvEmpShiftList.PerformLayout()
        empshifttable.Dispose()
    End Sub

    Dim n_ShiftList As New ShiftList

    Private Sub dgvWeek_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvWeek.CellMouseDown
        If e.Button = Windows.Forms.MouseButtons.Right _
            And e.RowIndex > -1 Then

            dgvWeek.Item(e.ColumnIndex, e.RowIndex).Selected = True

            Dim ii = Application.OpenForms().OfType(Of ShiftList).Count

            Dim MousePosition As Point

            MousePosition = Cursor.Position

            Dim ptX = MousePosition.X - n_ShiftList.Width
            ptX = If(ptX < 0, 0, ptX)

            Dim ptY = MousePosition.Y - n_ShiftList.Height
            ptY = If(ptY < 0, 0, ptY)

            If ii > 0 Then

                n_ShiftList.Location = New Point(ptX,
                                                 ptY)

                n_ShiftList.BringToFront()
            Else
                n_ShiftList = New ShiftList

                n_ShiftList.Location = New Point(ptX,
                                                 ptY)

                If n_ShiftList.ShowDialog("") = Windows.Forms.DialogResult.OK Then

                    Dim i1 = Nothing
                    Dim i2 = Nothing
                    Dim i3 = Nothing

                    'Try
                    i1 = n_ShiftList.ShiftRowID
                    i2 = n_ShiftList.TimeFromValue
                    i3 = n_ShiftList.TimeToValue

                    For Each seldgvcells As DataGridViewCell In dgvWeek.SelectedCells

                        With seldgvcells 'dgvWeek.Rows(e.RowIndex).Cells(seldgvcells.ColumnIndex)

                            If i1 = Nothing Then

                                .Tag = Nothing

                                .Value = Nothing
                            Else

                                .Tag = i1

                                .Value =
                                    Format(i2, machineShortTime) & " to " &
                                    Format(i3, machineShortTime)

                            End If

                        End With

                    Next

                End If
            End If
        ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
        End If
    End Sub

    Private Sub dgvWeek_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvWeek.RowsAdded
        dgvWeek.Rows(e.RowIndex).Height = 75
    End Sub

    Private Sub TabPage1_Enter(sender As Object, e As EventArgs) Handles TabPage1.Enter
        TabPage1.Text = TabPage1.Text.Trim & Space(15)
        TabPage2.Text = TabPage2.Text.Trim
        tsbtnImportEmpShift.Enabled = True
        btnDelete.Enabled = True

        If btnNew.Enabled Then
            fillemployeeshiftSelected()
        End If

    End Sub

    Private Sub TabPage2_Enter(sender As Object, e As EventArgs) Handles TabPage2.Enter
        TabPage2.Text = TabPage2.Text.Trim & Space(15)
        TabPage1.Text = TabPage1.Text.Trim
        tsbtnImportEmpShift.Enabled = False

        Dim n_ReadSQLProcedureToDatatable As _
            New SQLQueryToDatatable("SELECT sh.RowID AS shRowID" &
                ",TIME_FORMAT(sh.TimeFrom, '%l:%i %p') AS TimeFrom" &
                ",TIME_FORMAT(sh.TimeTo, '%l:%i %p') AS TimeTo" &
                " FROM employeeshiftbyday esd" &
                " LEFT JOIN shift sh ON sh.RowID=esd.ShiftID AND sh.OrganizationID=esd.OrganizationID" &
                " WHERE esd.EmployeeID='" & dgvEmpList.Tag & "'" &
                " AND esd.OrganizationID='" & orgztnID & "'" &
                " ORDER BY esd.OrderByValue;")

        Dim shiftbyday As New DataTable

        shiftbyday = n_ReadSQLProcedureToDatatable.ResultTable

        dgvWeek.Rows.Clear()

        Dim n_row =
        dgvWeek.Rows.Add()

        Dim ii = 0

        For Each drow As DataRow In shiftbyday.Rows
            If IsDBNull(drow("shRowID")) = False Then
                dgvWeek.Item(ii, n_row).Value = drow("TimeFrom") & " to " & drow("TimeTo")
                dgvWeek.Item(ii, n_row).Tag = drow("shRowID")
            End If

            ii += 1
        Next
    End Sub

    Private Sub ByDateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ByDateToolStripMenuItem.Click
        CustomColoredTabControlActivateSelecting(True)
        CustomColoredTabControl1.SelectedIndex = 0
        CustomColoredTabControlActivateSelecting(False)
    End Sub

    Private Sub ByDayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ByDayToolStripMenuItem.Click
        CustomColoredTabControlActivateSelecting(True)
        CustomColoredTabControl1.SelectedIndex = 1
        chkbxNewShiftByDay.Checked = True
        CustomColoredTabControlActivateSelecting(False)
    End Sub

    Private Sub CustomColoredTabControl1_Selecting(sender As Object, e As TabControlCancelEventArgs) 'Handles CustomColoredTabControl1.Selecting
        e.Cancel = True
    End Sub

    Private Sub CustomColoredTabControlActivateSelecting(addhandlerToObject As Boolean)
        If addhandlerToObject Then
            RemoveHandler CustomColoredTabControl1.Selecting, AddressOf CustomColoredTabControl1_Selecting
        Else
            AddHandler CustomColoredTabControl1.Selecting, AddressOf CustomColoredTabControl1_Selecting
        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        btnSearch.Enabled = False

        fillemplyeelist()

        If dgvEmpList.RowCount > 0 Then
            dgvEmpList_CurrentCellChanged(dgvEmpList, New EventArgs)
        End If

        btnSearch.Enabled = True
    End Sub

    Private Sub txtEmpSearchBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEmpSearchBox.KeyPress
        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then
            btnSearch_Click(btnSearch, New EventArgs)
        End If
    End Sub

    Private Sub txtEmpSearchBox_TextChanged(sender As Object, e As EventArgs) Handles txtEmpSearchBox.TextChanged
        TextBox4.Text = txtEmpSearchBox.Text
    End Sub

    Dim divisionRowID = Nothing

    Private Sub trvDepartment_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles trvDepartment.AfterSelect
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

    Private Sub lnklblClearDivRowID_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblClearDivRowID.LinkClicked
        lnklblClearDivRowID.Enabled = False
        divisionRowID = 0
        btnSearch_Click(btnSearch, New EventArgs)
        lnklblClearDivRowID.Enabled = True
    End Sub

    Private Sub dgvEmpShiftList_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvEmpShiftList.CellMouseDown
        If e.Button = Windows.Forms.MouseButtons.Right _
            And btnDelete.Visible Then

            If e.ColumnIndex > -1 And e.RowIndex > -1 Then
                dgvEmpShiftList.Focus()
                dgvEmpShiftList.Item(e.ColumnIndex, e.RowIndex).Selected = True
                dgvEmpShiftList_CellClick(sender, New DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex))
                cmsDeleteShift.Show(MousePosition, ToolStripDropDownDirection.Default)
            End If
        End If
    End Sub

    Private Sub DeleteSelectedShiftToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteSelectedShiftToolStripMenuItem.Click
        Dim prompt = MessageBox.Show("Do you want to delete this employee shift ?", "Confirm deleting shift", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

        If prompt = Windows.Forms.DialogResult.Yes Then
            Dim selected_dgvcells As DataGridViewSelectedRowCollection = dgvEmpShiftList.SelectedRows
            Dim empshiftRowIDs As New List(Of String)
            Dim row_index As New List(Of String)

            For Each dgvrow As DataGridViewRow In selected_dgvcells
                empshiftRowIDs.Add(dgvrow.Cells("c_RowIDShift").Value)
                row_index.Add(dgvrow.Index)
            Next

            For Each strval In empshiftRowIDs
                EXECQUER("UPDATE employeetimeentry SET EmployeeShiftID=NULL WHERE EmployeeShiftID='" & strval & "';" &
                         "DELETE FROM employeeshift WHERE RowID='" & strval & "';" &
                         "ALTER TABLE employeeshift AUTO_INCREMENT = 0;")
            Next

            For Each rindx In row_index
                dgvEmpShiftList.Rows.Remove(dgvEmpShiftList.Rows(rindx))
            Next
        End If
    End Sub

    Private Sub tsbtnBulkEditShift_Click(sender As Object, e As EventArgs) Handles tsbtnBulkEditShift.Click
        Dim n_EmployeeShiftMassUpdate As New EmployeeShiftMassUpdate

        With n_EmployeeShiftMassUpdate
            .MinimizeBox = False
            .MaximizeBox = .MinimizeBox
            .StartPosition = FormStartPosition.CenterScreen
            .FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
        End With

        n_EmployeeShiftMassUpdate.ShowDialog("")
    End Sub

    Private Sub txtsrchEmpLName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtsrchEmpLName.KeyPress
        Dim e_asc As Integer = Asc(e.KeyChar)

        If e_asc = 13 Then
            btnSearch_Click(txtsrchEmpLName, New EventArgs)
        End If
    End Sub

    Private Sub CustomColoredTabControl1_Selecting_1(sender As Object, e As TabControlCancelEventArgs) Handles CustomColoredTabControl1.Selecting

    End Sub

    Private Sub CustomColoredTabControl1_SelectingTabPage(sender As Object, e As TabControlCancelEventArgs)

        e.Cancel =
            (sys_ownr.CurrentSystemOwner = SystemOwner.Cinema2000 _
            And CustomColoredTabControl1.SelectedIndex = 1)

    End Sub

    Private Sub setProperInterfaceBaseOnSystemOwner()

        Dim _bool As Boolean =
            (sys_ownr.CurrentSystemOwner = SystemOwner.Cinema2000)

        If _bool Then
            TabPage2.Text = String.Empty

            AddHandler CustomColoredTabControl1.Selecting, AddressOf CustomColoredTabControl1_SelectingTabPage

            Dim opposite_bool As Boolean = (Not _bool)

            ByDayToolStripMenuItem.Visible = opposite_bool

            tsbtnBulkEditShift.Visible = opposite_bool
        Else

        End If

    End Sub

    ''' <summary>
    ''' Model to be used for the employee shift data grid view.
    ''' </summary>
    Private Class ShiftModel

        Private Const TimeFormat = "hh\:mm"

        Private _shift As Shift

        Public ReadOnly Property Shift As Shift
            Get
                Return _shift
            End Get
        End Property

        Public Sub New(shift As Shift)
            _shift = shift
        End Sub

        Public ReadOnly Property Display As String
            Get
                Dim workPortion = $"{_shift.TimeFrom?.ToString(TimeFormat)} - {_shift.TimeTo?.ToString(TimeFormat)}"

                If _shift.HasBreaktime Then
                    Dim breakPortion = $"{_shift.BreaktimeFrom?.ToString(TimeFormat)} - {_shift.BreaktimeTo?.ToString(TimeFormat)}"

                    Return $"{workPortion} -- {breakPortion}"
                Else
                    Return workPortion
                End If
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Display
        End Function

    End Class

End Class