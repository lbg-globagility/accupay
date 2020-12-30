using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class EmployeeDutyScheduleDataService : BaseEmployeeDataService<EmployeeDutySchedule>
    {
        private const string UserActivityName = "Shift Schedule";

        private readonly EmployeeDutyScheduleRepository _shiftRepository;

        public EmployeeDutyScheduleDataService(
            EmployeeDutyScheduleRepository shiftRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(shiftRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Shift")
        {
            _shiftRepository = shiftRepository;
        }

        public async Task<BatchApplyResult<EmployeeDutySchedule>> BatchApply(
            IEnumerable<ShiftModel> shiftModels,
            int organizationId,
            int currentlyLoggedInUserId)
        {
            var minDate = shiftModels.Min(x => x.Date);
            var maxDate = shiftModels.Max(x => x.Date);

            var datePeriod = new TimePeriod(minDate, maxDate);

            var employeeIds = shiftModels.Select(x => x.EmployeeId.Value).Distinct().ToArray();

            var existingShifts = await _shiftRepository
                .GetByEmployeeAndDatePeriodAsync(organizationId, employeeIds, datePeriod);

            List<EmployeeDutySchedule> addedShifts = new List<EmployeeDutySchedule>();
            List<EmployeeDutySchedule> updatedShifts = new List<EmployeeDutySchedule>();

            foreach (var shifModel in shiftModels)
            {
                var existingShift = existingShifts
                    .Where(x => x.EmployeeID == shifModel.EmployeeId)
                    .Where(x => x.DateSched == shifModel.Date)
                    .FirstOrDefault();

                if (existingShift != null)
                {
                    existingShift.StartTime = shifModel.StartTime;
                    existingShift.EndTime = shifModel.EndTime;
                    existingShift.BreakStartTime = shifModel.BreakTime;
                    existingShift.BreakLength = shifModel.BreakLength;

                    existingShift.IsRestDay = shifModel.IsRestDay;

                    updatedShifts.Add(existingShift);
                }
                else
                {
                    addedShifts.Add(shifModel.ToEmployeeDutySchedule(organizationId));
                }
            }

            await SaveManyAsync(
                currentlyLoggedInUserId: currentlyLoggedInUserId,
                added: addedShifts,
                updated: updatedShifts);

            return new BatchApplyResult<EmployeeDutySchedule>(addedList: addedShifts, updatedList: updatedShifts);
        }

        #region Overrides

        protected override string GetUserActivityName(EmployeeDutySchedule shift)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(EmployeeDutySchedule shift)
        {
            return $" with date '{shift.DateSched.ToShortDateString()}'";
        }

        protected override async Task SanitizeEntity(EmployeeDutySchedule shift, EmployeeDutySchedule oldShift, int changedByUserId)
        {
            await base.SanitizeEntity(
                entity: shift,
                oldEntity: oldShift,
                currentlyLoggedInUserId: changedByUserId);

            if (shift.DateSched < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

            if (shift.StartTime == null)
                throw new BusinessLogicException("Start Time is required.");

            if (shift.EndTime == null)
                throw new BusinessLogicException("End Time is required.");

            shift.ComputeShiftHours(_policy.ShiftBasedAutomaticOvertimePolicy);
        }

        protected override async Task AdditionalSaveManyValidation(List<EmployeeDutySchedule> entities, List<EmployeeDutySchedule> oldEntities, SaveType saveType)
        {
            var organizationEntities = entities.GroupBy(x => x.OrganizationID);

            foreach (var organization in organizationEntities)
            {
                if (organization.Key != null)
                    await CheckIfDataIsWithinClosedPayPeriod(organization.ToList().Select(x => x.DateSched).Distinct(), organization.Key.Value);
            }
        }

        protected override async Task RecordUpdate(EmployeeDutySchedule newValue, EmployeeDutySchedule oldValue)
        {
            List<UserActivityItem> changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(newValue)}.";

            if (newValue.StartTime != oldValue.StartTime)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = newValue.RowID.Value,
                    Description = $"Updated start time from '{oldValue.StartTime.ToStringFormat("hh:mm tt")}' to '{newValue.StartTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.EndTime != oldValue.EndTime)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = newValue.RowID.Value,
                    Description = $"Updated end time from '{oldValue.EndTime.ToStringFormat("hh:mm tt")}' to '{newValue.EndTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.BreakStartTime != oldValue.BreakStartTime)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = newValue.RowID.Value,
                    Description = $"Updated break start from '{oldValue.BreakStartTime.ToStringFormat("hh:mm tt")}' to '{newValue.BreakStartTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.BreakLength != oldValue.BreakLength)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = newValue.RowID.Value,
                    Description = $"Updated break length from '{oldValue.BreakLength}' to '{newValue.BreakLength}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.IsRestDay != oldValue.IsRestDay)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = newValue.RowID.Value,
                    Description = $"Updated offset from '{oldValue.IsRestDay}' to '{newValue.IsRestDay}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }

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
    }
}
