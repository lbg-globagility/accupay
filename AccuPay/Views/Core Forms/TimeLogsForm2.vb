Option Strict On

Imports System.IO
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.Services.Imports
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports log4net
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Public Class TimeLogsForm2

#Region "VariableDeclarations"

    Private Shared logger As ILog = LogManager.GetLogger("TimeLogsFormAppender")

    Private Const SUNDAY_SHORT_NAME As String = "Sun"

    Private Const FormEntityName As String = "Time Log"

    Private currRowIndex As Integer = -1
    Private currColIndex As Integer = -1

    Private thefilepath As String

    Private _useShiftSchedulePolicy As Boolean

    Private _originalDates As TimePeriod

    Private ReadOnly _employeeDutyScheduleRepository As EmployeeDutyScheduleRepository

    Private ReadOnly _employeeRepository As EmployeeRepository

    Private ReadOnly _overtimeRepository As OvertimeRepository

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Private ReadOnly _shiftScheduleRepository As ShiftScheduleRepository

    Private ReadOnly _userActivityRepo As UserActivityRepository

    Public Enum TimeLogsFormat
        Optimized = 0
        Conventional = 1
    End Enum

#End Region

    Sub New()

        InitializeComponent()

        _employeeDutyScheduleRepository = MainServiceProvider.GetRequiredService(Of EmployeeDutyScheduleRepository)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _overtimeRepository = MainServiceProvider.GetRequiredService(Of OvertimeRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

        _shiftScheduleRepository = MainServiceProvider.GetRequiredService(Of ShiftScheduleRepository)

        _userActivityRepo = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
    End Sub

#Region "Methods"

    Private Sub ResetCurrentCell()
        currRowIndex = -1
        currColIndex = -1
    End Sub

    Private Async Function ReloadAsync() As Task

        Dim startDate As Date = dtpDateFrom.Value.Date
        Dim endDate As Date = dtpDateTo.Value.Date

        Dim datePeriod = New TimePeriod(startDate, endDate)

        If startDate > endDate Then Return

        Me.Cursor = Cursors.WaitCursor

        MainSplitContainer.Enabled = False

        Dim hasSelectedEmployees = EmployeeTreeView1.GetTickedEmployees().Any()

        If Not hasSelectedEmployees Then
            grid.DataSource = Nothing

            ResetCurrentCell()

            MainSplitContainer.Enabled = True

            Me.Cursor = Cursors.Default

            Return
        End If

        Dim employeeIDs = EmployeeTreeView1.GetTickedEmployees.Select(Function(emp) emp.RowID.Value).ToArray()

        Dim employees = (Await _employeeRepository.GetByMultipleIdAsync(employeeIDs)).ToList()

        Dim shiftSchedules = Await _employeeDutyScheduleRepository.
                GetByMultipleEmployeeAndDatePeriodAsync(z_OrganizationID, employeeIDs, datePeriod)

        shiftSchedules = shiftSchedules.
                                Where(Function(s) s.StartTime.HasValue).
                                Where(Function(s) s.EndTime.HasValue).
                                ToList()

        Dim dataSource As New List(Of TimeLogModel)

        Dim models = CreatedResults(employees, startDate, endDate)

        Dim timeLogRepository = MainServiceProvider.GetRequiredService(Of TimeLogRepository)

        Dim timeLogs = Await timeLogRepository.
                            GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(employeeIDs, datePeriod)

        For Each model In models
            Dim seek = timeLogs.
                Where(Function(etd) etd.EmployeeID.Value = model.EmployeeID).
                Where(Function(etd) etd.LogDate = model.DateIn)

            Dim seekShiftSched = shiftSchedules.
                Where(Function(ss) ss.EmployeeID.Value = model.EmployeeID).
                Where(Function(ss) ss.DateSched = model.DateIn)

            Dim hasShiftSched = seekShiftSched.Any()

            If seek.Any Then
                Dim timeLog = seek.
                                OrderByDescending(Function(t) t.LastUpd).
                                FirstOrDefault

                If hasShiftSched Then
                    dataSource.Add(New TimeLogModel(timeLog) With {.ShiftSchedule = seekShiftSched.FirstOrDefault})
                    Continue For
                End If
                dataSource.Add(New TimeLogModel(timeLog))
            Else
                If hasShiftSched Then model.ShiftSchedule = seekShiftSched.FirstOrDefault
                dataSource.Add(model)
            End If
        Next

        RefreshDataSource(grid, dataSource)

        MainSplitContainer.Enabled = True

        Me.Cursor = Cursors.Default
    End Function

    Private Sub RefreshDataSource(datagGrid As DataGridView, dataSource As List(Of TimeLogModel))
        datagGrid.DataSource = dataSource
        datagGrid.Refresh()
    End Sub

    Private Sub ColoriseSundayRows(gridRows As DataGridViewRowCollection)
        Dim rows = gridRows.OfType(Of DataGridViewRow)

        Dim sundayRows = rows.Where(Function(row) Convert.ToString(row.Cells(colDay.Name).FormattedValue) = SUNDAY_SHORT_NAME)

        Dim count = sundayRows.Count

        For Each row In sundayRows
            row.DefaultCellStyle.ForeColor = Color.Red
        Next
    End Sub

    Private Sub HasChangedRow(gridRow As DataGridViewRow)
        gridRow.DefaultCellStyle.Font = New Font("Segoe UI", 8.25!, FontStyle.Bold, GraphicsUnit.Point, CType(0, Byte))
    End Sub

    Private Sub HasNoChangedRow(gridRow As DataGridViewRow)
        gridRow.DefaultCellStyle.Font = Nothing
    End Sub

    Private Sub BindGridCurrentCellChanged()
        AddHandler grid.CurrentCellChanged, AddressOf grid_CurrentCellChanged
    End Sub

    Private Sub UnbindGridCurrentCellChanged()
        RemoveHandler grid.CurrentCellChanged, AddressOf grid_CurrentCellChanged
    End Sub

    Private Sub ZebraliseEmployeeRows()
        Dim ebonyStyle = Color.LightGray
        Dim ivoryStyle = Color.White

        Dim groupEmployeeIDs = GridRowToTimeLogModels(grid).
            GroupBy(Function(ssm) ssm.EmployeeID).
            ToList()

        Dim isEven = False
        Dim i = 1

        For Each eID In groupEmployeeIDs
            isEven = i Mod 2 = 0

            Dim employeePrimKey = eID.FirstOrDefault.EmployeeID

            If isEven Then
                ColorEmployeeRows(employeePrimKey, ebonyStyle)
            Else
                ColorEmployeeRows(employeePrimKey, ivoryStyle)
            End If

            i += 1
        Next

    End Sub

    Private Sub ColorEmployeeRows(employeePrimKey As Integer, backColor As Color)
        Dim employeeGridRows =
                grid.Rows.OfType(Of DataGridViewRow).
                Where(Function(row) _
                          Equals(Convert.ToInt32(row.Cells(colEmployeeID.Name).Value),
                                 employeePrimKey))

        For Each eGridRow In employeeGridRows
            eGridRow.DefaultCellStyle.BackColor = backColor
        Next
    End Sub

    Private Sub ToSaveCountChanged()
        Dim models = GridRowToTimeLogModels(grid)

        Dim countHasChanged = models.Where(Function(tlm) tlm.HasChanged).ToList()
        labelChangedCount.Text = countHasChanged.Count.ToString("#,##0")
    End Sub

    Private Sub ShowSuccessBalloon()
        InfoBalloon("Successfully saved.",
                  "Successfully saved.", btnSave, 0, -70)
    End Sub

    Private Sub ShowSuccessImportBalloon()
        InfoBalloon("Imported successfully.",
                  "Imported successfully.", btnImport, 0, -70)
    End Sub

    Private Function GetShiftSchedulePolicy() As Boolean
        Dim listOfValueService = MainServiceProvider.GetRequiredService(Of ListOfValueService)
        Dim settings = listOfValueService.Create()
        Dim policy = New TimeEntryPolicy(settings)
        Return policy.UseShiftSchedule
    End Function

    Private Async Sub NewTimeEntryAlternateLineImport()
        Dim importer = MainServiceProvider.GetRequiredService(Of TimeLogsReader)
        Dim importOutput = importer.Read(thefilepath)

        If importOutput.IsImportSuccess = False Then
            MessageBox.Show(importOutput.ErrorMessage)
            Return
        End If

        Dim logs = importOutput.Logs.ToList()

        If logs.Count = 0 Then
            MessageBox.Show("No logs were parsed. Please make sure the log files follows the right format.")
            Return
        End If

        Dim timeLogsImportParser = MainServiceProvider.GetRequiredService(Of TimeLogImportParser)
        Dim timeAttendanceHelper As ITimeAttendanceHelper = Await timeLogsImportParser.
                                        GetHelper(logs, organizationId:=z_OrganizationID, userId:=z_User)

        'preview the logs here
        Dim previewDialog As New _
            TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog(timeAttendanceHelper, importOutput.Errors)

        With previewDialog
            .ShowDialog()
            .BringToFront()
        End With

        If previewDialog.Cancelled Then
            Return
        End If

        HouseKeepingBeforeStartBackgroundWork()
        bgworkTypicalImport.RunWorkerAsync(timeAttendanceHelper)
    End Sub

    Private Sub bgworkTypicalImport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkTypicalImport.DoWork
        Dim timeAttendanceHelper = CType(e.Argument, ITimeAttendanceHelper)

        NewTimeEntryAlternateLineImportSave(timeAttendanceHelper)

    End Sub

    Private Sub bgworkTypicalImport_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkTypicalImport.ProgressChanged
        ToolStripProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackGroundWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) _
        Handles bgworkTypicalImport.RunWorkerCompleted, bgworkImport.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MessageBox.Show("Error: " & e.Error.Message)

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")
        Else

            ShowSuccessImportBalloon()

            'This codes work even if it is not awaited.
            'Using async and await does not update the UI.
            'Tried using ConfigureAwait(False) and .Wait() but it did not work
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
            ReloadAsync()
#Enable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed

        End If

        MainSplitContainer.Enabled = True

        ToolStripProgressBar1.Visible = False

        ToolStripProgressBar1.Value = 0
    End Sub

    Private Async Sub NewTimeEntryAlternateLineImportSave(timeAttendanceHelper As ITimeAttendanceHelper)
        Try
            Dim timeLogs = timeAttendanceHelper.GenerateTimeLogs()
            Dim timeAttendanceLogs = timeAttendanceHelper.GenerateTimeAttendanceLogs()

            Dim timeLogService = MainServiceProvider.GetRequiredService(Of TimeLogDataService)
            Await timeLogService.SaveImportAsync(timeLogs, timeAttendanceLogs)

            Dim importList = New List(Of UserActivityItem)
            For Each log In timeLogs
                importList.Add(New UserActivityItem() With
                    {
                    .Description = $"Imported a new {FormEntityName.ToLower()}.",
                    .EntityId = CInt(log.RowID)
                    })
            Next

            _userActivityRepo.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeImport, importList)
        Catch ex As Exception

            logger.Error("NewTimeEntryAlternateLineImport", ex)

            MessageBoxHelper.DefaultErrorMessage("Import Logs")

            Throw ex
        End Try

    End Sub

    Private Sub ResetGridRowsDefaultCellStyle()
        For Each row As DataGridViewRow In grid.Rows
            row.DefaultCellStyle = Nothing
        Next
    End Sub

    Private Sub ReAssignLastSelectedCell()
        If currRowIndex < grid.Rows.Count _
                    And currColIndex > 0 Then
            grid.CurrentCell = grid.Item(currColIndex, currRowIndex)
        End If
    End Sub

    Private Sub RecordUpdate(updatedTimeLogs As List(Of TimeLog), oldRecords As IEnumerable(Of TimeLog))
        For Each item In updatedTimeLogs
            Dim changes As New List(Of UserActivityItem)
            Dim entityName = FormEntityName.ToLower()
            Dim oldValue = oldRecords.
                Where(Function(tl) tl.EmployeeID.Value = item.EmployeeID.Value).
                Where(Function(tl) tl.LogDate = item.LogDate).
                FirstOrDefault()

            If Not Nullable.Equals(item.TimeIn, oldValue.TimeIn) Then
                changes.Add(New UserActivityItem() With
                    {
                    .EntityId = CInt(item.RowID),
                    .Description = $"Updated {entityName} time in from '{oldValue.TimeIn}' to '{item.TimeIn}' on '{oldValue.LogDate.ToShortDateString}'."
                    })
            End If
            If Not Nullable.Equals(item.TimeOut, oldValue.TimeOut) Then
                changes.Add(New UserActivityItem() With
                    {
                    .EntityId = CInt(item.RowID),
                    .Description = $"Updated {entityName} time out from '{oldValue.TimeOut}' to '{item.TimeOut}' on '{oldValue.LogDate.ToShortDateString}'."
                    })
            End If
            If item.TimeStampOut.NullableEquals(oldValue.TimeStampOut) = False Then
                'TimeStampOut is null by default. It means TimeStampOut is equals to LogDate
                Dim dontSave = oldValue.TimeStampOut Is Nothing AndAlso
                                item.TimeStampOut IsNot Nothing AndAlso
                                item.TimeStampOut.Value.Date = item.LogDate.Date

                If dontSave = False Then

                    changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(item.RowID),
                        .Description = $"Updated {entityName} date out from '{oldValue.TimeStampOut.ToShortDateString}' to '{item.TimeStampOut.ToShortDateString}' on '{oldValue.LogDate.ToShortDateString}'."
                        })

                End If
            End If
            If Not Nullable.Equals(item.BranchID, oldValue.BranchID) Then
                Dim branches As List(Of Branch) = CType(colBranchID.DataSource, List(Of Branch))
                Dim oldBranch = ""
                Dim newBranch = ""

                If oldValue.BranchID.HasValue Then
                    oldBranch = branches.Where(Function(x) x.RowID.Value = oldValue.BranchID.Value).FirstOrDefault.Name
                End If
                If item.BranchID.HasValue Then
                    newBranch = branches.Where(Function(x) x.RowID.Value = item.BranchID.Value).FirstOrDefault.Name
                End If

                changes.Add(New UserActivityItem() With
                    {
                    .EntityId = CInt(item.RowID),
                    .Description = $"Updated {entityName} branch from '{oldBranch}' to '{newBranch}' on '{oldValue.LogDate.ToShortDateString}'."
                    })
            End If

            _userActivityRepo.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)
        Next
    End Sub

#End Region

#Region "Functions"

    Private Function CreatedResults(employees As List(Of Employee), startDate As Date, endDate As Date) As List(Of TimeLogModel)
        Dim returnList As New List(Of TimeLogModel)

        Dim dateRanges = Calendar.EachDay(startDate, endDate)

        Dim sortedEmployees = employees.OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst)

        For Each e In sortedEmployees
            For Each d In dateRanges
                returnList.Add(New TimeLogModel(e, d))
            Next
        Next

        Return returnList
    End Function

    Private Function GridRowToTimeLogModel(gridRow As DataGridViewRow) As TimeLogModel
        Return DirectCast(gridRow.DataBoundItem, TimeLogModel)
    End Function

    Private Function GridRowToTimeLogModels(dataGrid As DataGridView) As IList(Of TimeLogModel)
        Return dataGrid.Rows.OfType(Of DataGridViewRow).Select(Function(row) GridRowToTimeLogModel(row)).ToList()
    End Function

    Private Function TimeLogsImportOption() As TimeLogsFormat?

        Dim time_logformat As TimeLogsFormat?

        MessageBoxManager.Yes = "Alternating line"
        MessageBoxManager.No = "Same line"

        MessageBoxManager.Register()

        Dim custom_prompt =
            MessageBox.Show("Which format are you going to import ?",
                            "", MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1)

        If custom_prompt = Windows.Forms.DialogResult.Yes Then
            time_logformat = TimeLogsFormat.Conventional
        ElseIf custom_prompt = Windows.Forms.DialogResult.No Then
            time_logformat = TimeLogsFormat.Optimized
        ElseIf custom_prompt = Windows.Forms.DialogResult.Cancel Then
            time_logformat = Nothing
        End If

        MessageBoxManager.Unregister()

        Return time_logformat

    End Function

#End Region

#Region "PrivateClass"

    Private Class TimeLogModel

        Private Const CUSTOM_SHORT_TIME_FORMAT As String = "HH:mm"
        Public Property RowID As Integer
        Public Property ShiftSchedule As EmployeeDutySchedule
        Public Property DateIn As Date

        Private _timeLog As TimeLog
        Private _employee As Employee
        Private _dateOut, origDateIn, origDateOut As Date
        Private origTimeIn, origTimeOut As String
        Private _dateOutDisplay As Date?
        Private _timeIn, _timeOut As String
        Private _branchId, origBranchId As Integer?

        Sub New(timeLog As TimeLog)
            _timeLog = timeLog

            With _timeLog
                RowID = .RowID.Value

                DateIn = .LogDate
                _dateOut = .LogDate

                origDateIn = .LogDate
                origDateOut = .LogDate

                Dim completeTimeLog = .TimeIn.HasValue And .TimeOut.HasValue
                If completeTimeLog Then
                    If .TimeStampIn.Value.Date < .TimeStampOut.Value.Date Then
                        Dim logOutDate = .TimeStampOut.Value.Date
                        DateOut = logOutDate
                        origDateOut = logOutDate
                    End If
                End If

                origTimeIn = TimeSpanToString(.TimeIn)
                origTimeOut = TimeSpanToString(.TimeOut)

                _timeIn = origTimeIn
                _timeOut = origTimeOut

                _branchId = .BranchID
                origBranchId = .BranchID
            End With

        End Sub

        Sub New(employee As Employee, d As Date)
            _employee = employee

            origBranchId = employee.BranchID
            BranchID = employee.BranchID

            origDateIn = d
            DateIn = d

            origDateOut = d
            DateOut = d

        End Sub

        Public ReadOnly Property EmployeeID As Integer
            Get
                Return If(_timeLog?.Employee?.RowID.Value, _employee.RowID.Value)
            End Get
        End Property

        Public ReadOnly Property EmployeeNo As String
            Get
                Return If(_timeLog?.Employee?.EmployeeNo, _employee.EmployeeNo)
            End Get
        End Property

        Public ReadOnly Property FullName As String
            Get
                Return If(_timeLog?.Employee?.FullNameWithMiddleInitialLastNameFirst, _employee.FullNameWithMiddleInitialLastNameFirst)
            End Get
        End Property

        Public ReadOnly Property EmployeeBranchID As Integer?
            Get
                Return If(_timeLog?.Employee?.BranchID, _employee.BranchID)
            End Get
        End Property

        Public ReadOnly Property ShiftScheduleText As String
            Get
                Dim hasShift = ShiftSchedule IsNot Nothing

                If Not hasShift Then Return Nothing

                'Dim timeShifts = {TimeSpanToString(ShiftSchedule.StartTime), TimeSpanToString(ShiftSchedule.EndTime)}
                'Dim properArray = timeShifts.Where(Function(shiftTime) Not String.IsNullOrWhiteSpace(shiftTime)).ToArray()
                'Return String.Join(" - ", properArray)

                Return $"{TimeSpanToString(ShiftSchedule.StartTime)} - {TimeSpanToString(ShiftSchedule.EndTime)}"
            End Get
        End Property

        Public Property TimeIn As String
            Get
                Return _timeIn
            End Get
            Set(value As String)
                _timeIn = value
            End Set
        End Property

        Public Property DateOut As Date
            Get
                Return _dateOut
            End Get
            Set(value As Date)
                _dateOut = value
                UpdateDateOutDisplay(_dateOut)
            End Set
        End Property

        Public Property DateOutDisplay As Date?
            Get
                Return _dateOutDisplay
            End Get
            Set(value As Date?)
                UpdateDateOutDisplay(value)
            End Set
        End Property

        Private Sub UpdateDateOutDisplay(value As Date?)
            If DateIn.Date >= _dateOut.Date Then
                _dateOutDisplay = Nothing
                _dateOut = DateIn.Date
            Else
                _dateOutDisplay = value
            End If
        End Sub

        Public Property TimeOut As String
            Get
                Return _timeOut
            End Get
            Set(value As String)
                _timeOut = value
            End Set
        End Property

        Public Property BranchID As Integer?
            Get
                Return _branchId
            End Get
            Set(value As Integer?)
                _branchId = value
            End Set
        End Property

        Public ReadOnly Property IsExisting As Boolean
            Get
                Return RowID > 0
            End Get
        End Property

        Public ReadOnly Property HasChanged As Boolean
            Get
                Dim differs = Not Equals(origDateIn, DateIn) _
                    OrElse Not Equals(origTimeIn, _timeIn) _
                    OrElse Not Equals(origDateOut, DateOut) _
                    OrElse Not Equals(origTimeOut, _timeOut) _
                    OrElse Not origBranchId.NullableEquals(_branchId)

                Return differs
            End Get
        End Property

        Public ReadOnly Property IsValidToSave As Boolean
            Get
                Dim hasLog = Not String.IsNullOrWhiteSpace(_timeIn) _
                    OrElse Not String.IsNullOrWhiteSpace(_timeOut) _
                    OrElse _branchId IsNot Nothing

                Return hasLog
            End Get
        End Property

        Public ReadOnly Property ConsideredDelete As Boolean
            Get
                Return Not IsValidToSave And IsExisting
            End Get
        End Property

        Public ReadOnly Property ToTimeLog As TimeLog
            Get
                If _timeLog Is Nothing Then
                    _timeLog = New TimeLog With {
                        .RowID = RowID,
                        .EmployeeID = EmployeeID,
                        .OrganizationID = z_OrganizationID,
                        .LogDate = DateIn,
                        .CreatedBy = z_User,
                        .Created = Now
                    }
                End If

                With _timeLog
                    .LastUpd = Now
                    .LastUpdBy = z_User

                    .TimeIn = Calendar.ToTimespan(TimeIn)
                    .TimeOut = Calendar.ToTimespan(TimeOut)
                    .BranchID = _branchId
                End With

                Return _timeLog
            End Get
        End Property

        Public Sub Restore()
            DateIn = origDateIn
            _timeIn = origTimeIn

            DateOut = origDateOut
            _timeOut = origTimeOut
            _branchId = origBranchId
        End Sub

        Public Sub Commit()
            origDateIn = DateIn
            origTimeIn = _timeIn

            origDateOut = DateOut
            origTimeOut = _timeOut
            origBranchId = _branchId

            Remove()
        End Sub

        Public Sub ClearLogTime()
            _timeIn = Nothing
            _timeOut = Nothing

            _branchId = Nothing
        End Sub

        Public Sub ClearLogDateOut()
            DateOut = DateIn
        End Sub

        Public Sub Added(primaryKey As Integer)
            RowID = primaryKey
        End Sub

        Public Sub Remove()
            If ConsideredDelete Then
                RowID = 0
                If _timeLog IsNot Nothing Then
                    _timeLog.RowID = Nothing
                End If
            End If
        End Sub

        Public Shared Function TimeSpanToString(timeSpan As TimeSpan?) As String
            If Not timeSpan.HasValue Then Return Nothing
            Return TimeUtility.ToDateTime(timeSpan.Value).Value.ToString(CUSTOM_SHORT_TIME_FORMAT)
        End Function

    End Class

#End Region

#Region "EventHandlers"

    Private Async Sub TimeLogsForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        grid.AutoGenerateColumns = False

        EmployeeTreeView1.SwitchOrganization(z_OrganizationID)

        BindGridCurrentCellChanged()

        _useShiftSchedulePolicy = GetShiftSchedulePolicy()

        Dim currentlyWorkedOnPayPeriod = Await _payPeriodRepository.GetCurrentPayPeriodAsync(z_OrganizationID)

        If currentlyWorkedOnPayPeriod IsNot Nothing Then

            dtpDateFrom.Value = currentlyWorkedOnPayPeriod.PayFromDate
            dtpDateTo.Value = currentlyWorkedOnPayPeriod.PayToDate

        End If

        Await PopulateBranchComboBox()

    End Sub

    Private Async Function PopulateBranchComboBox() As Task

        colBranchID.ValueMember = "RowID"
        colBranchID.DisplayMember = "Name"

        Dim branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)
        colBranchID.DataSource = Await branchRepository.GetAllAsync

    End Function

    Private Sub TimeLogsForm2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
        InfoBalloon(, , lblFormTitle, , , 1)
    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Close()

    End Sub

    Private Async Sub btnSave_ClickAsync(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim models = GridRowToTimeLogModels(grid)

        Dim toSaveList = models.Where(Function(tlm) tlm.HasChanged).ToList()
        Dim toSaveListEmployeeIDs = toSaveList.Select(Function(tlm) tlm.EmployeeID).ToArray()

        Dim datePeriod = New TimePeriod(dtpDateFrom.Value.Date, dtpDateTo.Value.Date)

        Dim timeLogRepositoryQuery = MainServiceProvider.GetRequiredService(Of TimeLogRepository)
        Dim existingRecords = Await timeLogRepositoryQuery.
            GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(toSaveListEmployeeIDs, datePeriod)
        addDefaultBranchIDs(existingRecords)
        Dim oldRecords = existingRecords.CloneJson()

        Dim addedTimeLogs As New List(Of TimeLog)
        Dim updatedTimeLogs As New List(Of TimeLog)
        Dim deletedTimeLogs As New List(Of TimeLog)

        For Each model In toSaveList
            Dim seek = existingRecords.
                Where(Function(tl) tl.EmployeeID.Value = model.EmployeeID).
                Where(Function(tl) tl.LogDate = model.DateIn).
                OrderByDescending(Function(tl) tl.LastUpd)

            Dim exists = seek.Any
            Dim timeLog = seek.FirstOrDefault

            If model.ConsideredDelete Then

                deletedTimeLogs.Add(timeLog)

                Dim duplicateTimeLogs = Await timeLogRepositoryQuery.
                                        GetByEmployeeAndDateAsync(timeLog.EmployeeID.Value, timeLog.LogDate)

                deletedTimeLogs.AddRange(duplicateTimeLogs)

            ElseIf model.IsExisting Then
                Dim timeOut = Calendar.ToTimespan(model.TimeOut)

                timeLog.TimeIn = Calendar.ToTimespan(model.TimeIn)
                timeLog.TimeOut = timeOut

                If timeOut IsNot Nothing Then
                    timeLog.TimeStampOut = model.DateOut.ToMinimumHourValue.Add(Calendar.ToTimespan(model.TimeOut).Value)
                End If

                timeLog.BranchID = model.BranchID

                timeLog.LastUpdBy = z_User
                timeLog.LastUpd = Now

                updatedTimeLogs.Add(timeLog)
            ElseIf Not exists And model.IsValidToSave Then
                model.Remove()
                If model.BranchID Is Nothing Then
                    model.BranchID = model.EmployeeBranchID
                End If
                Dim addedTimeLog = model.ToTimeLog
                'addedTimeLog.RowID = Nothing
                addedTimeLog.RowID = Nothing
                addedTimeLogs.Add(addedTimeLog)
            End If

        Next

        Try
            Dim timeLogRepositorySave = MainServiceProvider.GetRequiredService(Of TimeLogRepository)
            Await timeLogRepositorySave.ChangeManyAsync(addedTimeLogs:=addedTimeLogs,
                                                     updatedTimeLogs:=updatedTimeLogs,
                                                     deletedTimeLogs:=deletedTimeLogs)

            CreateUserActivityRecords(oldRecords, addedTimeLogs, updatedTimeLogs, deletedTimeLogs)

            If addedTimeLogs.Any() Then
                Dim addedTimeLogEmployeeIDs = addedTimeLogs.Select(Function(tl) tl.EmployeeID.Value).ToArray()
                Dim addedTimeLogMinDate = addedTimeLogs.Min(Function(tl) tl.LogDate)
                Dim addedTimeLogMaxDate = addedTimeLogs.Max(Function(tl) tl.LogDate)

                Dim addedTimeLogsDatePeriod = New TimePeriod(addedTimeLogMinDate, addedTimeLogMaxDate)

                Dim newlyAdded = Await timeLogRepositoryQuery.
                    GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(addedTimeLogEmployeeIDs,
                                                                        addedTimeLogsDatePeriod)

                For Each model In toSaveList
                    Dim seek = newlyAdded.
                    Where(Function(tl) tl.EmployeeID.Value = model.EmployeeID).
                    Where(Function(tl) tl.LogDate = model.DateIn).
                    FirstOrDefault

                    If seek IsNot Nothing Then
                        model.Added(seek.RowID.Value)

                    ElseIf model.ConsideredDelete Then
                        model.Remove()

                    End If

                    model.Commit()
                Next
            Else
                For Each model In toSaveList
                    model.Remove()
                    model.Commit()
                Next

            End If

            ShowSuccessBalloon()

            ResetGridRowsDefaultCellStyle()

            ColoriseSundayRows(grid.Rows)

            ZebraliseEmployeeRows()

            ToSaveCountChanged()
        Catch ex As Exception
            logger.Error("TimeLogsForm2Saving", ex)
            Dim errMsg = String.Concat("Oops! something went wrong, please", Environment.NewLine, "contact ", My.Resources.AppCreator, " for assistance.")
            MessageBox.Show(errMsg, "Help", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub addDefaultBranchIDs(records As IEnumerable(Of TimeLog))
        For Each record In records

            If record.BranchID Is Nothing Then
                record.BranchID = record.Employee?.BranchID
            End If

        Next
    End Sub

    Private Sub CreateUserActivityRecords(oldRecords As IEnumerable(Of TimeLog), addedTimeLogs As List(Of TimeLog), updatedTimeLogs As List(Of TimeLog), deletedTimeLogs As List(Of TimeLog))
        For Each item In addedTimeLogs      'for new
            _userActivityRepo.RecordAdd(z_User, FormEntityName, item.RowID.Value, z_OrganizationID)
        Next

        RecordUpdate(updatedTimeLogs, oldRecords)

        Dim groupedDeletedTimeLogs = deletedTimeLogs.GroupBy(Function(x) New With {Key x.EmployeeID, Key x.LogDate}).
                                                    Select(Function(x) x.First).ToArray

        For Each item In groupedDeletedTimeLogs
            _userActivityRepo.RecordDelete(z_User, FormEntityName, item.RowID.Value, z_OrganizationID)
        Next
    End Sub

    Private Sub btnDiscard_Click(sender As Object, e As EventArgs) Handles btnDiscard.Click
        UnbindGridCurrentCellChanged()

        Dim models = GridRowToTimeLogModels(grid)
        Dim changedModels = models.Where(Function(tlm) tlm.HasChanged).ToList()
        For Each model In models
            model.Restore()
        Next

        RefreshDataSource(grid, models.ToList())

        labelChangedCount.Text = "0"

        BindGridCurrentCellChanged()
    End Sub

    Private Sub btnDeleteAll_Click(sender As Object, e As EventArgs) Handles btnDeleteAll.Click

        UnbindGridCurrentCellChanged()

        Dim models = GridRowToTimeLogModels(grid)
        For Each model In models
            model.ClearLogTime()
            model.ClearLogDateOut()
        Next

        RefreshDataSource(grid, models.ToList())

        ToSaveCountChanged()

        BindGridCurrentCellChanged()

    End Sub

    Private Sub DateFilter_Enter(sender As Object, e As EventArgs) Handles dtpDateFrom.Enter, dtpDateTo.Enter

        _originalDates = New TimePeriod(dtpDateFrom.Value, dtpDateTo.Value)

    End Sub

    Private Async Sub DateFilter_LeaveChangedAsync(sender As Object, e As EventArgs) _
        Handles dtpDateFrom.Leave, dtpDateTo.Leave

        If _originalDates.Equals(New TimePeriod(dtpDateFrom.Value, dtpDateTo.Value)) Then
            Return
        End If

        Dim dtp = DirectCast(sender, DateTimePicker)

        Dim start As Date = dtpDateFrom.Value.Date
        Dim finish As Date = dtpDateTo.Value.Date

        If start > finish Then
            If dtp.Name = dtpDateFrom.Name Then dtpDateTo.Value = start
            If dtp.Name = dtpDateTo.Name Then dtpDateFrom.Value = finish

        End If

        Await ReloadAsync()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles labelChangedCount.Click

    End Sub

    Private Sub Label1_TextChanged(sender As Object, e As EventArgs) Handles labelChangedCount.TextChanged
        Dim isZero = labelChangedCount.Text = "0"
        Dim hideBool = Not isZero

        Panel3.Visible = hideBool

    End Sub

    Private Sub grid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellContentClick
        Dim incremDecremButtonIndexes = {colDecrement.Index, colIncrement.Index}

        Dim satisfied = incremDecremButtonIndexes.Any(Function(i) i = e.ColumnIndex) _
            Or e.ColumnIndex = colDelete.Index _
            Or e.ColumnIndex = colRestore.Index

        If Not satisfied Then Return

        grid.EndEdit()

        Dim currRow = grid.Rows(e.RowIndex)
        Dim model = GridRowToTimeLogModel(currRow)

        If incremDecremButtonIndexes.Any(Function(i) i = e.ColumnIndex) Then
            Dim arithmeticOperand = If(e.ColumnIndex = colDecrement.Index, -1, 1)

            model.DateOut = model.DateOut.AddDays(arithmeticOperand)

            If model.HasChanged Then
                HasChangedRow(currRow)
            Else
                HasNoChangedRow(currRow)
            End If

            ToSaveCountChanged()

        ElseIf e.ColumnIndex = colDelete.Index Then
            model.ClearLogTime()

            If model.HasChanged Then HasChangedRow(currRow)

        ElseIf e.ColumnIndex = colRestore.Index Then
            model.Restore()

            If Not model.HasChanged Then HasNoChangedRow(currRow)

        End If

        ToSaveCountChanged()

        grid.Refresh()

        grid.ResumeLayout()

    End Sub

    Private Async Sub EmployeeTreeView1_TickedEmployee(s As Object, e As EventArgs) Handles EmployeeTreeView1.TickedEmployee

        Await ReloadAsync()

    End Sub

    Private Sub grid_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellEndEdit
        Dim parseableIndexes = {colTimeIn.Index, colTimeOut.Index, colBranchID.Index}

        If parseableIndexes.Any(Function(i) i = e.ColumnIndex) Then
            Dim currRow = grid.Rows(e.RowIndex)
            Dim model = GridRowToTimeLogModel(currRow)

            If e.ColumnIndex = colTimeIn.Index Then
                Dim fdsfsd = TimeLogModel.TimeSpanToString(Calendar.ToTimespan(model.TimeIn))
                model.TimeIn = fdsfsd
            ElseIf e.ColumnIndex = colTimeOut.Index Then
                model.TimeOut = TimeLogModel.TimeSpanToString(Calendar.ToTimespan(model.TimeOut))
            End If

            If model.HasChanged Then
                HasChangedRow(currRow)
            Else
                HasNoChangedRow(currRow)
            End If

            ToSaveCountChanged()
        End If
    End Sub

    Private Sub grid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles grid.DataError

    End Sub

    Private Sub grid_DataSourceChanged(sender As Object, e As EventArgs) Handles grid.DataSourceChanged
        ZebraliseEmployeeRows()

        ColoriseSundayRows(grid.Rows)

        ReAssignLastSelectedCell()

    End Sub

    Private Sub grid_CurrentCellChanged(sender As Object, e As EventArgs)
        If grid.CurrentRow IsNot Nothing Then
            currRowIndex = grid.CurrentRow.Index
            currColIndex = grid.CurrentCell.ColumnIndex
        Else
            ResetCurrentCell()
        End If

    End Sub

    Private Sub grid_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles grid.ColumnWidthChanged
        Console.WriteLine("{0} : {1}", {e.Column.Name, e.Column.Width})
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click

        Static employeeleaveRowID As Integer = -1

        Dim timeLogsFormat_ As TimeLogsFormat? = TimeLogsImportOption()

        'They chose Cancel or used the close button
        If timeLogsFormat_ Is Nothing Then Return

        Try
            Dim browsefile As OpenFileDialog = New OpenFileDialog()
            browsefile.Filter = "Text Documents (*.txt)|*.txt" &
                                "|All files (*.*)|*.*"

            If browsefile.ShowDialog = Windows.Forms.DialogResult.OK Then

                thefilepath = browsefile.FileName

                If timeLogsFormat_ = TimeLogsFormat.Conventional Then
                    NewTimeEntryAlternateLineImport()
                Else
                    HouseKeepingBeforeStartBackgroundWork()
                    bgworkImport.RunWorkerAsync()
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message & " Error on file initialization")
        Finally

        End Try

    End Sub

    Private Async Sub bgworkImport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkImport.DoWork

        Dim parser = New TimeInTimeOutParser()
        Dim parsedTimeLogs = parser.Parse(thefilepath)

        If parsedTimeLogs.Count = 0 Then
            MessageBox.Show("No logs were parsed. Please make sure the log files follows the right format.")
            Return
        End If

        Dim parsedTimeLogsGroupedByEmployee = parsedTimeLogs.
            GroupBy(Function(t) t.EmployeeNo).
            ToList()

        Dim employeeNos = parsedTimeLogsGroupedByEmployee.Select(Function(emp) emp.Key).ToArray()
        Dim employees = Await _employeeRepository.GetByMultipleEmployeeNumberAsync(employeeNos, z_OrganizationID)

        Dim dateCreated = Date.Now

        Dim timeLogs As New List(Of TimeLog)
        For Each timeLogList In parsedTimeLogsGroupedByEmployee
            Dim employee = employees.
                Where(Function(et) et.EmployeeNo = timeLogList.Key).
                Where(Function(et) Nullable.Equals(et.OrganizationID, z_OrganizationID)).
                FirstOrDefault()

            If employee Is Nothing Then
                Continue For
            End If

            For Each timeLog In timeLogList
                Dim t = New TimeLog() With {
                    .OrganizationID = z_OrganizationID,
                    .EmployeeID = employee.RowID,
                    .Created = dateCreated,
                    .CreatedBy = z_User,
                    .LogDate = timeLog.DateOccurred,
                    .BranchID = employee.BranchID
                }

                If Not String.IsNullOrWhiteSpace(timeLog.TimeIn) Then
                    t.TimeIn = TimeSpan.Parse(timeLog.TimeIn)
                End If

                If Not String.IsNullOrWhiteSpace(timeLog.TimeOut) Then
                    t.TimeOut = TimeSpan.Parse(timeLog.TimeOut)
                End If

                timeLogs.Add(t)
            Next
        Next

        'this should also create TimeAttendanceLogs per log
        Dim timeLogService = MainServiceProvider.GetRequiredService(Of TimeLogDataService)
        Await timeLogService.SaveImportAsync(timeLogs)

        Return

    End Sub

    Private Sub HouseKeepingBeforeStartBackgroundWork()

        ToolStripProgressBar1.Value = 0

        MainSplitContainer.Enabled = False

        ToolStripProgressBar1.Visible = True
    End Sub

    Private Sub grid_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellMouseEnter
        If Not e.RowIndex > -1 Then grid.Cursor = Cursors.Default : Return

        Dim model = GridRowToTimeLogModel(grid.Rows(e.RowIndex))

        Dim hasChanged = model.HasChanged

        Dim satsified = e.ColumnIndex = colDelete.Index And model.IsValidToSave _
            Or e.ColumnIndex = colRestore.Index And hasChanged

        If satsified Then
            grid.Cursor = Cursors.Hand
        Else
            grid.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub grid_CellMouseLeave(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellMouseLeave
        If e.ColumnIndex = colDelete.Index Then grid.Cursor = Cursors.Default
    End Sub

    Private Sub ToolStripLabel1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        Try
            Dim saveFileDialog = New SaveFileDialog()
            saveFileDialog.FileName = z_CompanyName & "_" & dtpDateFrom.Value.ToString("MM'-'dd'-'yyyy") & "_" & dtpDateTo.Value.ToString("MM'-'dd'-'yyyy")
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx"

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                Dim fileName = saveFileDialog.FileName
                Dim file = New FileInfo(fileName)

                If file.Exists() Then

                    file.Delete()
                End If

                Using excelPackage = New ExcelPackage(file)
                    Dim worksheet = excelPackage.Workbook.Worksheets.Add("Time logs")
                    worksheet.Column(5).Style.Numberformat.Format = "mm/dd/yyyy"
                    worksheet.Column(7).Style.Numberformat.Format = "mm/dd/yyyy"

                    worksheet.Cells("A1").Value = "Employee ID"
                    worksheet.Cells("B1").Value = "Name"
                    worksheet.Cells("C1").Value = "Shift Schedule"
                    worksheet.Cells("D1").Value = "Time In"
                    worksheet.Cells("E1").Value = "Date In"
                    worksheet.Cells("F1").Value = "Time Out"
                    worksheet.Cells("G1").Value = "Date Out"

                    Dim i = 2
                    For Each row As DataGridViewRow In grid.Rows
                        If row.Cells(7).Value IsNot Nothing Or row.Cells(12).Value IsNot Nothing Then
                            worksheet.Cells($"A{i}").Value = row.Cells(2).Value
                            worksheet.Cells($"B{i}").Value = row.Cells(3).Value
                            worksheet.Cells($"C{i}").Value = row.Cells(4).Value
                            worksheet.Cells($"D{i}").Value = row.Cells(7).Value
                            worksheet.Cells($"E{i}").Value = row.Cells(6).Value
                            worksheet.Cells($"F{i}").Value = row.Cells(12).Value
                            worksheet.Cells($"G{i}").Value = row.Cells(8).Value
                            i += 1
                        End If
                    Next

                    excelPackage.Save()
                    Process.Start(fileName)
                    'MsgBox("Time entry logs has been exported.", MsgBoxStyle.OkOnly, "Exported time entry logs")
                End Using
            End If
        Catch ex As IOException

            MessageBoxHelper.ErrorMessage(ex.Message)
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage()

        End Try

    End Sub

    Private Sub btnUserActivity_Click(sender As Object, e As EventArgs) Handles btnUserActivity.Click
        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

#End Region

End Class