using AccuPay.CrystalReports;
using AccuPay.Data;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.ServiceProcess;

namespace AccupayWindowsService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceProvider serviceProvider = BuildServiceProvider();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                serviceProvider.GetRequiredService<EmailService>()
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddDbContext<PayrollContext>(options =>
            {
                options
                    .UseMySql(ConfigurationManager.ConnectionStrings["accupaydb"].ConnectionString)
                    .EnableSensitiveDataLogging();
            });

            services.AddScoped<EmailService>();

            services.AddScoped<PolicyHelper>();
            services.AddScoped<ListOfValueService>();

            services.AddScoped<OrganizationRepository>();
            services.AddScoped<PaystubEmailRepository>();
            services.AddScoped<PayPeriodRepository>();
            services.AddScoped<PaystubEmailRepository>();
            services.AddScoped<PaystubEmailHistoryRepository>();

            services.AddScoped<PaystubEmailDataService>();
            services.AddScoped<PayslipDataService>();
            services.AddScoped<SystemOwnerService>();
            services.AddScoped<PayslipBuilder>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}