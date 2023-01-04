Option Strict On

Imports System.Text.RegularExpressions
Imports MySql.Data.MySqlClient

Public Class ExecuteQuery

    Private priv_conn As New MySqlConnection

    Private priv_da As New MySqlDataAdapter

    Private priv_cmd As New MySqlCommand

    Private getResult As Object = Nothing

    Dim dr As MySqlDataReader

    Sub New(ByVal cmdsql As String,
            Optional cmd_time_out As Integer = 0)

        Static except_this_string() As String = {"CALL", "UPDATE", "DELETE"}

        'Dim n_DataBaseConnection As New DataBaseConnection

        If cmd_time_out > 0 Then
            'n_DataBaseConnection.GetStringMySQLConnectionString
            priv_conn.ConnectionString = mysql_conn_text &
                "default command timeout=" & cmd_time_out & ";"
        Else
            'n_DataBaseConnection.GetStringMySQLConnectionString
            priv_conn.ConnectionString = mysql_conn_text

        End If

        Try

            If priv_conn.State = ConnectionState.Open Then : priv_conn.Close() : End If

            priv_conn.Open()

            priv_cmd = New MySqlCommand

            Dim isContainsCallClause = False

            With priv_cmd

                .CommandType = CommandType.Text

                .Connection = priv_conn

                .CommandText = cmdsql

                If cmd_time_out > 0 Then
                    .CommandTimeout = cmd_time_out
                End If

                Dim pattern As String = "'[^']*'"
                Dim text = Regex.Replace(cmdsql, pattern, "")
                isContainsCallClause = text.Contains("CALL")

                If isContainsCallClause Then

                    .ExecuteNonQuery()

                ElseIf FindingWordsInString(cmdsql,
                                            except_this_string) Then

                    .ExecuteNonQuery()
                Else

                    dr = .ExecuteReader()

                End If

            End With

            If isContainsCallClause Then
                getResult = Nothing
            ElseIf FindingWordsInString(cmdsql,
                                        except_this_string) Then
                getResult = Nothing
            Else

                If dr.Read = True Then

                    If IsDBNull(dr(0)) Then
                        getResult = String.Empty
                    Else
                        getResult = dr(0)

                    End If
                Else
                    getResult = Nothing

                End If

            End If
        Catch ex As Exception

            _hasError = True
            _error_message = getErrExcptn(ex, "ExecuteQuery")
            MsgBox(_error_message, , cmdsql)
        Finally

            If dr IsNot Nothing Then
                dr.Close()
                dr.Dispose()
            End If

            priv_conn.Close()

            priv_cmd.Dispose()

        End Try

    End Sub

    Property Result As Object

        Get
            Return getResult

        End Get

        Set(value As Object)
            getResult = value

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
