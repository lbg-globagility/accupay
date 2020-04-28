using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class ShiftScheduleRepository
    {
        public async Task<IEnumerable<ShiftSchedule>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return await context.ShiftSchedules.
                                Include(l => l.Shift).
                                Where(l => l.OrganizationID == organizationId).
                                Where(l => timePeriod.Start <= l.EffectiveFrom).
                                Where(l => l.EffectiveTo <= timePeriod.End).
                                ToListAsync();
            }
        }
    }
}