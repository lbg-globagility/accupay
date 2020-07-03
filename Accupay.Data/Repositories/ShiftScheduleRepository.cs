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

        public ICollection<ShiftSchedule> GetByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                .ToList();
        }

        public async Task<ICollection<ShiftSchedule>> GetByDatePeriodAsync(int organizationId, TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                .ToListAsync();
        }

        public async Task<ICollection<ShiftSchedule>> GetByEmployeeAndDatePeriodAsync(int organizationId, int[] employeeIds, TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                .Where(s => employeeIds.Contains(s.EmployeeID.Value))
                .ToListAsync();
        }

        private IQueryable<ShiftSchedule> CreateBaseQueryByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return _context.ShiftSchedules
                .Include(x => x.Shift)
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => x.EffectiveFrom <= datePeriod.End)
                .Where(x => datePeriod.Start <= x.EffectiveTo);
        }
    }
}