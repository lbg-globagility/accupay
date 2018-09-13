Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class PhilHealthReportProvider
    Implements IReportProvider

    Public Property Name As String = "PhilHealth Monthly Report" Implements IReportProvider.Name

    Private Sub Run() Implements IReportProvider.Run
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

        Dim data = DirectCast(callProcAsDatTab(params, "RPT_PhilHealth_Monthly"), DataTable)

        Dim philHealthReport = New Phil_Health_Monthly_Report
        Dim objText As TextObject = DirectCast(philHealthReport.ReportDefinition.Sections(1).ReportObjects("Text2"), TextObject)
        objText.Text = "For the month of " & date_from
        philHealthReport.SetDataSource(data)

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = philHealthReport

        crvwr.Show()
    End Sub

End Class
