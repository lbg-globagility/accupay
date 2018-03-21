Imports MySql.Data.MySqlClient

Public Class ReadSQLFunctionV2

    Dim n_ReturnValue = Nothing

    Private priv_conn As New MySqlConnection

    Private priv_da As New MySqlDataAdapter

    Private priv_cmd As New MySqlCommand

    Property ReturnValue As Object

        Get
            Return n_ReturnValue

        End Get

        Set(value As Object)
            n_ReturnValue = value

        End Set

    End Property

    Sub New(SQLProcedureName As String,
            returnName As String,
            ParaMeterNames() As String,
            ParamArray ParameterInput() As Object)

        'If cmd_time_out > 0 Then

        '    priv_conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString &
        '        "default command timeout=" & cmd_time_out & ";"

        'Else

        '    priv_conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString

        'End If

        'Dim n_DataBaseConnection As New DataBaseConnection

        priv_conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString

        ''priv_conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString &
        ''    "default command timeout=" & 500 & ";"

        SQLProcedureName = SQLProcedureName.Trim

        'Dim n_ExecuteQuery = _
        'New ExecuteQuery("SET group_concat_max_len = 2048;" & _
        '                 "SELECT GROUP_CONCAT(ii.PARAMETER_NAME)" &
        '                 " FROM information_schema.PARAMETERS ii" &
        '                 " WHERE ii.SPECIFIC_NAME = '" & SQLProcedureName & "'" &
        '                 " AND ii.`SPECIFIC_SCHEMA`='" & sys_db & "'" &
        '                 " AND ii.PARAMETER_NAME IS NOT NULL;", 999999)

        'Dim paramName = n_ExecuteQuery.Result

        Dim paramNames = ParaMeterNames 'Split(paramName, ",")

        Try

            If priv_conn.State = ConnectionState.Open Then : priv_conn.Close() : End If

            priv_cmd = New MySqlCommand(SQLProcedureName, priv_conn)

            priv_conn.Open()

            With priv_cmd

                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add(returnName, MySqlDbType.Int32)

                For e = 0 To paramNames.GetUpperBound(0)

                    Dim param_Name = paramNames(e)

                    Dim paramVal = ParameterInput(e)

                    .Parameters.AddWithValue(param_Name, paramVal)

                Next

                .Parameters(returnName).Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()

                n_ReturnValue = datread(0)

            End With

        Catch ex As Exception
            _hasError = True
            ErrMessage = getErrExcptn(ex, MyBase.ToString)
            MsgBox(ErrMessage, , SQLProcedureName)
        Finally
            priv_da.Dispose()

            priv_conn.Close()

            priv_cmd.Dispose()

        End Try

    End Sub

    Private _hasError As Boolean = False

    Property HasError As Boolean

        Get
            Return _hasError

        End Get

        Set(value As Boolean)
            _hasError = value

        End Set

    End Property

    Private ErrMessage As String = String.Empty

    Property ErrorMessage As String

        Get
            Return ErrMessage

        End Get

        Set(value As String)
            ErrMessage = value

        End Set

    End Property

End Class