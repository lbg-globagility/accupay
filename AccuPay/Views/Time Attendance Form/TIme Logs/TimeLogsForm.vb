Option Strict On

Imports System.IO
Imports System.Threading.Tasks
Imports AccuPay.AccuPay.Desktop.Helpers
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Exceptions
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Core.ValueObjects
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports log4net
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Public Class TimeLogsForm

#Region "VariableDeclarations"

    Private Shared logger As ILog = LogManager.GetLogger("TimeLogsFormAppender")

    Private Const SUNDAY_SHORT_NAME As String = "Sun"

    Private currRowIndex As Integer = -1
    Private currColIndex As Integer = -1

    Private thefilepath As String

    Private _originalDates As TimePeriod

    Private ReadOnly _policyHelper As IPolicyHelper

    Private ReadOnly _employeeRepository As IEmployeeRepository

    Private ReadOnly _overtimeRepository As IOvertimeRepository

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Private ReadOnly _shiftRepository As IShiftRepository

    Private _currentRolePermission As RolePermission

    Public Enum TimeLogsFormat
        Optimized = 0
        Conventional = 1
    End Enum

#End Region

    Sub New()

        InitializeComponent()

        _policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _overtimeRepository = MainServiceProvider.GetRequiredService(Of IOvertimeRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _shiftRepository = MainServiceProvider.GetRequiredService(Of IShiftRepository)
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

        Dim shifts = Await _shiftRepository.
                GetByEmployeeAndDatePeriodAsync(z_OrganizationID, employeeIDs, datePeriod)

        shifts = shifts.
            Where(Function(s) s.StartTime.HasValue).
            Where(Function(s) s.EndTime.HasValue).
            ToList()

        Dim dataSource As New List(Of TimeLogModel)

        Dim models = CreatedResults(employees, startDate, endDate)

        Dim timeLogRepository = MainServiceProvider.GetRequiredService(Of ITimeLogRepository)

        Dim timeLogs = Await timeLogRepository.
            GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(employeeIDs, datePeriod)

        For Each model In models
            Dim seek = timeLogs.
                Where(Function(etd) etd.EmployeeID.Value = model.EmployeeID).
                Where(Function(etd) etd.LogDate = model.DateIn)

            Dim seekShift = shifts.
                Where(Function(ss) ss.EmployeeID.Value = model.EmployeeID).
                Where(Function(ss) ss.DateSched = model.DateIn)

            Dim hasShift = seekShift.Any()

            If seek.Any Then
                'TODO: employeetimeentrydetails date should be unique so no query like this should be needed.
                Dim timeLog = seek.
                    OrderByDescending(Function(t) t.LastUpd).
                    FirstOrDefault

                If hasShift Then
                    dataSource.Add(New TimeLogModel(timeLog) With {.Shift = seekShift.FirstOrDefault})
                    Continue For
                End If
                dataSource.Add(New TimeLogModel(timeLog))
            Else
                If hasShift Then model.Shift = seekShift.FirstOrDefault
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
                  "Imported successfully.", dtpDateFrom, 0, -70)
    End Sub

    Private Async Function NewTimeEntryAlternateLineImport() As Task
        Dim importer = MainServiceProvider.GetRequiredService(Of ITimeLogsReader)
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

        Dim timeLogsImportParser = MainServiceProvider.GetRequiredService(Of ITimeLogImportParser)
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
    End Function

    Private Sub bgworkTypicalImport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkTypicalImport.DoWork
        Dim timeAttendanceHelper = CType(e.Argument, ITimeAttendanceHelper)

        NewTimeEntryAlternateLineImportSave(timeAttendanceHelper).GetAwaiter().GetResult()

    End Sub

    Private Sub bgworkTypicalImport_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkTypicalImport.ProgressChanged
        ToolStripProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackGroundWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) _
        Handles bgworkTypicalImport.RunWorkerCompleted, bgworkImport.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            Const MessageTitle As String = "Import Time logs"

            If e.Error.GetType() Is GetType(BusinessLogicException) Then

                MessageBoxHelper.ErrorMessage(e.Error.Message, MessageTitle)
            Else
                Debugger.Break()
                MessageBoxHelper.DefaultErrorMessage(MessageTitle, e.Error)
            End If

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

    Private Async Function NewTimeEntryAlternateLineImportSave(timeAttendanceHelper As ITimeAttendanceHelper) As Task
        Dim timeLogs = timeAttendanceHelper.GenerateTimeLogs()
        Dim timeAttendanceLogs = timeAttendanceHelper.GenerateTimeAttendanceLogs()

        Dim timeLogService = MainServiceProvider.GetRequiredService(Of ITimeLogDataService)
        Await timeLogService.SaveImportAsync(timeLogs, z_User)
    End Function

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
        Public Property Shift As Shift
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

                origTimeIn = TimeSpanToString(.TimeIn, DateIn)
                origTimeOut = TimeSpanToString(.TimeOut, DateOut)

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
                Return If(_timeLog?.Employee?.RowID, If(_timeLog?.EmployeeID, _employee.RowID.Value))
            End Get
        End Property

        Public ReadOnly Property EmployeeNo As String
            Get
                Return If(_timeLog?.Employee?.EmployeeNo, _employee?.EmployeeNo)
            End Get
        End Property

        Public ReadOnly Property FullName As String
            Get
                Return If(_timeLog?.Employee?.FullNameWithMiddleInitialLastNameFirst, _employee?.FullNameWithMiddleInitialLastNameFirst)
            End Get
        End Property

        Public ReadOnly Property EmployeeBranchID As Integer?
            Get
                Return If(_timeLog?.Employee?.BranchID, _employee?.BranchID)
            End Get
        End Property

        Public ReadOnly Property ShiftText As String
            Get
                Dim hasShift = Shift IsNot Nothing

                If Not hasShift Then Return Nothing

                Return $"{TimeSpanToString(Shift.StartTime, DateIn)} - {TimeSpanToString(Shift.EndTime, DateOut)}"
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
                        .LogDate = DateIn
                    }
                End If

                With _timeLog
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

        Public Shared Function TimeSpanToString(timeSpan As TimeSpan?, [date] As Date) As String
            If Not timeSpan.HasValue Then Return Nothing
            Return TimeUtility.ToDateTime(timeSpan.Value, [date]).Value.ToString(CUSTOM_SHORT_TIME_FORMAT)
        End Function

    End Class

#End Region

#Region "EventHandlers"

    Private Async Sub TimeLogsForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        grid.AutoGenerateColumns = False

        Await CheckRolePermissions()

        Await EmployeeTreeView1.SwitchOrganization(z_OrganizationID)

        BindGridCurrentCellChanged()

        Dim currentlyWorkedOnPayPeriod = Await _payPeriodRepository.GetOpenOrCurrentPayPeriodAsync(
            organizationId:=z_OrganizationID,
            currentUserId:=z_User)

        If currentlyWorkedOnPayPeriod IsNot Nothing Then

            dtpDateFrom.Value = currentlyWorkedOnPayPeriod.PayFromDate
            dtpDateTo.Value = currentlyWorkedOnPayPeriod.PayToDate

        End If

        Await PopulateBranchComboBox()

        colBranchID.Visible = _policyHelper.UseCostCenter

    End Sub

    Private Async Function CheckRolePermissions() As Task
        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.TIMELOG)

        ImportToolStripButton.Visible = True
        btnDeleteAll.Visible = True
        ActionPanel.Visible = True
        grid.ReadOnly = False

        If role.Success Then

            _currentRolePermission = role.RolePermission

            If Not _currentRolePermission.Create AndAlso Not _currentRolePermission.Update Then

                ImportToolStripButton.Visible = False

                If Not _currentRolePermission.Delete Then

                    btnDeleteAll.Visible = False
                    ActionPanel.Visible = False
                    grid.ReadOnly = True

                End If

            End If
        End If
    End Function

    Private Async Function PopulateBranchComboBox() As Task

        colBranchID.ValueMember = "Id"
        colBranchID.DisplayMember = "DisplayMember"

        Dim branchRepository = MainServiceProvider.GetRequiredService(Of IBranchRepository)
        Dim branches = (Await branchRepository.GetAllAsync).
            OrderBy(Function(b) b.Name).
            ToList()

        Dim branchLookUpItems = LookUpItem.Convert(
            branches,
            idPropertyName:="RowID",
            displayMemberPropertyName:="Name",
            hasDefaultItem:=True)

        colBranchID.DataSource = branchLookUpItems

    End Function

    Private Sub TimeLogsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
        InfoBalloon(, , lblFormTitle, , , 1)
    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles CloseToolStripButton.Click
        Close()

    End Sub

    Private Async Sub btnSave_ClickAsync(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim models = GridRowToTimeLogModels(grid)

        Dim toSaveList = models.Where(Function(tlm) tlm.HasChanged).ToList()

        If Not toSaveList.Any() Then

            MessageBoxHelper.Warning("No unsaved changes!")
            Return
        End If

        Dim toSaveListEmployeeIDs = toSaveList.Select(Function(tlm) tlm.EmployeeID).ToArray()
        Dim earliestDate = toSaveList.Min(Function(tlm) tlm.DateIn)
        Dim latestDate = toSaveList.Max(Function(tlm) tlm.DateIn)

        Dim datePeriod = New TimePeriod(earliestDate, latestDate)

        Dim timeLogRepositoryQuery = MainServiceProvider.GetRequiredService(Of ITimeLogRepository)
        Dim existingRecords = Await timeLogRepositoryQuery.
            GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(toSaveListEmployeeIDs, datePeriod)

        addDefaultBranchIDs(existingRecords)

        Dim addedTimeLogs As New List(Of TimeLog)
        Dim updatedTimeLogs As New List(Of TimeLog)
        Dim deletedTimeLogs As New List(Of TimeLog)

        For Each model In toSaveList
            'TODO: employeetimeentrydetails date should be unique so no query like this should be needed.
            Dim seek = existingRecords.
                Where(Function(tl) tl.EmployeeID.Value = model.EmployeeID).
                Where(Function(tl) tl.LogDate = model.DateIn).
                OrderByDescending(Function(tl) tl.LastUpd)

            Dim exists = seek.Any
            Dim timeLog = seek.FirstOrDefault

            If model.ConsideredDelete Then

                deletedTimeLogs.AddRange(seek.ToList())

            ElseIf model.IsExisting Then
                Dim timeOut = Calendar.ToTimespan(model.TimeOut)

                timeLog.TimeIn = Calendar.ToTimespan(model.TimeIn)
                timeLog.TimeOut = timeOut

                If timeOut IsNot Nothing Then
                    timeLog.TimeStampOut = model.DateOut.ToMinimumHourValue.Add(Calendar.ToTimespan(model.TimeOut).Value)
                End If

                timeLog.BranchID = model.BranchID

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

        If Not addedTimeLogs.Any() AndAlso
            Not updatedTimeLogs.Any() AndAlso
            Not deletedTimeLogs.Any() Then

            MessageBoxHelper.Warning("No valid time logs to save.")
            Return

        End If

        Dim allowedActions As New List(Of String)
        If _currentRolePermission.Create Then allowedActions.Add("CREATE")
        If _currentRolePermission.Update Then allowedActions.Add("UPDATE")
        If _currentRolePermission.Delete Then allowedActions.Add("DELETE")

        Dim allowedActionsMessage = $"You are only allowed to perform ({ _
            String.Join(", ", allowedActions.ToArray())}) actions."

        Const UnathorizedActionTitle As String = "Unauthorized Action"

        If addedTimeLogs.Any() AndAlso Not _currentRolePermission.Create Then

            MessageBoxHelper.Warning("You are prohibited to create new time logs. " & allowedActionsMessage, UnathorizedActionTitle)
            Return
        End If

        If updatedTimeLogs.Any() AndAlso Not _currentRolePermission.Update Then

            MessageBoxHelper.Warning("You are prohibited to update existing time logs. " & allowedActionsMessage, UnathorizedActionTitle)
            Return
        End If

        If deletedTimeLogs.Any() AndAlso Not _currentRolePermission.Delete Then

            MessageBoxHelper.Warning("You are prohibited to delete existing time logs. " & allowedActionsMessage, UnathorizedActionTitle)
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save Time Logs",
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of ITimeLogDataService)
                Await dataService.SaveManyAsync(
                    currentlyLoggedInUserId:=z_User,
                    added:=addedTimeLogs,
                    updated:=updatedTimeLogs,
                    deleted:=deletedTimeLogs)

                If addedTimeLogs.Any() Then
                    Dim addedTimeLogEmployeeIDs = addedTimeLogs.Select(Function(tl) tl.EmployeeID.Value).ToArray()
                    Dim addedTimeLogMinDate = addedTimeLogs.Min(Function(tl) tl.LogDate)
                    Dim addedTimeLogMaxDate = addedTimeLogs.Max(Function(tl) tl.LogDate)

                    Dim addedTimeLogsDatePeriod = New TimePeriod(addedTimeLogMinDate, addedTimeLogMaxDate)

                    Dim newlyAdded = Await timeLogRepositoryQuery.
                        GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(
                            addedTimeLogEmployeeIDs,
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

            End Function)

    End Sub

    Private Sub addDefaultBranchIDs(records As IEnumerable(Of TimeLog))
        For Each record In records

            If record.BranchID Is Nothing Then
                record.BranchID = record.Employee?.BranchID
            End If

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

    Private Async Sub FilterButton_Click(sender As Object, e As EventArgs) Handles FilterButton.Click

        If EmployeeTreeView1.GetTickedEmployees().Any() = False Then

            MessageBoxHelper.Warning("No selected employees.")
            Return
        End If

        If _originalDates Is Nothing OrElse _originalDates.Equals(New TimePeriod(dtpDateFrom.Value, dtpDateTo.Value)) Then
            Return
        End If

        Dim start As Date = dtpDateFrom.Value.Date
        Dim finish As Date = dtpDateTo.Value.Date

        If start > finish Then
            dtpDateTo.Value = start

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

    Private Async Sub EmployeeTreeView1_TickedEmployee(s As Object, e As EventArgs) Handles EmployeeTreeView1.EmployeeTicked

        Await ReloadAsync()

    End Sub

    Private Sub grid_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellEndEdit
        Dim parseableIndexes = {colTimeIn.Index, colTimeOut.Index, colBranchID.Index}

        If parseableIndexes.Any(Function(i) i = e.ColumnIndex) Then
            Dim currRow = grid.Rows(e.RowIndex)
            Dim model = GridRowToTimeLogModel(currRow)

            If e.ColumnIndex = colTimeIn.Index Then
                model.TimeIn = TimeLogModel.TimeSpanToString(Calendar.ToTimespan(model.TimeIn), model.DateIn)

            ElseIf e.ColumnIndex = colTimeOut.Index Then
                model.TimeOut = TimeLogModel.TimeSpanToString(Calendar.ToTimespan(model.TimeOut), model.DateOut)
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

    Private Async Sub btnImport_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click

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
                    Await NewTimeEntryAlternateLineImport()
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

        Dim timeLogService = MainServiceProvider.GetRequiredService(Of ITimeLogDataService)
        Await timeLogService.SaveImportAsync(timeLogs, z_User)

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

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles ExportToolStripButton.Click

        ExportToolStripButton.Enabled = False

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
                        If row.Cells(colTimeIn.Index).Value IsNot Nothing Or row.Cells(colTimeOut.Index).Value IsNot Nothing Then
                            worksheet.Cells($"A{i}").Value = row.Cells(colEmployeeNo.Index).Value
                            worksheet.Cells($"B{i}").Value = row.Cells(colFullName.Index).Value
                            worksheet.Cells($"C{i}").Value = row.Cells(colShift.Index).Value
                            worksheet.Cells($"D{i}").Value = row.Cells(colTimeIn.Index).Value
                            worksheet.Cells($"E{i}").Value = row.Cells(colDateIn.Index).Value
                            worksheet.Cells($"F{i}").Value = row.Cells(colTimeOut.Index).Value
                            worksheet.Cells($"G{i}").Value = row.Cells(colDateOutDisplay.Index).Value
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
        Finally
            ExportToolStripButton.Enabled = True
        End Try

    End Sub

    Private Sub btnUserActivity_Click(sender As Object, e As EventArgs) Handles UserActivityToolStripButton.Click

        'TODO: create a string constant class for this
        Dim FormEntityName As String = "Time Log"

        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

#End Region

End Class
