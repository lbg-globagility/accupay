using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Interfaces;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services.Imports.Overtimes;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class OvertimeDataService : BaseDailyPayrollDataService<Overtime>
    {
        private const string USER_ACTIVITY_ENTITY_NAME = "OVERTIME";

        private readonly OvertimeRepository _overtimeRepository;
        private readonly UserActivityRepository _userActivityRepository;

        private bool nullableStartTime;

        public OvertimeDataService(
            OvertimeRepository overtimeRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(overtimeRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Overtime")
        {
            _overtimeRepository = overtimeRepository;
            _userActivityRepository = userActivityRepository;
        }

        public async Task DeleteManyAsync(IEnumerable<int> overtimeIds, int organizationId, int userId)
        {
            var overtimes = await _overtimeRepository.GetManyByIdsAsync(overtimeIds.ToArray());

            await _overtimeRepository.DeleteManyAsync(overtimeIds);

            foreach (var overtime in overtimes)
            {
                _userActivityRepository.RecordDelete(
                    userId,
                    USER_ACTIVITY_ENTITY_NAME,
                    entityId: overtime.RowID.Value,
                    organizationId: organizationId,
                    suffixIdentifier: CreateUserActivitySuffixIdentifier(overtime),
                    changedEmployeeId: overtime.EmployeeID.Value);
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        protected override async Task SanitizeEntity(Overtime overtime, Overtime oldOvertime)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (overtime.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (overtime.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (overtime.OTStartDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

            if (!_policy.UseMassOvertime)
                if (overtime.OTStartTime == null)
                    throw new BusinessLogicException("Start Time is required.");

            if (overtime.OTEndTime == null)
                throw new BusinessLogicException("End Time is required.");

            if (new string[] { Overtime.StatusPending, Overtime.StatusApproved }
                            .Contains(overtime.Status) == false)
            {
                throw new BusinessLogicException("Status is not valid.");
            }

            if (overtime.OTStartTime.HasValue)
                overtime.OTStartTime = overtime.OTStartTime.Value.StripSeconds();

            overtime.OTEndTime = overtime.OTEndTime.Value.StripSeconds();

            if (overtime.OTStartTime == overtime.OTEndTime)
                throw new BusinessLogicException("End Time cannot be equal to Start Time");

            overtime.UpdateEndDate();
        }

        public async Task<List<Overtime>> BatchApply(IReadOnlyCollection<OvertimeImportModel> validRecords, int organizationId, int userId)
        {
            List<Overtime> overtimes = new List<Overtime>();

            foreach (var ot in validRecords)
            {
                overtimes.Add(new Overtime()
                {
                    CreatedBy = userId,
                    EmployeeID = ot.EmployeeID,
                    OrganizationID = organizationId,
                    OTEndTimeFull = ot.EndTime.Value,
                    OTStartDate = ot.StartDate.Value,
                    OTStartTimeFull = ot.StartTime.Value,
                    Status = Overtime.StatusPending
                });
            }

            await _overtimeRepository.SaveManyAsync(overtimes);

            return overtimes;
        }

        public async Task GenerateOvertimeByShift(IEnumerable<IShift> modifiedShifts, List<int> employeeIds, int organizationId, int userId)
        {
            if (!modifiedShifts.Any())
                return;

            var timePeriod = new TimePeriod(modifiedShifts.Min(s => s.Date), modifiedShifts.Max(s => s.Date));

            (List<Overtime> saveOvertimes, List<Overtime> deleteOvertimes, List<Overtime> existingOvertimes) =
                CreateOvertimesByShift(modifiedShifts, organizationId, userId, employeeIds, timePeriod);

            if (saveOvertimes.Any())
            {
                IList<Overtime> newOvertimes = saveOvertimes.Where(ot => IsNewEntity(ot.RowID)).ToList();
                IList<Overtime> updatedOvertimes = saveOvertimes.Where(ot => !IsNewEntity(ot.RowID)).ToList();

                await SaveManyAsync(saveOvertimes);

                // TODO: Insert user activity
                CreateInsertAndUpdateUserActivities(
                    newOvertimes: newOvertimes,
                    updatedOvertimes: updatedOvertimes,
                    existingOvertimes: existingOvertimes,
                    organizationId: organizationId,
                    userId: userId);
            }

            if (deleteOvertimes.Any())
                await DeleteManyAsync(deleteOvertimes.Select(ot => ot.RowID.Value), organizationId, userId);
        }

        public void GenerateUpdateUserActivityItems(Overtime newOvertime, Overtime oldOvertime, int organizationId, int userId)
        {
            var changes = new List<UserActivityItem>();

            var suffixIdentifier = $"of overtime {CreateUserActivitySuffixIdentifier(oldOvertime)}.";

            if (newOvertime.OTStartDate != oldOvertime.OTStartDate)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldOvertime.RowID.Value,
                    Description = $"Updated start date from '{oldOvertime.OTStartDate.ToShortDateString()}' to '{newOvertime.OTStartDate.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldOvertime.EmployeeID.Value
                });
            if (newOvertime.OTStartTime != oldOvertime.OTStartTime)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldOvertime.RowID.Value,
                    Description = $"Updated start time from '{oldOvertime.OTStartTime.ToStringFormat("hh:mm tt")}' to '{newOvertime.OTStartTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldOvertime.EmployeeID.Value
                });
            if (newOvertime.OTEndTime != oldOvertime.OTEndTime)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldOvertime.RowID.Value,
                    Description = $"Updated end time from '{oldOvertime.OTEndTime.ToStringFormat("hh:mm tt")}' to '{newOvertime.OTEndTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldOvertime.EmployeeID.Value
                });
            if (newOvertime.Reason != oldOvertime.Reason)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldOvertime.RowID.Value,
                    Description = $"Updated reason from '{oldOvertime.Reason}' to '{newOvertime.Reason}' {suffixIdentifier}",
                    ChangedEmployeeId = oldOvertime.EmployeeID.Value
                });
            if (newOvertime.Comments != oldOvertime.Comments)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldOvertime.RowID.Value,
                    Description = $"Updated comments from '{oldOvertime.Comments}' to '{newOvertime.Comments}' {suffixIdentifier}",
                    ChangedEmployeeId = oldOvertime.EmployeeID.Value
                });
            if (newOvertime.Status != oldOvertime.Status)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldOvertime.RowID.Value,
                    Description = $"Updated status from '{oldOvertime.Status}' to '{newOvertime.Status}' {suffixIdentifier}",
                    ChangedEmployeeId = oldOvertime.EmployeeID.Value
                });

            if (changes.Any())
                _userActivityRepository.CreateRecord(userId, USER_ACTIVITY_ENTITY_NAME, organizationId, UserActivityRepository.RecordTypeEdit, changes);
        }

        #region Private Methods

        private void CreateInsertAndUpdateUserActivities(
            IList<Overtime> newOvertimes,
            IList<Overtime> updatedOvertimes,
            IList<Overtime> existingOvertimes,
            int organizationId,
            int userId)
        {
            foreach (var overtime in newOvertimes)
            {
                var suffixIdentifier = CreateUserActivitySuffixIdentifier(overtime);
                _userActivityRepository.RecordAdd(
                    userId,
                    USER_ACTIVITY_ENTITY_NAME,
                    entityId: overtime.RowID.Value,
                    organizationId: organizationId,
                    changedEmployeeId: overtime.EmployeeID.Value,
                    suffixIdentifier: suffixIdentifier);
            }

            foreach (var overtime in updatedOvertimes)
            {
                var oldOvertime = existingOvertimes.Where(ot => ot.RowID == overtime.RowID).FirstOrDefault();
                if (oldOvertime == null) continue;

                GenerateUpdateUserActivityItems(
                    newOvertime: overtime,
                    oldOvertime: oldOvertime,
                    organizationId: organizationId,
                    userId: userId);
            }
        }

        private static string CreateUserActivitySuffixIdentifier(Overtime overtime)
        {
            return $" with date '{overtime.OTStartDate.ToShortDateString()}' and time period '{overtime.OTStartTime.ToStringFormat("hh:mm tt")} to {overtime.OTEndTime.ToStringFormat("hh:mm tt")}'";
        }

        private (List<Overtime> saveOvertimes, List<Overtime> deleteOvertimes, List<Overtime> existingOvertimes) CreateOvertimesByShift(
            IEnumerable<IShift> shiftSchedSaveList,
            int organizationId,
            int userId,
            List<int> employeeIds,
            TimePeriod timePeriod)
        {
            var saveOvertimes = new List<Overtime>();
            var deleteOvertimes = new List<Overtime>();

            var existingOvertimes = _overtimeRepository.GetByEmployeeIDsAndDatePeriod(organizationId, employeeIds, timePeriod);

            shiftSchedSaveList.ToList().ForEach(shiftModel =>
            {
                var employeeID = shiftModel.EmployeeId;
                var dateValue = shiftModel.Date;

                var overtimesThisDay = existingOvertimes
                    .Where(ot => ot.EmployeeID == employeeID)
                    .Where(ot => ot.OTStartDate == dateValue)
                    .ToList();

                var overtime = overtimesThisDay.FirstOrDefault();
                if (overtime == null)
                {
                    overtime = Overtime.NewOvertime(shiftModel, organizationId, userId);
                }
                else
                {
                    overtime = overtime.CloneJson();
                }

                var hasShiftPeriod = shiftModel.StartTime.HasValue && shiftModel.EndTime.HasValue;
                if (hasShiftPeriod)
                {
                    var shiftTimePeriod = TimePeriod.FromTime(shiftModel.StartTime.Value, shiftModel.EndTime.Value, shiftModel.Date);
                    var validOTStartTime = _policy.ShiftBasedAutomaticOvertimePolicy.GetExpectedEndTime(shiftTimePeriod.Start, shiftModel.BreakLength);

                    overtime.OTStartTimeFull = validOTStartTime;
                    overtime.OTEndTimeFull = validOTStartTime;

                    var shiftPeriod = new TimePeriod(shiftTimePeriod.Start, shiftTimePeriod.End);
                    if (shiftPeriod.TotalHours >= (_policy.ShiftBasedAutomaticOvertimePolicy.DefaultWorkHoursAndMinimumOTHours + shiftModel.BreakLength))
                    {
                        overtime.OTEndTimeFull = shiftTimePeriod.End;
                    }
                }

                var overtimeId = 0;

                if (overtime.OTStartTime != overtime.OTEndTime && hasShiftPeriod)
                {
                    if (overtime.RowID.HasValue)
                        overtimeId = overtime.RowID.Value;

                    overtime.LastUpdBy = userId;

                    saveOvertimes.Add(overtime);
                }

                var discardOthers = overtimesThisDay
                    .Where(ot => overtimeId != ot.RowID)
                    .ToList();
                discardOthers.ForEach(ot => deleteOvertimes.Add(ot));
            });

            return (
                saveOvertimes: saveOvertimes,
                deleteOvertimes: deleteOvertimes,
                existingOvertimes: existingOvertimes.ToList());
        }

        #endregion Private Methods
    }
}
