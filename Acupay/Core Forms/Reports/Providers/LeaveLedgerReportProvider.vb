Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class LeaveLedgerReportProvider
    Implements IReportProvider

    Public Property Name As String = "Leave Ledger" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        Dim dateSelector As New PayrollSummaDateSelection()

        If Not dateSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = dateSelector.DateFrom
        Dim dateTo = dateSelector.DateTo

        Dim params = New Object(,) {
            {"OrganizID", orgztnID},
            {"paramDateFrom", dateFrom},
            {"paramDateTo", dateTo},
            {"PayPeriodDateFromID", dateSelector.PayPeriodFromID},
            {"PayPeriodDateToID", dateSelector.PayPeriodToID}
        }

        Dim data = DirectCast(callProcAsDatTab(params, "RPT_leave_ledger"), DataTable)

        Dim report = New Employee_Leave_Ledger()
        report.SetDataSource(data)

        Dim title = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCutoffDate"), TextObject)
        Dim dateFromTitle = dateFrom?.ToString("MMM dd, yyyy")
        Dim dateToTitle = dateTo?.ToString("MMM dd, yyyy")
        title.Text = $"From {dateFromTitle} to {dateToTitle}"

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub

End Class
