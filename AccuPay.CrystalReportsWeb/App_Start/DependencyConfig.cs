using AccuPay.CrystalReports.Payslip;
using AccuPay.CrystalReportsWeb.DependencyHelper;
using AccuPay.Data;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace AccuPay.CrystalReportsWeb
{
    public class DependencyConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var provider = ConfigureServiceProvider();

            var resolver = new DefaultDependencyResolver(provider);

            config.DependencyResolver = resolver;
        }

        private static IServiceProvider ConfigureServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddControllersAsServices(typeof(DependencyConfig).Assembly.GetExportedTypes()
            .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
            .Where(t => typeof(IHttpController).IsAssignableFrom(t)
                        || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));

            services.AddDbContext<PayrollContext>(options =>
            {
                options
                    .UseMySql(ConfigurationManager.ConnectionStrings["accupaydb"].ConnectionString)
                        .EnableSensitiveDataLogging()
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<AddressRepository>();
            services.AddScoped<OrganizationRepository>();
            services.AddScoped<PayPeriodRepository>();
            services.AddScoped<PayslipDataService>();
            services.AddScoped<SystemOwnerService>();
            services.AddScoped<PayslipCreator>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }

    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services,
            IEnumerable<Type> controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }
}