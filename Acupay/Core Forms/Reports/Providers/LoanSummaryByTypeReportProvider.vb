Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class LoanSummaryByTypeReportProvider
    Implements ReportProvider

    Public Property Name As String = "Loan Summary by Type" Implements ReportProvider.Name

    Public Sub Run() Implements ReportProvider.Run
        Dim dateSelector As New PayrollSummaDateSelection

        If Not dateSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = CDate(dateSelector.DateFromstr)
        Dim dateTo = CDate(dateSelector.DateTostr)

        Dim params = New Object(,) {
            {"OrganizID", orgztnID},
            {"PayDateFrom", CDate(dateSelector.DateFromstr)},
            {"PayDateTo", CDate(dateSelector.DateTostr)}
        }

        Dim data = DirectCast(callProcAsDatTab(params, "RPT_LoansByType"), DataTable)

        Dim report = New LoanReportByType()
        report.SetDataSource(data)

        Dim dateFromTitle = dateFrom.ToString("MMMM d, yyyy")
        Dim dateTotTitle = dateTo.ToString("MMMM d, yyyy")

        Dim title = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("Text14"), TextObject)
        title.Text = $"For the period of {dateFromTitle} to {dateTotTitle}"

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = report
        crvwr.Show()
    End Sub

End Class
