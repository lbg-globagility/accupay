Option Strict On

Imports System.Threading.Tasks
Imports AccuPay
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.Tools
Imports log4net
Imports Microsoft.EntityFrameworkCore

Public Class ShiftScheduleForm

#Region "VariableDeclarations"
    Private Const ENABLED_TEXT As String = "Enable"
    Private Const DISABLED_TEXT As String = "Disable"

    Private Shared logger As ILog = LogManager.GetLogger("ShiftScheduleAppender")

    Const defaultWorkHours As Integer = 8

    Private organizationId As Integer

    Private sunDayName As String = DayNames.Sun.ToString

    Private _dutyShiftPolicy As DutyShiftPolicy

    Private _originDataSource As List(Of ShiftScheduleModel)

#End Region

#Region "Methods"

    Private Async Sub LoadShiftScheduleConfigurablePolicy()
        _dutyShiftPolicy = Await DutyShiftPolicy.Load1

        txtBreakLength.Value = _dutyShiftPolicy.BreakHour
    End Sub

    Private Sub LoadWeek()
        Dim sundayDate = GetSundayDate()
        Dim weekBoundaryDate = sundayDate.AddDays(DayOfWeek.Saturday)

        Dim sevenDays = Calendar.EachDay(sundayDate, weekBoundaryDate)

        Dim _models = New List(Of ShiftScheduleModel)
        For Each d In sevenDays
            _models.Add(New ShiftScheduleModel With {.DateValue = d})
        Next

        RefreshDataSource(gridWeek, _models)
    End Sub

    Private Sub CollectShiftSchedModel(ee As ShiftScheduleModel, dateVal As Date, modelList As ICollection(Of ShiftScheduleModel), isRaw As Boolean)
        Dim newEe As New ShiftScheduleModel With {
            .EmployeeId = ee.EmployeeId,
            .EmployeeNo = ee.EmployeeNo,
            .FullName = ee.FullName,
            .DateValue = dateVal}

        If Not isRaw Then ApplyChangesToModel(newEe)

        modelList.Add(newEe)
    End Sub

    Private Sub ApplyChangesToModel(ssm As ShiftScheduleModel)
        With ssm
            .TimeFrom = ShortTimeSpan(txtTimeFrom.Value)
            .TimeTo = ShortTimeSpan(txtTimeTo.Value)
            .BreakFrom = ShortTimeSpan(txtBreakFrom.Value)

            If txtBreakLength.Value <= txtBreakLength.Minimum Then
                .BreakLength = Nothing
            Else
                .BreakLength = txtBreakLength.Value
            End If
        End With
    End Sub

    Private Sub ApplyChangesToModel(ssm As ShiftScheduleModel, update As ShiftScheduleModel)
        With ssm
            .TimeFrom = update.TimeFrom
            .TimeTo = update.TimeTo
            .BreakFrom = update.BreakFrom
            .BreakLength = update.BreakLength
            .IsRestDay = update.IsRestDay
        End With
    End Sub

    Private Sub ZebraliseEmployeeRows()
        Dim ebonyStyle = Color.LightGray
        Dim ivoryStyle = Color.White

        Dim groupEmployeeIDs = ConvertGridRowsToShiftScheduleModels(grid).
            GroupBy(Function(ssm) ssm.EmployeeId).
            ToList()

        Dim isEven = False
        Dim i = 1

        For Each eID In groupEmployeeIDs
            isEven = i Mod 2 = 0

            Dim employeePrimKey = eID.FirstOrDefault.EmployeeId.Value

            If isEven Then
                ColorEmployeeRows(employeePrimKey, ivoryStyle)
            Else
                ColorEmployeeRows(employeePrimKey, ebonyStyle)
            End If

            i += 1
        Next

    End Sub

    Private Sub ColorEmployeeRows(employeePrimKey As Integer, backColor As Color)
        Dim employeeGridRows =
                grid.Rows.OfType(Of DataGridViewRow).
                Where(Function(row) _
                          Equals(Convert.ToInt32(row.Cells(colEmployeeId.Name).Value),
                                 employeePrimKey))

        For Each eGridRow In employeeGridRows
            eGridRow.DefaultCellStyle.BackColor = backColor

            ColorizeSundays(eGridRow, colDayName)
        Next
    End Sub

    Private Sub CommitTimeValues()
        CommitTimeValue(GroupBox2)
        CommitTimeValue(GroupBox3)
    End Sub

    Private Sub CommitTimeValue(contrl As Control)
        Dim timeTextbox = contrl.Controls.OfType(Of TimeTextBox)
        For Each timeTBox In timeTextbox
            timeTBox.Commit()
        Next
    End Sub

    Private Sub ColorizeSundays(gridRow As DataGridViewRow, dataGridColumn As DataGridViewColumn)
        Dim shortDayName =
            Convert.ToDateTime(gridRow.Cells(dataGridColumn.Name).Value).ToString("ddd")

        If shortDayName = sunDayName Then
            gridRow.DefaultCellStyle.ForeColor = Color.Red
        Else
            Return
        End If
    End Sub

    Private Sub ShiftProper(dataGrid As DataGridView, e As DataGridViewCellEventArgs)
        Dim _currRow = dataGrid.Rows(e.RowIndex)
        If Not IsStartTimeCellParseable(dataGrid, e.ColumnIndex) Then
            TransformTouchedRow(_currRow)
            Return
        End If

        Dim model = DirectCast(_currRow.DataBoundItem, ShiftScheduleModel)

        AutomaticShiftCompute(model)

        TransformTouchedRow(_currRow)
    End Sub

    Private Sub TransformTouchedRow(dataGridCurrentRow As DataGridViewRow)
        Dim currRow = dataGridCurrentRow
        Dim ssm = DirectCast(currRow.DataBoundItem, ShiftScheduleModel)

        If ssm.HasChanged Then
            currRow.DefaultCellStyle.Font = New Font("Segoe UI", 8.25!, FontStyle.Bold, GraphicsUnit.Point, CType(0, Byte))
        Else
            currRow.DefaultCellStyle.Font = Nothing
        End If
    End Sub

    Private Sub AutomaticShiftCompute(shiftSched As ShiftScheduleModel)
        Dim shiftHours = _dutyShiftPolicy.DefaultWorkHour
        Dim breakHours = shiftSched.BreakLength

        Dim _empty = String.Empty

        Dim ClearBreakTimes = Sub(ss As ShiftScheduleModel)
                                  ss.BreakFrom = String.Empty
                                  ss.BreakLength = Nothing
                              End Sub

        With shiftSched
            Dim startTime As TimeSpan? = Calendar.ToTimespan(.TimeFrom)
            Dim breakStartTime As TimeSpan? = Calendar.ToTimespan(.BreakFrom)

            If startTime.HasValue Then
                .TimeTo = MilitarDateTime(ToDateTime(startTime.Value).Value.AddHours(shiftHours))
                'If breakHours = 0 Then breakHours = _dutyShiftPolicy.BreakHour : .BreakLength = breakHours

                If String.IsNullOrWhiteSpace(.BreakFrom) And .BreakLength > 0 Then
                    Dim firstHalf = Math.Floor(shiftHours / 2)
                    Dim breakStart = ToDateTime(startTime.Value).Value.AddHours(firstHalf)

                    breakHours = _dutyShiftPolicy.BreakHour
                    .BreakLength = breakHours

                    breakStartTime = breakStart.TimeOfDay

                    If breakStartTime.HasValue Then
                        .BreakFrom = MilitarDateTime(breakStartTime.Value)
                    Else
                        ClearBreakTimes(shiftSched)
                    End If

                ElseIf Not String.IsNullOrWhiteSpace(.BreakFrom) Then
                    If Nullable.Equals(.BreakLength, Nothing) Then
                        breakHours = _dutyShiftPolicy.BreakHour
                        .BreakLength = breakHours
                    End If
                Else
                    ClearBreakTimes(shiftSched)
                End If
            Else
                ClearBreakTimes(shiftSched)

                .TimeTo = Nothing
            End If

            .IsRestDay = .IsRestDay
        End With

    End Sub

    Private Sub ResetInputFields(control As Control)
        For Each ctl In control.Controls.OfType(Of TimeTextBox)
            ctl.Clear()
            ctl.Commit()
        Next

        For Each ctl In control.Controls.OfType(Of NumericUpDown)
            ctl.Value = ctl.Minimum
        Next
    End Sub

    Private Sub RefreshDataSource(dataGrid As DataGridView, source As IList(Of ShiftScheduleModel))
        dataGrid.DataSource = source
        dataGrid.Refresh()
    End Sub

    Private Sub EnterKeySameAsTabKey(e As KeyPressEventArgs)
        Dim eAsc = Asc(e.KeyChar)
        If Not eAsc = 13 Then
            Return
        End If

        SendKeys.Send("{TAB}")
    End Sub

    Private Sub AffectedRows(_newSource As List(Of ShiftScheduleModel))
        Dim changesCount = _newSource.Where(Function(data) data.HasChanged).Count
        labelAffectedRows.Text = $"Affected rows : {changesCount}"
    End Sub

    Private Sub NoAffectedRows()
        labelAffectedRows.Text = labelAffectedRows.AccessibleDescription
    End Sub

#End Region

#Region "Functions"

    Private Function MilitarDateTime(dateTimeValue As DateTime) As String
        Return dateTimeValue.ToString("HH\:mm")
    End Function

    Public Function MilitarDateTime(dateTimeValue As TimeSpan) As String
        Return dateTimeValue.ToString("hh\:mm")
    End Function

    Private Function GetSundayDate() As Date
        Dim sundayDate As DateTime = Today.AddDays((Today.DayOfWeek - DayOfWeek.Sunday) * -1)
        Return sundayDate
    End Function

    Private Function IsCellTimeParseable(dataGrid As DataGridView, columnIndexNumber As Integer) As Boolean
        Dim timeColumnIndexes() = New Integer() {}
        If dataGrid.Name = grid.Name Then
            timeColumnIndexes = New Integer() {colTimeFrom.Index, colTimeTo.Index, colBreakTimeFrom.Index}
        ElseIf dataGrid.Name = gridWeek.Name Then
            timeColumnIndexes = New Integer() {Column3.Index, Column4.Index, Column5.Index}
        End If

        Return timeColumnIndexes.Any(Function(i) Equals(i, columnIndexNumber))
    End Function

    Private Function IsStartTimeCellParseable(dataGrid As DataGridView, columnIndexNumber As Integer) As Boolean
        Dim timeColumnIndexes() = New Integer() {}
        If dataGrid.Name = grid.Name Then
            timeColumnIndexes = New Integer() {colTimeFrom.Index, colBreakTimeFrom.Index, colBreakLength.Index, colIsRestDay.Index}
        ElseIf dataGrid.Name = gridWeek.Name Then
            timeColumnIndexes = New Integer() {Column3.Index, Column5.Index, Column8.Index, Column7.Index}
        End If

        Return timeColumnIndexes.Any(Function(i) Equals(i, columnIndexNumber))
    End Function

    Private Function ShortTimeSpan(ts As TimeSpan?) As String
        If ts.HasValue Then
            Return ts.Value.ToString("hh\:mm")
        Else
            Return Nothing
        End If
    End Function

    Private Function TimeSpanAddMinute(text As String, addendMinute As Integer) As TimeSpan?
        Return TimeSpan.Parse(text).Add(New TimeSpan(0, CInt(60 * addendMinute), 0))
    End Function

    Public Function TimeSpanAddMinute(text As String, addendMinute As Decimal) As TimeSpan?
        If String.IsNullOrWhiteSpace(text) Then
            Return Nothing
        End If

        Return TimeSpan.Parse(text).Add(New TimeSpan(0, CInt(60 * addendMinute), 0))
    End Function

    Private Function ToDateTime(timeSpanValue As TimeSpan) As Date?
        Return TimeUtility.ToDateTime(timeSpanValue)
    End Function

    Private Function CreatedResult(isRawData As Boolean) As List(Of ShiftScheduleModel)
        CommitTimeValues()

        Dim beginDate = dtpDateFrom.Value.Date
        Dim endDate = dtpDateTo.Value.Date

        Dim dates =
            Calendar.EachDay(beginDate,
                             endDate)

        Dim employees = EmployeeTreeView1.GetTickedEmployees
        Dim employeeList = From emp In employees
                           Order By String.Concat(emp.LastName, emp.FirstName)
                           Let ee = emp
                           Select New ShiftScheduleModel(ee)

        Dim _models = New List(Of ShiftScheduleModel)

        For Each ee In employeeList.ToList
            For Each dateVal In dates
                CollectShiftSchedModel(ee,
                                       dateVal,
                                       _models,
                                       isRawData)
            Next
        Next

        Return _models
    End Function

    Private Async Function RangeApply(beginDate As Date, endDate As Date, _models As List(Of ShiftScheduleModel)) As Task(Of List(Of ShiftScheduleModel))
        Dim employees = EmployeeTreeView1.GetTickedEmployees
        Dim eIDs = employees.Select(Function(e) e.RowID.Value).ToList
        Using context = New PayrollContext
            Dim _empShiftScheds = Await context.EmployeeDutySchedules.
                Include(Function(e) e.Employee).
                Where(Function(e) eIDs.Any(Function(eID) Equals(e.EmployeeID, eID))).
                Where(Function(e) e.DateSched >= beginDate And e.DateSched <= endDate).
                ToListAsync

            If _empShiftScheds.Any Then
                Dim notExists = Function(shSched As ShiftScheduleModel)
                                    Dim seekResult = _empShiftScheds.
                                    Where(Function(ess) Nullable.Equals(ess.EmployeeID, shSched.EmployeeId)).
                                    Where(Function(ess) Nullable.Equals(ess.DateSched.Date, shSched.DateValue.Date))

                                    Return Not seekResult.Any
                                End Function
                Dim notInDb = _models.Where(Function(ssm) notExists(ssm)).ToList

                _models.Clear()
                For Each ess In _empShiftScheds
                    _models.Add(New ShiftScheduleModel(ess))
                Next

                For Each ssm In notInDb
                    _models.Add(ssm)
                Next

                Dim _dataSource = _models.
                    OrderBy(Function(ssm) ssm.FullName.ToLower).
                    ThenBy(Function(ssm) ssm.DateValue.Date).
                    ToList
                _models = _dataSource
            End If

        End Using

        Return _models
    End Function

    Private Function ConvertGridRowsToShiftScheduleModels(dataGrid As DataGridView) As List(Of ShiftScheduleModel)
        Dim _weekCycleRows = dataGrid.Rows.OfType(Of DataGridViewRow)
        If gridWeek.Name = dataGrid.Name Then
            Dim satisfied = Function(dgvRow As DataGridViewRow) As Boolean
                                Dim _cell = dgvRow.Cells(Column6.Name)
                                Dim isDefaultValue = _cell.Value Is Nothing
                                Dim isApplied = Convert.ToString(_cell.Value) = ENABLED_TEXT
                                Dim _satisfied = isApplied Or isDefaultValue

                                Return _satisfied
                            End Function

            _weekCycleRows = _weekCycleRows.Where(Function(r) satisfied(r))
        End If
        Return _weekCycleRows.Select(Function(r) DirectCast(r.DataBoundItem, ShiftScheduleModel)).ToList
    End Function

    Private Function TimeSpanToString(text As String) As String
        Dim _value = Calendar.ToTimespan(text)
        Dim hasValue = _value.HasValue

        If hasValue Then
            Return _value.Value.ToString("hh\:mm")
        Else
            Return String.Empty
        End If
    End Function

#End Region

#Region "Classes"

    Private Class ShiftScheduleModel

        Private origStartTime, origEndTime, origBreakStart As String
        Private origOffset As Boolean
        Private origBreakLength As Decimal
        Private _timeFrom, _timeTo, _breakFrom As String
        Private _isNew, _madeChanges, _isValid As Boolean
        Private _eds As EmployeeDutySchedule

        Public Sub New()

        End Sub

        Public Sub New(employee As Employee)
            AssignEmployee(employee)

        End Sub

        Public Sub New(ess As EmployeeDutySchedule)
            _eds = ess

            _RowID = ess.RowID
            AssignEmployee(ess.Employee)

            _DateValue = ess.DateSched
            _timeFrom = ShiftScheduleForm.MilitarDateTime(ess.StartTime.GetValueOrDefault)
            _timeTo = ShiftScheduleForm.MilitarDateTime(ess.EndTime.GetValueOrDefault)

            _BreakLength = ess.BreakLength

            Dim _hasBreakStart = ess.BreakStartTime.HasValue
            If _hasBreakStart Then
                _breakFrom = ShiftScheduleForm.MilitarDateTime(ess.BreakStartTime.GetValueOrDefault)

            End If

            _IsRestDay = ess.IsRestDay

            origStartTime = _timeFrom
            origEndTime = _timeTo

            origBreakStart = _breakFrom
            origBreakLength = _BreakLength

            origOffset = _IsRestDay
        End Sub

        Private Sub AssignEmployee(employee As Employee)
            _EmployeeId = employee.RowID
            _EmployeeNo = employee.EmployeeNo
            _FullName = String.Join(", ", employee.LastName, employee.FirstName)

        End Sub

        Public Property RowID As Integer
        Public Property EmployeeId As Integer?
        Public Property EmployeeNo As String
        Public Property FullName As String
        Public Property DateValue As Date

        Public Property TimeFrom As String
            Get
                Return _timeFrom
            End Get
            Set(value As String)
                _timeFrom = value
            End Set
        End Property

        Public Property TimeTo As String
            Get
                Return _timeTo
            End Get
            Set(value As String)
                _timeTo = value
            End Set
        End Property

        Public Property BreakFrom As String
            Get
                Return _breakFrom
            End Get
            Set(value As String)
                _breakFrom = value
            End Set
        End Property

        Public Property BreakLength As Decimal

        Public Property IsRestDay As Boolean

        Public ReadOnly Property DayName As String
            Get
                Return GetDayName(DateValue)
            End Get
        End Property

        Public Shared Function GetDayName(dateValue As Date) As String
            Dim machineCulture As Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture

            Dim dayOfWeek As DayOfWeek = machineCulture.Calendar.GetDayOfWeek(dateValue)
            Return machineCulture.DateTimeFormat.GetDayName(dayOfWeek)
        End Function

        Public ReadOnly Property IsExisting As Boolean
            Get
                Return _RowID > 0
            End Get
        End Property

        Public ReadOnly Property HasChanged As Boolean
            Get
                _madeChanges = Not Equals(origStartTime, _timeFrom) _
                    Or Not Equals(origEndTime, _timeTo) _
                    Or Not Equals(origBreakStart, _breakFrom) _
                    Or Not Equals(origBreakLength, _BreakLength) _
                    Or Not Equals(origOffset, _IsRestDay)

                Return _madeChanges
            End Get
        End Property

        Public ReadOnly Property IsValidToSave As Boolean
            Get
                Dim hasShiftTime = Not String.IsNullOrWhiteSpace(_timeFrom) _
                    And Not String.IsNullOrWhiteSpace(_timeTo)

                _isValid = hasShiftTime Or _IsRestDay

                Return _isValid
            End Get
        End Property

        Public ReadOnly Property IsExist As Boolean
            Get
                Return _eds IsNot Nothing
            End Get
        End Property

        Public ReadOnly Property IsNew As Boolean
            Get
                Return (_eds?.RowID).GetValueOrDefault() = 0 And IsValidToSave
            End Get
        End Property

        Public ReadOnly Property IsUpdate As Boolean
            Get
                Return Not IsNew And _madeChanges And IsValidToSave
            End Get
        End Property

        Public ReadOnly Property ConsideredDelete As Boolean
            Get
                Dim _deleteable = Not IsValidToSave And IsExist

                Return _deleteable
            End Get
        End Property

        Public Sub RemoveShift()
            _eds = Nothing
        End Sub

        Public ReadOnly Property ToEmployeeDutySchedule As EmployeeDutySchedule
            Get
                If _eds Is Nothing Then
                    _eds = New EmployeeDutySchedule With {
                        .EmployeeID = _EmployeeId,
                        .OrganizationID = z_OrganizationID,
                        .DateSched = _DateValue,
                        .CreatedBy = z_User,
                        .Created = Now
                    }
                End If

                With _eds
                    .LastUpdBy = z_User
                    .LastUpd = Now
                    .StartTime = Calendar.ToTimespan(_timeFrom)
                    .EndTime = Calendar.ToTimespan(_timeTo)
                    .BreakStartTime = Calendar.ToTimespan(_breakFrom)
                    .BreakLength = _BreakLength
                    .IsRestDay = _IsRestDay
                End With

                Return _eds
            End Get
        End Property

    End Class

    Private Class DutyShiftPolicy
        Private Const DEFAULT_SHIFT_HOUR As Integer = 9
        Private Const DEFAULT_BREAK_HOUR As Integer = 1

        Private _dutyShiftPolicy As IEnumerable(Of ListOfValue)

        Private _listOfValueRepository As New ListOfValueRepository

        Private settings As ListOfValueCollection = Nothing

        Private _defaultWorkHour, _breakHour As Decimal

        Private Sub New()

        End Sub

        Public Shared Async Function Load1() As Task(Of DutyShiftPolicy)
            Dim policy = New DutyShiftPolicy
            Await policy.Load()
            Return policy
        End Function

        Private Async Function Load() As Task
            _dutyShiftPolicy = Await _listOfValueRepository.GetDutyShiftPolicies()

            settings = New ListOfValueCollection(_dutyShiftPolicy.ToList)

            _defaultWorkHour = settings.GetDecimal("DefaultShiftHour", DEFAULT_SHIFT_HOUR)
            _breakHour = settings.GetDecimal("BreakHour", DEFAULT_BREAK_HOUR)
        End Function

        Public ReadOnly Property DefaultWorkHour() As Decimal
            Get
                Return _defaultWorkHour
            End Get
        End Property

        Public ReadOnly Property BreakHour() As Decimal
            Get
                Return _breakHour
            End Get
        End Property

        Public ReadOnly Property IsUserDefine As Boolean
            Get
                Return DefaultWorkHour = 0
            End Get
        End Property

        Public ReadOnly Property IsBreakUserDefine As Boolean
            Get
                Return BreakHour = 0
            End Get
        End Property

    End Class

#End Region

#Region "Enums"

    Private Enum DayNames
        Sun
        Mon
        Tue
        Wed
        Thur
        Fri
        Sat
    End Enum

#End Region

#Region "EventHandlers"

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click, Button1.Click

        Dim _currCell = grid.CurrentCell

        Dim start As Date = dtpDateFrom.Value.Date
        Dim finish As Date = dtpDateTo.Value.Date

        Dim _newSource = ConvertGridRowsToShiftScheduleModels(grid)

        If tcSchedule.SelectedTab Is tabRange Then
            For Each ssm In _newSource
                ApplyChangesToModel(ssm)
            Next

        ElseIf tcSchedule.SelectedTab Is tabWeekCycle Then

            Dim _weekCycles = ConvertGridRowsToShiftScheduleModels(gridWeek)

            Dim isSatisfy = _weekCycles.Any

            If isSatisfy Then

                For Each w In _weekCycles
                    Dim _update = _newSource.
                        Where(Function(ssm) Equals(w.DayName, ssm.DayName))

                    If Not _update.Any Then Continue For
                    For Each ssm In _update
                        ApplyChangesToModel(ssm, w)
                    Next
                Next

            End If

        End If

        AffectedRows(_newSource)

        RefreshDataSource(grid, _newSource)

        If _currCell IsNot Nothing Then grid.CurrentCell = _currCell
    End Sub

    Private Async Sub btnSave_ClickAsync(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim _saveList = ConvertGridRowsToShiftScheduleModels(grid)

        Using context = New PayrollContext
            Dim _toSaveList = _saveList.Where(Function(ssm) ssm.HasChanged)

            For Each ssm In _toSaveList
                If ssm.IsNew Then
                    context.EmployeeDutySchedules.Add(ssm.ToEmployeeDutySchedule)
                ElseIf ssm.IsUpdate Then
                    context.Entry(ssm.ToEmployeeDutySchedule).State = EntityState.Modified
                ElseIf ssm.ConsideredDelete Then
                    context.EmployeeDutySchedules.Remove(ssm.ToEmployeeDutySchedule)
                    ssm.RemoveShift()
                End If
            Next

            Try
                Dim i = Await context.SaveChangesAsync

                MessageBox.Show("Save successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)

                For Each row As DataGridViewRow In grid.Rows
                    row.DefaultCellStyle = Nothing
                Next

                ZebraliseEmployeeRows()

                NoAffectedRows()
            Catch ex As Exception
                logger.Error("ShiftScheduleSaving", ex)
                Dim errMsg = String.Concat("Oops! something went wrong, please", Environment.NewLine, "contact ", My.Resources.AppCreator, " for assistance.")
                MessageBox.Show(errMsg, "Help", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub EmployeeTreeView1_TickedEmployee(s As Object, e As EventArgs) Handles EmployeeTreeView1.TickedEmployee
        DateFilter_ValueChangedAsync(dtpDateFrom, e)
    End Sub

    Private Sub EmployeeTreeView1_Load(sender As Object, e As EventArgs) Handles EmployeeTreeView1.Load

    End Sub

    Private Sub ShiftScheduleForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
    End Sub

    Private Sub ShiftScheduleForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        LoadShiftScheduleConfigurablePolicy()

        organizationId = z_OrganizationID

        Dim grids = {grid, gridWeek}
        For Each dGrid In grids
            dGrid.AutoGenerateColumns = False
            dGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText

        Next

        EmployeeTreeView1.SwitchOrganization(organizationId)

        LoadWeek()

        Dim ctrls = New Control() {dtpDateFrom, dtpDateTo, txtTimeFrom, txtTimeTo, txtBreakFrom, txtBreakLength}
        For Each c In ctrls
            AddHandler c.KeyPress, AddressOf EnterKeySameAsTabKey_KeyPress
        Next

    End Sub

    Private Sub grid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellContentClick

    End Sub

    Private Sub grid_DataSourceChanged(sender As Object, e As EventArgs) Handles grid.DataSourceChanged
        ZebraliseEmployeeRows()

        For Each row As DataGridViewRow In grid.Rows
            TransformTouchedRow(row)
        Next
    End Sub

    Private Sub dataGrid_TimeCellParsing(sender As Object, e As DataGridViewCellParsingEventArgs) _
        Handles grid.CellParsing, gridWeek.CellParsing

        If IsCellTimeParseable(DirectCast(sender, DataGridView), e.ColumnIndex) Then
            e.Value = TimeSpanToString(DirectCast(e.Value, String))

            Dim hasValue = e.Value IsNot Nothing
            e.ParsingApplied = hasValue
        Else
            e.ParsingApplied = False
        End If

    End Sub

    Private Sub txtTimeFrom_TextChanged(sender As Object, e As EventArgs) Handles txtTimeFrom.TextChanged
        Dim _value = txtTimeFrom.Text.Trim
        If Not _dutyShiftPolicy.IsUserDefine Then
            Dim tsFrom As TimeSpan? = Calendar.ToTimespan(_value)
            Dim shiftHours = _dutyShiftPolicy.DefaultWorkHour

            If tsFrom.HasValue Then
                Dim startDateTime = ToDateTime(tsFrom.Value).Value
                Dim nextTimeTo = startDateTime.AddHours(shiftHours)
                txtTimeTo.Text = MilitarDateTime(nextTimeTo)

                If String.IsNullOrWhiteSpace(txtBreakFrom.Text) Then
                    txtBreakFrom.Text = MilitarDateTime(startDateTime.AddHours(Math.Floor(shiftHours / 2)))
                    txtBreakLength.Value = _dutyShiftPolicy.BreakHour
                    'Else
                    '    txtBreakFrom.Clear()
                    '    txtBreakLength.Value = 0
                End If
            Else
                txtBreakFrom.Clear()
                txtBreakLength.Value = txtBreakLength.Minimum
            End If
        End If
    End Sub

    Private Sub txtTimeTo_TextChanged(sender As Object, e As EventArgs) Handles txtTimeTo.TextChanged

    End Sub

    Private Sub txtBreakFrom_TextChanged(sender As Object, e As EventArgs) Handles txtBreakFrom.TextChanged
        Dim startTime = Calendar.ToTimespan(txtBreakFrom.Text.Trim)
        If startTime.HasValue Then
            Dim breakStart = ToDateTime(startTime.Value).Value
        Else
            txtBreakLength.Value = txtBreakLength.Minimum
        End If
    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Close()
    End Sub

    Private Sub txtBreakLength_ValueChanged(sender As Object, e As EventArgs) Handles txtBreakLength.ValueChanged
        txtBreakFrom_TextChanged(txtBreakFrom, e)
    End Sub

    Private Sub grid_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles grid.ColumnWidthChanged

    End Sub

    Private Sub grid_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles grid.CellPainting
        Dim isSatisfy = e.ColumnIndex = colIsRestDay.Index And e.RowIndex > -1
        If isSatisfy Then
            Dim isFormattedValue = If(CBool(e.FormattedValue), ButtonState.Checked, ButtonState.Normal)

            e.PaintBackground(e.CellBounds, True)
            ControlPaint.DrawCheckBox(e.Graphics, e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 2, e.CellBounds.Height - 2, isFormattedValue)

            e.Handled = True
        End If
    End Sub

    Private Sub gridWeek_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles gridWeek.CellPainting
        Dim isSatisfy = e.ColumnIndex = Column7.Index And e.RowIndex > -1
        If isSatisfy Then
            Dim isFormattedValue = If(CBool(e.FormattedValue), ButtonState.Checked, ButtonState.Normal)

            e.PaintBackground(e.CellBounds, True)
            ControlPaint.DrawCheckBox(e.Graphics, e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 2, e.CellBounds.Height - 2, isFormattedValue)

            e.Handled = True
        End If
    End Sub

    Private Sub gridWeek_CellParsing(sender As Object, e As DataGridViewCellParsingEventArgs) Handles gridWeek.CellParsing

    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        DateFilter_ValueChangedAsync(dtpDateFrom, e)
    End Sub

    Private Sub gridWeek_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles gridWeek.RowsAdded
        Dim row = gridWeek.Rows(e.RowIndex)
        If Equals(row.Cells(Column1.Name).Value, sunDayName) Then
            ColorizeSundays(row, Column2)
        End If

    End Sub

    Private Sub DataGrid_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellEndEdit, gridWeek.CellEndEdit
        Dim _dataGrid = DirectCast(sender, DataGridView)
        ShiftProper(_dataGrid, e)

        _dataGrid.Refresh()
    End Sub

    Private Async Sub DateFilter_ValueChangedAsync(sender As Object, e As EventArgs) _
        Handles dtpDateFrom.ValueChanged, dtpDateTo.ValueChanged

        Dim dtp = DirectCast(sender, DateTimePicker)

        Dim start As Date = dtpDateFrom.Value.Date
        Dim finish As Date = dtpDateTo.Value.Date

        If start > finish Then
            If dtp.Name = dtpDateFrom.Name Then dtpDateTo.Value = start : finish = start
            If dtp.Name = dtpDateTo.Name Then dtpDateFrom.Value = finish : start = finish

        End If

        _originDataSource = Await RangeApply(start, finish, CreatedResult(True))
        RefreshDataSource(grid, _originDataSource)

        NoAffectedRows()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnDiscard1.Click
        ResetInputFields(GroupBox2)
        ResetInputFields(GroupBox3)

    End Sub

    Private Sub btnDiscard2_Click(sender As Object, e As EventArgs) Handles btnDiscard2.Click
        LoadWeek()

    End Sub

    Private Sub EnterKeySameAsTabKey_KeyPress(sender As Object, e As KeyPressEventArgs)
        EnterKeySameAsTabKey(e)
    End Sub

    Private Sub tabRange_Click(sender As Object, e As EventArgs) Handles tabRange.Click

    End Sub

    Private Sub tabRange_Enter(sender As Object, e As EventArgs) Handles tabRange.Enter
        tabRange.SelectNextControl(GroupBox2, True, True, True, True)
    End Sub

    Private Sub tabWeekCycle_Enter(sender As Object, e As EventArgs) Handles tabWeekCycle.Enter
        tabWeekCycle.SelectNextControl(tabWeekCycle, True, True, True, True)
    End Sub

    Private Sub gridWeek_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridWeek.CellContentClick

        Dim avoidColumns = {Column1.Name, Column6.Name}

        Dim gridWeekEditableColumns = gridWeek.Columns.OfType(Of DataGridViewColumn).
            Where(Function(col) Not avoidColumns.Any(Function(colName) colName = col.Name)).
            Where(Function(col) col.Visible)

        If gridWeek.Columns(e.ColumnIndex).Name = Column6.Name Then
            Dim _cellButton = gridWeek.Item(Column6.Name, e.RowIndex)

            Dim _currRow = gridWeek.Rows(e.RowIndex)
            _currRow.DefaultCellStyle = Nothing

            If Convert.ToString(_cellButton.Value) = ENABLED_TEXT _
                Or _cellButton.Value Is Nothing Then
                _cellButton.Value = DISABLED_TEXT

                'With _cellButton.Style
                With _currRow.DefaultCellStyle
                    .ForeColor = Color.Silver
                    '.BackColor = _color

                    .SelectionForeColor = Color.White
                    .SelectionBackColor = Color.LightGray
                End With

                For Each col In gridWeekEditableColumns
                    _currRow.Cells(col.Name).ReadOnly = True
                Next
            Else
                _cellButton.Value = ENABLED_TEXT
                _cellButton.Style = Nothing

                For Each col In gridWeekEditableColumns
                    _currRow.Cells(col.Name).ReadOnly = False
                Next

                TransformTouchedRow(_currRow)
            End If

        End If
    End Sub

    Private Sub gridWeek_KeyDown(sender As Object, e As KeyEventArgs) Handles gridWeek.KeyDown
        Console.WriteLine("gridWeek_KeyDown.KeyCode : {0}", e.KeyCode)
        Console.WriteLine("gridWeek_KeyDown.KeyData : {0}", e.KeyData)
        Console.WriteLine("gridWeek_KeyDown.KeyValue : {0}", e.KeyValue)

        'Dim keyData = e.KeyData
        'Dim fsdfsd = Keys.ControlKey And Keys.V
        'If keyData = fsdfsd Then Console.WriteLine("Clipboard.GetText : {0}", Clipboard.GetText)
    End Sub

    Private Sub gridWeek_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridWeek.KeyPress
        Console.WriteLine("gridWeek_KeyPress : {0}", e.KeyChar)

    End Sub

#End Region

End Class