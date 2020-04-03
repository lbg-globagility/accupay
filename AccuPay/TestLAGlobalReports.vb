Imports AccuPay.Repository

Public Class TestLAGlobalReports
    Private Sub TestLAGlobalReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        z_OrganizationID = 19
    End Sub

    Private repo As New EmployeeRepository

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim employee = Await repo.GetByEmployeeNumberAsync("0062")

        Dim report = New LaGlobalEmployeeReports(employee)
        report.Print(LaGlobalEmployeeReportName.SmDeploymentEndorsement)
    End Sub


End Class
