Imports System.Configuration

Public Class DatabaseConnect

    Private database_connection_credentials As Specialized.NameValueCollection =
        ConfigurationManager.AppSettings

    Private DatabaseName As String =
        database_connection_credentials.Get("database")

    Private ServerName As String =
        database_connection_credentials.Get("server")

    Private UserId As String =
        database_connection_credentials.Get("userid")

    Private Password As String =
        database_connection_credentials.Get("password")

    ReadOnly Property ConnectionText As String
        Get
            Dim conn_str As String =
                String.Join(";",
                            String.Concat("database=", DatabaseName),
                            String.Concat("server=", ServerName),
                            String.Concat("user id=", UserId),
                            String.Concat("password=", Password)
                            )

            Return conn_str

        End Get

    End Property

End Class
