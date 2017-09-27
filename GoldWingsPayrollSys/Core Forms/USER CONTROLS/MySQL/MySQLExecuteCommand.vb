Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions

Public Class MySQLExecuteCommand

#Region "Variables"

    Private mysql_cmd As IMySQLCommander

    Private prepared_mysqlcmd As MySqlCommand

    Private is_user_defined_dbobj As Boolean = False

    Private dat_adap As New MySqlDataAdapter

    Private regex_pattern As String =
        String.Concat("\", firstchar_requiredforparametername, "\w+")

    Private has_error As Boolean = False

    Private err_msg As String = String.Empty

    Private err As Exception

#End Region

#Region "Properties"

    Sub New(imysql_cmd As IMySQLCommander)
        mysql_cmd = imysql_cmd

        is_user_defined_dbobj =
            (mysql_cmd.CommandQuery.Contains(firstchar_requiredforparametername))

        PrepareMySQLCommand()

    End Sub

    ReadOnly Property GetFoundRow As Object
        Get

            ResetError()

            Dim return_val As Object = Nothing

            Dim dat_reader As MySqlDataReader

            Dim mysql_transac As MySqlTransaction

            mysql_transac = prepared_mysqlcmd.Connection.BeginTransaction

            Try

                ''prepared_mysqlcmd

                dat_reader = prepared_mysqlcmd.ExecuteReader

                Do While dat_reader.Read
                    return_val = dat_reader(0)

                Loop

                If IsDBNull(return_val) Then
                    return_val = Nothing

                End If

                dat_reader.Close()
                dat_reader.Dispose()

                mysql_transac.Commit()

            Catch ex As Exception
                mysql_transac.Rollback()

                AssignError(ex)

            Finally

                DisposeCommand()

            End Try

            Return return_val

        End Get

    End Property

    ReadOnly Property GetFoundRows As DataSet
        Get
            ResetError()

            Dim datset_result As New DataSet

            Dim mysql_transac As MySqlTransaction

            mysql_transac = prepared_mysqlcmd.Connection.BeginTransaction

            Try

                dat_adap.SelectCommand = prepared_mysqlcmd
                'dat_adap.SelectCommand = Me.PrepareMySQLCommand

                dat_adap.Fill(datset_result)

                mysql_transac.Commit()

            Catch ex As Exception
                mysql_transac.Rollback()

                AssignError(ex)

            Finally

                DisposeCommand()

                dat_adap.Dispose()

            End Try

            Return datset_result

        End Get

    End Property

    ReadOnly Property ErrorException As Exception

        Get
            Return err
        End Get
    End Property

    ReadOnly Property HasError As Boolean
        Get
            Return has_error

        End Get

    End Property

    ReadOnly Property ErrorMessage As String
        Get
            Return err_msg

        End Get

    End Property

#End Region

#Region "Functions"

    Private Function PrepareMySQLCommand() As MySqlCommand

        ResetError()

        Try

            Dim mysql_connectn = New MySqlConnection(mysql_conn_text) 'dbserver_connstring

            prepared_mysqlcmd =
                New MySqlCommand(mysql_cmd.CommandQuery,
                                 mysql_connectn)

            With prepared_mysqlcmd

                If .Connection.State = ConnectionState.Open Then
                    .Connection.Close()

                End If

                .Connection.Open()

                Try
                    'If is_user_defined_dbobj Then
                    .CommandType = CommandType.Text

                    .Connection = mysql_connectn

                    'Else
                    '    .CommandType = CommandType.StoredProcedure

                    'End If

                Catch ex As Exception
                    .CommandType = CommandType.StoredProcedure

                End Try

                If mysql_cmd.CommandTimingOut > 0 Then
                    .CommandTimeout = mysql_cmd.CommandTimingOut

                End If

                .Parameters.Clear()

                If mysql_cmd.ParameterValueCollection IsNot Nothing Then

                    Dim param_names =
                        Regex.Matches(mysql_cmd.CommandQuery,
                                     regex_pattern)

                    Dim i = 0

                    For Each param_val In mysql_cmd.ParameterValueCollection

                        Dim param_name As String = param_names(i).Value

                        Dim sel_isparam_exists = .Parameters.Cast(Of MySqlParameter).Where(Function(sql_p) sql_p.ParameterName = param_name)

                        If sel_isparam_exists.Count = 0 Then

                            .Parameters.
                                AddWithValue(param_name,
                                             param_val)

                        End If

                        i += 1

                    Next

                End If

                If mysql_cmd.VariableNameOfReturningValue.Length > 0 Then
                    .Parameters(mysql_cmd.VariableNameOfReturningValue).Direction =
                        ParameterDirection.ReturnValue

                End If

            End With

        Catch ex As Exception

            AssignError(ex)

        End Try

        Return prepared_mysqlcmd

    End Function

#End Region

#Region "Methods"

    Private Sub ResetError()
        err = Nothing
        err_msg = String.Empty
        has_error = False

    End Sub

    Private Sub AssignError(excptn As Exception)
        err = excptn
        err_msg = excptn.Message
        has_error = True

    End Sub

    Public Sub Execute()

        ResetError()

        Dim mysql_transac As MySqlTransaction

        mysql_transac = prepared_mysqlcmd.Connection.BeginTransaction

        Try

            prepared_mysqlcmd.ExecuteNonQuery()

            mysql_transac.Commit()

        Catch ex As Exception
            mysql_transac.Rollback()

            AssignError(ex)

        Finally

            DisposeCommand()

        End Try

    End Sub

    Private Sub DisposeCommand()

        If prepared_mysqlcmd IsNot Nothing Then

            prepared_mysqlcmd.Connection.Close()

            prepared_mysqlcmd.Dispose()

        End If

    End Sub

#End Region

End Class