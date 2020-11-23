Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Interfaces
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.Services.Policies
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports log4net
Imports Microsoft.Extensions.DependencyInjection

Public Class ShiftScheduleForm

#Region "VariableDeclarations"

    Private Const ENABLED_TEXT As String = "Enabled"
    Private Const DISABLED_TEXT As String = "Disabled"
    Private Const FormEntityName As String = "Shift Schedule"

    Private Shared logger As ILog = LogManager.GetLogger("ShiftScheduleAppender")

    Private organizationId As Integer

    Private sunDayName As String = DayNames.Sun.ToString

    Private _dutyShiftPolicy As DutyShiftPolicy

    Private _originDataSource As List(Of ShiftScheduleModel)

    Private _currCell As DataGridViewCell

    Private _originalDates As TimePeriod

    Private _currentRolePermission As RolePermission
    Private _shiftBasedAutoOvertimePolicy As ShiftBasedAutomaticOvertimePolicy
    Private _isShiftBasedAutoOvertimeEnabled As Boolean

    Private WriteOnly Property ChangesCount As Integer
        Set(value As Integer)
            If value > 0 Then
                labelChangesCount.ForeColor = Color.Red
            Else
                labelChangesCount.ForeColor = Nothing
            End If

            labelChangesCount.Text = value.ToString("#,##0")
        End Set
    End Property

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Private ReadOnly _userActivityRepo As UserActivityRepository

#End Region

    Sub New()

        InitializeComponent()

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

        _userActivityRepo = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
    End Sub

#Region "Methods"

    Public Async Function LoadShiftScheduleConfigurablePolicy() As Task
        _dutyShiftPolicy = Await DutyShiftPolicy.Load1

        _shiftBasedAutoOvertimePolicy = _dutyShiftPolicy.ShiftBasedAutomaticOvertimePolicy
        _isShiftBasedAutoOvertimeEnabled = _shiftBasedAutoOvertimePolicy.Enabled

        txtBreakLength.Value = _dutyShiftPolicy.BreakHour
    End Function

    Private Sub LoadWeek()
        Dim sundayDate = GetSundayDate()
        Dim weekBoundaryDate = sundayDate.AddDays(DayOfWeek.Saturday)

        Dim sevenDays = Calendar.EachDay(sundayDate, weekBoundaryDate)

        Dim _models = New List(Of ShiftScheduleModel)
        For Each d In sevenDays
            _models.Add(New ShiftScheduleModel() With {.DateValue = d})
        Next

        RefreshDataSource(gridWeek, _models)
    End Sub

    Private Sub CollectShiftSchedModel(ee As Employee, dateVal As Date, modelList As ICollection(Of ShiftScheduleModel), isRaw As Boolean)
        Dim shiftModelSchedModel =
            New ShiftScheduleModel(ee) With {
            .DateValue = dateVal}

        If Not isRaw Then ApplyChangesToModel(shiftModelSchedModel)

        modelList.Add(shiftModelSchedModel)
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
                                  ss.BreakFrom = Nothing
                                  ss.BreakLength = Nothing
                              End Sub

        With shiftSched
            Dim startTime As TimeSpan? = Calendar.ToTimespan(.TimeFrom)
            Dim breakStartTime As TimeSpan? = Calendar.ToTimespan(.BreakFrom)

            If startTime.HasValue Then
                If .IsEmptyTimeTo Then
                    breakHours = _dutyShiftPolicy.BreakHour
                    .BreakLength = breakHours
                    .TimeTo = MilitarDateTime(ToDateTime(startTime.Value, .DateValue).Value.AddHours(shiftHours + breakHours))
                End If

                If String.IsNullOrWhiteSpace(.BreakFrom) AndAlso .BreakLength > 0 Then
                    Dim firstHalf = Math.Floor(shiftHours / 2)
                    Dim breakStart = ToDateTime(startTime.Value, .DateValue).Value.AddHours(firstHalf)

                    breakHours = _dutyShiftPolicy.BreakHour
                    .BreakLength = breakHours

                    breakStartTime = breakStart.TimeOfDay

                    If breakStartTime.HasValue Then
                        .BreakFrom = MilitarDateTime(breakStartTime.Value)
                    Else
                        ClearBreakTimes(shiftSched)
                    End If

                    'ElseIf Not String.IsNullOrWhiteSpace(.BreakFrom) Then
                    '    If .BreakLength = 0 Then
                    '        breakHours = _dutyShiftPolicy.BreakHour
                    '        .BreakLength = breakHours
                    '    End If

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
        ChangesCount = _newSource.Where(Function(data) data.HasChanged).Count
    End Sub

    Private Sub NoAffectedRows()
        ChangesCount = 0
    End Sub

    Private Sub ShowSuccessBalloon()
        InfoBalloon("Successfully saved.",
                  "Successfully saved.", btnSave, 0, -70)
    End Sub

    Private Sub ShowSuccessImportBalloon()
        InfoBalloon("Imported successfully.",
                  "Imported successfully.", DateFilterGroupBox, 0, -70)
    End Sub

    Private Sub RecordUpdate(oldRecords As IEnumerable(Of EmployeeDutySchedule), updatedShifts As List(Of EmployeeDutySchedule))
        For Each ssm In updatedShifts
            Dim changes As New List(Of UserActivityItem)
            Dim entityName = FormEntityName.ToLower()

            Dim oldShifts = oldRecords.Where(Function(x) x.RowID = ssm.RowID).FirstOrDefault()

            If oldShifts Is Nothing Then Return

            Dim suffixIdentifier = $"of shift with date '{ssm.DateSched.ToShortDateString()}'."

            If Not Nullable.Equals(ssm.StartTime, oldShifts.StartTime) Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = ssm.RowID,
                    .Description = $"Updated start time from '{oldShifts.StartTime.ToStringFormat("hh:mm tt")}' to '{ssm.StartTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    .ChangedEmployeeId = oldShifts.EmployeeID.Value
                })
            End If
            If Not Nullable.Equals(ssm.EndTime, oldShifts.EndTime) Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = ssm.RowID,
                    .Description = $"Updated end time from '{oldShifts.EndTime.ToStringFormat("hh:mm tt")}' to '{ssm.EndTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    .ChangedEmployeeId = oldShifts.EmployeeID.Value
                })
            End If
            If Not Nullable.Equals(ssm.BreakStartTime, oldShifts.BreakStartTime) Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = ssm.RowID,
                    .Description = $"Updated break start from '{oldShifts.BreakStartTime.ToStringFormat("hh:mm tt")}' to '{ssm.BreakStartTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    .ChangedEmployeeId = oldShifts.EmployeeID.Value
                })
            End If
            If oldShifts.BreakLength <> ssm.BreakLength Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = ssm.RowID,
                    .Description = $"Updated break length from '{oldShifts.BreakLength}' to '{ssm.BreakLength}' {suffixIdentifier}",
                    .ChangedEmployeeId = oldShifts.EmployeeID.Value
                })
            End If
            If oldShifts.IsRestDay <> ssm.IsRestDay Then
                changes.Add(New UserActivityItem() With
                {
                    .EntityId = ssm.RowID,
                    .Description = $"Updated offset from '{oldShifts.IsRestDay}' to '{ssm.IsRestDay}' {suffixIdentifier}",
                    .ChangedEmployeeId = oldShifts.EmployeeID.Value
                })
            End If

            _userActivityRepo.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)
        Next
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
            timeColumnIndexes = New Integer() {colStartTime.Index, colEndTime.Index, Column5.Index}
        End If

        Return timeColumnIndexes.Any(Function(i) Equals(i, columnIndexNumber))
    End Function

    Private Function IsStartTimeCellParseable(dataGrid As DataGridView, columnIndexNumber As Integer) As Boolean
        Dim timeColumnIndexes() = New Integer() {}
        If dataGrid.Name = grid.Name Then
            timeColumnIndexes = New Integer() {colTimeFrom.Index, colBreakTimeFrom.Index, colBreakLength.Index, colIsRestDay.Index}
            If _isShiftBasedAutoOvertimeEnabled Then timeColumnIndexes = timeColumnIndexes.Concat({colTimeTo.Index}).ToArray()
        ElseIf dataGrid.Name = gridWeek.Name Then
            timeColumnIndexes = New Integer() {colStartTime.Index, Column5.Index, gridWeekBreakLength.Index, Column7.Index}
            If _isShiftBasedAutoOvertimeEnabled Then timeColumnIndexes = timeColumnIndexes.Concat({colEndTime.Index}).ToArray()
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

    Private Function ToDateTime(timeSpanValue As TimeSpan, [date] As Date) As Date?
        Return TimeUtility.ToDateTime(timeSpanValue, [date])
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
            Dim emloyee = employees.Where(Function(e) CBool(e.RowID = ee.EmployeeId)).FirstOrDefault()
            For Each dateVal In dates
                CollectShiftSchedModel(emloyee,
                                       dateVal,
                                       _models,
                                       isRawData)
            Next
        Next

        Return _models
    End Function

    Private Async Function RangeApply(datePeriod As TimePeriod, models As List(Of ShiftScheduleModel)) As Task(Of List(Of ShiftScheduleModel))

        Dim employees = EmployeeTreeView1.GetTickedEmployees
        Dim employeeIds = employees.Select(Function(e) e.RowID.Value).ToArray()

        Dim isShiftBasedAutoOvertimeEnabled = _isShiftBasedAutoOvertimeEnabled
        Dim repository = MainServiceProvider.GetRequiredService(Of EmployeeDutyScheduleRepository)
        Dim empShiftScheds = Await repository.GetByEmployeeAndDatePeriodWithEmployeeAsync(
            z_OrganizationID,
            employeeIds,
            datePeriod)

        If empShiftScheds.Any Then
            Dim notExists = Function(shSched As ShiftScheduleModel)
                                Dim seekResult = empShiftScheds.
                                Where(Function(ess) Nullable.Equals(ess.EmployeeID, shSched.EmployeeId)).
                                Where(Function(ess) Nullable.Equals(ess.DateSched.Date, shSched.DateValue.Date))

                                Return Not seekResult.Any
                            End Function
            Dim notInDb = models.Where(Function(ssm) notExists(ssm)).ToList

            models.Clear()
            For Each ess In empShiftScheds
                models.Add(New ShiftScheduleModel(ess))
            Next

            For Each ssm In notInDb
                models.Add(ssm)
            Next

            Dim dataSource = models.
                OrderBy(Function(ssm) ssm.FullName.ToLower).
                ThenBy(Function(ssm) ssm.DateValue.Date).
                ToList
            models = dataSource
        End If

        Return models
    End Function

    Private Function ConvertGridRowsToShiftScheduleModels(dataGrid As DataGridView) As List(Of ShiftScheduleModel)
        Dim _weekCycleRows = dataGrid.Rows.OfType(Of DataGridViewRow)
        If gridWeek.Name = dataGrid.Name Then
            Dim satisfied = Function(dgvRow As DataGridViewRow) As Boolean
                                Dim _cell = dgvRow.Cells(Column6.Name)
                                Dim isDefaultValue = _cell.Value Is Nothing
                                Dim isApplied = Convert.ToString(_cell.Value) = ENABLED_TEXT
                                Dim _satisfied = isApplied OrElse isDefaultValue

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

    Function GridSelectedCells() As IEnumerable(Of DataGridViewCell)
        Return grid.SelectedCells.OfType(Of DataGridViewCell)
    End Function

#End Region

#Region "Classes"

    Private Class ShiftScheduleModel
        Implements IShift
        Private Const ONE_DAY_HOURS As Integer = 24
        Private Const MINUTES_PER_HOUR As Integer = 60
        Private origStartTime, origEndTime, origBreakStart As String
        Private origOffset As Boolean
        Private origBreakLength As Decimal
        Private _isNew, _madeChanges, _isValid As Boolean
        Private _eds As EmployeeDutySchedule

        Public Sub New()
        End Sub

        Public Sub New(employee As Employee)
            AssignEmployee(employee)
        End Sub

        Public Sub New(ess As EmployeeDutySchedule)
            InitModel(ess)
        End Sub

        Private Sub InitModel(ess As EmployeeDutySchedule)
            _eds = ess

            _RowID = ess.RowID
            AssignEmployee(ess.Employee)

            _DateValue = ess.DateSched

            Dim timeFrom = ess.StartTime
            Dim timeTo = ess.EndTime

            Me.TimeFrom = If(timeFrom Is Nothing, "", ShiftScheduleForm.MilitarDateTime(timeFrom.GetValueOrDefault))
            Me.TimeTo = If(timeTo Is Nothing, "", ShiftScheduleForm.MilitarDateTime(timeTo.GetValueOrDefault))

            _BreakLength = ess.BreakLength

            Dim _hasBreakStart = ess.BreakStartTime.HasValue
            If _hasBreakStart Then

                Dim breakFrom = ess.BreakStartTime
                Me.BreakFrom = If(breakFrom Is Nothing, "", ShiftScheduleForm.MilitarDateTime(breakFrom.GetValueOrDefault))

            End If

            _IsRestDay = ess.IsRestDay

            origStartTime = _TimeFrom
            origEndTime = _TimeTo

            origBreakStart = _BreakFrom
            origBreakLength = _BreakLength

            origOffset = _IsRestDay
        End Sub

        Private Sub AssignEmployee(employee As Employee)
            _EmployeeId = employee.RowID
            _EmployeeNo = employee.EmployeeNo
            _FullName = String.Join(", ", employee.LastName, employee.FirstName)

        End Sub

        Public Property RowID As Integer
        Public Property EmployeeId As Integer? Implements IShift.EmployeeId
        Public Property EmployeeNo As String
        Public Property FullName As String
        Public Property DateValue As Date Implements IShift.Date

        Public Property TimeFrom As String

        Public Property TimeTo As String

        Public Property BreakFrom As String

        Public Property BreakLength As Decimal Implements IShift.BreakLength

        Public Property IsRestDay As Boolean Implements IShift.IsRestDay

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
                _madeChanges = Not Equals(origStartTime, TimeFrom) _
                    OrElse Not Equals(origEndTime, TimeTo) _
                    OrElse Not Equals(origBreakStart, BreakFrom) _
                    OrElse Not Equals(origBreakLength, _BreakLength) _
                    OrElse Not Equals(origOffset, _IsRestDay)

                Return _madeChanges
            End Get
        End Property

        Public ReadOnly Property IsValidToSave As Boolean
            Get
                Dim hasShiftTime = Not String.IsNullOrWhiteSpace(TimeFrom) _
                    AndAlso Not String.IsNullOrWhiteSpace(TimeTo)

                _isValid = hasShiftTime OrElse _IsRestDay

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
                Return HasNewId() AndAlso IsValidToSave
            End Get
        End Property

        Private Function HasNewId() As Boolean
            Return (_eds?.RowID).GetValueOrDefault() <= 0
        End Function

        Public ReadOnly Property IsUpdate As Boolean
            Get
                Return Not IsNew AndAlso _madeChanges AndAlso IsValidToSave
            End Get
        End Property

        Public ReadOnly Property ConsideredDelete As Boolean
            Get
                Dim _deleteable = Not HasNewId() AndAlso Not IsValidToSave AndAlso IsExist

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
                    .StartTime = TimeFromTimeSpan
                    .EndTime = TimeToTimeSpan
                    .BreakStartTime = BreakFromTimeSpan
                    .BreakLength = _BreakLength
                    .IsRestDay = _IsRestDay
                End With

                Return _eds
            End Get
        End Property

        Public Sub CommitChanges()
            origStartTime = _TimeFrom
            origEndTime = _TimeTo

            origBreakStart = _BreakFrom
            origBreakLength = _BreakLength

            origOffset = _IsRestDay
        End Sub

        Public ReadOnly Property IsEmptyTimeTo As Boolean
            Get
                Return String.IsNullOrWhiteSpace(TimeTo)
            End Get
        End Property

        Public ReadOnly Property TimeFromTimeSpan As TimeSpan? Implements IShift.StartTime
            Get
                Return Calendar.ToTimespan(TimeFrom)
            End Get
        End Property

        Public ReadOnly Property TimeFromDateTime As DateTime?
            Get
                Return TimeUtility.ToDateTime(TimeFromTimeSpan, DateValue)
            End Get
        End Property

        Public ReadOnly Property TimeToTimeSpan As TimeSpan? Implements IShift.EndTime
            Get
                Return Calendar.ToTimespan(TimeTo)
            End Get
        End Property

        Public ReadOnly Property TimeToDateTime As DateTime?
            Get
                If TimeFromTimeSpan.HasValue AndAlso TimeToTimeSpan.HasValue Then
                    If TimeFromTimeSpan.Value.Hours > TimeToTimeSpan.Value.Hours Then
                        Return TimeUtility.ToDateTime(TimeToTimeSpan.Value.AddOneDay, DateValue)
                    End If
                End If

                Return TimeUtility.ToDateTime(TimeToTimeSpan, DateValue)
            End Get
        End Property

        Public ReadOnly Property BreakFromTimeSpan As TimeSpan? Implements IShift.BreakTime
            Get
                Return Calendar.ToTimespan(BreakFrom)
            End Get
        End Property

        Public ReadOnly Property BreakTimeDateTime As DateTime?
            Get
                Dim breakTime = Calendar.ToTimespan(BreakFrom)
                Return TimeUtility.ToDateTime(breakTime, DateValue)
            End Get
        End Property

    End Class

    Private Class DutyShiftPolicy
        Private Const DEFAULT_SHIFT_HOUR As Integer = 9
        Private Const DEFAULT_BREAK_HOUR As Integer = 1

        Private _dutyShiftPolicy As IEnumerable(Of ListOfValue)

        Private _listOfValueRepository As ListOfValueRepository

        Private _listOfValueService As ListOfValueService

        Private settings As ListOfValueCollection = Nothing

        Private _defaultWorkHour, _breakHour As Decimal
        Private _shiftBasedAutoOvertimePolicy As ShiftBasedAutomaticOvertimePolicy

        Private Sub New()

            _listOfValueRepository = MainServiceProvider.GetRequiredService(Of ListOfValueRepository)

            _listOfValueService = MainServiceProvider.GetRequiredService(Of ListOfValueService)
        End Sub

        Public Shared Async Function Load1() As Task(Of DutyShiftPolicy)
            Dim policy = New DutyShiftPolicy
            Await policy.Load()
            Return policy
        End Function

        Private Async Function Load() As Task
            _dutyShiftPolicy = Await _listOfValueRepository.GetDutyShiftPoliciesAsync()

            settings = _listOfValueService.Create(_dutyShiftPolicy.ToList)

            _defaultWorkHour = settings.GetDecimal("DefaultShiftHour", DEFAULT_SHIFT_HOUR)
            _breakHour = settings.GetDecimal("BreakHour", DEFAULT_BREAK_HOUR)

            _shiftBasedAutoOvertimePolicy = New ShiftBasedAutomaticOvertimePolicy(settings)
        End Function

        Public ReadOnly Property DefaultWorkHour() As Decimal
            Get
                Return _defaultWorkHour - _breakHour
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

        Public ReadOnly Property ShiftBasedAutomaticOvertimePolicy As ShiftBasedAutomaticOvertimePolicy
            Get
                Return _shiftBasedAutoOvertimePolicy
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

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply1.Click, btnApply2.Click
        CommitTimeValues()

        Dim _currRowIndex, _currColIndex As Integer
        Dim selectedCell = GridSelectedCells()
        Dim hasSelectedCell = selectedCell.Any
        If hasSelectedCell Then
            _currCell = selectedCell.FirstOrDefault
            _currRowIndex = _currCell.RowIndex
            _currColIndex = _currCell.ColumnIndex
        Else
            MessageBoxHelper.Warning("No selected employees.")
        End If

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

        Dim rowCount = grid.Rows.Count

        If hasSelectedCell Then
            If _currCell IsNot Nothing _
                AndAlso _currRowIndex > -1 _
                AndAlso rowCount > _currRowIndex Then _
                grid.CurrentCell = grid.Item(_currColIndex, _currRowIndex)
        End If
    End Sub

    Private Async Sub btnSave_ClickAsync(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim saveList = ConvertGridRowsToShiftScheduleModels(grid)

        Dim toSaveList = saveList.Where(Function(ssm) ssm.HasChanged)
        If Not toSaveList.Any() Then

            MessageBoxHelper.Warning("No unsaved changes!")
            Return
        End If

        Dim toSaveListEmployeeIDs = toSaveList.Select(Function(tlm) tlm.EmployeeId.Value).ToArray()

        Dim datePeriod = New TimePeriod(dtpDateFrom.Value.Date, dtpDateTo.Value.Date)

        Dim addedShifts As New List(Of EmployeeDutySchedule)
        Dim updatedShifts As New List(Of EmployeeDutySchedule)
        Dim deletedShifts As New List(Of EmployeeDutySchedule)

        For Each ssm In toSaveList

            If ssm.IsNew Then
                addedShifts.Add(ssm.ToEmployeeDutySchedule)
            ElseIf ssm.IsUpdate Then
                updatedShifts.Add(ssm.ToEmployeeDutySchedule)
            ElseIf ssm.ConsideredDelete Then
                deletedShifts.Add(ssm.ToEmployeeDutySchedule)
            End If

        Next

        If Not addedShifts.Any() AndAlso
            Not updatedShifts.Any() AndAlso
            Not deletedShifts.Any() Then

            MessageBoxHelper.Warning("No valid shifts to save.")
            Return

        End If

        Dim allowedActions As New List(Of String)
        If _currentRolePermission.Create Then allowedActions.Add("CREATE")
        If _currentRolePermission.Update Then allowedActions.Add("UPDATE")
        If _currentRolePermission.Delete Then allowedActions.Add("DELETE")

        Dim allowedActionsMessage = $"You are only allowed to perform ({ _
            String.Join(", ", allowedActions.ToArray())}) actions."

        Const UnathorizedActionTitle As String = "Unauthorized Action"

        If addedShifts.Any() AndAlso Not _currentRolePermission.Create Then

            MessageBoxHelper.Warning("You are prohibited to create new shifts. " & allowedActionsMessage, UnathorizedActionTitle)
            Return
        End If

        If updatedShifts.Any() AndAlso Not _currentRolePermission.Update Then

            MessageBoxHelper.Warning("You are prohibited to update existing shifts. " & allowedActionsMessage, UnathorizedActionTitle)
            Return
        End If

        If deletedShifts.Any() AndAlso Not _currentRolePermission.Delete Then

            MessageBoxHelper.Warning("You are prohibited to delete existing shifts. " & allowedActionsMessage, UnathorizedActionTitle)
            Return
        End If

        Dim repository = MainServiceProvider.GetRequiredService(Of EmployeeDutyScheduleRepository)
        Dim oldRecords = Await repository.GetByEmployeeAndDatePeriodAsync(
            z_OrganizationID,
            toSaveListEmployeeIDs,
            datePeriod)

        Await FunctionUtils.TryCatchFunctionAsync("Save Shift",
            Async Function()
                If _isShiftBasedAutoOvertimeEnabled Then Await SaveOvertimeOfShiftBasedAutoOvertimePolicy(toSaveList)

                Dim dataService = MainServiceProvider.GetRequiredService(Of EmployeeDutyScheduleDataService)
                Await dataService.ChangeManyAsync(
                    organizationId:=z_OrganizationID,
                    added:=addedShifts,
                    updated:=updatedShifts,
                    deleted:=deletedShifts)

                For Each ssm In toSaveList

                    If ssm.ConsideredDelete Then
                        ssm.RemoveShift()
                    End If

                    ssm.CommitChanges()
                Next

                RecordUserActivity(addedShifts, updatedShifts, deletedShifts, oldRecords)

                ShowSuccessBalloon()

                For Each row As DataGridViewRow In grid.Rows
                    row.DefaultCellStyle = Nothing
                Next

                ZebraliseEmployeeRows()

                NoAffectedRows()
            End Function)
    End Sub

    Private Async Function SaveOvertimeOfShiftBasedAutoOvertimePolicy(toSaveList As IEnumerable(Of ShiftScheduleModel)) As Task
        Dim employees = EmployeeTreeView1.GetTickedEmployees
        Dim employeeIds = employees.Select(Function(ee) ee.RowID.Value).ToList()

        Dim modifiedShifts = toSaveList.
            Where(Function(sh) sh.IsNew OrElse sh.IsUpdate OrElse sh.ConsideredDelete).
            ToList()

        Dim overtimeDataService = MainServiceProvider.GetRequiredService(Of OvertimeDataService)

        Await overtimeDataService.GenerateOvertimeByShift(modifiedShifts, employeeIds, z_OrganizationID, z_User)
    End Function

    Private Sub RecordUserActivity(addedShiftSchedules As List(Of EmployeeDutySchedule), updatedShiftSchedules As List(Of EmployeeDutySchedule), deletedShiftSchedules As List(Of EmployeeDutySchedule), oldRecords As ICollection(Of EmployeeDutySchedule))
        For Each ssm In addedShiftSchedules      'for new
            _userActivityRepo.RecordAdd(
                z_User,
                FormEntityName,
                entityId:=ssm.RowID,
                organizationId:=z_OrganizationID,
                suffixIdentifier:=$" with date '{ssm.DateSched.ToShortDateString()}'",
                changedEmployeeId:=ssm.EmployeeID.Value)
        Next

        RecordUpdate(oldRecords, updatedShiftSchedules)

        For Each ssm In deletedShiftSchedules   'for delete
            _userActivityRepo.RecordDelete(
                z_User,
                FormEntityName,
                entityId:=ssm.RowID,
                organizationId:=z_OrganizationID,
                suffixIdentifier:=$" with date '{ssm.DateSched.ToShortDateString()}'",
                changedEmployeeId:=ssm.EmployeeID.Value)
        Next
    End Sub

    Private Sub btnUserActivity_Click(sender As Object, e As EventArgs) Handles btnUserActivity.Click
        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Async Sub EmployeeTreeView1_TickedEmployee(sender As Object, e As EventArgs) Handles EmployeeTreeView1.EmployeeTicked

        Await RefreshGridByDateFilter()
    End Sub

    Private Sub ShiftScheduleForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
    End Sub

    Private Async Sub ShiftScheduleForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        Await LoadShiftScheduleConfigurablePolicy()

        Await CheckRolePermissions()

        organizationId = z_OrganizationID

        Dim grids = {grid, gridWeek}
        For Each dGrid In grids
            dGrid.AutoGenerateColumns = False
            dGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText

        Next

        Await EmployeeTreeView1.SwitchOrganization(organizationId)

        LoadWeek()

        Dim ctrls = New Control() {dtpDateFrom, dtpDateTo, txtTimeFrom, txtTimeTo, txtBreakFrom, txtBreakLength}
        For Each c In ctrls
            AddHandler c.KeyPress, AddressOf EnterKeySameAsTabKey_KeyPress
        Next

        Dim currentlyWorkedOnPayPeriod = Await _payPeriodRepository.GetOpenOrCurrentPayPeriodAsync(
            organizationId:=z_OrganizationID,
            currentUserId:=z_User)

        If currentlyWorkedOnPayPeriod IsNot Nothing Then

            dtpDateFrom.Value = currentlyWorkedOnPayPeriod.PayFromDate
            dtpDateTo.Value = currentlyWorkedOnPayPeriod.PayToDate

        End If

    End Sub

    Private Async Function CheckRolePermissions() As Task
        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.SHIFT)

        tsBtnImport.Visible = True
        tcSchedule.Visible = True
        ActionPanel.Visible = True
        grid.ReadOnly = False

        If role.Success Then

            _currentRolePermission = role.RolePermission

            If Not _currentRolePermission.Create AndAlso Not _currentRolePermission.Update Then

                tsBtnImport.Visible = False

                If Not _currentRolePermission.Delete Then

                    tcSchedule.Visible = False
                    ActionPanel.Visible = False
                    grid.ReadOnly = True

                End If

            End If
        End If
    End Function

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

        If e.ColumnIndex < 0 Then Return

        Dim selectedGridView = DirectCast(sender, DataGridView)

        If (sender Is grid AndAlso selectedGridView.Columns(e.ColumnIndex) Is colBreakLength) OrElse
            (sender Is gridWeek AndAlso selectedGridView.Columns(e.ColumnIndex) Is gridWeekBreakLength) Then

            Dim decimalValue = ObjectUtils.ToNullableDecimal(e.Value)

            If decimalValue Is Nothing Then
                e.Value = 0D

                Dim hasValue = True
                e.ParsingApplied = hasValue
            Else
                e.ParsingApplied = False
            End If

        ElseIf IsCellTimeParseable(selectedGridView, e.ColumnIndex) Then
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
                Dim startDateTime = ToDateTime(tsFrom.Value, Date.Now.Date).Value

                If String.IsNullOrWhiteSpace(txtBreakFrom.Text) AndAlso _dutyShiftPolicy.BreakHour > 0 Then
                    txtBreakFrom.Text = MilitarDateTime(startDateTime.AddHours(Math.Floor(shiftHours / 2)))
                    txtBreakLength.Value = _dutyShiftPolicy.BreakHour
                End If

                Dim breakLength = txtBreakLength.Value
                Dim nextTimeTo = startDateTime.AddHours(shiftHours + breakLength)
                txtTimeTo.Text = MilitarDateTime(nextTimeTo)
            Else
                txtBreakFrom.Clear()
                txtBreakLength.Value = txtBreakLength.Minimum
            End If
        End If
    End Sub

    Private Sub txtBreakLength_ValueChanged(sender As Object, e As EventArgs) Handles txtBreakLength.ValueChanged
        If Not String.IsNullOrWhiteSpace(txtTimeFrom.Text) AndAlso txtBreakLength.Value > 0 AndAlso String.IsNullOrWhiteSpace(txtBreakFrom.Text) Then
            Dim startDateTime = ToDateTime(Calendar.ToTimespan(txtTimeFrom.Text.Trim).Value, Date.Now.Date).Value
            Dim shiftHours = _dutyShiftPolicy.DefaultWorkHour
            txtBreakFrom.Text = MilitarDateTime(startDateTime.AddHours(Math.Floor(shiftHours / 2)))
        End If
    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Close()
    End Sub

    Private Sub txtBreakLength_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBreakLength.KeyDown
        If e.KeyCode = Keys.Back Or
            e.KeyCode = Keys.Delete Then

            Dim isEmpty = String.IsNullOrWhiteSpace(txtBreakLength.Text)
            If isEmpty Then
                e.Handled = True
                txtBreakLength.Text = "0.00"
            End If
        End If
    End Sub

    Private Sub grid_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles grid.ColumnWidthChanged

    End Sub

    Private Sub grid_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles grid.CellPainting
        Dim isSatisfy = e.ColumnIndex = colIsRestDay.Index AndAlso e.RowIndex > -1
        If isSatisfy Then
            Dim isFormattedValue = If(CBool(e.FormattedValue), ButtonState.Checked, ButtonState.Normal)

            e.PaintBackground(e.CellBounds, True)
            ControlPaint.DrawCheckBox(e.Graphics, e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 2, e.CellBounds.Height - 2, isFormattedValue)

            e.Handled = True
        End If
    End Sub

    Private Sub gridWeek_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles gridWeek.CellPainting
        Dim isSatisfy = e.ColumnIndex = Column7.Index AndAlso e.RowIndex > -1
        If isSatisfy Then
            Dim isFormattedValue = If(CBool(e.FormattedValue), ButtonState.Checked, ButtonState.Normal)

            e.PaintBackground(e.CellBounds, True)
            ControlPaint.DrawCheckBox(e.Graphics, e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 2, e.CellBounds.Height - 2, isFormattedValue)

            e.Handled = True
        End If
    End Sub

    Private Sub gridWeek_CellParsing(sender As Object, e As DataGridViewCellParsingEventArgs) Handles gridWeek.CellParsing

    End Sub

    Private Async Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        Await RefreshGridByDateFilter()

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

        If _dataGrid.Name = grid.Name Then
            Dim shiftSchedModels = ConvertGridRowsToShiftScheduleModels(grid)
            AffectedRows(shiftSchedModels)
        End If
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

        Await RefreshGridByDateFilter()

    End Sub

    Private Async Function RefreshGridByDateFilter() As Task

        Dim start As Date = dtpDateFrom.Value.Date
        Dim finish As Date = dtpDateTo.Value.Date

        If start > finish Then Return

        Me.Cursor = Cursors.WaitCursor

        SplitContainer1.Enabled = False

        Dim _currRowIndex, _currColIndex As Integer
        Dim selectedCell = GridSelectedCells()
        Dim hasSelectedCell = selectedCell.Any
        If hasSelectedCell Then
            _currCell = selectedCell.FirstOrDefault
            _currRowIndex = _currCell.RowIndex
            _currColIndex = _currCell.ColumnIndex
        End If

        _originDataSource = Await RangeApply(New TimePeriod(start, finish), CreatedResult(True))
        RefreshDataSource(grid, _originDataSource)

        NoAffectedRows()

        Dim rowCount = grid.Rows.Count

        If hasSelectedCell Then
            If _currCell IsNot Nothing _
                AndAlso _currRowIndex > -1 _
                AndAlso rowCount > _currRowIndex Then _
                grid.CurrentCell = grid.Item(_currColIndex, _currRowIndex)
        End If

        'BUG
        'this is called twice when changing the datepicker which results to
        'the Cursor changing to default but does not enable the form for quite some time
        SplitContainer1.Enabled = True

        Me.Cursor = Cursors.Default
    End Function

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
                OrElse _cellButton.Value Is Nothing Then
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

    Private Sub gridWeek_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridWeek.KeyPress
        Console.WriteLine("gridWeek_KeyPress : {0}", e.KeyChar)

    End Sub

    Private Sub labelChangesCount_TextChanged(sender As Object, e As EventArgs) Handles labelChangesCount.TextChanged
        Dim isNotZero = labelChangesCount.Text <> "0"
        labelChangesCount.Visible = isNotZero
        labelAffectedRows.Visible = isNotZero

        'btnReset.Enabled = isNotZero
        'btnSave.Enabled = isNotZero
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        Dim _text = Clipboard.GetText()

        Dim _selectedCells = gridWeek.SelectedCells.OfType(Of DataGridViewCell)
        For Each _cell In _selectedCells
            _cell.Value = _text
            DataGrid_CellEndEdit(gridWeek, New DataGridViewCellEventArgs(_cell.ColumnIndex, _cell.RowIndex))
        Next
    End Sub

    Private Sub PasteToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem1.Click
        Dim _text = Clipboard.GetText()

        Dim _selectedCells = grid.SelectedCells.OfType(Of DataGridViewCell).Where(Function(_cell) Not _cell.ReadOnly)
        For Each _cell In _selectedCells
            _cell.Value = _text
            DataGrid_CellEndEdit(grid, New DataGridViewCellEventArgs(_cell.ColumnIndex, _cell.RowIndex))
        Next
    End Sub

    Private Sub grid_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles grid.CellMouseUp

        If grid.CurrentCell Is Nothing Then Return

        Dim currentColumnIndex = grid.CurrentCell.ColumnIndex

        If currentColumnIndex < 0 Then Return

        Dim currentColumn = grid.Columns(currentColumnIndex)

        If currentColumn IsNot colIsRestDay Then Return

        grid.EndEdit()

    End Sub

    Private Async Sub tsBtnImport_Click(sender As Object, e As EventArgs) Handles tsBtnImport.Click

        Using form = New ImportedShiftSchedulesForm(_shiftBasedAutoOvertimePolicy)
            form.ShowDialog()

            If form.IsSaved Then

                Await RefreshGridByDateFilter()

                ShowSuccessImportBalloon()

            End If

        End Using

    End Sub

#End Region

End Class
