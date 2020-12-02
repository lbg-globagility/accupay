using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;
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
            PolicyHelper policy) :

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
            int changedByUserId)
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
                    existingShift.LastUpd = DateTime.Now;
                    existingShift.LastUpdBy = changedByUserId;

                    updatedShifts.Add(existingShift);
                }
                else
                {
                    addedShifts.Add(
                        shifModel.ToEmployeeDutySchedule(
                            organizationId: organizationId,
                            changedByUserId: changedByUserId));
                }
            }

            await SaveManyAsync(
                changedByUserId: changedByUserId,
                added: addedShifts,
                updated: updatedShifts);

            return new BatchApplyResult<EmployeeDutySchedule>(addedList: addedShifts, updatedList: updatedShifts);
        }

        #region Overrides

        protected override string GetUserActivityName(EmployeeDutySchedule shift) => UserActivityName;

        protected override string CreateUserActivitySuffixIdentifier(EmployeeDutySchedule shift) =>
            $" with date '{shift.DateSched.ToShortDateString()}'";

        protected override Task SanitizeEntity(EmployeeDutySchedule shift, EmployeeDutySchedule oldShift)
        {
            if (shift.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (shift.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (shift.DateSched < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

            if (shift.StartTime == null)
                throw new BusinessLogicException("Start Time is required.");

            if (shift.EndTime == null)
                throw new BusinessLogicException("End Time is required.");

            if (shift.IsNewEntity && shift.CreatedBy == null)
                throw new BusinessLogicException("Created By is required.");

            if (!shift.IsNewEntity && shift.LastUpdBy == null)
                throw new BusinessLogicException("Last Updated By is required.");

            shift.ComputeShiftHours(_policy.ShiftBasedAutomaticOvertimePolicy);

            return Task.CompletedTask;
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

        protected override Task RecordUpdate(IReadOnlyCollection<EmployeeDutySchedule> updatedShifts, IReadOnlyCollection<EmployeeDutySchedule> oldRecords)
        {
            foreach (var newValue in updatedShifts)
            {
                var oldValue = oldRecords.Where(x => x.RowID == newValue.RowID).FirstOrDefault();
                if (oldValue == null) continue;

                RecordUpdate(newValue, oldValue);
            }

            return Task.CompletedTask;
        }

        protected override Task RecordUpdate(EmployeeDutySchedule newValue, EmployeeDutySchedule oldValue)
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
                _userActivityRepository.CreateRecord(
                    newValue.LastUpdBy.Value,
                    UserActivityName,
                    newValue.OrganizationID.Value,
                    UserActivityRepository.RecordTypeEdit,
                    changes);
            }

            return Task.CompletedTask;
        }

        #endregion Overrides
    }
}
