Option Strict On

Imports System.Threading.Tasks
Imports AccuPay
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore

Public Class ShiftScheduleForm

#Region "VariableDeclarations"

    Const defaultWorkHours As Integer = 8

    Private organizationId As Integer

    Private sunDayName As String = DayNames.Sun.ToString

    Private _dutyShiftPolicy As DutyShiftPolicy

#End Region

#Region "Methods"

    Private Async Sub LoadDutyReference()
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

        gridWeek.DataSource = _models
    End Sub

    Private Async Sub LoadChangesAsync()
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
                                       _models)
            Next
        Next

        Dim eIDs = employees.Select(Function(e) e.RowID.Value).ToList
        Using context = New PayrollContext
            Dim _empShiftScheds = Await context.EmployeeDutySchedules.
                Include(Function(e) e.Employee).
                Where(Function(e) eIDs.Any(Function(eID) Equals(e.EmployeeID, eID))).
                Where(Function(e) e.DateSched.Value >= beginDate And e.DateSched.Value <= endDate).
                ToListAsync

            If _empShiftScheds.Any Then
                Dim notExists = Function(shSched As ShiftScheduleModel)
                                    Dim seekResult = _empShiftScheds.
                                    Where(Function(ess) Nullable.Equals(ess.EmployeeID, shSched.EmployeeId)).
                                    Where(Function(ess) Nullable.Equals(ess.DateSched.Value.Date, shSched.DateValue.Value.Date))

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

                Dim _dataSource = _models.OrderBy(Function(ssm) ssm.FullName.ToLower).ThenBy(Function(ssm) ssm.DateValue.Value.Date).ToList
                _models = _dataSource
            End If

            grid.DataSource = _models
        End Using

    End Sub

    Private Sub CollectShiftSchedModel(ee As ShiftScheduleModel, dateVal As Date?, modelList As ICollection(Of ShiftScheduleModel))
        Dim newEe As New ShiftScheduleModel With {
            .EmployeeId = ee.EmployeeId,
            .EmployeeNo = ee.EmployeeNo,
            .FullName = ee.FullName,
            .DateValue = dateVal,
            .TimeFrom = ShortTimeSpan(txtTimeFrom.Value),
            .TimeTo = ShortTimeSpan(txtTimeTo.Value),
            .BreakFrom = ShortTimeSpan(txtBreakFrom.Value),
            .BreakTo = ShortTimeSpan(txtBreakTo.Value)}

        If txtBreakLength.Value <= txtBreakLength.Minimum Then
            newEe.BreakLength = Nothing
        Else
            newEe.BreakLength = txtBreakLength.Value
        End If

        modelList.Add(newEe)
    End Sub

    Private Sub ZebraliseEmployeeRows()
        Dim ebonyStyle = Color.FromArgb(237, 237, 237)
        Dim ivoryStyle = Color.White

        Dim eIDs = EmployeeTreeView1.GetTickedEmployees.Select(Function(e) e.RowID.Value).ToList

        Dim isEven = False
        Dim i = 1

        For Each eID In eIDs
            isEven = i Mod 2 = 0

            If isEven Then
                ColorEmployeeRows(eID, ivoryStyle)
            Else
                ColorEmployeeRows(eID, ebonyStyle)
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
        If Not IsStartTimeCellParseable(dataGrid, e.ColumnIndex) Then
            Return
        End If

        Dim _currRow = dataGrid.Rows(e.RowIndex)
        Dim model = DirectCast(_currRow.DataBoundItem, ShiftScheduleModel)

        AutomaticShiftCompute(model)
    End Sub

    Private Sub AutomaticShiftCompute(shiftSched As ShiftScheduleModel)
        Dim shiftHours = _dutyShiftPolicy.DefaultWorkHour
        Dim breakHours = shiftSched.BreakLength.GetValueOrDefault

        Dim _empty = String.Empty

        Dim ClearBreakTimes = Sub(ss As ShiftScheduleModel)
                                  ss.BreakFrom = String.Empty
                                  ss.BreakTo = String.Empty
                              End Sub

        With shiftSched
            Dim startTime As TimeSpan? = Calendar.ToTimespan(.TimeFrom)
            Dim breakStartTime As TimeSpan? = Calendar.ToTimespan(.BreakFrom)

            If startTime.HasValue Then
                .TimeTo = MilitarDateTime(ToDateTime(startTime.Value).Value.AddHours(shiftHours))

                If String.IsNullOrWhiteSpace(.BreakFrom) Then
                    Dim firstHalf = Math.Floor(shiftHours / 2)
                    Dim breakStart = ToDateTime(startTime.Value).Value.AddHours(firstHalf)

                    breakHours = _dutyShiftPolicy.BreakHour
                    .BreakLength = breakHours

                    breakStartTime = breakStart.TimeOfDay
                    .BreakFrom = MilitarDateTime(breakStartTime.Value)

                    If breakStartTime.HasValue Then
                        .BreakTo = MilitarDateTime(breakStart.AddHours(breakHours))
                    Else
                        .BreakTo = _empty
                    End If

                Else
                    Dim breakEnd = TimeSpanAddMinute(.BreakFrom, breakHours)
                    If breakEnd.HasValue Then
                        .BreakTo = MilitarDateTime(breakEnd.Value)
                    Else
                        .BreakTo = String.Empty
                    End If
                End If

                If breakHours <= 0 Then
                    ClearBreakTimes(shiftSched)
                End If
            Else
                ClearBreakTimes(shiftSched)

                .TimeTo = _empty
            End If

        End With

    End Sub

    Private Sub SetMinCoveredDate()
        If dtpCoverDateFrom.Value < dtpDateFrom.Value Then
            dtpCoverDateFrom.Value = dtpDateFrom.Value
        End If
        dtpCoverDateFrom.MinDate = dtpDateFrom.Value
    End Sub

    Private Sub SetMaxCoveredDate()
        If dtpCoverDateTo.Value > dtpDateTo.Value Then
            dtpCoverDateTo.Value = dtpDateTo.Value
        End If
        dtpCoverDateTo.MaxDate = dtpDateTo.Value
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
            timeColumnIndexes = New Integer() {colTimeFrom.Index, colTimeTo.Index, colBreakTimeFrom.Index, colBreakTimeTo.Index}
        ElseIf dataGrid.Name = gridWeek.Name Then
            timeColumnIndexes = New Integer() {Column3.Index, Column4.Index, Column5.Index, Column6.Index}
        End If

        Return timeColumnIndexes.Any(Function(i) Equals(i, columnIndexNumber))
    End Function

    Private Function IsStartTimeCellParseable(dataGrid As DataGridView, columnIndexNumber As Integer) As Boolean
        Dim timeColumnIndexes() = New Integer() {}
        If dataGrid.Name = grid.Name Then
            timeColumnIndexes = New Integer() {colTimeFrom.Index, colBreakTimeFrom.Index, colBreakLength.Index}
        ElseIf dataGrid.Name = gridWeek.Name Then
            timeColumnIndexes = New Integer() {Column3.Index, Column5.Index, Column8.Index}
        End If

        Return timeColumnIndexes.Any(Function(i) Equals(i, columnIndexNumber))
    End Function

    Private Function ShortTimeSpan(ts As TimeSpan?) As String
        If ts.HasValue Then
            Return ts.Value.ToString("hh\:mm")
        Else
            Return String.Empty
        End If
    End Function

    Private Function TimeSpanAddMinute(text As String, addendMinute As Integer) As TimeSpan?
        Return TimeSpan.Parse(text).Add(New TimeSpan(0, CInt(60 * addendMinute), 0))
    End Function

    Public Function TimeSpanAddMinute(text As String, addendMinute As Decimal) As TimeSpan?
        Return TimeSpan.Parse(text).Add(New TimeSpan(0, CInt(60 * addendMinute), 0))
    End Function

    Private Function ToDateTime(timeSpanValue As TimeSpan) As Date?
        Return TimeUtility.ToDateTime(timeSpanValue)
    End Function

#End Region

#Region "Classes"

    Private Class ShiftScheduleModel

        Public Sub New()

        End Sub

        Public Sub New(employee As Employee)
            AssignEmployee(employee)
        End Sub

        Public Sub New(ess As EmployeeDutySchedule)
            AssignEmployee(ess.Employee)

            _DateValue = ess.DateSched
            _TimeFrom = ShiftScheduleForm.MilitarDateTime(ess.StartTime.GetValueOrDefault)
            _TimeTo = ShiftScheduleForm.MilitarDateTime(ess.EndTime.GetValueOrDefault)

            Dim _hasBreakStart = ess.BreakStartTime.HasValue
            If _hasBreakStart Then _BreakFrom = ShiftScheduleForm.MilitarDateTime(ess.BreakStartTime.GetValueOrDefault)

            Dim _hasBreakEnd = ess.BreakEndTime.HasValue
            If _hasBreakEnd Then _BreakTo = ShiftScheduleForm.MilitarDateTime(ess.BreakEndTime.GetValueOrDefault)

            If _hasBreakEnd And _hasBreakEnd Then
                Dim _breakDifference = ess.BreakEndTime.GetValueOrDefault.Subtract(ess.BreakStartTime.GetValueOrDefault).TotalHours
                _BreakLength = CDec(_breakDifference)
            Else
                _BreakLength = 0
            End If

            _IsRestDay = ess.IsRestDay
        End Sub

        Private Sub AssignEmployee(employee As Employee)
            _EmployeeId = employee.RowID
            _EmployeeNo = employee.EmployeeNo
            _FullName = String.Join(", ", employee.LastName, employee.FirstName)

        End Sub

        Public Property EmployeeId As Integer?
        Public Property EmployeeNo As String
        Public Property FullName As String
        Public Property DateValue As Date?
        Public Property TimeFrom As String
        Public Property TimeTo As String
        Public Property BreakFrom As String

        Public Property BreakLength As Decimal?

        Public Property BreakTo As String
        Public Property IsRestDay As Boolean

        Public ReadOnly Property DayName As String
            Get
                Return GetDayName(DateValue.Value)
            End Get
        End Property

        Public Shared Function GetDayName(dateValue As Date) As String
            Dim machineCulture As Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture

            Dim dayOfWeek As DayOfWeek = machineCulture.Calendar.GetDayOfWeek(dateValue)
            Return machineCulture.DateTimeFormat.GetDayName(dayOfWeek)
        End Function

    End Class

    Private Class DutyShiftPolicy
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

            _defaultWorkHour = settings.GetDecimal("DefaultShiftHour")
            _breakHour = settings.GetDecimal("BreakHour")
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

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click

        LoadChangesAsync()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
    End Sub

    Private Sub EmployeeTreeView1_TickedEmployee(s As Object, e As EventArgs) Handles EmployeeTreeView1.TickedEmployee
        LoadChangesAsync()
    End Sub

    Private Sub EmployeeTreeView1_Load(sender As Object, e As EventArgs) Handles EmployeeTreeView1.Load

    End Sub

    Private Sub dtpDateFrom_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateFrom.ValueChanged
        Dim start As Date = dtpDateFrom.Value.Date
        Dim finish As Date = dtpDateTo.Value.Date

        If start > finish Then
            dtpDateTo.Value = start
        Else
            LoadChangesAsync()
        End If

        SetMinCoveredDate()
    End Sub

    Private Sub dtpDateTo_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateTo.ValueChanged
        Dim start As Date = dtpDateFrom.Value.Date
        Dim finish As Date = dtpDateTo.Value.Date

        If start > finish Then
            dtpDateFrom.Value = finish
        Else
            LoadChangesAsync()
        End If

        SetMaxCoveredDate()
    End Sub

    Private Sub ShiftScheduleForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)
    End Sub

    Private Sub ShiftScheduleForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadDutyReference()

        organizationId = z_OrganizationID

        grid.AutoGenerateColumns = False
        gridWeek.AutoGenerateColumns = False

        EmployeeTreeView1.SwitchOrganization(organizationId)

        LoadWeek()

        SetMinCoveredDate()
        SetMaxCoveredDate()
    End Sub

    Private Sub grid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellContentClick

    End Sub

    Private Sub grid_DataSourceChanged(sender As Object, e As EventArgs) Handles grid.DataSourceChanged
        ZebraliseEmployeeRows()
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

    Private Function TimeSpanToString(text As String) As String
        Dim _value = Calendar.ToTimespan(text)
        Dim hasValue = _value.HasValue

        If hasValue Then
            Return _value.Value.ToString("hh\:mm")
        Else
            Return String.Empty
        End If
    End Function

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
                    txtBreakFrom.Clear()
                    txtBreakLength.Value = 0
                Else
                    txtBreakFrom.Text = MilitarDateTime(startDateTime.AddHours(Math.Floor(shiftHours / 2)))
                    txtBreakLength.Value = _dutyShiftPolicy.BreakHour
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
            txtBreakTo.Text = MilitarDateTime(breakStart.AddHours(txtBreakLength.Value))
        Else
            txtBreakTo.Clear()
            txtBreakLength.Value = txtBreakLength.Minimum
        End If
    End Sub

    Private Sub txtBreakTo_TextChanged(sender As Object, e As EventArgs) Handles txtBreakTo.TextChanged

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

    Private Sub gridWeek_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridWeek.CellContentClick

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

    End Sub

    Private Sub gridWeek_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles gridWeek.RowsAdded
        Dim row = gridWeek.Rows(e.RowIndex)
        If Equals(row.Cells(Column1.Name).Value, sunDayName) Then
            ColorizeSundays(row, Column2)
        End If

    End Sub

    Private Sub DataGrid_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) _
        Handles grid.CellEndEdit, gridWeek.CellEndEdit

        Dim _dataGrid = DirectCast(sender, DataGridView)
        ShiftProper(_dataGrid, e)

        _dataGrid.Refresh()
    End Sub

    Private Sub dtpCoverDateFrom_ValueChanged(sender As Object, e As EventArgs) Handles dtpCoverDateFrom.ValueChanged
        dtpCoverDateTo.MinDate = dtpCoverDateFrom.Value
    End Sub

    Private Sub dtpCoverDateTo_ValueChanged(sender As Object, e As EventArgs) Handles dtpCoverDateTo.ValueChanged
        dtpCoverDateFrom.MaxDate = dtpCoverDateTo.Value
    End Sub

#End Region

End Class