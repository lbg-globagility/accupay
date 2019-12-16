Public Class WaitForm
    Public IsAllowClose As Boolean = False
    Private Sub WaitForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Not IsAllowClose Then
            e.Cancel = True
        End If
    End Sub
End Class