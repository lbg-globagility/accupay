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
        #region CRUD

        public async Task ChangeManyAsync(List<EmployeeDutySchedule> addedShifts = null,
                                        List<EmployeeDutySchedule> updatedShifts = null,
                                        List<EmployeeDutySchedule> deletedShifts = null)
        {
            using (var context = new PayrollContext())
            {
                if (addedShifts != null)
                {
                    addedShifts.ForEach(shift =>
                    {
                        context.Entry(shift).State = EntityState.Added;
                    });
                }

                if (updatedShifts != null)
                {
                    updatedShifts.ForEach(shift =>
                    {
                        context.Entry(shift).State = EntityState.Modified;
                    });
                }

                if (deletedShifts != null)
                {
                    deletedShifts = deletedShifts.
                                    GroupBy(x => x.RowID).
                                    Select(x => x.FirstOrDefault()).
                                    ToList();
                    context.EmployeeDutySchedules.RemoveRange(deletedShifts);
                }

                await context.SaveChangesAsync();
            }
        }

        #endregion CRUD

        #region Queries

        public IEnumerable<EmployeeDutySchedule> GetByDatePeriod(int organizationId,
                                                                TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return CreateBaseQueryByDatePeriod(organizationId, timePeriod, context).ToList();
            }
        }

        public async Task<IEnumerable<EmployeeDutySchedule>> GetByDatePeriodAsync(int organizationId,
                                                                                TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return await CreateBaseQueryByDatePeriod(organizationId, timePeriod, context).ToListAsync();
            }
        }

        public async Task<IEnumerable<EmployeeDutySchedule>> GetByMultipleEmployeeAndDatePeriodAsync(
                                                                                int organizationId,
                                                                                int[] employeeIds,
                                                                                TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return await CreateBaseQueryByMultipleEmployeeDatePeriod(organizationId,
                                                                        employeeIds,
                                                                        timePeriod,
                                                                        context).
                    ToListAsync();
            }
        }

        public async Task<IEnumerable<EmployeeDutySchedule>> GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(
                                                                                int organizationId,
                                                                                int[] employeeIds,
                                                                                TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return await CreateBaseQueryByMultipleEmployeeDatePeriod(organizationId,
                                                                        employeeIds,
                                                                        timePeriod,
                                                                        context).
                    Include(x => x.Employee).
                    ToListAsync();
            }
        }

        #endregion Queries

        private static IQueryable<EmployeeDutySchedule> CreateBaseQueryByDatePeriod(int organizationId,
                                                                                    TimePeriod timePeriod,
                                                                                    PayrollContext context)
        {
            return context.EmployeeDutySchedules.
                            Where(l => l.OrganizationID == organizationId).
                            Where(l => timePeriod.Start <= l.DateSched).
                            Where(l => l.DateSched <= timePeriod.End);
        }

        private static IQueryable<EmployeeDutySchedule> CreateBaseQueryByMultipleEmployeeDatePeriod(
                                                                                int organizationId,
                                                                                int[] employeeIds,
                                                                                TimePeriod timePeriod,
                                                                                PayrollContext context)
        {
            return CreateBaseQueryByDatePeriod(organizationId, timePeriod, context).
                    Where(x => employeeIds.Contains(x.EmployeeID.Value));
        }
    }
}