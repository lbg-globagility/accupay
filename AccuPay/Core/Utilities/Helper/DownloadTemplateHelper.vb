Option Strict On

Imports System.IO
Imports System.Threading.Tasks
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services.Imports.Policy
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml
Imports OfficeOpenXml.DataValidation

Namespace Global.AccuPay.Desktop.Helpers

    Public Class DownloadTemplateHelper

        Private Const excelFileExtension As String = "xlsx"
        Private Const excelFileFilter As String = "Excel Files|*.xls;*.xlsx;"
        Private Const OPTION_WORKSHEET_NAME = "Options"

        Public Shared ReadOnly EMPLOYEE_IMPORTER_EXCEL_TEMPLATES As ExcelTemplates() = {
            ExcelTemplates.Allowance,
            ExcelTemplates.Leave,
            ExcelTemplates.Loan,
            ExcelTemplates.OfficialBusiness,
            ExcelTemplates.Overtime,
            ExcelTemplates.Salary,
            ExcelTemplates.Shift,
            ExcelTemplates.TripTicket,
            ExcelTemplates.GovernmentPremium}

        Public Shared Async Sub DownloadExcel(excelTemplate As ExcelTemplates)

            Dim excelName = TemplatesHelper.GetFileName(excelTemplate)
            Dim template = TemplatesHelper.GetFullPath(excelTemplate)

            Try

                Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(excelName, excelFileExtension, excelFileFilter)

                If saveFileDialogHelperOutPut.IsSuccess = False Then Return

                File.Copy(template, saveFileDialogHelperOutPut.FileInfo.FullName)

                Dim fileInfo = saveFileDialogHelperOutPut.FileInfo

                Await AssignEmployeeSelectionAsync(excelTemplate, fileInfo)

                Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)
            Catch ex As IOException

                MessageBoxHelper.ErrorMessage(ex.Message)
            Catch ex As Exception

                MessageBoxHelper.DefaultErrorMessage()

            End Try

        End Sub

        Public Shared Async Function DownloadExcelWithData(excelTemplate As ExcelTemplates) As Threading.Tasks.Task(Of FileInfo)
            Dim excelName = TemplatesHelper.GetFileName(excelTemplate)
            Dim template = TemplatesHelper.GetFullPath(excelTemplate)

            Try

                Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(excelName, excelFileExtension, excelFileFilter)

                If saveFileDialogHelperOutPut.IsSuccess = False Then Return Nothing

                File.Copy(template, saveFileDialogHelperOutPut.FileInfo.FullName)

                Dim fileInfo = saveFileDialogHelperOutPut.FileInfo

                Await AssignEmployeeSelectionAsync(excelTemplate, fileInfo)

                Return fileInfo
            Catch ex As IOException

                MessageBoxHelper.ErrorMessage(ex.Message)
            Catch ex As Exception

                MessageBoxHelper.DefaultErrorMessage()

            End Try

            Return Nothing
        End Function

        Private Shared Async Function AssignEmployeeSelectionAsync(excelTemplate As ExcelTemplates, fileInfo As FileInfo) As Task
            Await FunctionUtils.TryCatchFunctionAsync("Assign Employee Selection",
                Async Function()
                    If EMPLOYEE_IMPORTER_EXCEL_TEMPLATES.Contains(excelTemplate) Then
                        Dim employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)
                        Dim listOfValueService = MainServiceProvider.GetRequiredService(Of IListOfValueService)
                        'Import employee numbers
                        Using package As New ExcelPackage(fileInfo)
                            Dim worksheet = package.Workbook.
                                Worksheets.
                                OfType(Of ExcelWorksheet).
                                FirstOrDefault(Function(s) s.Name = OPTION_WORKSHEET_NAME)
                            If worksheet Is Nothing Then worksheet = package.Workbook.Worksheets.Add(Name:=OPTION_WORKSHEET_NAME)

                            Dim importPolicy1 = New ImportPolicy(settings:=listOfValueService.Create(type:=ImportPolicy.TYPE))

                            If Not importPolicy1.IsOpenToAllImportMethod Then
                                Dim allEmployees = Await employeeRepository.GetAllByOrganizationWithPositionAsync(z_OrganizationID)
                                Dim allEmployeIds = allEmployees.
                                    Where(Function(emp) emp.IsActive).
                                    Select(Function(emp) emp.EmployeeNo).
                                    OrderBy(Function(no) no).
                                    ToList()
                                Dim count = allEmployeIds.Count - 1
                                For index As Integer = 0 To count
                                    worksheet.Cells(index + 2, 1).Value = allEmployeIds(index)
                                Next
                            ElseIf importPolicy1.IsOpenToAllImportMethod Then
                                Dim allEmployees = (Await employeeRepository.GetAllAcrossOrganizationWithPositionAsync()).
                                    Where(Function(emp) emp.IsActive).
                                    OrderBy(Function(emp) emp.FullnameEmployeeIdCompanyname).
                                    ToList()

                                Dim count = allEmployees.Count - 1
                                Dim lastIndex = 0
                                For index As Integer = 0 To count
                                    Dim employee = allEmployees(index)
                                    Dim currentIndex = index + 2
                                    worksheet.Cells(currentIndex, 1).Value = employee.FullnameEmployeeIdCompanyname
                                    worksheet.Cells(currentIndex, 3).Value = employee.RowID.Value
                                    lastIndex = currentIndex
                                Next

                                SetEmployeeDataValidationList(package:=package,
                                    excelTemplate:=excelTemplate,
                                    lastIndex:=lastIndex)

                            End If

                            package.Save()
                        End Using
                    End If
                End Function)
        End Function

        Public Shared Sub SetEmployeeDataValidationList(package As ExcelPackage,
            excelTemplate As ExcelTemplates,
            lastIndex As Integer)

            If Not EMPLOYEE_IMPORTER_EXCEL_TEMPLATES.Contains(excelTemplate) Then Return

            Dim sheets = package.Workbook.
                Worksheets.
                OfType(Of ExcelWorksheet).
                ToList()

            Dim sheet As ExcelWorksheet = sheets.FirstOrDefault()
            Select Case excelTemplate
                'Case ExcelTemplates.Allowance Or
                '    ExcelTemplates.Loan Or
                '    ExcelTemplates.OfficialBusiness Or
                '    ExcelTemplates.Overtime

                'Case ExcelTemplates.GovernmentPremium
                '    sheet = sheets.FirstOrDefault()
                '    sheet.DataValidations.Clear()

                Case ExcelTemplates.Leave
                    sheet = sheets.FirstOrDefault(Function(s) s.Name = "Employee Leave")

                Case ExcelTemplates.Salary
                    sheet = sheets.FirstOrDefault(Function(s) s.Name = "Employee Salary")

                Case ExcelTemplates.Shift
                    sheet = sheets.FirstOrDefault(Function(s) s.Name = "ShiftSchedule")

                Case ExcelTemplates.TripTicket
                    sheet = sheets.FirstOrDefault(Function(s) s.Name = "Default")

                Case Else
                    sheet = sheets.FirstOrDefault()
            End Select

            Dim worksheet = sheets.FirstOrDefault(Function(s) s.Name = OPTION_WORKSHEET_NAME)
            If worksheet Is Nothing Then worksheet = package.Workbook.Worksheets.Add(Name:=OPTION_WORKSHEET_NAME)

            SetEmployeeDataValidationList(employeeValidatedWorksheet:=sheet,
                employeeSourceListWorksheet:=worksheet,
                lastIndex:=lastIndex)
        End Sub

        Public Shared Sub SetEmployeeDataValidationList(employeeValidatedWorksheet As ExcelWorksheet,
            employeeSourceListWorksheet As ExcelWorksheet,
            lastIndex As Integer)

            Dim employeeRowIdColumnHeaderAddress = GetColumnAddressByName(excelWorksheet:=employeeValidatedWorksheet, columnName:="EmployeeRowId")
            Dim columnLetter = GetColumnLetterByName(excelWorksheet:=employeeValidatedWorksheet, columnName:="EmployeeRowId")
            Dim rowIndexs = Enumerable.Range(2, 999) '1048575
            'Dim lastIndex2 = lastIndex
            For Each i In rowIndexs
                Dim cellAddress = $"{columnLetter}{i}"
                employeeValidatedWorksheet.Cells(cellAddress).Formula = $"VLOOKUP(A{i},Options!A2:C{lastIndex},3,TRUE)"
                'lastIndex2 += 1
            Next
            Dim validation = employeeValidatedWorksheet.DataValidations.AddListValidation("$A$2:$A$1048576")
            With validation
                .ShowErrorMessage = True
                .ErrorStyle = ExcelDataValidationWarningStyle.stop
                .ErrorTitle = "An invalid value was entered"
                .Error = "Select a value from the list"
                .Formula.ExcelFormula = $"{employeeSourceListWorksheet.Name}!$A$2:$A${lastIndex}"
            End With

        End Sub

        Public Shared Function GetColumnIndexByName(excelWorksheet As ExcelWorksheet, columnName As String) As Integer
            If excelWorksheet Is Nothing Then Throw New ArgumentNullException(NameOf(excelWorksheet))
            Dim cellStart = excelWorksheet.Cells("1:1").First(Function(c) c.Value.ToString() = columnName).Start
            Return cellStart.Column
        End Function

        Public Shared Function GetColumnAddressByName(excelWorksheet As ExcelWorksheet, columnName As String) As String
            If excelWorksheet Is Nothing Then Throw New ArgumentNullException(NameOf(excelWorksheet))
            Dim cellStart = excelWorksheet.Cells("1:1").First(Function(c) c.Value.ToString() = columnName).Start
            Return cellStart.Address
        End Function

        Public Shared Function GetColumnLetterByName(excelWorksheet As ExcelWorksheet, columnName As String) As String
            If excelWorksheet Is Nothing Then Throw New ArgumentNullException(NameOf(excelWorksheet))
            Dim cellStart = excelWorksheet.Cells("1:1").First(Function(c) c.Value.ToString() = columnName).Start
            Dim columnIndex = cellStart.Column
            Return ExcelCellAddress.GetColumnLetter(columnIndex)
        End Function

    End Class

End Namespace
