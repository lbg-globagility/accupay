Option Strict On

Imports AccuPay.Core.Entities

Public Class ViewLeaveLedgerTypeSelector

    Public Event SelectionChanged(leaveType As Product)

    Private _leaveTypes As ICollection(Of Product)

    Private _currentLeaveType As Product

    Public WriteOnly Property LeaveTypes As ICollection(Of Product)
        Set(value As ICollection(Of Product))
            _leaveTypes = value
            DisplayButtons()
        End Set
    End Property

    Private Sub DisplayButtons()
        For Each leaveType In _leaveTypes
            Dim button = New Button()
            button.Text = leaveType.PartNo
            button.Tag = leaveType

            AddHandler button.Click, AddressOf Selected

            FlowLayoutPanel.Controls.Add(button)
        Next

        If _currentLeaveType Is Nothing Then
            _currentLeaveType = _leaveTypes.FirstOrDefault()
            RaiseEvent SelectionChanged(_currentLeaveType)
        End If
    End Sub

    Private Sub Selected(sender As Object, e As EventArgs)
        Dim button = DirectCast(sender, Button)
        _currentLeaveType = DirectCast(button.Tag, Product)

        RaiseEvent SelectionChanged(_currentLeaveType)
    End Sub

End Class
