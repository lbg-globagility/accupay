Option Strict On

Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.ValueObjects
Imports AccuPay.CrystalReports
Imports Microsoft.Extensions.DependencyInjection

Public Class ThirteenthMonthSummaryReportProvider
    Implements IReportProvider

    Public Property Name As String = "Thirteenth Month Pay (Summary)" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Async Sub Run() Implements IReportProvider.Run
        Dim payperiodSelector As New MultiplePayPeriodSelectionDialog()

        If Not payperiodSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = payperiodSelector.DateFrom
        Dim dateTo = payperiodSelector.DateTo

        Dim builder = MainServiceProvider.GetRequiredService(Of IThirteenthMonthSummaryReportBuilder)
        Dim service = MainServiceProvider.GetRequiredService(Of IThirteenthMonthSummaryReportDataService)

        Dim timePeriod As New TimePeriod(CDate(dateFrom), CDate(dateTo))
        Dim data = Await service.GetData(z_OrganizationID, timePeriod)

        Dim thirteenthMonthReport = builder.
            CreateReportDocument(data, timePeriod).
            GetReportDocument()

        Dim crvwr As New CrysRepForm()
        crvwr.crysrepvwr.ReportSource = thirteenthMonthReport
        crvwr.Show()
    End Sub

End Class
