using AccuPay.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccuPay.Web
{
    public static class DatabaseServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PayrollContext>(options =>
            {
                ConfigureDbContextOptions(configuration, options);
            });

            return services;
        }

        private static void ConfigureDbContextOptions(IConfiguration configuration, DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder
                .UseMySql(configuration.GetConnectionString("accupaydb"))
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
