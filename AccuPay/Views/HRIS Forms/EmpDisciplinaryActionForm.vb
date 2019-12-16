Public Class EmpDisciplinaryActionForm
    Dim IsNew As Integer
    Dim RowID As Integer
    Private Sub fillemplyeelist()
        'If dgvEmplist.Rows.Count = 0 Then
        'ElseCOALESCE(StreetAddress1,' ')
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select concat(COALESCE(Lastname, ' '),' ', COALESCE(Firstname, ' '), ' ', COALESCE(MiddleName, ' ')) as name, EmployeeID, RowID from employee where organizationID = '" & z_OrganizationID & "'")

        dgvEmpList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvEmpList.Rows.Add()
            With drow

                dgvEmpList.Rows.Item(n).Cells(c_EmployeeID.Index).Value = .Item("EmployeeID").ToString
                'txtempid.Text = .Item("EmployeeID").ToString
                dgvEmpList.Rows.Item(n).Cells(c_EmployeeName.Index).Value = .Item("Name").ToString
                dgvEmpList.Rows.Item(n).Cells(c_ID.Index).Value = .Item("RowID").ToString
            End With
        Next
        'End If

    End Sub

    Private Sub fillemplyeelistselected()
        'If dgvEmplist.Rows.Count = 0 Then
        'ElseCOALESCE(StreetAddress1,' ')
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select concat(COALESCE(Lastname, ' '),' ', COALESCE(Firstname, ' '), ' ', COALESCE(MiddleName, ' ')) as name, EmployeeID, RowID from employee where organizationID = '" & z_OrganizationID & "' And RowID = '" & dgvEmpList.CurrentRow.Cells(c_ID.Index).Value & "'")

        For Each drow As DataRow In dt.Rows

            With drow

                txtEmpID.Text = .Item("EmployeeID").ToString
                'txtempid.Text = .Item("EmployeeID").ToString
                txtEmpName.Text = .Item("Name").ToString
                rowid = .Item("RowID").ToString
            End With
        Next
        'End If

    End Sub
    Private Sub controltrue()
        cmbFinding.Enabled = True
        txtAction.Enabled = True
        txtComments.Enabled = True
        txtDesc.Enabled = True
        dtpFrom.Enabled = True
        dtpTo.Enabled = True
    End Sub
    Private Sub controlclear()
        cmbFinding.SelectedIndex = -1
        txtAction.Clear()
        txtComments.Clear()
        txtDesc.Clear()
        dtpFrom.Value = Date.Now
        dtpTo.Value = Date.Now
    End Sub

    Sub fillfindingcombobox()
        Dim strQuery As String = "select partno from product Where OrganizationID = '" & z_OrganizationID & "'"
        cmbFinding.Items.Clear()
        cmbFinding.Items.Add("-Please select one-")
        cmbFinding.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbFinding.SelectedIndex = 0
    End Sub
    Private Sub fillempdisciplinary()
        If Not dgvEmpList.Rows.Count = 0 Then
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * From employeedisciplinaryaction ed inner join product p on ed.FindingID = p.RowID " & _
                                    "Where ed.OrganizationID = '" & z_OrganizationID & "' And ed.EmployeeID = '" & RowID & "' Order by ed.RowID DESC")
            dgvDisciplinaryList.Rows.Clear()
            If dt.Rows.Count > 0 Then
                For Each drow As DataRow In dt.Rows
                    Dim n As Integer = dgvDisciplinaryList.Rows.Add()
                    With drow

                        dgvDisciplinaryList.Rows.Item(n).Cells(c_action.Index).Value = .Item("Action").ToString
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_datefrom.Index).Value = CDate(.Item("DateFrom")).ToString("MM/dd/yyyy")
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_dateto.Index).Value = CDate(.Item("DateTo")).ToString("MM/dd/yyyy")
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_FindingName.Index).Value = .Item("FindingDescription").ToString
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_comment.Index).Value = .Item("Comments").ToString
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_desc.Index).Value = .Item("PartNo").ToString
                        dgvDisciplinaryList.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("RowID").ToString
                    End With
                Next
            End If
        End If
        

    End Sub
    Private Function fillempdisciplinaryselected(ByVal dID As Integer) As Boolean

        If Not dgvDisciplinaryList.Rows.Count = 0 Then
            Dim dt As New DataTable

            dt = getDataTableForSQL("Select * From employeedisciplinaryaction ed inner join product p on ed.FindingID = p.RowID " & _
                                    "Where ed.OrganizationID = '" & z_OrganizationID & "' And ed.RowID = '" & dID & "'")

            If dt.Rows.Count > 0 Then
                For Each drow As DataRow In dt.Rows

                    With drow

                        txtAction.Text = .Item("Action").ToString
                        dtpFrom.Text = CDate(.Item("DateFrom")).ToString("MM/dd/yyyy")
                        dtpTo.Text = CDate(.Item("DateTo")).ToString("MM/dd/yyyy")
                        txtDesc.Text = .Item("FindingDescription").ToString
                        txtComments.Text = .Item("Comments").ToString
                        cmbFinding.Text = .Item("PartNo").ToString
                    End With
                Next
            End If
        End If
        Return True

    End Function
    Private Sub lblAddFindingname_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddFindingname.LinkClicked
        FindingForm.ShowDialog()

    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        IsNew = 1
        btnNew.Enabled = False
        dgvEmpList.Enabled = False
        controltrue()
        controlclear()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If IsNew = 1 Then


            Dim fID As String = getStringItem("Select RowID From product where PartNo = '" & cmbFinding.Text & "' And organizationID = '" & z_OrganizationID & "'")
            Dim getfID As Integer = Val(fID)

            sp_employeedisciplinaryaction(z_datetime, z_User, z_datetime, z_OrganizationID, z_User, txtAction.Text, txtComments.Text, _
                                          txtDesc.Text, getfID, RowID, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd"))

            dgvEmpList.Enabled = True
            fillempdisciplinary()
            fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
            myBalloon("Successfully Save", "Saving...", lblSaveMsg, , -100)
            btnNew.Enabled = True
            IsNew = 0
        Else
            Dim fID As String = getStringItem("Select RowID From product where PartNo = '" & cmbFinding.Text & "' And organizationID = '" & z_OrganizationID & "'")
            Dim getfID As Integer = Val(fID)
            DirectCommand("UPDATE employeedisciplinaryaction SET Action = '" & txtAction.Text & "', DateFrom = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' " & _
                          ", DateTO = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "', FindingDescription = '" & txtDesc.Text & "', Comments = '" & txtComments.Text & "', " & _
                          "FindingID = '" & getfID & "' Where RowID = '" & dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value & "'")
            fillempdisciplinary()
            fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
            myBalloon("Successfully Save", "Saving...", lblSaveMsg, , -100)
            IsNew = 0
        End If
    End Sub

    Private Sub EmpDisciplinaryActionForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        fillfindingcombobox()
        fillemplyeelist()
        fillemplyeelistselected()
        fillempdisciplinary()
        If Not dgvDisciplinaryList.Rows.Count = 0 Then
            fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
        End If


    End Sub

    Private Sub dgvEmpList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmpList.CellClick
        Try
            fillemplyeelistselected()

            fillempdisciplinary()
            If Not dgvDisciplinaryList.Rows.Count = 0 Then
                fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub dgvDisciplinaryList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDisciplinaryList.CellClick
        Try
            If Not dgvDisciplinaryList.Rows.Count = 0 Then
                fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
            End If

            controltrue()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            controlclear()

            If Not dgvDisciplinaryList.Rows.Count = 0 Then
                fillempdisciplinaryselected(dgvDisciplinaryList.CurrentRow.Cells(c_rowid.Index).Value)
            End If

        Catch ex As Exception

        End Try
    End Sub

End Class