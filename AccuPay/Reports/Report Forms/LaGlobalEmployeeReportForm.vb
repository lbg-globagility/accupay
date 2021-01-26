Imports CrystalDecisions.CrystalReports.Engine

Public Class LaGlobalEmployeeReportForm
    Private _reportDocument As ReportClass

    Private Sub LaGlobalEmployeeReportForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub SetReportSource(reportDocument As ReportClass)
        _reportDocument = reportDocument
        reportViewer.ReportSource = _reportDocument
    End Sub

    Private Sub LaGlobalEmployeeReportForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        _reportDocument.Dispose()
        reportViewer.ReportSource.Dispose()
    End Sub
End Class