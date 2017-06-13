Public Class CrysRepForm

    Private Sub CrysRepForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        Dim result = MessageBox.Show("Do you want to close " & Me.Text & " ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
        If result = DialogResult.No Then
            e.Cancel = True
        Else
            crysrepvwr.ReportSource.Dispose()
        End If

    End Sub

    'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

    '    If keyData = (Keys.RControlKey AndAlso keyData = Keys.W) Then

    '        Me.Close()

    '        Return True

    '    Else

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    End If

    'End Function

    Private Sub crysrepvwr_KeyDown(sender As Object, e As KeyEventArgs) Handles crysrepvwr.KeyDown, Me.KeyDown

        If e.Control AndAlso e.KeyCode = Keys.W Then
            Me.Close()
        ElseIf e.Control AndAlso e.KeyCode = Keys.P Then
            crysrepvwr.PrintReport()

        End If

    End Sub

    Private Sub CrysRepForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

End Class