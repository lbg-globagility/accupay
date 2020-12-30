using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services.Imports.Salaries;
using AccuPay.Data.Services.Policies;
using AccuPay.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class SalaryDataService : BaseEmployeeDataService<Salary>
    {
        private const string UserActivityName = "Salary";

        private readonly PaystubRepository _paystubRepository;
        private readonly SalaryRepository _salaryRepository;

        public SalaryDataService(
            SalaryRepository salaryRepository,
            PayPeriodRepository payPeriodRepository,
            PaystubRepository paystubRepository,
            UserActivityRepository userActivityRepository,
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
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

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
            foreach (var salary in salaries)
            {
                var oldSalary = oldSalaries.FirstOrDefault(x => x.RowID == salary.RowID);
                // TODO: think of a better way to not call this query for every salary
                await ValidateSalaryIfAlreadyUsed(salary, oldSalary);
            }
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
                    UserActivityRepository.RecordTypeEdit,
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

        #endregion Private Methods
    }
}
