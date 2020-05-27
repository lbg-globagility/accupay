using Accupay.Web.Core.Configurations;
using AccuPay.Web.Account;
using AccuPay.Web.Allowances.Services;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Employees.Services;
using AccuPay.Web.Leaves;
using AccuPay.Web.Loans;
using AccuPay.Web.OfficialBusinesses;
using AccuPay.Web.Organizations;
using AccuPay.Web.Overtimes;
using AccuPay.Web.Salaries.Services;
using AccuPay.Web.Shifts.Services;
using AccuPay.Web.Users;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<AllowanceService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<LeaveService>();
            services.AddScoped<LoanService>();
            services.AddScoped<OfficialBusinessService>();
            services.AddScoped<OvertimeService>();
            services.AddScoped<OrganizationService>();
            services.AddScoped<SalaryService>();
            services.AddScoped<ShiftService>();

            services.AddScoped<JwtConfiguration>();

            services.AddScoped<ICurrentUser, CurrentUser>();

            return services;
        }
    }
}
