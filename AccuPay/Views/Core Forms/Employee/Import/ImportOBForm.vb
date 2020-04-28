Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Helpers
Imports AccuPay.Utils
Imports Globagility.AccuPay

Public Class ImportOBForm

    Private _officialBusinesses As List(Of OfficialBusiness)

    Private _employeeRepository As New EmployeeRepository

    Private _officialBusinessRepository As New OfficialBusinessRepository

    Public IsSaved As Boolean

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
        Dim parser = New ExcelParser(Of OBRowRecord)()

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunctionAsync(
            Sub()
                records = parser.Read(fileName).ToList
            End Sub)

        If parsedSuccessfully = False Then Return

        _officialBusinesses = New List(Of OfficialBusiness)

        Dim acceptedRecords As New List(Of OBRowRecord)
        Dim rejectedRecords As New List(Of OBRowRecord)

        Dim _okEmployees As New List(Of String)

        For Each record In records
            'TODO: this is an N+1 query problem. Refactor this
            Dim employee = Await _employeeRepository.GetByEmployeeNumberAsync(record.EmployeeID, z_OrganizationID)
            record.Status = OfficialBusiness.StatusApproved

            If employee Is Nothing Then
                record.ErrorMessage = "Employee number does not exist."
                rejectedRecords.Add(record)
                Continue For
            End If

            'For displaying on datagrid view; placed here in case record is rejected soon
            record.EmployeeFullName = employee.FullNameWithMiddleInitialLastNameFirst
            record.EmployeeID = employee.EmployeeNo

            If CheckIfRecordIsValid(record, rejectedRecords) = False Then

                Continue For

            End If

            If Not record.StartDate.HasValue Then
                record.ErrorMessage = "No start date"
                rejectedRecords.Add(record)
                Continue For
            ElseIf Not record.EndDate.HasValue Then
                record.ErrorMessage = "No end date"
                rejectedRecords.Add(record)
                Continue For
            End If

            If record.StartTime Is Nothing And record.EndTime Is Nothing Then
                record.ErrorMessage = "No start and end time"
                rejectedRecords.Add(record)
                Continue For
            End If

            'For database
            Dim officialbus = New OfficialBusiness With {
                .RowID = Nothing,
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .EmployeeID = employee.RowID,
                .StartDate = record.StartDate.Value,
                .EndDate = record.EndDate.Value,
                .StartTime = record.StartTime,
                .EndTime = record.EndTime,
                .Status = record.Status
            }

            acceptedRecords.Add(record)
            _officialBusinesses.Add(officialbus)
        Next

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({Me._officialBusinesses.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _officialBusinesses.Count > 0

        OBDataGrid.DataSource = acceptedRecords
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

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Me.Cursor = Cursors.WaitCursor

        Dim messageTitle = "Import Official Businesses"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()

                Await _officialBusinessRepository.SaveManyAsync(Me._officialBusinesses)
                Me.IsSaved = True

                Me.Close()

            End Function)

    End Sub

    Private Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click

        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.OfficialBusiness)

    End Sub

    Private Function CheckIfRecordIsValid(record As OBRowRecord, rejectedRecords As List(Of OBRowRecord)) As Boolean

        Dim officialBusiness = record.ToOfficialBusiness()

        If officialBusiness Is Nothing Then
            record.ErrorMessage = "Cannot parse data."
            rejectedRecords.Add(record)
            Return False
        End If

        Dim validationErrorMessage = officialBusiness.Validate()

        If Not String.IsNullOrWhiteSpace(validationErrorMessage) Then
            record.ErrorMessage = validationErrorMessage
            rejectedRecords.Add(record)
            Return False
        End If

        Return True
    End Function

End Class