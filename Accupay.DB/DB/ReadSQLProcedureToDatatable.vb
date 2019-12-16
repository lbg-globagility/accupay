Imports MySql.Data.MySqlClient

Public Class ReadSQLProcedureToDatatable

    Private priv_conn As New MySqlConnection

    Private priv_da As New MySqlDataAdapter

    Private priv_cmd As New MySqlCommand

    Sub New(SQLProcedureName As String, ParamArray ParameterInput() As Object)

        'Dim n_DataBaseConnection As New DataBaseConnection

        priv_conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString

        SQLProcedureName = SQLProcedureName.Trim

        Dim n_ExecuteQuery =
            New ExecuteQuery("SET group_concat_max_len = 2048;" &
                             "SELECT GROUP_CONCAT(ii.PARAMETER_NAME)" &
                             " FROM information_schema.PARAMETERS ii" &
                             " WHERE ii.SPECIFIC_NAME = '" & SQLProcedureName & "'" &
                             " AND ii.`SPECIFIC_SCHEMA`='" & sys_db & "'" &
                             " AND ii.PARAMETER_NAME IS NOT NULL;", 999999)

        Dim paramName = n_ExecuteQuery.Result

        Dim paramNames = Split(paramName, ",")

        Try

            If priv_conn.State = ConnectionState.Open Then
                priv_conn.Close()

            End If

            priv_conn.Open()

            priv_cmd = New MySqlCommand(SQLProcedureName, priv_conn)

            With priv_cmd

                .Connection = priv_conn

                .Parameters.Clear()

                Dim nameParam As String = String.Empty

                Dim paramVal As Object = Nothing

                Dim parametervalue = String.Empty

                For e = 0 To ParameterInput.GetUpperBound(0)

                    nameParam = paramNames(e)

                    paramVal = ParameterInput(e)

                    .Parameters.AddWithValue(nameParam, paramVal)

                    'If e > 0 Then

                    '    parametervalue &= ",'" & ParameterInput(e) & "'"

                    'Else

                    '    parametervalue &= "'" & ParameterInput(e) & "'"

                    'End If

                Next

                '.CommandText = "CALL " & SQLProcedureName & "(" & parametervalue & ");"

                .CommandType = CommandType.StoredProcedure 'Text

                priv_da.SelectCommand = priv_cmd

                priv_da.Fill(n_ResultTable)

            End With
        Catch ex As Exception
            _hasError = True
            MsgBox(getErrExcptn(ex, MyBase.ToString), , SQLProcedureName)
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