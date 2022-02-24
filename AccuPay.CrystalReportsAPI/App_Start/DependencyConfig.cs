using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Reports;
using AccuPay.Core.Interfaces.Repositories;
using AccuPay.CrystalReports;
using AccuPay.CrystalReportsAPI.Services;
using AccuPay.Infrastructure.Data;
using AccuPay.Infrastructure.Data.Reports;
using AccuPay.Infrastructure.Data.Repositories;
using AccuPay.Infrastructure.Reports;
using AccuPay.Infrastructure.Reports.Customize;
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

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IListOfValueService, ListOfValueService>();
            services.AddScoped<IPolicyHelper, PolicyHelper>();
            services.AddScoped<IPayPeriodRepository, PayPeriodRepository>();
            services.AddScoped<IPayslipDataService, PayslipDataService>();
            services.AddScoped<ISystemOwnerService, SystemOwnerService>();
            services.AddScoped<IPayslipBuilder, PayslipBuilder>();

            services.AddScoped<ISSSMonthlyReportDataService, SSSMonthlyReportDataService>();
            services.AddScoped<ISSSMonthyReportBuilder, SSSMonthyReportBuilder>();
            services.AddScoped<IPhilHealthMonthlyReportDataService, PhilHealthMonthlyReportDataService>();
            services.AddScoped<IPhilHealthMonthlyReportBuilder, PhilHealthMonthlyReportBuilder>();
            services.AddScoped<IPagIBIGMonthlyReportDataService, PagIBIGMonthlyReportDataService>();
            services.AddScoped<IPagIBIGMonthlyReportBuilder, PagIBIGMonthlyReportBuilder>();
            services.AddScoped<ILoanSummaryByTypeReportDataService, LoanSummaryByTypeReportDataService>();
            services.AddScoped<ILoanSummaryByTypeReportBuilder, LoanSummaryByTypeReportBuilder>();
            services.AddScoped<ILoanSummaryByEmployeeReportDataService, LoanSummaryByEmployeeReportDataService>();
            services.AddScoped<ILoanSummaryByEmployeeReportBuilder, LoanSummaryByEmployeeReportBuilder>();
            services.AddScoped<ITaxMonthlyReportDataService, TaxMonthlyReportDataService>();
            services.AddScoped<ITaxMonthlyReportBuilder, TaxMonthlyReportBuilder>();
            services.AddScoped<IThirteenthMonthSummaryReportDataService, ThirteenthMonthSummaryReportDataService>();
            services.AddScoped<IThirteenthMonthSummaryReportBuilder, ThirteenthMonthSummaryReportBuilder>();
            services.AddScoped<ILaGlobalAlphaListReportDataService, LaGlobalAlphaListReportDataService>();
            services.AddScoped<ILaGlobalAlphaListReportBuilder, LaGlobalAlphaListReportBuilder>();
            services.AddScoped<IAlphaListReportDataService, AlphaListReportDataService>();
            services.AddScoped<IAlphalistReportBuilder, AlphalistReportBuilder>();
            services.AddScoped<IPaystubRepository, PaystubRepository>();
            services.AddScoped<ICashoutUnusedLeaveRepository, CashoutUnusedLeaveRepository>();

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
