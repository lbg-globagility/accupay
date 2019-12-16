Public Class Pay_Frequency_Form
    Dim ifNew As Integer = 0

    Private Sub initializepayrollList()
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * From `payfrequency`")

        dgvPayrollList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvPayrollList.Rows.Add()
            With drow
                dgvPayrollList.Rows.Item(n).Cells(c_PayFrequency.Index).Value = .Item("PayFrequencyType").ToString
                dgvPayrollList.Rows.Item(n).Cells(c_StartDate.Index).Value = CDate(.Item("PayFrequencyStartDate")).ToString("MM/dd/yyyy")
                dgvPayrollList.Rows.Item(n).Cells(c_RowID.Index).Value = .Item("RowID").ToString
            End With
        Next

    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        ifNew = 1
        grpPayroll.Enabled = True
        btnSave.Enabled = True
        btnNew.Enabled = False
        btnDelete.Enabled = False

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If ifNew = 1 Then
            If cmbPayType.Text = Nothing Then
                MsgBox("Please Select Payroll type.", MsgBoxStyle.Exclamation, "Required Field")
            Else
                SP_payfrequency(1, 1, z_datetime, z_datetime, cmbPayType.Text, dtpStartDate.Value.ToString("yyyy-MM-dd"))
                MsgBox("Save Successfully", MsgBoxStyle.Information, "Saved!")
                initializepayrollList()
                btnSave.Enabled = False
                btnNew.Enabled = True
                btnDelete.Enabled = False
                grpPayroll.Enabled = False

            End If
        Else
            DirectCommand("UPDATE payfrequency SET `PayFrequency` = '" & cmbPayType.Text & "', PayFrequencyStartDate = '" & dtpStartDate.Value.ToString("yyyy-MM-dd") & "' WHere RowID = '" & dgvPayrollList.CurrentRow.Cells(c_RowID.Index).Value & "'")
            MsgBox("Save Successfully", MsgBoxStyle.Information, "Saved!")
            initializepayrollList()
            btnSave.Enabled = False
            btnNew.Enabled = True
            btnDelete.Enabled = False
            grpPayroll.Enabled = False
        End If
    End Sub

    Private Sub Pay_Frequency_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initializepayrollList()
    End Sub

    Private Sub dgvPayrollList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPayrollList.CellClick
        Try
            btnSave.Enabled = True
            btnDelete.Enabled = True
        Catch ex As Exception

        End Try
    End Sub
End Class