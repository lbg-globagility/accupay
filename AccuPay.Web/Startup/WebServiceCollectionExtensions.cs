using AccuPay.Web.Employees.Services;
using AccuPay.Web.Users;
using Microsoft.Extensions.DependencyInjection;

namespace AccuPay.Web
{
    public static class WebServiceCollectionExtensions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>()
                .AddScoped<EmployeeService>();

            return services;
        }
    }
}
