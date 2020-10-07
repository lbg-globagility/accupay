Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Interfaces.Excel
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Helpers
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Public Class ImportAllowanceForm

    Private Const FormEntityName As String = "Allowance"

    Private _allowances As List(Of Allowance)

    Private _allowanceTypeList As List(Of Product)

    Private _allowanceFrequencyList As New List(Of String)

    Public IsSaved As Boolean

    Private _allowanceRepository As AllowanceRepository

    Private _employeeRepository As EmployeeRepository

    Private _productRepository As ProductRepository

    Private _userActivityRepository As UserActivityRepository

    Sub New()

        InitializeComponent()

        _allowanceRepository = MainServiceProvider.GetRequiredService(Of AllowanceRepository)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

    End Sub

    Private Async Sub ImportAllowanceForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.IsSaved = False

        Me._allowanceFrequencyList = _allowanceRepository.GetFrequencyList()

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
        Dim parser = MainServiceProvider.GetRequiredService(Of IExcelParser(Of AllowanceRowRecord))

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
                                GetByEmployeeNumberAsync(record.EmployeeNumber, z_OrganizationID)

            If employee?.RowID Is Nothing Then

                record.ErrorMessage = "Employee number does not exist."
                rejectedRecords.Add(record)

                Continue For
            End If

            'For displaying on datagrid view; placed here in case record is rejected soon
            record.EmployeeFullName = employee.FullNameWithMiddleInitialLastNameFirst
            record.EmployeeNumber = employee.EmployeeNo

            Dim allowance = Await ConvertToAllowance(record, employee.RowID.Value)

            If allowance Is Nothing Then

                If String.IsNullOrWhiteSpace(record.ErrorMessage) Then
                    record.ErrorMessage = "Cannot parse data."

                End If
                rejectedRecords.Add(record)
                Continue For
            End If

            Dim validationErrorMessage = ValidateAllowance(allowance, record.AllowanceType)

            If Not String.IsNullOrWhiteSpace(validationErrorMessage) Then

                record.ErrorMessage = validationErrorMessage
                rejectedRecords.Add(record)
                Continue For
            End If

            _allowances.Add(allowance)
            acceptedRecords.Add(record)
        Next

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({Me._allowances.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _allowances.Count > 0

        AllowancesDataGrid.DataSource = acceptedRecords
        RejectedRecordsGrid.DataSource = rejectedRecords

    End Sub

    Private Async Function ConvertToAllowance(record As AllowanceRowRecord, employeeId As Integer) As Task(Of Allowance)

        If record.EffectiveStartDate Is Nothing Then

            record.ErrorMessage = "Start Date cannot be empty."
            Return Nothing
        End If

        If record.EffectiveStartDate < Data.Helpers.PayrollTools.SqlServerMinimumDate Then

            record.ErrorMessage = "Dates cannot be earlier than January 1, 1753."
            Return Nothing
        End If

        If record.EffectiveEndDate IsNot Nothing AndAlso record.EffectiveEndDate < Data.Helpers.PayrollTools.SqlServerMinimumDate Then

            record.ErrorMessage = "Dates cannot be earlier than January 1, 1753."
            Return Nothing
        End If

        'Type
        If String.IsNullOrWhiteSpace(record.Type) Then

            record.ErrorMessage = "Name of allowance/allowance type cannot be blank."
            Return Nothing

        End If

        'TODO: this is an N+1 query problem. Refactor this
        Dim allowanceType = Await _productRepository.GetOrCreateAllowanceTypeAsync(record.Type, z_OrganizationID, z_User)

        If allowanceType Is Nothing Then

            record.ErrorMessage = "Cannot get or create allowance type. Please contact " & My.Resources.AppCreator
            Return Nothing

        End If
        record.Type = allowanceType.PartNo
        record.AllowanceType = allowanceType

        'Frequency
        Dim allowanceFrequency = Me._allowanceFrequencyList.
                FirstOrDefault(Function(a) a.Equals(record.AllowanceFrequency, StringComparison.InvariantCultureIgnoreCase))

        If allowanceFrequency Is Nothing Then

            record.ErrorMessage = "The frequency '" & record.AllowanceFrequency & "' is not valid."
            Return Nothing

        End If

        record.AllowanceFrequency = allowanceFrequency

        'Amount
        If record.Amount Is Nothing Then

            record.ErrorMessage = "Allowance amount cannot be blank."
            Return Nothing

        End If

        Return record.ToAllowance(employeeId)
    End Function

    Private Function ValidateAllowance(allowance As Allowance, allowanceType As Product) As String

        If allowance.OrganizationID Is Nothing Then Return "Organization is required."

        If allowance.EmployeeID Is Nothing Then Return "Employee is required."

        If _allowanceRepository.GetFrequencyList().Contains(allowance.AllowanceFrequency) = False Then Return "Invalid frequency."

        If allowance.ProductID Is Nothing Then Return "Allowance type is required."

        If allowance.EffectiveEndDate IsNot Nothing AndAlso allowance.EffectiveStartDate > allowance.EffectiveEndDate Then Return "Start date cannot be greater than end date."

        If allowance.Amount < 0 Then Return "Amount cannot be less than 0."

        If allowance.IsMonthly AndAlso Not allowanceType.Fixed Then Return "Only fixed allowance type are allowed for Monthly allowances."

        Return Nothing
    End Function

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

                Dim dataService = MainServiceProvider.GetRequiredService(Of AllowanceDataService)
                Await dataService.SaveManyAsync(_allowances)

                Await RecordUserActivity()

                Me.IsSaved = True
                Me.Cursor = Cursors.Default
                Me.Close()

            End Function)

        Me.Cursor = Cursors.Default
    End Sub

    Private Async Function RecordUserActivity() As Task
        Me._allowanceTypeList = (Await _productRepository.GetAllowanceTypesAsync(z_OrganizationID)).ToList()

        Dim importList = New List(Of UserActivityItem)
        For Each item In _allowances

            Dim allowanceType = Me._allowanceTypeList.Where(Function(t) t.RowID.Value = item.ProductID.Value).FirstOrDefault()

            Dim suffixIdentifier = $" with type '{allowanceType?.PartNo}' and start date '{item.EffectiveStartDate.ToShortDateString()}'."

            importList.Add(New UserActivityItem() With
            {
                .Description = $"Imported a new allowance {suffixIdentifier}",
                .EntityId = item.RowID.Value,
                .ChangedEmployeeId = item.EmployeeID.Value
            })
        Next

        _userActivityRepository.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeImport, importList)
    End Function

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