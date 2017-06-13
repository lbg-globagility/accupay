Public Class newEmpStat

    Private Sub newEmpStat_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        WarnBalloon(, , txtNewEmpStat, , , 1) : Employee.Enabled = True
    End Sub

    Private Sub newEmpStat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Me.Parent = MDIParent1
        'Employee.Enabled = False

        txtNewEmpStat.ContextMenu = New ContextMenu

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        txtNewEmpStat.Text = Employee.strTrimProper(txtNewEmpStat.Text)
        If txtNewEmpStat.Text = "" Then
            txtNewEmpStat.Focus()
            WarnBalloon("Pleas try another.", "Invalid Employee Status", txtNewEmpStat, txtNewEmpStat.Width - 16, -69, , 3000) : Exit Sub
        Else
            For Each itm In Employee.cboEmpStat.Items
                If txtNewEmpStat.Text = itm Then
                    txtNewEmpStat.Focus()
                    WarnBalloon(txtNewEmpStat.Text & " is already exist.", "Invalid Employee Status", txtNewEmpStat, txtNewEmpStat.Width - 16, -69, , 3000)
                    Exit For : Exit Sub
                End If
            Next
        End If

        Employee.cboEmpStat.Items.Add(txtNewEmpStat.Text)
        INS_LoL(txtNewEmpStat.Text, txtNewEmpStat.Text, "Status", , "Yes", , , 1)

    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNewEmpStat.KeyDown, Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1_Click(sender, e)
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtNewEmpStat.TextChanged

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