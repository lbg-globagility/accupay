using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PreviousEmployerRepository
    {
        private readonly PayrollContext _context;

        public PreviousEmployerRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PreviousEmployer>> GetListByEmployeeAsync(int employeeId)
        {
            return await _context.PreviousEmployers.
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }

        public async Task DeleteAsync(PreviousEmployer previousEmployer)
        {
            _context.PreviousEmployers.Remove(previousEmployer);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(PreviousEmployer previousEmployer)
        {
            _context.PreviousEmployers.Add(previousEmployer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PreviousEmployer previousEmployer)
        {
            _context.Entry(previousEmployer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}