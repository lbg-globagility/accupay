Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services.[Imports]
Imports AccuPay.Core.Services.Imports.Policy
Imports AccuPay.Core.Services.Policies
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportShiftForm

#Region "VariableDeclarations"

    Private _dataSourceOk As IReadOnlyCollection(Of ShiftImportModel)

    Private _dataSourceFailed As IReadOnlyCollection(Of ShiftImportModel)

    Public IsSaved As Boolean

    Private ReadOnly _importParser As IShiftImportParser
    Private ReadOnly _shiftBasedAutoOvertimePolicy As ShiftBasedAutomaticOvertimePolicy
    Private ReadOnly _importPolicy As ImportPolicy
    Private ReadOnly _employeeRepository As IEmployeeRepository

    Sub New(shiftBasedAutoOvertimePolicy As ShiftBasedAutomaticOvertimePolicy)

        InitializeComponent()

        _dataSourceFailed = New List(Of ShiftImportModel)

        _importParser = MainServiceProvider.GetRequiredService(Of IShiftImportParser)

        _shiftBasedAutoOvertimePolicy = shiftBasedAutoOvertimePolicy
        _importParser.SetShiftBasedAutoOvertimePolicy(_shiftBasedAutoOvertimePolicy)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        Dim policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _importPolicy = policyHelper.ImportPolicy
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

                If _importPolicy.IsOpenToAllImportMethod Then
                    Await ParseShiftRowRecords(fileName:=filePath)
                Else
                    Dim parsedResult = Await _importParser.Parse(filePath, z_OrganizationID)

                    _dataSourceOk = parsedResult.ValidRecords.
                        OrderBy(Function(s) s.FullName).
                        ThenBy(Function(s) s.Date).
                        ToList()

                    _dataSourceFailed = parsedResult.InvalidRecords
                End If

                gridOK.DataSource = _dataSourceOk
                gridFailed.DataSource = _dataSourceFailed

            End Function)
    End Sub

    Private Async Function ParseShiftRowRecords(fileName As String) As Task
        Dim records As New List(Of ShiftRowRecord)

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
                                Sub()
                                    records = ExcelService(Of ShiftRowRecord).Read(fileName, "ShiftSchedule").ToList
                                End Sub)

        If parsedSuccessfully = False Then Return

        If records Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read the template.")

            Return
        End If

        Dim shiftImportModels = New List(Of ShiftImportModel)

        Await StandardImport2(records, shiftImportModels)

        _dataSourceOk = shiftImportModels.Where(Function(t) t.IsValidToSave).ToList()

        _dataSourceFailed = shiftImportModels.Where(Function(t) Not t.IsValidToSave).ToList()
    End Function

    Private Async Function StandardImport2(records As List(Of ShiftRowRecord),
        shiftImportModels As List(Of ShiftImportModel)) As Task

        Dim employeeIdList = records.Select(Function(t) t.EmployeeRowId).ToArray()
        Dim employees = (Await _employeeRepository.GetByMultipleIdAsync(employeeIdList:=employeeIdList)).ToList()

        For Each record In records
            Dim employee = employees.FirstOrDefault(Function(e) Integer.Equals(e.RowID, record.EmployeeRowId))

            If employee Is Nothing Then
                record.ErrorMessage = "Employee does not exist!"
                Continue For
            End If

            If record.StartDate < PayrollTools.SqlServerMinimumDate Then
                record.ErrorMessage = "Dates cannot be earlier than January 1, 1753."
                Continue For
            End If

            If CheckIfRecordIsValid(record) = False Then
                Continue For
            End If

            shiftImportModels.Add(New ShiftImportModel(employee:=employee, _shiftBasedAutoOvertimePolicy) With {
                .Date = record.StartDate,
                .BreakTime = record.BreakStartTime,
                .BreakLength = record.BreakLength,
                .IsRestDay = record.IsRestDay,
                .StartTime = record.StartTime,
                .EndTime = record.EndTime
            })
        Next
    End Function

    Private Function CheckIfRecordIsValid(record As ShiftRowRecord) As Boolean
        If record.BreakStartTime Is Nothing And record.BreakLength = 0 Then Return False

        If (record.BreakStartTime IsNot Nothing AndAlso
            record.BreakLength <= 0) OrElse (
            record.BreakLength > 0 AndAlso
            record.BreakStartTime Is Nothing) Then

            record.ErrorMessage = "Invalid break time"
            Return False
        End If

        Return True
    End Function

    Private Async Sub btnSave_ClickAsync(sender As Object, e As EventArgs) Handles btnSave.Click

        Await FunctionUtils.TryCatchFunctionAsync("Import Shift Schedule",
            Async Function()

                If _shiftBasedAutoOvertimePolicy.Enabled Then Await SaveShiftBasedOvertimes()

                Dim dataService = MainServiceProvider.GetRequiredService(Of IShiftDataService)

                If _importPolicy.IsOpenToAllImportMethod Then
                    Dim shiftSchedules = _dataSourceOk.
                        Select(Function(t) t.ToShift(t.Employee.OrganizationID.Value)).
                        ToList()
                    Await dataService.SaveManyAsync(entities:=shiftSchedules, z_User)
                Else
                    Await dataService.BatchApply(
                        _dataSourceOk,
                        organizationId:=z_OrganizationID,
                        currentlyLoggedInUserId:=z_User)
                End If

                Me.IsSaved = True
                DialogResult = DialogResult.OK

            End Function)

    End Sub

    Private Async Function SaveShiftBasedOvertimes() As Task
        Dim employeeIds = _dataSourceOk.Select(Function(sh) sh.EmployeeId.Value).Distinct().ToList()

        Dim overtimeDataService = MainServiceProvider.GetRequiredService(Of IOvertimeDataService)

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
