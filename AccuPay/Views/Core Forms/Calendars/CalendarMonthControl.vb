Option Strict On

Imports System.Collections.ObjectModel
Imports AccuPay.Entity

Public Class CalendarMonthControl

    Public Property CalendarDays As ICollection(Of CalendarDay)

    Public Event DayClicked(sender As CalendarMonthControl, calendarDay As CalendarDay)

    Private Const DaysPerWeek As Integer = 7

    Private ReadOnly WeekDays As ICollection(Of String) = New Collection(Of String) From {
        "Sunday",
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday"
    }

    Private Sub CalendarMonth_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DaysTableLayout.SuspendLayout()
        DaysTableLayout.Controls.Clear()

        For Each dayName In WeekDays
            Dim weekdayHeader = New Label()
            weekdayHeader.Font = New Font("Segoe UI", 9.75)
            weekdayHeader.Text = dayName

            DaysTableLayout.Controls.Add(weekdayHeader)
        Next

        Dim firstDay = CalendarDays.First()
        Dim monthOffset = CInt(firstDay.Date.DayOfWeek)

        MonthLabel.Text = firstDay.Date.ToString("MMMM yyyy")

        For Each calendarDay In CalendarDays
            Dim column = CInt(calendarDay.Date.DayOfWeek)
            Dim row = CInt(Math.Ceiling((calendarDay.Date.Day + monthOffset) / DaysPerWeek))

            Dim dayControl = New CalendarDayControl()
            dayControl.CalendarDay = calendarDay
            dayControl.Dock = DockStyle.Fill

            AddHandler dayControl.Click, AddressOf DayControl_Click

            DaysTableLayout.Controls.Add(dayControl, column, row)
        Next

        Dim i = 0
        For Each rowStyle As RowStyle In DaysTableLayout.RowStyles
            Dim rowHeight = If(i = 0, 24, 48)

            rowStyle.SizeType = SizeType.Absolute
            rowStyle.Height = rowHeight
            i += 1
        Next

        DaysTableLayout.ResumeLayout()
    End Sub

    Public Sub RefreshData()
        For Each control In DaysTableLayout.Controls
            If TypeOf control Is CalendarDayControl Then
                Dim month = DirectCast(control, CalendarDayControl)
                month.RefreshData()
            End If
        Next
    End Sub

    Private Sub DayControl_Click(sender As CalendarDayControl)
        Dim calendarDay = sender.CalendarDay

        RaiseEvent DayClicked(Me, calendarDay)
    End Sub

End Class
