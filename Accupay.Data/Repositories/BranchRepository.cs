using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class BranchRepository
    {
        private readonly PayrollContext _context;

        public BranchRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<Branch> GetById(int id)
        {
            return await _context.Branches.FindAsync(id);
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

        public IEnumerable<Branch> GetAll()
        {
            return _context.Branches.ToList();
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _context.Branches.ToListAsync();
        }
    }
}
