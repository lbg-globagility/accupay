

Public Class LoanScheduleForm

    Dim IsNew As Integer = 0
    Sub filldedsched()
        cmbdedsched.Items.Clear()
        fillCombobox("Select DisplayValue From ListOFVal where Type = 'Deduction Schedule'", cmbdedsched)
    End Sub
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
    Private Sub fillloadsched()
        If dgvEmpList.Rows.Count = 0 Then
            Exit Sub
        End If
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * from employeeloanschedule Where EmployeeID = '" & dgvEmpList.CurrentRow.Cells(c_ID.Index).Value & "' And OrganizationID = '" & z_OrganizationID & "'")
        dgvLoanList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvLoanList.Rows.Add()
            With drow

                dgvLoanList.Rows.Item(n).Cells(c_loanno.Index).Value = .Item("LoanNumber").ToString
                dgvLoanList.Rows.Item(n).Cells(c_totloanamt.Index).Value = FormatNumber(.Item("totalloanAmount").ToString, 2)
                dgvLoanList.Rows.Item(n).Cells(c_totballeft.Index).Value = FormatNumber(.Item("TotalBalanceLeft").ToString, 2)
                dgvLoanList.Rows.Item(n).Cells(c_dedamt.Index).Value = FormatNumber(.Item("DeductionAmount").ToString, 2)
                dgvLoanList.Rows.Item(n).Cells(c_DedPercent.Index).Value = FormatNumber(.Item("DeductionPercentage").ToString)
                dgvLoanList.Rows.Item(n).Cells(c_dedsched.Index).Value = .Item("DeductionSchedule").ToString
                dgvLoanList.Rows.Item(n).Cells(c_noofpayperiod.Index).Value = .Item("Noofpayperiod").ToString
                dgvLoanList.Rows.Item(n).Cells(c_remarks.Index).Value = .Item("Comments").ToString
                dgvLoanList.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("RowID").ToString
                dgvLoanList.Rows.Item(n).Cells(c_status.Index).Value = .Item("Status").ToString
            End With
        Next
    End Sub
    Private Sub fillloadschedselected()
        If dgvLoanList.Rows.Count = 0 Then
            Exit Sub
        End If
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * from employeeloanschedule Where RowID = '" & dgvLoanList.CurrentRow.Cells(c_rowid.Index).Value & "' And OrganizationID = '" & z_OrganizationID & "'")
        cleartextbox()
        For Each drow As DataRow In dt.Rows
            With drow
                Dim empid As Integer = .Item("EmployeeID").ToString
                Dim eID As String = getStringItem("Select EmployeeID From Employee Where RowID = '" & empid & "'")
                Dim geteID As Integer = Val(eID)
                txtempid.Text = geteID
                txtloannumber.Text = .Item("LoanNumber").ToString
                txtloanamt.Text = FormatNumber(.Item("totalloanAmount").ToString, 2)
                txtbal.Text = FormatNumber(.Item("TotalBalanceLeft").ToString, 2)
                txtdedamt.Text = FormatNumber(.Item("DeductionAmount").ToString, 2)
                txtdedpercent.Text = FormatNumber(.Item("DeductionPercentage").ToString)
                cmbdedsched.Text = .Item("DeductionSchedule").ToString
                txtnoofpayper.Text = .Item("Noofpayperiod").ToString
                txtremarks.Text = .Item("Comments").ToString
                datefrom.Value = CDate(.Item("DedEffectiveDateFrom")).ToString("MM/dd/yyyy")
                dateto.Value = CDate(.Item("DedEffectiveDateTo")).ToString("MM/dd/yyyy")
                cmbStatus.Text = .Item("Status").ToString

            End With
        Next
    End Sub
    Private Sub cleartextbox()
        txtloanamt.Clear()
        txtbal.Clear()
        txtdedamt.Clear()
        txtdedpercent.Clear()
        cmbdedsched.SelectedIndex = -1
        txtnoofpayper.Clear()
        cmbStatus.SelectedIndex = -1
        datefrom.Value = Date.Now
        dateto.Value = Date.Now
        txtremarks.Clear()
    End Sub
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        btnNew.Enabled = False
        IsNew = 1
        Dim loanno As String = getStringItem("Select MAX(LoanNumber) from employeeloanschedule where OrganizationID = '" & z_OrganizationID & "' And EmployeeID = '" & dgvEmpList.CurrentRow.Cells(c_ID.Index).Value & "'")
        Dim getloanno As Integer = Val(loanno) + 1
        txtloannumber.Text = getloanno
        txtloanamt.Clear()
        txtbal.Text = "0.00"
        txtdedamt.Clear()
        txtdedpercent.Clear()
        cmbdedsched.SelectedIndex = -1
        txtnoofpayper.Clear()
        cmbStatus.Text = "In Progress"
        datefrom.Value = Date.Now
        dateto.Value = Date.Now
        txtremarks.Clear()
        dgvEmpList.Enabled = False
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If txtloannumber.Text = "" And txtbal.Text = "" And txtdedamt.Text = "" And txtdedpercent.Text = "" _
            And cmbdedsched.Text = "" And txtnoofpayper.Text = "" And cmbStatus.Text = "" Then
            If SetWarningIfEmpty(txtloannumber) And SetWarningIfEmpty(txtbal) And SetWarningIfEmpty(txtdedamt) And SetWarningIfEmpty(txtdedpercent) _
                And SetWarningIfEmpty(cmbdedsched) And SetWarningIfEmpty(txtnoofpayper) And SetWarningIfEmpty(cmbStatus) Then
                Exit Sub
            End If
            Exit Sub
        End If
        Dim empid As Integer = dgvEmpList.CurrentRow.Cells(c_ID.Index).Value
        If IsNew = 1 Then

            SP_LoadSchedule(z_User, z_User, z_datetime, z_datetime, Val(txtloannumber.Text), datefrom.Value.ToString("yyyy-MM-dd"), dateto.Value.ToString("yyyy-MM-dd"), _
                            z_OrganizationID, Val(empid), CDec(txtloanamt.Text), cmbdedsched.Text, CDec(txtbal.Text), CDec(txtdedamt.Text), _
                            CDec(txtnoofpayper.Text), txtremarks.Text, cmbStatus.Text, CDec(txtdedpercent.Text))
            fillloadsched()
            fillloadschedselected()
            myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
        Else
            SP_UpdateLoadSchedule(z_User, z_datetime, Val(txtloannumber.Text), datefrom.Value.ToString("yyyy-MM-dd"), dateto.Value.ToString("yyyy-MM-dd"), _
                                 CDec(txtloanamt.Text), cmbdedsched.Text, CDec(txtdedamt.Text), _
                                 CDec(txtnoofpayper.Text), txtremarks.Text, cmbStatus.Text, CDec(txtdedpercent.Text), dgvLoanList.CurrentRow.Cells(c_rowid.Index).Value)
            fillloadsched()
            fillloadschedselected()
            myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
        End If


        IsNew = 0
        dgvEmpList.Enabled = True
        btnNew.Enabled = True
    End Sub

    Private Sub LoanScheduleForm_Load(sender As Object, e As EventArgs) Handles Me.Load


        filldedsched()
        fillemplyeelist()
        fillloadsched()
        fillloadschedselected()
    End Sub

    Private Sub dgvEmpList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmpList.CellClick

        fillloadsched()
        fillloadschedselected()
    End Sub

    Private Sub dgvLoanList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLoanList.CellClick
        fillloadschedselected()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub txtdedamt_TextChanged(sender As Object, e As EventArgs) Handles txtdedamt.TextChanged
        Dim loanamt, dedamt, totalded As Double
        If Double.TryParse(txtdedamt.Text, dedamt) Then
            dedamt = CDec(txtdedamt.Text)
            loanamt = CDec(txtloanamt.Text)
            totalded = loanamt / dedamt
        End If
        txtnoofpayper.Text = FormatNumber(totalded, 2)

    End Sub

    Private Sub lblAdd_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAdd.LinkClicked
        AddListOfValueForm.lblName.Text = "Deduction Schedule"
        AddListOfValueForm.ShowDialog()

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        cleartextbox()
        IsNew = 0
        dgvEmpList.Enabled = True
        fillloadsched()
        fillloadschedselected()
    End Sub

    Private Sub txtloanamt_TextChanged(sender As Object, e As EventArgs) Handles txtloanamt.TextChanged
        txtdedamt.Clear()

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

    End Sub

    Private Sub dgvLoanList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLoanList.CellContentClick

    End Sub

    Private Sub txtdedpercent_TextChanged(sender As Object, e As EventArgs) Handles txtdedpercent.TextChanged

    End Sub
End Class