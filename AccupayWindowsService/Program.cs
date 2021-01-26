using AccuPay.Core.Interfaces;
using AccuPay.CrystalReports;
using AccuPay.Infrastructure.Data;
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

            services.AddScoped<IPolicyHelper, PolicyHelper>();
            services.AddScoped<IListOfValueService, ListOfValueService>();

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPaystubEmailRepository, PaystubEmailRepository>();
            services.AddScoped<IPayPeriodRepository, PayPeriodRepository>();
            services.AddScoped<IPaystubEmailRepository, PaystubEmailRepository>();
            services.AddScoped<IPaystubEmailHistoryRepository, PaystubEmailHistoryRepository>();

            services.AddScoped<IPaystubEmailDataService, PaystubEmailDataService>();
            services.AddScoped<IPayslipDataService, PayslipDataService>();
            services.AddScoped<ISystemOwnerService, SystemOwnerService>();
            services.AddScoped<IPayslipBuilder, PayslipBuilder>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
