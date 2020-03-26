Imports Microsoft.Win32

Public Class DataBaseConnection

    Dim regKey As RegistryKey

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
        Catch ex As Exception
            'MsgBox(getErrExcptn(ex, "DataBaseConnection"))
        Finally
            regKey.Close()

        End Try

        Return connstringresult

    End Function

End Class