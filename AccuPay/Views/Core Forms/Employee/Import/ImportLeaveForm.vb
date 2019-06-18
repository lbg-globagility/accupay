Imports System.Threading.Tasks
Imports AccuPay.Attributes
Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports AccuPay.Repository
Imports AccuPay.Utils
Imports Globagility.AccuPay
Imports log4net
Imports Microsoft.EntityFrameworkCore
Imports OfficeOpenXml

Public Class ImportLeaveForm
    Private Shared logger As ILog = LogManager.GetLogger("EmployeeFormAppender")

    Private _filePath As String
    Private _worksheetName As String
    Private _ep As New ExcelParser(Of LeaveModel)
    Private _okModels As List(Of LeaveModel)
    Private _failModels As List(Of LeaveModel)
    Private _leaveRepository As New LeaveRepository()

#Region "Properties"

    Private Property FileDirectory As String
        Get
            Return _filePath
        End Get
        Set(value As String)
            _filePath = value

            FilePathChangedAsync()
        End Set
    End Property

#End Region

#Region "Methods"

    'Private Sub ExcelParserPreparation(filePath As String, Optional worksheetName As String = Nothing)

    '    _filePath = filePath

    '    If Not String.IsNullOrWhiteSpace(worksheetName) Then
    '        _worksheetName = worksheetName
    '        _ep = New ExcelParser(Of LeaveModel)(_worksheetName)
    '    End If

    '    _ep = New ExcelParser(Of LeaveModel)
    'End Sub

    Private Async Sub FilePathChangedAsync()
        Dim models As New List(Of LeaveModel)

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunctionAsync(
            Sub()
                models = _ep.Read(_filePath).ToList
            End Sub)

        If parsedSuccessfully = False Then Return

        Dim employeeNos = models.Select(Function(lm) lm.EmployeeNo).ToList()

        Dim dataSource As New List(Of LeaveModel)

        Using context = New PayrollContext
            Dim employees = Await context.Employees.
                Where(Function(e) e.OrganizationID = z_OrganizationID).
                Where(Function(e) employeeNos.Contains(e.EmployeeNo)).
                ToListAsync()

            For Each model In models

                Dim employee = employees.Where(Function(e) e.EmployeeNo = model.EmployeeNo).FirstOrDefault

                dataSource.Add(CreateLeaveModel(model, employee))
            Next

            _okModels = dataSource.Where(Function(ee) Not ee.ConsideredFailed).ToList()
            _failModels = dataSource.Where(Function(ee) ee.ConsideredFailed).ToList()
            DataGridView1.DataSource = _okModels
            DataGridView2.DataSource = _failModels
        End Using

        SaveButton.Enabled = _okModels.Count > 0

        TabPage1.Text = $"Ok ({Me._okModels.Count})"
        TabPage2.Text = $"Failed ({Me._failModels.Count})"

        UpdateStatusLabel(_failModels.Count)

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

    Private Shared Function CreateLeaveModel(model As LeaveModel, employee As Employee) As LeaveModel
        Return New LeaveModel(employee) With {
            .Status = model.Status,
            .Comment = model.Comment,
            .EndDate = model.EndDate,
            .EndTime = model.EndTime,
            .LeaveType = model.LeaveType,
            .Reason = model.Reason,
            .StartDate = model.StartDate,
            .StartTime = model.StartTime,
            .LineNumber = model.LineNumber}
    End Function

    Public Async Function SaveAsync() As Task(Of Boolean)
        Dim succeed As Boolean = False

        If Not _okModels.Any() Then Return succeed

        succeed = Await SaveImport()

        Return succeed
    End Function

    Private Async Function SaveImport() As Task(Of Boolean)

        Dim messageTitle = "Import Employee Leave"

        Dim leaves As List(Of Leave) = Await GetLeaves()

        If leaves.Any = False Then

            MessageBoxHelper.Warning("No employee leaves to be added!")

            Return False

        End If

        Try
            Await _leaveRepository.SaveManyAsync(leaves)

            Return True
        Catch ex As ArgumentException

            MessageBoxHelper.ErrorMessage("One of the employees has reached its maximum leave allowance. Please check your data and try again.", messageTitle)

            Return False
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex, "EmployeeImportLeave")

            Return False

        End Try

    End Function

    Private Async Function GetLeaves() As Task(Of List(Of Leave))

        Dim leaves As New List(Of Leave)

        Using context = New PayrollContext
            Dim employeeIDs = _okModels.Select(Function(lm) lm.EmployeeID).ToList()
            Dim minDate = _okModels.Min(Function(lm) lm.StartDate.Value.Date)
            Dim maxDate = _okModels.Max(Function(lm) lm.EndDateProper.Date)

            Dim currentLeaves = Await context.Leaves.
                Where(Function(lv) lv.OrganizationID = z_OrganizationID).
                Where(Function(lv) employeeIDs.Contains(lv.EmployeeID.Value)).
                Where(Function(lv) lv.StartDate >= minDate AndAlso lv.EndDate.Value.Date <= maxDate).
                ToListAsync()

            If currentLeaves.Any() Then
                For Each model In _okModels
                    Dim leave = currentLeaves.
                        Where(Function(lv) lv.EmployeeID.Value = model.EmployeeID).
                        Where(Function(lv) lv.StartDate >= model.StartDate.Value.Date AndAlso lv.EndDate.Value.Date <= model.EndDateProper.Date).
                        FirstOrDefault

                    If leave IsNot Nothing Then
                        With leave
                            .Reason = model.Reason
                            .Comments = model.Comment
                            .StartTime = model.StartTime
                            .StartDate = model.StartDate
                            .EndTime = model.EndTime
                            .EndDate = model.EndDateProper
                            .LastUpd = Now
                            .LastUpdBy = z_User
                        End With

                        leaves.Add(leave)
                    Else
                        leaves.Add(CreateNewLeave(model))
                    End If
                Next
            Else
                For Each model In _okModels
                    leaves.Add(CreateNewLeave(model))
                Next

            End If
        End Using

        Return leaves
    End Function

    Private Shared Function CreateNewLeave(model As LeaveModel) As Leave
        Return New Leave() With {
            .OrganizationID = z_OrganizationID,
            .Reason = model.Reason,
            .Comments = model.Comment,
            .CreatedBy = z_User,
            .LastUpdBy = z_User,
            .EmployeeID = model.EmployeeID,
            .StartTime = model.StartTime,
            .StartDate = model.StartDate,
            .EndTime = model.EndTime,
            .EndDate = model.EndDateProper,
            .LeaveType = model.LeaveType,
            .Status = model.Status}
    End Function

#End Region

    Private Class LeaveModel
        Implements IExcelRowRecord
        Private Const PENDING_STATUS As String = "Pending"
        Private Const ADDITIONAL_VACATION_LEAVETYPE As String = "Additional VL"
        Private Const REASON_LENGTH As Integer = 500
        Private Const COMMENT_LENGTH As Integer = 2000
        Private VALID_STATUS As String() = {"approved", "pending"}
        Private _employee As Employee
        Private _noEmployeeNo As Boolean
        Private _noLeaveType As Boolean
        Private _noStartDate As Boolean
        Private _employeeNotExists As Boolean
        Private _noStatus As Boolean
        Private _notMeantToUseAddtlVL As Boolean
        Private _reasons As String
        Private _comments As String

        'Private _startTime As TimeSpan?
        'Private _endTime As TimeSpan?
        Private _status As String

        Private Shared _grantsAdditionalVacationLeaveTypeFeaure As Boolean

        Public Sub New()
            FeatureAdditionalVacationLeaveType()

        End Sub

        Private Shared Sub FeatureAdditionalVacationLeaveType()
            Dim checker = FeatureListChecker.Instance
            _grantsAdditionalVacationLeaveTypeFeaure = checker.HasAccess(Feature.AdditionalVacationLeaveType)
        End Sub

        Public Sub New(employee As Employee)
            FeatureAdditionalVacationLeaveType()

            If employee IsNot Nothing Then
                _employee = employee

                EmployeeNo = _employee.EmployeeNo
                FullName = _employee.FullNameWithMiddleInitialLastNameFirst
                EmployeeID = _employee.RowID.Value
            End If

        End Sub

        Public Property EmployeeID As Integer

        <ColumnName("Employee ID")>
        Public Property EmployeeNo As String

        Public Property FullName As String

        <ColumnName("Leave Type")>
        Public Property LeaveType As String

        <ColumnName("Status")>
        Public Property Status As String
            Get
                Return _status
            End Get
            Set(value As String)
                _status = value
                If String.IsNullOrWhiteSpace(_status) Then _status = PENDING_STATUS
            End Set
        End Property

        <ColumnName("Start Time (Optional)")>
        Public Property StartTime As TimeSpan?

        '    Get
        '        Return _startTime
        '    End Get
        '    Set(value As TimeSpan?)
        '        _startTime = value
        '    End Set
        'End Property

        'Public ReadOnly Property StartTimeDisplay As String
        '    Get
        '        Return ShortTimeSpan(_startTime)
        '    End Get
        'End Property

        <ColumnName("End Time (Optional)")>
        Public Property EndTime As TimeSpan?

        '    Get
        '        Return _endTime
        '    End Get
        '    Set(value As TimeSpan?)
        '        _endTime = value
        '    End Set
        'End Property

        'Public ReadOnly Property EndTimeDisplay As String
        '    Get
        '        Return ShortTimeSpan(_endTime)
        '    End Get
        'End Property

        <ColumnName("Start Date")>
        Public Property StartDate As Date?

        <ColumnName("End Date To (Optional)")>
        Public Property EndDate As Date?

        Public ReadOnly Property EndDateProper As Date
            Get
                If Not EndDate.HasValue Then
                    Return StartDate.Value.Date
                Else
                    Return EndDate.Value.Date
                End If
            End Get
        End Property

        <ColumnName("Reason")>
        Public Property Reason As String
            Get
                Return _reasons
            End Get
            Set(value As String)
                _reasons = Strings.Left(value, REASON_LENGTH)
            End Set
        End Property

        <ColumnName("Comment")>
        Public Property Comment As String
            Get
                Return _comments
            End Get
            Set(value As String)
                _comments = Strings.Left(value, COMMENT_LENGTH)
            End Set
        End Property

        Public ReadOnly Property ConsideredFailed As Boolean
            Get
                _noEmployeeNo = String.IsNullOrWhiteSpace(EmployeeNo)
                _noLeaveType = String.IsNullOrWhiteSpace(LeaveType)
                _noStartDate = Not StartDate.HasValue
                _employeeNotExists = Not If(_employee?.RowID.HasValue, False)

                _noStatus = String.IsNullOrWhiteSpace(Status) _
                    Or Not VALID_STATUS.Any(Function(s) s = Status.ToLower())

                _notMeantToUseAddtlVL = Not _grantsAdditionalVacationLeaveTypeFeaure And LeaveType = ADDITIONAL_VACATION_LEAVETYPE

                Return _noEmployeeNo _
                    Or _noLeaveType _
                    Or _noStartDate _
                    Or _employeeNotExists _
                    Or _noStatus _
                    Or _notMeantToUseAddtlVL
            End Get
        End Property

        Public ReadOnly Property FailureDescription
            Get
                Dim description As New List(Of String)

                If _noEmployeeNo Then description.Add("no Employee No")

                If _noLeaveType Then description.Add("no Leave Type")

                If _noStartDate Then description.Add("no Start Date")

                If _employeeNotExists Then description.Add("Employee doesn't belong here")

                If _noStatus Then description.Add("invalid status")

                If _notMeantToUseAddtlVL Then description.Add($"AccuPay doesn't support {ADDITIONAL_VACATION_LEAVETYPE} leave type")

                Dim stringsToJoin = description.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray()
                If Not stringsToJoin.Any() Then Return Nothing
                Return String.Join("; ", stringsToJoin)
            End Get
        End Property

        <Ignore>
        Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

        Private Function ShortTimeSpan(ts As TimeSpan?) As String
            If ts.HasValue Then
                Return ts.Value.ToString("hh\:mm")
            Else
                Return Nothing
            End If
        End Function

    End Class

#Region "EventHandlers"

    Private Sub ImportLeaveForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.AutoGenerateColumns = False
        DataGridView2.AutoGenerateColumns = False
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_DataSourceChanged(sender As Object, e As EventArgs) Handles DataGridView1.DataSourceChanged
        Dim count = DataGridView1.Rows.Count
        TabPage1.Text = $"OK ({count})"

        SaveButton.Enabled = count > 0
    End Sub

    Private Sub DataGridView2_DataSourceChanged(sender As Object, e As EventArgs) Handles DataGridView2.DataSourceChanged
        TabPage2.Text = $"Failed ({DataGridView2.Rows.Count})"

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Static fileFilter = String.Join("|", {"Microsoft Excel Workbook Documents 2007-13 (*.xlsx)", "*.xlsx", "Microsoft Excel Documents 97-2003 (*.xls)", "*.xls"})

        Dim browseFile = New OpenFileDialog With {
            .Filter = fileFilter,
            .RestoreDirectory = True
        }

        If Not browseFile.ShowDialog() = DialogResult.OK Then Return

        FileDirectory = browseFile.FileName

    End Sub

    Private Async Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        Dim fileInfo = Await DownloadTemplateHelper.DownloadWithData(ExcelTemplates.Leave)

        If fileInfo IsNot Nothing Then
            Using package As New ExcelPackage(fileInfo)
                Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets("Options")

                Dim leaveTypes
                Using context As New PayrollContext
                    Dim categleavID = Await context.Categories.
                                        Where(Function(c) Nullable.Equals(c.OrganizationID, z_OrganizationID)).
                                        Where(Function(c) Nullable.Equals(c.CategoryName, "Leave Type")).
                                        Select(Function(c) c.RowID).
                                        FirstOrDefaultAsync()

                    leaveTypes = Await context.Products.
                                    Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                    Where(Function(p) Nullable.Equals(p.CategoryID, categleavID)).
                                    Select(Function(p) p.PartNo).
                                    OrderBy(Function(p) p).
                                    ToListAsync()
                End Using

                For index = 0 To leaveTypes.Count - 1
                    worksheet.Cells(index + 2, 2).Value = leaveTypes(index)
                Next

                package.Save()

                Process.Start(fileInfo.FullName)
            End Using
        End If
    End Sub

#End Region

End Class