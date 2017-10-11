Imports Femiani.Forms.UI.Input

Class AreaForm

    Dim br_RowID As Object = Nothing

    Public Sub New(Optional Area_RowID As Object = Nothing)

        br_RowID = Area_RowID

        ' This call is required by the designer.

        InitializeComponent()

        Dim bool_result As Boolean = (ValNoComma(Area_RowID) > 0)

        SearchPanel.Visible = Not bool_result

        ' Add any initialization after the InitializeComponent() call.

        If z_User = 0 Then
            z_User = 2
        Else

        End If

        'AreaForm()
        txtSearchBox.Text = String.Empty

    End Sub

    'Public Shared Sub Main()

    'End Sub

    Sub AreaForm()

        txtSearchBox.Text = String.Empty

    End Sub

    Dim isShowAsDialog As Boolean = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult

        With Me

            isShowAsDialog = True

            .Text = someValue

        End With

        Return Me.ShowDialog

    End Function

    Protected Overrides Sub OnLoad(e As EventArgs)

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            ReLoad()

        End If

        MyBase.OnLoad(e)

    End Sub

    Private Sub ReLoad()

        Dim _search = Nothing

        If br_RowID = Nothing Then
            _search = txtSearchBox.Tag
        Else
            _search = br_RowID 'New ExecuteQuery("SELECT RowID FROM area WHERE OrganizationID='" & orgztnID & "' AND RowID='" & br_RowID & "';").Result
        End If

        Dim n_ReadSQLProcedureToDatatable As _
            New ReadSQLProcedureToDatatable("VIEW_area",
                                            orgztnID,
                                            pagination,
                                            If(_search = Nothing, DBNull.Value, _search))

        Dim catchdt As New DataTable

        catchdt = n_ReadSQLProcedureToDatatable.ResultTable

        dgvarea.Rows.Clear()

        For Each drow As DataRow In catchdt.Rows

            Dim row_array = drow.ItemArray

            dgvarea.Rows.Add(row_array)

        Next

        AddHandler_CurrentCellChanged(True)

    End Sub

    Private Sub AreaForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        bgworksearchautocompleter.RunWorkerAsync()

    End Sub

    Private Sub AreaForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

    Private Sub tsbtnNewArea_Click(sender As Object, e As EventArgs) Handles tsbtnNewArea.Click

        dgvarea.EndEdit(True)

        If dgvarea.RowCount > 0 Then

            dgvarea.Focus()

            Dim dgvrow_cnt = dgvarea.RowCount

            Dim mod_div_result = dgvrow_cnt Mod (limit_record + 1)

            If mod_div_result = 0 Then

                dgvarea.Rows.Clear()

                tsbtnNewArea_Click(tsbtnNewArea, New EventArgs)

                pagination = ValNoComma(pagination) + ValNoComma(limit_record)

                ''Dim fsdfs As New LinkLabel.Link

                ''fsdfs.Name = NextLink.Name

                ''Pagination_LinkClicked(NextLink, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link))

                Exit Sub

            End If

            dgvarea.Item("arName", (dgvrow_cnt - 1)).Selected = True

        End If

    End Sub

    Private Sub tsbtnSaveArea_Click(sender As Object, e As EventArgs) Handles tsbtnSaveArea.Click

        dgvarea.EndEdit(True)

        tsbtnSaveArea.Enabled = False

        Dim hasError As Boolean = False

        For Each dgvrow As DataGridViewRow In dgvarea.Rows

            If dgvrow.IsNewRow = False Then

                With dgvrow

                    If .Cells("arName").Value = Nothing Then
                        hasError = True
                        Exit For
                    ElseIf Trim(.Cells("arName").Value).Length = 0 Then
                        hasError = True
                        Exit For
                    End If

                End With

            Else
                Continue For
            End If

        Next

        If hasError = False Then

            For Each dgvrow As DataGridViewRow In dgvarea.Rows

                If dgvrow.IsNewRow = False Then

                    With dgvrow

                        If .Cells("arRowID").Value = Nothing Then

                            Dim n_ReadSQLFunction As _
                                New ReadSQLFunction("INSUPD_area",
                                                        "returnvalue",
                                                    DBNull.Value,
                                                    orgztnID,
                                                    .Cells("arName").Value,
                                                    z_User,
                                                    DBNull.Value)

                            .Cells("arRowID").Value = n_ReadSQLFunction.ReturnValue

                        Else

                            If ValNoComma(.Cells("ChangeIndicator").Value) > 0 Then

                                Dim n_ReadSQLFunction As _
                                    New ReadSQLFunction("INSUPD_area",
                                                            "returnvalue",
                                                        .Cells("arRowID").Value,
                                                        orgztnID,
                                                        .Cells("arName").Value,
                                                        z_User,
                                                        DBNull.Value)

                            End If

                        End If

                    End With

                Else

                    Continue For

                End If

            Next

        Else

        End If

        tsbtnSaveArea.Enabled = True

    End Sub

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click

        AddHandler_CurrentCellChanged(False)

        'OnLoad(New EventArgs)
        ReLoad()

    End Sub

    Private Sub tsbtnDelArea_Click(sender As Object, e As EventArgs) Handles tsbtnDelArea.Click

        dgvarea.EndEdit(True)

        dgvarea.Focus()

        Dim dgvcurr_row = dgvarea.CurrentRow

        If dgvcurr_row.IsNewRow = False Then

            tsbtnDelArea.Enabled = False

            Dim prompt = MessageBox.Show("Do you want to delete this area ?",
                                         "Area deletion",
                                         MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Information)

            If prompt = Windows.Forms.DialogResult.Yes Then

                Dim n_ExecuteQuery As _
                    New ExecuteQuery("UPDATE area SET LastUpdBy='" & z_User & "' WHERE RowID='" & dgvcurr_row.Cells("arRowID").Value & "';")
                'New ExecuteQuery("UPDATE area SET LastUpdBy='" & z_User & "' WHERE RowID='" & dgvarea.Tag & "';")
                dgvarea.Rows.Remove(dgvcurr_row)

            End If

            tsbtnDelArea.Enabled = True

        End If

    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click

        If tsbtnSaveArea.Enabled Then

            Me.Close()

        End If

    End Sub

    Dim data_priorvalue As Object = Nothing

    Private Sub dgvarea_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvarea.CellBeginEdit

        data_priorvalue = dgvarea.Item(e.ColumnIndex, e.RowIndex).Value

    End Sub

    Private Sub dgvarea_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvarea.CellContentClick

    End Sub

    Private Sub dgvarea_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvarea.CellEndEdit

        'ChangesIndicator

        If data_priorvalue = dgvarea.Item(e.ColumnIndex, e.RowIndex).Value Then

            dgvarea.Item("ChangeIndicator", e.RowIndex).Value = 1

        Else

        End If


    End Sub

    Dim limit_record As Object = 10

    Dim pagination = Val(0)

    Private Sub Pagination_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles FirstLink.LinkClicked, PrevLink.LinkClicked,
                                                                                                        NextLink.LinkClicked, LastLink.LinkClicked

        dgvarea.EndEdit(True)

        Panel2.Enabled = False

        Dim sender_obj = DirectCast(sender, LinkLabel).Name

        If sender_obj = "FirstLink" Then

            pagination = 0

        ElseIf sender_obj = "PrevLink" Then

            Dim result = pagination - limit_record

            If ValNoComma(result) < 0 Then
                pagination = 0
            Else
                pagination = result
            End If

        ElseIf sender_obj = "NextLink" Then

            pagination = ValNoComma(pagination) + ValNoComma(limit_record)

        ElseIf sender_obj = "LastLink" Then

            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / " & limit_record & " FROM area WHERE OrganizationID=" & orgztnID & ";"))

            Dim remender = lastpage Mod 1

            pagination = (lastpage - remender) * ValNoComma(limit_record)

        End If

        'OnLoad(New EventArgs)
        ReLoad()

        Panel2.Enabled = True

    End Sub

    Private Sub txtSearchBox_EnabledChanged(sender As Object, e As EventArgs) Handles txtSearchBox.EnabledChanged
        btnRefresh.Enabled = txtSearchBox.Enabled
    End Sub

    Private Sub txtSearchBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearchBox.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 _
            And btnRefresh.Enabled Then

            btnRefresh_Click(btnRefresh, New EventArgs)

        Else

            txtSearchBox.Tag = Nothing

        End If

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        btnRefresh.Enabled = False

        AddHandler_CurrentCellChanged(False)

        pagination = 0

        txtSearchBox.Tag = New ExecuteQuery("SELECT GROUP_CONCAT(Name)" &
                                            " FROM `area`" &
                                            " WHERE OrganizationID='" & orgztnID & "'" &
                                            " AND Name='" & txtSearchBox.Text.Trim & "';").Result

        ReLoad()

        btnRefresh.Enabled = True

    End Sub

    Private Sub dgvarea_CurrentCellChanged(sender As Object, e As EventArgs) 'Handles dgvarea.CurrentCellChanged

        'MsgBox("dgvarea_CurrentCellChanged")
        Static curr_rowindx As Integer = -1

        If dgvarea.RowCount > 0 Then

            If dgvarea.CurrentRow Is Nothing Then
                curr_rowindx = -1
            Else

                With dgvarea.CurrentRow

                    Dim currrow_indx = .Index

                    If curr_rowindx <> currrow_indx Then

                        curr_rowindx = currrow_indx

                        dgvarea.Tag = .Cells("arRowID").Value

                    Else

                    End If

                End With

            End If

        Else
            dgvarea.Tag = Nothing
        End If

        Label1.Text = ValNoComma(dgvarea.Tag)

    End Sub

    Private Sub dgvarea_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvarea.RowsRemoved

        'If dgvarea.SortedColumn IsNot Nothing Then
        '    dgvarea.Sort(dgvarea.SortedColumn, SortOrder.None)
        'End If

        AddHandler_CurrentCellChanged(False)

    End Sub

    Private Sub dgvarea_Sorted(sender As Object, e As EventArgs) Handles dgvarea.Sorted

        ''As Boolean _
        'Dim bool_result _
        '    = _
        '    Convert.ToInt16(dgvarea.SortOrder)

        'Label1.Text = CStr(bool_result)

    End Sub

    Private Sub AddHandler_CurrentCellChanged(ByVal bool_result As Boolean)

        RemoveHandler dgvarea.SelectionChanged, AddressOf dgvarea_CurrentCellChanged

        If bool_result Then

            'AddHandler dgvarea.CurrentCellChanged, AddressOf dgvarea_CurrentCellChanged
            AddHandler dgvarea.SelectionChanged, AddressOf dgvarea_CurrentCellChanged

            dgvarea_CurrentCellChanged(dgvarea, New EventArgs)

        Else

            'RemoveHandler dgvarea.CurrentCellChanged, AddressOf dgvarea_CurrentCellChanged
            RemoveHandler dgvarea.SelectionChanged, AddressOf dgvarea_CurrentCellChanged

        End If

    End Sub

    Private Sub Panel2_EnabledChanged(sender As Object, e As EventArgs) Handles Panel2.EnabledChanged

        If Panel2.Enabled Then
        Else
            AddHandler_CurrentCellChanged(Panel2.Enabled)
        End If

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub bgworksearchautocompleter_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworksearchautocompleter.DoWork

        Dim dtempfullname As New DataTable

        dtempfullname = New SQLQueryToDatatable("SELECT RowID,Name AS SearchValue FROM area WHERE OrganizationID='" & orgztnID & "';").ResultTable

        If dtempfullname IsNot Nothing Then

            For Each drow As DataRow In dtempfullname.Rows

                If CStr(drow("SearchValue")) <> Nothing Then

                    txtSearchBox.Items.Add(New AutoCompleteEntry(CStr(drow("SearchValue")),
                                                                 StringToArray(CStr(drow("SearchValue")))))

                End If

            Next

        End If

    End Sub

    Private Sub bgworksearchautocompleter_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworksearchautocompleter.ProgressChanged

    End Sub

    Private Sub bgworksearchautocompleter_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworksearchautocompleter.RunWorkerCompleted

        If e.Error IsNot Nothing Then

        ElseIf e.Cancelled Then

        Else

            txtSearchBox.Enabled = True

        End If

    End Sub

    Private Sub txtSearchBox_TextChanged(sender As Object, e As EventArgs) Handles txtSearchBox.TextChanged

    End Sub

    Private Sub dgvEmp_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmp.CellContentClick

    End Sub

    Private Sub dgvEmp_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvEmp.CellMouseDown

        If e.Button = Windows.Forms.MouseButtons.Right Then

            If e.ColumnIndex > -1 And e.RowIndex > -1 Then

                dgvEmp.Focus()

                dgvEmp.Item(e.ColumnIndex, e.RowIndex).Selected = True

                dgvEmp_CellContentClick(sender, New DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex))

                RemoveToolStripMenuItem.Visible = True

                ContextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Default)

                'Else

                '    RemoveToolStripMenuItem.Visible = False

            End If

            'ContextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Default)

        Else

        End If

    End Sub

    Private Sub dgvEmp_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvEmp.MouseDown

        If e.Button = Windows.Forms.MouseButtons.Right Then

            RemoveToolStripMenuItem.Visible = False

            ContextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Default)

        End If

    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

        e.Cancel = (dgvarea.CurrentRow.IsNewRow)

    End Sub

    Private Sub AddToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click

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

                dgvEmp.Rows.Add(.ERowIDValue,
                                .EmployeeIDValue,
                                .EmpFirstNameValue,
                                .EmpLastNameValue,
                                nameposition)

            End With

        End If

    End Sub

    Private Sub RemoveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveToolStripMenuItem.Click

    End Sub

End Class