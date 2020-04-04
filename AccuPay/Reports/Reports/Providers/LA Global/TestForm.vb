Public Class TestForm
    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim report = New EmploymentContractReportProvider
        report.Output()
    End Sub
End Class