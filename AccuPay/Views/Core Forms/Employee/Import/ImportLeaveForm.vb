Imports System.Threading.Tasks
Imports AccuPay
Imports AccuPay.Attributes
Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Globagility.AccuPay
Imports log4net
Imports Microsoft.EntityFrameworkCore

Public Class ImportLeaveForm
    Private Shared logger As ILog = LogManager.GetLogger("EmployeeFormAppender")

    Private _filePath As String
    Private _worksheetName As String
    Private _ep As ExcelParser(Of LeaveModel)
    Private _okModels As List(Of LeaveModel)
    Private _failModels As List(Of LeaveModel)

#Region "VariableDeclarations"

#End Region

#Region "Contructors"

    Public Sub New(filePath As String)
        InitializeComponent()

        ExcelParserPreparation(filePath)

    End Sub

    Public Sub New(filePath As String, worksheetName As String)
        InitializeComponent()

        ExcelParserPreparation(filePath, worksheetName)

    End Sub

#End Region

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

    Private Sub ExcelParserPreparation(filePath As String, Optional worksheetName As String = Nothing)

        _filePath = filePath

        If Not String.IsNullOrWhiteSpace(worksheetName) Then
            _worksheetName = worksheetName
            _ep = New ExcelParser(Of LeaveModel)(_worksheetName)
        End If

        _ep = New ExcelParser(Of LeaveModel)
    End Sub

    Private Async Sub FilePathChangedAsync()
        Dim models = _ep.Read(_filePath)

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
            .StartTime = model.StartTime}
    End Function

    Public Async Function SaveAsync() As Task(Of Boolean)
        Dim succeed As Boolean = False

        If Not _okModels.Any() Then Return succeed

        Using context = New PayrollContext
            Dim employeeIDs = _okModels.Select(Function(lm) lm.EmployeeID).ToList()
            Dim minDate = _okModels.Min(Function(lm) lm.StartDate.Value.Date)
            Dim maxDate = _okModels.Min(Function(lm) lm.EndDateProper)

            Dim leaves = Await context.Leaves.
                Where(Function(lv) lv.OrganizationID = z_OrganizationID).
                Where(Function(lv) employeeIDs.Contains(lv.EmployeeID.Value)).
                Where(Function(lv) lv.StartDate >= minDate AndAlso lv.EndDate.Value.Date <= maxDate).
                ToListAsync()

            If leaves.Any() Then
                For Each model In _okModels
                    Dim leave = leaves.
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
                    Else
                        context.Leaves.Add(CreateNewLeave(model))
                    End If
                Next

            Else
                For Each model In _okModels
                    context.Leaves.Add(CreateNewLeave(model))
                Next

            End If

            Try
                Await context.SaveChangesAsync()

                succeed = True
            Catch ex As Exception
                succeed = False
                logger.Error("EmployeeImportLeave", ex)
                Dim errMsg = String.Concat("Oops! something went wrong, please", Environment.NewLine, "contact ", My.Resources.AppCreator, " for assistance.")
                MessageBox.Show(errMsg, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try
        End Using

        Return succeed
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

#Region "Functions"

#End Region

    Private Class LeaveModel
        Private Const PENDING_STATUS As String = "Pending"
        Private Const ADDITIONAL_VACATION_LEAVETYPE As String = "Additional VL"
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
        Private _startTime As TimeSpan?
        Private _endTime As TimeSpan?
        Private REASON_LENGTH As Integer = 500
        Private COMMENT_LENGTH As Integer = 2000
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

            _employee = employee

            EmployeeNo = _employee.EmployeeNo
            FullName = _employee.Fullname
            EmployeeID = _employee.RowID.Value

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

        <ColumnName("Start Time")>
        Public Property StartTime As TimeSpan?
            Get
                Return _startTime
            End Get
            Set(value As TimeSpan?)
                _startTime = value
            End Set
        End Property

        Public ReadOnly Property StartTimeDisplay As String
            Get
                Return ShortTimeSpan(_startTime)
            End Get
        End Property

        <ColumnName("End Time")>
        Public Property EndTime As TimeSpan?
            Get
                Return _endTime
            End Get
            Set(value As TimeSpan?)
                _endTime = value
            End Set
        End Property

        Public ReadOnly Property EndTimeDisplay As String
            Get
                Return ShortTimeSpan(_endTime)
            End Get
        End Property

        <ColumnName("Start Date")>
        Public Property StartDate As Date?

        <ColumnName("End Date (Optional)")>
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

                If _noEmployeeNo Then
                    description.Add("no Employee No")
                ElseIf _noLeaveType Then
                    description.Add("no Leave Type")
                ElseIf _noStartDate Then
                    description.Add("no Start Date")
                ElseIf _employeeNotExists Then
                    description.Add("Employee doesn't belong here")
                ElseIf _noStatus Then
                    description.Add("invalid status")
                ElseIf _notMeantToUseAddtlVL Then
                    description.Add($"AccuPay doesn't support {ADDITIONAL_VACATION_LEAVETYPE} leave type")
                End If

                Dim stringsToJoin = description.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray()
                If Not stringsToJoin.Any() Then Return Nothing
                Return String.Join("; ", stringsToJoin)
            End Get
        End Property

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

        FilePathChangedAsync()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_DataSourceChanged(sender As Object, e As EventArgs) Handles DataGridView1.DataSourceChanged
        Dim count = DataGridView1.Rows.Count
        TabPage1.Text = $"OK ({count})"

        Button1.Enabled = count > 0
    End Sub

    Private Sub DataGridView2_DataSourceChanged(sender As Object, e As EventArgs) Handles DataGridView2.DataSourceChanged
        TabPage2.Text = $"Failed ({DataGridView2.Rows.Count})"

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim browseFile = New OpenFileDialog With {
            .Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls",
            .RestoreDirectory = True
        }

        If Not browseFile.ShowDialog() = DialogResult.OK Then Return

        FileDirectory = browseFile.FileName

    End Sub

#End Region

End Class