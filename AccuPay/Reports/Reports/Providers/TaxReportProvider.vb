Option Strict On

Imports AccuPay.CrystalReports
Imports AccuPay.Data.Repositories
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.Extensions.DependencyInjection

Public Class TaxReportProvider
    Implements IReportProvider

    Public Property Name As String = "Tax Monthly Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private _payPeriodRepository As PayPeriodRepository

    Sub New()
        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)
    End Sub

    Public Sub Run() Implements IReportProvider.Run
        Dim n_selectMonth As New selectMonth

        If Not n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim month = CDate(n_selectMonth.MonthFirstDate)

        Dim payPeriods = _payPeriodRepository.GetByMonthYearAndPayPrequency(
                                z_OrganizationID,
                                month:=month.Month,
                                year:=month.Year,
                                payFrequencyId:=AccuPay.Data.Helpers.PayrollTools.PayFrequencySemiMonthlyId).
                            ToList()

        Dim dateFrom = payPeriods.First().PayFromDate
        Dim dateTo = payPeriods.Last().PayToDate

        Dim service = MainServiceProvider.GetRequiredService(Of TaxMonthlyReportCreator)

        Dim taxMonthlyReport = service.CreateReportDocument(z_OrganizationID, dateFrom, dateTo)

        Dim crvwr As New CrysRepForm()
        crvwr.crysrepvwr.ReportSource = taxMonthlyReport.GetReportDocument
        crvwr.Show()
    End Sub

End Class