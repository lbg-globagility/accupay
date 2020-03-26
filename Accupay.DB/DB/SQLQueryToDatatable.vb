Imports MySql.Data.MySqlClient

Public Class SQLQueryToDatatable

    Public Property ResultTable As DataTable
    Public Property HasError As Boolean

    Sub New(SQLProcedureName As String,
            Optional cmd_time_out As Integer = 0)

        Dim mysqlCon As New MySqlConnection
        Dim mysqlDataAdapater = New MySqlDataAdapter

        mysqlCon.ConnectionString = New DataBaseConnection().GetStringMySQLConnectionString

        If cmd_time_out > 0 Then

            mysqlCon.ConnectionString &= "default command timeout=" & cmd_time_out & ";"
        End If

        SQLProcedureName = SQLProcedureName.Trim

        Try

            If mysqlCon.State = ConnectionState.Open Then
                mysqlCon.Close()

            End If

            mysqlCon.Open()

            Dim mysqlCmd = New MySqlCommand

            With mysqlCmd
                .Parameters.Clear()
                .Connection = mysqlCon
                .CommandText = SQLProcedureName
                .CommandType = CommandType.Text

                mysqlDataAdapater.SelectCommand = mysqlCmd
                ResultTable = New DataTable
                mysqlDataAdapater.Fill(ResultTable)

            End With
        Catch ex As Exception
            HasError = True
            MsgBox(Utils.getErrExcptn(ex, MyBase.ToString))
        Finally
            mysqlDataAdapater.Dispose()
            mysqlCon.Close()
            mysqlDataAdapater.Dispose()
        End Try

    End Sub

End Class