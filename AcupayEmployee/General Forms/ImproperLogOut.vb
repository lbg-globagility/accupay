'Imports System.IO
'Imports MySql.Data.MySqlClient
'Imports System.Threading
'Imports System.Threading.Tasks

Public Class ImproperLogOut

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Sub ImproperLogOut()

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim listOfTextBox = Me.Controls.OfType(Of TextBox).ToList()

        For Each txtbxctrl In listOfTextBox
            txtbxctrl.Clear()
        Next

        MyBase.OnLoad(e)

    End Sub

    Private Sub ImproperLogOut_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnDone_Click(sender As Object, e As EventArgs) Handles btnDone.Click

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub txtPassword1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword1.KeyPress,
                                                                                        txtPassword2.KeyPress

        Dim e_asc As Integer = Asc(e.KeyChar)

        If e_asc = 13 Then
            btnDone.PerformClick()
        End If

    End Sub

    Private Sub txtPassword1_TextChanged(sender As Object, e As EventArgs) Handles txtPassword1.TextChanged,
                                                                                    txtPassword2.TextChanged

        Dim bool_result As Boolean = lblretry.Visible

        If bool_result Then
            lblretry.Visible = (Not bool_result)
        Else

        End If

    End Sub

End Class