Option Strict On

Imports AccuPay.CrystalReports
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.Extensions.DependencyInjection

Public Class LoanSummaryByEmployeeReportProvider
    Implements IReportProvider

    Public Property Name As String = "Loan Summary by Employee" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub Run() Implements IReportProvider.Run

        Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

        Try
            If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then


                Dim date_from, date_to As Object

                date_from = n_PayrollSummaDateSelection.DateFrom
                date_to = n_PayrollSummaDateSelection.DateTo

                Dim service = MainServiceProvider.GetRequiredService(Of LoanSummaryByEmployeeReportBuilder)

                Dim loanSummaryByEmployeeReport = service.CreateReportDocument(z_OrganizationID, CDate(date_from), CDate(date_to))


                Dim crvwr As New CrysRepForm
                crvwr.crysrepvwr.ReportSource = loanSummaryByEmployeeReport.GetReportDocument

                crvwr.Show()

            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))

        Finally

        End Try

    End Sub

End Class