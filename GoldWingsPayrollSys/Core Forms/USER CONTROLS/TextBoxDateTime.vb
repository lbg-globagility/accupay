
Public Class TextBoxDateTime

    Private n_ValueFormat As New DateTimeValueFormat

    Public Property ValueFormat() As DateTimeValueFormat

        Get
            Return n_ValueFormat

        End Get

        Set(value As DateTimeValueFormat)
            n_ValueFormat = value

        End Set

    End Property

    Enum DateTimeValueFormat
        YearMonthDay
        MonthDayYear
        DayYearMonth
        YearMonthDayHourMinuteSecondAMPM
        MonthDayYearHourMinuteSecondAMPM
        DayYearMonthHourMinuteSecondAMPM
    End Enum

    Dim n_DateDelimiter As String = "-"

    Property DateDelimiter As Char

        Get
            Return n_DateDelimiter

        End Get

        Set(value As Char)
            n_DateDelimiter = value

        End Set

    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)

        OnTextChanged(New EventArgs)

        MyBase.OnKeyDown(e)

    End Sub

    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)

        '45	-
        '47	/
        '59	:
        '32	space bar

        Dim e_asc = Asc(e.KeyChar)

        Dim isAlright = TrapNumKey(e_asc)

        'Dim str_sel =Me.TextBox1 .Text .Substring

        If n_ValueFormat = DateTimeValueFormat.YearMonthDay Then

            If e_asc = 8 Then

                e.Handled = False

                my_str_len -= 1
                If my_str_len < 0 Then
                    my_str_len = 0
                End If

                If my_str_len <> 0 Then

                    If Me.TextBox1.Text.Substring(TextBox1.SelectionStart, 1) = n_DateDelimiter Then
                        my_str_len -= 1

                        Me.TextBox1.Text = Me.TextBox1.Text.Substring(0, (TextBox1.SelectionStart + 1))

                        Me.TextBox1.Select(TextBox1.SelectionStart, 0)

                    End If

                End If

            Else

                Dim ii = TextBox1.SelectionStart

                If my_str_len = 4 Then

                    e.Handled = True

                    addDelimiter(e.KeyChar)

                Else

                    e.Handled = isAlright

                End If

                If isAlright = False Then

                    my_str_len += 1

                End If

                If my_str_len = 4 Then

                    e.Handled = True

                    addDelimiter(e.KeyChar)

                End If

            End If

            'my_str_len()

        ElseIf n_ValueFormat = DateTimeValueFormat.MonthDayYear Then

        ElseIf n_ValueFormat = DateTimeValueFormat.DayYearMonth Then

        ElseIf n_ValueFormat = DateTimeValueFormat.YearMonthDayHourMinuteSecondAMPM Then

        ElseIf n_ValueFormat = DateTimeValueFormat.MonthDayYearHourMinuteSecondAMPM Then

        ElseIf n_ValueFormat = DateTimeValueFormat.DayYearMonthHourMinuteSecondAMPM Then

        End If

        MyBase.OnKeyPress(e)

    End Sub

    Dim my_str_len = 0

    Protected Overrides Sub OnTextChanged(e As EventArgs)

        my_str_len = Me.TextBox1.Text.Length

        MyBase.OnTextChanged(e)

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        OnKeyDown(e)
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        OnKeyPress(e)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        my_str_len = Me.TextBox1.Text.Length
        OnTextChanged(e)
    End Sub

    Private Sub TextBoxDateTime_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        n_DateDelimiter = "-"

    End Sub

    Private Sub addDelimiter(Optional OriginalCharacter As Char = Nothing)
        Dim concat_str = OriginalCharacter & n_DateDelimiter

        Me.TextBox1.Text &= concat_str

        Me.TextBox1.Select(my_str_len, 0)

    End Sub

End Class