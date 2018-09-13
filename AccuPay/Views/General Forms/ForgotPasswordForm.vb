Imports System.Net.Mail

Public Class ForgotPasswordForm
    Sub fillPosition()
        Dim strQuery As String = "select Name from Organization"
        cmbCompany.Items.Clear()
        cmbCompany.Items.Add("-Select One-")
        cmbCompany.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbCompany.SelectedIndex = 0
    End Sub
    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        'If txtEmailAdd.Text = "" Or txtUserID.Text = "" Or cmbCompany.Text = "--Select One" Then
        '    MsgBox("Please complete all fields.", MsgBoxStyle.Exclamation, "System message")
        'Else

        If cmbCompany.Text = "-Select One-" Then
            MsgBox("Please select your company.", MsgBoxStyle.Exclamation, "System message")
        ElseIf txtUserID.Text = "" Then
            MsgBox("Please enter your User ID", MsgBoxStyle.Exclamation, "System message.")
        Else
            Dim orgID As String = getStringItem("Select RowID from organization where name = '" & cmbCompany.Text & "'")
            Dim getID As Integer = Val(orgID)
            Dim emailadd As String = getStringItem("Select EmailAddress from user where UserID = '" & EncrypedData(txtUserID.Text) & "' And OrganizationID = " & getID & "")
            Dim geteamiladd As String = emailadd

            If emailadd = "" Then
                MsgBox("You don't have an email address, the system failed to send your password.", MsgBoxStyle.Exclamation, "No email address detected")
                Exit Sub
            End If

            Try
                Dim org As String = getStringItem("Select RowID from Organization where name = '" & cmbCompany.Text & "'")
                Dim getOrgID As Integer = Val(org)
                Dim pw As String = getStringItem("Select Password from user where userid = '" & EncrypedData(txtUserID.Text) & "' And OrganizationID = " & getOrgID & "")
                Dim getpw As String = DecrypedData(pw)
                Dim stmp_server As New SmtpClient
                Dim email As New MailMessage
                stmp_server.UseDefaultCredentials = False

                stmp_server.Credentials = New Net.NetworkCredential("testemailforglobagility@gmail.com", "PIN4545global")
                stmp_server.Port = 587
                stmp_server.EnableSsl = True
                stmp_server.Host = "smtp.gmail.com"

                email = New MailMessage()
                email.From = New MailAddress("testemailforglobagility@gmail.com")
                email.To.Add(geteamiladd)
                email.Subject = "Forgot password."
                email.IsBodyHtml = False
                email.Body = "Your password is " & getpw & ""
                stmp_server.Send(email)

                MsgBox("Your Password will be sent to the email address associated to your user name", MsgBoxStyle.Information, "Sent Successfully.")

                Me.Close()
            Catch ex As Exception
                '"No Connection."
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Sending password failed") 'No Internet Connection.
            End Try

        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtUserID.Clear()
        Me.Close()

    End Sub

    Private Sub ForgotPasswordForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        MetroLogin.Show()

        MetroLogin.BringToFront()

        MetroLogin.txtbxUserID.Focus()

    End Sub

    Private Sub ForgotPasswordForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillPosition()

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