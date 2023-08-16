Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportTimeLogsOptimizedForm

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Property IsSaved As Boolean

    Private Sub ImportTimeLogsOptimizedForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TimeLogDataGrid.AutoGenerateColumns = False
        RejectedRecordsGrid.AutoGenerateColumns = False

    End Sub

    Private Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click

        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.TimeLogsOptimize,
            defaultExtension:="*.xlsm",
            filter:="Excel Macro-Enabled Workbook (*.xlsm)|*.xlsm")

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

        Dim records As New List(Of TimeLogsOptimizedRowRecord)

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
            Sub()
                records = ExcelService(Of TimeLogsOptimizedRowRecord).
                    Read(fileName).
                    ToList()
            End Sub)

        If Not parsedSuccessfully Then Return

        Dim employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)
        Dim allEmployees = (Await employeeRepository.GetAllAcrossOrganizationWithPositionAsync()).
            Where(Function(emp) emp.IsActive).
            OrderBy(Function(emp) emp.FullnameEmployeeIdCompanyname).
            ToList()
        For Each record In records
            Dim employee = allEmployees.FirstOrDefault(Function(em) em.RowID = record.EmployeeRowId)
            If employee Is Nothing Then Continue For
            record.EmployeeFullName = employee.FullNameLastNameFirst
            record.EmployeeNumber = employee.EmployeeNo
            record.OrganizationName = employee.Organization.Name
            record.OrganizationId = employee.OrganizationID
        Next

        Dim validParse = records.Where(Function(t) String.IsNullOrEmpty(t.ErrorMessage)).
            OrderBy(Function(t) t.LineNumber).
            ToList()
        Dim invalidParse = records.Where(Function(t) Not String.IsNullOrEmpty(t.ErrorMessage)).
            OrderBy(Function(t) t.LineNumber).
            ToList()

        ParsedTabControl.Text = $"Ok ({validParse.Count})"
        ErrorsTabControl.Text = $"Errors ({invalidParse.Count})"

        SaveButton.Enabled = validParse.Count > 0

        TimeLogDataGrid.DataSource = validParse
        RejectedRecordsGrid.DataSource = invalidParse

    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Me.Cursor = Cursors.WaitCursor

        Dim messageTitle = "Import Time Logs"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim timeLogRowRecords = TimeLogDataGrid.Rows.
                    OfType(Of DataGridViewRow).
                    Select(Function(r) DirectCast(r.DataBoundItem, TimeLogsOptimizedRowRecord)).
                    ToList()

                Dim timeLog = New List(Of TimeLog)
                For Each record In timeLogRowRecords
                    timeLog.Add(New TimeLog() With {
                        .EmployeeID = record.EmployeeRowId,
                        .OrganizationID = record.OrganizationId,
                        .LogDate = record.Date,
                        .TimeIn = record.TimeIn,
                        .TimeOut = record.TimeOut
                    })
                Next

                Dim dataService = MainServiceProvider.GetRequiredService(Of ITimeLogDataService)
                Await dataService.SaveManyAsync(timeLog, z_User)

                Me.DialogResult = DialogResult.OK

                Me.IsSaved = True

                Me.Close()

                Return Task.FromResult(False)
            End Function)

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

End Class
