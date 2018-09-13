
Public Class promptyear

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim maxyear = EXECQUER("SELECT YEAR(CURDATE());")

        NumericUpDown1.Maximum = maxyear

        NumericUpDown1.Value = maxyear

        MyBase.OnLoad(e)

    End Sub

    Dim sel_year = Nothing

    Public Property YearValue As Object

        Get
            Return sel_year
        End Get

        Set(value As Object)
            sel_year = value

        End Set

    End Property


    Private Sub promptyear_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Me.Close()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Me.DialogResult = Windows.Forms.DialogResult.OK

        sel_year = NumericUpDown1.Value

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Button2_Click(Button2, New EventArgs)

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub NumericUpDown1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles NumericUpDown1.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then

            Button1_Click(Button1, New EventArgs)

        End If

    End Sub

End Class