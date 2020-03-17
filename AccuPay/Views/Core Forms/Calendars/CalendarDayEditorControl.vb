Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Repository

Public Class CalendarDayEditorControl

    Public Event Ok(payrate As CalendarDay)

    Private _calendarDay As CalendarDay

    Private _dayTypes As ICollection(Of DayType)

    Private _repository As DayTypeRepository

    Public Sub New()
        _repository = New DayTypeRepository()

        InitializeComponent()
    End Sub

    Private Sub CalendarDayEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDayTypes()
    End Sub

    Private Async Sub LoadDayTypes()
        _dayTypes = Await _repository.GetAll()
        DayTypesComboBox.DataSource = _dayTypes
    End Sub

    Public Sub ChangePayRate(calendarDay As CalendarDay)
        _calendarDay = calendarDay

        DayLabel.Text = _calendarDay.Date.Day.ToString()
        DescriptionTextBox.Text = _calendarDay.Description
        DayTypesComboBox.SelectedValue = _calendarDay.DayTypeID
    End Sub

    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        _calendarDay.Description = DescriptionTextBox.Text
        _calendarDay.DayTypeID = DirectCast(DayTypesComboBox.SelectedValue, Integer?)
        _calendarDay.DayType = DirectCast(DayTypesComboBox.SelectedItem, DayType)

        RaiseEvent Ok(_calendarDay)
        Hide()
    End Sub

End Class
