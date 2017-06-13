Public Class newProdAllowa

    Private Sub newProd_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()
        'INS_product

        TextBox1.ContextMenu = New ContextMenu

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Trim(TextBox1.Text) <> "" Then

            TextBox1.Text = StrConv(TextBox1.Text, VbStrConv.ProperCase)

            Dim new_ID = Employee.INS_product(Trim(TextBox1.Text), _
                             Trim(TextBox1.Text), _
                             "Allowance Type", _
                             istaxab,
                             chkFixed.Checked)

            Employee.cboallowtype.Items.Add(Trim(TextBox1.Text))

            Employee.eall_Type.Items.Add(Trim(TextBox1.Text))

            Employee.allowance_type.Add(Trim(TextBox1.Text) & "@" & new_ID)

        End If

        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Dim istaxab As SByte = 0

    Private Sub chktaxab_CheckedChanged(sender As Object, e As EventArgs) Handles chktaxab.CheckedChanged

        istaxab = If(chktaxab.Checked, 1, 0)

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