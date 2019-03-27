Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Helper.TimeLogsReader
Imports AccuPay.Tools
Imports AccuPay.Utils
Imports log4net
Imports Microsoft.EntityFrameworkCore

Public Class TimeLogsForm2

#Region "VariableDeclarations"
    Private Shared logger As ILog = LogManager.GetLogger("TimeLogsFormAppender")

    Private Const SUNDAY_SHORT_NAME As String = "Sun"

    Private currRowIndex As Integer = -1
    Private currColIndex As Integer = -1

    Private _balloonToolTips As IList(Of ToolTip)
    Private thefilepath As String

    Private _useShiftSchedulePolicy As Boolean

#End Region

#Region "Methods"

    Private Sub ResetCurrentCell()
        currRowIndex = -1
        currColIndex = -1
    End Sub

    Private Async Sub ReloadAsync(startDate As Date, endDate As Date)
        Dim hasSelectedEmployees = EmployeeTreeView1.GetTickedEmployees().Any()

        If Not hasSelectedEmployees Then
            grid.DataSource = Nothing

            ResetCurrentCell()
            Return
        End If

        Dim employeeIDs = EmployeeTreeView1.GetTickedEmployees.Select(Function(emp) emp.RowID.Value).ToList()

        Using context = New PayrollContext
            Dim employees = Await context.Employees.
                Where(Function(e) employeeIDs.Contains(e.RowID.Value)).
                ToListAsync() 'employeeIDs.Any(Function(eID) e.RowID.Value = eID)

            Dim shiftSchedules = Await context.EmployeeDutySchedules.
                Where(Function(ss) ss.OrganizationID.Value = z_OrganizationID).
                Where(Function(ss) ss.DateSched >= startDate AndAlso ss.DateSched <= endDate).
                Where(Function(ss) employeeIDs.Contains(ss.EmployeeID.Value)).
                Where(Function(ss) ss.StartTime.HasValue AndAlso ss.EndTime.HasValue).
                ToListAsync()

            Dim dataSource As New List(Of TimeLogModel)

            Dim models = CreatedResults(employees, startDate, endDate)

            Dim timeLogs = Await context.TimeLogs.
                Include(Function(etd) etd.Employee).
                Where(Function(etd) employeeIDs.Contains(etd.EmployeeID.Value)).
                Where(Function(etd) etd.LogDate >= startDate AndAlso etd.LogDate <= endDate).
                ToListAsync()

            For Each model In models
                Dim seek = timeLogs.
                    Where(Function(etd) etd.EmployeeID.Value = model.EmployeeID).
                    Where(Function(etd) etd.LogDate = model.DateIn)

                Dim seekShiftSched = shiftSchedules.
                    Where(Function(ss) ss.EmployeeID.Value = model.EmployeeID).
                    Where(Function(ss) ss.DateSched = model.DateIn)

                Dim hasShiftSched = seekShiftSched.Any()

                If seek.Any Then
                    Dim timeLog = seek.FirstOrDefault
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
        End Using
    End Sub

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
        Dim infohint As New ToolTip
        infohint.IsBalloon = True
        infohint.ToolTipTitle = "Save successfully"
        infohint.ToolTipIcon = ToolTipIcon.Info

        infohint.Show(String.Empty, btnSave)
        infohint.Show("Done.", btnSave, 3475)
        'infohint.Show("Done.", btnSave, New Point(btnSave.Location.X, btnSave.Location.Y - 76), 3475)

        _balloonToolTips.Add(infohint)
    End Sub

    Private Sub ShowSuccessImportBalloon()
        Dim infohint As New ToolTip
        infohint.IsBalloon = True
        infohint.ToolTipTitle = "Imported successfully"
        infohint.ToolTipIcon = ToolTipIcon.Info

        infohint.Show(String.Empty, btnImport)
        infohint.Show("Done.", btnImport, 3475)

        _balloonToolTips.Add(infohint)
    End Sub

    Private Function GetShiftSchedulePolicy() As Boolean
        Using context = New PayrollContext()

            Dim settings = New ListOfValueCollection(context.ListOfValues.ToList())

            Dim policy = New TimeEntryPolicy(settings)

            Return policy.UseShiftSchedule
        End Using
    End Function

    Private Async Sub NewTimeEntryAlternateLineImport()
        Dim importer = New TimeLogsReader()
        Dim importOutput = importer.Import(thefilepath)

        If importOutput.IsImportSuccess = False Then
            MessageBox.Show(importOutput.ErrorMessage)
            Return
        End If

        Dim logs = importOutput.Logs.ToList()

        If logs.Count = 0 Then
            MessageBox.Show("No logs were parsed. Please make sure the log files follows the right format.")
            Return
        End If

        Dim timeAttendanceHelper As ITimeAttendanceHelper = Await GetTimeAttendanceHelper(logs)

        'determines the IstimeIn, LogDate, and Employee values
        logs = timeAttendanceHelper.Analyze()
        Dim validLogs = logs.Where(Function(l) l.HasError = False).ToList()
        Dim invalidLogs = logs.Where(Function(l) l.HasError = True).ToList()

        invalidLogs.AddRange(importOutput.Errors)


        'preview the logs here
        Dim previewDialog As New _
            TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog(validLogs, invalidLogs)

        With previewDialog
            .ShowDialog()
            .BringToFront()
        End With

        If previewDialog.Cancelled Then
            Return
        End If

        NewTimeEntryAlternateLineImportSave(timeAttendanceHelper)
    End Sub

    Private Async Sub NewTimeEntryAlternateLineImportSave(timeAttendanceHelper As ITimeAttendanceHelper)
        Dim succeed As Boolean = False
        Try
            Dim timeLogs = timeAttendanceHelper.GenerateTimeLogs()
            Dim timeAttendanceLogs = timeAttendanceHelper.GenerateTimeAttendanceLogs()

            Using context = New PayrollContext()

                Dim importId = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
                Dim originalImportId = importId

                Dim counter As Integer = 0

                While context.TimeLogs.FirstOrDefault(Function(t) t.TimeentrylogsImportID = importId) IsNot Nothing OrElse
                        context.TimeAttendanceLogs.FirstOrDefault(Function(t) t.ImportNumber = importId) IsNot Nothing
                    counter += 1

                    importId = originalImportId & "_" & counter

                End While

                For Each timeLog In timeLogs

                    timeLog.TimeentrylogsImportID = importId

                    context.TimeLogs.Add(timeLog)
                Next

                For Each timeAttendanceLog In timeAttendanceLogs

                    timeAttendanceLog.ImportNumber = importId

                    context.TimeAttendanceLogs.Add(timeAttendanceLog)
                Next

                Await context.SaveChangesAsync()

                succeed = True
            End Using

        Catch ex As Exception

            logger.Error("NewTimeEntryAlternateLineImport", ex)

            MessageBoxHelper.DefaultErrorMessage("Import Logs")

            Throw ex

        Finally
            If succeed Then
                ShowSuccessImportBalloon()

                DateFilter_ValueChanged(dtpDateFrom, New EventArgs)
            End If
        End Try

    End Sub

    Private Sub ResetGridRowsDefaultCellStyle()
        For Each row As DataGridViewRow In grid.Rows
            row.DefaultCellStyle = Nothing
        Next
    End Sub

#End Region

#Region "Functions"

    Private Function CreatedResults(employees As List(Of Employee), startDate As Date, endDate As Date) As List(Of TimeLogModel)
        Dim returnList As New List(Of TimeLogModel)

        Dim dateRanges = Calendar.EachDay(startDate, endDate)

        Dim sortedEmployees = employees.OrderBy(Function(e) e.Fullname)

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

    Private Function TimeLogsImportOption() As TimeLogsForm.TimeLogsFormat?

        Dim time_logformat As TimeLogsForm.TimeLogsFormat?

        MessageBoxManager.Yes = "Alternating line"
        MessageBoxManager.No = "Same line"

        MessageBoxManager.Register()

        Dim custom_prompt =
            MessageBox.Show("Which format are you going to import ?",
                            "", MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1)

        If custom_prompt = Windows.Forms.DialogResult.Yes Then
            time_logformat = TimeLogsForm.TimeLogsFormat.Conventional
        ElseIf custom_prompt = Windows.Forms.DialogResult.No Then
            time_logformat = TimeLogsForm.TimeLogsFormat.Optimized
        ElseIf custom_prompt = Windows.Forms.DialogResult.Cancel Then
            time_logformat = Nothing
        End If

        MessageBoxManager.Unregister()

        Return time_logformat

    End Function

    Private Async Function GetTimeAttendanceHelper(logs As List(Of ImportTimeAttendanceLog)) _
                            As Threading.Tasks.Task(Of ITimeAttendanceHelper)

        Dim logsGroupedByEmployee = ImportTimeAttendanceLog.GroupByEmployee(logs)
        Dim employees As List(Of Employee) = Await GetEmployeesFromLogGroup(logsGroupedByEmployee)

        Dim firstDate = logs.FirstOrDefault.DateTime.ToMinimumHourValue
        Dim lastDate = logs.LastOrDefault.DateTime.ToMaximumHourValue

        Dim timeAttendanceHelper As ITimeAttendanceHelper

        If _useShiftSchedulePolicy Then

            Dim employeeShifts As List(Of EmployeeDutySchedule) =
                    Await GetEmployeeDutyShifts(firstDate, lastDate)

            timeAttendanceHelper = New TimeAttendanceHelperNew(logs, employees, employeeShifts)

        Else

            Dim employeeShifts As List(Of ShiftSchedule) =
                    Await GetEmployeeShifts(firstDate, lastDate)

            timeAttendanceHelper = New TimeAttendanceHelper(logs, employees, employeeShifts)

        End If

        Return timeAttendanceHelper
    End Function

    Private Async Function GetEmployeeShifts(firstDate As Date, lastDate As Date) As Threading.Tasks.Task(Of List(Of ShiftSchedule))

        Using context = New PayrollContext()
            Return Await context.ShiftSchedules.
                           Include(Function(s) s.Shift).
                           Where(Function(s) s.OrganizationID = z_OrganizationID).
                           Where(Function(s) s.EffectiveFrom >= firstDate).
                           Where(Function(s) s.EffectiveTo <= lastDate).
                           ToListAsync()
        End Using

    End Function

    'new shift table
    Private Async Function GetEmployeeDutyShifts(firstDate As Date, lastDate As Date) As Threading.Tasks.Task(Of List(Of EmployeeDutySchedule))

        Using context = New PayrollContext()
            Return Await context.EmployeeDutySchedules.
                           Where(Function(s) s.OrganizationID.Value = z_OrganizationID).
                           Where(Function(s) s.DateSched >= firstDate).
                           Where(Function(s) s.DateSched <= lastDate).
                           ToListAsync()
        End Using

    End Function

    Private Async Function GetEmployeesFromLogGroup(logsGroupedByEmployee As List(Of IGrouping(Of String, ImportTimeAttendanceLog))) As Threading.Tasks.Task(Of List(Of Employee))

        Using context As New PayrollContext
            If logsGroupedByEmployee.Count < 1 Then
                Return New List(Of Employee)
            End If

            Dim employeeNumbersArray(logsGroupedByEmployee.Count - 1) As String

            For index = 0 To logsGroupedByEmployee.Count - 1
                employeeNumbersArray(index) = logsGroupedByEmployee(index).Key
            Next

            Return Await context.Employees.
                            Where(Function(e) employeeNumbersArray.Contains(e.EmployeeNo)).
                            Where(Function(e) Nullable.Equals(e.OrganizationID, z_OrganizationID)).
                            ToListAsync
        End Using

    End Function

    Private Shared Function SeekBetweenDates(tl As TimeLog, addedTimeLogMinDate As Date, addedTimeLogMaxDate As Date) As Boolean
        Return tl.LogDate >= addedTimeLogMinDate AndAlso tl.LogDate <= addedTimeLogMaxDate
    End Function

    Private Shared Function SeekEmployeeID(tl As TimeLog, addedTimeLogEmployeeIDs As IEnumerable(Of Integer)) As Boolean
        Return addedTimeLogEmployeeIDs.Contains(tl.EmployeeID.Value)
    End Function

#End Region

#Region "PrivateClass"
    Private Class TimeLogModel
        Private Const CUSTOM_SHORT_TIME_FORMAT As String = "HH:mm"

        Private _timeLog As TimeLog
        Private _employee As Employee
        Private _dateOut, origDateIn, origDateOut As Date
        Private origTimeIn, origTimeOut As String
        Private _dateOutDisplay As Date?
        Private _timeIn, _timeOut As String

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
                        _dateOut = logOutDate
                        origDateOut = logOutDate
                    End If
                End If

                origTimeIn = TimeSpanToString(.TimeIn)
                origTimeOut = TimeSpanToString(.TimeOut)

                _timeIn = origTimeIn
                _timeOut = origTimeOut
            End With

        End Sub

        Sub New(employee As Employee, d As Date)
            _employee = employee

            origDateIn = d
            DateIn = d

            origDateOut = d
            DateOut = d

        End Sub

        Public Property RowID As Integer

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
                Return If(_timeLog?.Employee?.Fullname, _employee.Fullname)
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

        Public Property ShiftSchedule As EmployeeDutySchedule

        Public Property DateIn As Date

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

        Public ReadOnly Property IsExisting As Boolean
            Get
                Return RowID > 0
            End Get
        End Property

        Public ReadOnly Property HasChanged As Boolean
            Get
                Dim differs = Not Equals(origDateIn, DateIn) _
                    Or Not Equals(origTimeIn, _timeIn) _
                    Or Not Equals(origDateOut, DateOut) _
                    Or Not Equals(origTimeOut, _timeOut)

                Return differs
            End Get
        End Property

        Public ReadOnly Property IsValidToSave As Boolean
            Get
                Dim hasLog = Not String.IsNullOrWhiteSpace(_timeIn) _
                    Or Not String.IsNullOrWhiteSpace(_timeOut)

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
                        .EmployeeID = EmployeeID,
                        .OrganizationID = z_OrganizationID,
                        .LogDate = DateIn,
                        .CreatedBy = z_User,
                        .Created = Now}
                End If

                With _timeLog
                    .LastUpd = Now
                    .LastUpdBy = z_User

                    .TimeIn = Calendar.ToTimespan(TimeIn)
                    .TimeOut = Calendar.ToTimespan(TimeOut)
                End With

                Return _timeLog
            End Get
        End Property

        Public Sub Restore()
            DateIn = origDateIn
            _timeIn = origTimeIn

            DateOut = origDateOut
            _timeOut = origTimeOut
        End Sub

        Public Sub Commit()
            origDateIn = DateIn
            origTimeIn = _timeIn

            origDateOut = DateOut
            origTimeOut = _timeOut

            Remove()
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

    Private Sub TimeLogsForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        grid.AutoGenerateColumns = False

        EmployeeTreeView1.SwitchOrganization(z_OrganizationID)

        BindGridCurrentCellChanged()

        _balloonToolTips = New List(Of ToolTip)

        _useShiftSchedulePolicy = GetShiftSchedulePolicy()

    End Sub

    Private Sub TimeLogsForm2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)

    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Close()

    End Sub

    Private Async Sub btnSave_ClickAsync(sender As Object, e As EventArgs) Handles btnSave.Click
        Using context = New PayrollContext
            Dim models = GridRowToTimeLogModels(grid)

            Dim toSaveList = models.Where(Function(tlm) tlm.HasChanged).ToList()
            Dim toSaveListEmployeeIDs = toSaveList.Select(Function(tlm) tlm.EmployeeID)

            Dim existingRecords = Await context.TimeLogs.
                Where(Function(tl) tl.OrganizationID.Value = z_OrganizationID).
                Where(Function(tl) SeekEmployeeID(tl, toSaveListEmployeeIDs)).
                Where(Function(tl) SeekBetweenDates(tl, dtpDateFrom.Value.Date, dtpDateTo.Value.Date)).
                ToListAsync()

            Dim addedTimeLogs As New List(Of TimeLog)

            For Each model In toSaveList
                Dim seek = existingRecords.
                    Where(Function(tl) tl.EmployeeID.Value = model.EmployeeID).
                    Where(Function(tl) tl.LogDate = model.DateIn)

                Dim exists = seek.Any
                Dim timeLog = seek.FirstOrDefault

                If model.ConsideredDelete Then
                    context.TimeLogs.Remove(timeLog)
                    'model.Remove()
                ElseIf model.IsExisting Then
                    timeLog.TimeIn = Calendar.ToTimespan(model.TimeIn)
                    timeLog.TimeOut = Calendar.ToTimespan(model.TimeOut)
                    timeLog.LastUpdBy = z_User
                    timeLog.LastUpd = Now

                    'context.Entry(model.ToTimeLog).State = EntityState.Modified
                    context.Entry(timeLog).State = EntityState.Modified
                ElseIf Not exists And model.IsValidToSave Then
                    model.Remove()
                    Dim addedTimeLog = model.ToTimeLog
                    'addedTimeLog.RowID = Nothing
                    context.TimeLogs.Add(addedTimeLog)
                    addedTimeLogs.Add(addedTimeLog)
                End If

            Next

            Try
                Await context.SaveChangesAsync()

                If addedTimeLogs.Any() Then
                    Dim addedTimeLogEmployeeIDs = addedTimeLogs.Select(Function(tl) tl.EmployeeID.Value).ToList()
                    Dim addedTimeLogMinDate = addedTimeLogs.Min(Function(tl) tl.LogDate)
                    Dim addedTimeLogMaxDate = addedTimeLogs.Max(Function(tl) tl.LogDate)

                    Dim newlyAdded = Await context.TimeLogs.
                    Where(Function(tl) SeekEmployeeID(tl, addedTimeLogEmployeeIDs)).
                    Where(Function(tl) SeekBetweenDates(tl, addedTimeLogMinDate, addedTimeLogMaxDate)).
                    ToListAsync()

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

        End Using
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

    End Sub

    Private Sub DateFilter_ValueChanged(sender As Object, e As EventArgs) _
        Handles dtpDateFrom.ValueChanged, dtpDateTo.ValueChanged

        Dim dtp = DirectCast(sender, DateTimePicker)

        Dim start As Date = dtpDateFrom.Value.Date
        Dim finish As Date = dtpDateTo.Value.Date

        If start > finish Then
            If dtp.Name = dtpDateFrom.Name Then dtpDateTo.Value = start : finish = start
            If dtp.Name = dtpDateTo.Name Then dtpDateFrom.Value = finish : start = finish

        End If

        ReloadAsync(start, finish)
    End Sub

    Private Sub dtpDateTo_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateTo.ValueChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles labelChangedCount.Click

    End Sub

    Private Sub Label1_TextChanged(sender As Object, e As EventArgs) Handles labelChangedCount.TextChanged
        Dim isZero = labelChangedCount.Text = "0"
        Dim hideBool = Not isZero

        Panel3.Visible = hideBool

    End Sub

    Private Sub grid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellContentClick
        Dim buttonIndexes = {colDecrement.Index, colIncrement.Index}

        If buttonIndexes.Any(Function(i) i = e.ColumnIndex) Then
            grid.EndEdit()

            Dim arithmeticOperand = If(e.ColumnIndex = colDecrement.Index, -1, 1)

            Dim currRow = grid.Rows(e.RowIndex)
            Dim model = GridRowToTimeLogModel(currRow)

            model.DateOut = model.DateOut.AddDays(arithmeticOperand)

            If model.HasChanged Then
                HasChangedRow(currRow)
            Else
                HasNoChangedRow(currRow)
            End If

            ToSaveCountChanged()

            grid.Refresh()
        End If

        Console.WriteLine("{0}", {grid.Columns(e.ColumnIndex).Name})
    End Sub

    Private Sub EmployeeTreeView1_Load(sender As Object, e As EventArgs) Handles EmployeeTreeView1.Load

    End Sub

    Private Sub EmployeeTreeView1_TickedEmployee(s As Object, e As EventArgs) Handles EmployeeTreeView1.TickedEmployee
        DateFilter_ValueChanged(dtpDateFrom, e)
    End Sub

    Private Sub grid_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellEndEdit
        Dim parseableIndexes = {colTimeIn.Index, colTimeOut.Index}

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

        If currRowIndex < grid.Rows.Count _
            And currColIndex > 0 Then
            grid.CurrentCell = grid.Item(currColIndex, currRowIndex)
        End If

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

        Dim timeLogsFormat_ As TimeLogsForm.TimeLogsFormat? = TimeLogsImportOption()

        'They chose Cancel or used the close button
        If timeLogsFormat_ Is Nothing Then Return

        Try
            Dim browsefile As OpenFileDialog = New OpenFileDialog()
            browsefile.Filter = "Text Documents (*.txt)|*.txt" &
                                "|All files (*.*)|*.*"

            If browsefile.ShowDialog = Windows.Forms.DialogResult.OK Then

                thefilepath = browsefile.FileName

                If timeLogsFormat_ = TimeLogsForm.TimeLogsFormat.Conventional Then
                    NewTimeEntryAlternateLineImport()

                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message & " Error on file initialization")
        Finally

        End Try

    End Sub

    Private Sub TimeLogsForm2_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        For Each tTip In _balloonToolTips
            tTip.Dispose()
        Next
    End Sub

    Private Sub grid_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellMouseEnter
        If e.ColumnIndex = colDelete.Index Then
            grid.Cursor = Cursors.Hand
        Else
            grid.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub grid_CellMouseLeave(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellMouseLeave
        If e.ColumnIndex = colDelete.Index Then grid.Cursor = Cursors.Default
    End Sub

#End Region

End Class
