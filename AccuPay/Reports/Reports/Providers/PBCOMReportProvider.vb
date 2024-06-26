Option Strict On
Imports AccuPay.CrystalReports
Imports Microsoft.Extensions.DependencyInjection
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Infrastructure.Data

Imports OfficeOpenXml
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Interfaces.Reports
Imports System.Security
Imports System.IO
Imports OfficeOpenXml.DataValidation

Public Class PBCOMReportProvider
    Implements IReportProvider
    Private ReadOnly _reportBuilder As IPbcomReportBuilder

    Public Property Name As String = "PBCOM Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private Const excelFileExtension As String = "xlsx"
    Private Const excelFileFilter As String = "Excel Files|*.xls;*.xlsx;"
    Public Sub New()
        _reportBuilder = MainServiceProvider.GetRequiredService(Of IPbcomReportBuilder)
    End Sub
    Private Async Sub Run() Implements IReportProvider.Run
        Dim payperiodSelector = New MultiplePayPeriodSelectionDialog()

        If Not payperiodSelector.ShowDialog() = DialogResult.OK Then
            Return
        End If

        Dim dateFrom = CDate(payperiodSelector.DateFrom)
        Dim dateTo = CDate(payperiodSelector.DateTo)

        Dim fileName = $"PBCOM-TEMPLATE.xlsm"
        Dim directory = $"D:/project/accupay/AccuPay/ImportTemplates/"
        Dim directoryAndFileName = String.Concat(directory, fileName)
        Dim xcelFile = New FileInfo(directoryAndFileName)

        Dim result = $"PBCOM-TEMPLATE-RESULT.xlsm"
        Dim resultAndFileName = String.Concat(directory, result)
        Dim excelResultFile = New FileInfo(resultAndFileName)
        If excelResultFile.Exists Then
            excelResultFile.Delete()
            excelResultFile = New FileInfo(resultAndFileName)
        End If

        If xcelFile IsNot Nothing Then
            Using package As New ExcelPackage(xcelFile)
                Dim casaDefault As ExcelWorksheet = package.Workbook.Worksheets("CASA default")
                Dim casaWithName As ExcelWorksheet = package.Workbook.Worksheets("CASA w Name")


                Dim allowanceTypes = Await _reportBuilder.GetData(dateFrom, dateTo)

                For index = 0 To allowanceTypes.Count - 1
                    casaDefault.Cells(index + 8, 2).Value = allowanceTypes(index).ATMNo
                    casaDefault.Cells(index + 8, 3).Value = allowanceTypes(index).TotalNetSalary

                    casaWithName.Cells(index + 8, 2).Value = allowanceTypes(index).ATMNo
                    casaWithName.Cells(index + 8, 3).Value = allowanceTypes(index).TotalNetSalary
                    casaWithName.Cells(index + 8, 4).Value = allowanceTypes(index).LastName
                    casaWithName.Cells(index + 8, 5).Value = allowanceTypes(index).FirstName
                    casaWithName.Cells(index + 8, 6).Value = allowanceTypes(index).MiddleName
                Next
                package.SaveAs(excelResultFile)
                Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(fileName, excelFileExtension, excelFileFilter)

                If saveFileDialogHelperOutPut.IsSuccess = False Then Return

                File.Copy(resultAndFileName, saveFileDialogHelperOutPut.FileInfo.FullName)

                Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)
            End Using
        End If
    End Sub
End Class
