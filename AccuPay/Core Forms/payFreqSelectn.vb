Public Class payFreqSelectn

    Private Sub payFreqSelectn_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim dtpayfreq As New DataTable

        dtpayfreq = retAsDatTbl("SELECT FALSE 'CheckBox',PayFrequencyType,RowID FROM payfrequency;")

        For Each drow As DataRow In dtpayfreq.Rows

            dgvpayfreq.Rows.Add(CBool(drow("CheckBox")), _
                                CStr(drow("PayFrequencyType")), _
                                CInt(drow("RowID")))

        Next

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        dgvpayfreq.EndEdit(True)

        Me.DialogResult = Windows.Forms.DialogResult.OK

        Me.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Me.Close()

    End Sub

    Private Sub dgvpayfreq_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpayfreq.CellContentClick

    End Sub

    Private Sub dgvpayfreq_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpayfreq.CellEndEdit

    End Sub

    'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

    '    If keyData = Keys.Escape Then

    '        Button2_Click(Button2, New EventArgs)

    '        Return True

    '    Else

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    End If

    'End Function

End Class