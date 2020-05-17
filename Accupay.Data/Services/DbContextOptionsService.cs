using Microsoft.EntityFrameworkCore;

namespace AccuPay.Data.Services
{
    public class DbContextOptionsService
    {
        public DbContextOptions DbContextOptions { get; set; }

        public DbContextOptionsService(DbContextOptions dbContextOptions)
        {
            DbContextOptions = dbContextOptions;
        }
    }
}