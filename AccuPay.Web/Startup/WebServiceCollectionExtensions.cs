using AccuPay.Web.Account;
using AccuPay.Web.Allowances.Services;
using AccuPay.Web.AllowanceType;
using AccuPay.Web.Branches;
using AccuPay.Web.Calendars;
using AccuPay.Web.Clients;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Configurations;
using AccuPay.Web.Core.Emails;
using AccuPay.Web.Core.Views;
using AccuPay.Web.Divisions;
using AccuPay.Web.Employees.Services;
using AccuPay.Web.EmploymentPolicies.Services;
using AccuPay.Web.Files.Services;
using AccuPay.Web.Leaves;
using AccuPay.Web.Loans;
using AccuPay.Web.Loans.LoanType;
using AccuPay.Web.OfficialBusinesses;
using AccuPay.Web.Organizations;
using AccuPay.Web.Overtimes;
using AccuPay.Web.Payroll;
using AccuPay.Web.Positions;
using AccuPay.Web.Reports;
using AccuPay.Web.Salaries.Services;
using AccuPay.Web.Shifts.Services;
using AccuPay.Web.TimeEntries;
using AccuPay.Web.TimeLogs;
using AccuPay.Web.Users;
using AccuPay.Web.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AccuPay.Web
{
    public static class WebServiceCollectionExtensions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<TokenService>();
            services.AddScoped<UserService>();
            services.AddScoped<AccountService>();
            services.AddScoped<AccountTokenService>();
            services.AddScoped<AllowanceService>();
            services.AddScoped<AllowanceTypeService>();
            services.AddScoped<BranchService>();
            services.AddScoped<CalendarService>();
            services.AddScoped<ClientService>();
            services.AddScoped<DivisionService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<EmploymentPolicyService>();
            services.AddScoped<LeaveService>();
            services.AddScoped<LoanService>();
            services.AddScoped<LoanTypeService>();
            services.AddScoped<RoleService>();
            services.AddScoped<OfficialBusinessService>();
            services.AddScoped<OvertimeService>();
            services.AddScoped<OrganizationService>();
            services.AddScoped<PermissionService>();
            services.AddScoped<PositionService>();
            services.AddScoped<ReportService>();
            services.AddScoped<SalaryService>();
            services.AddScoped<ShiftService>();
            services.AddScoped<TimeLogService>();
            services.AddScoped<PayperiodService>();
            services.AddScoped<PaystubService>();
            services.AddScoped<TimeEntryService>();

            services.AddScoped<JwtConfiguration>();
            services.AddScoped<EmailConfiguration>();

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddScoped<EmailService>();
            services.AddScoped<ViewRenderService>();
            services.AddScoped<UserEmailService>();
            services.AddScoped<UserTokenService>();

            services.AddScoped<GenerateDefaultImageService>();
            services.AddScoped<GenerateDefaultUserImageService>();

            return services;
        }
    }
}
