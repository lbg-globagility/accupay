Option Strict On
Imports AccuPay.CrystalReports
Imports Microsoft.Extensions.DependencyInjection

Public Class ThirteenthMonthSummaryReportProvider
    Implements IReportProvider

    Public Property Name As String = "Thirteenth Month Pay (Summary)" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub Run() Implements IReportProvider.Run
        Dim payperiodSelector As New MultiplePayPeriodSelectionDialog()

        If Not payperiodSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = payperiodSelector.DateFrom
        Dim dateTo = payperiodSelector.DateTo

        Dim service = MainServiceProvider.GetRequiredService(Of ThirteenthMonthSummaryReportBuilder)

        Dim thirteenthMonthReport = service.CreateReportDocument(z_OrganizationID, CDate(dateFrom), CDate(dateTo))


        Dim crvwr As New CrysRepForm()
        crvwr.crysrepvwr.ReportSource = thirteenthMonthReport.GetReportDocument
        crvwr.Show()
    End Sub

End Class