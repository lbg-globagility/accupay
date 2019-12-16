Public Class newProdBonus

    Private Sub newProdBonus_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        TextBox1.ContextMenu = New ContextMenu

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Trim(TextBox1.Text) <> "" Then
            TextBox1.Text = StrConv(TextBox1.Text, VbStrConv.ProperCase)

            Dim istaxab = If(chktaxab.Checked, "1", "0")

            Dim new_ID = EmployeeForm.INS_product(Trim(TextBox1.Text), _
                             Trim(TextBox1.Text), _
                             "Bonus", _
                             istaxab)

            EmployeeForm.cbobontype.Items.Add(Trim(TextBox1.Text))

            EmployeeForm.bon_Type.Items.Add(Trim(TextBox1.Text))

            EmployeeForm.bonus_type.Add(Trim(TextBox1.Text) & "@" & new_ID)

        End If

        Me.Close()
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