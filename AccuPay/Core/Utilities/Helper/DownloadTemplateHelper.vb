Option Strict On

Imports System.IO
Imports AccuPay.Data
Imports AccuPay.Data.Repositories
Imports AccuPay.Utils
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Namespace Global.AccuPay.Helpers

    Public Class DownloadTemplateHelper

        Private Const excelFileExtension As String = "xlsx"
        Private Const excelFileFilter As String = "Excel Files|*.xls;*.xlsx;"

        Public Shared Sub DownloadExcel(excelTemplate As ExcelTemplates)

            Dim excelName = TemplatesHelper.GetFileName(excelTemplate)
            Dim template = TemplatesHelper.GetFullPath(excelTemplate)

            Try

                Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(excelName, excelFileExtension, excelFileFilter)

                If saveFileDialogHelperOutPut.IsSuccess = False Then Return

                File.Copy(template, saveFileDialogHelperOutPut.FileInfo.FullName)

                Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)
            Catch ex As IOException

                MessageBoxHelper.ErrorMessage(ex.Message)
            Catch ex As Exception

                MessageBoxHelper.DefaultErrorMessage()

            End Try

        End Sub

        Public Shared Async Function DownloadExcelWithData(excelTemplate As ExcelTemplates) As Threading.Tasks.Task(Of FileInfo)
            Dim _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
            Dim excelName = TemplatesHelper.GetFileName(excelTemplate)
            Dim template = TemplatesHelper.GetFullPath(excelTemplate)

            Try

                Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(excelName, excelFileExtension, excelFileFilter)

                If saveFileDialogHelperOutPut.IsSuccess = False Then Return Nothing

                File.Copy(template, saveFileDialogHelperOutPut.FileInfo.FullName)

                Dim fileInfo = saveFileDialogHelperOutPut.FileInfo

                'Import employee numbers
                Using package As New ExcelPackage(fileInfo)
                    Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets("Options")
                    Dim allEmployees = Await _employeeRepository.GetAllWithPositionAsync(z_OrganizationID)
                    Dim allEmployed = allEmployees.
                        Where(Function(emp) emp.IsActive).
                        Select(Function(emp) emp.EmployeeNo).
                        OrderBy(Function(no) no).
                        ToList()

                    For index = 0 To allEmployed.Count - 1
                        worksheet.Cells(index + 2, 1).Value = allEmployed(index)
                    Next

                    package.Save()
                End Using

                Return fileInfo
            Catch ex As IOException

                MessageBoxHelper.ErrorMessage(ex.Message)
            Catch ex As Exception

                MessageBoxHelper.DefaultErrorMessage()

            End Try

            Return Nothing
        End Function

    End Class

End Namespace