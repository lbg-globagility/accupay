Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore

Public Class TimeLogsForm2

#Region "VariableDeclarations"

    Private Const SUNDAY_SHORT_NAME As String = "Sun"

    Private currRowIndex As Integer = -1
    Private currColIndex As Integer = -1

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

            Dim dataSource As New List(Of TimeLogModel)

            Dim models = CreatedResults(employees, startDate, endDate)

            Dim timeLogs = Await context.TimeLogs.
                Include(Function(etd) etd.Employee).
                Where(Function(etd) employeeIDs.Contains(etd.EmployeeID.Value)).
                Where(Function(etd) etd.LogDate >= startDate AndAlso etd.LogDate <= endDate).
                ToListAsync()
            'employeeIDs.Any(Function(eID) etd.EmployeeID.Value = eID)

            For Each model In models
                Dim seek = timeLogs.
                    Where(Function(etd) etd.EmployeeID.Value = model.EmployeeID).
                    Where(Function(etd) etd.LogDate = model.DateIn)

                If seek.Any Then
                    Dim timeLog = seek.FirstOrDefault
                    dataSource.Add(New TimeLogModel(timeLog))
                Else
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

        Dim sundayRows = rows.Where(Function(row) Convert.ToString(row.Cells(Day.Name).FormattedValue) = SUNDAY_SHORT_NAME)

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
                          Equals(Convert.ToInt32(row.Cells(EmployeeID.Name).Value),
                                 employeePrimKey))

        For Each eGridRow In employeeGridRows
            eGridRow.DefaultCellStyle.BackColor = backColor
        Next
    End Sub

    Private Sub CountHasChanged()
        Dim models = GridRowToTimeLogModels(grid)

        Dim countHasChanged = models.Where(Function(tlm) tlm.HasChanged).ToList()
        labelChangedCount.Text = countHasChanged.Count.ToString("#,##0")
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

#End Region

#Region "PrivateClass"
    Private Class TimeLogModel
        Private Const CUSTOM_SHORT_TIME_FORMAT As String = "HH:mm"

        Private _timeLog As TimeLog
        Private _employee As Employee
        Private _dateOut, origDateIn, origDateOut As Date
        Private origTimeIn, origTimeOut As String
        Private _dateOutDisplay As Date?

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

                TimeIn = origTimeIn
                TimeOut = origTimeOut
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

        Public Property DateIn As Date
        Public Property TimeIn As String

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

        Public ReadOnly Property IsExisting As Boolean
            Get
                Return RowID > 0
            End Get
        End Property

        Public ReadOnly Property HasChanged As Boolean
            Get
                Dim differs = Not Equals(origDateIn, DateIn) _
                    Or Not Equals(origTimeIn, TimeIn) _
                    Or Not Equals(origDateOut, DateOut) _
                    Or Not Equals(origTimeOut, TimeOut)

                Return differs And IsValidToSave
            End Get
        End Property

        Public ReadOnly Property IsValidToSave As Boolean
            Get
                Dim hasShiftTime = Not String.IsNullOrWhiteSpace(TimeIn) _
                    Or Not String.IsNullOrWhiteSpace(TimeOut)

                Return hasShiftTime
            End Get
        End Property

        Public Sub Restore()
            DateIn = origDateIn
            TimeIn = origTimeIn

            DateOut = origDateOut
            TimeOut = origTimeOut
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

    End Sub

    Private Sub TimeLogsForm2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TimeAttendForm.listTimeAttendForm.Remove(Name)

    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
        Close()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

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

            CountHasChanged()

            grid.Refresh()
        End If
    End Sub

    Private Sub EmployeeTreeView1_Load(sender As Object, e As EventArgs) Handles EmployeeTreeView1.Load

    End Sub

    Private Sub EmployeeTreeView1_TickedEmployee(s As Object, e As EventArgs) Handles EmployeeTreeView1.TickedEmployee
        DateFilter_ValueChanged(dtpDateFrom, e)
    End Sub

    Private Sub grid_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellEndEdit
        Dim parseableIndexes = {TimeIn.Index, TimeOut.Index}

        If parseableIndexes.Any(Function(i) i = e.ColumnIndex) Then
            Dim currRow = grid.Rows(e.RowIndex)
            Dim model = GridRowToTimeLogModel(currRow)

            If e.ColumnIndex = TimeIn.Index Then
                model.TimeIn = TimeLogModel.TimeSpanToString(Calendar.ToTimespan(model.TimeIn))
            ElseIf e.ColumnIndex = TimeOut.Index Then
                model.TimeOut = TimeLogModel.TimeSpanToString(Calendar.ToTimespan(model.TimeOut))
            End If

            If model.HasChanged Then
                HasChangedRow(currRow)
            Else
                HasNoChangedRow(currRow)
            End If

            CountHasChanged()
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        dtpDateTo.Value = New Date(2018, 12, 31)
        dtpDateFrom.Value = New Date(2018, 12, 1)
    End Sub

#End Region

End Class
