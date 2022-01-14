Imports AccuPay.Desktop.Helpers
Imports AccuPay.Infrastructure.Reports.Customize
Imports Microsoft.Extensions.DependencyInjection

Public Class LaGlobalAlphaListReportProvider
    Implements IReportProvider

    Private startPeriodId As Integer
    Private endPeriodId As Integer
    Private actualSwitch As Boolean
    Private dateFrom As Date
    Private dateTo As Date
    Private ReadOnly _reportBuilder As ILaGlobalAlphaListReportBuilder
    Public Property Name As String = "Alpha list" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub New()
        _reportBuilder = MainServiceProvider.GetRequiredService(Of ILaGlobalAlphaListReportBuilder)
    End Sub

    Public Async Sub Run() Implements IReportProvider.Run
        Dim periodSelector = GetPeriodSelection()
        If periodSelector Is Nothing Then Return

        startPeriodId = periodSelector.PayPeriodFromID.Value
        endPeriodId = periodSelector.PayPeriodToID.Value
        actualSwitch = periodSelector.IsActual
        dateFrom = periodSelector.DateFrom.Value
        dateTo = periodSelector.DateTo.Value

        Try
            Dim reportName As String = "Alphalist"

            Dim defaultFileName = GetDefaultFileName(reportName, periodSelector)

            Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx")

            If saveFileDialogHelperOutPut.IsSuccess = False Then
                Return
            End If

            Dim saveFilePath = saveFileDialogHelperOutPut.FileInfo.FullName

            Await _reportBuilder.GenerateReportAsync(organizationId:=z_OrganizationID,
                actualSwitch:=actualSwitch,
                startPeriodId:=startPeriodId,
                endPeriodId:=endPeriodId,
                saveFilePath:=saveFilePath)

            Process.Start(saveFilePath)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Shared Function GetPeriodSelection() As MultiplePayPeriodSelectionDialog
        Dim payrollSelector = New MultiplePayPeriodSelectionDialog With {
            .ShowDeclaredOrActualOptionsPanel = True
        }

        If payrollSelector.ShowDialog() <> DialogResult.OK Then
            Return Nothing
        End If

        Return payrollSelector
    End Function

    Private Function GetDefaultFileName(
        reportName As String,
        payrollSelector As MultiplePayPeriodSelectionDialog) As String

        Return String.Concat(
            orgNam, "_",
            reportName, "_",
            payrollSelector.SalaryDistributionComboBox.Text.Replace(" ", ""),
            "Report_",
            String.Concat(
                payrollSelector.DateFrom.Value.
                    ToShortDateString().Replace("/", "-"),
                "TO",
                payrollSelector.DateTo.Value.
                    ToShortDateString().Replace("/", "-")),
            ".xlsx")
    End Function

End Class
