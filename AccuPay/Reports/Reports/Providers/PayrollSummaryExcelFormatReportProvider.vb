Option Strict On

Imports System.IO
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Interfaces
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Helpers
Imports Microsoft.Extensions.DependencyInjection

Public Class PayrollSummaryExcelFormatReportProvider
    Inherits ExcelFormatReport
    Implements IReportProvider

    Public Property Name As String = "Payroll Summary" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private _reportBuilder As IPayrollSummaryReportBuilder

    Sub New()
        _reportBuilder = MainServiceProvider.GetRequiredService(Of IPayrollSummaryReportBuilder)
    End Sub

    Public Async Sub Run() Implements IReportProvider.Run

        Dim payrollSelector = GetPayrollSelector()
        If payrollSelector Is Nothing Then Return

        Dim keepInOneSheet = Convert.ToBoolean(ExcelOptionFormat())
        Dim salaryDistribution = payrollSelector.SalaryDistributionComboBox.Text
        Dim isActual = payrollSelector.IsActual

        Dim distributionTypes = {PayrollSummaryCategory.Cash, PayrollSummaryCategory.DirectDeposit, PayrollSummaryCategory.All}
        If distributionTypes.Contains(salaryDistribution) = False Then

            salaryDistribution = Nothing

        End If

        Dim hideEmptyColumns = payrollSelector.chkHideEmptyColumns.Checked

        Try
            Dim reportName As String = "PayrollSummary"

            Dim defaultFileName = GetDefaultFileName(reportName, payrollSelector)

            Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx")

            If saveFileDialogHelperOutPut.IsSuccess = False Then
                Return
            End If

            Dim saveFilePath = saveFileDialogHelperOutPut.FileInfo.FullName

            Await _reportBuilder.CreateReport(
                keepInOneSheet:=keepInOneSheet,
                hideEmptyColumns:=hideEmptyColumns,
                organizationId:=z_OrganizationID,
                payPeriodFromId:=payrollSelector.PayPeriodFromID.Value,
                payPeriodToId:=payrollSelector.PayPeriodToID.Value,
                salaryDistributionType:=salaryDistribution,
                isActual:=isActual,
                saveFilePath:=saveFilePath)

            Process.Start(saveFilePath)
        Catch ex As IOException

            MessageBoxHelper.ErrorMessage(ex.Message)
        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))
        End Try
    End Sub

    Private Function GetDefaultFileName(
        reportName As String,
        payrollSelector As MultiplePayPeriodSelectionDialog) As String

        Return String.Concat(
            orgNam,
            reportName,
            payrollSelector.SalaryDistributionComboBox.Text.Replace(" ", ""),
            "Report",
            String.Concat(
                payrollSelector.DateFrom.Value.
                    ToShortDateString().Replace("/", "-"),
                "TO",
                payrollSelector.DateTo.Value.
                    ToShortDateString().Replace("/", "-")),
            ".xlsx")
    End Function

    Private Function ExcelOptionFormat() As ExcelOption

        Dim result_value As ExcelOption

        MessageBoxManager.OK = "(A)"

        MessageBoxManager.Cancel = "(B)"

        MessageBoxManager.Register()

        Dim message_content As String =
            String.Concat(
                "Please select an option :", NewLiner(2),
                "A ) keep all in one sheet", NewLiner,
                "B ) separate sheet by department")

        Dim custom_prompt =
            MessageBox.Show(
                message_content,
                "Excel sheet format",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1)

        If custom_prompt = Windows.Forms.DialogResult.OK Then
            result_value = ExcelOption.KeepAllInOneSheet
        Else 'If custom_prompt = Windows.Forms.DialogResult.Cancel Then
            result_value = ExcelOption.SeparateEachDepartment
        End If

        MessageBoxManager.Unregister()

        Return result_value

    End Function

    Private Function NewLiner(Optional repetition As Integer = 1) As String
        Dim _result As String = String.Empty

        Dim i = 0
        While i < repetition
            _result = String.Concat(_result, Environment.NewLine)
            i += 1
        End While

        Return _result
    End Function

End Class