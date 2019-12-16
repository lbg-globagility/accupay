Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System.Configuration
Imports System.Data
Imports System.IO

Module mdlStoredProcedure

    Public connectionString As String = n_DataBaseConnection.GetStringMySQLConnectionString

    Public connection As MySqlConnection = New MySqlConnection(connectionString)

End Module