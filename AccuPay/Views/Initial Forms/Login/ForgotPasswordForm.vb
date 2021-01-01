Option Strict On

Imports System.Net
Imports System.Net.Mail
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class ForgotPasswordForm

    Private ReadOnly _encryptor As IEncryption
    Private ReadOnly _userRepository As AspNetUserRepository

    Sub New()

        InitializeComponent()

        _encryptor = MainServiceProvider.GetRequiredService(Of IEncryption)

        _userRepository = MainServiceProvider.GetRequiredService(Of AspNetUserRepository)
    End Sub

    Private Async Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click

        If String.IsNullOrWhiteSpace(txtUserID.Text) Then
            MsgBox("Please enter your User ID", MsgBoxStyle.Exclamation, "System message.")
            Return
        End If

        Dim user = Await _userRepository.GetByUserNameAsync(txtUserID.Text)

        Dim emailAddress As String = user.Email

        If String.IsNullOrWhiteSpace(emailAddress) Then
            MsgBox("You don't have an email address, the system failed to send your password.", MsgBoxStyle.Exclamation, "No email address detected")
            Return
        End If

        Try
            Dim password As String = _encryptor.Decrypt(user.DesktopPassword)

            SendEmail(emailAddress, password)

            MsgBox("Your Password was sent to the email address associated to your user name", MsgBoxStyle.Information, "Sent Successfully.")

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

End Class