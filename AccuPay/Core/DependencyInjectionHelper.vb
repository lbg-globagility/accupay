Option Strict On

Imports AccuPay.Benchmark
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Interfaces.Excel
Imports AccuPay.Core.Interfaces.Reports
Imports AccuPay.Core.Services
Imports AccuPay.Core.Services.Imports
Imports AccuPay.CrystalReports
Imports AccuPay.Infrastructure.Data
Imports AccuPay.Infrastructure.Data.Reports
Imports AccuPay.Infrastructure.Reports
Imports AccuPay.Infrastructure.Reports.Customize
Imports AccuPay.Infrastructure.Services.Encryption
Imports AccuPay.Infrastructure.Services.Excel
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging
Imports Microsoft.Extensions.Logging.Console

Public Class DependencyInjectionHelper

    Public Shared Sub ConfigureDependencyInjection()
        Dim services = New ServiceCollection()

        ConfigureServices(services)

        MainServiceProvider = services.BuildServiceProvider()
    End Sub

    Private Shared Sub ConfigureServices(services As ServiceCollection)

        services.AddDbContext(Of PayrollContext)(
            Sub(options As DbContextOptionsBuilder)
                ConfigureDbContextOptions(options)
            End Sub,
            ServiceLifetime.Transient)

        services.AddTransient(Of BenchmarkPayrollHelper)

        services.AddTransient(Of IAddressRepository, AddressRepository)
        services.AddTransient(Of IActualTimeEntryRepository, ActualTimeEntryRepository)
        services.AddTransient(Of IAgencyFeeRepository, AgencyFeeRepository)
        services.AddTransient(Of IAgencyRepository, AgencyRepository)
        services.AddTransient(Of IAllowanceRepository, AllowanceRepository)
        services.AddTransient(Of IAllowanceTypeRepository, AllowanceTypeRepository)
        services.AddTransient(Of IAspNetUserRepository, AspNetUserRepository)
        services.AddTransient(Of IAttachmentRepository, AttachmentRepository)
        services.AddTransient(Of IAwardRepository, AwardRepository)
        services.AddTransient(Of IBonusRepository, BonusRepository)
        services.AddTransient(Of IBranchRepository, BranchRepository)
        services.AddTransient(Of IBreakTimeBracketRepository, BreakTimeBracketRepository)
        services.AddTransient(Of ICalendarRepository, CalendarRepository)
        services.AddTransient(Of ICategoryRepository, CategoryRepository)
        services.AddTransient(Of ICertificationRepository, CertificationRepository)
        services.AddTransient(Of IDayTypeRepository, DayTypeRepository)
        services.AddTransient(Of IDisciplinaryActionRepository, DisciplinaryActionRepository)
        services.AddTransient(Of IDivisionRepository, DivisionRepository)
        services.AddTransient(Of IEducationalBackgroundRepository, EducationalBackgroundRepository)
        services.AddTransient(Of IEmploymentPolicyRepository, EmploymentPolicyRepository)
        services.AddTransient(Of IEmployeeQueryBuilder, EmployeeQueryBuilder)
        services.AddTransient(Of IEmployeeRepository, EmployeeRepository)
        services.AddTransient(Of IJobCategoryRepository, JobCategoryRepository)
        services.AddTransient(Of IJobLevelRepository, JobLevelRepository)
        services.AddTransient(Of ILeaveRepository, LeaveRepository)
        services.AddTransient(Of ILeaveLedgerRepository, LeaveLedgerRepository)
        services.AddTransient(Of IListOfValueRepository, ListOfValueRepository)
        services.AddTransient(Of ILoanPaymentFromBonusRepository, LoanPaymentFromBonusRepository)
        services.AddTransient(Of ILoanPaymentFromThirteenthMonthPayRepository, LoanPaymentFromThirteenthMonthPayRepository)
        services.AddTransient(Of ILoanRepository, LoanRepository)
        services.AddTransient(Of IOfficialBusinessRepository, OfficialBusinessRepository)
        services.AddTransient(Of IOrganizationRepository, OrganizationRepository)
        services.AddTransient(Of IOvertimeRepository, OvertimeRepository)
        services.AddTransient(Of IPayFrequencyRepository, PayFrequencyRepository)
        services.AddTransient(Of IPayPeriodRepository, PayPeriodRepository)
        services.AddTransient(Of IPaystubActualRepository, PaystubActualRepository)
        services.AddTransient(Of IPaystubEmailHistoryRepository, PaystubEmailHistoryRepository)
        services.AddTransient(Of IPaystubEmailRepository, PaystubEmailRepository)
        services.AddTransient(Of IPaystubRepository, PaystubRepository)
        services.AddTransient(Of IPermissionRepository, PermissionRepository)
        services.AddTransient(Of IPhilHealthBracketRepository, PhilHealthBracketRepository)
        services.AddTransient(Of IPositionRepository, PositionRepository)
        services.AddTransient(Of IPreviousEmployerRepository, PreviousEmployerRepository)
        services.AddTransient(Of IProductRepository, ProductRepository)
        services.AddTransient(Of IRoleRepository, RoleRepository)
        services.AddTransient(Of IRouteRepository, RouteRepository)
        services.AddTransient(Of IRouteRateRepository, RouteRateRepository)
        services.AddTransient(Of ISalaryRepository, SalaryRepository)
        services.AddTransient(Of IShiftRepository, ShiftRepository)
        services.AddTransient(Of ISocialSecurityBracketRepository, SocialSecurityBracketRepository)
        services.AddTransient(Of ISystemInfoRepository, SystemInfoRepository)
        services.AddTransient(Of ITimeAttendanceLogRepository, TimeAttendanceLogRepository)
        services.AddTransient(Of ITimeEntryRepository, TimeEntryRepository)
        services.AddTransient(Of ITimeLogRepository, TimeLogRepository)
        services.AddTransient(Of ITripTicketRepository, TripTicketRepository)
        services.AddTransient(Of IUserActivityRepository, UserActivityRepository)
        services.AddTransient(Of IVehicleRepository, VehicleRepository)
        services.AddTransient(Of IWithholdingTaxBracketRepository, WithholdingTaxBracketRepository)

        services.AddTransient(Of ICalendarService, CalendarService)
        services.AddTransient(Of IListOfValueService, ListOfValueService)

        services.AddTransient(Of ICostCenterReportResources, CostCenterReportResources)
        services.AddTransient(Of IPayrollGenerator, PayrollGenerator)
        services.AddTransient(Of IPayrollResources, PayrollResources)

        services.AddTransient(Of IPolicyHelper, PolicyHelper)

        services.AddTransient(Of IBpiInsuranceAmountReportDataService, BpiInsuranceAmountReportDataService)
        services.AddTransient(Of ICinemaTardinessReportDataService, CinemaTardinessReportDataService)
        services.AddTransient(Of IFiledLeaveReportDataService, FiledLeaveReportDataService)
        services.AddTransient(Of ILeaveLedgerReportDataService, LeaveLedgerReportDataService)
        services.AddTransient(Of IPaystubPayslipModelDataService, PaystubPayslipModelDataService)

        services.AddTransient(Of ITimeEntryGenerator, TimeEntryGenerator)
        services.AddTransient(Of ITimeEntryResources, TimeEntryResources)

        services.AddTransient(Of ILeaveAccrualService, LeaveAccrualService)
        services.AddTransient(Of IOvertimeRateService, OvertimeRateService)
        services.AddTransient(Of IPayPeriodDataService, PayPeriodDataService)
        services.AddTransient(Of IProductDataService, ProductDataService)

        services.AddTransient(Of ISystemOwnerService, SystemOwnerService)

        services.AddTransient(Of IAllowanceDataService, AllowanceDataService)
        services.AddTransient(Of IBonusDataService, BonusDataService)
        services.AddTransient(Of ICalendarDataService, CalendarDataService)
        services.AddTransient(Of IDivisionDataService, DivisionDataService)
        services.AddTransient(Of IEmployeeDataService, EmployeeDataService)
        services.AddTransient(Of IShiftDataService, ShiftDataService)
        services.AddTransient(Of ILeaveDataService, LeaveDataService)
        services.AddTransient(Of IListOfValueDataService, ListOfValueDataService)
        services.AddTransient(Of ILoanDataService, LoanDataService)
        services.AddTransient(Of ILoanPaymentFromThirteenthMonthPayDataService, LoanPaymentFromThirteenthMonthPayDataService)
        services.AddTransient(Of IOfficialBusinessDataService, OfficialBusinessDataService)
        services.AddTransient(Of IOrganizationDataService, OrganizationDataService)
        services.AddTransient(Of IOvertimeDataService, OvertimeDataService)
        services.AddTransient(Of IPaystubDataService, PaystubDataService)
        services.AddTransient(Of IPaystubEmailDataService, PaystubEmailDataService)
        services.AddTransient(Of IPositionDataService, PositionDataService)
        services.AddTransient(Of IRoleDataService, RoleDataService)
        services.AddTransient(Of ISalaryDataService, SalaryDataService)
        services.AddTransient(Of ITimeEntryDataService, TimeEntryDataService)
        services.AddTransient(Of ITimeLogDataService, TimeLogDataService)
        services.AddTransient(Of ITripTicketDataService, TripTicketDataService)
        services.AddTransient(Of IUserDataService, UserDataService)

        services.AddTransient(Of IShiftImportParser, ShiftImportParser)
        services.AddTransient(Of ITimeLogImportParser, TimeLogImportParser)
        services.AddTransient(Of ITimeLogsReader, TimeLogsReader)
        services.AddTransient(Of IPayslipDataService, PayslipDataService)
        services.AddTransient(Of IPayslipBuilder, PayslipBuilder)
        services.AddTransient(Of IBenchmarkAlphalistReportDataService, BenchmarkAlphalistReportDataService)
        services.AddTransient(Of IBenchmarkAlphalistBuilder, BenchmarkAlphalistBuilder)
        services.AddTransient(Of ILoanSummaryByEmployeeReportDataService, LoanSummaryByEmployeeReportDataService)
        services.AddTransient(Of ILoanSummaryByEmployeeReportBuilder, LoanSummaryByEmployeeReportBuilder)
        services.AddTransient(Of ILoanSummaryByTypeReportDataService, LoanSummaryByTypeReportDataService)
        services.AddTransient(Of ILoanSummaryByTypeReportBuilder, LoanSummaryByTypeReportBuilder)
        services.AddTransient(Of IPagIBIGMonthlyReportDataService, PagIBIGMonthlyReportDataService)
        services.AddTransient(Of IPagIBIGMonthlyReportBuilder, PagIBIGMonthlyReportBuilder)
        services.AddTransient(Of IPhilHealthMonthlyReportDataService, PhilHealthMonthlyReportDataService)
        services.AddTransient(Of IPhilHealthMonthlyReportBuilder, PhilHealthMonthlyReportBuilder)
        services.AddTransient(Of ISSSMonthlyReportDataService, SSSMonthlyReportDataService)
        services.AddTransient(Of ISSSMonthyReportBuilder, SSSMonthyReportBuilder)
        services.AddTransient(Of ITaxMonthlyReportDataService, TaxMonthlyReportDataService)
        services.AddTransient(Of ITaxMonthlyReportBuilder, TaxMonthlyReportBuilder)
        services.AddTransient(Of IThirteenthMonthSummaryReportDataService, ThirteenthMonthSummaryReportDataService)
        services.AddTransient(Of IThirteenthMonthSummaryReportBuilder, ThirteenthMonthSummaryReportBuilder)
        services.AddTransient(Of ILaGlobalAlphaListReportDataService, LaGlobalAlphaListReportDataService)
        services.AddTransient(Of ILaGlobalAlphaListReportBuilder, LaGlobalAlphaListReportBuilder)
        services.AddTransient(Of IAlphaListReportDataService, AlphaListReportDataService)
        services.AddTransient(Of IAlphalistReportBuilder, AlphalistReportBuilder)

        services.AddTransient(Of PaystubDataHelper)
        services.AddTransient(Of TimeEntryDataHelper)

        services.AddTransient(GetType(IExcelParser(Of)), GetType(ExcelParser(Of)))
        services.AddTransient(Of IEncryption, AccuPayDesktopEncryption)

        services.AddTransient(Of IPayrollSummaryExcelFormatReportDataService, PayrollSummaryExcelFormatReportDataService)
        services.AddTransient(Of IPayrollSummaryReportBuilder, PayrollSummaryReportBuilder)
        services.AddTransient(Of IEmployeePersonalProfilesExcelFormatReportDataService, EmployeePersonalProfilesExcelFormatReportDataService)
        services.AddTransient(Of IEmployeePersonalProfilesReportBuilder, EmployeePersonalProfilesReportBuilder)
        services.AddTransient(Of ICostCenterReportBuilder, CostCenterReportBuilder)

        services.AddTransient(Of IAttachmentDataService, AttachmentDataService)
        services.AddTransient(Of IAwardDataService, AwardDataService)
        services.AddTransient(Of IBonusDataService, BonusDataService)
        services.AddTransient(Of ICertificationDataService, CertificationDataService)
        services.AddTransient(Of IDisciplinaryActionDataService, DisciplinaryActionDataService)
        services.AddTransient(Of IEducationalBackgroundDataService, EducationalBackgroundDataService)
        services.AddTransient(Of IPreviousEmployerDataService, PreviousEmployerDataService)
        services.AddTransient(Of IAlphalistReportBuilder, AlphalistReportBuilder)
    End Sub

    Private Shared Sub ConfigureDbContextOptions(dbContextOptionsBuilder As DbContextOptionsBuilder)

        dbContextOptionsBuilder.
            UseMySql(mysql_conn_text) '.
        'UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)

        If Debugger.IsAttached Then

            Dim dbCommandConsoleLoggerFactory As LoggerFactory = New LoggerFactory({
                New ConsoleLoggerProvider(
                    Function(category, level)
                        Return category = DbLoggerCategory.Database.Command.Name AndAlso
                            level = LogLevel.Information
                    End Function, True)
                })

            dbContextOptionsBuilder = dbContextOptionsBuilder.
                EnableSensitiveDataLogging().
                UseLoggerFactory(dbCommandConsoleLoggerFactory)
        End If
    End Sub

End Class
