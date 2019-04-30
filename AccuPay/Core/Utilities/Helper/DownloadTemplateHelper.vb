Option Strict On
Imports System.IO
Imports AccuPay.Utils

Namespace Global.AccuPay.Helpers

    Public Class DownloadTemplateHelper

        Public Shared Sub Download(excelTemplate As ExcelTemplates)

            Dim excelName = TemplatesHelper.GetFileName(excelTemplate)
            Dim template = TemplatesHelper.GetFullPath(excelTemplate)

            Try

                Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(excelName)

                If saveFileDialogHelperOutPut.IsSuccess = False Then Return

                File.Copy(template, saveFileDialogHelperOutPut.FileInfo.FullName)

                Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)

            Catch ex As Exception

                MessageBoxHelper.DefaultErrorMessage()

            End Try

        End Sub

    End Class

End Namespace
