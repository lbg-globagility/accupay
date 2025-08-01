using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class BranchRepository : IBranchRepository
    {
        private readonly PayrollContext _context;

        public BranchRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<Branch> GetByIdAsync(int id)
        {
            return await _context.Branches.FindAsync(id);
        }

        public async Task<ICollection<Branch>> GetManyByIdsAsync(int[] ids)
        {
            return await _context.Branches
                .Where(x => ids.Contains(x.RowID.Value))
                .ToListAsync();
        }

        public async Task DeleteAsync(Branch branch)
        {
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> CreateAsync(Branch branch)
        {
            if (await _context.Branches.
                    Where(b => b.Code.Trim().ToUpper() == branch.Code.Trim().ToUpper()).
                    AnyAsync())
            {
                throw new BusinessLogicException("Branch already exists.");
            }

            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();

            return branch.RowID;
        }

        public async Task UpdateAsync(Branch branch)
        {
            if (await _context.Branches.
                    Where(b => b.Code.Trim().ToUpper() == branch.Code.Trim().ToUpper()).
                    Where(b => b.RowID != branch.RowID)
                    .AnyAsync())
            {
                throw new BusinessLogicException("Branch already exists.");
            }

            _context.Entry(branch).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public ICollection<Branch> GetAll()
        {
            return _context.Branches.ToList();
        }

        public async Task<ICollection<Branch>> GetAllAsync()
        {
            return await _context.Branches.ToListAsync();
        }

        public async Task<bool> HasCalendarAsync(PayCalendar payCalendar)
        {
            return await _context.Branches
                .Where(b => b.CalendarID == payCalendar.RowID)
                .AnyAsync();
        }
    }
}
