
Public Class ShiftScheduleForm

    Dim organizationId As Integer = 1

    Private Sub ShiftScheduleForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = organizationId
        EmployeeTreeView1.OrganizationID = organizationId
    End Sub

    Private Sub EmployeeTreeView1_Load(sender As Object, e As EventArgs) Handles EmployeeTreeView1.Load

    End Sub

    Private Sub EmployeeTreeView1_OrganizationIDChanged(s As Object, e As EventArgs) Handles EmployeeTreeView1.OrganizationIDChanged
        Console.WriteLine("EmployeeTreeView1_OrganizationIDChanged")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        organizationId += 1
        Label1.Text = organizationId
        EmployeeTreeView1.OrganizationID = organizationId
    End Sub

    Private Sub EmployeeTreeView1_FiltersEmployee(s As Object, e As EventArgs) Handles EmployeeTreeView1.FiltersEmployee
        Console.WriteLine("EmployeeTreeView1_FiltersEmployee")
    End Sub
End Class
