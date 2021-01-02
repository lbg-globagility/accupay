Option Strict On

Imports AccuPay.CrystalReports
Imports Microsoft.Extensions.DependencyInjection

Public Class PhilHealthReportProvider
    Implements IReportProvider

    Public Property Name As String = "PhilHealth Monthly Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private Sub Run() Implements IReportProvider.Run
        Dim n_selectMonth As New selectMonth

        If Not n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim service = MainServiceProvider.GetRequiredService(Of IPhilHealthMonthlyReportBuilder)

        Dim philHealthReport = service.CreateReportDocument(z_OrganizationID, CDate(n_selectMonth.MonthValue))

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = philHealthReport.GetReportDocument
        crvwr.Show()
    End Sub

End Class
