Option Strict On

Imports System.Globalization
Imports AccuPay.Data.Entities

Public Class CalendarDayEditorControl

    ''' <summary>
    ''' Occurs when the OK button is clicked.
    ''' </summary>
    ''' <param name="payrate"></param>
    Public Event Ok(payrate As CalendarDay)

    Private _calendarDay As CalendarDay

    Private _dayTypes As ICollection(Of DayType)

    ''' <summary>
    ''' The day types that are available for selection
    ''' </summary>
    Public WriteOnly Property DayTypes As ICollection(Of DayType)
        Set(value As ICollection(Of DayType))
            _dayTypes = value
            DayTypesComboBox.DataSource = _dayTypes
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub ChangeCalendarDay(calendarDay As CalendarDay)
        _calendarDay = calendarDay

        DayLabel.Text = _calendarDay.Date.ToString("MMM d", CultureInfo.InvariantCulture)
        DescriptionTextBox.Text = _calendarDay.Description

        ' If the calendar day has no day type select the first day type as the default
        If _calendarDay.DayTypeID Is Nothing Then
            DayTypesComboBox.SelectedIndex = 0
        Else
            DayTypesComboBox.SelectedValue = _calendarDay.DayTypeID
        End If
    End Sub

    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        _calendarDay.Description = DescriptionTextBox.Text
        _calendarDay.DayTypeID = DirectCast(DayTypesComboBox.SelectedValue, Integer?)
        _calendarDay.DayType = DirectCast(DayTypesComboBox.SelectedItem, DayType)

        RaiseEvent Ok(_calendarDay)
        Hide()
    End Sub

End Class