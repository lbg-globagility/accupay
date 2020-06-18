using AccuPay.CrystalReports.Payslip;
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
                    .UseMySql(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString)
                    .EnableSensitiveDataLogging();
            });

            services.AddScoped<EmailService>();
            services.AddScoped<PaystubEmailRepository>();

            services.AddScoped<PayPeriodRepository>();
            services.AddScoped<OrganizationRepository>();
            services.AddScoped<AddressRepository>();
            services.AddScoped<SystemOwnerService>();
            services.AddScoped<PayslipCreator>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}