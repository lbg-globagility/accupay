Option Strict On

Public Class NullableDatePicker

    Private _lastCheckValue As Boolean

    Public Event CheckedChanged As EventHandler

    Protected Sub OnCheckedChanged()
        RaiseEvent CheckedChanged(Me, EventArgs.Empty)
    End Sub

    Private Sub NullableDatePicker_Load(sender As Object, e As EventArgs) Handles Me.Load
        DateTimePicker1.MinDate = Date.MinValue

        _lastCheckValue = DateTimePicker1.Checked
    End Sub

    Public Overloads Sub ResetText()
        DateTimePicker1.Checked = True
        DateTimePicker1.Value = Date.Now
    End Sub

    Public Overloads Property Value() As Date?
        Get
            If DateTimePicker1.Checked Then
                Return DateTimePicker1.Value
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Date?)
            If value Is Nothing Or value < DateTimePicker1.MinDate Then

                DateTimePicker1.Checked = False
            Else

                DateTimePicker1.Checked = True
                DateTimePicker1.Value = CDate(value)

            End If
        End Set
    End Property

    Public Overloads Function ToString() As String
        Return If(Me.Value Is Nothing, "Nothing", Me.Value.Value.ToString("MM/dd/yyyy hh:mm tt"))
    End Function

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged

        If _lastCheckValue <> DateTimePicker1.Checked AndAlso DateTimePicker1.Checked = False Then

            Me.Value = Nothing

        End If

        _lastCheckValue = DateTimePicker1.Checked

        OnCheckedChanged()

    End Sub

    Public Property Checked() As Boolean
        Get
            Return DateTimePicker1.Checked

        End Get

        Set(ByVal value As Boolean)
            DateTimePicker1.Checked = value

            'If Not _lastCheckValue = value Then OnCheckedChanged()

        End Set

    End Property

End Class
