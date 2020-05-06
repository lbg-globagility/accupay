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
        public IEnumerable<ShiftSchedule> GetByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return context.ShiftSchedules.
                                Include(x => x.Shift).
                                Where(x => x.OrganizationID == organizationId).
                                Where(x => x.EffectiveFrom <= timePeriod.End).
                                Where(x => timePeriod.Start <= x.EffectiveTo).
                                ToList();
            }
        }

        public async Task<IEnumerable<ShiftSchedule>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return await context.ShiftSchedules.
                                Include(x => x.Shift).
                                Where(x => x.OrganizationID == organizationId).
                                Where(x => x.EffectiveFrom <= timePeriod.End).
                                Where(x => timePeriod.Start <= x.EffectiveTo).
                                ToListAsync();
            }
        }
    }
}