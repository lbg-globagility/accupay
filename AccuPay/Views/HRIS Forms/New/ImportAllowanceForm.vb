Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Helpers
Imports AccuPay.Loans
Imports AccuPay.Repository
Imports AccuPay.Utils
Imports Globagility.AccuPay
Imports Globagility.AccuPay.Loans
Imports Microsoft.EntityFrameworkCore
Imports OfficeOpenXml

Public Class ImportAllowanceForm

    Private _allowances As List(Of Allowance)

    Private _employeeRepository As New EmployeeRepository

    Private _productRepository As New ProductRepository

    Private _allowanceRepository As New LoanScheduleRepository

    Private _listOfValueRepository As New ListOfValueRepository

    Private _allowanceTypeList As List(Of Product)

    Private _allowanceFrequencyList As New List(Of String) From {"One time", "Daily", "Semi-monthly", "Monthly"}

    Public IsSaved As Boolean

    Private Async Sub ImportAllowanceForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.IsSaved = False

        Dim categoryName = ProductConstant.ALLOWANCE_TYPE_CATEGORY
        Using context = New PayrollContext()

            Dim categoryProduct = Await context.Categories.
                                    Where(Function(c) Nullable.Equals(c.OrganizationID, z_OrganizationID)).
                                    Where(Function(c) c.CategoryName = categoryName).
                                    FirstOrDefaultAsync

            If categoryProduct Is Nothing Then
                'get the existing category with same name to use as CategoryID
                Dim existingCategoryProduct = Await context.Categories.
                                    Where(Function(c) c.CategoryName = categoryName).
                                    FirstOrDefaultAsync

                Dim existingCategoryProductId = existingCategoryProduct?.RowID

                categoryProduct = New Category
                categoryProduct.CategoryID = existingCategoryProductId
                categoryProduct.CategoryName = categoryName
                categoryProduct.OrganizationID = z_OrganizationID
                categoryProduct.CatalogID = Nothing
                categoryProduct.LastUpd = Date.Now

                context.Categories.Add(categoryProduct)
                context.SaveChanges()

                'if there is no existing category with same name,
                'use the newly added category's RowID as its CategoryID

                If existingCategoryProductId Is Nothing Then

                    Try
                        categoryProduct.CategoryID = categoryProduct.RowID
                        Await context.SaveChangesAsync()
                    Catch ex As Exception
                        'if for some reason hindi na update, we can't let that row
                        'to have no CategoryID so dapat i-delete rin yung added category
                        context.Categories.Remove(categoryProduct)
                        context.SaveChanges()

                        Throw ex
                    End Try

                End If
            End If

            If categoryProduct Is Nothing Then
                Dim ex = New Exception("ProductRepository->GetOrCreate: Category not found.")
                Throw ex
            End If

            _allowanceTypeList = Await context.Products.
                                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                Where(Function(p) Nullable.Equals(p.CategoryID, categoryProduct.RowID)).
                                ToListAsync
        End Using

        AllowancesDataGrid.AutoGenerateColumns = False
        RejectedRecordsGrid.AutoGenerateColumns = False

        SaveButton.Enabled = False

    End Sub

    Private Async Sub BrowseButton_Click(sender As Object, e As EventArgs) Handles BrowseButton.Click

        Dim browseFile = New OpenFileDialog With {
            .Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"
        }

        If browseFile.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim fileName = browseFile.FileName

        Dim parser = New ExcelParser(Of AllowanceRowRecord)()
        Dim records = parser.Read(fileName)

        _allowances = New List(Of Allowance)

        Dim rejectedRecords As New List(Of AllowanceRowRecord)
        Dim acceptedRecords As New List(Of AllowanceRowRecord)

        Dim _okEmployees As New List(Of String)

        Dim lineNumber = 0

        For Each record In records
            lineNumber += 1
            record.LineNumber = lineNumber
            Dim employee = Await _employeeRepository.GetByEmployeeNumberAsync(record.EmployeeID)

            If employee Is Nothing Then

                record.ErrorMessage = "Employee number does not exist."
                rejectedRecords.Add(record)

                Continue For
            End If

            'For displaying on datagrid view; placed here in case record is rejected soon
            record.EmployeeFullName = employee.FullNameWithMiddleInitialLastNameFirst
            record.EmployeeID = employee.EmployeeNo

            If String.IsNullOrWhiteSpace(record.Type) Then

                record.ErrorMessage = "Name of allowance/allowance type cannot be blank."

                rejectedRecords.Add(record)

                Continue For

            End If

            Dim allowanceType = Await _productRepository.GetOrCreateAllowanceType(record.Type)

            If allowanceType Is Nothing Then

                record.ErrorMessage = "Cannot get or create allowance type. Please contact " & My.Resources.AppCreator

                rejectedRecords.Add(record)

                Continue For

            End If

            record.Type = allowanceType.PartNo 'For displaying on datagrid view

            If record.EffectiveStartDate Is Nothing Then

                record.ErrorMessage = "Effective Start Date cannot be blank."

                rejectedRecords.Add(record)

                Continue For

            End If

            Dim allowanceFrequency = Me._allowanceFrequencyList.
                FirstOrDefault(Function(a) a.Equals(record.AllowanceFrequency, StringComparison.InvariantCultureIgnoreCase))

            If allowanceFrequency Is Nothing Then

                record.ErrorMessage = "The frequency '" & record.AllowanceFrequency & "' is not valid."

                rejectedRecords.Add(record)

                Continue For

            End If

            'For database
            Dim allowance = New Allowance With {
                .RowID = Nothing,
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .EmployeeID = employee.RowID,
                .AllowanceFrequency = allowanceFrequency,
                .Amount = record.Amount,
                .EffectiveStartDate = record.EffectiveStartDate,
                .EffectiveEndDate = record.EffectiveEndDate,
                .ProductID = allowanceType.RowID
            }
            _allowances.Add(allowance)

            'For displaying on datagrid view
            record.AllowanceFrequency = allowanceFrequency
            acceptedRecords.Add(record)
        Next

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({Me._allowances.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _allowances.Count > 0

        AllowancesDataGrid.DataSource = acceptedRecords
        RejectedRecordsGrid.DataSource = rejectedRecords

    End Sub

    Private Sub UpdateStatusLabel(errorCount As Integer)
        If errorCount > 0 Then

            If errorCount = 1 Then
                lblStatus.Text = $"There is 1 error."
            Else
                lblStatus.Text = $"There are {errorCount} errors."

            End If

            lblStatus.Text += " Failed records will not be saved."
            lblStatus.BackColor = Color.Red
        Else
            lblStatus.Text = $"There is no error."
            lblStatus.BackColor = Color.Green
        End If
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Me.Cursor = Cursors.WaitCursor

        Dim messageTitle = "Import Allowances"

        Try
            Using context As New PayrollContext
                For Each allowance In _allowances
                    context.Allowances.Add(allowance)
                Next

                Await context.SaveChangesAsync()
            End Using

            Me.IsSaved = True

            Me.Close()
        Catch ex As ArgumentException

            Dim errorMessage = "One of the allowances has an error:" & Environment.NewLine & ex.Message

            MessageBoxHelper.ErrorMessage(errorMessage, messageTitle)
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)
        Finally

            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Async Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click
        Dim fileInfo = Await DownloadTemplateHelper.DownloadWithData(ExcelTemplates.Allowance)

        If fileInfo IsNot Nothing Then
            Using package As New ExcelPackage(fileInfo)
                Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets("Options")

                Dim allowanceTypes = _allowanceTypeList.
                                        Select(Function(p) p.PartNo).
                                        OrderBy(Function(p) p).
                                        ToList()

                For index = 0 To allowanceTypes.Count - 1
                    worksheet.Cells(index + 2, 2).Value = allowanceTypes(index)
                Next

                package.Save()

                Process.Start(fileInfo.FullName)
            End Using
        End If
    End Sub

End Class