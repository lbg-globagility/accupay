Imports AccuPay.Core.Interfaces.Reports
Imports Microsoft.Extensions.DependencyInjection

Public Class AlphalistExcelFormatReportProvider
    Implements IReportProvider

    Public Property Name As String = "Alphalist" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _reportBuilder As IAlphalistReportBuilder

    Public Sub New()
        _reportBuilder = MainServiceProvider.GetRequiredService(Of IAlphalistReportBuilder)
    End Sub

    Public Async Sub Run() Implements IReportProvider.Run
        Dim payrollSelector = GetPayrollSelector()
        If payrollSelector Is Nothing Then Return

        Dim selectedPeriods = {payrollSelector.PayPeriodFrom, payrollSelector.PayPeriodTo}

        Dim startPeriod = selectedPeriods.FirstOrDefault()
        Dim endPeriod = selectedPeriods.LastOrDefault()

        Dim dateFrom = startPeriod.PayFromDate
        Dim dateTo = endPeriod.PayToDate

        Dim saveFileDiretory = GetSaveDirectory()
        If saveFileDiretory.Trim.Length = 0 Then Return

        Try
            Dim outputDirectory = Await _reportBuilder.GenerateReportAsync(organizationId:=z_OrganizationID,
                actualSwitch:=payrollSelector.IsActual,
                startPeriod:=startPeriod,
                endPeriod:=endPeriod,
                saveFileDiretory:=saveFileDiretory)

            'Process.Start("explorer.exe", outputDirectory)
            Process.Start(outputDirectory)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Function GetPayrollSelector() As MultiplePayPeriodSelectionDialog
        Dim payrollSelector = New MultiplePayPeriodSelectionDialog With {
            .ShowPayrollSummaryPanel = False,
            .ShowDeclaredOrActualOptionsPanel = True
        }

        If payrollSelector.ShowDialog() <> DialogResult.OK Then
            Return Nothing
        End If

        Return payrollSelector
    End Function

    Private Function GetSaveDirectory() As String
        Dim folderBrowserDialog As New FolderBrowserDialog()

        If folderBrowserDialog.ShowDialog() = DialogResult.OK AndAlso
            Not folderBrowserDialog.SelectedPath.Trim.Equals(String.Empty) Then
            Return folderBrowserDialog.SelectedPath.Trim
        End If

        Return String.Empty
    End Function

End Class
