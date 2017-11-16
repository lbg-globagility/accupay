Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class SSSMonthlyReportProvider
    Implements IReportProvider

    Public Property Name As String = "SSS Monthly Report" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run

        Dim n_selectMonth As New selectMonth

        If Not n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim params(2, 2) As Object
        params(0, 0) = "OrganizID"
        params(1, 0) = "paramDate"
        params(0, 1) = orgztnID
        params(1, 1) = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

        Dim date_from = Format(CDate(n_selectMonth.MonthValue), "MMMM  yyyy")

        Dim data = callProcAsDatTab(params, "RPT_SSS_Monthly")

        Dim sssMonthlyReport = New SSS_Monthly_Report
        sssMonthlyReport.SetDataSource(data)

        Dim objText As TextObject = DirectCast(sssMonthlyReport.ReportDefinition.Sections(1).ReportObjects("Text2"), TextObject)

        objText.Text = "for the month of " & date_from

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = sssMonthlyReport
        crvwr.Show()
    End Sub

End Class
