Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class NewCalendarDialog

    Private ReadOnly _repository As CalendarRepository

    Public Sub New()
        _repository = MainServiceProvider.GetRequiredService(Of CalendarRepository)

        InitializeComponent()
    End Sub

    Private Async Sub NewCalendarDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim calendars = Await _repository.GetAllAsync()
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

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim calendar = New PayCalendar()
                calendar.Name = NameTextbox.Text

                Dim copiedCalendar = DirectCast(CopyCalendarComboBox.SelectedItem, PayCalendar)

                Dim dataService = MainServiceProvider.GetRequiredService(Of CalendarDataService)
                Await dataService.CreateAsync(calendar, copiedCalendar)

                DialogResult = DialogResult.OK
            End Function)
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        DialogResult = DialogResult.Cancel
    End Sub

End Class