Imports Femiani.Forms.UI.Input

Class BranchForm

    Dim br_RowID As Object = Nothing

    Public Sub New(Optional Branch_RowID As Object = Nothing)

        br_RowID = Branch_RowID

        ' This call is required by the designer.

        InitializeComponent()

        Dim bool_result As Boolean = (ValNoComma(Branch_RowID) > 0)

        SearchPanel.Visible = Not bool_result

        ' Add any initialization after the InitializeComponent() call.

        If z_User = 0 Then
            z_User = 2
        Else

        End If

        'BranchForm()
        txtSearchBox.Text = String.Empty

    End Sub

    'Public Shared Sub Main()

    'End Sub

    Sub BranchForm()

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
            _search = New ExecuteQuery("SELECT BranchCode FROM branch WHERE OrganizationID='" & orgztnID & "' AND RowID='" & br_RowID & "';").Result
        End If

        Dim n_ReadSQLProcedureToDatatable As _
            New ReadSQLProcedureToDatatable("VIEW_branch",
                                            orgztnID,
                                            pagination,
                                            If(_search = Nothing, DBNull.Value, _search))

        Dim catchdt As New DataTable

        catchdt = n_ReadSQLProcedureToDatatable.ResultTable

        dgvbranch.Rows.Clear()

        For Each drow As DataRow In catchdt.Rows

            Dim row_array = drow.ItemArray

            dgvbranch.Rows.Add(row_array)

        Next

        AddHandler_CurrentCellChanged(True)

    End Sub

    Private Sub BranchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        bgworksearchautocompleter.RunWorkerAsync()

    End Sub

    Private Sub BranchForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

    Private Sub tsbtnNewBranch_Click(sender As Object, e As EventArgs) Handles tsbtnNewBranch.Click

        dgvbranch.EndEdit(True)

        If dgvbranch.RowCount > 0 Then

            dgvbranch.Focus()

            Dim dgvrow_cnt = dgvbranch.RowCount

            Dim mod_div_result = dgvrow_cnt Mod (limit_record + 1)

            If mod_div_result = 0 Then

                dgvbranch.Rows.Clear()

                tsbtnNewBranch_Click(tsbtnNewBranch, New EventArgs)

                pagination = ValNoComma(pagination) + ValNoComma(limit_record)

                ''Dim fsdfs As New LinkLabel.Link

                ''fsdfs.Name = NextLink.Name

                ''Pagination_LinkClicked(NextLink, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link))

                Exit Sub

            End If

            dgvbranch.Item("brCode", (dgvrow_cnt - 1)).Selected = True

        End If

    End Sub

    Private Sub tsbtnSaveBranch_Click(sender As Object, e As EventArgs) Handles tsbtnSaveBranch.Click

        dgvbranch.EndEdit(True)

        tsbtnSaveBranch.Enabled = False

        Dim hasError As Boolean = False

        For Each dgvrow As DataGridViewRow In dgvbranch.Rows

            If dgvrow.IsNewRow = False Then

                With dgvrow

                    If .Cells("brCode").Value = Nothing Then
                        hasError = True
                        Exit For
                    ElseIf Trim(.Cells("brCode").Value).Length = 0 Then
                        hasError = True
                        Exit For
                    End If

                End With

            Else
                Continue For
            End If

        Next

        If hasError = False Then

            For Each dgvrow As DataGridViewRow In dgvbranch.Rows

                If dgvrow.IsNewRow = False Then

                    With dgvrow

                        If .Cells("brRowID").Value = Nothing Then

                            Dim n_ReadSQLFunction As _
                                New ReadSQLFunction("INSUPD_branch",
                                                    "returnvalue",
                                                    DBNull.Value,
                                                    orgztnID,
                                                    z_User,
                                                    .Cells("brCode").Value,
                                                    .Cells("brName").Value)

                            .Cells("brRowID").Value = n_ReadSQLFunction.ReturnValue

                        Else

                            If ValNoComma(.Cells("ChangeIndicator").Value) > 0 Then

                                Dim n_ReadSQLFunction As _
                                    New ReadSQLFunction("INSUPD_branch",
                                                        "returnvalue",
                                                        .Cells("brRowID").Value,
                                                        orgztnID,
                                                        z_User,
                                                        .Cells("brCode").Value,
                                                        .Cells("brName").Value)

                            End If

                        End If

                    End With

                Else

                    Continue For

                End If

            Next

        Else

        End If

        tsbtnSaveBranch.Enabled = True

    End Sub

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click

        AddHandler_CurrentCellChanged(False)

        'OnLoad(New EventArgs)
        ReLoad()

    End Sub

    Private Sub tsbtnDelBranch_Click(sender As Object, e As EventArgs) Handles tsbtnDelBranch.Click

        dgvbranch.EndEdit(True)

        dgvbranch.Focus()

        Dim dgvcurr_row = dgvbranch.CurrentRow

        If dgvcurr_row.IsNewRow = False Then

            tsbtnDelBranch.Enabled = False

            Dim prompt = MessageBox.Show("Do you want to delete this branch ?",
                                         "Branch deletion",
                                         MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Information)

            If prompt = Windows.Forms.DialogResult.Yes Then

                Dim n_ExecuteQuery As _
                    New ExecuteQuery("UPDATE branch SET LastUpdBy='" & z_User & "' WHERE RowID='" & dgvcurr_row.Cells("brRowID").Value & "';")
                'New ExecuteQuery("UPDATE branch SET LastUpdBy='" & z_User & "' WHERE RowID='" & dgvbranch.Tag & "';")
                dgvbranch.Rows.Remove(dgvcurr_row)

            End If

            tsbtnDelBranch.Enabled = True

        End If

    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click

        If tsbtnSaveBranch.Enabled Then

            Me.Close()

        End If

    End Sub

    Dim data_priorvalue As Object = Nothing

    Private Sub dgvbranch_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvbranch.CellBeginEdit

        data_priorvalue = dgvbranch.Item(e.ColumnIndex, e.RowIndex).Value

    End Sub

    Private Sub dgvbranch_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvbranch.CellContentClick

    End Sub

    Private Sub dgvbranch_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvbranch.CellEndEdit

        'ChangesIndicator

        If data_priorvalue = dgvbranch.Item(e.ColumnIndex, e.RowIndex).Value Then

            dgvbranch.Item("ChangeIndicator", e.RowIndex).Value = 1

        Else

        End If


    End Sub

    Dim limit_record As Object = 10

    Dim pagination = Val(0)

    Private Sub Pagination_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles FirstLink.LinkClicked, PrevLink.LinkClicked,
                                                                                                        NextLink.LinkClicked, LastLink.LinkClicked

        dgvbranch.EndEdit(True)

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

            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / " & limit_record & " FROM branch WHERE OrganizationID=" & orgztnID & ";"))

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

        txtSearchBox.Tag = New ExecuteQuery("SELECT GROUP_CONCAT(BranchCode)" &
                                            " FROM branch" &
                                            " WHERE OrganizationID='" & orgztnID & "'" &
                                            " AND (BranchCode='" & txtSearchBox.Text.Trim & "' OR BranchName='" & txtSearchBox.Text.Trim & "');").Result

        ReLoad()

        btnRefresh.Enabled = True

    End Sub

    Private Sub dgvbranch_CurrentCellChanged(sender As Object, e As EventArgs) 'Handles dgvbranch.CurrentCellChanged

        'MsgBox("dgvbranch_CurrentCellChanged")
        Static curr_rowindx As Integer = -1

        If dgvbranch.RowCount > 0 Then

            If dgvbranch.CurrentRow Is Nothing Then
                curr_rowindx = -1
            Else

                With dgvbranch.CurrentRow

                    Dim currrow_indx = .Index

                    If curr_rowindx <> currrow_indx Then

                        curr_rowindx = currrow_indx

                        dgvbranch.Tag = .Cells("brRowID").Value

                    Else

                    End If

                End With

            End If

        Else
            dgvbranch.Tag = Nothing
        End If

        Label1.Text = ValNoComma(dgvbranch.Tag)

    End Sub

    Private Sub dgvbranch_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvbranch.RowsRemoved

        'If dgvbranch.SortedColumn IsNot Nothing Then
        '    dgvbranch.Sort(dgvbranch.SortedColumn, SortOrder.None)
        'End If

        AddHandler_CurrentCellChanged(False)

    End Sub

    Private Sub dgvbranch_Sorted(sender As Object, e As EventArgs) Handles dgvbranch.Sorted

        ''As Boolean _
        'Dim bool_result _
        '    = _
        '    Convert.ToInt16(dgvbranch.SortOrder)

        'Label1.Text = CStr(bool_result)

    End Sub

    Private Sub AddHandler_CurrentCellChanged(ByVal bool_result As Boolean)

        RemoveHandler dgvbranch.SelectionChanged, AddressOf dgvbranch_CurrentCellChanged

        If bool_result Then

            'AddHandler dgvbranch.CurrentCellChanged, AddressOf dgvbranch_CurrentCellChanged
            AddHandler dgvbranch.SelectionChanged, AddressOf dgvbranch_CurrentCellChanged

            dgvbranch_CurrentCellChanged(dgvbranch, New EventArgs)

        Else

            'RemoveHandler dgvbranch.CurrentCellChanged, AddressOf dgvbranch_CurrentCellChanged
            RemoveHandler dgvbranch.SelectionChanged, AddressOf dgvbranch_CurrentCellChanged

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

        dtempfullname = New SQLQueryToDatatable("SELECT RowID,BranchName AS SearchValue FROM branch WHERE OrganizationID='" & orgztnID & "'" &
                                                " UNION ALL" &
                                                " SELECT RowID,BranchCode AS SearchValue FROM branch WHERE OrganizationID='" & orgztnID & "';").ResultTable

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

End Class