using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports;
using Microsoft.Extensions.DependencyInjection;

namespace AccuPay.Web
{
    public static class AccuPayCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddAccuPayCoreServices(this IServiceCollection services)
        {
            services.AddScoped<ActualTimeEntryRepository>();
            services.AddScoped<AddressRepository>();
            services.AddScoped<AdjustmentRepository>();
            services.AddScoped<AgencyFeeRepository>();
            services.AddScoped<AgencyRepository>();
            services.AddScoped<AllowanceRepository>();
            services.AddScoped<AspNetUserRepository>();
            services.AddScoped<AttachmentRepository>();
            services.AddScoped<AwardRepository>();
            services.AddScoped<BonusRepository>();
            services.AddScoped<BranchRepository>();
            services.AddScoped<BreakTimeBracketRepository>();
            services.AddScoped<CalendarRepository>();
            services.AddScoped<CategoryRepository>();
            services.AddScoped<CertificationRepository>();
            services.AddScoped<DayTypeRepository>();
            services.AddScoped<DisciplinaryActionRepository>();
            services.AddScoped<DivisionMinimumWageRepository>();
            services.AddScoped<DivisionRepository>();
            services.AddScoped<EducationalBackgroundRepository>();
            services.AddScoped<EmployeeDutyScheduleRepository>();
            services.AddScoped<EmployeeQueryBuilder>();
            services.AddScoped<EmployeeRepository>();
            services.AddScoped<FilingStatusTypeRepository>();
            services.AddScoped<JobCategoryRepository>();
            services.AddScoped<JobLevelRepository>();
            services.AddScoped<LeaveRepository>();
            services.AddScoped<ListOfValueRepository>();
            services.AddScoped<LoanScheduleRepository>();
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
            services.AddScoped<PositionViewQueryBuilder>();
            services.AddScoped<PositionViewRepository>();
            services.AddScoped<PreviousEmployerRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<PromotionRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<SalaryRepository>();
            services.AddScoped<ShiftRepository>();
            services.AddScoped<ShiftScheduleRepository>();
            services.AddScoped<SocialSecurityBracketRepository>();
            services.AddScoped<TimeAttendanceLogRepository>();
            services.AddScoped<TimeEntryRepository>();
            services.AddScoped<TimeLogRepository>();
            services.AddScoped<UserActivityRepository>();
            services.AddScoped<UserQueryBuilder>();
            services.AddScoped<UserRepository>();
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

            services.AddScoped<AdjustmentService>();
            services.AddScoped<OvertimeRateService>();
            services.AddScoped<PayPeriodService>();
            services.AddScoped<ProductDataService>();
            services.AddScoped<SystemOwnerService>();

            services.AddScoped<AllowanceDataService>();
            services.AddScoped<DivisionDataService>();
            services.AddScoped<EmployeeDutyScheduleDataService>();
            services.AddScoped<LeaveDataService>();
            services.AddScoped<LoanDataService>();
            services.AddScoped<OfficialBusinessDataService>();
            services.AddScoped<OvertimeDataService>();
            services.AddScoped<PositionDataService>();
            services.AddScoped<TimeLogDataService>();

            services.AddScoped<ShiftImportParser>();
            services.AddScoped<TimeLogImportParser>();
            services.AddScoped<TimeLogsReader>();

            return services;
        }
    }
}
