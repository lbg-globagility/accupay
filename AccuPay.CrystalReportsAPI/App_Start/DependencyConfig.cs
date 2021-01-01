using AccuPay.CrystalReports;
using AccuPay.CrystalReportsAPI.Services;
using AccuPay.Core;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace AccuPay.CrystalReportsAPI
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

            services.AddClassesFromAssembly(typeof(DependencyConfig).Assembly.GetExportedTypes()
            .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
            .Where(t => typeof(IHttpController).IsAssignableFrom(t)
                        || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                        || typeof(IControllerService).IsAssignableFrom(t)));

            services.AddDbContext<PayrollContext>(options =>
            {
                options
                    .UseMySql(ConfigurationManager.ConnectionStrings["accupaydb"].ConnectionString)
                        .EnableSensitiveDataLogging()
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<OrganizationRepository>();
            services.AddScoped<ListOfValueService>();
            services.AddScoped<IPolicyHelper, PolicyHelper>();
            services.AddScoped<PayPeriodRepository>();
            services.AddScoped<PayslipDataService>();
            services.AddScoped<SystemOwnerService>();
            services.AddScoped<PayslipBuilder>();

            services.AddScoped<SSSMonthlyReportDataService>();
            services.AddScoped<SSSMonthyReportBuilder>();
            services.AddScoped<PhilHealthMonthlyReportDataService>();
            services.AddScoped<PhilHealthMonthlyReportBuilder>();
            services.AddScoped<PagIBIGMonthlyReportDataService>();
            services.AddScoped<PagIBIGMonthlyReportBuilder>();
            services.AddScoped<LoanSummaryByTypeReportDataService>();
            services.AddScoped<LoanSummaryByTypeReportBuilder>();
            services.AddScoped<LoanSummaryByEmployeeReportDataService>();
            services.AddScoped<LoanSummaryByEmployeeReportBuilder>();
            services.AddScoped<TaxMonthlyReportDataService>();
            services.AddScoped<TaxMonthlyReportBuilder>();
            services.AddScoped<ThirteenthMonthSummaryReportDataService>();
            services.AddScoped<ThirteenthMonthSummaryReportBuilder>();
            services.AddScoped<PaystubRepository>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }

    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddClassesFromAssembly(this IServiceCollection services,
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
