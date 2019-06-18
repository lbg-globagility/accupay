Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports AccuPay.Repository
Imports AccuPay.Utils
Imports Globagility.AccuPay

Public Class ImportOvertimeForm

    Private _overtimes As List(Of Overtime)

    Private _employeeRepository As New EmployeeRepository

    Public IsSaved As Boolean

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
        Dim parser = New ExcelParser(Of OvertimeRowRecord)()

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunctionAsync(
            Sub()
                records = parser.Read(fileName).ToList
            End Sub)

        If parsedSuccessfully = False Then Return

        _overtimes = New List(Of Overtime)

        Dim acceptedRecords As New List(Of OvertimeRowRecord)
        Dim rejectedRecords As New List(Of OvertimeRowRecord)

        Dim _okEmployees As New List(Of String)

        For Each record In records
            Dim employee = Await _employeeRepository.GetByEmployeeNumberAsync(record.EmployeeID)

            If employee Is Nothing Then

                record.ErrorMessage = "Employee number does not exist."
                rejectedRecords.Add(record)

                Continue For
            End If

            'For displaying on datagrid view; placed here in case record is rejected soon
            record.EmployeeFullName = employee.FullNameWithMiddleInitialLastNameFirst
            record.EmployeeID = employee.EmployeeNo
            record.Type = Overtime.DefaultType

            If CheckIfRecordIsValid(record, rejectedRecords) = False Then

                Continue For

            End If

            'For database
            Dim newOvertime = New Overtime With {
                .RowID = Nothing,
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .EmployeeID = employee.RowID,
                .Type = record.Type,
                .OTStartDate = record.EffectiveStartDate.Value,
                .OTEndDate = record.EffectiveEndDate.Value,
                .OTStartTime = record.EffectiveStartTime,
                .OTEndTime = record.EffectiveEndTime,
                .Status = Overtime.StatusApproved
            }

            acceptedRecords.Add(record)
            _overtimes.Add(newOvertime)
        Next

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({Me._overtimes.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _overtimes.Count > 0

        OvertimeDataGrid.DataSource = acceptedRecords
        RejectedRecordsGrid.DataSource = rejectedRecords

    End Sub

    Private Function CheckIfRecordIsValid(record As OvertimeRowRecord, rejectedRecords As List(Of OvertimeRowRecord)) As Boolean

        If record.EffectiveStartDate Is Nothing Then

            record.ErrorMessage = "Effective Start Date cannot be empty."
            rejectedRecords.Add(record)
            Return False
        End If

        If record.EffectiveStartTime Is Nothing Then

            record.ErrorMessage = "Effective Start Time cannot be empty."
            rejectedRecords.Add(record)
            Return False
        End If

        If record.EffectiveEndDate Is Nothing Then

            record.ErrorMessage = "Effective End Date cannot be empty."
            rejectedRecords.Add(record)
            Return False
        End If

        If record.EffectiveEndTime Is Nothing Then

            record.ErrorMessage = "Effective End Time cannot be empty."
            rejectedRecords.Add(record)
            Return False
        End If

        Return True
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

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Me.Cursor = Cursors.WaitCursor

        Dim messageTitle = "Import Overtimes"

        Try
            Using context As New PayrollContext
                For Each overtime In _overtimes
                    context.Overtimes.Add(overtime)
                Next

                Await context.SaveChangesAsync()
            End Using

            Me.IsSaved = True

            Me.Close()
        Catch ex As ArgumentException

            Dim errorMessage = "One of the overtimes has an error:" & Environment.NewLine & ex.Message

            MessageBoxHelper.ErrorMessage(errorMessage, messageTitle)
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)
        Finally

            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click

        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.Overtime)

    End Sub

End Class