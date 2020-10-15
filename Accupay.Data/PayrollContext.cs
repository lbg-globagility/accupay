using AccuPay.Data.Data.EntityFrameworkCore;
using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AccuPay.Data
{
    public class PayrollContext
        : IdentityDbContext<
            AspNetUser,
            AspNetRole,
            int,
            UserClaim,
            UserRole,
            UserLogin,
            RoleClaim,
            UserToken>
    {
        internal virtual DbSet<ActualAdjustment> ActualAdjustments { get; set; }
        internal virtual DbSet<ActualTimeEntry> ActualTimeEntries { get; set; }
        internal virtual DbSet<Address> Addresses { get; set; }
        internal virtual DbSet<Adjustment> Adjustments { get; set; }
        internal virtual DbSet<Agency> Agencies { get; set; }
        internal virtual DbSet<AgencyFee> AgencyFees { get; set; }
        internal virtual DbSet<Allowance> Allowances { get; set; }
        internal virtual DbSet<AllowanceItem> AllowanceItems { get; set; }
        internal virtual DbSet<AllowanceType> AllowanceTypes { get; set; }
        internal virtual DbSet<Attachment> Attachments { get; set; }
        internal virtual DbSet<Award> Awards { get; set; }
        internal virtual DbSet<Bonus> Bonuses { get; set; }
        internal virtual DbSet<BreakTimeBracket> BreakTimeBrackets { get; set; }
        internal virtual DbSet<Branch> Branches { get; set; }
        internal virtual DbSet<PayCalendar> Calendars { get; set; }
        internal virtual DbSet<CalendarDay> CalendarDays { get; set; }
        internal virtual DbSet<Category> Categories { get; set; }
        internal virtual DbSet<Certification> Certifications { get; set; }
        internal virtual DbSet<Client> Clients { get; set; }
        internal virtual DbSet<DayType> DayTypes { get; set; }
        internal virtual DbSet<DisciplinaryAction> DisciplinaryActions { get; set; }
        internal virtual DbSet<Division> Divisions { get; set; }
        internal virtual DbSet<DivisionMinimumWage> DivisionMinimumWages { get; set; }
        internal virtual DbSet<EducationalBackground> EducationalBackgrounds { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        internal virtual DbSet<EmployeeDutySchedule> EmployeeDutySchedules { get; set; }
        internal virtual DbSet<EmploymentPolicy> EmploymentPolicies { get; set; }
        internal virtual DbSet<EmploymentPolicyType> EmploymentPolicyTypes { get; set; }
        internal virtual DbSet<File> Files { get; set; }
        internal virtual DbSet<FilingStatusType> FilingStatusTypes { get; set; }
        internal virtual DbSet<JobCategory> JobCategories { get; set; }
        internal virtual DbSet<JobLevel> JobLevels { get; set; }
        internal virtual DbSet<Leave> Leaves { get; set; }
        internal virtual DbSet<LeaveLedger> LeaveLedgers { get; set; }
        internal virtual DbSet<LeaveTransaction> LeaveTransactions { get; set; }
        internal virtual DbSet<ListOfValue> ListOfValues { get; set; }
        internal virtual DbSet<LoanPaymentFromBonus> LoanPaymentFromBonuses { get; set; }
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
        internal virtual DbSet<Permission> Permissions { get; set; }
        internal virtual DbSet<PhilHealthBracket> PhilHealthBrackets { get; set; }
        internal virtual DbSet<Position> Positions { get; set; }
        internal virtual DbSet<PreviousEmployer> PreviousEmployers { get; set; }
        internal virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<RoutePayRate> RoutePayRates { get; set; }
        internal virtual DbSet<Salary> Salaries { get; set; }
        internal virtual DbSet<SocialSecurityBracket> SocialSecurityBrackets { get; set; }
        internal virtual DbSet<SystemOwner> SystemOwners { get; set; }
        internal virtual DbSet<TardinessRecord> TardinessRecords { get; set; }
        internal virtual DbSet<TimeEntry> TimeEntries { get; set; }
        internal virtual DbSet<TimeAttendanceLog> TimeAttendanceLogs { get; set; }
        internal virtual DbSet<TimeLog> TimeLogs { get; set; }
        public virtual DbSet<TripTicket> TripTickets { get; set; }
        public virtual DbSet<TripTicketEmployee> TripTicketEmployees { get; set; }
        internal virtual DbSet<UserActivity> UserActivities { get; set; }
        internal virtual DbSet<UserActivityItem> UserActivityItems { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        internal virtual DbSet<ThirteenthMonthPay> ThirteenthMonthPays { get; set; }
        internal virtual DbSet<WithholdingTaxBracket> WithholdingTaxBrackets { get; set; }

        public PayrollContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<Organization>()
                .Property(x => x.IsInActive)
                .IsUnicode(false)
                .HasConversion(typeof(string));

            modelBuilder.Entity<PayPeriod>()
                .Property(t => t.Status)
                .HasConversion(new EnumToStringConverter<PayPeriodStatus>());

            modelBuilder.Entity<AspNetUser>(b =>
            {
                b.ToTable("AspNetUsers");
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);
                b.Property(t => t.Status).HasConversion(new EnumToStringConverter<AspNetUserStatus>());
            });

            modelBuilder.Entity<AspNetRole>(b =>
            {
                b.ToTable("AspNetRoles");
                b.HasKey(r => r.Id);
                b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);
            });
            modelBuilder.Entity<PayPeriod>()
                .Property(t => t.Status)
                .HasConversion(new EnumToStringConverter<PayPeriodStatus>());

            modelBuilder.Entity<UserRole>(b =>
            {
                b.ToTable("AspNetUserRoles");
                b.HasKey(t => new { t.UserId, t.RoleId, t.OrganizationId });
            });

            modelBuilder.Entity<UserClaim>(b =>
            {
                b.ToTable("AspNetUserClaims");
                b.HasKey(uc => uc.Id);
            });

            modelBuilder.Entity<RoleClaim>(b =>
            {
                b.ToTable("AspNetRoleClaims");
                b.HasKey(rc => rc.Id);
            });

            modelBuilder.Entity<UserLogin>(b =>
            {
                b.ToTable("AspNetUserLogins");
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            });

            modelBuilder.Entity<UserToken>(b =>
            {
                b.ToTable("AspNetUserTokens");
                b.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });
            });

            modelBuilder.Entity<RolePermission>()
                .HasOne(t => t.Role)
                .WithMany(t => t.RolePermissions)
                .HasForeignKey(t => t.RoleId);
        }
    }
}