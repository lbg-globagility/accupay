using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class BranchRepository
    {
        public IEnumerable<Branch> GetAll()
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Branches.ToList();
            }
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Branches.ToListAsync();
            }
        }

        public async Task<int?> CreateAsync(Branch branch)
        {
            using (PayrollContext context = new PayrollContext())
            {
                if (await context.Branches.
                        Where(b => b.Code.Trim().ToUpper() == branch.Code.Trim().ToUpper()).
                        AnyAsync())
                {
                    throw new AccuPayRepositoryException("Branch already exists.");
                }

                context.Branches.Add(branch);
                await context.SaveChangesAsync();

                return branch.RowID;
            }
        }

        public async Task UpdateAsync(Branch branch)
        {
            using (PayrollContext context = new PayrollContext())
            {
                if (await context.Branches.
                        Where(b => b.Code.Trim().ToUpper() == branch.Code.Trim().ToUpper()).
                        Where(b => b.RowID != branch.RowID)
                        .AnyAsync())
                {
                    throw new AccuPayRepositoryException("Branch already exists.");
                }

                context.Entry(branch).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Branch branch)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Branches.Remove(branch);
                await context.SaveChangesAsync();
            }
        }
    }
}