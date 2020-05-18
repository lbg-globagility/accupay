using AccuPay.Data;
using AccuPay.Data.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace AccuPay.Web
{
    public static class DatabaseServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddDbContext<PayrollContext>(options =>
            {
                ConfigureDbContextOptions(configuration, options, env);
            });

            // passing DbContextOptions for services that needs to directly access PayrollContext.
            // It is prohibited to access PayrollContext unless there is no other choice
            // like in the case of services accessed in multiple threads.
            services.AddSingleton(serviceProvider =>
            {
                var options = GetDbContextOptions(configuration, env);
                return new DbContextOptionsService(options);
            });

            return services;
        }

        private static DbContextOptions GetDbContextOptions(IConfiguration configuration, IWebHostEnvironment env)
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            ConfigureDbContextOptions(configuration, builder, env);

            return builder.Options;
        }

        private static void ConfigureDbContextOptions(IConfiguration configuration, DbContextOptionsBuilder dbContextOptionsBuilder, IWebHostEnvironment env)
        {
            dbContextOptionsBuilder
                .UseMySql(configuration.GetConnectionString("accupaydb"))
                .EnableSensitiveDataLogging();

            if (env.IsDevelopment())
            {
                dbContextOptionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            }
        }
    }
}
