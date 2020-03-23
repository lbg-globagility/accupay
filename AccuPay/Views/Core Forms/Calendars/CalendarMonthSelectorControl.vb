Option Strict On

Public Class CalendarMonthSelectorControl

    Private Const MinYear As Integer = 1000
    Private Const MaxYear As Integer = 9999

    Public Event MonthChanged(year As Integer, month As Integer)

    Private _currentYear As Integer

    Private _currentMonth As Integer

    Private Sub YearTextBox_TextChanged(sender As Object, e As EventArgs) Handles YearTextBox.TextChanged
        Dim year = 0
        Dim successful = Integer.TryParse(YearTextBox.Text, year)

        If Not successful Then Return
        If Not IsValidYear(year) Then Return
        If Not YearHasChanged(year) Then Return

        _currentYear = year

        RaiseEvent MonthChanged(_currentYear, _currentMonth)
    End Sub

    Private Function IsValidYear(year As Integer) As Boolean
        Return MinYear <= year AndAlso year <= MaxYear
    End Function

    Private Function YearHasChanged(year As Integer) As Boolean
        Return _currentYear <> year
    End Function

End Class
