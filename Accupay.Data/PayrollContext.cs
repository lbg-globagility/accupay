using Microsoft.EntityFrameworkCore;
using Accupay.DB;
using Accupay.Data.Entities;

namespace Accupay.Data
{
    internal class PayrollContext : DbContext
    {
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