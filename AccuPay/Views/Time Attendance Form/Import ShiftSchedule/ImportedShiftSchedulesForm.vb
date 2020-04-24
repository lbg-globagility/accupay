Option Strict On

Imports AccuPay.Data
Imports AccuPay.Data.Repositories
Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports AccuPay.Tools
Imports AccuPay.Utilities
Imports AccuPay.Utils
Imports Globagility.AccuPay
Imports Globagility.AccuPay.ShiftSchedules
Imports Microsoft.EntityFrameworkCore

Public Class ImportedShiftSchedulesForm

#Region "VariableDeclarations"

    Private _dataSource As IList(Of ShiftScheduleModel)

    Private _dataSourceOk As IList(Of ShiftScheduleModel)

    Private _dataSourceFailed As IList(Of ShiftScheduleModel)

    Private _shiftScheduleRowRecords As IList(Of ShiftScheduleRowRecord)

    Private _employeeRepository As EmployeeRepository

    Private _employees As IList(Of IEmployee)

    Public IsSaved As Boolean

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _shiftScheduleRowRecords = New List(Of ShiftScheduleRowRecord)

        _dataSourceFailed = New List(Of ShiftScheduleModel)

        _employeeRepository = New EmployeeRepository

    End Sub

#End Region

#Region "Methods"

    Private Async Sub GetEmployeesAsync(listOfShiftScheduleRowRecord As IList(Of ShiftScheduleRowRecord))

        Dim employeeNumberList As String() = listOfShiftScheduleRowRecord.
                                                    Select(Function(s) s.EmployeeNo).
                                                    ToArray

        _employees = New List(Of IEmployee)((Await _employeeRepository.GetByMultipleEmployeeNumbersAsync(
                                                employeeNumberList,
                                                z_OrganizationID)))
        Using context = New PayrollContext

            For Each shiftSched In _shiftScheduleRowRecords

                Dim seek = _employees.Where(Function(ee) ee.EmployeeNo = shiftSched.EmployeeNo)

                Dim endDate = If(shiftSched.EndDate.HasValue, shiftSched.EndDate.Value, shiftSched.StartDate)
                Dim dates = Calendar.EachDay(shiftSched.StartDate, endDate)
                If seek.Any Then
                    Dim employee = seek.FirstOrDefault

                    AppendToDataSourceWithEmployee(shiftSched, dates, employee)
                Else
                    AppendToDataSourceWithNoEmployee(shiftSched, dates)
                End If

            Next

            Dim satisfy = Function(ssm As ShiftScheduleModel)
                              Return ssm.IsValidToSave And ssm.IsExistingEmployee
                          End Function

            _dataSourceOk = _dataSource.
                Where(satisfy).
                ToList()

            gridOK.DataSource = _dataSourceOk

            _dataSourceFailed = _dataSource.
                Where(Function(ssm) Not satisfy(ssm)).
                ToList()

            For Each ssm In _dataSourceFailed
                Dim reasons As New List(Of String)

                If Not ssm.IsValidToSave Then reasons.Add("no shift")

                If Not ssm.IsExistingEmployee Then reasons.Add("employee doesn't exists")

                ssm.Remarks = String.Join("; ", reasons.ToArray())
            Next
            gridFailed.DataSource = _dataSourceFailed

        End Using
    End Sub

    Private Sub AppendToDataSourceWithNoEmployee(shiftSched As ShiftScheduleRowRecord, dates As IEnumerable(Of Date))
        For Each d In dates
            _dataSource.Add(New ShiftScheduleModel() With {
                        .EmployeeNo = shiftSched.EmployeeNo,
                        .DateValue = d,
                        .BreakFrom = shiftSched.BreakStartTime,
                        .BreakLength = shiftSched.BreakLength,
                        .IsRestDay = shiftSched.IsRestDay,
                        .TimeFrom = shiftSched.StartTime,
                        .TimeTo = shiftSched.EndTime})

        Next
    End Sub

    Private Sub AppendToDataSourceWithEmployee(shiftSched As ShiftScheduleRowRecord,
                                               dates As IEnumerable(Of Date),
                                               employee As IEmployee)
        For Each d In dates
            _dataSource.Add(New ShiftScheduleModel(employee) With {
                        .DateValue = d,
                        .BreakFrom = shiftSched.BreakStartTime,
                        .BreakLength = shiftSched.BreakLength,
                        .IsRestDay = shiftSched.IsRestDay,
                        .TimeFrom = shiftSched.StartTime,
                        .TimeTo = shiftSched.EndTime})

        Next
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

    Private Sub ResetDataSource()
        _dataSource = New List(Of ShiftScheduleModel)

        GetEmployeesAsync(_shiftScheduleRowRecords)
    End Sub

#End Region

#Region "PrivateClasses"

    Private Class ShiftScheduleModel
        Private Const ONE_DAY_HOURS As Integer = 24
        Private Const MINUTES_PER_HOUR As Integer = 60
        Private origStartTime, origEndTime, origBreakStart As TimeSpan?
        Private origOffset As Boolean
        Private origBreakLength As Decimal
        Private _timeFrom, _timeTo, _breakFrom As TimeSpan?
        Private _isNew, _madeChanges, _isValid As Boolean
        Private _eds As EmployeeDutySchedule

        Public Sub New()

        End Sub

        Public Sub New(employee As IEmployee)
            AssignEmployee(employee)

        End Sub

        Public Sub New(ess As EmployeeDutySchedule)
            _eds = ess

            _RowID = ess.RowID
            AssignEmployee(ess.Employee)

            _DateValue = ess.DateSched
            _timeFrom = ess.StartTime
            _timeTo = ess.EndTime

            _BreakLength = ess.BreakLength

            Dim _hasBreakStart = ess.BreakStartTime.HasValue
            If _hasBreakStart Then
                _breakFrom = ess.BreakStartTime

            End If

            _IsRestDay = ess.IsRestDay

            origStartTime = _timeFrom
            origEndTime = _timeTo

            origBreakStart = _breakFrom
            origBreakLength = _BreakLength

            origOffset = _IsRestDay
        End Sub

        Private Sub AssignEmployee(employee As IEmployee)
            _EmployeeId = employee.RowID
            _EmployeeNo = employee.EmployeeNo
            _FullName = String.Join(", ", employee.LastName, employee.FirstName)

        End Sub

        Public Property RowID As Integer
        Public Property EmployeeId As Integer?
        Public Property EmployeeNo As String
        Public Property FullName As String
        Public Property DateValue As Date

        Public Property TimeFrom As TimeSpan?
            Get
                Return _timeFrom
            End Get
            Set(value As TimeSpan?)
                _timeFrom = value
            End Set
        End Property

        Public ReadOnly Property TimeFromDisplay As DateTime?
            Get
                Return TimeUtility.ToDateTime(_timeFrom)
            End Get
        End Property

        Public ReadOnly Property TimeToDisplay As DateTime?
            Get
                Return TimeUtility.ToDateTime(_timeTo)
            End Get
        End Property

        Public Property TimeTo As TimeSpan?
            Get
                Return _timeTo
            End Get
            Set(value As TimeSpan?)
                _timeTo = value
            End Set
        End Property

        Public ReadOnly Property BreakFromDisplay As DateTime?
            Get
                Return TimeUtility.ToDateTime(_breakFrom)
            End Get
        End Property

        Public Property BreakFrom As TimeSpan?
            Get
                Return _breakFrom
            End Get
            Set(value As TimeSpan?)
                _breakFrom = value
            End Set
        End Property

        Public Property BreakLength As Decimal

        Public Property IsRestDay As Boolean

        Public Property ShiftHours As Decimal
        Public Property WorkHours As Decimal

        Public Property Remarks As String

        Public Sub ComputeShiftHours()
            Dim shiftStart = _timeFrom
            Dim shiftEnd = _timeTo

            Dim isValidForCompute = shiftStart.HasValue And shiftStart.HasValue

            If isValidForCompute Then
                Dim sdfsd = shiftEnd - shiftStart
                If sdfsd.Value.Hours <= 0 Then sdfsd = sdfsd.Value.Add(New TimeSpan(ONE_DAY_HOURS, 0, 0))

                ShiftHours = Convert.ToDecimal(sdfsd.Value.TotalMinutes / MINUTES_PER_HOUR)
            Else
                ShiftHours = 0
            End If
        End Sub

        Public Sub ComputeWorkHours()
            WorkHours = ShiftHours - BreakLength
        End Sub

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
                Dim hasShiftTime = _timeFrom.HasValue _
                    And _timeTo.HasValue

                _isValid = hasShiftTime Or _IsRestDay

                Return _isValid
            End Get
        End Property

        Public ReadOnly Property IsExistingEmployee() As Boolean
            Get
                Return EmployeeId.HasValue
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
                    .StartTime = _timeFrom
                    .EndTime = _timeTo
                    .BreakStartTime = _breakFrom
                    .BreakLength = _BreakLength
                    .IsRestDay = _IsRestDay
                    .ShiftHours = ShiftHours
                    .WorkHours = WorkHours
                End With

                Return _eds
            End Get
        End Property

    End Class

#End Region

#Region "EventHandlers"

    Private Sub ImportedShiftSchedulesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.IsSaved = False

        btnSave.Enabled = False

        gridOK.AutoGenerateColumns = False
        gridFailed.AutoGenerateColumns = False

    End Sub

    Private Async Sub btnSave_ClickAsync(sender As Object, e As EventArgs) Handles btnSave.Click

        Using context = New PayrollContext

            Dim minDate = _dataSourceOk.Min(Function(ssm) ssm.DateValue)
            Dim maxDate = _dataSourceOk.Max(Function(ssm) ssm.DateValue)
            Dim employeeIDs = _dataSourceOk.Select(Function(ssm) ssm.EmployeeId).Distinct

            Dim eDutyScheds = Await context.EmployeeDutySchedules.
                Where(Function(eds) eds.DateSched >= minDate AndAlso eds.DateSched <= maxDate).
                Where(Function(eds) eds.OrganizationID.Value = z_OrganizationID).
                Where(Function(eds) employeeIDs.Contains(eds.EmployeeID)).
                ToListAsync()

            Dim newShiftScheduleList As New List(Of EmployeeDutySchedule)
            Dim existingShiftScheduleList As New List(Of EmployeeDutySchedule)

            For Each ssm In _dataSourceOk
                Dim seek = eDutyScheds.
                    Where(Function(eSched) eSched.EmployeeID.Value = ssm.EmployeeId.Value).
                    Where(Function(eSched) eSched.DateSched = ssm.DateValue)

                Dim eds = seek.FirstOrDefault
                ssm.ComputeShiftHours()
                ssm.ComputeWorkHours()

                If seek.Any Then
                    eds.StartTime = ssm.TimeFrom
                    eds.EndTime = ssm.TimeTo
                    eds.BreakStartTime = ssm.BreakFrom
                    eds.BreakLength = ssm.BreakLength

                    eds.ShiftHours = ssm.ShiftHours
                    eds.WorkHours = ssm.WorkHours

                    existingShiftScheduleList.Add(eds)
                Else

                    newShiftScheduleList.Add(ssm.ToEmployeeDutySchedule)
                    context.EmployeeDutySchedules.Add(ssm.ToEmployeeDutySchedule)
                End If

            Next

            Dim succeed As Boolean = False
            Try
                Dim i = Await context.SaveChangesAsync

                Dim importList = New List(Of Data.Entities.UserActivityItem)

                For Each schedule In newShiftScheduleList
                    importList.Add(New Data.Entities.UserActivityItem() With
                        {
                        .Description = $"Imported a new shift schedule.",
                        .EntityId = schedule.RowID
                        })
                Next
                For Each schedule In existingShiftScheduleList
                    importList.Add(New Data.Entities.UserActivityItem() With
                        {
                        .Description = $"Updated a shift schedule.",
                        .EntityId = schedule.RowID
                        })
                Next

                Dim repo = New UserActivityRepository
                repo.CreateRecord(z_User, "Shift Schedule", z_OrganizationID, UserActivityRepository.RecordTypeImport, importList)

                succeed = True
            Catch ex As Exception
                succeed = False
                Dim errMsg = String.Concat("Oops! something went wrong, please", Environment.NewLine, "contact ", My.Resources.AppCreator, " for assistance.")
                MessageBox.Show(errMsg, "Help", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                If succeed Then DialogResult = DialogResult.OK

                Me.IsSaved = True
            End Try

        End Using

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Private Sub gridOK_DataSourceChanged(sender As Object, e As EventArgs) Handles gridOK.DataSourceChanged
        Dim validCount = _dataSourceOk.Count

        tabPageOK.Text = String.Concat(tabPageOK.AccessibleDescription, " (", validCount, ")")

        btnSave.Enabled = validCount > 0

    End Sub

    Private Sub gridFailed_DataSourceChanged(sender As Object, e As EventArgs) Handles gridFailed.DataSourceChanged
        tabPageFailed.Text = String.Concat(tabPageFailed.AccessibleDescription, " (", _dataSourceFailed.Count, ")")

        UpdateStatusLabel(_dataSourceFailed.Count)
    End Sub

    Private Sub gridOK_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles gridOK.DataError

    End Sub

    Private Sub gridFailed_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles gridFailed.DataError

    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click

        Dim workSheetName = "ShiftSchedule"

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunctionAsync(
            Sub()
                Dim excelParserOutput = ExcelParser(Of ShiftScheduleRowRecord).Parse(workSheetName)

                If excelParserOutput.IsSuccess = False Then Return

                _shiftScheduleRowRecords = excelParserOutput.Records

                ResetDataSource()
            End Sub)

        If parsedSuccessfully = False Then Return

    End Sub

    Private Sub btnDownloadTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadTemplate.Click

        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.NewShift)

    End Sub

#End Region

End Class