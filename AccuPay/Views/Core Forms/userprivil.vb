Public Class userprivil

    Dim view_id As Integer = Nothing

    Private Sub userprivil_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        view_id = VIEW_privilege("User Privilege", orgztnID)

        VIEW_position_organization_user()

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_id)

        If formuserprivilege.Count = 0 Then
            tsbtnNewUserPrivil.Visible = 0
            tsbtnSaveUserPrivil.Visible = 0
            dontUpdate = 1
            dontCreate = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    ''ToolStripButton2.Visible = 0

                    tsbtnNewUserPrivil.Visible = 0
                    tsbtnSaveUserPrivil.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        ''ToolStripButton2.Visible = 0

                        tsbtnNewUserPrivil.Visible = 0
                        dontCreate = 1
                    Else
                        tsbtnNewUserPrivil.Visible = 1
                        dontCreate = 0
                        ''ToolStripButton2.Visible = 1
                    End If

                    'If drow("Deleting").ToString = "N" Then
                    '    ToolStripButton3.Visible = 0
                    'Else
                    '    ToolStripButton3.Visible = 1
                    'End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

        dgvposit_SelectionChanged(sender, e)

        AddHandler dgvposit.SelectionChanged, AddressOf dgvposit_SelectionChanged

    End Sub

    Dim pagination As Integer = 0

    Sub VIEW_position_organization_user()

        Dim params(2, 2) As Object

        params(0, 0) = "pos_OrganizationID"
        params(1, 0) = "pagination"
        params(2, 0) = "current_userID"

        params(0, 1) = orgztnID
        params(1, 1) = pagination
        params(2, 1) = z_User

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_position_organization_user",
                             dgvposit)

    End Sub

    Sub VIEW_position_view(ByVal PositionID As Object)

        Dim params(1, 2) As Object

        params(0, 0) = "pv_OrganizationID"
        params(1, 0) = "pv_PositionID"

        params(0, 1) = orgztnID
        params(1, 1) = PositionID

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_position_view",
                             dgvpositview)

    End Sub

    Sub VIEW_view_of_organization()

        Dim params(1, 2) As Object

        params(0, 0) = "vw_OrganizationID"

        params(0, 1) = orgztnID

        EXEC_VIEW_PROCEDURE(params,
                             "VIEW_view_of_organization",
                             dgvpositview)

    End Sub

    Private Sub dgvposit_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvposit.CellContentClick

    End Sub

    Private Sub dgvposit_SelectionChanged(sender As Object, e As EventArgs) ' Handles dgvposit.SelectionChanged

        If tsbtnNewUserPrivil.Enabled = False Then
            tsbtnNewUserPrivil.Enabled = True
        End If

        RemoveHandler CheckBox1.CheckedChanged, AddressOf CheckBox1_CheckedChanged
        RemoveHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
        RemoveHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
        RemoveHandler CheckBox4.CheckedChanged, AddressOf CheckBox4_CheckedChanged

        listofeditrow.Clear()

        If dgvposit.RowCount <> 0 Then
            With dgvposit.CurrentRow
                VIEW_position_view(.Cells("Column1").Value)

                If dgvpositview.RowCount = 1 Then
                    VIEW_view_of_organization()
                End If

            End With

            CheckBox1.Checked = False
            CheckBox2.Checked = False
            CheckBox3.Checked = False
            CheckBox4.Checked = False

            Dim createcount = 0
            Dim updatecount = 0
            Dim deletecount = 0
            Dim read_only = 0

            For Each dgvrow As DataGridViewRow In dgvpositview.Rows

                If dgvrow.Cells("Column11").Value = 1 Then
                    createcount += 1
                End If

                If dgvrow.Cells("Column12").Value = 1 Then
                    updatecount += 1
                End If

                If dgvrow.Cells("Column13").Value = 1 Then
                    deletecount += 1
                End If

                If dgvrow.Cells("Column14").Value = 1 Then
                    read_only += 1
                End If

            Next

            If createcount = 0 Then
                CheckBox1.Checked = False
            Else
                CheckBox1.Checked = If(dgvpositview.RowCount - 1 <> createcount, False, True)
            End If

            If updatecount = 0 Then
                CheckBox2.Checked = False
            Else
                CheckBox2.Checked = If(dgvpositview.RowCount - 1 <> updatecount, False, True)
            End If

            If deletecount = 0 Then
                CheckBox3.Checked = False
            Else
                CheckBox3.Checked = If(dgvpositview.RowCount - 1 <> deletecount, False, True)
            End If

            If read_only = 0 Then
                CheckBox4.Checked = False
            Else

                CheckBox4.Checked = If(dgvpositview.RowCount - 1 <> read_only, False, True)
            End If
        Else

            CheckBox1.Checked = False
            CheckBox1_CheckedChanged(sender, e)

            CheckBox2.Checked = False
            CheckBox2_CheckedChanged(sender, e)

            CheckBox3.Checked = False
            CheckBox3_CheckedChanged(sender, e)

            CheckBox4.Checked = False
            CheckBox4_CheckedChanged(sender, e)

        End If

        AddHandler CheckBox1.CheckedChanged, AddressOf CheckBox1_CheckedChanged
        AddHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
        AddHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
        AddHandler CheckBox4.CheckedChanged, AddressOf CheckBox4_CheckedChanged

    End Sub

    Private Sub First_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles First.LinkClicked, Prev.LinkClicked,
                                                                                                Nxt.LinkClicked, Last.LinkClicked

        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        RemoveHandler dgvposit.SelectionChanged, AddressOf dgvposit_SelectionChanged

        If sendrname = "First" Then
            pagination = 0
        ElseIf sendrname = "Prev" Then
            'If pagination - 100 < 0 Then
            '    pagination = 0
            'Else
            '    pagination -= 100
            'End If

            Dim modcent = pagination Mod 100

            If modcent = 0 Then

                pagination -= 100
            Else

                pagination -= modcent

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sendrname = "Nxt" Then

            Dim modcent = pagination Mod 100

            If modcent = 0 Then
                pagination += 100
            Else
                pagination -= modcent

                pagination += 100

            End If
        ElseIf sendrname = "Last" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 100 FROM position WHERE OrganizationID=" & orgztnID & ";"))

            Dim remender = lastpage Mod 1

            pagination = (lastpage - remender) * 100

            If pagination - 100 < 100 Then
                'pagination = 0

            End If

            'pagination = If(lastpage - 100 >= 100, _
            '                lastpage - 100, _
            '                lastpage)

        End If

        VIEW_position_organization_user()

        dgvposit_SelectionChanged(sender, e)

        AddHandler dgvposit.SelectionChanged, AddressOf dgvposit_SelectionChanged

    End Sub

    Private Sub tsbtnNewUserPrivil_Click(sender As Object, e As EventArgs) Handles tsbtnNewUserPrivil.Click

        'tsbtnNewUserPrivil.Enabled = False

        'RemoveHandler CheckBox1.CheckedChanged, AddressOf CheckBox1_CheckedChanged
        'RemoveHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
        'RemoveHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
        'RemoveHandler CheckBox4.CheckedChanged, AddressOf CheckBox4_CheckedChanged

        'CheckBox1.Checked = False

        'CheckBox2.Checked = False

        'CheckBox3.Checked = False

        'CheckBox4.Checked = False

        'VIEW_view_of_organization()

        'AddHandler CheckBox1.CheckedChanged, AddressOf CheckBox1_CheckedChanged
        'AddHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
        'AddHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
        'AddHandler CheckBox4.CheckedChanged, AddressOf CheckBox4_CheckedChanged

    End Sub

    Dim dontUpdate As SByte = 0

    Dim dontCreate As SByte = 0

    Private Sub tsbtnSaveUserPrivil_Click(sender As Object, e As EventArgs) Handles tsbtnSaveUserPrivil.Click

        dgvpositview.EndEdit(True)

        lblforballoon.Focus()

        If dgvposit.RowCount = 0 Then
            Exit Sub
        End If

        RemoveHandler dgvposit.SelectionChanged, AddressOf dgvposit_SelectionChanged

        For Each dgvrow As DataGridViewRow In dgvpositview.Rows
            With dgvrow
                If .IsNewRow Then
                Else

                    Dim posview_create = If(.Cells("Column11").Value = 1 Or .Cells("Column11").Value = True, "Y", "N")
                    Dim posview_update = If(.Cells("Column12").Value = 1 Or .Cells("Column12").Value = True, "Y", "N")
                    Dim posview_delete = If(.Cells("Column13").Value = 1 Or .Cells("Column13").Value = True, "Y", "N")
                    Dim posview_readonly = If(.Cells("Column14").Value = 1 Or .Cells("Column14").Value = True, "Y", "N")

                    If .Cells("Column9").Value = Nothing Then

                        If .Cells("view_RowID").Value = Nothing Then
                        Else
                            .Cells("Column9").Value =
                                                INSUPD_position_view(,
                                                                    dgvposit.CurrentRow.Cells("Column1").Value,
                                                                    .Cells("view_RowID").Value,
                                                                    posview_create,
                                                                    posview_readonly,
                                                                    posview_update,
                                                                    posview_delete)

                        End If
                    Else

                        If listofeditrow.Contains(.Cells("Column9").Value) Then
                            INSUPD_position_view(.Cells("Column9").Value,
                                                 dgvposit.CurrentRow.Cells("Column1").Value,
                                                  .Cells("view_RowID").Value,
                                                  posview_create,
                                                  posview_readonly,
                                                  posview_update,
                                                  posview_delete)
                        End If

                    End If

                End If

            End With
            'INSUPD_position_view
        Next

        position_view_table = retAsDatTbl("SELECT *" &
                                          " FROM position_view" &
                                          " WHERE PositionID=(SELECT PositionID FROM user WHERE RowID=" & z_User & ")" &
                                          " AND OrganizationID='" & orgztnID & "';")

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_id)

        If formuserprivilege.Count = 0 Then
            tsbtnNewUserPrivil.Visible = 0
            tsbtnSaveUserPrivil.Visible = 0
            dontUpdate = 1
            dontCreate = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    tsbtnSaveUserPrivil.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        'ToolStripButton2.Visible = 0
                        dontCreate = 1
                    Else
                        dontCreate = 0
                        'ToolStripButton2.Visible = 1
                    End If

                    'If drow("Deleting").ToString = "N" Then
                    '    ToolStripButton3.Visible = 0
                    'Else
                    '    ToolStripButton3.Visible = 1
                    'End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

        Dim row_indx As Integer = dgvpositview.CurrentRow.Index
        Dim col_indx As String = dgvpositview.Columns(dgvposit.CurrentCell.ColumnIndex).Name

        listofeditrow.Clear()

        InfoBalloon("User privilege has been successfully saved.", "Successfully save", lblforballoon, 0, -69)

        dgvposit_SelectionChanged(sender, e)

        If dgvpositview.RowCount - 1 > row_indx Then
            dgvpositview.Item(col_indx, row_indx).Selected = True
        End If

        AddHandler dgvposit.SelectionChanged, AddressOf dgvposit_SelectionChanged

    End Sub

    Function INSUPD_position_view(Optional pv_RowID As Object = Nothing,
                                  Optional pv_PositionID As Object = Nothing,
                                  Optional pv_ViewID As Object = Nothing,
                                  Optional pv_Creates As Object = Nothing,
                                  Optional pv_ReadOnly As Object = Nothing,
                                  Optional pv_Updates As Object = Nothing,
                                  Optional pv_Deleting As Object = Nothing) As Object

        Dim params(9, 2) As Object

        params(0, 0) = "pv_RowID"
        params(1, 0) = "pv_PositionID"
        params(2, 0) = "pv_OrganizationID"
        params(3, 0) = "pv_CreatedBy"
        params(4, 0) = "pv_LastUpdBy"
        params(5, 0) = "pv_ViewID"
        params(6, 0) = "pv_Creates"
        params(7, 0) = "pv_ReadOnly"
        params(8, 0) = "pv_Updates"
        params(9, 0) = "pv_Deleting"

        params(0, 1) = If(pv_RowID = Nothing, DBNull.Value, pv_RowID)
        params(1, 1) = pv_PositionID
        params(2, 1) = orgztnID
        params(3, 1) = z_User
        params(4, 1) = z_User
        params(5, 1) = pv_ViewID
        params(6, 1) = pv_Creates
        params(7, 1) = pv_ReadOnly
        params(8, 1) = pv_Updates
        params(9, 1) = pv_Deleting

        INSUPD_position_view =
            EXEC_INSUPD_PROCEDURE(params,
                                "INSUPD_position_view",
                                "pvRowID")

    End Function

    Private Sub tsbtnCancelUserPrivil_Click(sender As Object, e As EventArgs) Handles tsbtnCancelUserPrivil.Click
        dgvposit_SelectionChanged(sender, e)

    End Sub

    Private Sub tsbtnCloseUserPrivil_Click(sender As Object, e As EventArgs) Handles tsbtnCloseUserPrivil.Click
        Me.Close()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) 'Handles CheckBox1.CheckedChanged

        Dim boolval = CheckBox1.Checked

        'MsgBox(boolval.ToString)

        For Each dgvrow As DataGridViewRow In dgvpositview.Rows
            With dgvrow
                If .IsNewRow Then
                Else
                    .Cells("Column11").Value = boolval
                    If .Cells("Column9").Value = Nothing Then
                    Else
                        listofeditrow.Add(Trim(.Cells("Column9").Value))
                    End If
                End If
            End With
        Next

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) 'Handles CheckBox2.CheckedChanged

        Dim boolval = CheckBox2.Checked

        'MsgBox(boolval.ToString)

        For Each dgvrow As DataGridViewRow In dgvpositview.Rows
            With dgvrow
                If .IsNewRow Then
                Else
                    .Cells("Column12").Value = boolval
                    If .Cells("Column9").Value = Nothing Then
                    Else
                        listofeditrow.Add(Trim(.Cells("Column9").Value))
                    End If
                End If
            End With
        Next

    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) 'Handles CheckBox3.CheckedChanged

        Dim boolval = CheckBox3.Checked

        'MsgBox(boolval.ToString)

        For Each dgvrow As DataGridViewRow In dgvpositview.Rows
            With dgvrow
                If .IsNewRow Then
                Else
                    .Cells("Column13").Value = boolval
                    If .Cells("Column9").Value = Nothing Then
                    Else
                        listofeditrow.Add(Trim(.Cells("Column9").Value))
                    End If
                End If
            End With
        Next

    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) 'Handles CheckBox4.CheckedChanged

        Dim boolval = CheckBox4.Checked

        'MsgBox(boolval.ToString)

        For Each dgvrow As DataGridViewRow In dgvpositview.Rows
            With dgvrow
                If .IsNewRow Then
                Else
                    .Cells("Column14").Value = boolval
                    If .Cells("Column9").Value = Nothing Then
                    Else
                        listofeditrow.Add(Trim(.Cells("Column9").Value))
                    End If
                End If
            End With
        Next

    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem
        TabControlColor(TabControl1, e)
    End Sub

    Private Sub dgvpositview_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpositview.CellContentClick

    End Sub

    Dim listofeditrow As New AutoCompleteStringCollection

    Private Sub dgvpositview_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpositview.CellEndEdit
        If dgvpositview.Item("Column9", e.RowIndex).Value = Nothing Then
        Else '17352

            Dim theRowID = dgvpositview.Item("Column9", e.RowIndex).Value

            listofeditrow.Add(Trim(dgvpositview.Item("Column9", e.RowIndex).Value))
        End If

    End Sub

    Private Sub userprivil_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        InfoBalloon(, , lblforballoon, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        GeneralForm.listGeneralForm.Remove(Me.Name)
    End Sub

    Private Sub dgvpositview_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvpositview.DataError
        e.ThrowException = False

    End Sub

End Class