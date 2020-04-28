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
        public async Task<IEnumerable<EmployeeDutySchedule>> GetByDatePeriodAsync(int organizationId,
                                                                                TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return await context.EmployeeDutySchedules.
                                Where(l => l.OrganizationID == organizationId).
                                Where(l => timePeriod.Start <= l.DateSched).
                                Where(l => l.DateSched <= timePeriod.End).
                                ToListAsync();
            }
        }
    }
}