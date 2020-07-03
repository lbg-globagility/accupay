Option Strict On

Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.Extensions.DependencyInjection
Imports AccuPay.CrystalReports

Public Class SSSMonthlyReportProvider
    Implements IReportProvider

    Public Property Name As String = "SSS Monthly Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub Run() Implements IReportProvider.Run

        Dim n_selectMonth As New selectMonth

        If Not n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim service = MainServiceProvider.GetRequiredService(Of SSSMonthyReportBuilder)

        Dim sssMonthlyReport = service.CreateReportDocument(z_OrganizationID, CDate(n_selectMonth.MonthValue))

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = sssMonthlyReport.GetReportDocument
        crvwr.Show()

    End Sub

End Class