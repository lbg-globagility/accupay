Option Strict On

Imports AccuPay.Data.Services
Imports AccuPay.Utils
Imports CrystalDecisions.CrystalReports.Engine

Public Class Cinema2000TardinessReportProvider
    Implements IReportProvider
    Public Property Name As String = "Tardiness Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _dataService As CinemaTardinessReportDataService

    Sub New(dataService As CinemaTardinessReportDataService)

        _dataService = dataService

    End Sub

    Public Async Sub Run() Implements IReportProvider.Run

        Dim n_selectMonth As New selectMonth

        If Not n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim firstDate = CDate(n_selectMonth.MonthValue)

        Dim isLimitedReport As Boolean = False

        Dim question = $"Do you want to show only employees with 8 or more days late for {firstDate.ToString("MMMM")} {firstDate.Year}?"
        If MessageBoxHelper.Confirm(Of Boolean) _
            (question, "Filter Tardiness Report", messageBoxIcon:=MessageBoxIcon.Question) Then

            isLimitedReport = True

        End If

        Try

            Dim report = New Cinemas_Tardiness_Report

            Dim txtOrgName As TextObject = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("txtOrgName"), TextObject)
            txtOrgName.Text = orgNam.ToUpper

            Dim txtReportName As TextObject = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtReportName"), TextObject)
            txtReportName.Text = $"Tadiness Report - {firstDate.ToString("MMMM yyyy")}"

            Dim txtAddress = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("txtAddress"), TextObject)

            txtAddress.Text = PayrollTools.GetOrganizationAddress()

            Dim tardinessReportModels = Await _dataService.GetData(z_OrganizationID,
                                                                    firstDate,
                                                                    isLimitedReport)

            report.SetDataSource(tardinessReportModels)

            Dim crvwr As New CrysRepForm
            crvwr.crysrepvwr.ReportSource = report
            crvwr.Show()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try

    End Sub

End Class