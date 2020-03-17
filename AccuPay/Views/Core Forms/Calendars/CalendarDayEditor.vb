Option Strict On

Imports System.Collections.ObjectModel
Imports AccuPay.Entity

Public Class CalendarDayEditor

    Public Event Ok(payrate As PayRate)

    Private _payRate As PayRate

    Private _dayTypes As ICollection(Of String)

    Public Sub New()
        InitializeComponent()

        _dayTypes = New Collection(Of String) From {
            "Regular Day",
            "Special Non-Working Holiday",
            "Regular Holiday"
        }
    End Sub

    Private Sub CalendarDayEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DayTypesComboBox.DataSource = _dayTypes
    End Sub

    Public Sub ChangePayRate(payrate As PayRate)
        _payRate = payrate

        DayLabel.Text = _payRate.Date.Day.ToString()
        DescriptionTextBox.Text = _payRate.Description
        DayTypesComboBox.SelectedItem = _payRate.PayType
    End Sub

    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        _payRate.Description = DescriptionTextBox.Text
        _payRate.PayType = DirectCast(DayTypesComboBox.SelectedItem, String)

        RaiseEvent Ok(_payRate)
        Hide()
    End Sub

End Class
