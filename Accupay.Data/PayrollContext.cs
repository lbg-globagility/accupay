using Accupay.DB;
using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AccuPay.Data
{
    internal class PayrollContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public static readonly LoggerFactory DbCommandConsoleLoggerFactory = new LoggerFactory(new[] {
            new ConsoleLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name &&
                                                    level == LogLevel.Information, true)
        });

        public virtual DbSet<ActualAdjustment> ActualAdjustments { get; set; }
        public virtual DbSet<ActualTimeEntry> ActualTimeEntries { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Adjustment> Adjustments { get; set; }
        public virtual DbSet<Agency> Agencies { get; set; }
        public virtual DbSet<AgencyFee> AgencyFees { get; set; }
        public virtual DbSet<Allowance> Allowances { get; set; }
        public virtual DbSet<AllowanceItem> AllowanceItems { get; set; }
        public virtual DbSet<Award> Awards { get; set; }
        public virtual DbSet<Bonus> Bonuses { get; set; }
        public virtual DbSet<BreakTimeBracket> BreakTimeBrackets { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<PayCalendar> Calendars { get; set; }
        public virtual DbSet<CalendarDay> CalendarDays { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Certification> Certifications { get; set; }
        public virtual DbSet<DayType> DayTypes { get; set; }
        public virtual DbSet<DisciplinaryAction> DisciplinaryActions { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<DivisionMinimumWage> DivisionMinimumWages { get; set; }
        public virtual DbSet<EducationalBackground> EducationalBackgrounds { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeDutySchedule> EmployeeDutySchedules { get; set; }
        internal virtual DbSet<FilingStatusType> FilingStatusTypes { get; set; }
        public virtual DbSet<JobCategory> JobCategories { get; set; }
        public virtual DbSet<JobLevel> JobLevels { get; set; }
        public virtual DbSet<Leave> Leaves { get; set; }
        public virtual DbSet<LeaveLedger> LeaveLedgers { get; set; }
        public virtual DbSet<LeaveTransaction> LeaveTransactions { get; set; }
        public virtual DbSet<ListOfValue> ListOfValues { get; set; }
        public virtual DbSet<LoanSchedule> LoanSchedules { get; set; }
        public virtual DbSet<LoanTransaction> LoanTransactions { get; set; }
        public virtual DbSet<OfficialBusiness> OfficialBusinesses { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Overtime> Overtimes { get; set; }
        public virtual DbSet<PayFrequency> PayFrequencies { get; set; }
        public virtual DbSet<PayPeriod> PayPeriods { get; set; }
        public virtual DbSet<PayRate> PayRates { get; set; }
        public virtual DbSet<Paystub> Paystubs { get; set; }
        public virtual DbSet<PaystubActual> PaystubActuals { get; set; }
        public virtual DbSet<PaystubEmail> PaystubEmails { get; set; }
        public virtual DbSet<PaystubEmailHistory> PaystubEmailHistories { get; set; }
        public virtual DbSet<PaystubItem> PaystubItems { get; set; }
        public virtual DbSet<PhilHealthBracket> PhilHealthBrackets { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<PositionView> PositionViews { get; set; }
        public virtual DbSet<PreviousEmployer> PreviousEmployers { get; set; }
        public virtual DbSet<Privilege> Privileges { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<ShiftSchedule> ShiftSchedules { get; set; }
        public virtual DbSet<SocialSecurityBracket> SocialSecurityBrackets { get; set; }
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

        public PayrollContext()
        {
        }

        public PayrollContext(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(new DataBaseConnection().GetStringMySQLConnectionString()).
                            UseLoggerFactory(_loggerFactory).
                            EnableSensitiveDataLogging();
        }

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