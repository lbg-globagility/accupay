using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class EmployeeDutyScheduleRepository
    {
        private readonly PayrollContext _context;

        public EmployeeDutyScheduleRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task ChangeManyAsync(List<EmployeeDutySchedule> addedShifts = null,
                                        List<EmployeeDutySchedule> updatedShifts = null,
                                        List<EmployeeDutySchedule> deletedShifts = null)
        {
            if (addedShifts != null)
            {
                addedShifts.ForEach(shift =>
                {
                    _context.Entry(shift).State = EntityState.Added;
                });
            }

            if (updatedShifts != null)
            {
                updatedShifts.ForEach(shift =>
                {
                    _context.Entry(shift).State = EntityState.Modified;
                });
            }

            if (deletedShifts != null)
            {
                deletedShifts = deletedShifts.
                                GroupBy(x => x.RowID).
                                Select(x => x.FirstOrDefault()).
                                ToList();
                _context.EmployeeDutySchedules.RemoveRange(deletedShifts);
            }

            await _context.SaveChangesAsync();
        }

        #endregion CRUD

        #region Queries

        public IEnumerable<EmployeeDutySchedule> GetByDatePeriod(int organizationId,
                                                                TimePeriod timePeriod)
        {
            return CreateBaseQueryByDatePeriod(organizationId, timePeriod).
                    ToList();
        }

        public async Task<IEnumerable<EmployeeDutySchedule>> GetByDatePeriodAsync(int organizationId,
                                                                                TimePeriod timePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, timePeriod).
                        ToListAsync();
        }

        public async Task<IEnumerable<EmployeeDutySchedule>> GetByMultipleEmployeeAndDatePeriodAsync(
                                                                                int organizationId,
                                                                                int[] employeeIds,
                                                                                TimePeriod timePeriod)
        {
            return await CreateBaseQueryByMultipleEmployeeDatePeriod(organizationId,
                                                                    employeeIds,
                                                                    timePeriod).
                ToListAsync();
        }

        public async Task<IEnumerable<EmployeeDutySchedule>> GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(
                                                                                int organizationId,
                                                                                int[] employeeIds,
                                                                                TimePeriod timePeriod)
        {
            return await CreateBaseQueryByMultipleEmployeeDatePeriod(organizationId,
                                                                    employeeIds,
                                                                    timePeriod).
                Include(x => x.Employee).
                ToListAsync();
        }

        #endregion Queries

        private IQueryable<EmployeeDutySchedule> CreateBaseQueryByMultipleEmployeeDatePeriod(
                                                                                int organizationId,
                                                                                int[] employeeIds,
                                                                                TimePeriod timePeriod)
        {
            return CreateBaseQueryByDatePeriod(organizationId, timePeriod).
                    Where(x => employeeIds.Contains(x.EmployeeID.Value));
        }

        private IQueryable<EmployeeDutySchedule> CreateBaseQueryByDatePeriod(int organizationId,
                                                                                    TimePeriod timePeriod)
        {
            return _context.EmployeeDutySchedules.
                            Where(l => l.OrganizationID == organizationId).
                            Where(l => timePeriod.Start <= l.DateSched).
                            Where(l => l.DateSched <= timePeriod.End);
        }
    }
}