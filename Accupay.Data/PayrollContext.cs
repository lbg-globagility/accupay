using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AccuPay.Data
{
    public class PayrollContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        //internal static readonly LoggerFactory DbCommandConsoleLoggerFactory = new LoggerFactory(new[] {
        //    new ConsoleLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name &&
        //                                            level == LogLevel.Information, true)
        //});

        internal virtual DbSet<ActualAdjustment> ActualAdjustments { get; set; }
        internal virtual DbSet<ActualTimeEntry> ActualTimeEntries { get; set; }
        internal virtual DbSet<Address> Addresses { get; set; }
        internal virtual DbSet<Adjustment> Adjustments { get; set; }
        internal virtual DbSet<Agency> Agencies { get; set; }
        internal virtual DbSet<AgencyFee> AgencyFees { get; set; }
        internal virtual DbSet<Allowance> Allowances { get; set; }
        internal virtual DbSet<AllowanceItem> AllowanceItems { get; set; }
        internal virtual DbSet<Award> Awards { get; set; }
        internal virtual DbSet<Bonus> Bonuses { get; set; }
        internal virtual DbSet<BreakTimeBracket> BreakTimeBrackets { get; set; }
        internal virtual DbSet<Branch> Branches { get; set; }
        internal virtual DbSet<PayCalendar> Calendars { get; set; }
        internal virtual DbSet<CalendarDay> CalendarDays { get; set; }
        internal virtual DbSet<Category> Categories { get; set; }
        internal virtual DbSet<Certification> Certifications { get; set; }
        internal virtual DbSet<DayType> DayTypes { get; set; }
        internal virtual DbSet<Division> Divisions { get; set; }
        internal virtual DbSet<DivisionMinimumWage> DivisionMinimumWages { get; set; }
        internal virtual DbSet<EducationalBackground> EducationalBackgrounds { get; set; }
        internal virtual DbSet<Employee> Employees { get; set; }
        internal virtual DbSet<EmployeeDutySchedule> EmployeeDutySchedules { get; set; }
        internal virtual DbSet<FilingStatusType> FilingStatusTypes { get; set; }
        internal virtual DbSet<JobCategory> JobCategories { get; set; }
        internal virtual DbSet<JobLevel> JobLevels { get; set; }
        internal virtual DbSet<Leave> Leaves { get; set; }
        internal virtual DbSet<LeaveLedger> LeaveLedgers { get; set; }
        internal virtual DbSet<LeaveTransaction> LeaveTransactions { get; set; }
        internal virtual DbSet<ListOfValue> ListOfValues { get; set; }
        internal virtual DbSet<LoanSchedule> LoanSchedules { get; set; }
        internal virtual DbSet<LoanTransaction> LoanTransactions { get; set; }
        internal virtual DbSet<OfficialBusiness> OfficialBusinesses { get; set; }
        internal virtual DbSet<Organization> Organizations { get; set; }
        internal virtual DbSet<Overtime> Overtimes { get; set; }
        internal virtual DbSet<PayFrequency> PayFrequencies { get; set; }
        internal virtual DbSet<PayPeriod> PayPeriods { get; set; }
        internal virtual DbSet<PayRate> PayRates { get; set; }
        internal virtual DbSet<Paystub> Paystubs { get; set; }
        internal virtual DbSet<PaystubActual> PaystubActuals { get; set; }
        internal virtual DbSet<PaystubEmail> PaystubEmails { get; set; }
        internal virtual DbSet<PaystubEmailHistory> PaystubEmailHistories { get; set; }
        internal virtual DbSet<PaystubItem> PaystubItems { get; set; }
        internal virtual DbSet<PhilHealthBracket> PhilHealthBrackets { get; set; }
        internal virtual DbSet<Position> Positions { get; set; }
        internal virtual DbSet<PositionView> PositionViews { get; set; }
        internal virtual DbSet<PreviousEmployer> PreviousEmployers { get; set; }
        internal virtual DbSet<Privilege> Privileges { get; set; }
        internal virtual DbSet<Product> Products { get; set; }
        internal virtual DbSet<Salary> Salaries { get; set; }
        internal virtual DbSet<Shift> Shifts { get; set; }
        internal virtual DbSet<ShiftSchedule> ShiftSchedules { get; set; }
        internal virtual DbSet<SocialSecurityBracket> SocialSecurityBrackets { get; set; }
        internal virtual DbSet<SystemOwner> SystemOwners { get; set; }
        internal virtual DbSet<TardinessRecord> TardinessRecords { get; set; }
        internal virtual DbSet<TimeEntry> TimeEntries { get; set; }
        internal virtual DbSet<TimeAttendanceLog> TimeAttendanceLogs { get; set; }
        internal virtual DbSet<TimeLog> TimeLogs { get; set; }
        internal virtual DbSet<User> Users { get; set; }
        internal virtual DbSet<UserActivity> UserActivities { get; set; }
        internal virtual DbSet<UserActivityItem> UserActivityItems { get; set; }
        internal virtual DbSet<ThirteenthMonthPay> ThirteenthMonthPays { get; set; }
        internal virtual DbSet<WithholdingTaxBracket> WithholdingTaxBrackets { get; set; }

        public PayrollContext(DbContextOptions options)
            : base(options)
        {
        }

        //public PayrollContext()
        //{
        //}

        //public PayrollContext(ILoggerFactory loggerFactory)
        //{
        //    _loggerFactory = loggerFactory;
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql(new DataBaseConnection().GetStringMySQLConnectionString()).
        //                    UseLoggerFactory(_loggerFactory).
        //                    EnableSensitiveDataLogging();
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Paystub>().
            HasOne(x => x.ThirteenthMonthPay).
            WithOne(x => x.Paystub).
            HasForeignKey<ThirteenthMonthPay>(x => x.PaystubID);

            modelBuilder.Entity<Paystub>().
                HasMany(x => x.AllowanceItems).
                WithOne(x => x.Paystub);

            modelBuilder.Entity<Paystub>().
                HasMany(x => x.LoanTransactions).
                WithOne(x => x.Paystub);

            // Leave transaction should be tied to Time Entry Generation not Payroll Generation
            // thus leave transactions should be processed when we generate time entries.
            modelBuilder.Entity<Paystub>().
                HasMany(x => x.LeaveTransactions).
                WithOne(x => x.Paystub);

            modelBuilder.Entity<TardinessRecord>().
                HasKey(t => new { t.EmployeeId, t.Year });
        }
    }
}