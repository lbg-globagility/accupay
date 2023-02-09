using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data.Repositories
{
    public class DateEntityRepository : IDateEntityRepository
    {
        private readonly PayrollContext _context;

        public DateEntityRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<DateEntity>> GetByDateRangeAsync(DateTime start, DateTime end) => await _context.Dates
            .AsNoTracking()
            .Where(t => t.Value.Date >= start)
            .Where(t => t.Value.Date <= end)
            .ToListAsync();

        public async Task<ICollection<DateEntity>> GetByYearAndMonthAsync(int year, int month) => await _context.Dates
            .AsNoTracking()
            .Where(t => t.Value.Date.Year == year)
            .Where(t => t.Value.Date.Month == month)
            .ToListAsync();

        public async Task<ICollection<DateEntity>> GetByYearAsync(int year) => await _context.Dates
            .AsNoTracking()
            .Where(t => t.Value.Date.Year == year)
            .ToListAsync();
    }
}
