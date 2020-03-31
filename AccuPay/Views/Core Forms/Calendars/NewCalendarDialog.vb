Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.Utils

Public Class NewCalendarDialog

    Private ReadOnly _repository As CalendarRepository

    Public Sub New()
        _repository = New CalendarRepository()

        InitializeComponent()
    End Sub

    Private Async Sub NewCalendarDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim calendars = Await _repository.GetAll()
        CopyCalendarComboBox.DataSource = calendars
    End Sub

    Private Async Sub CreateButton_Click(sender As Object, e As EventArgs) Handles CreateButton.Click
        Dim errorMessage As String = Nothing

        If String.IsNullOrWhiteSpace(NameTextbox.Text) Then
            errorMessage = "A `Name` is required"
        ElseIf CopyCalendarComboBox.SelectedItem Is Nothing Then
            errorMessage = "A `Calendar` is required"
        End If

        Dim messageTitle = "New Calendar"

        If Not String.IsNullOrWhiteSpace(errorMessage) Then
            If MessageBoxHelper.Confirm(Of DialogResult)(
                    errorMessage,
                    messageTitle,
                    messageBoxButton:=MessageBoxButtons.OK,
                    messageBoxIcon:=MessageBoxIcon.Warning) = DialogResult.OK Then
                Return
            End If
        End If

        Dim calendar = New PayCalendar()
        calendar.Name = NameTextbox.Text

        Dim copiedCalendar = DirectCast(CopyCalendarComboBox.SelectedItem, PayCalendar)

        Await _repository.Create(calendar, copiedCalendar)

        DialogResult = DialogResult.OK
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        DialogResult = DialogResult.Cancel
    End Sub

End Class
