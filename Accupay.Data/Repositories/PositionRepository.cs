using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PositionRepository : SavableRepository<Position>
    {
        public PositionRepository(PayrollContext context) : base(context)
        {
        }

        protected override void DetachNavigationProperties(Position entity)
        {
            if (entity.Division != null)
            {
                _context.Entry(entity.Division).State = EntityState.Detached;
            }

            if (entity.JobLevel != null)
            {
                _context.Entry(entity.JobLevel).State = EntityState.Detached;
            }
        }

        #region Queries

        #region Single entity

        internal async Task<Position> GetByIdWithDivisionAsync(int positionId)
        {
            return await _context.Positions
                .Include(p => p.Division)
                .Where(p => p.RowID == positionId)
                .FirstOrDefaultAsync();
        }

        internal async Task<Position> GetByNameAsync(int organizationId, string positionName)
        {
            return await _context.Positions
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.Name.Trim().ToLower() == positionName.ToTrimmedLowerCase())
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Position> GetFirstPositionAsync()
        {
            return await _context.Positions
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        #endregion Single entity

        #region List of entities

        internal async Task<ICollection<Position>> GetAllAsync(int organizationId)
        {
            return await _context.Positions
                .Where(p => p.OrganizationID == organizationId)
                .ToListAsync();
        }

        internal async Task<PaginatedList<Position>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
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

            return new PaginatedList<Position>(positions, count);
        }

        #endregion List of entities

        #endregion Queries
    }
}