Public Class LoanType

    Private Sub LoanType_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        TextBox1.ContextMenu = New ContextMenu

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Trim(TextBox1.Text) <> "" Then
            'TextBox1.Text = StrConv(TextBox1.Text, VbStrConv.ProperCase)
            Dim new_ID = Employee.INS_product(Trim(TextBox1.Text), _
                             Trim(TextBox1.Text), _
                             "Loan Type")

            Employee.cboloantype.Items.Add(Trim(TextBox1.Text))

            Employee.loan_type.Add(Trim(TextBox1.Text) & "@" & new_ID)

        End If

        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

End Class