Option Strict On

Imports AccuPay.Entity

Public Class DayTypeControl

    Private _dayType As DayType

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
    End Sub

End Class
