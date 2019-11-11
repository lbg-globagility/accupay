Public Class FindingForm
    Dim IsNew As Integer
    Dim rowid As Integer
    Dim pID As Integer

    Private Sub fillfinding()
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * From product Where OrganizationID = '" & z_OrganizationID & _
                                "' AND CategoryID='" & Employee.categDiscipID & "'" & _
                                " Order By RowID DESC")
        dgvFindingsList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvFindingsList.Rows.Add()
            With drow
                dgvFindingsList.Rows.Item(n).Cells(c_findingdesc.Index).Value = .Item("Description").ToString
                dgvFindingsList.Rows.Item(n).Cells(c_findingname.Index).Value = .Item("partno").ToString
                dgvFindingsList.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("ROwID").ToString
                rowid = .Item("ROwID").ToString
            End With
        Next
    End Sub

    Private Sub fillfindingselected()
        Try
            If Not dgvFindingsList.Rows.Count = 0 Then
                Dim dt As New DataTable
                dt = getDataTableForSQL("Select * From product Where OrganizationID = '" & z_OrganizationID & "' And RowID = '" & dgvFindingsList.CurrentRow.Cells(c_rowid.Index).Value & "'")

                For Each drow As DataRow In dt.Rows
                    With drow
                        txtdesc.Text = .Item("Description").ToString
                        txtname.Text = .Item("partno").ToString
                        pID = .Item("ROwID").ToString
                    End With
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        
    End Sub
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        IsNew = 1
        txtdesc.Clear()
        txtname.Clear()
        txtdesc.Enabled = True
        txtname.Enabled = True
        dgvFindingsList.Enabled = False
        btnNew.Enabled = False
        txtname.Focus()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If IsNew = 1 Then

            sp_finding(z_datetime, z_User, z_datetime, z_OrganizationID, z_User, txtname.Text, txtdesc.Text, Employee.categDiscipID)
            fillfinding()
            fillfindingselected()
            myBalloon("Successfully Save", "Saving..", lblSaveMsg, , -100)
            EmpDisciplinaryActionForm.fillfindingcombobox()
            btnNew.Enabled = True
            dgvFindingsList.Enabled = True
            EmpDisciplinaryActionForm.cmbFinding.Text = txtname.Text
            Me.Hide()

        Else
            DirectCommand("UPDATE product SET PartNo = '" & txtname.Text & "', Description = '" & txtdesc.Text & "', lastupd = '" & z_datetime & "', lastupdby = '" & z_User & "' where RowID = '" & pID & "'")
            fillfinding()
            myBalloon("Successfully Save", "Saving..", lblSaveMsg, , -100)
            EmpDisciplinaryActionForm.fillfindingcombobox()
            EmpDisciplinaryActionForm.cmbFinding.Text = txtname.Text
            Me.Hide()
        End If
    End Sub

    Private Sub FindingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillfinding()
        fillfindingselected()

    End Sub

    Private Sub dgvFindingsList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFindingsList.CellClick
        Try
            fillfindingselected()
            txtname.Enabled = True
            txtdesc.Enabled = True
            btnDelete.Enabled = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            fillfindingselected()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgvFindingsList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFindingsList.CellContentClick

    End Sub

    Private Sub btnAudittrail_Click(sender As Object, e As EventArgs) Handles btnAudittrail.Click

    End Sub
End Class