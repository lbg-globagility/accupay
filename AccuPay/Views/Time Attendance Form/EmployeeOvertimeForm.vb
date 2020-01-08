Public Class EmployeeOvertimeForm

    Private Sub EmployeeOvertimeForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
        InfoBalloon(, , FormTitleLabel, , , 1)
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Sub ImportToolStripButton_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click
        Using form = New ImportOvertimeForm()
            form.ShowDialog()

            If form.IsSaved Then
                myBalloon("Overtimes Successfully Imported", "Import Overtimes", EmployeePictureBox, 100, -20)
                'Refresh list
            End If
        End Using
    End Sub

End Class