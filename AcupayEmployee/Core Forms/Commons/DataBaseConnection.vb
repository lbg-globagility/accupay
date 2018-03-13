Imports Microsoft.Win32
Imports System.IO

Public Class DataBaseConnection

    Dim regKey As RegistryKey

    Dim n_NameOfServer As String = String.Empty

    Property NameOfServer As String

        Get
            Return n_NameOfServer

        End Get

        Set(value As String)
            n_NameOfServer = value

        End Set

    End Property

    Dim n_IDOfUser As String = String.Empty

    Property IDOfUser As String
        Get
            Return n_IDOfUser

        End Get

        Set(value As String)
            n_IDOfUser = value

        End Set

    End Property

    Dim n_PasswordOfDatabase As String = String.Empty

    Property PasswordOfDatabase As String
        Get
            Return n_PasswordOfDatabase

        End Get

        Set(value As String)
            n_PasswordOfDatabase = value

        End Set

    End Property

    Dim n_NameOfDatabase As String = String.Empty

    Property NameOfDatabase As String
        Get
            Return n_NameOfDatabase

        End Get

        Set(value As String)
            n_NameOfDatabase = value

        End Set

    End Property

    Function GetStringMySQLConnectionString() As String

        Dim connstringresult As String

        connstringresult = New DatabaseConnect().ConnectionText

        Return connstringresult

    End Function

End Class