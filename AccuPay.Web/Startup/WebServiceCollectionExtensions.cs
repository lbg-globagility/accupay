using AccuPay.Web.Account;
using AccuPay.Web.Allowances.Services;
using AccuPay.Web.Branches;
using AccuPay.Web.Calendars;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Configurations;
using AccuPay.Web.Core.Emails;
using AccuPay.Web.Core.Views;
using AccuPay.Web.Divisions;
using AccuPay.Web.Employees.Services;
using AccuPay.Web.Leaves;
using AccuPay.Web.Loans;
using AccuPay.Web.OfficialBusinesses;
using AccuPay.Web.Organizations;
using AccuPay.Web.Overtimes;
using AccuPay.Web.Payroll;
using AccuPay.Web.Positions;
using AccuPay.Web.Salaries.Services;
using AccuPay.Web.Shifts.Services;
using AccuPay.Web.Users;
using Microsoft.Extensions.DependencyInjection;
using Notisphere.Users.Services;

namespace AccuPay.Web
{
    public static class WebServiceCollectionExtensions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<AccountService>();
            services.AddScoped<AccountTokenService>();
            services.AddScoped<CalendarService>();
            services.AddScoped<BranchService>();
            services.AddScoped<TokenService>();
            services.AddScoped<AllowanceService>();
            services.AddScoped<DivisionService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<LeaveService>();
            services.AddScoped<LoanService>();
            services.AddScoped<OfficialBusinessService>();
            services.AddScoped<OvertimeService>();
            services.AddScoped<OrganizationService>();
            services.AddScoped<PositionService>();
            services.AddScoped<SalaryService>();
            services.AddScoped<ShiftService>();
            services.AddScoped<PayperiodService>();

            services.AddScoped<JwtConfiguration>();
            services.AddScoped<EmailConfiguration>();

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddScoped<EmailService>();
            services.AddScoped<ViewRenderService>();
            services.AddScoped<UserEmailService>();
            services.AddScoped<UserTokenService>();

            return services;
        }
    }
}
