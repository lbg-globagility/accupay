using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class EmployeeDutyScheduleDataService
    {
        private readonly EmployeeDutyScheduleRepository _repository;

        public EmployeeDutyScheduleDataService(EmployeeDutyScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task DeleteAsync(int id)
        {
            var shift = await _repository.GetByIdAsync(id);

            if (shift == null)
                throw new BusinessLogicException("Shift does not exists.");

            await _repository.DeleteAsync(shift);
        }

        public async Task CreateAsync(EmployeeDutySchedule shift)
        {
            if (shift == null)
                throw new BusinessLogicException("Invalid shift.");

            if (shift.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            var key = new EmployeeDutyScheduleRepository.CompositeKey(shift.EmployeeID.Value, shift.DateSched);
            var existingShift = await _repository.GetByIdAsync(key);

            if (existingShift != null)
                throw new BusinessLogicException("Employee already has a shift for that day.");

            await _repository.CreateAsync(shift);
        }

        public async Task UpdateAsync(EmployeeDutySchedule shift)
        {
            if (shift.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            await _repository.UpdateAsync(shift);
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

            var existingShifts = await _repository
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

                    existingShift.ComputeShiftHours();
                    updatedShifts.Add(existingShift);
                }
                else
                {
                    addedShifts.Add(shifModel.ToEmployeeDutySchedule(organizationId: organizationId, userId: userId));
                }
            }

            await _repository.ChangeManyAsync(addedShifts: addedShifts, updatedShifts: updatedShifts);

            return new BatchApplyResult<EmployeeDutySchedule>(addedList: addedShifts, updatedList: updatedShifts);
        }
    }
}