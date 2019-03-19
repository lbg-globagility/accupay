Imports Femiani.Forms.UI.Input

Public Class ListOfValueForm
    Dim isNew As Integer = 0
    Dim rowid As Integer = 0

    Protected Overrides Sub OnLoad(e As EventArgs)

        Button4_Click(Button4, New EventArgs)

        dgvlistofvaltype_SelectionChanged(dgvlistofvaltype, New EventArgs)

        MyBase.OnLoad(e)

    End Sub

    Private Sub filllistofvalues()
        Try
            Dim dt As New DataTable

            dt = getDataTableForSQL("Select * From listofval WHERE DisplayValue!='' AND `Type`='" & strcurrent_type & "' ORDER BY orderby ASC;")

            dglistofval.Rows.Clear()

            If dt.Rows.Count > 0 Then
                For Each rowd As DataRow In dt.Rows
                    Dim n As Integer = dglistofval.Rows.Add()
                    With rowd

                        dglistofval.Rows.Item(n).Cells(0).Value = .Item("RowID").ToString
                        dglistofval.Rows.Item(n).Cells(1).Value = .Item("DisplayValue").ToString
                        dglistofval.Rows.Item(n).Cells(2).Value = .Item("LIC").ToString
                        dglistofval.Rows.Item(n).Cells(3).Value = .Item("Type").ToString
                        dglistofval.Rows.Item(n).Cells(4).Value = .Item("ParentLIC").ToString
                        dglistofval.Rows.Item(n).Cells(5).Value = IIf(.Item("Active") = "Yes", True, False)
                        dglistofval.Rows.Item(n).Cells(6).Value = .Item("Description").ToString

                    End With
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error code FillListofvalues")
        End Try

    End Sub

    Private Sub fillsearchlistofvalues()
        Try
            Dim search As String

            If txtSearch.Text = Nothing Then
                search = ""
            Else
                search = " Where DisplayValue = '" & txtSearch.Text & "'"
            End If
            Dim dt As New DataTable

            dt = getDataTableForSQL("Select * From listofval" & search)
            dglistofval.Rows.Clear()

            If dt.Rows.Count > 0 Then
                For Each rowd As DataRow In dt.Rows
                    Dim n As Integer = dglistofval.Rows.Add()
                    With rowd

                        dglistofval.Rows.Item(n).Cells(0).Value = .Item("RowID").ToString
                        dglistofval.Rows.Item(n).Cells(1).Value = .Item("DisplayValue").ToString
                        dglistofval.Rows.Item(n).Cells(2).Value = .Item("LIC").ToString
                        dglistofval.Rows.Item(n).Cells(3).Value = .Item("Type").ToString
                        dglistofval.Rows.Item(n).Cells(4).Value = .Item("ParentLIC").ToString
                        dglistofval.Rows.Item(n).Cells(5).Value = IIf(.Item("Active") = "Yes", True, False)
                        dglistofval.Rows.Item(n).Cells(6).Value = .Item("Description").ToString

                    End With
                Next
            Else
                If txtSearch.Text = Nothing Then
                    search = ""
                Else
                    search = " Where LIC = '" & txtSearch.Text & "'"
                End If
                dt = getDataTableForSQL("Select * From listofval" & search)

                If dt.Rows.Count > 0 Then
                    For Each rowd As DataRow In dt.Rows
                        Dim n As Integer = dglistofval.Rows.Add()
                        With rowd

                            dglistofval.Rows.Item(n).Cells(0).Value = .Item("RowID").ToString
                            dglistofval.Rows.Item(n).Cells(1).Value = .Item("DisplayValue").ToString
                            dglistofval.Rows.Item(n).Cells(2).Value = .Item("LIC").ToString
                            dglistofval.Rows.Item(n).Cells(3).Value = .Item("Type").ToString
                            dglistofval.Rows.Item(n).Cells(4).Value = .Item("ParentLIC").ToString
                            dglistofval.Rows.Item(n).Cells(5).Value = IIf(.Item("Active") = "Yes", True, False)
                            dglistofval.Rows.Item(n).Cells(6).Value = .Item("Description").ToString

                        End With
                    Next
                Else
                    If txtSearch.Text = Nothing Then
                        search = ""
                    Else
                        search = " Where Type = '" & txtSearch.Text & "'"
                    End If
                    dt = getDataTableForSQL("Select * From listofval" & search)

                    If dt.Rows.Count > 0 Then
                        For Each rowd As DataRow In dt.Rows
                            Dim n As Integer = dglistofval.Rows.Add()
                            With rowd

                                dglistofval.Rows.Item(n).Cells(0).Value = .Item("RowID").ToString
                                dglistofval.Rows.Item(n).Cells(1).Value = .Item("DisplayValue").ToString
                                dglistofval.Rows.Item(n).Cells(2).Value = .Item("LIC").ToString
                                dglistofval.Rows.Item(n).Cells(3).Value = .Item("Type").ToString
                                dglistofval.Rows.Item(n).Cells(4).Value = .Item("ParentLIC").ToString
                                dglistofval.Rows.Item(n).Cells(5).Value = IIf(.Item("Active") = "Yes", True, False)
                                dglistofval.Rows.Item(n).Cells(6).Value = .Item("Description").ToString

                            End With
                        Next
                    Else
                        If txtSearch.Text = Nothing Then
                            search = ""
                        Else
                            search = " Where ParentLIC = '" & txtSearch.Text & "'"
                        End If
                        dt = getDataTableForSQL("Select * From listofval" & search)

                        If dt.Rows.Count > 0 Then
                            For Each rowd As DataRow In dt.Rows
                                Dim n As Integer = dglistofval.Rows.Add()
                                With rowd

                                    dglistofval.Rows.Item(n).Cells(0).Value = .Item("RowID").ToString
                                    dglistofval.Rows.Item(n).Cells(1).Value = .Item("DisplayValue").ToString
                                    dglistofval.Rows.Item(n).Cells(2).Value = .Item("LIC").ToString
                                    dglistofval.Rows.Item(n).Cells(3).Value = .Item("Type").ToString
                                    dglistofval.Rows.Item(n).Cells(4).Value = .Item("ParentLIC").ToString
                                    dglistofval.Rows.Item(n).Cells(5).Value = IIf(.Item("Active") = "Yes", True, False)
                                    dglistofval.Rows.Item(n).Cells(6).Value = .Item("Description").ToString

                                End With
                            Next
                        Else
                            If txtSearch.Text = Nothing Then
                                search = ""
                            Else
                                search = " Where Active = '" & txtSearch.Text & "'"
                            End If
                            dt = getDataTableForSQL("Select * From listofval" & search)

                            If dt.Rows.Count > 0 Then
                                For Each rowd As DataRow In dt.Rows
                                    Dim n As Integer = dglistofval.Rows.Add()
                                    With rowd

                                        dglistofval.Rows.Item(n).Cells(0).Value = .Item("RowID").ToString
                                        dglistofval.Rows.Item(n).Cells(1).Value = .Item("DisplayValue").ToString
                                        dglistofval.Rows.Item(n).Cells(2).Value = .Item("LIC").ToString
                                        dglistofval.Rows.Item(n).Cells(3).Value = .Item("Type").ToString
                                        dglistofval.Rows.Item(n).Cells(4).Value = .Item("ParentLIC").ToString
                                        dglistofval.Rows.Item(n).Cells(5).Value = IIf(.Item("Active") = "Yes", True, False)
                                        dglistofval.Rows.Item(n).Cells(6).Value = .Item("Description").ToString

                                    End With
                                Next
                            Else

                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error code FillSeachListofvalues")
        End Try

    End Sub

    Dim curr_type As String = String.Empty

    Private Sub fillSelectedValue()
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * From Listofval where RowID = '" & dglistofval.CurrentRow.Cells(c_rowid.Index).Value & "'")

        cleartextbox()
        If dt.Rows.Count > 0 Then
            For Each drow As DataRow In dt.Rows
                With drow
                    Dim type As String = String.Empty
                    txtDescription.Text = .Item("Description").ToString

                    txtDescription.SelectionStart = 1

                    txtDisplayval.Text = .Item("DisplayValue").ToString
                    txtLIC.Text = .Item("LIC").ToString
                    txtParentLIC.Text = .Item("ParentLIC").ToString
                    txtType.Text = .Item("Type").ToString

                    curr_type = txtType.Text

                    rowid = .Item("RowID").ToString
                    Try
                        type = IIf(.Item("Active") = "Yes", True, False)
                    Catch ex As Exception
                        MsgBox(getErrExcptn(ex, Name))
                    End Try
                    If type = "True" Then
                        cmbStatus.Text = "Yes"
                    Else
                        cmbStatus.Text = "No"
                    End If
                    'cmbStatus.Text = type
                End With
            Next
        End If
    End Sub
    Private Sub cleartextbox()
        txtDescription.Clear()
        txtDisplayval.Clear()
        txtLIC.Clear()
        txtType.Clear()
        cmbStatus.SelectedIndex = -1
        txtParentLIC.Clear()


    End Sub
    Private Sub tsNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        grplistval.Enabled = True
        btnSave.Enabled = True
        btnNew.Enabled = False
        cleartextbox()
        isNew = 1

        dglistofval.Rows.Clear()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        grplistval.Enabled = False
        btnSave.Enabled = False
        btnNew.Enabled = True
        cleartextbox()
        isNew = 0
    End Sub

    Dim dontUpdate As SByte = 0

    Private Sub btnSave_Click(sender As Object, e As EventArgs)

        For Each dgvrow As DataGridViewRow In dglistofval.Rows

            With dgvrow

                If .Cells("c_rowid").Value = Nothing Then

                    .Cells("c_rowid").Value = _
                    sp_list(txtDisplayval.Text, txtLIC.Text, txtType.Text, txtParentLIC.Text, cmbStatus.Text, txtDescription.Text, z_datetime, z_User, z_datetime, 1, z_User)

                Else

                End If

            End With

        Next

    End Sub

    Private Sub btnSave_Previous_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        btnSave.Enabled = False

        dglistofval.EndEdit(True)

        If isNew = 1 Then
            If txtDisplayval.Text = Nothing Or txtType.Text = Nothing Or cmbStatus.Text = Nothing Then
                If Not SetWarningIfEmpty(txtDisplayval) And SetWarningIfEmpty(txtType) And SetWarningIfEmpty(cmbStatus) Then

                End If
            Else
                sp_list(txtDisplayval.Text, txtLIC.Text, txtType.Text, txtParentLIC.Text, cmbStatus.Text, txtDescription.Text, z_datetime, z_User, z_datetime, 1, z_User)
                myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
                'filllistofvalues()
                isNew = 0

            End If
        Else
            If dontUpdate = 1 Then
                Exit Sub
            End If

            If txtDisplayval.Text = Nothing Or txtType.Text = Nothing Or cmbStatus.Text = Nothing Then
                If Not SetWarningIfEmpty(txtDisplayval) And SetWarningIfEmpty(txtType) And SetWarningIfEmpty(cmbStatus) Then

                End If
            Else

                If curr_type.Trim <> txtType.Text.Trim Then

                    txtType.Text = Trim(txtType.Text)

                    EXECQUER("UPDATE listofval lv" &
                             " INNER JOIN listofval lov ON lov.RowID='" & dglistofval.CurrentRow.Cells(c_rowid.Index).Value & "'" &
                             " SET lv.`Type`='" & txtType.Text.Trim & "'" &
                             ",LastUpd=CURRENT_TIMESTAMP()" &
                             ",LastUpdBy=" & z_User &
                             " WHERE lv.`Type`=lov.`Type`;")

                End If

                sp_listupd(dglistofval.CurrentRow.Cells(c_rowid.Index).Value, txtDisplayval.Text, txtLIC.Text, txtType.Text, txtParentLIC.Text, cmbStatus.Text, txtDescription.Text, z_datetime, z_User, z_datetime, 1, z_User)
                myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
                'filllistofvalues()
                isNew = 0
            End If

        End If

        If btnNew.Enabled = False Then

            btnNew.Enabled = True

        End If

        If autcoListOfValType.Text.Trim.Length = 0 Then

            Button4_Click(Button4, New EventArgs)

        Else

            Button4_Click(btnSearchNow, New EventArgs)

        End If

        btnSave.Enabled = True

        If txtType.Text = "DutyShift" _
            And Application.OpenForms.OfType(Of ShiftScheduleForm).Any Then
            Application.OpenForms.OfType(Of ShiftScheduleForm).FirstOrDefault.LoadShiftScheduleConfigurablePolicy()
        End If

    End Sub

    Private Sub ListOfValueForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        'If FormLeft.Contains("List of value") Then
        '    FormLeft.Remove("List of value")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.Text = "Welcome"
        'Else
        '    MDIPrimaryForm.Text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

    Dim view_ID As Integer = Nothing

    Private Sub ListOfValueForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        'filllistofvalues()


        view_ID = VIEW_privilege("List of value", orgztnID)

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

        bgworklistofvaltypes.RunWorkerAsync()

    End Sub

    Private Sub dglistofval_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dglistofval.CellContentClick

    End Sub

    Private Sub dglistofval_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dglistofval.CellClick
        curr_type = String.Empty
        fillSelectedValue()
        grplistval.Enabled = True
        btnSave.Enabled = True
        btnDelete.Enabled = True
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        fillsearchlistofvalues()

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If MsgBox("Are you sure you want to delete " & dglistofval.CurrentRow.Cells(c_display.Index).Value & "?", MsgBoxStyle.YesNo, "Deleting...") = MsgBoxResult.No Then
        Else
            DirectCommand("Delete from listofval where RowID = '" & rowid & "';")
            'filllistofvalues()

        End If
    End Sub

    Private Sub tsAudittrail_Click(sender As Object, e As EventArgs) Handles tsAudittrail.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Dim pagination As Integer = 0

    Private Sub pagination_handler(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked,
                                                                                                    LinkLabel2.LinkClicked,
                                                                                                    LinkLabel3.LinkClicked,
                                                                                                    LinkLabel4.LinkClicked

        Dim sender_obj = DirectCast(sender, LinkLabel)

        If sender_obj.Name = "LinkLabel1" Then 'First

            pagination = 0

        ElseIf sender_obj.Name = "LinkLabel2" Then 'Previous

            'If pagination - 50 < 0 Then

            '    pagination = 0

            'Else

            '    pagination -= 50

            'End If

            Dim modcent = pagination Mod 50

            If modcent = 0 Then

                pagination -= 50

            Else

                pagination -= modcent

                'pagination -= 50

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sender_obj.Name = "LinkLabel4" Then 'Next

            Dim modcent = pagination Mod 50

            If modcent = 0 Then

                pagination += 50

            Else

                pagination -= modcent

                pagination += 50

            End If

        ElseIf sender_obj.Name = "LinkLabel3" Then 'Last
            
            Dim lastpage = Val(EXECQUER("SELECT COUNT(DISTINCT(`Type`)) / 50 FROM listofval;"))

            Dim remender = lastpage Mod 1

            pagination = (lastpage - remender) * 50

        End If

        Button4_Click(Button4, New EventArgs)

    End Sub

    Dim prev_rowindex As Integer = 0

    Dim prev_colindex As Integer = 0

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        'pagination = 0

        Button4.Enabled = False

        Static once As SByte = 0

        If once = 0 Then

        Else

            Try

                prev_rowindex = dgvlistofvaltype.CurrentRow.Index

                prev_colindex = dgvlistofvaltype.CurrentCell.ColumnIndex

            Catch ex As Exception

                prev_rowindex = -1

                prev_colindex = -1

            End Try

        End If

        RemoveHandler dgvlistofvaltype.SelectionChanged, AddressOf dgvlistofvaltype_SelectionChanged

        Dim dt As New DataTable

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT DISTINCT(`Type`) FROM listofval ORDER BY `Type` LIMIT " & pagination & ", 50;")

        If sender.Name = "btnSearchNow" Then

            If autcoListOfValType.Text.Trim.Length = 0 Then

                dt = n_SQLQueryToDatatable.ResultTable

            Else

                n_SQLQueryToDatatable = New SQLQueryToDatatable("SELECT DISTINCT(`Type`) FROM listofval WHERE `Type` LIKE '%" & autcoListOfValType.Text & "%' LIMIT " & pagination & ", 50;")

                dt = n_SQLQueryToDatatable.ResultTable

            End If

        Else

            n_SQLQueryToDatatable = New SQLQueryToDatatable("SELECT DISTINCT(`Type`) FROM listofval ORDER BY `Type` LIMIT " & pagination & ", 50;")

            dt = n_SQLQueryToDatatable.ResultTable

        End If

        dgvlistofvaltype.Rows.Clear()

        For Each drow As DataRow In dt.Rows

            dgvlistofvaltype.Rows.Add(drow(0))

        Next

        If once = 0 Then
            once = 1
        Else

            If prev_rowindex <> -1 Then

                If prev_rowindex < dgvlistofvaltype.RowCount Then

                    dgvlistofvaltype.Item(prev_colindex, prev_rowindex).Selected = True

                End If

            End If

        End If

        dgvlistofvaltype_SelectionChanged(sender, e)

        AddHandler dgvlistofvaltype.SelectionChanged, AddressOf dgvlistofvaltype_SelectionChanged

        Button4.Enabled = True

    End Sub

    Private Sub dgvlistofvaltype_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvlistofvaltype.CellContentClick

    End Sub

    Dim strcurrent_type As String = String.Empty

    Private Sub dgvlistofvaltype_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvlistofvaltype.SelectionChanged

        If dgvlistofvaltype.RowCount <> 0 Then

            strcurrent_type = dgvlistofvaltype.CurrentRow.Cells("Column1").Value

        Else

            strcurrent_type = String.Empty

        End If

        filllistofvalues()

        If dglistofval.RowCount <> 0 Then

            dglistofval_CellClick(sender, New DataGridViewCellEventArgs(c_display.Index, 0))

        End If

    End Sub

    Dim thestr = String.Empty

    Private Sub bgworklistofvaltypes_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworklistofvaltypes.DoWork

        Dim listtypes As New AutoCompleteStringCollection

        enlistTheLists("SELECT DISTINCT(`Type`) 'TypeName' FROM listofval WHERE IFNULL(`Type`,'')!='';",
                       listtypes) ' AND LOCATE('(',`Type`) = 0 AND LOCATE(')',`Type`) = 0

        'MsgBox(listtypes.Count.ToString)

        For Each strval In listtypes

            thestr = String.Empty

            thestr = strval.ToString

            If thestr <> String.Empty Then

                autcoListOfValType.Items.Add(New AutoCompleteEntry(thestr, StringToArray(thestr)))

            End If

        Next

        '***************************

        'dattbl = retAsDatTbl("SELECT DISTINCT(`Type`) 'TypeName' FROM listofval WHERE IFNULL(`Type`,'')!='';")

        'If dattbl IsNot Nothing Then

        '    Dim row_count = dattbl.Rows.Count

        '    For Each drow As DataRow In dattbl.Rows

        '        If IsDBNull(drow(0)) Then

        '            Continue For

        '        Else

        '            Dim thestr = String.Empty

        '            thestr = CStr(drow(0))

        '            If thestr <> String.Empty Then

        '                autcoListOfValType.Items.Add(New AutoCompleteEntry(CStr(drow(0))))

        '            End If

        '        End If

        '    Next

        '    dattbl.Dispose()

        'End If

    End Sub

    Private Sub bgworklistofvaltypes_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworklistofvaltypes.ProgressChanged

    End Sub

    Private Sub bgworklistofvaltypes_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworklistofvaltypes.RunWorkerCompleted

        If e.Error IsNot Nothing Then

            MsgBox(thestr)

            MessageBox.Show("Error: " & e.Error.Message)

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")

        Else

        End If

        autcoListOfValType.Enabled = True

        autcoListOfValType.Text = String.Empty

    End Sub

    Private Sub btnSearchNow_Click(sender As Object, e As EventArgs) Handles btnSearchNow.Click

        pagination = 0

        Button4_Click(btnSearchNow, New EventArgs)

    End Sub

    Private Sub autcoListOfValType_KeyDown(sender As Object, e As KeyEventArgs) Handles autcoListOfValType.KeyDown

    End Sub

    Private Sub autcoListOfValType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles autcoListOfValType.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then

            btnSearchNow_Click(sender, e)

        End If

    End Sub

    Private Sub txtDescription_TextChanged(sender As Object, e As EventArgs) Handles txtDescription.TextChanged

    End Sub

End Class