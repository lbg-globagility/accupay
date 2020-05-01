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
                return await CreateBaseQueryByDatePeriod(organizationId, timePeriod, context).
                    Where(x => employeeIds.Contains(x.EmployeeID.Value)).
                    Where(x => x.StartTime != null && x.EndTime != null).
                    ToListAsync();
            }
        }

        private static IQueryable<EmployeeDutySchedule> CreateBaseQueryByDatePeriod(int organizationId,
                                                                                    TimePeriod timePeriod,
                                                                                    PayrollContext context)
        {
            return context.EmployeeDutySchedules.
                            Where(l => l.OrganizationID == organizationId).
                            Where(l => timePeriod.Start <= l.DateSched).
                            Where(l => l.DateSched <= timePeriod.End);
        }
    }
}