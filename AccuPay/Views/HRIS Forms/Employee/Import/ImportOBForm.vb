Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportOBForm

    Private _officialBusinesses As List(Of OfficialBusiness)

    Public IsSaved As Boolean

    Private ReadOnly _employeeRepository As IEmployeeRepository

    Sub New()

        InitializeComponent()

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

    End Sub

    Private Sub ImportOBForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.IsSaved = False

        OBDataGrid.AutoGenerateColumns = False
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

        Dim records As New List(Of OBRowRecord)

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
            Sub()
                records = ExcelService(Of OBRowRecord).
                    Read(fileName).
                    ToList()
            End Sub)

        If parsedSuccessfully = False Then Return

        _officialBusinesses = New List(Of OfficialBusiness)

        Dim acceptedRecords As New List(Of OBRowRecord)
        Dim rejectedRecords As New List(Of OBRowRecord)

        Dim _okEmployees As New List(Of String)

        For Each record In records
            'TODO: this is an N+1 query problem. Refactor this
            Dim employee = Await _employeeRepository.GetByEmployeeNumberAsync(record.EmployeeNumber, z_OrganizationID)

            If employee?.RowID Is Nothing Then
                record.ErrorMessage = "Employee number does not exist."
                rejectedRecords.Add(record)
                Continue For
            End If

            'For displaying on datagrid view; placed here in case record is rejected soon
            record.EmployeeFullName = employee.FullNameWithMiddleInitialLastNameFirst
            record.EmployeeNumber = employee.EmployeeNo

            Dim officialBusiness = ConvertToOfficialBusiness(record, employee.RowID.Value)

            If officialBusiness Is Nothing Then

                If String.IsNullOrWhiteSpace(record.ErrorMessage) Then
                    record.ErrorMessage = "Cannot parse data."

                End If
                rejectedRecords.Add(record)
                Continue For
            End If

            Dim validationErrorMessage = ValidateOfficialBusiness(officialBusiness)

            If Not String.IsNullOrWhiteSpace(validationErrorMessage) Then

                record.ErrorMessage = validationErrorMessage
                rejectedRecords.Add(record)
                Continue For
            End If

            acceptedRecords.Add(record)
            _officialBusinesses.Add(officialBusiness)
        Next

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({_officialBusinesses.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _officialBusinesses.Count > 0

        OBDataGrid.DataSource = acceptedRecords
        RejectedRecordsGrid.DataSource = rejectedRecords

    End Sub

    Private Function ConvertToOfficialBusiness(record As OBRowRecord, employeeId As Integer) As OfficialBusiness

        If record.StartDate Is Nothing Then

            record.ErrorMessage = "Start Date cannot be empty."
            Return Nothing
        End If

        If record.StartDate < PayrollTools.SqlServerMinimumDate Then

            record.ErrorMessage = "Dates cannot be earlier than January 1, 1753."
            Return Nothing
        End If

        Return record.ToOfficialBusiness(employeeId)
    End Function

    Private Function ValidateOfficialBusiness(officialBusiness As OfficialBusiness) As String

        If officialBusiness.OrganizationID Is Nothing Then Return "Organization is required."
        If officialBusiness.EmployeeID Is Nothing Then Return "Employee is required."
        If officialBusiness.StartDate Is Nothing Then Return "Start Date is required."
        If officialBusiness.StartTime Is Nothing Then Return "Start Time is required."
        If officialBusiness.EndTime Is Nothing Then Return "End Time is required."

        If {OfficialBusiness.StatusPending, OfficialBusiness.StatusApproved}.Contains(officialBusiness.Status) = False Then
            Return "Status is not valid."
        End If

        If officialBusiness.StartTime = officialBusiness.EndTime Then Return "End Time cannot be equal to Start Time"

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

        Dim messageTitle = "Import Official Businesses"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()

                Dim dataService = MainServiceProvider.GetRequiredService(Of IOfficialBusinessDataService)
                Await dataService.SaveManyAsync(_officialBusinesses, z_User)

                Me.IsSaved = True

                Me.Close()

            End Function)

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click

        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.OfficialBusiness)

    End Sub

End Class
