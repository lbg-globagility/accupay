Public Class FilingStatusForm
    Dim isNew As Integer = 0
    Private Sub initializeFilingStatus()
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * From filingStatus")

        dgvfilingstatus.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvfilingstatus.Rows.Add()
            With drow
                dgvfilingstatus.Rows.Item(n).Cells(c_FilingStatus.Index).Value = .Item("filingStatus").ToString
                dgvfilingstatus.Rows.Item(n).Cells(c_MaritalStatus.Index).Value = .Item("MaritalStatus").ToString
                dgvfilingstatus.Rows.Item(n).Cells(c_RowID.Index).Value = .Item("RowID").ToString
            End With
        Next
    End Sub
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        btnSave.Enabled = True
        btnDelete.Enabled = False
        btnNew.Enabled = False
        grpfilingdetails.Enabled = True
        txtFillingStatus.Clear()

    End Sub
    '  Public Function SP_FilingStatus(ByVal Createdby As Integer, _
    'ByVal LastUpdby As Integer, _
    'ByVal Created As DateTime, _
    'ByVal LastUpd As DateTime, _
    'ByVal filingstatus As String, _
    'ByVal maritalstatus As String, _
    'ByVal dependent As Integer) As Boolean
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If txtFillingStatus.Text = "" Or cmbStatus.Text = "" Then
            MsgBox("Please complete all fileds.", MsgBoxStyle.Exclamation, "Empty required fields")

        Else
            SP_FilingStatus(1, 1, z_datetime, z_datetime, txtFillingStatus.Text, cmbStatus.Text, txtDependant.Text)
            MsgBox("Save Successfully!", MsgBoxStyle.Information, "Saved.")
        End If
    End Sub


End Class