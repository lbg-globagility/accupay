Public Class LeaveBalanceSummaryReportProvider
    Implements IReportProvider

    Public Property GotescoReportName As String = "Employee Leave Balance Summary" Implements IReportProvider.GotescoReportName

    Public Property IsFreeRangeOfDate As Boolean = True Implements IReportProvider.IsFreeRangeOfDate

    Public Property Name As String = GotescoReportName Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        If IsFreeRangeOfDate Then
            Dim newDateRangeDialog As New DateRangeDialog

            If newDateRangeDialog.ShowDialog = DialogResult.OK Then
                With newDateRangeDialog
                    ShowSampleLeaveBalanceSummaryReport(.DateFrom, .DateTo)
                End With
            End If
        End If
    End Sub

    Private Sub ShowSampleLeaveBalanceSummaryReport(date_from As Date, date_to As Date)
        Dim params = New Object() {org_rowid, MYSQLDateFormat(date_from), MYSQLDateFormat(date_to)}

        Dim sql As New SQL("CALL `RPT_leave_summary`(?org_rowid, NULL, ?date_from, ?date_to);", params)

        Dim dt As New DataTable
        dt = sql.GetFoundRows.Tables.OfType(Of DataTable).First

        Dim report = New LeaveBalanceSummary()
        report.SetDataSource(dt)

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub

End Class