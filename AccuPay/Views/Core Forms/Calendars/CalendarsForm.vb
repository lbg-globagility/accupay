Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class CalendarsForm

    Private Const WM_PARENTNOTIFY As Integer = &H210

    Private Const WM_LBUTTONDOWN As Integer = &H201

    Private Const MonthsPerYear As Integer = 12

    Private WithEvents Editor As CalendarDayEditorControl

    Private _calendars As ICollection(Of PayCalendar)

    Private _calendarDays As ICollection(Of CalendarDay)

    Private _currentCalendar As PayCalendar

    Private _currentYear As Integer

    Private _currentMonth As Integer

    Private _currentMonthControl As CalendarMonthControl

    Private ReadOnly _changeTracker As ICollection(Of CalendarDay)

    Private _nameHasChanged As Boolean
    Private _currentPermission As RolePermission

    Public Sub New()
        Editor = New CalendarDayEditorControl()
        _changeTracker = New Collection(Of CalendarDay)

        InitializeComponent()
        InitializeView()
    End Sub

    Private Sub InitializeView()
        ' Initialize CalendarsDataGridView
        CalendarsDataGridView.AutoGenerateColumns = False

        ' Initialize CalendarDayEditor
        Editor.Hide()
        Controls.Add(Editor)

        InitializeMonthControls()
    End Sub

    Private Sub InitializeMonthControls()
        CalendarPanel.SuspendLayout()

        For Each i In Enumerable.Range(0, MonthsPerYear)
            Dim monthControl = New CalendarMonthControl()
            monthControl.Dock = DockStyle.Top
            AddHandler monthControl.DayClick, AddressOf MonthControl_DayClick

            CalendarPanel.Controls.Add(monthControl)
        Next

        CalendarPanel.ResumeLayout()
    End Sub

    Private Async Sub CalendarsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.CALENDAR)

        NewToolStripButton.Visible = False
        SaveToolStripButton.Visible = False
        CancelToolStripButton.Visible = False
        DeleteToolStripButton.Visible = False

        If role.Success Then

            _currentPermission = role.RolePermission

            If _currentPermission.Create Then
                NewToolStripButton.Visible = True

            End If

            If _currentPermission.Update Then
                SaveToolStripButton.Visible = True
                CancelToolStripButton.Visible = True
            End If

            If _currentPermission.Delete Then
                DeleteToolStripButton.Visible = True

            End If

        End If

        _currentYear = Date.Today.Year
        MonthSelectorControl.Year = _currentYear
        LoadCalendars()
    End Sub

    Private Async Sub CalendarsDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles CalendarsDataGridView.SelectionChanged
        Dim selectedCalendar = DirectCast(CalendarsDataGridView.CurrentRow.DataBoundItem, PayCalendar)

        If _currentCalendar?.RowID = selectedCalendar?.RowID Then
            Return
        End If

        _currentCalendar = selectedCalendar
        ClearChangeTracker()
        CalendarLabel.Text = _currentCalendar.Name
        MonthSelectorControl.CalendarName = _currentCalendar.Name
        Await LoadCalendarDays()
    End Sub

    Private Async Sub LoadCalendars()
        Dim repository = MainServiceProvider.GetRequiredService(Of CalendarRepository)
        _calendars = Await repository.GetAllAsync()
        CalendarsDataGridView.DataSource = _calendars
    End Sub

    Private Async Function LoadCalendarDays() As Task
        Dim repository = MainServiceProvider.GetRequiredService(Of CalendarRepository)
        _calendarDays = Await repository.GetCalendarDays(_currentCalendar.RowID.Value, _currentYear)
        DisplayCalendarDays()
    End Function

    Private Sub DisplayCalendarDays()
        If _calendarDays Is Nothing Then Return

        Dim payratesByMonths = _calendarDays.
            GroupBy(Function(p) p.Date.Month)

        For Each payratesByMonth In payratesByMonths
            Dim index = MonthsPerYear - payratesByMonth.Key

            Dim control = DirectCast(CalendarPanel.Controls(index), CalendarMonthControl)
            control.CalendarDays = payratesByMonth.ToList()
            control.RefreshControl()
        Next
    End Sub

    Private Sub MonthControl_DayClick(sender As CalendarMonthControl, calendarDay As CalendarDay)

        If _currentPermission.Update = False Then Return

        Dim p = PointToClient(MousePosition)

        _currentMonthControl = sender
        Editor.ChangeCalendarDay(calendarDay)
        Editor.Top = p.Y
        Editor.Left = p.X
        Editor.BringToFront()
        Editor.Show()
    End Sub

    Private Sub Editor_Ok(calendarDay As CalendarDay) Handles Editor.Ok
        AddToChangeTracker(calendarDay)
        _currentMonthControl.RefreshData()
    End Sub

    Private Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click

        Dim dialog = New NewCalendarDialog()
        If dialog.ShowDialog() = DialogResult.OK Then
            LoadCalendars()

        End If
    End Sub

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        Await FunctionUtils.TryCatchFunctionAsync("Save Changes",
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of CalendarDataService)

                Await dataService.UpdateAsync(_currentCalendar)
                Await dataService.UpdateDaysAsync(Enumerable.Empty(Of CalendarDay).ToList(), _changeTracker)

                ClearChangeTracker()

                ShowBalloonInfo("Changes have been saved.", "Changes Saved")
            End Function)
    End Sub

    Private Async Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click
        ClearChangeTracker()
        Await LoadCalendarDays()
    End Sub

    Private Sub DayTypesToolStripButton_Click(sender As Object, e As EventArgs) Handles DayTypesToolStripButton.Click
        Dim dialog = New DayTypesDialog()
        dialog.ShowDialog()
    End Sub

    Private Async Sub MonthSelectorControl_MonthChanged(year As Integer, month As Integer) Handles MonthSelectorControl.MonthChanged
        _currentYear = year
        Await LoadCalendarDays()
    End Sub

    Private Sub MonthSelectorControl_NameChanged(name As String) Handles MonthSelectorControl.NameChanged
        _currentCalendar.Name = name
        CancelToolStripButton.Enabled = True
        SaveToolStripButton.Enabled = True
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        ' Capture all mouse left button clicks
        If (m.Msg = WM_LBUTTONDOWN Or (m.Msg = WM_PARENTNOTIFY AndAlso m.WParam.ToInt32() = WM_LBUTTONDOWN)) Then

            ' When the Editor is visible, and the mouse was clicked outside the control, then hide the editor.
            If Editor.Visible Then
                Dim pointer = Editor.PointToClient(Cursor.Position)
                Dim isHit = Editor.ClientRectangle.Contains(pointer)

                If Not isHit Then
                    Editor.Hide()
                End If
            End If

        End If

        MyBase.WndProc(m)
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Sub AddToChangeTracker(calendarDay As CalendarDay)
        _changeTracker.Add(calendarDay)
        CancelToolStripButton.Enabled = True
        SaveToolStripButton.Enabled = True
    End Sub

    Private Sub ClearChangeTracker()
        _changeTracker.Clear()
        CancelToolStripButton.Enabled = False
        SaveToolStripButton.Enabled = False
    End Sub

    Private Async Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete the calendar `{_currentCalendar.Name}`?", "Confirm Deletion") = False Then

            Return
        End If

        Dim dataService = MainServiceProvider.GetRequiredService(Of CalendarDataService)

        Await FunctionUtils.TryCatchFunctionAsync("Delete Calendar",
        Async Function()
            Await dataService.DeleteAsync(_currentCalendar)
            LoadCalendars()
            ShowBalloonInfo("Calendar has been deleted.", "Calendar Deleted")
        End Function)
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, MonthSelectorControl, 0, -100)
    End Sub

    Private Sub EmployeeLeavesForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        myBalloon(, , MonthSelectorControl, , , 1)

        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

End Class