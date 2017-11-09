Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class PhilHealthReportProvider
    Implements ReportProvider

    Public Property Name As String = "PhilHealth Monthly Report" Implements ReportProvider.Name

    Public Property DataTable As DataTable Implements ReportProvider.DataTable

    Public Property ReportFile As Object Implements ReportProvider.ReportFile

    Private Sub Run() Implements ReportProvider.Run
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

        DataTable = DirectCast(callProcAsDatTab(params, "RPT_PhilHealth_Monthly"), DataTable)

        Dim philHealthReport = New Phil_Health_Monthly_Report
        Dim objText As TextObject = DirectCast(philHealthReport.ReportDefinition.Sections(1).ReportObjects("Text2"), TextObject)
        objText.Text = "For the month of " & date_from

        ReportFile = philHealthReport
    End Sub

End Class
