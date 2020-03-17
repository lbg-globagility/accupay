Option Strict On

Imports AccuPay.Entity

Public Class CalendarDay

    Public Property PayRate As PayRate

    Private Sub CalendarDay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If PayRate Is Nothing Then Return

        DayLabel.Text = PayRate.Date.Day.ToString()
        DescriptionLabel.Text = PayRate.Description.ToString()
    End Sub

End Class
