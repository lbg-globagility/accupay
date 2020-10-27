Option Strict On

Imports System.Collections.ObjectModel
Imports AccuPay.Data.Entities

Public Class CalendarMonthControl

    Public Property CalendarDays As ICollection(Of CalendarDay)

    Public Event DayClick(sender As CalendarMonthControl, calendarDay As CalendarDay)

    Private Const DaysPerWeek As Integer = 7

    ' The maximum number of rows that could appear on any calendar month
    Private Const MaximumRows As Integer = 6

    ' The maximum number of 'cells' that could appear on any calendar month
    Private Const MaximumCells As Integer = DaysPerWeek * MaximumRows

    Private ReadOnly WeekDays As ICollection(Of String) = New Collection(Of String) From {
        "Sunday",
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday"
    }

    Public Sub RefreshControl()
        Dim firstDay = CalendarDays.First()
        Dim lastDay = CalendarDays.Last()

        Dim offsetOfFirstDay = firstDay.Date.DayOfWeek

        MonthLabel.Text = firstDay.Date.ToString("MMMM yyyy")

        Dim firstDayIndex = firstDay.Date.Day + offsetOfFirstDay - 1
        Dim lastDayIndex = lastDay.Date.Day + offsetOfFirstDay - 1

        For Each control As Control In DaysTableLayout.Controls
            Dim position = DaysTableLayout.GetPositionFromControl(control)
            ' If row is 0, then that means the headers. Nothing is needed with the headers so skip the control.
            If position.Row = 0 Then Continue For

            Dim dayControl = DirectCast(control, CalendarDayControl)
            Dim controlIndex = ((position.Row - 1) * DaysPerWeek) + (position.Column)

            If firstDayIndex <= controlIndex AndAlso controlIndex <= lastDayIndex Then
                Dim calendarDay = CalendarDays.ElementAt(controlIndex - offsetOfFirstDay)
                dayControl.CalendarDay = calendarDay
            Else
                dayControl.CalendarDay = Nothing
            End If

            dayControl.RefreshControl()
        Next
    End Sub

    Public Sub RefreshData()
        For Each control In DaysTableLayout.Controls
            If TypeOf control Is CalendarDayControl Then
                Dim dayControl = DirectCast(control, CalendarDayControl)
                dayControl.RefreshControl()
            End If
        Next
    End Sub

    Private Sub CalendarMonth_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DaysTableLayout.SuspendLayout()
        DaysTableLayout.Controls.Clear()

        ' Render the week day headers on the table
        For Each dayName In WeekDays
            Dim weekdayHeader = New Label()
            weekdayHeader.Font = New Font("Segoe UI", 9.75)
            weekdayHeader.Text = dayName

            DaysTableLayout.Controls.Add(weekdayHeader)
        Next

        ' Render the cells to contain the days on the table. Make sure that there are enough cells to cover all
        ' possible days.
        For Each calendarDay In Enumerable.Range(1, MaximumCells)
            Dim dayControl = New CalendarDayControl()
            dayControl.Dock = DockStyle.Fill

            AddHandler dayControl.Click, AddressOf DayControl_Click

            DaysTableLayout.Controls.Add(dayControl)
        Next

        ' Set the table rows' height.
        Dim i = 0
        For Each rowStyle As RowStyle In DaysTableLayout.RowStyles
            Dim rowHeight = If(i = 0, 24, 48)

            rowStyle.SizeType = SizeType.Absolute
            rowStyle.Height = rowHeight
            i += 1
        Next

        DaysTableLayout.ResumeLayout()
    End Sub

    Private Sub DayControl_Click(sender As CalendarDayControl)
        Dim calendarDay = sender.CalendarDay

        If calendarDay IsNot Nothing Then
            RaiseEvent DayClick(Me, calendarDay)
        End If
    End Sub

End Class