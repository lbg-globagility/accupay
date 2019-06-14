Option Strict On

Imports System.IO
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.Utils
Imports OfficeOpenXml

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

        Public Shared Async Function DownloadWithData(excelTemplate As ExcelTemplates) As Threading.Tasks.Task(Of FileInfo)
            Dim _employeeRepository As New EmployeeRepository
            Dim excelName = TemplatesHelper.GetFileName(excelTemplate)
            Dim template = TemplatesHelper.GetFullPath(excelTemplate)

            Try

                Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(excelName)

                If saveFileDialogHelperOutPut.IsSuccess = False Then Return Nothing

                File.Copy(template, saveFileDialogHelperOutPut.FileInfo.FullName)

                Dim fileInfo = saveFileDialogHelperOutPut.FileInfo

                'Import employee numbers
                Using package As New ExcelPackage(fileInfo)
                    Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets("Options")
                    Dim allEmployees = Await _employeeRepository.GetAllWithPositionAsync()
                    Dim allEmployed = allEmployees.
                        Where(Function(emp) emp.EmploymentStatus <> "Terminated").
                        Where(Function(emp) emp.EmploymentStatus <> "Resigned").
                        Select(Function(emp) emp.EmployeeNo).
                        OrderBy(Function(no) no).
                        ToList()

                    For index = 0 To allEmployed.Count - 1
                        worksheet.Cells(index + 2, 1).Value = allEmployed(index)
                    Next

                    package.Save()
                End Using

                Return fileInfo
            Catch ex As Exception

                MessageBoxHelper.DefaultErrorMessage()

            End Try

            Return Nothing
        End Function

    End Class

End Namespace