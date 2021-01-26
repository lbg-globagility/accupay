Option Strict On

Imports AccuPay.CrystalReports
Imports Microsoft.Extensions.DependencyInjection

Public Class LoanSummaryByTypeReportProvider
    Implements IReportProvider

    Public Property Name As String = "Loan Summary by Type" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub Run() Implements IReportProvider.Run
        Dim dateSelector As New MultiplePayPeriodSelectionDialog

        If Not dateSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim pagingStylePrompt =
            MsgBox("Do you want to print separate pages every Loan Type ?",
                   MsgBoxStyle.YesNo,
                   String.Concat("Print ", Name, " report"))

        Dim dateFrom = CDate(dateSelector.DateFrom)
        Dim dateTo = CDate(dateSelector.DateTo)

        Dim service = MainServiceProvider.GetRequiredService(Of ILoanSummaryByTypeReportBuilder)

        Dim loanReport As ILoanSummaryByTypeReportBuilder

        If pagingStylePrompt = MsgBoxResult.Yes Then
            'per page
            loanReport = service.CreateReportDocumentPerPage(z_OrganizationID, dateFrom, dateTo)
        Else
            loanReport = service.CreateReportDocument(z_OrganizationID, dateFrom, dateTo)
        End If

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = loanReport.GetReportDocument
        crvwr.Show()
    End Sub

End Class
