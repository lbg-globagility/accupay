Option Strict On

Imports System.IO
Imports AccuPay.Infrastracture.Services.Excel

Namespace Global.Globagility.AccuPay

    Public Class ExcelService(Of T As {IExcelRowRecord, New})

        Public Shared Function Read(filePath As String, Optional workSheetName As String = Nothing) As IList(Of T)

            ' TODO: use GetValidFilePath of ExcelHelper
            filePath = GetValidFilePath(filePath)

            Dim parser = New ExcelParser(Of T)(workSheetName)
            Return parser.Read(filePath)

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

End Namespace