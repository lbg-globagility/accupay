Option Strict On

Public Class CalendarMonthSelectorControl

    Public Event NameChanged(name As String)

    Public Property CalendarName As String
        Get
            Return NameTextBox.Text
        End Get
        Set(value As String)
            NameTextBox.Text = value
        End Set
    End Property

    Private Sub NameTextBox_TextChanged(sender As Object, e As EventArgs) Handles NameTextBox.TextChanged
        RaiseEvent NameChanged(NameTextBox.Text)
    End Sub

End Class
