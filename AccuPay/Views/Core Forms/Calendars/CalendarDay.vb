Option Strict On

Imports AccuPay.Entity

Public Class CalendarDay

    Public Property PayRate As PayRate

    Public Sub RefreshData()
        DayLabel.Text = PayRate.Date.Day.ToString()
        DescriptionLabel.Text = PayRate.Description.ToString()
    End Sub

    Private Sub CalendarDay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If PayRate Is Nothing Then Return
        RefreshData()
    End Sub

End Class
