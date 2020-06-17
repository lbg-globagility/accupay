using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AllowanceTypeRepository
    {
        private readonly PayrollContext _context;

        public AllowanceTypeRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task<AllowanceType> CreateAsync(AllowanceType allowanceType)
        {
            _context.AllowanceTypes.Add(allowanceType);

            await _context.SaveChangesAsync();

            return allowanceType;
        }

        public async Task UpdateAsync(AllowanceType allowanceType)
        {
            _context.Entry(allowanceType).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allowanceType = await GetByIdAsync(id);

            _context.Remove(allowanceType);

            await _context.SaveChangesAsync();
        }

        #endregion CRUD

        #region Single entity

        public async Task<AllowanceType> GetByIdAsync(int id)
        {
            return await _context.AllowanceTypes
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        #endregion Single entity

        #region List of entities

        public async Task<PaginatedListResult<AllowanceType>> GetPaginatedListAsync(PageOptions options,
            string searchTerm = "")
        {
            var query = _context.AllowanceTypes
                .OrderByDescending(x => x.Name)
                .ThenBy(x => x.DisplayString)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, searchTerm) ||
                    EF.Functions.Like(x.DisplayString, searchTerm));
            }

            var allowanceTypes = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<AllowanceType>(allowanceTypes, count);
        }

        #endregion List of entities
    }
}