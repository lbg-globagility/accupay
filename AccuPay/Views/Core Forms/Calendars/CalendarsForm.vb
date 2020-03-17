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

    Private WithEvents _editor As CalendarDayEditor

    Public Sub New()
        _repository = New CalendarRepository()
        _editor = New CalendarDayEditor()

        InitializeComponent()
        InitializeView()
    End Sub

    Public Sub InitializeView()
        CalendarsDataGridView.AutoGenerateColumns = False
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

            AddHandler dayControl.Click, AddressOf DaysTableLayout_Click
            dayControl.PayRate = payrate
            dayControl.Dock = DockStyle.Fill

            DaysTableLayout.Controls.Add(dayControl, column, row)
        Next

        DaysTableLayout.ResumeLayout()
    End Sub

    Private Sub DaysTableLayout_Click(sender As Object, e As EventArgs) Handles DaysTableLayout.Click
        If Not TypeOf sender Is CalendarDay Then Return

        Dim control = DirectCast(sender, CalendarDay)
        Dim payrate = control.PayRate

        Dim p = PointToClient(MousePosition)

        _editor.ChangePayRate(control.PayRate)
        _editor.Top = p.Y
        _editor.Left = p.X
        _editor.BringToFront()
        _editor.Show()
    End Sub

    Private Sub Editor_Ok(payrate As PayRate) Handles _editor.Ok
        For Each control In DaysTableLayout.Controls
            Dim calendarDayControl = DirectCast(control, CalendarDay)

            If payrate.RowID = calendarDayControl.PayRate.RowID Then
                calendarDayControl.RenderText()
            End If
        Next

        Debug.WriteLine("Ok")
    End Sub

End Class
