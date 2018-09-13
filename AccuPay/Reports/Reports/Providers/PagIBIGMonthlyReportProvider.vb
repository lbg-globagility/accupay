Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class PagIBIGMonthlyReportProvider
    Implements IReportProvider

    Public Property Name As String = "Pag-IBIG Monthly Report" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        Dim n_selectMonth As New selectMonth

        If Not n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim params(2, 2) As Object

        params(0, 0) = "OrganizID"
        params(1, 0) = "paramDate"

        params(0, 1) = orgztnID

        Dim thedatevalue = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

        params(1, 1) = Format(CDate(n_selectMonth.MonthValue), "yyyy-MM-dd")

        Dim date_from = Format(CDate(n_selectMonth.MonthValue), "MMMM  yyyy")

        Dim data = callProcAsDatTab(params, "RPT_PAGIBIG_Monthly")

        Dim report = New Pagibig_Monthly_Report

        Dim objText As TextObject = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("Text2"), TextObject)
        objText.Text = "for the month of " & date_from

        report.SetDataSource(data)

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = report
        crvwr.Show()
    End Sub

End Class
