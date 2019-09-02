Public Class dutyshift

    Dim view_ID As Integer = Nothing

    Private Sub dutyshift_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        showAuditTrail.Close()

    End Sub

    Private Sub dutyshift_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        view_ID = VIEW_privilege("Employee Shift", orgztnID)

        loadShift()

        AddHandler dgvshift.SelectionChanged, AddressOf dgvshift_SelectionChanged

    End Sub

    Dim shifttable As New DataTable

    Sub loadShift()

        shifttable = retAsDatTbl("SELECT RowID,TIME_FORMAT(TimeFrom,'%h:%i %p') 'TimeFrom',TIME_FORMAT(TimeTo,'%h:%i %p') 'TimeTo' FROM shift WHERE OrganizationID=" & orgztnID &
                                 " ORDER BY TIME_FORMAT(TimeFrom,'%p %h:%i'),TIME_FORMAT(TimeTo,'%p %h:%i');")

        dgvshift.Rows.Clear()

        For Each drow As DataRow In shifttable.Rows

            dgvshift.Rows.Add(drow("RowID"),
                              Format(CDate(drow("TimeFrom")), "hh:mm tt"),
                              drow("TimeTo"))

        Next

    End Sub

    Dim dgvleaveRowindx As Integer = Nothing

    Private Sub tsbtnNewShift_Click(sender As Object, e As EventArgs) Handles tsbtnNewShift.Click

        'dgvshift.EndEdit(True)

        'For Each dgvrow As DataGridViewRow In dgvshift.Rows
        '    If dgvrow.IsNewRow Then
        '        dgvrow.Cells("shf_TimeFrom").Selected = True
        '        dgvleaveRowindx = dgvrow.Index
        '        Exit For
        '    End If
        'Next

        'mtxtTimeFrom.Text = ""

        'mtxtTimeTo.Text = ""

        'dtpTimeFrom.Focus()

        MDIPrimaryForm.ToolStripButton1_Click(sender, e)

        GeneralForm.ChangeForm(ShiftEntryForm)
        previousForm = ShiftEntryForm

        ShiftEntryForm.tsbtnNewShift.PerformClick()

        Me.Close()

    End Sub

    Private Sub tsbtnSaveShift_Click(sender As Object, e As EventArgs) Handles tsbtnSaveShift.Click

        dgvshift.EndEdit(True)

        mtxtTimeFrom.Focus()
        dtpTimeFrom.Focus()

        Label1.Focus()

        mtxtTimeTo.Focus()
        dtpTimeTo.Focus()

        Label1.Focus()

        If haserrinputshift = 1 Then
            Exit Sub
        End If

        For Each dgvrow As DataGridViewRow In dgvshift.Rows
            With dgvrow
                If .IsNewRow Then
                Else
                    If Val(.Cells("shf_RowID").Value) = 0 Then
                        'shf_TimeFrom

                        .Cells("shf_RowID").Value = INSUPD_shift(,
                                         MilitTime(.Cells("shf_TimeFrom").Value),
                                         MilitTime(.Cells("shf_TimeTo").Value))
                    Else
                        If listofEditRowShift.Contains(.Cells("shf_RowID").Value) Then
                            INSUPD_shift(.Cells("shf_RowID").Value,
                                         MilitTime(.Cells("shf_TimeFrom").Value),
                                         MilitTime(.Cells("shf_TimeTo").Value))

                        End If

                    End If

                End If

            End With

        Next

        haserrinputshift = 0
        listofEditRowShift.Clear()

    End Sub

    Function INSUPD_shift(Optional sh_RowID As Object = Nothing,
                          Optional sh_TimeFrom As Object = Nothing,
                          Optional sh_TimeTo As Object = Nothing) As Object

        Dim params(5, 2) As Object

        params(0, 0) = "sh_RowID"
        params(1, 0) = "sh_OrganizationID"
        params(2, 0) = "sh_CreatedBy"
        params(3, 0) = "sh_LastUpdBy"
        params(4, 0) = "sh_TimeFrom"
        params(5, 0) = "sh_TimeTo"

        params(0, 1) = If(sh_RowID = Nothing, DBNull.Value, sh_RowID)
        params(1, 1) = orgztnID
        params(2, 1) = z_User
        params(3, 1) = z_User
        params(4, 1) = sh_TimeFrom
        params(5, 1) = sh_TimeTo

        INSUPD_shift =
            EXEC_INSUPD_PROCEDURE(params,
                                   "INSUPD_shift",
                                   "shiftRowID")

    End Function

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click
        RemoveHandler dgvshift.SelectionChanged, AddressOf dgvshift_SelectionChanged

        loadShift()

        AddHandler dgvshift.SelectionChanged, AddressOf dgvshift_SelectionChanged

        dgvshift_SelectionChanged(sender, e)

        haserrinputshift = 0
        listofEditRowShift.Clear()

    End Sub

    Private Sub dtpTimeFrom_Leave(sender As Object, e As EventArgs) 'Handles dtpTimeFrom.Leave

        colNameleave = "shf_TimeFrom"

        Dim thegetval = Format(CDate(dtpTimeFrom.Value), "hh:mm tt")

        Dim theretval = ""

        If dgvshift.RowCount = 1 Then
            If Trim(thegetval) <> "" Then
                dgvshift.Rows.Add()
                dgvleaveRowindx = dgvshift.RowCount - 2
                'dgvshift.Item("shf_TimeFrom", dgvleaveRowindx).Selected = True
            End If
        Else

            If dgvshift.CurrentRow.IsNewRow Then
                If Trim(thegetval) <> "" Then
                    dgvshift.Rows.Add()
                    dgvleaveRowindx = dgvshift.RowCount - 2
                    'dgvshift.Item("shf_TimeFrom", dgvleaveRowindx).Selected = True
                End If
            Else
                dgvleaveRowindx = dgvshift.CurrentRow.Index
            End If

        End If

        Dim dateobj As Object = thegetval.Replace(" ", ":")
        Dim ampm As String = Nothing

        If Trim(thegetval) <> "" Then
            Try

                If dateobj.ToString.Contains("A") Or
                    dateobj.ToString.Contains("P") Or
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If
                '    dateobj = getStrBetween(dateobj.ToString, "", " ")
                '    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm")
                '    dgvshift.Item("shf_TimeFrom", rowIndx).Value = valtime.ToShortTimeString
                'Else
                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")

                If ampm = Nothing Then
                    theretval = valtime.ToShortTimeString
                Else
                    theretval = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                End If
                'End If
                'valtime = DateTime.Parse(e.FormattedValue)
                'valtime = valtime.ToShortTimeString
                'Format(valtime, "hh:mm tt")
                haserrinputshift = 0

                dgvshift.Item("shf_TimeFrom", dgvleaveRowindx).ErrorText = Nothing
            Catch ex As Exception
                Try
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5))
                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")
                    'valtime = DateTime.Parse(e.FormattedValue)
                    'valtime = valtime.ToShortTimeString
                    theretval = valtime.ToShortTimeString
                    'Format(valtime, "hh:mm tt")

                    haserrinputshift = 0

                    dgvshift.Item("shf_TimeFrom", dgvleaveRowindx).ErrorText = Nothing
                Catch ex_1 As Exception
                    haserrinputshift = 1
                    dgvshift.Item("shf_TimeFrom", dgvleaveRowindx).ErrorText = "     Invalid time value"
                Finally
                    If Trim(thegetval) <> "" Then
                        dgvshift.Item("shf_TimeFrom", dgvleaveRowindx).Value = theretval
                        If haserrinputshift = 1 Then
                        Else
                            dtpTimeFrom.Value = Format(CDate(theretval), "1/1/2000 hh:mm tt")
                        End If

                        If theretval <> prev_elv_StartTime _
                        And dgvshift.Item("shf_RowID", dgvleaveRowindx).Value <> Nothing Then
                            listofEditRowShift.Add(dgvshift.Item("shf_RowID", dgvleaveRowindx).Value)
                        End If

                    End If
                End Try
            Finally
                If Trim(thegetval) <> "" Then
                    dgvshift.Item("shf_TimeFrom", dgvleaveRowindx).Value = theretval

                    If haserrinputshift = 1 Then
                    Else
                        dtpTimeFrom.Value = Format(CDate(theretval), "1/1/2000 hh:mm tt")
                    End If

                    If theretval <> prev_elv_StartTime _
                        And dgvshift.Item("shf_RowID", dgvleaveRowindx).Value <> Nothing Then
                        listofEditRowShift.Add(dgvshift.Item("shf_RowID", dgvleaveRowindx).Value)
                    End If

                    dgvshift.Item("shf_TimeFrom", dgvleaveRowindx).Selected = True
                    dgvshift_SelectionChanged(sender, e)
                End If

            End Try

        End If

    End Sub

    Private Sub mtxtTimeFrom_Leave(sender As Object, e As EventArgs) Handles mtxtTimeFrom.Leave

    End Sub

    Private Sub mtxtTimeFrom_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mtxtTimeFrom.MaskInputRejected

    End Sub

    Private Sub dtpTimeTo_Leave(sender As Object, e As EventArgs) 'Handles dtpTimeTo.Leave

        colNameleave = "shf_TimeTo"

        Dim thegetval = Format(CDate(dtpTimeTo.Value), "hh:mm tt")

        Dim theretval = ""

        If dgvshift.RowCount <> 0 Then

            If dgvshift.RowCount = 1 Then
                If Trim(thegetval) <> "" Then
                    dgvshift.Rows.Add()
                    dgvleaveRowindx = dgvshift.RowCount - 2
                    dgvshift.Item("shf_TimeTo", dgvleaveRowindx).Selected = True
                End If
            Else

                If dgvshift.CurrentRow.IsNewRow Then
                    If Trim(thegetval) <> "" Then
                        dgvshift.Rows.Add()
                        dgvleaveRowindx = dgvshift.RowCount - 2
                        dgvshift.Item("shf_TimeTo", dgvleaveRowindx).Selected = True
                    End If
                Else
                    dgvleaveRowindx = dgvshift.CurrentRow.Index
                End If

            End If

        End If

        Dim dateobj As Object = thegetval.Replace(" ", ":")
        Dim ampm As String = Nothing

        If Trim(thegetval) <> "" Then
            Try

                If dateobj.ToString.Contains("A") Or
                    dateobj.ToString.Contains("P") Or
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If
                '    dateobj = getStrBetween(dateobj.ToString, "", " ")
                '    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm")
                '    dgvshift.Item("shf_TimeTo", rowIndx).Value = valtime.ToShortTimeString
                'Else
                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")

                If ampm = Nothing Then
                    theretval = valtime.ToShortTimeString
                Else
                    theretval = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
                End If
                'End If
                'valtime = DateTime.Parse(e.FormattedValue)
                'valtime = valtime.ToShortTimeString
                'Format(valtime, "hh:mm tt")
                haserrinputshift = 0

                dgvshift.Item("shf_TimeTo", dgvleaveRowindx).ErrorText = Nothing
            Catch ex As Exception
                Try
                    dateobj = dateobj.ToString.Replace(":", " ")
                    dateobj = Trim(dateobj.ToString.Substring(0, 5))
                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")
                    'valtime = DateTime.Parse(e.FormattedValue)
                    'valtime = valtime.ToShortTimeString
                    theretval = valtime.ToShortTimeString
                    'Format(valtime, "hh:mm tt")

                    haserrinputshift = 0

                    dgvshift.Item("shf_TimeTo", dgvleaveRowindx).ErrorText = Nothing
                Catch ex_1 As Exception
                    haserrinputshift = 1
                    dgvshift.Item("shf_TimeTo", dgvleaveRowindx).ErrorText = "     Invalid time value"
                Finally
                    If Trim(thegetval) <> "" Then
                        dgvshift.Item("shf_TimeTo", dgvleaveRowindx).Value = theretval

                        'If haserrinputshift = 1 Then
                        'Else
                        dtpTimeTo.Value = Format(CDate(theretval), "1/1/2000 hh:mm tt")

                        'End If

                        If theretval <> prev_elv_StartTime _
                        And dgvshift.Item("shf_RowID", dgvleaveRowindx).Value <> Nothing Then
                            listofEditRowShift.Add(dgvshift.Item("shf_RowID", dgvleaveRowindx).Value)
                        End If

                    End If
                End Try
            Finally
                If Trim(thegetval) <> "" Then
                    dgvshift.Item("shf_TimeTo", dgvleaveRowindx).Value = theretval

                    dtpTimeTo.Value = Format(CDate(theretval), "1/1/2000 hh:mm tt")

                    If theretval <> prev_elv_StartTime _
                        And dgvshift.Item("shf_RowID", dgvleaveRowindx).Value <> Nothing Then
                        listofEditRowShift.Add(dgvshift.Item("shf_RowID", dgvleaveRowindx).Value)
                    End If

                    dgvshift.Item("shf_TimeFrom", dgvleaveRowindx).Selected = True
                    dgvshift_SelectionChanged(sender, e)
                End If

            End Try

        End If

    End Sub

    Private Sub mtxtTimeTo_Leave(sender As Object, e As EventArgs) Handles mtxtTimeTo.Leave

    End Sub

    Private Sub mtxtTimeTo_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mtxtTimeTo.MaskInputRejected

    End Sub

    Private Sub dgvshift_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvshift.CellContentClick

    End Sub

    Dim prev_elv_StartTime = Nothing

    Dim prev_elv_EndTime = Nothing

    Private Sub dgvshift_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvshift.SelectionChanged

        mtxtTimeFrom.Text = ""

        mtxtTimeTo.Text = ""

        If dgvshift.RowCount <> 1 Then
            With dgvshift.CurrentRow
                dgvleaveRowindx = .Index

                If .IsNewRow Then

                    prev_elv_StartTime = Nothing

                    prev_elv_EndTime = Nothing

                    mtxtTimeFrom.Text = ""

                    mtxtTimeTo.Text = ""
                Else

                    prev_elv_StartTime = .Cells("shf_TimeFrom").Value

                    prev_elv_EndTime = .Cells("shf_TimeFrom").Value

                    If .Cells("shf_TimeFrom").Value = Nothing Then

                        mtxtTimeFrom.Text = ""
                    Else

                        mtxtTimeFrom.Text = .Cells("shf_TimeFrom").Value 'getStrBetween(.Cells("shf_TimeFrom").Value.ToString, "", " ")

                        dtpTimeFrom.Value = "1/1/2000 " & mtxtTimeFrom.Text

                    End If

                    If .Cells("shf_TimeTo").Value = Nothing Then

                        mtxtTimeTo.Text = ""
                    Else

                        mtxtTimeTo.Text = .Cells("shf_TimeTo").Value 'getStrBetween(.Cells("shf_TimeTo").Value.ToString, "", " ")

                        dtpTimeTo.Value = "1/1/2000 " & mtxtTimeTo.Text

                    End If

                End If

            End With
        Else

        End If

    End Sub

    Dim colNameleave As String = Nothing

    Dim rowIndxleave As Integer = Nothing

    Dim haserrinputshift As SByte = 0

    Dim listofEditRowShift As New AutoCompleteStringCollection

    Private Sub dgvshift_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvshift.CellEndEdit

        colNameleave = dgvshift.Columns(e.ColumnIndex).Name
        rowIndxleave = e.RowIndex

        If dgvshift.Rows(rowIndxleave).IsNewRow Then

            haserrinputshift = 0

            dgvshift.Item(colNameleave, rowIndxleave).ErrorText = Nothing

            Exit Sub

        End If

        'And dgvshift.Item(colName, rowIndx).Value <> Nothing Then
        Dim dateobj As Object = Trim(dgvshift.Item(colNameleave, rowIndxleave).Value).Replace(" ", ":")

        'Dim colonindx As Integer = dateobj.ToString.IndexOf(":")

        'dateobj = dateobj.ToString.Remove(colonindx, 1)
        'MsgBox(dateobj.ToString) '.IndexOf(":")
        Dim ampm As String = Nothing

        Try

            If dateobj.ToString.Contains("A") Or
                dateobj.ToString.Contains("P") Or
                dateobj.ToString.Contains("M") Then

                ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))
                dateobj = dateobj.ToString.Replace(":", " ")
                dateobj = Trim(dateobj.ToString.Substring(0, 5)) 'dateobj.ToString.Substring(0, 4)
                dateobj = dateobj.ToString.Replace(" ", ":")

            End If
            '    dateobj = getStrBetween(dateobj.ToString, "", " ")
            '    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm")
            '    dgvshift.Item(colName, rowIndx).Value = valtime.ToShortTimeString
            'Else
            Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm tt")
            If ampm = Nothing Then
                dgvshift.Item(colNameleave, rowIndxleave).Value = valtime.ToShortTimeString
            Else
                dgvshift.Item(colNameleave, rowIndxleave).Value = Trim(valtime.ToShortTimeString.Substring(0, 5)) & ampm
            End If
            'End If
            'valtime = DateTime.Parse(e.FormattedValue)
            'valtime = valtime.ToShortTimeString
            'Format(valtime, "hh:mm tt")
            haserrinputshift = 0

            dgvshift.Item(colNameleave, rowIndxleave).ErrorText = Nothing
        Catch ex As Exception
            Try
                dateobj = dateobj.ToString.Replace(":", " ")
                dateobj = Trim(dateobj.ToString.Substring(0, 5))
                dateobj = dateobj.ToString.Replace(" ", ":")

                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm")
                'valtime = DateTime.Parse(e.FormattedValue)
                'valtime = valtime.ToShortTimeString
                dgvshift.Item(colNameleave, rowIndxleave).Value = valtime.ToShortTimeString
                'Format(valtime, "hh:mm tt")

                haserrinputshift = 0

                dgvshift.Item(colNameleave, rowIndxleave).ErrorText = Nothing
            Catch ex_1 As Exception
                haserrinputshift = 1
                dgvshift.Item(colNameleave, rowIndxleave).ErrorText = "     Invalid time value"
            End Try
        End Try

        listofEditRowShift.Add(dgvshift.Item("shf_RowID", rowIndxleave).Value)

    End Sub

    Function MilitTime(ByVal timeval As Object) As Object

        Dim retrnObj As Object

        retrnObj = New Object

        If timeval = Nothing Then
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

            End If

        End If

        Return retrnObj

    End Function

    Private Sub dtpTimeFrom_ValueChanged(sender As Object, e As EventArgs) Handles dtpTimeFrom.ValueChanged

    End Sub

    Private Sub dtpTimeTo_ValueChanged(sender As Object, e As EventArgs) Handles dtpTimeTo.ValueChanged

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        dgvshift.EndEdit(True)

        If dgvshift.CurrentRow.IsNewRow Then
            dgvshift.Focus()
        Else
            If dgvshift.CurrentRow.Cells("shf_RowID").Value = Nothing Then
            Else

                EmployeeShiftEntryForm.dtpTimeFrom.Value = "1/1/2000 " & dgvshift.CurrentRow.Cells("shf_TimeFrom").Value
                EmployeeShiftEntryForm.dtpTimeTo.Value = "1/1/2000 " & dgvshift.CurrentRow.Cells("shf_TimeTo").Value

                EmployeeShiftEntryForm.dutyShiftRowID = dgvshift.CurrentRow.Cells("shf_RowID").Value

                EmployeeShiftEntryForm.lblShiftID.Text = EmployeeShiftEntryForm.dutyShiftRowID

                Me.Close()

            End If

        End If

    End Sub

    Private Sub dgvshift_DoubleClick(sender As Object, e As EventArgs) Handles dgvshift.DoubleClick
        Button1_Click(sender, e)
    End Sub

    Private Sub dgvshift_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvshift.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()

        End If

    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click

        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

End Class