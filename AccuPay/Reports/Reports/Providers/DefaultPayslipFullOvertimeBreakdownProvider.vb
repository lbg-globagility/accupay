Option Strict On

Imports AccuPay.Data.Services
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.Extensions.DependencyInjection

Public Class DefaultPayslipFullOvertimeBreakdownProvider
    Implements IReportProvider

    Public Property Name As String = "Payslip" Implements IReportProvider.Name

    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Async Sub Run() Implements IReportProvider.Run

        Dim form As New selectPayPeriod()
        form.ShowDialog()

        Dim payPeriod = form.PayPeriod

        If payPeriod Is Nothing Then Return

        Dim dataService = MainServiceProvider.GetRequiredService(Of PaystubPayslipModelDataService)

        Dim paystubModels = Await dataService.GetData(z_OrganizationID, payPeriod)

        Dim report As New Payslip.DefaulltPayslipFullOvertimeBreakdown
        report.SetDataSource(paystubModels)

        Dim detailsSection = report.ReportDefinition.Sections(1)
        Dim txtOrganizationName As TextObject = DirectCast(detailsSection.ReportObjects("txtOrganizationName"), TextObject)
        Dim txtPayPeriod As TextObject = DirectCast(detailsSection.ReportObjects("txtPayPeriod"), TextObject)

        txtOrganizationName.Text = orgNam.ToUpper
        txtPayPeriod.Text = $"Payslip for the period of {payPeriod.PayFromDate.ToShortDateString} to {payPeriod.PayToDate.ToShortDateString}"

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()

    End Sub

End Class