using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class DivisionRepository : BaseRepository
    {
        public const string DIVISION_TYPE_DEPARTMENT = "Department";
        public const string DIVISION_TYPE_BRANCH = "Branch";
        public const string DIVISION_TYPE_SUB_BRANCH = "Sub branch";

        private readonly PayrollContext _context;

        public DivisionRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        internal async Task DeleteAsync(Division division)
        {
            _context.Remove(division);

            await _context.SaveChangesAsync();
        }

        internal async Task SaveAsync(Division division)
        {
            if (IsNewEntity(division.RowID))
            {
                _context.Divisions.Add(division);
            }
            else
            {
                _context.Entry(division).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        internal async Task<Division> GetByIdWithParentAsync(int id)
        {
            return await _context.Divisions
                                .Include(x => x.ParentDivision)
                                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        internal IEnumerable<Division> GetAll(int organizationId)
        {
            return _context.Divisions.
                            Where(d => d.OrganizationID == organizationId).
                            ToList();
        }

        internal async Task<IEnumerable<Division>> GetAllAsync(int organizationId)
        {
            return await _context.Divisions.
                                Where(d => d.OrganizationID == organizationId).
                                ToListAsync();
        }

        internal async Task<PaginatedListResult<Division>> GetPaginatedListAsync(PageOptions options, int organizationId, bool isRoot, string searchTerm = "")
        {
            var query = _context.Divisions
                                .Include(x => x.ParentDivision)
                                .Where(x => x.OrganizationID == organizationId)
                                .OrderBy(x => x.ParentDivision.Name)
                                .ThenBy(x => x.Name)
                                .AsQueryable();

            if (isRoot)
            {
                query = query.Where(x => x.ParentDivisionID == null);
            }
            else
            {
                query = query.Where(x => x.ParentDivisionID != null);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, searchTerm) ||
                    EF.Functions.Like(x.ParentDivision.Name, searchTerm));
            }

            var divisions = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<Division>(divisions, count);
        }

        internal async Task<IEnumerable<Division>> GetAllParentsAsync(int organizationId)
        {
            return await _context.Divisions.
                            Where(d => d.OrganizationID == organizationId).
                            Where(d => d.IsRoot).
                            ToListAsync();
        }

        #endregion List of entities

        #region Others

        internal List<string> GetDivisionTypeList()
        {
            return new List<string>()
            {
                DIVISION_TYPE_DEPARTMENT,
                DIVISION_TYPE_BRANCH,
                DIVISION_TYPE_SUB_BRANCH
            };
        }

        #endregion Others

        #endregion Queries
    }
}