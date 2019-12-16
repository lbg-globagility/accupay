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
    Private ReadOnly _applicationStartupPath As String

    Property NameOfDatabase As String
        Get
            Return n_NameOfDatabase

        End Get

        Set(value As String)
            n_NameOfDatabase = value

        End Set

    End Property

    Function GetStringMySQLConnectionString(Optional updateModuleVariables As Boolean = True) As String

        Dim ver = Nothing

        Dim connstringresult As String = String.Empty

        Try

            regKey = Registry.LocalMachine.OpenSubKey("Software\Globagility\DBConn\GoldWings", True)

            If regKey Is Nothing Then

                regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)

                regKey.CreateSubKey("Globagility\DBConn\GoldWings")

                regKey = Registry.LocalMachine.OpenSubKey("Software\Globagility\DBConn\GoldWings", True)

                regKey.SetValue("server", "localhost")

                regKey.SetValue("user id", "root")

                regKey.SetValue("password", "globagility")

                regKey.SetValue("database", "goldwingspayrolldb")

            End If

            ver = regKey.GetValue("server") & ";" &
                regKey.GetValue("user id") & ";" &
                regKey.GetValue("password") & ";" &
                regKey.GetValue("database") & ";"

            Dim server As String = regKey.GetValue("server")
            Dim userId As String = regKey.GetValue("user id")
            Dim password As String = regKey.GetValue("password")
            Dim database As String = regKey.GetValue("database")
            Dim apppath As String = regKey.GetValue("apppath")

            connstringresult = "server=" & server &
                ";user id=" & userId &
                ";password=" & password &
                ";database=" & database & ";"

            n_NameOfServer = server
            n_IDOfUser = userId
            n_PasswordOfDatabase = password
            n_NameOfDatabase = database

            'did this so that professional code can access this shit
            'visual basic modules are fucking trash
            If updateModuleVariables Then

                sys_servername = server
                sys_userid = userId
                sys_password = password
                sys_db = database
                sys_apppath = apppath
                installerpath = sys_apppath
                db_connectinstring = connstringresult

            End If
        Catch ex As Exception
            'MsgBox(getErrExcptn(ex, "DataBaseConnection"))
        Finally
            regKey.Close()

        End Try

        Return connstringresult

    End Function

End Class