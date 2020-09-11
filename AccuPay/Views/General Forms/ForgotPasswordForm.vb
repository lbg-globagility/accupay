Imports System.Net
Imports System.Net.Mail
Imports AccuPay.Data.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Public Class ForgotPasswordForm

    Private ReadOnly _encryptor As IEncryption

    Sub New()

        InitializeComponent()

        _encryptor = MainServiceProvider.GetRequiredService(Of IEncryption)
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click

        If txtUserID.Text = "" Then
            MsgBox("Please enter your User ID", MsgBoxStyle.Exclamation, "System message.")

        End If

        Dim encryptedUsername = _encryptor.Encrypt(txtUserID.Text)

        Dim emailadd As String = getStringItem("Select EmailAddress from user where UserID = '" & encryptedUsername & "'")
        Dim getemailadd As String = emailadd

        If emailadd = "" Then
            MsgBox("You don't have an email address, the system failed to send your password.", MsgBoxStyle.Exclamation, "No email address detected")
            Exit Sub
        End If

        Try
            Dim pw As String = getStringItem("Select Password from user where userid = '" & encryptedUsername & "'")

            Dim getpw As String = _encryptor.Decrypt(pw)

            SendEmail(emailadd, getpw)

            MsgBox("Your Password will be sent to the email address associated to your user name", MsgBoxStyle.Information, "Sent Successfully.")

            Me.Close()
        Catch ex As Exception
            '"No Connection."
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Sending password failed") 'No Internet Connection.
        End Try

    End Sub

    Private Shared Sub SendEmail(sendTo As String, getpw As String)
        Dim mail As MailMessage = New MailMessage()
        Dim SmtpServer As SmtpClient = New SmtpClient()
        Dim smtpsender As String = "testemailforglobagility@gmail.com"
        Dim subject As String = "Forgot Password"
        Dim body As String = "Your password is " & getpw & ""
        Dim smtpDisplayName As String = "Accupay"
        Dim smtpHost As String = "smtp.gmail.com"
        Dim smtpPort As Integer = 587
        Dim smtpPassword As String = "PIN4545global"
        mail.From = New MailAddress(smtpsender, smtpDisplayName)
        mail.[To].Add(sendTo)
        mail.Subject = subject
        mail.Body = body

        SmtpServer.Host = smtpHost
        SmtpServer.Port = smtpPort
        SmtpServer.UseDefaultCredentials = False
        SmtpServer.Credentials = New NetworkCredential(smtpsender, smtpPassword)
        SmtpServer.EnableSsl = True
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
        SmtpServer.Send(mail)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtUserID.Clear()
        Me.Close()

    End Sub

    Private Sub ForgotPasswordForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        MetroLogin.Show()

        MetroLogin.BringToFront()

        MetroLogin.UserNameTextBox.Focus()

    End Sub

    Private Sub txtUserID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUserID.KeyPress

        Dim e_asc As String = Asc(e.KeyChar)

        If e_asc = 13 Then
            btnSend_Click(sender, e)
        End If

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return True
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

End Class