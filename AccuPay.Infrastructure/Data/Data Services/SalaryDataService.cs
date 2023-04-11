using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.Imports.Salaries;
using AccuPay.Core.Services.Policies;
using AccuPay.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class SalaryDataService : BaseEmployeeDataService<Salary>, ISalaryDataService
    {
        private const string UserActivityName = "Salary";

        private readonly IPaystubRepository _paystubRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public SalaryDataService(
            ISalaryRepository salaryRepository,
            IRoleRepository roleRepository,
            IOrganizationRepository organizationRepository,
            IPayPeriodRepository payPeriodRepository,
            IPaystubRepository paystubRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(salaryRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Salary",
                entityNamePlural: "Salaries")
        {
            _paystubRepository = paystubRepository;
            _salaryRepository = salaryRepository;
            _roleRepository = roleRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<List<Salary>> BatchApply(IReadOnlyCollection<SalaryImportModel> validRecords, int organizationId, int currentlyLoggedInUserId)
        {
            List<Salary> added = new List<Salary>();
            foreach (var validRecord in validRecords)
            {
                var salary = new Salary
                {
                    OrganizationID = organizationId,
                    EmployeeID = validRecord.EmployeeId,
                    BasicSalary = validRecord.BasicSalary.Value,
                    AllowanceSalary = validRecord.AllowanceSalary.HasValue ? validRecord.AllowanceSalary.Value : 0,
                    EffectiveFrom = validRecord.EffectiveFrom.Value
                };

                added.Add(salary);
            }

            await SaveManyAsync(added, currentlyLoggedInUserId);

            return added;
        }

        #region Overrides

        protected override string GetUserActivityName(Salary salary) => UserActivityName;

        protected override string CreateUserActivitySuffixIdentifier(Salary salary) =>
            $" with start date '{salary.EffectiveFrom.ToShortDateString()}'";

        protected override async Task SanitizeEntity(Salary salary, Salary oldSalary, int changedByUserId)
        {
            await base.SanitizeEntity(
                entity: salary,
                oldEntity: oldSalary,
                currentlyLoggedInUserId: changedByUserId);

            if (salary.EffectiveFrom < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753.");

            if (salary.BasicSalary < 0)
                throw new BusinessLogicException("Basic Salary cannot be less than 0.");

            if (salary.AllowanceSalary < 0)
                throw new BusinessLogicException("Allowance Salary cannot be less than 0.");

            var salariesWithSameDate = await _salaryRepository.GetByEmployeeAndEffectiveFromAsync(
                salary.EmployeeID.Value,
                salary.EffectiveFrom);

            if (salary.IsNewEntity)
            {
                if (salariesWithSameDate.Any())
                    throw new BusinessLogicException("Salary already exists!");
            }
            else
            {
                if (salariesWithSameDate.Any(x => x.RowID != salary.RowID))
                    throw new BusinessLogicException("Salary already exists!");
            }

            salary.UpdateTotalSalary();
        }

        protected override async Task AdditionalDeleteValidation(Salary salary)
        {
            if (await CheckIfSalaryHasBeenUsed(salary))
                throw new BusinessLogicException("This salary has already been used and therefore cannot be deleted.");
        }

        protected override async Task AdditionalSaveValidation(Salary salary, Salary oldSalary)
        {
            await ValidateSalaryIfAlreadyUsed(salary, oldSalary);
        }

        protected override async Task AdditionalSaveManyValidation(List<Salary> salaries, List<Salary> oldSalaries, SaveType saveType)
        {
            if (_policy.ImportPolicy.IsOpenToAllImportMethod &&
                (saveType == SaveType.Insert || saveType == SaveType.Update))
            {
                var createUserId = salaries.Where(a => a.CreatedBy.HasValue).Select(a => a.CreatedBy).FirstOrDefault();
                var updateUserId = salaries.Where(a => a.LastUpdBy.HasValue).Select(a => a.LastUpdBy).FirstOrDefault();
                var userId = updateUserId ?? createUserId;
                var userRoles = await _roleRepository.GetUserRolesByUserAsync(userId: userId ?? 0);
                await CheckForClosedPayPeriod(salaries, oldSalaries, userRoles: userRoles);
            }
            else
            {
                foreach (var salary in salaries)
                {
                    var oldSalary = oldSalaries.FirstOrDefault(x => x.RowID == salary.RowID);
                    // TODO: think of a better way to not call this query for every salary
                    await ValidateSalaryIfAlreadyUsed(salary, oldSalary);
                }
            }
        }

        private async Task CheckForClosedPayPeriod(List<Salary> salaries, List<Salary> oldSalaries, List<UserRole> userRoles)
        {
            List<int?> organizationIds = await ValidateStartDates(salaries, oldSalaries, userRoles: userRoles);
        }

        private async Task<List<int?>> ValidateStartDates(List<Salary> salaries, List<Salary> oldSalaries, List<UserRole> userRoles)
        {
            var organizationIds = new List<int?>();
            if (userRoles != null && userRoles.Any())
            {
                foreach (var x in salaries)
                {
                    var userRole = userRoles.FirstOrDefault(ur => ur.OrganizationId == x.OrganizationID);

                    var hasCreatePermission = userRole.Role.HasPermission(permissionName: PermissionConstant.SALARY, action: "create");
                    if (!hasCreatePermission)
                    {
                        var organization = await _organizationRepository.GetByIdAsync(x.OrganizationID.Value);
                        throw new BusinessLogicException($"Insufficient permission. You cannot create data for company: {organization.Name}.");
                    }

                    var hasUpdatePermission = userRole.Role.HasPermission(permissionName: PermissionConstant.SALARY, action: "update");
                    if (!hasUpdatePermission)
                    {
                        var organization = await _organizationRepository.GetByIdAsync(x.OrganizationID.Value);
                        throw new BusinessLogicException($"Insufficient permission. You cannot update data for company: {organization.Name}.");
                    }

                    organizationIds.Add(x.OrganizationID);
                }
            }
            else
            {
                int? organizationId = null;
                salaries.ForEach(x =>
                {
                    organizationId = ValidateOrganization(organizationId, x.OrganizationID);
                });
                organizationIds = new List<int?>() { organizationId };
            }

            var validatableStartDates = salaries
                 .Where(salary =>
                 {
                     return CheckIfStartDateNeedsToBeValidated(oldSalaries, salary);
                 })
                 .Select(x => x.EffectiveFrom)
                 .ToArray();

            foreach (var orgId in organizationIds)
                await CheckIfDataIsWithinClosedPayPeriod(validatableStartDates, orgId.Value);

            return organizationIds;
        }

        private bool CheckIfStartDateNeedsToBeValidated(List<Salary> oldSalaries, Salary salary)
        {
            if (salary.IsNewEntity) return true;

            var oldSalary = oldSalaries.Where(o => o.RowID == salary.RowID).FirstOrDefault();

            if (salary.EffectiveFrom.ToMinimumHourValue() != oldSalary.EffectiveFrom.ToMinimumHourValue())
                return true;

            return false;
        }

        protected override async Task RecordUpdate(Salary newValue, Salary oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.EffectiveFrom != oldValue.EffectiveFrom)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start date from '{oldValue.EffectiveFrom.ToShortDateString()}' to '{newValue.EffectiveFrom.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.BasicSalary != oldValue.BasicSalary)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated basic salary from '{oldValue.BasicSalary}' to '{newValue.BasicSalary}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.AllowanceSalary != oldValue.AllowanceSalary)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated allowance salary from '{oldValue.AllowanceSalary}' to '{newValue.AllowanceSalary}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.PhilHealthDeduction != oldValue.PhilHealthDeduction)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated PhilHealth deduction from '{oldValue.PhilHealthDeduction}' to '{newValue.PhilHealthDeduction}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.AutoComputePhilHealthContribution != oldValue.AutoComputePhilHealthContribution)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated PhilHealth autocompute option from '{oldValue.AutoComputePhilHealthContribution}' to '{newValue.AutoComputePhilHealthContribution}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.DoPaySSSContribution != oldValue.DoPaySSSContribution)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated SSS pay option from '{oldValue.DoPaySSSContribution}' to '{newValue.DoPaySSSContribution}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.HDMFAmount != oldValue.HDMFAmount)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated PAGIBIG deduction from '{oldValue.HDMFAmount}' to '{newValue.HDMFAmount}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.AutoComputeHDMFContribution != oldValue.AutoComputeHDMFContribution)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated PAGIBIG autocompute option from '{oldValue.AutoComputeHDMFContribution}' to '{newValue.AutoComputeHDMFContribution}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });

            if (changes.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    newValue.LastUpdBy.Value,
                    UserActivityName,
                    newValue.OrganizationID.Value,
                    UserActivity.RecordTypeEdit,
                    changes);
            }
        }

        #endregion Overrides

        #region Private Methods

        private async Task ValidateSalaryIfAlreadyUsed(Salary salary, Salary oldSalary)
        {
            if (salary.IsNewEntity || salary.EffectiveFrom.ToMinimumHourValue() != oldSalary.EffectiveFrom.ToMinimumHourValue())
            {
                if (await _payPeriodRepository.HasClosedPayPeriodAfterDateAsync(salary.OrganizationID.Value, salary.EffectiveFrom))
                    throw new BusinessLogicException("Salary cannot be saved in or before a \"Closed\" pay period.");
            }

            if (salary.IsNewEntity == false)
            {
                if (await CheckIfSalaryHasBeenUsed(salary))
                    throw new BusinessLogicException("This salary has already been used and therefore cannot be modified.");
            }
        }

        private async Task<bool> CheckIfSalaryHasBeenUsed(Salary salary)
        {
            DayValueSpan firstHalf = _policy.DefaultFirstHalfDaysSpan(salary.OrganizationID.Value);
            DayValueSpan endOfTheMonth = _policy.DefaultEndOfTheMonthDaysSpan(salary.OrganizationID.Value);

            (DayValueSpan currentDaySpan, int month, int year) = PayPeriodHelper
                .GetCutOffDayValueSpan(salary.EffectiveFrom, firstHalf, endOfTheMonth);

            var salaryFirstCutOffStartDate = currentDaySpan.From.GetDate(month: month, year: year);

            return await _paystubRepository.HasPaystubsAfterDateAsync(salaryFirstCutOffStartDate, salary.EmployeeID.Value);
        }

        public async Task<List<Salary>> GetSalariesByIds(int[] rowIds)
        {
            return await _salaryRepository.GetSalariesByIds(rowIds: rowIds);
        }

        #endregion Private Methods
    }
}
