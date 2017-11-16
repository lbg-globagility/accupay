Option Strict On

Public Class ThirteenthMonthSummaryReportProvider
    Implements IReportProvider

    Public Property Name As String = "Thirteenth Month Summary" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        Dim promptYear = New promptyear()

        If Not promptYear.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim params = New Object(,) {
            {"OrganizID", orgztnID},
            {"paramYear", promptYear.YearValue}
        }

        Dim data = callProcAsDatTab(params, "RPT_13thmonthpay")

        Dim report = New ThirteenthMonthSummary()
        report.SetDataSource(data)

        Dim crvwr As New CrysRepForm()
        crvwr.crysrepvwr.ReportSource = report
        crvwr.Show()
    End Sub

End Class
