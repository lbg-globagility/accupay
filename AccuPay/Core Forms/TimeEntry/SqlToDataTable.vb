Option Strict On

Imports System.Threading.Tasks
Imports MySql.Data.MySqlClient

Public Class SqlToDataTable

    Private _query As String
    Private _timeOut As Integer
    Private _params As Dictionary(Of String, Object)

    Public Sub New(query As String, Optional timeOut As Integer = 0)
        _query = query
        _timeOut = timeOut
    End Sub

    Public Sub New(query As String, params As Dictionary(Of String, Object), Optional timeOut As Integer = 0)
        Me.New(query, timeOut)
        _params = params
    End Sub

    Public Function Read() As DataTable
        Dim dataTable = New DataTable()

        Using connection = CreateConnection(),
            command = New MySqlCommand(_query, connection),
            adapter = New MySqlDataAdapter(command)

            ApplyParameters(command)

            command.CommandType = CommandType.Text
            connection.Open()

            adapter.Fill(dataTable)
        End Using

        Return dataTable
    End Function

    Public Async Function ReadAsync() As Task(Of DataTable)
        Dim dataTable = New DataTable()

        Using connection = CreateConnection(),
            command = New MySqlCommand(_query, connection),
            adapter = New MySqlDataAdapter(command)

            ApplyParameters(command)

            command.CommandType = CommandType.Text

            Await connection.OpenAsync()
            adapter.Fill(dataTable)
        End Using

        Return dataTable
    End Function

    Private Sub ApplyParameters(command As MySqlCommand)
        If _params IsNot Nothing Then
            For Each pair In _params
                command.Parameters.AddWithValue(pair.Key, pair.Value)
            Next
        End If
    End Sub

    Private Function CreateConnection() As MySqlConnection
        Dim connectionString = String.Empty

        If _timeOut > 0 Then
            connectionString = mysql_conn_text & "default command timeout=" & _timeOut & ";"
        Else
            connectionString = mysql_conn_text
        End If

        Return New MySqlConnection(connectionString)
    End Function

End Class
