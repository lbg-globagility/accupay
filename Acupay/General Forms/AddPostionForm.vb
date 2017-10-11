Public Class AddPostionForm


    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If txtPosition.Text = "" Then
            MsgBox("Please enter position name.", MsgBoxStyle.Exclamation, "No entry.")
            txtPosition.Focus()
            Exit Sub
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * From Position where PositionName = '" & txtPosition.Text & "' And OrganizationID = " & z_OrganizationID & "")

            If dt.Rows.Count > 0 Then
                MsgBox("Position Name is already exist.", MsgBoxStyle.Exclamation, "Duplicate Entry.")
                txtPosition.Clear()
                Exit Sub
            Else

                I_Position(txtPosition.Text, z_datetime, z_datetime, z_User, z_OrganizationID, z_User)
                txtPosition.Clear()

                myBalloon("Added to database", "Done", btnSave, , -65)
                UsersForm.fillPosition()
                Me.Close()

                'MsgBox("Added to database", MsgBoxStyle.Information, "Done")

            End If
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtPosition.Clear()
        Me.Close()

    End Sub
End Class