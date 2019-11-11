Public Class EmployeeListForm
    Private Sub fillemplist()
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * From employee")
        dgvemplist.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvemplist.Rows.Add()
            With drow
                dgvemplist.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("RowID").ToString
                dgvemplist.Rows.Item(n).Cells(c_EmployeeID.Index).Value = .Item("EmployeeID").ToString
                dgvemplist.Rows.Item(n).Cells(c_fname.Index).Value = .Item("Firstname").ToString
                dgvemplist.Rows.Item(n).Cells(c_mname.Index).Value = .Item("Middlename").ToString
                dgvemplist.Rows.Item(n).Cells(c_lname.Index).Value = .Item("Lastname").ToString
                dgvemplist.Rows.Item(n).Cells(c_jobtitle.Index).Value = .Item("JobTitle").ToString
                dgvemplist.Rows.Item(n).Cells(c_positonid.Index).Value = .Item("PositionID").ToString
                dgvemplist.Rows.Item(n).Cells(c_hphone.Index).Value = .Item("HomePhone").ToString
                dgvemplist.Rows.Item(n).Cells(c_mphone.Index).Value = .Item("mobilephone").ToString
                dgvemplist.Rows.Item(n).Cells(c_wphone.Index).Value = .Item("WorkPhone").ToString
                dgvemplist.Rows.Item(n).Cells(c_bday.Index).Value = CDate(.Item("Birthdate")).ToString("MMM dd, yyyy")
                dgvemplist.Rows.Item(n).Cells(c_empStat.Index).Value = .Item("EmploymentStatus").ToString
                dgvemplist.Rows.Item(n).Cells(c_tinno.Index).Value = .Item("TINNO").ToString
                dgvemplist.Rows.Item(n).Cells(c_sssno.Index).Value = .Item("SSSNO").ToString
                dgvemplist.Rows.Item(n).Cells(c_hdmfno.Index).Value = .Item("HDMFNO").ToString
                dgvemplist.Rows.Item(n).Cells(c_philhealth.Index).Value = .Item("PHILHEALTHNO").ToString
            End With
        Next

    End Sub
    Private Sub EmployeeListForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillemplist()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        'Try
        '    em.txtEmpID.Text = dgvemplist.CurrentRow.Cells(c_EmployeeID.Index).Value
        '    Me.Close()
        'Catch ex As Exception

        'End Try
      
    End Sub

    Private Sub dgvemplist_DoubleClick(sender As Object, e As EventArgs) Handles dgvemplist.DoubleClick
        'Try
        '    EmployeeSalaryForm.txtEmpID.Text = dgvemplist.CurrentRow.Cells(c_EmployeeID.Index).Value
        '    Me.Close()
        'Catch ex As Exception

        'End Try

    End Sub
End Class