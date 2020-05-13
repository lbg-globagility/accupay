using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class DisciplinaryActionRepository
    {
        private readonly PayrollContext _context;

        public DisciplinaryActionRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DisciplinaryAction>> GetListByEmployeeAsync(int employeeId)
        {
            return await _context.DisciplinaryActions.
                                Include(x => x.Finding).
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }

        public async Task DeleteAsync(DisciplinaryAction disciplinaryAction)
        {
            _context.DisciplinaryActions.Remove(disciplinaryAction);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(DisciplinaryAction disciplinaryAction)
        {
            _context.DisciplinaryActions.Add(disciplinaryAction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DisciplinaryAction disciplinaryAction)
        {
            _context.Entry(disciplinaryAction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}