Imports Microsoft.Win32
Public Class DivisionForm

    Dim ParentDiv As New DataTable

    Dim ChiledDiv As New DataTable

    Dim ArrayWeekFormat() As String

    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Dim machineShortTime As String = RegKey.GetValue("sShortTime").ToString

    Dim IsNew As Integer = 0

    Dim divisiontbl As New DataTable

    Dim dontUpdate As SByte = 0

    Dim view_ID As Integer = Nothing


    Dim divisiontable As New DataTable

    Dim alphadivision As New DataTable

    Dim pagination As Integer = 0

    Dim currentNode As TreeNode = Nothing

    Dim currentDivisionRowID = Nothing

    Dim VIEW_division As New DataTable

    Dim n_ShiftList As New ShiftList

    Protected Overrides Sub OnLoad(e As EventArgs)

        OjbAssignNoContextMenu(txtgraceperiod)

        OjbAssignNoContextMenu(txtmindayperyear)

        OjbAssignNoContextMenu(txtslallow)
        OjbAssignNoContextMenu(txtvlallow)
        OjbAssignNoContextMenu(txtmlallow)
        OjbAssignNoContextMenu(txtpatlallow)

        Dim payfrequencytable As New DataTable

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT RowID,PayFrequencyType FROM payfrequency;")

        payfrequencytable = n_SQLQueryToDatatable.ResultTable

        cbopayfrequency.ValueMember = payfrequencytable.Columns(0).ColumnName

        cbopayfrequency.DisplayMember = payfrequencytable.Columns(1).ColumnName

        cbopayfrequency.DataSource = payfrequencytable

        'enlistTheLists("SELECT DisplayValue FROM listofval WHERE `Type`='Government deduction schedule' AND Active='Yes' ORDER BY OrderBy;", govdeducsched)

        n_SQLQueryToDatatable =
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

        MyBase.OnLoad(e)

    End Sub

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

            childnod.Tag = divrow

            childnod.NodeFont =
                New System.Drawing.Font("Segoe UI", 8.75!, System.Drawing.FontStyle.Regular)

        Next

    End Sub


    'Private Sub AddNode(parentNode As String, nodeText As String)
    '    Dim node As New List(Of TreeNode)
    '    node.AddRange(TreeView1.Nodes.Find(parentNode, True))
    '    If Not node.Any Then
    '        node.Add(TreeView1.Nodes.Add(parentNode, parentNode))
    '    End If
    '    node(0).Nodes.Add(nodeText, nodeText)
    'End Sub

    'Private Sub filltreeview()
    '    Dim dt As New DataTable()
    '    dt = getDataTableForSQL("Select * from division where DivisionType = 'Department' And OrganizationID = '" & z_OrganizationID & "'")
    '    TreeView1.Nodes.Clear()
    '    For Each dr As DataRow In dt.Rows

    '        Dim dvID As String = dr("RowID").ToString
    '        Dim dv As New DataTable
    '        dv = getDataTableForSQL("Select * from division Where ParentDivisionID = '" & Val(dvID) & "' And OrganizationID = '" & z_OrganizationID & "' And DivisionType = 'Branch'")

    '        For Each drow As DataRow In dv.Rows
    '            AddNode(dr("Name").ToString, drow("Name").ToString)
    '            Dim dvID2 As String = drow("RowID").ToString
    '            Dim dv2 As New DataTable
    '            dv2 = getDataTableForSQL("Select * from division Where ParentDivisionID = '" & Val(dvID2) & "' And OrganizationID = '" & z_OrganizationID & "' And DivisionType = 'Sub branch'")


    '            For Each drow2 As DataRow In dv2.Rows
    '                AddNode(drow("Name").ToString, drow2("Name").ToString)
    '            Next


    '        Next

    '    Next
    '    TreeView1.ExpandAll()
    'End Sub

    Sub fillDivisionCMB(Optional ExceptDivision As String = Nothing)

        Dim additionalQuery As String = Nothing

        If ExceptDivision = Nothing Then
            additionalQuery = ";"
        Else
            additionalQuery = " AND Name != '" & ExceptDivision & "';"
        End If

        Dim strQuery As String = "select Name from Division Where OrganizationID = '" & z_OrganizationID & "'" & additionalQuery

        cmbDivision.Items.Clear()
        cmbDivision.Items.Add("")
        cmbDivision.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbDivision.SelectedIndex = 0

    End Sub

    Private Sub fillDivision()
        Dim dt As New DataTable
        dt = getDataTableForSQL("SELECT d.*" & _
                                ",IFNULL(pf.PayFrequencyType,'') AS PayFrequencyType" & _
                                ",IFNULL(pf.RowID,'') AS pfRowID" & _
                                " FROM `division` d" & _
                                " LEFT JOIN payfrequency pf ON pf.RowID=d.PayFrequencyID" & _
                                " WHERE d.OrganizationID = '" & z_OrganizationID & "'" & _
                                " LIMIT " & pagination & ", 20;")

        dgvDivisionList.Rows.Clear()
        For Each row As DataRow In dt.Rows
            Dim n As Integer = dgvDivisionList.Rows.Add()
            With row
                Dim dvID As String = .Item("ParentDivisionID").ToString
                Dim dv As String = getStringItem("Select Name from division Where RowID = '" & Val(dvID) & "' And OrganizationID = '" & z_OrganizationID & "'")
                Dim getdv As String = dv

                dgvDivisionList.Rows.Item(n).Cells(c_divisionName.Index).Value = .Item("DivisionType").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_division.Index).Value = getdv
                dgvDivisionList.Rows.Item(n).Cells(c_name.Index).Value = .Item("Name").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_TradeName.Index).Value = .Item("TradeName").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_MainPhone.Index).Value = .Item("MainPhone").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_AltMainPhone.Index).Value = .Item("AltPhone").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_emailaddr.Index).Value = .Item("EmailAddress").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_altemailaddr.Index).Value = .Item("AltEmailAddress").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_FaxNo.Index).Value = .Item("FaxNumber").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_tinno.Index).Value = .Item("TinNo").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_url.Index).Value = .Item("URL").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_contactName.Index).Value = .Item("ContactName").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_businessaddr.Index).Value = .Item("BusinessAddress").ToString
                dgvDivisionList.Rows.Item(n).Cells(c_rowID.Index).Value = .Item("RowID").ToString

                dgvDivisionList.Rows.Item(n).Cells(GracePeriod.Index).Value = .Item("GracePeriod").ToString

                dgvDivisionList.Rows.Item(n).Cells(WorkDaysPerYear.Index).Value = .Item("WorkDaysPerYear").ToString

                dgvDivisionList.Rows.Item(n).Cells(PhHealthDeductSched.Index).Value = .Item("PhHealthDeductSched").ToString

                dgvDivisionList.Rows.Item(n).Cells(HDMFDeductSched.Index).Value = .Item("HDMFDeductSched").ToString
                
                dgvDivisionList.Rows.Item(n).Cells(SSSDeductSched.Index).Value = .Item("SSSDeductSched").ToString
                
                dgvDivisionList.Rows.Item(n).Cells(WTaxDeductSched.Index).Value = .Item("WTaxDeductSched").ToString


                dgvDivisionList.Rows.Item(n).Cells(DefaultVacationLeave.Index).Value = .Item("DefaultVacationLeave").ToString

                dgvDivisionList.Rows.Item(n).Cells(DefaultSickLeave.Index).Value = .Item("DefaultSickLeave").ToString

                dgvDivisionList.Rows.Item(n).Cells(DefaultMaternityLeave.Index).Value = .Item("DefaultMaternityLeave").ToString

                dgvDivisionList.Rows.Item(n).Cells(DefaultPaternityLeave.Index).Value = .Item("DefaultPaternityLeave").ToString
                
                dgvDivisionList.Rows.Item(n).Cells(DefaultOtherLeave.Index).Value = .Item("DefaultOtherLeave").ToString


                dgvDivisionList.Rows.Item(n).Cells(PayFrequencyType.Index).Value = .Item("PayFrequencyType").ToString

                dgvDivisionList.Rows.Item(n).Cells(PayFrequencyID.Index).Value = .Item("pfRowID").ToString


                dgvDivisionList.Rows.Item(n).Cells(PayFrequencyID.Index).Value = .Item("PhHealthDeductSchedAgency").ToString

                dgvDivisionList.Rows.Item(n).Cells(PayFrequencyID.Index).Value = .Item("HDMFDeductSchedAgency").ToString

                dgvDivisionList.Rows.Item(n).Cells(PayFrequencyID.Index).Value = .Item("SSSDeductSchedAgency").ToString

                dgvDivisionList.Rows.Item(n).Cells(PayFrequencyID.Index).Value = .Item("WTaxDeductSchedAgency").ToString

            End With

        Next


    End Sub

    Private Sub fillSelectedDivision()
        If dgvDivisionList.Rows.Count = 0 Then
        Else
            Dim dt As New DataTable
            'dt = getDataTableForSQL("Select * From Division Where OrganizationID = '" & z_OrganizationID & "' And RowID = '" & dgvDivisionList.CurrentRow.Cells(c_rowID.Index).Value & "'")

            dt = getDataTableForSQL("SELECT d.*" & _
                                    ",IFNULL(pf.PayFrequencyType,'') AS PayFrequencyType" & _
                                    ",IFNULL(pf.RowID,'') AS pfRowID" & _
                                    " FROM `division` d" & _
                                    " LEFT JOIN payfrequency pf ON pf.RowID=d.PayFrequencyID" & _
                                    " WHERE d.OrganizationID = '" & z_OrganizationID & "'" & _
                                    " AND d.RowID = '" & dgvDivisionList.CurrentRow.Cells(c_rowID.Index).Value & "'" & _
                                    " LIMIT " & pagination & ", 20;")

            For Each row As DataRow In dt.Rows

                With row
                    Dim dvID As String = .Item("ParentDivisionID").ToString
                    Dim dv As String = getStringItem("Select Name from division Where RowID = '" & Val(dvID) & "' And OrganizationID = '" & z_OrganizationID & "'")
                    Dim getdv As String = dv

                    cmbDivision.Text = dv
                    cmbDivisionType.Text = .Item("DivisionType").ToString
                    txtname.Text = .Item("Name").ToString
                    txttradename.Text = .Item("TradeName").ToString
                    txtmainphone.Text = .Item("MainPhone").ToString
                    txtaltphone.Text = .Item("AltPhone").ToString
                    txtemailaddr.Text = .Item("EmailAddress").ToString
                    txtaltemailaddr.Text = .Item("AltEmailAddress").ToString
                    txtfaxno.Text = .Item("FaxNumber").ToString
                    txttinno.Text = .Item("TinNo").ToString
                    txturl.Text = .Item("URL").ToString
                    txtcontantname.Text = .Item("ContactName").ToString
                    txtbusinessaddr.Text = .Item("BusinessAddress").ToString


                    txtgraceperiod.Text = .Item("GracePeriod").ToString

                    txtmindayperyear.Text = .Item("WorkDaysPerYear").ToString

                    cbophhdeductsched.Text = .Item("PhHealthDeductSched").ToString

                    cbohdmfdeductsched.Text = .Item("HDMFDeductSched").ToString

                    cbosssdeductsched.Text = .Item("SSSDeductSched").ToString

                    cboTaxDeductSched.Text = .Item("WTaxDeductSched").ToString


                    txtvlallow.Text = .Item("DefaultVacationLeave").ToString

                    txtslallow.Text = .Item("DefaultSickLeave").ToString

                    txtmlallow.Text = .Item("DefaultMaternityLeave").ToString

                    txtpatlallow.Text = .Item("DefaultPaternityLeave").ToString

                    txtotherallow.Text = .Item("DefaultOtherLeave").ToString

                    cbopayfrequency.Text = .Item("PayFrequencyType").ToString


                    cbophhdeductsched2.Text = .Item("PhHealthDeductSchedAgency").ToString

                    cbohdmfdeductsched2.Text = .Item("HDMFDeductSchedAgency").ToString

                    cbosssdeductsched2.Text = .Item("SSSDeductSchedAgency").ToString

                    cboTaxDeductSched2.Text = .Item("WTaxDeductSchedAgency").ToString

                    txtminwage.Text = .Item("MinimumWageAmount")

                End With

            Next

        End If

    End Sub

    Private Sub cleartext()
        txtaltemailaddr.Clear()
        txtaltphone.Clear()
        txtbusinessaddr.Clear()
        txtcontantname.Clear()
        cmbDivisionType.SelectedIndex = -1
        txtemailaddr.Clear()
        txtfaxno.Clear()
        txtmainphone.Clear()
        txtname.Clear()
        txttradename.Clear()
        txttinno.Clear()
        txturl.Clear()


        txtgraceperiod.Clear()

        txtmindayperyear.Clear()


        cbophhdeductsched.Text = String.Empty

        cbohdmfdeductsched.Text = String.Empty

        cbosssdeductsched.Text = String.Empty

        cboTaxDeductSched.Text = String.Empty


        txtvlallow.Clear()

        txtslallow.Clear()

        txtmlallow.Clear()

        txtpatlallow.Clear()

        txtotherallow.Clear()

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click, ToolStripButton2.Click, ToolStripButton3.Click
        Me.Close()

    End Sub



    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        cleartext()

        IsNew = 1

        btnNew.Enabled = False

        cmbDivisionType.Focus()

        divisiontbl = retAsDatTbl("SELECT RowID,Name FROM division WHERE OrganizationID=" & orgztnID & " AND ParentDivisionID IS NULL;")

        cmbDivision.Items.Clear()

        For Each drow As DataRow In divisiontbl.Rows
            cmbDivision.Items.Add(Trim(drow("Name")))
        Next

        chkbxautomaticOT.Checked = False

    End Sub



    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If cbopayfrequency.SelectedValue = Nothing Then
            cbopayfrequency.Focus()

            IsNew = 2

        End If

        If IsNew = 1 _
            And CustomColoredTabControl1.SelectedIndex = 0 Then

            SP_Division(Trim(txtname.Text), Trim(txtmainphone.Text), Trim(txtfaxno.Text), Trim(txtemailaddr.Text), Trim(txtaltemailaddr.Text), _
                        Trim(txtaltphone.Text), Trim(txturl.Text), z_datetime, z_User, z_datetime, z_User, Trim(txttinno.Text), _
                        Trim(txttradename.Text), Trim(cmbDivisionType.Text), Trim(txtbusinessaddr.Text), Trim(txtcontantname.Text),
                        z_OrganizationID,
                        FormatNumber(ValNoComma(txtgraceperiod.Text), 2).Replace(",", ""),
                        txtmindayperyear.Text,
                        cbophhdeductsched.Text,
                        cbohdmfdeductsched.Text,
                        cbosssdeductsched.Text,
                        cboTaxDeductSched.Text,
                        FormatNumber(ValNoComma(txtvlallow.Text), 2).Replace(",", ""),
                        FormatNumber(ValNoComma(txtslallow.Text), 2).Replace(",", ""),
                        FormatNumber(ValNoComma(txtmlallow.Text), 2).Replace(",", ""),
                        FormatNumber(ValNoComma(txtpatlallow.Text), 2).Replace(",", ""),
                        FormatNumber(ValNoComma(txtotherallow.Text), 2).Replace(",", ""),
                        cbopayfrequency.SelectedValue,
                        cbophhdeductsched2.Text,
                        cbohdmfdeductsched2.Text,
                        cbosssdeductsched2.Text,
                        cboTaxDeductSched2.Text,
                        txtminwage.Text,
                        chkbxautomaticOT.Tag)

            If cmbDivision.Text IsNot Nothing Then
                Dim dvID As String = getStringItem("Select RowID from division Where Name = '" & txtname.Text & "' And OrganizationID = '" & z_OrganizationID & "'")
                Dim getdvID As Integer = Val(dvID)
                Dim dv As String = getStringItem("Select RowID from division Where Name = '" & cmbDivision.Text & "' And OrganizationID = '" & z_OrganizationID & "'")
                Dim getdv As Integer = Val(dv)
                DirectCommand("UPDATE division SET ParentDivisionID = '" & getdv & "' Where RowID = '" & getdvID & "'")
            End If
            fillDivision()
            'filltreeview()

        ElseIf IsNew = 0 _
            And CustomColoredTabControl1.SelectedIndex = 0 Then

            If dontUpdate = 1 Then
                Exit Sub
            End If

            SP_DivisionUpdate(txtname.Text, txtmainphone.Text, txtfaxno.Text, txtemailaddr.Text, txtaltemailaddr.Text, _
                       txtaltphone.Text, txturl.Text, z_datetime, z_User, txttinno.Text, _
                       txttradename.Text, cmbDivisionType.Text, txtbusinessaddr.Text, txtcontantname.Text,
                       currentDivisionRowID,
                        FormatNumber(ValNoComma(txtgraceperiod.Text), 2).Replace(",", ""),
                        txtmindayperyear.Text,
                        cbophhdeductsched.Text,
                        cbohdmfdeductsched.Text,
                        cbosssdeductsched.Text,
                        cboTaxDeductSched.Text,
                        FormatNumber(ValNoComma(txtvlallow.Text), 2).Replace(",", ""),
                        FormatNumber(ValNoComma(txtslallow.Text), 2).Replace(",", ""),
                        FormatNumber(ValNoComma(txtmlallow.Text), 2).Replace(",", ""),
                        FormatNumber(ValNoComma(txtpatlallow.Text), 2).Replace(",", ""),
                        FormatNumber(ValNoComma(txtotherallow.Text), 2).Replace(",", ""),
                        cbopayfrequency.SelectedValue,
                        cbophhdeductsched2.Text,
                        cbohdmfdeductsched2.Text,
                        cbosssdeductsched2.Text,
                        cboTaxDeductSched2.Text,
                        txtminwage.Text,
                        chkbxautomaticOT.Tag)

            If cmbDivision.Text IsNot Nothing Then
                Dim dvID As String = getStringItem("Select RowID from division Where Name = '" & txtname.Text & "' And OrganizationID = '" & z_OrganizationID & "'")
                Dim getdvID As Integer = Val(dvID)
                Dim dv As String = getStringItem("Select RowID from division Where Name = '" & cmbDivision.Text & "' And OrganizationID = '" & z_OrganizationID & "'")
                Dim getdv As Integer = Val(dv)
                DirectCommand("UPDATE division SET ParentDivisionID = '" & getdv & "' Where RowID = '" & getdvID & "'")
            End If

            fillDivision()
            'filltreeview()

        End If

        IsNew = 0

        btnSave.Enabled = True

    End Sub

    Private Sub DivisionForm_EnabledChanged(sender As Object, e As EventArgs) Handles Me.EnabledChanged
        tsProgresSavingShift.Visible = Not Me.Enabled 'Me.Enabled
    End Sub

    Private Sub DivisionForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If bgworkDelDeptShift.IsBusy Then

            e.Cancel = True

        Else

            If previousForm IsNot Nothing Then
                If previousForm.Name = Me.Name Then
                    previousForm = Nothing
                End If
            End If


            HRISForm.listHRISForm.Remove(Me.Name)

        End If

    End Sub


    Private Sub DivisionForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        'fillDivision()

        'fillSelectedDivision()

        'filltreeview()

        'divisiontable = retAsDatTbl("SELECT * FROM division WHERE OrganizationID=" & orgztnID & ";")

        'alphadivision = retAsDatTbl("SELECT * FROM division WHERE OrganizationID=" & orgztnID & " AND ParentDivisionID IS NULL;")

        'For Each drow As DataRow In alphadivision.Rows

        '    Divisiontreeviewfiller(drow("RowID"), drow("Name"), )

        'Next

        'TreeView1.ExpandAll()

        view_ID = VIEW_privilege("Division", orgztnID)

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

        If dgvDivisionList.RowCount <> 0 Then
            fillDivisionCMB(dgvDivisionList.Item("c_name", 0).Value)
        Else

        End If

    End Sub

    Sub Divisiontreeviewfiller(Optional primkey As Object = Nothing, _
                       Optional strval As Object = Nothing, _
                       Optional trnod As TreeNode = Nothing)

        Dim n_nod As TreeNode = Nothing

        strval = strval & "[Department]"

        If trnod Is Nothing Then
            'n_nod = TreeView1.Nodes.Add(primkey, strval)
        Else
            n_nod = trnod.Nodes.Add(primkey, strval)
        End If

        Dim selchild = divisiontable.Select("ParentDivisionID=" & primkey)

        For Each drow In selchild

            Divisiontreeviewfiller(drow("RowID"), drow("Name"), n_nod)

        Next

    End Sub
    Private Sub dgvDivisionList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDivisionList.CellClick

        'fillSelectedDivision()

        'If dgvDivisionList.RowCount <> 0 Then

        '    fillDivisionCMB(dgvDivisionList.CurrentRow.Cells("c_name").Value)

        'Else

        'End If

    End Sub

    Private Sub tsAudittrail_Click(sender As Object, e As EventArgs) Handles tsAudittrail.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        btnNew.Enabled = True

        If dgvDivisionList.RowCount <> 0 Then
            dgvDivisionList_CellClick(sender, New DataGridViewCellEventArgs(c_divisionName.Index, 0))

        End If

        IsNew = 0

    End Sub

    Private Sub txtgraceperiod_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtgraceperiod.KeyPress

        Dim e_KAsc As String = Asc(e.KeyChar)

        Static onedot As SByte = 0

        If (e_KAsc >= 48 And e_KAsc <= 57) Or e_KAsc = 8 Or e_KAsc = 46 Then

            If e_KAsc = 46 Then

                onedot += 1

                If onedot >= 2 Then

                    If txtgraceperiod.Text.Contains(".") Then
                        e.Handled = True

                        onedot = 2

                    Else
                        e.Handled = False

                        onedot = 0

                    End If

                Else
                    If txtgraceperiod.Text.Contains(".") Then
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

    Private Sub txtmindayperyear_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtmindayperyear.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        Dim n_TrapDecimalKey As New TrapDecimalKey(e_asc, txtmindayperyear.Text)

        e.Handled = n_TrapDecimalKey.ResultTrap

    End Sub

    Private Sub Leaves_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtslallow.KeyPress, _
                                                                                    txtvlallow.KeyPress, _
                                                                                    txtmlallow.KeyPress, _
                                                                                    txtpatlallow.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        Dim obj_sndr = DirectCast(sender, TextBox)

        Dim n_TrapDecimalKey As New TrapDecimalKey(e_asc, obj_sndr.Text)

        e.Handled = n_TrapDecimalKey.ResultTrap

    End Sub

    Private Sub Pagination_Link(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked,
                                                                                                Prev.LinkClicked,
                                                                                                Nxt.LinkClicked,
                                                                                                Last.LinkClicked
        'RemoveHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "First" Then
            pagination = 0
        ElseIf sendrname = "Prev" Then

            Dim modcent = pagination Mod 20

            If modcent = 0 Then

                pagination -= 20

            Else

                pagination -= modcent

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sendrname = "Nxt" Then

            Dim modcent = pagination Mod 20

            If modcent = 0 Then
                pagination += 20

            Else
                pagination -= modcent

                pagination += 20

            End If
        ElseIf sendrname = "Last" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 20 FROM employee WHERE OrganizationID=" & orgztnID & ";"))



            Dim remender = lastpage Mod 1
            
            pagination = (lastpage - remender) * 50

            If pagination - 20 < 20 Then
                'pagination = 0

            End If

            'pagination = If(lastpage - 20 >= 20, _
            '                lastpage - 20, _
            '                lastpage)

        End If

        btnRefresh_Click(sender, e)

        'dgvemployees_SelectionChanged(sender, e)

        'AddHandler dgvemployees.SelectionChanged, AddressOf dgvemployees_SelectionChanged

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        VIEW_division = New DataTable

        Dim n_ReadSQLProcedureToDatatable As New ReadSQLProcedureToDatatable("VIEW_division",
                                                                             orgztnID,
                                                                             autcomptxtdivision.Text.Trim)

        VIEW_division = n_ReadSQLProcedureToDatatable.ResultTable

        dgvDivisionList.Rows.Clear()

        For Each drow As DataRow In VIEW_division.Rows

            Dim rowarray = drow.ItemArray

            dgvDivisionList.Rows.Add(rowarray)

        Next

    End Sub

    Private Sub txtminwage_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtminwage.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        Dim n_TrapDecimalKey As New TrapDecimalKey(e_asc, txtminwage.Text)

        e.Handled = n_TrapDecimalKey.ResultTrap

    End Sub

    Private Sub trvDepartment_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles trvDepartment.AfterSelect

        If currentNode Is Nothing Then
            
            clearObjControl(Panel1)
            clearObjControl(GroupBox1)
            clearObjControl(TabPage2)
            clearObjControl(TabPage3)

            txtDivLocName.Text = ""

            currentDivisionRowID = Nothing

        Else

            If TypeOf currentNode.Tag Is DataRow Then


                Dim user_doing_works_in_shift As Boolean = False

                If CustomColoredTabControl1.SelectedIndex = 2 Then
                    user_doing_works_in_shift = True

                Else

                    CustomColoredTabControl1.SelectedIndex = 0

                End If
                Dim ndrow = DirectCast(currentNode.Tag, DataRow)

                Dim n_drow As DataRow

                n_drow = ndrow

                currentDivisionRowID = n_drow("RowID")

                cmbDivision.Text = n_drow("ParentDivisionName").ToString
                cmbDivisionType.Text = n_drow("DivisionType").ToString
                txtname.Text = n_drow("Name").ToString
                txttradename.Text = n_drow("TradeName").ToString
                txtmainphone.Text = n_drow("MainPhone").ToString
                txtaltphone.Text = n_drow("AltPhone").ToString
                txtemailaddr.Text = n_drow("EmailAddress").ToString
                txtaltemailaddr.Text = n_drow("AltEmailAddress").ToString
                txtfaxno.Text = n_drow("FaxNumber").ToString
                txttinno.Text = n_drow("TinNo").ToString
                txturl.Text = n_drow("URL").ToString
                txtcontantname.Text = n_drow("ContactName").ToString
                txtbusinessaddr.Text = n_drow("BusinessAddress").ToString


                txtgraceperiod.Text = n_drow("GracePeriod").ToString

                txtmindayperyear.Text = n_drow("WorkDaysPerYear").ToString

                cbophhdeductsched.Text = n_drow("PhHealthDeductSched").ToString

                cbohdmfdeductsched.Text = n_drow("HDMFDeductSched").ToString

                cbosssdeductsched.Text = n_drow("SSSDeductSched").ToString

                cboTaxDeductSched.Text = n_drow("WTaxDeductSched").ToString


                txtvlallow.Text = n_drow("DefaultVacationLeave").ToString

                txtslallow.Text = n_drow("DefaultSickLeave").ToString

                txtmlallow.Text = n_drow("DefaultMaternityLeave").ToString

                txtpatlallow.Text = n_drow("DefaultPaternityLeave").ToString

                txtotherallow.Text = n_drow("DefaultOtherLeave").ToString

                cbopayfrequency.Text = n_drow("PayFrequencyType").ToString


                cbophhdeductsched2.Text = n_drow("PhHealthDeductSchedAgency").ToString

                cbohdmfdeductsched2.Text = n_drow("HDMFDeductSchedAgency").ToString

                cbosssdeductsched2.Text = n_drow("SSSDeductSchedAgency").ToString

                cboTaxDeductSched2.Text = n_drow("WTaxDeductSchedAgency").ToString

                txtminwage.Text = n_drow("MinimumWageAmount")

                If IsDBNull(n_drow("AutomaticOvertimeFiling")) Then

                    chkbxautomaticOT.Checked = False

                Else

                    chkbxautomaticOT.Checked = Convert.ToInt16(n_drow("AutomaticOvertimeFiling"))

                End If

                If user_doing_works_in_shift Then

                    CustomColoredTabControl1.SelectedIndex = 2

                    tbpageShift_Enter(tbpageShift, New EventArgs)

                End If

                'If CustomColoredTabControl1.SelectedIndex = 2 Then

                'End If
            Else

                CustomColoredTabControl1.SelectedIndex = 1

                currentDivisionRowID = currentNode.Tag

                txtDivLocName.Text = currentNode.Text.Trim

                chkbxautomaticOT.Checked = False

            End If

            AddHandler CustomColoredTabControl1.Selecting, AddressOf CustomColoredTabControl1_Selecting

            trvDepartment.Focus()

        End If

    End Sub

    Private Sub trvDepartment_BeforeSelect(sender As Object, e As TreeViewCancelEventArgs) Handles trvDepartment.BeforeSelect

        Try
            currentNode = e.Node
        Catch ex As Exception
            currentNode = Nothing
            MsgBox(getErrExcptn(ex, Me.Name))

        Finally
            RemoveHandler CustomColoredTabControl1.Selecting, AddressOf CustomColoredTabControl1_Selecting

        End Try

    End Sub

    Private Sub CustomColoredTabControl1_Selecting(sender As Object, e As TabControlCancelEventArgs) ' Handles CustomColoredTabControl1.Selecting

        Try

            If TypeOf currentNode.Tag Is DataRow _
                And e.TabPageIndex <> 1 Then

                e.Cancel = False

            Else

                e.Cancel = True

            End If

        Catch ex As Exception

            e.Cancel = True

        End Try
    End Sub

    Private Sub tsbtnNewDivLoc_Click(sender As Object, e As EventArgs) Handles tsbtnNewDivLoc.Click

        tsbtnNewDivLoc.Enabled = False

        txtDivLocName.Text = ""

        txtDivLocName.Focus()

    End Sub

    Private Sub tsbtnSaveDivLoc_Click(sender As Object, e As EventArgs) Handles tsbtnSaveDivLoc.Click

        Dim NewValueString = txtDivLocName.Text.Trim

        Dim customRowID = If(tsbtnNewDivLoc.Enabled, currentDivisionRowID, DBNull.Value)
        
        Dim n_ReadSQLFunction As _
            New ReadSQLFunction("INSUPD_division_location",
                                "returnvalue",
                                customRowID,
                                orgztnID,
                                NewValueString,
                                z_User)

        If tsbtnNewDivLoc.Enabled = False Then

            LoadDivision()

        Else
            currentNode.Text = NewValueString & Space(5)

        End If

        tsbtnNewDivLoc.Enabled = True

    End Sub

    Private Sub tsbtnCancelDivLoc_Click(sender As Object, e As EventArgs) Handles tsbtnCancelDivLoc.Click

        txtDivLocName.Text = currentNode.Text.Trim

        tsbtnNewDivLoc.Enabled = True

    End Sub

    Private Sub chkbxautomaticOT_CheckedChanged(sender As Object, e As EventArgs) Handles chkbxautomaticOT.CheckedChanged
        chkbxautomaticOT.Tag = "0"
        If chkbxautomaticOT.Checked Then
            chkbxautomaticOT.Tag = "1"
        End If
    End Sub

    Private Sub tbpageShift_Enter(sender As Object, e As EventArgs) Handles tbpageShift.Enter

        If tsbtnNewShift.Enabled = False Then
            tsbtnNewShift.Enabled = True
        End If

        Dim n_ReadSQLProcedureToDatatable As _
            New SQLQueryToDatatable("CALL `VIEW_division_shift`('" & orgztnID & "', '" & currentDivisionRowID & "');")

        Dim shiftbyday As New DataTable

        shiftbyday = n_ReadSQLProcedureToDatatable.ResultTable

        dgvWeek.Rows.Clear()

        Dim n_row = _
            dgvWeek.Rows.Add()

        Dim ii = 0

        Try

            For Each drow As DataRow In shiftbyday.Rows

                If IsDBNull(drow("shRowID")) = False Then
                    dgvWeek.Item(ii, n_row).Value = drow("TimeFrom") & " to " & drow("TimeTo")
                    dgvWeek.Item(ii, n_row).Tag = drow("shRowID")
                End If

                ii += 1

            Next

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try

        CustomColoredTabControl2.SelectedIndex = 1

    End Sub

    Private Sub dgvWeek_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvWeek.CellMouseDown

        If e.Button = Windows.Forms.MouseButtons.Right _
            And e.RowIndex > -1 Then

            dgvWeek.Item(e.ColumnIndex, e.RowIndex).Selected = True

            'dgvcalendar_CellContentClick(dgvcalendar, New DataGridViewCellEventArgs(1, 1))

            'If Application.OpenForms().OfType(Of ShiftList).Count Then
            'End If

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

                'For i = 0 To ii
                '    ShiftList.Close()
                '    ShiftList.Dispose()
                'Next

                'n_ShiftList.Size = New Size(1, 1)

                'n_ShiftList.Location = New Point(MousePosition.X,
                '                                 MousePosition.Y)

                'n_ShiftList.Location = New Point((MousePosition.X + n_ShiftList.Width),
                '                                 (MousePosition.Y + n_ShiftList.Height))

                'n_ShiftList.Size = New Size(493, 401)

                n_ShiftList = New ShiftList

                n_ShiftList.Location = New Point(ptX,
                                                 ptY)

                'n_ShiftList.Show()
                If n_ShiftList.ShowDialog("") = Windows.Forms.DialogResult.OK Then

                    Dim i1 = Nothing
                    Dim i2 = Nothing
                    Dim i3 = Nothing

                    'Try
                    i1 = n_ShiftList.ShiftRowID
                    i2 = n_ShiftList.TimeFromValue
                    i3 = n_ShiftList.TimeToValue
                    'Catch ex As Exception
                    '    MsgBox(getErrExcptn(ex, Me.Name), , "n_ShiftList.ShowDialog")
                    'End Try

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
                'n_ShiftList.ShowDialog("")

            End If

        ElseIf e.Button = Windows.Forms.MouseButtons.Left Then

        End If

    End Sub

    Private Sub dgvWeek_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvWeek.RowsAdded
        dgvWeek.Rows(e.RowIndex).Height = 75
    End Sub

    Private Sub tsbtnNewShift_Click(sender As Object, e As EventArgs) Handles tsbtnNewShift.Click

        tsbtnNewShift.Enabled = False

        Select Case CustomColoredTabControl2.SelectedIndex

            Case 0

                Dim n_ShiftTemplater As New ShiftTemplater

                'n_ShiftTemplater.ShowDialog()

                If n_ShiftTemplater.ShowDialog() = Windows.Forms.DialogResult.OK Then

                    tbpageDate_Enter(sender, e)

                End If

            Case 1

        End Select

    End Sub

    Private Sub tsbtnNewShift_EnabledChanged(sender As Object, e As EventArgs) Handles tsbtnNewShift.EnabledChanged

        If tsbtnNewShift.Enabled = False Then

            tbpageShift.Focus()

        End If

    End Sub

    Private Sub tsbtnSaveShift_Click(sender As Object, e As EventArgs) Handles tsbtnSaveShift.Click

        tsbtnSaveShift.Enabled = False

        tsProgresSavingShift.Value = 0

        Me.Enabled = False

        bgworkShiftSaving.RunWorkerAsync()

    End Sub

    Private Sub tsbtnCancelShift_Click(sender As Object, e As EventArgs) Handles tsbtnCancelShift.Click

        tbpageShift_Enter(tbpageShift, New EventArgs)

    End Sub

    Private Sub bgworkShiftSaving_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkShiftSaving.DoWork

        Dim customArrayWeekFormat As String = ""

        For Each strval In ArrayWeekFormat
            customArrayWeekFormat &= ",'" & strval & "'"
        Next 'concatenates all the day names, separated with comma sign

        Dim trimcustomArrayWeekFormat = customArrayWeekFormat.Substring(1, (customArrayWeekFormat.Length - 1))
        'the result of this variable is the concatenation of the day names in a week

        'customEmployeeShift(EmpStartDate,AddDay,ShiftRowID)

        '",EXISTS(SELECT RowID" &
        '" FROM employeeshiftbyday" &
        '" WHERE EmployeeID=e.RowID" &
        '" AND OrganizationID=e.OrganizationID" &
        ''" AND SampleDate BETWEEN ADDDATE(SampleDate, INTERVAL (1 - DAYOFWEEK(SampleDate)) DAY)" &
        '" AND ADDDATE(SampleDate, INTERVAL (7 - DAYOFWEEK(SampleDate)) DAY)) AS ShiftByDayIsExists" &

        Dim reset_employeeshift_ofthisdivision =
            New ExecuteQuery("CALL DEL_division_shift('" & orgztnID & "'" &
                             ",'" & currentDivisionRowID & "'" &
                             ");")

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("SELECT e.*" &
                                    " FROM employee e" &
                                    " INNER JOIN position pos ON pos.DivisionId='" & currentDivisionRowID & "' AND pos.OrganizationID=e.OrganizationID AND pos.RowID=e.PositionID" &
                                    " WHERE e.OrganizationID='" & orgztnID & "';")

        Dim dtemployee_ofdivision As New DataTable

        dtemployee_ofdivision = n_SQLQueryToDatatable.ResultTable

        'Dim EmployeeStartingDate = New ExecuteQuery("SELECT StartDate FROM employee WHERE RowID='" & dgvEmpList.Tag & "' AND OrganizationID='" & orgztnID & "';").Result

        Dim customEmployeeShift As New DataTable

        With customEmployeeShift.Columns
            .Add("EmpStartDate")
            .Add("AddDay")
            .Add("ShiftRowID")
            .Add("NameOfDay")
        End With

        Dim shiftrowIDs As New List(Of String)

        Dim shiftbydayIsExists As Boolean = False

        Dim work_indx = 1

        Dim count_employee_of_this_division = dtemployee_ofdivision.Rows.Count

        Dim colBValues = (From row In dtemployee_ofdivision Select colB = row("RowID").ToString).ToList

        'For Each strval In colBValues

        '    Dim ala_ii = strval

        'Next

        For Each edrow As DataRow In dtemployee_ofdivision.Rows

            'shiftbydayIsExists = (Val(edrow("ShiftByDayIsExists")) = 1)

            'New ExecuteQuery("SELECT EXISTS(SELECT RowID" &
            '                 " FROM employeeshiftbyday" &
            '                 " WHERE EmployeeID='" & edrow("RowID") & "'" &
            '                 " AND OrganizationID='" & orgztnID & "'" &
            '                 " LIMIT 1);").Result

            If shiftbydayIsExists Then

            Else

            End If

            'If chkbxNewShiftByDay.Checked Then

            'Else

            'End If

            For Each dgvcol As DataGridViewColumn In dgvWeek.Columns

                For Each dgvrow As DataGridViewRow In dgvWeek.Rows

                    Dim bool_result = Convert.ToInt16(Not tsbtnNewShift.Enabled)

                    Dim IDShift = dgvWeek.Item(dgvcol.Index, dgvrow.Index).Tag

                    Dim n_ReadSQLFunction As _
                        New ReadSQLFunction("INSUPD_employeeshiftbyday",
                                                "returnvalue",
                                            orgztnID,
                                            z_User,
                                            edrow("RowID"),
                                            If(IDShift = Nothing, DBNull.Value, IDShift),
                                            ArrayWeekFormat(dgvcol.Index),
                                            "0",
                                            If(IDShift = Nothing, 1, 0),
                                            dgvcol.Index,
                                            bool_result)

                    Exit For

                Next

            Next

            'If shiftbydayIsExists = False Then

            Dim n_ExecuteQuery As _
                New ExecuteQuery("CALL AUTOMATICUPD_employeeshiftbyday('" & orgztnID & "','" & edrow("RowID") & "');")

            'Else

            '    If tsbtnNewShift.Enabled Then

            '        'Dim n_ExecuteQuery As _
            '        '    New ExecuteQuery("CALL AUTOMATICUPD_employeeshiftbyday('" & orgztnID & "','" & edrow("RowID") & "');")

            '    End If

            'End If

            Dim progressvalue = CInt((work_indx / count_employee_of_this_division) * 100)

            work_indx += 1

            bgworkShiftSaving.ReportProgress(progressvalue)

        Next

        customEmployeeShift.Dispose()

        'Dim n_SQLQueryToDatatable As _
        '    New SQLQueryToDatatable("SELECT * FROM employeeshiftbyday WHERE EmployeeID='" & dgvEmpList.Tag & "' AND OrganizationID='" & orgztnID & "' ORDER BY OrderByValue;")

        'dgvEmpList.Tag

    End Sub

    Private Sub bgworkShiftSaving_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkShiftSaving.ProgressChanged

        tsProgresSavingShift.Value = CType(e.ProgressPercentage, Integer)

    End Sub

    Private Sub bgworkShiftSaving_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkShiftSaving.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox(getErrExcptn(e.Error, Me.Name))
        ElseIf e.Cancelled Then
            MessageBox.Show("Process has been cancelled.")
        Else


            'bgworkUpdateEmployeeShiftIDOfEmployeetimeentry.RunWorkerAsync()

        End If

        Me.Enabled = True

        tsbtnNewShift.Enabled = True

        tsbtnSaveShift.Enabled = True

    End Sub

    Private Sub bgworkUpdateEmployeeShiftIDOfEmployeetimeentry_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkUpdateEmployeeShiftIDOfEmployeetimeentry.DoWork

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("SELECT e.*" &
                                    ",EXISTS(SELECT RowID FROM employeeshiftbyday WHERE EmployeeID=e.RowID AND OrganizationID=e.OrganizationID LIMIT 1) AS ShiftByDayIsExists" &
                                    " FROM employee e" &
                                    " INNER JOIN position pos ON pos.DivisionId='" & currentDivisionRowID & "' AND pos.OrganizationID=e.OrganizationID AND pos.RowID=e.PositionID" &
                                    " WHERE e.OrganizationID='" & orgztnID & "';")

        Dim dtemployee_ofdivision As New DataTable

        dtemployee_ofdivision = n_SQLQueryToDatatable.ResultTable

        Dim work_indx = 0

        Dim count_employee_of_this_division = dtemployee_ofdivision.Rows.Count

        For Each edrow As DataRow In dtemployee_ofdivision.Rows

            Dim progressvalue = CInt((work_indx / count_employee_of_this_division) * 50)

            bgworkShiftSaving.ReportProgress(progressvalue)

        Next

    End Sub

    Private Sub bgworkUpdateEmployeeShiftIDOfEmployeetimeentry_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkUpdateEmployeeShiftIDOfEmployeetimeentry.ProgressChanged

        tsProgresSavingShift.Value += CType(e.ProgressPercentage, Integer)

    End Sub

    Private Sub bgworkUpdateEmployeeShiftIDOfEmployeetimeentry_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkUpdateEmployeeShiftIDOfEmployeetimeentry.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox(getErrExcptn(e.Error, Me.Name))
        ElseIf e.Cancelled Then
            MessageBox.Show("Process has been cancelled.")
        Else

        End If

    End Sub

    Private Sub btnDelete_VisibleChanged(sender As Object, e As EventArgs) Handles btnDelete.VisibleChanged

        tsbtnDelAllShiftOfThisDivision.Visible = btnDelete.Visible

    End Sub

    Private Sub tsbtnDelAllShiftOfThisDivision_Click(sender As Object, e As EventArgs) Handles tsbtnDelAllShiftOfThisDivision.Click

        If currentNode IsNot Nothing Then

            Dim result = MessageBox.Show("Are you sure you want to delete" & vbNewLine &
                                         "   shift of this department ?",
                                         "Delete all shift of " & currentNode.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)

            If result = DialogResult.Yes Then

                tsbtnDelAllShiftOfThisDivision.Enabled = False

                RemoveHandler tsbtnDelAllShiftOfThisDivision.EnabledChanged, AddressOf tsbtnDelAllShiftOfThisDivision_EnabledChanged

                bgworkDelDeptShift.RunWorkerAsync()

            End If

        End If

    End Sub

    Private Sub bgworkDelDeptShift_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkDelDeptShift.DoWork

        AddHandler tsbtnDelAllShiftOfThisDivision.EnabledChanged, AddressOf tsbtnDelAllShiftOfThisDivision_EnabledChanged

        Dim reset_employeeshift_ofthisdivision =
            New ExecuteQuery("CALL DEL_division_shift('" & orgztnID & "'" &
                             ",'" & currentDivisionRowID & "'" &
                             ");")

        bgworkShiftSaving.ReportProgress(75)

        reset_employeeshift_ofthisdivision =
            New ExecuteQuery("ALTER TABLE employeeshift AUTO_INCREMENT = 0;")

        bgworkShiftSaving.ReportProgress(80)

        reset_employeeshift_ofthisdivision =
            New ExecuteQuery("ALTER TABLE employeeshiftbyday AUTO_INCREMENT = 0;")

        bgworkShiftSaving.ReportProgress(85)

        reset_employeeshift_ofthisdivision =
            New ExecuteQuery("ALTER TABLE employeefirstweekshift AUTO_INCREMENT = 0;")

        bgworkShiftSaving.ReportProgress(99)

    End Sub

    Private Sub bgworkDelDeptShift_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkDelDeptShift.ProgressChanged

        tsProgresSavingShift.Value = CType(e.ProgressPercentage, Integer)

    End Sub

    Private Sub bgworkDelDeptShift_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkDelDeptShift.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MsgBox(getErrExcptn(e.Error, Me.Name))
        ElseIf e.Cancelled Then
            MessageBox.Show("Process has been cancelled.")
        Else

            tsProgresSavingShift.Value = 100

        End If

        tsbtnDelAllShiftOfThisDivision.Enabled = True

        RemoveHandler tsbtnDelAllShiftOfThisDivision.EnabledChanged, AddressOf tsbtnDelAllShiftOfThisDivision_EnabledChanged

        tsProgresSavingShift.Value = 0

        tsProgresSavingShift.Visible = False

    End Sub

    Private Sub tbpageDate_Enter(sender As Object, e As EventArgs) Handles tbpageDate.Enter

        Static once As SByte = 0

        If once = 0 Then

            once = 1

        End If

        'divisionshiftdate

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("SELECT dsd.RowID" &
                                    ",IFNULL(TIME_FORMAT(sh.TimeFrom, '%l:%i %p'),'') AS TimeFrom" &
                                    ",IFNULL(TIME_FORMAT(sh.TimeTo, '%l:%i %p'),'') AS TimeTo" &
                                    ",dsd.EffectiveFrom" &
                                    ",dsd.EffectiveTo" &
                                    ",dsd.RestDay" &
                                    " FROM divisionshiftdate dsd" &
                                    " LEFT JOIN shift sh ON sh.RowID=dsd.ShiftID" &
                                    " WHERE dsd.DivisionID='" & currentDivisionRowID & "'" &
                                    " AND dsd.OrganizationID='" & orgztnID & "'" &
                                    " ORDER BY dsd.EffectiveFrom, dsd.EffectiveTo" &
                                    " LIMIT " & 0 & ", 20;")

        Dim divisionshiftdate As New DataTable

        dgvDivisionShift.Rows.Clear()

        'For Each drow As DataRow In divisionshiftdate.Rows

        '    Dim row_array = drow.ItemArray

        '    dgvDivisionShift.Rows.Add(row_array)

        'Next

        If divisionshiftdate IsNot Nothing Then
            divisionshiftdate.Dispose()
        End If

    End Sub

    Private Sub tsbtnDelAllShiftOfThisDivision_EnabledChanged(sender As Object, e As EventArgs) 'Handles tsbtnDelAllShiftOfThisDivision.EnabledChanged

        Dim booleanresult As Boolean = False
        Try
            booleanresult = bgworkDelDeptShift.IsBusy
        Catch ex As Exception

        Finally
            ToolStrip3.Enabled = Not booleanresult
        End Try

    End Sub

    Private Sub CustomColoredTabControl2_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles CustomColoredTabControl2.Selecting

        Select Case CustomColoredTabControl2.SelectedIndex

            Case 0
                e.Cancel = True
            Case 1
                e.Cancel = False

        End Select

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked

        If currentDivisionRowID = Nothing Then

        Else

            Dim n_DepartmentMinWages As _
                New DepartmentMinWages(currentDivisionRowID)

            If n_DepartmentMinWages.ShowDialog = Windows.Forms.DialogResult.OK Then



            End If

        End If

    End Sub

End Class