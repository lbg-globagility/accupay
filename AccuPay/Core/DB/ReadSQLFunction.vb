Option Strict On

Imports MySql.Data.MySqlClient

Public Class ReadSQLFunction

    Dim n_ReturnValue As Object = Nothing

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
            ParamArray ParameterInput() As Object)

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

        Dim paramNames() As String

        If Not IsDBNull(paramName) AndAlso paramName IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(paramName.ToString()) Then

            paramNames = Split(paramName.ToString(), ",")

        End If

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
            MsgBox(getErrExcptn(ex, MyBase.ToString), , SQLProcedureName)
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

End Class