Public Class ShiftEntryForm

    Private IsNew As Integer

    Private my_RowID = Nothing

    Private view_ID As Integer = Nothing

    Private UpKey As SByte = 0

    Private DownKey As SByte = 0

    Private dontUpdate As SByte = 0

    Private my_TimeFrom = Nothing

    Private my_TimeTo = Nothing

    Dim SecondSelector As String = ""

    Property ShiftRowID As Object
        Get
            Return my_RowID
        End Get
        Set(value As Object)
            my_RowID = value
        End Set
    End Property

    Property ShiftTimeFrom As Object
        Get
            Return my_TimeFrom
        End Get
        Set(value As Object)
            my_TimeFrom = value
        End Set
    End Property

    Property ShiftTimeTo As Object
        Get
            Return my_TimeTo
        End Get
        Set(value As Object)
            my_TimeTo = value
        End Set
    End Property

    Private Sub fillshiftentry()
        Dim maxidonshift As DataTable = getDataTableForSQL($"
                SELECT  MAX(RowId)
                FROM shift
                WHERE
                    OrganizationID = '{z_OrganizationID}'
                ")
        Dim dt As New DataTable
        dt = New SQLQueryToDatatable("SELECT TIME_FORMAT(TimeFrom, '%h:%i %p') timef, TIME_FORMAT(TimeTo, '%h:%i %p') timet" &
                                     ", DivisorToDailyRate" &
                                     ", ADDTIME(TIMESTAMP(CURDATE()),BreakTimeFrom) AS BreakTimeFrom" &
                                     ", ADDTIME(TIMESTAMP(CURDATE()),BreakTimeTo) AS BreakTimeTo" &
                                     ", Hidden" &
                                     ", RowID FROM shift WHERE OrganizationID = '" & z_OrganizationID & "' ORDER BY TIME_FORMAT(TimeFrom,'%p %H:%i:%s'),TIME_FORMAT(TimeTo,'%p %H:%i:%s');").ResultTable
        dgvshiftentry.Rows.Clear()

        Dim maxid As Integer = Convert.ToInt32(maxidonshift.Rows(0).Item(0).ToString)

        'Dim maxidresult As Integer = 830

        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvshiftentry.Rows.Add()
            With drow
                dgvshiftentry.Rows.Item(n).Cells(c_timef.Index).Value = .Item("timef").ToString
                dgvshiftentry.Rows.Item(n).Cells(c_timef.Index).Value = .Item("timef").ToString
                dgvshiftentry.Rows.Item(n).Cells(c_timet.Index).Value = .Item("timet").ToString

                If IsNew = 0 Then
                    If .Item("RowID").ToString = SecondSelector Then
                        dgvshiftentry.Rows(n).Selected = True
                        dgvshiftentry.CurrentCell = dgvshiftentry.Rows(n).Cells(0)
                    End If
                ElseIf IsNew = 1 Then
                    If .Item("RowID").ToString = maxid.ToString Then
                        dgvshiftentry.Rows(n).Selected = True
                        dgvshiftentry.CurrentCell = dgvshiftentry.Rows(n).Cells(0)
                    End If
                End If

                dgvshiftentry.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("RowID").ToString
                dgvshiftentry.Item("DivisorToDailyRate", n).Value = ValNoComma(.Item("DivisorToDailyRate"))

                dgvshiftentry.Rows.Item(n).Cells(breaktimefrom.Index).Value = If(IsDBNull(.Item("BreakTimeFrom")), Nothing, .Item("BreakTimeFrom"))
                dgvshiftentry.Rows.Item(n).Cells(breaktimeto.Index).Value = If(IsDBNull(.Item("BreakTimeTo")), Nothing, .Item("BreakTimeTo"))
                dgvshiftentry.Rows.Item(n).Cells(IsHidden.Index).Value = .Item("Hidden")
            End With
        Next

        'SecondSelector = 0
        'maxid = 0

        'If Selector = Nothing Then

        'Else
        '    dgvshiftentry.Rows(Convert.ToUInt32(Selector)).Selected = True
        '    dgvshiftentry.CurrentCell = dgvshiftentry.Rows(Convert.ToUInt32(Selector)).Cells(0)
        'End If

    End Sub

    Private Sub ShiftEntryForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        'If FormLeft.Contains("Duty shifting") Then
        '    FormLeft.Remove("Duty shifting")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.Text = "Welcome"
        'Else
        '    MDIPrimaryForm.Text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If

        GeneralForm.listGeneralForm.Remove(Me.Name)
    End Sub

    Private Sub ShiftEntryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not dgvshiftentry.Rows.Count = 0 Then
            dtpTimeFrom.Text = dgvshiftentry.CurrentRow.Cells(c_timef.Index).Value
            dtpTimeTo.Text = dgvshiftentry.CurrentRow.Cells(c_timet.Index).Value
        End If
        fillshiftentry()

        view_ID = VIEW_privilege("Duty shifting", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then

            tsbtnNewShift.Visible = 0
            tsbtnSaveShift.Visible = 0

            'btnNew.Visible = 0
            'btnSave.Visible = 0
            'btnDelete.Visible = 0
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    'btnNew.Visible = 0
                    tsbtnNewShift.Visible = 0

                    'btnSave.Visible = 0
                    tsbtnSaveShift.Visible = 0

                    btnDelete.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        'btnNew.Visible = 0
                        tsbtnNewShift.Visible = 0
                    Else
                        'btnNew.Visible = 1
                        tsbtnNewShift.Visible = 1
                    End If

                    'If drow("Deleting").ToString = "N" Then
                    btnDelete.Visible = 0
                    'Else
                    '    btnDelete.Visible = 1
                    'End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

        If dgvshiftentry.RowCount <> 0 Then
            dgvshiftentry_CellClick(sender, New DataGridViewCellEventArgs(c_timef.Index, 0))
        End If
        txtDivisorToDailyRate.ContextMenuStrip = New ContextMenuStrip
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If MsgBox("Are you sure you want to remove this Shift Entry. FROM" & dgvshiftentry.CurrentRow.Cells(c_timef.Index).Value & " TO " & dgvshiftentry.CurrentRow.Cells(c_timet.Index).Value & "?", MsgBoxStyle.YesNo, "Removing...") = MsgBoxResult.Yes Then
            DirectCommand("DELETE FROM Shift Where RowID = '" & dgvshiftentry.CurrentRow.Cells(c_rowid.Index).Value & "'")
            fillshiftentry()
            myBalloon("Successfully Deleted", "Deleting", btnDelete, , -65)
        End If
    End Sub

    Private Sub dgvshiftentry_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvshiftentry.CellClick
        Try
            dtpTimeFrom.Text = dgvshiftentry.CurrentRow.Cells(c_timef.Index).Value
            dtpTimeTo.Text = dgvshiftentry.CurrentRow.Cells(c_timet.Index).Value

            txtDivisorToDailyRate.Text = dgvshiftentry.CurrentRow.Cells("DivisorToDailyRate").Value

            my_TimeFrom = Format(CDate(dgvshiftentry.CurrentRow.Cells(c_timef.Index).Value), "HH:mm")
            my_TimeTo = Format(CDate(dgvshiftentry.CurrentRow.Cells(c_timet.Index).Value), "HH:mm")

            my_RowID = dgvshiftentry.CurrentRow.Cells(c_rowid.Index).Value

            Dim bool_result As Boolean = False

            If dgvshiftentry.CurrentRow.Cells("breaktimefrom").Value = Nothing Then
            Else
                dtpBreakTimeFrom.Text = Format(CDate(dgvshiftentry.CurrentRow.Cells("breaktimefrom").Value), dtpBreakTimeFrom.CustomFormat)
                bool_result = True
            End If

            If dgvshiftentry.CurrentRow.Cells("breaktimeto").Value = Nothing Then
            Else
                dtpBreakTimeTo.Text = Format(CDate(dgvshiftentry.CurrentRow.Cells("breaktimeto").Value), dtpBreakTimeTo.CustomFormat)
                bool_result = (bool_result = True)
            End If
            chkHasLunchBreak.Checked = bool_result
            dgvshiftentry.Tag = dgvshiftentry.CurrentRow.Cells(c_rowid.Index).Value
            chkHidden.Checked = Convert.ToInt16(dgvshiftentry.CurrentRow.Cells(IsHidden.Index).Value)

            SecondSelector = dgvshiftentry.CurrentRow.Cells(c_rowid.Index).Value
        Catch ex As Exception
            my_RowID = Nothing
            my_TimeFrom = Nothing
            my_TimeTo = Nothing
        Finally
            RemoveHandler dtpBreakTimeFrom.ValueChanged, AddressOf dtpBreakTimeFrom_ValueChanged
            RemoveHandler dtpBreakTimeTo.ValueChanged, AddressOf dtpBreakTimeTo_ValueChanged

            AddHandler dtpBreakTimeFrom.ValueChanged, AddressOf dtpBreakTimeFrom_ValueChanged
            AddHandler dtpBreakTimeTo.ValueChanged, AddressOf dtpBreakTimeTo_ValueChanged
        End Try
    End Sub

    Private Sub dgvshiftentry_DoubleClick(sender As Object, e As EventArgs) Handles dgvshiftentry.DoubleClick
        'EmployeeShiftEntryForm.lblShiftID.Text = dgvshiftentry.CurrentRow.Cells(c_rowid.Index).Value
        'EmployeeShiftEntryForm.dtpTimeFrom.Text = dgvshiftentry.CurrentRow.Cells(c_timef.Index).Value
        'EmployeeShiftEntryForm.dtpTimeTo.Text = dgvshiftentry.CurrentRow.Cells(c_timet.Index).Value
        'Me.Hide()

        If dgvshiftentry.RowCount <> 0 Then
            dgvshiftentry_CellClick(sender, New DataGridViewCellEventArgs(c_timef.Index, dgvshiftentry.CurrentRow.Index))
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Else
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub

    Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        IsNew = 1
        btnNew.Enabled = False

    End Sub

    Sub tsbtnNewShift_Click(sender As Object, e As EventArgs) Handles tsbtnNewShift.Click
        IsNew = 1

        tsbtnNewShift.Enabled = False
        dtpTimeFrom.Focus()
        chkHasLunchBreak.Checked = False
    End Sub

    Sub Updatequery()
        Dim str_quer As String = "UPDATE shift SET LastUpd = CURRENT_TIMESTAMP()" &
                        ", LastUpdBy = '" & z_User & "'" &
                        ", TimeFrom=" & "'" & dtpTimeFrom.Value.ToString("HH:mm") & "'" &
                         ", TimeTo=" & "'" & dtpTimeTo.Value.ToString("HH:mm") & "'" &
                       ", BreakTimeFrom=" & If(chkHasLunchBreak.Checked, ("'" & dtpBreakTimeFrom.Tag.ToString & "'"), "NULL") &
                       ", BreakTimeTo=" & If(chkHasLunchBreak.Checked, ("'" & dtpBreakTimeTo.Tag.ToString & "'"), "NULL") &
                        ", Hidden=" & Convert.ToInt16(chkHidden.Checked) &
                        " WHERE RowID = '" & dgvshiftentry.Tag & "';"
        Dim n_ExecuteQuery As New ExecuteQuery(str_quer)

        If n_ExecuteQuery.HasError = False Then
            myBalloon("Successfully Updated", "Updating...", btnSave, , -65)
        Else
            Try
                Throw New Exception(n_ExecuteQuery.ErrorMessage)
            Catch ex As Exception
                MsgBox(getErrExcptn(ex, Name))
            End Try
        End If
    End Sub

    Private Sub tsbtnSaveShift_Click(sender As Object, e As EventArgs) Handles tsbtnSaveShift.Click
        Dim timeFrom = dtpTimeFrom.Value.ToString("HH:mm")
        Dim timeTo = dtpTimeTo.Value.ToString("HH:mm")
        Dim breaktimeFrom = dtpBreakTimeFrom.Tag
        Dim breaktimeTo = dtpBreakTimeTo.Tag
        Dim hasBreaktime = (breaktimeFrom IsNot Nothing) Or (breaktimeTo IsNot Nothing)

        Dim existingShifts As DataTable = getDataTableForSQL($"
            SELECT *
            FROM shift
            WHERE TimeFrom = '{timeFrom}' AND
                TimeTo = '{timeTo}' AND
                (BreaktimeFrom  = '{breaktimeFrom}' OR NOT {hasBreaktime}) AND
                (BreaktimeTo = '{breaktimeTo}' OR NOT {hasBreaktime}) AND
                OrganizationID = '{z_OrganizationID}'
        ")

        If IsNew = 1 Then

            ' /test fix bugs

            Dim timeFrom12Hour = dtpTimeFrom.Value.ToString("hh:mm")
            Dim timeTo12Hour = dtpTimeTo.Value.ToString("hh:mm")

            Dim breaktimeFrom12Hour = dtpBreakTimeFrom.Value.ToString("hh:mm")
            Dim breaktimeto12Hour = dtpBreakTimeTo.Value.ToString("hh:mm")

            ' Has a lunch break
            If chkHasLunchBreak.Checked Then

                Dim existingShifts4updatehaslunch As DataTable = getDataTableForSQL($"
                SELECT *
                FROM shift
                WHERE TimeFrom = '{timeFrom}' AND
                    TimeTo = '{timeTo}' AND
                    BreaktimeFrom  = '{breaktimeFrom}' AND
                    BreaktimeTo = '{breaktimeTo}'  AND
                    OrganizationID = '{z_OrganizationID}'
                ")

                If existingShifts4updatehaslunch.Rows.Count > 0 Then
                    MsgBox($"A shift from {timeFrom12Hour} to {timeTo12Hour} and breaktime from {breaktimeFrom12Hour} to {breaktimeto12Hour} already exists.", MsgBoxStyle.Exclamation, "Shift already exists")
                    Return
                Else

                    sp_shift(z_datetime, z_User, z_datetime, z_OrganizationID, z_User, dtpTimeFrom.Value, dtpTimeTo.Value)

                    myBalloon("successfully save", "saving...", btnSave, , -65)
                    Dim shiftid As String = getStringItem("select max(rowid) from shift")
                    Dim getshiftid As Integer = Val(shiftid)

                    fillshiftentry()
                    IsNew = 0

                End If
            Else
                Dim existingShifts4updatehasnolunch As DataTable = getDataTableForSQL($"
                SELECT *
                FROM shift
                WHERE TimeFrom = '{timeFrom}' AND
                TimeTo = '{timeTo}' AND
                BreaktimeFrom IS NULL  AND
                BreaktimeTo IS NULL  AND

                    OrganizationID = '{z_OrganizationID}'
                ")

                If existingShifts4updatehasnolunch.Rows.Count > 0 Then
                    MsgBox($"A shift from {timeFrom12Hour} to {timeTo12Hour} already exists.  ", MsgBoxStyle.Exclamation, "Shift already exists")

                    Return
                Else
                    sp_shift(z_datetime, z_User, z_datetime, z_OrganizationID, z_User, dtpTimeFrom.Value, dtpTimeTo.Value)

                    myBalloon("successfully save", "saving...", btnSave, , -65)
                    Dim shiftid As String = getStringItem("select max(rowid) from shift")
                    Dim getshiftid As Integer = Val(shiftid)

                    fillshiftentry()
                    IsNew = 0

                End If
            End If

            ' END Has a lunch break

            ' /end test fix bugs

            'Dim timeFrom12Hour = dtpTimeFrom.Value.ToString("hh:mm")
            'Dim timeTo12Hour = dtpTimeTo.Value.ToString("hh:mm")

            'If existingShifts.Rows.Count > 0 Then
            '    MsgBox($"A shift from {timeFrom12Hour} to {timeTo12Hour} already exists.", MsgBoxStyle.Exclamation, "Shift already exists")
            '    Return
            'End If

            'sp_shift(z_datetime, z_User, z_datetime, z_OrganizationID, z_User, dtpTimeFrom.Value, dtpTimeTo.Value)

            'myBalloon("Successfully Save", "Saving...", btnSave, , -65)
            'Dim shiftid As String = getStringItem("Select MAX(RowID) From Shift")
            'Dim getshiftid As Integer = Val(shiftid)
            'EmployeeShiftEntryForm.lblShiftID.Text = getshiftid
            'EmployeeShiftEntryForm.dtpTimeFrom.Text = dgvshiftentry.CurrentRow.Cells(c_timef.Index).Value
            'EmployeeShiftEntryForm.dtpTimeTo.Text = dgvshiftentry.CurrentRow.Cells(c_timet.Index).Value
            'IsNew = 0
            'fillshiftentry()
            'Me.Hide()
        Else
            If dontUpdate = 1 Then
                Exit Sub
            End If

            ' /test fix bugs

            Dim timeFrom12Hour = dtpTimeFrom.Value.ToString("hh:mm")
            Dim timeTo12Hour = dtpTimeTo.Value.ToString("hh:mm")

            Dim breaktimeFrom12Hour = dtpBreakTimeFrom.Value.ToString("hh:mm")
            Dim breaktimeto12Hour = dtpBreakTimeTo.Value.ToString("hh:mm")

            ' Has a lunch break
            If chkHasLunchBreak.Checked Then

                Dim existingShifts4updatehaslunch As DataTable = getDataTableForSQL($"
                SELECT *
                FROM shift
                WHERE TimeFrom = '{timeFrom}' AND
                    TimeTo = '{timeTo}' AND
                    BreaktimeFrom  = '{breaktimeFrom}' AND
                    BreaktimeTo = '{breaktimeTo}'  AND
                    OrganizationID = '{z_OrganizationID}'
                ")

                If existingShifts4updatehaslunch.Rows.Count > 0 Then
                    MsgBox($"A shift from {timeFrom12Hour} to {timeTo12Hour} and breaktime from {breaktimeFrom12Hour} to {breaktimeto12Hour} already exists.", MsgBoxStyle.Exclamation, "Shift already exists")
                    Return
                Else

                    Updatequery()
                    fillshiftentry()
                    IsNew = 0

                End If
            Else
                Dim existingShifts4updatehasnolunch As DataTable = getDataTableForSQL($"
                SELECT *
                FROM shift
                WHERE TimeFrom = '{timeFrom}' AND
                TimeTo = '{timeTo}' AND
                BreaktimeFrom IS NULL  AND
                BreaktimeTo IS NULL  AND

                    OrganizationID = '{z_OrganizationID}'
                ")

                If existingShifts4updatehasnolunch.Rows.Count > 0 Then
                    MsgBox($"A shift from {timeFrom12Hour} to {timeTo12Hour} already exists. ", MsgBoxStyle.Exclamation, "Shift already exists")

                    Return
                Else
                    Updatequery()

                    fillshiftentry()
                    IsNew = 0

                End If
            End If

            ' END Has a lunch break

            ' /end test fix bugs

            '/previous codes
            'Dim str_quer As String = "UPDATE shift SET LastUpd = CURRENT_TIMESTAMP()" &
            '        ", LastUpdBy = '" & z_User & "'" &
            '        ", DivisorToDailyRate=" & ValNoComma(txtDivisorToDailyRate.Text) &
            '        ", BreakTimeFrom=" & If(dtpBreakTimeFrom.Tag = Nothing, "NULL", ("'" & dtpBreakTimeFrom.Tag.ToString & "'")) &
            '        ", BreakTimeTo=" & If(dtpBreakTimeTo.Tag = Nothing, "NULL", ("'" & dtpBreakTimeTo.Tag.ToString & "'")) &
            '        ", Hidden=" & Convert.ToInt16(chkHidden.Checked) &
            '        " WHERE RowID = '" & dgvshiftentry.Tag & "';"
            'Dim n_ExecuteQuery As New ExecuteQuery(str_quer)

            'If n_ExecuteQuery.HasError = False Then
            '    myBalloon("Successfully Updated", "Updating...", btnSave, , -65)
            'Else
            '    Try
            '        Throw New Exception(n_ExecuteQuery.ErrorMessage)
            '    Catch ex As Exception
            '        MsgBox(getErrExcptn(ex, Name))
            '    End Try
            'End If
            'IsNew = 0
            'fillshiftentry()
            '/ end previous codes
        End If

        tsbtnNewShift.Enabled = True
    End Sub

    Private Sub tsbtnCancelShift_Click(sender As Object, e As EventArgs) Handles tsbtnCancelShift.Click
        tsbtnNewShift.Enabled = True

        IsNew = 0

        fillshiftentry()

        If dgvshiftentry.RowCount <> 0 Then
            dgvshiftentry_CellClick(sender, New DataGridViewCellEventArgs(c_timef.Index, 0))
        End If
    End Sub

    Private Sub tsbtnCloseShift_Click(sender As Object, e As EventArgs) Handles tsbtnCloseShift.Click
        Me.Close()
    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click
        showAuditTrail.Show()
        showAuditTrail.loadAudTrail(view_ID)
        showAuditTrail.BringToFront()
    End Sub

    Private Sub dgvshiftentry_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvshiftentry.KeyDown
        If e.KeyCode = Keys.Up Then
            UpKey = 1
            DownKey = 0
        ElseIf e.KeyCode = Keys.Down Then
            Dim ii = 0

            If UpKey = 1 Then
                UpKey = 0
                ii = dgvshiftentry.CurrentRow.Index + 1
            Else
                ii = dgvshiftentry.CurrentRow.Index + 1
            End If

            DownKey = 1
            UpKey = 0
        End If
    End Sub

    Private Sub txtBreakMinutes_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDivisorToDailyRate.KeyPress
        Dim n_TrapDecimalKey As New TrapDecimalKey(Asc(e.KeyChar), txtDivisorToDailyRate.Text.Trim)
        e.Handled = n_TrapDecimalKey.ResultTrap
    End Sub

    Private Sub chkHasLunchBreak_CheckedChanged(sender As Object, e As EventArgs) Handles chkHasLunchBreak.CheckedChanged
        'Dim bool_result As Boolean = chkHasLunchBreak.Checked
        'If bool_result Then
        '    dtpBreakTimeFrom.Tag = VBDateToMySQLDate(dtpBreakTimeFrom.Text)
        '    dtpBreakTimeTo.Tag = VBDateToMySQLDate(dtpBreakTimeTo.Text)

        'Else
        '    dtpBreakTimeFrom.Tag = Nothing
        '    dtpBreakTimeTo.Tag = Nothing

        'End If
        'dtpBreakTimeFrom.Enabled = bool_result
        'dtpBreakTimeTo.Enabled = bool_result

        dtpBreakTimeFrom_ValueChanged(dtpBreakTimeFrom, New EventArgs)
        dtpBreakTimeTo_ValueChanged(dtpBreakTimeFrom, New EventArgs)
    End Sub

    Private Sub dtpBreakTimeFrom_ValueChanged(sender As Object, e As EventArgs) 'Handles dtpBreakTimeFrom.ValueChanged
        If chkHasLunchBreak.Checked Then
            dtpBreakTimeFrom.Tag = VBDateToMySQLDate(dtpBreakTimeFrom.Text)
        Else
            dtpBreakTimeFrom.Tag = Nothing
        End If

        dtpBreakTimeFrom.Enabled = chkHasLunchBreak.Checked
    End Sub

    Private Sub dtpBreakTimeTo_ValueChanged(sender As Object, e As EventArgs) 'Handles dtpBreakTimeTo.ValueChanged
        If chkHasLunchBreak.Checked Then
            dtpBreakTimeTo.Tag = VBDateToMySQLDate(dtpBreakTimeTo.Text)
        Else
            dtpBreakTimeTo.Tag = Nothing
        End If

        dtpBreakTimeTo.Enabled = chkHasLunchBreak.Checked
    End Sub

    Private Function VBDateToMySQLDate(DateExpression As String) As Object
        Return New ExecuteQuery("SELECT STR_TO_DATE('" & DateExpression & "','%h:%i %p');").Result
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim hrs_shift = New ExecuteQuery("SELECT COMPUTE_TimeDifference(STR_TO_DATE('" & dtpTimeFrom.Text & "','%h:%i %p'), STR_TO_DATE('" & dtpTimeTo.Text & "','%h:%i %p'));").Result
        Dim hrs_break = New ExecuteQuery("SELECT COMPUTE_TimeDifference(STR_TO_DATE('" & dtpBreakTimeFrom.Text & "','%h:%i %p'), STR_TO_DATE('" & dtpBreakTimeTo.Text & "','%h:%i %p'));").Result

        MsgBox("hrs_shift is " & ValNoComma(hrs_shift) & vbNewLine &
               "hrs_break is " & ValNoComma(hrs_break) & vbNewLine &
               "and their difference is " & ValNoComma(hrs_shift) - ValNoComma(hrs_break))
    End Sub

    Private Sub chkHidden_CheckedChanged(sender As Object, e As EventArgs) Handles chkHidden.CheckedChanged

    End Sub

    Private Sub dgvshiftentry_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvshiftentry.CellContentClick

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

    End Sub

    Private Sub dtpBreakTimeFrom_ValueChanged_1(sender As Object, e As EventArgs) Handles dtpBreakTimeFrom.ValueChanged

    End Sub

End Class