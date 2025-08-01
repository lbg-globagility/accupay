using AccuPay.Core.Entities;
using AccuPay.Core.Entities.LeaveReset;
using AccuPay.Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq.Expressions;

namespace AccuPay.Infrastructure.Data
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
        internal virtual DbSet<BankFileHeader> BankFileHeaders { get; set; }
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
        internal virtual DbSet<EducationalBackground> EducationalBackgrounds { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        internal virtual DbSet<Shift> Shifts { get; set; }
        internal virtual DbSet<EmploymentPolicy> EmploymentPolicies { get; set; }
        internal virtual DbSet<EmploymentPolicyType> EmploymentPolicyTypes { get; set; }
        internal virtual DbSet<File> Files { get; set; }
        internal virtual DbSet<JobCategory> JobCategories { get; set; }
        internal virtual DbSet<JobLevel> JobLevels { get; set; }
        internal virtual DbSet<Leave> Leaves { get; set; }
        internal virtual DbSet<LeaveLedger> LeaveLedgers { get; set; }
        internal virtual DbSet<LeaveTransaction> LeaveTransactions { get; set; }
        internal virtual DbSet<ListOfValue> ListOfValues { get; set; }
        internal virtual DbSet<LoanPaymentFromBonus> LoanPaymentFromBonuses { get; set; }
        internal virtual DbSet<LoanPaymentFromThirteenthMonthPay> LoanPaymentFromThirteenthMonthPays { get; set; }
        internal virtual DbSet<Loan> Loans { get; set; }
        internal virtual DbSet<LoanTransaction> LoanTransactions { get; set; }
        internal virtual DbSet<OfficialBusiness> OfficialBusinesses { get; set; }
        internal virtual DbSet<Organization> Organizations { get; set; }
        internal virtual DbSet<Overtime> Overtimes { get; set; }
        internal virtual DbSet<PayFrequency> PayFrequencies { get; set; }
        internal virtual DbSet<PayPeriod> PayPeriods { get; set; }
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
        internal virtual DbSet<SystemInfo> SystemInfo { get; set; }
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
        internal virtual DbSet<LeaveReset> LeaveResets { get; set; }
        internal virtual DbSet<LeaveTenure> LeaveTenures { get; set; }
        internal virtual DbSet<LeaveTypeRenewable> LeaveTypeRenewables { get; set; }
        internal virtual DbSet<CashoutUnusedLeave> CashoutUnusedLeaves { get; set; }
        internal virtual DbSet<ResetLeaveCredit> ResetLeaveCredits { get; set; }
        internal virtual DbSet<ResetLeaveCreditItem> ResetLeaveCreditItems { get; set; }

        public PayrollContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            SetGeneratedColumnsToReadOnly(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasMany(x => x.Products)
                .WithOne(x => x.CategoryEntity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Organization>()
                .HasMany(x => x.Categories)
                .WithOne(x => x.Organization)
                .OnDelete(DeleteBehavior.Cascade);

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
            modelBuilder.Entity<Paystub>()
                .HasMany(x => x.LeaveTransactions)
                .WithOne(x => x.Paystub);

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

            modelBuilder.Entity<SystemInfo>(
            b =>
            {
                b.HasKey(e => e.Name);
                b.Property(e => e.Value);
            });

            modelBuilder.Entity<YearlyLoanInterest>(
            b =>
            {
                b.HasKey(x => new { x.LoanId, x.Year });

                b.HasOne(x => x.Loan)
                    .WithMany(l => l.YearlyLoanInterests)
                    .HasForeignKey(x => x.LoanId);
            });

            modelBuilder.Entity<LeaveTenure>(
            b =>
            {
                b.HasKey(x => new { x.LeaveResetId, x.OrdinalValue });

                b.HasOne(x => x.LeaveReset).
                    WithMany(x => x.LeaveTenures).
                    HasForeignKey(x => x.LeaveResetId);
            });

            modelBuilder.Entity<LeaveTypeRenewable>(
            b =>
            {
                b.HasKey(x => new { x.LeaveResetId, x.LeaveTypeId });

                b.HasOne(x => x.LeaveReset).
                    WithMany(x => x.LeaveTypeRenewables).
                    HasForeignKey(x => x.LeaveResetId);

                b.HasOne(x => x.Product).
                    WithMany(x => x.LeaveTypeRenewables).
                    HasForeignKey(x => x.LeaveTypeId);

                b.Property(x => x.BasisStartDate).
                    HasConversion<string>();
            });

            modelBuilder.Entity<CashoutUnusedLeave>(t => {
                t.HasKey(x => x.Id);

                t.HasOne(c => c.LeaveLedger).
                    WithMany(l => l.CashoutUnusedLeaves).
                    HasForeignKey(c => c.LeaveLedgerID);

                t.HasOne(c => c.PayPeriod).
                    WithMany(p => p.CashoutUnusedLeaves).
                    HasForeignKey(c => c.PayPeriodID);
            });

            modelBuilder.Entity<ResetLeaveCredit>(t => {
                t.HasKey(x => x.RowID);

                t.HasOne(x => x.PayPeriod)
                    .WithMany(p => p.ResetLeaveCredits)
                    .HasForeignKey(r => r.StartPeriodId);

                t.HasMany(x => x.ResetLeaveCreditItems)
                    .WithOne(r => r.ResetLeaveCredit)
                    .HasForeignKey(x => x.ResetLeaveCreditId);

            });

            modelBuilder.Entity<ResetLeaveCreditItem>(t => {
                t.HasKey(x => x.RowID);

                t.HasOne(x => x.Employee)
                    .WithMany(e => e.ResetLeaveCreditItems)
                    .HasForeignKey(x => x.EmployeeID);
            });
        }

        private static void SetGeneratedColumnsToReadOnly(ModelBuilder modelBuilder)
        {
            var created = GetPropertyName<AuditableEntity>(x => x.Created);
            var lastUpd = GetPropertyName<AuditableEntity>(x => x.LastUpd);
            var createdBy = GetPropertyName<AuditableEntity>(x => x.CreatedBy);
            var lastUpdBy = GetPropertyName<AuditableEntity>(x => x.LastUpdBy);

            foreach (var t in modelBuilder.Model.GetEntityTypes())
            {
                if (CheckIfDerivableByAuditableEntity(t.ClrType.BaseType))
                {
                    // Even if the LastUpd and Created columns are private
                    // they are still included in the update query.
                    // If we strictly want them to be never be altered by
                    // ef core and only the database can update them,
                    // we can use the code below:
                    var createdMetaData = modelBuilder
                        .Entity(t.ClrType)
                        .Property(created)
                        .ValueGeneratedOnAddOrUpdate()
                        .Metadata;

                    createdMetaData.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
                    createdMetaData.AfterSaveBehavior = PropertySaveBehavior.Ignore;

                    var lastupdMetaData = modelBuilder
                        .Entity(t.ClrType)
                        .Property(lastUpd)
                        .ValueGeneratedOnAddOrUpdate()
                        .Metadata;

                    lastupdMetaData.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
                    lastupdMetaData.AfterSaveBehavior = PropertySaveBehavior.Ignore;

                    // CreatedBy can only be modified by ef core when EntityState is EntityState.Added
                    var createdByMetaData = modelBuilder
                        .Entity(t.ClrType)
                        .Property(createdBy)
                        .ValueGeneratedOnAddOrUpdate()
                        .Metadata;

                    createdByMetaData.BeforeSaveBehavior = PropertySaveBehavior.Save;
                    createdByMetaData.AfterSaveBehavior = PropertySaveBehavior.Ignore;

                    // LastUpdBy can only be modified by ef core when EntityState is not EntityState.Added
                    var lastUpdByMetaData = modelBuilder
                        .Entity(t.ClrType)
                        .Property(lastUpdBy)
                        .ValueGeneratedOnAddOrUpdate()
                        .Metadata;

                    lastUpdByMetaData.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
                    lastUpdByMetaData.AfterSaveBehavior = PropertySaveBehavior.Save;
                }
            }
        }

        private static bool CheckIfDerivableByAuditableEntity(Type baseType)
        {
            while (baseType != null)
            {
                if (baseType == typeof(AuditableEntity))
                {
                    return true;
                }
                else
                {
                    baseType = baseType?.BaseType;
                }
            }

            return false;
        }

        private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                return ((MemberExpression)expression.Body).Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }
    }
}
