Imports AccuPay.Core.Interfaces.Reports
Imports AccuPay.Desktop.Helpers
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml
Imports OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
Imports System.IO

Public Class PBCOMSelection
    Private ReadOnly _reportBuilder As IPbcomReportBuilder

    Public Sub New()
        InitializeComponent()
        _reportBuilder = MainServiceProvider.GetRequiredService(Of IPbcomReportBuilder)
    End Sub
    Private Const excelFileExtension As String = "xlsm"
    Private Const excelFileFilter As String = "Excel Files|*.xls;*.xlsx;*.xlsm"
    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim version = "PBCOM Report"
        Dim defaultFileName = "PBCOM"
        Dim template = UsePBCOMTemplate(version, defaultFileName)


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim version = "13th Month Pay"
        Dim defaultFileName = "PBCOM 13th Month Pay"
        Dim template = UsePBCOMTemplate(version, defaultFileName)
    End Sub
    Public Async Function UsePBCOMTemplate(version As String, defaultFileName As String) As Threading.Tasks.Task


        Dim fileName = $"PBCOM-TEMPLATE.xlsm"
        Dim directory = $"ImportTemplates/"
        Dim directoryAndFileName = String.Concat(directory, fileName)
        Dim xcelFile = New FileInfo(directoryAndFileName)

        Dim result = $"PBCOM-TEMPLATE-RESULT.xlsm"
        Dim resultAndFileName = String.Concat(directory, result)
        Dim excelResultFile = New FileInfo(resultAndFileName)
        If excelResultFile.Exists Then
            excelResultFile.Delete()
            excelResultFile = New FileInfo(resultAndFileName)
        End If
        Dim dateFrom = New DateTime(DateTime.Now.Year, 1, 1)
        Dim dateTo = New DateTime(DateTime.Now.Year, 12, 31)
        If xcelFile IsNot Nothing Then
            Using package As New ExcelPackage(xcelFile)
                Dim casaDefault As ExcelWorksheet = package.Workbook.Worksheets("CASA default")
                Dim casaWithName As ExcelWorksheet = package.Workbook.Worksheets("CASA w Name")


                If (version = "PBCOM Report") Then
                    Dim payperiodSelector = New MultiplePayPeriodSelectionDialog()

                    If Not payperiodSelector.ShowDialog() = DialogResult.OK Then
                        Return
                    End If

                    dateFrom = CDate(payperiodSelector.DateFrom)
                    dateTo = CDate(payperiodSelector.DateTo)
                End If

                Dim pbcomPaystubs = Await _reportBuilder.GetData(dateFrom, dateTo, z_OrganizationID, version)

                For index = 0 To pbcomPaystubs.Count - 1
                    casaDefault.Cells(index + 8, 2).Value = pbcomPaystubs(index)?.ATMNo
                    casaDefault.Cells(index + 8, 3).Value = pbcomPaystubs(index).Amount

                    casaWithName.Cells(index + 8, 2).Value = pbcomPaystubs(index)?.ATMNo
                    casaWithName.Cells(index + 8, 3).Value = pbcomPaystubs(index).Amount
                    casaWithName.Cells(index + 8, 4).Value = pbcomPaystubs(index).LastName
                    casaWithName.Cells(index + 8, 5).Value = pbcomPaystubs(index).FirstName
                    casaWithName.Cells(index + 8, 6).Value = pbcomPaystubs(index).MiddleName
                Next
                package.SaveAs(excelResultFile)
                Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, excelFileExtension, excelFileFilter)

                If saveFileDialogHelperOutPut.IsSuccess = False Then Return

                File.Copy(resultAndFileName, saveFileDialogHelperOutPut.FileInfo.FullName)

                Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)
            End Using
        End If
    End Function

End Class
