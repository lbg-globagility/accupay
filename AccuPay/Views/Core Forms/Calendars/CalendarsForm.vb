Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Repository

Public Class CalendarsForm

    Private WithEvents Editor As CalendarDayEditorControl

    Private ReadOnly _repository As CalendarRepository

    Private _calendars As ICollection(Of PayCalendar)

    Private _calendarDays As ICollection(Of CalendarDay)

    Private _currentCalendar As PayCalendar

    Private _currentMonth As CalendarMonthControl

    Private ReadOnly _changedCalendarDays As ICollection(Of CalendarDay)

    Public Sub New()
        _repository = New CalendarRepository()
        Editor = New CalendarDayEditorControl()
        _changedCalendarDays = New Collection(Of CalendarDay)

        InitializeComponent()
        InitializeView()
    End Sub

    Public Sub InitializeView()
        ' Initialize CalendarsDataGridView
        CalendarsDataGridView.AutoGenerateColumns = False

        ' Initialize CalendarDayEditor
        Editor.Hide()
        Controls.Add(Editor)
    End Sub

    Private Sub CalendarsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCalendars()
    End Sub

    Private Async Sub CalendarsDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles CalendarsDataGridView.SelectionChanged
        _currentCalendar = DirectCast(CalendarsDataGridView.CurrentRow.DataBoundItem, PayCalendar)
        _changedCalendarDays.Clear()
        Await LoadCalendarDays()
        DisplayCalendarDays()
    End Sub

    Private Async Sub LoadCalendars()
        _calendars = Await _repository.GetAll()
        CalendarsDataGridView.DataSource = _calendars
    End Sub

    Private Async Function LoadCalendarDays() As Task
        _calendarDays = Await _repository.GetCalendarDays(_currentCalendar.RowID.Value, 2020)
    End Function

    Private Sub DisplayCalendarDays()
        If _calendarDays Is Nothing Then Return

        Dim payratesByMonths = _calendarDays.
            GroupBy(Function(p) p.Date.Month).
            Reverse()

        CalendarPanel.SuspendLayout()
        CalendarPanel.Controls.Clear()

        For Each payratesByMonth In payratesByMonths
            Dim calendarMonthComponent = New CalendarMonthControl()
            AddHandler calendarMonthComponent.DayClicked, AddressOf DaysTableLayout_Click

            calendarMonthComponent.Dock = DockStyle.Top
            calendarMonthComponent.CalendarDays = payratesByMonth.ToList()

            CalendarPanel.Controls.Add(calendarMonthComponent)
        Next

        CalendarPanel.ResumeLayout()
    End Sub

    Private Sub DaysTableLayout_Click(sender As CalendarMonthControl, calendarDay As CalendarDay)
        Dim p = PointToClient(MousePosition)

        _currentMonth = sender
        Editor.ChangePayRate(calendarDay)
        Editor.Top = p.Y
        Editor.Left = p.X
        Editor.BringToFront()
        Editor.Show()
    End Sub

    Private Sub Editor_Ok(calendarDay As CalendarDay) Handles Editor.Ok
        _changedCalendarDays.Add(calendarDay)
        _currentMonth.RefreshData()
    End Sub

    Private Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click
        Dim dialog = New NewCalendarDialog()
        dialog.ShowDialog()
    End Sub

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        Using context = New PayrollContext()
            For Each calendarDay In _changedCalendarDays
                context.Entry(calendarDay).State = Microsoft.EntityFrameworkCore.EntityState.Modified
            Next

            Await context.SaveChangesAsync()
            _changedCalendarDays.Clear()
        End Using
    End Sub

    Private Async Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click
        _changedCalendarDays.Clear()
        Await LoadCalendarDays()
        DisplayCalendarDays()
    End Sub

End Class
