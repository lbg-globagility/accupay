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
        private readonly PayrollContext _context;

        public ShiftScheduleRepository(PayrollContext context)
        {
            _context = context;
        }

        public IEnumerable<ShiftSchedule> GetByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            return _context.ShiftSchedules.
                            Include(x => x.Shift).
                            Where(x => x.OrganizationID == organizationId).
                            Where(x => x.EffectiveFrom <= timePeriod.End).
                            Where(x => timePeriod.Start <= x.EffectiveTo).
                            ToList();
        }

        public async Task<IEnumerable<ShiftSchedule>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            return await _context.ShiftSchedules.
                            Include(x => x.Shift).
                            Where(x => x.OrganizationID == organizationId).
                            Where(x => x.EffectiveFrom <= timePeriod.End).
                            Where(x => timePeriod.Start <= x.EffectiveTo).
                            ToListAsync();
        }
    }
}