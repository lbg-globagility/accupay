using Accupay.DB;
using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccuPay.Data
{
    internal class PayrollContext : DbContext
    {
        //        public static readonly LoggerFactory DbCommandConsoleLoggerFactory = new LoggerFactory(
        //{
        //    new ConsoleLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information, true)
        //});

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AllowanceItem> AllowanceItems { get; set; }
        public virtual DbSet<Allowance> Allowances { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<PayPeriod> PayPeriods { get; set; }
        public virtual DbSet<Paystub> Paystubs { get; set; }
        public virtual DbSet<PaystubEmail> PaystubEmails { get; set; }
        public virtual DbSet<PaystubEmailHistory> PaystubEmailHistories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        internal virtual DbSet<SystemOwner> SystemOwners { get; set; }
        internal virtual DbSet<UserActivity> UserActivities { get; set; }
        internal virtual DbSet<UserActivityItem> UserActivityItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(new DataBaseConnection().GetStringMySQLConnectionString());
            //UseLoggerFactory(_loggerFactory).
            //EnableSensitiveDataLogging()
        }
    }
}