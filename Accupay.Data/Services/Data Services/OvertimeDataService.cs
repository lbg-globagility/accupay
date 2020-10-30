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
        private readonly OvertimeRepository _overtimeRepository;
        private bool nullableStartTime;

        public OvertimeDataService(
            OvertimeRepository overtimeRepository,
            PayPeriodRepository payPeriodRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(overtimeRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Overtime")
        {
            _overtimeRepository = overtimeRepository;
        }

        public async Task DeleteManyAsync(IEnumerable<int> overtimeIds)
        {
            await _overtimeRepository.DeleteManyAsync(overtimeIds);
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

            if (!nullableStartTime)
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

        public async Task GenerateOvertimeByShift(IEnumerable<IShift> shiftSchedSaveList, List<int> employeeIds, TimePeriod timePeriod, int organizationId, int userId)
        {
            if (!shiftSchedSaveList.Any() || timePeriod.Start > timePeriod.End)
                return;

            var overtimes = _overtimeRepository.GetByEmployeeIDsAndDatePeriod(organizationId, employeeIds, timePeriod);

            var saveOvertimes = new List<Overtime>();
            var deleteOvertimes = new List<Overtime>();

            Func<DateTime?, decimal, DateTime?> getOTStartTime = (DateTime? starting, decimal breakLength) =>
            {
                return _policy.ShiftBasedAutomaticOvertimePolicy.GetExpectedEndTime(starting, breakLength);
            };

            Func<IShift, Overtime> createNewOvertime = (IShift shiftModel) =>
            {
                return new Overtime()
                {
                    Created = DateTime.Now,
                    CreatedBy = userId,
                    EmployeeID = shiftModel.EmployeeId,
                    OrganizationID = organizationId,
                    OTStartDate = shiftModel.Date,
                    OTEndDate = shiftModel.Date,
                    Status = Overtime.StatusApproved
                };
            };

            shiftSchedSaveList.ToList().ForEach(shiftModel =>
            {
                var employeeID = shiftModel.EmployeeId;
                var dateValue = shiftModel.Date;

                var overtimesThisDay = overtimes
                    .Where(ot => ot.EmployeeID == employeeID)
                    .Where(ot => ot.OTStartDate == dateValue)
                    .ToList();

                var overtime = overtimesThisDay.FirstOrDefault();
                if (overtime == null)
                    overtime = createNewOvertime(shiftModel);

                var hasShiftPeriod = shiftModel.StartTime.HasValue && shiftModel.EndTime.HasValue;
                if (hasShiftPeriod)
                {
                    var shiftTimePeriod = TimePeriod.FromTime(shiftModel.StartTime.Value, shiftModel.EndTime.Value, shiftModel.Date);
                    var validOTStartTime = getOTStartTime(shiftTimePeriod.Start, shiftModel.BreakLength);

                    overtime.OTStartTimeFull = validOTStartTime;
                    overtime.OTEndTimeFull = shiftTimePeriod.End;
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

            if (saveOvertimes.Any())
                await SaveManyAsync(saveOvertimes);

            if (deleteOvertimes.Any())
                await DeleteManyAsync(deleteOvertimes.Select(ot => ot.RowID.Value));
        }

        #region Other Methods

        public void CheckMassOvertimeFeature(bool isMassOvertime)
        {
            nullableStartTime = isMassOvertime;
        }

        #endregion Other Methods
    }
}