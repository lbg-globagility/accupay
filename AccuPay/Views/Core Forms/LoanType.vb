'TBD
Public Class LoanType

    Private Sub LoanType_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        TextBox1.ContextMenu = New ContextMenu

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Trim(TextBox1.Text) <> "" Then
            'TextBox1.Text = StrConv(TextBox1.Text, VbStrConv.ProperCase)
            Dim new_ID = EmployeeForm.INS_product(Trim(TextBox1.Text),
                             Trim(TextBox1.Text),
                             "Loan Type")

            EmployeeForm.cboloantype.Items.Add(Trim(TextBox1.Text))

            EmployeeForm.loan_type.Add(Trim(TextBox1.Text) & "@" & new_ID)

        End If

        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub LoanType_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        enlistTheLists(
            String.Concat("SELECT CONCAT(COALESCE(PartNo,''),'@',RowID)",
                          " FROM product",
                          " WHERE `Category`='Loan Type'",
                          " AND OrganizationID='" & orgztnID & "'",
                          " AND PartNo IN ('Calamity', 'Cash Advance', 'PAGIBIG', 'PhilHealth', 'SSS')",
                          " UNION",
                          " SELECT CONCAT(COALESCE(PartNo,''),'@',RowID)",
                          " FROM product",
                          " WHERE `Category`='Loan Type'",
                          " AND OrganizationID='" & orgztnID & "';"),
                           EmployeeForm.loan_type)

        EmployeeForm.cboloantype.Items.Clear()

        For Each strval In EmployeeForm.loan_type
            EmployeeForm.cboloantype.Items.Add(getStrBetween(strval, "", "@"))
        Next

    End Sub

End Class