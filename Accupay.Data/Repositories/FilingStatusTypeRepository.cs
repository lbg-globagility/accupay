using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class FilingStatusTypeRepository
    {
        private readonly PayrollContext _context;

        public FilingStatusTypeRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FilingStatusType>> GetAllAsync()
        {
            return await _context.FilingStatusTypes.ToListAsync();
        }
    }
}