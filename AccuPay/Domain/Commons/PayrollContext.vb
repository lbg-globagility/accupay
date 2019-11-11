Imports AccuPay.Entity
Imports AccuPay.JobLevels
Imports AccuPay.Loans
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging
Imports PayrollSys

Public Class PayrollContext
    Inherits DbContext

    Private ReadOnly _loggerFactory As ILoggerFactory

    Public Overridable Property Agencies As DbSet(Of Agency)

    Public Overridable Property AgencyFees As DbSet(Of AgencyFee)

    Public Overridable Property Categories As DbSet(Of Category)

    Public Overridable Property JobCategories As DbSet(Of JobCategory)

    Public Overridable Property JobLevels As DbSet(Of JobLevel)

    Public Overridable Property LoanSchedules As DbSet(Of LoanSchedule)

    Public Overridable Property LoanTransactions As DbSet(Of LoanTransaction)

    Public Overridable Property PayPeriods As DbSet(Of PayPeriod)

    Public Overridable Property PaystubActuals As DbSet(Of PaystubActual)

    Public Overridable Property PaystubEmailHistories As DbSet(Of PaystubEmailHistory)

    Public Overridable Property PaystubEmails As DbSet(Of PaystubEmail)

    Public Overridable Property PaystubItems As DbSet(Of PaystubItem)

    Public Overridable Property Paystubs As DbSet(Of Paystub)

    Public Overridable Property PhilHealthBrackets As DbSet(Of PhilHealthBracket)

    Public Overridable Property Positions As DbSet(Of Position)

    Public Overridable Property Products As DbSet(Of Product)

    Public Overridable Property Salaries As DbSet(Of Salary)

    Public Overridable Property SocialSecurityBrackets As DbSet(Of SocialSecurityBracket)

    Public Overridable Property ThirteenthMonthPays As DbSet(Of ThirteenthMonthPay)

    Public Overridable Property TimeLogs As DbSet(Of TimeLog)

    Public Overridable Property WithholdingTaxBrackets As DbSet(Of WithholdingTaxBracket)

    Public Overridable Property Employees As DbSet(Of Employee)

    Public Overridable Property Shifts As DbSet(Of Shift)

    Public Overridable Property ListOfValues As DbSet(Of ListOfValue)

    Public Overridable Property PayRates As DbSet(Of PayRate)

    Public Overridable Property Organizations As DbSet(Of Organization)

    Public Overridable Property Divisions As DbSet(Of Division)

    Public Overridable Property ShiftSchedules As DbSet(Of ShiftSchedule)

    Public Overridable Property TimeEntries As DbSet(Of TimeEntry)

    Public Overridable Property Allowances As DbSet(Of Allowance)

    Public Overridable Property AllowanceItems As DbSet(Of AllowanceItem)

    Public Overridable Property Adjustments As DbSet(Of Adjustment)

    Public Overridable Property ActualAdjustments As DbSet(Of ActualAdjustment)

    Public Overridable Property LeaveLedgers As DbSet(Of LeaveLedger)

    Public Overridable Property LeaveTransactions As DbSet(Of LeaveTransaction)

    Public Overridable Property Leaves As DbSet(Of Leave)

    Public Overridable Property Overtimes As DbSet(Of Overtime)

    Public Overridable Property OfficialBusinesses As DbSet(Of OfficialBusiness)

    Public Overridable Property ActualTimeEntries As DbSet(Of ActualTimeEntry)

    Public Overridable Property DivisionMinimumWages As DbSet(Of DivisionMinimumWage)

    Public Overridable Property EmployeeDutySchedules As DbSet(Of EmployeeDutySchedule)

    Public Overridable Property TimeAttendanceLogs As DbSet(Of TimeAttendanceLog)

    Public Overridable Property BreakTimeBrackets As DbSet(Of BreakTimeBracket)

    Public Overridable Property PayFrequencies As DbSet(Of PayFrequency)

    Public Overridable Property Branches As DbSet(Of Branch)

    Public Overridable Property Users As DbSet(Of User)

    Public Overridable Property TardinessRecords As DbSet(Of TardinessRecord)

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