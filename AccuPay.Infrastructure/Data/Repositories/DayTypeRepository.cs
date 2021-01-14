using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class DayTypeRepository : IDayTypeRepository
    {
        private readonly PayrollContext _context;

        public DayTypeRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(DayType dayType)
        {
            if (dayType.RowID == null)
                _context.DayTypes.Add(dayType);
            else
                _context.Entry(dayType).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<DayType>> GetAllAsync()
        {
            return await _context.DayTypes.ToListAsync();
        }

        public async Task<DayType> GetOrCreateRegularDayAsync()
        {
            var regularDayType = await _context.DayTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name == CalendarConstant.RegularDay);

            if (regularDayType == null)
            {
                regularDayType = DayType.CreateRegularDayType();
                await SaveAsync(regularDayType);
            }

            return regularDayType;
        }
    }
}
