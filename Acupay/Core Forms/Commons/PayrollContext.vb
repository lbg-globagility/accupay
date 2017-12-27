Imports System.Data.Common
Imports System.Data.Entity
Imports AccuPay.Entity
Imports AccuPay.JobLevels
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

    Public Sub New(connection As DbConnection)
        MyBase.New(connection, True)
    End Sub

    Public Overridable Property Salaries As DbSet(Of Salary)

    Public Overridable Property SocialSecurityBrackets As DbSet(Of SocialSecurityBracket)

    Public Overridable Property PhilHealthBrackets As DbSet(Of PhilHealthBracket)

    Public Overridable Property LoanSchedules As DbSet(Of LoanSchedule)

    Public Overridable Property LoanTransactions As DbSet(Of LoanTransaction)

    Public Overridable Property TimeLogs As DbSet(Of TimeLog)

    Public Overridable Property Products As DbSet(Of Product)

    Public Overridable Property Paystubs As DbSet(Of AccuPay.Entity.Paystub)

    Public Overridable Property PayPeriods As DbSet(Of PayPeriod)

    Public Overridable Property PaystubItems As DbSet(Of PaystubItem)

    Public Overridable Property ThirteenthMonthPays As DbSet(Of ThirteenthMonthPay)

    Public Overridable Property JobCategories As DbSet(Of JobCategory)

    Public Overridable Property JobLevels As DbSet(Of JobLevel)

    Public Overridable Property Position As DbSet(Of Position)

    Public Overridable Property Employees As DbSet(Of Employee)

    Public Overridable Property Shifts As DbSet(Of Shift)

    Public Overridable Property ListOfValues As DbSet(Of ListOfValue)

    Public Overridable Property PayRates As DbSet(Of PayRate)

    Public Overridable Property Organizations As DbSet(Of Organization)

    Public Overridable Property ShiftSchedules As DbSet(Of ShiftSchedule)

    Public Overridable Property TimeEntries As DbSet(Of TimeEntry)

End Class