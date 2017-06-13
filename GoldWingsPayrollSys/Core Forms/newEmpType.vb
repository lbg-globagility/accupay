Public Class newEmpType

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        txtNewEmpType.Text = Employee.strTrimProper(txtNewEmpType.Text)
        If txtNewEmpType.Text = "" Then
            txtNewEmpType.Focus()
            WarnBalloon("Please input an Employee Type.", "Invalid Employee Type", txtNewEmpType, txtNewEmpType.Width - 16, -69, , 3000)
            Exit Sub
        End If
        For Each itm In Employee.cboEmpType.Items
            If itm = txtNewEmpType.Text Then
                txtNewEmpType.Focus()
                WarnBalloon(txtNewEmpType.Text & " is already exist. Please try another.", "Invalid Employee Type", txtNewEmpType, txtNewEmpType.Width - 16, -69, , 3000)
                Exit Sub
            End If
        Next

        Employee.cboEmpType.Items.Add(txtNewEmpType.Text)
        Employee.cboEmpType.Text = txtNewEmpType.Text

        INS_LoL(txtNewEmpType.Text, txtNewEmpType.Text, "Employee Type", , "Yes", , , 1)
        Me.Hide() : Me.Close()
    End Sub

    Private Sub txtNewEmpStat_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNewEmpType.KeyDown, Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1_Click(sender, e)
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub txtNewEmpType_TextChanged(sender As Object, e As EventArgs) Handles txtNewEmpType.TextChanged

    End Sub

    Private Sub newEmpType_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        WarnBalloon(, , txtNewEmpType, , , 1)
    End Sub

    Private Sub newEmpType_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        txtNewEmpType.ContextMenu = New ContextMenu

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Button2_Click(Button2, New EventArgs)

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

End Class