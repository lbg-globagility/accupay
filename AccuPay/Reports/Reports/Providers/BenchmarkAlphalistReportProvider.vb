Option Strict On
Option Explicit On

Imports AccuPay.CrystalReports
Imports Microsoft.Extensions.DependencyInjection

Public Class BenchmarkAlphalistReportProvider
    Implements IReportProvider

    Public Property Name As String = "Alphalist" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Async Sub Run() Implements IReportProvider.Run

        Dim report = New BenchmarkAlphalist

        Dim year = 2020

        Dim service = MainServiceProvider.GetRequiredService(Of IBenchmarkAlphalistBuilder)
        Dim sssMonthlyReport = Await service.CreateReportDocument(z_OrganizationID, year)

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = sssMonthlyReport.GetReportDocument()
        crvwr.Show()

    End Sub

End Class
