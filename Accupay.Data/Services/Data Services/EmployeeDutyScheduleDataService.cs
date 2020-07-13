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

        public async Task ChangeManyAsync(
            List<EmployeeDutySchedule> added,
            List<EmployeeDutySchedule> updated,
            List<EmployeeDutySchedule> deleted)
        {
            if (added == null && updated == null && deleted == null)
                throw new BusinessLogicException("No shifts to be saved.");

            // TODO: validations
            await _repository.ChangeManyAsync(
                addedShifts: added,
                updatedShifts: updated,
                deletedShifts: deleted);
        }

        public async Task<(ICollection<Employee> employees, int total, ICollection<EmployeeDutySchedule>)> ListByEmployeeAsync(int organizationId, ShiftsByEmployeePageOptions options)
        {
            return await _repository.ListByEmployeeAsync(organizationId, options);
        }
    }
}