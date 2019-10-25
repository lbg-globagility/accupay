Imports MySql.Data.MySqlClient

Public Class SQLQueryToDatatable

    Private priv_conn As New MySqlConnection

    Private priv_da As New MySqlDataAdapter

    Private priv_cmd As New MySqlCommand

    Sub New(SQLProcedureName As String,
            Optional cmd_time_out As Integer = 0)

        'Static mysql_conn_text As String = n_DataBaseConnection.GetStringMySQLConnectionString

        'Dim n_DataBaseConnection As New DataBaseConnection

        If cmd_time_out > 0 Then

            priv_conn.ConnectionString = mysql_conn_text &
                "default command timeout=" & cmd_time_out & ";"

        Else

            priv_conn.ConnectionString = mysql_conn_text

        End If

        SQLProcedureName = SQLProcedureName.Trim

        Try

            If priv_conn.State = ConnectionState.Open Then
                priv_conn.Close()

            End If

            priv_conn.Open()

            priv_cmd = New MySqlCommand

            With priv_cmd

                .Parameters.Clear()

                .Connection = priv_conn

                .CommandText = SQLProcedureName

                .CommandType = CommandType.Text

                priv_da.SelectCommand = priv_cmd

                priv_da.Fill(n_ResultTable)

            End With

        Catch ex As Exception
            _hasError = True
            MsgBox(getErrExcptn(ex, MyBase.ToString))
        Finally
            priv_da.Dispose()

            priv_conn.Close()

            priv_cmd.Dispose()

        End Try

    End Sub

    Dim n_ResultTable As New DataTable

    Property ResultTable As DataTable

        Get
            Return n_ResultTable

        End Get

        Set(value As DataTable)

            n_ResultTable = value

        End Set

    End Property

    Dim _hasError As Boolean = False

    Property HasError As Boolean

        Get
            Return _hasError

        End Get

        Set(value As Boolean)
            _hasError = value

        End Set

    End Property

End Class
