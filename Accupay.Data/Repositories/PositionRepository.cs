using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PositionRepository : BaseRepository
    {
        private readonly PayrollContext _context;

        public PositionRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        internal async Task DeleteAsync(Position position)
        {
            _context.Remove(position);

            await _context.SaveChangesAsync();
        }

        internal async Task SaveAsync(Position position)
        {
            if (IsNewEntity(position.RowID))
            {
                _context.Positions.Add(position);
            }
            else
            {
                _context.Entry(position).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        internal async Task<Position> GetByIdAsync(int positionId)
        {
            return await _context.Positions.
                            Where(p => p.RowID == positionId).
                            FirstOrDefaultAsync();
        }

        internal async Task<Position> GetByIdWithDivisionAsync(int positionId)
        {
            return await _context.Positions.
                            Include(p => p.Division).
                            Where(p => p.RowID == positionId).
                            FirstOrDefaultAsync();
        }

        public async Task<Position> GetByNameAsync(int organizationId, string positionName)
        {
            return await _context.Positions
                            .Where(p => p.OrganizationID == organizationId)
                            .Where(p => p.Name.Trim().ToLower() == positionName.ToTrimmedLowerCase())
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
        }

        public async Task<List<Position>> GetAllByNameAsync(string positionName)
        {
            return await _context.Positions.
                Where(p => p.Name.Trim().ToLower() == positionName.ToTrimmedLowerCase()).
                ToListAsync();
        }

        #endregion Single entity

        #region List of entities

        internal async Task<IEnumerable<Position>> GetAllAsync(int organizationId)
        {
            return await _context.Positions.
                Where(p => p.OrganizationID == organizationId).
                ToListAsync();
        }

        public async Task<PaginatedListResult<Position>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.Positions
                                .Include(x => x.Division)
                                .Where(x => x.OrganizationID == organizationId)
                                .OrderBy(x => x.Division.Name)
                                .ThenBy(x => x.Name)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, searchTerm) ||
                    EF.Functions.Like(x.Division.Name, searchTerm));
            }

            var positions = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<Position>(positions, count);
        }

        #endregion List of entities

        #endregion Queries
    }
}