Public Class UserPrivilegeForm
    Dim IsNew As Integer = 0
    Private Sub fillUserPrev()
        If dgvPositionList.Rows.Count = 0 Then
        Else
            For Each drow As DataGridViewRow In dgvGeneral.Rows
                Dim dt As New DataTable
                dt = getDataTableForSQL("Select * from position_view pv inner join position p on pv.PositionID = p.RowID " & _
                                        "inner join `view` v on pv.ViewID = v.RowID " & _
                                        "Where pv.OrganizationID = " & z_OrganizationID & " And ViewName = '" & drow.Cells(0).Value & "' And p.PositionName = '" & dgvPositionList.CurrentRow.Cells(c_Position.Index).Value & "'")
                If dt.Rows.Count > 0 Then
                    With dt.Rows(0)
                        drow.Cells(1).Value = IIf(.Item("Creates") = "Y", True, False)
                        drow.Cells(2).Value = IIf(.Item("Updates") = "Y", True, False)
                        drow.Cells(3).Value = IIf(.Item("Deleting") = "Y", True, False)
                        drow.Cells(4).Value = IIf(.Item("ReadOnly") = "Y", True, False)
                    End With
                End If
            Next

            For Each drow As DataGridViewRow In dgvHRIS.Rows
                Dim dt As New DataTable
                dt = getDataTableForSQL("Select * from position_view pv inner join position p on pv.PositionID = p.RowID " & _
                                        "inner join `view` v on pv.ViewID = v.RowID " & _
                                        "Where pv.OrganizationID = " & z_OrganizationID & " And ViewName = '" & drow.Cells(0).Value & "' And p.PositionName = '" & dgvPositionList.CurrentRow.Cells(c_Position.Index).Value & "' ")
                If dt.Rows.Count > 0 Then
                    With dt.Rows(0)
                        drow.Cells(1).Value = IIf(.Item("Creates") = "Y", True, False)
                        drow.Cells(2).Value = IIf(.Item("Updates") = "Y", True, False)
                        drow.Cells(3).Value = IIf(.Item("Deleting") = "Y", True, False)
                        drow.Cells(4).Value = IIf(.Item("ReadOnly") = "Y", True, False)
                    End With
                End If
            Next
        End If

    End Sub
    Private Sub mainformtabs()
        With MainForm
            '


            dgvGeneral.Rows.Add(.hbtnUsers.Text)
            dgvGeneral.Rows.Add(.hbtnorganization.Text)
            dgvGeneral.Rows.Add(.hbtnListofval.Text)
            dgvGeneral.Rows.Add(.hbtnaudittrail.Text)
            dgvGeneral.Rows.Add(.hbtnChangePassword.Text)

            dgvHRIS.Rows.Add(.hbtnEducationbGround.Text)
            dgvHRIS.Rows.Add(.hbtnEmpSalary.Text)
            dgvHRIS.Rows.Add(.hbtnPrevEmployer.Text)
            dgvHRIS.Rows.Add(.hbtnDivision.Text)
        End With
    End Sub
    Private Sub fillPostionName()
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * From position Where OrganizationID = '" & z_OrganizationID & "'")
        dgvPositionList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvPositionList.Rows.Add()
            With drow
                dgvPositionList.Rows.Item(n).Cells(c_Position.Index).Value = .Item("PositionName").ToString
                dgvPositionList.Rows.Item(n).Cells(c_rowID.Index).Value = .Item("RowID").ToString

            End With
        Next
    End Sub

    Private Sub UserPreviligeForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            hintInfo.Dispose()

        Catch ex As Exception

        End Try

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

    End Sub
    Private Sub UserPreviligeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillPostionName()
        mainformtabs()
        fillUserPrev()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim postID As String = getStringItem("Select RowID From Position Where RowID = '" & dgvPositionList.CurrentRow.Cells(c_rowID.Index).Value & "' And OrganizationID = '" & z_OrganizationID & "'")
        Dim getPostID As Integer = Val(postID)
        Dim dt As New DataTable
        '======General========
        For Each drow As DataGridViewRow In dgvGeneral.Rows

            dt = getDataTableForSQL("Select * from `view` where viewname = '" & drow.Cells(0).Value & "' And OrganizationID = '" & z_OrganizationID & "'")
            If Not dt.Rows.Count > 0 Then
                DirectCommand("INSERT INTO `view` (viewname, OrganizationID) VALUES ('" & drow.Cells(0).Value & "', '" & z_OrganizationID & "')")
            End If


            dt = getDataTableForSQL("Select * from position_view pv inner join position p on pv.PositionID = p.RowID " & _
                                    "inner join `view` v on pv.ViewID = v.RowID " & _
                                    "Where pv.OrganizationID = '" & z_OrganizationID & "' And viewname = '" & drow.Cells(0).Value & "' And PositionName = '" & dgvPositionList.CurrentRow.Cells(c_Position.Index).Value & "'")

            If dt.Rows.Count > 0 Then
                Dim rowid As String = getStringItem("Select pv.RowID From position_view pv inner join position p on pv.PositionID = p.RowID " & _
                                                "inner join `view` v on pv.ViewID = v.RowID " & _
                                                "where p.PositionName = '" & dgvPositionList.CurrentRow.Cells(c_Position.Index).Value & "' And p.OrganizationID = " & z_OrganizationID & " And v.ViewName = '" & drow.Cells(0).Value & "'")

                Dim ID As Integer = Val(rowid)
                Dim add, modify, delete, read As String
                Dim datenow As String = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
                add = IIf(drow.Cells(1).FormattedValue = True, "Y", "N")
                modify = IIf(drow.Cells(2).FormattedValue = True, "Y", "N")
                delete = IIf(drow.Cells(3).FormattedValue = True, "Y", "N")
                read = IIf(drow.Cells(4).FormattedValue = True, "Y", "N")

                DirectCommand("UPDATE position_view SET creates = '" & add & "', ReadOnly = '" & read & "', " & _
                              "Updates = '" & modify & "', Deleting = '" & delete & "', LastUpd = '" & datenow & "', LastUpdBy = '" & z_User & "' Where RowID = " & ID & "")

            Else


                Dim viewid As String = getStringItem("Select RowID From `view` where viewname = '" & drow.Cells(0).Value & "' And OrganizationID = " & z_OrganizationID & "")
                Dim getviewid As Integer = Val(viewid)
                Dim add, modify, delete, read As String
                Dim datenow As String = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
                add = IIf(drow.Cells(1).FormattedValue = True, "Y", "N")
                modify = IIf(drow.Cells(2).FormattedValue = True, "Y", "N")
                delete = IIf(drow.Cells(3).FormattedValue = True, "Y", "N")
                read = IIf(drow.Cells(4).FormattedValue = True, "Y", "N")

                I_PositionViewproc(getPostID, getviewid, add, z_OrganizationID, read, modify, delete, datenow, z_User, datenow, z_User)

            End If
        Next
        '==========General=========


        '==========HRIS============
        For Each drow1 As DataGridViewRow In dgvHRIS.Rows

            dt = getDataTableForSQL("Select * from `view` where viewname = '" & drow1.Cells(0).Value & "' And OrganizationID = '" & z_OrganizationID & "'")
            If Not dt.Rows.Count > 0 Then
                DirectCommand("INSERT INTO `view` (viewname, OrganizationID) VALUES ('" & drow1.Cells(0).Value & "', '" & z_OrganizationID & "')")
            End If


            dt = getDataTableForSQL("Select * from position_view pv inner join position p on pv.PositionID = p.RowID " & _
                                    "inner join `view` v on pv.ViewID = v.RowID " & _
                                    "Where pv.OrganizationID = '" & z_OrganizationID & "' And viewname = '" & drow1.Cells(0).Value & "' And PositionName = '" & dgvPositionList.CurrentRow.Cells(c_Position.Index).Value & "'")

            If dt.Rows.Count > 0 Then
                Dim rowid As String = getStringItem("Select pv.RowID From position_view pv inner join position p on pv.PositionID = p.RowID " & _
                                 "inner join `view` v on pv.ViewID = v.RowID " & _
                                 "where p.PositionName = '" & dgvPositionList.CurrentRow.Cells(c_Position.Index).Value & "' And p.OrganizationID = " & z_OrganizationID & " And v.ViewName = '" & drow1.Cells(0).Value & "'")

                Dim ID As Integer = Val(rowid)
                Dim add, modify, delete, read As String
                Dim datenow As String = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
                add = IIf(drow1.Cells(1).FormattedValue = True, "Y", "N")
                modify = IIf(drow1.Cells(2).FormattedValue = True, "Y", "N")
                delete = IIf(drow1.Cells(3).FormattedValue = True, "Y", "N")
                read = IIf(drow1.Cells(4).FormattedValue = True, "Y", "N")

                DirectCommand("UPDATE position_view SET creates = '" & add & "', ReadOnly = '" & read & "', " & _
                              "Updates = '" & modify & "', Deleting = '" & delete & "', LastUpd = '" & datenow & "', LastUpdBy = '" & z_User & "' Where RowID = " & ID & "")


            Else


                Dim viewid As String = getStringItem("Select RowID From `view` where viewname = '" & drow1.Cells(0).Value & "' And OrganizationID = " & z_OrganizationID & "")
                Dim getviewid As Integer = Val(viewid)
                Dim add, modify, delete, read As Boolean
                Dim datenow As String = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
                add = IIf(drow1.Cells(1).FormattedValue = True, "Y", "N")
                modify = IIf(drow1.Cells(2).FormattedValue = True, "Y", "N")
                delete = IIf(drow1.Cells(3).FormattedValue = True, "Y", "N")
                read = IIf(drow1.Cells(4).FormattedValue = True, "Y", "N")

                I_PositionViewproc(getPostID, getviewid, add, z_OrganizationID, read, modify, delete, datenow, z_User, datenow, z_User)

            End If
        Next
        '==========HRIS============
        fillUserPrev()

        myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For Each TG As DataGridViewRow In dgvGeneral.Rows
            Dim Checked As Boolean = True
            TG.Cells(1).Value = Checked
            TG.Cells(2).Value = Checked
            TG.Cells(3).Value = Checked
            TG.Cells(4).Value = Checked
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For Each TG As DataGridViewRow In dgvGeneral.Rows
            Dim Checked As Boolean = False
            TG.Cells(1).Value = Checked
            TG.Cells(2).Value = Checked
            TG.Cells(3).Value = Checked
            TG.Cells(4).Value = Checked
        Next
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        For Each TG As DataGridViewRow In dgvHRIS.Rows
            Dim Checked As Boolean = False
            TG.Cells(1).Value = Checked
            TG.Cells(2).Value = Checked
            TG.Cells(3).Value = Checked
            TG.Cells(4).Value = Checked
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For Each TG As DataGridViewRow In dgvHRIS.Rows
            Dim Checked As Boolean = True
            TG.Cells(1).Value = Checked
            TG.Cells(2).Value = Checked
            TG.Cells(3).Value = Checked
            TG.Cells(4).Value = Checked
        Next
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        IsNew = 1
    End Sub

    Private Sub dgvPositionList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPositionList.CellClick
        Try
            fillUserPrev()

        Catch ex As Exception

        End Try
    End Sub
End Class