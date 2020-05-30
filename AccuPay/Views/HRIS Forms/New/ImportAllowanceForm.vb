Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Helpers
Imports AccuPay.Infrastructure.Services.Excel
Imports AccuPay.Utils
Imports Globagility.AccuPay.Loans
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Public Class ImportAllowanceForm

    Private Const FormEntityName As String = "Allowance"

    Private _allowances As List(Of Allowance)

    Private _allowanceTypeList As List(Of Product)

    Private _allowanceFrequencyList As New List(Of String)

    Public IsSaved As Boolean

    Private _allowanceService As AllowanceDataService

    Private _employeeRepository As EmployeeRepository

    Private _productRepository As ProductRepository

    Private _userActivityRepository As UserActivityRepository

    Sub New()

        InitializeComponent()

        _allowanceService = MainServiceProvider.GetRequiredService(Of AllowanceDataService)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

    End Sub

    Private Async Sub ImportAllowanceForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.IsSaved = False

        Me._allowanceFrequencyList = _allowanceService.GetFrequencyList()

        Me._allowanceTypeList = (Await _productRepository.GetAllowanceTypesAsync(z_OrganizationID)).ToList()

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

        Dim records As New List(Of AllowanceRowRecord)
        Dim parser = New ExcelParser(Of AllowanceRowRecord)()

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
            Sub()
                records = parser.Read(fileName).ToList
            End Sub)

        If parsedSuccessfully = False Then Return

        _allowances = New List(Of Allowance)

        Dim rejectedRecords As New List(Of AllowanceRowRecord)
        Dim acceptedRecords As New List(Of AllowanceRowRecord)

        Dim _okEmployees As New List(Of String)

        For Each record In records
            Dim employee = Await _employeeRepository.
                                GetByEmployeeNumberAsync(record.EmployeeID, z_OrganizationID)

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

            Dim allowanceType = Await _productRepository.GetOrCreateAllowanceTypeAsync(record.Type, z_OrganizationID, z_User)

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

            If record.Amount Is Nothing Then

                record.ErrorMessage = "Allowance amount cannot be blank."

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
                .Amount = record.Amount.Value,
                .EffectiveStartDate = record.EffectiveStartDate.Value,
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
                lblStatus.Text = "There is 1 error."
            Else
                lblStatus.Text = $"There are {errorCount} errors."

            End If

            lblStatus.Text += " Failed records will not be saved."
            lblStatus.BackColor = Color.Red
        Else
            lblStatus.Text = "No errors found."
            lblStatus.BackColor = Color.Green
        End If
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Me.Cursor = Cursors.WaitCursor

        Dim messageTitle = "Import Allowances"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Await _allowanceService.SaveManyAsync(_allowances)

                Dim importList = New List(Of UserActivityItem)
                For Each item In _allowances
                    importList.Add(New UserActivityItem() With
                        {
                        .Description = $"Imported a new {FormEntityName.ToLower()}.",
                        .EntityId = item.RowID.Value
                        })
                Next

                _userActivityRepository.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeImport, importList)

                Me.IsSaved = True
                Me.Cursor = Cursors.Default
                Me.Close()

            End Function)

        Me.Cursor = Cursors.Default
    End Sub

    Private Async Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click
        Dim fileInfo = Await DownloadTemplateHelper.DownloadExcelWithData(ExcelTemplates.Allowance)

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