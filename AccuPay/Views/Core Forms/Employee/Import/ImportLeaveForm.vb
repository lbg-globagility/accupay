Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Attributes
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Helpers
Imports AccuPay.Utils
Imports Globagility.AccuPay
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Public Class ImportLeaveForm

    Private Const FormEntityName As String = "Leave"
    Private _filePath As String
    Private _worksheetName As String
    Private _ep As New ExcelParser(Of LeaveModel)("Employee Leave")
    Private _okModels As List(Of LeaveModel)
    Private _failModels As List(Of LeaveModel)

    Private _employeeRepository As EmployeeRepository
    Private _productRepository As ProductRepository
    Private _userActivityRepository As UserActivityRepository

    Sub New(employeeRepository As EmployeeRepository,
            leaveRepository As LeaveRepository,
            productRepository As ProductRepository,
            userActivityRepository As UserActivityRepository,
            leaveService As LeaveService)

        InitializeComponent()

        _categoryRepository = MainServiceProvider.GetRequiredService(Of CategoryRepository)
        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
        _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)
        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

    End Sub

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

    Private Async Sub FilePathChangedAsync()
        Dim models As New List(Of LeaveModel)

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunctionAsync(
            Sub()
                models = _ep.Read(_filePath).ToList
            End Sub)

        If parsedSuccessfully = False Then Return

        Dim employeeNos = models.Select(Function(lm) lm.EmployeeNo).ToList()

        Dim dataSource As New List(Of LeaveModel)

        Dim employeeFromRepo = Await _employeeRepository.GetAllAsync(z_OrganizationID)
        Dim employees = employeeFromRepo.
            Where(Function(e) employeeNos.Contains(e.EmployeeNo)).
            ToList()

        For Each model In models

            Dim employee = employees.Where(Function(e) e.EmployeeNo = model.EmployeeNo).FirstOrDefault

            dataSource.Add(CreateLeaveModel(model, employee))
        Next

        _okModels = dataSource.Where(Function(ee) Not ee.ConsideredFailed).ToList()
        _failModels = dataSource.Where(Function(ee) ee.ConsideredFailed).ToList()
        DataGridView1.DataSource = _okModels
        DataGridView2.DataSource = _failModels

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

        Dim leaves As List(Of Leave) = Await GetOkLeaves()

        If leaves.Any = False Then

            MessageBoxHelper.Warning("No employee leaves to be added!")

            Return False

        End If

        Return Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                        Async Function() As Task(Of Boolean)

                            Dim leaveService = MainServiceProvider.GetRequiredService(Of LeaveService)
                            Await leaveService.SaveManyAsync(leaves,
                                                             z_OrganizationID)

                            Dim importList = New List(Of UserActivityItem)
                            Dim entityName = FormEntityName.ToLower()

                            For Each item In leaves

                                If item.IsNew Then
                                    importList.Add(New UserActivityItem() With
                                    {
                                    .Description = $"Imported a new {entityName}.",
                                    .EntityId = item.RowID.Value
                                    })
                                Else
                                    importList.Add(New UserActivityItem() With
                                    {
                                    .Description = $"Updated a {entityName} on import.",
                                    .EntityId = item.RowID.Value
                                    })
                                End If

                            Next

                            _userActivityRepository.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeImport, importList)

                            Return True
                        End Function)

        Return False

    End Function

    Private Async Function GetOkLeaves() As Task(Of List(Of Leave))

        Dim leaves As New List(Of Leave)

        Dim employeeIDs = _okModels.Select(Function(lm) lm.EmployeeID).ToList()
        Dim minDate = _okModels.Min(Function(lm) lm.StartDate.Value.Date)

        Dim leaveRepository = MainServiceProvider.GetRequiredService(Of LeaveRepository)
        Dim currentLeaves = Await leaveRepository.
                    GetFilteredAllAsync(Function(lv) lv.OrganizationID.Value = z_OrganizationID AndAlso
                                        employeeIDs.Contains(lv.EmployeeID.Value) AndAlso
                                        lv.StartDate >= minDate)

        If currentLeaves.Any() Then
            For Each model In _okModels
                Dim leave = currentLeaves.
                    Where(Function(lv) lv.EmployeeID.Value = model.EmployeeID).
                    Where(Function(lv) lv.StartDate = model.StartDate.Value.Date).
                    FirstOrDefault

                If leave IsNot Nothing Then
                    With leave
                        .Reason = model.Reason
                        .Comments = model.Comment
                        .StartTime = model.StartTime
                        .StartDate = model.StartDate.Value
                        .EndDate = model.EndDate
                        .EndTime = model.EndTime
                        .LastUpd = Now
                        .LastUpdBy = z_User
                        .IsNew = False
                    End With

                    leaves.Add(leave)
                Else
                    Dim newLeave = model.ToLeave()
                    newLeave.CreatedBy = z_User
                    leaves.Add(newLeave)
                End If
            Next
        Else
            For Each model In _okModels
                Dim newLeave = model.ToLeave()
                newLeave.CreatedBy = z_User
                leaves.Add(model.ToLeave())
            Next

        End If

        Return leaves
    End Function

#End Region

    Private Class LeaveModel
        Implements IExcelRowRecord
        Private Const PENDING_STATUS As String = Data.Entities.Leave.StatusPending
        Private Const ADDITIONAL_VACATION_LEAVETYPE As String = "Additional VL"
        Private Const REASON_LENGTH As Integer = 500
        Private Const COMMENT_LENGTH As Integer = 2000
        Private VALID_STATUS As String() = {Data.Entities.Leave.StatusApproved.ToLower, Data.Entities.Leave.StatusPending.ToLower} '{"approved", "pending"}
        Private _employee As Employee
        Private _noEmployeeNo As Boolean
        Private _noLeaveType As Boolean
        Private _noStartDate As Boolean
        Private _invalidStartDate As Boolean
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

        <ColumnName("End Time (Optional)")>
        Public Property EndTime As TimeSpan?

        <ColumnName("Start Date")>
        Public Property StartDate As Date?

        <ColumnName("Reason (Optional)")>
        Public Property Reason As String
            Get
                Return _reasons
            End Get
            Set(value As String)
                _reasons = Strings.Left(value, REASON_LENGTH)
            End Set
        End Property

        <ColumnName("Comment (Optional)")>
        Public Property Comment As String
            Get
                Return _comments
            End Get
            Set(value As String)
                _comments = Strings.Left(value, COMMENT_LENGTH)
            End Set
        End Property

        Public ReadOnly Property EndDate As Date?
            Get
                If StartDate.HasValue = False Then
                    Return Nothing
                ElseIf StartTime.HasValue = False OrElse EndTime.HasValue = False Then
                    Return StartDate
                ElseIf StartTime.HasValue = False AndAlso EndTime.HasValue = False Then
                    Return StartDate
                Else
                    Return If(EndTime < StartTime, StartDate.Value.AddDays(1), StartDate)
                End If
            End Get
        End Property

        Public ReadOnly Property ConsideredFailed As Boolean
            Get
                _noEmployeeNo = String.IsNullOrWhiteSpace(EmployeeNo)
                _noLeaveType = String.IsNullOrWhiteSpace(LeaveType)
                _noStartDate = Not StartDate.HasValue
                _invalidStartDate = _noStartDate = False AndAlso StartDate.Value < Data.Helpers.PayrollTools.MinimumMicrosoftDate
                _employeeNotExists = Not If(_employee?.RowID.HasValue, False)

                _noStatus = String.IsNullOrWhiteSpace(Status) _
                    Or Not VALID_STATUS.Any(Function(s) s = Status.ToLower())

                _notMeantToUseAddtlVL = Not _grantsAdditionalVacationLeaveTypeFeaure And LeaveType = ADDITIONAL_VACATION_LEAVETYPE

                Return _noEmployeeNo _
                    Or _noLeaveType _
                    Or _noStartDate _
                    Or _invalidStartDate _
                    Or _employeeNotExists _
                    Or _noStatus _
                    Or _notMeantToUseAddtlVL _
                    Or (_noStartDate = False AndAlso Not String.IsNullOrWhiteSpace(ToLeave().Validate))
            End Get
        End Property

        Public ReadOnly Property FailureDescription As String
            Get
                Dim description As New List(Of String)

                If _noEmployeeNo Then description.Add("no Employee No")

                If _noLeaveType Then description.Add("no Leave Type")

                If _noStartDate Then description.Add("no Start Date")

                If _invalidStartDate Then description.Add("dates cannot be earlier than January 1, 1753.")

                If _employeeNotExists Then description.Add("Employee doesn't belong here")

                If _noStatus Then description.Add("invalid status")

                If _notMeantToUseAddtlVL Then description.Add($"AccuPay doesn't support {ADDITIONAL_VACATION_LEAVETYPE} leave type")

                'add the model validation if there are no error message (there are checks that are here and in the model validation so only use the model validation if it there is no error detected here)
                If Not description.Any Then
                    Dim validationErrorMessage = ToLeave().Validate

                    If Not String.IsNullOrWhiteSpace(validationErrorMessage) Then

                        description.Add(validationErrorMessage)
                    End If

                End If

                Dim stringsToJoin = description.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray()
                If Not stringsToJoin.Any() Then Return Nothing
                Return String.Join("; ", stringsToJoin)
            End Get
        End Property

        <Ignore>
        Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

        Public Function ToLeave(Optional isNew As Boolean = True) As Leave

            Return New Leave With {
                .OrganizationID = z_OrganizationID,
                .StartDate = StartDate.Value,
                .EndDate = EndDate.Value,
                .StartTime = StartTime,
                .EndTime = EndTime,
                .EmployeeID = EmployeeID,
                .Status = Status,
                .LeaveType = LeaveType,
                .Reason = Reason,
                .Comments = Comment,
                .CreatedBy = z_User,
                .LastUpdBy = z_User,
                .IsNew = isNew
            }
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
        Static fileFilter As String = String.Join("|", {"Microsoft Excel Workbook Documents 2007-13 (*.xlsx)", "*.xlsx", "Microsoft Excel Documents 97-2003 (*.xls)", "*.xls"})

        Dim browseFile = New OpenFileDialog With {
            .Filter = fileFilter,
            .RestoreDirectory = True
        }

        If Not browseFile.ShowDialog() = DialogResult.OK Then Return

        FileDirectory = browseFile.FileName

    End Sub

    Private Async Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        Dim fileInfo = Await DownloadTemplateHelper.DownloadExcelWithData(ExcelTemplates.Leave,
                                                                        _employeeRepository)

        If fileInfo IsNot Nothing Then
            Using package As New ExcelPackage(fileInfo)
                Dim worksheet As ExcelWorksheet = package.Workbook.Worksheets("Options")

                Dim leaveTypes = (Await _productRepository.GetLeaveTypesAsync(z_OrganizationID)).
                            Select(Function(p) p.PartNo).
                            OrderBy(Function(p) p).
                            ToList()

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