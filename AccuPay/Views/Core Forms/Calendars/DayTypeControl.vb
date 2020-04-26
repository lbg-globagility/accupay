Option Strict On

Imports System.Collections.ObjectModel
Imports AccuPay.Data.Entities

Public Class DayTypeControl

    Private _dayType As DayType

    Private ReadOnly _dayConsideredAsOptions As Collection(Of String) = New Collection(Of String) From {
        "Regular Day",
        "Special Non-Working Holiday",
        "Regular Holiday"
    }

    Public Property DayType As DayType
        Get
            UpdateDayType()
            Return _dayType
        End Get
        Set(value As DayType)
            _dayType = value
            RefreshForm()
        End Set
    End Property

    Private Sub DayTypeControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DayConsideredAsComboBox.DataSource = _dayConsideredAsOptions
    End Sub

    Private Sub UpdateDayType()
        If _dayType Is Nothing Then Return

        _dayType.Name = NameTextBox.Text
        _dayType.RegularRate = CDec(RegularTextBox.Text)
        _dayType.OvertimeRate = CDec(OvertimeTextBox.Text)
        _dayType.NightDiffRate = CDec(NightDiffTextBox.Text)
        _dayType.NightDiffOTRate = CDec(NightDiffOTTextBox.Text)
        _dayType.RestDayRate = CDec(RestDayTextBox.Text)
        _dayType.RestDayOTRate = CDec(RestDayOTTextBox.Text)
        _dayType.RestDayNDRate = CDec(RestDayNDTextBox.Text)
        _dayType.RestDayNDOTRate = CDec(RestDayNDOTTextBox.Text)
        _dayType.DayConsideredAs = DirectCast(DayConsideredAsComboBox.SelectedItem, String)
    End Sub

    Private Sub RefreshForm()
        If _dayType Is Nothing Then Return

        NameTextBox.Text = _dayType.Name
        RegularTextBox.Text = _dayType.RegularRate.ToString()
        OvertimeTextBox.Text = _dayType.OvertimeRate.ToString()
        NightDiffTextBox.Text = _dayType.NightDiffRate.ToString()
        NightDiffOTTextBox.Text = _dayType.NightDiffOTRate.ToString()
        RestDayTextBox.Text = _dayType.RestDayRate.ToString()
        RestDayOTTextBox.Text = _dayType.RestDayOTRate.ToString()
        RestDayNDTextBox.Text = _dayType.RestDayNDRate.ToString()
        RestDayNDOTTextBox.Text = _dayType.RestDayNDOTRate.ToString()
        DayConsideredAsComboBox.SelectedItem = _dayType.DayConsideredAs
    End Sub

End Class