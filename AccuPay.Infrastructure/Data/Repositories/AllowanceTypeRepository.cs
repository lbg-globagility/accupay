using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class AllowanceTypeRepository : IAllowanceTypeRepository
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

        public async Task<List<AllowanceType>> CreateManyAsync(List<AllowanceType> notYetExistsAllowanceTypes)
        {
            _context.AllowanceTypes.AddRange(notYetExistsAllowanceTypes);

            await _context.SaveChangesAsync();

            return notYetExistsAllowanceTypes;
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

        public async Task<PaginatedList<AllowanceType>> GetPaginatedListAsync(PageOptions options,
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

            return new PaginatedList<AllowanceType>(allowanceTypes, count);
        }

        public async Task<ICollection<AllowanceType>> GetAllAsync()
        {
            return await _context.AllowanceTypes
                .ToListAsync();
        }

        #endregion List of entities
    }
}
