Option Strict On

Imports AccuPay.Core.Interfaces
Imports AccuPay.CrystalReports.Payslip
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.Extensions.DependencyInjection

Public Class DefaulltPayslipAllowanceSalaryOnlyReportProvider
    Implements IReportProvider

    Public Property Name As String = "Payslip [Allowance Salary]" Implements IReportProvider.Name

    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Async Sub Run() Implements IReportProvider.Run

        Dim form As New SelectPayPeriodDialog()

        If form.ShowDialog() <> DialogResult.OK OrElse form.SelectedPayPeriod Is Nothing Then Return

        Dim payPeriod = form.SelectedPayPeriod

        Dim paystubPayslipModelDataService = MainServiceProvider.GetRequiredService(Of IPaystubPayslipModelDataService)
        Dim paystubModels = Await paystubPayslipModelDataService.GetDataAllowanceSalaryOnly(z_OrganizationID, payPeriod)

        Dim report As New DefaulltPayslipAllowanceSalaryOnly
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
