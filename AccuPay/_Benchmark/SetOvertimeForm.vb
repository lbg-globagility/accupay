Option Strict On

Imports System.ComponentModel
Imports AccuPay.Benchmark
Imports AccuPay.Extensions
Imports AccuPay.Utils

Public Class SetOvertimeForm

    Public Property Overtimes As List(Of OvertimeInput)

    Private _payPerHour As Decimal

    Private _overtimeRateList As New List(Of Rate)

    Sub New(payPerHour As Decimal, overtimeRateList As List(Of Rate), Optional overtimes As List(Of OvertimeInput) = Nothing)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If overtimes Is Nothing Then
            Me.Overtimes = New List(Of OvertimeInput)
        Else
            Me.Overtimes = overtimes
        End If

        _payPerHour = payPerHour

        _overtimeRateList = overtimeRateList
    End Sub

    Private Sub BackButton_Click(sender As Object, e As EventArgs) Handles BackButton.Click

        Me.Close()

    End Sub

    Private Sub SetOvertimeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        OvertimeComboBox.DisplayMember = "Name"
        OvertimeComboBox.DataSource = _overtimeRateList

        OvertimeGridView.AutoGenerateColumns = False

        RefreshGridView()

        ClearForm()

    End Sub

    Private Function GetSelectedOvertimeRate() As Rate

        If OvertimeComboBox.SelectedIndex < 0 OrElse
            OvertimeComboBox.SelectedIndex >= Me._overtimeRateList.Count Then

            Return Nothing

        End If

        Return Me._overtimeRateList(OvertimeComboBox.SelectedIndex)

    End Function

    Private Sub RefreshGridView()

        TotalOvertimeHoursLabel.Text = $"Total Overtime (Hours): {Me.Overtimes.Sum(Function(o) o.Hours)}"
        TotalOvertimeAmountLabel.Text = Me.Overtimes.Sum(Function(o) o.Amount).ToString("#,##0.00#")

        OvertimeGridView.DataSource = New BindingList(Of OvertimeInput)(Me.Overtimes)

    End Sub

    Private Sub ClearForm()

        OvertimeComboBox.SelectedIndex = -1
        InputTextBox.Clear()
        PercentageTextBox.Clear()

        InputTextBox.Enabled = False

    End Sub

    Private Sub OvertimeComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) Handles OvertimeComboBox.DrawItem

        Dim backgroundColor As Brush
        Dim g = e.Graphics
        Dim rect = e.Bounds
        Dim displayString = ""
        Dim font = e.Font

        Dim selectedRate As Rate

        If e.Index < 0 Then

            Return
        Else

            selectedRate = _overtimeRateList(e.Index)

        End If

        If e.Index < 7 Then
            backgroundColor = Brushes.White

        ElseIf e.Index < 15 Then

            backgroundColor = Brushes.PaleVioletRed

        ElseIf e.Index < 24 Then

            backgroundColor = Brushes.Turquoise

        End If

        If e.Index >= 0 AndAlso selectedRate IsNot Nothing Then

            displayString = selectedRate.Name

        End If

        g.FillRectangle(backgroundColor, rect.X, rect.Y, rect.Width, rect.Height)
        g.DrawString(displayString, font, Brushes.Black, rect.X, rect.Y)

    End Sub

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click

        Dim overtimeRate = GetSelectedOvertimeRate()

        If overtimeRate Is Nothing Then

            MessageBoxHelper.Warning("Select an overtime type first.")
            Return
        End If

        If HoursRadioButton.Checked = False AndAlso DaysRadioButton.Checked = False Then

            MessageBoxHelper.Warning("Choose if input is in hours or days.")
            Return

        End If

        Dim isDay As Boolean = DaysRadioButton.Checked
        Dim input As Decimal = InputTextBox.Text.ToDecimal

        If input <= 0 Then

            MessageBoxHelper.Warning($"Enter a valid input for {If(isDay, "days", "hours")}.")
            Return

        End If

        Me.Overtimes.Add(New OvertimeInput(overtimeRate, input, isDay, _payPerHour))

        RefreshGridView()

        ClearForm()
    End Sub

    Private Sub OvertimeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles OvertimeComboBox.SelectedIndexChanged

        Dim overtimeRate = GetSelectedOvertimeRate()

        If overtimeRate Is Nothing Then

            PercentageTextBox.Clear()
            InputTextBox.Enabled = False
        Else

            PercentageTextBox.Text = overtimeRate.Rate.ToString
            InputTextBox.Enabled = True
            InputTextBox.Focus()
        End If

    End Sub

    Private Sub RemoveButton_Click(sender As Object, e As EventArgs) Handles RemoveButton.Click

        Dim overtime = CType(OvertimeGridView.CurrentRow?.DataBoundItem, OvertimeInput)

        If overtime Is Nothing Then

            MessageBoxHelper.Warning("No selected overtime.")

        End If

        Me.Overtimes.Remove(overtime)

        RefreshGridView()

        ClearForm()
    End Sub

End Class