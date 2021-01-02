Option Strict On

Imports AccuPay.Benchmark
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Interfaces.Excel
Imports AccuPay.Core.Repositories
Imports AccuPay.Core.Services
Imports AccuPay.Core.Services.Imports
Imports AccuPay.Core.Services.Reports
Imports AccuPay.CrystalReports
Imports AccuPay.Infrastructure.Data
Imports AccuPay.Infrastructure.Reports
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

        services.AddDbContext(Of Infrastructure.Data.PayrollContext)(
            Sub(options As DbContextOptionsBuilder)
                ConfigureDbContextOptions(options)
            End Sub,
            ServiceLifetime.Transient)

        services.AddTransient(Of BenchmarkPayrollHelper)
        services.AddTransient(Of OvertimeRateService)

        services.AddTransient(GetType(ISavableRepository(Of)), GetType(Infrastructure.Data.SavableRepository(Of)))

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

        services.AddTransient(Of CalendarService)
        services.AddTransient(Of ListOfValueService)

        services.AddTransient(Of CostCenterReportResources)
        services.AddTransient(Of PayrollGenerator)
        services.AddTransient(Of PayrollResources)

        services.AddTransient(Of IPolicyHelper, PolicyHelper)
        'services.AddTransient(Of ActualTimeEntryPolicy)
        'services.AddTransient(Of AllowancePolicy)
        'services.AddTransient(Of PhilHealthPolicy)
        'services.AddTransient(Of RenewLeaveBalancePolicy)
        'services.AddTransient(Of TimeEntryPolicy)

        services.AddTransient(Of BpiInsuranceAmountReportDataService)
        services.AddTransient(Of CinemaTardinessReportDataService)
        services.AddTransient(Of FiledLeaveReportDataService)
        services.AddTransient(Of LeaveLedgerReportDataService)
        services.AddTransient(Of PaystubPayslipModelDataService)

        services.AddTransient(Of TimeEntryGenerator)
        services.AddTransient(Of TimeEntryResources)

        services.AddTransient(Of OvertimeRateService)
        services.AddTransient(Of LeaveAccrualService)
        services.AddTransient(Of PayPeriodDataService)
        services.AddTransient(Of ProductDataService)
        services.AddTransient(Of SystemOwnerService)

        services.AddTransient(Of AllowanceDataService)
        services.AddTransient(Of BonusDataService)
        services.AddTransient(Of CalendarDataService)
        services.AddTransient(Of DivisionDataService)
        services.AddTransient(Of EmployeeDataService)
        services.AddTransient(Of ShiftDataService)
        services.AddTransient(Of LeaveDataService)
        services.AddTransient(Of ListOfValueDataService)
        services.AddTransient(Of LoanDataService)
        services.AddTransient(Of LoanPaymentFromThirteenthMonthPayDataService)
        services.AddTransient(Of OfficialBusinessDataService)
        services.AddTransient(Of OrganizationDataService)
        services.AddTransient(Of OvertimeDataService)
        services.AddTransient(Of PaystubDataService)
        services.AddTransient(Of PaystubEmailDataService)
        services.AddTransient(Of PositionDataService)
        services.AddTransient(Of RoleDataService)
        services.AddTransient(Of SalaryDataService)
        services.AddTransient(Of TimeEntryDataService)
        services.AddTransient(Of TimeLogDataService)
        services.AddTransient(Of UserDataService)

        services.AddTransient(Of ShiftImportParser)
        services.AddTransient(Of TimeLogImportParser)
        services.AddTransient(Of TimeLogsReader)

        services.AddTransient(Of PayslipDataService)
        services.AddTransient(Of PayslipBuilder)
        services.AddTransient(Of SSSMonthlyReportDataService)
        services.AddTransient(Of SSSMonthyReportBuilder)
        services.AddTransient(Of PhilHealthMonthlyReportDataService)
        services.AddTransient(Of PhilHealthMonthlyReportBuilder)
        services.AddTransient(Of PagIBIGMonthlyReportDataService)
        services.AddTransient(Of PagIBIGMonthlyReportBuilder)
        services.AddTransient(Of LoanSummaryByTypeReportDataService)
        services.AddTransient(Of LoanSummaryByTypeReportBuilder)
        services.AddTransient(Of LoanSummaryByEmployeeReportDataService)
        services.AddTransient(Of LoanSummaryByEmployeeReportBuilder)
        services.AddTransient(Of TaxMonthlyReportDataService)
        services.AddTransient(Of TaxMonthlyReportBuilder)
        services.AddTransient(Of ThirteenthMonthSummaryReportDataService)
        services.AddTransient(Of ThirteenthMonthSummaryReportBuilder)
        services.AddTransient(Of TripTicketDataService)

        services.AddTransient(Of PaystubDataHelper)
        services.AddTransient(Of TimeEntryDataHelper)

        services.AddTransient(GetType(IExcelParser(Of)), GetType(ExcelParser(Of)))
        services.AddTransient(Of IEncryption, AccuPayDesktopEncryption)

        services.AddTransient(Of PayrollSummaryExcelFormatReportDataService)
        services.AddTransient(Of IPayrollSummaryReportBuilder, PayrollSummaryReportBuilder)
        services.AddTransient(Of EmployeePersonalProfilesExcelFormatReportDataService)
        services.AddTransient(Of IEmployeePersonalProfilesReportBuilder, EmployeePersonalProfilesReportBuilder)
        services.AddTransient(Of ICostCenterReportBuilder, CostCenterReportBuilder)

        services.AddTransient(Of IAttachmentDataService, AttachmentDataService)
        services.AddTransient(Of IAwardDataService, AwardDataService)
        services.AddTransient(Of IBonusDataService, BonusDataService)
        services.AddTransient(Of ICertificationDataService, CertificationDataService)
        services.AddTransient(Of IDisciplinaryActionDataService, DisciplinaryActionDataService)
        services.AddTransient(Of IEducationalBackgroundDataService, EducationalBackgroundDataService)
        services.AddTransient(Of IPreviousEmployerDataService, PreviousEmployerDataService)
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
