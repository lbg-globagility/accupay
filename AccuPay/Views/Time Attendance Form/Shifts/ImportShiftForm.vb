Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Services
Imports AccuPay.Data.Services.[Imports]
Imports AccuPay.Data.Services.Policies
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportShiftForm

#Region "VariableDeclarations"

    Private _dataSourceOk As IReadOnlyCollection(Of ShiftImportModel)

    Private _dataSourceFailed As IReadOnlyCollection(Of ShiftImportModel)

    Public IsSaved As Boolean

    Private ReadOnly _importParser As ShiftImportParser
    Private ReadOnly _shiftBasedAutoOvertimePolicy As ShiftBasedAutomaticOvertimePolicy

    Sub New(shiftBasedAutoOvertimePolicy As ShiftBasedAutomaticOvertimePolicy)

        InitializeComponent()

        _dataSourceFailed = New List(Of ShiftImportModel)

        _importParser = MainServiceProvider.GetRequiredService(Of ShiftImportParser)

        _shiftBasedAutoOvertimePolicy = shiftBasedAutoOvertimePolicy
        _importParser.SetShiftBasedAutoOvertimePolicy(_shiftBasedAutoOvertimePolicy)
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

    Private Sub ImportShiftForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

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

                _dataSourceOk = parsedResult.ValidRecords.
                    OrderBy(Function(s) s.FullName).
                    ThenBy(Function(s) s.Date).
                    ToList()

                _dataSourceFailed = parsedResult.InvalidRecords

                gridOK.DataSource = _dataSourceOk
                gridFailed.DataSource = _dataSourceFailed

            End Function)

    End Sub

    Private Async Sub btnSave_ClickAsync(sender As Object, e As EventArgs) Handles btnSave.Click

        Await FunctionUtils.TryCatchFunctionAsync("Import Shift Schedule",
            Async Function()

                If _shiftBasedAutoOvertimePolicy.Enabled Then Await SaveShiftBasedOvertimes()

                Dim dataService = MainServiceProvider.GetRequiredService(Of ShiftDataService)
                Dim result = Await dataService.BatchApply(
                    _dataSourceOk,
                    organizationId:=z_OrganizationID,
                    currentlyLoggedInUserId:=z_User)

                Me.IsSaved = True
                DialogResult = DialogResult.OK

            End Function)

    End Sub

    Private Async Function SaveShiftBasedOvertimes() As Task
        Dim employeeIds = _dataSourceOk.Select(Function(sh) sh.EmployeeId.Value).Distinct().ToList()

        Dim overtimeDataService = MainServiceProvider.GetRequiredService(Of OvertimeDataService)

        Await overtimeDataService.GenerateOvertimeByShift(_dataSourceOk, employeeIds, z_OrganizationID, z_User)
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
