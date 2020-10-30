using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class EmployeeDutyScheduleDataService : BaseDataService
    {
        private readonly EmployeeDutyScheduleRepository _shiftRepository;

        public EmployeeDutyScheduleDataService(
            EmployeeDutyScheduleRepository shiftRepository,
            PayPeriodRepository payPeriodRepository,
            PolicyHelper policy) :

            base(payPeriodRepository,
                policy)
        {
            _shiftRepository = shiftRepository;
        }

        public async Task<BatchApplyResult<EmployeeDutySchedule>> BatchApply(
            IEnumerable<ShiftModel> shiftModels,
            int organizationId,
            int userId)
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

                    updatedShifts.Add(existingShift);
                }
                else
                {
                    addedShifts.Add(shifModel.ToEmployeeDutySchedule(organizationId: organizationId, userId: userId));
                }
            }

            addedShifts.ForEach(x => SanitizeEntity(x));
            updatedShifts.ForEach(x => SanitizeEntity(x));

            await _shiftRepository.ChangeManyAsync(added: addedShifts, updated: updatedShifts);

            return new BatchApplyResult<EmployeeDutySchedule>(addedList: addedShifts, updatedList: updatedShifts);
        }

        public async Task ChangeManyAsync(
            int organizationId,
            List<EmployeeDutySchedule> added = null,
            List<EmployeeDutySchedule> updated = null,
            List<EmployeeDutySchedule> deleted = null)
        {
            if (added == null && updated == null && deleted == null)
                throw new BusinessLogicException("No shifts to be saved.");

            if (added != null)
            {
                added.ForEach(x => SanitizeEntity(x));
                await CheckIfDataIsWithinClosedPayPeriod(added.Select(x => x.DateSched).Distinct(), organizationId);
            }

            if (updated != null)
            {
                updated.ForEach(x => SanitizeEntity(x));
                await CheckIfDataIsWithinClosedPayPeriod(updated.Select(x => x.DateSched).Distinct(), organizationId);
            }

            if (deleted != null)
            {
                await CheckIfDataIsWithinClosedPayPeriod(deleted.Select(x => x.DateSched).Distinct(), organizationId);
            }

            await _shiftRepository.ChangeManyAsync(
                added: added,
                updated: updated,
                deleted: deleted);
        }

        private void SanitizeEntity(EmployeeDutySchedule shift)
        {
            if (shift == null)
                throw new BusinessLogicException("Invalid data.");

            if (shift.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (shift.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (shift.StartTime == null)
                throw new BusinessLogicException("Start Time is required.");

            if (shift.EndTime == null)
                throw new BusinessLogicException("End Time is required.");

            shift.ComputeShiftHours(_policy.ShiftBasedAutomaticOvertimePolicy);
        }
    }
}