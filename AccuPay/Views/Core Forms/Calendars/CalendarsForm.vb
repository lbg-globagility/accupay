Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Repository

Public Class CalendarsForm

    Private ReadOnly _repository As CalendarRepository

    Private _calendars As ICollection(Of PayCalendar)

    Private _payrates As ICollection(Of PayRate)

    Private _currentCalendar As PayCalendar

    Private _currentMonth As CalendarMonth

    Private WithEvents Editor As CalendarDayEditor

    Private ReadOnly _changedPayrates As ICollection(Of PayRate)

    Public Sub New()
        _repository = New CalendarRepository()
        Editor = New CalendarDayEditor()
        _changedPayrates = New Collection(Of PayRate)

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
        Await LoadPayrates()
        DisplayRates()
    End Sub

    Private Async Sub LoadCalendars()
        _calendars = Await _repository.GetAll()
        CalendarsDataGridView.DataSource = _calendars
    End Sub

    Private Async Function LoadPayrates() As Task
        _payrates = Await _repository.GetPayRates(_currentCalendar.RowID.Value, 2020)
    End Function

    Private Sub DisplayRates()
        If _payrates Is Nothing Then Return

        Dim payratesByMonths = _payrates.
            GroupBy(Function(p) p.Date.Month).
            Reverse()

        CalendarPanel.SuspendLayout()
        CalendarPanel.Controls.Clear()

        For Each payratesByMonth In payratesByMonths
            Dim calendarMonthComponent = New CalendarMonth()
            AddHandler calendarMonthComponent.DayClicked, AddressOf DaysTableLayout_Click

            calendarMonthComponent.Dock = DockStyle.Top
            calendarMonthComponent.Payrates = payratesByMonth.ToList()

            CalendarPanel.Controls.Add(calendarMonthComponent)
        Next

        CalendarPanel.ResumeLayout()
    End Sub

    Private Sub DaysTableLayout_Click(sender As CalendarMonth, payrate As PayRate)
        Dim p = PointToClient(MousePosition)

        _currentMonth = sender
        Editor.ChangePayRate(payrate)
        Editor.Top = p.Y
        Editor.Left = p.X
        Editor.BringToFront()
        Editor.Show()
    End Sub

    Private Sub Editor_Ok(payrate As PayRate) Handles Editor.Ok
        _changedPayrates.Add(payrate)
        _currentMonth.RefreshData()
    End Sub

    Private Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click
        Dim dialog = New NewCalendarDialog()
        dialog.ShowDialog()
    End Sub

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        Using context = New PayrollContext()
            For Each payrate In _changedPayrates
                context.Entry(payrate).State = Microsoft.EntityFrameworkCore.EntityState.Modified
            Next

            Await context.SaveChangesAsync()
            _changedPayrates.Clear()
        End Using
    End Sub

    Private Async Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click
        _changedPayrates.Clear()
        Await LoadPayrates()
        DisplayRates()
    End Sub

End Class
