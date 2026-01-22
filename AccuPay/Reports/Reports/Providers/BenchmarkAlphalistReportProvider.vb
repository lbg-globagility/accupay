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

        Dim year = SelectedYear()

        Dim service = MainServiceProvider.GetRequiredService(Of IBenchmarkAlphalistBuilder)
        Dim sssMonthlyReport = Await service.CreateReportDocument(z_OrganizationID, year)

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = sssMonthlyReport.GetReportDocument()
        crvwr.Show()

    End Sub

    Private Function SelectedYear() As Integer
        Dim input As String = InputBox(Prompt:="Please enter the year:", Title:="Alphalist: Year Selection", DefaultResponse:=Date.Now.Year.ToString())

        If Integer.TryParse(input, SelectedYear) Then
            Return SelectedYear

        End If

        Return Date.Now.Year

    End Function

End Class
