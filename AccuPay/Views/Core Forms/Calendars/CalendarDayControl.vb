Option Strict On

Imports AccuPay.Entity

Public Class CalendarDayControl

    Public Property CalendarDay As CalendarDay

    Public Sub RefreshData()
        DayLabel.Text = CalendarDay.Date.Day.ToString()
        DescriptionLabel.Text = CalendarDay.Description?.ToString()
    End Sub

    Private Sub CalendarDay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If CalendarDay Is Nothing Then Return
        RefreshData()
    End Sub

End Class
