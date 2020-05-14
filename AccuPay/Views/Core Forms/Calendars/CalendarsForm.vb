Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class CalendarsForm

    Private Const WM_PARENTNOTIFY As Integer = &H210

    Private Const WM_LBUTTONDOWN As Integer = &H201

    Private Const MonthsPerYear As Integer = 12

    Private WithEvents Editor As CalendarDayEditorControl

    Private ReadOnly _repository As CalendarRepository

    Private _calendars As ICollection(Of PayCalendar)

    Private _calendarDays As ICollection(Of CalendarDay)

    Private _currentCalendar As PayCalendar

    Private _currentYear As Integer

    Private _currentMonth As Integer

    Private _currentMonthControl As CalendarMonthControl

    Private ReadOnly _changeTracker As ICollection(Of CalendarDay)

    Public Sub New()
        Editor = New CalendarDayEditorControl()
        _repository = MainServiceProvider.GetRequiredService(Of CalendarRepository)
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

    Private Sub CalendarsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        Await LoadCalendarDays()
    End Sub

    Private Async Sub LoadCalendars()
        _calendars = Await _repository.GetAllAsync()
        CalendarsDataGridView.DataSource = _calendars
    End Sub

    Private Async Function LoadCalendarDays() As Task
        _calendarDays = Await _repository.GetCalendarDays(_currentCalendar.RowID.Value, _currentYear)
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
        dialog.ShowDialog()
    End Sub

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click

        Await _repository.UpdateManyAsync(_changeTracker)

        ClearChangeTracker()

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
        GeneralForm.listGeneralForm.Remove(Me.Name)
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

End Class