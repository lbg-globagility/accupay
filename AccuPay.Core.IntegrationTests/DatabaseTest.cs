using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Excel;
using AccuPay.Core.Services;
using AccuPay.Core.Services.Imports;
using AccuPay.Core.Services.Imports.Allowances;
using AccuPay.Core.Services.Imports.Employees;
using AccuPay.Core.Services.Imports.Loans;
using AccuPay.Core.Services.Imports.OfficialBusiness;
using AccuPay.Core.Services.Imports.Overtimes;
using AccuPay.Core.Services.Imports.Salaries;
using AccuPay.Infrastructure.Data;
using AccuPay.Infrastructure.Reports;
using AccuPay.Infrastructure.Services.Encryption;
using AccuPay.Infrastructure.Services.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccuPay.Core.IntegrationTests
{
    public abstract class DatabaseTest
    {
        public DatabaseTest()
        {
            ConfigureDependencyInjection();
        }

        public ServiceProvider MainServiceProvider { get; private set; }

        private void ConfigureDependencyInjection()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            MainServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<PayrollContext>(options =>
            {
                options.UseMySql("server=localhost;userid=root;password=globagility;database=accupaydb_cinema2k;")
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IActualTimeEntryRepository, ActualTimeEntryRepository>();
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
            services.AddScoped<DivisionRepository>();
            services.AddScoped<EducationalBackgroundRepository>();
            services.AddScoped<ShiftRepository>();
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

            services.AddScoped<PayrollGenerator>();
            services.AddScoped<PayrollResources>();

            services.AddScoped<IPolicyHelper, PolicyHelper>();

            services.AddScoped<BpiInsuranceAmountReportDataService>();
            services.AddScoped<CinemaTardinessReportDataService>();
            services.AddScoped<CostCenterReportDataService>();
            services.AddScoped<FiledLeaveReportDataService>();
            services.AddScoped<LeaveLedgerReportDataService>();
            services.AddScoped<PaystubPayslipModelDataService>();

            services.AddScoped<TimeEntryGenerator>();
            services.AddScoped<TimeEntryResources>();

            services.AddScoped<OvertimeRateService>();
            services.AddScoped<PayPeriodDataService>();
            services.AddScoped<SystemOwnerService>();

            services.AddScoped<AllowanceDataService>();
            services.AddScoped<CalendarDataService>();
            services.AddScoped<DivisionDataService>();
            services.AddScoped<EmployeeDataService>();
            services.AddScoped<ShiftDataService>();
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
            services.AddScoped<TripTicketDataService>();

            services.AddScoped(typeof(IExcelParser<>), typeof(ExcelParser<>));
            services.AddScoped<IEncryption, AccuPayDesktopEncryption>();

            services.AddScoped<PayrollSummaryExcelFormatReportDataService>();
            services.AddScoped<IPayrollSummaryReportBuilder, PayrollSummaryReportBuilder>();
            services.AddScoped<EmployeePersonalProfilesExcelFormatReportDataService>();
            services.AddScoped<IEmployeePersonalProfilesReportBuilder, EmployeePersonalProfilesReportBuilder>();
        }
    }
}
