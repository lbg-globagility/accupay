Option Strict On

Imports AccuPay.Core.Interfaces
Imports AccuPay.CrystalReports
Imports Microsoft.Extensions.DependencyInjection

Public Class TaxReportProvider
    Implements IReportProvider

    Public Property Name As String = "Tax Monthly Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Sub New()
        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)
    End Sub

    Public Sub Run() Implements IReportProvider.Run
        Dim n_selectMonth As New selectMonth

        If Not n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim month = CDate(n_selectMonth.MonthFirstDate)

        Dim service = MainServiceProvider.GetRequiredService(Of ITaxMonthlyReportBuilder)

        Dim taxMonthlyReport = service.CreateReportDocument(z_OrganizationID, month:=month.Month, year:=month.Year)

        Dim crvwr As New CrysRepForm()
        crvwr.crysrepvwr.ReportSource = taxMonthlyReport.GetReportDocument
        crvwr.Show()
    End Sub

End Class
