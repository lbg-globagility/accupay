Imports CrystalDecisions.CrystalReports.Engine

Public Class FiledLeavesReportProvider
    Implements IReportProvider

    Public Property GotescoReportName As String = "Filed Leaves Report" Implements IReportProvider.GotescoReportName

    Public Property IsFreeRangeOfDate As Boolean = True Implements IReportProvider.IsFreeRangeOfDate

    Public Property Name As String = GotescoReportName Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        If IsFreeRangeOfDate Then
            Dim newDateRangeDialog As New DateRangeDialog

            If newDateRangeDialog.ShowDialog = DialogResult.OK Then
                With newDateRangeDialog
                    ShowSampleFiledLeaveReport(.DateFrom, .DateTo)
                End With
            End If
        End If
    End Sub

    Private Sub ShowSampleFiledLeaveReport(date_from As Date, date_to As Date)
        Dim params = New Object() {org_rowid, MYSQLDateFormat(date_from), MYSQLDateFormat(date_to)}

        Dim sql As New SQL("CALL `RPT_filed_leaves`(?org_rowid, NULL, ?date_from, ?date_to);", params)

        Dim dt As New DataTable
        dt = sql.GetFoundRows.Tables.OfType(Of DataTable).First

        Dim report = New FiledLeaves()
        report.SetDataSource(dt)

        Dim objText As TextObject = Nothing

        objText = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("PeriodDate"), TextObject)

        objText.Text =
            String.Concat("As of ",
                          date_to.ToLongDateString)

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub

End Class