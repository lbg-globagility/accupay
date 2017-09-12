Imports System
Imports System.Data.Entity
Imports System.Linq

Imports PayrollSys

Public Class PayrollContext
    Inherits DbContext

    ' Your context has been configured to use a 'PayrollContext' connection string from your application's 
    ' configuration file (App.config or Web.config). By default, this connection string targets the 
    ' 'GoldWingsPayrollSys.PayrollContext' database on your LocalDb instance. 
    ' 
    ' If you wish to target a different database and/or database provider, modify the 'PayrollContext' 
    ' connection string in the application configuration file.
    Public Sub New()
        MyBase.New("name=PayrollContext")
    End Sub

    ' Add a DbSet for each entity type that you want to include in your model. For more information 
    ' on configuring and using a Code First model, see http:'go.microsoft.com/fwlink/?LinkId=390109.
    ' Public Overridable Property MyEntities() As DbSet(Of MyEntity)

    Public Overridable Property LoanSchedules As DbSet(Of LoanSchedule)

    Public Overridable Property LoanTransactions As DbSet(Of LoanTransaction)

End Class
