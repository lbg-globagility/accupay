Public Class TestLAGlobalReports
    Private Sub TestLAGlobalReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim reportDocument As New SMDeploymentEndorsement
        reportViewer.ReportSource = reportDocument
    End Sub
End Class
