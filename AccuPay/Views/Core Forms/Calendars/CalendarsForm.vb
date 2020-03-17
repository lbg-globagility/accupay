Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Repository

Public Class CalendarsForm

    Private Const DaysInWeek As Integer = 7

    Private ReadOnly _repository As CalendarRepository

    Private _calendars As ICollection(Of PayCalendar)

    Private _payrates As ICollection(Of PayRate)

    Private _currentCalendar As PayCalendar

    Public Sub New()
        InitializeComponent()
        InitializeView()

        _repository = New CalendarRepository()
    End Sub

    Public Sub InitializeView()
        CalendarsDataGridView.AutoGenerateColumns = False
    End Sub

    Private Sub CalendarsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCalendars()
    End Sub

    Private Async Sub CalendarsDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles CalendarsDataGridView.SelectionChanged
        _currentCalendar = CType(CalendarsDataGridView.CurrentRow.DataBoundItem, PayCalendar)
        Await LoadPayrates()
        DisplayRates()
    End Sub

    Private Async Sub LoadCalendars()
        _calendars = Await _repository.GetAll()
        CalendarsDataGridView.DataSource = _calendars
    End Sub

    Private Async Function LoadPayrates() As Task
        _payrates = Await _repository.GetPayRates(_currentCalendar.RowID.Value, New Date(2020, 1, 1), New Date(2020, 1, 31))
    End Function

    Private Sub DisplayRates()
        If _payrates Is Nothing Then Return

        DaysTableLayout.SuspendLayout()
        DaysTableLayout.Controls.Clear()

        Dim firstDay = _payrates.First()
        Dim monthOffset = CInt(firstDay.Date.DayOfWeek)

        For Each payrate In _payrates
            Dim column = CInt(payrate.Date.DayOfWeek)
            Dim row = CInt(Math.Ceiling((payrate.Date.Day + monthOffset) / DaysInWeek) - 1)

            Dim dayControl = New CalendarDay()
            dayControl.PayRate = payrate
            dayControl.Dock = DockStyle.Fill

            DaysTableLayout.Controls.Add(dayControl, column, row)

            'Dim label = New Label()
            'label.Text = payrate.Date.Day.ToString()

            'DaysTableLayout.Controls.Add(Label, column, row)
        Next

        DaysTableLayout.ResumeLayout()
    End Sub

    Private Sub DaysTableLayout_Click(sender As Object, e As EventArgs) Handles DaysTableLayout.Click

    End Sub

End Class
