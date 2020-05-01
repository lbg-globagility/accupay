Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Loans
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging
Imports Microsoft.Extensions.Logging.Console
Imports PayrollSys

Public Class PayrollContext
    Inherits DbContext

    Private ReadOnly _loggerFactory As ILoggerFactory

    Public Shared ReadOnly DbCommandConsoleLoggerFactory As LoggerFactory =
       New LoggerFactory({New ConsoleLoggerProvider(Function(category, level) category = DbLoggerCategory.Database.Command.Name AndAlso level = LogLevel.Information, True)})

    Public Overridable Property Salaries As DbSet(Of Salary)

    Public Overridable Property TimeEntries As DbSet(Of TimeEntry)

    Public Overridable Property PayPeriods As DbSet(Of PayPeriod)

    Public Overridable Property Adjustments As DbSet(Of Adjustment)

    Public Overridable Property ActualAdjustments As DbSet(Of ActualAdjustment)

    Public Overridable Property TardinessRecords As DbSet(Of TardinessRecord)

    Public Overridable Property Products As DbSet(Of Product)

    Public Overridable Property LoanTransactions As DbSet(Of LoanTransaction)

    Public Overridable Property PaystubItems As DbSet(Of PaystubItem)

    Public Overridable Property Paystubs As DbSet(Of Paystub)
    Public Overridable Property Allowances As DbSet(Of Allowance)

    Public Overridable Property AllowanceItems As DbSet(Of AllowanceItem)

    Public Overridable Property LeaveLedgers As DbSet(Of LeaveLedger)

    Public Overridable Property LeaveTransactions As DbSet(Of LeaveTransaction)

    Public Sub New()
    End Sub

    Public Sub New(loggerFactory As ILoggerFactory)
        _loggerFactory = loggerFactory
    End Sub

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.
            UseMySql(connectionString).
            UseLoggerFactory(_loggerFactory).
            EnableSensitiveDataLogging()
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
        MyBase.OnModelCreating(modelBuilder)

        modelBuilder.Entity(Of Paystub).
            HasOne(Function(p) p.ThirteenthMonthPay).
            WithOne(Function(t) t.Paystub).
            HasForeignKey(Of ThirteenthMonthPay)(Function(t) t.PaystubID)

        modelBuilder.Entity(Of Paystub).
            HasMany(Function(p) p.AllowanceItems).
            WithOne(Function(a) a.Paystub)

        modelBuilder.Entity(Of TardinessRecord)().
            HasKey(Function(t) New With {t.EmployeeId, t.Year})
    End Sub

End Class