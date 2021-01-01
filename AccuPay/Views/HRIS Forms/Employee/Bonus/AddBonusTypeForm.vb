Option Strict On

Imports AccuPay.Core.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class AddBonusTypeForm

    Public Property HasChanges As Boolean

    Private Sub AddBonusTypeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TextBox1.ContextMenu = New ContextMenu

        HasChanges = False

    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        Try

            'TODO: check if bonus type name already exists!

            If Trim(TextBox1.Text) <> "" Then
                TextBox1.Text = StrConv(TextBox1.Text, VbStrConv.ProperCase)

                Dim repository = MainServiceProvider.GetRequiredService(Of ProductRepository)

                Await repository.AddBonusTypeAsync(
                    TextBox1.Text,
                    organizationId:=z_OrganizationID,
                    userId:=z_User,
                    isTaxable:=chktaxab.Checked)

            End If

            HasChanges = True

            Me.Close()
        Catch ex As Exception
            MsgBox("Something wrong occured.", MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        CloseDialog()
    End Sub

    Private Sub CloseDialog()
        Me.Close()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            CloseDialog()

            Return True
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

End Class
