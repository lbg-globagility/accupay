using AccuPay.Data.Interfaces;
using AccuPay.Data.Interfaces.Excel;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports;
using AccuPay.Data.Services.Imports.Allowances;
using AccuPay.Data.Services.Imports.Employees;
using AccuPay.Data.Services.Imports.Loans;
using AccuPay.Data.Services.Imports.OfficialBusiness;
using AccuPay.Data.Services.Imports.Overtimes;
using AccuPay.Data.Services.Imports.Salaries;
using AccuPay.Data.Services.Reports;
using AccuPay.Infrastructure.Reports;
using AccuPay.Infrastructure.Services.Encryption;
using AccuPay.Infrastructure.Services.Excel;
using Microsoft.Extensions.DependencyInjection;

namespace AccuPay.Web
{
    public static class AccuPayCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddAccuPayCoreServices(this IServiceCollection services)
        {
            services.AddScoped<ActualTimeEntryRepository>();
            services.AddScoped<AgencyFeeRepository>();
            services.AddScoped<AgencyRepository>();
            services.AddScoped<AllowanceRepository>();
            services.AddScoped<AllowanceTypeRepository>();
            services.AddScoped<AspNetUserRepository>();
            services.AddScoped<AttachmentRepository>();
            services.AddScoped<AwardRepository>();
            services.AddScoped<BonusRepository>();
            services.AddScoped<BranchRepository>();
            services.AddScoped<BreakTimeBracketRepository>();
            services.AddScoped<CalendarRepository>();
            services.AddScoped<CategoryRepository>();
            services.AddScoped<CertificationRepository>();
            services.AddScoped<ClientRepository>();
            services.AddScoped<DayTypeRepository>();
            services.AddScoped<DisciplinaryActionRepository>();
            services.AddScoped<DivisionMinimumWageRepository>();
            services.AddScoped<DivisionRepository>();
            services.AddScoped<EducationalBackgroundRepository>();
            services.AddScoped<EmployeeDutyScheduleRepository>();
            services.AddScoped<EmployeeQueryBuilder>();
            services.AddScoped<EmployeeRepository>();
            services.AddScoped<EmploymentPolicyRepository>();
            services.AddScoped<FileRepository>();
            services.AddScoped<JobCategoryRepository>();
            services.AddScoped<JobLevelRepository>();
            services.AddScoped<LeaveRepository>();
            services.AddScoped<LeaveLedgerRepository>();
            services.AddScoped<ListOfValueRepository>();
            services.AddScoped<LoanRepository>();
            services.AddScoped<OfficialBusinessRepository>();
            services.AddScoped<OrganizationRepository>();
            services.AddScoped<OvertimeRepository>();
            services.AddScoped<PayFrequencyRepository>();
            services.AddScoped<PayPeriodRepository>();
            services.AddScoped<PaystubActualRepository>();
            services.AddScoped<PaystubEmailHistoryRepository>();
            services.AddScoped<PaystubEmailRepository>();
            services.AddScoped<PaystubRepository>();
            services.AddScoped<PermissionRepository>();
            services.AddScoped<PhilHealthBracketRepository>();
            services.AddScoped<PositionRepository>();
            services.AddScoped<PreviousEmployerRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<RouteRateRepository>();
            services.AddScoped<SalaryRepository>();
            services.AddScoped<SocialSecurityBracketRepository>();
            services.AddScoped<TimeAttendanceLogRepository>();
            services.AddScoped<TimeEntryRepository>();
            services.AddScoped<TimeLogRepository>();
            services.AddScoped<TripTicketRepository>();
            services.AddScoped<UserActivityRepository>();
            services.AddScoped<WithholdingTaxBracketRepository>();

            services.AddScoped<CalendarService>();
            services.AddScoped<ListOfValueService>();

            services.AddScoped<PayrollGeneration>();
            services.AddScoped<PayrollResources>();

            services.AddScoped<PolicyHelper>();

            services.AddScoped<BpiInsuranceAmountReportDataService>();
            services.AddScoped<CinemaTardinessReportDataService>();
            services.AddScoped<CostCenterReportDataService>();
            services.AddScoped<FiledLeaveReportDataService>();
            services.AddScoped<LeaveLedgerReportDataService>();
            services.AddScoped<PaystubPayslipModelDataService>();

            services.AddScoped<TimeEntryGenerator>();

            services.AddScoped<OvertimeRateService>();
            services.AddScoped<PayPeriodDataService>();
            services.AddScoped<SystemOwnerService>();

            services.AddScoped<AllowanceDataService>();
            services.AddScoped<CalendarDataService>();
            services.AddScoped<DivisionDataService>();
            services.AddScoped<EmployeeDataService>();
            services.AddScoped<EmployeeDutyScheduleDataService>();
            services.AddScoped<LeaveDataService>();
            services.AddScoped<LoanDataService>();
            services.AddScoped<OfficialBusinessDataService>();
            services.AddScoped<OrganizationDataService>();
            services.AddScoped<OvertimeDataService>();
            services.AddScoped<PaystubDataService>();
            services.AddScoped<PositionDataService>();
            services.AddScoped<ProductDataService>();
            services.AddScoped<RoleDataService>();
            services.AddScoped<SalaryDataService>();
            services.AddScoped<TimeEntryDataService>();
            services.AddScoped<TimeLogDataService>();
            services.AddScoped<UserDataService>();

            services.AddScoped<AllowanceImportParser>();
            services.AddScoped<EmployeeImportParser>();
            services.AddScoped<LoanImportParser>();
            services.AddScoped<OfficialBusinessImportParser>();
            services.AddScoped<OvertimeImportParser>();
            services.AddScoped<SalaryImportParser>();
            services.AddScoped<ShiftImportParser>();
            services.AddScoped<TimeLogImportParser>();
            services.AddScoped<TimeLogsReader>();

            services.AddScoped(typeof(IExcelParser<>), typeof(ExcelParser<>));
            services.AddScoped<IEncryption, AccuPayDesktopEncryption>();

            services.AddScoped<PayrollSummaryExcelFormatReportDataService>();
            services.AddScoped<IPayrollSummaryReportBuilder, PayrollSummaryReportBuilder>();
            services.AddScoped<EmployeePersonalProfilesExcelFormatReportDataService>();
            services.AddScoped<IEmployeePersonalProfilesReportBuilder, EmployeePersonalProfilesReportBuilder>();

            return services;
        }
    }
}
