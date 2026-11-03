Option Strict On

Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class RemittanceReportProvider
    Implements IReportProvider

    Public Property Name As String = "Remittance Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _reportBuilder As IRemittanceReportBuilder

    Sub New()
        _reportBuilder = MainServiceProvider.GetRequiredService(Of IRemittanceReportBuilder)
    End Sub

    Public Async Sub Run() Implements IReportProvider.Run

        Dim n_selectMonth As New selectMonth

        If Not n_selectMonth.ShowDialog = DialogResult.OK Then
            Return
        End If

        Dim dateMonth = CDate(n_selectMonth.MonthFirstDate)

        Try
            Dim defaultFileName = $"{orgNam}RemittanceReport{dateMonth:MMMyyyy}.xlsx"

            Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx")

            If saveFileDialogHelperOutPut.IsSuccess = False Then
                Return
            End If

            Dim saveFilePath = saveFileDialogHelperOutPut.FileInfo.FullName

            Await _reportBuilder.CreateReport(
                organizationId:=z_OrganizationID,
                dateMonth:=dateMonth,
                saveFilePath:=saveFilePath)

            Process.Start(saveFilePath)
        Catch ex As Exception

            MessageBoxHelper.ErrorMessage(ex.Message)
        End Try

    End Sub

End Class
