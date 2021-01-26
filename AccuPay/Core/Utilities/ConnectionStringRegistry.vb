Option Strict On

Imports Microsoft.Win32

Namespace Desktop.Helpers

    Public Class ConnectionStringRegistry

        Private _serverName As String

        Public ReadOnly Property ServerName As String
            Get
                Return _serverName
            End Get
        End Property

        Private _userId As String

        Public ReadOnly Property UserId As String
            Get
                Return _userId
            End Get
        End Property

        Private _password As String

        Public ReadOnly Property Password As String
            Get
                Return _password
            End Get
        End Property

        Private _databaseName As String

        Public ReadOnly Property DatabaseName As String
            Get
                Return _databaseName
            End Get
        End Property

        Private Sub New(serverName As String, userId As String, password As String, databaseName As String)
            _serverName = serverName
            _userId = userId
            _password = password
            _databaseName = databaseName
        End Sub

        Public Shared Function GetCurrent() As ConnectionStringRegistry

            Dim regKey = Registry.LocalMachine.OpenSubKey("Software\Globagility\DBConn\GoldWings", True)

            Dim server As String = ""
            Dim userId As String = ""
            Dim password As String = ""
            Dim database As String = ""

            If regKey IsNot Nothing Then

                server = regKey.GetValue("server").ToString
                userId = regKey.GetValue("user id").ToString
                password = regKey.GetValue("password").ToString
                database = regKey.GetValue("database").ToString

            End If

            Return New ConnectionStringRegistry(server, userId, password, database)

        End Function

    End Class

End Namespace