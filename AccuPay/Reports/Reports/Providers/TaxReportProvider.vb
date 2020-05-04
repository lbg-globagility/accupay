Option Strict On

Imports AccuPay.Data.Repositories
Imports CrystalDecisions.CrystalReports.Engine

Public Class TaxReportProvider
    Implements IReportProvider

    Public Property Name As String = "Tax Monthly Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private _payPeriodRepository As PayPeriodRepository

    Sub New()
        _payPeriodRepository = New PayPeriodRepository()
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

        Dim params = New Object(,) {
            {"OrganizID", orgztnID},
            {"paramDateFrom", dateFrom.ToString("s")},
            {"paramDateTo", dateTo.ToString("s")}
        }

        Dim data = callProcAsDatTab(params, "RPT_Tax_Monthly")

        Dim report = New Tax_Monthly_Report()
        Dim objText = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("Text2"), TextObject)
        objText.Text = "For the month of  " & dateFrom.ToString("MMMM yyyy")

        report.SetDataSource(data)

        Dim crvwr As New CrysRepForm()
        crvwr.crysrepvwr.ReportSource = report
        crvwr.Show()
    End Sub

End Class