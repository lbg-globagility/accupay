using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class DayTypeRepository
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
    }
}