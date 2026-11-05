Option Strict On

Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.ValueObjects
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class NonBdoBankReportProvider
    Implements IReportProvider

    Public Property Name As String = "Non-BDO Bank Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _reportBuilder As INonBdoBankReportBuilder

    Sub New()
        _reportBuilder = MainServiceProvider.GetRequiredService(Of INonBdoBankReportBuilder)
    End Sub

    Public Async Sub Run() Implements IReportProvider.Run

        Dim payPeriodSelector As New MultiplePayPeriodSelectionDialog()

        If Not payPeriodSelector.ShowDialog = DialogResult.OK Then
            Return
        End If

        Dim timePeriod = New TimePeriod(payPeriodSelector.DateFrom.Value, payPeriodSelector.DateTo.Value)

        Try
            Dim defaultFileName = $"{orgNam}NonBDOBankReport{timePeriod.Start:MMddyyyy}-{timePeriod.End:MMddyyyy}.xlsx"

            Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx")

            If saveFileDialogHelperOutPut.IsSuccess = False Then
                Return
            End If

            Dim saveFilePath = saveFileDialogHelperOutPut.FileInfo.FullName

            Await _reportBuilder.CreateReport(
                organizationId:=z_OrganizationID,
                timePeriod:=timePeriod,
                saveFilePath:=saveFilePath)

            Process.Start(saveFilePath)
        Catch ex As Exception

            MessageBoxHelper.ErrorMessage(ex.Message)
        End Try

    End Sub

End Class
