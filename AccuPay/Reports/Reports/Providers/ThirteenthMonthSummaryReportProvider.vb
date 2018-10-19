Option Strict On

Public Class ThirteenthMonthSummaryReportProvider
    Implements IReportProvider

    Public Property Name As String = "Thirteenth Month Pay (Summary)" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        Dim payperiodSelector As New PayrollSummaDateSelection()

        If Not payperiodSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = payperiodSelector.DateFrom
        Dim dateTo = payperiodSelector.DateTo

        Dim params = New Object(,) {
            {"$organizationID", orgztnID},
            {"$dateFrom", dateFrom},
            {"$dateTo", dateTo}
        }

        Dim data = callProcAsDatTab(params, "RPT_13thmonthpay")

        Dim report = New ThirteenthMonthSummary()
        report.SetDataSource(data)

        Dim crvwr As New CrysRepForm()
        crvwr.crysrepvwr.ReportSource = report
        crvwr.Show()
    End Sub

End Class
