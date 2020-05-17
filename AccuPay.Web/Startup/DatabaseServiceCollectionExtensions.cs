using AccuPay.Data;
using AccuPay.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            // passing DbContextOptions for services that needs to directly access PayrollContext.
            // It is prohibited to access PayrollContext unless there is no other choice
            // like in the case of services accessed in multiple threads.
            services.AddSingleton(serviceProvider =>
            {
                var options = GetDbContextOptions(configuration);
                return new DbContextOptionsService(options);
            });

            return services;
        }

        private static DbContextOptions GetDbContextOptions(IConfiguration configuration)
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            ConfigureDbContextOptions(configuration, builder);

            return builder.Options;
        }

        private static void ConfigureDbContextOptions(IConfiguration configuration, DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder
                .UseMySql(configuration.GetConnectionString("accupaydb"))
                .EnableSensitiveDataLogging();
        }
    }
}
