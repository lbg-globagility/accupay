Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports AccuPay.Repository
Imports AccuPay.Utils
Imports Globagility.AccuPay
Imports Microsoft.EntityFrameworkCore

Public Class ImportOBForm

    Private _officialbus As List(Of OfficialBusiness)

    Private _officialbusTypeList As List(Of ListOfValue)

    Private _employeeRepository As New EmployeeRepository

    Public IsSaved As Boolean

    Private Async Sub ImportOBForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.IsSaved = False

        Using context = New PayrollContext()
            _officialbusTypeList = Await context.ListOfValues.
                                    Where(Function(l) l.Type = "Official Business Type").
                                    Where(Function(l) l.Active = "Yes").
                                    ToListAsync
        End Using

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

        _officialbus = New List(Of OfficialBusiness)

        Dim acceptedRecords As New List(Of OBRowRecord)
        Dim rejectedRecords As New List(Of OBRowRecord)

        Dim _okEmployees As New List(Of String)

        For Each record In records
            Dim employee = Await _employeeRepository.GetByEmployeeNumberAsync(record.EmployeeID)
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

            Dim officialbusType As New ListOfValue

            Using context = New PayrollContext()

                officialbusType = Await context.ListOfValues.
                                    Where(Function(l) l.Type = "Official Business Type").
                                    Where(Function(l) l.Active = "Yes").
                                    Where(Function(l) l.DisplayValue.Equals(record.Type, StringComparison.InvariantCultureIgnoreCase)).
                                    FirstOrDefaultAsync()

                If officialbusType Is Nothing Then
                    Try
                        Using OBContext = New PayrollContext

                            Dim listOfVal As New ListOfValue
                            listOfVal.DisplayValue = record.Type.Trim()
                            listOfVal.Type = "Official Business Type"
                            listOfVal.Active = "Yes"

                            listOfVal.Created = Date.Now
                            listOfVal.CreatedBy = z_User
                            listOfVal.LastUpd = Date.Now
                            listOfVal.LastUpdBy = z_User
                            OBContext.ListOfValues.Add(listOfVal)

                            Await OBContext.SaveChangesAsync()

                            officialbusType = Await context.ListOfValues.
                            FirstOrDefaultAsync(Function(l) Nullable.Equals(l.RowID, listOfVal.RowID))

                        End Using
                    Catch ex As DbUpdateException
                        officialbusType = Nothing
                    Catch ex As Exception
                        officialbusType = Nothing
                    End Try
                End If

            End Using

            If officialbusType Is Nothing Then

                record.ErrorMessage = "Cannot get or create official business type. Please contact " & My.Resources.AppCreator

                rejectedRecords.Add(record)

                Continue For

            End If

            record.Type = officialbusType.DisplayValue 'For displaying on datagrid view

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
                .Type = record.Type,
                .StartDate = record.StartDate.Value,
                .EndDate = record.EndDate.Value,
                .StartTime = record.StartTime,
                .EndTime = record.EndTime,
                .Status = record.Status
            }

            acceptedRecords.Add(record)
            _officialbus.Add(officialbus)
        Next

        UpdateStatusLabel(rejectedRecords.Count)

        ParsedTabControl.Text = $"Ok ({Me._officialbus.Count})"
        ErrorsTabControl.Text = $"Errors ({rejectedRecords.Count})"

        SaveButton.Enabled = _officialbus.Count > 0

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

        Try
            Using context As New PayrollContext
                For Each officialbus In _officialbus
                    context.OfficialBusinesses.Add(officialbus)
                Next

                Await context.SaveChangesAsync()
            End Using

            Me.IsSaved = True

            Me.Close()
        Catch ex As ArgumentException

            Dim errorMessage = "One of the official businesses has an error:" & Environment.NewLine & ex.Message

            MessageBoxHelper.ErrorMessage(errorMessage, messageTitle)
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)
        Finally

            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click

        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.OfficialBusiness)

    End Sub

    Private Function CheckIfRecordIsValid(record As OBRowRecord, rejectedRecords As List(Of OBRowRecord)) As Boolean

        If String.IsNullOrWhiteSpace(record.Type) Then

            record.ErrorMessage = "OB Type cannot be blank."
            rejectedRecords.Add(record)
            Return False
        End If

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