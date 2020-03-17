Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Repository

Public Class CalendarsForm

    Private ReadOnly _repository As CalendarRepository

    Private _calendars As ICollection(Of PayCalendar)

    Private _payrates As ICollection(Of PayRate)

    Private _currentCalendar As PayCalendar

    Private _currentMonth As CalendarMonth

    Private WithEvents _editor As CalendarDayEditor

    Public Sub New()
        _repository = New CalendarRepository()
        _editor = New CalendarDayEditor()

        InitializeComponent()
        InitializeView()
    End Sub

    Public Sub InitializeView()
        ' Initialize CalendarsDataGridView
        CalendarsDataGridView.AutoGenerateColumns = False

        ' Initialize CalendarDayEditor
        _editor.Hide()
        Controls.Add(_editor)
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
        _editor.ChangePayRate(payrate)
        _editor.Top = p.Y
        _editor.Left = p.X
        _editor.BringToFront()
        _editor.Show()
    End Sub

    Private Sub Editor_Ok(payrate As PayRate) Handles _editor.Ok
        _currentMonth.RefreshData()
    End Sub

End Class
