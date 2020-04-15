using Microsoft.EntityFrameworkCore;
using Accupay.DB;
using AccuPay.Data.Entities;

namespace AccuPay.Data
{
    internal class PayrollContext : DbContext
    {
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<PayPeriod> PayPeriods { get; set; }
        public virtual DbSet<Paystub> Paystubs { get; set; }
        public virtual DbSet<PaystubEmail> PaystubEmails { get; set; }
        public virtual DbSet<PaystubEmailHistory> PaystubEmailHistories { get; set; }
        internal virtual DbSet<SystemOwner> SystemOwners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(new DataBaseConnection().GetStringMySQLConnectionString());
            //UseLoggerFactory(_loggerFactory).
            //EnableSensitiveDataLogging()
        }
    }
}