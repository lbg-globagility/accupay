﻿Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class LoanSummaryByTypeReportProvider
    Implements IReportProvider

    Public Property Name As String = "Loan Summary by Type" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub Run() Implements IReportProvider.Run
        Dim dateSelector As New PayrollSummaDateSelection

        If Not dateSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim report As ReportClass
        report = New LoanReportByType()

        Dim pagingStylePrompt =
            MsgBox("Do you want to print separate pages every Loan Type ?",
                   MsgBoxStyle.YesNo,
                   String.Concat("Print ", Name, " report"))

        If pagingStylePrompt = MsgBoxResult.Yes Then
            report = New LoanReportByTypePerPage()
        End If

        Dim dateFrom = CDate(dateSelector.DateFrom)
        Dim dateTo = CDate(dateSelector.DateTo)

        Dim params = New Object(,) {
            {"OrganizID", orgztnID},
            {"PayDateFrom", CDate(dateSelector.DateFrom)},
            {"PayDateTo", CDate(dateSelector.DateTo)}
        }

        Dim data = DirectCast(callProcAsDatTab(params, "RPT_LoansByType"), DataTable)

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