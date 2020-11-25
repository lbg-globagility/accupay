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
    public class EmployeeDutyScheduleDataService : BaseSavableDataService<EmployeeDutySchedule>
    {
        private const string UserActivityName = "Shift Schedule";

        private readonly EmployeeDutyScheduleRepository _shiftRepository;
        private readonly UserActivityRepository _userActivityRepository;

        public EmployeeDutyScheduleDataService(
            EmployeeDutyScheduleRepository shiftRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(shiftRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Shift")
        {
            _shiftRepository = shiftRepository;
            _userActivityRepository = userActivityRepository;
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

            await SaveManyAsync(added: addedShifts, updated: updatedShifts);

            return new BatchApplyResult<EmployeeDutySchedule>(addedList: addedShifts, updatedList: updatedShifts);
        }

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

        protected override Task PostSaveManyAction(List<EmployeeDutySchedule> entities, List<EmployeeDutySchedule> oldEntities, SaveType saveType)
        {
            switch (saveType)
            {
                case SaveType.Insert:

                    foreach (var item in entities)
                    {
                        _userActivityRepository.RecordAdd(
                            item.CreatedBy.Value,
                            UserActivityName,
                            entityId: item.RowID.Value,
                            organizationId: item.OrganizationID.Value,
                            suffixIdentifier: CreateUserActivitySuffixIdentifier(item),
                            changedEmployeeId: item.EmployeeID.Value);
                    }

                    break;

                case SaveType.Update:

                    RecordUpdate(entities, oldEntities);
                    break;

                case SaveType.Delete:

                    foreach (var item in entities)
                    {
                        _userActivityRepository.RecordDelete(
                            item.LastUpdBy.Value,
                            UserActivityName,
                            entityId: item.RowID.Value,
                            organizationId: item.OrganizationID.Value,
                            suffixIdentifier: CreateUserActivitySuffixIdentifier(item),
                            changedEmployeeId: item.EmployeeID.Value);
                    }
                    break;

                default:
                    break;
            }

            return Task.CompletedTask;
        }

        private void RecordUpdate(List<EmployeeDutySchedule> updatedShifts, List<EmployeeDutySchedule> oldRecords)
        {
            foreach (var item in updatedShifts)
            {
                List<UserActivityItem> changes = new List<UserActivityItem>();
                var entityName = UserActivityName.ToLower();

                var oldShifts = oldRecords.Where(x => x.RowID == item.RowID).FirstOrDefault();

                if (oldShifts == null) continue;

                var suffixIdentifier = $"of shift{CreateUserActivitySuffixIdentifier(item)}.";

                if (item.StartTime != oldShifts.StartTime)
                {
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = item.RowID.Value,
                        Description = $"Updated start time from '{oldShifts.StartTime.ToStringFormat("hh:mm tt")}' to '{item.StartTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                        ChangedEmployeeId = oldShifts.EmployeeID.Value
                    });
                }
                if (item.EndTime != oldShifts.EndTime)
                {
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = item.RowID.Value,
                        Description = $"Updated end time from '{oldShifts.EndTime.ToStringFormat("hh:mm tt")}' to '{item.EndTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                        ChangedEmployeeId = oldShifts.EmployeeID.Value
                    });
                }
                if (item.BreakStartTime != oldShifts.BreakStartTime)
                {
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = item.RowID.Value,
                        Description = $"Updated break start from '{oldShifts.BreakStartTime.ToStringFormat("hh:mm tt")}' to '{item.BreakStartTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                        ChangedEmployeeId = oldShifts.EmployeeID.Value
                    });
                }
                if (item.BreakLength != oldShifts.BreakLength)
                {
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = item.RowID.Value,
                        Description = $"Updated break length from '{oldShifts.BreakLength}' to '{item.BreakLength}' {suffixIdentifier}",
                        ChangedEmployeeId = oldShifts.EmployeeID.Value
                    });
                }
                if (item.IsRestDay != oldShifts.IsRestDay)
                {
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = item.RowID.Value,
                        Description = $"Updated offset from '{oldShifts.IsRestDay}' to '{item.IsRestDay}' {suffixIdentifier}",
                        ChangedEmployeeId = oldShifts.EmployeeID.Value
                    });
                }

                if (changes.Any())
                {
                    _userActivityRepository.CreateRecord(
                        item.LastUpdBy.Value,
                        UserActivityName,
                        item.OrganizationID.Value,
                        UserActivityRepository.RecordTypeEdit,
                        changes);
                }
            }
        }

        private static string CreateUserActivitySuffixIdentifier(EmployeeDutySchedule shift)
        {
            return $" with date '{shift.DateSched.ToShortDateString()}'";
        }
    }
}
