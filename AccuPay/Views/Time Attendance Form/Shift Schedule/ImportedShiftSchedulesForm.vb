Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.Services.[Imports]
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Desktop.Helpers
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportedShiftSchedulesForm

#Region "VariableDeclarations"

    Private Const FormEntityName As String = "Shift Schedule"

    Private _dataSourceOk As IReadOnlyCollection(Of ShiftImportModel)

    Private _dataSourceFailed As IReadOnlyCollection(Of ShiftImportModel)

    Public IsSaved As Boolean

    Private _userActivityRepository As UserActivityRepository

    Private _importParser As ShiftImportParser

    Sub New()

        InitializeComponent()

        _dataSourceFailed = New List(Of ShiftImportModel)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

        _importParser = MainServiceProvider.GetRequiredService(Of ShiftImportParser)
    End Sub

#End Region

#Region "Methods"

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

#End Region

#Region "EventHandlers"

    Private Sub ImportedShiftSchedulesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.IsSaved = False

        btnSave.Enabled = False

        gridOK.AutoGenerateColumns = False
        gridFailed.AutoGenerateColumns = False

    End Sub

    Private Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click

        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.Shift)

    End Sub

    Private Async Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click

        Dim workSheetName = "ShiftSchedule"

        Await FunctionUtils.TryCatchExcelParserReadFunctionAsync(
            Async Function()

                Dim result = ExcelHelper.GetFilePath()

                If result.IsSuccess = False Then Return

                Dim filePath = result.Result
                Dim parsedResult = Await _importParser.Parse(filePath, z_OrganizationID)

                _dataSourceOk = parsedResult.ValidRecords
                _dataSourceFailed = parsedResult.InvalidRecords

                gridOK.DataSource = _dataSourceOk
                gridFailed.DataSource = _dataSourceFailed

            End Function)

    End Sub

    Private Async Sub btnSave_ClickAsync(sender As Object, e As EventArgs) Handles btnSave.Click

        Await FunctionUtils.TryCatchFunctionAsync("Import Shift Schedule",
            Async Function()

                Dim employeeDutyScheduleRepositorySave = MainServiceProvider.GetRequiredService(Of EmployeeDutyScheduleDataService)
                Dim result = Await employeeDutyScheduleRepositorySave.BatchApply(
                    _dataSourceOk,
                    organizationId:=z_OrganizationID,
                    userId:=z_User)

                Dim importList = New List(Of UserActivityItem)

                For Each schedule In result.AddedList

                    Dim suffixIdentifier = CreateSuffixIdentifier(schedule)

                    importList.Add(New UserActivityItem() With
                    {
                        .Description = $"Imported a new shift {suffixIdentifier}",
                        .EntityId = schedule.RowID
                    })
                Next
                For Each schedule In result.UpdatedList

                    Dim suffixIdentifier = CreateSuffixIdentifier(schedule)

                    importList.Add(New UserActivityItem() With
                    {
                        .Description = $"Updated a shift on import {suffixIdentifier}",
                        .EntityId = schedule.RowID
                    })
                Next

                _userActivityRepository.CreateRecord(
                    z_User,
                    FormEntityName,
                    z_OrganizationID,
                    UserActivityRepository.RecordTypeImport,
                    importList)

                Me.IsSaved = True
                DialogResult = DialogResult.OK

            End Function)

    End Sub

    Private Shared Function CreateSuffixIdentifier(schedule As EmployeeDutySchedule) As String
        Return $"with date '{schedule.DateSched.ToShortDateString()}'."
    End Function

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Private Sub gridOK_DataSourceChanged(sender As Object, e As EventArgs) Handles gridOK.DataSourceChanged
        Dim validCount = _dataSourceOk.Count

        tabPageOK.Text = String.Concat(tabPageOK.AccessibleDescription, " (", validCount, ")")

        btnSave.Enabled = validCount > 0

    End Sub

    Private Sub gridFailed_DataSourceChanged(sender As Object, e As EventArgs) Handles gridFailed.DataSourceChanged
        tabPageFailed.Text = String.Concat(tabPageFailed.AccessibleDescription, " (", _dataSourceFailed.Count, ")")

        UpdateStatusLabel(_dataSourceFailed.Count)
    End Sub

    Private Sub gridOK_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles gridOK.DataError

    End Sub

    Private Sub gridFailed_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles gridFailed.DataError

    End Sub

#End Region

End Class