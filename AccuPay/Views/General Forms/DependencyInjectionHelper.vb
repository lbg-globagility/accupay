Option Strict On

Imports AccuPay.Benchmark
Imports AccuPay.CrystalReports
Imports AccuPay.Data
Imports AccuPay.Data.Interfaces.Excel
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.Services.Imports
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
            End Sub, ServiceLifetime.Transient)

        ' passing DbContextOptions for services that needs to directly access PayrollContext.
        ' It Is prohibited to access PayrollContext unless there Is no other choice
        ' Like in the case of services accessed in multiple threads.
        services.AddSingleton(Of DbContextOptionsService)(
            Function(service As IServiceProvider)
                Dim options = GetDbContextOptions()
                Return New DbContextOptionsService(options)
            End Function)

        services.AddTransient(Of BenchmarkPayrollHelper)
        services.AddTransient(Of OvertimeRateService)

        services.AddTransient(Of ActualTimeEntryRepository)
        services.AddTransient(Of AddressRepository)
        services.AddTransient(Of AgencyFeeRepository)
        services.AddTransient(Of AgencyRepository)
        services.AddTransient(Of AllowanceRepository)
        services.AddTransient(Of AttachmentRepository)
        services.AddTransient(Of AwardRepository)
        services.AddTransient(Of BonusRepository)
        services.AddTransient(Of BranchRepository)
        services.AddTransient(Of BreakTimeBracketRepository)
        services.AddTransient(Of CalendarRepository)
        services.AddTransient(Of CategoryRepository)
        services.AddTransient(Of CertificationRepository)
        services.AddTransient(Of DayTypeRepository)
        services.AddTransient(Of DisciplinaryActionRepository)
        services.AddTransient(Of DivisionMinimumWageRepository)
        services.AddTransient(Of DivisionRepository)
        services.AddTransient(Of EducationalBackgroundRepository)
        services.AddTransient(Of EmployeeDutyScheduleRepository)
        services.AddTransient(Of EmployeeQueryBuilder)
        services.AddTransient(Of EmployeeRepository)
        services.AddTransient(Of FilingStatusTypeRepository)
        services.AddTransient(Of JobCategoryRepository)
        services.AddTransient(Of JobLevelRepository)
        services.AddTransient(Of LeaveRepository)
        services.AddTransient(Of LeaveLedgerRepository)
        services.AddTransient(Of ListOfValueRepository)
        services.AddTransient(Of LoanRepository)
        services.AddTransient(Of OfficialBusinessRepository)
        services.AddTransient(Of OrganizationRepository)
        services.AddTransient(Of OvertimeRepository)
        services.AddTransient(Of PayFrequencyRepository)
        services.AddTransient(Of PayPeriodRepository)
        services.AddTransient(Of PaystubActualRepository)
        services.AddTransient(Of PaystubEmailHistoryRepository)
        services.AddTransient(Of PaystubEmailRepository)
        services.AddTransient(Of PaystubRepository)
        services.AddTransient(Of PhilHealthBracketRepository)
        services.AddTransient(Of PositionRepository)
        services.AddTransient(Of PositionViewQueryBuilder)
        services.AddTransient(Of PositionViewRepository)
        services.AddTransient(Of PreviousEmployerRepository)
        services.AddTransient(Of ProductRepository)
        services.AddTransient(Of PromotionRepository)
        services.AddTransient(Of SalaryRepository)
        services.AddTransient(Of ShiftRepository)
        services.AddTransient(Of ShiftScheduleRepository)
        services.AddTransient(Of SocialSecurityBracketRepository)
        services.AddTransient(Of TimeAttendanceLogRepository)
        services.AddTransient(Of TimeEntryRepository)
        services.AddTransient(Of TimeLogRepository)
        services.AddTransient(Of UserActivityRepository)
        services.AddTransient(Of UserQueryBuilder)
        services.AddTransient(Of UserRepository)
        services.AddTransient(Of WithholdingTaxBracketRepository)

        services.AddTransient(Of CalendarService)
        services.AddTransient(Of ListOfValueService)

        services.AddTransient(Of PayrollGeneration)
        services.AddTransient(Of PayrollResources)

        services.AddTransient(Of PolicyHelper)
        'services.AddTransient(Of ActualTimeEntryPolicy)
        'services.AddTransient(Of AllowancePolicy)
        'services.AddTransient(Of PhilHealthPolicy)
        'services.AddTransient(Of RenewLeaveBalancePolicy)
        'services.AddTransient(Of TimeEntryPolicy)

        services.AddTransient(Of BpiInsuranceAmountReportDataService)
        services.AddTransient(Of CinemaTardinessReportDataService)
        services.AddTransient(Of CostCenterReportDataService)
        services.AddTransient(Of FiledLeaveReportDataService)
        services.AddTransient(Of LeaveLedgerReportDataService)
        services.AddTransient(Of PaystubPayslipModelDataService)

        services.AddTransient(Of TimeEntryGenerator)

        services.AddTransient(Of AdjustmentService)
        services.AddTransient(Of OvertimeRateService)
        services.AddTransient(Of LeaveAccrualService)
        services.AddTransient(Of PayPeriodService)
        services.AddTransient(Of ProductDataService)
        services.AddTransient(Of SystemOwnerService)

        services.AddTransient(Of AllowanceDataService)
        services.AddTransient(Of DivisionDataService)
        services.AddTransient(Of EmployeeDataService)
        services.AddTransient(Of EmployeeDutyScheduleDataService)
        services.AddTransient(Of LeaveDataService)
        services.AddTransient(Of LoanDataService)
        services.AddTransient(Of OfficialBusinessDataService)
        services.AddTransient(Of OvertimeDataService)
        services.AddTransient(Of PositionDataService)
        services.AddTransient(Of TimeLogDataService)

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
        services.AddTransient(Of LoanSummaryByTypeReportCreator)
        services.AddTransient(Of LoanSummaryByEmployeeReportDataService)
        services.AddTransient(Of LoanSummaryByEmployeeReportCreator)
        services.AddTransient(Of TaxMonthlyReportDataService)
        services.AddTransient(Of TaxMonthlyReportCreator)
        services.AddTransient(Of ThirteenthMonthSummaryReportDataService)
        services.AddTransient(Of ThirteenthMonthSummaryReportCreator)
        services.AddTransient(GetType(IExcelParser(Of)), GetType(ExcelParser(Of)))

        'services.AddTransient(Of MetroLogin)
        'services.AddTransient(Of MDIPrimaryForm)
        'services.AddTransient(Of GeneralForm)
        'services.AddTransient(Of HRISForm)
        'services.AddTransient(Of PayrollForm)
        'services.AddTransient(Of BenchmarkPayrollForm)
        'services.AddTransient(Of TimeAttendForm)

        'services.AddTransient(Of AddBranchForm)
    End Sub

    Private Shared Function GetDbContextOptions() As DbContextOptions
        Dim builder As DbContextOptionsBuilder = New DbContextOptionsBuilder()
        ConfigureDbContextOptions(builder)
        Return builder.Options
    End Function

    Private Shared Sub ConfigureDbContextOptions(dbContextOptionsBuilder As DbContextOptionsBuilder)

        Dim dbCommandConsoleLoggerFactory As LoggerFactory = New LoggerFactory({
                         New ConsoleLoggerProvider(
                               Function(category, level)
                                   Return category = DbLoggerCategory.Database.Command.Name AndAlso
                                        level = LogLevel.Information
                               End Function, True)
                         })

        dbContextOptionsBuilder.
            UseMySql(mysql_conn_text).
            UseLoggerFactory(dbCommandConsoleLoggerFactory).
            EnableSensitiveDataLogging()
    End Sub

End Class