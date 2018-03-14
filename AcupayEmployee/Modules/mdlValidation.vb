Imports MySql.Data.MySqlClient

Module mdlValidation

    Public z_OrganizationID As Integer
    Public z_User As Integer
    Public Z_UserName As String
    Public z_postName As String
    Public z_CompanyName As String
    Public z_CompanyAddr As String
    Public Z_ErrorProvider As New ErrorProvider

    Public Function getDataTableForSQL(ByVal COMMD As String)
        Dim command As MySqlCommand = New MySqlCommand(COMMD, connection)

        Try
            Dim DataReturn As New DataTable
            '    Dim sql As String = COMMD

            command.Connection.Open()


            Dim adapter As MySqlDataAdapter = New MySqlDataAdapter(command)
            adapter.Fill(DataReturn)
            command.Connection.Close()
            Return DataReturn
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        Finally
            command.Connection.Close()
        End Try
    End Function

    Public Function SQL_ArrayList(ByVal Sqlcommand As String) As ArrayList
        connection = New MySqlConnection(connectionString)

        Dim ArString As New ArrayList
        Try
            connection.Open()
            Dim command As MySqlCommand =
                   New MySqlCommand(Sqlcommand, connection)
            command.CommandType = CommandType.Text
            Dim DR As MySqlDataReader = command.ExecuteReader
            Do While DR.Read
                ArString.Add(DR.GetValue(0))
            Loop
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            connection.Close()
        End Try
        Return ArString
    End Function

    Public Function EncrypedData(ByVal a As String)
        Dim Encryped As String = Nothing
        If Not a Is Nothing Then
            For Each x As Char In a
                Dim ToCOn As Integer = Convert.ToInt64(x) + 133
                Encryped &= Convert.ToChar(Convert.ToInt64(ToCOn))
            Next
        End If

        Return Encryped
    End Function

    Public Function DecrypedData(ByVal a As String)
        Dim DEcrypedio As String = Nothing
        If Not a Is Nothing Then
            For Each x As Char In a
                Dim ToCOn As Integer = Convert.ToInt64(x) - 133
                DEcrypedio &= Convert.ToChar(Convert.ToInt64(ToCOn))
            Next
        End If
        Return DEcrypedio
    End Function

    Public Function ObjectToString(ByVal obj As Object) As String
        Try
            If IsDBNull(obj) Then
                Return ""
            ElseIf obj = Nothing Then
                Return ""
            Else
                Return obj
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function getStringItem(ByVal Sqlcommand As String) As String
        connection = New MySqlConnection(connectionString)
        Dim itemSTR As String = Nothing
        Try
            connection.Open()
            Dim command As MySqlCommand =
                   New MySqlCommand(Sqlcommand, connection)
            command.CommandType = CommandType.Text
            Dim DR As MySqlDataReader = command.ExecuteReader
            Do While DR.Read
                itemSTR = ObjectToString(DR.GetValue(0))
            Loop
        Catch ex As Exception
            itemSTR = ""
        Finally
            connection.Close()
        End Try
        Return itemSTR
    End Function

End Module
