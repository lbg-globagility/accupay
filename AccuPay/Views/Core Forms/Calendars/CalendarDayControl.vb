Option Strict On

Imports System.Globalization
Imports AccuPay.Entity

Public Class CalendarDayControl

    Public Shadows Event Click(sender As CalendarDayControl)

    Private ReadOnly RegularDayColor As Color = Color.FromArgb(0)

    Private ReadOnly HolidayColor As Color = Color.FromArgb(1, 255, 0, 0)

    Public Property CalendarDay As CalendarDay

    Public Sub RefreshData()
        RefreshContent()
        RefreshColor()
    End Sub

    Private Sub RefreshContent()
        DayLabel.Text = CalendarDay.Date.ToString("%d", CultureInfo.InvariantCulture)
        DescriptionLabel.Text = CalendarDay.Description?.ToString()
    End Sub

    Private Sub RefreshColor()
        Dim color As Color
        If CalendarDay.IsRegularDay Then
            color = RegularDayColor
        ElseIf CalendarDay.IsHoliday Then
            color = HolidayColor
        End If

        DayLabel.ForeColor = color
        DescriptionLabel.ForeColor = color
    End Sub

    Private Sub CalendarDay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If CalendarDay Is Nothing Then Return
        RefreshData()
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs) Handles MyBase.Click,
                                                                       DescriptionLabel.Click,
                                                                       DayLabel.Click
        RaiseEvent Click(Me)
    End Sub

End Class
