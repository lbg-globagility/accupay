Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories

Public Class newProdBonus

    Private _newProduct As New Product

    Private Sub newProdBonus_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        TextBox1.ContextMenu = New ContextMenu

    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If Trim(TextBox1.Text) <> "" Then
                TextBox1.Text = StrConv(TextBox1.Text, VbStrConv.ProperCase)

                Dim productRepo = New ProductRepository
                _newProduct = Await productRepo.AddBonusType(TextBox1.Text,
                                                             organizationID:=z_OrganizationID,
                                                             userID:=z_User,
                                                             isTaxable:=chktaxab.Checked)

            End If

            Me.Close()
        Catch ex As Exception
            MsgBox("Something wrong occured.", MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Button2_Click(Button2, New EventArgs)

            Return True
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

End Class