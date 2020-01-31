Imports System.Configuration
Imports Accupay.Utilities
Imports GlobagilityShared.EmailSender

Public Class EmailConfig
    Implements IEmailConfig

    Public ReadOnly Property SmtpHost As String Implements IEmailConfig.SmtpHost
        Get
            Return ConfigurationManager.AppSettings("SmtpHost")
        End Get
    End Property

    Public ReadOnly Property SmtpPort As Integer Implements IEmailConfig.SmtpPort
        Get
            Dim port = ConfigurationManager.AppSettings("SmtpPort")

            Return ObjectUtils.ToInteger(port)
        End Get
    End Property

    Public ReadOnly Property SmtpSender As String Implements IEmailConfig.SmtpSender
        Get
            Return ConfigurationManager.AppSettings("SmtpSender")
        End Get
    End Property

    Public ReadOnly Property SmtpDisplayName As String Implements IEmailConfig.SmtpDisplayName
        Get
            Return ConfigurationManager.AppSettings("SmtpDisplayName")
        End Get
    End Property

    Public ReadOnly Property SmtpPassword As String Implements IEmailConfig.SmtpPassword
        Get
            Return ConfigurationManager.AppSettings("SmtpPassword")
        End Get
    End Property

End Class