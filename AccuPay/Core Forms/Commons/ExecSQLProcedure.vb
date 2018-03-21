Imports MySql.Data.MySqlClient

Public Class ExecSQLProcedure

    Dim n_ReturnValue = Nothing

    Private priv_conn As New MySqlConnection

    Private priv_da As New MySqlDataAdapter

    Private priv_cmd As New MySqlCommand

    Sub New(SQLProcedureName As String,
            ParamArray ParameterInput() As Object)

        priv_conn.ConnectionString = n_DataBaseConnection.GetStringMySQLConnectionString

        SQLProcedureName = SQLProcedureName.Trim

        Dim n_SQLQueryToDatatable = _
            New SQLQueryToDatatable("SELECT GROUP_CONCAT(ii.PARAMETER_NAME) `Result`" &
                                    " FROM information_schema.PARAMETERS ii" &
                                    " WHERE ii.SPECIFIC_NAME = '" & SQLProcedureName & "'" &
                                    " AND ii.`SPECIFIC_SCHEMA` = '" & sys_db & "'" &
                                    " AND ii.PARAMETER_NAME IS NOT NULL;")

        Dim catchdt As New DataTable
        catchdt = n_SQLQueryToDatatable.ResultTable

        Dim paramName As String = String.Empty

        For Each drow As DataRow In catchdt.Rows
            paramName = Convert.ToString(drow(0))
        Next

        Dim paramNames = Split(paramName, ",")

        Try

            If priv_conn.State = ConnectionState.Open Then : priv_conn.Close() : End If

            priv_cmd = New MySqlCommand(SQLProcedureName, priv_conn)

            priv_conn.Open()

            With priv_cmd

                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                For e = 0 To paramNames.GetUpperBound(0)

                    Dim param_Name = paramNames(e)

                    Dim paramVal = ParameterInput(e)

                    .Parameters.AddWithValue(param_Name, paramVal)

                Next

                .ExecuteNonQuery()

            End With

            _error_message = String.Empty

        Catch ex As Exception
            _hasError = True

            _error_message = getErrExcptn(ex, MyBase.ToString)

            MsgBox(_error_message, , SQLProcedureName)

        Finally

            priv_da.Dispose()

            priv_conn.Close()

            priv_cmd.Dispose()

        End Try

    End Sub

    Dim _hasError As Boolean = False

    Property HasError As Boolean

        Get
            Return _hasError

        End Get

        Set(value As Boolean)
            _hasError = value

        End Set

    End Property

    Dim _error_message As String = String.Empty

    Property ErrorMessage As String

        Get
            Return _error_message

        End Get

        Set(value As String)
            _error_message = value

        End Set

    End Property

End Class