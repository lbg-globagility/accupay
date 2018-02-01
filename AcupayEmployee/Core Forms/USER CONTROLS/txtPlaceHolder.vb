
Namespace PayrollSystem

End Namespace

Public Class txtPlaceHolder

    Inherits TextBox

    Private strEmpty As String = String.Empty

    Dim n_PlaceHolderText As String = Me.Name

    Dim TrueForeColor As Color = Color.Black

    Dim TruePasswordChar As Char = Me.PasswordChar

    Sub New()
        TrueForeColor = Me.ForeColor

        Me.ForeColor = me_PlaceHolderForeColor
        Me.Text = Me.Name

    End Sub

    Dim n_StringValue As String = Me.Name

    Public Property StringValue As String

        Get
            Return n_StringValue
        End Get
        Set(value As String)
            n_StringValue = value
        End Set
    End Property

    Public Property PlaceHolderText As String

        Get
            Return n_PlaceHolderText
        End Get
        Set(value As String)
            n_PlaceHolderText = value
        End Set
    End Property

    Dim me_PlaceHolderForeColor As Color = Color.FromArgb(191, 191, 255)
    '<Browsable(True), DefaultValue(staticPlaceHolderForeColor), Description("Gets or sets the place holder of this object.")> _
    Public Property PlaceHolderForeColor As Color

        Get
            Return me_PlaceHolderForeColor

        End Get

        Set(value As Color)
            me_PlaceHolderForeColor = value

        End Set

    End Property

    Protected Overrides Sub OnGotFocus(e As EventArgs)

        If Me.Text.Length = 0 Then
            'Me.PasswordChar = strEmpty
            Me.ForeColor = TrueForeColor
            Me.Text = strEmpty
            Me.PasswordChar = TruePasswordChar
        Else
            If Me.ForeColor = me_PlaceHolderForeColor Then
                Me.ForeColor = TrueForeColor
                Me.Text = strEmpty
            Else
                Me.PasswordChar = TruePasswordChar
            End If
        End If

        MyBase.OnGotFocus(e)

    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)

        'Me.ForeColor = TrueForeColor

        'If Me.Text.Length = 0 Then
        '    Me.PasswordChar = strEmpty
        '    Me.ForeColor = me_PlaceHolderForeColor
        '    Me.Text = n_PlaceHolderText
        'Else

        '    Me.PasswordChar = TruePasswordChar
        '    Me.ForeColor = TrueForeColor
        '    If Me.Text.Length = 0 Then
        '        Me.Text = strEmpty
        '    End If

        'End If

        MyBase.OnLeave(e)

    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)

        'If n_PlaceHolderText = Me.Text _
        '    And Me.ForeColor = me_PlaceHolderForeColor Then

        '    StringValue = strEmpty

        'Else
        '    If Me.Text.Length = 0 Then
        '        Me.PasswordChar = strEmpty
        '        Me.ForeColor = me_PlaceHolderForeColor
        '        Me.Text = n_PlaceHolderText
        '    Else

        '        Me.PasswordChar = TruePasswordChar
        '        Me.ForeColor = TrueForeColor
        '        If Me.Text.Length = 0 Then
        '            Me.Text = strEmpty
        '        End If

        '    End If

        'End If


        MyBase.OnTextChanged(e)

    End Sub

    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
        'n_PlaceHolderText = Me.Text _
        '    And 
        'If Me.ForeColor = me_PlaceHolderForeColor Then

        '    Me.PasswordChar = strEmpty
        'Else
        '    Me.PasswordChar = TruePasswordChar

        'End If

        MyBase.OnKeyPress(e)

    End Sub

End Class