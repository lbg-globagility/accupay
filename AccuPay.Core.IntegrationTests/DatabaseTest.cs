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
using AccuPay.CrystalReports;
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
            services.AddScoped<IAgencyFeeRepository, AgencyFeeRepository>();
            services.AddScoped<IAgencyRepository, AgencyRepository>();
            services.AddScoped<IAllowanceRepository, AllowanceRepository>();
            services.AddScoped<IAllowanceTypeRepository, AllowanceTypeRepository>();
            services.AddScoped<IAspNetUserRepository, AspNetUserRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IAwardRepository, AwardRepository>();
            services.AddScoped<IBonusRepository, BonusRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IBreakTimeBracketRepository, BreakTimeBracketRepository>();
            services.AddScoped<ICalendarRepository, CalendarRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICertificationRepository, CertificationRepository>();
            services.AddScoped<IDayTypeRepository, DayTypeRepository>();
            services.AddScoped<IDisciplinaryActionRepository, DisciplinaryActionRepository>();
            services.AddScoped<IDivisionRepository, DivisionRepository>();
            services.AddScoped<IEducationalBackgroundRepository, EducationalBackgroundRepository>();
            services.AddScoped<IEmploymentPolicyRepository, EmploymentPolicyRepository>();
            services.AddScoped<IEmployeeQueryBuilder, EmployeeQueryBuilder>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IJobCategoryRepository, JobCategoryRepository>();
            services.AddScoped<IJobLevelRepository, JobLevelRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<ILeaveLedgerRepository, LeaveLedgerRepository>();
            services.AddScoped<IListOfValueRepository, ListOfValueRepository>();
            services.AddScoped<ILoanPaymentFromBonusRepository, LoanPaymentFromBonusRepository>();
            services.AddScoped<ILoanPaymentFromThirteenthMonthPayRepository, LoanPaymentFromThirteenthMonthPayRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IOfficialBusinessRepository, OfficialBusinessRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOvertimeRepository, OvertimeRepository>();
            services.AddScoped<IPayFrequencyRepository, PayFrequencyRepository>();
            services.AddScoped<IPayPeriodRepository, PayPeriodRepository>();
            services.AddScoped<IPaystubActualRepository, PaystubActualRepository>();
            services.AddScoped<IPaystubEmailHistoryRepository, PaystubEmailHistoryRepository>();
            services.AddScoped<IPaystubEmailRepository, PaystubEmailRepository>();
            services.AddScoped<IPaystubRepository, PaystubRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPhilHealthBracketRepository, PhilHealthBracketRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IPreviousEmployerRepository, PreviousEmployerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IRouteRateRepository, RouteRateRepository>();
            services.AddScoped<ISalaryRepository, SalaryRepository>();
            services.AddScoped<IShiftRepository, ShiftRepository>();
            services.AddScoped<ISocialSecurityBracketRepository, SocialSecurityBracketRepository>();
            services.AddScoped<ISystemInfoRepository, SystemInfoRepository>();
            services.AddScoped<ITimeAttendanceLogRepository, TimeAttendanceLogRepository>();
            services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
            services.AddScoped<ITimeLogRepository, TimeLogRepository>();
            services.AddScoped<ITripTicketRepository, TripTicketRepository>();
            services.AddScoped<IUserActivityRepository, UserActivityRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IWithholdingTaxBracketRepository, WithholdingTaxBracketRepository>();

            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IListOfValueService, ListOfValueService>();

            services.AddScoped<ICostCenterReportResources, CostCenterReportResources>();
            services.AddScoped<IPayrollGenerator, PayrollGenerator>();
            services.AddScoped<IPayrollResources, PayrollResources>();

            services.AddScoped<IPolicyHelper, PolicyHelper>();

            services.AddScoped<IBpiInsuranceAmountReportDataService, BpiInsuranceAmountReportDataService>();
            services.AddScoped<ICinemaTardinessReportDataService, CinemaTardinessReportDataService>();
            services.AddScoped<IFiledLeaveReportDataService, FiledLeaveReportDataService>();
            services.AddScoped<ILeaveLedgerReportDataService, LeaveLedgerReportDataService>();
            services.AddScoped<IPaystubPayslipModelDataService, PaystubPayslipModelDataService>();

            services.AddScoped<ITimeEntryGenerator, TimeEntryGenerator>();
            services.AddScoped<ITimeEntryResources, TimeEntryResources>();

            services.AddScoped<ILeaveAccrualService, LeaveAccrualService>();
            services.AddScoped<IOvertimeRateService, OvertimeRateService>();
            services.AddScoped<IPayPeriodDataService, PayPeriodDataService>();
            services.AddScoped<IProductDataService, ProductDataService>();

            services.AddScoped<ISystemOwnerService, SystemOwnerService>();

            services.AddScoped<IAllowanceDataService, AllowanceDataService>();
            services.AddScoped<IBonusDataService, BonusDataService>();
            services.AddScoped<ICalendarDataService, CalendarDataService>();
            services.AddScoped<IDivisionDataService, DivisionDataService>();
            services.AddScoped<IEmployeeDataService, EmployeeDataService>();
            services.AddScoped<IShiftDataService, ShiftDataService>();
            services.AddScoped<ILeaveDataService, LeaveDataService>();
            services.AddScoped<IListOfValueDataService, ListOfValueDataService>();
            services.AddScoped<ILoanDataService, LoanDataService>();
            services.AddScoped<ILoanPaymentFromThirteenthMonthPayDataService, LoanPaymentFromThirteenthMonthPayDataService>();
            services.AddScoped<IOfficialBusinessDataService, OfficialBusinessDataService>();
            services.AddScoped<IOrganizationDataService, OrganizationDataService>();
            services.AddScoped<IOvertimeDataService, OvertimeDataService>();
            services.AddScoped<IPaystubDataService, PaystubDataService>();
            services.AddScoped<IPaystubEmailDataService, PaystubEmailDataService>();
            services.AddScoped<IPositionDataService, PositionDataService>();
            services.AddScoped<IRoleDataService, RoleDataService>();
            services.AddScoped<ISalaryDataService, SalaryDataService>();
            services.AddScoped<ITimeEntryDataService, TimeEntryDataService>();
            services.AddScoped<ITimeLogDataService, TimeLogDataService>();
            services.AddScoped<IUserDataService, UserDataService>();

            services.AddScoped<IShiftImportParser, ShiftImportParser>();
            services.AddScoped<ITimeLogImportParser, TimeLogImportParser>();
            services.AddScoped<ITimeLogsReader, TimeLogsReader>();
            services.AddScoped<IPayslipDataService, PayslipDataService>();
            services.AddScoped<IPayslipBuilder, PayslipBuilder>();
            services.AddScoped<ISSSMonthlyReportDataService, SSSMonthlyReportDataService>();
            services.AddScoped<ISSSMonthyReportBuilder, SSSMonthyReportBuilder>();
            services.AddScoped<IPhilHealthMonthlyReportDataService, PhilHealthMonthlyReportDataService>();
            services.AddScoped<IPhilHealthMonthlyReportBuilder, PhilHealthMonthlyReportBuilder>();
            services.AddScoped<IPagIBIGMonthlyReportDataService, PagIBIGMonthlyReportDataService>();
            services.AddScoped<IPagIBIGMonthlyReportBuilder, PagIBIGMonthlyReportBuilder>();
            services.AddScoped<ILoanSummaryByTypeReportDataService, LoanSummaryByTypeReportDataService>();
            services.AddScoped<ILoanSummaryByTypeReportBuilder, LoanSummaryByTypeReportBuilder>();
            services.AddScoped<ILoanSummaryByEmployeeReportDataService, LoanSummaryByEmployeeReportDataService>();
            services.AddScoped<ILoanSummaryByEmployeeReportBuilder, LoanSummaryByEmployeeReportBuilder>();
            services.AddScoped<ITaxMonthlyReportDataService, TaxMonthlyReportDataService>();
            services.AddScoped<ITaxMonthlyReportBuilder, TaxMonthlyReportBuilder>();
            services.AddScoped<IThirteenthMonthSummaryReportDataService, ThirteenthMonthSummaryReportDataService>();
            services.AddScoped<IThirteenthMonthSummaryReportBuilder, ThirteenthMonthSummaryReportBuilder>();
            services.AddScoped<ITripTicketDataService, TripTicketDataService>();

            services.AddScoped<PaystubDataHelper>();
            services.AddScoped<TimeEntryDataHelper>();

            services.AddScoped(typeof(IExcelParser<>), typeof(ExcelParser<>));
            services.AddScoped<IEncryption, AccuPayDesktopEncryption>();

            services.AddScoped<IPayrollSummaryExcelFormatReportDataService, PayrollSummaryExcelFormatReportDataService>();
            services.AddScoped<IPayrollSummaryReportBuilder, PayrollSummaryReportBuilder>();
            services.AddScoped<IEmployeePersonalProfilesExcelFormatReportDataService, EmployeePersonalProfilesExcelFormatReportDataService>();
            services.AddScoped<IEmployeePersonalProfilesReportBuilder, EmployeePersonalProfilesReportBuilder>();
            services.AddScoped<ICostCenterReportBuilder, CostCenterReportBuilder>();

            services.AddScoped<IAttachmentDataService, AttachmentDataService>();
            services.AddScoped<IAwardDataService, AwardDataService>();
            services.AddScoped<IBonusDataService, BonusDataService>();
            services.AddScoped<ICertificationDataService, CertificationDataService>();
            services.AddScoped<IDisciplinaryActionDataService, DisciplinaryActionDataService>();
            services.AddScoped<IEducationalBackgroundDataService, EducationalBackgroundDataService>();
            services.AddScoped<IPreviousEmployerDataService, PreviousEmployerDataService>();
        }
    }
}
