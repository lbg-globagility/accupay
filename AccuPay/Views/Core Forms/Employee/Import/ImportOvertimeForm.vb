Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Helpers
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportOvertimeForm

    Private Const FormEntityName As String = "Overtime"

    Private _overtimes As List(Of Overtime)

    Public IsSaved As Boolean

    Private _employeeRepository As EmployeeRepository

    Private _userActivityRepository As UserActivityRepository

    Sub New()

        InitializeComponent()

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

    End Sub

    Private Sub ImportOvertimeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.IsSaved = False

        OvertimeDataGrid.AutoGenerateColumns = False
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

        Dim records As New List(Of OvertimeRowRecord)

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
            Sub()
                records = ExcelService(Of OvertimeRowRecord).
                                Read(fileName).
                                ToList()
            End Sub)

        If parsedSuccessfully = False Then Return

        _overtimes = New List(Of Overtime)

        Dim acceptedRecords As New List(Of OvertimeRowRecord)
        Dim rejectedRecords As New List(Of OvertimeRowRecord)

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

            Dim overtime = ConvertToOvertime(record, employee.RowID.Value)

            If overtime Is Nothing Then

                If String.IsNullOrWhiteSpace(record.ErrorMessage) Then
                    record.ErrorMessage = "Cannot parse data."

                End If
                rejectedRecords.Add(record)
                Continue For
            End If

            Dim validationErrorMessage = ValidateOvertime(overtime)

            If Not String.IsNullOrWhiteSpace(validationErrorMessage) Then

                record.ErrorMessage = validationErrorMessage
                rejectedRecords.Add(record)
                Continue For
            End If

            acceptedRecords.Add(record)
            _overtimes.Add(overtime)
        Next

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({Me._overtimes.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _overtimes.Count > 0

        OvertimeDataGrid.DataSource = acceptedRecords
        RejectedRecordsGrid.DataSource = rejectedRecords

    End Sub

    Private Function ConvertToOvertime(record As OvertimeRowRecord, employeeId As Integer) As Overtime

        If record.StartDate Is Nothing Then

            record.ErrorMessage = "Start Date cannot be empty."
            Return Nothing
        End If

        If record.StartDate < Data.Helpers.PayrollTools.SqlServerMinimumDate Then

            record.ErrorMessage = "Dates cannot be earlier than January 1, 1753."
            Return Nothing
        End If

        Return record.ToOvertime(employeeId)
    End Function

    Private Function ValidateOvertime(ByVal overtime As Overtime) As String
        If overtime.OrganizationID Is Nothing Then Return "Organization is required."
        If overtime.EmployeeID Is Nothing Then Return "Employee is required."
        If overtime.OTStartTime Is Nothing Then Return "Start Time is required."
        If overtime.OTEndTime Is Nothing Then Return "End Time is required."

        If {Overtime.StatusPending, Overtime.StatusApproved}.Contains(overtime.Status) = False Then
            Return "Status is not valid."
        End If

        If overtime.OTStartTime = overtime.OTEndTime Then Return "End Time cannot be equal to Start Time"

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

        Dim messageTitle = "Import Overtimes"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()

                Dim dataService = MainServiceProvider.GetRequiredService(Of OvertimeDataService)
                Await dataService.SaveManyAsync(_overtimes)

                Dim importlist = New List(Of UserActivityItem)

                For Each overtime In _overtimes
                    importlist.Add(New UserActivityItem() With
                    {
                        .Description = $"Imported a new {FormEntityName.ToLower()}.",
                        .EntityId = CInt(overtime.RowID)
                    })
                Next

                _userActivityRepository.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeImport, importlist)

                Me.IsSaved = True

                Me.Close()

            End Function)

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click

        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.Overtime)

    End Sub

End Class