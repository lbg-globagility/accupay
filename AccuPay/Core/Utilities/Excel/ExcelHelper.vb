Option Strict On

Imports System.IO
Imports AccuPay.Data.Helpers
Imports AccuPay.Desktop.Helpers

Public Class ExcelHelper

    Public Shared Function GetFilePath() As FunctionResult(Of String)

        Dim browseFileOut = OpenFileDialogImportHelper.BrowseFile()
        If browseFileOut.IsSuccess = False Then Return FunctionResult(Of String).Failed()

        Dim filePath = GetValidFilePath(browseFileOut.FileName)
        Return FunctionResult(Of String).Success(filePath)

    End Function

    Private Shared Function GetValidFilePath(filePath As String) As String
        If Path.GetExtension(filePath) = ".xls" Then
            Dim tempFileName = Path.GetTempFileName() + ".xlsx"

            Dim app = New Microsoft.Office.Interop.Excel.Application()
            Dim workbook = app.Workbooks.Open(filePath)
            workbook.SaveAs(Filename:=tempFileName, FileFormat:=Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook)
            workbook.Close()
            app.Quit()
            filePath = tempFileName
        End If

        Return filePath
    End Function

End Class