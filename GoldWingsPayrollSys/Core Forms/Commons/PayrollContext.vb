
Imports System
Imports System.Data.Entity
Imports System.Data.Common
Imports System.Linq

Imports PayrollSys

Public Class PayrollContext
    Inherits DbContext

    Private Shared Function GetConnection() As DbConnection
        Dim connection = DbProviderFactories.GetFactory("MySql.Data.MySqlClient").CreateConnection()
        connection.ConnectionString = connectionString
        Return connection
    End Function

    Public Sub New()
        MyBase.New(GetConnection(), True)
    End Sub

    Public Overridable Property SocialSecurityBrackets As DbSet(Of SocialSecurityBracket)

    Public Overridable Property PhilHealthBrackets As DbSet(Of PhilHealthBracket)

    Public Overridable Property LoanSchedules As DbSet(Of LoanSchedule)

    Public Overridable Property LoanTransactions As DbSet(Of LoanTransaction)

End Class
