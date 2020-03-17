Option Strict On

Imports System.Collections.ObjectModel
Imports AccuPay.Entity

Public Class CalendarMonthControl

    Public Property Payrates As ICollection(Of PayRate)

    Public Event DayClicked(sender As CalendarMonthControl, payrate As PayRate)

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
            weekdayHeader.Text = dayName

            DaysTableLayout.Controls.Add(weekdayHeader)
        Next

        Dim firstDay = Payrates.First()
        Dim monthOffset = CInt(firstDay.Date.DayOfWeek)

        MonthLabel.Text = firstDay.Date.ToString("MMM")

        For Each payrate In Payrates
            Dim column = CInt(payrate.Date.DayOfWeek)
            Dim row = CInt(Math.Ceiling((payrate.Date.Day + monthOffset) / DaysPerWeek))

            Dim dayControl = New CalendarDayControl()
            dayControl.PayRate = payrate
            dayControl.Dock = DockStyle.Fill

            AddHandler dayControl.Click, AddressOf DaysTableLayout_Click

            DaysTableLayout.Controls.Add(dayControl, column, row)
        Next

        For Each rowStyle As RowStyle In DaysTableLayout.RowStyles
            rowStyle.SizeType = SizeType.Absolute
            rowStyle.Height = 50
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

    Private Sub DaysTableLayout_Click(sender As Object, e As EventArgs)
        If Not TypeOf sender Is CalendarDayControl Then Return

        Dim control = DirectCast(sender, CalendarDayControl)
        Dim payrate = control.PayRate

        RaiseEvent DayClicked(Me, payrate)
    End Sub

End Class
