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


            ver = regKey.GetValue("server") & ";" & _
                regKey.GetValue("user id") & ";" & _
                regKey.GetValue("password") & ";" & _
                regKey.GetValue("database") & ";"



            sys_servername = regKey.GetValue("server")

            sys_userid = regKey.GetValue("user id")

            sys_password = regKey.GetValue("password")

            sys_db = regKey.GetValue("database")

            sys_apppath = regKey.GetValue("apppath")

            installerpath = sys_apppath

            n_NameOfServer = sys_servername
            n_IDOfUser = sys_userid
            n_PasswordOfDatabase = sys_password
            n_NameOfDatabase = sys_db


            installerpath = regKey.GetValue("apppath")

            connstringresult = "server=" & sys_servername & _
                ";user id=" & sys_userid & _
                ";password=" & sys_password & _
                ";database=" & sys_db & ";"

            db_connectinstring = connstringresult

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, "DataBaseConnection"))

        Finally
            regKey.Close()

            Dim netBiosName = System.Environment.MachineName

            If sys_servername = netBiosName Then

                Dim regconnpath = Application.StartupPath & "\regconn.reg"

                File.Delete(regconnpath)
                '                Windows Registry Editor Version 5.00

                ';Created by LBG

                File.AppendAllText(regconnpath,
                                      "Windows Registry Editor Version 5.00" & Environment.NewLine &
                                      Environment.NewLine &
                                      ";Created by Globagility, Inc." & Environment.NewLine &
                                      Environment.NewLine &
                                      "[HKEY_LOCAL_MACHINE\SOFTWARE\Globagility\DBConn\GoldWings]" & Environment.NewLine &
                                      """apppath""=""" & Application.StartupPath & """" & Environment.NewLine &
                                      """database""=""" & sys_db & """" & Environment.NewLine &
                                      """password""=""" & sys_password & """" & Environment.NewLine &
                                      """server""=""" & netBiosName & """" & Environment.NewLine &
                                      """user id""=""" & sys_userid & """")

            End If

        End Try

        Return connstringresult

    End Function

End Class