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
    public class SalaryDataService : BaseSavableDataService<Salary>
    {
        private readonly PaystubRepository _paystubRepository;
        private readonly SalaryRepository _salaryRepository;

        public SalaryDataService(
            SalaryRepository salaryRepository,
            PayPeriodRepository payPeriodRepository,
            PaystubRepository paystubRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(salaryRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Salary",
                entityNamePlural: "Salaries")
        {
            _paystubRepository = paystubRepository;
            _salaryRepository = salaryRepository;
        }

        protected override async Task SanitizeEntity(Salary salary, Salary oldSalary)
        {
            if (salary.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (salary.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

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

        protected override async Task AdditionalSaveManyValidation(List<Salary> salaries, List<Salary> oldEntities)
        {
            foreach (var salary in salaries)
            {
                var oldSalary = salaries.FirstOrDefault(x => x.RowID == salary.RowID);
                // TODO: think of a better way to not call this query for every salary
                await ValidateSalaryIfAlreadyUsed(salary, oldSalary);
            }
        }

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

        public async Task<List<Salary>> BatchApply(IReadOnlyCollection<SalaryImportModel> validRecords, int organizationId, int userId)
        {
            List<Salary> added = new List<Salary>();
            foreach (var validRecord in validRecords)
            {
                var salary = new Salary
                {
                    OrganizationID = organizationId,
                    EmployeeID = validRecord.EmployeeId,
                    CreatedBy = userId,
                    BasicSalary = validRecord.BasicSalary.Value,
                    AllowanceSalary = validRecord.AllowanceSalary.HasValue ? validRecord.AllowanceSalary.Value : 0,
                    EffectiveFrom = validRecord.EffectiveFrom.Value
                };

                added.Add(salary);
            }

            await _salaryRepository.SaveManyAsync(added);

            return added;
        }
    }
}
