Imports Femiani.Forms.UI.Input

Public Class Agency

    Public ReadOnly Property ViewIdentification As Object

        Get

            'agencyview_ID = VIEW_privilege("Agency", orgztnID)

            Return agencyview_ID

        End Get

    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)

        btnRefresh_Click(btnRefresh, e)

        MyBase.OnLoad(e)

    End Sub

    Private Sub Agency_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

    Dim agencyview_ID = VIEW_privilege("Agency", orgztnID)

    Dim dontUpdateAgency = Nothing

    Private Sub Agency_Load(sender As Object, e As EventArgs) Handles Me.Load
        If agencyview_ID = Nothing Then
            agencyview_ID = VIEW_privilege("Agency", orgztnID)
        End If

        Dim formuserprivilege = position_view_table.Select("ViewID = " & agencyview_ID)

        If formuserprivilege.Count = 0 Then

            tsbtnNewAgency.Visible = 0
            tsbtnSaveAgency.Visible = 0

            tsbtnNewAgency.Visible = 0
            tsbtnSaveAgency.Visible = 0

            dontUpdateAgency = 1

        Else

            For Each drow In formuserprivilege

                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    tsbtnNewAgency.Visible = 0
                    tsbtnSaveEmp.Visible = 0

                    tsbtnNewAgency.Visible = 0
                    tsbtnSaveAgency.Visible = 0

                    dontUpdateAgency = 1

                    Exit For

                Else

                    If drow("Creates").ToString = "N" Then
                        tsbtnNewAgency.Visible = 0

                        tsbtnNewAgency.Visible = 0

                    Else
                        tsbtnNewAgency.Visible = 1

                        tsbtnNewAgency.Visible = 1

                    End If

                    'If drow("Deleting").ToString = "N" Then
                    '    btnDelete.Visible = 0
                    'Else
                    '    btnDelete.Visible = 1
                    'End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdateAgency = 1

                    Else
                        dontUpdateAgency = 0

                    End If

                End If

            Next

        End If

        bgworksearchbox.RunWorkerAsync()

    End Sub



    Private Sub dgvemployee_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvemployee.CellBeginEdit

    End Sub

    Private Sub dgvemployee_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployee.CellContentClick

    End Sub

    Private Sub dgvemployee_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployee.CellEndEdit

    End Sub

    Private Sub dgvemployee_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvemployee.DataError
        'e.ThrowException = False

    End Sub

    Private Sub dgvemployee_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvemployee.KeyDown

        If e.KeyValue = 46 _
            And dgvemployee.CurrentRow.IsNewRow = False Then 'Delete Key

            'MsgBox("Delete Key")

            Dim prompt = MessageBox.Show("Discard his/her agnecy?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information)

            If prompt = Windows.Forms.DialogResult.Yes Then

                Dim n_ExecuteQuery As New ExecuteQuery("UPDATE employee SET AgencyID=NULL" &
                                                       ",LastUpd=CURRENT_TIMESTAMP()" &
                                                       ",LastUpdBy='" & z_User & "'" &
                                                       " WHERE RowID='" &
                                                       dgvemployee.CurrentRow.Cells("eRowID").Value & "';")

                Dim del_dgvrow = dgvemployee.CurrentRow

                dgvemployee.Rows.Remove(del_dgvrow)

            End If

        End If

    End Sub

    Private Sub dgvemployee_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dgvemployee.KeyPress

    End Sub

    Private Sub dgvemployee_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvemployee.Scroll

        myEllipseButton(dgvemployee, _
                        "EmployeeID", _
                        btnEmpID)

    End Sub

    Private Sub dgvemployee_SelectionChanged(sender As Object, e As EventArgs) Handles dgvemployee.SelectionChanged

        If dgvemployee.RowCount = 1 Then

        Else
            With dgvemployee.CurrentRow
                .Cells("EmployeeID").ReadOnly = True

                If .IsNewRow Then
                    .Cells("EmployeeID").ReadOnly = False
                Else
                    myEllipseButton(dgvemployee, "EmployeeID", btnEmpID)

                End If

            End With

        End If

        myEllipseButton(dgvemployee, _
                        "EmployeeID", _
                        btnEmpID)

    End Sub

    Private Sub btnEmpID_Click(sender As Object, e As EventArgs) Handles btnEmpID.Click

        Dim n_EmployeeSelection As New EmployeeSelection

        If n_EmployeeSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

            With n_EmployeeSelection
                'LastNameFirstNameMiddleNameDivisionID

                'Dim div_rowID = EXECQUER("")

                Dim div_and_pos_name As New DataTable

                Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT ps.PositionName AS psName" & _
                                               ",dv.Name AS dvName" & _
                                               " FROM position ps" & _
                                               " LEFT JOIN `division` dv ON dv.RowID='" & .EmpDivisionIDValue & "'" & _
                                               " WHERE ps.RowID='" & .EmpPositionIDValue & "';")

                div_and_pos_name = n_SQLQueryToDatatable.ResultTable

                Dim nameposition, namedivision As String

                nameposition = String.Empty
                namedivision = nameposition

                If div_and_pos_name.Rows.Count <> 0 Then

                    Try
                        nameposition = div_and_pos_name(0)("psName")
                    Catch ex As Exception
                        nameposition = String.Empty
                    End Try

                    Try
                        namedivision = div_and_pos_name(0)("dvName")
                    Catch ex As Exception
                        namedivision = String.Empty
                    End Try

                End If

                dgvemployee.Rows.Add(.ERowIDValue,
                                     .EmployeeIDValue,
                                     .EmpLastNameValue,
                                     .EmpFirstNameValue,
                                     .EmpMiddleNameValue,
                                     namedivision,
                                     nameposition)

            End With

        End If

    End Sub

    Private Sub tbpEmployee_Click(sender As Object, e As EventArgs) Handles tbpEmployee.Click

    End Sub

    Dim empview_ID = Nothing

    Private Sub TabPage2_Enter(sender As Object, e As EventArgs) Handles tbpEmployee.Enter

        tabctrlIndex = CustomColoredTabControl1.SelectedIndex

        RemoveHandler dgvagency.SelectionChanged, AddressOf dgvagency_SelectionChanged

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Dim groupnames As New DataTable
            
            'Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT '' AS RowID,'' AS Name" & _
            '                                                     " UNION" & _
            '                                                     " SELECT RowID,Name FROM `division` WHERE OrganizationID='" & orgztnID & "';")

            Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT RowID,Name FROM `division` WHERE OrganizationID='" & orgztnID & "';")

            groupnames = n_SQLQueryToDatatable.ResultTable

            'DivisionID.ValueMember = Nothing
            'DivisionID.DisplayMember = Nothing
            'DivisionID.DataSource = groupnames

            If groupnames.Rows.Count <> 0 Then

                DivisionID.ValueMember = groupnames.Columns(0).ColumnName
                DivisionID.DisplayMember = groupnames.Columns(1).ColumnName

                DivisionID.DataSource = groupnames

            End If


            Dim positionnames As New DataTable
            
            'n_SQLQueryToDatatable = New SQLQueryToDatatable("SELECT '' AS RowID,'' AS PositionName" & _
            '                                                " UNION" & _
            '                                                " SELECT RowID,PositionName FROM position WHERE OrganizationID='" & orgztnID & "' AND DivisionId IS NOT NULL;")

            n_SQLQueryToDatatable = New SQLQueryToDatatable("SELECT RowID,PositionName FROM position WHERE OrganizationID='" & orgztnID & "' AND DivisionId IS NOT NULL;")

            positionnames = n_SQLQueryToDatatable.ResultTable

            'PositionID.ValueMember = Nothing
            'PositionID.DisplayMember = Nothing
            'PositionID.DataSource = groupnames

            If groupnames.Rows.Count <> 0 Then

                PositionID.ValueMember = positionnames.Columns(0).ColumnName
                PositionID.DisplayMember = positionnames.Columns(1).ColumnName

                PositionID.DataSource = positionnames

            End If

            empview_ID = VIEW_privilege("Employee Personal Profile", orgztnID)

            Dim formuserprivilege = position_view_table.Select("ViewID = " & empview_ID)

            If formuserprivilege.Count = 0 Then

                tsbtnSaveEmp.Visible = False

            Else

                For Each drow In formuserprivilege

                    If drow("ReadOnly").ToString = "Y" Then
                        'ToolStripButton2.Visible = 0

                        tsbtnSaveEmp.Visible = False

                        Exit For

                    Else
                        'If drow("Creates").ToString = "N" Then
                        '    tsbtnNewEmp.Visible = 0
                        '    tsbtnNewDepen.Visible = 0
                        'Else
                        '    tsbtnNewEmp.Visible = 1
                        '    tsbtnNewDepen.Visible = 1
                        'End If

                        'If drow("Deleting").ToString = "N" Then
                        '    btnDelete.Visible = 0
                        'Else
                        '    btnDelete.Visible = 1
                        'End If

                        If drow("Updates").ToString = "N" Then
                            tsbtnSaveEmp.Visible = False

                        Else
                            tsbtnSaveEmp.Visible = True

                        End If

                    End If

                Next

            End If

        End If

        dgvagency_SelectionChanged(sender, e)

        AddHandler dgvagency.SelectionChanged, AddressOf dgvagency_SelectionChanged

    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click, _
                                                                                tsbtnClose.Click
        Me.Close()
    End Sub

    Private Sub tsbtnSaveEmp_Click(sender As Object, e As EventArgs) Handles tsbtnSaveEmp.Click

        Dim total_query As String = String.Empty

        Dim list_total_query As New List(Of String)

        For Each dgvrow As DataGridViewRow In dgvemployee.Rows

            If dgvrow.Cells("eRowID").Value = Nothing Then

                Continue For

            ElseIf dgvrow.IsNewRow Then

                Continue For

            Else

                list_total_query.Add("," & CStr(dgvrow.Cells("eRowID").Value))

            End If

        Next

        Dim str_query = "UPDATE employee SET AgencyID='" & agencyRowID & "' WHERE RowID='" & 1 & "';"

        'MsgBox(String.Concat(list_total_query))

        Dim n_ExecuteQuery As New ExecuteQuery("CALL MASSUPD_employee_agency('" & orgztnID &
                                               "','" & agencyRowID &
                                               "','" & z_User &
                                               "','" & String.Concat(list_total_query) & "');")

    End Sub

    Private Sub tsbtnCancelEmp_Click(sender As Object, e As EventArgs) Handles tsbtnCancelEmp.Click

    End Sub

    Private Sub dgvagency_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvagency.CellContentClick

    End Sub

    Dim agencyRowID = Nothing

    Private Sub dgvagency_SelectionChanged(sender As Object, e As EventArgs) ' Handles dgvagency.SelectionChanged

        If tsbtnNewAgency.Enabled = False Then
            tsbtnNewAgency.Enabled = True

        End If

        'Dim sender_type = sender.GetType.ToString

        agencyRowID = Nothing

        Select Case tabctrlIndex

            Case 0

                If dgvagency.RowCount = 0 Then

                    clearObjControl(Panel2)

                    address_RowID = Nothing

                Else

                    With dgvagency.CurrentRow

                        agencyRowID = .Cells("agRowID").Value

                        txtagencyname.Text = .Cells("agName").Value
                        txtagencyfee.Text = .Cells("agFee").Value
                        txtbxAddress.Text = .Cells("agAddress").Value

                        address_RowID = .Cells("agAddressID").Value

                        CheckBox1.Checked = Convert.ToInt16(.Cells("agIsActive").Value)

                    End With

                End If

            Case 1

                If dgvagency.RowCount = 0 Then

                    clearObjControl(Panel3)

                Else

                    With dgvagency.CurrentRow

                        'txtagencyname.Text = .Cells("agName").Value
                        'txtagencyfee.Text = .Cells("agFee").Value
                        'txtbxAddress.Text = .Cells("agAddress").Value

                        'address_RowID = .Cells("agAddressID").Value

                        Dim agency_row_id = .Cells("agRowID").Value

                        agencyRowID = agency_row_id

                        VIEW_employeewiththisagency(agency_row_id)

                    End With

                End If

        End Select

    End Sub

    Sub VIEW_employeewiththisagency(ByVal Agency_row_id As Object)


        'DivisionID.ValueMember = ""
        'DivisionID.DisplayMember = ""

        'DivisionID.DataSource = Nothing


        'PositionID.ValueMember = ""
        'PositionID.DisplayMember = ""

        'PositionID.DataSource = Nothing

        Dim n_ReadSQLProcedureToDatatable As New ReadSQLProcedureToDatatable("VIEW_employeewiththisagency",
                                                                             orgztnID,
                                                                             Agency_row_id)

        Dim employee_table = n_ReadSQLProcedureToDatatable.ResultTable

        dgvemployee.Rows.Clear()

        'DivisionID.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox

        For Each drow As DataRow In employee_table.Rows

            Dim rowarray = drow.ItemArray

            Dim newrowindex = _
            dgvemployee.Rows.Add(rowarray)

            'dgvemployee.Item("DivisionID", newrowindex).Value = drow("Name").ToString

            'dgvemployee.Item("PositionID", newrowindex).Value = drow("PositionName").ToString

        Next

        'DivisionID.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton

    End Sub

    Dim agencytable As New DataTable

    Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        RemoveHandler dgvagency.SelectionChanged, AddressOf dgvagency_SelectionChanged

        Dim indx = -1

        If dgvagency.RowCount <> 0 Then
            indx = dgvagency.CurrentRow.Index
        End If

        btnRefresh.Enabled = False

        Dim n_ReadSQLProcedureToDatatable As New ReadSQLProcedureToDatatable("VIEW_agency",
                                                                             orgztnID,
                                                                             autcomptxtagency.Text.Trim)

        agencytable = n_ReadSQLProcedureToDatatable.ResultTable

        dgvagency.Rows.Clear()

        For Each drow As DataRow In agencytable.Rows

            Dim rowarray = drow.ItemArray

            dgvagency.Rows.Add(rowarray)

        Next

        'agencytable.Dispose()

        btnRefresh.Enabled = True

        If indx <> -1 Then
            If dgvagency.RowCount > indx Then
                dgvagency.Item("agName", indx).Selected = True

            End If
            
        End If

        dgvagency_SelectionChanged(sender, e)

        AddHandler dgvagency.SelectionChanged, AddressOf dgvagency_SelectionChanged

    End Sub

    Private Sub bgworksearchbox_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworksearchbox.DoWork

        'autcomptxtagency
        Dim agencynames As New DataTable

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT AgencyName FROM agency WHERE OrganizationID='" & orgztnID & "';")

        agencynames = n_SQLQueryToDatatable.ResultTable

        For Each drow As DataRow In agencynames.Rows

            autcomptxtagency.Items.Add(New AutoCompleteEntry(CStr(drow("AgencyName")), StringToArray(CStr(drow("AgencyName")))))

        Next

        agencynames.Dispose()

    End Sub

    Private Sub bgworksearchbox_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworksearchbox.ProgressChanged

    End Sub

    Private Sub bgworksearchbox_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworksearchbox.RunWorkerCompleted

        autcomptxtagency.Enabled = True

    End Sub

    Private Sub autcomptxtagency_KeyPress(sender As Object, e As KeyPressEventArgs) Handles autcomptxtagency.KeyPress

        RemoveHandler dgvagency.SelectionChanged, AddressOf dgvagency_SelectionChanged

        Dim e_asc As String = Asc(e.KeyChar)

        If e_asc = 13 Then

            btnRefresh_Click(btnRefresh, e)

        End If

    End Sub

    Private Sub autcomptxtagency_TextChanged(sender As Object, e As EventArgs) Handles autcomptxtagency.TextChanged

    End Sub

    Private Sub txtagencyfee_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtagencyfee.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        Dim n_TrapDecimalKey As New TrapDecimalKey(e_asc, txtagencyfee.Text)

        e.Handled = n_TrapDecimalKey.ResultTrap

    End Sub

    Private Sub txtagencyfee_TextChanged(sender As Object, e As EventArgs) Handles txtagencyfee.TextChanged

    End Sub

    Private Sub CustomColoredTabControl1_Click(sender As Object, e As EventArgs) Handles CustomColoredTabControl1.Click

    End Sub

    Private Sub tsbtnNewAgency_Click(sender As Object, e As EventArgs) Handles tsbtnNewAgency.Click

        txtagencyname.Focus()

        clearObjControl(Panel2)

        tsbtnNewAgency.Enabled = False

    End Sub

    Dim address_RowID = Nothing

    Private Sub tsbtnSaveAgency_Click(sender As Object, e As EventArgs) Handles tsbtnSaveAgency.Click

        If tsbtnNewAgency.Enabled = False Then

            Dim ag_rowID = _
                INSUPD_agency(, txtagencyname.Text,
                    txtagencyfee.Text,
                    address_RowID)
            'txtTIN

            dgvagency.Rows.Add(ag_rowID,
                               txtagencyname.Text,
                               txtagencyfee.Text,
                               txtbxAddress.Text,
                               address_RowID)

            tsbtnNewAgency.Enabled = True

        Else
            With dgvagency.CurrentRow

                INSUPD_agency(.Cells("agRowID").Value,
                              txtagencyname.Text,
                              txtagencyfee.Text,
                              address_RowID)

                .Cells("agName").Value = txtagencyname.Text
                .Cells("agFee").Value = txtagencyfee.Text
                .Cells("agAddress").Value = txtbxAddress.Text
                .Cells("agAddressID").Value = address_RowID

            End With

        End If

    End Sub

    Function INSUPD_agency(Optional ag_RowID = Nothing,
                           Optional ag_AgencyName = Nothing,
                           Optional ag_AgencyFee = Nothing,
                           Optional ag_AddressID = Nothing) As Object

        Dim returnvalue = Nothing

        Dim params(6, 1)

        params(0, 0) = "ag_RowID"
        params(1, 0) = "ag_OrganizationID"
        params(2, 0) = "ag_UserRowID"
        params(3, 0) = "ag_AgencyName"
        params(4, 0) = "ag_AgencyFee"
        params(5, 0) = "ag_AddressID"
        params(6, 0) = "ag_IsActive"

        params(0, 1) = If(ag_RowID = Nothing, DBNull.Value, ag_RowID)
        params(1, 1) = orgztnID
        params(2, 1) = z_User
        params(3, 1) = ag_AgencyName
        params(4, 1) = ValNoComma(ag_AgencyFee)
        params(5, 1) = If(ag_AddressID = Nothing, DBNull.Value, ag_AddressID)
        params(6, 1) = CheckBox1.Checked

        returnvalue = _
            EXEC_INSUPD_PROCEDURE(params,
                                  "INSUPD_agency",
                                  "returnvalue")

        Return returnvalue

    End Function

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click

        btnRefresh_Click(btnRefresh, e)

    End Sub

    Private Sub btnSelectAddress_Click(sender As Object, e As EventArgs) Handles btnSelectAddress.Click

        Dim n_AddressClass As New AddressClass

        If n_AddressClass.ShowDialog("") = Windows.Forms.DialogResult.OK Then

            Dim full_address = String.Empty

            With n_AddressClass

                address_RowID = .AddresRowID
                
                If .StreetAddress1 = Nothing Then
                    full_address = Nothing
                Else
                    full_address = .StreetAddress1 & ","
                End If
                
                If .StreetAddress2 <> Nothing Then
                    full_address &= .StreetAddress2 & ","
                End If
                
                If .Barangay <> Nothing Then
                    full_address &= .Barangay & ","
                End If
                
                If .City <> Nothing Then
                    full_address &= .City & ","
                End If
                
                If .State <> Nothing Then
                    full_address &= "," & .State & ","
                End If
                
                If .Country <> Nothing Then
                    full_address &= .Country & ","
                End If

                If .ZipCode <> Nothing Then
                    full_address &= .ZipCode
                End If

            End With

            Dim addressstringlength = full_address.Length

            Dim LastCharIsComma = String.Empty

            Try
                LastCharIsComma = _
                full_address.Substring((addressstringlength - 1), 1)
            Catch ex As Exception
                LastCharIsComma = String.Empty
            End Try

            If LastCharIsComma.Trim = "," Then
                full_address = full_address.Substring(0, (addressstringlength - 1))

            End If

            full_address = full_address.Replace(",,", ",")

            txtbxAddress.Text = full_address

        End If

    End Sub

    Private Sub tbpAgency_Click(sender As Object, e As EventArgs) Handles tbpAgency.Click

    End Sub

    Private Sub tbpAgency_Enter(sender As Object, e As EventArgs) Handles tbpAgency.Enter

        tabctrlIndex = CustomColoredTabControl1.SelectedIndex

        RemoveHandler dgvagency.SelectionChanged, AddressOf dgvagency_SelectionChanged

        dgvagency_SelectionChanged(sender, e)

        AddHandler dgvagency.SelectionChanged, AddressOf dgvagency_SelectionChanged

    End Sub

    Dim tabctrlIndex As SByte = 0

    Private Sub CustomColoredTabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CustomColoredTabControl1.SelectedIndexChanged

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        CheckBox1.Tag = Convert.ToInt16(CheckBox1.Checked)
    End Sub

End Class