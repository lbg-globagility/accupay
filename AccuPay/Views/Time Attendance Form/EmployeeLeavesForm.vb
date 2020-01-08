Public Class EmployeeLeavesForm

    Private Sub EmployeeLeavesForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
        InfoBalloon(, , FormTitleLabel, , , 1)
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Async Sub ImportToolStripButton_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click
        Dim importForm As New ImportLeaveForm()
        If Not importForm.ShowDialog() = DialogResult.OK Then Return

        Dim succeed = Await importForm.SaveAsync()

        If succeed Then
            'Refresh list
            InfoBalloon("Imported successfully.", "Done Importing Employee Leave(s)", EmployeePictureBox, 0, -69)
        End If
    End Sub

End Class