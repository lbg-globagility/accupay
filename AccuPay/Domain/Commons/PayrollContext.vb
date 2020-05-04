Option Strict On

Imports AccuPay.Loans
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging
Imports Microsoft.Extensions.Logging.Console

Public Class PayrollContext
    Inherits DbContext

    Private ReadOnly _loggerFactory As ILoggerFactory

    Public Shared ReadOnly DbCommandConsoleLoggerFactory As LoggerFactory =
       New LoggerFactory({New ConsoleLoggerProvider(Function(category, level) category = DbLoggerCategory.Database.Command.Name AndAlso level = LogLevel.Information, True)})

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

        'modelBuilder.Entity(Of Paystub).
        '    HasOne(Function(p) p.ThirteenthMonthPay).
        '    WithOne(Function(t) t.Paystub).
        '    HasForeignKey(Of ThirteenthMonthPay)(Function(t) t.PaystubID)

        'modelBuilder.Entity(Of Paystub).
        '    HasMany(Function(p) p.AllowanceItems).
        '    WithOne(Function(a) a.Paystub)
    End Sub

End Class