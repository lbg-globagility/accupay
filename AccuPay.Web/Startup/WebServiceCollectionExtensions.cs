using Accupay.Web.Core.Configurations;
using AccuPay.Web.Account;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Employees.Services;
using AccuPay.Web.Leaves;
using AccuPay.Web.Loans;
using AccuPay.Web.OfficialBusinesses;
using AccuPay.Web.Overtimes;
using AccuPay.Web.Users;
using Microsoft.Extensions.DependencyInjection;
using Accupay.Web.Core.Auth;

namespace AccuPay.Web
{
    public static class WebServiceCollectionExtensions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<AccountService>();
            services.AddScoped<AccountTokenService>();
            services.AddScoped<TokenService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<CurrentUser>();
            services.AddScoped<LeaveService>();
            services.AddScoped<LoanService>();
            services.AddScoped<OfficialBusinessService>();
            services.AddScoped<OvertimeService>();

            services.AddScoped<JwtConfiguration>();

            return services;
        }
    }
}
