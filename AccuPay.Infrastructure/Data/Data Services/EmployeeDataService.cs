using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Core.Services.Imports.Employees;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class EmployeeDataService : BaseOrganizationDataService<Employee>, IEmployeeDataService
    {
        private const string UserActivityName = "Employee";

        private readonly ILeaveLedgerRepository _leaveLedgerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPositionRepository _positionRepository;

        public EmployeeDataService(
            IEmployeeRepository employeeRepository,
            ILeaveLedgerRepository leaveLedgerRepository,
            IPayPeriodRepository payPeriodRepository,
            IProductRepository productRepository,
            IPositionRepository positionRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(employeeRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Employee")
        {
            _leaveLedgerRepository = leaveLedgerRepository;
            _productRepository = productRepository;
            _positionRepository = positionRepository;
        }

        public async Task ImportAsync(ICollection<EmployeeWithLeaveBalanceData> employeeWithLeaveBalanceModels, int organizationId, int userId)
        {
            var vacationLeaveProduct = await _productRepository
                .GetOrCreateLeaveTypeAsync(
                    ProductConstant.VACATION_LEAVE,
                    organizationId: organizationId,
                    userId: userId);

            var sickLeaveProduct = await _productRepository
                .GetOrCreateLeaveTypeAsync(
                    ProductConstant.SICK_LEAVE,
                    organizationId: organizationId,
                    userId: userId);

            if (vacationLeaveProduct?.RowID == null || sickLeaveProduct?.RowID == null)
                throw new BusinessLogicException("Error accessing leave type data.");

            var employees = employeeWithLeaveBalanceModels.Select(x => x.Employee).ToList();

            await SaveManyAsync(employees, userId);

            foreach (var model in employeeWithLeaveBalanceModels)
            {
                // vacation leave balance
                await _leaveLedgerRepository.CreateBeginningBalanceAsync(
                    employeeId: model.Employee.RowID.Value,
                    leaveTypeId: vacationLeaveProduct.RowID.Value,
                    userId: userId,
                    organizationId: organizationId,
                    balance: model.VacationLeaveBalance);

                // sick leave balance
                await _leaveLedgerRepository.CreateBeginningBalanceAsync(
                    employeeId: model.Employee.RowID.Value,
                    leaveTypeId: sickLeaveProduct.RowID.Value,
                    userId: userId,
                    organizationId: organizationId,
                    balance: model.SickLeaveBalance);
            }
        }

        public async Task<List<Employee>> BatchApply(IReadOnlyCollection<EmployeeImportModel> validRecords, List<string> jobNames, int organizationId, int changedByUserId)
        {
            var jobs = await _positionRepository.CreateManyAsync(jobNames, organizationId, changedByUserId);
            foreach (var parsedEmployee in validRecords.Where(t => t.JobNotYetExists))
            {
                var job = jobs.FirstOrDefault(j => j.Name == parsedEmployee.JobPosition);
                parsedEmployee.SetPositionId(job.RowID);
            }

            var employees = validRecords.Select(x => x.Employee).ToList();

            await SaveManyAsync(employees, changedByUserId);

            return validRecords.Select(x => x.Employee).ToList();
        }

        #region Overrides

        protected override string GetUserActivityName(Employee employee)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(Employee employee)
        {
            return string.Empty;
        }

        protected override async Task SanitizeEntity(Employee entity, Employee oldEntity, int changedByUserId)
        {
            await base.SanitizeEntity(entity, oldEntity, changedByUserId);

            // 1. Set TerminationDate to null if IsActive = true
        }

        protected override async Task RecordDelete(Employee entity, int currentlyLoggedInUserId)
        {
            await _userActivityRepository.RecordDeleteAsync(
                currentlyLoggedInUserId,
                entityId: entity.RowID.Value,
                entityName: GetUserActivityName(entity),
                suffixIdentifier: CreateUserActivitySuffixIdentifier(entity),
                organizationId: entity.OrganizationID.Value,
                changedEmployeeId: entity.RowID.Value);
        }

        protected override async Task RecordAdd(Employee entity)
        {
            await _userActivityRepository.RecordAddAsync(
                entity.CreatedBy.Value,
                entityId: entity.RowID.Value,
                entityName: GetUserActivityName(entity),
                suffixIdentifier: CreateUserActivitySuffixIdentifier(entity),
                organizationId: entity.OrganizationID.Value,
                changedEmployeeId: entity.RowID.Value);
        }

        #endregion Overrides
    }
}
