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
        private readonly IRoleRepository _roleRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public EmployeeDataService(
            IEmployeeRepository employeeRepository,
            ILeaveLedgerRepository leaveLedgerRepository,
            IRoleRepository roleRepository,
            IOrganizationRepository organizationRepository,
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
            _roleRepository = roleRepository;
            _organizationRepository = organizationRepository;
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

            var singleParentLeaveProduct = await _productRepository
                .GetOrCreateLeaveTypeAsync(
                    ProductConstant.SINGLE_PARENT_LEAVE,
                    organizationId: organizationId,
                    userId: userId);

            if (vacationLeaveProduct?.RowID == null || sickLeaveProduct?.RowID == null
                || singleParentLeaveProduct?.RowID == null)
                throw new BusinessLogicException("Error accessing leave type data.");

            var employees = employeeWithLeaveBalanceModels.Select(x => x.Employee).ToList();

            await SaveManyAsync(employees, userId);

            var leaveTypes = await _productRepository.GetLeaveTypesAsync(organizationId);
            int[] defaultLeaveTypeIds = { vacationLeaveProduct.RowID.Value, sickLeaveProduct.RowID.Value, singleParentLeaveProduct.RowID.Value };
            var leaveTypeIds = leaveTypes.
                Where(p => !defaultLeaveTypeIds.Contains(p.RowID.Value)).
                Select(p => p.RowID.Value).
                ToArray();

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

                // single parent leave balance
                await _leaveLedgerRepository.CreateBeginningBalanceAsync(
                    employeeId: model.Employee.RowID.Value,
                    leaveTypeId: singleParentLeaveProduct.RowID.Value,
                    userId: userId,
                    organizationId: organizationId,
                    balance: model.SingleParentLeaveBalance);

                var employeeRowId = model.Employee.RowID.Value;
                foreach (var leaveTypeId in leaveTypeIds)
                {
                    await _leaveLedgerRepository.CreateBeginningBalanceAsync(
                        employeeId: employeeRowId,
                        leaveTypeId: leaveTypeId,
                        userId: userId,
                        organizationId: organizationId,
                        balance: 0);
                }
            }
        }

        public async Task ImportAsync(ICollection<EmployeeWithLeaveBalanceData> employeeWithLeaveBalanceModels,
            int userId)
        {
            var organizationIds = employeeWithLeaveBalanceModels
                .Select(e => e.Employee.OrganizationID.Value)
                .ToArray();

            var vacationLeaveProducts = new List<Product>();
            var sickLeaveProducts = new List<Product>();
            var singleParentLeaveProducts = new List<Product>();

            foreach (var organizationId in organizationIds)
            {
                vacationLeaveProducts.Add(item: await _productRepository
                .GetOrCreateLeaveTypeAsync(
                    ProductConstant.VACATION_LEAVE,
                    organizationId: organizationId,
                    userId: userId));

                sickLeaveProducts.Add(item: await _productRepository
                    .GetOrCreateLeaveTypeAsync(
                        ProductConstant.SICK_LEAVE,
                        organizationId: organizationId,
                        userId: userId));

                singleParentLeaveProducts.Add(item: await _productRepository
                    .GetOrCreateLeaveTypeAsync(
                        ProductConstant.SINGLE_PARENT_LEAVE,
                        organizationId: organizationId,
                        userId: userId));
            }

            var employees = employeeWithLeaveBalanceModels.Select(x => x.Employee).ToList();

            await SaveManyAsync(employees, userId);

            foreach (var organizationId in organizationIds)
            {
                var leaveTypes = await _productRepository.GetLeaveTypesAsync(organizationId);

                var vacationLeaveProduct = leaveTypes.FirstOrDefault(p => p.IsVacationLeave);
                var sickLeaveProduct = leaveTypes.FirstOrDefault(p => p.IsSickLeave);
                var singleParentLeaveProduct = leaveTypes.FirstOrDefault(p => p.IsSingleParentLeave);

                int[] defaultLeaveTypeIds = { vacationLeaveProduct.RowID.Value,
                    sickLeaveProduct.RowID.Value,
                    singleParentLeaveProduct.RowID.Value };

                var leaveTypeIds = leaveTypes.
                    Where(p => !defaultLeaveTypeIds.Contains(p.RowID.Value)).
                    Select(p => p.RowID.Value).
                    ToArray();

                var models = employeeWithLeaveBalanceModels
                    .Where(t => t.Employee.OrganizationID == organizationId)
                    .ToList();

                foreach (var model in models)
                {
                    var employee = model.Employee;

                    // vacation leave balance
                    await _leaveLedgerRepository.CreateBeginningBalanceAsync(
                        employeeId: employee.RowID.Value,
                        leaveTypeId: vacationLeaveProduct.RowID.Value,
                        userId: userId,
                        organizationId: organizationId,
                        balance: model.VacationLeaveBalance);

                    // sick leave balance
                    await _leaveLedgerRepository.CreateBeginningBalanceAsync(
                        employeeId: employee.RowID.Value,
                        leaveTypeId: sickLeaveProduct.RowID.Value,
                        userId: userId,
                        organizationId: organizationId,
                        balance: model.SickLeaveBalance);

                    // single parent leave balance
                    await _leaveLedgerRepository.CreateBeginningBalanceAsync(
                        employeeId: employee.RowID.Value,
                        leaveTypeId: singleParentLeaveProduct.RowID.Value,
                        userId: userId,
                        organizationId: organizationId,
                        balance: model.SingleParentLeaveBalance);

                    var employeeRowId = employee.RowID.Value;
                    foreach (var leaveTypeId in leaveTypeIds)
                    {
                        await _leaveLedgerRepository.CreateBeginningBalanceAsync(
                            employeeId: employeeRowId,
                            leaveTypeId: leaveTypeId,
                            userId: userId,
                            organizationId: organizationId,
                            balance: 0);
                    }
                }
            }
        }

        protected override async Task AdditionalSaveManyValidation(List<Employee> entities, List<Employee> oldEntities, SaveType saveType)
        {
            if (_policy.ImportPolicy.IsOpenToAllImportMethod &&
                (saveType == SaveType.Insert || saveType == SaveType.Update))
            {
                var createUserId = entities.Where(a => a.CreatedBy.HasValue).Select(a => a.CreatedBy).FirstOrDefault();
                var updateUserId = entities.Where(a => a.LastUpdBy.HasValue).Select(a => a.LastUpdBy).FirstOrDefault();
                var userId = updateUserId ?? createUserId;
                var userRoles = await _roleRepository.GetUserRolesByUserAsync(userId: userId ?? 0);

                var organizationIds = new List<int?>();
                if (userRoles != null && userRoles.Any())
                {
                    foreach (var x in entities)
                    {
                        var userRole = userRoles.FirstOrDefault(ur => ur.OrganizationId == x.OrganizationID);

                        var hasCreatePermission = userRole.Role.HasPermission(permissionName: PermissionConstant.EMPLOYEE, action: "create");
                        if (!hasCreatePermission)
                        {
                            var organization = await _organizationRepository.GetByIdAsync(x.OrganizationID.Value);
                            throw new BusinessLogicException($"Insufficient permission. You cannot create data for company: {organization.Name}.");
                        }

                        var hasUpdatePermission = userRole.Role.HasPermission(permissionName: PermissionConstant.EMPLOYEE, action: "update");
                        if (!hasUpdatePermission)
                        {
                            var organization = await _organizationRepository.GetByIdAsync(x.OrganizationID.Value);
                            throw new BusinessLogicException($"Insufficient permission. You cannot update data for company: {organization.Name}.");
                        }

                        organizationIds.Add(x.OrganizationID);
                    }
                }
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
