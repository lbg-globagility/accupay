Option Strict On

Imports AccuPay.Core.Interfaces
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.Extensions.DependencyInjection

Public Class DefaultPayslipFullOvertimeBreakdownProvider
    Implements IReportProvider

    Public Property Name As String = "Payslip" Implements IReportProvider.Name

    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Async Sub Run() Implements IReportProvider.Run

        Dim form As New SelectPayPeriodDialog()

        If form.ShowDialog() <> DialogResult.OK OrElse form.SelectedPayPeriod Is Nothing Then Return

        Dim payPeriod = form.SelectedPayPeriod

        Dim dataService = MainServiceProvider.GetRequiredService(Of IPaystubPayslipModelDataService)
        Dim paystubModels = Await dataService.GetData(z_OrganizationID, payPeriod)

        Dim report As New DefaulltPayslipFullOvertimeBreakdown
        report.SetDataSource(paystubModels)

        Dim detailsSection = report.ReportDefinition.Sections(1)
        Dim txtOrganizationName As TextObject = DirectCast(detailsSection.ReportObjects("txtOrganizationName"), TextObject)
        Dim txtPayPeriod As TextObject = DirectCast(detailsSection.ReportObjects("txtPayPeriod"), TextObject)

        txtOrganizationName.Text = z_OrganizationName.ToUpper
        txtPayPeriod.Text = $"Payslip for the period of {payPeriod.PayFromDate.ToShortDateString} to {payPeriod.PayToDate.ToShortDateString}"

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()

    End Sub

End Class
