Option Strict On

Imports System.Globalization
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories

Public Class CalendarDayEditorControl

    ''' <summary>
    ''' Occurs when the OK button is clicked.
    ''' </summary>
    ''' <param name="payrate"></param>
    Public Event Ok(payrate As CalendarDay)

    Private _calendarDay As CalendarDay

    Private _dayTypes As ICollection(Of DayType)

    Private ReadOnly _repository As DayTypeRepository

    Public Sub New(repository As DayTypeRepository)
        _repository = repository
        InitializeComponent()
    End Sub

    Private Sub CalendarDayEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDayTypes()
    End Sub

    Private Async Sub LoadDayTypes()
        _dayTypes = Await _repository.GetAllAsync()
        DayTypesComboBox.DataSource = _dayTypes
    End Sub

    Public Sub ChangeCalendarDay(calendarDay As CalendarDay)
        _calendarDay = calendarDay

        DayLabel.Text = _calendarDay.Date.ToString("MMM d", CultureInfo.InvariantCulture)
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