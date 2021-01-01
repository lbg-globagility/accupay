Option Strict On

Imports System.Globalization
Imports AccuPay.Core.Entities

Public Class CalendarDayControl

    Public Shadows Event Click(sender As CalendarDayControl)

    Private ReadOnly RegularDayColor As Color = Color.FromArgb(0)

    Private ReadOnly HolidayColor As Color = Color.FromArgb(1, 255, 0, 0)

    Public Property CalendarDay As CalendarDay

    Public Sub RefreshControl()
        If CalendarDay Is Nothing Then
            DayLabel.Text = String.Empty
            DescriptionLabel.Text = String.Empty
        Else
            RefreshContent()
            RefreshColor()
        End If
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
        RefreshControl()
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs) Handles MyBase.Click,
                                                                       DescriptionLabel.Click,
                                                                       DayLabel.Click
        RaiseEvent Click(Me)
    End Sub

End Class